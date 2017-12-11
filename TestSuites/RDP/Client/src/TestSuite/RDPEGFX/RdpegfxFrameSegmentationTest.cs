// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
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
        [Priority(0)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to verify if the segmented messages, the compress flag, and Rdpegfx Header can be handled correctly by RDP Client")]
        public void RDPEGFX_Segmentation_Positive_MultipleSegmentedPacking()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encode Header/Data Messages to client.");
            byte compFlag = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8;

            Image bgImage;
            Image compImage = RdpegfxTestUtility.captureFromImage(image_64X64, RdpegfxTestUtility.imgPos,
                                    RdpegfxTestUtility.ccLargeBandWidth, RdpegfxTestUtility.ccLargeBandWidth, out bgImage);
            // Send a big size bitmap with raw data exceeds 65535 bytes to client, multiple segmented packing are adopted for this message
            this.TestSite.Log.Add(LogEntryKind.Comment, "A large size bitmap is sent to the client using multipart datapacking for frame {0}.", fid);
            fid = this.rdpegfxAdapter.SendUncompressedImage(compImage, RdpegfxTestUtility.imgPos.x, RdpegfxTestUtility.imgPos.y, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, compFlag, RdpegfxTestUtility.segmentPartSize);
            
            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Delete the surface
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
        }



        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle an uncompressed message without segment header.")]
        public void RDPEGFX_Segmentation_Negative_UncompressedNoSegmentHeader()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            this.TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_Uncompressed_NoSegmentHeader);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_Uncompressed_NoSegmentHeader);

            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm message not using RDP 8.0 compression technology without segment header to client.");
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            RDPClientTryDropConnection("capability confirm message without RDP 8.0 compression technology without segment header");            
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle an uncompressed message with segment header.")]
        public void RDPEGFX_Segmentation_Positive_UncompressedSegmentHeader()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            this.TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Debug, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            // Set the test type, based on the test type, all the messages in this test case will not use RDP 8.0 Bulk Compression techniques
            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_Uncompressed_WithSegmentHeader);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_Uncompressed_WithSegmentHeader);

            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm message not using RDP 8.0 compression technology to client.");
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Delete the surface
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a compressed message without segment header.")]
        public void RDPEGFX_Segmentation_Negative_CompressedNoSegmentHeader()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            this.TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_Compressed_NoSegemntHeader);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_Compressed_NoSegemntHeader);

            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm message without segment header to the client. This message is compressed by RDP 8.0 compression technology.");
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            RDPClientTryDropConnection("capability confirm message without segment header");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with segment descriptor is neither 0xE0 nor 0xE1.")]
        public void RDPEGFX_Segmentation_Negative_IncorrectSegmentDescriptor()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            this.TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_Incorrect_SegmentDescriptor);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_Incorrect_SegmentDescriptor);

            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm message with segment descriptor is neither 0xE0 nor 0xE1 to client.");
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            RDPClientTryDropConnection("capability confirm message with segment descriptor is neither 0xE0 nor 0xE1");
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with single segment with segmentCount field")]
        public void RDPEGFX_Segmentation_Negative_SingleSegmentWithSegmentCount()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            this.TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_SingleSegment_WithSegmentCount);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_SingleSegment_WithSegmentCount);

            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm message with single segment with segmentCount field to client.");
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            RDPClientTryDropConnection("capability confirm message with single segment with segmentCount field");
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with single segment with UncompressedSize field")]
        public void RDPEGFX_Segmentation_Negative_SingleSegmentWithUncompressedSize()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_SingleSegment_WithSegmentArray);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_SingleSegment_WithSegmentArray);

            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm message with single segment with UncompressedSize field to client.");
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            RDPClientTryDropConnection("capability confirm message with single segment with UncompressedSize field");
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with single segment with SegmentArray field")]
        public void RDPEGFX_Segmentation_Negative_SingleSegmentWithSegmentArray()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_SingleSegment_WithSegmentArray);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_SingleSegment_WithSegmentArray);

            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm message with single segment with SegmentArray field to client.");
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            RDPClientTryDropConnection("capability confirm message with single segment with SegmentArray field");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with multiple segments without SegmentCount field")]
        public void RDPEGFX_Segmentation_Negative_MultiSegmentNoSegmentCount()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_MultiSegments_WithoutSegmentCount);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_MultiSegments_WithoutSegmentCount);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encode Header/Data Messages to client.");
            byte compFlag = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8;

            Image bgImage;
            Image compImage = RdpegfxTestUtility.captureFromImage(image_64X64, RdpegfxTestUtility.imgPos,
                                   RdpegfxTestUtility.ccLargeBandWidth, RdpegfxTestUtility.ccLargeBandHeight, out bgImage);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a large size bitmap using multipart datapacking but without SegmentCount field to the client for frame {0}.", fid);
            fid = this.rdpegfxAdapter.SendUncompressedImage(compImage, RdpegfxTestUtility.imgPos.x, RdpegfxTestUtility.imgPos.y, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, compFlag, RdpegfxTestUtility.segmentPartSize);

            RDPClientTryDropConnection("a large size bitmap using multipart datapacking but without SegmentCount field");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with multiple segments without UncompressedSize field")]
        public void RDPEGFX_Segmentation_Negative_MultiSegmentNoUncompressedSize()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_MultiSegments_WithoutUncompressedSize);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_MultiSegments_WithoutUncompressedSize);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encode Header/Data Messages to client.");
            byte compFlag = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8;

            Image bgImage;
            Image compImage = RdpegfxTestUtility.captureFromImage(image_64X64, RdpegfxTestUtility.imgPos,
                                   RdpegfxTestUtility.ccLargeBandWidth, RdpegfxTestUtility.ccLargeBandHeight, out bgImage);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a large size bitmap using multipart datapacking but without UncompressedSize field to the client for frame {0}.", fid);
            fid = this.rdpegfxAdapter.SendUncompressedImage(compImage, RdpegfxTestUtility.imgPos.x, RdpegfxTestUtility.imgPos.y, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, compFlag, RdpegfxTestUtility.segmentPartSize);

            RDPClientTryDropConnection("a large size bitmap using multipart datapacking but without SegmentCount field");
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with multiple segment without SegmentArray field")]
        public void RDPEGFX_Segmentation_Negative_MultiSegmentNoSegmentArray()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Segmentation_MultiSegments_WithoutSegmentArray);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Segmentation_MultiSegments_WithoutSegmentArray);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create the first surface and map the surface to output");
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // To be modified .... 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encode Header/Data Messages to client.");
            // byte compFlag = RdpSegmentedPdu.PACKET_COMPR_TYPE_RDP8 | RdpSegmentedPdu.PACKET_COMPRESSED;
            byte compFlag = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8;

            Image bgImage;
            Image compImage = RdpegfxTestUtility.captureFromImage(image_64X64, RdpegfxTestUtility.imgPos,
                                   RdpegfxTestUtility.ccLargeBandWidth, RdpegfxTestUtility.ccLargeBandHeight, out bgImage);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a large size bitmap using multipart datapacking but without SegmentArray field to the client for frame {0}.", fid);
            fid = this.rdpegfxAdapter.SendUncompressedImage(compImage, RdpegfxTestUtility.imgPos.x, RdpegfxTestUtility.imgPos.y, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, compFlag, RdpegfxTestUtility.segmentPartSize);

            RDPClientTryDropConnection("a large size bitmap using multipart datapacking but without SegmentArray field");
        }
    }

}
