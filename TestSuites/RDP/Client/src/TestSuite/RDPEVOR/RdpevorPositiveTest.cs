// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEVOR")]        
        [Description("This test case is used to test if MS-RDPEVOR is supported by SUT (RDP client).")]
        public void Rdpevor_ProtocolSupportTest_Positive()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEVOR ...");
            bool bProtocolSupported = this.rdpevorAdapter.ProtocolInitialize(rdpedycServer);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEVOR")]        
        [Description("This test case is used to test video control messages such as a presentation request/response.")]
        public void Rdpevor_VideoControlTest_Positive()
        {
           
            UInt64 geoId = testData.TestGeometryId;
            uint width = testData.TestVideoWidth;
            uint height = testData.TestVideoHeight;
            byte pId = 1; //Presentation Id, start with 1.

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEVOR ...");
            bool bProtocolSupported = this.rdpevorAdapter.ProtocolInitialize(rdpedycServer);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            Site.Log.Add(LogEntryKind.Debug, "Creating a geometry mapping for video stream. Geometry Id:{0}.", geoId);
            this.rdpegtAdapter.SendMappedGeometryPacket(geoId, UpdateTypeValues.GEOMETRY_UPDATE, testData.TestVideoLeft, testData.TestVideoTop, width, height);

            Site.Log.Add(LogEntryKind.Debug, "Sending Presentation Request (Start) Message. Presentation Id:{0}, Geometry Id:{1}.", pId, geoId);
            this.rdpevorAdapter.SendPresentationRequest(pId, CommandValues.Start, width, height, width, height, geoId, VideoSubtype.MFVideoFormat_H264, testData.extraData);

            Site.Log.Add(LogEntryKind.Debug, "Expecting Presentation Response Message from client. Presentation Id:{0}.", pId);
            this.rdpevorAdapter.ExpectPresentationiResponse(pId);

            Site.Log.Add(LogEntryKind.Debug, "Sending Presentation Request (Start) Message.");
            //If the command is to stop the presentation, only the Header, PresentationId, Version, and Command fields are valid, So set all other fields to 0.
            this.rdpevorAdapter.SendPresentationRequest(pId, CommandValues.Stop, 0, 0, 0, 0, 0, VideoSubtype.MFVideoFormat_H264, testData.extraData);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEVOR")]        
        [Description("This test case is used to test video streaming, SUT (RDP client) is expected to decode and render the video.")]
        public void Rdpevor_VideoStreamingTest_Positive()
        {
           
            UInt64 geoId = testData.TestGeometryId;
            uint width = testData.TestVideoWidth;
            uint height = testData.TestVideoHeight;
            byte pId = 1; //Presentation Id, start with 1.

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEVOR ...");
            bool bProtocolSupported = this.rdpevorAdapter.ProtocolInitialize(rdpedycServer);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            Site.Log.Add(LogEntryKind.Debug, "Creating a geometry mapping for video stream. Geometry Id:{0}.", geoId);
            this.rdpegtAdapter.SendMappedGeometryPacket(geoId, UpdateTypeValues.GEOMETRY_UPDATE, testData.TestVideoLeft, testData.TestVideoTop, width, height);

            Site.Log.Add(LogEntryKind.Debug, "Sending Presentation Request (Start) Message. Presentation Id:{0}, Geometry Id:{1}.", pId, geoId);
            this.rdpevorAdapter.SendPresentationRequest(pId, CommandValues.Start, width, height, width, height, geoId, VideoSubtype.MFVideoFormat_H264, testData.extraData);

            Site.Log.Add(LogEntryKind.Debug, "Expecting Presentation Response Message from client. Presentation Id:{0}.", pId);
            this.rdpevorAdapter.ExpectPresentationiResponse(pId);

            uint sampeNumber = 1;
            foreach (Sample sample in testData.sampleList)
            {
                Site.Log.Add(LogEntryKind.Debug, "Sending a sample to client. Presentation Id:{0}, Is key frame: {1}, sample number: {2}", pId, true, sampeNumber);
                this.rdpevorAdapter.SendVideoSample(pId, true, sampeNumber++, sample.data, sample.timeStamp);
            }
            
            //Wait 10 seconds, so that the video can be displayed on client before test case exited
            System.Threading.Thread.Sleep(10000);
            
            Site.Log.Add(LogEntryKind.Debug, "Sending Presentation Request (Start) Message.");
            //If the command is to stop the presentation, only the Header, PresentationId, Version, and Command fields are valid, So set all other fields to 0.
            this.rdpevorAdapter.SendPresentationRequest(pId, CommandValues.Stop, 0, 0, 0, 0, 0, VideoSubtype.MFVideoFormat_H264, testData.extraData);
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEVOR")]        
        [Description("This test case is used to test video streaming over UDP transport, SUT (RDP client) is expected to decode and render the video.")]
        public void Rdpevor_VideoStreamingTest_Positive_OverUDPTransport()
        {
            CheckSecurityProtocolForMultitransport();

            UInt64 geoId = testData.TestGeometryId;
            uint width = testData.TestVideoWidth;
            uint height = testData.TestVideoHeight;
            byte pId = 1; //Presentation Id, start with 1.

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection(true);

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEVOR ...");
            bool bProtocolSupported = this.rdpevorAdapter.ProtocolInitialize(rdpedycServer, DynamicVC_TransportType.RDP_UDP_Reliable);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            Site.Log.Add(LogEntryKind.Debug, "Creating a geometry mapping for video stream. Geometry Id:{0}.", geoId);
            this.rdpegtAdapter.SendMappedGeometryPacket(geoId, UpdateTypeValues.GEOMETRY_UPDATE, testData.TestVideoLeft, testData.TestVideoTop, width, height);

            Site.Log.Add(LogEntryKind.Debug, "Sending Presentation Request (Start) Message. Presentation Id:{0}, Geometry Id:{1}.", pId, geoId);
            this.rdpevorAdapter.SendPresentationRequest(pId, CommandValues.Start, width, height, width, height, geoId, VideoSubtype.MFVideoFormat_H264, testData.extraData);

            Site.Log.Add(LogEntryKind.Debug, "Expecting Presentation Response Message from client. Presentation Id:{0}.", pId);
            this.rdpevorAdapter.ExpectPresentationiResponse(pId);

            uint sampeNumber = 1;
            foreach (Sample sample in testData.sampleList)
            {
                Site.Log.Add(LogEntryKind.Debug, "Sending a sample to client. Presentation Id:{0}, Is key frame: {1}, sample number: {2}", pId, true, sampeNumber);
                this.rdpevorAdapter.SendVideoSample(pId, true, sampeNumber++, sample.data, sample.timeStamp);
            }

            //Wait 10 seconds, so that the video can be displayed on client before test case exited
            System.Threading.Thread.Sleep(10000);

            Site.Log.Add(LogEntryKind.Debug, "Sending Presentation Request (Start) Message.");
            //If the command is to stop the presentation, only the Header, PresentationId, Version, and Command fields are valid, So set all other fields to 0.
            this.rdpevorAdapter.SendPresentationRequest(pId, CommandValues.Stop, 0, 0, 0, 0, 0, VideoSubtype.MFVideoFormat_H264, testData.extraData);
        }


    }
}
