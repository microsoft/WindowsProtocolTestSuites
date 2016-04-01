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
        public void Rdprfx_VideoMode_PositiveTest_RLGR1()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send multiple frames of Encode Data Messages (encoded with RLGR1) to SUT.
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
            OperationalMode opMode = OperationalMode.VideoMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 64; //the left bound of the frame.
            ushort destTop = 64; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);
            rdprfxAdapter.SendImageToClient(imageForVideoMode, opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, imageForVideoMode.Width, imageForVideoMode.Height);
            this.VerifySUTDisplay(true, compareRect);
           
            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Sending client an encoded bitmap data which encoded with RLGR3 algorithm.")]
        public void Rdprfx_VideoMode_PositiveTest_RLGR3()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send multiple frames of Encode Data Messages (encoded with RLGR3) to SUT.
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
            OperationalMode opMode = OperationalMode.VideoMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 64; //the left bound of the frame.
            ushort destTop = 64; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);
            rdprfxAdapter.SendImageToClient(imageForVideoMode, opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, imageForVideoMode.Width, imageForVideoMode.Height);
            this.VerifySUTDisplay(true, compareRect);
           
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Send a TS_RFX_SYNC message among frames of the video mode stream.")]
        public void Rdprfx_VideoMode_PositiveTest_SendTsRfxSyncInStream()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] set operation mode to video mode, send one pair of Encode Header Messages to client.
            Step 4: [RDPRFX] send one frame of Encode Data Messages to client.
            Step 5: [RDPRFX] send a TS_RFX_SYNC message to client.
            Step 6: [RDPRFX] send another frame of Encode Data Messages to client.
            Step 7: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 8: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
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

            uint frameId = 0;
            OperationalMode opMode = OperationalMode.VideoMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
                if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Client does not support Video Mode, so stop to run this test case.");
                    return;
                }
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending the encode header messages to client.");
            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels();
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encode data messages to client, and set the frameId of TS_RFX_FRAME_BEGIN to 0.");
            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a TS_RFX_SYNC message to client.");
            rdprfxAdapter.SendTsRfxSync();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending another frame of encode data messages to client, and set the frameId of TS_RFX_FRAME_BEGIN to 2.");
            rdprfxAdapter.SendTsRfxFrameBegin(1);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            checked
            {
                rdprfxAdapter.FlushEncodedData((ushort)(destLeft + 0x40), (ushort)(destTop + 0x40));
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Send a TS_RFX_CODEC_VERSIONS message among frames of the video mode stream.")]
        public void Rdprfx_VideoMode_PositiveTest_SendTsRfxCodecVersioinsInStream()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] set operation mode to video mode, send one pair of Encode Header Messages to client.
            Step 4: [RDPRFX] send one frame of Encode Data Messages to client.
            Step 5: [RDPRFX] send a TS_RFX_CODEC_VERSIONS message to client.
            Step 6: [RDPRFX] send another frame of Encode Data Messages to client.
            Step 7: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 8: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
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

            uint frameId = 0;
            OperationalMode opMode = OperationalMode.VideoMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
                if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Client does not support Video Mode, so stop running this test case.");
                    return;
                }
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending the encode header messages to client.");
            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels();
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encode data messages to client, and set the frameId of TS_RFX_FRAME_BEGIN to 0.");
            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a TS_RFX_CODEC_VERSIONS message to client.");
            rdprfxAdapter.SendTsRfxCodecVersions();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending another frame of encode data messages to client, and set the frameId of TS_RFX_FRAME_BEGIN to 2.");
            rdprfxAdapter.SendTsRfxFrameBegin(1);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            checked
            {
                rdprfxAdapter.FlushEncodedData((ushort)(destLeft + 0x40), (ushort)(destTop + 0x40));
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Send a TS_RFX_CHANNELS message among frames of the video mode stream.")]
        public void Rdprfx_VideoMode_PositiveTest_SendTsRfxChannelsInStream()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] set operation mode to video mode, send one pair of Encode Header Messages to client.
            Step 4: [RDPRFX] send one frame of Encode Data Messages to client.
            Step 5: [RDPRFX] send a TS_RFX_CHANNELS message to client.
            Step 6: [RDPRFX] send another frame of Encode Data Messages to client.
            Step 7: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 8: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
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

            uint frameId = 0;
            OperationalMode opMode = OperationalMode.VideoMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
                if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Client does not support Video Mode, so stop running this test case.");
                    return;
                }
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending the encode header messages to client.");
            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels();
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encode data messages to client, and set the frameId of TS_RFX_FRAME_BEGIN to 0.");
            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a TS_RFX_CHANNELS message to client.");
            rdprfxAdapter.SendTsRfxChannels();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending another frame of encode data messages to client, and set the frameId of TS_RFX_FRAME_BEGIN to 2.");
            rdprfxAdapter.SendTsRfxFrameBegin(1);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            checked
            {
                rdprfxAdapter.FlushEncodedData((ushort)(destLeft + 0x40), (ushort)(destTop + 0x40));
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Send a TS_RFX_CONTEXT message among frames of the video mode stream.")]
        public void Rdprfx_VideoMode_PositiveTest_SendTsRfxContextInStream()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] set operation mode to video mode, send one pair of Encode Header Messages to client.
            Step 4: [RDPRFX] send one frame of Encode Data Messages to client.
            Step 5: [RDPRFX] send a TS_RFX_CONTEXT message to client.
            Step 6: [RDPRFX] send another frame of Encode Data Messages to client.
            Step 7: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 8: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
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

            uint frameId = 0;
            OperationalMode opMode = OperationalMode.VideoMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
                if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Client does not support Video Mode, so stop running this test case.");
                    return;
                }
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending the encode header messages to client.");
            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels();
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encode data messages to client, and set the frameId of TS_RFX_FRAME_BEGIN to 0.");
            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a TS_RFX_CONTEXT message to client.");
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending another frame of encode data messages to client, and set the frameId of TS_RFX_FRAME_BEGIN to 2.");
            rdprfxAdapter.SendTsRfxFrameBegin(1);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            checked
            {
                rdprfxAdapter.FlushEncodedData((ushort)(destLeft + 0x40), (ushort)(destTop + 0x40));
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"When Video Mode is in effect, ensure the client terminates the RDP connection when the server uses an unsupported entropy algorithm to encode data.")]
        public void Rdprfx_VideoMode_NegativeTest_UnsupportedEntropyAlgorithm()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPRFX] send one frame of Encode Header and Data Messages to client, the bitmap is encoded with an entropy algorithem that client not supported.
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

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.VideoMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
                if (this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Client supports both RLGR1 and RLGR3 while operation mode is video mode, so stop running this test case.");
                    return;
                }
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);
            rdprfxAdapter.SendImageToClient(image_64X64, opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the client terminates the RDP connection.");
            bool bDisconnected = rdpbcgrAdapter.WaitForDisconnection(waitTime);

            this.TestSite.Assert.IsTrue(bDisconnected, "Client is expected to drop the connection if the received data is encoded with unsupported entropy algorithm.");
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Ensure  the client terminates the RDP connection when received a TS_RFX_FRAME_BEGIN with the blockLen field set to an invalid value.")]
        public void Rdprfx_VideoMode_NegativeTest_TsRfxFrameBegin_InvalidBlockLen()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPRFX] send one frame of Encode Header and Data Messages to client, set the blockLen field of TS_RFX_FRAME_BEGIN to an invalid value (less than the actual).
            Step 3: [RDPRFX] expect  the client terminates the RDP connection.
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

            uint frameId = 0;
            OperationalMode opMode = OperationalMode.VideoMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
                if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Client does not support Video Mode, so stop running this test case.");
                    return;
                }
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdprfxNegativeType.TsRfxFrameBegin_InvalidBlockLen);
            rdprfxAdapter.SetTestType(RdprfxNegativeType.TsRfxFrameBegin_InvalidBlockLen);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encode Header/Data Messages to client.");
            rdprfxAdapter.SendImageToClient(image_64X64, opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting  the client terminates the RDP connection.");
            bool bDisconnected = rdpbcgrAdapter.WaitForDisconnection(waitTime);

            this.TestSite.Assert.IsTrue(bDisconnected, "Client is expected to drop the connection if received encode data in Video Mode when Image Mode is in effect.");

            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Test video mode differencing scenario.")]
        public void Rdprfx_VideoMode_PositiveTest_Differencing()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send multiple frames of Encode Data Messages (encoded with RLGR1) to SUT.
            Step 4: [RDPRFX] Send multiple frames with one frame no changes (encoded with RLGR1) to SUT.
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
            OperationalMode opMode = OperationalMode.VideoMode;
            // OperationalMode opMode = OperationalMode.ImageMode; // This also works. Strange. Because in TD it mentions that When operating in image mode, the Encode Headers messages (section 2.2.2.2) MUST always precede an encoded frame. When operating in video mode, the header messages MUST be present at the beginning of the stream and MAY be present elsewhere. 
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a rectangle whose width and height is TileSize * 2.");
            Rectangle clipRect = new Rectangle(destLeft, destTop, RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            Rectangle[] rects = new Rectangle[] { clipRect };

            uint surfFrameId = 0;

            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels(RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            rdprfxAdapter.SendTsRfxFrameBegin(surfFrameId++);
            rdprfxAdapter.SendTsRfxRegion(rects);

            TILE_POSITION[] positions = new TILE_POSITION[4]{
                new TILE_POSITION{ xIdx = 0, yIdx = 0 },
                new TILE_POSITION{ xIdx = 0, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 0 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 }
            };

            var bitmapBlue = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            Graphics graphics = Graphics.FromImage(bitmapBlue);
            graphics.FillRectangle(Brushes.Blue, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));
            var bitmapRed = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            graphics = Graphics.FromImage(bitmapRed);
            graphics.FillRectangle(Brushes.Red, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));

            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { bitmapBlue, bitmapBlue, bitmapBlue, bitmapBlue },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            System.Threading.Thread.Sleep(1000);

            rdprfxAdapter.SendTsRfxFrameBegin(surfFrameId++);
            rdprfxAdapter.SendTsRfxRegion(rects);
            positions = new TILE_POSITION[3]{
                new TILE_POSITION{ xIdx = 0, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 0 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 }
            };

            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { bitmapRed, bitmapRed, bitmapRed },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            System.Threading.Thread.Sleep(1000);

            rdprfxAdapter.SendTsRfxFrameBegin(surfFrameId++);
            rdprfxAdapter.SendTsRfxRegion(rects);

            positions = new TILE_POSITION[1]{
                new TILE_POSITION{ xIdx = 0, yIdx = 0 }
            };

            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { bitmapRed },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when one rectangular is composed by four tiles in the corner in TS_RFX_REGION and the rectangle has no common boundaries with each tile.")]
        public void Rdprfx_VideoMode_PositiveTest_FourTilesComposeOneRectWithoutCommonBoundary()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains rectangular which is composed by four tiles and the rectangle has no common boundaries with each tile.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            FourTilesComposeOneRectWithoutCommonBoundary(OperationalMode.VideoMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when one rectangular is composed by four tiles in the corner in TS_RFX_REGION and the rectangle has common boundaries with each tile.")]
        public void Rdprfx_VideoMode_PositiveTest_FourTilesComposeOneRectWithCommonBoundary()
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

            FourTilesComposeOneRectWithCommonBoundary(OperationalMode.VideoMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when send a list of rectangle.")]
        public void Rdprfx_VideoMode_PositiveTest_ListOfRects()
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

            SendListOfRects(OperationalMode.VideoMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when send a list of rectangles with overlapping.")]
        public void Rdprfx_VideoMode_PositiveTest_ListOfRectsOverlap()
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

            SendListOfRectsOverlap(OperationalMode.VideoMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when send a list of rectangles with overlapping and duplicated tile.")]
        public void Rdprfx_VideoMode_PositiveTest_ListOfRectsOverlapWithDuplicateTiles()
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

            SendListOfRectsOverlapWithDuplicateTiles(OperationalMode.VideoMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can handle a duplicated tile correctly.")]
        public void Rdprfx_VideoMode_NegtiveTest_DuplicatedTile()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains rectangular which contains a duplicated tile.
            Step 5: [RDPRFX] Expect  the client terminates the RDP connection.
            */
            #endregion

            NegtiveTest_DuplicatedTile(OperationalMode.VideoMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when the numRects field of TS_RFX_REGION is set to zero.")]
        public void Rdprfx_VideoMode_PositiveTest_numRectsSetToZero()
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

            SendNumRectsSetToZero(OperationalMode.VideoMode);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can decode a RemoteFX encoded bitmap and render correctly when some tiles are out of the rectangle in TS_RFX_REGION.")]
        public void Rdprfx_VideoMode_PositiveTest_OutOfRects()
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

            SendOutOfRects(OperationalMode.VideoMode);
        }
    }
}
