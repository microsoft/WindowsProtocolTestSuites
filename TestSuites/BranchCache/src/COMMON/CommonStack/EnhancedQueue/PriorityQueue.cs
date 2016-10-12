// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An unbounded priority queue based on a priority heap.
    /// The elements of the priority queue are ordered according to their comparable natural ordering or
    /// by a comparator provided at queue construction time, depending on which constructor is used by the caller.
    /// The head of the queue is the least element with respect to the specified ordering.
    /// </summary>
    /// <typeparam name="T">The generic type for recording need info</typeparam>
    public class PriorityQueue<T>
    {
        /// <summary>
        /// Default capacity
        /// </summary>
        private const int DEFAULTINITIALCAPACITY = 11;

        /// <summary>
        /// The comparer
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Priority queue represented as a balanced binary heap: 
        /// the two children of queue[n] are queue[2*n+1] and queue[2*(n+1)].
        /// The element with the lowest value is in queue[0], assuming the queue is non-empty.
        /// </summary>
        private T[] heap;

        /// <summary>
        /// The count for recording
        /// </summary>
        private int count;

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class with the default
        /// initial capacity that orders the elements according to their comparable natural ordering.
        /// </summary>
        public PriorityQueue() : this(DEFAULTINITIALCAPACITY, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class with the specified initial capacity
        /// that orders the elements according to their comparable natural ordering.
        /// </summary>
        /// <param name="capacity"> The specified initial capacity.</param>
        public PriorityQueue(int capacity) : this(capacity, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class with the default initial capacity
        /// that orders the elements according to the specified comparator.
        /// </summary>
        /// <param name="comparer"> The specified comparator.</param>
        public PriorityQueue(IComparer<T> comparer) : this(DEFAULTINITIALCAPACITY, comparer)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class with the specified initial capacity
        /// that orders the elements according to the specified comparator.
        /// </summary>
        /// <param name="capacity"> The specified initial capacity.</param>
        /// <param name="comparer"> The specified comparator.</param>
        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            if (capacity < 1)
            {
                throw new ArgumentException("capacity should be greater than 0.");
            }

            this.comparer = (null == comparer) ? Comparer<T>.Default : comparer;
            this.heap = new T[capacity];
        }

        /// <summary>
        /// Gets the count property
        /// </summary>
        public int Count
        {
            get { return this.count; }
            private set { this.count = value; }
        }

        /// <summary>
        /// Inserts the specified element into this priority queue.
        /// </summary>
        /// <param name="v"> The specified element to add.</param>
        public void Push(T v)
        {
            if (this.count >= this.heap.Length)
            {
                Array.Resize(ref this.heap, this.count * 2);
            }

            this.heap[this.count] = v;

            this.SiftUp(this.count++);
        }

        /// <summary>
        /// Gets and removes the head of this queue.
        /// </summary>
        /// <returns> Returns the head of this queue.</returns>
        public T Pop()
        {
            var v = this.Top();

            this.heap[0] = this.heap[--this.count];

            if (this.count > 0)
            {
                this.SiftDown(0);
            }

            return v;
        }

        /// <summary>
        /// Gets the head of this queue without removing.
        /// </summary>
        /// <returns> Returns the head.</returns>
        public T Top()
        {
            if (this.count > 0)
            {
                return this.heap[0];
            }

            throw new InvalidOperationException("queue is null");
        }

        /// <summary>
        /// Re-order the queue.
        /// </summary>
        /// <param name="n"> The position to fill.</param>
        private void SiftUp(int n)
        {
            var v = this.heap[n];

            for (var n2 = n / 2; n > 0 && this.comparer.Compare(v, this.heap[n2]) > 0; n = n2, n2 /= 2)
            {
                this.heap[n] = this.heap[n2];

                this.heap[n] = v;
            }
        }

        /// <summary>
        /// Re-order the queue.
        /// </summary>
        /// <param name="n"> The position to fill.</param>
        private void SiftDown(int n)
        {
            var v = this.heap[n];

            for (var n2 = n * 2; n2 < this.count; n = n2, n2 *= 2)
            {
                if (n2 + 1 < this.count && this.comparer.Compare(this.heap[n2 + 1], this.heap[n2]) > 0)
                {
                    n2++;
                }

                if (this.comparer.Compare(v, this.heap[n2]) >= 0)
                {
                    break;
                }

                this.heap[n] = this.heap[n2];
            }

            this.heap[n] = v;
        }
    }
}
