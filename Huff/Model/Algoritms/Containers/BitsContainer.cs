using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huff.Algoritms.Container
{
    internal class BitsContainer
    {
        List<byte> bytes;

        public BitsContainer()
        {
            bytes = new List<byte>();
            bytes.Add(0);
            CountBits = 0;
        }
        public int CountBits { get; private set; }

        public void AddBits(int code, int lenth)
        {
            for (int i = lenth - 1; i >= 0; i--)
            {
                byte bit = (byte)((code >> i) & 1);
                AddBit(bit);
            }
        }
        public void AddBit(byte bit)
        {
            int currentByteNumber = CountBits / 8;
            if (currentByteNumber >= bytes.Count)
                bytes.Add(0);

            byte currentBitNumber = (byte)(CountBits % 8);
            bytes[currentByteNumber] = AddBit(bit, bytes[currentByteNumber], currentBitNumber);
            CountBits++;
        }
        byte AddBit(byte bit, byte bt, byte numberBit)
        {
            bt <<= 1;
            return (byte)(bt | bit);
        }

        public byte[] GetBytes()
        {
            if (CountBits % 8 != 0)
            {
                bytes[bytes.Count - 1] <<= (8 - (CountBits % 8));
            }
            return bytes.ToArray();
        }

        public byte GetBit(int numBit, byte[] encodeBytes)
        {
            int currentByteNumber = numBit / 8;
            byte currentBitNumber = (byte)(7 - (numBit % 8));
            return (byte)((encodeBytes[currentByteNumber] >> currentBitNumber) & 1);
        }
    }
}
