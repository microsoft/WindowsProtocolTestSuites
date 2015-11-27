// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Defines methods for a PER ASN.1 encoding buffer.
    /// </summary>
    /// <remarks>
    /// Bit operations are supported.
    /// PER is only used by RDP Test Suites (Rdpbcgr) and only ALIGNED variant are used.
    /// Therefore the operations only deal with ALIGNED case. 
    /// </remarks>
    public interface IAsn1PerEncodingBuffer
    {
        /// <summary>
        /// Indicates the rest number of bits in last byte in the buffer.
        /// </summary>
        int RestBitsNumberInLastByte { get; }

        /// <summary>
        /// Indictes whether the buffer is defined as aligned.
        /// </summary>
        bool IsAligned { get; }

        /// <summary>
        /// Writes a bit to the buffer.
        /// </summary>
        /// <param name="data">If data is true, 1 will be written. If data is false, 0 will be written.</param>
        void WriteBit(bool data);

        /// <summary>
        /// Writes a bit to the buffer.
        /// </summary>
        /// <param name="data">If data is not equal to 0, write bit 1 to the buffer. If data if equal to 0, write bit 0 to the buffer.</param>
        void WriteBit(int data);

        /// <summary>
        /// Writes some bits to the buffer.
        /// </summary>
        /// <param name="data">If data[i] is true, 1 will be written. If data[i] is false, 0 will be written.</param>
        void WriteBits(bool[] data);

        /// <summary>
        /// Writes some bits to the buffer.
        /// </summary>
        /// <param name="data"></param>
        void WriteBits(BitArray data);

        /// <summary>
        /// Writes the bits stored in a byte arry to the buffer.
        /// </summary>
        /// <param name="data"></param>
        void WriteBits(byte data);

        /// <summary>
        /// Writes some bits stored in a byte array to the buffer.
        /// </summary>
        /// <param name="data">A byte array that stores the bits.</param>
        /// <param name="bitsOffset">The offset of the foremost bit to be written to the buffer.</param>
        /// <param name="count">The number of the bits to be written.</param>
        void WriteBits(byte[] data, int bitsOffset, int count);

        /// <summary>
        /// Writes a byte to the front of the buffer.
        /// </summary>
        /// <param name="b">The byte to be written to the buffer.</param>
        void WriteByte(byte b);

        /// <summary>
        /// Writes some bytes to the front of the buffer.
        /// </summary>
        /// <param name="bytes">An array that contains the bytes.</param>
        /// <param name="offset">The begin index of the bytes in the array.</param>
        /// <param name="count">The number of the bytes.</param>
        void WriteBytes(byte[] bytes, int offset, int count);

        /// <summary>
        /// Write all the bytes in a byte array to the front of the buffer.
        /// </summary>
        /// <param name="bytes"></param>
        void WriteBytes(byte[] bytes);

        /// <summary>
        /// Align the data by octet in the buffer.
        /// </summary>
        void AlignData();

        /// <summary>
        /// Gets the encoding result in byte array form.
        /// </summary>
        byte[] ByteArrayData { get; }

        /// <summary>
        /// Gets the encoding result in bit array form.
        /// </summary>
        bool[] BitArrayData { get; }
    }
}
