using BasicStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comp
{
    public class Rle
    {
        public void Encode(byte[] bytes, string fullPath)
        {
            IArray<byte> encode = new FactorArray<byte>();
            int countBytes = 0;
            int count = 1;
            int totalLength = 0;
            while (count < bytes.Length)
            {
                encode = new FactorArray<byte>();
                countBytes = 0;
                byte length = 1;
                while (length < 127 && count < bytes.Length && bytes[count - 1] == bytes[count])
                {
                    count++;
                    length++;
                }
                if (length > 1)
                {
                    encode.Add((byte)(length | 0x80), countBytes++);
                    encode.Add(bytes[count - 1], countBytes++);
                    count++;
                }
                length = 1;
                while (length < 127 && count < bytes.Length && bytes[count - 1] != bytes[count])
                {
                    length++;
                    count++;
                }
                if (bytes.Length == count)
                {
                    length++;
                    count++;
                }
                if (length > 1)
                {
                    encode.Add((byte)(length - 1), countBytes++);
                    for (int i = length; i > 1; i--)
                        encode.Add(bytes[count - i], countBytes++);
                }

                WriteToFile(encode, countBytes, fullPath);
                totalLength += countBytes;
            }
        }
        public void Decode(byte[] encode, string fullPath)
        {
            IArray<byte> decode = new FactorArray<byte>();
            int decodeLength = 0;
            byte length;
            int totalCount = 0;
            while (totalCount < encode.Length)
            {
                decodeLength = 0;
                decode = new FactorArray<byte>();
                if (encode[totalCount] > 128)
                {
                    length = (byte)(encode[totalCount++] - 128);
                    byte val = encode[totalCount++];
                    while (length > 0)
                    {
                        decode.Add(val, decodeLength++);
                        length--;
                    }
                }
                else
                {
                    length = (byte)(encode[totalCount++]);
                    while (length > 0)
                    {
                        decode.Add(encode[totalCount++], decodeLength++);
                        length--;
                    }
                }
                WriteToFile(decode, decodeLength, fullPath);
            }
        }
        void WriteToFile(IArray<byte> bytes, int len, string path)
        {
            using (FileStream fstream = new FileStream(path, FileMode.Append))
            {
                for (int i = 0; i < len; i++)
                    fstream.WriteByte(bytes.Get(i));
            }
        }
    }
}
