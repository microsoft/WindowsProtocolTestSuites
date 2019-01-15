// ------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment
{
    /// <summary>
    /// Ssim Index Object. The assess method is optimized in time complexity.
    /// </summary>
    public class Ssim : FRIQAIndexBase
    {
        /// <summary>
        /// In AutoScale, the image will be downscaled by a maximum possible integer that the scaled image have width and height not smaller than ScaleToLength 
        /// </summary>
        private const int ScaleMinLength = 192;
        /// <summary>
        /// SSIM Window width
        /// </summary>
        protected const int WindowWidth = 8;
        /// <summary>
        /// SSIM Window height
        /// </summary>
        protected const int WindowHeight = 8;
        /// <summary>
        /// The constant C1 in algorithm
        /// </summary>
        protected double C1 = 0.01 * 0.01 * 255 * 255;
        /// <summary>
        /// The constant C2 in algorithm
        /// </summary>
        protected double C2 = 0.03 * 0.03 * 255 * 255;
        /// <summary>
        /// The constant C2 in algorithm
        /// </summary>
        protected double C3 = 0.03 * 0.03 * 255 * 255 * 0.5;

        /// <summary>
        /// The constant Alpha in algorithm
        /// </summary>
        protected double Alpha = 1;

        /// <summary>
        /// The constant Beta in algorithm
        /// </summary>
        protected double Beta = 1;

        /// <summary>
        /// The constant Gamma in algorithm
        /// </summary>
        protected double Gamma = 1;

        /// <summary>
        /// The constant L in algorithm
        /// </summary>
        protected double L = 255;

        /// <summary>
        /// Min Width of image to assess SSIM value
        /// </summary>
        public const int MinWidth = 64; //Loose the 256 pixels limitation to 64

        /// <summary>
        /// Min Height of image to assess SSIM value
        /// </summary>
        public const int MinHeight = 64; //Loose the 256 pixels limitation to 64

        /// <summary>
        /// Constructor
        /// </summary>
        public Ssim()
            : base()
        {
        }

        /// <summary>
        /// Indicate the name of the index
        /// </summary>
        public override string IndexName
        {
            get { return "SSIM"; }
        }

        public void SetParameters(double k1, double k2, double alpha, double beta, double gamma, double l = 255)
        {
            L = l;
            C1 = k1 * k1 * L * L;
            C2 = k2 * k2 * L * L;
            C3 = C2 * 0.5;
            Alpha = alpha;
            Beta = beta;
            Gamma = gamma;
        }

        /// <summary>
        /// Check if the two bitmaps are valid for the specified index.
        /// </summary>
        /// <param name="reference">Reference bitmap</param>
        /// <param name="distorted">Distorted bitmap</param>
        /// <exception cref="ArgumentNullException">Thrown when at least one param is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the bitmaps don't fit the assess precondition of the specified index.</exception>
        protected override void CheckException(Bitmap reference, Bitmap distorted)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            if (distorted == null)
            {
                throw new ArgumentNullException("distorted");
            }
            if (reference.Height != distorted.Height || reference.Width != distorted.Width)
            {
                throw new ArgumentException("Cannot assess two bitmaps with different size.");
            }
            int height = reference.Height;
            int width = reference.Width;
            if (height < MinWidth || width < MinHeight)
            {
                throw new ArgumentException("Cannot assess bitmaps with width*height less than " + MinWidth + "*" + MinHeight + ".");
            }
        }

        /// <summary>
        /// Assess Mean SSIM of a distorted bitmap with the reference bitmap
        /// </summary>
        /// <param name="reference">Reference bitmap</param>
        /// <param name="distorted">Distorted bitmap</param>
        /// <param name="component">Specify which components will be assessed, multiple components can be set by using bitwise-or</param>
        /// <exception cref="ArgumentNullException">Thrown when at least one param is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the bitmaps don't fit the assess precondition of the specified index.</exception>
        /// <returns>A new AssessResult object indicates the assess result in every specified components.</returns>
        public override AssessResult Assess(Bitmap reference, Bitmap distorted, UseComponent component)
        {
            InitBitmaps(reference, distorted);
            LumaReference = Downscaler.DownscaleByKeepMinLength(LumaReference, ScaleMinLength);
            LumaDistorted = Downscaler.DownscaleByKeepMinLength(LumaDistorted, ScaleMinLength);

            SsimParameters ssimParams = new SsimParameters(WindowWidth, WindowHeight, C1, C2, C3, Alpha, Beta, Gamma);
            SsimCalculator ssimCalc = new SsimCalculator(ssimParams);
            AssessResult res = new AssessResult();
            if (component.HasFlag(UseComponent.Luma))
            {
                res.Luma = ssimCalc.CalcMeanSsim(LumaReference, LumaDistorted);
            }
            if (component.HasFlag(UseComponent.Cb))
            {
                res.Cb = ssimCalc.CalcMeanSsim(CbReference, CbDistorted);
            }
            if (component.HasFlag(UseComponent.Cr))
            {
                res.Cr = ssimCalc.CalcMeanSsim(CrReference, CrDistorted);
            }
            return res;
        }
    }

    /// <summary>
    /// Collection of parameters used in SSIM
    /// </summary>
    internal class SsimParameters
    {
        /// <summary>
        /// SSIM Window width
        /// </summary>
        public int WindowWidth { get; set; }
        /// <summary>
        /// SSIM Window height
        /// </summary>
        public int WindowHeight { get; set; }
        /// <summary>
        /// SSIM Window Area = width * height
        /// </summary>
        public int WindowArea { get; set; }
        /// <summary>
        /// The constant C1 in algorithm
        /// </summary>
        public double C1 { get; set; }
        /// <summary>
        /// The constant C2 in algorithm
        /// </summary>
        public double C2 { get; set; }
        /// <summary>
        /// The constant C3 in algorithm
        /// </summary>
        public double C3 { get; set; }
        /// <summary>
        /// The exponential weight of luminance component
        /// </summary>
        public double Alpha { get; set; }
        /// <summary>
        /// The exponential weight of contrast component
        /// </summary>
        public double Beta { get; set; }
        /// <summary>
        /// The exponential weight of structure component
        /// </summary>
        public double Gamma { get; set; }

        /// <summary>
        /// Constructor, fill the struct with specified params
        /// </summary>
        /// <param name="windowWidth">Window Width</param>
        /// <param name="windowHeight">Window Height</param>
        /// <param name="c1">C1</param>
        /// <param name="c2">C2</param>
        /// <param name="c3">C3</param>
        /// <param name="alpha">Alpha</param>
        /// <param name="beta">Beta</param>
        /// <param name="gamma">Gamma</param>
        public SsimParameters(int windowWidth, int windowHeight, double c1, double c2, double c3, double alpha = 1.0, double beta = 1.0, double gamma = 1.0)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            WindowArea = WindowWidth * WindowHeight;
            C1 = c1;
            C2 = c2;
            C3 = c3;
            Alpha = alpha;
            Beta = beta;
            Gamma = gamma;
        }
    }

    /// <summary>
    /// Store the auxiliary data of reference data(LumaReference) and distorted data(LumaDistorted) for fast calculation of SSIM
    /// </summary>
    internal class SsimAuxiliaries
    {
        /// <summary>
        /// the value in [x,y] is the sum of all elements in the rectangle with the two corners [0,0] and [x,y] in LumaReference
        /// </summary>
        public double[,] WinSumRef { get; set; }
        /// <summary>
        /// the value in [x,y] is the sum of all elements in the rectangle with the two corners [0,0] and [x,y] in LumaDistortion
        /// </summary>
        public double[,] WinSumDis { get; set; }
        /// <summary>
        /// the value in [x,y] is the square of LumaReference[x,y]
        /// </summary>
        public double[,] SquareRef { get; set; }
        /// <summary>
        /// the value in [x,y] is the square of LumaDistortion[x,y]
        /// </summary>
        public double[,] SquareDis { get; set; }
        /// <summary>
        /// the value in [x,y] is the sum of all elements in the rectangle with the two corners [0,0] and [x,y] in squareLumaRef
        /// </summary>
        public double[,] SquareWinSumRef { get; set; }
        /// <summary>
        /// the value in [x,y] is the sum of all elements in the rectangle with the two corners [0,0] and [x,y] in squareLumaDis
        /// </summary>
        public double[,] SquareWinSumDis { get; set; }
        /// <summary>
        /// the value in [x,y] is the product of LumaReference[x,y] and LumaDistortion[x,y]
        /// </summary>
        public double[,] Comul { get; set; }
        /// <summary>
        /// the value in [x,y] is the sum of all elements in the rectangle with the two corners [0,0] and [x,y] in lumaComul
        /// </summary>
        public double[,] WinSumComul { get; set; }

        /// <summary>
        /// Generate the auxiliaries with the reference and distorted data
        /// </summary>
        /// <param name="lumaReference">Reference image data</param>
        /// <param name="lumaDistorted">Distorted image data</param>
        public SsimAuxiliaries(double[,] lumaReference, double[,] lumaDistorted, int windowWidth, int windowHeight)
        {
            WinSumRef = GenerateWindowSum(lumaReference, windowWidth, windowHeight);
            WinSumDis = GenerateWindowSum(lumaDistorted, windowWidth, windowHeight);
            SquareRef = GenerateSquare(lumaReference);
            SquareDis = GenerateSquare(lumaDistorted);
            SquareWinSumRef = GenerateWindowSum(SquareRef, windowWidth, windowHeight);
            SquareWinSumDis = GenerateWindowSum(SquareDis, windowWidth, windowHeight);
            Comul = GenerateDotProduct(lumaReference, lumaDistorted);
            WinSumComul = GenerateWindowSum(Comul, windowWidth, windowHeight);
        }

        /// <summary>
        /// Return a new 2d-array where the value in [x,y] is the product of a[x,y] and b[x,y]
        /// </summary>
        /// <param name="a">2d-array a</param>
        /// <param name="b">2d-array b</param>
        /// <returns>The new 2d-array</returns>
        private double[,] GenerateDotProduct(double[,] a, double[,] b)
        {
            int width = a.GetLength(0);
            int height = b.GetLength(1);
            double[,] res = new double[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    res[x, y] = a[x, y] * b[x, y];
                }
            }
            return res;
        }

        /// <summary>
        /// Return a new 2d-array where the value in [x,y] is the square of src[x,y]
        /// </summary>
        /// <param name="src">2d-array src</param>
        /// <returns>The new 2d-array</returns>
        private double[,] GenerateSquare(double[,] src)
        {
            int width = src.GetLength(0);
            int height = src.GetLength(1);
            double[,] res = new double[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    res[x, y] = src[x, y] * src[x, y];
                }
            }
            return res;
        }

        /// <summary>
        /// Return a new 2d-array which has same size with specified 2d-array src and where the value in [x,y] is the sum of all elements 
        /// in the window with the left-upper corner[x,y] and specified window height and width in 2d-array src.
        /// If the window with the left-upper corner[x,y] is out-of-bound, the value in [x,y] will be 0 
        /// </summary>
        /// <param name="src">2d-array src</param>
        /// <param name="winWidth">Window width</param>
        /// <param name="winHeight">Window height</param>
        /// <returns>The new 2d-array</returns>
        private double[,] GenerateWindowSum(double[,] src, int winWidth, int winHeight)
        {
            int width = src.GetLength(0);
            int height = src.GetLength(1);
            int resWidth = width - winWidth + 1;
            int resHeight = height - winHeight + 1;
            if (resWidth <= 0 || resHeight <= 0)
            {
                return new double[0, 0];
            }
            double[,] res = new double[resWidth, resHeight];
            //x0WinWidthSum[i] equals the sum of src[0,i] to src[winWidth-1,i]
            double[] x0WinWidthSum = new double[height];
            //winHeightSum[i,j] equals the sum of src[i,j] to src[i,j+winHeight-1]
            double[,] winHeightSum = new double[width, resHeight];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < winWidth; x++)
                {
                    x0WinWidthSum[y] += src[x, y];
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < winHeight; y++)
                {
                    winHeightSum[x, 0] += src[x, y];
                }
                for (int y = 1; y < resHeight; y++)
                {
                    winHeightSum[x, y] = winHeightSum[x, y - 1] - src[x, y - 1] + src[x, y + winHeight - 1];
                }
            }

            for (int y = 0; y < winHeight; y++)
            {
                res[0, 0] += x0WinWidthSum[y];
            }
            for (int y = 1; y < resHeight; y++)
            {
                res[0, y] = res[0, y - 1] - x0WinWidthSum[y - 1] + x0WinWidthSum[y + winHeight - 1];
            }
            for (int x = 1; x < resWidth; x++)
            {
                for (int y = 0; y < resHeight; y++)
                {
                    res[x, y] = res[x - 1, y] - winHeightSum[x - 1, y] + winHeightSum[x + winWidth - 1, y];
                }
            }
            return res;
        }
    }

    /// <summary>
    /// This Class is used to calculate Mean-SSIM between two value sets
    /// </summary>
    internal sealed class SsimCalculator
    {
        /// <summary>
        /// Parameters collection
        /// </summary>
        private SsimParameters Params;
        /// <summary>
        /// Auxiliary data collection, used for fast calculation of SSIM
        /// </summary>
        private SsimAuxiliaries SsimAuxis;

        /// <summary>
        /// Construct SSIM Calculator with specified parameters in the algorithm
        /// </summary>
        public SsimCalculator(SsimParameters param)
        {
            Params = param;
        }

        /// <summary>
        /// Calculate Mean SSIM by pixels of two image 
        /// </summary>
        /// <param name="lumaReference">value of pixels in reference image</param>
        /// <param name="lumaDistorted">value of pixels in distorted image</param>
        /// <returns>Mean SSIM</returns>
        public double CalcMeanSsim(double[,] lumaReference, double[,] lumaDistorted)
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
            SsimAuxis = new SsimAuxiliaries(lumaReference, lumaDistorted, Params.WindowWidth, Params.WindowHeight);

            double quality = 0.0;
            for (int x = 0; x <= width - Params.WindowWidth; x++)
            {
                for (int y = 0; y <= height - Params.WindowHeight; y++)
                {
                    quality += CalcWindowSsim(x, y);
                }
            }
            return quality / (width - Params.WindowWidth + 1) / (height - Params.WindowHeight + 1);
        }

        /// <summary>
        /// Calculate local SSIM by pixels of two image in the specified window
        /// </summary>
        /// <param name="x">X coordinate of upperleft corner of the window</param>
        /// <param name="y">Y coordinate of upperleft corner of the window</param>
        /// <returns>Local SSIM</returns>
        private double CalcWindowSsim(int x, int y)
        {
            double meanRef = SsimAuxis.WinSumRef[x, y] / Params.WindowArea;
            double meanDis = SsimAuxis.WinSumDis[x, y] / Params.WindowArea;
            double varRef = (SsimAuxis.SquareWinSumRef[x, y] - 2 * meanRef * SsimAuxis.WinSumRef[x, y] + meanRef * meanRef * Params.WindowArea) / (Params.WindowArea - 1);
            double varDis = (SsimAuxis.SquareWinSumDis[x, y] - 2 * meanDis * SsimAuxis.WinSumDis[x, y] + meanDis * meanDis * Params.WindowArea) / (Params.WindowArea - 1);
            if (varRef < 0)
            {
                varRef = 0;
            }
            if (varDis < 0)
            {
                varDis = 0;
            }
            double cov = (SsimAuxis.WinSumComul[x, y] - meanRef * SsimAuxis.WinSumDis[x, y] - meanDis * SsimAuxis.WinSumRef[x, y] + meanRef * meanDis * Params.WindowArea) / (Params.WindowArea - 1);

            double luminance = (2 * meanRef * meanDis + Params.C1) / (meanRef * meanRef + meanDis * meanDis + Params.C1);
            double contrast = (2 * Math.Sqrt(varRef) * Math.Sqrt(varDis) + Params.C2) / (varRef + varDis + Params.C2);
            double structure = (cov + Params.C3) / (Math.Sqrt(varRef) * Math.Sqrt(varDis) + Params.C3);

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
