// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    using RLEX_RGB_TRIPLET = Color_RGB;

    /// <summary>
    /// It decode byte stream into CLEARCODEC_SUBCODECS_DATA structure
    /// </summary>
    class ClearCodecSubCodecDecoder : BasicTypeDecoder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClearCodecSubCodecDecoder(byte[] Data)
            : base(Data)
        {
        }

        /// <summary>
        /// It decode subcodec layer byte stream into CLEARCODEC_SUBCODECS_DATA structure.
        /// </summary>
        /// <param name="subcodecData">The structure that decode result save to.</param>
        /// <returns>true if decode success, otherwise return false.</returns>
        public bool Decode(ref CLEARCODEC_SUBCODEC_DATA subcodecData)
        {
            if (decodeData == null) return false;

            List<CLEARCODEC_SUBCODEC> subcodecList = new List<CLEARCODEC_SUBCODEC>();
            while (offset < decodeData.Count())
            {
                CLEARCODEC_SUBCODEC subcodec = new CLEARCODEC_SUBCODEC();
                if (!DecodeSubcodec(ref subcodec)) return false;
                subcodecList.Add(subcodec);
            }
            subcodecData.subcodecArr = subcodecList.ToArray();
            return true;
        }

        /// <summary>
        /// It decode a subcodec bitmap byte stream into CLEARCODEC_SUBCODEC structure
        /// </summary>
        /// <param name = "subcodec"> the structure that decode result save to </param>
        /// <return> true if decode success, otherwise return false </return>
        public bool DecodeSubcodec(ref CLEARCODEC_SUBCODEC subcodec)
        {
            if (!DecodeUShort(ref subcodec.xStart)) return false;
            if (!DecodeUShort(ref subcodec.yStart)) return false;
            if (!DecodeUShort(ref subcodec.width)) return false;
            if (!DecodeUShort(ref subcodec.height)) return false;

            if (!DecodeUInt(ref subcodec.bitmapDataByteCount)) return false;

            byte scId = 0;
            if (!DecodeByte(ref scId)) return false;
            subcodec.bitmapData.subCodecId = (CLEARCODEC_SUBCODEC_ID)scId;

            if (subcodec.bitmapData.subCodecId == CLEARCODEC_SUBCODEC_ID.SUBCODEC_RLEX)
            {
                if (!DecodeRlexData(subcodec.bitmapDataByteCount, ref subcodec.bitmapData.bmpRlex)) return false;
            }
            else if (subcodec.bitmapData.subCodecId == CLEARCODEC_SUBCODEC_ID.SUBCODEC_RAW)
            {
                if (subcodec.bitmapDataByteCount != 3 * subcodec.width * subcodec.height)
                    return false;
                if (!DecodeRawData(subcodec.bitmapDataByteCount, ref subcodec.bitmapData.bmpPixels)) return false;
            }
            else   // NSCodec data
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// It decode rlexByteCount bytes data into CLEARCODEC_SUBCODEC_RLEX structure
        /// </summary>
        /// <param name = "rlexByteCount"> the number of bytes to be decoded into CLEARCODEC_SUBCODEC_RLEX structure </param>
        /// <param name = "rlex"> the structure that decode result save to </param>
        /// <return> true if decode success, otherwise return false </return>
        public bool DecodeRlexData(uint rlexByteCount, ref CLEARCODEC_SUBCODEC_RLEX rlex)
        {
            if (!DecodeByte(ref rlex.paletteCount)) return false;
            rlexByteCount--;

            // calculate bitNum of stopIndex in rlex segment
            byte bitNum = 0;
            byte count = rlex.paletteCount;
            // calculate bit number for stopIndex
            while (count > 0)
            {
                bitNum++;
                count >>= 1;
            }

            // decode pletteEntries
            List<RLEX_RGB_TRIPLET> pletList = new List<RLEX_RGB_TRIPLET>();
            for (int i = 0; i < rlex.paletteCount; i++)
            {
                RLEX_RGB_TRIPLET plet = new RLEX_RGB_TRIPLET();
                if (!DecodeByte(ref plet.B)) return false;
                if (!DecodeByte(ref plet.G)) return false;
                if (!DecodeByte(ref plet.R)) return false;
                rlexByteCount -= 3;
                pletList.Add(plet);
            }
            rlex.paletteEntries = pletList.ToArray();

            // decode rlex segments
            List<CLEARCODEC_SUBCODEC_RLEX_SEGMENT> segList = new List<CLEARCODEC_SUBCODEC_RLEX_SEGMENT>();
            while (rlexByteCount > 0)
            {
                CLEARCODEC_SUBCODEC_RLEX_SEGMENT rlex_seg = new CLEARCODEC_SUBCODEC_RLEX_SEGMENT();
                byte decodedBytes = DecodeRlexSeg(bitNum, ref rlex_seg);
                if (decodedBytes == 0) return false;
                rlexByteCount -= decodedBytes;
                segList.Add(rlex_seg);
            }
            
            if (rlexByteCount < 0) return false;
            rlex.segments = segList.ToArray();
            return true;
        }

        /// <summary>
        /// It decode byte stream into CLEARCODEC_SUBCODEC_RLEX_SEGMENT structure
        /// </summary>
        /// <param name = "stopIdxBitNum"> the bit number of stopIndex field </param>
        /// <param name = "relSeg"> the structure that decode result save to </param>
        /// <return> > 0 if decode success, otherwise return 0 </return>
        public byte DecodeRlexSeg(byte stopIdxBitNum, ref CLEARCODEC_SUBCODEC_RLEX_SEGMENT relSeg)
        {
            byte stopIdx_suiteDepth = 0;
            byte decodedCount = 0;
            if (!DecodeByte(ref stopIdx_suiteDepth)) return 0;
            decodedCount++;
            byte stopIdxMask =  (byte)((1 << stopIdxBitNum) - 1);
            relSeg.stopIndex = (byte)(stopIdx_suiteDepth & stopIdxMask);
            relSeg.suiteDepth = (byte)(stopIdx_suiteDepth >> stopIdxBitNum);

            if (!DecodeRulLengthFactor(ref relSeg.runLengthFactor)) return 0;
            if (relSeg.runLengthFactor < 0xff)
                decodedCount++;
            else if (relSeg.runLengthFactor < 0xffff)
                decodedCount += 3;
            else
                decodedCount += 7;

            return decodedCount;
        }

        /// <summary>
        /// It decode rawDataCount bytes data into RLEX_RGB_TRIPLET structure array
        /// </summary>
        /// <param name = "rawDataCount"> the number of bytes to be decoeded as raw bitmap </param>
        /// <param name = "bmpPixels"> the structure array that decode result save to </param>
        /// <return> > true if decode success, otherwise return false </return>
        public bool DecodeRawData(uint rawDataCount, ref RLEX_RGB_TRIPLET[] bmpPixels)
        {
            List<RLEX_RGB_TRIPLET> pixelList = new List<RLEX_RGB_TRIPLET>();
            while (rawDataCount > 0)
            {
                RLEX_RGB_TRIPLET pixel = new RLEX_RGB_TRIPLET();
                if (!DecodeByte(ref pixel.B)) return false;
                if (!DecodeByte(ref pixel.G)) return false;
                if (!DecodeByte(ref pixel.R)) return false;
                rawDataCount -= 3;
                pixelList.Add(pixel);
            }
            if (rawDataCount < 0) return false;
            bmpPixels = pixelList.ToArray();
            return true;
        }
    }
}
