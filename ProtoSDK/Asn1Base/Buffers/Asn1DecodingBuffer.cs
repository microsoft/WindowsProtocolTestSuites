// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents an ASN.1 decoding buffer.
    /// </summary>
    public class Asn1DecodingBuffer : IAsn1DecodingBuffer, IDisposable
    {
        private MemoryStream buffer;
        private int tempData;
        private int curBitIndexInTempData;

        /// <summary>
        /// Align the data by octet in the buffer.
        /// </summary>
        public void AlignData()
        {
            if (RestBitsNumberInTempByte != 8)
            {
                RefreshTempByte();
            }
        }

        /// <summary>
        /// Indicates the rest number of bits in temp byte in the buffer.
        /// </summary>
        public int RestBitsNumberInTempByte
        {
            get { return 8 - curBitIndexInTempData; }
        }

        /// <summary>
        /// Initializes a new instance of the Asn1DecodingBuffer class that contains a BER/PER encoding result.
        /// </summary>
        /// <param name="bytes">An encoding result used to construct the buffer.</param>
        public Asn1DecodingBuffer(byte[] bytes)
        {
            buffer = new MemoryStream(bytes);
            RefreshTempByte();
        }

        /// <summary>
        /// Reads a byte from the buffer.
        /// </summary>
        /// <returns>Value of the byte.</returns>
        /// <exception cref="Asn1DecodingOutOfBufferRangeException">Thrown when decoding is out of range (no enough bytes can be read from the buffer).</exception>
        /// <remarks>
        /// The Position in the buffer moves to next byte.
        /// </remarks>
        public byte ReadByte()
        {
            if (IsNomoreData())
            {
                throw new Asn1DecodingOutOfBufferRangeException(
                     ExceptionMessages.DecodingOutOfRange); 
            }

            int nextVal;
            if (curBitIndexInTempData == 0)
            {
                nextVal = tempData;
            }
            else
            {
                nextVal = buffer.ReadByte();
            }
            RefreshTempByte();
            return (byte)nextVal;
        }

        public byte PeekByte()
        {
            if (curBitIndexInTempData == 0 && tempData == -1
                || curBitIndexInTempData != 0 && buffer.Position == buffer.Length)
            {
                throw new Asn1DecodingOutOfBufferRangeException(
                     ExceptionMessages.DecodingOutOfRange);
            }

            int nextVal;
            if (curBitIndexInTempData == 0)
            {
                nextVal = tempData;
            }
            else
            {
                nextVal = buffer.ReadByte();
                buffer.Position -= 1;
            }
            return (byte)nextVal;
        }

        public bool IsNomoreData()
        {
            return curBitIndexInTempData == 0 && tempData == -1
                || curBitIndexInTempData != 0 && buffer.Position == buffer.Length;
        }

        /// <summary>
        /// Reads some bytes from the buffer.
        /// </summary>
        /// <param name="count">The number of the bytes.</param>
        /// <returns>A byte array that contains the bytes read from the buffer.</returns>
        /// <exception cref="Asn1DecodingOutOfBufferRangeException">Thrown when decoding is out of range (no enough bytes can be read from the buffer).</exception>
        public byte[] ReadBytes(int count)
        {
            if (curBitIndexInTempData == 0 && buffer.Position - 1 + count > buffer.Length
                || curBitIndexInTempData != 0 && buffer.Position + count > buffer.Length)
            {
                throw new Asn1DecodingOutOfBufferRangeException(
                      ExceptionMessages.DecodingOutOfRange);  
            }
            byte[] result = new byte[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = ReadByte();
            }
            return result;
        }

        /// <summary>
        /// Seeks byte position in the buffer.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position.</param>
        public void SeekBytePosition(long offset, SeekOrigin origin = SeekOrigin.Current)
        {
            if (curBitIndexInTempData != 0)
            {
                throw new Asn1InvalidOperation(ExceptionMessages.SeekPositionFailed + " Some bits are read already. It's not octet aligned.");
            }
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset < 0L || offset >= buffer.Length)
                    {
                        throw new Asn1InvalidArgument(ExceptionMessages.SeekPositionFailed);
                    }
                    buffer.Position = offset;
                    break;

                case SeekOrigin.Current:
                    if (offset + buffer.Position - 1 < 0 || offset + buffer.Position - 1 >= buffer.Length)
                    {
                        throw new Asn1InvalidArgument(ExceptionMessages.SeekPositionFailed);
                    }
                    buffer.Position += offset-1;
                    break;

                case SeekOrigin.End:
                    if (offset > 0 || buffer.Length + offset < 0)
                    {
                        throw new Asn1InvalidArgument(ExceptionMessages.SeekPositionFailed);
                    }
                    buffer.Position = buffer.Length + offset;
                    break;

                default:
                    throw new Asn1InvalidArgument("Seek origin is invalid");
            }
            RefreshTempByte();
        }

        private void RefreshTempByte()
        {
            curBitIndexInTempData = 0;
            tempData = buffer.ReadByte();
        }

        /// <summary>
        /// Reads a bit from the buffer.
        /// </summary>
        /// <returns>Value of the bit. True stands for 1, false stands for 0.</returns>
        public bool ReadBit()
        {
            if (curBitIndexInTempData == 8)
            {
                RefreshTempByte();
            }
            if (tempData == -1)
            {
                throw new Asn1DecodingOutOfBufferRangeException(ExceptionMessages.DecodingOutOfRange);
            }
            return ((1 << (7 - curBitIndexInTempData++)) & tempData) != 0;
        }

        /// <summary>
        /// Reads some bits from the buffer.
        /// </summary>
        /// <param name="count">The number of the bits.</param>
        /// <returns>A byte array that contains the bits read from the buffer.</returns>
        public bool[] ReadBits(int count)
        {
            long restCount = RestBitsNumberInTempByte + 8*(buffer.Length - buffer.Position);
            if (restCount < count)
            {
                throw new Asn1DecodingOutOfBufferRangeException(ExceptionMessages.DecodingOutOfRange); 
            }
            bool[] bits = new bool[count];
            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = ReadBit();
            }
            return bits;
        }

        #region Dispose

        private bool disposed = false;

        /// <summary>
        /// Dispose method from IDisposable.
        /// </summary>
        /// <param name="disposing">Indicating whether the managed resources need to be disposed immediately.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //managed resources are disposed here
                    buffer.Dispose();
                }

                //unmanaged resources are disposed here

                this.disposed = true;
            }
        }

        /// <summary>
        /// Dispose method that can be manually called.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor which disposes the unmanaged resources.
        /// </summary>
        ~Asn1DecodingBuffer()
        {
            Dispose(false);
        }

        #endregion Dispose

    }
}
