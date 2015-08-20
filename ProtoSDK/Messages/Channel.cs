// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Reflection;

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Runtime.Marshaling;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{

    /// <summary>
    /// A wrapper class for a stream.
    /// This class allows reading and writing values which are automatically marshalled.
    /// </summary>
    public class Channel : IDisposable
    {
        Marshaler readerMarshaler;
        Marshaler writerMarshaler;
        Stream stream;

        /// <summary>
        /// Gets underlying stream.
        /// </summary>
        public Stream Stream
        {
            get
            {
                return stream;
            }
        }

        byte[] readBuffer;
        int readBufferFill;
        byte[] writeBuffer;
        int writeBufferFill;
        object readerLock;
        object writerLock;
        int writeGrouping;

        long readOffset;
        long writeOffset;

        /// <summary>
        /// Constructs a typed stream which uses underlying stream and default marshaling configuration
        /// for block protocols.
        /// </summary>
        /// <param name="host">The message runtime host.</param>
        /// <param name="stream">The NetworkStream object.</param>
        public Channel(IRuntimeHost host, Stream stream)
            : this(host, stream, BlockMarshalingConfiguration.Configuration)
        {
        }

        /// <summary>
        /// Constructs a channel which uses underlying stream and given marshaler configuration.
        /// </summary>
        /// <param name="host">The message runtime host.</param>
        /// <param name="stream">The general stream object.</param>
        /// <param name="marshalingConfig">The marshaling configuration.</param>
        public Channel(
            IRuntimeHost host,
            Stream stream,
            MarshalingConfiguration marshalingConfig)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            this.readerMarshaler = new Marshaler(host, marshalingConfig);
            this.writerMarshaler = new Marshaler(host, marshalingConfig);
            this.stream = stream;
            this.readerLock = new object();
            this.writerLock = new object();

            if (stream.CanSeek)
            {
                readOffset = writeOffset = stream.Position;
            }
        }

        /// <summary>
        /// Returns the marshaler which is associated with this channel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Obsolete("The 'Marshaler' property is obsolete. Do not call this method.")]
        public Marshaler Marshaler
        {
            get
            {
                throw new NotSupportedException("The 'Marshaler' property is not supported.");
            }
        }

        /// <summary>
        /// Closes this channel.
        /// This method resets the marshaler and closes the stream.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void Close()
        {
            Marshaler.Reset();
            Marshaler.Reset();
        }

        /// <summary>
        /// Implements <see cref="IDisposable.Dispose"/>
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes this object.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        ~Channel()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        /// <remarks>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If the parameter 'disposing' equals true, the method is called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If the parameter 'disposing' equals false, the method is called by the 
        /// runtime from the inside of the finalizer and you should not refer to 
        /// other objects. Therefore, only unmanaged resources can be disposed.
        /// </remarks>
        /// <param name="disposing">Indicates if Dispose is called by user.</param>
        [SecurityPermission(SecurityAction.Demand)]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (readerMarshaler != null)
                {
                    readerMarshaler.Dispose();
                    readerMarshaler = null;
                }

                if (writerMarshaler != null)
                {
                    writerMarshaler.Dispose();
                    writerMarshaler = null;
                }
            }
        }

        void FillReadBuffer(int fill)
        {
            if (readBuffer == null)
            {
                readBuffer = new byte[fill * 2];
                readBufferFill = 0;
            }
            else if (readBuffer.Length < fill)
            {
                byte[] newReadBuffer = new byte[fill * 2];
                Array.Copy(readBuffer, newReadBuffer, readBufferFill);
                readBuffer = newReadBuffer;
            }
            if (readBufferFill < fill)
            {

                int n = fill - readBufferFill;
                while (n > 0)
                {
                    int b;

                    if (stream.CanSeek)
                    {
                        stream.Position = readOffset;
                        b = stream.ReadByte();
                        readOffset = stream.Position;
                    }
                    else
                    {
                        b = stream.ReadByte();
                    }

                    if (b < 0)
                    {
                        if (stream is NetworkStream)
                        {
                            readerMarshaler.TestAssertFail("connection is closed");
                        }
                        else
                        {
                            readerMarshaler.TestAssertFail("unexpected end of channel stream: expected {0} more byte(s)", n);
                        }
                    }
                    readBuffer[readBufferFill++] = (byte)b;
                    n--;
                }
            }
        }

        bool IsDataAvailable()
        {
            if (stream is NetworkStream)
            {
                return ((NetworkStream)stream).DataAvailable;
            }
            else
            {
                return (writeOffset - readOffset) > 0 ? true : false;
            }
        }

        void AllocateWriteBuffer(int offset)
        {
            if (writeBuffer == null)
            {
                writeBuffer = new byte[offset * 2];
            }
            else if (offset > writeBuffer.Length)
            {
                byte[] newWriteBuffer = new byte[offset * 2];
                Array.Copy(writeBuffer, newWriteBuffer, writeBuffer.Length);
                writeBuffer = newWriteBuffer;
            }
        }

        /// <summary>
        /// A class which implements IRegion based on Channel.
        /// </summary>
        class StreamRegion : IRegion
        {
            Channel channel;
            int startOffset;
            int offset;

            internal StreamRegion(Channel channel, int startOffset)
            {
                this.channel = channel;
                this.startOffset = startOffset;
                this.offset = startOffset;
            }

            #region IRegion Members

            public byte ReadByte()
            {
                channel.FillReadBuffer(offset + 1);
                byte x = channel.readBuffer[offset];
                offset += 1;
                return x;
            }

            unsafe public short ReadInt16()
            {
                channel.FillReadBuffer(offset + 2);
                short x;
                fixed (byte* p = &channel.readBuffer[offset])
                {
                    x = *(short*)(void*)p;
                }
                offset += 2;
                return x;
            }

            unsafe public int ReadInt32()
            {
                channel.FillReadBuffer(offset + 4);
                int x;
                fixed (byte* p = &channel.readBuffer[offset])
                {
                    x = *(int*)(void*)p;
                }
                offset += 4;
                return x;
            }

            unsafe public long ReadInt64()
            {
                channel.FillReadBuffer(offset + 8);
                long x;
                fixed (byte* p = &channel.readBuffer[offset])
                {
                    x = *(long*)(void*)p;
                }
                offset += 8;
                return x;
            }

            unsafe public IntPtr ReadIntPtr()
            {
                channel.FillReadBuffer(offset + IntPtr.Size);
                IntPtr x;
                fixed (byte* p = &channel.readBuffer[offset])
                {
                    x = *(IntPtr*)(void*)p;
                }
                offset += IntPtr.Size;
                return x;
            }

            public int SpaceLeft
            {
                get { return -1; }
            }

            public int Offset
            {
                get { return offset - startOffset; }
                set { }
            }

            public IntPtr NativeMemory
            {
                get { return IntPtr.Zero; }
            }

            public bool TryReset()
            {
                offset = 0;
                return true;
            }

            public void WriteByte(byte x)
            {
                channel.AllocateWriteBuffer(offset + 1);
                channel.writeBuffer[offset] = x;
                offset += 1;
            }

            unsafe public void WriteInt16(short x)
            {
                channel.AllocateWriteBuffer(offset + 2);
                fixed (byte* p = &channel.writeBuffer[offset])
                {
                    *(short*)(void*)p = x;
                }
                offset += 2;
            }

            unsafe public void WriteInt32(int x)
            {
                channel.AllocateWriteBuffer(offset + 4);
                fixed (byte* p = &channel.writeBuffer[offset])
                {
                    *(int*)(void*)p = x;
                }
                offset += 4;
            }

            unsafe public void WriteInt64(long x)
            {
                channel.AllocateWriteBuffer(offset + 8);
                fixed (byte* p = &channel.writeBuffer[offset])
                {
                    *(long*)(void*)p = x;
                }
                offset += 8;
            }

            unsafe public void WriteIntPtr(IntPtr x)
            {
                channel.AllocateWriteBuffer(offset + IntPtr.Size);
                fixed (byte* p = &channel.writeBuffer[offset])
                {
                    *(IntPtr*)(void*)p = x;
                }
                offset += IntPtr.Size;
            }

            public bool UseSpaceChecking
            {
                get
                {
                    throw new InvalidOperationException("Not supported by StreamRegion");
                }

                set
                {
                    throw new InvalidOperationException("Not supported by StreamRegion");
                }
            }

            #endregion
        }

        /// <summary>
        /// Reads a value of the given type T from the stream which uses the underlying marshaler to unmarshal it.
        /// </summary>
        /// <typeparam name="T">The type of the value to be read.</typeparam>
        /// <returns>The value read from the channel.</returns>
        public virtual T Read<T>()
        {
            lock (readerLock)
            {
                StreamRegion region = new StreamRegion(this, 0);
                object value = readerMarshaler.UnmarshalFrom(new MarshalingDescriptor(typeof(T)), region);
                AdvanceReadBuffer(region.Offset);
                return (T)value;
            }
        }


        void AdvanceReadBuffer(int offset)
        {
            if (offset >= readBufferFill)
            {
                readBuffer = null;
                readBufferFill = 0;
            }
            else
            {
                int remains = readBufferFill - offset;
                Array.Copy(readBuffer, offset, readBuffer, 0, remains);
                readBufferFill = remains;
            }
        }

        /// <summary>
        /// Reads the given number of bytes from the stream.
        /// </summary>
        /// <param name="count">The number of bytes which is read from the stream.</param>
        /// <returns>The byte array which contains the bytes reading from the stream.</returns>
        public byte[] ReadBytes(int count)
        {
            byte[] result = new byte[count];
            lock (readerLock)
            {
                StreamRegion region = new StreamRegion(this, 0);
                for (int i = 0; i < count; i++)
                    result[i] = region.ReadByte();
                AdvanceReadBuffer(region.Offset);
                return result;
            }
        }

        /// <summary>
        /// Peeks the value at given offset in the stream without changing stream position.
        /// </summary>
        /// <typeparam name="T">The type of the value which is peeked from the stream.</typeparam>
        /// <param name="offset">The offset which the channel peeks value from.</param>
        /// <returns>The value peeked from the channel.</returns>
        public T Peek<T>(int offset)
        {
            lock (readerLock)
            {
                StreamRegion region = new StreamRegion(this, offset);
                object x = readerMarshaler.UnmarshalFrom(new MarshalingDescriptor(typeof(T)), region);
                return (T)x;
            }
        }

        /// <summary>
        /// Begins a write group.
        /// All subsequent write operations are buffered.
        /// </summary>
        public void BeginWriteGroup()
        {
            lock (writerLock)
            {
                writeGrouping++;
            }
        }

        /// <summary>
        /// Ends a write group. 
        /// If the number of nested write groups drops to zero, 
        /// the content of the write buffer is flushed to the network.
        /// </summary>
        public void EndWriteGroup()
        {
            lock (writerLock)
            {
                if (writeGrouping == 0)
                    throw new InvalidOperationException("no write group to end here");

                writeGrouping--;

                if (writeGrouping == 0 && writeBuffer != null)
                {
                    FlushWriteBuffer();
                }
            }
        }

        /// <summary>
        /// Writes a value of given type T to the stream which uses the underlying marshaler to marshal it.
        /// </summary>
        /// <typeparam name="T">The type of the value which is written to the stream.</typeparam>
        /// <param name="value">The value which is written to the stream.</param>
        public virtual void Write<T>(T value)
        {
            lock (writerLock)
            {
                StreamRegion region = new StreamRegion(this, writeBufferFill);
                writerMarshaler.MarshalInto(new MarshalingDescriptor(typeof(T)), region, value);
                writeBufferFill += region.Offset;
                if (writeGrouping == 0)
                    FlushWriteBuffer();
            }
        }

        /// <summary>
        /// Writes the given bytes of data to the stream.
        /// </summary>
        /// <param name="data">The byte array which contains the data that is written to the stream.</param>
        public void WriteBytes(byte[] data)
        {
            lock (writerLock)
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }

                StreamRegion region = new StreamRegion(this, writeBufferFill);
                for (int i = 0; i < data.Length; i++)
                    region.WriteByte(data[i]);
                writeBufferFill += region.Offset;
                if (writeGrouping == 0)
                    FlushWriteBuffer();
            }
        }

        void FlushWriteBuffer()
        {
            if (writeBuffer != null)
            {
                if (stream.CanSeek)
                {
                    stream.Position = writeOffset;
                    stream.Write(writeBuffer, 0, writeBufferFill);
                    writeOffset = stream.Position;
                }
                else
                {
                    stream.Write(writeBuffer, 0, writeBufferFill);
                }

                if (writerMarshaler.tracing)
                {
                    StringBuilder b = new StringBuilder();
                    for (int i = 0; i < writeBufferFill; i++)
                        b.AppendFormat(" {0:x2}", writeBuffer[i]);
                    writerMarshaler.Trace("sending {0} bytes: {1}", writeBufferFill, b.ToString());
                }
                stream.Flush();
                writeBuffer = null;
                writeBufferFill = 0;
            }
        }
    }
}
