// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdpegfx
{
    public partial class RdpegfxTestSuite : RdpTestClassBase
    {
        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("DeviceNeeded")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify the client support H264 bitmap codec via RDPEGFX.")]
        public void RDPEGFX_H264Codec_PositiveTest_H264Support()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();
            bool h264Supported = IsH264Supported();
            this.TestSite.Assert.IsTrue(h264Supported, "To test H264 codec, client must indicates support for H264 codec in RDPGFX_CAPS_ADVERTISE_PDU");
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("DeviceNeeded")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC420_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode and Base profile.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC420_BaseProfile()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, false);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("DeviceNeeded")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC420_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode and High profile.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC420_HighProfile()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, false);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("DeviceNeeded")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC420_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode and Main profile.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC420_MainProfile()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, false);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("DeviceNeeded")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC420_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode, Main profile and CABAC entropy coding mode.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC420_MainProfile_CABACEnabled()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, false);
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode and Main profile, only send the first frame (I slice)")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444_YUV420Only_MainProfile_ISliceOnly()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode and Base profile.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444_YUV420Only_BaseProfile()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }


        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode and Main profile.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444_YUV420Only_MainProfile()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode, Main profile and CABAC entropy coding mode.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444_YUV420Only_MainProfile_CABACEnabled()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode and Main profile, the image size is large (1024*768).")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444_YUV420Only_MainProfile_LargeSize()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444_BITMAP_STREAM structure with H264 encoded bitmap using YUV420p mode and High profile.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444_YUV420Only_HighProfile()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444_BITMAP_STREAM structure with H264 encoded bitmap using YUV444 mode and Main profile,  luma frame and chroma frame are separated in two RFX_AVC444_BITMAP_STREAM structures.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444_YUV420Chroma420Separated_MainProfile()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444_BITMAP_STREAM structure with H264 encoded bitmap using YUV444 mode and Main profile, luma frame and chroma frame are together in one RFX_AVC444_BITMAP_STREAM structure.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444_YUV420Chroma420Together_MainProfile()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444_BITMAP_STREAM structure with H264 encoded bitmap using YUV444 mode and Main profile, the image size is large (1024*768), luma frame and chroma frame are together in one RFX_AVC444_BITMAP_STREAM structure.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444_YUV420Chroma420Together_MainProfile_LargeSize()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.0")]
        [TestCategory("RDPEGFX")]
        [Description("Verify client can accept a RFX_AVC444V2_BITMAP_STREAM structure with H264 encoded bitmap using YUV444v2 mode.")]
        public void RDPEGFX_H264Codec_PositiveTest_AVC444v2()
        {
            String h264TestDataPath = GetTestDataFile();
            SendH264CodecStream(h264TestDataPath, true);
        }

        #region private methods

        /// <summary>
        /// Common function to send H264 data to the client
        /// </summary>
        /// <param name="h264DataFile">XML file of H264 data</param>
        /// <param name="isAVC444">Whether need RDP client support AVC444/AVC444v2</param>
        private void SendH264CodecStream(string h264DataFile, bool isAVC444)
        {
            //Load H264 data
            RdpegfxH264TestDatas h264TestData = GetH264TestData(h264DataFile);

            // Init for capability exchange
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();
            if (isAVC444)
            {
                this.TestSite.Assume.IsTrue(this.isH264AVC444Supported, "This test case requires RDP client to support AVC444/AVC444v2.");
            }
            else
            {
                this.TestSite.Assume.IsTrue(this.isH264AVC420Supported, "To test H264 codec, client must indicate support for H264 codec in RDPGFX_CAPS_ADVERTISE_PDU");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_POINT16 surfPos = new RDPGFX_POINT16((ushort)h264TestData.SurfaceInfo.outputOriginX, (ushort)h264TestData.SurfaceInfo.outputOriginY);
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(surfPos, h264TestData.SurfaceInfo.width, h264TestData.SurfaceInfo.height);
            RDPGFX_RECT16 compareRect = RdpegfxTestUtility.ConvertToRect(surfPos, h264TestData.SurfaceInfo.width, h264TestData.SurfaceInfo.height);
            if (isWindowsImplementation && compareRect.top < 32 && compareRect.bottom > 32)
            {
                // Ignore the field of RDP client connection bar
                compareRect.top = 32;
            }
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, (PixelFormat)h264TestData.SurfaceInfo.pixelFormat);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color            
            RDPGFX_RECT16[] fillRects = { new RDPGFX_RECT16(h264TestData.TestDataList[0].DestRect.left,
                h264TestData.TestDataList[0].DestRect.top,
                h264TestData.TestDataList[0].DestRect.right,
                h264TestData.TestDataList[0].DestRect.bottom)};  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Send H264 codec data 
            foreach (TestData data in h264TestData.TestDataList)
            {
                ushort codecId = data.codecId;
                PixelFormat pixFormat = (PixelFormat)data.pixelFormat;
                RDPGFX_RECT16 bmRect = new RDPGFX_RECT16();
                bmRect.left = data.DestRect.left;
                bmRect.top = data.DestRect.top;
                bmRect.right = data.DestRect.right;
                bmRect.bottom = data.DestRect.bottom;

                if (codecId == (ushort)CodecType.RDPGFX_CODECID_AVC420 && data.AVC420BitmapStream != null)
                {

                    this.TestSite.Log.Add(LogEntryKind.Comment, "Sending H264 AVC420 Encoded Bitmap Data Messages to client.");
                    fid = this.rdpegfxAdapter.SendImageWithH264AVC420Codec(surf.Id, pixFormat, bmRect, data.AVC420BitmapStream.To_RFX_AVC420_BITMAP_STREAM(), data.GetBaseImage());
                    // Test case pass if frame acknowledge is received.
                    this.rdpegfxAdapter.ExpectFrameAck(fid);
                }
                else if (codecId == (ushort)CodecType.RDPGFX_CODECID_AVC444 && data.AVC444BitmapStream != null)
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Sending H264 AVC444 Encoded Bitmap Data Messages to client.");
                    fid = this.rdpegfxAdapter.SendImageWithH264AVC444Codec(surf.Id, pixFormat, bmRect, CodecType.RDPGFX_CODECID_AVC444, data.AVC444BitmapStream.To_RFX_AVC444_BITMAP_STREAM(), data.GetBaseImage());
                    // Test case pass if frame acknowledge is received.
                    this.rdpegfxAdapter.ExpectFrameAck(fid);
                }
                else if (codecId == (ushort)CodecType.RDPGFX_CODECID_AVC444v2 && data.AVC444v2BitmapStream != null)
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Sending H264 AVC444v2 Encoded Bitmap Data Messages to client.");
                    fid = this.rdpegfxAdapter.SendImageWithH264AVC444Codec(surf.Id, pixFormat, bmRect, CodecType.RDPGFX_CODECID_AVC444v2, data.AVC444v2BitmapStream.To_RFX_AVC444V2_BITMAP_STREAM(), data.GetBaseImage());
                    // Test case pass if frame acknowledge is received.
                    this.rdpegfxAdapter.ExpectFrameAck(fid);
                }
                else
                {
                    Site.Assert.Fail("Test data doesn't contain proper H264 encoded data corresponding to codec ID.");
                }

                this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
                this.VerifySUTDisplay(true, compareRect, 2);

            }

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        /// <summary>
        /// Get Test Data file path
        /// </summary>
        /// <param name="callStackIndex"></param>
        /// <returns></returns>
        private string GetTestDataFile(int callStackIndex = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(callStackIndex);
            string testCaseName = sf.GetMethod().Name;

            String h264TestDataPath;
            if (!PtfPropUtility.GetStringPtfProperty(TestSite, "RdpegfxH264TestDataPath", out h264TestDataPath))
            {
                Site.Assert.Fail("Cannot get its test data path");
            }

            return h264TestDataPath + @"\" + testCaseName + ".xml";
        }

        #endregion private methods
    }
}