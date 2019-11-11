// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrAdapter
    {

        #region Sequences Methods in IRdpbcgrAdapter

        /// <summary>
        /// Establish a RDP connection with RDP Server
        /// </summary>
        /// <param name="requestedProtocols">Flags indicate supported security protocols</param>
        /// <param name="SVCNames">Array of static virtual channels' name</param>
        /// <param name="highestCompressionTypeSupported">Indicate the highest compression type supported</param>
        /// <param name="isReconnect">Whether this is in a reconnection sequence</param>        /// 
        /// <param name="autoLogon">Whether auto logon using username and password in client info PDU</param>
        /// <param name="supportEGFX">Set the support of RDPEGFX</param>
        /// <param name="supportAutoDetect">Set the support of auto-detect</param>
        /// <param name="supportHeartbeatPDU">Set the support of Heartbeat PDU</param>
        /// <param name="supportMultitransportReliable">Set the support of reliable multitransport</param>
        /// <param name="supportMultitransportLossy">Set the support of lossy multitransport</param>
        /// <param name="supportAutoReconnect">Set the support of auto-reconnect</param>
        /// <param name="supportFastPathInput">Set the support of fast-path input</param>
        /// <param name="supportFastPathOutput">Set the support of fast-path output</param>
        /// <param name="supportSurfaceCommands">Set the support of surface commands</param>
        /// <param name="supportSVCCompression">Set the support of static virtual channel data compression</param>
        /// <param name="supportRemoteFXCodec">Set the support of RemoteFX codecs</param>
        public void EstablishRDPConnection(requestedProtocols_Values requestedProtocols,
            string[] SVCNames,
            CompressionType highestCompressionTypeSupported = CompressionType.PACKET_COMPR_TYPE_RDP61,
            bool isReconnect = false,
            bool autoLogon = false,
            bool supportEGFX = false,
            bool supportAutoDetect = false,
            bool supportHeartbeatPDU = false,
            bool supportMultitransportReliable = false,
            bool supportMultitransportLossy = false,
            bool supportAutoReconnect = false,
            bool supportFastPathInput = false,
            bool supportFastPathOutput = false,
            bool supportSurfaceCommands = false,
            bool supportSVCCompression = false,
            bool supportRemoteFXCodec = false)
        {
            #region logging
            string requestProtocolString = "PROTOCOL_RDP_FLAG";
            if (requestedProtocols.HasFlag(requestedProtocols_Values.PROTOCOL_SSL_FLAG))
            {
                requestProtocolString = requestProtocolString + "|PROTOCOL_SSL_FLAG";
            }
            if (requestedProtocols.HasFlag(requestedProtocols_Values.PROTOCOL_HYBRID_FLAG))
            {
                requestProtocolString = requestProtocolString + "|PROTOCOL_HYBRID_FLAG";
            }
            if (requestedProtocols.HasFlag(requestedProtocols_Values.PROTOCOL_HYBRID_EX))
            {
                requestProtocolString = requestProtocolString + "|PROTOCOL_HYBRID_EX";
            }
            string svcNameString = "";
            if (SVCNames != null && SVCNames.Length > 0)
            {
                foreach (string svcName in SVCNames)
                {
                    svcNameString = svcNameString + svcName + ",";
                }
            }
            this.Site.Log.Add(LogEntryKind.Comment, @"EstablishRDPConnection:
                request Protocols = {0},
                Name of static virtual channels = {1},
                Highest compression type supported = {2},
                Is Reconnect = {3}
                RDPEGFX supported = {4},
                AutoDetect supported = {5},
                HeartbeatPDU supported = {6},
                Reliable Multitransport supported = {7},
                Lossy Multitransport supported = {8},
                AutoReconnect supported = {9},
                FastPathInput supported= {10},
                FastPathOutput supported = {11},
                SurfaceCommands supported = {12},
                SVCCompression supported = {13},
                RemoteFXCodec supported = {14}.",
                requestProtocolString, svcNameString, highestCompressionTypeSupported.ToString(), isReconnect, supportEGFX, supportAutoDetect, supportHeartbeatPDU,
                supportMultitransportReliable, supportMultitransportLossy, supportAutoReconnect, supportFastPathInput, supportFastPathOutput, supportSurfaceCommands, 
                supportSVCCompression, supportRemoteFXCodec);
            #endregion logging
            
            #region Connection Initiation

            SendClientX224ConnectionRequest(NegativeType.None, requestedProtocols);

            Server_X_224_Connection_Confirm_Pdu connectionConfirmPdu = ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(testConfig.timeout);
            if (connectionConfirmPdu == null)
            {
                TimeSpan waitTime = new TimeSpan(0, 0, 1);
                Server_X_224_Negotiate_Failure_Pdu failurePdu = ExpectPacket<Server_X_224_Negotiate_Failure_Pdu>(waitTime);
                if (failurePdu != null)
                {
                    Site.Assert.Fail("Received a Server X224 Connection confirm with RDP_NEG_FAILURE structure, failureCode is {0}.", failurePdu.rdpNegFailure.failureCode);
                }
                Site.Assert.Fail("Expecting a Server X224 Connection Confirm PDU.");
            }
            
            if (supportEGFX)
            {
                // Verify support of EGFX on Server
                Site.Assert.IsTrue(connectionConfirmPdu.rdpNegData != null
                && connectionConfirmPdu.rdpNegData.flags.HasFlag(RDP_NEG_RSP_flags_Values.DYNVC_GFX_PROTOCOL_SUPPORTED), "The RDP Server should support RDPEGFX.");
            }
            #endregion Connection Initiation

            #region Basic Setting Exchange

            SendClientMCSConnectInitialPDU(NegativeType.None, SVCNames, supportEGFX, supportAutoDetect, supportHeartbeatPDU, supportMultitransportReliable, supportMultitransportLossy, false);

            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response connectResponsePdu = ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(testConfig.timeout);
            Site.Assert.IsNotNull(connectResponsePdu, "Expecting a Server MCS Connect Response PDU with GCC Conference Create Response.");
            if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData != null)
            {
                if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR))
                {
                    this.serverSupportUDPFECR = true;
                }
                if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL))
                {
                    this.serverSupportUDPFECL = true;
                }
                if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDP_PREFERRED))
                {
                    this.serverSupportUDPPrefferred = true;
                }
            }
            
            #endregion Basic Setting Exchange

            #region Channel Connection

            SendClientMCSErectDomainRequest(NegativeType.None);

            SendClientMCSAttachUserRequest(NegativeType.None);

            Server_MCS_Attach_User_Confirm_Pdu userConfirmPdu = ExpectPacket<Server_MCS_Attach_User_Confirm_Pdu>(testConfig.timeout);
            Site.Assert.IsNotNull(userConfirmPdu, "Expecting a Server MCS Attach User Confirm PDU.");
            
            ChannelJoinRequestAndConfirm();
           
            #endregion

            #region RDP Security Commencement

            if (rdpbcgrClientStack.Context.ServerSelectedProtocol == (uint)selectedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                SendClientSecurityExchangePDU(NegativeType.None);
            }
            
            #endregion

            #region Secure Setting Exchange

            SendClientInfoPDU(NegativeType.None, highestCompressionTypeSupported, isReconnect, autoLogon);

            #endregion

            #region Licensing
            ProcessLicenseSequence(testConfig.timeout);

            #endregion

            #region Capabilities Exchange

            Server_Demand_Active_Pdu demandActivePdu = ExpectPacket<Server_Demand_Active_Pdu>(testConfig.timeout);
            Site.Assert.IsNotNull(demandActivePdu, "Expecting a Server Demand Active PDU.");
            
            SendClientConfirmActivePDU(NegativeType.None, supportAutoReconnect, supportFastPathInput, supportFastPathOutput, supportSurfaceCommands, supportSVCCompression, supportRemoteFXCodec);
            
            #endregion

            #region Connection Finalization

            SendClientSynchronizePDU();

            Server_Synchronize_Pdu syncPdu = ExpectPacket<Server_Synchronize_Pdu>(testConfig.timeout);
            Site.Assert.IsNotNull(syncPdu, "Expecting a Server Synchronize PDU.");
            
            Server_Control_Pdu_Cooperate CoopControlPdu = ExpectPacket<Server_Control_Pdu_Cooperate>(testConfig.timeout);
            Site.Assert.IsNotNull(CoopControlPdu, "Expecting a Server Control PDU - Cooperate.");
            
            SendClientControlCooperatePDU();

            SendClientControlRequestPDU();

            Server_Control_Pdu_Granted_Control grantedControlPdu = ExpectPacket<Server_Control_Pdu_Granted_Control>(testConfig.timeout);
            Site.Assert.IsNotNull(grantedControlPdu, "Expecting a Server Control PDU - Granted Control.");
            

            if (IsBitmapCacheHostSupport)
            {
                SendClientPersistentKeyListPDU();
            }

            SendClientFontListPDU();

            Server_Font_Map_Pdu fontMapPdu = ExpectPacket<Server_Font_Map_Pdu>(testConfig.timeout);
            Site.Assert.IsNotNull(fontMapPdu, "Expecting a Server Font Map PDU.");
            
            #endregion

        }

        /// <summary>
        /// Complete the channel join sequence
        /// </summary>
        public void ChannelJoinRequestAndConfirm()
        {
            // Add all SVC ids in a List
            List<long> chIdList = new List<long>();
            // Add User channel
            chIdList.Add(rdpbcgrClientStack.Context.UserChannelId);
            // Add IO channel
            chIdList.Add(rdpbcgrClientStack.Context.IOChannelId);
            // Add message channel if exist
            long? msgChId = rdpbcgrClientStack.Context.MessageChannelId;
            if (msgChId != null)
            {
                chIdList.Add(msgChId.Value);
            }
            // Add the other channels from server network data
            if (rdpbcgrClientStack.Context.VirtualChannelIdStore != null)
            {
                for (int i = 0; i < rdpbcgrClientStack.Context.VirtualChannelIdStore.Length; i++)
                {
                    chIdList.Add((long)rdpbcgrClientStack.Context.VirtualChannelIdStore[i]);
                }
            }

            // Start join request and confirm sequence
            foreach (long channelId in chIdList)
            {
                this.Site.Log.Add(LogEntryKind.Comment, "Start to create static virtual channel with channel ID {0}.", channelId);
                SendClientMCSChannelJoinRequest(NegativeType.None, channelId);
                Server_MCS_Channel_Join_Confirm_Pdu confirm = ExpectPacket<Server_MCS_Channel_Join_Confirm_Pdu>(testConfig.timeout);
                this.Site.Assert.IsNotNull(confirm, "The RDP Server MUST response a Channel Join Confirm PDU after receiving a Channel Join Request PDU.");
                if (confirm.channelJoinConfirm.channelId.Value != channelId)
                {
                    this.Site.Assert.Fail("In Channel Join phase, channel ID in Server_MCS_Channel_Join_Confirm_Pdu is not the same as it in Client_MCS_Channel_Join_Request.");
                }                
            }
        }

        /// <summary>
        /// Generate static virtual channel traffics
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        public void GenerateStaticVirtualChannelTraffics(NegativeType invalidType)
        {
            if (rdpbcgrClientStack.Context.SVCManager == null)
            {
                Site.Assume.Fail("SVC Manager must be created before generate static virtual channel data.");
            }

            StaticVirtualChannel RDPEDYCChannel = rdpbcgrClientStack.Context.SVCManager.GetChannelByName(SVCNameForRDPEDYC);
            if(RDPEDYCChannel == null)
            {
                Site.Assume.Fail("Static virtual channel: {0} must be created.", SVCNameForRDPEDYC);
            }

            ushort channelId = RDPEDYCChannel.ChannelId;

            // Expect a RDPEDYC caps request PDU            
            Virtual_Channel_RAW_Server_Pdu svcPdu = this.ExpectPacket<Virtual_Channel_RAW_Server_Pdu>(testConfig.timeout);
            if (svcPdu == null)
            {
                Site.Assert.Fail("Timeout when receiving RDPEDYC static virtual channel data.");
            }

            ClientDecodingPduBuilder decoder = new ClientDecodingPduBuilder();
            PduBuilder pduBuilder = new PduBuilder();
            DynamicVCPDU pdu = decoder.ToPdu(svcPdu.virtualChannelData);
            if (pdu == null)
            {
                Site.Assert.Fail("Received static virtual channel data must be a DVC Capabilities Request PDU!");
            }
            DYNVC_CAPS_Version version = DYNVC_CAPS_Version.VERSION3;
            if (pdu is CapsVer1ReqDvcPdu)
            {
                version = DYNVC_CAPS_Version.VERSION1;
            }
            else if (pdu is CapsVer2ReqDvcPdu)
            {
                version = DYNVC_CAPS_Version.VERSION2;
            }

            // Response a RDPEDYC caps response PDU
            CapsRespDvcPdu capResp = pduBuilder.CreateCapsRespPdu((ushort)version);
            SendVirtualChannelPDU(channelId, pduBuilder.ToRawData(capResp), invalidType);
        }

        /// <summary>
        /// Send Client Input Event PDUs with all kinds of input events
        /// </summary>
        public void GenerateSlowPathInputs()
        {
            try
            {
                // Send Sync event
                TS_INPUT_EVENT synchronizeEvent = this.GenerateSynchronizeEvent(false, true, false, false);
                this.SendClientInputEventPDU(NegativeType.None, new TS_INPUT_EVENT[] { synchronizeEvent });

                // Send mouse event
                TS_INPUT_EVENT mouseEvent = this.GenerateMouseEvent(pointerFlags_Values.PTRFLAGS_WHEEL, RdpConstValue.X_POS, RdpConstValue.Y_POS);
                this.SendClientInputEventPDU(NegativeType.None, new TS_INPUT_EVENT[] { mouseEvent });

                TS_INPUT_EVENT extendedMouseEvent = this.GenerateExtendedMouseEvent(TS_POINTERX_EVENT_pointerFlags_Values.PTRXFLAGS_BUTTON1, RdpConstValue.X_POS, RdpConstValue.Y_POS);
                this.SendClientInputEventPDU(NegativeType.None, new TS_INPUT_EVENT[] { extendedMouseEvent });

                // Send keyboard event
                TS_INPUT_EVENT keyboardEvent = this.GenerateKeyboardEvent(keyboardFlags_Values.KBDFLAGS_RELEASE, RdpConstValue.KEY_CODE);
                this.SendClientInputEventPDU(NegativeType.None, new TS_INPUT_EVENT[] { keyboardEvent });

                TS_INPUT_EVENT unicodeKeyboardEvent = this.GenerateUnicodeKeyboardEvent(keyboardFlags_Values.KBDFLAGS_RELEASE, RdpConstValue.UNICODE_CODE);
                this.SendClientInputEventPDU(NegativeType.None, new TS_INPUT_EVENT[] { unicodeKeyboardEvent });
            }
            catch (System.IO.IOException ioE)
            {
                Site.Assert.Fail("Send slow-path input packets failed, got IOException: {1}.", ioE.Message);
            }
            catch (InvalidOperationException invalidE)
            {
                Site.Assert.Fail("Send slow-path input packets failed, got InvalidOperationException: {1}.", invalidE.Message);
            }
        }

        /// <summary>
        /// Send Client Fast-Path Input Event PDU with all kinds of input events
        /// </summary>
        public void GenerateFastPathInputs()
        {
            try
            {
                // Send Sync event
                TS_FP_INPUT_EVENT synchronizeEvent = this.GenerateFPSynchronizeEvent(false, true, false, false);
                this.SendClientFastPathInputEventPDU(NegativeType.None, new TS_FP_INPUT_EVENT[] { synchronizeEvent });
                Site.Log.Add(LogEntryKind.Comment, "TS_FP_INPUT_PDU with Synchronize Event has been sent");

                // Send mouse event
                TS_FP_INPUT_EVENT mouseEvent = this.GenerateFPMouseEvent(pointerFlags_Values.PTRFLAGS_WHEEL, RdpConstValue.X_POS, RdpConstValue.Y_POS);
                this.SendClientFastPathInputEventPDU(NegativeType.None, new TS_FP_INPUT_EVENT[] { mouseEvent });
                Site.Log.Add(LogEntryKind.Comment, "TS_FP_INPUT_PDU with Mouse Event has been sent");

                TS_FP_INPUT_EVENT extendedMouseEvent = this.GenerateFPExtendedMouseEvent(TS_POINTERX_EVENT_pointerFlags_Values.PTRXFLAGS_BUTTON1, RdpConstValue.X_POS, RdpConstValue.Y_POS);
                this.SendClientFastPathInputEventPDU(NegativeType.None, new TS_FP_INPUT_EVENT[] { extendedMouseEvent });
                Site.Log.Add(LogEntryKind.Comment, "TS_FP_INPUT_PDU with Extended Mouse Event has been sent");

                // Send keyboard event
                TS_FP_INPUT_EVENT keyboardEvent = this.GenerateFPKeyboardEvent(TS_FP_KEYBOARD_EVENT_Eventflags.FASTPATH_INPUT_KBDFLAGS_EXTENDED, RdpConstValue.FP_KEY_CODE);
                this.SendClientFastPathInputEventPDU(NegativeType.None, new TS_FP_INPUT_EVENT[] { keyboardEvent });
                Site.Log.Add(LogEntryKind.Comment, "TS_FP_INPUT_PDU with Keyboard Event has been sent");

                TS_FP_INPUT_EVENT unicodeKeyboardEvent = this.GenerateFPUnicodeKeyboardEvent(TS_FP_KEYBOARD_EVENT_Eventflags.FASTPATH_INPUT_KBDFLAGS_RELEASE, RdpConstValue.FP_UNICODE_CODE);
                this.SendClientFastPathInputEventPDU(NegativeType.None, new TS_FP_INPUT_EVENT[] { unicodeKeyboardEvent });
                Site.Log.Add(LogEntryKind.Comment, "TS_FP_INPUT_PDU with Unicode Keyboard Event has been sent");

                if (IsServerSupportFastpathInputQoeTimestampEvent())
                {
                    // Send QoE Timestamp Event
                    TS_FP_INPUT_EVENT qoeTimestampEvent = this.GenerateQoETimestampEvent((uint)DateTime.Now.Millisecond);
                    this.SendClientFastPathInputEventPDU(NegativeType.None, new TS_FP_INPUT_EVENT[] { qoeTimestampEvent });
                    Site.Log.Add(LogEntryKind.Comment, "TS_FP_INPUT_PDU with QoE Timestamp Event has been sent");
                }
            }
            catch (System.IO.IOException ioE)
            {
                Site.Assert.Fail("Send fast-path input packets failed, got IOException: {0}.", ioE.Message);
            }
            catch (InvalidOperationException invalidE)
            {
                Site.Assert.Fail("Send fast-path input packets failed, got InvalidOperationException: {0}.", invalidE.Message);
            }
        }

        /// <summary>
        /// Expect and verifies fast-path output events during a specific timespan
        /// </summary>
        /// <param name="timeout"></param>
        public void ExpectFastpathOutputs(TimeSpan timeout)
        {
            Dictionary<string, bool> receiveStatics = new Dictionary<string, bool>();
            DateTime endtime = DateTime.Now + timeout;
            while (timeout.TotalMilliseconds > 0)
            {
                TS_FP_UPDATE_PDU pdu = ExpectPacket<TS_FP_UPDATE_PDU>(timeout);
                if (pdu != null)
                {
                    foreach (TS_FP_UPDATE update in pdu.fpOutputUpdates)
                    {
                        if (update is TS_FP_UPDATE_PALETTE)
                        {
                            receiveStatics["TS_FP_UPDATE_PALETTE"] = true;
                        }
                        else if (update is TS_FP_UPDATE_ORDERS)
                        {
                            receiveStatics["TS_FP_UPDATE_ORDERS"] = true;
                        }
                        else if (update is TS_FP_UPDATE_BITMAP)
                        {
                            receiveStatics["TS_FP_UPDATE_BITMAP"] = true;
                        }
                        else if (update is TS_FP_UPDATE_SYNCHRONIZE)
                        {
                            receiveStatics["TS_FP_UPDATE_SYNCHRONIZE"] = true;
                        }
                        else if (update is TS_FP_POINTERPOSATTRIBUTE)
                        {
                            receiveStatics["TS_FP_POINTERPOSATTRIBUTE"] = true;
                        }
                        else if (update is TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE)
                        {
                            receiveStatics["TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE"] = true;
                        }
                        else if (update is TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE)
                        {
                            receiveStatics["TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE"] = true;
                        }
                        else if (update is TS_FP_COLORPOINTERATTRIBUTE)
                        {
                            receiveStatics["TS_FP_COLORPOINTERATTRIBUTE"] = true;
                        }
                        else if (update is TS_FP_POINTERATTRIBUTE)
                        {
                            receiveStatics["TS_FP_POINTERATTRIBUTE"] = true;
                        }
                        else if (update is TS_FP_CACHEDPOINTERATTRIBUTE)
                        {
                            receiveStatics["TS_FP_CACHEDPOINTERATTRIBUTE"] = true;
                        }
                        else if (update is TS_FP_SURFCMDS)
                        {
                            receiveStatics["TS_FP_SURFCMDS"] = true;
                        }
                    }
                }
                timeout = endtime - DateTime.Now;
            }
            string log = "Received TS_FP_UPDATE_PDU structures: ";
            foreach (string structName in receiveStatics.Keys)
            {
                log += "" + structName+ "\n";
            }
            Site.Log.Add(LogEntryKind.Comment, log);
        }

        /// <summary>
        /// Used to verify whether a RDP connection is still exist. 
        /// This function can only be used after RDP connection established and user logon.
        ///  Send a Shutdown Request PDU to the server.
        ///  Expect a Shutdown Request Denied PDU from server.
        /// </summary>
        /// <param name="timeout">Timeout when expect Shutdown Request Denied PDU</param>
        /// <returns></returns>
        public bool VerifyRDPConnection(TimeSpan timeout)
        {
            Site.Assume.IsTrue(isLogon, "VerifyRDPConnection can only used after user logon.");
            try
            {
                System.Threading.Thread.Sleep(sendInterval); // Wait some time before the verification, so the server can react to previous behaviors.
                this.SendClientShutdownRequestPDU();
                Server_Shutdown_Request_Denied_Pdu requestDenied = this.ExpectPacket<Server_Shutdown_Request_Denied_Pdu>(timeout);
                if (requestDenied != null)
                {
                    return true;
                }
                Site.Log.Add(LogEntryKind.Comment, "Timeout when receiving Server_Shutdown_Request_Denied_Pdu.");
            }
            catch (InvalidOperationException invalidE)
            {
                Site.Log.Add(LogEntryKind.Comment, "Underlayer transport throw exception indicates the connection has been terminated: " + invalidE.Message);
            }
            catch (System.IO.IOException ioE)
            {
                Site.Log.Add(LogEntryKind.Comment, "IO exception encountered: " + ioE.Message);
            }
            catch (TimeoutException)
            {
                Site.Log.Add(LogEntryKind.Comment, "Timeout when receiving Server_Shutdown_Request_Denied_Pdu.");
            }
            return false;
        }

        /// <summary>
        /// Process the network auto-detect sequence 
        /// </summary>
        public void ProcessAutoDetectSequence()
        {
            bool sequenceCompleted = false;

            while (!sequenceCompleted)
            {
                Server_Auto_Detect_Request_PDU autoDetectReq = this.ExpectPacket<Server_Auto_Detect_Request_PDU>(testConfig.timeout);
                Site.Assert.IsNotNull(autoDetectReq, "Timeout to receive a Server_Auto_Detect_Request_PDU before the auto-detect sequence completed.");

                if (autoDetectReq.autoDetectReqData is RDP_RTT_REQUEST)
                {
                    this.SendClientAutoDetectResponsePDUWithRTTResponse(NegativeType.None, autoDetectReq.autoDetectReqData.sequenceNumber);
                }
                else if (autoDetectReq.autoDetectReqData is RDP_BW_START)
                {
                    bandwidthDetectStartTime = DateTime.Now;
                    byteCount = 0;
                }
                else if (autoDetectReq.autoDetectReqData is RDP_BW_PAYLOAD)
                {
                    byteCount += (uint)autoDetectReq.autoDetectReqData.headerLength+(autoDetectReq.autoDetectReqData as RDP_BW_PAYLOAD).payloadLength;
                }
                else if (autoDetectReq.autoDetectReqData is RDP_BW_STOP)
                {
                    uint timeDelta = (uint)(DateTime.Now - bandwidthDetectStartTime).TotalMilliseconds;
                    byteCount += (uint)autoDetectReq.autoDetectReqData.headerLength + (autoDetectReq.autoDetectReqData as RDP_BW_STOP).payloadLength;
                    bool isDuringConnectTime = true;
                    if(autoDetectReq.autoDetectReqData.requestType != AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_IN_CONNECTTIME)
                    {
                        isDuringConnectTime = false;
                    }
                    this.SendClientAutoDetectResponsePDUWithBWResults(NegativeType.None, autoDetectReq.autoDetectReqData.sequenceNumber, timeDelta, byteCount, isDuringConnectTime);
                
                }
                else if (autoDetectReq.autoDetectReqData is RDP_NETCHAR_RESULT)
                {
                    baseRTT = (autoDetectReq.autoDetectReqData as RDP_NETCHAR_RESULT).baseRTT;
                    bandwidth = (autoDetectReq.autoDetectReqData as RDP_NETCHAR_RESULT).bandwidth;
                    averageRTT = (autoDetectReq.autoDetectReqData as RDP_NETCHAR_RESULT).averageRTT;
                    sequenceCompleted = true;
                }
            }
        }

        #endregion Sequences Methods in IRdpbcgrAdapter

        #region Private Methods

        private bool IsSurfaceCommandsSupported(Collection<ITsCapsSet> capabilitySets)
        {
            foreach (ITsCapsSet cap in capabilitySets)
            {
                if (cap is TS_SURFCMDS_CAPABILITYSET)
                {
                    if (((TS_SURFCMDS_CAPABILITYSET)cap).cmdFlags == (CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                
            }
            return false;
        }

        private bool IsSVCCompressionSupported(Collection<ITsCapsSet> capabilitySets)
        {
            foreach (ITsCapsSet cap in capabilitySets)
            {
                if (cap is TS_VIRTUALCHANNEL_CAPABILITYSET)
                {
                    if (((TS_VIRTUALCHANNEL_CAPABILITYSET)cap).flags.HasFlag(TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_COMPR_CS_8K))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            return false;
        }

        private bool IsRemoteFXCodecSupported(Collection<ITsCapsSet> capabilitySets)
        {
            foreach (ITsCapsSet cap in capabilitySets)
            {
                if (cap is TS_BITMAPCODECS_CAPABILITYSET)
                {
                    foreach (TS_BITMAPCODEC codec in ((TS_BITMAPCODECS_CAPABILITYSET)cap).supportedBitmapCodecs.bitmapCodecArray)
                    {
                        if(codec.codecGUID.codecGUID1 == 0x76772F12
                            &&codec.codecGUID.codecGUID2 == 0xBD72
                            &&codec.codecGUID.codecGUID3 == 0x4463
                            &&codec.codecGUID.codecGUID4 == 0xAF
                            &&codec.codecGUID.codecGUID5 == 0xB3
                            &&codec.codecGUID.codecGUID6 == 0xB7
                            &&codec.codecGUID.codecGUID7 == 0x3C
                            &&codec.codecGUID.codecGUID8 == 0x9C
                            &&codec.codecGUID.codecGUID9 == 0x6F
                            &&codec.codecGUID.codecGUID10 == 0x78
                            &&codec.codecGUID.codecGUID11 == 0x86)
                        return true;
                    }
                    return false;
                }

            }
            return false;
        }
        #endregion Private Methods
    }
}