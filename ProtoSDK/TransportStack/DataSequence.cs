// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the item for sequence, that stores the owner and the length of data.
    /// </summary>
    internal class SequenceItem
    {
        #region Fields

        /// <summary>
        /// the source of data or event.<para/>
        /// if data, the length must not be null/byte[0].<para/>
        /// if event, it's a TransportEvent object and length must be 0.
        /// </summary>
        private object source;

        /// <summary>
        /// the length of data or event.<para/>
        /// if data, it must not be 0.<para/>
        /// if event, it's must be 0.
        /// </summary>
        private int length;

        #endregion

        #region Properties

        /// <summary>
        /// get/set the source of data or event.<para/>
        /// if data, the length must not be null/byte[0].<para/>
        /// if event, it's a TransportEvent object and length must be 0.
        /// </summary>
        public object Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }


        /// <summary>
        /// get/set the length of data or event.<para/>
        /// if data, it must not be 0.<para/>
        /// if event, it's must be 0.
        /// </summary>
        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SequenceItem()
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">
        /// the owner of data or event.<para/>
        /// if data, the length must not be null/byte[0].<para/>
        /// if event, it's a TransportEvent object and length must be 0.
        /// </param>
        /// <param name="length">
        /// the length of data or event.<para/>
        /// if data, it must not be 0.<para/>
        /// if event, it's must be 0.
        /// </param>
        public SequenceItem(object owner, int length)
            : this()
        {
            this.Source = owner;
            this.Length = length;
        }

        #endregion
    }

    /// <summary>
    /// the data sequence manager, that stores the received data and its owner.<para/>
    /// it's thread-safe.
    /// </summary>
    internal class DataSequence : IDisposable
    {
        #region Fields

        /// <summary>
        /// the sequence of data.
        /// </summary>
        private List<SequenceItem> sequence;

        /// <summary>
        /// the owner list that stores the searched owner.<para/>
        /// when GetFirst, clear this list and add the first owner to owners.<para/>
        /// when GetNext, get the next owner, and check if not exits in this owners, add and return it.<para/>
        /// initialize at constructor, never be null util disposed.
        /// </summary>
        private List<object> visitedOwners;

        /// <summary>
        /// an int value that indicates the index.<para/>
        /// the index will be reset at Consume, Remove or timeout of Next.
        /// </summary>
        private int index;

        /// <summary>
        /// a ManualResetEvent object that specifies the received event.
        /// </summary>
        private ManualResetEvent receivedEvent;

        /// <summary>
        /// an int value that specifies the length of data.
        /// </summary>
        private int dataLength;

        /// <summary>
        /// an object that represents the locker.
        /// </summary>
        private object objectLock;

        #endregion

        #region Properties

        /// <summary>
        /// get an int value that indicates the count of sequence.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public int Count
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("DataSequence");
                }

                lock (this.objectLock)
                {
                    return this.sequence.Count;
                }
            }
        }


        /// <summary>
        /// get an int value that specifies the length of data.
        /// </summary>
        public int DataLength
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("DataSequence");
                }

                lock (this.objectLock)
                {
                    return this.dataLength;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DataSequence()
        {
            this.sequence = new List<SequenceItem>();
            this.receivedEvent = new ManualResetEvent(false);
            this.visitedOwners = new List<object>();
            this.objectLock = new object();
        }

        #endregion

        #region Methods

        /// <summary>
        /// add data from owner to buffer, and the sequence information to data sequence.
        /// </summary>
        /// <param name="owner">
        /// an object that specifies the owner of data or event.<para/>
        /// if data, the length must not be null/byte[0].<para/>
        /// if event, it's a TransportEvent object and data must be byte[0].
        /// </param>
        /// <param name="data">
        /// a bytes array that contains the data.<para/>
        /// if event, it can be null.
        /// </param>
        /// <param name="buffer">
        /// a BytesBuffer object that stores the data.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public void Add(object owner, byte[] data, BytesBuffer buffer)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("DataSequence");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            lock (this.objectLock)
            {
                // must add data to buffer, then add the sequence item to triggle received event.
                if (data.Length > 0)
                {
                    buffer.Add(data);
                    this.dataLength += data.Length;
                    this.sequence.Add(new SequenceItem(owner, data.Length));
                }
                else
                {
                    this.sequence.Add(new SequenceItem(owner, 0));
                }

                // if the owner in the search history, remove it to make it can be searched by user again.
                if (this.visitedOwners.Contains(owner))
                {
                    this.visitedOwners.Remove(owner);
                }

                this.receivedEvent.Set();
            }
        }


        /// <summary>
        /// reset the index, and clear all owners in history.<para/>
        /// the Next() will search at the begining of sequence.<para/>
        /// invoke this method before invoking Next().
        /// </summary>
        public void Reset()
        {
            lock (this.objectLock)
            {
                this.index = 0;
                this.visitedOwners.Clear();
            }
        }


        /// <summary>
        /// get the next item. please invoke Reset() at the first time.
        /// if overflow, then:<para/>
        /// if timeout = TimeSpan.MinValue, it means not wait the data coming.<para/>
        /// if not wait, return null.<para/>
        /// if wait, wait util data arrived or timeout.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan struct that specifies the timeout to wait for next data coming.<para/>
        /// if timeout = TimeSpan.MinValue, it means not wait the data coming.
        /// </param>
        /// <returns>
        /// a SequenceToken object that indicates the first item.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// thrown when it is timeout when waiting for data coming.
        /// </exception>
        public SequenceItem Next(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("DataSequence");
            }

            // a bool value that indicates whether need to wait for data coming when data is not enough.
            // if true, wait util timeout; otherwise, return null if no data.
            bool notNeedToWait = (timeout == TimeSpan.MinValue);

            // the end time for operation.
            DateTime endTime = DateTime.Now;
            TimeSpan currentTimeout = timeout;

            // if donot wait, the timeout is not used.
            // if need to wait, calculate the timeout.
            if (!notNeedToWait)
            {
                endTime += timeout;
            }

            while (true)
            {
                lock (this.objectLock)
                {
                    // try to return the next new item.
                    SequenceItem item = this.GetNextNewItem();
                    if (item != null)
                    {
                        return item;
                    }

                    // all items in sequence are not valid, wait for more if needed.
                    // if timeout is TimeSpan.MinValue, user does not accept wait,
                    // return null and donot reset the index.
                    if (timeout == TimeSpan.MinValue)
                    {
                        return null;
                    }

                    // need to wait for next item coming.
                    this.receivedEvent.Reset();
                }

                // it's timeout when wait for data coming, reset the index and throw exception.
                if (currentTimeout.Ticks < 0 || !this.receivedEvent.WaitOne(currentTimeout, false))
                {
                    throw new TimeoutException("it is timeout when DataSequence waiting for data coming");
                }

                // update the current timeout and get data in the next loop.
                currentTimeout = endTime - DateTime.Now;
            }
        }


        /// <summary>
        /// remove sequence of specified owner from data sequence.<para/>
        /// this method will reset the index to make the Next() return the first item.<para/>
        /// there is no item in sequence, return directly.
        /// </summary>
        /// <param name="owner">
        /// the owner of data.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Remove(object owner)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("DataSequence");
            }

            lock (this.objectLock)
            {
                // from the end to the begin of list.
                for (int i = this.sequence.Count - 1; i >= 0; --i)
                {
                    if (this.sequence[i] != null && this.sequence[i].Source == owner)
                    {
                        this.dataLength -= this.sequence[i].Length;
                        this.sequence.RemoveAt(i);
                    }
                }
            }
        }


        /// <summary>
        /// remove the specified length from data sequence.<para/>
        /// this method will reset the index to make the Next() return the first item.<para/>
        /// there is no item in sequence, return directly.<para/>
        /// if need to consume an event, please invoke Remove(transportEvent).
        /// </summary>
        /// <param name="owner">
        /// the owner of data.
        /// </param>
        /// <param name="length">
        /// the consumed length of data.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Consume(object owner, int length)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("DataSequence");
            }

            lock (this.objectLock)
            {
                int removedLength = 0;

                // whether there is no item belong to owner.
                bool notFound = false;

                // remove the specified length in sequence.
                while (removedLength < length && !notFound)
                {
                    notFound = true;

                    // remove one item for one time.
                    foreach (SequenceItem item in this.sequence)
                    {
                        if (item.Source != owner)
                        {
                            continue;
                        }

                        // found item
                        notFound = false;

                        // if need to remove the item
                        if (removedLength + item.Length <= length)
                        {
                            removedLength += item.Length;
                            this.sequence.Remove(item);
                        }
                        // just need to descrease the length of item.
                        else
                        {
                            item.Length -= length - removedLength;
                            removedLength = length;
                        }

                        // remove all index information
                        this.visitedOwners.Remove(owner);

                        break;
                    }
                }

                this.dataLength -= removedLength;
            }
        }

        #endregion
 
        #region IDisposable Members

        /// <summary>
        /// the dispose flags 
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Release the managed and unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources. 
        /// </summary>
        /// <param name = "disposing">
        /// If disposing equals true, managed and unmanaged resources are disposed. if false, Only unmanaged resources 
        /// can be disposed. 
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.receivedEvent != null)
                    {
                        this.receivedEvent.Close();
                        this.receivedEvent = null;
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer 
        /// </summary>
        ~DataSequence()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// get next valid item.
        /// </summary>
        /// <returns>
        /// a SeuqenceItem object that does not exists in the owners.
        /// </returns>
        private SequenceItem GetNextNewItem()
        {
            while (this.index < this.sequence.Count)
            {
                SequenceItem item = this.sequence[this.index++];

                // if current item is not the previous items, return.
                // otherwise, search for next item.
                if (!this.visitedOwners.Contains(item.Source))
                {
                    this.visitedOwners.Add(item.Source);

                    return item;
                }
            }

            return null;
        }

        #endregion
    }
}
