// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections;
using System.Security.Cryptography;
using SkiaSharp;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{

    /// <summary>
    /// The image tile in RGB format.
    /// </summary>
    public class RgbTile
    {
        #region RGB data
        /// <summary>
        /// Red component
        /// </summary>
        public byte[,] RSet;

        /// <summary>
        /// Green component
        /// </summary>
        public byte[,] GSet;

        /// <summary>
        /// Blue component
        /// </summary>
        public byte[,] BSet;

        /// <summary>
        /// The fixed tile size
        /// </summary>
        public const int TileSize = 64;

        #endregion

        private byte[] hash = null;

        /// <summary>
        /// Create a RGB tile from a given bitmap
        /// </summary>
        /// <param name="orgImg">The given bitmap</param>
        /// <param name="leftOffset">The left offset of the tile to the bitmap</param>
        /// <param name="topOffset">The top offset of the tile to the bitmap</param>
        /// <returns></returns>
        public static RgbTile GetFromImage(SKBitmap orgImg, int leftOffset, int topOffset)
        {
            RgbTile tile = new RgbTile();
            tile.RSet = new byte[TileSize, TileSize];
            tile.GSet = new byte[TileSize, TileSize];
            tile.BSet = new byte[TileSize, TileSize];

            int right = Math.Min(orgImg.Width - 1, leftOffset + TileSize - 1);
            int bottom = Math.Min(orgImg.Height - 1, topOffset + TileSize - 1);

            for (int y = topOffset; y < bottom; y++)
            {
                for (int x = leftOffset; x < right; x++)
                {
                    int tileX = x - leftOffset;
                    int tileY = y - topOffset;
                    SKColor color = orgImg.GetPixel(tileX, tileY);
                    tile.BSet[tileX, tileY] = color.Blue;
                    tile.GSet[tileX, tileY] = color.Green;
                    tile.RSet[tileX, tileY] = color.Red;
                }
            }
            return tile;
        }

        /// <summary>
        /// Reander RGB data to a bitmap
        /// </summary>
        /// <returns>Bitmap of the tile.</returns>
        public SKBitmap ToImage()
        {
            SKBitmap bmp = new SKBitmap(TileSize, TileSize);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var (r, g, b) = (this.RSet[x, y], this.GSet[x, y], this.BSet[x, y]);
                    bmp.SetPixel(x, y, new SKColor(r, g, b));
                }
            }
            return bmp;
        }

        /// <summary>
        /// Compare if equals with the given RGB tile
        /// </summary>
        /// <param name="cpTile">The given RGB tile</param>
        /// <returns>True if equals, otherwise false</returns>
        public bool EqualsWith(RgbTile cpTile)
        {
            byte[] lastHash = this.HashValue;
            byte[] newHash = cpTile.HashValue;
            for (int i = 0; i < lastHash.Length; i++)
            {
                if (lastHash[i] != newHash[i]) return false;
            }
            return true;
        }

        private bool ByteArrayEquals(byte[,] a, byte[,] b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;

            for (int x = 0; x < TileSize; x++)
            {
                for (int y = 0; y < TileSize; y++)
                {
                    if (a[x, y] != b[x, y]) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get Hash value of this tile.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        public byte[] HashValue
        {
            get
            {
                if (hash == null)
                {
                    // Suppress "CA5350:MD5CannotBeUsed" message, since MD5 here is only for get hash value, not for securit. 
                    MD5 hasher = MD5.Create();
                    byte[] temp = new byte[this.RSet.Length * 3];
                    Buffer.BlockCopy(this.RSet, 0, temp, 0, RSet.Length * sizeof(byte));
                    Buffer.BlockCopy(this.GSet, 0, temp, RSet.Length, RSet.Length * sizeof(byte));
                    Buffer.BlockCopy(this.BSet, 0, temp, RSet.Length * 2, RSet.Length * sizeof(byte));
                    hasher.ComputeHash(temp);
                    hash = hasher.Hash;
                }
                return hash;
            }
        }
    }

}
