// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The RDPGFX_AVC420_QUANT_QUALITY structure describes the quantization parameter 
    /// and quality level associated with a rectangular region.
    /// </summary>
    public struct RDPGFX_AVC420_QUANT_QUALITY
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the progressive indicator 
        /// and quantization parameter associated with a rectangular region. 
        /// </summary>
        public byte qpVal;
        /// <summary>
        /// An 8-bit unsigned integer that specifies the quality level associated with a rectangular region. 
        /// This value MUST be in the range 0 to 100 inclusive.
        /// </summary>
        public byte qualityVal;
    }

    /// <summary>
    /// The RFX_AVC420_METABLOCK structure describes metadata associated 
    /// with H.264 encoded data sent from the server to the client.
    /// </summary>
    public struct RFX_AVC420_METABLOCK
    {
        /// <summary>
        /// A 32-bit unsigned integer that specifies the total number of elements in the regionRects field.
        /// </summary>
        public uint numRegionRects;
        /// <summary>
        /// A variable-length array of RDPGFX_RECT16 structures that specifies the region mask to apply to the H.264 encoded data. 
        /// </summary>
        public RDPGFX_RECT16[] regionRects;
        /// <summary>
        /// A variable-length array of RDPGFX_H264_QUANT_QUALITY structures that describes the quantization parameter 
        /// and quality level associated with each rectangle in the regionRects field. 
        /// </summary>
        public RDPGFX_AVC420_QUANT_QUALITY[] quantQualityVals;
    }

    /// <summary>
    /// The RFX_AVC420_BITMAP_STREAM structure encapsulates regions of 
    /// a graphics frame compressed using MPEG-4 AVC/H.264 compression techniques [ITU-H.264-201201] in YUV420p mode. 
    /// </summary>
    public class RFX_AVC420_BITMAP_STREAM
    {
        /// <summary>
        /// A variable-length RFX_AVC420_METABLOCK structure.
        /// </summary>
        public RFX_AVC420_METABLOCK avc420MetaData;

        /// <summary>
        /// An array of bytes that represents a single frame encoded using the H.264 codec.
        /// </summary>
        public byte[] avc420EncodedBitstream;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="h264MetaData">h264MetaData field</param>
        /// <param name="h264EncodedBitstream">h264EncodedBitstream field</param>
        public RFX_AVC420_BITMAP_STREAM(RFX_AVC420_METABLOCK avc420MetaData, byte[] avc420EncodedBitstream)
        {
            this.avc420EncodedBitstream = avc420EncodedBitstream;
            this.avc420MetaData = avc420MetaData;
        }

        /// <summary>
        /// Encode to binary
        /// </summary>
        /// <returns></returns>
        public byte[] Encode()
        {
            List<byte> bufList = new List<byte>();

            bufList.AddRange(TypeMarshal.ToBytes<uint>(this.avc420MetaData.numRegionRects));

            if (this.avc420MetaData.regionRects != null)
            {
                foreach (RDPGFX_RECT16 rect in this.avc420MetaData.regionRects)
                {
                    bufList.AddRange(TypeMarshal.ToBytes<ushort>(rect.left));
                    bufList.AddRange(TypeMarshal.ToBytes<ushort>(rect.top));
                    bufList.AddRange(TypeMarshal.ToBytes<ushort>(rect.right));
                    bufList.AddRange(TypeMarshal.ToBytes<ushort>(rect.bottom));
                }
            }
            if (this.avc420MetaData.quantQualityVals != null)
            {
                foreach (RDPGFX_AVC420_QUANT_QUALITY quality in this.avc420MetaData.quantQualityVals)
                {
                    bufList.AddRange(TypeMarshal.ToBytes<byte>(quality.qpVal));
                    bufList.AddRange(TypeMarshal.ToBytes<byte>(quality.qualityVal));
                }
            }

            if (this.avc420EncodedBitstream != null)
            {
                bufList.AddRange(this.avc420EncodedBitstream);
            }
            return bufList.ToArray();
        }
    }

    /// <summary>
    /// Value of LC field in RFX_AVC444_BITMAP_STREAM
    /// </summary>
    public enum AVC444LCValue : byte
    {
        /// <summary>
        /// A luma frame is contained in the avc420EncodedBitstream1 field, 
        /// and a chroma frame is contained in the avc420EncodedBitstream2 field.
        /// </summary>
        BothLumaChroma = 0x00,

        /// <summary>
        /// A luma frame is contained in the avc420EncodedBitstream1 field, 
        /// and no data is present in the avc420EncodedBitstream2 field. No chroma frame is present.
        /// </summary>
        OnlyLuma = 0x01,

        /// <summary>
        /// A chroma frame is contained in avc420EncodedBitstream1 field, 
        /// and no data is present in the avc420EncodedBitstream2 field. No luma frame is present.
        /// </summary>
        OnlyChroma = 0x02,

        /// <summary>
        /// An invalid state that MUST NOT occur.
        /// </summary>
        Invalid = 0x03
    }

    /// <summary>
    /// Common interface for AVC444/AVC444v2 bitmap stream.
    /// </summary>
    public interface IRFX_AVC444_BITMAP_STREAM
    {
        /// <summary>
        /// Encode bitmap stream.
        /// </summary>
        /// <returns>Array of bytes containing bitmap stream.</returns>
        byte[] Encode();
    }

    /// <summary>
    /// The RFX_AVC444_BITMAP_STREAM structure encapsulates regions of 
    /// a graphics frame compressed using MPEG-4 AVC/H.264 compression techniques [ITU-H.264-201201] in YUV444 mode. 
    /// </summary>
    public class RFX_AVC444_BITMAP_STREAM : IRFX_AVC444_BITMAP_STREAM
    {
        /// <summary>
        /// A 30-bit unsigned integer that specifies the size, in bytes, of the luma frame present in the avc420EncodedBitstream1 field. 
        /// If no luma frame is present, then this field MUST be set to zero.
        /// </summary>
        public uint cbAvc420EncodedBitstream1;

        public AVC444LCValue LC;

        /// <summary>
        /// An RFX_AVC420_BITMAP_STREAM structure that contains the first YUV420p subframe of a single frame 
        /// that was encoded using the MPEG-4 AVC/H.264 codec in YUV444 mode.
        /// </summary>
        public RFX_AVC420_BITMAP_STREAM avc420EncodedBitstream1;

        /// <summary>
        /// An RFX_AVC420_BITMAP_STREAM structure that contains the second YUV420p subframe (if it exists)
        /// of a single frame that was encoded using the MPEG-4 AVC/H.264 codec in YUV444 mode.
        /// </summary>
        public RFX_AVC420_BITMAP_STREAM avc420EncodedBitstream2;

        public RFX_AVC444_BITMAP_STREAM(AVC444LCValue lc, RFX_AVC420_BITMAP_STREAM avc420EncodedBitstream1, RFX_AVC420_BITMAP_STREAM avc420EncodedBitstream2)
        {
            this.LC = lc;
            this.avc420EncodedBitstream1 = avc420EncodedBitstream1;
            this.avc420EncodedBitstream2 = avc420EncodedBitstream2;
        }

        /// <summary>
        /// Encode to binary
        /// </summary>
        /// <returns></returns>
        public byte[] Encode()
        {
            List<byte> bufList = new List<byte>();
            byte[] bitStream1 = null;
            if (avc420EncodedBitstream1 != null)
                bitStream1 = avc420EncodedBitstream1.Encode();
            byte[] bitStream2 = null;
            if (avc420EncodedBitstream2 != null)
                bitStream2 = avc420EncodedBitstream2.Encode();

            if (LC == AVC444LCValue.BothLumaChroma || LC == AVC444LCValue.OnlyLuma)
            {
                cbAvc420EncodedBitstream1 = (uint)bitStream1.Length;
            }

            uint avc420EncodedBitstreamInfo = (cbAvc420EncodedBitstream1 | (((uint)LC) << 30));
            bufList.AddRange(TypeMarshal.ToBytes<uint>(avc420EncodedBitstreamInfo));
            if (bitStream1 != null)
            {
                bufList.AddRange(bitStream1);
            }
            if (bitStream2 != null)
            {
                bufList.AddRange(bitStream2);
            }

            return bufList.ToArray();
        }
    }

    public class RFX_AVC444V2_BITMAP_STREAM : IRFX_AVC444_BITMAP_STREAM
    {
        /// <summary>
        /// A 30-bit unsigned integer that specifies the size, in bytes, of the luma frame present in the avc420EncodedBitstream1 field. 
        /// If no luma frame is present, then this field MUST be set to zero.
        /// </summary>
        public uint cbAvc420EncodedBitstream1;

        public AVC444LCValue LC;

        /// <summary>
        /// An RFX_AVC420_BITMAP_STREAM structure that contains the first YUV420p subframe of a single frame 
        /// that was encoded using the MPEG-4 AVC/H.264 codec in YUV444 mode.
        /// </summary>
        public RFX_AVC420_BITMAP_STREAM avc420EncodedBitstream1;

        /// <summary>
        /// An RFX_AVC420_BITMAP_STREAM structure that contains the second YUV420p subframe (if it exists)
        /// of a single frame that was encoded using the MPEG-4 AVC/H.264 codec in YUV444 mode.
        /// </summary>
        public RFX_AVC420_BITMAP_STREAM avc420EncodedBitstream2;

        public RFX_AVC444V2_BITMAP_STREAM(AVC444LCValue lc, RFX_AVC420_BITMAP_STREAM avc420EncodedBitstream1, RFX_AVC420_BITMAP_STREAM avc420EncodedBitstream2)
        {
            this.LC = lc;
            this.avc420EncodedBitstream1 = avc420EncodedBitstream1;
            this.avc420EncodedBitstream2 = avc420EncodedBitstream2;
        }

        /// <summary>
        /// Encode to binary
        /// </summary>
        /// <returns></returns>
        public byte[] Encode()
        {
            List<byte> bufList = new List<byte>();
            byte[] bitStream1 = null;
            if (avc420EncodedBitstream1 != null)
                bitStream1 = avc420EncodedBitstream1.Encode();
            byte[] bitStream2 = null;
            if (avc420EncodedBitstream2 != null)
                bitStream2 = avc420EncodedBitstream2.Encode();

            if (LC == AVC444LCValue.BothLumaChroma || LC == AVC444LCValue.OnlyLuma)
            {
                cbAvc420EncodedBitstream1 = (uint)bitStream1.Length;
            }

            uint avc420EncodedBitstreamInfo = (cbAvc420EncodedBitstream1 | (((uint)LC) << 30));
            bufList.AddRange(TypeMarshal.ToBytes<uint>(avc420EncodedBitstreamInfo));
            if (bitStream1 != null)
            {
                bufList.AddRange(bitStream1);
            }
            if (bitStream2 != null)
            {
                bufList.AddRange(bitStream2);
            }

            return bufList.ToArray();
        }
    }

    #region Types for test data
    /// <summary>
    /// Type to store Test data for RDPEGFX H264 codec
    /// </summary>
    [XmlRoot]
    public class RdpegfxH264TestDatas
    {

        public SurfaceInfo SurfaceInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Test Data list
        /// </summary>
        [XmlElementAttribute("TestData")]
        public List<TestData> TestDataList
        {
            get;
            set;
        }

    }

    public class SurfaceInfo
    {
        /// <summary>
        /// string: The Width of the surface to create
        /// </summary>
        [XmlElementAttribute]
        public string Width
        {
            get;
            set;
        }

        /// <summary>
        /// string: The Height of the surface to create
        /// </summary>
        [XmlElementAttribute]
        public string Height
        {
            get;
            set;
        }

        /// <summary>
        /// string: The PixelFormat of the surface to create
        /// </summary>
        [XmlElementAttribute]
        public string PixelFormat
        {
            get;
            set;
        }

        /// <summary>
        /// string: The x-coordinate on the output to map this surface
        /// </summary>
        [XmlElementAttribute]
        public string OutputOriginX
        {
            get;
            set;
        }

        /// <summary>
        /// string: The y-coordinate on the output to map this surface
        /// </summary>
        [XmlElementAttribute]
        public string OutputOriginY
        {
            get;
            set;
        }

        /// <summary>
        /// ushort: The width of the surface to create
        /// </summary>
        public ushort width
        {
            get
            {
                ushort res = 0;
                try
                {
                    if (Width.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToUInt16(Width, 16);
                    }
                    else
                    {
                        res = ushort.Parse(Width);
                    }
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// ushort: The height of the surface to create
        /// </summary>
        public ushort height
        {
            get
            {
                ushort res = 0;
                try
                {
                    if (Height.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToUInt16(Height, 16);
                    }
                    else
                    {
                        res = ushort.Parse(Height);
                    }
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// byte: The PixelFormat of the surface to create
        /// </summary>
        public byte pixelFormat
        {
            get
            {
                byte res = 0;
                try
                {
                    if (PixelFormat.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToByte(PixelFormat, 16);
                    }
                    else
                    {
                        res = byte.Parse(PixelFormat);
                    }
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// uint: The x-coordinate on the output to map this surface
        /// </summary>
        public uint outputOriginX
        {
            get
            {
                uint res = 0;
                try
                {
                    if (OutputOriginX.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToUInt32(OutputOriginX, 16);
                    }
                    else
                    {
                        res = uint.Parse(OutputOriginX);
                    }
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// uint: The y-coordinate on the output to map this surface
        /// </summary>
        public uint outputOriginY
        {
            get
            {
                uint res = 0;
                try
                {
                    if (OutputOriginY.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToUInt32(OutputOriginY, 16);
                    }
                    else
                    {
                        res = uint.Parse(OutputOriginY);
                    }
                }
                catch { };
                return res;
            }
        }
    }

    /// <summary>
    /// Type for Test data
    /// </summary>
    public class TestData
    {
        /// <summary>
        /// string: codec id of encoded stream, 0x0B or 0x0E
        /// </summary>
        [XmlElementAttribute]
        public string CodecId
        {
            get;
            set;
        }

        /// <summary>
        /// string: The PixelFormat of the surface to create
        /// </summary>
        [XmlElementAttribute]
        public string PixelFormat
        {
            get;
            set;
        }


        /// <summary>
        /// Dest rect
        /// </summary>
        [XmlElementAttribute]
        public Rect DestRect
        {
            get;
            set;
        }

        /// <summary>
        /// Encoded Data for AVC 420 mode
        /// </summary>
        [XmlElementAttribute]
        public AVC420BitmapStream AVC420BitmapStream
        {
            get;
            set;
        }

        /// <summary>
        /// Encoded Data for AVC 444 mode
        /// </summary>
        [XmlElementAttribute]
        public AVC444BitmapStream AVC444BitmapStream
        {
            get;
            set;
        }

        /// <summary>
        /// Encoded Data for AVC 444 v2 mode
        /// </summary>
        [XmlElementAttribute]
        public AVC444v2BitmapStream AVC444v2BitmapStream
        {
            get;
            set;
        }

        /// <summary>
        /// string: Data for base image, a serialize bitmap encoded to base64 string.
        /// </summary>
        [XmlElementAttribute]
        public string BaseImage
        {
            get;
            set;
        }
        /// <summary>
        /// ushort: The height of the surface to create
        /// </summary>
        public ushort codecId
        {
            get
            {
                ushort res = 0;
                try
                {
                    if (CodecId.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToUInt16(CodecId, 16);
                    }
                    else
                    {
                        res = ushort.Parse(CodecId);
                    }
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// byte: The PixelFormat of the surface to create
        /// </summary>
        public byte pixelFormat
        {
            get
            {
                byte res = 0;
                try
                {
                    if (PixelFormat.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToByte(PixelFormat, 16);
                    }
                    else
                    {
                        res = byte.Parse(PixelFormat);
                    }
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// BaseImage string to an image
        /// </summary>
        /// <returns></returns>
        public Image GetBaseImage()
        {
            try
            {
                if (!string.IsNullOrEmpty(BaseImage))
                {
                    Image image = Bitmap.FromFile(BaseImage);
                    return image;
                }
            }
            catch { }
            return null;
        }

    }
    public class AVC444BitmapStream
    {
        /// <summary>
        /// string: LC code
        /// </summary>
        [XmlElementAttribute]
        public string LC
        {
            get;
            set;
        }

        /// <summary>
        /// First YUV420p subframe 
        /// </summary>
        [XmlElementAttribute]
        public AVC420BitmapStream AVC420EncodedBitstream1
        {
            get;
            set;
        }

        /// <summary>
        /// Second YUV420p subframe 
        /// </summary>
        [XmlElementAttribute]
        public AVC420BitmapStream AVC420EncodedBitstream2
        {
            get;
            set;
        }

        public byte LCCode
        {
            get
            {
                byte res = 0;
                try
                {
                    if (LC.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToByte(LC, 16);
                    }
                    res = byte.Parse(LC);
                }
                catch { }
                return res;
            }
        }

        /// <summary>
        /// Convert this AVC444BitmapStream to RFX_AVC444_BITMAP_STREAM
        /// </summary>
        public RFX_AVC444_BITMAP_STREAM To_RFX_AVC444_BITMAP_STREAM()
        {
            RFX_AVC420_BITMAP_STREAM stream1 = null;
            if (AVC420EncodedBitstream1 != null)
                stream1 = AVC420EncodedBitstream1.To_RFX_AVC420_BITMAP_STREAM();
            RFX_AVC420_BITMAP_STREAM stream2 = null;
            if (AVC420EncodedBitstream2 != null)
                stream2 = AVC420EncodedBitstream2.To_RFX_AVC420_BITMAP_STREAM();
            return new RFX_AVC444_BITMAP_STREAM((AVC444LCValue)LCCode, stream1, stream2);
        }
    }
    public class AVC444v2BitmapStream
    {
        /// <summary>
        /// string: LC code
        /// </summary>
        [XmlElementAttribute]
        public string LC
        {
            get;
            set;
        }

        /// <summary>
        /// First YUV420p subframe 
        /// </summary>
        [XmlElementAttribute]
        public AVC420BitmapStream AVC420EncodedBitstream1
        {
            get;
            set;
        }

        /// <summary>
        /// Second YUV420p subframe 
        /// </summary>
        [XmlElementAttribute]
        public AVC420BitmapStream AVC420EncodedBitstream2
        {
            get;
            set;
        }

        public byte LCCode
        {
            get
            {
                byte res = 0;
                try
                {
                    if (LC.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToByte(LC, 16);
                    }
                    res = byte.Parse(LC);
                }
                catch { }
                return res;
            }
        }

        /// <summary>
        /// Convert this AVC444v2BitmapStream to RFX_AVC444V2_BITMAP_STREAM
        /// </summary>
        public RFX_AVC444V2_BITMAP_STREAM To_RFX_AVC444V2_BITMAP_STREAM()
        {
            RFX_AVC420_BITMAP_STREAM stream1 = null;
            if (AVC420EncodedBitstream1 != null)
                stream1 = AVC420EncodedBitstream1.To_RFX_AVC420_BITMAP_STREAM();
            RFX_AVC420_BITMAP_STREAM stream2 = null;
            if (AVC420EncodedBitstream2 != null)
                stream2 = AVC420EncodedBitstream2.To_RFX_AVC420_BITMAP_STREAM();
            return new RFX_AVC444V2_BITMAP_STREAM((AVC444LCValue)LCCode, stream1, stream2);
        }
    }
    public class AVC420BitmapStream
    {
        /// <summary>
        /// Number of rects.
        /// </summary>
        [XmlElementAttribute]
        public string NumRegionRects
        {
            get;
            set;
        }

        /// <summary>
        /// region rects of image
        /// </summary>
        [XmlElementAttribute]
        public RegionRects RegionRects
        {
            get;
            set;
        }

        /// <summary>
        /// Encoded stream
        /// </summary>
        [XmlElementAttribute]
        public string AVC420EncodedBitstream
        {
            get;
            set;
        }

        public uint numRegionRects
        {
            get
            {
                uint res = 0;
                try
                {
                    if (NumRegionRects.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                    {
                        res = Convert.ToUInt32(NumRegionRects, 16);
                    }
                    else
                    {
                        res = uint.Parse(NumRegionRects);
                    }
                }
                catch { };
                return res;
            }
        }
        /// <summary>
        /// Get byte array of encoded stream
        /// </summary>
        /// <returns></returns>
        public byte[] GetAVC420EncodedBitstream()
        {
            if (AVC420EncodedBitstream != null)
            {
                byte[] data = ParseStringToByteArray(AVC420EncodedBitstream.Trim());
                return data;
            }
            return null;
        }

        /// <summary>
        /// Convert this AVC420BitmapStream to a RFX_AVC420_BITMAP_STREAM
        /// </summary>
        /// <returns></returns>
        public RFX_AVC420_BITMAP_STREAM To_RFX_AVC420_BITMAP_STREAM()
        {
            RFX_AVC420_METABLOCK metaBlock = new RFX_AVC420_METABLOCK();
            metaBlock.numRegionRects = numRegionRects;
            List<RDPGFX_RECT16> regionList = new List<RDPGFX_RECT16>();
            List<RDPGFX_AVC420_QUANT_QUALITY> qualityList = new List<RDPGFX_AVC420_QUANT_QUALITY>();
            foreach (Rect rect in RegionRects.RectList)
            {
                regionList.Add(new RDPGFX_RECT16(rect.left, rect.top, rect.right, rect.bottom));
                RDPGFX_AVC420_QUANT_QUALITY quality = new RDPGFX_AVC420_QUANT_QUALITY();
                quality.qpVal = rect.Quality.qpVal;
                quality.qualityVal = rect.Quality.qualityVal;
                qualityList.Add(quality);
            }
            metaBlock.regionRects = regionList.ToArray();
            metaBlock.quantQualityVals = qualityList.ToArray();

            return new RFX_AVC420_BITMAP_STREAM(metaBlock, GetAVC420EncodedBitstream());
        }
        /// <summary>
        /// Parse a string to byte array
        /// </summary>
        /// <param name="arrayString"></param>
        /// <returns></returns>
        private byte[] ParseStringToByteArray(String arrayString)
        {
            String[] hexStringArray = arrayString.Split(new char[] { ' ', '\n' });
            List<byte> dataList = new List<byte>();
            for (int i = 0; i < hexStringArray.Length; i++)
            {
                String hexString = hexStringArray[i].Trim(new char[] { ' ', '\n', '\t', '\r' });
                if (!hexString.Equals(""))
                    dataList.Add(Convert.ToByte(hexString, 16));
            }
            return dataList.ToArray();
        }
    }
    /// <summary>
    /// Rect
    /// </summary>
    public class Rect
    {
        /// <summary>
        /// string: left
        /// </summary>
        [XmlElementAttribute]
        public string Left
        {
            get;
            set;
        }

        /// <summary>
        /// string: top
        /// </summary>
        [XmlElementAttribute]
        public string Top
        {
            get;
            set;
        }

        /// <summary>
        /// string: right
        /// </summary>
        [XmlElementAttribute]
        public string Right
        {
            get;
            set;
        }

        /// <summary>
        /// string: bottom
        /// </summary>
        [XmlElementAttribute]
        public string Bottom
        {
            get;
            set;
        }


        /// <summary>
        /// qualities value of this rect
        /// </summary>
        [XmlElementAttribute]
        public Quality Quality
        {
            get;
            set;
        }

        /// <summary>
        /// ushort: left
        /// </summary>
        public ushort left
        {
            get
            {
                ushort res = 0;
                try
                {
                    res = ushort.Parse(Left);
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// ushort: top
        /// </summary>
        public ushort top
        {
            get
            {
                ushort res = 0;
                try
                {
                    res = ushort.Parse(Top);
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// ushort: right
        /// </summary>
        public ushort right
        {
            get
            {
                ushort res = 0;
                try
                {
                    res = ushort.Parse(Right);
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// ushort: bottom
        /// </summary>
        public ushort bottom
        {
            get
            {
                ushort res = 0;
                try
                {
                    res = ushort.Parse(Bottom);
                }
                catch { };
                return res;
            }
        }
    }

    /// <summary>
    /// Region rects
    /// </summary>
    public class RegionRects
    {
        /// <summary>
        /// rect list
        /// </summary>
        [XmlElementAttribute("Rect")]
        public List<Rect> RectList
        {
            get;
            set;
        }
    }


    /// <summary>
    /// Quality
    /// </summary>
    public class Quality
    {
        /// <summary>
        /// string: progressive indicator and quantization parameter 
        /// </summary>
        [XmlElementAttribute]
        public string QpVal
        {
            get;
            set;
        }

        /// <summary>
        /// string: quality level associated with a rectangular region
        /// </summary>
        [XmlElementAttribute]
        public string QualityVal
        {
            get;
            set;
        }

        /// <summary>
        /// byte: progressive indicator and quantization parameter 
        /// </summary>
        public byte qpVal
        {
            get
            {
                byte res = 0;
                try
                {
                    res = byte.Parse(QpVal);
                }
                catch { };
                return res;
            }
        }

        /// <summary>
        /// byte: quality level associated with a rectangular region
        /// </summary>
        public byte qualityVal
        {
            get
            {
                byte res = 0;
                try
                {
                    res = byte.Parse(QualityVal);
                }
                catch { };
                return res;
            }
        }
    }
    #endregion
}
