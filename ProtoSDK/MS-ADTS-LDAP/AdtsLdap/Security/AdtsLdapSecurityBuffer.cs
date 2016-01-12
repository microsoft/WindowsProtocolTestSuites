// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// a thread-safe bytes buffer, to manage bytes data.
    /// </summary>
    public class AdtsLdapSecurityBuffer
    {
        #region Fields

        /// <summary>
        /// a bytes array that contains the data.
        /// </summary>
        private byte[] data;

        /// <summary>
        /// an object value for lock.
        /// </summary>
        private object lockObject;

        #endregion

        #region Properties

        /// <summary>
        /// get a bytes array that contains a copy of data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                lock (this.lockObject)
                {
                    return ArrayUtility.SubArray<byte>(data, 0);
                }
            }
        }


        /// <summary>
        /// get an int value that indicates the length of buffer.
        /// </summary>
        public int Length
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.data.Length;
                }
            }
        }

        #endregion

        #region Consturctors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transport">
        /// an AdtsLdapSslTlsSecurityLayer object that provides the security layer.
        /// </param>
        public AdtsLdapSecurityBuffer()
        {
            this.data = new byte[0];
            this.lockObject = new object();
        }

        #endregion

        #region Methods

        /// <summary>
        /// add received data to buffer.
        /// </summary>
        /// <param name="buffer">
        /// a bytes array that contains the data received from server.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public void AddReceivedData(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            lock (this.lockObject)
            {
                this.data = ArrayUtility.ConcatenateArrays<byte>(this.data, buffer);
            }
        }


        /// <summary>
        /// read data from buffer, remove the read data.<para/>
        /// if not enough data, return 0.
        /// </summary>
        /// <param name="buffer">
        /// a bytes buffer that is used to stores the data.
        /// </param>
        /// <param name="offset">
        /// an int value that indicates the offset at which start to store data.
        /// </param>
        /// <param name="count">
        /// an int value that indicates the maximum count to read the data.
        /// </param>
        /// <returns>
        /// an int value that indicates the read count.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// thrown when offset must not be negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// thrown when count must not be negative.
        /// </exception>
        public int Read(byte[] buffer, int offset, int count)
        {
            if (count == 0)
            {
                return 0;
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", offset, "offset must not be negative.");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "count must not be negative.");
            }

            lock (this.lockObject)
            {
                if (this.data.Length < count)
                {
                    return 0;
                }

                Array.Copy(this.data, 0, buffer, offset, count);

                this.Remove(count);

                return count;
            }
        }


        /// <summary>
        /// remove the sub bytes array, from head to count.
        /// </summary>
        /// <param name="count">
        /// an int value that indicates the count to remove.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// thrown when count must not be negative.
        /// </exception>
        public void Remove(int count)
        {
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
                this.data = ArrayUtility.SubArray<byte>(this.data, count);
            }
        }


        /// <summary>
        /// remove all data of buffer.
        /// </summary>
        public void Clear()
        {
            lock (this.lockObject)
            {
                this.data = new byte[0];
            }
        }


        /// <summary>
        /// peek the bytes, get a copy of specifies count bytes data.
        /// </summary>
        /// <param name="count">
        /// an int value that specifies the maximum number to peek.
        /// </param>
        /// <returns>
        /// a bytes data that contains the peek data.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// thrown when count must be larger than 0.
        /// </exception>
        public byte[] Peek(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "count must be larger than 0.");
            }

            lock (this.lockObject)
            {
                if (count > this.data.Length)
                {
                    return this.Data;
                }

                return ArrayUtility.SubArray<byte>(this.data, 0, count);
            }
        }

        #endregion
    }
}