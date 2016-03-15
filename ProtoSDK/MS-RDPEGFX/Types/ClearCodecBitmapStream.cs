// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    using RLEX_RGB_TRIPLET = Color_RGB;
    /// <summary>
    /// The CLEARCODEC_COMPOSITE_PAYLOAD structure contains bitmap data encoded using ClearCodec compression techniques.   
    /// </summary>
    public struct CLEARCODEC_COMPOSITE_PAYLOAD
    {
        /// <summary>
        /// A 32-bit, unsigned integer. specify the number of bytes in the residualData field.   
        /// </summary>
        public uint residualByteCount;
        /// <summary>
        /// A 32-bit, unsigned integer. specify the number of bytes in the bandData field.   
        /// </summary>
        public uint bandByteCount;
        /// <summary>
        /// A 32-bit, unsigned integer. specify the number of bytes in the subcodecData field.   
        /// </summary>
        public uint subcodecByteCount;
        /// <summary>
        /// A optional variable-length byte stream. specify the compressed data for the first layer of the image.  
        /// </summary>
        public byte[] residualData;
        /// <summary>
        /// A optional variable-length byte stream. specify the compressed data for the second layer of the image.  
        /// </summary>
        public byte[] bandData;
        /// <summary>
        /// A optional variable-length byte stream. specify the compressed data for the third layer of the image.  
        /// </summary>
        public byte[] subcodecData;
    }
    
    public class ClearCodec_BitmapStream
    {
        #region Const    
        // The blow const specifies glyph and control flags in clearcodec encoded message body

        /// <summary>
        /// No flag is set.
        /// </summary>
        public const byte CLEARCODEC_FLAG_NONE = 0x00;

        /// <summary>
        /// Indicates that the glyphIndex field is present. This flag MUST NOT be used in conjunction 
        /// with a bitmap that has an area larger than 1024 square pixels.  
        /// </summary>
        public const byte CLEARCODEC_FLAG_GLYPH_INDEX = 0x01;

        /// <summary>
        /// Indicates the source of the glyph data. This flag MUST NOT be present if the CLEARCODEC_FLAG_GLYPH_INDEX (0x01) flag 
        /// is not present.
        /// If the CLEARCODEC_FLAG_GLYPH_HIT flag is not present, the glyph data is present in the compositePayload field.
        /// The decompressed payload MUST be placed in the Decompressor Glyph Storage ADM element at the index specified by the glyphIndex field. 
        /// If the CLEARCODEC_FLAG_GLYPH_HIT flag is present, the glyph data is already present in the Decompressor Glyph Storage ADM element 
        /// at the index specified by the glyphIndex field. In this case, the compositePayload field MUST NOT be present.
        /// </summary>
        public const byte CLEARCODEC_FLAG_GLYPH_HIT = 0x02;

        /// <summary>
        /// Indicates that both the V-Bar Storage Cursor ADM element and Short V-Bar Storage Cursor ADM element MUST be reset to 0 before decoding the stream.  
        /// </summary>
        public const byte CLEARCODEC_FLAG_CACHE_RESET = 0x04;

        #endregion

        #region Fields

        /// <summary>
        /// An 8-bit unsigned integer, specifies glyph and control flags.   
        /// </summary>
        public byte flags;

        /// <summary>
        /// An optional 16-bit unsigned integer, specifies the position in the Decompressor Glyph Storage ADM element for the current glyph.    
        /// </summary>
        public ushort glyphIndex;

        /// <summary>
        /// An 8-bit unsigned integer, specifies the sequencing of the stream.     
        /// </summary>
        public byte seqNumber;

        // Below are private varialbles.

        /// <summary>
        /// Contains bitmap data encoded using ClearCodec compression techniques.     
        /// </summary>
        CLEARCODEC_COMPOSITE_PAYLOAD compPayload;

        /// <summary>
        /// Residual layer bitmap.
        /// </summary>
        Bitmap resBmp;

        /// <summary>
        /// Band layer bitmap and position, can be multiple.  
        /// </summary>
        Dictionary<ClearCodec_RECT16, Bitmap> bandDict;

        /// <summary>
        /// subcodec layer bitmap, position and subcodecID, can be multiple     
        /// </summary>
        Dictionary<ClearCodec_RECT16, BMP_INFO> subcodecDict;

        /// <summary>
        /// An 16-bit unsigned integer, specifies the test type of clearcodec stream     
        /// </summary>
        public RdpegfxNegativeTypes ccTestType;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name = "flag">Specifies glyph and control flags.</param>
        /// <param name = "glyphIdx">Specifies the position in the Decompressor Glyph Storage ADM element for the current glyph.</param>
        public ClearCodec_BitmapStream(byte flag, ushort glyphIdx)
        {
            this.flags = flag;
            this.glyphIndex = glyphIdx;

            this.resBmp = null;
            this.bandDict = new Dictionary<ClearCodec_RECT16, Bitmap>();
            this.subcodecDict = new Dictionary<ClearCodec_RECT16, BMP_INFO>();
            
        }

        /// <summary>
        /// Set test type.
        /// </summary>
        public void SetTestType(RdpegfxNegativeTypes type)
        {
            ccTestType = type;
        }

        /// <summary>
        /// Load a residual layer bitmap into clearcode encoder.   
        /// </summary>
        /// <param name = "resBitmap"> specifies a residual layer image </param>
        public void LoadResidualBitmap(Bitmap resBitmap)
        {
            resBmp = resBitmap;
        }

        /// <summary>
        /// Load a residual layer bitmap into clearcode encoder.   
        /// </summary>
        /// <param name = "bandBitmap"> specifies a band layer image </param>
        /// <param name = "pos"> specifies position of band layer image, relative to residual layer image </param>
        public void LoadBandBitmap(Bitmap bandBitmap, RDPGFX_POINT16 pos)
        {
            if (bandBitmap != null)
            {
                ClearCodec_RECT16 bRect = new ClearCodec_RECT16();
                bRect.left = pos.x;
                bRect.top = pos.y;
                bRect.right = (ushort)(pos.x + bandBitmap.Width);
                bRect.bottom = (ushort)(pos.y + bandBitmap.Height);
                this.bandDict.Add(bRect, bandBitmap);
            }
        }

        /// <summary>
        /// Load a residual layer bitmap into clearcode encoder.   
        /// </summary>
        /// <param name = "subcodecBitmap"> specifies a subcodec layer image </param>
        /// <param name = "pos"> specifies position of subcodec layer image, relative to residual layer image </param>
        /// <param name = "sbcID"> specifies subcodec ID to encoding subcodec layer image </param>
        public void LoadSubcodecBitmap(Bitmap subcodecBitmap, RDPGFX_POINT16 pos, CLEARCODEC_SUBCODEC_ID sbcID)
        {
            if (subcodecBitmap != null)
            {
                ClearCodec_RECT16 scRect = new ClearCodec_RECT16();
                scRect.left = pos.x;
                scRect.top = pos.y;
                scRect.right = (ushort)(pos.x + subcodecBitmap.Width);
                scRect.bottom = (ushort)(pos.x + subcodecBitmap.Height);
                BMP_INFO bmp_info = new BMP_INFO();
                bmp_info.bmp = subcodecBitmap;
                bmp_info.scID = sbcID;
                this.subcodecDict.Add(scRect, bmp_info);
            }
        }

        /// <summary>
        /// Encode a clearcodec byte stream based on flag.   
        /// </summary>
        public byte[] Encode()
        {
            List<byte> bufList = new List<byte>();;

            bufList.AddRange(TypeMarshal.ToBytes<byte>(this.flags));
            bufList.AddRange(TypeMarshal.ToBytes<byte>(this.seqNumber));

            if ((flags & CLEARCODEC_FLAG_GLYPH_INDEX) != 0)  // Glyph_index is set.         
            {
                if ((flags & CLEARCODEC_FLAG_GLYPH_HIT) != 0)// Both glyph_index and glyph_hit are set.
                {
                    // No composite payload field, glyphIndex field exists only.
                    bufList.AddRange(TypeMarshal.ToBytes<ushort>(this.glyphIndex));

                    if (ccTestType == RdpegfxNegativeTypes.ClearCodec_GraphHitFlagSet_CompositePayloadExist)
                    {
                        bufList.AddRange(this.EncodeCompositePayload());
                    }
                }
                else
                {
                    // Both glyphIndex and composite payload exists.
                    bufList.AddRange(TypeMarshal.ToBytes<ushort>(this.glyphIndex));
                    bufList.AddRange(this.EncodeCompositePayload());
                }
            }
            else // Glyph_index is not set.
            {
                // No glyphIndex  field,  composite payload field exists only.
                bufList.AddRange(this.EncodeCompositePayload());
            }

            return bufList.ToArray();
        }

        /// <summary>
        /// Convert encoded residual layer structure into byte stream.
        /// </summary>
        /// <param name = "resData">The structure data to be converted into byte stream.</param>
        public byte[] ToBytes(CLEARCODEC_RESIDUAL_DATA resData)
        {
            if (resData.resRLSegArr == null) return null;
            List<byte> bufList = new List<byte>();

            for (int i = 0; i < resData.resRLSegArr.Count(); i++)
            {
                CLEARCODEC_RGB_RUN_SEGMENT seg = resData.resRLSegArr[i];

                bufList.AddRange(TypeMarshal.ToBytes<byte>(seg.buleValue));
                bufList.AddRange(TypeMarshal.ToBytes<byte>(seg.greenValue));
                bufList.AddRange(TypeMarshal.ToBytes<byte>(seg.redValue));

                if (ccTestType == RdpegfxNegativeTypes.ClearCodec_Residual_ZeroRunLengthFactor)
                {
                    bufList.AddRange(TypeMarshal.ToBytes<byte>(0x00));
                    break;
                }

                if (seg.rlFactor < 255) // RLF1 exists, RLF2 & 3 doesn't exit.
                {
                    bufList.AddRange(TypeMarshal.ToBytes<byte>((byte)seg.rlFactor));
                }
                else if (seg.rlFactor < 65535)  // RLF1 is 0xff, RLF2 exists, RLF3 doesn't exit.
                {
                    if (ccTestType != RdpegfxNegativeTypes.ClearCodec_Residual_RedundantRunLengthFactor2)
                    {
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(0xff));
                    }
                    else
                    {
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(0xf0));
                    }

                    if (ccTestType != RdpegfxNegativeTypes.ClearCodec_Residual_AbsentRunLengthFactor2)
                    {
                        bufList.AddRange(TypeMarshal.ToBytes<ushort>((ushort)seg.rlFactor));
                    }
                }
                else  // RLF1 is 0xff, RLF2 is 0xffff, RLF3 exists.
                {
                    if (ccTestType != RdpegfxNegativeTypes.ClearCodec_Residual_RedundantRunLengthFactor2)
                    {
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(0xff));
                    }
                    else
                    {
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(0xf0));
                    }

                    if (ccTestType != RdpegfxNegativeTypes.ClearCodec_Residual_RedundantRunLengthFactor3)
                    {
                        bufList.AddRange(TypeMarshal.ToBytes<ushort>(0xffff));
                    }
                    else
                    {
                        bufList.AddRange(TypeMarshal.ToBytes<ushort>(0xf0f0));
                        // Change RLF3 highest byte to avoid RLF3 is parsed into a color(3 bytes) plus RLF1=0
                        // when RLF1 is zero, all the left area will be filled by the color, no error triggers.
                        seg.rlFactor |= 0xf0000000;
                    }

                    if (ccTestType != RdpegfxNegativeTypes.ClearCodec_Residual_AbsentRunLengthFactor3)
                    {
                        bufList.AddRange(TypeMarshal.ToBytes<uint>((uint)seg.rlFactor));
                    }                    
                }
            }
            return bufList.ToArray();
        }

        /// <summary>
        /// Convert VBAR_CACHE_HIT structure into byte stream.
        /// </summary>
        /// <param name = "vbarCacheHit"> the structure data to be converted into byte stream </param>
        public byte[] ToBytes(VBAR_CACHE_HIT vbarCacheHit)
        {
            List<byte> bufList = new List<byte>();

            bufList.AddRange(TypeMarshal.ToBytes<ushort>(vbarCacheHit.vBarIndex_x));

            return bufList.ToArray();
        }

        /// <summary>
        /// Convert SHORT_VBAR_CACHE_HIT structure into byte stream.
        /// </summary>
        /// <param name = "svbarCacheHit"> The structure data to be converted into byte stream </param>
        public byte[] ToBytes(SHORT_VBAR_CACHE_HIT svbarCacheHit)
        {
            List<byte> bufList = new List<byte>();

            bufList.AddRange(TypeMarshal.ToBytes<ushort>(svbarCacheHit.shortVBarIndex_x));
            bufList.AddRange(TypeMarshal.ToBytes<byte>(svbarCacheHit.shortVBarYOn));
            return bufList.ToArray();
        }

        /// <summary>
        /// Convert SHORT_VBAR_CACHE_MISS structure into byte stream.
        /// </summary>
        /// <param name = "cacheMiss"> the structure data to be converted into byte stream </param>
        public byte[] ToBytes(SHORT_VBAR_CACHE_MISS cacheMiss)
        {
            List<byte> bufList = new List<byte>();

            bufList.AddRange(TypeMarshal.ToBytes<ushort>(cacheMiss.shortVBarYOnOff_x));

            if (ccTestType != RdpegfxNegativeTypes.ClearCodec_Band_ShortVBarCacheMiss_IncorrectPixelNumber)
            {
                if (cacheMiss.shortVBarPixels != null)
                {
                    for (int i = 0; i < cacheMiss.shortVBarPixels.Count(); i++)
                    {
                        Color_RGB pixel = cacheMiss.shortVBarPixels[i];
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(pixel.B));
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(pixel.G));
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(pixel.R));
                    }
                }
            }

            return bufList.ToArray();
        }

        /// <summary>
        /// Convert encoded band layer structure into byte stream.
        /// </summary>
        /// <param name = "bandData">The structure data to be converted into byte stream.</param>
        public byte[] ToBytes(CLEARCODEC_BAND bandData)
        {
            List<byte> bufList = new List<byte>();

            bufList.AddRange(TypeMarshal.ToBytes<ushort>(bandData.xStart));
            bufList.AddRange(TypeMarshal.ToBytes<ushort>(bandData.xEnd));
            bufList.AddRange(TypeMarshal.ToBytes<ushort>(bandData.yStart));
            bufList.AddRange(TypeMarshal.ToBytes<ushort>(bandData.yEnd));

            bufList.AddRange(TypeMarshal.ToBytes<byte>(bandData.blueBkg));
            bufList.AddRange(TypeMarshal.ToBytes<byte>(bandData.greenBkg));
            bufList.AddRange(TypeMarshal.ToBytes<byte>(bandData.redBkg));

            for (int i = 0; i < bandData.vBars.Count(); i++)
            {
                if (bandData.vBars[i].type == VBAR_TYPE.VBAR_CACHE_HIT)  // 1 bit check.
                {
                    VBAR_CACHE_HIT vbarCacheHit = bandData.vBars[i].vbarCacheHit;
                    bufList.AddRange(ToBytes(vbarCacheHit));

                    if (ccTestType == RdpegfxNegativeTypes.ClearCodec_Band_VBarCacheHit_ShortVBarPixelsExist)
                    {
                        // Add an additional pixel into encoded data.
                        Color_RGB pixel = new Color_RGB();
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(pixel.B));
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(pixel.G));
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(pixel.R));
                    }
                }
                else if (bandData.vBars[i].type == VBAR_TYPE.SHORT_VBAR_CACHE_HIT)  // 2 bits check.
                {
                    SHORT_VBAR_CACHE_HIT svbarCacheHit = bandData.vBars[i].shortVbarCacheHit;
                    bufList.AddRange(ToBytes(svbarCacheHit));

                    if (ccTestType == RdpegfxNegativeTypes.ClearCodec_Band_ShortVBarCacheHit_ShortVBarPixelsExist)
                    {
                        // add an additional pixel into encoded data
                        Color_RGB pixel = new Color_RGB();
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(pixel.B));
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(pixel.G));
                        bufList.AddRange(TypeMarshal.ToBytes<byte>(pixel.R));
                    }
                }
                else if (bandData.vBars[i].type == VBAR_TYPE.SHORT_VBAR_CACHE_MISS)  // 2 bits check.
                {
                    SHORT_VBAR_CACHE_MISS svbarCacheMiss = bandData.vBars[i].shortVbarCacheMiss;
                    bufList.AddRange(ToBytes(svbarCacheMiss));                  
                }
            }
            return bufList.ToArray();
        }

        /// <summary>
        /// Encode all the band images saved in CLEARCODEC_BAND_DATA structure into byte stream.
        /// </summary>
        /// <param name = "bandsData">The structure data to be converted into byte stream.</param>
        public byte[] ToBytes(CLEARCODEC_BAND_DATA bandsData)
        {
            List<byte> bufList = new List<byte>();
            if (bandsData.bandArr == null) return null;
            for (int i = 0; i < bandsData.bandArr.Length; i++)
            {
                bufList.AddRange(ToBytes(bandsData.bandArr[i]));
            }
            return bufList.ToArray();
        }

        /// <summary>
        /// Convert CLEARCODEC_SUBCODEC_RLEX structure into byte stream.
        /// </summary>
        /// <param name="rlex">The encoded CLEARCODEC_SUBCODEC_RLEX structure.</param>
        public byte[] ToBytes(CLEARCODEC_SUBCODEC_RLEX rlex)
        {
            List<byte> bufList = new List<byte>();

            if (ccTestType != RdpegfxNegativeTypes.ClearCodec_Band_Subcodec_IncorrectPaletteCount)
            {
                bufList.AddRange(TypeMarshal.ToBytes<byte>(rlex.paletteCount));
            }
            else
            {
                bufList.AddRange(TypeMarshal.ToBytes<byte>((byte)(rlex.paletteCount + 1)));
            }
            for (int i = 0; i < rlex.paletteEntries.Count(); i++)
            {
                bufList.AddRange(TypeMarshal.ToBytes<byte>(rlex.paletteEntries[i].B));
                bufList.AddRange(TypeMarshal.ToBytes<byte>(rlex.paletteEntries[i].G));
                bufList.AddRange(TypeMarshal.ToBytes<byte>(rlex.paletteEntries[i].R));
            }

            // Blow encode rlex segment.

            // Calculate bit number for stopIndex.
            byte bitNum = 0;
            byte count = (byte)(rlex.paletteCount - 1);

            while (count > 0)
            {
                bitNum++;
                count >>= 1;
            }

            for (int i = 0; i < rlex.segments.Count(); i++)
            {
                byte stopIdx_suiteDepth = (byte)(rlex.segments[i].stopIndex | (rlex.segments[i].suiteDepth << bitNum));
                bufList.AddRange(TypeMarshal.ToBytes<byte>(stopIdx_suiteDepth));
                if (rlex.segments[i].runLengthFactor < 0xff)
                {
                    bufList.AddRange(TypeMarshal.ToBytes<byte>((byte)rlex.segments[i].runLengthFactor));
                }
                else if (rlex.segments[i].runLengthFactor < 0xffff)
                {
                    bufList.AddRange(TypeMarshal.ToBytes<byte>((byte)0xff));
                    bufList.AddRange(TypeMarshal.ToBytes<ushort>((ushort)rlex.segments[i].runLengthFactor));
                }
                else
                {
                    bufList.AddRange(TypeMarshal.ToBytes<byte>((byte)0xff));
                    bufList.AddRange(TypeMarshal.ToBytes<ushort>((ushort)0xffff));
                    bufList.AddRange(TypeMarshal.ToBytes<uint>((uint)rlex.segments[i].runLengthFactor));
                }
            }

            return bufList.ToArray();
        }

        /// <summary>
        /// Convert RLEX_RGB_TRIPLET structure array into byte stream.
        /// </summary>
        /// <param name="bmpPixels">The encoded RLEX_RGB_TRIPLET structure Array.</param>
        public byte[] ToBytes(RLEX_RGB_TRIPLET[] bmpPixels)
        {
            List<byte> bufList = new List<byte>();
            if (bmpPixels == null) return null;

            for (int i = 0; i < bmpPixels.Count(); i++)
            {
                bufList.AddRange(TypeMarshal.ToBytes<byte>(bmpPixels[i].B));
                bufList.AddRange(TypeMarshal.ToBytes<byte>(bmpPixels[i].G));
                bufList.AddRange(TypeMarshal.ToBytes<byte>(bmpPixels[i].R));
            }

            return bufList.ToArray();
        }

        /// <summary>
        /// Convert CLEARCODEC_SUBCODEC structure into byte stream.
        /// </summary>
        /// <param name="scStruct">The encoded CLEARCODEC_SUBCODEC structure.</param>
        public byte[] ToBytes(CLEARCODEC_SUBCODEC scStruct)
        {
            List<byte> bufList = new List<byte>();

            bufList.AddRange(TypeMarshal.ToBytes<ushort>(scStruct.xStart));
            bufList.AddRange(TypeMarshal.ToBytes<ushort>(scStruct.yStart));
            bufList.AddRange(TypeMarshal.ToBytes<ushort>(scStruct.width));
            bufList.AddRange(TypeMarshal.ToBytes<ushort>(scStruct.height));

            byte[] bmpData = null;

            if (scStruct.bitmapData.subCodecId == CLEARCODEC_SUBCODEC_ID.SUBCODEC_RAW)
            {
                bmpData = ToBytes(scStruct.bitmapData.bmpPixels);
            }
            else if (scStruct.bitmapData.subCodecId == CLEARCODEC_SUBCODEC_ID.SUBCODEC_RLEX)
            {
                bmpData = ToBytes(scStruct.bitmapData.bmpRlex);
            }
            else    // NSCodec bitmap data for subcodec layer.
            {
                
            }

            // Set bitmapData byte length.
            if (bmpData != null)
            {
                scStruct.bitmapDataByteCount = (uint)bmpData.Count();
            }

            bufList.AddRange(TypeMarshal.ToBytes<uint>(scStruct.bitmapDataByteCount));
            bufList.AddRange(TypeMarshal.ToBytes<byte>((byte)scStruct.bitmapData.subCodecId));
            if (bmpData != null)
                bufList.AddRange(bmpData);

            return bufList.ToArray();
        }

        /// <summary>
        /// Convert CLEARCODEC_SUBCODEC structure into byte stream.
        /// </summary>
        /// <param name="scStruct">The encoded CLEARCODEC_SUBCODEC structure.</param>
        public byte[] ToBytes(CLEARCODEC_SUBCODEC_DATA subcodecsData)
        {
            List<byte> bufList = new List<byte>();

            if (subcodecsData.subcodecArr == null) return null;
            for (int i = 0; i < subcodecsData.subcodecArr.Count(); i++)
            {
                bufList.AddRange(ToBytes(subcodecsData.subcodecArr[i]));
            }

            return bufList.ToArray();
        }

        /// <summary>
        /// Encode the residual, band, and subcodec layer images into a bytestream.   
        /// </summary>
        public byte[] EncodeCompositePayload()
        {
            List<byte> buf = new List<byte>();

            if (resBmp != null)
            {
                CLEARCODEC_RESIDUAL_DATA resData = ClearCodecResidualEncoder.Encode(resBmp);
                compPayload.residualData = ToBytes(resData);
                if (compPayload.residualData == null)
                {
                    return null;
                }
                compPayload.residualByteCount = (uint)compPayload.residualData.Count();
            }
            else
            {
                compPayload.residualByteCount = 0;
                compPayload.residualData = null;
            }

            if (bandDict.Count() != 0)  // Band image are loaded before.
            {           
                ClearCodecBandEncoder bandencoder = ClearCodecBandEncoder.GetInstance();
                if ((this.flags & ClearCodec_BitmapStream.CLEARCODEC_FLAG_CACHE_RESET) != 0)
                    bandencoder.ResetVBarStorage();
                CLEARCODEC_BAND_DATA bandsData = bandencoder.Encode(bandDict);
                compPayload.bandData = ToBytes(bandsData);
                if (compPayload.bandData == null)
                {
                    return null;
                }
                compPayload.bandByteCount = (uint)compPayload.bandData.Count();
            }
            else
            {
                compPayload.bandByteCount = 0;
                compPayload.bandData = null;
            }

            if (subcodecDict.Count() != 0)  // Subcodec image are loaded before.
            {
                CLEARCODEC_SUBCODEC_DATA subcodecs = ClearCodecSubCodecEncoder.Encode(subcodecDict);
                compPayload.subcodecData = ToBytes(subcodecs);
                if (compPayload.subcodecData == null)
                {
                    return null;
                }
                compPayload.subcodecByteCount = (uint)compPayload.subcodecData.Count();
            }
            else
            {
                compPayload.subcodecByteCount = 0;
                compPayload.subcodecData = null;
            }

            buf.AddRange(TypeMarshal.ToBytes<uint>(compPayload.residualByteCount));
            buf.AddRange(TypeMarshal.ToBytes<uint>(compPayload.bandByteCount));
            buf.AddRange(TypeMarshal.ToBytes<uint>(compPayload.subcodecByteCount));

            if (compPayload.residualByteCount > 0)
            {
                buf.AddRange(compPayload.residualData);
            }

            if (compPayload.bandByteCount > 0)
            {
                buf.AddRange(compPayload.bandData);
            }

            if (compPayload.subcodecByteCount > 0)
            {
                buf.AddRange(compPayload.subcodecData);
            }

            return buf.ToArray();
        }


        #endregion
    }
    
}
