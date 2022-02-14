using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tools.Util
{
    /// <summary>
    /// 泛型阻塞队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BlockQueue<T> where T:class
    {
        private readonly Queue<T> queue = new Queue<T>();
        private readonly int maxSize;
        bool closing;
        public BlockQueue(int maxSize)
        {
            this.maxSize = maxSize;
        }

        public void Enqueue(T item)
        {
            lock (queue)
            {
                while (queue.Count >= maxSize)
                {
                    Monitor.Wait(queue);
                }
                queue.Enqueue(item);
                if(queue.Count == 1)
                {
                    Monitor.PulseAll(queue);
                }
            }
        }

        public bool TryDequeue(out T value)
        {
            lock (queue)
            {
                while(queue.Count == 0)
                {
                    if (closing)
                    {
                        value = default(T);
                        return false;
                    }
                    Monitor.Wait(queue);
                }
                value = queue.Dequeue();
                if(queue.Count == maxSize - 1)
                {
                    Monitor.PulseAll(queue);
                }
                return true;
            }
        }

        public int Count()
        {
            lock (queue)
            {
                return queue.Count;
            }
        }

        public void Close()
        {
            lock (queue)
            {
                closing = true;
                Monitor.PulseAll(queue);
            }
        }
    }
}
