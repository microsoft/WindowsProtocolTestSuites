// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    using System;

    /// <summary>
    /// An unbounded queue of Delayed elements, in which an element can only be taken when its delay has expired.
    /// The head of the queue is that Delayed element whose delay expired furthest in the past.
    /// If no delay has expired there is no head and Poll() method will return null.
    /// Expiration occurs when an element's GetDelay() method returns a value less than or equal to zero.
    /// </summary>
    /// <typeparam name="T">The generic type for get the need info</typeparam>
    public class DelayQueue<T> where T : Delayed
    {
        /// <summary>
        /// The locker
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// A instance of PriorityQueue
        /// </summary>
        private PriorityQueue<T> q = new PriorityQueue<T>();

        /// <summary>
        /// Initializes a new instance of the DelayQueue class
        /// </summary>
        public DelayQueue()
        {
        }

        /// <summary>
        /// Gets the count of elements.
        /// </summary>
        public int Count
        {
            get { return this.q.Count; }
        }

        /// <summary>
        /// Inserts the specified elememnt into this delay queue.
        /// </summary>
        /// <param name="v"> The element to add.</param>
        public void Add(T v)
        {
            lock (this.locker)
            {
                this.q.Push(v);
            }
        }

        /// <summary>
        /// Retrieves and removes the head or returns null if this queue has no element with an expired delay.
        /// </summary>
        /// <returns> The head of this queue, or null if this queue has no element with an expired delay.</returns>
        public T Poll()
        {
            lock (this.locker)
            {
                T first = null;
                try
                {
                    first = this.q.Top();
                }
                catch (Exception)
                {
                }

                if (first == null || first.GetDelay() > 0)
                {
                    return null;
                }
                else
                {
                    return this.q.Pop();
                }
            }
        }
    }
}

