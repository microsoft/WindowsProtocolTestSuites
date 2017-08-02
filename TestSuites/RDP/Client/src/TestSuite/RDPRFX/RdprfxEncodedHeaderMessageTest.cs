// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
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
        #region Header Message sending order test

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]        
        [Description(@"Verify the encode header messages can be sent in order of Sync -> Versions -> Channels -> Context.")]
        public void Rdprfx_HeaderMessage_PositiveTest_OrderTest_VersionsChannelsContext()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT in order of TS_RFX_SYNC -> TS_RFX_CODECVERSIONS -> TS_RFX_CHANNELS -> TS_RFX_CONTEXT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages to SUT.
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

            TS_RFX_ICAP[] clientSupportedCaps;
            rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out clientSupportedCaps);

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Set OperationalMode/EntropyAlgorithm to valid pair.
            if (clientSupportedCaps != null)
            {
                opMode = (OperationalMode)clientSupportedCaps[0].flags;
                enAlgorithm = (EntropyAlgorithm)clientSupportedCaps[0].entropyBits;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment,
                "Sending the encode header messages in the order of TS_RFX_SYNC -> TS_RFX_CODECVERSIONS -> TS_RFX_CHANNELS -> TS_RFX_CONTEXT.");
            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels();
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending the encode data messages to client.");
            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop);

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
        [Description(@"Verify the encode header messages can be sent in order of Sync -> Channels -> Versions -> Context.")]
        public void Rdprfx_HeaderMessage_PositiveTest_OrderTest_ChannelsVersionsContext()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT in order of TS_RFX_SYNC -> TS_RFX_CHANNELS -> TS_RFX_CODECVERSIONS -> TS_RFX_CONTEXT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages to SUT.
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

            TS_RFX_ICAP[] clientSupportedCaps;
            rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out clientSupportedCaps);

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Set OperationalMode/EntropyAlgorithm to valid pair.
            if (clientSupportedCaps != null)
            {
                opMode = (OperationalMode)clientSupportedCaps[0].flags;
                enAlgorithm = (EntropyAlgorithm)clientSupportedCaps[0].entropyBits;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment,
                "Sending the encode header messages in the order of TS_RFX_SYNC -> TS_RFX_CHANNELS -> TS_RFX_CODECVERSIONS -> TS_RFX_CONTEXT.");
            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxChannels();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending the encode data messages to client.");
            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop);

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
        [Description(@"Verify the encode header messages can be sent in order of Sync -> Context -> Versions -> Channels.")]
        public void Rdprfx_HeaderMessage_PositiveTest_OrderTest_ContextVersionsChannels()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT in order of TS_RFX_SYNC -> TS_RFX_CONTEXT -> TS_RFX_CODECVERSIONS -> TS_RFX_CHANNELS.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages to SUT.
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

            TS_RFX_ICAP[] clientSupportedCaps;
            rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out clientSupportedCaps);

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Set OperationalMode/EntropyAlgorithm to valid pair.
            if (clientSupportedCaps != null)
            {
                opMode = (OperationalMode)clientSupportedCaps[0].flags;
                enAlgorithm = (EntropyAlgorithm)clientSupportedCaps[0].entropyBits;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment,
                "Sending the encode header messages in the order of TS_RFX_SYNC -> TS_RFX_CONTEXT -> TS_RFX_CODECVERSIONS -> TS_RFX_CHANNELS.");
            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending the encode data messages to client.");
            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop);

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
        [Description(@"Verify the encode header messages can be sent in order of Sync -> Versions -> Context -> Channels.")]
        public void Rdprfx_HeaderMessage_PositiveTest_OrderTest_VersionsContextChannels()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT in order of TS_RFX_SYNC -> TS_RFX_VERSIONS -> TS_RFX_CONTEXT -> TS_RFX_CHANNELS.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages to SUT.
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

            TS_RFX_ICAP[] clientSupportedCaps;
            rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out clientSupportedCaps);

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Set OperationalMode/EntropyAlgorithm to valid pair.
            if (clientSupportedCaps != null)
            {
                opMode = (OperationalMode)clientSupportedCaps[0].flags;
                enAlgorithm = (EntropyAlgorithm)clientSupportedCaps[0].entropyBits;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment,
                "Sending the encode header messages in the order of TS_RFX_SYNC -> TS_RFX_VERSIONS -> TS_RFX_CONTEXT -> TS_RFX_CHANNELS.");
            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);
            rdprfxAdapter.SendTsRfxChannels();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending the encode data messages to client.");
            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);
            #endregion
        } 

        #endregion
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Ensure that the client terminates the RDP connection when received a TS_RFX_SYNC message
        with the blockLen field set to an invalid value.")]
        public void Rdprfx_HeaderMessage_NegativeTest_TsRfxSync_InvalidBlockLen()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establish the RDP connection.
            Step 2: [RDPRFX] send one frame of Encode Header and Data Messages to SUT, and set the blockLen field of TS_RFX_SYNC to an invalid value (less than acutal).
            Step 3: [RDPRFX] expect SUT terminate the RDP connectioin.
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

            #region Fill parameters
            TS_RFX_ICAP[] clientSupportedCaps;
            rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out clientSupportedCaps);

            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Set OperationalMode/EntropyAlgorithm to valid pair.
            if (clientSupportedCaps != null)
            {
                opMode = (OperationalMode)clientSupportedCaps[0].flags;
                enAlgorithm = (EntropyAlgorithm)clientSupportedCaps[0].entropyBits;
            }
            #endregion

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdprfxNegativeType.TsRfxSync_InvalidBlockLen);
            rdprfxAdapter.SetTestType(RdprfxNegativeType.TsRfxSync_InvalidBlockLen);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encode Header/Data Messages to client.");
            rdprfxAdapter.SendImageToClient(image_64X64, opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the client terminates the RDP connection.");
            bool bDisconnected = rdpbcgrAdapter.WaitForDisconnection(waitTime);

            this.TestSite.Assert.IsTrue(bDisconnected, "Client is expected to drop the connection when received an invalid message.");

            #endregion
        }

        [TestMethod]
        [Priority(1)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Ensure the client terminates the RDP connection when received an unspecified type of message.")]
        public void Rdprfx_HeaderMessage_NegativeTest_UnspecifiedMessage()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establish the RDP connection.
            Step 2: [RDPRFX] send one unspecified message to client.
            Step 3: [RDPRFX] expect SUT terminate the RDP connectioin.
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

            #region Fill parameters
            TS_RFX_ICAP[] clientSupportedCaps;
            rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out clientSupportedCaps);

            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Set OperationalMode/EntropyAlgorithm to valid pair.
            if (clientSupportedCaps != null)
            {
                opMode = (OperationalMode)clientSupportedCaps[0].flags;
                enAlgorithm = (EntropyAlgorithm)clientSupportedCaps[0].entropyBits;
            }
            #endregion

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdprfxNegativeType.UnspecifiedBlockType);
            rdprfxAdapter.SetTestType(RdprfxNegativeType.UnspecifiedBlockType);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encode Header/Data Messages to client.");
            rdprfxAdapter.SendImageToClient(image_64X64, opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the client terminates the RDP connection.");
            bool bDisconnected = rdpbcgrAdapter.WaitForDisconnection(waitTime);

            this.TestSite.Assert.IsTrue(bDisconnected, "Client is expected to drop the connection when received an invalid message.");

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can support the smallest channel 1x1.")]
        public void Rdprfx_HeaderMessage_PositiveTest_ChannelBoundaryTest_1x1Channel()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send an smallest channel 1x1 to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages to SUT.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            ChannelBoundaryTest_Channel(1, 1);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can support the channel 2x2.")]
        public void Rdprfx_HeaderMessage_PositiveTest_ChannelBoundaryTest_2x2Channel()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send an channel 2x2 to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages to SUT.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            ChannelBoundaryTest_Channel(2, 2);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can support the largest channel 4096x2048.")]
        public void Rdprfx_HeaderMessage_PositiveTest_ChannelBoundaryTest_4096x2048Channel()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send an channel 4096x2048 to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages to SUT.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            ChannelBoundaryTest_Channel(4096, 2048);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Verify the client can support the channel 4095x2047.")]
        public void Rdprfx_HeaderMessage_PositiveTest_ChannelBoundaryTest_4095x2047Channel()
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send an channel 4095x2047 to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages to SUT.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            ChannelBoundaryTest_Channel(4095, 2047);
        }

    }
}
