// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment
{
    /// <summary>
    /// The base class of FR-IQA (Full reference based image quality assessment).
    /// </summary>
    public abstract class FRIQAIndexBase
    {
        /// <summary>
        /// Protected constructor
        /// </summary>
        protected FRIQAIndexBase()
        {
        }

        /// <summary>
        /// Y value for every pixel in reference bitmap in YCbCr color space, values are between 0.0-255.0
        /// </summary>
        protected double[,] LumaReference = null;
        /// <summary>
        /// Y value for every pixel in distorted bitmap in YCbCr color space, values are between 0.0-255.0
        /// </summary>
        protected double[,] LumaDistorted = null;
        /// <summary>
        /// Cb value for every pixel in reference bitmap in YCbCr color space, values are between 0.0-255.0
        /// </summary>
        protected double[,] CbReference = null;
        /// <summary>
        /// Cb value for every pixel in distorted bitmap in YCbCr color space, values are between 0.0-255.0
        /// </summary>
        protected double[,] CbDistorted = null;
        /// <summary>
        /// Cr value for every pixel in reference bitmap in YCbCr color space, values are between 0.0-255.0
        /// </summary>
        protected double[,] CrReference = null;
        /// <summary>
        /// Cr value for every pixel in distorted bitmap in YCbCr color space, values are between 0.0-255.0
        /// </summary>
        protected double[,] CrDistorted = null;

        /// <summary>
        /// Indicate the name of the index
        /// </summary>
        public abstract string IndexName { get; }

        /// <summary>
        /// Check if the two bitmaps are valid for the specified index.
        /// </summary>
        /// <param name="reference">Reference bitmap</param>
        /// <param name="distorted">Distorted bitmap</param>
        /// <exception cref="ArgumentNullException">Thrown when at least one param is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the bitmaps don't fit the assess precondition of the specified index.</exception>
        protected abstract void CheckException(Bitmap reference, Bitmap distorted);

        /// <summary>
        /// Assess the quality of a distorted bitmap with the reference bitmap
        /// </summary>
        /// <param name="reference">Reference bitmap</param>
        /// <param name="distorted">Distorted bitmap</param>
        /// <param name="component">Specify which components will be assessed, multiple components can be set by using bitwise-or</param>
        /// <exception cref="ArgumentNullException">Thrown when at least one param is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the bitmaps don't fit the assess precondition of the specified index.</exception>
        /// <returns>A new AssessResult object indicates the assess results in every specified component.</returns> 
        public abstract AssessResult Assess(Bitmap reference, Bitmap distorted, UseComponent component);

        /// <summary>
        /// Check exceptions and initialize the variables before measure.
        /// </summary>
        /// <param name="reference">Reference bitmap</param>
        /// <param name="distorted">Distorted bitmap</param>
        /// <exception cref="ArgumentNullException">Thrown when at least one param is null</exception>
        /// <exception cref="ArgumentException">Thrown when the size of two bitmaps is invalid</exception>
        protected void InitBitmaps(Bitmap reference, Bitmap distorted)
        {
            CheckException(reference, distorted);
            
            GetValues(reference, ref LumaReference, ref CbReference, ref CrReference);
            GetValues(distorted, ref LumaDistorted, ref CbDistorted, ref CrDistorted);
        }

        /// <summary>
        /// Get YCbCr value for every pixel of a bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="luma">Output Y component</param>
        /// <param name="cb">Output Cb component</param>
        /// <param name="cr">Output Cr component</param>
        private void GetValues(Bitmap bitmap, ref double[,] luma, ref double[,] cb, ref double[,] cr)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            luma = new double[width, height];
            cb = new double[width, height];
            cr = new double[width, height];
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            IntPtr ptr = bitmapData.Scan0;
            int stride = bitmapData.Stride;
            byte[] linebytes = new byte[Math.Abs(stride)];
            for (int y = 0; y < height; y++)
            {
                Marshal.Copy(ptr + stride * y, linebytes, 0, linebytes.Length);
                for (int x = 0; x < width; x++)
                {
                    //Strangely the color order in linebytes is B,G,R
                    int red = linebytes[3 * x + 2];
                    int green = linebytes[3 * x + 1];
                    int blue = linebytes[3 * x];
                    //Rec.601 color space conversation
                    luma[x, y] = 0.2989 * red + 0.5870 * green + 0.1140 * blue;
                    cb[x, y] = 127.5 - 0.1687 * red - 0.3312 * green + 0.5000 * blue;
                    cr[x, y] = 127.5 + 0.5000 * red - 0.4186 * green - 0.0813 * blue;
                }
            }
            bitmap.UnlockBits(bitmapData);
        }
    }

    /// <summary>
    /// Enum for the use of specifying which component will be assessed in the IQA index.
    /// Multiple components can be chosen by bitwise or them.
    /// </summary>
    [Flags]
    public enum UseComponent
    {
        Luma = 0x1,
        Cb = 0x2,
        Cr = 0x4
    }

    /// <summary>
    /// Store the result of the assessment of an IQA index in three components.
    /// Getting the result of a component by calling GetY, GetCb or GetCr.
    /// </summary>
    public class AssessResult
    {
        /// <summary>
        /// Storing the result in Luma component
        /// </summary>
        private double? luma;
        /// <summary>
        /// Storing the result in Cb component
        /// </summary>
        private double? cb;
        /// <summary>
        /// Storing the result in Cr component
        /// </summary>
        private double? cr;

        /// <summary>
        /// Assess result in the Luma component.
        /// </summary>
        public double Luma
        {
            get
            {
                if (this.luma == null)
                {
                    throw new InvalidOperationException("Cannot get result in Luma component, the assessment hasn't measured on this component.");
                }
                return this.luma.Value;
            }

            set
            {
                this.luma = value;
            }
        }

        /// <summary>
        /// Assess result in the Cb component
        /// </summary>
        public double Cb
        {
            get
            {
                if (this.cb == null)
                {
                    throw new InvalidOperationException("Cannot get result in Cb component, the assessment hasn't measured on this component.");
                }
                return this.cb.Value;
            }

            set
            {
                this.cb = value;
            }
        }

        /// <summary>
        /// Assess result in the Cr component
        /// </summary>
        public double Cr
        {
            get
            {
                if (this.cr == null)
                {
                    throw new InvalidOperationException("Cannot get result in Cr component, the assessment hasn't measured on this component.");
                }
                return this.cr.Value;
            }

            set
            {
                this.cr = value;
            }
        }
                

        /// <summary>
        /// Constructor
        /// </summary>
        public AssessResult()
        {
            luma = null;
            cb = null;
            cr = null;
        }
    }
}
