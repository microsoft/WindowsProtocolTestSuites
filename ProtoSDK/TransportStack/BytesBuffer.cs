// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// a thread-safe bytes buffer, to manage bytes data.
    /// </summary>
    internal class BytesBuffer : IDisposable
    {
        #region Fields

        /// <summary>
        /// a bytes array that contains the data.
        /// </summary>
        private List<byte[]> data;

        /// <summary>
        /// an object value for lock.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// a ManualResetEvent object that specifies the received event.
        /// </summary>
        private ManualResetEvent receivedEvent;

        /// <summary>
        /// a bool value that indicates whether buffer is closed.<para/>
        /// if true, buffer is closed, and can not add bytes to buffer.
        /// </summary>
        private bool isClosed;

        /// <summary>
        /// an int value that indicates the length of bytes in buffer.
        /// </summary>
        private int length;

        #endregion

        #region Properties

        /// <summary>
        /// a const int value that specifies the max count.<para/>
        /// it means the buffer should return all the data in buffer.
        /// </summary>
        public const int MaxCount = Int32.MaxValue;


        /// <summary>
        /// get an int value that indicates the length of bytes in buffer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public int Length
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("BytesBuffer");
                }

                lock (this.lockObject)
                {
                    return this.length;
                }
            }
        }

        #endregion

        #region Consturctors

        /// <summary>
        /// Constructor.
        /// </summary>
        public BytesBuffer()
        {
            this.data = new List<byte[]>();
            this.lockObject = new object();
            this.receivedEvent = new ManualResetEvent(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// add received data to buffer.
        /// </summary>
        /// <param name="buffer">
        /// a bytes array that contains the data received from server.<para/>
        /// if buffer is byte[0], directly return and do not trigger the received event.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when buffer contains no data, it is byte[0].
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when buffer is closed, cannot add bytes to buffer.
        /// </exception>
        public void Add(byte[] buffer)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BytesBuffer");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException("buffer contains no data, it is byte[0].", "buffer");
            }

            lock (this.lockObject)
            {
                if (this.isClosed)
                {
                    throw new InvalidOperationException("buffer is closed, cannot add bytes to buffer.");
                }

                this.data.Add(buffer);
                this.length += buffer.Length;
                this.receivedEvent.Set();
            }
        }


        /// <summary>
        /// remove the sub bytes array, from head to count.<para/>
        /// if the count is larger than or equals to data length, clear and return all the data directly.
        /// </summary>
        /// <param name="count">
        /// an int value that indicates the count to remove.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// thrown when count is negative.
        /// </exception>
        public void Remove(int count)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BytesBuffer");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "count must not be negative.");
            }

            if (count == 0)
            {
                return;
            }

            lock (this.lockObject)
            {
                // remove all data.
                if (count >= this.length)
                {
                    this.data.Clear();
                    this.length = 0;

                    return;
                }

                // descrease the length
                this.length -= count;

                // remove specified length data.
                int removedLength = 0;

                while (removedLength < count && this.data.Count > 0)
                {
                    int needToRemoveLength = count - removedLength;

                    byte[] firstData = this.data[0];
                    this.data.RemoveAt(0);

                    // if the first chunk need to be remove, remove it and continue.
                    if (firstData.Length < needToRemoveLength)
                    {
                        removedLength += firstData.Length;
                        continue;
                    }

                    // remove specified length data and put it back the remaing data.
                    byte[] remaingData = ArrayUtility.SubArray<byte>(firstData, needToRemoveLength);
                    this.data.Insert(0, remaingData);

                    break;
                }
            }
        }


        /// <summary>
        /// get a copy of specified count data or all data, and output the bufferClosed state.<para/>
        /// if buffer is closed, return specified cout or all data directly.<para/>
        /// if the buffer.Length &gt; minCount, donot wait and return data.<para/>
        /// otherwise, wait for the next data coming which Add/Close will trigger it.<para/>
        /// invoke this method when wait for more data, such as decode packet.<para/>
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan struct that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum number to peek.<para/>
        /// if it equals to MaxCount, return all data in buffer.
        /// </param>
        /// <param name="minCount">
        /// an int value that specifies the minimum number in buffer.<para/>
        /// if the buffer.Length &gt;= minCount, donot wait and return data.<para/>
        /// otherwise, wait for the next data coming which Add/Close will trigger it.
        /// </param>
        /// <param name="bufferClosed">
        /// return a bool value that indicates whether the buffer is closed.
        /// </param>
        /// <returns>
        /// return a bytes array that contains the data.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// thrown when it is timeout when BytesBuffer waiting for data coming
        /// </exception>
        public byte[] Read(TimeSpan timeout, int maxCount, int minCount, out bool bufferClosed)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BytesBuffer");
            }

            // the end time for operation.
            DateTime endTime = DateTime.Now + timeout;
            TimeSpan currentTimeout = timeout;

            while (true)
            {
                lock (this.lockObject)
                {
                    // if buffer is closed, return directly.
                    if (this.isClosed || minCount < this.length)
                    {
                        return this.GetSpecifiesData(maxCount, out bufferClosed);
                    }

                    // exists data is not enough, set the event to wait for more data.
                    this.receivedEvent.Reset();
                }

                // wait for receiving data
                if (currentTimeout.Ticks < 0 || !this.receivedEvent.WaitOne(currentTimeout, false))
                {
                    throw new TimeoutException("it is timeout when BytesBuffer waiting for data coming");
                }

                // update the current timeout and get data in the next loop.
                currentTimeout = endTime - DateTime.Now;
            }
        }


        /// <summary>
        /// close the buffer.<para/>
        /// it will notify all blocked thread that the buffer is close and no data will coming,<para/>
        /// so no need to wait for data coming.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Close()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BytesBuffer");
            }

            lock (this.lockObject)
            {
                // prevent invoking multiple times.
                if (this.isClosed)
                {
                    return;
                }

                this.isClosed = true;

                // to notify all blocked thread that the buffer is close and no data will coming,
                // so no need to wait for data coming.
                this.receivedEvent.Set();
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
        /// If disposing equals true, Managed and unmanaged resources are disposed. if false, Only unmanaged resources 
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
        ~BytesBuffer()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// get specifies data.
        /// </summary>
        /// <param name="maxCount">
        /// an int value that specifies the max count of return.<para/>
        /// if it equals to MaxCount, return all data in buffer.
        /// </param>
        /// <param name="bufferClosed">
        /// return a bool value that indicates whether the buffer is closed.
        /// </param>
        /// <returns>
        /// return a bytes array that contains the data.
        /// </returns>
        private byte[] GetSpecifiesData(int maxCount, out bool bufferClosed)
        {
            bufferClosed = this.isClosed;

            // if no data, return.
            if (this.data.Count == 0 || maxCount == 0)
            {
                return new byte[0];
            }

            // if only one chunks and the data is less than maxcount, return the first.
            // do not copy it.
            if (this.data.Count == 1 && this.length <= maxCount)
            {
                return this.data[0];
            }

            // get more data, return the copy of them.
            List<byte> chunkData = new List<byte>();

            for (int i = 0; i < this.data.Count; i++)
            {
                byte[] bytesData = this.data[i];

                // if the data is not enough.
                if (chunkData.Count + bytesData.Length <= maxCount)
                {
                    chunkData.AddRange(bytesData);
                }
                // data is more than needed.
                else
                {
                    chunkData.AddRange(ArrayUtility.SubArray<byte>(bytesData, 0, maxCount - chunkData.Count));
                    break;
                }
            }

            return chunkData.ToArray();
        }

        #endregion
    }
}
