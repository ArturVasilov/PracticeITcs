﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEndedQueue
{
    class ConcurrentDequeue<T> : DEQueue<T>
    {
        private static readonly object monitor = new object();

        private DEQueue<T> dequeue;

        public ConcurrentDequeue()
        {
            dequeue = new ListDequeue<T>();
        }

        public ConcurrentDequeue(DEQueue<T> dequeue)
        {
            this.dequeue = dequeue;
        }

        public ConcurrentDequeue(T element)
        {
            dequeue = new ListDequeue<T>(element);
        }

        public ConcurrentDequeue(params T[] values)
        {
            dequeue = new ListDequeue<T>(values);
        }

        public ConcurrentDequeue(IEnumerable<T> collection)
        {
            dequeue = new ListDequeue<T>(collection);
        }

        public void addFront(T element)
        {
            lock (monitor)
            {
                dequeue.addFront(element);
            }
        }

        public void addBack(T element)
        {
            lock (monitor)
            {
                dequeue.addBack(element);
            }
        }

        public T popFront()
        {
            lock (monitor)
            {
                return dequeue.popFront();
            }
        }

        public T popBack()
        {
            lock (monitor)
            {
                return dequeue.popBack();
            }
        }

        public T peekFront()
        {
            lock (monitor)
            {
                return dequeue.peekFront();
            }
        }

        public T peekBack()
        {
            lock (monitor)
            {
                return dequeue.peekBack();
            }
        }

        public void addAllBack(IEnumerable<T> collection)
        {
            lock (monitor)
            {
                dequeue.addAllBack(collection);
            }
        }

        public void addAllFront(IEnumerable<T> collection)
        {
            lock (monitor)
            {
                dequeue.addAllFront(collection);
            }
        }

        public void clear()
        {
            lock (monitor)
            {
                dequeue.clear();
            }
        }

        public int size()
        {
            lock (monitor)
            {
                return dequeue.size();
            }
        }

        public bool isEmpty()
        {
            lock (monitor)
            {
                return dequeue.isEmpty();
            }
        }

        public override string ToString()
        {
            lock (monitor)
            {
                return dequeue.ToString();
            }
        }
    }
}
