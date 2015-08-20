// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// Implements a simple thread-safe queue used for observations like events and returns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ObservationQueue<T>
    {
        object queueLock = new object();
        object queueFullLock = new object();
        Queue<T> queue = new Queue<T>();
        int maxQueueSize;

        /// <summary>
        /// Constructs a queue with given maximal size.
        /// </summary>
        /// <param name="maxSize"></param>
        public ObservationQueue(int maxSize)
        {
            this.maxQueueSize = maxSize;
        }

        /// <summary>
        /// Adds an item to the queue. Will block if queue
        /// is currently full.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (queueLock)
            {
                queue.Enqueue(item);
                Monitor.Pulse(queueLock);
            }
        }

        /// <summary>
        /// Try gets an item from the queue.
        /// </summary>
        /// <param name="timeOut"></param>
        /// <param name="consume"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryGet(TimeSpan timeOut, bool consume, out T item)
        {
            lock (queueLock)
            {
                if (queue.Count == 0)
                {
                    Monitor.Wait(queueLock, timeOut);
                    if (queue.Count == 0)
                    {
                        item = default(T);
                        return false;
                    }
                }
                if (consume)
                {
                    item = queue.Dequeue();
                }
                else
                    item = queue.Peek();
            }
            if (consume)
                lock (queueFullLock)
                    Monitor.PulseAll(queueFullLock);
            return true;
        }

        /// <summary>
        /// Returns a copy of the queue content as a list. 
        /// </summary>
        /// <returns></returns>
        public IList<T> GetEnumerator()
        {
            lock (queueLock)
                return new List<T>(queue);
        }

    }

}
