using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huff.BasicStructures
{
    public class PriorityQueue<T>
    {
        Node<T> header;
        Node<T> tail;

        int size;
        public int Size
        { get => size; set => size = value; }
        void Push(T data, int priority = 0)
        {
            Node<T> node = new Node<T>(data, priority);

            if (header == null)
                header = node;
            else
                tail.Next = node;
            tail = node;

            size++;
        }

        public void Enqueue(int priority, T data)
        {
            Node<T> newNode = new Node<T>(data, priority);

            if (header == null)
            {
                Push(data, priority);
                return;
            }

            Node<T> current = header;

            int count = 0;
            while (count < size)
            {
                if (current.Priority > priority)
                {
                    Insert(data, count, priority);
                    return;
                }
                current = current.Next;
                count++;
            }
            Push(data, priority);
        }

        public T Dequeue()
        {
            Node<T> dequeue = header;
            header = header.Next;
            size--;
            return dequeue.Data;
        }

        bool Insert(T data, int index, int priority = 0)
        {
            Node<T> current = header;
            Node<T> newNode = new Node<T>(data, priority);

            if (index == 0)
            {
                newNode.Next = current;
                header = newNode;
                size++;
                return true;
            }

            int count = 0;
            while (count < size)
            {
                if (current.Next != null)
                {
                    if (count == index - 1)
                    {
                        newNode.Next = current.Next;
                        current.Next = newNode;
                        size++;
                        return true;
                    }
                    else
                    {
                        current = current.Next;
                        count++;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

    }
}
