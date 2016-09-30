// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc
{
    /// <summary>
    /// Mppc Compressor, has two sliding windows size: 8k or 64k
    /// </summary>
    public class Compressor : IDisposable
    {
        private class OffsetLengthPair
        {
            private int offset;
            private int length;


            public int Offset
            {
                get 
                { 
                    return offset; 
                }
                set
                {
                    offset = value;
                }
            }


            public int Length
            {
                get 
                { 
                    return length;
                }
                set 
                { 
                    length = value;
                }
            }
        }

        /// <summary>
        /// the remain data needed to be written to outputstream
        /// </summary>
        private uint remain;

        /// <summary>
        /// the bit count of valid data in remain
        /// </summary>
        private int remainBitsCount;

        private SlidingWindow slidingWindow;

        /// <summary>
        /// to store hash value for the data in sliding window
        /// </summary>
        private HashTable hashTable;

        private Stream outputStream = new MemoryStream();
        private SlidingWindowSize mode;

        //the sliding window size for 8k
        private const int window8k = 8 * 1024;

        //the sliding window size for 64k
        private const int window64k = 64 * 1024;

        //the bit count of one byte
        private const int oneByteBitsCount = 8;

        //the minimum length of data which will be encoded as (offset, length)
        private const int minimumEncodeLength = 3;

        private bool disposed;

        /// <summary>
        /// constructor with specified sliding window size 
        /// </summary>
        /// <param name="mode">the mode of compressor, specify the size of sliding window</param>
        public Compressor(SlidingWindowSize mode)
        {
            this.mode = mode;
            if (mode == SlidingWindowSize.EightKB)
            {
                slidingWindow = new SlidingWindow(window8k);
            }
            else if (mode == SlidingWindowSize.SixtyFourKB)
            {
                slidingWindow = new SlidingWindow(window64k);
            }
            else
            {
                throw new ArgumentException(
                    "mode should be EightKB or SixtyFourKB", "mode");
            }
            hashTable = new HashTable(slidingWindow);
        }


        /// <summary>
        /// compress the input data
        /// </summary>
        /// <param name="input">the input data</param>
        /// <param name="compressMode">the compress mode of this compress</param>
        /// <returns>compressed data, if the compressed data is larger than input data, the original data
        /// is returned</returns>
        public byte[] Compress(byte[] input, out CompressMode compressMode)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            compressMode = CompressMode.Compressed;
            if (input.Length > (window8k - slidingWindow.Count))
            {
                compressMode = compressMode | CompressMode.SetToFront;
                hashTable.Clear();
                slidingWindow.Clear();
            }

            CompressCore(input);

            byte[] ret = new byte[outputStream.Length];
            outputStream.Position = 0;
            outputStream.Read(ret, 0, ret.Length);

            //put stream position to 0 for next round compress
            outputStream.SetLength(0);

            if (ret.Length > input.Length)
            {
                compressMode = CompressMode.Flush;
                hashTable.Clear();
                slidingWindow.Clear();
                return input;
            }

            return ret;
        }


        /// <summary>
        /// Compress the input buffer and write it into output stream.
        /// </summary>
        /// <param name="input">The buffer needed to be compressed</param>
        private void CompressCore(byte[] input, int maxBit =16)
        {
            int compressIndex = 0;
            int lookAheadBytesCount = 0;

            //we don't need to find match for the last two bytes, because it 
            //alway can't be found.
            while (compressIndex < (input.Length - (minimumEncodeLength - 1)))
            {
                OffsetLengthPair match = FindMatchInSlidingWindow(input, compressIndex);
                if (match.Length == 0)
                {
                    //if there is no match, move forward one byte
                    lookAheadBytesCount = 1;
                    EncodeLiteral(input[compressIndex]);
                }
                else
                {
                    lookAheadBytesCount = match.Length;
                    EncodeOffsetLengthPair(match, maxBit);
                }

                byte[] lookAheadBuffer = new byte[lookAheadBytesCount];
                Array.Copy(input, compressIndex, lookAheadBuffer, 0, lookAheadBuffer.Length);
                hashTable.Update(lookAheadBuffer);
                slidingWindow.Update(lookAheadBuffer);

                compressIndex += lookAheadBytesCount;               
            }

            // upate hash table and sliding window
            byte[] toBeUptated = new byte[input.Length - compressIndex];
            Array.Copy(input, compressIndex, toBeUptated, 0, toBeUptated.Length);
            hashTable.Update(toBeUptated);
            slidingWindow.Update(toBeUptated);

            while (compressIndex < input.Length)
            {
                EncodeLiteral(input[compressIndex]);
                compressIndex++;
            }

            //write the last byte to output stream if it exists
            if (remainBitsCount > 0)
            {
                outputStream.WriteByte((byte)(remain << (oneByteBitsCount - remainBitsCount)));
                remainBitsCount = 0;
            }

            outputStream.Flush();
        }


        /// <summary>
        /// find a match in sliding window
        /// </summary>
        /// <param name="input">the input data</param>
        /// <param name="startIndex">the start position of input data,
        /// we will start finding from that position</param>
        /// <returns>the match found</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private OffsetLengthPair FindMatchInSlidingWindow(byte[] input, int startIndex)
        {
            OffsetLengthPair match = new OffsetLengthPair();

            if ((input.Length - startIndex) < minimumEncodeLength)
            {
                return match;
            }

            //create minimumEncodeLength bytes key
            byte[] threeBytesHashKey = new byte[minimumEncodeLength];
            Array.Copy(input, startIndex, threeBytesHashKey, 0, threeBytesHashKey.Length);
            int[] findedPositions = hashTable.GetKeyMatchPositions(threeBytesHashKey);

            //because hash table does not contain the hash value of last two characters
            //so we need to caculate it after getting hash value from hash table.
            List<int> matchPositions = new List<int>(findedPositions);

            // indicate the match position
            int matchedPosition = -1;
            bool isMatchFound = false;

            // if sliding windows count <=0, there is no match can be found, so do nothing
            if (slidingWindow.Count > 0)
            {
                // test if there is a match at the last position of sliding windows
                if (slidingWindow[slidingWindow.Count - 1] == input[startIndex] 
                    && input[startIndex] == input[startIndex + 1] 
                    && input[startIndex + 1] == input[startIndex + 2])
                {
                    // the position is the last byte in sliding window
                    matchedPosition  = slidingWindow.Count-1;
                    isMatchFound = true;
                }
                // test if there is a match at the second last position of sliding windows 
                if (slidingWindow.Count > 1)
                {
                    if (slidingWindow[slidingWindow.Count - 2] == input[startIndex]
                        && slidingWindow[slidingWindow.Count - 1] == input[startIndex + 1]
                        && input[startIndex] == input[startIndex + 2])
                    {
                        // the position is the second last byte in sliding window
                        matchPositions.Add(slidingWindow.Count - 2);
                        isMatchFound = true;
                    }
                }
            }

            //make sure the position will be added in ascending order.
            if (matchedPosition != -1)
            {
                matchPositions.Add(matchedPosition);
            }

            // translate list to array if match is found from the second last position of sliding window
            if (isMatchFound)
            {
                findedPositions = matchPositions.ToArray();
            }

            // if there is no match, return default match
            if (findedPositions == null || findedPositions.Length == 0)
            {
                return match;
            }
           
            //caculate the offset and length
            match.Offset = slidingWindow.Count - findedPositions[findedPositions.Length - 1];
            match.Length = minimumEncodeLength;

            for (int i = 0; i < findedPositions.Length; i++)
            {
                //skip already matched 3 characters
                int searchIndex = startIndex + minimumEncodeLength;

                int j;

                for (j = (findedPositions[i]+minimumEncodeLength); j < slidingWindow.Count; j++)
                {
                    // get the last unmatched position
                    if ((searchIndex == input.Length) 
                        || (slidingWindow[j] != input[searchIndex++]))
                    {
                        if (match.Length < (searchIndex - startIndex -1))
                        {
                            match.Length = searchIndex - startIndex -1;
                            match.Offset = slidingWindow.Count - findedPositions[i];
                        }
                        break;
                    }
                }
                
                // if length > offset 
                if (j >= slidingWindow.Count)
                {
                    int mark = startIndex;

                    // the last two character case
                    if (j > 0)
                    {
                        mark = startIndex + j - slidingWindow.Count;
                    }

                    //continue find match against the current data
                    while (searchIndex < input.Length)
                    {
                        if (input[mark++] != input[searchIndex++])
                        {
                            // because upper line use searchIndex++, so at this position
                            // we need to reduce it by 1;
                            if (match.Length < (searchIndex - startIndex - 1))
                            {
                                match.Length = searchIndex - startIndex - 1;
                                match.Offset = slidingWindow.Count - findedPositions[i];
                            }
                            break;
                        }
                    }

                    // if searchIndex == inputLength, the way to caculate offset and length 
                    // is different with the normal situation.
                    if (searchIndex == input.Length)
                    {
                        if (match.Length < searchIndex - startIndex)
                        {
                            match.Length = searchIndex - startIndex;
                            match.Offset = slidingWindow.Count - findedPositions[i];
                        }
                    }
                }
            }
            
            return match;
        }


        /// <summary>
        /// Encode literal according to RFC2118
        /// </summary>
        /// <param name="literal">the literal needed to encoded</param>
        private void EncodeLiteral(byte literal)
        {
            // If the value of the Literal is
            // below hex 0x80, it is encoded unchanged.
            if (literal < 0x80)
            {
                EncodeData(literal, (int)LiteralBitSize.LessThanHex80);
            }
            else
            {
                uint temp = (uint)(((int)LiteralSymbol.MoreThanHex7f 
                    << (int)LiteralBitSizeWithoutSymbol.MoreThanHex7f) 
                    + literal - LiteralAdding.MoreThanHex7f);
                EncodeData(temp, (int)LiteralBitSize.MoreThanHex7f);
            }
        }


        /// <summary>
        /// Encode offset and length according to RFC2118
        /// </summary>
        /// <param name="pair">offset and length pair</param>
        private void EncodeOffsetLengthPair(OffsetLengthPair pair, int maxBit = 16)
        {
            EncodeOffset(pair.Offset);
            EncodeLength(pair.Length, maxBit);
        }


        /// <summary>
        /// Encoding offset according to RFC2118
        /// </summary>
        /// <param name="offset">offset</param>
        private void EncodeOffset(int offset)
        {
            uint temp;
            if (mode == SlidingWindowSize.EightKB)
            {

                // Offset value less than 64 is encoded as bits 1111 followed by the
                // lower 6 bits of the value
                if (offset < 64)
                {
                    //offset < 64
                    temp = (uint)(((int)OffsetSymbol8k.LessThan64 
                        << (int)OffsetBitSizeWithoutSymbol8k.LessThan64) 
                        + offset - OffsetAdding8k.LessThan64);
                    EncodeData(temp, (int)OffsetBitSize8k.LessThan64);
                }
                else if (offset < 320)
                {
                    // Offset value between 64 and 320 is encoded as bits 1110 followed by
                    // the lower 8 bits of the computation (value - 64).
                    temp = (uint)(((int)OffsetSymbol8k.Between64And320
                        << (int)OffsetBitSizeWithoutSymbol8k.Between64And320)
                        + offset - OffsetAdding8k.Between64And320);
                    EncodeData(temp, (int)OffsetBitSize8k.Between64And320);
                }
                else if (offset <= 8192)
                {
                    // Offset value between 320 and 8191 is encoded as bits 110 followed
                    // by the lower 13 bits of the computation (value - 320).
                    temp = (uint)(((int)OffsetSymbol8k.Between320And8191 
                        << (int)OffsetBitSizeWithoutSymbol8k.Between320And8191)
                        + offset - OffsetAdding8k.Between320And8191);
                    EncodeData(temp, (int)OffsetBitSize8k.Between320And8191);
                }
                else
                {
                    // do nothing
                    // because offset is a reference to sliding window, so it won't be larger than
                    // sliding window's size. 
                }
            }
            else if (mode == SlidingWindowSize.SixtyFourKB)
            {
                if (offset < 64)
                {
                    //if offset < 64, encoded as 11111 + lower 6 bits of offset
                    temp = (uint)(((int)OffsetSymbol64k.LessThan64
                        <<(int)OffsetBitSizeWithoutSymbol64k.LessThan64)
                        + offset);
                    EncodeData(temp, (int)OffsetBitSize64k.LessThan64);
                }
                else if (offset < 320)
                {
                    //if offset < 320, encoded as 11110 + lower 8 bits of (offset – 64)
                    temp = (uint)(((int)OffsetSymbol64k.Between64And319 
                        << (int)OffsetBitSizeWithoutSymbol64k.Between64And319)
                        + offset - OffsetAdding64k.Between64And319);
                    EncodeData(temp, (int)OffsetBitSize64k.Between64And319);
                }
                else if (offset < 2368)
                {
                    //if offset < 320, encoded as 1110 + lower 11 bits of (offset – 320)
                    temp = (uint)(((int)OffsetSymbol64k.Between320And2367 
                        << (int)OffsetBitSizeWithoutSymbol64k.Between320And2367)
                        + offset - OffsetAdding64k.Between320And2367);
                    EncodeData(temp, (int)OffsetBitSize64k.Between320And2367);
                }
                else
                {
                    //else, encoded as 110 + lower 16 bits of (offset – 2368)
                    temp = (uint)(((int)OffsetSymbol64k.LargerThan2368 
                        << (int)OffsetBitSizeWithoutSymbol64k.LargerThan2368)
                        + offset - OffsetAdding64k.LargerThan2368);
                    EncodeData(temp, (int)OffsetBitSize64k.LargerThan2367);
                }
            }
        }


        /// <summary>
        /// encoding length according to RFC2118
        /// </summary>
        /// <param name="length">length</param>
        private void EncodeLength(int length, int maxBit = 16)
        {
            // the range of length is from 0 to 65535, the max bits it may use is 16
            int highestBitOnePosition = GetHighestBitOnePos(length, 16);
            // get the length bit size without symbol
            int bitCountWithoutSymbol = ((highestBitOnePosition < 3) ? 0 : (highestBitOnePosition - 1));
            int lengthBitCount = ((highestBitOnePosition < 3) ? 1 : (bitCountWithoutSymbol * 2));

            // the last bit of symbol is zero, and other bits are one.
            int lengthSymbol = ((1 << bitCountWithoutSymbol) - 2);

            // the symbol length should be caculated differently when length < 3 
            lengthSymbol = ((lengthSymbol > 0) ? lengthSymbol : 0);

            // length has the {1+0} form prefix.
            int temp = (lengthSymbol << bitCountWithoutSymbol) | (length & ((1 << bitCountWithoutSymbol) - 1));

            //write data to stream
            EncodeData((uint)temp, lengthBitCount);
        }


        /// <summary>
        /// Get the highest position of bit one
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataBitsCount">the bits count which data may use</param>
        /// <returns>the highest bit one position, start from 1</returns>
        private int GetHighestBitOnePos(int data, int dataBitsCount)
        {
            int highestBitOnePosition = 0;
            // get half bits count of maxBits, if there is remainder, add the remainder
            int maskBitsCount = dataBitsCount / 2 + dataBitsCount % 2;
            while (true)
            {
                // the mask form is like 11110000
                int mask = ((1 << maskBitsCount) - 1) << (dataBitsCount - maskBitsCount);
                if ((mask & data) != 0)
                {
                    // if high-bits part of data is not zero
                    highestBitOnePosition += (dataBitsCount - maskBitsCount);
                    data = data >> (dataBitsCount - maskBitsCount);
                }
                if (maskBitsCount == 1)
                {
                    // if the maskBits's length become 1, then we can get the highest bit position
                    // of data. it counts from 1.
                    return (highestBitOnePosition + 1);
                }
                dataBitsCount = maskBitsCount;
                // reduce maskbits' length by half, if there is remainder, add the remainder.
                maskBitsCount = dataBitsCount / 2 + dataBitsCount % 2;
            }
        }


        /// <summary>
        /// encode data of variable length
        /// </summary>
        /// <param name="data">the data</param>
        /// <param name="dataBitsCount">how many bits the data used</param>
        private void EncodeData(uint data, int dataBitsCount)
        {
            if (dataBitsCount <= 0)
            {
                throw new ArgumentException("Can't encode data whose bit length is less than or equal to 0",
                    "dataBitsCount");
            }

            byte temp = 0;

            // when the data's bit size is larger than 8 bits
            // we will encode the data every 8 bits
            while (dataBitsCount >= oneByteBitsCount)
            {
                remain = (remain << oneByteBitsCount) | (data >> (dataBitsCount - oneByteBitsCount));
                dataBitsCount = dataBitsCount - oneByteBitsCount;
                // data & the value whose form is like 00000111111
                data = data & (uint)((1 << dataBitsCount) - 1);
                temp = (byte)(remain >> remainBitsCount);
                outputStream.WriteByte(temp);
                // remain & the value whose form is like 00000111111
                // this action will eliminate the bits which has been written to
                // output stream
                remain = remain & (uint)((1 << remainBitsCount) - 1);
            }

            // if there is un-encoded data, we write it to 
            // the "remain".
            if (dataBitsCount > 0)
            {
                remain = (remain << dataBitsCount) | data;
                remainBitsCount += dataBitsCount;
            }

            // if the length of remain is larger than 8-bits after upper
            // operation, we will write it to ouputstream one by one.
            while (remainBitsCount >= oneByteBitsCount)
            {
                remainBitsCount -= oneByteBitsCount;
                temp = (byte)(remain >> (remainBitsCount));
                outputStream.WriteByte(temp);
                // this action will eliminate the bits which has been written to
                // output stream from remain
                remain = remain & (uint)((1 << remainBitsCount) - 1);
            }
        }


        /// <summary>
        /// release resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //Take this object out of the finalization queue of the GC
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// release resources
        /// </summary>
        /// <param name="disposing">indicate whether GC or user call this function</param>
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
        ~Compressor()
        {
            Dispose(false);
        }
    }
}
