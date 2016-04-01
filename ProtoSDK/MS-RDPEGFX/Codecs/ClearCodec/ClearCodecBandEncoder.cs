// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The Color_RGB structure defines a color in RGB format.
    /// </summary>
    public struct Color_RGB
    {
        public byte B;  // blue
        public byte G;  // green
        public byte R;  // red
    }

    /// <summary>
    /// The ClearCodec_RECT16 structure defines left-top, right-bottom position of a rectangle.
    /// </summary>
    public struct ClearCodec_RECT16
    {
        public ushort left;
        public ushort top;
        public ushort right;
        public ushort bottom;
    }

    /// <summary>
    /// The CLEARCODEC_BANDS_DATA structure contains the second layer of pixels in an encoded image. 
    /// This layer MUST be decoded on top of the first layer, in some cases overwriting pixels in the first layer. 
    /// </summary>
    public struct CLEARCODEC_BAND_DATA
    {
        public CLEARCODEC_BAND[] bandArr;
    }

    /// <summary>
    /// The CLEARCODEC_BAND structure specifies a horizontal band that is composed of columns of pixels. 
    /// Each of these columns is referred to as a "V-Bar". The maximum height of a band is 52 pixels.
    /// </summary>
    public struct CLEARCODEC_BAND
    {
        /// <summary>
        /// An 16-bit unsigned integer, specifies the horizontal position (relative to the left edge of the bitmap) where the band starts. 
        /// </summary>
        public ushort xStart;
        /// <summary>
        /// An 16-bit unsigned integer, specifies the horizontal position (relative to the left edge of the bitmap) where the band ends. 
        /// </summary>
        public ushort xEnd;
        /// <summary>
        /// An 16-bit unsigned integer, specifies the vertical position (relative to the left edge of the bitmap) where the band starts. 
        /// </summary>
        public ushort yStart;
        /// <summary>
        /// An 16-bit unsigned integer, specifies the vertical position (relative to the left edge of the bitmap) where the band ends. 
        /// </summary>
        public ushort yEnd;
        /// <summary>
        /// An 8-bit unsigned integer, specifies the blue value of the background for this band. 
        /// </summary>
        public byte blueBkg;
        /// <summary>
        /// An 8-bit unsigned integer, specifies the green  value of the background for this band. 
        /// </summary>
        public byte greenBkg;
        /// <summary>
        /// An 8-bit unsigned integer, specifies the red  value of the background for this band. 
        /// </summary>
        public byte redBkg;

        public CLEARCODEC_VBAR[] vBars; 
    }

    /// <summary>
    /// specifies the type of vbar in band layer. 
    /// </summary>
    public enum VBAR_TYPE : byte
    {
        VBAR_CACHE_HIT = 0x01,
        SHORT_VBAR_CACHE_HIT = 0x02,
        SHORT_VBAR_CACHE_MISS = 0x03
    }

    /// <summary>
    /// The CLEARCODEC_VBAR structure is used to encode a single column of pixels 
    /// </summary>
    //[StructLayout(LayoutKind.Explicit)]
    public struct CLEARCODEC_VBAR
    {
        /// <summary>
        /// The vbar type flag, indicate which below fields(vbarCacheHit,shortVbarCacheHit,shortVbarCacheMiss) are applied for use.
        /// </summary>
        public VBAR_TYPE type;
        /// <summary>
        /// specify a V-Bar cache hit
        /// </summary>
        //[FieldOffset(1)]
        public VBAR_CACHE_HIT vbarCacheHit;
        /// <summary>
        /// specify a short V-Bar cache hit
        /// </summary>
        //[FieldOffset(1)]
        public SHORT_VBAR_CACHE_HIT shortVbarCacheHit;
        /// <summary>
        /// specify a short V-Bar cache miss
        /// </summary>
        //[FieldOffset(1)]
        public SHORT_VBAR_CACHE_MISS shortVbarCacheMiss;
    }

    /// <summary>
    /// The VBAR_CACHE_HIT structure is used to specify a V-Bar cache hit. 
    /// </summary>
    public struct VBAR_CACHE_HIT
    {
        public ushort vBarIndex_x;

        // the lowest 15 bits
        public ushort vBarIndex
        {
            get
            {
                return (ushort)(vBarIndex_x & 0x7fff);
            }
            set
            {
                ushort temp = value;
                vBarIndex_x = (ushort)((vBarIndex_x & 0x8000) | temp);
            }
        }
        // the highest 1 bit
        public ushort x
        {
            get
            {
                return (ushort)((vBarIndex_x >> 15) & 0x01);
            }
            set
            {
                ushort temp = (ushort)((value << 15) & 0x8000);
                vBarIndex_x = (ushort)((vBarIndex_x & 0x7fff) | temp);
            }
        }
    }

    /// <summary>
    /// The SHORT_VBAR_CACHE_HIT structure is used to specify a Short V-Bar cache hit. 
    /// </summary>
    public struct SHORT_VBAR_CACHE_HIT
    {
        public ushort shortVBarIndex_x;

        public byte shortVBarYOn;

        // the lowest 14 bits
        public ushort shortVBarIndex
        {
            get
            {
                return (ushort)(shortVBarIndex_x & 0x3fff);
            }
            set
            {
                ushort temp = value;
                shortVBarIndex_x = (ushort)((shortVBarIndex_x & 0xc000) | temp);
            }
        }

        // the highest 2 bits
        public ushort x
        {
            get
            {
                return (ushort)((shortVBarIndex_x >> 14) & 0x03);
            }
            set
            {
                ushort temp = (ushort)((value << 14) & 0xc000);
                shortVBarIndex_x = (ushort)((shortVBarIndex_x & 0x3fff) | temp);
            }

        }
    }


    /// <summary>
    /// The SHORT_VBAR_CACHE_MISS structure is used to specify a Short V-Bar cache miss.
    /// </summary>
    public struct SHORT_VBAR_CACHE_MISS
    {
        public ushort shortVBarYOnOff_x;

        public Color_RGB[] shortVBarPixels;

        // the lowest 8 bits
        public ushort shortVBarYOn
        {
            get
            {
                return (ushort)(shortVBarYOnOff_x & 0x00ff);
            }
            set
            {
                ushort temp = value;
                shortVBarYOnOff_x = (ushort)((shortVBarYOnOff_x & 0xff00) | temp);
            }
        }
        // the middle 6 bits
        public ushort shortVBarYOff   
        {
            get 
            {
                return (ushort)((shortVBarYOnOff_x >> 8) & 0x003f);
            }
            set
            {
                ushort temp = (ushort)((value << 8) & 0x3f00);
                shortVBarYOnOff_x = (ushort)((shortVBarYOnOff_x & 0xc0ff) | temp);
            }
        }

        // the highest 2 bits
        public ushort x
        {
            get
            {
                return (ushort)(shortVBarYOnOff_x >> 14);
            }
            set
            {
                ushort temp = (ushort)(value << 14);
                shortVBarYOnOff_x = (ushort)((shortVBarYOnOff_x & 0x3fff) | temp);
            }
        }
    }

    /// <summary>
    /// ClearCodecBandEncoder contains the second layer of pixels in an encoded image. 
    /// This layer MUST be decoded on top of the first layer, in some cases overwriting pixels in the first layer.
    /// </summary>
    public class ClearCodecBandEncoder
    {
        #region private variable
        // save the vBar and its index
        private Dictionary<List<Color_RGB>, ushort> vBarDict;
        // the next index of a new vBar to be saved
        private ushort vBarCursor;
        // save the short vBar and its index
        private Dictionary<List<Color_RGB>, ushort> shortvBarDict;
        // the next index of a new short vBar to be saved
        private ushort shortvBarCursor;

        // sigelton mode
        private static ClearCodecBandEncoder _instance;
        // use lock to make sigelton thread safe
        private static object synclock = new object(); 
        #endregion 

        #region static method
        /// <summary>
        /// create a single instance of band encoder
        /// </summary>
        public static ClearCodecBandEncoder GetInstance()
        {
            lock (synclock)
            {
                if (null == _instance)
                {
                    _instance = new ClearCodecBandEncoder();
                }
            }
            return _instance;
        }

        #endregion 

        #region method
        /// <summary>
        /// constructor
        /// </summary>
        public ClearCodecBandEncoder()
        {
            vBarDict = new Dictionary<List<Color_RGB>, ushort>();
            vBarCursor = 0;
            shortvBarDict = new Dictionary<List<Color_RGB>, ushort>();
            shortvBarCursor = 0;
        }

        /// <summary>
        /// reset vBarDict, vBarCursor, shortvBarDict, shortvBarCursor
        /// </summary>
        public void ResetVBarStorage()
        {
            vBarDict.Clear();
            vBarCursor = 0;
            shortvBarDict.Clear();
            shortvBarCursor = 0;
        }

        /// <summary>
        /// compare if the two vbar has same pixels 
        /// </summary>
        public bool vBarCompare(List<Color_RGB> vbar1, List<Color_RGB> vbar2)
        {
            // one of vbar is null
            if ((vbar2 == null) || (vbar1 == null))
            {
                if (vbar1 == vbar2)
                    return true;
                else
                    return false;
            }

            // both vbar1 &2 are not null
            if (vbar1.Count() != vbar2.Count()) return false;
            for (int i = 0; i < vbar1.Count(); i++)
            {
                if ((vbar1[i].B != vbar2[i].B) ||
                    (vbar1[i].G != vbar2[i].G) ||
                    (vbar1[i].R != vbar2[i].R))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// get index of a vbar in vbar storage on RDP server
        /// </summary>
        public ushort GetVbarIndex(List<Color_RGB> vBar)
        {
            foreach (KeyValuePair<List<Color_RGB>, ushort> pair in vBarDict)
            {
                if (vBarCompare(pair.Key, vBar)) return pair.Value;
            }
            return 0xffff;
        }

        /// <summary>
        /// get index of a short vbar in short vbar storage on RDP server
        /// </summary>
        public ushort GetShortVbarIndex(List<Color_RGB> shortvBar)
        {
            foreach (KeyValuePair<List<Color_RGB>, ushort> pair in shortvBarDict)
            {
                if (vBarCompare(pair.Key, shortvBar)) return pair.Value;
            }
            return 0xffff;
        }

        /// <summary>
        /// Encode a band bitmap into byte stream
        /// </summary>
        /// <param name="bandBmp">The bitmap to be encoded in band layer.</param>
        /// <param name="bandRect">The band position relative to bitmap left-top edge.</param>
        public CLEARCODEC_BAND EncodeBand(Bitmap bandBmp, ClearCodec_RECT16 bandRect)
        {
          
            CLEARCODEC_BAND bandData = new CLEARCODEC_BAND();

            Color bgColor = bandBmp.GetPixel(0, 0);

            bandData.xStart = bandRect.left;
            bandData.xEnd = (ushort)(bandRect.right-1);
            bandData.yStart = bandRect.top;
            bandData.yEnd = (ushort)(bandRect.bottom-1);

            bandData.blueBkg = bgColor.B;
            bandData.greenBkg = bgColor.G;
            bandData.redBkg = bgColor.R;

            List<CLEARCODEC_VBAR> vBarList = new List<CLEARCODEC_VBAR>();

            // use short vbar cache miss method
            for (ushort x = 0; x < bandBmp.Width; x++)
            {           
                byte shortVBarYOn = 0;  // relative to top of V-Bar, won't excceed 52
                byte shortVBarYOff = 0; // relative to top of V-Bar, won't excceed 52

                // find shortVBarYOn from top
                byte y;  // bandBmp.Height can't exceed 52 pixels
                for ( y = 0; y < bandBmp.Height; y++)
                {
                    Color pixelColor = bandBmp.GetPixel(x, y);
                    if (!bgColor.Equals(pixelColor))
                    {
                        shortVBarYOn = (byte)y;
                        break;
                    }
                }

                // the whole vbar use bgColor
                if (y == (ushort)bandBmp.Height)
                {
                    shortVBarYOn = (byte)y;
                    shortVBarYOff = (byte)y;
                }

                // find shortVBarYOff from bottem, which is the first position for left bgcolor
                for (y = (byte)(bandBmp.Height - 1); y > shortVBarYOn; y--)
                {
                    Color pixelColor = bandBmp.GetPixel(x, y);
                    if (!bgColor.Equals(pixelColor))
                    {
                        shortVBarYOff = (byte)(y + 1);
                        break;
                    }
                }

                // only one point in shortVBar is different from bgColor
                if (y == shortVBarYOn)
                {
                    shortVBarYOff = (byte)(shortVBarYOn + 1);
                }
               
                // construct short Vbar Pixels

                List<Color_RGB> shortVBarPixelList = new List<Color_RGB>();
                for (y = shortVBarYOn; y < shortVBarYOff; y++)
                {
                    Color pixelColor = bandBmp.GetPixel(x, y);
                    Color_RGB svbarColor = new Color_RGB();
                    svbarColor.B = pixelColor.B;
                    svbarColor.G = pixelColor.G;
                    svbarColor.R = pixelColor.R;

                    shortVBarPixelList.Add(svbarColor);
                }

                // construct vbar pixels
                List<Color_RGB> vBarPixelList = new List<Color_RGB>();
                for (int k = 0; k < bandBmp.Height; k++)
                {
                    Color pixelColor = bandBmp.GetPixel(x, k);
                    Color_RGB vbarColor = new Color_RGB();
                    vbarColor.B = pixelColor.B;
                    vbarColor.G = pixelColor.G;
                    vbarColor.R = pixelColor.R;
                    vBarPixelList.Add(vbarColor);
                }

                // check if vbar or short vbar cache matched
                ushort vbarIdx = GetVbarIndex(vBarPixelList);
                if (vbarIdx != 0xffff)
                {
                    // use cache index
                    CLEARCODEC_VBAR vBar = new CLEARCODEC_VBAR();
                    vBar.type = VBAR_TYPE.VBAR_CACHE_HIT;
                    vBar.vbarCacheHit.x = 0x1;
                    vBar.vbarCacheHit.vBarIndex = vbarIdx;

                    vBarList.Add(vBar);
                            
                    continue;
                }
                else
                {
                    vBarDict.Add(vBarPixelList, vBarCursor);
                    vBarCursor = (ushort)((vBarCursor + 1) % CLEARCODEC_CONST.CLEARCODEC_BAND_MAX_VBAR_CACHE_SLOT);
                }

                // cechk short vbar cache
                ushort shortvbarIdx = GetShortVbarIndex(shortVBarPixelList);
                if (shortvbarIdx != 0xffff)
                {
                    // use short vbar cache index
                    CLEARCODEC_VBAR svBar = new CLEARCODEC_VBAR();
                    svBar.type = VBAR_TYPE.SHORT_VBAR_CACHE_HIT;
                    svBar.shortVbarCacheHit.x = 0x1;
                    svBar.shortVbarCacheHit.shortVBarIndex = shortvbarIdx;
                    svBar.shortVbarCacheHit.shortVBarYOn = shortVBarYOn;

                    vBarList.Add(svBar);
                    continue;
                }
                else
                {
                    // add short vbar into cache dictionary and send the short vbar vbar in SHORT_VBAR_CACHE_MISS structure later.
                    shortvBarDict.Add(shortVBarPixelList, shortvBarCursor);
                    shortvBarCursor = (ushort)((shortvBarCursor + 1) % CLEARCODEC_CONST.CLEARCODEC_BAND_MAX_SHORT_VBAR_CACHE_SLOT);
                }
                

                // get to here mean no vbar or short vbar cache hit
                CLEARCODEC_VBAR vBarUncache = new CLEARCODEC_VBAR();
                vBarUncache.type = VBAR_TYPE.SHORT_VBAR_CACHE_MISS;
                vBarUncache.shortVbarCacheMiss.x = 0x0; 

                // pack data into vBar.shortVbarCacheMiss
                vBarUncache.shortVbarCacheMiss.shortVBarYOn = shortVBarYOn;
                vBarUncache.shortVbarCacheMiss.shortVBarYOff = shortVBarYOff;
                if (shortVBarYOff > shortVBarYOn )
                {
                    vBarUncache.shortVbarCacheMiss.shortVBarPixels = shortVBarPixelList.ToArray();
                }

                vBarList.Add(vBarUncache);              
            }

            bandData.vBars = vBarList.ToArray();

            return bandData;
        }



        /// <summary>
        /// Encode multiple band bitmap into byte stream
        /// </summary>
        /// <param name="bandDict">The structure saves multiple band bitmap and position.</param>
        /// <param name="vbarCacheEnabled">if vbar or short vbar cache is used when encoding band</param>
        public CLEARCODEC_BAND_DATA Encode(Dictionary<ClearCodec_RECT16, Bitmap> bandDict)
        {
            List<CLEARCODEC_BAND> bandList = new List<CLEARCODEC_BAND>();

            foreach (KeyValuePair<ClearCodec_RECT16, Bitmap> band in bandDict)
            {
                if (band.Value == null) continue;
                CLEARCODEC_BAND bandData = EncodeBand(band.Value, band.Key);

                bandList.Add(bandData);
            }

            CLEARCODEC_BAND_DATA bands = new CLEARCODEC_BAND_DATA();
            bands.bandArr = bandList.ToArray();
            return bands;
        }
        #endregion
    }
}
