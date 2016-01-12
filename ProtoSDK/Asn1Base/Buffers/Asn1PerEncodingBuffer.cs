// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents an PER ASN.1 encoding buffer.
    /// </summary>
    /// <remarks>
    /// Bits operations are supported. 
    /// Only support the ALIGNED case.
    /// </remarks>
    public class Asn1PerEncodingBuffer : IAsn1PerEncodingBuffer, IDisposable
    {
        private MemoryStream buffer;
        private bool aligned;

        //range: [0,7], indicates the index of the position to which next bit will be written.
        //The most significant bit has index 0.
        private int curIndexInTempByte; 
        private byte tempByte;

        private void Init(bool aligned)
        {
            buffer = new MemoryStream();
            this.aligned = aligned;
            tempByte = 0;
            curIndexInTempByte = 0;
        }

        /// <summary>
        /// Create a new aligned instance of class Asn1PerEncodingBuffer.
        /// </summary>
        public Asn1PerEncodingBuffer()
            : this(true)
        {
        }

        /// <summary>
        /// Create a new instance of class Asn1PerEncodingBuffer with a parameter which specifies whether the buffer is aligned or not.
        /// </summary>
        /// <param name="aligned"></param>
        public Asn1PerEncodingBuffer(bool aligned)
        {
            if (aligned == false)
            {
                //UNALIGNED variant are not used in our Test Suite.
                throw new NotImplementedException("UNALIGNED variant are not implemented in PER.");
            }
            Init(aligned);
        }

        /// <summary>
        /// Align the data by octet in the buffer.
        /// </summary>
        public void AlignData()
        {
            if (curIndexInTempByte != 0)
            {
                buffer.WriteByte(tempByte);
                tempByte = 0;
                curIndexInTempByte = 0;
            }
        }

        /// <summary>
        /// Indictes whether the buffer is defined as aligned.
        /// </summary>
        public bool IsAligned
        {
            get { return aligned; }
        }

        /// <summary>
        /// Indicates the rest number of bits in last byte in the buffer.
        /// </summary>
        public int RestBitsNumberInLastByte
        {
            get { return 8 - curIndexInTempByte; }
        }

        #region BitOperation

        /// <summary>
        /// Writes a bit to the buffer.
        /// </summary>
        /// <param name="data">If data is true, 1 will be written. If data is false, 0 will be written.</param>
        public void WriteBit(bool data)
        {
            if(data)  
            {
                tempByte |= (byte)(1 << (7 - curIndexInTempByte));
            }
            curIndexInTempByte++;
            if (curIndexInTempByte == 8)
            {
                curIndexInTempByte = 0;
                buffer.WriteByte(tempByte);
                tempByte = 0;
            }
        }

        /// <summary>
        /// Writes a bit to the buffer.
        /// </summary>
        /// <param name="data">If data is not equal to 0, write bit 1 to the buffer. If data if equal to 0, write bit 0 to the buffer.</param>
        public void WriteBit(int data)
        {
            WriteBit(data!=0); 
        }

        /// <summary>
        /// Writes some bits to the buffer.
        /// </summary>
        /// <param name="data">If data[i] is true, 1 will be written. If data[i] is false, 0 will be written.</param>
        public void WriteBits(bool[] data)
        {
            if (data == null) return;
            foreach (var b in data)
            {
                WriteBit(b);
            }
        }

        /// <summary>
        /// Writes some bits to the buffer.
        /// </summary>
        /// <param name="data"></param>
        public void WriteBits(BitArray data)
        {
            if (data == null) return;
            foreach (var b in data)
            {
               WriteBit((bool)b); 
            }
        }

        /// <summary>
        /// Writes the bits stored in a byte arry to the buffer.
        /// </summary>
        /// <param name="data"></param>
        public void WriteBits(byte data)
        {
            for (int i = 7; i >= 0; i--)
            {
                WriteBit(data & (1<<i));
            }
        }

        /// <summary>
        /// Writes some bits stored in a byte array to the buffer.
        /// </summary>
        /// <param name="data">A byte array that stores the bits.</param>
        /// <param name="bitsOffset">The offset of the foremost bit to be written to the buffer.</param>
        /// <param name="count">The number of the bits to be written.</param>
        public void WriteBits(byte[] data, int bitsOffset, int count)
        {
            if(bitsOffset < 0) throw new Asn1InvalidArgument();
            if(count < 0) throw new Asn1InvalidArgument();

            int indexInArray = bitsOffset / 8;
            int indexInByte = bitsOffset % 8;

            for(int i=0; i<count; i++)
            {
                WriteBit(data[indexInArray] & (1 << (7 - indexInByte)));
                indexInByte++;
                if (indexInByte == 8)
                {
                    indexInByte = 0;
                    indexInArray++;
                }
            }
        }

        #endregion BitOperation

        #region ByteOperation 

        /// <summary>
        /// Writes a byte to the front of the buffer.
        /// </summary>
        /// <param name="b">The byte to be written to the buffer.</param>
        public void WriteByte(byte b)
        {
            AlignData(); 
            buffer.WriteByte(b);
        }

        /// <summary>
        /// Writes some bytes to the front of the buffer.
        /// </summary>
        /// <param name="bytes">An array that contains the bytes.</param>
        /// <param name="offset">The begin index of the bytes in the array.</param>
        /// <param name="count">The number of the bytes.</param>
        public void WriteBytes(byte[] bytes, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                WriteByte(bytes[offset+i]);
            }
        }

        /// <summary>
        /// Write all the bytes in a byte array to the front of the buffer.
        /// </summary>
        /// <param name="bytes"></param>
        public void WriteBytes(byte[] bytes)
        {
            if (bytes == null) return; 
            WriteBytes(bytes, 0, bytes.Length);
        }

        #endregion ByteOperation

        /// <summary>
        /// Gets the encoding result in byte array form.
        /// </summary>
        public byte[] ByteArrayData
        {
            get
            {
                byte[] dataInBuffer = buffer.ToArray();
                if (curIndexInTempByte == 0)
                {
                    return dataInBuffer;
                }
                byte[] allData = new byte[dataInBuffer.Length+1];
                Array.Copy(dataInBuffer, allData, dataInBuffer.Length);
                allData[allData.Length - 1] = tempByte;
                return allData;
            }
        }

        /// <summary>
        /// Gets the encoding result in bit array form.
        /// </summary>
        public bool[] BitArrayData
        {
            get
            {
                BitArray ba = new BitArray(buffer.ToArray());
                bool[] bits = new bool[ba.Count + curIndexInTempByte];
                ba.CopyTo(bits, 0);
                for (int i = 0; i < curIndexInTempByte; i++)
                {
                    bits[ba.Count + i] = ((tempByte & (1 << (7 - i))) != 0);
                }
                return bits;
            }
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
        ~Asn1PerEncodingBuffer()
        {
            Dispose(false);
        }

        #endregion Dispose

    }
}
