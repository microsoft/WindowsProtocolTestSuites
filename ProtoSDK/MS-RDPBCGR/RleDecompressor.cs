// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Specifies color depth modes of RLE-compressed data
    /// </summary>
    public enum ColorDepth : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// 8 bits color depth
        /// </summary>
        Bits8 = 8,

        /// <summary>
        /// 15 bits color depth, red, green, blue each 5 bits respectively
        /// </summary>
        Bits15 = 15,

        /// <summary>
        /// 16 bits color depth, red, green, blue each 5 bits respectively, 1 bit is alpha/not used
        /// </summary>
        Bits16 = 16,

        /// <summary>
        /// 24 bits color depth, red, green, blue each 8 bits respectively
        /// </summary>
        Bits24 = 24,

        /// <summary>
        /// 32 bits color depth, red, green, blue, and alpha respectively
        /// </summary>
        Bits32 = 32
    }
    
    /// <summary>
    /// RleDecompress class, decompresses RLE-compressed data
    /// </summary>
    public static class RleDecompressor
    {
        #region Internal used structures and enums
        /// <summary>
        /// Specifies REGULAR code types
        /// </summary>
        private enum RegularCodeType : byte
        {
            /// <summary>
            /// The compression order encodes a regular-form background run.
            /// </summary>
            REGULAR_BG_RUN = 0,

            /// <summary>
            /// The compression order encodes a regular-form foreground run.
            /// </summary>
            REGULAR_FG_RUN = 0x1,

            /// <summary>
            /// The compression order encodes a regular-form foreground/background image.
            /// </summary>
            REGULAR_FGBG_IMAGE = 0x2,

            /// <summary>
            /// The compression order encodes a regular-form color run.
            /// </summary>
            REGULAR_COLOR_RUN = 0x3,

            /// <summary>
            /// The compression order encodes a regular-form color image.
            /// </summary>
            REGULAR_COLOR_IMAGE = 0x4
        }

        /// <summary>
        /// Specifies LITE code types
        /// </summary>
        private enum LiteCodeType : byte
        {
            /// <summary>
            /// The compression order encodes a "set" variant lite-form foreground run.
            /// </summary>
            LITE_SET_FG_FG_RUN = 0xC,

            /// <summary>
            /// The compression order encodes a "set" variant lite-form foreground/background image.
            /// </summary>
            LITE_SET_FG_FGBG_IMAGE = 0xD,

            /// <summary>
            /// The compression order encodes a lite-form dithered run.
            /// </summary>
            LITE_DITHERED_RUN = 0xE,
        }

        /// <summary>
        /// Specifies MEGA_MEGA code types
        /// </summary>
        private enum MegaMegaCodeType : byte
        {
            /// <summary>
            /// The compression order encodes a MEGA_MEGA background run.
            /// </summary>
            MEGA_MEGA_BG_RUN = 0xF0,

            /// <summary>
            /// The compression order encodes a MEGA_MEGA foreground run.
            /// </summary>
            MEGA_MEGA_FG_RUN = 0xF1,

            /// <summary>
            /// The compression order encodes a MEGA_MEGA foreground/background image.
            /// </summary>
            MEGA_MEGA_FGBG_IMAGE = 0xF2,

            /// <summary>
            /// The compression order encodes a MEGA_MEGA color run.
            /// </summary>
            MEGA_MEGA_COLOR_RUN = 0xF3,

            /// <summary>
            /// The compression order encodes a MEGA_MEGA color image.
            /// </summary>
            MEGA_MEGA_COLOR_IMAGE = 0xF4,

            /// <summary>
            /// The compression order encodes a "set" variant MEGA_MEGA foreground run.
            /// </summary>
            MEGA_MEGA_SET_FG_RUN = 0xF6,

            /// <summary>
            /// The compression order encodes a "set" variant MEGA_MEGA foreground/background image.
            /// </summary>
            MEGA_MEGA_SET_FGBG_IMAGE = 0xF7,

            /// <summary>
            /// The compression order encodes a MEGA_MEGA dithered run.
            /// </summary>
            MEGA_MEGA_DITHERED_RUN = 0xF8
        }

        /// <summary>
        /// Specifies special code types
        /// </summary>
        private enum SpecialCodeType : byte
        {
            /// <summary>
            /// The compression order encodes a foreground/background image with an 8-bit bitmask of 0x03.
            /// </summary>
            SPECIAL_FGBG_1 = 0xF9,

            /// <summary>
            /// The compression order encodes a foreground/background image with an 8-bit bitmask of 0x05. 
            /// </summary>
            SPECIAL_FGBG_2 = 0xFA,

            /// <summary>
            /// The compression order encodes a single white pixel.
            /// </summary>
            WHITE = 0xFD,

            /// <summary>
            /// The compression order encodes a single black pixel.
            /// </summary>
            BLACK = 0xFE
        }
        #endregion Internal used structures and enums


        #region Constants
        /// <summary>
        /// Black color constant, default background color
        /// </summary>
        private const uint COLOR_BLACK = 0;

        /// <summary>
        /// White color constant, default foreground color
        /// </summary>
        private const uint COLOR_WHITE = 0x00ffffff;

        /// <summary>
        /// Regular code mask, uses the least significant 5 bits for run length
        /// </summary>
        private const int REGULAR_MASK = 0x1f;

        /// <summary>
        /// Lite code mask, uses the least significant 4 bits for run length
        /// </summary>
        private const int LITE_MASK = 0x0f;
        #endregion Constants


        #region Private methods

        /// <summary>
        /// Extracts run length regular foreground/background code type
        /// </summary>
        /// <param name="startIndex">Start index in compressed data buffer</param>
        /// <param name="buffer">The compressed data buffer</param>
        /// <param name="consumedLength">Number of bytes consumed by code type and run length</param>
        /// <returns>The run length</returns>
        private static int ExtractRunLengthRegularFgBg(int startIndex, byte[] buffer, out int consumedLength)
        {
            int runLength = (int)(buffer[startIndex] & REGULAR_MASK);

            // If the run length is 0, it's value of following byte incremented by 1
            // otherwise it's the run length multiplied by 8. 
            // Refer to MS-RDPBCGR TD 2.2.9.1.1.3.1.2.4 for details.
            if (runLength == 0)
            {
                runLength = (int)(buffer[startIndex + 1] + 1);
                consumedLength = 2;
            }
            else
            {
                runLength = runLength * 8;
                consumedLength = 1;
            }

            return runLength;
        }


        /// <summary>
        /// Extracts run length from lite foreground/background code type
        /// </summary>
        /// <param name="startIndex">Start index in compressed data buffer</param>
        /// <param name="buffer">The compressed data buffer</param>
        /// <param name="consumedLength">Number of bytes consumed by code type and run length</param>
        /// <returns>The run length</returns>
        private static int ExtractRunLengthLiteFgBg(int startIndex, byte[] buffer, out int consumedLength)
        {
            int runLength = (int)(buffer[startIndex] & LITE_MASK);

            // If the run length is 0, it's value of following byte incremented by 1
            // otherwise it's the run length multiplied by 8
            // Refer to MS-RDPBCGR TD 2.2.9.1.1.3.1.2.4 for details.
            if (runLength == 0)
            {
                runLength = (int)(buffer[startIndex + 1] + 1);
                consumedLength = 2;
            }
            else
            {
                runLength = runLength * 8;
                consumedLength = 1;
            }

            return runLength;
        }


        /// <summary>
        /// Extracts run length from regular code type(excludes regular fg/bg)
        /// </summary>
        /// <param name="startIndex">Start index in compressed data buffer</param>
        /// <param name="buffer">The compressed data buffer</param>
        /// <param name="consumedLength">Number of bytes consumed by code type and run length</param>
        /// <returns>The run length</returns>
        private static int ExtractRunLengthRegular(int startIndex, byte[] buffer, out int consumedLength)
        {
            int runLength = (int)(buffer[startIndex] & REGULAR_MASK);

            // If the run length is 0, it's value of following byte incremented by 32
            // Refer to MS-RDPBCGR TD 2.2.9.1.1.3.1.2.4 for details.
            if (runLength == 0)
            {
                runLength = (int)(buffer[startIndex + 1] + 32);
                consumedLength = 2;
            }
            else
            {
                consumedLength = 1;
            }

            return runLength;
        }


        /// <summary>
        /// Extracts run length from lite code type(excludes lite fg/bg)
        /// </summary>
        /// <param name="startIndex">Start index in compressed data buffer</param>
        /// <param name="buffer">The compressed data buffer</param>
        /// <param name="consumedLength">Number of bytes consumed by code type and run length</param>
        /// <returns>The run length</returns>
        private static int ExtractRunLengthLite(int startIndex, byte[] buffer, out int consumedLength)
        {
            int runLength = (int)(buffer[startIndex] & LITE_MASK);

            // If run length is 0, it's value of following byte incremented by 16
            // Refer to MS-RDPBCGR TD 2.2.9.1.1.3.1.2.4 for details.
            if (runLength == 0)
            {
                runLength = (int)(buffer[startIndex + 1] + 16);
                consumedLength = 2;
            }
            else
            {
                consumedLength = 1;
            }

            return runLength;
        }


        /// <summary>
        /// Extracts run length from mega_mega code type
        /// </summary>
        /// <param name="startIndex">Start index in compressed data buffer</param>
        /// <param name="buffer">The compressed data buffer</param>
        /// <param name="consumedLength">Number of bytes consumed by code type and run length</param>
        /// <returns>The run length</returns>
        private static int ExtractRunLengthMegaMega(int startIndex, byte[] buffer, out int consumedLength)
        {
            // The run length is the value of the following 2 bytes
            // Refer to MS-RDPBCGR TD 2.2.9.1.1.3.1.2.4 for details.
            int runLength = (int)BitConverter.ToUInt16(buffer, startIndex + 1);
            consumedLength = 3;

            return runLength;
        }


        /// <summary>
        /// Checks if a code type is MEGA_MEGA code type
        /// </summary>
        /// <param name="code">The code type</param>
        /// <returns>True if it's MEGA_MEGA code type, false otherwise</returns>
        private static bool IsMegaMegaCode(byte code)
        {
            return Enum.IsDefined(typeof(MegaMegaCodeType), code);
        }


        /// <summary>
        /// Checks if a code type is LITE code type except LITE_SET_FG_FGBG_IMAGE
        /// </summary>
        /// <param name="code">The code type</param>
        /// <returns>True if it's LITE code type(but not LITE_SET_FG_FGBG_IMAGE), false otherwise</returns>
        private static bool IsLiteCode(byte code)
        {
            return Enum.IsDefined(typeof(LiteCodeType), code)
                && (code != (byte)LiteCodeType.LITE_SET_FG_FGBG_IMAGE);
        }


        /// <summary>
        /// Checks if a code type is REGULAR code type except REGULAR_FGBG_IMAGE
        /// </summary>
        /// <param name="code">The code type</param>
        /// <returns>True if it's REGULAR code type(but not REGULAR_FGBG_IMAGE), false otherwise</returns>
        private static bool IsRegularCode(byte code)
        {
            return Enum.IsDefined(typeof(RegularCodeType), code)
                && (code != (byte)RegularCodeType.REGULAR_FGBG_IMAGE);
        }


        /// <summary>
        /// Extracts run length from compressed data with specififed code type
        /// </summary>
        /// <param name="code">The code type</param>
        /// <param name="index">Start index in compressed data buffer</param>
        /// <param name="buffer">The compressed data buffer</param>
        /// <returns>The run length of specified code type</returns>
        private static int ExtractRunLength(byte code, byte[] buffer, ref int index)
        {
            int runLength = 0;
            int consumedLength = 0;

            // Run length of REGULAR_FGBG_IMAGE and LITE_SET_FG_FGBG_IMAGE needs special treatment. Both run 
            // length extracted from compressed data buffer must multiply by 8 to get the final run length.
            // Refer to MS-RDPBCGR TD 2.2.9.1.1.3.1.2.4 for details.
            if (code == (byte)RegularCodeType.REGULAR_FGBG_IMAGE)
            {
                runLength = ExtractRunLengthRegularFgBg(index, buffer, out consumedLength);
            }
            else if (code == (byte)LiteCodeType.LITE_SET_FG_FGBG_IMAGE)
            {
                runLength = ExtractRunLengthLiteFgBg(index, buffer, out consumedLength);
            }
            else if (IsRegularCode(code))
            {
                runLength = ExtractRunLengthRegular(index, buffer, out consumedLength);
            }
            else if (IsLiteCode(code))
            {
                runLength = ExtractRunLengthLite(index, buffer, out consumedLength);
            }
            else if (IsMegaMegaCode(code))
            {
                runLength = ExtractRunLengthMegaMega(index, buffer, out consumedLength);
            }
            else
            {
                runLength = 0;
                consumedLength = 1;
            }
            // Move index forward
            index += consumedLength;

            return runLength;
        }


        /// <summary>
        /// Sets pixel value at specified index in the buffer
        /// </summary>
        /// <param name="destBuffer">The buffer in which the pixel value is set</param>
        /// <param name="index">The specified index</param>
        /// <param name="bytesPerPixel">Number of bytes used by a pixel</param>
        /// <param name="pixelValue">The pixel value to be set</param>
        private static void SetPixel(byte[] destBuffer, int index, int bytesPerPixel, uint pixelValue)
        {
            byte[] pixelBytes = BitConverter.GetBytes(pixelValue);
            Array.Copy(pixelBytes, 0, destBuffer, index, bytesPerPixel);
        }


        /// <summary>
        /// Gets pixel value at specified index in the buffer
        /// </summary>
        /// <param name="buffer">The input buffer</param>
        /// <param name="index">The specified index</param>
        /// <param name="bytesPerPixel">Number of bytes used by a pixel</param>
        /// <returns>The pixel value</returns>
        private static uint GetPixel(byte[] buffer, int index, int bytesPerPixel)
        {
            switch (bytesPerPixel)
            {
                case 1:
                    // A 8-bit pixel occupies 1 byte
                    return (uint)buffer[index];

                case 2:
                    // Both 15-bit & 16-bit pixel occupies 2 bytes
                    return BitConverter.ToUInt16(buffer, index);

                case 3:
                case 4:
                    // when bytesPerPixel == 3, the last byte will be set to zero,
                    // which is the most significant byte in little endian
                    byte[] pixelBuf = new byte[4];
                    Array.Copy(buffer, index, pixelBuf, 0, bytesPerPixel);
                    return BitConverter.ToUInt32(pixelBuf, 0);

                default:
                    throw new InvalidOperationException("Invalid color depth");
            }
        }


        /// <summary>
        /// Writes a foreground/background image of specified length to destination buffer with specified bitmask
        /// </summary>
        /// <param name="destBuffer">The destination buffer</param>
        /// <param name="startIndex">The start index in dest buffer</param>
        /// <param name="bitmask">The bitmask used to decide if the pixel needs to be XOR'ed</param>
        /// <param name="currentForegroundPixel">Current foreground pixel value</param>
        /// <param name="length">Length to be written to dest buffer</param>
        /// <param name="bytesPerPixel">Number of bytes used by a pixel</param>
        /// <param name="rowDelta">Scan line width</param>
        private static void WriteFgBgImage(
            byte[] destBuffer, 
            ref int startIndex, 
            byte bitmask,
            uint currentForegroundPixel, 
            int length, 
            int bytesPerPixel, 
            int rowDelta)
        {
            uint mask = 0x1;

            while (length > 0)
            {
                // The pixel on the last scanline
                uint xorPixel = GetPixel(destBuffer, (int)(startIndex - rowDelta), bytesPerPixel);

                // If mask equals bitmask, set pixel with current foreground color xor'ed by the pixel on the
                // previous line, otherwise set pixel with background color
                if ((bitmask & mask) == mask)
                {
                    SetPixel(destBuffer, startIndex, bytesPerPixel, (uint)(xorPixel ^ currentForegroundPixel));
                }
                else
                {
                    SetPixel(destBuffer, startIndex, bytesPerPixel, xorPixel);
                }
                startIndex += bytesPerPixel;
                mask <<= 1;
                length--;
            }
        }


        /// <summary>
        /// Writes a foreground/background image to the first line of dest buffer
        /// </summary>
        /// <param name="destBuffer">The destination buffer</param>
        /// <param name="startIndex">The start index in dest buffer</param>
        /// <param name="bitmask">The bitmask used to decide if the pixel needs to be XOR'ed</param>
        /// <param name="currentForegroundPixel">Current foreground pixel value</param>
        /// <param name="length">Length to be written to dest buffer</param>
        /// <param name="bytesPerPixel">Number of bytes used by a pixel</param>
        private static void WriteFirstLineFgBgImage(
            byte[] destBuffer, 
            ref int startIndex, 
            byte bitmask,
            uint currentForegroundPixel,
            int length, 
            int bytesPerPixel)
        {
            uint mask = 0x1;

            while (length > 0)
            {
                // If mask equals bitmask, set pixel with current foreground color
                // otherwise set pixel with background color
                if ((bitmask & mask) == mask)
                {
                    SetPixel(destBuffer, startIndex, bytesPerPixel, currentForegroundPixel);
                }
                else
                {
                    SetPixel(destBuffer, startIndex, bytesPerPixel, COLOR_BLACK);
                }
                startIndex += bytesPerPixel;
                mask <<= 1;
                length--;
            }
        }


        /// <summary>
        /// Extracts code type from srcBuffer at specified index
        /// </summary>
        /// <param name="inputBuffer">The input buffer</param>
        /// <param name="index">The index in the input buffer</param>
        /// <returns>The code type</returns>
        private static byte ExtractCodeId(byte[] inputBuffer, int index)
        {
            // Try MEGA_MEGA code types(uses 1 byte for code type)
            byte unknownCodeId = inputBuffer[index];
            if (Enum.IsDefined(typeof(MegaMegaCodeType), unknownCodeId)
                || Enum.IsDefined(typeof(SpecialCodeType), unknownCodeId))
            {
                return unknownCodeId;
            }

            // Try LITE code types(uses high 4 bits of 1 byte)
            byte liteCode = (byte)(unknownCodeId >> 4);
            if (Enum.IsDefined(typeof(LiteCodeType), liteCode))
            {
                return liteCode;
            }

            // Try REGULAR code types(uses high 3 bits of 1 byte)
            byte regCode = (byte)(unknownCodeId >> 5);
            if (Enum.IsDefined(typeof(RegularCodeType), regCode))
            {
                return regCode;
            }
            
            // Not of any type, just reutrn the value
            return unknownCodeId;
        }


        /// <summary>
        /// Decompresses an RLE-compressed buffer
        /// </summary>
        /// <param name="inputBuffer">The input buffer</param>
        /// <param name="outputBuffer">The output buffer</param>
        /// <param name="bytesPerPixel">Number of bytes used by a pixel</param>
        /// <param name="rowDelta">Scan line width</param>
        /// <returns>The decompressed size</returns>
        private static int InternalRleDecompress(
            byte[] inputBuffer, 
            byte[] outputBuffer,
            int bytesPerPixel, 
            int rowDelta)
        {
            const int fgbgImageLength = 8;
            const byte specialFgbg1Mask = 0x03;
            const byte specialFgbg2Mask = 0x05;
            
            // Current foreground
            uint currentForegroundPixel = COLOR_WHITE;
            // Indicates if a pixel of current foreground needs to be inserted
            bool insertFgPixel = false;
            // Indicates if it's on the first scanline
            bool isFirstLine = true;
            int inputIndex = 0;
            int outputIndex = 0;
            int runLength = 0;
            byte codeType = 0;

            while (inputIndex < inputBuffer.Length)
            {
                if (isFirstLine && (outputIndex >= rowDelta))
                {
                    isFirstLine = false;
                    insertFgPixel = false;
                }

                // Get code type
                codeType = (byte)ExtractCodeId(inputBuffer, inputIndex);

                #region Handle BG_RUN
                // A Background Run Order encodes a run of pixels where each pixel in the run matches the 
                // decompressed pixel on the previous scanline. If there is no previous scanline then each 
                // pixel in the run MUST be black. 
                if (codeType == (byte)RegularCodeType.REGULAR_BG_RUN
                    || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_BG_RUN)
                {

                    runLength = ExtractRunLength(codeType, inputBuffer, ref inputIndex);

                    // First scanline
                    if (isFirstLine)
                    {
                        if (insertFgPixel)
                        {
                            SetPixel(outputBuffer, outputIndex, bytesPerPixel, currentForegroundPixel);
                            outputIndex += bytesPerPixel;
                            runLength--;
                        }

                        while (runLength > 0)
                        {
                            SetPixel(outputBuffer, outputIndex, bytesPerPixel, COLOR_BLACK);
                            outputIndex += bytesPerPixel;
                            runLength--;
                        }
                    }
                    // Not first scanline
                    else
                    {
                        if (insertFgPixel)
                        {
                            SetPixel(
                                outputBuffer,
                                outputIndex,
                                bytesPerPixel,
                                GetPixel(outputBuffer,
                                    (int)(outputIndex - rowDelta), bytesPerPixel) ^ currentForegroundPixel
                            );
                            outputIndex += bytesPerPixel;
                            runLength--;
                        }

                        while (runLength > 0)
                        {
                            SetPixel(
                                outputBuffer,
                                outputIndex,
                                bytesPerPixel,
                                GetPixel(outputBuffer, (int)(outputIndex - rowDelta), bytesPerPixel));
                            outputIndex += bytesPerPixel;
                            runLength--;
                        }
                    }

                    // A follow-on background run order will need a foreground pixel inserted. 
                    insertFgPixel = true;
                    continue;
                }
                #endregion Handle BG_RUN

                // For any of the other run-types a follow-on background run  
                // order does not need a foreground pel inserted. 
                insertFgPixel = false;

                #region Handle FG_RUN
                // A Foreground Run Order encodes a run of pixels where each pixel in the run matches the 
                // decompressed pixel on the previous scanline XOR’ed with the current foreground color. 
                // If there is no previous scanline, then each pixel in the run MUST be set to the current 
                // foreground color (the initial foreground color is white).
                if (codeType == (byte)RegularCodeType.REGULAR_FG_RUN
                        || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_FG_RUN
                        || codeType == (byte)LiteCodeType.LITE_SET_FG_FG_RUN
                        || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_SET_FG_RUN)
                {
                    runLength = ExtractRunLength(codeType, inputBuffer, ref inputIndex);

                    // If the order is a "set" variant, then in addition to encoding a run of pixels, the 
                    // order also encodes a new foreground color (in little-endian format) in the bytes 
                    // following the optional run length.
                    if (codeType == (byte)LiteCodeType.LITE_SET_FG_FG_RUN
                        || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_SET_FG_RUN)
                    {
                        currentForegroundPixel = GetPixel(inputBuffer, inputIndex, bytesPerPixel);
                        inputIndex += bytesPerPixel;
                    }

                    while (runLength > 0)
                    {
                        if (isFirstLine)
                        {
                            SetPixel(outputBuffer, outputIndex, bytesPerPixel, currentForegroundPixel);
                            outputIndex += bytesPerPixel;
                        }
                        else
                        {
                            SetPixel(
                                outputBuffer, 
                                outputIndex, 
                                bytesPerPixel, 
                                (uint)(GetPixel(outputBuffer, 
                                    (int)(outputIndex - rowDelta), bytesPerPixel) ^ currentForegroundPixel)
                            );
                            outputIndex += bytesPerPixel;
                        }
                        runLength--;
                    }
                    continue;
                }
                #endregion Handle FG_RUN

                #region Handle DITHERED_RUN
                // A Dithered Run Order encodes a run of pixels which is composed of two alternating 
                // colors. The two colors are encoded (in little-endian format) in the bytes following the 
                // optional run length
                if (codeType == (byte)LiteCodeType.LITE_DITHERED_RUN
                    || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_DITHERED_RUN)
                {
                    runLength = ExtractRunLength(codeType, inputBuffer, ref inputIndex);

                    // Get the two alternating colors
                    uint pixelA = GetPixel(inputBuffer, inputIndex, bytesPerPixel);
                    inputIndex += bytesPerPixel;
                    uint pixelB = GetPixel(inputBuffer, inputIndex, bytesPerPixel);
                    inputIndex += bytesPerPixel;

                    // The run length encodes the number of pixel-pairs in the run (not pixels). 
                    while (runLength > 0)
                    {
                        SetPixel(outputBuffer, outputIndex, bytesPerPixel, pixelA);
                        outputIndex += bytesPerPixel;
                        SetPixel(outputBuffer, outputIndex, bytesPerPixel, pixelB);
                        outputIndex += bytesPerPixel;

                        runLength--;
                    }
                    continue;
                }
                #endregion Handle DITHERED_RUN

                #region Handle COLOR_RUN
                // A Color Run Order encodes a run of pixels where each pixel is the same color. The color 
                // is encoded (in little-endian format) in the bytes following the optional run length.
                if (codeType == (byte)RegularCodeType.REGULAR_COLOR_RUN
                    || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_COLOR_RUN)
                {
                    runLength = ExtractRunLength(codeType, inputBuffer, ref inputIndex);

                    uint pixel = GetPixel(inputBuffer, inputIndex, bytesPerPixel);
                    inputIndex += bytesPerPixel;

                    while (runLength > 0)
                    {
                        SetPixel(outputBuffer, outputIndex, bytesPerPixel, pixel);
                        outputIndex += bytesPerPixel;

                        runLength--;
                    }
                    continue;
                }
                #endregion Handle COLOR_RUN

                #region Handle FGBG_IMAGE
                // A Foreground/Background Image Order encodes a binary image.
                if (codeType == (byte)RegularCodeType.REGULAR_FGBG_IMAGE
                        || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_FGBG_IMAGE
                        || codeType == (byte)LiteCodeType.LITE_SET_FG_FGBG_IMAGE
                        || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_SET_FGBG_IMAGE)
                {
                    runLength = ExtractRunLength(codeType, inputBuffer, ref inputIndex);

                    // If the order is a "set" variant, then in addition to encoding a binary image, the 
                    // order also encodes a new foreground color (in little-endian format) in the bytes 
                    // following the optional run length. 
                    if (codeType == (byte)LiteCodeType.LITE_SET_FG_FGBG_IMAGE
                        || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_SET_FGBG_IMAGE)
                    {
                        currentForegroundPixel = GetPixel(inputBuffer, inputIndex, bytesPerPixel);
                        inputIndex += bytesPerPixel;
                    }

                    while (runLength > fgbgImageLength)
                    {
                        byte bitmask = inputBuffer[inputIndex];
                        inputIndex++;

                        if (isFirstLine)
                        {
                            WriteFirstLineFgBgImage(
                                outputBuffer, 
                                ref outputIndex, 
                                bitmask, 
                                currentForegroundPixel, 
                                fgbgImageLength, 
                                bytesPerPixel);
                        }
                        else
                        {
                            WriteFgBgImage(
                                outputBuffer, 
                                ref outputIndex, 
                                bitmask, 
                                currentForegroundPixel, 
                                fgbgImageLength,
                                bytesPerPixel, 
                                rowDelta);
                        }
                        runLength -= fgbgImageLength;
                    }

                    if (runLength > 0)
                    {
                        byte bitmask = inputBuffer[inputIndex];
                        inputIndex++;

                        if (isFirstLine)
                        {
                            WriteFirstLineFgBgImage(
                                outputBuffer, 
                                ref outputIndex, 
                                bitmask, 
                                currentForegroundPixel, 
                                runLength, 
                                bytesPerPixel);
                        }
                        else
                        {
                            WriteFgBgImage(
                                outputBuffer, 
                                ref outputIndex, 
                                bitmask, 
                                currentForegroundPixel, 
                                runLength, 
                                bytesPerPixel, 
                                rowDelta);
                        }
                    }
                    continue;
                }
                #endregion Handle FGBG_IMAGE

                #region Handle COLOR_IMAGE
                // Handle Color Image Orders. A Color Image Order encodes a run of decompressed pixels.
                if (codeType == (byte)RegularCodeType.REGULAR_COLOR_IMAGE
                    || codeType == (byte)MegaMegaCodeType.MEGA_MEGA_COLOR_IMAGE)
                {
                    int byteCount = 0;
                    runLength = ExtractRunLength(codeType, inputBuffer, ref inputIndex);

                    // The run length encodes the number of pixels in the run.  So, to compute the actual 
                    // number of bytes which follow the optional run length, the run length MUST be multiplied 
                    // by the color depth (in bits-per-pixel) of the bitmap data. 
                    byteCount = runLength * bytesPerPixel;
                    Array.Copy(inputBuffer, inputIndex, outputBuffer, outputIndex, byteCount);
                    inputIndex += byteCount;
                    outputIndex += byteCount;

                    continue;
                }
                #endregion Handle COLOR_IMAGE

                #region Handle SPECIAL_FGBG_1
                // Special Order 1. The compression order encodes a foreground/background image with 
                // an 8-bit bitmask of 0x03. 
                if (codeType == (byte)SpecialCodeType.SPECIAL_FGBG_1)
                {
                    if (isFirstLine)
                    {
                        WriteFirstLineFgBgImage(
                            outputBuffer, 
                            ref outputIndex, 
                            specialFgbg1Mask, 
                            currentForegroundPixel, 
                            fgbgImageLength, 
                            bytesPerPixel);
                    }
                    else
                    {
                        WriteFgBgImage(
                            outputBuffer, 
                            ref outputIndex, 
                            specialFgbg1Mask, 
                            currentForegroundPixel, 
                            fgbgImageLength, 
                            bytesPerPixel, 
                            rowDelta);
                    }
                    inputIndex++;
                    continue;
                }
                #endregion Handle SPECIAL_FGBG_1

                #region Handle SPECIAL_FGBG_2
                // Special Order 2. The compression order encodes a foreground/background image with
                // an 8-bit bitmask of 0x05. 
                if (codeType == (byte)SpecialCodeType.SPECIAL_FGBG_2)
                {
                    if (isFirstLine)
                    {
                        WriteFirstLineFgBgImage(
                            outputBuffer, 
                            ref outputIndex, 
                            specialFgbg2Mask, 
                            currentForegroundPixel, 
                            fgbgImageLength, 
                            bytesPerPixel);
                    }
                    else
                    {
                        WriteFgBgImage(
                            outputBuffer, 
                            ref outputIndex, 
                            specialFgbg2Mask, 
                            currentForegroundPixel, 
                            fgbgImageLength, 
                            bytesPerPixel, 
                            rowDelta);
                    }
                    inputIndex++;
                    continue;
                }
                #endregion Handle SPECIAL_FGBG_2

                #region Handle a WHITE color pixel
                // White Order. Encodes a single white pixel. 
                if (codeType == (byte)SpecialCodeType.WHITE)
                {
                    SetPixel(outputBuffer, outputIndex, bytesPerPixel, COLOR_WHITE);
                    outputIndex += bytesPerPixel;
                    inputIndex++;
                    continue;
                }
                #endregion Handle a WHITE color pixel

                #region Handle a BLACK color pixel
                // Black Order. Encodes a single black pixel
                if (codeType == (byte)SpecialCodeType.BLACK)
                {
                    SetPixel(outputBuffer, outputIndex, bytesPerPixel, COLOR_BLACK);
                    outputIndex += bytesPerPixel;
                    inputIndex++;
                    continue;
                }
                #endregion Handle a BLACK color pixel

                // The code type doesn't match any, move forward the index to avoid infinite loop
                inputIndex++;
            }

            return outputIndex;
        }
        #endregion Private methods


        #region internal methods
        /// <summary>
        /// Decompresses an RLE-compressed buffer
        /// </summary>
        /// <param name="inputBuffer">The compressed data buffer</param>
        /// <param name="colorDepth">Color depth of compressed image, can be 8, 16, 24(bits). 
        /// Note 4-bit color depth is not supported </param>
        /// <param name="width">Width of compressed image, in pixels</param>
        /// <param name="height">Height of compressed image, in pixels</param>
        /// <returns>The decompressed data, if inputBuffer contains invalid data or decompression fails, null is 
        /// returned</returns>
        public static byte[] Decompress(byte[] inputBuffer, ColorDepth colorDepth, int width, int height)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            const int dwordSize = sizeof(int);
            const int bitsPerByte = 8;
            
            // Number of bytes used by a pixel
            int bytesPerPixel = 0;
            // 15 bits, special case
            if (colorDepth == ColorDepth.Bits15)
            {
                bytesPerPixel = 2;
            }
            else
            {
                // 8 bits, 16 bits or 24 bits, just divide by bitsPerByte
                bytesPerPixel = (int)colorDepth / bitsPerByte;
            }

            int bytesPerLine = width * bytesPerPixel;
            // The rowDelta must be DWORD-aligned
            if (bytesPerLine % dwordSize > 0)
            {
                // Remove the remainder, and add a DWORD to make bytesPerLine DWORD-aligned
                bytesPerLine = bytesPerLine / dwordSize * dwordSize + dwordSize;
            }
            // Scan line width
            int rowDelta = bytesPerLine;

            // Set dest buffer, this is not accurate size. The decompressed size cannot be decided until 
            // decompression is completed.
            byte[] destBuffer = new byte[rowDelta * height * bytesPerPixel];

            int decompressedLength = 0;
            byte[] outBuffer = null;
            try
            {
                decompressedLength = InternalRleDecompress(
                    inputBuffer,
                    destBuffer,
                    bytesPerPixel,
                    rowDelta);
                // Now we know the accurate decompressed size
                outBuffer = new byte[decompressedLength];
                // Copy decompressed data
                Array.Copy(destBuffer, 0, outBuffer, 0, outBuffer.Length);
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            
            return outBuffer;
        }
        #endregion internal methods
    }
}
