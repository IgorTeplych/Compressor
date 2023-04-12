using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huff.Algoritms
{
    delegate void MessageHandler(string message);
    internal class Compress
    {
        public MessageHandler ProgressEncodingMessageHandler;
        public MessageHandler EncodingMessageHandler;
        public MessageHandler ProgressDecodingMessageHandler;
        public void Encode(byte[] bytes, string pathFile)
        {
            //Добавляем ненулевой последний байт в исходный массив для недопущения появления ошибки при нескольких последних байт, равных нулю
            bytes = bytes.Concat(new byte[] { 255 }).ToArray();

            AlgoHuffman encodeAlgoHuffman = new AlgoHuffman();
            encodeAlgoHuffman.ProgressEncodingMessageHandler = this.ProgressEncodingMessageHandler;
            encodeAlgoHuffman.ProgressDecodingMessageHandler = this.ProgressDecodingMessageHandler;
            //Получаем частоту появления байт в массиве и кодированный массив байт
            var freqAndEncodeBytes = encodeAlgoHuffman.EncodeBytes(bytes);
            //Декодируем частоту поялвения символов в массиве в массив байт
            Alphabet alphabet = new Alphabet();
            var alph = alphabet.ConvertToBytes(freqAndEncodeBytes.Item1);

            //Записываем в файл результаты
            WriteToFile(alph, alph.Length, pathFile); //Записываем алфавит
            WriteToFile(freqAndEncodeBytes.Item2, freqAndEncodeBytes.Item2.Length, pathFile); //Записываем значащие байты

            string message = "Добавление словаря . . .";
            message += Environment.NewLine;
            message += "Степень сжатия со словарем ";
            message += ((alph.Length + freqAndEncodeBytes.Item2.Length) * 100 / bytes.Length) + "%";
            message += Environment.NewLine;
            message += "Сжатие выполнено!";
            EncodingMessageHandler.Invoke(message);
        }

        public void Decode(string encodePathFile, string decodePathFile)
        {
            byte[] bytes = ReadFile(encodePathFile);
            //Получаем частоту появления байт в исходном массиве и индекс первого байта массива
            Alphabet alphabet = new Alphabet();
            var freqAndStartIndex = alphabet.GetAlphabet(bytes);

            AlgoHuffman encodeAlgoHuffman = new AlgoHuffman();
            encodeAlgoHuffman.ProgressEncodingMessageHandler = this.ProgressEncodingMessageHandler;
            encodeAlgoHuffman.ProgressDecodingMessageHandler = this.ProgressDecodingMessageHandler;
            encodeAlgoHuffman.ProgressDecodingMessageHandler = this.ProgressDecodingMessageHandler;
            //Восстанавливаем дерево Хафмана
            var root = encodeAlgoHuffman.CreateHuffmanTree(freqAndStartIndex.Item1);
            //Получаем массив кодированных байт
            byte[] encodeBytes = new ArraySegment<byte>(bytes, freqAndStartIndex.Item2, bytes.Length - freqAndStartIndex.Item2).ToArray();

            //Декодируем массив байт
            var decodeBytes = encodeAlgoHuffman.DecodeBytes(encodeBytes, root);

            //Пишем массив в файл без последнего байта, который мы добавили при кодировании для исключения ошибки кодирования при последовательности нулей в конце исходного массива
            WriteToFile(decodeBytes, decodeBytes.Length - 1, decodePathFile);
        }

        byte[] ReadFile(string pathFile)
        {
            return File.ReadAllBytes(pathFile);
        }
        void WriteToFile(byte[] bytes, int len, string path)
        {
            using (FileStream fstream = new FileStream(path, FileMode.Append))
            {
                for (int i = 0; i < len; i++)
                    fstream.WriteByte(bytes[i]);
            }
        }
    }
}
