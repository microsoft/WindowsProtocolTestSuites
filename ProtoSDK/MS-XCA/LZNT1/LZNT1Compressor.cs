// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Compressor Implementation of LZNT1 Compression Algorithm
    /// </summary>
    public class LZNT1Compressor : XcaCompressor
    {
        const int MaximumMatchDistance = 4097;
        const int MaximumMatchLength = 4099;


        static LZ77Code lz77 = new LZ77Code(MaximumMatchDistance, MaximumMatchLength);

        public byte[] Compress(byte[] arg)
        {
            var result = new LZNT1_Buffer();

            var chunks = new List<LZNT1_Chunk>();
            int index = 0;
            while (index < arg.Length)
            {
                int length = Math.Min(arg.Length - index, 4096);

                var chunk = CompressChunk(arg.Skip(index).Take(length).ToArray());

                chunks.Add(chunk);

                index += length;
            }


            result.Chunk = chunks.ToArray();

            var buffer = new LittleEndianByteBuffer();
            int offset = 0;

            result.Marshal(buffer, ref offset);

            return buffer.GetBytes();
        }

        private static LZNT1_Chunk CompressChunk(byte[] arg)
        {
            var symbols = lz77.Encode(arg);

            int processedBytes = 0;

            var lznt1Data = new List<LZNT1_Data>();

            foreach (var symbol in symbols)
            {
                var lznt1DataForSymbol = LZNT1_Data.GenerateFromLZ77Symbol(arg, symbol, ref processedBytes);

                lznt1Data.AddRange(lznt1DataForSymbol);
            }

            var result = LZNT1_Chunk.Compress(arg, lznt1Data);

            return result;

        }
    }
}
