// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Compressor Implementation of Plain LZ77 Compression Algorithm
    /// </summary>
    public class PlainLZ77Compressor : XcaCompressor
    {
        const int MaximumMatchDistance = 8192;
        const int MaximumMatchLength = 65538;

        static LZ77Code lz77Code = new LZ77Code(MaximumMatchDistance, MaximumMatchLength);

        /// <summary>
        /// Compress data.
        /// </summary>
        /// <param name="data">Data to be compressed.</param>
        /// <returns>Compressed data.</returns>
        public byte[] Compress(byte[] data)
        {
            var encoder = new PlainLZ77Encoder();

            var lz77Symbols = lz77Code.Encode(data);

            var result = encoder.Encode(lz77Symbols);

            return result;
        }
    }

    /// <summary>
    /// Plain LZ77 Encoder.
    /// </summary>
    class PlainLZ77Encoder
    {
        /// <summary>
        /// Encode LZ77 symbols.
        /// </summary>
        /// <param name="symbols">LZ77 symbols to be encoded.</param>
        /// <returns>Byte array containing encoded result.</returns>
        public byte[] Encode(List<LZ77Symbol> symbols)
        {
            long Flags = 0;
            int FlagCount = 0;
            int FlagOutputPosition = 0;
            int OutputPosition = 4;
            int LastLengthHalfByte = 0;

            var buffer = new LittleEndianByteBuffer();

            foreach (var symbol in symbols)
            {
                if (symbol is LZ77Literal)
                {
                    var literal = symbol as LZ77Literal;

                    buffer.WriteBytes(OutputPosition, literal.Literal, 1);

                    OutputPosition += 1;

                    Flags <<= 1;

                    FlagCount += 1;

                }
                else if (symbol is LZ77Match)
                {
                    var match = symbol as LZ77Match;
                    int MatchLength = match.Length;
                    int MatchOffset = match.Distance;

                    MatchLength -= 3;

                    MatchOffset -= 1;

                    MatchOffset <<= 3;

                    MatchOffset |= Math.Min(MatchLength, 7);

                    buffer.WriteBytes(OutputPosition, MatchOffset, 2);

                    OutputPosition += 2;

                    if (MatchLength >= 7)
                    {
                        MatchLength -= 7;

                        if (LastLengthHalfByte == 0)
                        {
                            LastLengthHalfByte = OutputPosition;

                            buffer.WriteBytes(OutputPosition, Math.Min(MatchLength, 15), 1);

                            OutputPosition += 1;
                        }
                        else
                        {
                            byte LastLength = buffer[LastLengthHalfByte];

                            LastLength |= (byte)(Math.Min(15, MatchLength) << 4);

                            buffer.WriteBytes(LastLengthHalfByte, LastLength, 1);

                            LastLengthHalfByte = 0;
                        }

                        if (MatchLength >= 15)
                        {
                            MatchLength -= 15;

                            buffer.WriteBytes(OutputPosition, Math.Min(MatchLength, 255), 1);

                            OutputPosition += 1;

                            if (MatchLength >= 255)
                            {

                                MatchLength += (15 + 7);

                                buffer.WriteBytes(OutputPosition, MatchLength, 2);

                                OutputPosition += 2;
                            }
                        }
                    }
                    Flags = (Flags << 1) | 1;

                    FlagCount += 1;

                }
                else if (symbol is LZ77EOF)
                {
                    // Do nothing to EOF.
                }
                else
                {
                    throw new XcaException("Unreachable code!");
                }

                if (FlagCount == 32)
                {
                    buffer.WriteBytes(FlagOutputPosition, (int)Flags, 4);

                    FlagCount = 0;

                    FlagOutputPosition = OutputPosition;

                    OutputPosition += 4;
                }
            }

            Flags <<= (32 - FlagCount);
            Flags |= (((long)1 << (32 - FlagCount)) - 1);

            buffer.WriteBytes(FlagOutputPosition, (int)Flags, 4);

            return buffer.GetBytes();

        }
    }
}
