// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using System.Collections;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The RemoteFX Progressive Decoder
    /// </summary>
    public class RfxProgressiveDecoder
    {
        /// <summary>
        /// Decode an encoded tile
        /// </summary>
        /// <param name="enTile">Represents an encoded tile.</param>
        /// <param name="tState">The context state of the tile that going to be decoded.</param>
        public static void DecodeTile(EncodedTile enTile, TileState tState)
        {
            RfxProgressiveCodecContext codecContext = new RfxProgressiveCodecContext(
                enTile.CodecQuantVals,
                enTile.QuantIdxY, 
                enTile.QuantIdxCb, 
                enTile.QuantIdxCr,
                enTile.DataType == EncodedTileType.Simple ? false : true, 
                enTile.IsDifferenceTile, 
                enTile.UseReduceExtrapolate);

            //RLGR/SRL Decode
            if (enTile.DataType == EncodedTileType.FirstPass || enTile.DataType == EncodedTileType.Simple)
            {   //first pass or simple
                codecContext.YData = enTile.YEncodedData;
                codecContext.CbData = enTile.CbEncodedData;
                codecContext.CrData = enTile.CrEncodedData;
                RemoteFXDecoder.RLGRDecode(codecContext);
                ComputeOriginalLL3FromDeltas(codecContext);
            }
            else
            {
                SRLDecode(codecContext, enTile, tState);
            }

            //Progressive Dequantization
            if (enTile.DataType != EncodedTileType.Simple)
            {
                ProgressiveDeQuantization(codecContext, enTile.ProgCodecQuant);
            }

            // Create a DwtTile instance for tri-state
            DwtTile triStateDwt = new DwtTile(codecContext.YComponent, codecContext.CbComponent, codecContext.CrComponent, 
                enTile.CodecQuantVals, enTile.QuantIdxY, enTile.QuantIdxCb, enTile.QuantIdxCr, enTile.UseReduceExtrapolate, enTile.ProgCodecQuant);
                
            //Set Tri-State for progressive codec
            if (enTile.DataType == EncodedTileType.FirstPass)
            {
                //DwtTile tileTriStat = SetTriState(diffDwt, enTile.UseReduceExtrapolate);
                tState.UpdateTriState(triStateDwt);
            }
            else if (enTile.DataType == EncodedTileType.UpgradePass)
            {
                DwtTile prvStat = tState.GetTriState();
                prvStat.Add(triStateDwt);
                // update ProCodecQuant
                prvStat.ProgCodecQuant = triStateDwt.ProgCodecQuant;
                tState.UpdateTriState(prvStat);
            }

            // Create another DwtTile instance for DWT Data.
            // The data in diffDwt is the same as triStateDwt, this will makesure the DWT data and tri-state not share the same DWT tile instance
            DwtTile diffDwt = new DwtTile(codecContext.YComponent, codecContext.CbComponent, codecContext.CrComponent, 
                enTile.CodecQuantVals, enTile.QuantIdxY, enTile.QuantIdxCb, enTile.QuantIdxCr, enTile.UseReduceExtrapolate, enTile.ProgCodecQuant);

            //Sum difference
            if ( enTile.IsDifferenceTile || enTile.DataType == EncodedTileType.UpgradePass)
            {
                tState.AddDwt(diffDwt);
            }
            else
            {
                tState.UpdateDwt(diffDwt);
            }
        }

        /// <summary>
        /// Decode a tile from DWT
        /// </summary>
        /// <param name="codecContext">The codec context which contains the DWT data of a tile.</param>
        public static void DecodeTileFromDwtQ(RfxProgressiveCodecContext codecContext)
        {
            //Sub Band Reconstruction
            SubBandReconstruction(codecContext);

            //De-quantization
            Dequantization(codecContext);

            //Inverse DWT
            if (codecContext.UseReduceExtrapolate)
            {
                InverseDWT(codecContext);
            }
            else
            {
                RemoteFXDecoder.InverseDWT(codecContext);
            }

            //(Y, U, V) to (R, G, B)
            RemoteFXDecoder.YCbCrToRGB(codecContext);
        }

        #region SRL Decode

        public static void SRLDecode(RfxProgressiveCodecContext codecContext, EncodedTile enTile, TileState tState)
        {
            SRLDecoder yDecoder = null;
            SRLDecoder cbDecoder = null;
            SRLDecoder crDecoder = null;

            List<short> yData = new List<short>();
            List<short> cbData = new List<short>();
            List<short> crData = new List<short>();
            DwtTile triState = tState.GetTriState();
            RFX_PROGRESSIVE_CODEC_QUANT prvProgQuant = tState.GetDwt().ProgCodecQuant;
            int nonLL3Len = RdpegfxTileUtils.ComponentElementCount - RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, enTile.UseReduceExtrapolate);

            if (enTile.YEncodedData != null)
            {
                yDecoder = new SRLDecoder(enTile.YEncodedData);
            }

            if (enTile.CbEncodedData != null)
            {
                cbDecoder = new SRLDecoder(enTile.CbEncodedData);
            }

            if (enTile.CrEncodedData != null)
            {
                crDecoder = new SRLDecoder(enTile.CrEncodedData);
            }

            BitStream yRaw = BitStream.GetFromBytes(enTile.YRawData);
            BitStream cbRaw = BitStream.GetFromBytes(enTile.CbRawData);
            BitStream crRaw = BitStream.GetFromBytes(enTile.CrRawData);

            for (int i = 0; i < RdpegfxTileUtils.ComponentElementCount; i++)
            {
                BandType_Values band = RdpegfxTileUtils.GetBandByIndex(i, enTile.UseReduceExtrapolate);

                //Y
                int curBitPos = RdpegfxTileUtils.GetBitPosFromQuant(enTile.ProgCodecQuant.yQuantValues, band);
                int prvBitPos = RdpegfxTileUtils.GetBitPosFromQuant(prvProgQuant.yQuantValues, band);
                int bitCount = prvBitPos - curBitPos;
                int sign = triState.Y_DwtQ[i];
                if (bitCount > 0)
                {
                    if (sign == 0 && i < nonLL3Len)
                    {
                        if (yDecoder != null)
                        {
                            short? decodedValue = yDecoder.DecodeOne(bitCount);
                            if (decodedValue.HasValue)
                            {
                                yData.Add(decodedValue.Value);
                            }
                            else
                            {
                                yData.Add(0);
                            }
                        }
                        else
                        {
                            yData.Add(0);
                        }
                    }
                    else
                    {
                        int output;
                        if (yRaw.ReadInt32(bitCount, out output))
                        {
                            if (sign < 0 && i < nonLL3Len) output = -output;
                            yData.Add((short)output);
                        }
                        else
                        {
                            yData.Add(0);
                        }
                    }
                }
                else
                {
                    yData.Add(0);
                }

                //Cb
                curBitPos = RdpegfxTileUtils.GetBitPosFromQuant(enTile.ProgCodecQuant.cbQuantValues, band);
                prvBitPos = RdpegfxTileUtils.GetBitPosFromQuant(prvProgQuant.cbQuantValues, band);
                bitCount = prvBitPos - curBitPos;
                sign = triState.Cb_DwtQ[i];
                if (bitCount > 0)
                {
                    if (sign == 0 && i < nonLL3Len)
                    {
                        if (cbDecoder != null)
                        {
                            short? decodedValue = cbDecoder.DecodeOne(bitCount);
                            if (decodedValue.HasValue)
                            {
                                cbData.Add(decodedValue.Value);
                            }
                            else
                            {
                                cbData.Add(0);
                            }
                        }
                        else
                        {
                            cbData.Add(0);
                        }
                    }
                    else
                    {
                        int output;
                        if (cbRaw.ReadInt32(bitCount, out output))
                        {
                            if (sign < 0 && i < nonLL3Len) output = -output;
                            cbData.Add((short)output);
                        }
                        else
                        {
                            cbData.Add(0);
                        }
                    }
                }
                else
                {
                    cbData.Add(0);
                }

                //cr
                curBitPos = RdpegfxTileUtils.GetBitPosFromQuant(enTile.ProgCodecQuant.crQuantValues, band);
                prvBitPos = RdpegfxTileUtils.GetBitPosFromQuant(prvProgQuant.crQuantValues, band);
                bitCount = prvBitPos - curBitPos;
                sign = triState.Cr_DwtQ[i];
                if (bitCount > 0)
                {
                    if (sign == 0 && i < nonLL3Len)
                    {
                        if (crDecoder != null)
                        {
                            short? decodedValue = crDecoder.DecodeOne(bitCount);
                            if (decodedValue.HasValue)
                            {
                                crData.Add(decodedValue.Value);
                            }
                            else
                            {
                                crData.Add(0);
                            }
                        }
                        else
                        {
                            crData.Add(0);
                        }
                    }
                    else
                    {
                        int output;
                        if (crRaw.ReadInt32(bitCount, out output))
                        {
                            if (sign < 0 && i < nonLL3Len) output = -output;
                            crData.Add((short)output);
                        }
                        else
                        {
                            crData.Add(0);
                        }
                    }
                }
                else
                {
                    crData.Add(0);
                }
            }

            codecContext.YComponent = yData.ToArray();
            codecContext.CbComponent = cbData.ToArray();
            codecContext.CrComponent = crData.ToArray();
        }

        #endregion

        #region Sub-Band Reconstruction
        public static void SubBandReconstruction(RfxProgressiveCodecContext codecContext)
        {
            reconstruction_Component(codecContext.YComponent, out codecContext.YSet, codecContext.UseReduceExtrapolate);
            reconstruction_Component(codecContext.CbComponent, out codecContext.CbSet, codecContext.UseReduceExtrapolate);
            reconstruction_Component(codecContext.CrComponent, out codecContext.CrSet, codecContext.UseReduceExtrapolate);
        }

        public static void ComputeOriginalLL3FromDeltas(RfxProgressiveCodecContext codecContext)
        {
            int ll3Len = RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, codecContext.UseReduceExtrapolate);
            int ll3Idx = RdpegfxTileUtils.ComponentElementCount - ll3Len;

            for (int i = ll3Idx + 1; i < RdpegfxTileUtils.ComponentElementCount; i++)
            {
                codecContext.YComponent[i] = (short)(codecContext.YComponent[i] + codecContext.YComponent[i - 1]);
                codecContext.CbComponent[i] = (short)(codecContext.CbComponent[i] + codecContext.CbComponent[i - 1]);
                codecContext.CrComponent[i] = (short)(codecContext.CrComponent[i] + codecContext.CrComponent[i - 1]);
            }

        }

        public static void reconstruction_Component(short[] component1D, out short[,] compontent2D, bool useReduceExtrapolate)
        {
            //sequence: HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            //lineOutput = new short[TileSize * TileSize];
            compontent2D = new short[RdpegfxTileUtils.TileSize, RdpegfxTileUtils.TileSize];
            BandType_Values[] bandArr = new BandType_Values[] { BandType_Values.HL1, BandType_Values.LH1, BandType_Values.HH1, BandType_Values.HL2, BandType_Values.LH2, BandType_Values.HH2, BandType_Values.HL3, BandType_Values.LH3, BandType_Values.HH3, BandType_Values.LL3 };

            int offset = 0;
            BandRect curBand;

            for (int i = 0; i < bandArr.Length; i++)
            {
                curBand = RdpegfxTileUtils.GetBandRect(bandArr[i], useReduceExtrapolate);
                reconstruction_SubBand(compontent2D, curBand.left, curBand.top, curBand.right, curBand.bottom, component1D, ref offset);
            }
        }

        public static void reconstruction_SubBand(short[,] input, int left, int top, int right, int bottom, short[] bandOutput, ref int offset)
        {
            //int totalNum = (right - left + 1) * (bottom - top + 1);
            //bandOutput = new short[totalNum];
            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    input[x, y] = bandOutput[offset++];
                }
            }
        }

        #endregion

        #region Inverse DWT

        //InverseDWT
        public static void InverseDWT(RfxProgressiveCodecContext codecContext)
        {
            InverseDWT_Component(codecContext.YSet);
            InverseDWT_Component(codecContext.CbSet);
            InverseDWT_Component(codecContext.CrSet);
        }

        protected static void InverseDWT_Component(short[,] component)
        {
            //Level 3, 2, 1
            InverseDWT_2D(component, 3);
            InverseDWT_2D(component, 2);
            InverseDWT_2D(component, 1);
        }

        private static void InverseDWT_2D(short[,] data2D, int pass)
        {
            //level > 0
            //data2D.Length % (1<<(level - 1)) == 0
            int inScopelen;
            switch (pass)
            {
                case 1:
                    {
                        // First pass
                        inScopelen = 64;
                        break;
                    }
                case 2:
                    {
                        // Second pass
                        inScopelen = 33;
                        break;
                    }
                case 3:
                    {
                        // Third pass
                        inScopelen = 17;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("DWT_2D: the parameter level should only be 1, 2, or 3.");
                    }
            }

            //Horizontal DWT
            for (int y = 0; y < inScopelen; y++)
            {
                short[] row;
               RemoteFXDecoder.getRowFrom2DArr<short>(data2D, out row, y, inScopelen);
                row = InverseDWT_1D(row);
                for (int x = 0; x < inScopelen; x++)
                {
                    data2D[x, y] = row[x];
                }
            }

            //Vertical DWT
            for (int x = 0; x < inScopelen; x++)
            {
                short[] col;
                RemoteFXDecoder.getColFrom2DArr<short>(data2D, out col, x, inScopelen);
                col = InverseDWT_1D(col);
                for (int y = 0; y < inScopelen; y++)
                {
                    data2D[x, y] = col[y];
                }
            }
        }

        public static short[] InverseDWT_1D(short[] elements)
        {
            int hOffset = elements.Length / 2 + 1;
            short[] encodedData;
            if (elements.Length == RdpegfxTileUtils.TileSize)
            {
                //first pass
                encodedData = new short[elements.Length + 1];
                Array.Copy(elements, encodedData, elements.Length);
                encodedData[elements.Length] = 0;
            }
            else
            {
                encodedData = elements;
            }
            int hLen = encodedData.Length - hOffset;
            short[] decodedData = new short[encodedData.Length];
            for (int i = 1; 2 * i < encodedData.Length - 1; i++)
            {
                //X[2n] = L[n] - (H[n-1] + H[n] + 1) / 2
                //decodedData[2 * i] = (short)Math.Round(encodedData[i] - (encodedData[hOffset + i - 1] + encodedData[hOffset + i] + 1.0f) / 2);
                decodedData[2 * i] = (short)(encodedData[i] - ((encodedData[hOffset + i - 1] + encodedData[hOffset + i] + 1) >> 1));
            }

            //decodedData[decodedData.Length - 1] = (short)Math.Round(2 * decodedData[decodedData.Length - 2] - 4 * encodedData[hOffset + hLen - 1] - decodedData[decodedData.Length - 3] + 0.0f);
            decodedData[decodedData.Length - 1] = (short)Math.Round(encodedData[hOffset - 1] - encodedData[hOffset + hLen - 1] - 0.5f);
            for (int i = 1; 2 * i + 2 < encodedData.Length; i++)
            {
                //X[2n + 1] = 2*H[n] + (X[2n] + X[2n + 2])/2
                //decodedData[2 * i + 1] = (short)Math.Round(2 * encodedData[hOffset + i] + (decodedData[2 * i] + decodedData[2 * i + 2] + 0.0f) / 2);
                decodedData[2 * i + 1] = (short)(2 * encodedData[hOffset + i] + ((decodedData[2 * i] + decodedData[2 * i + 2]) >> 1));
            }

            //Handle X[0], [1], [len-1]
            //H(-1) = H[0]
            decodedData[0] = (short)Math.Round(encodedData[0] - (encodedData[hOffset] + encodedData[hOffset] + 1.0f) / 2);
            decodedData[1] = (short)Math.Round(2 * encodedData[hOffset] + (decodedData[0] + decodedData[2] + 0.0f) / 2);
            //decodedData[decodedData.Length - 1] = (short)(2 * encodedData[hOffset + hLen - 1] + decodedData[decodedData.Length - 2]);
            //decodedData[decodedData.Length - 1] = (short)Math.Round(encodedData[hOffset - 1] - (2 * encodedData[hOffset + hLen - 1] + 1.0f) / 2);

            short[] reElements;
            if (decodedData.Length > elements.Length)
            {
                reElements = new short[elements.Length];
                Array.Copy(decodedData, reElements, reElements.Length);
            }
            else
            {
                reElements = decodedData;
            }
            return reElements;
        }

        #endregion

        #region Dequantization
        public static void Dequantization(RfxProgressiveCodecContext codecContext)
        {
            dequantization_Component(codecContext.YSet, codecContext.CodecQuantVals[codecContext.QuantIdxY], codecContext.UseReduceExtrapolate);
            dequantization_Component(codecContext.CbSet, codecContext.CodecQuantVals[codecContext.QuantIdxCb], codecContext.UseReduceExtrapolate);
            dequantization_Component(codecContext.CrSet, codecContext.CodecQuantVals[codecContext.QuantIdxCr], codecContext.UseReduceExtrapolate);
        }

        protected static void dequantization_Component(short[,] compontent, TS_RFX_CODEC_QUANT tsRfxCodecQuant, bool useReduceExtrapolate)
        {
            // Quantization factor: HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, LL3            
            Hashtable scaleValueTable = new Hashtable();
            int HL1_Factor = tsRfxCodecQuant.HL1_HH1 & 0x0f;
            int LH1_Factor = (tsRfxCodecQuant.HH2_LH1 & 0xf0) >> 4;
            int HH1_Factor = (tsRfxCodecQuant.HL1_HH1 & 0xf0) >> 4;
            int HL2_Factor = (tsRfxCodecQuant.LH2_HL2 & 0xf0) >> 4;
            int LH2_Factor = tsRfxCodecQuant.LH2_HL2 & 0x0f;
            int HH2_Factor = tsRfxCodecQuant.HH2_LH1 & 0x0f;
            int HL3_Factor = tsRfxCodecQuant.HL3_HH3 & 0x0f;
            int LH3_Factor = (tsRfxCodecQuant.LL3_LH3 & 0xf0) >> 4;
            int HH3_Factor = (tsRfxCodecQuant.HL3_HH3 & 0xf0) >> 4;
            int LL3_Factor = tsRfxCodecQuant.LL3_LH3 & 0x0f;
            int[] HL_Factor = { HL1_Factor, HL2_Factor, HL3_Factor };
            int[] LH_Factor = { LH1_Factor, LH2_Factor, LH3_Factor };
            int[] HH_Factor = { HH1_Factor, HH2_Factor, HH3_Factor };

            BandType_Values[] bandArr = new BandType_Values[] { BandType_Values.HL1, BandType_Values.LH1, BandType_Values.HH1, BandType_Values.HL2, BandType_Values.LH2, BandType_Values.HH2, BandType_Values.HL3, BandType_Values.LH3, BandType_Values.HH3, BandType_Values.LL3 };
            int[] bandFactor = new int[] { HL1_Factor, LH1_Factor, HH1_Factor, HL2_Factor, LH2_Factor, HH2_Factor, HL3_Factor, LH3_Factor, HH3_Factor, LL3_Factor };

            for (int i = 0; i < bandArr.Length; i++)
            {
                BandRect br = RdpegfxTileUtils.GetBandRect(bandArr[i], useReduceExtrapolate);
                doDequantization_Subband(compontent, br.left, br.top, br.right, br.bottom, bandFactor[i]);
            }
        }

        private static void doDequantization_Subband(short[,] input, int left, int top, int right, int bottom, int factor)
        {
            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    input[x, y] = (short)(input[x, y] << (factor - 6));//<< DWT_PREC);
                }
            }
        }
        #endregion

        #region Progressive Dequantization

        public static void ProgressiveDeQuantization(RfxProgressiveCodecContext codecContext, RFX_PROGRESSIVE_CODEC_QUANT progCodecQuant )
        {
            ProgressiveDeQuantization_Component(codecContext.YComponent,  progCodecQuant.yQuantValues, codecContext.UseReduceExtrapolate);
            ProgressiveDeQuantization_Component(codecContext.CbComponent,  progCodecQuant.cbQuantValues, codecContext.UseReduceExtrapolate);
            ProgressiveDeQuantization_Component(codecContext.CrComponent, progCodecQuant.crQuantValues, codecContext.UseReduceExtrapolate);
        }

        protected static void ProgressiveDeQuantization_Component(short[] data, RFX_COMPONMENT_CODEC_QUANT quant, bool useReduceExtrapolate)
        {
            int bitPos = 0;
            for (int i = 0; i < RdpegfxTileUtils.ComponentElementCount; i++)
            {
                BandType_Values band = RdpegfxTileUtils.GetBandByIndex(i, useReduceExtrapolate);
                bitPos = RdpegfxTileUtils.GetBitPosFromQuant(quant, band);
                data[i] = FunProgDeQ(data[i], bitPos, band);
            }
        }

        static short FunProgDeQ(short v, int bitPos, BandType_Values band)
        {
            if (band != BandType_Values.LL3)
            {
                if (v >= 0) return (short)(v << bitPos);
                return (short)-((-v) << bitPos);
            }
            else
            {
                return (short)(v << bitPos);
            }
        }
        #endregion

        static DwtTile SetTriState(DwtTile inputTile, bool useReduceExtrapolate)
        {
            int ll3Len = RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, useReduceExtrapolate);
            int ll3Idx = RdpegfxTileUtils.ComponentElementCount - ll3Len;

            DwtTile stateTile = new DwtTile(
                new short[RdpegfxTileUtils.ComponentElementCount], //y state
                new short[RdpegfxTileUtils.ComponentElementCount], //cb state
                new short[RdpegfxTileUtils.ComponentElementCount]); //cr state

            for (int i = 0; i < RdpegfxTileUtils.ComponentElementCount; i++)
            {
                if (i < ll3Idx)
                {
                    stateTile.Y_DwtQ[i] = (short)Math.Sign(inputTile.Y_DwtQ[i]);
                    stateTile.Cb_DwtQ[i] = (short)Math.Sign(inputTile.Cb_DwtQ[i]);
                    stateTile.Cr_DwtQ[i] = (short)Math.Sign(inputTile.Cr_DwtQ[i]);
                }
                else
                {
                    stateTile.Y_DwtQ[i] = 1;
                    stateTile.Cb_DwtQ[i] = 1;
                    stateTile.Cr_DwtQ[i] = 1;
                }
            }
            return stateTile;
        }
    }
}
