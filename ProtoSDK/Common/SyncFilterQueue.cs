// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// A callback to determine if the object is what user is searching.
    /// </summary>
    /// <typeparam name="T">The type of the obj.</typeparam>
    /// <param name="obj">obj to search.</param>
    /// <returns>true: The obj is what user wants.</returns>
    public delegate bool Filter<T>(T obj);


    /// <summary>
    /// Filter Queue to support multiple threads to add/get object.
    /// </summary>
    public class SyncFilterQueue<T> : IDisposable
    {
        #region Fields members

        //Disposing has been done
        private bool disposed;

        //Object locker
        private object locker;

        //The list to store the added object
        private List<T> objectList;

        //The list to store FilterAutoResetEvent. 
        private List<FilterAutoResetEvent<T>> filterAutoResetEvents;

        #endregion


        #region ctor

        /// <summary>
        /// Initialize SyncFilterQueue.
        /// </summary>
        public SyncFilterQueue()
        {
            filterAutoResetEvents = new List<FilterAutoResetEvent<T>>();
            objectList = new List<T>();

            locker = new object();
        }

        #endregion


        #region Properties

        /// <summary>
        /// This property returns current count of elements in the queue. 
        /// If it is followed by Enqueue/Dequeue operation, user must add a lock to make it an atom operation.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when the queue has been disposed.</exception>
        public int Count
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("SyncFilterQueue");
                }

                lock (locker)
                {
                    return objectList.Count;
                }
            }
        }

        #endregion


        #region user interface

        /// <summary>
        /// Add an object to the end of queue.
        /// </summary>
        /// <param name="obj">The object to add to the queue.</param>
        /// <exception cref="ArgumentNullException">Thrown when obj is null.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the queue has been disposed.</exception>
        public void Enqueue(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (disposed)
            {
                throw new ObjectDisposedException("SyncFilterQueue");
            }

            //Add obj to the list.
            lock (locker)
            {
                //set the state of the event to signaled.
                foreach (FilterAutoResetEvent<T> item in filterAutoResetEvents)
                {
                    if (item.Filter == null || item.Filter(obj))
                    {
                        item.ObjectExists = true;
                        item.ObjectData = obj;
                        filterAutoResetEvents.Remove(item);
                        item.ResetEvent.Set();
                        return;
                    }
                }

                objectList.Add(obj);
            }
        }


        /// <summary>
        /// Remove and return the object at the beginning of the queue. 
        /// </summary>
        /// <param name="timeout">Timeout of waiting to find the object.</param>
        /// <returns>The object at the beginning of the queue.</returns>
        public T Dequeue(TimeSpan timeout)
        {
            return Dequeue(timeout, null);
        }


        /// <summary>
        /// Remove and return the object with the specific filter.
        /// </summary>
        /// <param name="timeout">Timeout of waiting to find the object.</param>
        /// <param name="filter">A callback to filter.</param>
        /// <returns>The object with the specific filter.</returns>
        /// <exception cref="TimeoutException">Thrown when it is timeout when searching an object.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the queue has been disposed.</exception>
        public T Dequeue(TimeSpan timeout, Filter<T> filter)
        {
            T obj;
            AutoResetEvent autoEvent = null;
            FilterAutoResetEvent<T> filterAutoResetEvent;

            if (disposed)
            {
                throw new ObjectDisposedException("SyncFilterQueue");
            }

            try
            {
                lock (locker)
                {
                    //Get obj out the list.
                    foreach (T data in objectList)
                    {
                        if (filter == null || filter(data))
                        {
                            obj = data;
                            objectList.Remove(data);

                            return obj;
                        }
                    }

                    // There's really no object in objectList, add event.
                    autoEvent = new AutoResetEvent(false);
                    filterAutoResetEvent = new FilterAutoResetEvent<T>(autoEvent, filter);
                    filterAutoResetEvents.Add(filterAutoResetEvent);
                }

                bool found = autoEvent.WaitOne(timeout, false);

                if (found)
                {
                    if (!filterAutoResetEvent.ObjectExists)
                    {
                        // SyncFilterQueue.Dispose() will Set event. but object not exist.
                        throw new ObjectDisposedException("SyncFilterQueue");
                    }

                    return filterAutoResetEvent.ObjectData;
                }

                if (disposed)
                {
                    throw new ObjectDisposedException("SyncFilterQueue");
                }

                lock (locker)
                {
                    if (filterAutoResetEvent.ObjectExists)
                    {
                        return filterAutoResetEvent.ObjectData;
                    }

                    filterAutoResetEvents.Remove(filterAutoResetEvent);
                    throw new TimeoutException("Timeout for waiting an object.");
                }
            }
            finally
            {
                if (autoEvent != null)
                {
                    autoEvent.Close();
                }
            }
        }


        /// <summary>
        /// Remove all objects in the queue.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when the queue has been disposed.</exception>
        public void Clear()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("SyncFilterQueue");
            }

            // Clear list.
            lock (locker)
            {
                objectList.Clear();
            }
        }


        /// <summary>
        /// Return the object at the begging of the queue.
        /// If it is followed by Enqueue/Dequeue operation, user must add a lock to make it an atom operation.
        /// </summary>
        /// <returns>The object at the begging of the queue.</returns>
        /// <exception cref="InvalidOperationException">Thrown when queue is empty.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the queue has been disposed.</exception>
        public T Peek()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("SyncFilterQueue");
            }

            lock (locker)
            {
                if (objectList.Count == 0)
                {
                    throw new InvalidOperationException("Queue is empty");
                }
                else
                {
                    return objectList[0];
                }
            }
        }

        #endregion


        #region IDisposable Members

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
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                lock (locker)
                {
                    if (disposing)
                    {
                        //Release managed resource.
                        foreach (FilterAutoResetEvent<T> item in filterAutoResetEvents)
                        {
                            item.ResetEvent.Set();
                        }
                        filterAutoResetEvents.Clear();

                        if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
                        {
                            foreach (T item in objectList)
                            {
                                (item as IDisposable).Dispose();
                            }
                        }
                        objectList.Clear();
                    }

                    //Note disposing has been done.
                    disposed = true;
                }
            }
        }

        /// <summary>
        /// finalizer
        /// </summary>
        ~SyncFilterQueue()
        {
            Dispose(false);
        }

        #endregion

    }


    /// <summary>
    /// A wrapper for AutoResetEvent and Filter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class FilterAutoResetEvent<T>
    {
        #region field members

        private AutoResetEvent resetEvent;
        private Filter<T> objectFilter;
        private bool exists;
        private T data;

        #endregion


        #region ctor

        /// <summary>
        /// Initialize FilterAutoResetEvent.
        /// </summary>
        /// <param name="autoResetEvent">Notifies a waiting the thread that an event has occurred</param>
        /// <param name="filter">A callback to filter.</param>
        internal FilterAutoResetEvent(AutoResetEvent autoResetEvent, Filter<T> filter)
        {
            resetEvent = autoResetEvent;
            objectFilter = filter;
            exists = false;
        }

        #endregion


        #region Properties

        /// <summary>
        /// A callback to filter.
        /// </summary>
        internal Filter<T> Filter
        {
            get
            {
                return objectFilter;
            }
        }


        /// <summary>
        /// Object exists.
        /// </summary>
        internal bool ObjectExists
        {
            get
            {
                return exists;
            }
            set
            {
                exists = value;
            }
        }


        /// <summary>
        /// Object data.
        /// </summary>
        internal T ObjectData
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }


        /// <summary>
        /// Notifies a waiting the thread that an event has occurred.
        /// </summary>
        internal AutoResetEvent ResetEvent
        {
            get
            {
                return resetEvent;
            }
            set
            {
                resetEvent = value;
            }
        }

        #endregion
    }
}
