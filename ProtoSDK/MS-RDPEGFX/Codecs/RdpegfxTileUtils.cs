// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// Util class for tile operation
    /// </summary>
    public class RdpegfxTileUtils
    {
        /// <summary>
        /// The fixed tile size
        /// </summary>
        public const int TileSize = 64;

        /// <summary>
        /// The count of tile in a surface is larger than TileDiffMinCount, the surface will perform tile diff by default.
        /// </summary>
        public const int TileDiffMinCount = 4;

        /// <summary>
        /// The fixed length of elements in a tile component
        /// </summary>
        public const int ComponentElementCount = 4096;
        
        //Store the mapping of quality and codec quantity
        static int[,] QualityQuantMap = new int[4, 10]
        {  
            { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6},       // LOSSLESS          
            { 8, 8, 6, 6, 6, 6, 6, 6, 6, 6},       // HI          
            { 8, 9, 7, 7, 7, 8, 6, 6, 6, 6},       // MED          
            { 9, 10, 8, 8, 8, 9, 6, 6, 6, 7}       // LO      
      
        };

        //Store the progressive quality factors
        static int[] gQualityFactors = { 0, 20, 25, 50, 75, 100 };

        //Store the mapping between component, band and bit pos for progreseive codec
        static int[, ,] gProgressiveBitPosArray = new int[3, 6, 10] 
        { 
            // 3 components, 5 quality levels, 10 bands      
            { // Y         
                //LL  HL  LH  HH   HL  LH  HH   HL  LH  HH          
                {8, 8, 8, 8,  8, 8, 8,  8, 8, 8},  // none          
                {5, 4, 4, 7,  5, 4, 6,  4, 4, 7},  //  20%          
                {2, 3, 3, 3,  4, 4, 4,  2, 2, 2},  //  25%          
                {1, 1, 1, 1,  2, 2, 2,  1, 1, 1},  //  50%          
                {0, 1, 1, 1,  1, 1, 1,  0, 0, 0},  //  75%          
                {0, 0, 0, 0,  0, 0, 0,  0, 0, 0}   // 100%      
            },  
            { // Cb         
                //LL  HL  LH  HH   HL  LH  HH   HL  LH  HH          
                {8, 8, 8, 8,  8, 8, 8,  8, 8, 8},  // none          
                {3, 8, 8, 8,  8, 8, 8,  8, 8, 8},  //  20%          
                {2, 3, 2, 3,  4, 4, 4,  2, 3, 2},  //  25%          
                {1, 1, 1, 1,  2, 2, 2,  1, 1, 1},  //  50%          
                {0, 1, 1, 1,  2, 1, 1,  0, 0, 0},  //  75%          
                {0, 0, 0, 0,  0, 0, 0,  0, 0, 0}   // 100%      
            },  
            { // Cr         
                //LL  HL  LH  HH   HL  LH  HH   HL  LH  HH          
                {8, 8, 8, 8,  8, 8, 8,  8, 8, 8},  // none          
                {3, 8, 8, 8,  8, 8, 8,  8, 8, 8},  //  20%          
                {2, 3, 3, 3,  4, 4, 4,  2, 5, 2},  //  25%          
                {1, 1, 1, 1,  2, 2, 2,  1, 1, 0},  //  50%          
                {0, 1, 1, 1,  2, 1, 1,  1, 0, 0},  //  75%          
                {0, 0, 0, 0,  0, 0, 0,  0, 0, 0}   // 100%      
            }
        };

        //Stores the rect area for each band when Reduce-Extrapolate is used
        static int[,] BandRectMap_ReduceExtrapolate = new int[10, 4]
        {
            //left, top, right, bottom
            {0, 0, 8, 8},//LL3 = 0,
            {9, 0, 16, 8},//HL3 = 1,
            {0, 9, 8, 16},//LH3 = 2,
            {9,9,16,16}, //HH3 = 3,
            {17,0,32,16},//HL2 = 4,
            {0,17,16,32},//LH2 = 5,
            {17,17,32,32},//HH2 = 6,
            {33,0,63,32},//HL1 = 7,
            {0,33,32,63},//LH1 = 8,
            {33,33,63,63}//HH1 = 9
        };

        //Stores the rect area for each band when Reduce-Extrapolate is not used
        static int[,] BandRectMap = new int[10, 4]
        {
            //left, top, right, bottom
            {0, 0, 7, 7},//LL3 = 0,
            {8, 0, 15, 7},//HL3 = 1,
            {0, 8, 7, 15},//LH3 = 2,
            {8,8,15,15}, //HH3 = 3,
            {16,0,31,15},//HL2 = 4,
            {0,16,15,31},//LH2 = 5,
            {16,16,31,31},//HH2 = 6,
            {32,0,63,31},//HL1 = 7,
            {0,32,31,63},//LH1 = 8,
            {32,32,63,63}//HH1 = 9
        };

        //Stores the data offset and length for each band when Reduce-Extrapolate is used
        static int[,] BandIndexRange_ReduceExtrapolate = new int[10,2]{
            //HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            {0, 31*33-1},//HL1
            {31*33, 31*33 + 31*33 - 1},//LH1
            {31*33 + 31*33, 31*33 + 31*33+ 31*31 - 1},//HH1
            {31*33 + 31*33+ 31*31, 31*33 + 31*33+ 31*31 + 16*17 -1},//HL2
            {31*33 + 31*33+ 31*31 + 16*17, 2*31*33 + 31*31 + 2*16*17 - 1},//LH2
            {2*31*33 + 31*31 + 2*16*17, 2*31*33 + 31*31 + 2*16*17 + 16*16 -1},//HH2
            {2*31*33 + 31*31 + 2*16*17 + 16*16, 2*31*33 + 31*31 + 2*16*17 + 16*16 + 8*9 - 1},//HL3
            {2*31*33 + 31*31 + 2*16*17 + 16*16 + 8*9, 2*31*33 + 31*31 + 2*16*17 + 16*16 + 2*8*9 - 1},//LH3
            {2*31*33 + 31*31 + 2*16*17 + 16*16 + 2*8*9, 2*31*33 + 31*31 + 2*16*17 + 16*16 + 2*8*9 + 8*8 -1},//HH3
            {2*31*33 + 31*31 + 2*16*17 + 16*16 + 2*8*9 + 8*8, 4095} //LL3
        };

        //Stores the data offset and length for each band when Reduce-Extrapolate is not used
        static int[,] BandIndexRange = new int[10,2]{
            //HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            {0, 32*32-1},//HL1
            {32*32, 2* 32*32 - 1},//LH1
            {2* 32*32, 3* 32*32 - 1},//HH1
            {3* 32*32, 3* 32*32 + 16*16 -1},//HL2
            {3* 32*32 + 16*16, 3* 32*32 + 2*16*16 - 1},//LH2
            {3* 32*32 + 2*16*16, 3* 32*32 + 3*16*16 -1},//HH2
            {3* 32*32 + 3*16*16, 3* 32*32 + 3*16*16 + 8*8 - 1},//HL3
            {3* 32*32 + 3*16*16 + 8*8, 3* 32*32 + 3*16*16 + 2*8*8 - 1},//LH3
            {3* 32*32 + 3*16*16 + 2*8*8, 3* 32*32 + 3*16*16 + 3*8*8 -1},//HH3
            {3* 32*32 + 3*16*16 + 3*8*8, 4095} //LL3
        };

        /// <summary>
        /// Get the bit-pos for a given chunk
        /// </summary>
        /// <param name="chunk">The chunk</param>
        /// <param name="component">The compoent</param>
        /// <param name="band">The band</param>
        /// <returns>The bit-pos of the given band for the given chunk</returns>
        public static int GetBitPosForChunk(ProgressiveChunk_Values chunk, TileComponents component, BandType_Values band)
        {
            if (chunk == ProgressiveChunk_Values.kChunk_None) return 15; // no matter what the band or the component      
            else if (chunk == ProgressiveChunk_Values.kChunk_100) return 0;
            return gProgressiveBitPosArray[(int)component, (int)chunk, (int)band];
        }

        /// <summary>
        /// Get the bit-pos for a given chunk
        /// </summary>
        /// <param name="chunk">The chunk</param>
        /// <param name="component">The compoent</param>
        /// <param name="band">The band</param>
        /// <returns>The bit-pos of the given band for the given chunk</returns>
        public static int GetBitPosForQuant(RFX_PROGRESSIVE_CODEC_QUANT quants, TileComponents component, BandType_Values band)
        {
            RFX_COMPONMENT_CODEC_QUANT quantsComponet = quants.yQuantValues;
            switch (component)
            {
                case TileComponents.Y:
                    quantsComponet = quants.yQuantValues;
                    break;
                case TileComponents.Cb:
                    quantsComponet = quants.cbQuantValues;
                    break;
                case TileComponents.Cr:
                    quantsComponet = quants.crQuantValues;
                    break;
            }
            switch (band)
            {
                case BandType_Values.LL3:
                    return quantsComponet.LL3_HL3 & 0x0F;
                case BandType_Values.HL3:
                    return quantsComponet.LL3_HL3 >> 4;
                case BandType_Values.LH3:
                    return quantsComponet.LH3_HH3 & 0x0F;
                case BandType_Values.HH3:
                    return quantsComponet.LH3_HH3 >> 4;
                case BandType_Values.HL2:
                    return quantsComponet.HL2_LH2 & 0x0F;
                case BandType_Values.LH2:
                    return quantsComponet.HL2_LH2 >> 4;
                case BandType_Values.HH2:
                    return quantsComponet.HH2_HL1 & 0x0F;
                case BandType_Values.HL1:
                    return quantsComponet.HH2_HL1 >> 4;
                case BandType_Values.LH1:
                    return quantsComponet.LH1_HH1 & 0x0F;
                case BandType_Values.HH1:
                    return quantsComponet.LH1_HH1 >> 4;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Get the bit-pos from a codec quantity for a specified band
        /// </summary>
        /// <param name="quant">codec quantity</param>
        /// <param name="band">a band tye</param>
        /// <returns>The bit-pos of the given band</returns>
        public static int GetBitPosFromQuant(RFX_COMPONMENT_CODEC_QUANT quant, BandType_Values band)
        {
            switch (band)
            {
                case BandType_Values.HL1: return (quant.HH2_HL1 & 0xf0) >> 4;
                case BandType_Values.LH1: return (quant.LH1_HH1 & 0x0f);
                case BandType_Values.HH1: return (quant.LH1_HH1 & 0xf0) >> 4;
                case BandType_Values.HL2: return (quant.HL2_LH2 & 0x0f);
                case BandType_Values.LH2: return (quant.HL2_LH2 & 0xf0) >> 4;
                case BandType_Values.HH2: return (quant.HH2_HL1 & 0x0f);
                case BandType_Values.HL3: return (quant.LL3_HL3 & 0xf0) >> 4;
                case BandType_Values.LH3: return (quant.LH3_HH3 & 0x0f);
                case BandType_Values.HH3: return (quant.LH3_HH3 & 0xf0) >> 4;
                case BandType_Values.LL3: return (quant.LL3_HL3 & 0x0f);
                default: return 0;
            }
        }

        /// <summary>
        /// Get the progressive codec quantity for the specified chunk
        /// </summary>
        /// <param name="chunk">the chunk type</param>
        /// <returns>The progressive codec quant for the specified chunk</returns>
        public static RFX_PROGRESSIVE_CODEC_QUANT GetProgCodecQuant(ProgressiveChunk_Values chunk)
        {
            RFX_PROGRESSIVE_CODEC_QUANT progQuant = new RFX_PROGRESSIVE_CODEC_QUANT();
            progQuant.yQuantValues = new RFX_COMPONMENT_CODEC_QUANT();
            progQuant.cbQuantValues = new RFX_COMPONMENT_CODEC_QUANT();
            progQuant.crQuantValues = new RFX_COMPONMENT_CODEC_QUANT();

            //Y
            progQuant.yQuantValues.LL3_HL3 = (byte)(GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.LL3) | (GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.HL3) << 4));
            progQuant.yQuantValues.LH3_HH3 = (byte)(GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.LH3) | (GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.HH3) << 4));
            progQuant.yQuantValues.HL2_LH2 = (byte)(GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.HL2) | (GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.LH2) << 4));
            progQuant.yQuantValues.HH2_HL1 = (byte)(GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.HH2) | (GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.HL1) << 4));
            progQuant.yQuantValues.LH1_HH1 = (byte)(GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.LH1) | (GetBitPosForChunk(chunk, TileComponents.Y, BandType_Values.HH1) << 4));

            //Cb
            progQuant.cbQuantValues.LL3_HL3 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.LL3) | (GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.HL3) << 4));
            progQuant.cbQuantValues.LH3_HH3 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.LH3) | (GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.HH3) << 4));
            progQuant.cbQuantValues.HL2_LH2 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.HL2) | (GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.LH2) << 4));
            progQuant.cbQuantValues.HH2_HL1 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.HH2) | (GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.HL1) << 4));
            progQuant.cbQuantValues.LH1_HH1 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.LH1) | (GetBitPosForChunk(chunk, TileComponents.Cb, BandType_Values.HH1) << 4));

            //Cr
            progQuant.crQuantValues.LL3_HL3 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.LL3) | (GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.HL3) << 4));
            progQuant.crQuantValues.LH3_HH3 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.LH3) | (GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.HH3) << 4));
            progQuant.crQuantValues.HL2_LH2 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.HL2) | (GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.LH2) << 4));
            progQuant.crQuantValues.HH2_HL1 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.HH2) | (GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.HL1) << 4));
            progQuant.crQuantValues.LH1_HH1 = (byte)(GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.LH1) | (GetBitPosForChunk(chunk, TileComponents.Cr, BandType_Values.HH1) << 4));

            return progQuant;
        }

        /// <summary>
        /// Get codec quant value for a band
        /// </summary>
        /// <param name="quality">the quality</param>
        /// <param name="band">the band</param>
        /// <returns>The codec quant value of the band</returns>
        public static int GetQuantValue(ImageQuality_Values quality, BandType_Values band)
        {
            return QualityQuantMap[(int)quality, (int)(9 - band)];
        }

        /// <summary>
        /// Get codec quant with the specified quality
        /// </summary>
        /// <param name="quality">the encoding quality</param>
        /// <returns>The codec quant</returns>
        public static TS_RFX_CODEC_QUANT GetCodecQuant(ImageQuality_Values quality)
        {
            TS_RFX_CODEC_QUANT quant = new TS_RFX_CODEC_QUANT();
            quant.LL3_LH3 = (byte)(GetQuantValue(quality, BandType_Values.LL3) | (GetQuantValue(quality, BandType_Values.LH3) << 4));
            quant.HL3_HH3  = (byte)(GetQuantValue(quality, BandType_Values.HL3) | (GetQuantValue(quality, BandType_Values.HH3) << 4));
            quant.LH2_HL2  = (byte)(GetQuantValue(quality, BandType_Values.LH2) | (GetQuantValue(quality, BandType_Values.HL2) << 4));
            quant.HH2_LH1 = (byte)(GetQuantValue(quality, BandType_Values.HH2) | (GetQuantValue(quality, BandType_Values.LH1) << 4));
            quant.HL1_HH1 = (byte)(GetQuantValue(quality, BandType_Values.HL1) | (GetQuantValue(quality, BandType_Values.HH1) << 4));
            return quant;
        }

        /// <summary>
        /// Compute the sub-diff DWT data
        /// </summary>
        /// <param name="newDwt">the new dwt data</param>
        /// <param name="oldDwt">the old dwt data</param>
        /// <param name="lenOfNonLL3">the length of elements that not in LL3 band</param>
        /// <param name="newZeroCount">the zero count of new dwt</param>
        /// <param name="diffZeroCount">the zero count of result dwt</param>
        /// <returns>the sub-diff dwt.</returns>
        public static short[] SubDiffingDwt(short[] newDwt, short[] oldDwt, int lenOfNonLL3,  out int newZeroCount,out int diffZeroCount)
        {
            diffZeroCount = 0;
            newZeroCount = 0;
            if (newDwt.Length != oldDwt.Length) return null;
            short[] diffDwt = new short[newDwt.Length];
            for (int i = 0; i < newDwt.Length; i++)
            {
                diffDwt[i] = (short)(newDwt[i] - oldDwt[i]);

                if (i < lenOfNonLL3)
                {
                    if (newDwt[i] == 0) newZeroCount++;
                    if (diffDwt[i] == 0) diffZeroCount++;
                }
            }
            return diffDwt;
        }

        /// <summary>
        /// Get the band size for a specified band
        /// </summary>
        /// <param name="band">the band type</param>
        /// <param name="useReduceExtrapolate">indicates if Reduce-Extrapolate method used.</param>
        /// <returns>The band size</returns>
        public static int GetBandSize(BandType_Values band, bool useReduceExtrapolate)
        {
            switch (band)
            {
                case BandType_Values.LL3:
                    {
                        return useReduceExtrapolate ? 9 * 9 : 8 * 8;
                    }
                case BandType_Values.LH3:
                    {
                        return useReduceExtrapolate ? 9 * 8 : 8 * 8;
                    }
                case BandType_Values.HL3:
                    {
                        return useReduceExtrapolate ? 8 * 9 : 8 * 8;
                    }
                case BandType_Values.HH3:
                    {
                        return 8 * 8;
                    }
                case BandType_Values.LH2:
                    {
                        return useReduceExtrapolate ? 16 * 17 : 16 * 16;
                    }
                case BandType_Values.HL2:
                    {
                        return useReduceExtrapolate ? 16 * 17 : 16 * 16;
                    }
                case BandType_Values.HH2:
                    {
                        return 16 * 16;
                    }
                case BandType_Values.LH1:
                    {
                        return useReduceExtrapolate ? 31 * 33 : 32 * 32;
                    }
                case BandType_Values.HL1:
                    {
                        return useReduceExtrapolate ? 31 * 33 : 32 * 32;
                    }
                case BandType_Values.HH1:
                    {
                        return useReduceExtrapolate ? 31 * 31 : 32 * 32;
                    }
                default: return 0;
            }
        }

        /// <summary>
        /// Get the Rect area of a band
        /// </summary>
        /// <param name="band">the band type</param>
        /// <param name="useReduceExtrapolate">indicates if Reduce-Extrapolate method used</param>
        /// <returns>The band rect area</returns>
        public static BandRect GetBandRect(BandType_Values band, bool useReduceExtrapolate)
        {
            BandRect br = new BandRect();
            int bandIdx = (int)band;
            if (useReduceExtrapolate)
            {
                br.left = BandRectMap_ReduceExtrapolate[bandIdx, 0];
                br.top = BandRectMap_ReduceExtrapolate[bandIdx, 1];
                br.right = BandRectMap_ReduceExtrapolate[bandIdx, 2];
                br.bottom = BandRectMap_ReduceExtrapolate[bandIdx, 3];
            }
            else
            {
                br.left = BandRectMap[bandIdx, 0];
                br.top = BandRectMap[bandIdx, 1];
                br.right = BandRectMap[bandIdx, 2];
                br.bottom = BandRectMap[bandIdx, 3];
            }
            return br;
        }

        /// <summary>
        /// Get the band type with the given element index 
        /// </summary>
        /// <param name="eleIdx">the index of an element</param>
        /// <param name="useReduceExtrapolate">indicates if Reduce-Extrapolate method used</param>
        /// <returns>The band type</returns>
        public static BandType_Values GetBandByIndex(int eleIdx, bool useReduceExtrapolate)
        {
            BandType_Values[] bandArr = new BandType_Values[] { BandType_Values.HL1, BandType_Values.LH1, BandType_Values.HH1, BandType_Values.HL2, BandType_Values.LH2, BandType_Values.HH2, BandType_Values.HL3, BandType_Values.LH3, BandType_Values.HH3, BandType_Values.LL3 };

            if (useReduceExtrapolate)
            {
                for (int i = 0; i < bandArr.Length; i++)
                {
                    if (BandIndexRange_ReduceExtrapolate[i, 0] <= eleIdx && eleIdx <= BandIndexRange_ReduceExtrapolate[i, 1])
                    {
                        return bandArr[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < bandArr.Length; i++)
                {
                    if (BandIndexRange[i, 0] <= eleIdx && eleIdx <= BandIndexRange[i, 1])
                    {
                        return bandArr[i];
                    }
                }
            }
            throw new InvalidProgramException("GetBandByIndex::eleIdx is invalid: " + eleIdx);
        }
    }


}
