// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{

    /// <summary>
    /// This class describes the huffman symbols or "tokens".
    /// </summary>
    internal class Token
    {
        public Token(int len, int code, int val, int type, int vbase, string prefix)
        {
            Len = len;
            BitPrefix = prefix;
            ValueBits = val;
            Code = code;
            Type = type;
            Vbase = vbase;
        }

        #region Private Variables
        /// <summary>
        /// The length of the bit prefix, corresponding to the bitPrefix field.
        /// </summary>
        private int len;
        /// <summary>
        /// The decimal value corresponding to the bitPrefix.
        /// </summary>
        private int code;
        /// <summary>
        /// Define the type of the token, currentl only have value "0" and "1".
        /// </summary>
        private int type;
        /// <summary>
        /// The prefix part of the token, uniquely distinguish a token.
        /// </summary>
        private string bitPrefix;
        /// <summary>
        /// The additional bits needed for get the indicated value of the token.
        /// </summary>
        private int valueBits;
        /// <summary>
        /// The base value for get the indicated value of the token.
        /// </summary>
        private int vbase;
        #endregion

        public int Len
        {
            get { return len; }
            set { len = value; }
        }

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public string BitPrefix
        {
            get { return bitPrefix; }
            set { bitPrefix = value; }
        }

        public int ValueBits
        {
            get { return valueBits; }
            set { valueBits = value; }
        }

        public int Vbase
        {
            get { return vbase; }
            set { vbase = value; }
        }

    }

    /// <summary>
    /// This class stores token tables and other configuration data
    /// used in the compression procedure.
    /// </summary>
    internal class Config
    {

        /// <summary>
        /// The table of all the huffman symbols, sorted by prefix length.
        /// </summary>
        public static Token[] tokenTableDis
            = {
                  new Token(1, 0, 8, 0, 0, "0"),              // 1   literal
                  new Token(5, 17, 5, 1, 0, "10001"),         // 2   1-31
                  new Token(5, 18, 7, 1, 32, "10010"),        // 3   32-159
                  new Token(5, 19, 9, 1, 160, "10011"),       // 4   160-671
                  new Token(5, 20, 10, 1, 672, "10100"),      // 5   672-1659
                  new Token(5, 21, 12, 1, 1696, "10101"),     // 6   1696-5791
                  new Token(5, 24, 0, 0, 0x00, "11000"),      // 7   
                  new Token(5, 25, 0, 0, 0x01, "11001"),      // 8   
                  new Token(6, 44, 14, 1, 5792, "101100"),    // 9   5792-22175
                  new Token(6, 45, 15, 1, 22176, "101101"),   // 10  22176-54943
                  new Token(6, 52, 0, 0, 0x02, "110100"),     // 11
                  new Token(6, 53, 0, 0, 0x03, "110101"),     // 12
                  new Token(6, 54, 0, 0, 0xFF, "110110"),     // 13
                  new Token(7, 92, 18, 1, 54944, "1011100"),  // 14   54944-317087
                  new Token(7, 93, 20, 1, 317088, "1011101"), // 15   317088-1365663
                  new Token(7, 110, 0, 0, 0x04, "1101110"),   // 16 
                  new Token(7, 111, 0, 0, 0x05, "1101111"),   // 17 
                  new Token(7, 112, 0, 0, 0x06, "1110000"),   // 18 
                  new Token(7, 113, 0, 0, 0x07, "1110001"),   // 19 
                  new Token(7, 114, 0, 0, 0x08, "1110010"),   // 20 
                  new Token(7, 115, 0, 0, 0x09, "1110011"),   // 21
                  new Token(7, 116, 0, 0, 0x0A, "1110100"),   // 22
                  new Token(7, 117, 0, 0, 0x0B, "1110101"),   // 23
                  new Token(7, 118, 0, 0, 0x3A, "1110110"),   // 24
                  new Token(7, 119, 0, 0, 0x3B, "1110111"),   // 25
                  new Token(7, 120, 0, 0, 0x3C, "1111000"),   // 26
                  new Token(7, 121, 0, 0, 0x3D, "1111001"),   // 27
                  new Token(7, 122, 0, 0, 0x3E, "1111010"),   // 28
                  new Token(7, 123, 0, 0, 0x3F, "1111011"),   // 29
                  new Token(7, 124, 0, 0, 0x40, "1111100"),   // 30
                  new Token(7, 125, 0, 0, 0x80, "1111101"),   // 31
                  new Token(8, 188, 20, 1, 1365664, "10111100"), // 32  1365664-2414239
                  new Token(8, 189, 21, 1, 2414240, "10111101"), // 33  2414240-4511391
                  new Token(8, 252, 0, 0, 0x0C, "11111100"),  // 34
                  new Token(8, 253, 0, 0, 0x38, "11111101"),  // 35
                  new Token(8, 254, 0, 0, 0x39, "11111110"),  // 36
                  new Token(8, 255, 0, 0, 0x66, "11111111"),  // 37
                  new Token(9, 380, 22, 1, 4511392, "101111100"), // 38  4511392-8705695
                  new Token(9, 381, 23, 1, 8705696, "101111101"), // 39  8705696-17094303
                  new Token(9, 382, 24, 1, 17094304, "101111111"), // 40 17094304-33871519
                  new Token(0, 0, 0, 0, 0, "")                // 41
              };


        /// <summary>
        /// The table of length token, a length token
        /// follows a match token.
        /// </summary>
        private static Token[] tokenTableLen
            = {
                  new Token(0, 0, 0, 0, 0, "0"),
                  new Token(0, 0, 2, 0, 4, "10"),
                  new Token(0, 0, 3, 0, 8,"110"),
                  new Token(0, 0, 4, 0, 16,"1110"),
                  new Token(0, 0, 5, 0, 32,"11110"),
                  new Token(0, 0, 6, 0, 64,"111110"),
                  new Token(0, 0, 7, 0, 128,"1111110"),
                  new Token(0, 0, 8, 0, 256,"11111110"),
                  new Token(0, 0, 9, 0, 512,"111111110"),
                  new Token(0, 0, 10, 0, 1024,"1111111110"),
                  new Token(0, 0, 11, 0, 2048,"11111111110"),
                  new Token(0, 0, 12, 0, 4096,"111111111110"),
                  new Token(0, 0, 13, 0, 8192,"1111111111110"),
                  new Token(0, 0, 14, 0, 16384,"11111111111110"),
                  new Token(0, 0, 15, 0, 32768,"111111111111110")
              };

        /// <summary>
        /// A static value indicates the length of the history buffer
        /// used in the compression procedure.
        /// </summary>
        private static int hitoryLen = 2500000;
        /// <summary>
        /// A static value indicates the maximum length of the repeat length.
        /// </summary>
        private static int maxLen = 10000;

        public static int HitoryLen
        {
            get { return hitoryLen; }
            set { hitoryLen = value; }
        }

        public static int MaxLen
        {
            get { return Config.maxLen; }
            set { Config.maxLen = value; }
        }

        /// <summary>
        /// Get a match token according to the match distance.
        /// </summary>
        /// <param name="distance">The distance here means the needed bytes backward.</param>
        public static Token getTokenDis(int distance)
        {
            if (distance < 32)
                return tokenTableDis[1];
            else if (distance < 160)
                return tokenTableDis[2];
            else if (distance < 672)
                return tokenTableDis[3];
            else if (distance < 1696)
                return tokenTableDis[4];
            else if (distance < 5792)
                return tokenTableDis[5];
            else if (distance < 22176)
                return tokenTableDis[8];
            else if (distance < 54944)
                return tokenTableDis[9];
            else
                return tokenTableDis[13];
        }

        /// <summary>
        /// Get a length token according to the match length.
        /// </summary>
        /// <param name="length">The length here means the number of bytes to be copied.</param>
        public static Token getTokenLen(int length)
        {
            int exp = 1;
            while (length >= Math.Pow(2, exp))
                ++exp;
            return tokenTableLen[exp - 2];
        }
    }

    /// <summary>
    /// This class offers the completion for the compression algorithm
    /// </summary>
    public class CompressFactory
    {
        #region Variables
        /// <summary>
        /// Raw data to be compressed.
        /// </summary>
        private byte[] rawData;
        /// <summary>
        /// Compressed data to be decompressed.
        /// </summary>
        private byte[] compressData;
        /// <summary>
        /// The length of the history buffer.
        /// </summary>
        private int MaxP;
        /// <summary>
        /// The maximum value of the repeat length.
        /// </summary>
        private int MaxL;
        /// <summary>
        /// The bit buffer used in the compression procedure.
        /// </summary>
        private List<bool> bitBuffer;
        /// <summary>
        /// An byte buffer to store the temp result from the bit buffer.
        /// </summary>
        private ArrayList compressDataList;

        /// <summary>
        /// Histoty byte buffer to store the MaxP length 
        /// decompressed result in the decompress procedure.
        /// </summary>
        private byte[] historyBuffer;
        private UInt32 historyIndex;
        /// <summary>
        /// The result byte buffer used in the decompress procedure.
        /// </summary>
        private List<byte> outputBuffer;
        /// <summary>
        /// The result byte array of the decompress algorithm.
        /// </summary>
        private byte[] recoverData;
        /// <summary>
        /// The remaining length of bits to be decompress from.s
        /// </summary>
        UInt32 bitsRemainingLen = 0;
        /// <summary>
        /// The bits currently useable value in decimal format.
        /// </summary>
        int bitsCurrent = 0;
        /// <summary>
        /// The length bits currently useable.
        /// </summary>
        UInt32 bitsCurrentLen = 0;
        UInt32 currentIndex = 0;
        #endregion

        /// <summary>
        /// Constructor, with some initialization.
        /// </summary>
        public CompressFactory() 
        {
            bitBuffer = new List<bool>();
            compressDataList = new ArrayList();
            MaxP = Config.HitoryLen;
            MaxL = Config.MaxLen;
            historyBuffer = new byte[MaxP];
        }

        #region Private Methods
        /// <summary>
        /// Convert an BitArray of length 8 to a byte.
        /// </summary>
        private byte ConvertToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }


        /// <summary>
        /// Reverse the bit sequence in a byte.
        /// </summary>
        private byte littleToBig(byte b)
        {
            byte[] ba = new byte[] { b };
            BitArray bits = new BitArray(ba);
            BitArray tbits = new BitArray(8);
            for (int i = 0; i < 4; ++i)
            {
                tbits[i] = bits[7 - i];
                tbits[4 + i] = bits[3 - i];
            }
            return ConvertToByte(tbits);
        }

        /// <summary>
        /// Add a byte (a literal) to the result bit buffer.
        /// </summary>
        private void addByte(byte b)
        {
            bitBuffer.Add(false);
            byte[] ba = new byte[] { b };
            BitArray bits = new BitArray(ba);
            for (int i = 7; i >= 0; --i)
                bitBuffer.Add(bits[i]);
        }


        /// <summary>
        /// Get the final byte array from the bit buffer.
        /// </summary>
        private void getCompressedResult()
        {
            int index = 0;
            BitArray bits;
            while (index + 8 < bitBuffer.Count)
            {
                bits = new BitArray(8);
                for (int i = 0; i < 8; ++i)
                {
                    bits[i] = bitBuffer[index + i];
                }
                compressDataList.Add(ConvertToByte(bits));
                index += 8;
            }
            byte finalByte = (byte)(index + 8 - bitBuffer.Count);
           
            bits = new BitArray(8);
            for (int i = 0; index + i < bitBuffer.Count; ++i)
                bits[i] = bitBuffer[index + i];
            compressDataList.Add(ConvertToByte(bits));

            index = bitBuffer.Count;
            bitBuffer.AddRange(convertByteToBits(finalByte));
            for (int i = 0; index + i < bitBuffer.Count; ++i)
                bits[i] = bitBuffer[index + i];
            compressDataList.Add(ConvertToByte(bits));

            compressData = new byte[compressDataList.Count];
            for (int i = 0; i < compressDataList.Count; ++i)
            {
                compressData[i] = (byte)compressDataList[i];
                compressData[i] = littleToBig(compressData[i]);
            }
                
        }

        /// <summary>
        /// Add the a match to the result bit buffer.
        /// </summary>
        /// <param name="distance">The number of bytes backward to copy from.</param>
        /// <param name="length">The total number of bytes to be copied.</param>
        private void addCompressBytes(int distance, int length)
        {
            Token token  = Config.getTokenDis(distance);
            bitBuffer.AddRange(convertStringToBits(token.BitPrefix));
            bitBuffer.AddRange(convertIntToBits(distance - token.Vbase, token.ValueBits));
            int exp = 1;
            while (length >= Math.Pow(2, exp))
                ++exp;
            token = Config.getTokenLen(length);
            bitBuffer.AddRange(convertStringToBits(token.BitPrefix));
            bitBuffer.AddRange(convertIntToBits(length - (int)Math.Pow(2, exp - 1), token.ValueBits));
        }

        /// <summary>
        /// Convert a byte to an bit list of length 8.
        /// </summary>
        private List<bool> convertByteToBits(byte b)
        {
            List<bool> result = new List<bool>();
            byte[] ba = new byte[] { b };
            BitArray bits = new BitArray(ba);

            for (int i = 7; i >= 0; --i)
                result.Add(bits[i]);
            //DisplayBitArray(bits);           
            return result;
        }

        /// <summary>
        /// Convert an integer to bit list.
        /// </summary>
        /// <param name="val">The value of the integer.</param>
        /// <param name="len">The length of the return bit list.</param>
        private List<bool> convertIntToBits(int val, int len)
        {
            List<bool> result = new List<bool>();
            for (int i = 0; i < len; ++i)
            {
                if ((val & 1) == 1)
                    result.Add(true);
                else
                    result.Add(false);
                val = val >> 1;
            }
            result.Reverse();
            return result;
        }

        /// <summary>
        /// Convert a string to bit list.
        /// </summary>
        /// <param name="s">The input string should only consists of "0" and "1".</param>
        private List<bool> convertStringToBits(string s)
        {
            List<bool> result = new List<bool>();
            for (int i = 0; i < s.Length; ++i)
            {
                if (s[i] == '0')
                    result.Add(false);
                else
                    result.Add(true);
            }
            return result;
        }

        /// <summary>
        /// The main procedure to do compress.
        /// </summary>
        private void doCompress()
        {
            int pos = 0;
            int totalLen = rawData.Length;
            while (pos < totalLen)
            {
                // Start looking for sequence.
                int l = 0;
                // Max left to go.
                int sbl = Math.Min(MaxP, pos);
                // List of positions of sequence.
                List<int> p = new List<int>();
                // Init with first appearences.
                for(int i = 0; i < sbl; ++i)
                {
                    if (rawData[pos - (i + 1)] == rawData[pos])
                        p.Add(i);
                }

                if (p.Count == 0)
                {
                    addByte(rawData[pos]);
                    // Step forward.
                    ++pos;
                }
                else
                {
                    l = 1;
                    while (pos + l < totalLen && l < MaxL - 1)
                    {
                        // Tempp stores sequences of length l + 1.
                        List<int> tempp = new List<int>();
                        foreach (int i in p)
                        {
                            if (rawData[pos - (i+1) + l % (i+1)] == rawData[pos + l])
                                tempp.Add(i);
                        }
                        if (tempp.Count == 0)
                            break; // Didn't find sequences of l + 1 length.
                        else
                        {
                            // Select only the sequences of length l + 1.
                            p.Clear();
                            p.AddRange(tempp);
                            // Go on.
                            ++l;
                        }
                    }
                    if (l < 3)
                    {
                        for (int j = 0; j < l; ++j)
                        {
                            addByte(rawData[pos]);
                            ++pos;
                        }
                    }
                    else
                    {
                        // Found a l length sequence.
                        addCompressBytes(p[0] + 1, l);
                        // Console.WriteLine("distance is {0}, length is {1}", p[0] + 1, l);
                        pos += l;
                    }

                }
            }
            // End sequence.
        }

        /// <summary>
        /// The main procedure to do decompress.
        /// </summary>
        private void doDecompress()
        {
            outputBuffer = new List<byte>();
            int decompressLen = compressData.Length;
            bitsRemainingLen = Convert.ToUInt32(8 * (decompressLen - 1) - compressData[decompressLen - 1]);

            bitsCurrent = 0;
            bitsCurrentLen = 0;
            currentIndex = 0;

            while (bitsRemainingLen > 0)
            {
                int haveBits = 0;
                int inPrefix = 0;

                byte c;
                UInt32 count;
                UInt32 distance;
                // Scan the token table, considering more bits as needed,
                // until the resulting token is found.
                for (int opIndex = 0; opIndex < Config.tokenTableDis.Length; ++opIndex)
                {
                    // Get more bits if needed.
                    while (haveBits < Config.tokenTableDis[opIndex].BitPrefix.Length)
                    {
                        inPrefix = (inPrefix << 1) + Convert.ToInt32(getBits(1));
                        haveBits++;
                    }
                    // A specified token is found.
                    if (inPrefix == Config.tokenTableDis[opIndex].Code)
                    {
                        // A litreal token, a single byte to output.
                        if (Config.tokenTableDis[opIndex].Type == 0)
                        {
                            c = (byte)(Config.tokenTableDis[opIndex].Vbase + getBits(Convert.ToUInt32(Config.tokenTableDis[opIndex].ValueBits)));
                            outputLiteral(c);
                        }
                        else // A match token.
                        {
                            distance = Convert.ToUInt32(Config.tokenTableDis[opIndex].Vbase) + getBits(Convert.ToUInt32(Config.tokenTableDis[opIndex].ValueBits));
                            // The distance back into the history from which to copy.
                            if (distance != 0)
                            {
                                if (getBits(1) == 0)
                                {
                                    count = 3;
                                }
                                else
                                {
                                    count = 4;
                                    int extra = 2;
                                    while (getBits(1) == 1)
                                    {
                                        count *= 2;
                                        extra++;
                                    }

                                    count += getBits(Convert.ToUInt32(extra));
                                }
                                // From the distance backward,copy count bytes to the output.
                                outputMatch(distance, count);
                            }
                            else   // match distance == 0 is a special case, an unencoded sequence is found.
                            {
                                count = getBits(15);
                                
                                // Discard reamining bits.
                                bitsRemainingLen -= bitsCurrentLen;
                                bitsCurrentLen = 0;
                                bitsCurrent = 0;
                                // Copy count unencoded bytes to the output.
                                outputUncoded(count);
                            }
                        }
                        break;
                    }

                }

            }

        }

        /// <summary>
        /// Add a byte to the decompression result buffer.
        /// </summary>
        /// <param name="c"></param>
        private void outputLiteral(byte c)
        {
            historyBuffer[historyIndex] = c;
            if (++historyIndex == historyBuffer.Length)
                historyIndex = 0;
            outputBuffer.Add(c);
        }

        /// <summary>
        /// Add bytes to the decompression result
        /// according to the match distance and length.
        /// </summary>
        /// <param name="distance">The backward length of bytes to copy.</param>
        /// <param name="count">The total length to be copied.</param>
        private void outputMatch(UInt32 distance, UInt32 count)
        {
            byte c;
            UInt32 preIndex = historyIndex + Convert.ToUInt32(historyBuffer.Length) - distance;
            preIndex = preIndex % Convert.ToUInt32(historyBuffer.Length);

            while (count > 0)
            {
                c = historyBuffer[preIndex];
                if (++preIndex == historyBuffer.Length)
                {
                    preIndex = 0;
                }

                historyBuffer[historyIndex] = c;
                if (++historyIndex == historyBuffer.Length)
                {
                    historyIndex = 0;
                }
                outputBuffer.Add(c);
                count--;
            }

        }

        /// <summary>
        /// Add bytes to the decompression result
        /// as a result of the uncoded token.
        /// </summary>
        /// <param name="count">The length of bytes directly copy from 
        /// stream input to output</param>
        private void outputUncoded(UInt32 count)
        {
            byte c;
            while(count > 0)
            {
                c = compressData[currentIndex++];
                bitsRemainingLen -= 8;
                historyBuffer[historyIndex] = c;
                if (++historyIndex == historyBuffer.Length)
                    historyIndex = 0;
                outputBuffer.Add(c);
                --count;
            }
        }

        /// <summary>
        /// Get the result byte array from the result byte buffer(a list).
        /// </summary>
        private byte[] getRecoveryResult()
        {
            recoverData = new byte[outputBuffer.Count];
            for (int i = 0; i < outputBuffer.Count; ++i)
            {
                recoverData[i] = (byte)outputBuffer[i];
            }
            return recoverData;
        }

        /// <summary>
        /// Return the value of the next "bitCount" bits as unsigned.
        /// </summary>
        UInt32 getBits(UInt32 bitCount)
        {
            while (bitsCurrentLen < bitCount)
            {
                bitsCurrent <<= 8;
                bitsCurrent += compressData[currentIndex++];
                bitsCurrentLen += 8;
            }

            bitsRemainingLen -= bitCount;
            bitsCurrentLen -= bitCount;
            UInt32 result = Convert.ToUInt32(bitsCurrent >> Convert.ToInt32(bitsCurrentLen));
            bitsCurrent -= Convert.ToInt32(result) << Convert.ToInt32(bitsCurrentLen);
            return result;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// The public interface of the compression algorithm.
        /// </summary>
        /// <param name="rawdata">The source byte array to compress.</param>
        public byte[] Compress(byte[] rawdata)
        {
            rawData = rawdata;
            doCompress();
            getCompressedResult();
            return compressData;
        }

        /// <summary>
        /// The public interface of the decompression algorithm.
        /// </summary>
        /// <param name="data">The source byte array to decompress.</param>
        /// <param name="header">The header of the bulk data</param>
        public byte[] Decompress(byte[] data, byte header)
        {
            if (header == (RdpSegmentedPdu.PACKET_COMPR_TYPE_RDP8 | RdpSegmentedPdu.PACKET_COMPRESSED))
            {
                // Data is compressed
                compressData = data;
                doDecompress();
                getRecoveryResult();
                return recoverData;
            }
            else
            {
                // Data is not compressed, copy data to history buffer directly.
                if(data != null && data.Length > 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        historyBuffer[historyIndex] = data[i];
                        if (++historyIndex == historyBuffer.Length)
                            historyIndex = 0;
                    }
                }
                return data;
            }
        }
        #endregion

    }
}
