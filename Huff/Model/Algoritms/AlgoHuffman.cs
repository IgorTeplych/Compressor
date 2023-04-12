using Huff.Algoritms.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huff.Algoritms
{
    internal class AlgoHuffman
    {
        BitsContainer bitsContainer;
        BitsContainer decodeBitContainer;
        Dictionary<byte, (int, int)> codes;
        byte[] bytes;
        public MessageHandler ProgressEncodingMessageHandler;
        public MessageHandler ProgressDecodingMessageHandler;
        public AlgoHuffman()
        {

        }
        /// <summary>
        /// Возвращает частоту появления байт в массиве и кодированный массив байт
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public (Dictionary<byte, int>, byte[]) EncodeBytes(byte[] bytes)
        {
            this.bytes = bytes;
            codes = new Dictionary<byte, (int, int)>();
            bitsContainer = new BitsContainer();

            Dictionary<byte, int> freq = CalcFrequency();
            Dictionary<byte, int> outFreq = freq.ToDictionary(k => k.Key, v => v.Value);

            Node hufTreeRoot = CreateHuffmanTree(freq);
            CreateEncodeBytes(hufTreeRoot);
            return (outFreq, bitsContainer.GetBytes());
        }
        Dictionary<byte, int> CalcFrequency()
        {
            Dictionary<byte, int> freq = new Dictionary<byte, int>();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (!freq.ContainsKey(bytes[i]))
                    freq.Add(bytes[i], 1);
                else
                    freq[bytes[i]]++;
            }
            return freq;
        }
        public Node CreateHuffmanTree(Dictionary<byte, int> freq)
        {
            BasicStructures.PriorityQueue<Node> priorityQueue = new BasicStructures.PriorityQueue<Node>();
            while (freq.Count > 0)
            {
                var item = freq.First();
                priorityQueue.Enqueue(item.Value, new Node(item.Key, item.Value, null, null));
                freq.Remove(item.Key);
            }

            while (priorityQueue.Size > 1)
            {
                Node l = priorityQueue.Dequeue();
                Node r = priorityQueue.Dequeue();

                var priority = l.Freq + r.Freq;
                priorityQueue.Enqueue(priority, new Node(0, priority, l, r));
            }
            Node root = priorityQueue.Dequeue();
            return root;
        }
        void CreateCodes(Node root, string str, Dictionary<byte, (int, int)> codes)
        {
            if (root == null)
                return;

            if (root.L == null && root.R == null)
            {
                string code = str.Length > 0 ? str : "1";
                var codeAndLenth = (Convert.ToInt32(code, 2), (byte)code.Length);
                codes.Add(root.bt, codeAndLenth);
            }
            CreateCodes(root.L, str + "0", codes);
            CreateCodes(root.R, str + "1", codes);
        }
        string str = "";
        void CreateEncodeBytes(Node huffTreeRoot)
        {
            CreateCodes(huffTreeRoot, "", codes);
            int i = 0;
            for (i = 0; i < bytes.Length; i++)
            {
                if (i % 512 == 0 && i > 0)
                {
                    str = "Кодирование выполнено " + ((i * 100) / (bytes.Length - 1)) + "%";
                    str += Environment.NewLine;
                    str += "Степень сжатия " + (((bitsContainer.CountBits / 8) * 100) / i) + "%";
                    ProgressEncodingMessageHandler.Invoke(str);
                }
                var codeAndLenth = codes[bytes[i]];
                bitsContainer.AddBits(codeAndLenth.Item1, codeAndLenth.Item2);
            }
            str = "Кодирование выполнено " + ((--i * 100) / (bytes.Length - 1)) + "%";
            str += Environment.NewLine;
            str += "Степень сжатия " + (((bitsContainer.CountBits / 8) * 100) / i) + "%";
            ProgressEncodingMessageHandler.Invoke(str);
        }
        public byte[] DecodeBytes(byte[] encodeBytes, Node Huffmanroot)
        {
            decodeBitContainer = new BitsContainer();
            int size = (encodeBytes.Length - 1) * 8;
            int index = -1;
            List<byte> decodeBytes = new List<byte>();

            while (index < size - 2)
            {
                index = Decode(Huffmanroot, index, decodeBytes, encodeBytes);

                if (index % 8192 == 0)
                    ProgressDecodingMessageHandler.Invoke("Декодировано " + ((index + 3) / 8) + " байт из " + (encodeBytes.Length - 1));
            }
            ProgressDecodingMessageHandler.Invoke("Декодировано " + ((index + 3) / 8) + " байт из " + (encodeBytes.Length - 1));
            return decodeBytes.ToArray();
        }
        int Decode(Node root, int index, List<byte> decodeBytes, byte[] encodeBytes)
        {
            if (root == null)
                return index;

            if (root.L == null && root.R == null)
            {
                decodeBytes.Add(root.bt);
                return index;
            }
            index++;

            root = (decodeBitContainer.GetBit(index, encodeBytes) == 0) ? root.L : root.R;
            index = Decode(root, index, decodeBytes, encodeBytes);
            return index;
        }
    }
}
