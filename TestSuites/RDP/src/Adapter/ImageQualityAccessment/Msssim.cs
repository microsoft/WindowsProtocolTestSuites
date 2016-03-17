// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment
{
    /// <summary>
    /// Multi scale SSIM Index Object. Assess image quality by MS-SSIM
    /// </summary>
    public class Msssim : Ssim
    {
        /// <summary>
        /// In AutoScale, the image will be downscaled by a maximum possible integer that the scaled image has width and height not smaller than ScaleToLength 
        /// </summary>
        private const int ScaleMinLength = 384;
        /// <summary>
        /// Max scale level in MSSsim
        /// </summary>
        private const int MaxScaleLevel = 5;
        /// <summary>
        /// param Alpha 1 to 5 in MS-SSIM algorithm (index is 1-based), only Alpha 5 is used 
        /// </summary>
        private readonly double[] Alpha = { Double.NaN, 0, 0, 0, 0, 0.1333 };
        /// <summary>
        /// param Beta 1 to 5 in MS-SSIM algorithm (index is 1-based) 
        /// </summary>
        private readonly double[] Beta = { Double.NaN, 0.0448, 0.2856, 0.3001, 0.2363, 0.1333 };
        /// <summary>
        /// param Gamma in MS-SSIM algorithm (index is 1-based)
        /// </summary>
        private readonly double[] Gamma = { Double.NaN, 0.0448, 0.2856, 0.3001, 0.2363, 0.1333 };

        /// <summary>
        /// Constructor
        /// </summary>
        public Msssim()
            : base()
        {
        }

        /// <summary>
        /// Indicate the name of the index
        /// </summary>
        public override string IndexName
        {
            get { return "MS-SSIM"; }
        }

        /// <summary>
        /// Assess Mean MS-SSIM of a distorted bitmap with the reference bitmap
        /// </summary>
        /// <param name="reference">Reference bitmap</param>
        /// <param name="distorted">Distorted bitmap</param>
        /// <param name="component">Specify which components will be assessed, multiple components can be set by using bitwise-or</param>
        /// <exception cref="ArgumentNullException">Thrown when at least one param is null</exception>
        /// <exception cref="ArgumentException">Thrown when the size of the two bitmaps are not illegal for assessment (Sizes are not same, Size too small, etc.)</exception>
        /// <returns>A new AssessResult object indicates the assess results in every specified component.</returns>
        public override AssessResult Assess(Bitmap reference, Bitmap distorted, UseComponent component)
        {
            InitBitmaps(reference, distorted);
            LumaReference = Downscaler.DownscaleByKeepMinLength(LumaReference, ScaleMinLength);
            LumaDistorted = Downscaler.DownscaleByKeepMinLength(LumaDistorted, ScaleMinLength);

            AssessResult res = new AssessResult();
            if (component.HasFlag(UseComponent.Luma))
            {
                res.Luma = GetComponentQuality(LumaReference, LumaDistorted);
            }
            if (component.HasFlag(UseComponent.Cb))
            {
                res.Cb = GetComponentQuality(CbReference, CbDistorted);
            }
            if (component.HasFlag(UseComponent.Cr))
            {
                res.Cr = GetComponentQuality(CrReference, CrDistorted);
            }
            return res;
        }

        /// <summary>
        /// Get the quality in one of the components(Y/Cb/Cr)
        /// </summary>
        /// <param name="valueRef">component value of reference image</param>
        /// <param name="valueDis">component value of distorted image</param>
        /// <returns>quality</returns>
        private double GetComponentQuality(double[,] valueRef, double[,] valueDis)
        {
            double quality = 1;
            for (int scaleLevel = 1; scaleLevel <= MaxScaleLevel; scaleLevel++)
            {
                if (scaleLevel != 1)
                {
                    valueRef = Downscaler.DownscaleByFactor(valueRef, 2);
                    valueDis = Downscaler.DownscaleByFactor(valueDis, 2);
                }
                SsimParameters ssimParams = new SsimParameters(WindowWidth, WindowHeight, C1, C2, C3, Alpha[scaleLevel], Beta[scaleLevel], Gamma[scaleLevel]);
                quality *= new SsimCalculator(ssimParams).CalcMeanSsim(valueRef, valueDis);
            }
            return quality;
        }
    }
}
