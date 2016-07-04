// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The RemoteeFX decoder.
    /// </summary>
    public class RemoteFXDecoder
    {
        protected const int DWT_PREC = 4;
        protected const int TileSize = 64;

        #region public methods

        /// <summary>
        /// Decode the tile data
        /// </summary>
        /// <param name="codecContext">The decoding context</param>
        /// <param name="yData">The Y data to be decoded.</param>
        /// <param name="cbData">The Cb data to be decoded.</param>
        /// <param name="crData">The Cr data to be decoded.</param>
        public static void DecodeTile(RemoteFXCodecContext codecContext, byte[] yData, byte[] cbData, byte[] crData)
        {
            //Load the encoded tile data
            FileEncodedData(codecContext, yData, cbData, crData);

            //Do ALGR decode
            RLGRDecode(codecContext);

            //Do reconstruction
            SubBandReconstruction(codecContext);

            //Dequantization
            Dequantization(codecContext);

            //Inverse DWT
            InverseDWT(codecContext);

            //Color coversion
            YCbCrToRGB(codecContext);
        }


        //Load encoded data into decoding context
        public static void FileEncodedData(RemoteFXCodecContext codecContext, byte[] yData, byte[] cbData, byte[] crData)
        {
            codecContext.YData = new byte[yData.Length];
            codecContext.CbData = new byte[cbData.Length];
            codecContext.CrData = new byte[crData.Length];
            Array.Copy(yData, codecContext.YData, yData.Length);
            Array.Copy(cbData, codecContext.CbData, cbData.Length);
            Array.Copy(crData, codecContext.CrData, crData.Length);
        }

        //RLGR decode
        public static void RLGRDecode(RemoteFXCodecContext codecContext)
        {
            RLGRDecoder decoder = new RLGRDecoder();
            int comLen = TileSize * TileSize;
            codecContext.YComponent = decoder.Decode(codecContext.YData, codecContext.Mode, comLen);
            codecContext.CbComponent = decoder.Decode(codecContext.CbData, codecContext.Mode, comLen);
            codecContext.CrComponent = decoder.Decode(codecContext.CrData, codecContext.Mode, comLen);
        }

        //Sub-Band Reconstruction
        public static void SubBandReconstruction(RemoteFXCodecContext codecContext)
        {
            reconstruction_Component(codecContext.YComponent, out codecContext.YSet);
            reconstruction_Component(codecContext.CbComponent, out codecContext.CbSet);
            reconstruction_Component(codecContext.CrComponent, out codecContext.CrSet);
        }

        //Dequantization
        public static void Dequantization(RemoteFXCodecContext codecContext)
        {
            dequantization_Component(codecContext.YSet, codecContext.CodecQuantVals[codecContext.QuantIdxY]);
            dequantization_Component(codecContext.CbSet, codecContext.CodecQuantVals[codecContext.QuantIdxCb]);
            dequantization_Component(codecContext.CrSet, codecContext.CodecQuantVals[codecContext.QuantIdxCr]);
        }

        //InverseDWT
        public static void InverseDWT(RemoteFXCodecContext codecContext)
        {
            InverseDWT_Component(codecContext.YSet);
            InverseDWT_Component(codecContext.CbSet);
            InverseDWT_Component(codecContext.CrSet);
        }

        //YCbCrToRGB
        public static void YCbCrToRGB(RemoteFXCodecContext codecContext)
        {
            YCbCrToRGB(codecContext.YSet, codecContext.CbSet, codecContext.CrSet, out codecContext.RSet, out codecContext.GSet, out codecContext.BSet);
        }


        public static void getColFrom2DArr<T>(T[,] input2D, out T[] output1D, int xIdx, int len)
        {
            output1D = new T[len];
            for (int i = 0; i < len; i++)
            {
                output1D[i] = input2D[xIdx, i];
            }
        }

        public static void getRowFrom2DArr<T>(T[,] input2D, out T[] output1D, int yIdx, int len)
        {
            output1D = new T[len];
            for (int i = 0; i < len; i++)
            {
                output1D[i] = input2D[i, yIdx];
            }
        }

        #endregion 

        #region Private Methods


        protected static void reconstruction_Component(short[] component1D, out short[,] compontent2D)
        {
            //sequence: HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            //lineOutput = new short[TileSize * TileSize];
            compontent2D = new short[TileSize, TileSize];

            int top, left, right, bottom;
            int offset = 0;

            for (int i = 0; i <= 2; i++)
            {
                int levelSize = TileSize >> i;

                //HL
                top = 0;
                left = levelSize / 2;
                right = levelSize - 1;
                bottom = levelSize / 2 - 1;
                reconstruction_SubBand(compontent2D, left, top, right, bottom, component1D, ref offset);

                //LH
                top = levelSize / 2;
                left = 0;
                right = levelSize / 2 - 1;
                bottom = levelSize - 1;
                reconstruction_SubBand(compontent2D, left, top, right, bottom, component1D, ref offset);

                //HH
                top = levelSize / 2;
                left = levelSize / 2;
                right = levelSize - 1;
                bottom = levelSize - 1;
                reconstruction_SubBand(compontent2D, left, top, right, bottom, component1D, ref offset);
            }

            //LL
            top = 0;
            left = 0;
            right = TileSize / 8 - 1;
            bottom = TileSize / 8 - 1;
            int llLen = (right - left + 1) * (bottom - top + 1);
            short[] llBand = new short[llLen];
            Array.Copy(component1D, offset, llBand, 0, llLen);
            for (int i = 1; i < llLen; i++)
            {
                llBand[i] = (short)(llBand[i - 1] + llBand[i]);
            }
            int llOffset = 0;
            reconstruction_SubBand(compontent2D, left, top, right, bottom, llBand, ref llOffset);
        }

        private static void reconstruction_SubBand(short[,] input, int left, int top, int right, int bottom, short[] bandOutput, ref int offset)
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

        protected static void dequantization_Component(short[,] compontent, TS_RFX_CODEC_QUANT tsRfxCodecQuant)
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

            int top, left, right, bottom;

            //Level 1, 2, 3
            for (int i = 0; i <= 2; i++)
            {
                int levelSize = TileSize >> i;

                //HL1,2,3
                top = 0;
                left = levelSize / 2;
                right = levelSize - 1;
                bottom = levelSize / 2 - 1;
                doDequantization_Subband(compontent, left, top, right, bottom, HL_Factor[i]);

                //LH1,2,3
                top = levelSize / 2;
                left = 0;
                right = levelSize / 2 - 1;
                bottom = levelSize - 1;
                doDequantization_Subband(compontent, left, top, right, bottom, LH_Factor[i]);

                //HH1,2,3
                top = levelSize / 2;
                left = levelSize / 2;
                right = levelSize - 1;
                bottom = levelSize - 1;
                doDequantization_Subband(compontent, left, top, right, bottom, HH_Factor[i]);
            }
            //LL3
            top = 0;
            left = 0;
            right = TileSize / 8 - 1;
            bottom = TileSize / 8 - 1;
            doDequantization_Subband(compontent, left, top, right, bottom, LL3_Factor);
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

        protected static void InverseDWT_Component(short[,] component)
        {
            //Level 3, 2, 1
            InverseDWT_2D(component, 3);
            InverseDWT_2D(component, 2);
            InverseDWT_2D(component, 1);
        }

        private static void InverseDWT_2D(short[,] data2D, int level)
        {
            //level > 0
            //data2D.Length % (1<<(level - 1)) == 0
            int inScopelen = TileSize >> (level - 1);

            //Horizontal DWT
            for (int y = 0; y < inScopelen; y++)
            {
                short[] row;
                getRowFrom2DArr<short>(data2D, out row, y, inScopelen);
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
                getColFrom2DArr<short>(data2D, out col, x, inScopelen);
                col = InverseDWT_1D(col);
                for (int y = 0; y < inScopelen; y++)
                {
                    data2D[x, y] = col[y];
                }
            }

        }
        
        private static short[] InverseDWT_1D(short[] encodedData)
        {
            int hOffset = encodedData.Length / 2;
            short[] decodedData = new short[encodedData.Length];
            for (int i = 1; 2 * i < encodedData.Length; i++)
            {
                //X[2n] = L[n] - (H[n-1] + H[n] + 1) / 2
                decodedData[2 * i] = (short)Math.Round(encodedData[i] - (encodedData[hOffset + i - 1] + encodedData[hOffset + i] + 1.0f) / 2);
            }

            for (int i = 1; 2 * i + 2 < encodedData.Length; i++)
            {
                //X[2n + 1] = 2*H[n] + (X[2n] + X[2n + 2])/2
                decodedData[2 * i + 1] = (short)Math.Round(2 * encodedData[hOffset + i] + (decodedData[2 * i] + decodedData[2 * i + 2] + 0.0f) / 2);
            }

            //Handle X[0], [1], [len-1]
            //H(-1) = H[0]
            decodedData[0] = (short)Math.Round(encodedData[0] - (encodedData[hOffset] + encodedData[hOffset] + 1.0f) / 2);
            decodedData[1] = (short)Math.Round(2 * encodedData[hOffset] + (decodedData[0] + decodedData[2] + 0.0f) / 2);
            decodedData[decodedData.Length - 1] = (short)(2 * encodedData[hOffset + hOffset - 1] + decodedData[decodedData.Length - 2]);
            return decodedData;
        }

        protected static void YCbCrToRGB(short[,] ySet, short[,] cbSet, short[,] crSet, out byte[,] rSet, out byte[,] gSet, out byte[,] bSet)
        {
            rSet = new byte[TileSize, TileSize];
            gSet = new byte[TileSize, TileSize];
            bSet = new byte[TileSize, TileSize];

            for (int x = 0; x < TileSize; x++)
            {
                for (int y = 0; y < TileSize; y++)
                {
                    byte[] rgb = yuvToRGB(ySet[x, y], cbSet[x, y], crSet[x, y]);
                    rSet[x, y] = rgb[0];
                    gSet[x, y] = rgb[1];
                    bSet[x, y] = rgb[2];
                }
            }
        }

        private static byte[] yuvToRGB(short y, short cb, short cr)
        {
            byte[] rgb = new byte[3];

            double yF = (y + 128.0f);
            double cbF = cb;
            double crF = cr;

            short r = (short)Math.Round((yF * 1.0 + cbF * 0 + crF * 1.403)); //R
            short g = (short)Math.Round((yF * 1.0 + cbF * (-0.344) + crF * (-0.714)));
            short b = (short)Math.Round((yF * 1.0 + cbF * 1.77 + crF * 0.0));

            if (r > 255) r = 255;
            if (r < 0) r = 0;
            if (g > 255) g = 255;
            if (g < 0) g = 0;
            if (b > 255) b = 255;
            if (b < 0) b = 0;

            rgb[0] = (byte)r;
            rgb[1] = (byte)g;
            rgb[2] = (byte)b;

            return rgb;
        }
        
        #endregion
    
    }

}
