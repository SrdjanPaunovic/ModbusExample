using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusExample
{
    public class FixedSizedQueue<T> : INotifyCollectionChanged, IEnumerable<T>
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        private object lockObject = new object();

        public FixedSizedQueue()
        {
            Limit = int.MaxValue;
        }

        public FixedSizedQueue(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; set; }

        public ConcurrentQueue<T> Queue
        {
            get
            {
                return queue;
            }

            set
            {
                queue = value;
            }
        }

        public void Enqueue(T obj)
        {
            Queue.Enqueue(obj);

            T overflow;
            while (Queue.Count > Limit && Queue.TryDequeue(out overflow)) ;

        }

        public IEnumerator<T> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
