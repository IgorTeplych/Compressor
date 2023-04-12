using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huff.Algoritms
{
    internal class Node
    {
        public byte bt { get; set; }
        public int Freq { get; set; }
        public Node L { get; set; }
        public Node R { get; set; }
        public Node(byte bt, int freq, Node l, Node r)
        {
            this.bt = bt;
            this.Freq = freq;
            this.L = l;
            this.R = r;
        }
    }
}
