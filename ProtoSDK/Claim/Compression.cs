// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory
{
    /// <summary>
    /// compression for claims
    /// </summary>
    public class ClaimsCompression
    {
        /// <summary>
        /// decompress CLAIMS_SET
        /// </summary>
        /// <param name="option"></param>
        /// <param name="compressedData"></param>
        /// <param name="decompressedSize"></param>
        /// <returns>Decompressed data</returns>
        public static byte[] Decompress(CLAIMS_COMPRESSION_FORMAT option, byte[] compressedData, int decompressedSize)
        {
            XcaDecompressor decompressor;
            switch (option)
            {
                case CLAIMS_COMPRESSION_FORMAT.COMPRESSION_FORMAT_LZNT1:
                    decompressor = new LZNT1Decompressor();
                    break;
                case CLAIMS_COMPRESSION_FORMAT.COMPRESSION_FORMAT_XPRESS:
                    decompressor = new PlainLZ77Decompressor();
                    break;
                case CLAIMS_COMPRESSION_FORMAT.COMPRESSION_FORMAT_XPRESS_HUFF:
                    decompressor = new LZ77HuffmanDecompressor();
                    break;
                default:
                    throw new NotSupportedException($"{option} is not supported.");
            }

            byte[] ret = decompressor.Decompress(compressedData);
            if (ret.Length != decompressedSize)
            {
                throw new Exception($"Size of decompressed data ({ret.Length}) is different from decompressedSize ({decompressedSize}).");
            }

            return ret;
        }
    }
}
