// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// Util structure for tile image
    /// </summary>
    public struct TileImage
    {
        /// <summary>
        /// the image object of a tile 
        /// </summary>
        public Image image;
        /// <summary>
        /// the x-coordinate of leftmost of tile image
        /// </summary>
        public int x;
        /// <summary>
        /// the y-coordinate of top of tile image
        /// </summary>
        public int y;
    }


    public class RdprfxTileUtils
    {
        public static TileImage[] SplitToTileImage(Image orgImage, ushort maxWidth, ushort maxHeigth)
        {
            List<TileImage> imgList = new List<TileImage>();
            int orgWidth = orgImage.Width;
            int orgHeight = orgImage.Height;

            int gridWidth = (orgWidth - 1) / maxWidth + 1;
            int gridHeigth = (orgHeight - 1) / maxHeigth + 1;

            int minWidth = orgWidth % maxWidth;
            int minHeigth = orgHeight % maxHeigth;

            if (minWidth == 0) minWidth = maxWidth;
            if (minHeigth == 0) minHeigth = maxHeigth;

            for (int xIdx = 0; xIdx < gridWidth; xIdx++)
            {
                int destWidth = maxWidth;
                if (xIdx == gridWidth - 1)
                {
                    destWidth = minWidth;
                }
                for (int yIdx = 0; yIdx < gridHeigth; yIdx++)
                {
                    int destHeight = maxHeigth;
                    if (yIdx == gridHeigth - 1)
                    {
                        destHeight = minHeigth;
                    }

                    Bitmap destImg = new Bitmap(destWidth, destHeight);
                    Graphics g = Graphics.FromImage(destImg);
                    Rectangle destRect = new Rectangle(0, 0, destWidth, destHeight);
                    Rectangle sourceRect = new Rectangle(xIdx * maxWidth, yIdx * maxHeigth, destWidth, destHeight);
                    g.DrawImage(orgImage, destRect, sourceRect, GraphicsUnit.Pixel);
                    TileImage tImg = new TileImage();
                    tImg.image = destImg;
                    tImg.x = xIdx * maxWidth;
                    tImg.y = yIdx * maxHeigth;
                    imgList.Add(tImg);
                }
            }
            return imgList.ToArray();
        }
    }
}
