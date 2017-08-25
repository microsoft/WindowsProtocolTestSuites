// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpevor;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegt;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdpegt;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdpevor
{
    public partial class RdpevorTestSuite
    {
        #region Video Control Test - Negative
        
        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEVOR")]        
        [Description("This test case is used to ensure RDP client drop the connection if the packet length of Presentation Request is invalid.")]
        public void Rdpevor_VideoControlTest_Negative_InvalidPacketLength()
        {

            UInt64 geoId = RdpevorTestData.NegativeTestGeometryId;
            uint width = RdpevorTestData.NegativeTestVideoWidth;
            uint height = RdpevorTestData.NegativeTestVideoHeight;
            byte pId = 1; //Presentation Id, start with 1.

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Set the test type to {0}.", RdpevorNegativeType.PresentationRequest_InvalidPacketLength);
            this.rdpevorAdapter.SetTestType(RdpevorNegativeType.PresentationRequest_InvalidPacketLength);

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEVOR ...");
            bool bProtocolSupported = this.rdpevorAdapter.ProtocolInitialize(rdpedycServer);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            Site.Log.Add(LogEntryKind.Debug, "Creating a geometry mapping for video stream. Geometry Id:{0}.", geoId);
            this.rdpegtAdapter.SendMappedGeometryPacket(geoId, UpdateTypeValues.GEOMETRY_UPDATE, RdpevorTestData.NegativeTestVideoLeft, RdpevorTestData.NegativeTestVideoTop, width, height);

            Site.Log.Add(LogEntryKind.Debug, "Sending Presentation Request (Start) Message. Presentation Id:{0}, Geometry Id:{1}.", pId, geoId);
            this.rdpevorAdapter.SendPresentationRequest(pId, CommandValues.Start, width, height, width, height, geoId, VideoSubtype.MFVideoFormat_H264, RdpevorTestData.GetNegativeTestH264ExtraData());

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDP client drop the connection when the packet length of Presentation Request is invalid.");
            RDPClientTryDropConnection("the packet length of Presentation Request is invalid");
        }

        #endregion

        #region Video Streaming Test - Negative

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEVOR")]        
        [Description("This test case is used to ensure RDP client drop the connection when the packet length of TSMM_VIDEO_DATA is invalid.")]
        public void Rdpevor_VideoStreamingTest_Negative_InvalidPacketLength()
        {
            UInt64 geoId = RdpevorTestData.NegativeTestGeometryId;
            uint width = RdpevorTestData.NegativeTestVideoWidth;
            uint height = RdpevorTestData.NegativeTestVideoHeight;
            byte pId = 1; //Presentation Id, start with 1.

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Set the test type to {0}.", RdpevorNegativeType.VideoData_InvalidPacketLength);
            this.rdpevorAdapter.SetTestType(RdpevorNegativeType.VideoData_InvalidPacketLength);

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEVOR ...");
            bool bProtocolSupported = this.rdpevorAdapter.ProtocolInitialize(rdpedycServer);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            Site.Log.Add(LogEntryKind.Debug, "Creating a geometry mapping for video stream. Geometry Id:{0}.", geoId);
            this.rdpegtAdapter.SendMappedGeometryPacket(geoId, UpdateTypeValues.GEOMETRY_UPDATE, RdpevorTestData.NegativeTestVideoLeft, RdpevorTestData.NegativeTestVideoTop, width, height);

            Site.Log.Add(LogEntryKind.Debug, "Sending Presentation Request (Start) Message. Presentation Id:{0}, Geometry Id:{1}.", pId, geoId);
            this.rdpevorAdapter.SendPresentationRequest(pId, CommandValues.Start, width, height, width, height, geoId, VideoSubtype.MFVideoFormat_H264, RdpevorTestData.GetNegativeTestH264ExtraData());

            Site.Log.Add(LogEntryKind.Debug, "Expecting Presentation Response Message from client. Presentation Id:{0}.", pId);
            this.rdpevorAdapter.ExpectPresentationiResponse(pId);

            Site.Log.Add(LogEntryKind.Debug, "Sending packet 1 to client. Presentation Id:{0}, Is key frame: {1}, sample number: {2}, packet index: {3}, total packets: {4}",
                pId, true, RdpevorTestData.NegativeTest_H264_Packet1SampleNumber, RdpevorTestData.NegativeTest_H264_Packet1Index, RdpevorTestData.NegativeTest_H264_Packet1TotalPackets);
            this.rdpevorAdapter.SendVideoPacket(pId, TsmmVideoData_FlagsValues.TSMM_VIDEO_DATA_FLAG_KEYFRAME, RdpevorTestData.NegativeTest_H264_Packet1Index,
                RdpevorTestData.NegativeTest_H264_Packet1TotalPackets, RdpevorTestData.NegativeTest_H264_Packet1SampleNumber, RdpevorTestData.GetNegativeTestH264VideoDataPacket1(),
                RdpevorTestData.NegativeTest_H264_Packet1Sample1Timestamp);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDP client drop the connection when the packet length of TSMM_VIDEO_DATA is invalid.");
            RDPClientTryDropConnection("the packet length of TSMM_VIDEO_DATA is invalid");
        }

        #endregion
    }
}
