// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Common exception of Xpress compression algorithm
    /// </summary>
    class XcaException : Exception
    {
        public XcaException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// Common interface of Xpress compressor
    /// </summary>
    public interface XcaCompressor
    {
        /// <summary>
        /// Compress data.
        /// </summary>
        /// <param name="data">Data to be compressed.</param>
        /// <returns>Compressed data.</returns>
        byte[] Compress(byte[] data);
    }

    /// <summary>
    /// Common interface of Xpress decompressor
    /// </summary>
    public interface XcaDecompressor
    {
        /// <summary>
        /// Decompress data.
        /// </summary>
        /// <param name="data">Data to be decompressed.</param>
        /// <returns>Decompressed Data.</returns>
        byte[] Decompress(byte[] data);
    }
}
