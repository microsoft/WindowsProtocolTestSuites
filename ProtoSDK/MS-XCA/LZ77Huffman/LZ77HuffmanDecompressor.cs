// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Decompressor Implementation of LZ77+Huffman Decompression Algorithm
    /// </summary>
    public class LZ77HuffmanDecompressor : XcaDecompressor
    {
        /// <summary>
        /// Decompress data.
        /// </summary>
        /// <param name="data">Data to be decompressed.</param>
        /// <returns>Decompressed Data.</returns>
        public byte[] Decompress(byte[] data)
        {
            int CurrentPosition = 0;

            var result = new List<byte>();

            /*
                Loop until a decompression terminating condition
	                Build the decoding table
	                CurrentPosition = 256    // start at the end of the Huffman table
	                NextBits = Read16Bits(InputBuffer + CurrentPosition)
	                CurrentPosition += 2
	                NextBits <<= 16
	                NextBits |= Read16Bits(InputBuffer + CurrentPosition)
	                CurrentPosition += 2
	                ExtraBits = 16
	                BlockEnd = OutputPosition + 65536
	
	                Loop until a block terminating condition
	                    If OutputPosition >= BlockEnd then terminate block processing
	                    Loop until a literal processing terminating condition
	                        Next15Bits = NextBits >> (32 – 15)
	                        HuffmanSymbol = DecodingTable[Next15Bits]
	                        HuffmanSymbolBitLength = the bit length of HuffmanSymbol, from the table in the input buffer
	                        If HuffmanSymbol <= 0
	                            NextBits <<= HuffmanSymbolBitLength
	                            ExtraBits -= HuffmanSymbolBitLength
	
                                Do
                                    HuffmanSymbol = - HuffmanSymbol
                                    HuffmanSymbol += (NextBits >> 31)
                                    NextBits *= 2
                                    ExtraBits = ExtraBits - 1
                                    HuffmanSymbol = DecodingTable[HuffmanSymbol]
                                While HuffmanSymbol <= 0
                            Else
                                DecodedBitCount = HuffmanSymbol & 15
                                NextBits <<= DecodedBitCount
                                ExtraBits -= DedcodedBitCount

	                        HuffmanSymbol >>= 4 // Shift by 4 bits to get the symbol value
	                                            // (the lower 4 bits are the bit length of the symbol)
	                        HuffmanSymbol -= 256
	                        If ExtraBits < 0
	                            NextBits |= Read16Bits(InputBuffer + CurrentPosition) << (-ExtraBits)
	                            ExtraBits += 16
	                            CurrentPosition += 2
	                        If HuffmanSymbol >= 0
	                            If HuffmanSymbol == 0
	                                If the entire input buffer has been read and
	                                the expected decompressed size has been written to the output buffer
	                                    Decompression is complete.  Return with success.
	                            Terminate literal processing
	                        Else
	                            Output the byte value of HuffmanSymbol to the output stream
	                    End of literal processing Loop
	   
	                    MatchLength = HuffmanSymbol mod 16
	                    MatchOffsetBitLength = HuffmanSymbol / 16
	                    If MatchLength == 15
	                        MatchLength = ReadByte(InputBuffer + CurrentPosition)
	                        CurrentPosition += 1
	                        If MatchLength == 255
	                            MatchLength = Read16Bits(InputBuffer + CurrentPosition)
	                            CurrentPosition += 2
	                            If MatchLength < 15
	                                The compressed data is invalid. Return error.
	                            MatchLength = MatchLength - 15
	                        MatchLength = MatchLength + 15
	                    MatchLength = MatchLength + 3
	                    MatchOffset = NextBits >> (32 – MatchOffsetBitLength)
	                    MatchOffset += (1 << MatchOffsetBitLength)
	                    NextBits <<= MatchOffsetBitLength
	                    ExtraBits -= MatchOffsetBitLength
	                    If ExtraBits < 0
	                        NextBits |= Read16Bits(InputBuffer + CurrentPosition) << (-ExtraBits)
	                        ExtraBits += 16
	                        CurrentPosition += 2
	                    For i = 0 to MatchLength - 1
	                        Output OutputBuffer[CurrentOutputPosition – MatchOffset + i]
	                 End of block loop
	            End of decoding loop
             */

            while (CurrentPosition < data.Length)
            {
                var header = new HuffmanHeader(data, CurrentPosition);

                header.buildDecodingTable();

                CurrentPosition += 256;

                uint NextBits = Read16Bits(data, CurrentPosition);

                CurrentPosition += 2;

                NextBits <<= 16;

                NextBits |= Read16Bits(data, CurrentPosition);

                CurrentPosition += 2;

                int ExtraBits = 16;

                int BlockEnd = result.Count + 65536;

                while (result.Count < BlockEnd)
                {
                    uint Next15Bits = NextBits >> (32 - 15);

                    int HuffmanSymbol = header.DecodingTable[Next15Bits];

                    int HuffmanSymbolBitLength = header.len[HuffmanSymbol];

                    NextBits <<= HuffmanSymbolBitLength;

                    ExtraBits -= HuffmanSymbolBitLength;

                    if (ExtraBits < 0)
                    {
                        NextBits |= Read16Bits(data, CurrentPosition) << (-ExtraBits);


                        ExtraBits += 16;

                        CurrentPosition += 2;
                    }

                    if (HuffmanSymbol < 256)
                    {
                        result.Add((byte)HuffmanSymbol);
                    }
                    else if (HuffmanSymbol == 256 && CurrentPosition == data.Length)
                    {
                        break;
                    }
                    else
                    {
                        HuffmanSymbol -= 256;
                        uint MatchLength = (uint)(HuffmanSymbol % 16);
                        int MatchOffsetBitLength = HuffmanSymbol / 16;
                        if (MatchLength == 15)
                        {
                            MatchLength = ReadByte(data, CurrentPosition);
                            CurrentPosition += 1;
                            if (MatchLength == 255)
                            {
                                MatchLength = Read16Bits(data, CurrentPosition);
                                CurrentPosition += 2;
                                if (MatchLength < 15)
                                {
                                    throw new XcaException("[Data error]: MatchLength should not be less than 15!");
                                }
                                MatchLength -= 15;
                            }
                            MatchLength += 15;
                        }

                        MatchLength += 3;

                        int MatchOffset = (int)((ulong)NextBits >> (32 - MatchOffsetBitLength));

                        MatchOffset += (1 << MatchOffsetBitLength);

                        NextBits <<= MatchOffsetBitLength;

                        ExtraBits -= MatchOffsetBitLength;


                        if (ExtraBits < 0)
                        {
                            NextBits |= Read16Bits(data, CurrentPosition) << (-ExtraBits);

                            ExtraBits += 16;

                            CurrentPosition += 2;
                        }

                        var location = result.Count;
                        for (int i = 0; i < MatchLength; i++)
                        {
                            int matchPosition = location - MatchOffset + i;
                            if (matchPosition < 0 || matchPosition >= result.Count)
                            {
                                throw new XcaException("[Data error]: Match offset out of buffer!");
                            }
                            byte matchLiteral = result[matchPosition];
                            result.Add(matchLiteral);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        static byte ReadByte(byte[] InputBuffer, int CurrentPosition)
        {
            byte result = InputBuffer[CurrentPosition + 0];
            return result;
        }

        static uint Read16Bits(byte[] InputBuffer, int CurrentPosition)
        {
            uint result = (uint)((InputBuffer[CurrentPosition + 0] << 0) + (InputBuffer[CurrentPosition + 1] << 8));
            return result;
        }
    }


    class HuffmanHeader
    {
        public int[] len;
        public int[] DecodingTable;

        public HuffmanHeader(byte[] input, int offset)
        {
            len = new int[512];

            for (int i = 0; i < 256; i++)
            {
                len[i * 2 + 0] = (input[offset + i] & 0x0F) >> 0;
                len[i * 2 + 1] = (input[offset + i] & 0xF0) >> 4;
            }
        }

        public void buildDecodingTable()
        {
            /*
                CurrentTableEntry = 0
                For BitLength = 1 to 15
                    For Symbol = 0 to 511
                        If the encoded bit length of Symbol equals BitLength
                            EntryCount = (1 << (15 – BitLength))
                            Repeat EntryCount times
                                If CurrentTableEntry >= 2^15
                                    The compressed data is not valid. Return with error.
                                DecodingTable[CurrentTableEntry] = Symbol
                                CurrentTableEntry = CurrentTableEntry + 1
                If CurrentTableEntry does not equal 2^15
                    The compressed data is not valid. Return with error.
             */
            DecodingTable = new int[1 << 15];
            int CurrentTableEntry = 0;
            for (int BitLength = 1; BitLength <= 15; BitLength++)
            {
                for (int Symbol = 0; Symbol <= 511; Symbol++)
                {
                    if (len[Symbol] == BitLength)
                    {
                        int EntryCount = 1 << (15 - BitLength);
                        for (int i = 0; i < EntryCount; i++)
                        {
                            if (CurrentTableEntry >= 1 << 15)
                            {
                                throw new XcaException("[Data error]: Too many symbols!");
                            }
                            DecodingTable[CurrentTableEntry] = Symbol;
                            CurrentTableEntry++;
                        }
                    }
                }
            }

            if (CurrentTableEntry != 1 << 15)
            {
                throw new XcaException("[Data error]: Symbol number is inconsistent!");
            }
        }
    }
}
