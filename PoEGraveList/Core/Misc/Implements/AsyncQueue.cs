using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoEGraveList.Core.Misc.Implements
{
    internal class AsyncQueue<T>
    {
        private Queue<T> _queue;

        public int Count
        {
            get
            {
                return _queue.Count;
            }
        }

        public AsyncQueue()
        {
            this._queue = new Queue<T>();
        }
        public AsyncQueue(IEnumerable<T> items)
        {
            this._queue = new Queue<T>(items);
        }

        public void Enqueue(T item)
        {
            lock (_queue)
            {
                this._queue.Enqueue(item);
            }
        }

        public T Dequeue()
        {
            lock (_queue)
            {
                T item = this._queue.Dequeue();
                return item;
            }
        }
    }
}
