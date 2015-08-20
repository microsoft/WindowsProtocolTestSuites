// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Queue manager, store all kinds of objects inherit from StackPacket.implement the IDisposable interface
    /// and the IDisposable.Dispose method.
    /// </summary>
    public class QueueManager : IDisposable
    {
        private bool disposed;

        private ManualResetEvent manualResetEvent;
        private Queue<object> objectList;

        /// <summary>
        /// Count of objects in the queue.
        /// </summary>
        /// <return>return the count of the objects in the queue</return>
        public int Count
        {
            get
            {
                return objectList.Count;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueueManager()
        {
            this.manualResetEvent = new ManualResetEvent(false);
            objectList = new Queue<object>();
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                    this.objectList.Clear();
                    this.manualResetEvent.Close();
                    this.objectList = null;
                    this.manualResetEvent = null;

                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }

        /// <summary>
        /// get a copy of the object list
        /// </summary>
        public ReadOnlyCollection<object> ObjectList
        {
            get
            {
                lock (this)
                {
                    return new ReadOnlyCollection<object>(this.objectList.ToArray());
                }
            }
        }

        /// <summary>
        /// Add obj to the queue
        /// </summary>
        /// <param name="value">the object which will be added to the queue</param>
        public void AddObject(object value)
        {
            if (value != null)
            {
                //Add obj to the queue.
                lock (this)
                {
                    objectList.Enqueue(value);
                    //Release get-signal.
                    manualResetEvent.Set();
                }
            }
        }

        /// <summary>
        /// Get first obj
        /// </summary>
        /// <param name="timeout">Receiving obj timeout. It will be set to the remaining time when return.</param>
        /// <returns>The first obj in the queue.</returns>
        public object GetObject(ref TimeSpan timeout)
        {
            object obj;

            lock (this)
            {
                if (objectList.Count > 0)
                {
                    obj = objectList.Dequeue();

                    if (objectList.Count == 0)
                    {
                        manualResetEvent.Reset();
                    }
                    return obj;
                }
            }

            //Waiting for signal to dequeue.
            DateTime startTime = DateTime.Now;
            if (!this.manualResetEvent.WaitOne(timeout, false))
            {
                throw new TimeoutException("It is time out when receiving an obj.");
            }

            //Get obj out the queue.
            lock (this)
            {
                if (objectList.Count > 0)
                {
                    obj = objectList.Dequeue();

                    if (objectList.Count == 0)
                    {
                        manualResetEvent.Reset();
                    }
                    timeout -= (DateTime.Now - startTime);
                    return obj;
                }
            }
            throw new TimeoutException("It is time out when receiving an obj.");
        }

        /// <summary>
        /// Clear all packets in the queue.
        /// </summary>
        public void ClearAll()
        {
            // Clear queue.
            lock (this)
            {
                objectList.Clear();
                manualResetEvent.Reset();
            }
        }
    }

}
