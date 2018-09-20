// ------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------------

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
        /// In AutoScale, the image will be downscaled by a maximum possible integer that the scaled image have width and height not smaller than ScaleToLength 
        /// </summary>
        private const int ScaleMinLength = 384;
        /// <summary>
        /// Max scale level in MSSsim
        /// </summary>
        private const int MaxScaleLevel = 5;
        /// <summary>
        /// param Alpha 1 to 5 in MS-SSIM algorithm (index is 1-based), only Alpha 5 is used 
        /// </summary>
        private double[] Alphas = { Double.NaN, 0, 0, 0, 0, 0.1333 };
        /// <summary>
        /// param Beta 1 to 5 in MS-SSIM algorithm (index is 1-based) 
        /// </summary>
        private double[] Betas = { Double.NaN, 0.0448, 0.2856, 0.3001, 0.2363, 0.1333 };
        /// <summary>
        /// param Gamma in MS-SSIM algorithm (index is 1-based)
        /// </summary>
        private double[] Gammas = { Double.NaN, 0.0448, 0.2856, 0.3001, 0.2363, 0.1333 };

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

        public void SetParameters(double k1, double k2, double[] alphas, double[] betas, double[] gammas, double l = 255)
        {
            L = l;
            C1 = k1 * k1 * L * L;
            C2 = k2 * k2 * L * L;
            C3 = C2 * 0.5;
            Alphas = alphas;
            Betas = betas;
            Gammas = gammas;
        }

        /// <summary>
        /// Assess Mean MS-SSIM of a distorted bitmap with the reference bitmap
        /// </summary>
        /// <param name="reference">Reference bitmap</param>
        /// <param name="distorted">Distorted bitmap</param>
        /// <param name="component">Specify which components will be assessed, multiple components can be set by using bitwise-or</param>
        /// <exception cref="ArgumentNullException">Thrown when at least one param is null</exception>
        /// <exception cref="ArgumentException">Thrown when the size of the two bitmaps are not illegal for assessment(Sizes are not same, Size too small, etc)</exception>
        /// <returns>A new AssessResult object indicates the assess result in every specified components.</returns>
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
        /// Get the quality in one of the component(Y/Cb/Cr)
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
                SsimParameters ssimParams = new SsimParameters(WindowWidth, WindowHeight, C1, C2, C3, Alphas[scaleLevel], Betas[scaleLevel], Gammas[scaleLevel]);
                double intermediateQuality = new SsimCalculator(ssimParams).CalcMeanSsim(valueRef, valueDis);
                // make sure when the image is downscaled to less then 8x8 it can get a meaningful result.
                if (!Double.Equals(intermediateQuality, 0.0))
                {
                    quality *= intermediateQuality;
                }
            }
            return quality;
        }
    }
}
