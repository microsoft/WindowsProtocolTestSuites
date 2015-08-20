// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc
{

    /// <summary>
    /// a decompressor for mppc
    /// </summary>
    public class Decompressor : IDisposable
    {
        private enum DecodeState
        {
            Init,
            DecodingLiteral,
            DecodingOffset,
            DecodingLength,
        }

        private SlidingWindow historyBuffer;
        private bool disposed;


        // the data will firstly be written to remain, then decode the remain.
        // this is because data is encoded to variable length bit sequence
        private uint remain;

        private int remainBitsCount;
        private uint literal;
        private uint offset;
        private uint length;
        private DecodeState decodeState;
        private bool needMoreData;
        private SlidingWindowSize mode;
        private Stream outputStream = new MemoryStream();

        //the sliding window size for 8k
        private const int window8k = 8 * 1024;

        //the sliding window size for 64k
        private const int window64k = 64 * 1024;

        //the bit count of on byte
        private const int oneByteBitsCount = 8;

        //the minimum length of data which will be encoded as (offset, length)
        private const int minimumEncodeLength = 3;


        /// <summary>
        /// the compress mode
        /// </summary>
        public SlidingWindowSize Mode
        {
            get 
            { 
                return mode;
            }
        }


        /// <summary>
        /// constructor with mode specified
        /// </summary>
        /// <param name="mode">indicates which mode are used</param>
        public Decompressor(SlidingWindowSize mode)
        {
            this.mode = mode;
            if (mode == SlidingWindowSize.EightKB)
            {
                historyBuffer = new SlidingWindow(window8k);
            }
            else if (mode == SlidingWindowSize.SixtyFourKB)
            {
                historyBuffer = new SlidingWindow(window64k);
            }
            else
            {
                throw new ArgumentException(
                    "mode should be EightKB or SixtyFourKB", "mode");
            }
            decodeState = DecodeState.Init;
        }


        /// <summary>
        /// Decompress input data
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="compressMode">the compress mode</param>
        /// <returns>decompressed data</returns>
        public byte[] Decompress(byte[] input, CompressMode compressMode)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            if (((compressMode & ~CompressMode.Flush) != 0) && ((compressMode & CompressMode.Flush) != 0))
            {
                throw new ArgumentException("invalid flag. CompressMode.Flush can't be mixed with others");
            }

            if ((compressMode & CompressMode.Flush) == CompressMode.Flush 
                || (compressMode & CompressMode.SetToFront) == CompressMode.SetToFront)
            {
                InitializeAll();
            }

            if ((compressMode & CompressMode.Compressed) != CompressMode.Compressed)
            {
                return (byte[])input.Clone();
            }

            DecompressCore(input);
            DecompressFinnal();
            byte[] ret = new byte[outputStream.Length];
            outputStream.Position = 0;
            outputStream.Read(ret, 0, ret.Length);
            remain = 0;
            remainBitsCount = 0;

            //set stream length to 0 for next round compress
            outputStream.SetLength(0);

            return ret;
        }


        /// <summary>
        /// decompress the input compressed data
        /// </summary>
        /// <param name="input">the input data</param>
        private void DecompressCore(byte[] input)
        {
            for (int i = 0; i < input.Length; )
            {
                uint temp = input[i];

                if ((remainBitsCount < oneByteBitsCount) || needMoreData)
                {
                    remain = remain << oneByteBitsCount | temp;
                    remainBitsCount += oneByteBitsCount;
                    i++;
                    needMoreData = false;
                }
                else
                {
                    DecodeRemain();
                }
            }
        }


        /// <summary>
        /// final decompress. decode the data in remain
        /// </summary>
        private void DecompressFinnal()
        {
            while ((remainBitsCount >= oneByteBitsCount) || (decodeState != DecodeState.Init))
            {
                DecodeRemain();
            }

            outputStream.Flush();
        }


        /// <summary>
        /// decode literal or (offset, length) from remain
        /// </summary>
        private void DecodeRemain()
        {
            if (decodeState == DecodeState.Init)
            {
                // 0x03 in Binary form is 011, it is the flag
                // to differ offset and literal
                if ((remain >> (remainBitsCount - 2)) == 0x3)
                {
                    decodeState = DecodeState.DecodingOffset;
                }
                else
                {
                    decodeState = DecodeState.DecodingLiteral;
                }
            }
            else if (decodeState == DecodeState.DecodingLength)
            {
                DecodeLength();
            }
            else if (decodeState == DecodeState.DecodingOffset)
            {
                DecodeOffset();
            }
            else if (decodeState == DecodeState.DecodingLiteral)
            {
                DecodeLiteral();
            }
            else
            {
                //never go into this path
            }
        }


        /// <summary>
        /// Decode length from remain
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private void DecodeLength()
        {
            uint lengthFlag = 0;
            if (remainBitsCount > oneByteBitsCount)
            {
                lengthFlag = remain >> (remainBitsCount - oneByteBitsCount);
            }
            else
            {
                lengthFlag = remain << (oneByteBitsCount - remainBitsCount);
            }

            //11111111 can be use to differ lengh > 512 or <= 512
            if (lengthFlag == 0xff)
            {
                //length value from 512 to 65536
                if (remainBitsCount > (2*oneByteBitsCount))
                {
                    needMoreData = true;
                    if ((remain >> (remainBitsCount - (int)LengthSymbolBitSize.Between512And1023)) 
                        == (uint)LengthSymbol.Between512And1023)
                    {
                        //512-1023
                        if (remainBitsCount >= (int)LengthBitSize.Between512And1023)
                        {
                            GetLengthFromRemain(LengthBitSize.Between512And1023);
                            needMoreData = false;
                        }
                    }
                    else if ((remain >> (remainBitsCount - (int)LengthSymbolBitSize.Between1024And2047))
                        == (uint)LengthSymbol.Between1024And2047)
                    {
                        //1024-2047
                        if (remainBitsCount >= (int)LengthBitSize.Between1024And2047)
                        {
                            GetLengthFromRemain(LengthBitSize.Between1024And2047);
                            needMoreData = false;
                        }

                    }
                    else if ((remain >> (remainBitsCount - (int)LengthSymbolBitSize.Between2048And4095))
                        == (uint)LengthSymbol.Between2048And4095)
                    {
                        //2048-4095
                        if (remainBitsCount >= (int)LengthBitSize.Between2048And4095)
                        {
                            GetLengthFromRemain(LengthBitSize.Between2048And4095);
                            needMoreData = false;
                        }

                    }
                    else if ((remain >> (remainBitsCount - (int)LengthSymbolBitSize.Between4096And8191))
                        == (uint)LengthSymbol.Between4096And8191)
                    {
                        //4096-8191
                        if (remainBitsCount >= (int)LengthBitSize.Between4096And8191)
                        {
                            GetLengthFromRemain(LengthBitSize.Between4096And8191);
                            needMoreData = false;
                        }

                    }
                    else if ((remain >> (remainBitsCount - (int)LengthSymbolBitSize.Between8192And16383))
                        == (uint)LengthSymbol.Between8192And16383)
                    {
                        //8192-16383
                        if (remainBitsCount >= (int)LengthBitSize.Between8192And16383)
                        {
                            GetLengthFromRemain(LengthBitSize.Between8192And16383);
                            needMoreData = false;
                        }
                    }
                    else if ((remain >> (remainBitsCount - (int)LengthSymbolBitSize.Between16384And32767))
                        == (uint)LengthSymbol.Between16384And32767)
                    {
                        //16384-32767
                        if (remainBitsCount >= (int)LengthBitSize.Between16384And32767)
                        {
                            GetLengthFromRemain(LengthBitSize.Between16384And32767);
                            needMoreData = false;
                        }
                    }
                    else if ((remain >> (remainBitsCount - (int)LengthSymbolBitSize.Between32768And65535))
                        == (uint)LengthSymbol.Between32768And65535)
                    {
                        //32767-65535
                        if (remainBitsCount >= (int)LengthBitSize.Between32768And65535)
                        {
                            GetLengthFromRemain(LengthBitSize.Between32768And65535);
                            needMoreData = false;
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("The compressed data format is not right");
                    }
                }
                else
                {
                    needMoreData = true;
                }
            }
            else
            {
                //length value from 3 to 512
                if ((lengthFlag >> (oneByteBitsCount - (int)LengthSymbolBitSize.Between0And3))
                    == (uint)LengthSymbol.Between0And3)
                {
                    //3
                    length = minimumEncodeLength;
                    remainBitsCount -= (int)LengthSymbolBitSize.Between0And3;
                    //elimate the decoded bit from remain
                    remain = remain & (uint)((1 << remainBitsCount) - 1);

                }
                else if ((lengthFlag >> (oneByteBitsCount - (int)LengthSymbolBitSize.Between4And7)) 
                    == (uint)LengthSymbol.Between4And7)
                {
                    //4-7
                    GetLengthFromRemain(LengthBitSize.Between4And7);
                }
                else if ((lengthFlag >> (oneByteBitsCount - (int)LengthSymbolBitSize.Between8And15)) 
                    == (uint)LengthSymbol.Between8And15)
                {
                    //8-15
                    GetLengthFromRemain(LengthBitSize.Between8And15);
                }
                else if ((lengthFlag >> (oneByteBitsCount - (int)LengthSymbolBitSize.Between16And31)) 
                    == (uint)LengthSymbol.Between16And31)
                {
                    //16-31
                    GetLengthFromRemain(LengthBitSize.Between16And31);
                }
                else
                {
                    needMoreData = true;
                    //32-511
                    if ((lengthFlag >> (oneByteBitsCount - (int)LengthSymbolBitSize.Between32And63))
                        == (uint)LengthSymbol.Between32And63)
                    {
                        //32-63
                        if (remainBitsCount >= (int)LengthBitSize.Between32And63)
                        {
                            GetLengthFromRemain(LengthBitSize.Between32And63);
                            needMoreData = false;
                        }
                    }
                    else if ((lengthFlag >> (oneByteBitsCount - (int)LengthSymbolBitSize.Between64And127))
                        == (uint)LengthSymbol.Between64And127)
                    {
                        //64-127
                        if (remainBitsCount >= (int)LengthBitSize.Between64And127)
                        {
                            GetLengthFromRemain(LengthBitSize.Between64And127);
                            needMoreData = false;
                        }
                    }
                    else if ((lengthFlag >> (oneByteBitsCount - (int)LengthSymbolBitSize.Between128And255))
                        == (uint)LengthSymbol.Between128And255)
                    {
                        //128-255
                        if (remainBitsCount >= (int)LengthBitSize.Between128And255)
                        {
                            GetLengthFromRemain(LengthBitSize.Between128And255);
                            needMoreData = false;
                        }
                    }
                    else if (lengthFlag == (uint)LengthSymbol.Between256And511)
                    {
                        //256-511
                        if (remainBitsCount >= (int)LengthBitSize.Between256And511)
                        {
                            GetLengthFromRemain(LengthBitSize.Between256And511);
                            needMoreData = false;
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("The compressed data format is not right");
                    }
                }
            }

            // if length has been processed, we can get the refferd data
            if (!needMoreData)
            {
                byte[] refferedData = historyBuffer.GetMatchedData(offset, length);
                outputStream.Write(refferedData, 0, refferedData.Length);
                historyBuffer.Update(refferedData);
                decodeState = DecodeState.Init;
            }
        }


        /// <summary>
        /// Decode offset
        /// </summary>
        private void DecodeOffset()
        {
            if (mode == SlidingWindowSize.EightKB)
            {
                DecodeOffset8K();
            }
            else if (mode == SlidingWindowSize.SixtyFourKB)
            {
                DecodeOffset64K();
            }
            else
            {
                //never go into this path
            }
        }


        /// <summary>
        /// Decode offset when sliding window is 64k
        /// </summary>
        private void DecodeOffset64K()
        {
            needMoreData = true;
            if ((remain >> (remainBitsCount - (int)OffsetSymbolBitSize64k.LessThan320))
                == (uint)OffsetSymbol64k.LessThan64)
            {
                //<64
                if (remainBitsCount >= (int)OffsetBitSize64k.LessThan64)
                {
                    GetOffsetFromRemain64K(OffsetBitSize64k.LessThan64, OffsetBitSizeWithoutSymbol64k.LessThan64);
                    needMoreData = false;
                }

            }
            else if ((remain >> (remainBitsCount - (int)OffsetSymbolBitSize64k.LessThan320))
                == (uint)OffsetSymbol64k.Between64And319)
            {
                // 64-320
                if (remainBitsCount >= (int)OffsetBitSize64k.Between64And319)
                {
                    GetOffsetFromRemain64K(OffsetBitSize64k.Between64And319,
                        OffsetBitSizeWithoutSymbol64k.Between64And319);
                    needMoreData = false;
                }
            }
            else if ((remain >> (remainBitsCount - (int)OffsetSymbolBitSize64k.Between320And2367)) 
                == (uint)OffsetSymbol64k.Between320And2367)
            {
                // 320-2368
                if (remainBitsCount >= (int)OffsetBitSize64k.Between320And2367)
                {
                    GetOffsetFromRemain64K(OffsetBitSize64k.Between320And2367, 
                        OffsetBitSizeWithoutSymbol64k.Between320And2367);
                    needMoreData = false;
                }
            }
            else if ((remain >> (remainBitsCount - (int)OffsetSymbolBitSize64k.LargerThan2367))
                == (uint)OffsetSymbol64k.LargerThan2368)
            {
                // >2367
                if (remainBitsCount >= (int)OffsetBitSize64k.LargerThan2367)
                {
                    GetOffsetFromRemain64K(OffsetBitSize64k.LargerThan2367, 
                        OffsetBitSizeWithoutSymbol64k.LargerThan2368);
                    needMoreData = false;
                }
            }
            else
            {
                throw new InvalidDataException("The compressed data format is not right");
            }

            if (!needMoreData)
            {
                decodeState = DecodeState.DecodingLength;
            }
        }


        /// <summary>
        /// Decode offset when sliding window is 8k
        /// </summary>
        private void DecodeOffset8K()
        {
            needMoreData = true;
            if ((remain >> (remainBitsCount - (int)OffsetSymbolBitSize8k.LessThan320))
                == (uint)OffsetSymbol8k.LessThan64)
            {
                // <64
                if (remainBitsCount >= (uint)OffsetBitSize8k.LessThan64)
                {

                    GetOffsetFromRemain8K(OffsetBitSize8k.LessThan64,
                        OffsetBitSizeWithoutSymbol8k.LessThan64);
                    needMoreData = false;
                }
            }
            else if ((remain >> (remainBitsCount - (int)OffsetSymbolBitSize8k.LessThan320)) 
                == (uint)OffsetSymbol8k.Between64And320)
            {
                //64 - 320
                if (remainBitsCount >= (uint)OffsetBitSize8k.Between64And320)
                {
                    GetOffsetFromRemain8K(OffsetBitSize8k.Between64And320,
                        OffsetBitSizeWithoutSymbol8k.Between64And320);
                    needMoreData = false;
                }
            }
            else if ((remain >> (remainBitsCount - (int)OffsetSymbolBitSize8k.Between320And8191))
                == (uint)OffsetSymbol8k.Between320And8191)
            {
                //320 - 8191
                if (remainBitsCount >= (uint)OffsetBitSize8k.Between320And8191)
                {
                    GetOffsetFromRemain8K(OffsetBitSize8k.Between320And8191,
                        OffsetBitSizeWithoutSymbol8k.Between320And8191);
                    needMoreData = false;
                }
            }
            else
            {
                throw new InvalidDataException("The compressed data format is not right");
            }

            if (!needMoreData)
            {
                decodeState = DecodeState.DecodingLength;
            }
        }


        /// <summary>
        /// Decode Literal from remain
        /// </summary>
        private void DecodeLiteral()
        {
            needMoreData = true;
            if ((remain >> (remainBitsCount - (int)LiteralSymbolBitSize.MoreThanHex7f)) 
                == (uint)LiteralSymbol.MoreThanHex7f)
            {
                //>0x7F
                if (remainBitsCount >= (int)LiteralBitSize.MoreThanHex7f)
                {
                    GetLiteralFromRemain(LiteralBitSize.MoreThanHex7f);
                    needMoreData = false;
                }
            }
            else
            {
                //<0x80
                GetLiteralFromRemain(LiteralBitSize.LessThanHex80);
                needMoreData = false;
            }

            if (!needMoreData)
            {
                outputStream.WriteByte((byte)literal);
                historyBuffer.Update((byte)literal);
                decodeState = DecodeState.Init;
            }
        }


        /// <summary>
        /// get length info from remain
        /// </summary>
        /// <param name="lengthBitCount"></param>
        private void GetLengthFromRemain(LengthBitSize lengthBitCount)
        {
            //remian bit count will reduce by lengthBitCount.
            remainBitsCount -= (int)lengthBitCount;
            //the mask to get the data part of the length
            uint mask = (uint)((1 << ((int)lengthBitCount / 2)) - 1);
            //add flag part and the data part of length.
            length = ((remain >> remainBitsCount) & mask) + (mask + 1);
            //caculate the remain after getting the length
            remain = remain & (uint)((1 << remainBitsCount) - 1);
        }


        /// <summary>
        /// Get the offset when compress mode is 8k
        /// </summary>
        /// <param name="offsetBitCount">the bits count offset used</param>
        /// <param name="actualOffsetBitCount">the bits count offset used without flag</param>
        private void GetOffsetFromRemain8K(
            OffsetBitSize8k offsetBitCount,
            OffsetBitSizeWithoutSymbol8k actualOffsetBitCount)
        {
            uint adding = 0;
            // determin the adding value according to offset flag
            if (actualOffsetBitCount == OffsetBitSizeWithoutSymbol8k.Between64And320)
            {
                adding = (uint)OffsetAdding8k.Between64And320;
            }
            else if (actualOffsetBitCount == OffsetBitSizeWithoutSymbol8k.Between320And8191)
            {
                adding = (uint)OffsetAdding8k.Between320And8191;
            }
            else
            {
                //adding = 0;
            }

            //get offset from high bits part of remain.
            //this bits are pushed to remain earlier than others
            offset = ((remain >> (remainBitsCount - (int)offsetBitCount)) & (uint)((1 << (int)actualOffsetBitCount) - 1)) + adding;
            remainBitsCount -= (int)offsetBitCount;
            // elimate the decoded bits
            remain = remain & (uint)((1 << remainBitsCount) - 1);
        }


        /// <summary>
        /// Get the offset when compress mode is 64k
        /// </summary>
        /// <param name="offsetBitCount">the bits count offset used</param>
        /// <param name="actualOffsetBitCount">the bits count offset used without flag</param>
        private void GetOffsetFromRemain64K(
            OffsetBitSize64k offsetBitCount, 
            OffsetBitSizeWithoutSymbol64k actualOffsetBitCount)
        {
            uint adding = 0;
            if (actualOffsetBitCount == OffsetBitSizeWithoutSymbol64k.Between64And319)
            {
                adding = (uint)OffsetAdding64k.Between64And319;
            }
            else if (actualOffsetBitCount == OffsetBitSizeWithoutSymbol64k.Between320And2367)
            {
                adding = (uint)OffsetAdding64k.Between320And2367;
            }
            else if (actualOffsetBitCount == OffsetBitSizeWithoutSymbol64k.LargerThan2368)
            {
                adding = (uint)OffsetAdding64k.LargerThan2368;
            }
            else
            {
                // adding = 0;
            }

            //get offset from high bits part of remain.
            //this bits are pushed to remain earlier than others
            offset = ((remain >> (remainBitsCount - (int)offsetBitCount)) 
                & (uint)((1 << (int)actualOffsetBitCount) - 1)) + adding;
            remainBitsCount -= (int)offsetBitCount;
            //elimate the decoded bits
            remain = remain & (uint)((1 << remainBitsCount) - 1);
        }


        /// <summary>
        /// Get Literal
        /// </summary>
        /// <param name="literalBitCount">the bits count literal used</param>
        private void GetLiteralFromRemain(LiteralBitSize literalBitCount)
        {
            uint adding = 0;
            if (literalBitCount == LiteralBitSize.MoreThanHex7f)
            {
                adding = (uint)LiteralAdding.MoreThanHex7f;
            }
            remainBitsCount -= (int)literalBitCount;
            literal = ((remain >> remainBitsCount) & 0xff) + adding;
            //elimate the decoded bits
            remain = remain & (uint)((1 << remainBitsCount) - 1);
        }


        /// <summary>
        /// Initialize all field of this class instance
        /// </summary>
        private void InitializeAll()
        {
            historyBuffer.Clear();
            decodeState = DecodeState.Init;
            needMoreData = false;
            remain = 0;
            remainBitsCount = 0;
            offset = 0;
            length = 0;
            literal = 0;
        }


        /// <summary>
        /// release all resource
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //Take this object out of the finalization queue of the GC
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// release resource
        /// </summary>
        /// <param name="disposing">Indicates GC or user calling this function</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types
                    outputStream.Dispose();
                }
                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed.
                disposed = true;
            }
        }


        /// <summary>
        /// Deconstructor
        /// </summary>
        ~Decompressor()
        {
            Dispose(false);
        }
    }
}
