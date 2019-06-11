// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Compressor Implementation of LZ77+Huffman Compression Algorithm
    /// </summary>
    public class LZ77HuffmanCompressor : XcaCompressor
    {
        const int MaximumMatchDistance = 65535;
        const int MaximumMatchLength = 65538;


        static LZ77Code lz77Code = new LZ77Code(MaximumMatchDistance, MaximumMatchLength);

        /// <summary>
        /// Compress data.
        /// </summary>
        /// <param name="data">Data to be compressed.</param>
        /// <returns>Compressed data.</returns>
        public byte[] Compress(byte[] data)
        {
            var encoder = new HuffmanEncoder();

            var lz77Symbols = lz77Code.Encode(data);

            var result = encoder.Encode(lz77Symbols);

            return result;
        }
    }
}
