using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huff.Algoritms
{
    internal class Alphabet
    {
        /// <summary>
        /// Возвращает частоту появления байт в исходном массиве и индекс первого значимого байта закодированного массива
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public (Dictionary<byte, int>, int) GetAlphabet(byte[] bytes)
        {
            Dictionary<byte, int> freq = new Dictionary<byte, int>();
            int count = 1;
            while (bytes[0] + 1 > freq.Count)
            {
                var key = bytes[count++];
                var val = BitConverter.ToInt32(bytes, count);
                freq.Add(key, val);
                count += 4;
            }
            return (freq, count);
        }
        public byte[] ConvertToBytes(Dictionary<byte, int> freq)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte)(freq.Count - 1));
            for (int i = 0; i < freq.Count; i++)
            {
                var keyValPair = freq.ElementAt(i);
                bytes.Add(keyValPair.Key);
                bytes.AddRange(BitConverter.GetBytes(keyValPair.Value));
            }
            return bytes.ToArray();
        }


    }
}
