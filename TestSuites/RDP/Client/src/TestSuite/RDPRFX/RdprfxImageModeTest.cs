// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestSuites.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdprfx
{
    public partial class RdprfxTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Sending client an encoded bitmap data which encoded with RLGR1 algorithm.")]
        public void Rdprfx_ImageMode_PositiveTest_RLGR1()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 64; //the left bound of the frame.
            ushort destTop = 64; //the top bound of the frame.

            //Check if the above setting is supported by the client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by the client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);
            rdprfxAdapter.SendImageToClient(image_64X64, opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, image_64X64.Width, image_64X64.Height);
            this.VerifySUTDisplay(true, compareRect);             
           
            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Sending client an encoded bitmap data which encoded with RLGR3 algorithm.")]
        public void Rdprfx_ImageMode_PositiveTest_RLGR3()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR3) to SUT.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 64; //the left bound of the frame.
            ushort destTop = 64; //the top bound of the frame.

            //Check if the above setting is supported by the client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by the client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);
            rdprfxAdapter.SendImageToClient(image_64X64, opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, image_64X64.Width, image_64X64.Height);
            this.VerifySUTDisplay(true, compareRect);
           
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when the bitmap should be clipped by rectangular in TS_RFX_REGION.")]
        public void Rdprfx_ImageMode_PositiveTest_ClippedByRegion()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains rectangular which will clip the encoded bitmap.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 64; //the left bound of the frame.
            ushort destTop = 64; //the top bound of the frame.

            //Check if the above setting is supported by the client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by the client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create rectangles whose height is TileSize/2, this rectangle is used for TS_RFX_REGION structure to clip the bitmap.");
            Rectangle clipRect = new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize / 2);
            Rectangle[] rects = new Rectangle[] { clipRect };

            TileImage[] tileImageArr = RdprfxTileUtils.SplitToTileImage(image_64X64, RdprfxServer.TileSize, RdprfxServer.TileSize);            
            for (int idx = 0; idx < tileImageArr.Length; idx++)
            {
                if (idx == 0)
                {
                    rdprfxAdapter.SendTsRfxSync();
                    rdprfxAdapter.SendTsRfxCodecVersions();
                    rdprfxAdapter.SendTsRfxChannels();
                    rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);
                }
                rdprfxAdapter.SendTsRfxFrameBegin((uint)idx);
                rdprfxAdapter.SendTsRfxRegion(rects);
                rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, tileImageArr[idx].image);
                rdprfxAdapter.SendTsRfxFrameEnd();
                rdprfxAdapter.FlushEncodedData((ushort)(destLeft + tileImageArr[idx].x), (ushort)(destTop + tileImageArr[idx].y));
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, image_64X64.Width, image_64X64.Height);
            this.VerifySUTDisplay(true, compareRect); 

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap when each component use different quantization value.")]
        public void Rdprfx_ImageMode_PositiveTest_MultiQuantVals()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, each component use different quantization value.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 64; //the left bound of the frame.
            ushort destTop = 64; //the top bound of the frame.

            //Check if the above setting is supported by the client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by the client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Quantization value, each component has different quant index.");
            TS_RFX_CODEC_QUANT[] quantVals = this.GenerateCodecQuantVals();
            byte quantIdxY = 0;
            byte quantIdxCb = 1;
            byte quantIdxCr = 2;

            TileImage[] tileImageArr = RdprfxTileUtils.SplitToTileImage(image_64X64, RdprfxServer.TileSize, RdprfxServer.TileSize);
            for (int idx = 0; idx < tileImageArr.Length; idx++)
            {
                if (idx == 0)
                {
                    rdprfxAdapter.SendTsRfxSync();
                    rdprfxAdapter.SendTsRfxCodecVersions();
                    rdprfxAdapter.SendTsRfxChannels();
                    rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);
                }
                rdprfxAdapter.SendTsRfxFrameBegin((uint)idx);
                rdprfxAdapter.SendTsRfxRegion();
                rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, tileImageArr[idx].image, quantVals, quantIdxY, quantIdxCb, quantIdxCr);
                rdprfxAdapter.SendTsRfxFrameEnd();
                rdprfxAdapter.FlushEncodedData((ushort)(destLeft + tileImageArr[idx].x), (ushort)(destTop + tileImageArr[idx].y));
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, image_64X64.Width, image_64X64.Height);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }
        
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Ensure the client terminates the RDP connection when received a TS_RFX_FRAME_BEGIN with the blockLen field set to an invalid value.")]
        public void Rdprfx_ImageMode_NegativeTest_TsRfxFrameBegin_InvalidBlockLen()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPRFX] send one frame of Encode Header and Data Messages to client, set the blockLen field of TS_RFX_FRAME_BEGIN to an invalid value (less than the actual).
            Step 3: [RDPRFX] expect the client terminates the RDP connection.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by the client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
                if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Client does not support Image Mode, so stop running this test case.");
                    return;
                }
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdprfxNegativeType.TsRfxFrameBegin_InvalidBlockLen);
            rdprfxAdapter.SetTestType(RdprfxNegativeType.TsRfxFrameBegin_InvalidBlockLen);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encode Header/Data Messages to client.");
            rdprfxAdapter.SendImageToClient(image_64X64, opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the client terminates the RDP connection.");
            bool bDisconnected = rdpbcgrAdapter.WaitForDisconnection(waitTime);

            this.TestSite.Assert.IsTrue(bDisconnected, "Client is expected to drop the connection if received encode data in Video Mode when Image Mode is in effect.");

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when one rectangular is composed by four tiles in the corner in TS_RFX_REGION and the rectangle has no common boundary with each tile.")]
        public void Rdprfx_ImageMode_PositiveTest_FourTilesComposeOneRectWithoutCommonBoundary()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains rectangular which is composed by four tiles  and the rectangle has no common boundary with each tile.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            FourTilesComposeOneRectWithoutCommonBoundary(OperationalMode.ImageMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when one rectangular is composed by four tiles in the corner in TS_RFX_REGION and the rectangle has common boundaries with each tile.")]
        public void Rdprfx_ImageMode_PositiveTest_FourTilesComposeOneRectWithCommonBoundary()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains rectangular which is composed by four tiles and the rectangle has common boundaries with each tile.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            FourTilesComposeOneRectWithCommonBoundary(OperationalMode.ImageMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can deal with the images without encoding.")]
        public void Rdprfx_PositiveTest_ImageWithoutEncoding()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Data Messages (not encoded) to SUT, the client should handle the unencoded image.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            ushort destLeft = 32; //the left bound of the frame.
            ushort destTop = 32; //the top bound of the frame.

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send bitmap data without encoding to client. destLeft = {0}, destTop = {1}.",  destLeft, destTop);

            this.rdprfxAdapter.SendImageToClientWithoutEncoding(image_64X64, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, image_64X64.Width, image_64X64.Height);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when the numRects field of TS_RFX_REGION is set to zero.")]
        public void Rdprfx_ImageMode_PositiveTest_numRectsSetToZero()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains numRects field of TS_RFX_REGION which is set to zero.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            SendNumRectsSetToZero(OperationalMode.ImageMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when some tiles are out of the rectangle in TS_RFX_REGION.")]
        public void Rdprfx_ImageMode_PositiveTest_OutOfRects()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, when some tiles are out of the rectangle in TS_RFX_REGION.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            SendOutOfRects(OperationalMode.ImageMode);
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when send a list of rectangle.")]
        public void Rdprfx_ImageMode_PositiveTest_ListOfRects()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains a list of rectangles.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            SendListOfRects(OperationalMode.ImageMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when send a list of rectangles with overlapping.")]
        public void Rdprfx_ImageMode_PositiveTest_ListOfRectsOverlap()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains a list of rectangular which overlap with each other.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            SendListOfRectsOverlap(OperationalMode.ImageMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when send a list of rectangles with overlapping and duplicated tile.")]
        public void Rdprfx_ImageMode_PositiveTest_ListOfRectsOverlapWithDuplicateTiles()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains a list of rectangular which overlap with each other and contains a duplicated tile.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            SendListOfRectsOverlapWithDuplicateTiles(OperationalMode.ImageMode);
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can handle a duplicated tile correctly.")]
        public void Rdprfx_ImageMode_NegtiveTest_DuplicatedTile()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains rectangular which contains a duplicated tile.
            Step 5: [RDPRFX] Expect the client terminates the RDP connection.
            */
            #endregion

            NegtiveTest_DuplicatedTile(OperationalMode.ImageMode);
        }

    }

}
