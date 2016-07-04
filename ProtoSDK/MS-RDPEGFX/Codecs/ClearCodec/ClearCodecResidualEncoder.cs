// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// below consts defines consts when bitmap is encoded via clearcodec
    /// </summary>
    public struct CLEARCODEC_CONST
    {
        // max width for residual layer bitmap
        public const ushort CLEARCODEC_BMP_MAX_WIDTH = 65535;
        // max height for residual layer bitmap
        public const ushort CLEARCODEC_BMP_MAX_HEIGHT = 65535;
        // max height for band layer bitmap
        public const ushort CLEARCODEC_BAND_MAX_HEIGHT = 52;
        // max cache number for v-bars in band layer
        public const ushort CLEARCODEC_BAND_MAX_VBAR_CACHE_SLOT = 0x8000;
        // max cache number for short v-bars in band layer
        public const ushort CLEARCODEC_BAND_MAX_SHORT_VBAR_CACHE_SLOT = 0x4000;
    }
    
    /// <summary>
    /// The CLEARCODEC_RGB_RUN_SEGMENT structure encodes a single RGB run segment.
    /// </summary>
    public struct CLEARCODEC_RGB_RUN_SEGMENT
    {
        /// <summary>
        /// An 8-bit unsigned integer, specifies the blue value of the current pixel. 
        /// </summary>
        public byte buleValue;
        /// <summary>
        /// An 8-bit unsigned integer, specifies the green value of the current pixel. 
        /// </summary>
        public byte greenValue;
        /// <summary>
        /// An 8-bit unsigned integer, specifies red blue value of the current pixel. 
        /// </summary>
        public byte redValue;
        /// <summary>
        /// An 32-bit unsigned integer, specifies the repeat times the current pixel. 
        /// </summary>
        public uint rlFactor;
    }

    /// <summary>
    /// The CLEARCODEC_RESIDUAL_DATA structure contains the first layer of pixels in an encoded image. 
    /// </summary>
    public struct CLEARCODEC_RESIDUAL_DATA
    {
        public CLEARCODEC_RGB_RUN_SEGMENT[] resRLSegArr;
    }

    /// <summary>
    /// The clearcodec residual layer encoder. The pixels are stored in upper-left to lower-right order.
    /// </summary>
    class ClearCodecResidualEncoder
    {
 
        #region method 


        /// <summary>
        /// Add a new pixel and run length count to list
        /// </summary>
        /// <param name = "resRLSegList"> the list to save all pixel and its run length count </param>
        /// <param name = "compPixel"> the new pixel to be added </param>
        /// <param name = "count"> the run length factor of new pixel to be added </param>
        public static void addPixelToRLSegList(List<CLEARCODEC_RGB_RUN_SEGMENT> resRLSegList, Color compPixel, uint count)
        {
            CLEARCODEC_RGB_RUN_SEGMENT rgbSeg = new CLEARCODEC_RGB_RUN_SEGMENT();
            rgbSeg.buleValue = compPixel.B;
            rgbSeg.greenValue = compPixel.G;
            rgbSeg.redValue = compPixel.R;
            rgbSeg.rlFactor = count;

            resRLSegList.Add(rgbSeg);
        }

        /// <summary>
        /// Encode a bitmap with Run-length format
        /// </summary>
        /// <param name="resBmp">The bitmap to be encoded in residual layer.</param>
        public static CLEARCODEC_RESIDUAL_DATA Encode(Bitmap resBmp)
        {

            CLEARCODEC_RESIDUAL_DATA resData = new CLEARCODEC_RESIDUAL_DATA();
            List<CLEARCODEC_RGB_RUN_SEGMENT> resRLSegList = new List<CLEARCODEC_RGB_RUN_SEGMENT>();       
            Color compColor = resBmp.GetPixel(0, 0);

            uint count = 0;
            for (ushort y = 0; y < resBmp.Height; y++)
            {
                for (ushort x = 0; x < resBmp.Width; x++)
                {
                    Color pixelColor = resBmp.GetPixel(x, y);
                    if (compColor.Equals(pixelColor))
                    {
                        count++;
                    }
                    else
                    {
                        // add old compared color with repeate count into list
                        addPixelToRLSegList(resRLSegList, compColor, count);

                        // reset to new compare color and count.
                        compColor = pixelColor;
                        count = 1;
                    }
                }
            }

            // add the last color RL seg here
            addPixelToRLSegList(resRLSegList, compColor, count);
            resData.resRLSegArr = resRLSegList.ToArray();

            return resData;
        }

        #endregion

    }
}
