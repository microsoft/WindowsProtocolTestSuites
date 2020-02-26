// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Compressor Implementation of LZ77+Huffman Compression Algorithm
    /// </summary>
    public class LZ77HuffmanCompressor : XcaCompressor
    {
        const int MaximumMatchDistance = 65535;
        const int MaximumMatchLength = 65538;
        const int BlockSize = 65536;

        static LZ77Code lz77Code = new LZ77Code(MaximumMatchDistance, MaximumMatchLength);

        /// <summary>
        /// Compress data.
        /// </summary>
        /// <param name="data">Data to be compressed.</param>
        /// <returns>Compressed data.</returns>
        public byte[] Compress(byte[] data)
        {
            var result = new List<byte>();

            for (int i = 0; i < data.Length; i += BlockSize)
            {
                int blockLength = BlockSize;

                if (i + blockLength > data.Length)
                {
                    blockLength = data.Length - i;
                }

                bool isLast = false;

                if (i + blockLength == data.Length)
                {
                    isLast = true;
                }

                var block = data.Skip(i).Take(blockLength).ToArray();

                var compressedData = CompressBlock(block, isLast);

                result.AddRange(compressedData);
            }

            return result.ToArray();
        }

        private byte[] CompressBlock(byte[] data, bool isLast)
        {
            var encoder = new HuffmanEncoder();

            var lz77Symbols = lz77Code.Encode(data);

            if (isLast)
            {
                lz77Symbols.Add(new LZ77EOF());
            }

            var result = encoder.Encode(lz77Symbols);

            return result;
        }
    }
}
