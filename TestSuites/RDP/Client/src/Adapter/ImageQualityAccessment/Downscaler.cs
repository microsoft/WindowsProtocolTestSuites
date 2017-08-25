// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment
{
    /// <summary>
    /// The class is for downscaling the image
    /// </summary>
    internal class Downscaler
    {
        /// <summary>
        /// Downscale the image by a maximum possible integer that the scaled image has width and height not smaller than the specified keepMinLength
        /// </summary>
        /// <param name="luma">The image data</param>
        /// <param name="keepMinLength">Input image data</param>
        /// <exception cref="ArgumentNullException">Thrown when luma is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when keepMinLength is non-positive</exception>
        /// <returns>Downscaled image</returns>
        public static double[,] DownscaleByKeepMinLength(double[,] luma, int keepMinLength)
        {
            if (luma == null)
            {
                throw new ArgumentNullException("luma");
            }
            if (keepMinLength < 1)
            {
                throw new ArgumentOutOfRangeException("keepMinLength");
            }

            int width = luma.GetLength(0);
            int height = luma.GetLength(1);
            int scaleFactor = Math.Min(width, height) / keepMinLength;
            if (scaleFactor < 2)
            {
                return luma;
            }
            else
            {
                return DownscaleByFactor(luma, scaleFactor);
            }
        }

        /// <summary>
        /// Downscale the image by the integer factor
        /// </summary>
        /// <param name="luma">The image data</param>
        /// <param name="factor">Downscale factor</param>
        /// <exception cref="ArgumentNullException">Thrown when luma is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when factor is non-positive</exception>
        /// <returns>Downscaled image</returns>
        public static double[,] DownscaleByFactor(double[,] luma, int factor)
        {
            if (luma == null)
            {
                throw new ArgumentNullException("luma");
            }

            int width = luma.GetLength(0);
            int height = luma.GetLength(1);
                
            if (factor < 1 || factor > width || factor > height)
            {
                throw new ArgumentOutOfRangeException("factor");
            }
            if (factor == 1)
            {
                return luma;
            }
            else
            {
                
                double scaleInverseSquare = 1.0 / (factor * factor);
                double[,] res = new double[width / factor, height / factor];

                width -= width % factor;
                height -= height % factor;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        res[x / factor, y / factor] += luma[x, y] * scaleInverseSquare;
                    }
                }
                return res;
            }
        }
    }
}
