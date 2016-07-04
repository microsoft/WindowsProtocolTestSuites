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

    using RLEX_RGB_TRIPLET = Color_RGB;
    /// <summary>
    /// The CLEARCODEC_SUBCODEC_ID structure defines the different subcodec encoding method. 
    /// </summary>
    public enum CLEARCODEC_SUBCODEC_ID : byte
    {
        SUBCODEC_RAW        = 0x00,     // subcodec data is encoded as raw RGB data.
        SUBCODEC_NSCODEC    = 0x01,     // subcodec data is encoded as NSCodec.
        SUBCODEC_RLEX       = 0x02      // subcodec data is encoded as RLEX format.
    }

    /// <summary>
    /// The BMP_INFO structure contains bitmap data and subcodec encoding ID. 
    /// </summary>
    public struct BMP_INFO
    {
        public Bitmap bmp;   // subcodec layer bitmap object
        public CLEARCODEC_SUBCODEC_ID scID;  // the encoding identifier of subcodec layer bitmap
    }

    /// <summary>
    /// The CLEARCODEC_SUBCODECS_DATA structure contains the third layer of pixels in an encoded image. 
    /// </summary>
    public struct CLEARCODEC_SUBCODEC_DATA 
    {
        public CLEARCODEC_SUBCODEC[] subcodecArr;
    }

    /// <summary>
    /// CLEARCODEC_SUBCODEC structure encapsulates an uncompressed bitmap or a bitmap encoded with the NSCodec Codec or the RLEX scheme 
    /// </summary>
    public struct CLEARCODEC_SUBCODEC
    {
        /// <summary>
        /// A 16-bit unsigned integer, specifies the horizontal position where the subcodec-encoded bitmap 
        /// should be placed once it has been decoded. (relative to the left edge of the bitmap) 
        /// </summary>
        public ushort xStart;
        /// <summary>
        /// A 16-bit unsigned integer, specifies the vertical position where the subcodec-encoded bitmap 
        /// should be placed once it has been decoded. (relative to the left edge of the bitmap) 
        /// </summary>
        public ushort yStart;
        /// <summary>
        /// A 16-bit unsigned integer, specifies the width of the subcodec-encoded bitmap. 
        /// </summary>
        public ushort width;
        /// <summary>
        /// A 16-bit unsigned integer, specifies the height of the subcodec-encoded bitmap. 
        /// </summary>
        public ushort height;
        /// <summary>
        /// A 32-bit unsigned integer, specifies the number of bytes in the bitmapData field.
        /// </summary>
        public uint bitmapDataByteCount;

        /// <summary>
        /// A CLEARCODEC_SUBCODEC_BMP_INFO structure, specifies subcodecID and encoded data.
        /// </summary>
        public CLEARCODEC_SUBCODEC_BMP_INFO bitmapData;
        
    }

    public struct CLEARCODEC_SUBCODEC_BMP_INFO
    {
        /// <summary>
        /// A 8-bit unsigned integer, specifies the encoding scheme used to encode the data in the bitmapData field.
        /// </summary>
        public CLEARCODEC_SUBCODEC_ID subCodecId;
        /// <summary>
        /// A variable-length array of RLEX_RGB_TRIPLET,  contains raw bitmap data.
        /// </summary>
        public RLEX_RGB_TRIPLET[] bmpPixels;
        /// <summary>
        /// A CLEARCODEC_SUBCODEC_RLEX structure,  contains RLEX encoded bitmap data.
        /// </summary>
        public CLEARCODEC_SUBCODEC_RLEX bmpRlex;
    }

    /// <summary>
    /// contains a palette and segments that contain encoded indexes that reference colors in the palette.
    /// </summary>
    public struct CLEARCODEC_SUBCODEC_RLEX
    {
        /// <summary>
        /// An 8-bit unsigned integer, specifies the number of RLEX_RGB_TRIPLET structures in the paletteEntries field. 
        /// </summary>
        public byte paletteCount;
        /// <summary>
        /// A variable-length array of RLEX_RGB_TRIPLET structures. The number of elements in this array is 
        /// specified by the paletteCount field.
        /// </summary>
        public RLEX_RGB_TRIPLET[] paletteEntries;
        /// <summary>
        /// A variable-length array of CLEARCODEC_SUBCODEC_RLEX_SEGMENT structures. The number of segments is decided by "bitmapData"
        /// </summary>
        public CLEARCODEC_SUBCODEC_RLEX_SEGMENT[] segments;
    }

    /// <summary>
    /// contains a collection of encoded palette indexes.
    /// </summary>
    public struct CLEARCODEC_SUBCODEC_RLEX_SEGMENT
    {
        /// <summary>
        /// A variable number of bits (maximum 8 bits) that defines an unsigned integer. 
        /// The bit number is given by floor(log2(paletteCount – 1)) + 1.
        /// </summary>
        public byte stopIndex;
        /// <summary>
        /// A variable number of bits (maximum 8 bits) that defines an unsigned integer.
        /// The sum of the number of bits in this field and the stopIndex field MUST equal 8, 
        /// and the bits in the suiteDepth field are present in the most significant bits of the containing byte.
        /// </summary>
        public byte suiteDepth;
        /// <summary>
        /// A 32-bit unsigned integer, specifies the startColor value MUST be applied to the next runLengthFactor pixels. 
        /// </summary>
        public uint runLengthFactor;

    }

    /// <summary>
    /// ClearCodecBandEncoder contains the third layer of pixels in an encoded image. 
    /// This layer MUST be decoded on top of the 2nd layer, in some cases overwriting pixels in the 1st and 2nd layer.
    /// </summary>
    class ClearCodecSubCodecEncoder
    {
        // demo data for test only
        public static byte[] subcodecRlexData = new byte[117] { 0x0e, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0xdb, 
                                            0xff, 0xff, 0x00, 0x3a, 0x90, 0xff, 0xb6, 0x66, 
                                            0x66, 0xb6, 0xff, 0xb6, 0x66, 0x00, 0x90, 0xdb, 
                                            0xff, 0x00, 0x00, 0x3a, 0xdb, 0x90, 0x3a, 0x3a, 
                                            0x90, 0xdb, 0x66, 0x00, 0x00, 0xff, 0xff, 0xb6, 
                                            0x64, 0x64, 0x64, 0x11, 0x04, 0x11, 0x4c, 0x11, 
                                            0x4c, 0x11, 0x4c, 0x11, 0x4c, 0x11, 0x4c, 0x00, 
                                            0x47, 0x13, 0x00, 0x01, 0x01, 0x04, 0x00, 0x01, 
                                            0x00, 0x00, 0x47, 0x16, 0x00, 0x11, 0x02, 0x00, 
                                            0x47, 0x29, 0x00, 0x11, 0x01, 0x00, 0x49, 0x0a, 
                                            0x00, 0x01, 0x00, 0x04, 0x00, 0x01, 0x00, 0x00, 
                                            0x4a, 0x0a, 0x00, 0x09, 0x00, 0x01, 0x00, 0x00, 
                                            0x47, 0x05, 0x00, 0x01, 0x01, 0x1c, 0x00, 0x01, 
                                            0x00, 0x11, 0x4c, 0x11, 0x4c, 0x11, 0x4c, 0x00, 
                                            0x47, 0x0d, 0x4d, 0x00, 0x4d };

        /// <summary>
        /// convert a color into RLEX_RGB_TRIPLET structure
        /// </summary>
        /// <param name="pixelColor">The color to be converted.</param>
        public static RLEX_RGB_TRIPLET Convert2RGB(Color pixelColor)
        {
            
            RLEX_RGB_TRIPLET rgbPlette = new RLEX_RGB_TRIPLET();
            rgbPlette.B = pixelColor.B;
            rgbPlette.G = pixelColor.G;
            rgbPlette.R = pixelColor.R;
            return rgbPlette;
        }

        /// <summary>
        /// Go through the bitmap to find all unique color
        /// </summary>
        /// <param name="subcodeBmp">The bitmap to be encoded in subcodec layer.</param>
        /// <return> a plette dictionary of all unique pixel and it's index in plette  </return>
        public static Dictionary<RLEX_RGB_TRIPLET, byte> GetTripletDict(Bitmap bitmap)
        {
            Dictionary<RLEX_RGB_TRIPLET, byte> pletteIndexDict = new Dictionary<RLEX_RGB_TRIPLET,byte>();
            byte index =0;

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++) 
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    RLEX_RGB_TRIPLET rgb = Convert2RGB(pixelColor);

                    if (!pletteIndexDict.ContainsKey(rgb))
                    {
                        pletteIndexDict.Add(rgb, index);
                        index++;
                    }
                    
                }
            }
            return pletteIndexDict;
        }

        /// <summary>
        /// Find the index of a color in pletteDict
        /// </summary>
        /// <param name="pixelColor">The color to search.</param>
        /// <param name="pletteDict">The plette dictionary to be searched.</param>
        /// <return> the index of a pixel in plette  </return>
        public static byte ColorToPletIdx(Color pixelColor, Dictionary<RLEX_RGB_TRIPLET, byte> pletteDict)
        {
            RLEX_RGB_TRIPLET rgbPlette = Convert2RGB(pixelColor);

            return pletteDict[rgbPlette];
        }   

        /// <summary>
        /// Encode a bitmap with RLEX method
        /// </summary>
        /// <param name="subcodeBmp">The bitmap to be encoded in subcodec layer.</param>
        /// <return> > 0 if encode success, otherwise return 0 </return>
        public static bool RlexEncode(Bitmap subcodecBmp, ref CLEARCODEC_SUBCODEC_RLEX rlex)
        {
            if (subcodecBmp == null) return false;

            Dictionary<RLEX_RGB_TRIPLET, byte> pletteDict = GetTripletDict(subcodecBmp);

            // if the bitmap color number is bigger than 255, RLEX subcodec encoding can't be applied.
            if (pletteDict.Count() > 0xff)
                return false;           

            rlex.paletteCount = (byte)pletteDict.Count();
            rlex.paletteEntries = new RLEX_RGB_TRIPLET[rlex.paletteCount];
            int index=0;
            foreach (KeyValuePair<RLEX_RGB_TRIPLET, byte> triPlette in pletteDict)
            {
                rlex.paletteEntries[index] = triPlette.Key;
                index++;
            }

            List<CLEARCODEC_SUBCODEC_RLEX_SEGMENT> rlexSegList = new List<CLEARCODEC_SUBCODEC_RLEX_SEGMENT>();
            CLEARCODEC_SUBCODEC_RLEX_SEGMENT rlex_seg = new CLEARCODEC_SUBCODEC_RLEX_SEGMENT();
            
            rlex_seg.suiteDepth = 0; // we always set suite depth is 0 due to only 1 color in a suite
            rlex_seg.stopIndex = ColorToPletIdx(subcodecBmp.GetPixel(0, 0), pletteDict);
            rlex_seg.runLengthFactor = 0;   // the first pixel in blow loop is at (0, 0), rlfactor is init as 0 here.

            for (int y = 0; y < subcodecBmp.Height; y++)  
            {
                for (int x = 0; x < subcodecBmp.Width; x++)
                {
                    if (x == 0 && y == 0) continue; // skip the pixel (0, 0)
                    Color pixelColor = subcodecBmp.GetPixel(x, y);
                    byte pletIdx = ColorToPletIdx(pixelColor, pletteDict);
                    
                    if (rlex_seg.stopIndex == pletIdx)  // same color as previous one
                    {
                        rlex_seg.runLengthFactor++;
                    }
                    else
                    {
                        // add old structure into list                          
                        rlexSegList.Add(rlex_seg);
                        // create a new structure for the pixel
                        rlex_seg = new CLEARCODEC_SUBCODEC_RLEX_SEGMENT();
                        rlex_seg.suiteDepth = 0;
                        rlex_seg.stopIndex = pletIdx;
                        rlex_seg.runLengthFactor = 0;
                    }
                }

            }
            // add the final structure into list
            rlexSegList.Add(rlex_seg);

            rlex.segments = rlexSegList.ToArray();

            return true;
        }

        /// <summary>
        /// Encode a bitmap with 
        /// </summary>
        /// <param name="subcodeBmp">The bitmap to be encoded in subcodec layer.</param>
        /// <return> > 0 if encode success, otherwise return 0 </return>
        public static bool RawEncode(Bitmap subcodecBmp, ref RLEX_RGB_TRIPLET[] bmpPixels)
        {
            if (subcodecBmp == null) return false;

            List<RLEX_RGB_TRIPLET> pixelList = new List<RLEX_RGB_TRIPLET>();

            for (int y = 0; y < subcodecBmp.Height; y++)          
            {
                for (int x = 0; x < subcodecBmp.Width; x++)  
                {
                    Color pixelColor = subcodecBmp.GetPixel(x, y);
                    RLEX_RGB_TRIPLET rgbPlette = Convert2RGB(pixelColor);
                    pixelList.Add(rgbPlette);       
                }
            }
            bmpPixels = pixelList.ToArray();
            return true ;
        }



        /// <summary>
        /// Encode a subcodec bitmap area into byte stream
        /// </summary>
        /// <param name="subcodeBmp">The bitmap to be encoded in subcodec layer.</param>
        /// <param name="subcodecRect">The subcodec area relative to bitmap left-top edge.</param>
        /// <param name="subcodecID">The subcodec ID to be chosen.</param>
        /// <param name="subcodec">The subcodec layer encode result.</param>
        public static bool EncodeSubcodec(Bitmap subcodecBmp, ClearCodec_RECT16 subcodecRect, CLEARCODEC_SUBCODEC_ID subcodecID, 
            ref CLEARCODEC_SUBCODEC subcodec)
        {
            if (subcodecBmp == null) return false;

            subcodec.xStart = subcodecRect.left;
            subcodec.yStart = subcodecRect.top;
            subcodec.width = (ushort)(subcodecRect.right - subcodecRect.left);
            subcodec.height = (ushort)(subcodecRect.bottom - subcodecRect.top);
            // save subcodecID
            subcodec.bitmapData.subCodecId = subcodecID;
            // get encoded data based on subcodecID
            if (subcodecID == CLEARCODEC_SUBCODEC_ID.SUBCODEC_RLEX)
            {
                // RLEX encoded bitmap data
                if( !RlexEncode(subcodecBmp, ref subcodec.bitmapData.bmpRlex)) return false;
                
            } 
            else 
            {
                // raw encoded bitmap data
                if (!RawEncode(subcodecBmp, ref subcodec.bitmapData.bmpPixels)) return false; 
            }

            return true;
        }

        /// <summary>
        /// Encode multiple bitmap areas via subcodec into byte stream
        /// </summary>
        /// <param name="bandDict">The structure saves multiple subcodec layer bitmap, subcodecID and position.</param>
        public static CLEARCODEC_SUBCODEC_DATA Encode(Dictionary<ClearCodec_RECT16, BMP_INFO> subcodecDict)
        {
            List<CLEARCODEC_SUBCODEC> subcodecList = new List<CLEARCODEC_SUBCODEC>();

            foreach (KeyValuePair<ClearCodec_RECT16, BMP_INFO> scArea in subcodecDict)
            {
                CLEARCODEC_SUBCODEC scData = new CLEARCODEC_SUBCODEC();
                if (!EncodeSubcodec((Bitmap)(scArea.Value.bmp), scArea.Key, scArea.Value.scID, ref scData))
                {
                    // skip a subcodec layer bitmap if it failed to be encoded.
                    continue;
                }

                subcodecList.Add(scData);
            }

            CLEARCODEC_SUBCODEC_DATA subcodecs = new CLEARCODEC_SUBCODEC_DATA();
            subcodecs.subcodecArr = subcodecList.ToArray();

            return subcodecs;
        }
    }

    
}
