// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using System.Drawing;
using System.Collections;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The RemoteFX Progressive encoder
    /// </summary>
    public class RfxProgressiveEncoder
    {
        const int TileSize = 64;

        /// <summary>
        /// Encode a Tile
        /// </summary>
        /// <param name="encodingContext">The tile encoding context</param>
        /// <param name="enTileInfo">The tile to be encoded</param>
        /// <returns>A array of CompressedTile which contains the encoded tiles</returns>
        public static EncodedTile[] EncodeTile(RfxProgressiveCodecContext encodingContext, TileState enTileInfo)
        {
            EncodedTileType targetType = encodingContext.UseProgressive ? EncodedTileType.FirstPass : EncodedTileType.Simple;

            //File RGB
            FillRgbData(encodingContext, enTileInfo.GetRgb());

            //Do color conversion
            RemoteFXEncoder.RGBToYCbCr(encodingContext);

            //Do DWT
            if (encodingContext.UseReduceExtrapolate)
            {
                //Do DWT using UseReduce Extrapolate method
                DWT(encodingContext);
            }
            else
            {
                RemoteFXEncoder.DWT(encodingContext);
            }

            //Do quantiztion
            Quantization(encodingContext);

            //Do linearization (LL3 delta not computed)
            Linearization_NoLL3Delta(encodingContext);

            //Update new DWT to tile
            DwtTile dwt = new DwtTile(
                (short[])encodingContext.YComponent.Clone(),
                (short[])encodingContext.CbComponent.Clone(),
                (short[])encodingContext.CrComponent.Clone(),
                encodingContext.CodecQuantVals,
                encodingContext.QuantIdxY,
                encodingContext.QuantIdxCb,
                encodingContext.QuantIdxCr,
                encodingContext.UseReduceExtrapolate
                );
            enTileInfo.UpdateDwt(dwt);
            

            //Sub-Band Diffing
            if (encodingContext.UseDifferenceTile)
            {
                SubBandDiffing_DT(encodingContext, enTileInfo);
            }

            if (targetType == EncodedTileType.Simple)
            {
                ComputeLL3Deltas(encodingContext);

                RemoteFXEncoder.RLGREncode(encodingContext);
                EncodedTile cpTile = new EncodedTile();
                cpTile.YEncodedData = (byte[])encodingContext.YData.Clone();
                cpTile.CbEncodedData = (byte[])encodingContext.CbData.Clone();
                cpTile.CrEncodedData = (byte[])encodingContext.CrData.Clone();
                cpTile.DataType = EncodedTileType.Simple;
                cpTile.IsDifferenceTile = encodingContext.UseDifferenceTile;
                cpTile.UseReduceExtrapolate = encodingContext.UseReduceExtrapolate;
                cpTile.CodecQuantVals = encodingContext.CodecQuantVals;
                cpTile.QuantIdxY = encodingContext.QuantIdxY;
                cpTile.QuantIdxCb = encodingContext.QuantIdxCb;
                cpTile.QuantIdxCr = encodingContext.QuantIdxCr;
                cpTile.ProgCodecQuant = null;
                return new EncodedTile[] { cpTile };
            }
            else
            {
                List<EncodedTile> progCTileList = new List<EncodedTile>();
                //Init DRS, DAS
                encodingContext.DRS = new DwtTile(encodingContext.YComponent, encodingContext.CbComponent, encodingContext.CrComponent);
                encodingContext.DAS = new DwtTile(new short[RdpegfxTileUtils.ComponentElementCount], new short[RdpegfxTileUtils.ComponentElementCount], new short[RdpegfxTileUtils.ComponentElementCount]);

                #region Chunk 25, first pass

                ProgressiveQuantization(encodingContext, ProgressiveChunk_Values.kChunk_25);

                //Compute ProgQ LL3 deltas
                encodingContext.YComponent = encodingContext.ProgQ.Y_DwtQ;
                encodingContext.CbComponent = encodingContext.ProgQ.Cb_DwtQ;
                encodingContext.CrComponent = encodingContext.ProgQ.Cr_DwtQ;          

                ComputeLL3Deltas(encodingContext);

                RemoteFXEncoder.RLGREncode(encodingContext);
                EncodedTile firstPassTile = new EncodedTile();
                firstPassTile.YEncodedData = (byte[])encodingContext.YData.Clone();
                firstPassTile.CbEncodedData = (byte[])encodingContext.CbData.Clone();
                firstPassTile.CrEncodedData = (byte[])encodingContext.CrData.Clone();
                firstPassTile.DataType = EncodedTileType.FirstPass;
                firstPassTile.IsDifferenceTile = encodingContext.UseDifferenceTile;
                firstPassTile.UseReduceExtrapolate = encodingContext.UseReduceExtrapolate;
                firstPassTile.CodecQuantVals = encodingContext.CodecQuantVals;
                firstPassTile.QuantIdxY = encodingContext.QuantIdxY;
                firstPassTile.QuantIdxCb = encodingContext.QuantIdxCb;
                firstPassTile.QuantIdxCr = encodingContext.QuantIdxCr;
                firstPassTile.ProgCodecQuant = RdpegfxTileUtils.GetProgCodecQuant(ProgressiveChunk_Values.kChunk_25);
                progCTileList.Add(firstPassTile);
                encodingContext.prevProgQuant = RdpegfxTileUtils.GetProgCodecQuant(ProgressiveChunk_Values.kChunk_25);

                //Update DRS
                encodingContext.DRS.Sub(encodingContext.DTS);
                //Update DAS
                encodingContext.DAS.Add(encodingContext.DTS);
                #endregion

                #region Chunk 50,75,100, upgrade pass
                ProgressiveChunk_Values[] upgradeChunks = { ProgressiveChunk_Values.kChunk_50, ProgressiveChunk_Values.kChunk_75, ProgressiveChunk_Values.kChunk_100 };

                foreach (ProgressiveChunk_Values tChunk in upgradeChunks)
                {
                    ProgressiveQuantization(encodingContext, tChunk);

                    RFX_PROGRESSIVE_CODEC_QUANT progquant = RdpegfxTileUtils.GetProgCodecQuant(tChunk);

                    progCTileList.Add(SRLEncode(encodingContext, progquant));

                    //Update DRS
                    encodingContext.DRS.Sub(encodingContext.DTS);
                    //Update DAS
                    encodingContext.DAS.Add(encodingContext.DTS);

                    encodingContext.prevProgQuant = progquant;
                }

                return progCTileList.ToArray();

                #endregion

            }
        }

        #region File RGB Data

        static void FillRgbData(RfxProgressiveCodecContext encodingContext, RgbTile rgbTile)
        {
            encodingContext.RSet = rgbTile.RSet;
            encodingContext.GSet = rgbTile.GSet;
            encodingContext.BSet = rgbTile.BSet;
        }

        #endregion

        #region DWT

        //DWT
        internal static void DWT(RfxProgressiveCodecContext encodingContext)
        {
            DWT_Component(encodingContext.YSet);
            DWT_Component(encodingContext.CbSet);
            DWT_Component(encodingContext.CrSet);
        }

        private static short DWT_H(short[] dataArr, int n)
        {
            //n < dataArr.length/2
            short x2n, x2n1, x2n2;
            int maxIdx = dataArr.Length - 1;
            if (n == -1)
            {
                x2n = dataArr[2];
                x2n1 = dataArr[1];
                x2n2 = dataArr[0];
            }
            else
            {
                x2n = dataArr[2 * n];

                if (2 * n + 1 > maxIdx)
                {
                    x2n1 = dataArr[2 * maxIdx - (2 * n + 1)];
                }
                else
                {
                    x2n1 = dataArr[2 * n + 1];
                }

                if (2 * n + 2 >= maxIdx)
                {
                    x2n2 = dataArr[2 * maxIdx - (2 * n + 2)];
                }
                else
                {
                    x2n2 = dataArr[2 * n + 2];
                }
            }

            //short result = (short)((x2n1 - (x2n + x2n2 + 0.0f) / 2) / 2);
            short result = (short)((x2n1 - (((x2n + x2n2) >> 1))) >> 1);
            return result;
        }

        private static short DWT_L(short[] dataArr, int n)
        {
            //short result = (short)(dataArr[2 * n] + (DWT_H(dataArr, n - 1) + DWT_H(dataArr, n)) / 2);
            short result = (short)(dataArr[2 * n] + ((DWT_H(dataArr, n - 1) + DWT_H(dataArr, n)) >> 1));
            return result;
        }

        public static void DWT(short[] dataArr)
        {
            int N = dataArr.Length / 2;
            int hLen, lLen;
            short[] data2Dwt;
            lLen = dataArr.Length / 2 + 1;
            hLen = dataArr.Length - lLen;

            short[] hResult = new short[hLen];
            short[] lResult = new short[lLen];

            if (dataArr.Length == TileSize)
            {
                //First pass
                data2Dwt = new short[TileSize + 1];
                Array.Copy(dataArr, data2Dwt, TileSize);
                data2Dwt[TileSize] = (short)(2 * data2Dwt[TileSize - 1] - data2Dwt[TileSize - 2]);
            }
            else
            {
                //Second and Third pass
                data2Dwt = dataArr;
            }

            for (int i = 0; i < hLen; i++)
            {
                hResult[i] = DWT_H(data2Dwt, i);
            }

            for (int i = 0; i < lLen; i++)
            {
                lResult[i] = DWT_L(data2Dwt, i);
            }

            for (int i = 0; i < lLen; i++)
            {
                dataArr[i] = lResult[i];
            }

            for (int i = 0; i < hLen; i++)
            {
                dataArr[lLen + i] = hResult[i];
            }
        }

        private static void DWT_Pass(short[,] data2D, int pass)
        {
            //level > 0
            //data2D.Length % (1<<(level - 1)) == 0
            //int inScopelen = TileSize >> (level - 1);
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

            //Vertical DWT
            for (int x = 0; x < inScopelen; x++)
            {
                short[] col;
                RemoteFXEncoder.getColFrom2DArr<short>(data2D, out col, x, inScopelen);
                DWT(col);
                for (int y = 0; y < inScopelen; y++)
                {
                    data2D[x, y] = col[y];
                }
            }
            //Horizontal DWT
            for (int y = 0; y < inScopelen; y++)
            {
                short[] row;
                RemoteFXEncoder.getRowFrom2DArr<short>(data2D, out row, y, inScopelen);
                DWT(row);
                for (int x = 0; x < inScopelen; x++)
                {
                    data2D[x, y] = row[x];
                }
            }
        }

        protected static void DWT_Component(short[,] component)
        {
            DWT_Pass(component, 1);
            DWT_Pass(component, 2);
            DWT_Pass(component, 3);
        }

        #endregion

        #region Quantization

        public static void Quantization(RfxProgressiveCodecContext encodingContext)
        {
            doQuantization_Component(encodingContext.YSet, encodingContext.CodecQuantVals[encodingContext.QuantIdxY], encodingContext.UseReduceExtrapolate);
            doQuantization_Component(encodingContext.CbSet, encodingContext.CodecQuantVals[encodingContext.QuantIdxCb], encodingContext.UseReduceExtrapolate);
            doQuantization_Component(encodingContext.CrSet, encodingContext.CodecQuantVals[encodingContext.QuantIdxCr], encodingContext.UseReduceExtrapolate);
        }

        protected static void doQuantization_Component(short[,] component, TS_RFX_CODEC_QUANT tsRfxCodecQuant, bool useReduceExtrapolate)
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

            BandType_Values[] bandArr = new BandType_Values[] { BandType_Values.HL1, BandType_Values.LH1, BandType_Values.HH1, BandType_Values.HL2, BandType_Values.LH2, BandType_Values.HH2, BandType_Values.HL3, BandType_Values.LH3, BandType_Values.HH3, BandType_Values.LL3 };
            int[] bandFactor = new int[] { HL1_Factor, LH1_Factor, HH1_Factor, HL2_Factor, LH2_Factor, HH2_Factor, HL3_Factor, LH3_Factor, HH3_Factor, LL3_Factor };

            for (int i = 0; i < bandArr.Length; i++)
            {
                BandRect br = RdpegfxTileUtils.GetBandRect(bandArr[i], useReduceExtrapolate);
                doQuantization_Subband(component, br.left, br.top, br.right, br.bottom, bandFactor[i]);
            }
        }

        private static void doQuantization_Subband(short[,] input, int left, int top, int right, int bottom, int factor)
        {
            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    input[x, y] = quant(input[x, y], factor);
                }
            }
        }

        private static short quant(short input, int factor)
        {
            short output;
            output = (short)(Math.Abs(input) >> (factor - 6));
            if (input < 0) output *= -1;
            return output;
        }
        #endregion

        #region Linearization

        public static void Linearization_NoLL3Delta(RfxProgressiveCodecContext encodingContext)
        {
             linearization_Compontent(encodingContext.YSet, encodingContext.UseReduceExtrapolate, out encodingContext.YComponent);
             linearization_Compontent(encodingContext.CbSet, encodingContext.UseReduceExtrapolate, out encodingContext.CbComponent);
             linearization_Compontent(encodingContext.CrSet, encodingContext.UseReduceExtrapolate, out encodingContext.CrComponent);
        }

        public static void ComputeLL3Deltas(RfxProgressiveCodecContext encodingContext)
        {
            int ll3Len = RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, encodingContext.UseReduceExtrapolate);
            int ll3Idx = RdpegfxTileUtils.ComponentElementCount - ll3Len;

            //for (int i = ll3Idx + 1; i < TileUtils.ComponentElementCount; i++)
            for (int i = RdpegfxTileUtils.ComponentElementCount - 1; i >= ll3Idx + 1; i--)
            {
                encodingContext.YComponent[i] = (short)(encodingContext.YComponent[i] - encodingContext.YComponent[i - 1]);
                encodingContext.CbComponent[i] = (short)(encodingContext.CbComponent[i] - encodingContext.CbComponent[i - 1]);
                encodingContext.CrComponent[i] = (short)(encodingContext.CrComponent[i] - encodingContext.CrComponent[i - 1]);
            }

        }

        protected static void linearization_Compontent(short[,] compontent, bool useReduceExtrapolate, out short[] lineOutput)
        {
            //sequence: HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            lineOutput = new short[TileSize * TileSize];
            int curIdx = 0;

            BandType_Values[] bandArr = new BandType_Values[] { BandType_Values.HL1, BandType_Values.LH1, BandType_Values.HH1, BandType_Values.HL2, BandType_Values.LH2, BandType_Values.HH2, BandType_Values.HL3, BandType_Values.LH3, BandType_Values.HH3, BandType_Values.LL3 };
            short[] bandOutput;
            BandRect curBand;

            for (int i = 0; i < bandArr.Length; i++)
            {
                curBand = RdpegfxTileUtils.GetBandRect(bandArr[i], useReduceExtrapolate);
                linearization_SubBand(compontent, curBand.left, curBand.top, curBand.right, curBand.bottom, out bandOutput);
                Array.Copy(bandOutput, 0, lineOutput, curIdx, bandOutput.Length);
                curIdx += bandOutput.Length;
            }
            
        }

        private static void linearization_SubBand(short[,] input, int left, int top, int right, int bottom, out short[] bandOutput)
        {
            int totalNum = (right - left + 1) * (bottom - top + 1);
            bandOutput = new short[totalNum];
            int curIdx = 0;
            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    bandOutput[curIdx++] = input[x, y];
                }
            }
        }



        #endregion

        #region SRL Encoder

        //SRLEncode
        public static EncodedTile SRLEncode(RfxProgressiveCodecContext encodingContext, Rdpegfx.RFX_PROGRESSIVE_CODEC_QUANT progQuant)
        {
            SRLEncoder encoder = new SRLEncoder();

            List<short> yDataToSrl = new List<short>();
            List<short> cbDataToSrl = new List<short>();
            List<short> crDataToSrl = new List<short>();

            List<int> yDataToSrlBitLen = new List<int>();
            List<int> cbDataToSrlBitLen = new List<int>();
            List<int> crDataToSrlBitLen = new List<int>();

            BitStream yRawBitStream = new BitStream();
            BitStream cbRawBitStream = new BitStream();
            BitStream crRawBitStream = new BitStream();

            int nonLL3Len = RdpegfxTileUtils.ComponentElementCount - RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, encodingContext.UseReduceExtrapolate);
            Rdpegfx.RFX_PROGRESSIVE_CODEC_QUANT prevProgQuant = encodingContext.prevProgQuant;
            Rdpegfx.RFX_PROGRESSIVE_CODEC_QUANT curProgQuant = progQuant;

            for (int i = 0; i < RdpegfxTileUtils.ComponentElementCount; i++)
            {
                BandType_Values band = RdpegfxTileUtils.GetBandByIndex(i, encodingContext.UseReduceExtrapolate);

                int targetBitPos = RdpegfxTileUtils.GetBitPosFromQuant(curProgQuant.yQuantValues, band);
                int prevBitPos = RdpegfxTileUtils.GetBitPosFromQuant(prevProgQuant.yQuantValues, band);
                int bitCount = prevBitPos - targetBitPos;
                if (bitCount > 0)
                {
                    if (encodingContext.DAS.Y_DwtQ[i] == 0 && i < nonLL3Len)
                    {
                        yDataToSrl.Add(encodingContext.ProgQ.Y_DwtQ[i]);
                        yDataToSrlBitLen.Add(bitCount);
                    }
                    else
                    {
                        //Add raw data
                        yRawBitStream.WriteBits(bitCount, Math.Abs(encodingContext.ProgQ.Y_DwtQ[i]));
                    }
                }

                targetBitPos = RdpegfxTileUtils.GetBitPosFromQuant(curProgQuant.cbQuantValues, band);
                prevBitPos = RdpegfxTileUtils.GetBitPosFromQuant(prevProgQuant.cbQuantValues, band);
                bitCount = prevBitPos - targetBitPos;
                if (bitCount > 0)
                {
                    if (encodingContext.DAS.Cb_DwtQ[i] == 0 && i < nonLL3Len)
                    {
                        cbDataToSrl.Add(encodingContext.ProgQ.Cb_DwtQ[i]);
                        cbDataToSrlBitLen.Add(bitCount);
                    }
                    else
                    {
                        //Add raw data
                        cbRawBitStream.WriteBits(bitCount, Math.Abs(encodingContext.ProgQ.Cb_DwtQ[i]));
                    }
                }

                targetBitPos = RdpegfxTileUtils.GetBitPosFromQuant(curProgQuant.crQuantValues, band);
                prevBitPos = RdpegfxTileUtils.GetBitPosFromQuant(curProgQuant.crQuantValues, band);
                bitCount = prevBitPos - targetBitPos;
                if (bitCount > 0)
                {
                    if (encodingContext.DAS.Cr_DwtQ[i] == 0 && i < nonLL3Len)
                    {
                        crDataToSrl.Add(encodingContext.ProgQ.Cr_DwtQ[i]);
                        crDataToSrlBitLen.Add(bitCount);
                    }
                    else
                    {
                        //Add raw data
                        crRawBitStream.WriteBits(bitCount, Math.Abs(encodingContext.ProgQ.Cr_DwtQ[i]));
                    }
                }
            }

            encodingContext.YData = encoder.Encode(yDataToSrl.ToArray(), yDataToSrlBitLen.ToArray());
            encodingContext.CbData = encoder.Encode(cbDataToSrl.ToArray(), cbDataToSrlBitLen.ToArray());
            encodingContext.CrData = encoder.Encode(crDataToSrl.ToArray(), crDataToSrlBitLen.ToArray());

            EncodedTile ugTile = new EncodedTile();
            ugTile.YEncodedData = (byte[])encodingContext.YData.Clone();
            ugTile.CbEncodedData = (byte[])encodingContext.CbData.Clone();
            ugTile.CrEncodedData = (byte[])encodingContext.CrData.Clone();
            ugTile.YRawData = yRawBitStream.ToBytes();
            ugTile.CbRawData = cbRawBitStream.ToBytes();
            ugTile.CrRawData = crRawBitStream.ToBytes();
            ugTile.DataType = EncodedTileType.UpgradePass;
            ugTile.IsDifferenceTile = encodingContext.UseDifferenceTile;
            ugTile.UseReduceExtrapolate = encodingContext.UseReduceExtrapolate;
            ugTile.CodecQuantVals = encodingContext.CodecQuantVals;
            ugTile.QuantIdxY = encodingContext.QuantIdxY;
            ugTile.QuantIdxCb = encodingContext.QuantIdxCb;
            ugTile.QuantIdxCr = encodingContext.QuantIdxCr;
            ugTile.ProgCodecQuant = curProgQuant;

            return ugTile;
            
        }

        #endregion

        #region Sub-Band Diffing
        //Compute the difference tile dwt
        public static void SubBandDiffing_DT(RfxProgressiveCodecContext encodingContext, TileState enTileInfo)
        {
            if (encodingContext.UseDifferenceTile)
            {
                DwtTile oldDwt = enTileInfo.GetOldDwt();
                if (oldDwt != null)
                {
                    short[] yDiffDwt, cbDiffDwt, crDiffDwt;
                    int lenOfNonLL3Band = (RdpegfxTileUtils.ComponentElementCount - RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, encodingContext.UseReduceExtrapolate));// ? 81 : 64;
                    int yNewZeroCount, cbNewZeroCount, crNewZeroCount;
                    int yDiffZeroCount, cbDiffZeroCount, crDiffZeroCount;
                    yDiffDwt = RdpegfxTileUtils.SubDiffingDwt(encodingContext.YComponent, oldDwt.Y_DwtQ, lenOfNonLL3Band, out yNewZeroCount, out yDiffZeroCount);
                    cbDiffDwt = RdpegfxTileUtils.SubDiffingDwt(encodingContext.CbComponent, oldDwt.Cb_DwtQ, lenOfNonLL3Band, out cbNewZeroCount, out cbDiffZeroCount);
                    crDiffDwt = RdpegfxTileUtils.SubDiffingDwt(encodingContext.CrComponent, oldDwt.Cr_DwtQ, lenOfNonLL3Band, out crNewZeroCount, out crDiffZeroCount);
                    if ((yDiffDwt != null && cbDiffDwt != null && crDiffDwt != null) &&
                        (yNewZeroCount + cbNewZeroCount + crNewZeroCount < yDiffZeroCount + cbDiffZeroCount + crDiffZeroCount))
                    {//use difference tile
                        encodingContext.YComponent = yDiffDwt;
                        encodingContext.CbComponent = cbDiffDwt;
                        encodingContext.CrComponent = crDiffDwt;
                        return;
                    }
                }
            }
            encodingContext.UseDifferenceTile = false;//use orginal tile
        }

        #endregion

        #region Extra(Progressive) Quantization 

        public static void ProgressiveQuantization(RfxProgressiveCodecContext encodingContext, ProgressiveChunk_Values chunk)
        {
            DwtBands yBD = DwtBands.GetFromLinearizationResult(encodingContext.DRS.Y_DwtQ, encodingContext.UseReduceExtrapolate);
            DwtBands cbBD = DwtBands.GetFromLinearizationResult(encodingContext.DRS.Cb_DwtQ, encodingContext.UseReduceExtrapolate);
            DwtBands crBD = DwtBands.GetFromLinearizationResult(encodingContext.DRS.Cr_DwtQ, encodingContext.UseReduceExtrapolate);

            ProgressiveQuantization_Component(yBD, TileComponents.Y, chunk);
            ProgressiveQuantization_Component(cbBD, TileComponents.Cb, chunk);
            ProgressiveQuantization_Component(crBD, TileComponents.Cr, chunk);

            DwtTile dwtDts = new DwtTile(yBD.GetLinearizationData(), cbBD.GetLinearizationData(), crBD.GetLinearizationData());
            encodingContext.ProgQ = dwtDts;

            //Compute DTS
            encodingContext.DTS = GetDTS(encodingContext, chunk);
        }

        protected static void ProgressiveQuantization_Component(DwtBands dwt, TileComponents component, ProgressiveChunk_Values chunk)
        {
            //HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            ProgressiveQuantization_Band(dwt.HL1, BandType_Values.HL1, component, chunk);
            ProgressiveQuantization_Band(dwt.LH1, BandType_Values.LH1, component, chunk);
            ProgressiveQuantization_Band(dwt.HH1, BandType_Values.HH1, component, chunk);
            ProgressiveQuantization_Band(dwt.HL2, BandType_Values.HL2, component, chunk);
            ProgressiveQuantization_Band(dwt.LH2, BandType_Values.LH2, component, chunk);
            ProgressiveQuantization_Band(dwt.HH2, BandType_Values.HH2, component, chunk);
            ProgressiveQuantization_Band(dwt.HL3, BandType_Values.HL3, component, chunk);
            ProgressiveQuantization_Band(dwt.LH3, BandType_Values.LH3, component, chunk);
            ProgressiveQuantization_Band(dwt.HH3, BandType_Values.HH3, component, chunk);
            ProgressiveQuantization_Band(dwt.LL3, BandType_Values.LL3, component, chunk);

        }

        protected static void ProgressiveQuantization_Component(DwtBands dwt, TileComponents component, RFX_PROGRESSIVE_CODEC_QUANT quants)
        {
            //HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            ProgressiveQuantization_Band(dwt.HL1, BandType_Values.HL1, component, quants);
            ProgressiveQuantization_Band(dwt.LH1, BandType_Values.LH1, component, quants);
            ProgressiveQuantization_Band(dwt.HH1, BandType_Values.HH1, component, quants);
            ProgressiveQuantization_Band(dwt.HL2, BandType_Values.HL2, component, quants);
            ProgressiveQuantization_Band(dwt.LH2, BandType_Values.LH2, component, quants);
            ProgressiveQuantization_Band(dwt.HH2, BandType_Values.HH2, component, quants);
            ProgressiveQuantization_Band(dwt.HL3, BandType_Values.HL3, component, quants);
            ProgressiveQuantization_Band(dwt.LH3, BandType_Values.LH3, component, quants);
            ProgressiveQuantization_Band(dwt.HH3, BandType_Values.HH3, component, quants);
            ProgressiveQuantization_Band(dwt.LL3, BandType_Values.LL3, component, quants);
        }

        static void ProgressiveQuantization_Band(short[] bandData, BandType_Values band, TileComponents component, ProgressiveChunk_Values chunk)
        {
            int bitPos = RdpegfxTileUtils.GetBitPosForChunk(chunk, component, band);
            for (int i = 0; i < bandData.Length; i++)
            {
                bandData[i] = FunProgQ(bandData[i], bitPos, band);
            }

        }

        static void ProgressiveQuantization_Band(short[] bandData, BandType_Values band, TileComponents component, RFX_PROGRESSIVE_CODEC_QUANT quants)
        {
            int bitPos = RdpegfxTileUtils.GetBitPosForQuant(quants, component, band);
            for (int i = 0; i < bandData.Length; i++)
            {
                bandData[i] = FunProgQ(bandData[i], bitPos, band);
            }
        }

        static short FunProgQ(short v, int bitPos, BandType_Values band)
        {
            if (band != BandType_Values.LL3)
            {
                if (v >= 0) return (short)(v >> bitPos);
                return (short)-((-v) >> bitPos);
            }
            else
            {
                return (short)(v >> bitPos);
            }
        }

        static DwtTile GetDTS(RfxProgressiveCodecContext encodingContext, ProgressiveChunk_Values chunk)
        {
            DwtBands yBD = DwtBands.GetFromLinearizationResult(encodingContext.DRS.Y_DwtQ, encodingContext.UseReduceExtrapolate);
            DwtBands cbBD = DwtBands.GetFromLinearizationResult(encodingContext.DRS.Cb_DwtQ, encodingContext.UseReduceExtrapolate);
            DwtBands crBD = DwtBands.GetFromLinearizationResult(encodingContext.DRS.Cr_DwtQ, encodingContext.UseReduceExtrapolate);

            DTS_Component(yBD, TileComponents.Y, chunk);
            DTS_Component(cbBD, TileComponents.Cb, chunk);
            DTS_Component(crBD, TileComponents.Cr, chunk);

            DwtTile dwtDts = new DwtTile(yBD.GetLinearizationData(), cbBD.GetLinearizationData(), crBD.GetLinearizationData());
            return dwtDts;
        }

        protected static void DTS_Component(DwtBands dwt, TileComponents component, ProgressiveChunk_Values chunk)
        {
            //HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            DTS_Band(dwt.HL1, BandType_Values.HL1, component, chunk);
            DTS_Band(dwt.LH1, BandType_Values.LH1, component, chunk);
            DTS_Band(dwt.HH1, BandType_Values.HH1, component, chunk);
            DTS_Band(dwt.HL2, BandType_Values.HL2, component, chunk);
            DTS_Band(dwt.LH2, BandType_Values.LH2, component, chunk);
            DTS_Band(dwt.HH2, BandType_Values.HH2, component, chunk);
            DTS_Band(dwt.HL3, BandType_Values.HL3, component, chunk);
            DTS_Band(dwt.LH3, BandType_Values.LH3, component, chunk);
            DTS_Band(dwt.HH3, BandType_Values.HH3, component, chunk);
            DTS_Band(dwt.LL3, BandType_Values.LL3, component, chunk);

        }

        protected static void DTS_Component(DwtBands dwt, TileComponents component, RFX_PROGRESSIVE_CODEC_QUANT quants)
        {
            //HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            DTS_Band(dwt.HL1, BandType_Values.HL1, component, quants);
            DTS_Band(dwt.LH1, BandType_Values.LH1, component, quants);
            DTS_Band(dwt.HH1, BandType_Values.HH1, component, quants);
            DTS_Band(dwt.HL2, BandType_Values.HL2, component, quants);
            DTS_Band(dwt.LH2, BandType_Values.LH2, component, quants);
            DTS_Band(dwt.HH2, BandType_Values.HH2, component, quants);
            DTS_Band(dwt.HL3, BandType_Values.HL3, component, quants);
            DTS_Band(dwt.LH3, BandType_Values.LH3, component, quants);
            DTS_Band(dwt.HH3, BandType_Values.HH3, component, quants);
            DTS_Band(dwt.LL3, BandType_Values.LL3, component, quants);

        }

        static void DTS_Band(short[] bandData, BandType_Values band, TileComponents component, ProgressiveChunk_Values chunk)
        {
            int bitPos = RdpegfxTileUtils.GetBitPosForChunk(chunk, component, band);
            for (int i = 0; i < bandData.Length; i++)
            {
                bandData[i] = FunDTS(bandData[i], bitPos, band);
            }

        }

        static void DTS_Band(short[] bandData, BandType_Values band, TileComponents component, RFX_PROGRESSIVE_CODEC_QUANT quants)
        {
            int bitPos = RdpegfxTileUtils.GetBitPosForQuant(quants, component, band);
            for (int i = 0; i < bandData.Length; i++)
            {
                bandData[i] = FunDTS(bandData[i], bitPos, band);
            }

        }

        static short FunDTS(short v, int bitPos, BandType_Values band)
        {
            if (band != BandType_Values.LL3)
            {
                if (v >= 0) return (short)((v >> bitPos) << bitPos);
                return (short)-(((-v) >> bitPos) << bitPos);
            }
            else
            {
                return (short)(v >> bitPos << bitPos);
            }
        }

        #endregion

        static void SetTriState(DwtTile inputTile, bool useReduceExtrapolate)
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
                    if (inputTile.Y_DwtQ[i] == 0)
                    {
                        stateTile.Y_DwtQ[i] = 0;
                    }
                    else if (inputTile.Y_DwtQ[i] > 0)
                    {
                        stateTile.Y_DwtQ[i] = 1;
                    } if (inputTile.Y_DwtQ[i] < 0)
                    {
                        stateTile.Y_DwtQ[i] = -1;
                    }

                    if (inputTile.Cb_DwtQ[i] == 0)
                    {
                        stateTile.Cb_DwtQ[i] = 0;
                    }
                    else if (inputTile.Cb_DwtQ[i] > 0)
                    {
                        stateTile.Cb_DwtQ[i] = 1;
                    } if (inputTile.Cb_DwtQ[i] < 0)
                    {
                        stateTile.Cb_DwtQ[i] = -1;
                    }

                    if (inputTile.Cr_DwtQ[i] == 0)
                    {
                        stateTile.Cr_DwtQ[i] = 0;
                    }
                    else if (inputTile.Cr_DwtQ[i] > 0)
                    {
                        stateTile.Cr_DwtQ[i] = 1;
                    } if (inputTile.Cr_DwtQ[i] < 0)
                    {
                        stateTile.Cr_DwtQ[i] = -1;
                    }
                }
                else
                {
                    stateTile.Y_DwtQ[i] = 1;
                    stateTile.Cb_DwtQ[i] = 1;
                    stateTile.Cr_DwtQ[i] = 1;
                }
            }
        }
    }
}
