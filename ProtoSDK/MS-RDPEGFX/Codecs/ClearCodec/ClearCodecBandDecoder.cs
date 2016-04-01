// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// It decode byte stream into CLEARCODEC_RESIDUAL_DATA structure
    /// </summary>
    class ClearCodecBandDecoder : BasicTypeDecoder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClearCodecBandDecoder(byte[] Data) 
            : base(Data)
        {           
        }

        /// <summary>
        /// It decode band layer byte stream into CLEARCODEC_BAND_DATA structure
        /// </summary>
        /// <param name = "bands"> The structure that decode result save to. </param>
        /// <return> true if decode success, otherwise return false </return>
        public bool Decode(ref CLEARCODEC_BAND_DATA bands)
        {
            if (decodeData == null) return false;

            List<CLEARCODEC_BAND> bandList = new List<CLEARCODEC_BAND>();
            while (offset < decodeData.Count())
            {
                CLEARCODEC_BAND band = new CLEARCODEC_BAND();
                if (!DecodeBand(ref band)) return false;
                bandList.Add(band);
            }
            bands.bandArr = bandList.ToArray();
            return true;
        }

        /// <summary>
        /// It decode byte stream into CLEARCODEC_BAND structure
        /// </summary>
        /// <param name = "band"> The structure that decode result save to. </param>
        public bool DecodeBand(ref CLEARCODEC_BAND band)
        {
            if (!DecodeUShort(ref band.xStart)) return false;
            if (!DecodeUShort(ref band.xEnd)) return false;
            if (!DecodeUShort(ref band.yStart)) return false;
            if (!DecodeUShort(ref band.yEnd)) return false;

            if (!DecodeByte(ref band.blueBkg)) return false;
            if (!DecodeByte(ref band.greenBkg)) return false;
            if (!DecodeByte(ref band.redBkg)) return false;

            ushort vbarCount = (ushort)(band.xEnd - band.xStart + 1);
            List<CLEARCODEC_VBAR> vbarList = new List<CLEARCODEC_VBAR>();
            for (int i = 0; i < vbarCount; i++)
            {
                CLEARCODEC_VBAR vbar = new CLEARCODEC_VBAR();
                if (!DecodeBandVBar(ref vbar)) return false;
                vbarList.Add(vbar);
            }
            band.vBars = vbarList.ToArray();

            return true;
        }

        /// <summary>
        /// It decode byte stream into CLEARCODEC_VBAR structure
        /// </summary>
        /// <param name = "bandVBar"> The structure that decode result save to. </param>
        public bool DecodeBandVBar(ref CLEARCODEC_VBAR bandVBar)
        {
            ushort vBarStat = 0;
            if (!DecodeUShort(ref vBarStat)) return false;

            if ((vBarStat & 0x8000) != 0)  // VBAR_CACHE_HIT structure
            {
                bandVBar.type = VBAR_TYPE.VBAR_CACHE_HIT;
                bandVBar.vbarCacheHit.vBarIndex_x = vBarStat;
            }
            else if ((vBarStat & 0xc000) != 0)  // SHORT_VBAR_CACHE_HIT structure
            {
                bandVBar.type = VBAR_TYPE.SHORT_VBAR_CACHE_HIT;
                bandVBar.shortVbarCacheHit.shortVBarIndex_x = vBarStat;
                if (!DecodeByte(ref bandVBar.shortVbarCacheHit.shortVBarYOn)) return false;
            }
            else  // Must be SHORT_VBAR_CACHE_MISS structure.
            {
                bandVBar.type = VBAR_TYPE.SHORT_VBAR_CACHE_MISS;
                bandVBar.shortVbarCacheMiss.shortVBarYOnOff_x = vBarStat;
                byte pixelCount = (byte)(bandVBar.shortVbarCacheMiss.shortVBarYOff - bandVBar.shortVbarCacheMiss.shortVBarYOn);

                // Decode shortVBarPixels.
                List<Color_RGB> shortVBarPixelList = new List<Color_RGB>();
                for (int i = 0; i < pixelCount; i++)
                {
                    Color_RGB rgb = new Color_RGB();
                    if (!DecodeByte(ref rgb.B)) return false;
                    if (!DecodeByte(ref rgb.G)) return false;
                    if (!DecodeByte(ref rgb.R)) return false;
                    shortVBarPixelList.Add(rgb);
                }

                bandVBar.shortVbarCacheMiss.shortVBarPixels = shortVBarPixelList.ToArray();
            }
            return true;
        }
    }
}
