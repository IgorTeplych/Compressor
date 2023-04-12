using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LZW.Model.CompressAlgo
{
    delegate void ProgressHandler(int num1, int num2);
    delegate void UserActionHandler();
    internal class LZW
    {
        public ProgressHandler EncodeProgressHandler;
        public ProgressHandler DecodeProgressHandler;
        public CancellationToken CancelToken { get; set; }
        public bool IsPaused { get; set; }
        /// <summary>
        /// Кодирует массив байт и записывает его в файл
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        public void Encode(byte[] bytes, string path)
        {
            CompressAlgo.File file = new CompressAlgo.File();
            var dictionary = GetDictionary();
            List<int> encode = new List<int>();
            string s = null;
            int count = 0;
            int encodeCount = 0;
            while (count < bytes.Length)
            {
                char c = (char)bytes[count++];
                string cs = s + c;

                if (dictionary.ContainsKey(cs))
                {
                    s = cs;
                }
                else
                {
                    encode.Add(dictionary[s]);
                    encodeCount += 2;
                    //Если число декодированных элементов равно 16384, то пишем их в файл и освобождаем список
                    if (encode.Count % 16384 == 0)
                        file.Write(encode, path);

                    //Задаем максимальную длину кодов словаря двумя байтами. Она определяет также длину декодированных элементов (также два байта).
                    if (dictionary.Count < 65536)
                        dictionary.Add(cs, dictionary.Count);
                    s = c.ToString();
                }

                if (count % 8192 == 0)
                {
                    CalcEncodeProgressAndRatio(count, encodeCount, bytes.Length);
                    if (CancelToken.IsCancellationRequested)
                        return;
                    while (IsPaused) 
                    {
                        if (CancelToken.IsCancellationRequested)
                            return;
                        Thread.Sleep(1000);
                    }
                }
            }
            if (s != null)
            {
                encode.Add(dictionary[s]);
                encodeCount += 2;
                file.Write(encode, path);
                CalcEncodeProgressAndRatio(count, encodeCount, bytes.Length);
            }
        }
        /// <summary>
        /// Декодирует файл 
        /// </summary>
        /// <param name="pathEncodeFile"></param>
        /// <param name="pathDecodeFile"></param>
        public void Decode(string pathEncodeFile, string pathDecodeFile)
        {
            CompressAlgo.File file = new CompressAlgo.File();

            int partSize = 1024;
            int partCount = 0;

            var dictionary = GetSimpleDictionary();
            List<int> encode = file.Read(pathEncodeFile, partCount, partSize);
            int idx = encode[0];
            encode.RemoveAt(0);
            string w = dictionary[idx];
            List<byte> decode = new List<byte>();
            decode.Add((byte)Convert.ToChar(w));
            int decodeBytesCount = 0;
            while (encode.Count > 0)
            {
                int count = 0;
                while (count < encode.Count)
                {
                    int k = encode[count++];
                    string entry = null;
                    if (dictionary.Count > k)
                    {
                        entry = dictionary[k];
                    }
                    else if (k == dictionary.Count)
                    {
                        entry = w + w[0];
                    }
                    dictionary.Add(w + entry[0]);

                    for (int i = 0; i < entry.Length; i++)
                    {
                        decode.Add((byte)entry[i]);
                        decodeBytesCount++;

                        if (decodeBytesCount % 8192 == 0)
                            CalcDecodeProgress(decodeBytesCount, -1);
                    }

                    w = entry;
                }

                if (decode.Count % 8192 == 0)
                {
                    file.WriteToFile(decode.ToArray(), pathDecodeFile);
                    decode.Clear();

                    if (CancelToken.IsCancellationRequested)
                        return;
                }

                while (IsPaused)
                {
                    if (CancelToken.IsCancellationRequested)
                        return;
                    Thread.Sleep(1000);
                }

                partCount += partSize;
                encode = file.Read(pathEncodeFile, partCount, partSize); 
            }
            file.WriteToFile(decode.ToArray(), pathDecodeFile);
            CalcDecodeProgress(decodeBytesCount, -1);
        }
        void CalcEncodeProgressAndRatio(int count, int lenght, int totalLength)
        {
            int rate = (100 * lenght) / count;
            int progr = (count * 100) / totalLength;
            EncodeProgressHandler.Invoke(progr, rate);
        }
        void CalcDecodeProgress(int count, int length)
        {
            DecodeProgressHandler.Invoke(count, length);
        }

        Dictionary<string, int> GetDictionary()
        {
            //Создаем словарь и наполняем его символами 
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            //Наполняем словарь символами из исходной строки
            for (char i = (char)0; i < 256; i++)
                dictionary.Add(Convert.ToString(i), i);
            return dictionary;
        }
        List<string> GetSimpleDictionary()
        {
            //Создаем словарь и наполняем его символами 
            List<string> dictionary = new List<string>();
            //Наполняем словарь символами из исходной строки
            for (char i = (char)0; i < 256; i++)
                dictionary.Add(Convert.ToString(i));
            return dictionary;
        }
    }
}
