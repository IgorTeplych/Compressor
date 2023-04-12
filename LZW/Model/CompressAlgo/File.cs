using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZW.Model.CompressAlgo
{
    internal class File
    {
        public void Write(List<int> data, string path)
        {
            byte[] bytes = new byte[data.Count * 2];

            int countBytes = 0;
            for (int i = 0; i < data.Count; i++)
            {
                byte low = (byte)data[i];
                byte high = (byte)(data[i] >> 8);

                bytes[countBytes++] = high;
                bytes[countBytes++] = low;
            }
            WriteToFile(bytes, path);
            data.Clear();
        }
        public List<int> Read(string path, int start, int length)
        {
            var bytes = ReadPartFile(path, start, length);
            List<int> part = BytesToInts(bytes.Item1, bytes.Item2);
            return part;
        }
        List<int> BytesToInts(byte[] bytes, int size)
        {
            List<int> ints = new List<int>();
            for (int i = 0; i < size; i++)
            {
                int num = bytes[i];
                num <<= 8;
                num |= bytes[++i];
                ints.Add(num);
            }
            return ints;
        }
        public void WriteToFile(byte[] bytes, string path)
        {
            using (FileStream fstream = new FileStream(path, FileMode.Append))
                for (int i = 0; i < bytes.Length; i++)
                    fstream.WriteByte(bytes[i]);
        }
        public (byte[], int) ReadPartFile(string path, int start, int length)
        {
            int size = 0;
            byte[] buff = new byte[length];
            using (FileStream fstream = System.IO.File.OpenRead(path))
            {
                fstream.Seek(start, SeekOrigin.Begin);
                size = fstream.Read(buff, 0, buff.Length);
            }
            return (buff, size);
        }
    }
}
