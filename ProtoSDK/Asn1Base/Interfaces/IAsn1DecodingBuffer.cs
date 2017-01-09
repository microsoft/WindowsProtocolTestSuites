// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Defines methods for an ASN.1 decoding buffer.
    /// </summary>
    /// <remarks>
    /// Used when decoding.
    /// Only the buffers that implement this interface could be passed as parameters when decoding.
    /// </remarks>
    public interface IAsn1DecodingBuffer
    {
        /// <summary>
        /// Reads a byte from the buffer.
        /// </summary>
        /// <returns>Value of the byte.</returns>
        /// <remarks>
        /// The Position in the buffer moves to next byte.
        /// </remarks>
        byte ReadByte();

        /// <summary>
        /// Reads some bytes from the buffer.
        /// </summary>
        /// <param name="count">The number of the bytes.</param>
        /// <returns>A byte array that contains the bytes read from the buffer.</returns>
        byte[] ReadBytes(int count);

        /// <summary>
        /// Reads a bit from the buffer.
        /// </summary>
        /// <returns>Value of the bit.</returns>
        bool ReadBit();

        /// <summary>
        /// Reads some bits from the buffer.
        /// </summary>
        /// <param name="count">The number of the bits.</param>
        /// <returns>A byte array that contains the bits read from the buffer.</returns>
        bool[] ReadBits(int count);

        /// <summary>
        /// Seeks byte position in the buffer.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position.</param>
        void SeekBytePosition(long offset, SeekOrigin origin = SeekOrigin.Current);

        /// <summary>
        /// Align the data by octet in the buffer.
        /// </summary>
        void AlignData();

        /// <summary>
        /// Peek one byte use current position, Position not changed.
        /// </summary>
        /// <returns>byte on current postion</returns>
        byte PeekByte();

        /// <summary>
        /// Check if read to end.
        /// </summary>
        /// <returns>True means postion = length</returns>
        bool IsNomoreData();
    }
}
