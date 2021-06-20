using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> Heap = new List<T>();

        public void Push(T data)
        {
            Heap.Add(data);


            int now = Heap.Count - 1;

            while (now > 0)
            {
                int parent = (now - 1) / 2;

                if (Heap[now].CompareTo(Heap[parent]) < 0)
                {
                    break;
                }

                T temp = Heap[now];
                Heap[now] = Heap[parent];
                Heap[parent] = temp;

                now = parent;
            }


        }


        public T Pop()
        {

            T ret = Heap[0];

            int lastIndex = Heap.Count - 1;

            Heap[0] = Heap[lastIndex];

            Heap.RemoveAt(lastIndex);

            lastIndex--;

            int now = 0;

            while (true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;

                int next = now;

                if (left <= lastIndex && Heap[next].CompareTo(Heap[left]) < 0)
                {
                    next = left;
                }
                if (right <= lastIndex && Heap[next].CompareTo(Heap[right]) < 0)
                {
                    next = right;
                }

                if (next == now)
                {
                    break;
                }

                T temp = Heap[now];
                Heap[now] = Heap[next];
                Heap[next] = temp;

                now = next;
            }


            return ret;
        }


        public int Count { get { return Heap.Count; } }
    }
}
