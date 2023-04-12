using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huff.BasicStructures
{
    public class Node<T>
    {
        public Node(T data, int priority = 0)
        {
            Data = data;
            Priority = priority;
        }
        public Node(Node<T> node)
        {
            Data = node.Data;
            Priority = node.Priority;
            Next = node.Next;
        }

        public T Data { get; set; }
        public Node<T> Next { get; set; }
        public int Priority { get; set; }
    }
}
