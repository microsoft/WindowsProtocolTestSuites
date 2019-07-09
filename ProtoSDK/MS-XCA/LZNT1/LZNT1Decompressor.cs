// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Decompressor Implementation of LZNT1 Decompression Algorithm
    /// </summary>
    public class LZNT1Decompressor : XcaDecompressor
    {
        const int MaximumMatchDistance = 4097;
        const int MaximumMatchLength = 4099;


        static LZ77Code lz77Code = new LZ77Code(MaximumMatchDistance, MaximumMatchLength);

        /// <summary>
        /// Decompress data.
        /// </summary>
        /// <param name="data">Data to be decompressed.</param>
        /// <returns>Decompressed Data.</returns>
        public byte[] Decompress(byte[] data)
        {
            var buffer = new LittleEndianByteBuffer(data);
            int offset = 0;
            var lznt1Buffer = LZNT1_Object.Unmarshal<LZNT1_Buffer>(buffer, ref offset);
            var result = new List<byte>();
            foreach (var chunk in lznt1Buffer.Chunk)
            {
                var symbols = chunk.ParseToLZ77Symbols();

                result.AddRange(lz77Code.Decode(symbols));
            }

            return result.ToArray();
        }
    }
}
