// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// a stream proxy, which provides the stream for SslStream, <para/>
    /// and specifies a set of interfaces to access the bytes data of SslStream Read and Write.
    /// </summary>
    public class AdtsLdapSecurityStream : Stream
    {
        #region Fields

        /// <summary>
        /// an AdtsLdapSslTlsSecurityLayer object that provides the security layer.<para/>
        /// this param will never be null.
        /// </summary>
        private AdtsLdapSslTlsSecurityLayer security;

        /// <summary>
        /// a ManualResetEvent object that is used to notify the received data.<para/>
        /// a ReceiveThread received data from server, if the data is enough, this thread reset this event,<para/>
        /// and start to read by this stream proxy.
        /// </summary>
        private ManualResetEvent receivedEvent;

        /// <summary>
        /// an int value that indicates the expected count of data to receive from server.
        /// </summary>
        private int expectedCount;

        /// <summary>
        /// a bytes array that contains the received data.
        /// </summary>
        private AdtsLdapSecurityBuffer receivedBuffer;

        /// <summary>
        /// a bytes array that contains the data to send.
        /// </summary>
        private AdtsLdapSecurityBuffer sentBuffer;

        /// <summary>
        /// an object that is used for lock between write(ReceivedThread) and read(WorkThread).
        /// </summary>
        private object lockObjectForReadWriteThread;

        /// <summary>
        /// an object that is used for lock between read threads.
        /// </summary>
        private object lockObjectForReadThreads;

        /// <summary>
        /// get a bool value that indicates whether data is available to read.
        /// </summary>
        private volatile bool waitingForDataComing;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transport">
        /// an AdtsLdapSslTlsSecurityLayer object that provides the security layer.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when security is null.
        /// </exception>
        public AdtsLdapSecurityStream(AdtsLdapSslTlsSecurityLayer security)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            this.security = security;
            this.receivedBuffer = new AdtsLdapSecurityBuffer();
            this.sentBuffer = new AdtsLdapSecurityBuffer();
            this.receivedEvent = new ManualResetEvent(false);
            this.lockObjectForReadWriteThread = new object();
            this.lockObjectForReadThreads = new object();
        }

        #endregion

        #region Properties

        /// <summary>
        /// get a bool value that indicates whether data is available to read.
        /// </summary>
        public bool WaitingForDataComing
        {
            get
            {
                return this.waitingForDataComing;
            }
        }


        /// <summary>
        /// get a bool value that indicates whether stream can read. return true.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }


        /// <summary>
        /// get a bool value that indicates whether stream can seek. return false.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// get a bool value that indicates whether stream can write. return true.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }


        /// <summary>
        /// get an AdtsLdapSecurityBuffer object that contains the data to send.
        /// </summary>
        public AdtsLdapSecurityBuffer SentBuffer
        {
            get
            {
                return this.sentBuffer;
            }
        }


        /// <summary>
        /// get an AdtsLdapSecurityBuffer object that contains the received data.
        /// </summary>
        public AdtsLdapSecurityBuffer ReceivedBuffer
        {
            get
            {
                return this.receivedBuffer;
            }
        }

        #endregion

        #region Not Supports

        /// <summary>
        /// get a long value that indicates the length of stream. do not support.
        /// </summary>
        public override long Length
        {
            get
            {
                throw new NotSupportedException("get length of stream is not supported");
            }
        }


        /// <summary>
        /// get/set a long value that indicate the position of stream. do not support.
        /// </summary>
        public override long Position
        {
            get
            {
                throw new NotSupportedException("get position of stream is not supported");
            }
            set
            {
                throw new NotSupportedException("set position of stream is not supported");
            }
        }


        /// <summary>
        /// seek stream. do not supported.
        /// </summary>
        /// <param name="offset">
        /// a long value that indicates the offset to seek.
        /// </param>
        /// <param name="origin">
        /// a SeekOrigin object that indicates the origin of seek.
        /// </param>
        /// <returns>
        /// a long value that indicates the seek result.
        /// </returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("seek of stream is not supported");
        }


        /// <summary>
        /// set the length of stream. do not support.
        /// </summary>
        /// <param name="value">
        /// a long value that indicates the new length.
        /// </param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("seek of stream is not supported");
        }


        /// <summary>
        /// flush the buffer, write to under layer device. do not support.
        /// </summary>
        public override void Flush()
        {
            throw new NotSupportedException("flush of stream is not supported");
        }

        #endregion

        #region Methods

        /// <summary>
        /// add received data to buffer, once count is enough, reset the read event for user to read.
        /// </summary>
        /// <param name="data">
        /// a bytes array that contains the data received from server.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public void AddReceivedData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // prevent read thread.
            lock (this.lockObjectForReadWriteThread)
            {
                this.receivedBuffer.AddReceivedData(data);

                if (this.receivedBuffer.Length >= this.expectedCount)
                {
                    this.waitingForDataComing = false;
                    this.receivedEvent.Set();
                }
            }
        }


        /// <summary>
        /// read data from stream. wait for the under-layer to receive data from server.
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
        public override int Read(byte[] buffer, int offset, int count)
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

            // prevent other read threads.
            lock (this.lockObjectForReadThreads)
            {
                if (!this.security.IsAuthenticated)
                {
                    byte[] data = null;

                    // if client, send bytes according client.
                    if (this.security.ClientTransport != null)
                    {
                        data = this.security.ClientTransport.ExpectBytes(new TimeSpan(Int32.MaxValue), count);
                    }
                    // if server, send bytes according server.
                    else
                    {
                        data = this.security.ServerTransport.ExpectBytes(
                            new TimeSpan(Int32.MaxValue), count, this.security.ServerContext);
                    }

                    Array.Copy(data, 0, buffer, offset, data.Length);

                    return data.Length;
                }
                else
                {
                    // wait until the data is enough.
                    while (true)
                    {
                        // prevent write thread.
                        lock (this.lockObjectForReadWriteThread)
                        {
                            // if the data is not enough, reset event.
                            if (this.receivedBuffer.Length < count)
                            {
                                this.expectedCount = count;
                                this.waitingForDataComing = true;
                                this.receivedEvent.Reset();
                            }
                            else
                            {
                                this.waitingForDataComing = false;
                                break;
                            }
                        }

                        // wait for the data coming.
                        this.receivedEvent.WaitOne();
                    }

                    return this.receivedBuffer.Read(buffer, offset, count);
                }
            }
        }


        /// <summary>
        /// write data to stream.<para/>
        /// write to the under-layer transport to server for sslAuthenticate.<para/>
        /// write to buffer after ssl authenticate for encode with ssl.
        /// </summary>
        /// <param name="buffer">
        /// a bytes array that contains the data to write to server.
        /// </param>
        /// <param name="offset">
        /// an int value that indicates the offset at which to start to write.
        /// </param>
        /// <param name="count">
        /// an int value that indicates the count of data to write to server.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// thrown when offset must not be negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// thrown when count must not be negative.
        /// </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count == 0)
            {
                return;
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

            byte[] data = ArrayUtility.SubArray<byte>(buffer, offset, count);

            if (!this.security.IsAuthenticated)
            {
                // if client, send bytes according client.
                if (this.security.ClientTransport != null)
                {
                    this.security.ClientTransport.SendBytes(data);
                }
                // if server, send bytes according server.
                else
                {
                    this.security.ServerTransport.SendBytes(this.security.ServerContext, data);
                }
            }
            else
            {
                this.sentBuffer.AddReceivedData(data);
            }
        }

        #endregion
    }
}