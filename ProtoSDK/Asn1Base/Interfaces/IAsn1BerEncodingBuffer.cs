// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Defines methods for a BER ASN.1 encoding buffer.
    /// </summary>
    /// <remarks>
    /// This interface is used when BER encoding.
    /// This buffer is a reversed buffer.
    /// A buffer that implements this interface should write data to the front of this buffer each time its "Write" or "WriteByte" is invoked
    /// When "Write" method is invoked, the order of the data in the byte array keeps in the buffer.
    /// </remarks>
    /// <example>
    ///     //buffer.Data: empty
    /// buffer.writeByte(1)
    ///     //buffer.Data: 1
    /// buffer.writeByte(2)
    ///     //buffer.Data: 21
    /// buffer.write(new byte[]{4,5,6})
    ///     //buffer.Data: 45612
    /// </example>
    public interface IAsn1BerEncodingBuffer
    {
        /// <summary>
        /// Gets the encoding result.
        /// </summary>
        byte[] Data { get; }
        
        /// <summary>
        /// Writes a byte to the front of the buffer.
        /// </summary>
        /// <param name="b">The byte to be written to the buffer</param>
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
    }
}
