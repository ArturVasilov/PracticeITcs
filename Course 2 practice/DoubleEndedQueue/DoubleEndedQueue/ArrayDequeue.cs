﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEndedQueue
{
    class ArrayDequeue<T> : DEQueue<T>
    {
        private const int INITIAL_CAPACITY = 16;

        private const double ENSURE_CAPACITY = 1.5;

        private T[] values;

        private int addIndex = 0;

        private int popIndex = -1;

        public ArrayDequeue()
        {
            values = new T[INITIAL_CAPACITY];
        }

        public ArrayDequeue(int initialCapacity)
        {
            values = new T[initialCapacity];
        }

        public ArrayDequeue(T element)
        {
            values = new T[INITIAL_CAPACITY];
            addBack(element);
        }

        public ArrayDequeue(params T[] values)
        {
            if ((int)(values.Length * ENSURE_CAPACITY) < INITIAL_CAPACITY)
            {
                this.values = new T[INITIAL_CAPACITY];
            }
            else
            {
                this.values = new T[(int)(values.Length * ENSURE_CAPACITY)];
            }
            addAllBack(values);
        }

        public ArrayDequeue(IEnumerable<T> collection)
        {
            if ((int)(collection.Count() * ENSURE_CAPACITY) < INITIAL_CAPACITY)
            {
                values = new T[INITIAL_CAPACITY];
            }
            else
            {
                values = new T[(int)(collection.Count() * ENSURE_CAPACITY)];
            }
            addAllBack(values);
        }

        private void ensureCapacity(int newCapacity)
        {
            T[] newValues = new T[newCapacity];
            Array.Copy(values, newValues, values.Length);
            values = newValues;
        }

        public void addFront(T element)
        {

        }

        public void addBack(T element)
        {

        }

        public T popFront()
        {
            return default(T);
        }

        public T popBack()
        {
            return default(T);
        }

        public T peekFront()
        {
            return default(T);
        }

        public T peekBack()
        {
            return default(T);
        }

        public void addAllBack(IEnumerable<T> collection)
        {

        }

        public void addAllFront(IEnumerable<T> collection)
        {

        }

        public override string ToString()
        {
            return "[]";
        }

        public void clear()
        {

        }

        public int size()
        {
            return values.Length;
        }

        public bool isEmpty()
        {
            return size() == 0;
        }

    }
}