// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The RemoteFX encoder
    /// </summary>
    public class RemoteFXEncoder
    {
        protected const int DWT_PREC = 4;
        protected const int TileSize = 64;


        #region public Methods

        /// <summary>
        /// Encode a tile from an image.
        /// </summary>
        /// <param name="image">The input image.</param>
        /// <param name="leftOffset">The left offset of the tile.</param>
        /// <param name="topOffset">The top offset of the tile.</param>
        /// <param name="encodingContext">The encoding context.</param>
        public static void EncodeTile(Image image, int leftOffset, int topOffset, RemoteFXCodecContext encodingContext)
        {

            //Initialize the encoding context
            GetTileData(image, leftOffset, topOffset, encodingContext);

            //Do color conversion
            RGBToYCbCr(encodingContext);

            //Do three level DWT
            DWT(encodingContext);

            //Do quantiztion
            Quantization(encodingContext);

            //Do linearization
            Linearization(encodingContext);

            //ALRG encode
            RLGREncode(encodingContext);
        }

        /// <summary>
        /// Encode a tile from an image.
        /// </summary>
        /// <param name="image">The input RgbTile.</param>
        /// <param name="leftOffset">The left offset of the tile.</param>
        /// <param name="topOffset">The top offset of the tile.</param>
        /// <param name="encodingContext">The encoding context.</param>
        public static void EncodeTile(RgbTile tile, int leftOffset, int topOffset, RemoteFXCodecContext encodingContext)
        {

            //Initialize the encoding context
            encodingContext.RSet = tile.RSet;
            encodingContext.GSet = tile.GSet;
            encodingContext.BSet = tile.BSet;

            //Do color conversion
            RGBToYCbCr(encodingContext);

            //Do three level DWT
            DWT(encodingContext);

            //Do quantiztion
            Quantization(encodingContext);

            //Do linearization
            Linearization(encodingContext);

            //ALRG encode
            RLGREncode(encodingContext);
        }

        //Load a tile from image
        public static void GetTileData(Image image, int leftOffset, int topOffset, RemoteFXCodecContext encodingContext)
        {
            Bitmap bitmap = new Bitmap(image);
            encodingContext.RSet = new byte[TileSize, TileSize];
            encodingContext.GSet = new byte[TileSize, TileSize];
            encodingContext.BSet = new byte[TileSize, TileSize];

            int right = Math.Min(image.Width - 1, leftOffset + TileSize - 1);
            int bottom = Math.Min(image.Height - 1, topOffset + TileSize - 1);

            BitmapData bmpData = bitmap.LockBits(new Rectangle(leftOffset, topOffset, right - leftOffset + 1, bottom - topOffset + 1), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* cusor = (byte*)bmpData.Scan0.ToPointer();
                for (int y = topOffset; y <= bottom; y++)
                {
                    for (int x = leftOffset; x <= right; x++)
                    {
                        int tileX = x - leftOffset;
                        int tileY = y - topOffset;
                        encodingContext.BSet[tileX, tileY] = cusor[0];
                        encodingContext.GSet[tileX, tileY] = cusor[1];
                        encodingContext.RSet[tileX, tileY] = cusor[2];
                        cusor += 3;
                    }
                    cusor += (bmpData.Stride - 3 * (bmpData.Width));
                }
            }
            bitmap.UnlockBits(bmpData);
        }

        //Color coversion
        public static void RGBToYCbCr(RemoteFXCodecContext encodingContext)
        {
            encodingContext.YSet = new short[TileSize, TileSize];
            encodingContext.CbSet = new short[TileSize, TileSize];
            encodingContext.CrSet = new short[TileSize, TileSize];
            for (int x = 0; x < TileSize; x++)
            {
                for (int y = 0; y < TileSize; y++)
                {
                    short[] yuv = RGBToYCbCr(encodingContext.RSet[x, y], encodingContext.GSet[x, y], encodingContext.BSet[x, y]);
                    encodingContext.YSet[x, y] = yuv[0];
                    encodingContext.CbSet[x, y] = yuv[1];
                    encodingContext.CrSet[x, y] = yuv[2];
                }
            }
        }

        //DWT
        public static void DWT(RemoteFXCodecContext encodingContext)
        {
            DWT_RomoteFX(encodingContext.YSet);
            DWT_RomoteFX(encodingContext.CbSet);
            DWT_RomoteFX(encodingContext.CrSet);
        }

        //Quantization
        public static void Quantization(RemoteFXCodecContext encodingContext)
        {
            doQuantization_Component(encodingContext.YSet, encodingContext.CodecQuantVals[encodingContext.QuantIdxY]);
            doQuantization_Component(encodingContext.CbSet, encodingContext.CodecQuantVals[encodingContext.QuantIdxCb]);
            doQuantization_Component(encodingContext.CrSet, encodingContext.CodecQuantVals[encodingContext.QuantIdxCr]);
        }

        //Linearization
        public static void Linearization(RemoteFXCodecContext encodingContext)
        {
            linearization_Compontent(encodingContext.YSet, out encodingContext.YComponent);
            linearization_Compontent(encodingContext.CbSet, out encodingContext.CbComponent);
            linearization_Compontent(encodingContext.CrSet, out encodingContext.CrComponent);
        }

        //RLGREncode
        public static void RLGREncode(RemoteFXCodecContext encodingContext)
        {
            RLGREncoder encoder = new RLGREncoder();
            encodingContext.YData = encoder.Encode(encodingContext.YComponent, encodingContext.Mode);
            encodingContext.CbData = encoder.Encode(encodingContext.CbComponent, encodingContext.Mode);
            encodingContext.CrData = encoder.Encode(encodingContext.CrComponent, encodingContext.Mode);
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



        protected static short[] RGBToYCbCr(byte r, byte g, byte b)
        {
            short[] yuv = new short[3];

            float rF = r / 255.0f;
            float gF = g / 255.0f;
            float bF = b / 255.0f;

            float y = 0.299f * rF + 0.587f * gF + 0.114f * bF - 0.5f;
            float u = -0.168935f * rF - 0.331665f * gF + 0.50059f * bF;// + 0.5f;
            float v = 0.499813f * rF - 0.418531f * gF - 0.081282f * bF;// + 0.5f;

            //* 16 == << 4: with 4 fractional bits
            // yuv[0] = (short)(y * 255.0f * 16);
            // yuv[1] = (short)(u * 255.0f * 16);
            // yuv[2] = (short)(v * 255.0f * 16);

            yuv[0] = (short)(y * 255.0f);
            yuv[1] = (short)(u * 255.0f);
            yuv[2] = (short)(v * 255.0f);

            return yuv;
        }

        protected static void DWT_RomoteFX(short[,] data2D)
        {
            DWT_2D(data2D, 1);//level 1
            DWT_2D(data2D, 2);//level 2
            DWT_2D(data2D, 3);//level 3
        }

        /// <summary>
        /// Quantization 
        /// </summary>
        /// <param name="component">Y_Set, Cb_Set, Cr_Set</param>
        /// <param name="tsRfxCodecQuant">TS_RFX_CODEC_QUANT struct stored quantization factor</param>
        protected static void doQuantization_Component(short[,] component, TS_RFX_CODEC_QUANT tsRfxCodecQuant)
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
                doQuantization_Subband(component, left, top, right, bottom, HL_Factor[i]);

                //LH1,2,3
                top = levelSize / 2;
                left = 0;
                right = levelSize / 2 - 1;
                bottom = levelSize - 1;
                doQuantization_Subband(component, left, top, right, bottom, LH_Factor[i]);

                //HH1,2,3
                top = levelSize / 2;
                left = levelSize / 2;
                right = levelSize - 1;
                bottom = levelSize - 1;
                doQuantization_Subband(component, left, top, right, bottom, HH_Factor[i]);
            }
            //LL3
            top = 0;
            left = 0;
            right = TileSize / 8 - 1;
            bottom = TileSize / 8 - 1;
            doQuantization_Subband(component, left, top, right, bottom, LL3_Factor);
        }

        private static short quant(short input, int factor)
        {
            short output;
            // output = (short)(Math.Abs(input) >> ((factor - 6) + 4));
            output = (short)(Math.Abs(input) >> (factor - 6));
            if (input < 0) output *= -1;
            return output;
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

        protected static void linearization_Compontent(short[,] compontent, out short[] lineOutput)
        {
            //sequence: HL1, LH1, HH1, HL2, LH2, HH2, HL3, LH3, HH3, and LL3
            lineOutput = new short[TileSize * TileSize];
            int curIdx = 0;

            int top, left, right, bottom;
            short[] bandOutput;

            for (int i = 0; i <= 2; i++)
            {
                int levelSize = TileSize >> i;

                //HL
                top = 0;
                left = levelSize / 2;
                right = levelSize - 1;
                bottom = levelSize / 2 - 1;
                linearization_SubBand(compontent, left, top, right, bottom, out bandOutput);
                Array.Copy(bandOutput, 0, lineOutput, curIdx, bandOutput.Length);
                curIdx += bandOutput.Length;

                //LH
                top = levelSize / 2;
                left = 0;
                right = levelSize / 2 - 1;
                bottom = levelSize - 1;
                linearization_SubBand(compontent, left, top, right, bottom, out bandOutput);
                Array.Copy(bandOutput, 0, lineOutput, curIdx, bandOutput.Length);
                curIdx += bandOutput.Length;

                //HH
                top = levelSize / 2;
                left = levelSize / 2;
                right = levelSize - 1;
                bottom = levelSize - 1;
                linearization_SubBand(compontent, left, top, right, bottom, out bandOutput);
                Array.Copy(bandOutput, 0, lineOutput, curIdx, bandOutput.Length);
                curIdx += bandOutput.Length;
            }
            //LL
            top = 0;
            left = 0;
            right = TileSize / 8 - 1;
            bottom = TileSize / 8 - 1;
            linearization_SubBand(compontent, left, top, right, bottom, out bandOutput);
            for (int i = bandOutput.Length - 1; i > 0; i--)
            {
                bandOutput[i] = (short)(bandOutput[i] - bandOutput[i - 1]);
            }

            Array.Copy(bandOutput, 0, lineOutput, curIdx, bandOutput.Length);
            curIdx += bandOutput.Length;
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

        private static short DWT_H(short[] dataArr, int n)
        {
            //n < dataArr.length/2
            short x2n, x2n1, x2n2;
            if (n == -1)
            {
                x2n = dataArr[2];
                x2n1 = dataArr[1];
                x2n2 = dataArr[0];
            }
            else
            {
                x2n = dataArr[2 * n];
                x2n1 = dataArr[2 * n + 1];
                if (2 * n + 2 >= dataArr.Length - 1)
                {
                    x2n2 = x2n;
                }
                else
                {
                    x2n2 = dataArr[2 * n + 2];
                }
            }

            short result = (short)((x2n1 - (x2n + x2n2 + 0.0f) / 2) / 2);
            return result;
        }

        private static short DWT_L(short[] dataArr, int n)
        {
            short result = (short)(dataArr[2 * n] + (DWT_H(dataArr, n - 1) + DWT_H(dataArr, n)) / 2);
            return result;
        }

        private static void DWT(short[] dataArr)
        {
            int N = dataArr.Length / 2;
            short[] hResult = new short[N];
            short[] lResult = new short[N];
            for (int i = 0; i < N; i++)
            {
                hResult[i] = DWT_H(dataArr, i);
                lResult[i] = DWT_L(dataArr, i);
            }
            for (int i = 0; i < N; i++)
            {
                dataArr[i] = lResult[i];
                dataArr[N + i] = hResult[i];
            }
        }

        private static void DWT_2D(short[,] data2D, int level)
        {
            //level > 0
            //data2D.Length % (1<<(level - 1)) == 0
            int inScopelen = TileSize >> (level - 1);

            //Vertical DWT
            for (int x = 0; x < inScopelen; x++)
            {
                short[] col;
                getColFrom2DArr<short>(data2D, out col, x, inScopelen);
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
                getRowFrom2DArr<short>(data2D, out row, y, inScopelen);
                DWT(row);
                for (int x = 0; x < inScopelen; x++)
                {
                    data2D[x, y] = row[x];
                }
            }
        }
        
        private static void Convert2Dto1D<T>(T[,] sourceArr, out T[] targetArr, ArrayDirection direction)
        {
            int len0 = sourceArr.GetLength(0);
            int len1 = sourceArr.GetLength(1);
            int totalLen = len0 * len1;

            targetArr = new T[totalLen];
            int idx = 0;
            if (direction == ArrayDirection.Horizontal)
            {
                for (int y = 0; y < len1; y++)
                {
                    for (int x = 0; x < len0; x++)
                    {
                        targetArr[idx++] = sourceArr[x, y];
                    }
                }
            }
            else
            {
                for (int x = 0; x < len0; x++)
                {
                    for (int y = 0; y < len1; y++)
                    {
                        targetArr[idx++] = sourceArr[x, y];
                    }
                }
            }
        }

        private static void Convert1Dto2D<T>(T[] sourceArr, int xLen, int yLen, out T[,] targetArr, ArrayDirection direction)
        {
            targetArr = new T[xLen, yLen];
            int curIdx = 0;
            if (direction == ArrayDirection.Horizontal)
            {
                for (int y = 0; y < yLen; y++)
                {
                    for (int x = 0; x < xLen; x++)
                    {
                        targetArr[x, y] = sourceArr[curIdx++];
                    }
                }
            }
            else
            {
                for (int x = 0; x < xLen; x++)
                {
                    for (int y = 0; y < yLen; y++)
                    {
                        targetArr[x, y] = sourceArr[curIdx++];
                    }
                }
            }
        }

        private static void RGBToYCbCr(byte[] rSet, byte[] gSet, byte[] bSet, out short[] ySet, out short[] cbSet, out short[] crSet)
        {
            ySet = new short[rSet.Length];
            cbSet = new short[rSet.Length];
            crSet = new short[rSet.Length];

            for (int i = 0; i < rSet.Length; i++)
            {
                float r = rSet[i] / 255.0f;
                float g = gSet[i] / 255.0f;
                float b = bSet[i] / 255.0f;

                float y = 0.299f * r + 0.587f * g + 0.114f * b - 0.5f;
                float u = -0.168935f * r - 0.331665f * g + 0.50059f * b;// + 0.5f;
                float v = 0.499813f * r - 0.418531f * g - 0.081282f * b;// + 0.5f;

                //<< 4
                ySet[i] = (short)(y * 255.0f * 16);
                cbSet[i] = (short)(u * 255.0f * 16);
                crSet[i] = (short)(v * 255.0f * 16);
            }
        }
        
        #endregion
    }

}
