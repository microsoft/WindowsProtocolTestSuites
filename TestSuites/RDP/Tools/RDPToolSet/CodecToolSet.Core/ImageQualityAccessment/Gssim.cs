// ------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment
{
    /// <summary>
    /// Gradient based SSIM Index Object. Assess image quality by Gradient based SSIM
    /// </summary>
    public class Gssim : Ssim
    {
        /// <summary>
        /// In AutoScale, the image will be downscaled by a maximum possible integer that the scaled image have width and height not smaller than ScaleToLength 
        /// </summary>
        private const int ScaleMinLength = 192;
        /// <summary>
        /// Constructor
        /// </summary>
        public Gssim()
            : base()
        {
        }

        /// <summary>
        /// Indicate the name of the index
        /// </summary>
        public override string IndexName
        {
            get { return "G-SSIM"; }
        }

        /// <summary>
        /// Assess Mean G-SSIM of a distorted bitmap with the reference bitmap
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

            SsimParameters gssimParams = new SsimParameters(WindowWidth, WindowHeight, C1, C2, C3, Alpha, Beta, Gamma);
            GssimCalculator gssimCalc = new GssimCalculator(gssimParams);
            AssessResult res = new AssessResult();
            if (component.HasFlag(UseComponent.Luma))
            {
                res.Luma = gssimCalc.CalcMeanGssim(LumaReference, LumaDistorted);
            }
            if (component.HasFlag(UseComponent.Cb))
            {
                res.Cb = gssimCalc.CalcMeanGssim(CbReference, CbDistorted);
            }
            if (component.HasFlag(UseComponent.Cr))
            {
                res.Cr = gssimCalc.CalcMeanGssim(CrReference, CrDistorted);
            }
            return res;
        }
    }

    /// <summary>
    /// This Class is used to calculate Mean G-SSIM between two value sets
    /// </summary>
    internal sealed class GssimCalculator
    {
        /// <summary>
        /// Parameters collection
        /// </summary>
        private SsimParameters Params;
        /// <summary>
        /// Auxiliary data collection of origin image data, used for fast calculation of G-SSIM
        /// </summary>
        private SsimAuxiliaries BaseAuxis;
        /// <summary>
        /// Auxiliary data collection of gradient magnitude of image data, used for fast calculation of G-SSIM
        /// </summary>
        private SsimAuxiliaries GradAuxis;

        /// <summary>
        /// Construct GSSIM Calculator with specified parameters in the algorithm
        /// </summary>
        public GssimCalculator(SsimParameters param)
        {
            Params = param;
        }

        // <summary>
        /// Calculate Mean G-SSIM by pixels of two image 
        /// </summary>
        /// <param name="lumaReference">value of pixels in reference image</param>
        /// <param name="lumaDistorted">value of pixels in distorted image</param>
        /// <returns>Mean G-SSIM</returns>
        public double CalcMeanGssim(double[,] lumaReference, double[,] lumaDistorted)
        {
            if (lumaReference == null)
            {
                throw new ArgumentNullException("lumaReference");
            }
            if (lumaDistorted == null)
            {
                throw new ArgumentNullException("lumaDistorted");
            }

            int width = lumaReference.GetLength(0);
            int height = lumaReference.GetLength(1);
            BaseAuxis = new SsimAuxiliaries(lumaReference, lumaDistorted, Params.WindowWidth, Params.WindowHeight);
            GradAuxis = new SsimAuxiliaries(GetGradient(lumaReference), GetGradient(lumaDistorted), Params.WindowWidth, Params.WindowHeight);

            double quality = 0.0;
            for (int x = 0; x <= width - Params.WindowWidth; x++)
            {
                for (int y = 0; y <= height - Params.WindowHeight; y++)
                {
                    quality += CalcWindowGssim(x, y);
                }
            }
            return quality / (width - Params.WindowWidth + 1) / (height - Params.WindowHeight + 1);
        }

        /// <summary>
        /// Get the gradient magnitude of the input data
        /// </summary>
        /// <param name="luma">Data to be processed</param>
        /// <returns>A new 2d-array represents gradient magnitude of the input</returns>
        private double[,] GetGradient(double[,] luma)
        {
            int width = luma.GetLength(0);
            int height = luma.GetLength(1);
            double[,] grad = new double[width, height];
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    double gradX = -luma[x - 1, y - 1] - 2 * luma[x - 1, y] - luma[x - 1, y + 1] + luma[x + 1, y - 1] + 2 * luma[x + 1, y] + luma[x + 1, y + 1];
                    double gradY = -luma[x - 1, y - 1] - 2 * luma[x, y - 1] - luma[x + 1, y - 1] + luma[x - 1, y + 1] + 2 * luma[x, y + 1] + luma[x + 1, y + 1];
                    gradX /= 4;
                    gradY /= 4;
                    grad[x, y] = Math.Sqrt(gradX * gradX + gradY * gradY);
                }
            }
            return grad;
        }

        private double CalcWindowGssim(int x, int y)
        {
            double baseMeanRef = BaseAuxis.WinSumRef[x, y] / Params.WindowArea;
            double baseMeanDis = BaseAuxis.WinSumDis[x, y] / Params.WindowArea;

            double gradMeanRef = GradAuxis.WinSumRef[x, y] / Params.WindowArea;
            double gradMeanDis = GradAuxis.WinSumDis[x, y] / Params.WindowArea;
            double gradVarRef = (GradAuxis.SquareWinSumRef[x, y] - 2 * gradMeanRef * GradAuxis.WinSumRef[x, y] + gradMeanRef * gradMeanRef * Params.WindowArea) / (Params.WindowArea - 1);
            double gradVarDis = (GradAuxis.SquareWinSumDis[x, y] - 2 * gradMeanDis * GradAuxis.WinSumDis[x, y] + gradMeanDis * gradMeanDis * Params.WindowArea) / (Params.WindowArea - 1);
            if (gradVarRef < 0)
            {
                gradVarRef = 0;
            }
            if (gradVarDis < 0)
            {
                gradVarDis = 0;
            }
            double gradCov = (GradAuxis.WinSumComul[x, y] - gradMeanRef * GradAuxis.WinSumDis[x, y] - gradMeanDis * GradAuxis.WinSumRef[x, y] + gradMeanRef * gradMeanDis * Params.WindowArea) / (Params.WindowArea - 1);

            double luminance = (2 * baseMeanRef * baseMeanDis + Params.C1) / (baseMeanRef * baseMeanRef + baseMeanDis * baseMeanDis + Params.C1);
            double contrast = (2 * Math.Sqrt(gradVarRef) * Math.Sqrt(gradVarDis) + Params.C2) / (gradVarRef + gradVarDis + Params.C2);
            double structure = (gradCov + Params.C3) / (Math.Sqrt(gradVarRef) * Math.Sqrt(gradVarDis) + Params.C3);

            double res = 1;
            if (structure < 0)
            {
                structure = -structure;
                res = -1;
            }
            res *= Math.Pow(luminance, Params.Alpha) * Math.Pow(contrast, Params.Beta) * Math.Pow(structure, Params.Gamma);

            return res;
        }
    }
}
