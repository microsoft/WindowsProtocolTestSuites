// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Net;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestManager.RDPServerPlugin
{
    public class RdpbcgrTestData
    {
        public const string DefaultX224ConnectReqCookie = "Cookie: mstshash=IDENTIFIER\r\n";
        public const string DefaultX224ConnectReqRoutingToken = "TestRoutingToken\r\n";
        public const uint DefaultClientBuild = 0x0A28;
        public const flags_Values DefaultClientInfoPduFlags = flags_Values.INFO_MOUSE
                               | flags_Values.INFO_DISABLECTRLALTDEL
                               | flags_Values.INFO_UNICODE
                               | flags_Values.INFO_MAXIMIZESHELL
                               | flags_Values.INFO_ENABLEWINDOWSKEY
                               | flags_Values.INFO_FORCE_ENCRYPTED_CS_PDU
                               | flags_Values.INFO_LOGONNOTIFY
                               | flags_Values.INFO_LOGONERRORS;
        public const int ClientRandomSize = 32;
        public const ushort RN_USER_REQUESTED = 3;

        public static byte[] GetDefaultX224ConnectReqCorrelationId()
        {
            byte[] returnArray = new byte[16];
            new System.Random().NextBytes(returnArray);

            if (returnArray[0] == 0x00 || returnArray[0] == 0xF4)
            {
                returnArray[0] = 0x01;
            }
            for (int i = 1; i < returnArray.Length; i++)
            {
                if (returnArray[i] == 0x0D)
                {
                    returnArray[i] = 0x01;
                }
            }
            return returnArray;
        }
    }

    public class RDPDetector : IDisposable
    {
        #region Variables
        private DetectionInfo detectInfo;
        private List<StackPacket> receiveBuffer = null;
        private RdpbcgrClient rdpbcgrClient = null;
        private string[] SVCNames;
        private int defaultPort = 3389;
        private IPAddress clientAddress;
        private EncryptedProtocol encryptedProtocol;
        private requestedProtocols_Values requestedProtocol;
        private TimeSpan timeout;
        #endregion Variables

        private const string SVCNAME_RDPEDYC = "drdynvc";

        #region Constructor
        public RDPDetector(DetectionInfo detectInfo)
        {
            this.detectInfo = detectInfo;
        }
        #endregion Constructor

        #region Methods

        /// <summary>
        /// Establish a RDP connection to detect RDP feature
        /// </summary>
        /// <returns>Return true if detection succeeded.</returns>
        public bool DetectRDPFeature(Configs config)
        {
            try
            {
                initialize(config);
                connectRDPServer();

                bool status = EstablishRDPConnection(
                    config, requestedProtocol, SVCNames,
                    CompressionType.PACKET_COMPR_TYPE_NONE,
                    false,
                    true);
                if (status == false)
                {
                    DetectorUtil.WriteLog("Failed", false, LogStyle.StepPassed);
                }
                else
                {
                    DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);
                }
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog("Exception occured when establishing RDP connection: " + e.Message);
                DetectorUtil.WriteLog("" + e.StackTrace);
                if (e.InnerException != null)
                {
                    DetectorUtil.WriteLog("**" + e.InnerException.Message);
                    DetectorUtil.WriteLog("**" + e.InnerException.StackTrace);
                }
                DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                return false;
            }
            checkSupportedFeatures();
            checkSupportedProtocols();
            Disconnect();
            return true;
        }

        private void initialize(Configs config)
        {
            receiveBuffer = new List<StackPacket>();
            SVCNames = new string[] { SVCNAME_RDPEDYC };
            loadConfig();

            int port;
            rdpbcgrClient = new RdpbcgrClient(
                config.ServerDomain,
                config.ServerName,
                config.ServerUserName,
                config.ServerUserPassword,
                clientAddress.ToString(),
                Int32.TryParse(config.ServerPort, out port) ? port : defaultPort
                );
        }

        private bool loadConfig()
        {
            string clientName = DetectorUtil.GetPropertyValue("RDP.ClientName");
            if (!IPAddress.TryParse(clientName, out clientAddress))
            {
                clientAddress = Dns.GetHostEntry(clientName).AddressList.First();
            }

            bool isNegotiationBased = true;
            string negotiation = DetectorUtil.GetPropertyValue("RDP.Security.Negotiation");
            if (negotiation.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                isNegotiationBased = true;
            }
            else if (negotiation.Equals("false", StringComparison.CurrentCultureIgnoreCase))
            {
                isNegotiationBased = false;
            }
            else
            {
                return false;
            }

            string protocol = DetectorUtil.GetPropertyValue("RDP.Security.Protocol");
            if (protocol.Equals("TLS", StringComparison.CurrentCultureIgnoreCase))
            {
                requestedProtocol = requestedProtocols_Values.PROTOCOL_SSL_FLAG;
                if (!isNegotiationBased)
                {
                    return false;
                }
                encryptedProtocol = EncryptedProtocol.NegotiationTls;
            }
            else if (protocol.Equals("CredSSP", StringComparison.CurrentCultureIgnoreCase))
            {
                requestedProtocol = requestedProtocols_Values.PROTOCOL_HYBRID_FLAG;
                if (isNegotiationBased)
                {
                    encryptedProtocol = EncryptedProtocol.NegotiationCredSsp;
                }
                else
                {
                    encryptedProtocol = EncryptedProtocol.NegotiationTls;
                }
            }
            else if (protocol.Equals("RDP", StringComparison.CurrentCultureIgnoreCase))
            {
                requestedProtocol = requestedProtocols_Values.PROTOCOL_RDP_FLAG;
                encryptedProtocol = EncryptedProtocol.Rdp;
            }
            else
            {
                return false;
            }

            string strWaitTime = DetectorUtil.GetPropertyValue("WaitTime");
            if (strWaitTime != null)
            {
                int waitTime = Int32.Parse(strWaitTime);
                timeout = new TimeSpan(0, 0, waitTime);
            }
            else
            {
                timeout = new TimeSpan(0, 0, 10);
            }

            return true;
        }

        private void connectRDPServer()
        {
            rdpbcgrClient.Connect(encryptedProtocol);
        }

        private void Disconnect()
        {
            if (rdpbcgrClient != null)
            {
                rdpbcgrClient.Disconnect();
                rdpbcgrClient = null;
            }
        }

        private bool EstablishRDPConnection(
            Configs config,
            requestedProtocols_Values requestedProtocols,
            string[] SVCNames,
            CompressionType highestCompressionTypeSupported,
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
            // Connection Initiation
            SendClientX224ConnectionRequest(requestedProtocols);
            Server_X_224_Connection_Confirm_Pdu connectionConfirmPdu = ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(timeout);
            if (connectionConfirmPdu == null)
            {
                return false;
            }

            // Basic Settings Exchange
            SendClientMCSConnectInitialPDU(
                SVCNames,
                supportEGFX,
                supportAutoDetect,
                supportHeartbeatPDU,
                supportMultitransportReliable,
                supportMultitransportLossy,
                false);
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response connectResponsePdu = ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(timeout);
            if (connectResponsePdu == null)
            {
                return false;
            }

            // Channel Connection
            SendClientMCSErectDomainRequest();
            SendClientMCSAttachUserRequest();
            Server_MCS_Attach_User_Confirm_Pdu userConfirmPdu = ExpectPacket<Server_MCS_Attach_User_Confirm_Pdu>(timeout);
            if (userConfirmPdu == null)
            {
                return false;
            }
            ChannelJoinRequestAndConfirm();

            // RDP Security Commencement
            if (rdpbcgrClient.Context.ServerSelectedProtocol == (uint)selectedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                SendClientSecurityExchangePDU();
            }

            // Secure Settings Exchange
            SendClientInfoPDU(config, highestCompressionTypeSupported, isReconnect, autoLogon);

            // Licensing
            Server_License_Error_Pdu_Valid_Client licenseErrorPdu = ExpectPacket<Server_License_Error_Pdu_Valid_Client>(timeout);
            if (licenseErrorPdu == null)
            {
                return false;
            }

            // Capabilities Exchange
            Server_Demand_Active_Pdu demandActivePdu = ExpectPacket<Server_Demand_Active_Pdu>(timeout);
            if (demandActivePdu == null)
            {
                return false;
            }
            SendClientConfirmActivePDU(
                supportAutoReconnect,
                supportFastPathInput,
                supportFastPathOutput,
                supportSurfaceCommands,
                supportSVCCompression,
                supportRemoteFXCodec);

            // Connection Finalization
            SendClientSynchronizePDU();
            Server_Synchronize_Pdu syncPdu = ExpectPacket<Server_Synchronize_Pdu>(timeout);
            if (syncPdu == null)
            {
                return false;
            }
            Server_Control_Pdu_Cooperate CoopControlPdu = ExpectPacket<Server_Control_Pdu_Cooperate>(timeout);
            if (CoopControlPdu == null)
            {
                return false;
            }
            SendClientControlCooperatePDU();
            SendClientControlRequestPDU();
            Server_Control_Pdu_Granted_Control grantedControlPdu = ExpectPacket<Server_Control_Pdu_Granted_Control>(timeout);
            if (grantedControlPdu == null)
            {
                return false;
            }
            SendClientFontListPDU();
            Server_Font_Map_Pdu fontMapPdu = ExpectPacket<Server_Font_Map_Pdu>(timeout);
            if (fontMapPdu == null)
            {
                return false;
            }
            return true;
        }

        private void checkSupportedFeatures()
        {
            detectInfo.IsSupportAutoReconnect = supportAutoReconnect();
            detectInfo.IsSupportFastPathInput = supportFastPathInput();

            // Notify the UI for detecting feature supported finished
            DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);
        }

        private bool supportAutoReconnect()
        {
            ITsCapsSet capset = GetServerCapSet(capabilitySetType_Values.CAPSTYPE_GENERAL);
            if (capset != null)
            {
                TS_GENERAL_CAPABILITYSET generalCap = (TS_GENERAL_CAPABILITYSET)capset;
                if (generalCap.extraFlags.HasFlag(extraFlags_Values.AUTORECONNECT_SUPPORTED))
                {
                    return true;
                }
            }
            return false;
        }

        private bool supportFastPathInput()
        {
            ITsCapsSet capset = GetServerCapSet(capabilitySetType_Values.CAPSTYPE_INPUT);
            if (capset != null)
            {
                TS_INPUT_CAPABILITYSET inputCap = (TS_INPUT_CAPABILITYSET)capset;
                if (inputCap.inputFlags.HasFlag(inputFlags_Values.INPUT_FLAG_FASTPATH_INPUT)
                    || inputCap.inputFlags.HasFlag(inputFlags_Values.INPUT_FLAG_FASTPATH_INPUT2))
                {
                    return true;
                }
            }
            return false;
        }

        private ITsCapsSet GetServerCapSet(capabilitySetType_Values capsetType)
        {
            Collection<ITsCapsSet> capsets = this.rdpbcgrClient.Context.demandActivemCapabilitySets;
            if (capsets != null)
            {
                foreach (ITsCapsSet capSet in capsets)
                {
                    if (capSet.GetCapabilityType() == capsetType)
                    {
                        return capSet;
                    }
                }
            }
            return null;
        }

        private void checkSupportedProtocols()
        {
            // Notify the UI for detecting protocol supported finished
            DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);
        }

        private void SendClientX224ConnectionRequest(
            requestedProtocols_Values requestedProtocols,
            bool isRdpNegReqPresent = true,
            bool isRoutingTokenPresent = false,
            bool isCookiePresent = false,
            bool isRdpCorrelationInfoPresent = false)
        {
            Client_X_224_Connection_Request_Pdu x224ConnectReqPdu = rdpbcgrClient.CreateX224ConnectionRequestPdu(requestedProtocols);

            if (isRoutingTokenPresent)
            {
                // Present the routingToken
                x224ConnectReqPdu.routingToken = ASCIIEncoding.ASCII.GetBytes(RdpbcgrTestData.DefaultX224ConnectReqRoutingToken);
                x224ConnectReqPdu.tpktHeader.length += (ushort)x224ConnectReqPdu.routingToken.Length;
                x224ConnectReqPdu.x224Crq.lengthIndicator += (byte)x224ConnectReqPdu.routingToken.Length;
            }
            if (isCookiePresent)
            {
                // Present the cookie
                x224ConnectReqPdu.cookie = RdpbcgrTestData.DefaultX224ConnectReqCookie;
                x224ConnectReqPdu.tpktHeader.length += (ushort)x224ConnectReqPdu.cookie.Length;
                x224ConnectReqPdu.x224Crq.lengthIndicator += (byte)x224ConnectReqPdu.cookie.Length;
            }
            if (!isRdpNegReqPresent)
            {
                // RdpNegReq is already present, remove it if isRdpNegReqPresent is false
                x224ConnectReqPdu.rdpNegData = null;
                int rdpNegDataSize = sizeof(byte) + sizeof(byte) + sizeof(ushort) + sizeof(uint);
                x224ConnectReqPdu.tpktHeader.length -= (ushort)rdpNegDataSize;
                x224ConnectReqPdu.x224Crq.lengthIndicator -= (byte)rdpNegDataSize;
            }
            if (isRdpCorrelationInfoPresent)
            {
                // Present the RdpCorrelationInfo
                x224ConnectReqPdu.rdpCorrelationInfo = new RDP_NEG_CORRELATION_INFO();
                x224ConnectReqPdu.rdpCorrelationInfo.type = RDP_NEG_CORRELATION_INFO_Type.TYPE_RDP_CORRELATION_INFO;
                x224ConnectReqPdu.rdpCorrelationInfo.flags = 0;
                x224ConnectReqPdu.rdpCorrelationInfo.length = 36;
                x224ConnectReqPdu.rdpCorrelationInfo.correlationId = RdpbcgrTestData.GetDefaultX224ConnectReqCorrelationId();
                x224ConnectReqPdu.rdpCorrelationInfo.reserved = new byte[16];
                x224ConnectReqPdu.tpktHeader.length += (ushort)x224ConnectReqPdu.rdpCorrelationInfo.length;
                x224ConnectReqPdu.x224Crq.lengthIndicator += (byte)x224ConnectReqPdu.rdpCorrelationInfo.length;
            }
            rdpbcgrClient.SendPdu(x224ConnectReqPdu);
        }

        private void SendClientMCSConnectInitialPDU(
            string[] SVCNames,
            bool supportEGFX,
            bool supportAutoDetect,
            bool supportHeartbeatPDU,
            bool supportMultitransportReliable,
            bool supportMultitransportLossy,
            bool isMonitorDataPresent)
        {
            Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request request = rdpbcgrClient.CreateMCSConnectInitialPduWithGCCConferenceCreateRequestPdu(
                clientAddress.ToString(),
                RdpbcgrTestData.DefaultClientBuild,
                System.Guid.NewGuid().ToString(),
                encryptionMethod_Values._128BIT_ENCRYPTION_FLAG,
                SVCNames,
                supportMultitransportReliable,
                supportMultitransportLossy,
                isMonitorDataPresent);

            if (supportEGFX)
            {
                // If support RDPEGFX, set flag: RNS_UD_CS_SUPPORT_DYNVC_GFX_PROTOCOL in clientCoreData.earlyCapabilityFlags
                request.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags.actualData |= (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_DYNVC_GFX_PROTOCOL;
            }

            if (supportAutoDetect)
            {
                // If support auto-detect, set flag: RNS_UD_CS_SUPPORT_NETWORK_AUTODETECT in clientCoreData.earlyCapabilityFlags
                request.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags.actualData |= (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_NETWORK_AUTODETECT;
                request.mcsCi.gccPdu.clientCoreData.connnectionType = new ByteClass((byte)ConnnectionType.CONNECTION_TYPE_AUTODETECT);
            }

            if (supportHeartbeatPDU)
            {
                // If support Heartbeat PDU, set flag: RNS_UD_CS_SUPPORT_HEARTBEAT_PDU in clientCoreData.earlyCapabilityFlags
                request.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags.actualData |= (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_HEARTBEAT_PDU;
            }

            rdpbcgrClient.SendPdu(request);
        }

        private void SendClientMCSErectDomainRequest()
        {
            Client_MCS_Erect_Domain_Request request = rdpbcgrClient.CreateMCSErectDomainRequestPdu();
            rdpbcgrClient.SendPdu(request);
        }

        private void SendClientMCSAttachUserRequest()
        {
            Client_MCS_Attach_User_Request request = rdpbcgrClient.CreateMCSAttachUserRequestPdu();
            rdpbcgrClient.SendPdu(request);
        }

        private void SendClientMCSChannelJoinRequest(long channelId)
        {
            Client_MCS_Channel_Join_Request request = rdpbcgrClient.CreateMCSChannelJoinRequestPdu(channelId);
            rdpbcgrClient.SendPdu(request);
        }

        private void ChannelJoinRequestAndConfirm()
        {
            // Add all SVC ids in a List
            List<long> chIdList = new List<long>();
            // Add User channel
            chIdList.Add(rdpbcgrClient.Context.UserChannelId);
            // Add IO channel
            chIdList.Add(rdpbcgrClient.Context.IOChannelId);
            // Add message channel if exist
            long? msgChId = rdpbcgrClient.Context.MessageChannelId;
            if (msgChId != null)
            {
                chIdList.Add(msgChId.Value);
            }
            // Add the other channels from server network data
            if (rdpbcgrClient.Context.VirtualChannelIdStore != null)
            {
                for (int i = 0; i < rdpbcgrClient.Context.VirtualChannelIdStore.Length; i++)
                {
                    chIdList.Add((long)rdpbcgrClient.Context.VirtualChannelIdStore[i]);
                }
            }

            // Start join request and confirm sequence
            foreach (long channelId in chIdList)
            {
                SendClientMCSChannelJoinRequest(channelId);
                Server_MCS_Channel_Join_Confirm_Pdu confirm = ExpectPacket<Server_MCS_Channel_Join_Confirm_Pdu>(timeout);
            }
        }

        private void SendClientSecurityExchangePDU()
        {
            // Create random data
            byte[] clientRandom = RdpbcgrUtility.GenerateRandom(RdpbcgrTestData.ClientRandomSize);
            Client_Security_Exchange_Pdu exchangePdu = rdpbcgrClient.CreateSecurityExchangePdu(clientRandom);
            rdpbcgrClient.SendPdu(exchangePdu);
        }

        private void SendClientInfoPDU(Configs config, CompressionType highestCompressionTypeSupported, bool isReconnect = false, bool autoLogon = true)
        {
            Client_Info_Pdu pdu = rdpbcgrClient.CreateClientInfoPdu(RdpbcgrTestData.DefaultClientInfoPduFlags, config.ServerDomain, config.ServerUserName, config.ServerUserPassword, clientAddress.ToString(), null, null, isReconnect);
            if (autoLogon)
            {
                pdu.infoPacket.flags |= flags_Values.INFO_AUTOLOGON;
            }
            if (highestCompressionTypeSupported != CompressionType.PACKET_COMPR_TYPE_NONE)
            {
                // Set the compression flag
                uint compressionFlag = ((uint)highestCompressionTypeSupported) << 9;
                pdu.infoPacket.flags |= (flags_Values)((uint)pdu.infoPacket.flags | compressionFlag);
            }
            rdpbcgrClient.SendPdu(pdu);
        }

        private void SendClientConfirmActivePDU(
            bool supportAutoReconnect,
            bool supportFastPathInput,
            bool supportFastPathOutput,
            bool supportSurfaceCommands,
            bool supportSVCCompression,
            bool supportRemoteFXCodec)
        {
            // Create capability sets
            RdpbcgrCapSet capSet = new RdpbcgrCapSet();
            Collection<ITsCapsSet> caps = capSet.CreateCapabilitySets(
                supportAutoReconnect,
                supportFastPathInput,
                supportFastPathOutput,
                supportSurfaceCommands,
                supportSVCCompression,
                supportRemoteFXCodec);

            Client_Confirm_Active_Pdu pdu = rdpbcgrClient.CreateConfirmActivePdu(caps);
            rdpbcgrClient.SendPdu(pdu);
        }

        private void SendClientSynchronizePDU()
        {
            Client_Synchronize_Pdu syncPdu = rdpbcgrClient.CreateSynchronizePdu();
            rdpbcgrClient.SendPdu(syncPdu);
        }

        private void SendClientControlCooperatePDU()
        {
            Client_Control_Pdu_Cooperate controlCooperatePdu = rdpbcgrClient.CreateControlCooperatePdu();
            rdpbcgrClient.SendPdu(controlCooperatePdu);
        }

        private void SendClientControlRequestPDU()
        {
            Client_Control_Pdu_Request_Control requestControlPdu = rdpbcgrClient.CreateControlRequestPdu();
            rdpbcgrClient.SendPdu(requestControlPdu);
        }

        private void SendClientPersistentKeyListPDU()
        {
            Client_Persistent_Key_List_Pdu persistentKeyListPdu = rdpbcgrClient.CreatePersistentKeyListPdu();
            rdpbcgrClient.SendPdu(persistentKeyListPdu);
        }

        private void SendClientFontListPDU()
        {
            Client_Font_List_Pdu fontListPdu = rdpbcgrClient.CreateFontListPdu();
            rdpbcgrClient.SendPdu(fontListPdu);
        }

        private void clientInitiatedDisconnect()
        {
            SendClientShutdownRequestPDU();
            Server_Shutdown_Request_Denied_Pdu shutDownReqDeniedPdu = ExpectPacket<Server_Shutdown_Request_Denied_Pdu>(timeout);
            ExpectPacket<Server_Shutdown_Request_Denied_Pdu>(timeout);
            MCS_Disconnect_Provider_Ultimatum_Pdu ultimatumPdu = rdpbcgrClient.CreateMCSDisconnectProviderUltimatumPdu(RdpbcgrTestData.RN_USER_REQUESTED);
            rdpbcgrClient.SendPdu(ultimatumPdu);
            rdpbcgrClient.Disconnect();
        }

        private void SendClientShutdownRequestPDU()
        {
            Client_Shutdown_Request_Pdu request = rdpbcgrClient.CreateShutdownRequestPdu();
            rdpbcgrClient.SendPdu(request);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (rdpbcgrClient != null)
            {
                rdpbcgrClient.Dispose();
                rdpbcgrClient = null;
            }
        }

        T ExpectPacket<T>(TimeSpan timeout) where T : StackPacket
        {
            T receivedPacket = null;
            // Firstly, go through the receive buffer, if have packet with type T, return it.
            if (receiveBuffer.Count > 0)
            {
                lock (receiveBuffer)
                {
                    for (int i = 0; i < receiveBuffer.Count; i++)
                    {
                        if (receiveBuffer[i] is T)
                        {
                            receivedPacket = receiveBuffer[i] as T;
                            receiveBuffer.RemoveAt(i);
                            return receivedPacket;
                        }
                    }
                }
            }

            // Then, expect new packet from lower level transport
            DateTime endTime = DateTime.Now + timeout;
            while (DateTime.Now < endTime)
            {
                timeout = endTime - DateTime.Now;
                if (timeout.TotalMilliseconds > 0)
                {
                    StackPacket packet = null;
                    try
                    {
                        packet = this.ExpectPdu(timeout);
                    }
                    catch (TimeoutException)
                    {
                        packet = null;
                    }

                    if (packet != null)
                    {
                        if (packet is T)
                        {
                            return packet as T;
                        }
                        else
                        {
                            // If the type of received packet is not T, add it into receive buffer
                            lock (receiveBuffer)
                            {
                                receiveBuffer.Add(packet);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private StackPacket ExpectPdu(TimeSpan timeout)
        {
            return rdpbcgrClient.ExpectPdu(timeout);
        }

        #endregion Methods
    }
}
