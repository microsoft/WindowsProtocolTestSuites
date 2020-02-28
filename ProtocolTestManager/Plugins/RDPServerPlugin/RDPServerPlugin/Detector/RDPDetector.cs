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
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpele;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using System.Runtime.InteropServices;
using System.Security.Authentication;

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
        private RdpedycClient rdpedycClient = null;
        private RdpeleClient rdpeleClient = null;

        private string[] SVCNames;
        private int defaultPort = 3389;
        private string clientName;
        private IPAddress clientAddress;
        private EncryptedProtocol encryptedProtocol;
        private requestedProtocols_Values requestedProtocol;
        private TimeSpan timeout;
        #endregion Variables

        #region Received Packets
        Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response connectResponsePdu = null;
        #endregion Received Packets

        private const string SVCNAME_RDPEDYC = "drdynvc";
        private const string DYVNAME_RDPEDYC = "Microsoft::Windows::RDS::Geometry::v08.01";

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
                DetectorUtil.WriteLog("Establish RDP connection with SUT...");

                Initialize(config);
                ConnectRDPServer();

                try
                {
                    bool status = EstablishRDPConnection(
                        config, requestedProtocol, SVCNames,
                        CompressionType.PACKET_COMPR_TYPE_NONE,
                        false,
                        true,
                        false,
                        false,
                        false,
                        false,
                        false);
                    if (!status)
                    {
                        DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                        return false;
                    }
                }
                catch (Exception e)
                {
                    DetectorUtil.WriteLog("" + e.StackTrace);
                }

                DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);

                CheckSupportedFeatures();            

                CheckSupportedProtocols();

                config.RDPEDYCSupported = detectInfo.IsSupportRDPEDYC.ToString();

                config.RDPELESupported = detectInfo.IsSupportRDPELE.ToString();
                
                SetRdpVersion(config);               
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


            // Disconnect
            ClientInitiatedDisconnect();
            Disconnect();

            DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);
            return true;
        }

        private void Initialize(Configs config)
        {
            receiveBuffer = new List<StackPacket>();
            SVCNames = new string[] { SVCNAME_RDPEDYC };
            clientName = config.ClientName;
            LoadConfig();

            int port;
            rdpbcgrClient = new RdpbcgrClient(
                config.ServerDomain,
                config.ServerName,
                config.ServerUserName,
                config.ServerUserPassword,
                clientAddress.ToString(),
                Int32.TryParse(config.ServerPort, out port) ? port : defaultPort
                );            
            rdpbcgrClient.TlsVersion = SslProtocols.None;
        }

        private bool LoadConfig()
        {
            if (!IPAddress.TryParse(clientName, out clientAddress))
            {
                clientAddress = Dns.GetHostEntry(clientName).AddressList.First();
            }
            
            requestedProtocol = requestedProtocols_Values.PROTOCOL_RDP_FLAG;
            encryptedProtocol = EncryptedProtocol.Rdp;
            
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

        private void ConnectRDPServer()
        {
            rdpbcgrClient.Connect(encryptedProtocol);
        }

        private void Disconnect()
        {
            rdpbcgrClient.Disconnect();
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
            Server_X_224_Connection_Confirm_Pdu connectionConfirmPdu = ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(new TimeSpan(0, 0, 60));
            if (connectionConfirmPdu == null)
            {
                TimeSpan waitTime = new TimeSpan(0, 0, 1);
                Server_X_224_Negotiate_Failure_Pdu failurePdu = ExpectPacket<Server_X_224_Negotiate_Failure_Pdu>(waitTime);
                if (failurePdu != null)
                {
                    DetectorUtil.WriteLog("Received a Server X224 Connection confirm with RDP_NEG_FAILURE structure.");
                }
                DetectorUtil.WriteLog("Expecting a Server X224 Connection Confirm PDU.");
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
            connectResponsePdu = ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(timeout);
            if (connectResponsePdu == null)
            {
                return false;
            }

            bool serverSupportUDPFECR = false;
            bool serverSupportUDPFECL = false;
            if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData != null)
            {
                if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR))
                {
                    serverSupportUDPFECR = true;
                }
                if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL))
                {
                    serverSupportUDPFECL = true;
                }
            }
            detectInfo.IsSupportRDPEMT = serverSupportUDPFECR || serverSupportUDPFECL;

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
            rdpeleClient = new RdpeleClient(rdpbcgrClient);

            try
            {
                detectInfo.IsSupportRDPELE = ProcessLicenseSequence(config, timeout);
            }
            catch
            {
                detectInfo.IsSupportRDPELE = false;
            }
            rdpeleClient.Dispose();

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
                supportSVCCompression);

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
        private void CheckSupportedFeatures()
        {
            DetectorUtil.WriteLog("Check specified features support...");

            detectInfo.IsSupportAutoReconnect = SupportAutoReconnect();
            detectInfo.IsSupportFastPathInput = SupportFastPathInput();

            // Notify the UI for detecting feature supported finished
            DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);
        }

        private bool SupportAutoReconnect()
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

        private bool SupportFastPathInput()
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

        private void CheckSupportedProtocols()
        {
            // Notify the UI for detecting protocol supported finished
            DetectorUtil.WriteLog("Check specified protocols support...");
            bool serverSupportUDPFECR = false;
            bool serverSupportUDPFECL = false;

            if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData != null)
            {
                if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR))
                {
                    serverSupportUDPFECR = true;
                }
                if (connectResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL))
                {
                    serverSupportUDPFECL = true;
                }
            }
            detectInfo.IsSupportRDPEMT = serverSupportUDPFECR || serverSupportUDPFECL;

            if (detectInfo.IsSupportRDPEMT)
            {
                DetectorUtil.WriteLog("Detect RDPEMT supported");
            }
            else
            {
                DetectorUtil.WriteLog("Detect RDPEMT unsupported");
            }

            rdpedycClient = new RdpedycClient(rdpbcgrClient.Context, false);

            try
            {
                DynamicVirtualChannel channel = rdpedycClient.ExpectChannel(timeout, DYVNAME_RDPEDYC, DynamicVC_TransportType.RDP_TCP);
                if (channel != null)
                {
                    detectInfo.IsSupportRDPEDYC = true;

                }
                rdpedycClient.CloseChannel((ushort)channel.ChannelId);

            }
            catch
            {
                detectInfo.IsSupportRDPEDYC = false;
            }

            if (detectInfo.IsSupportRDPEDYC)
            {
                DetectorUtil.WriteLog("Detect RDPEDYC supported");
            }
            else
            {
                DetectorUtil.WriteLog("Detect RDPEDYC unsupported");
            }

            if (detectInfo.IsSupportRDPELE)
            {
                DetectorUtil.WriteLog("Detect RDPELE supported");
            }
            else
            {
                DetectorUtil.WriteLog("Detect RDPELE unsupported");
            }

            DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);
        }
      
        private void SetRdpVersion(Configs config)
        {
            DetectorUtil.WriteLog("Detect RDP version...");

            config.Version = DetectorUtil.GetPropertyValue("RDP.Version");

            if (connectResponsePdu.mcsCrsp.gccPdu.serverCoreData == null)
            {
                DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                DetectorUtil.WriteLog("Detect RDP version failed, serverCoreData in Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response does not exist!");
            }

            TS_UD_SC_CORE_version_Values rdpVersion = connectResponsePdu.mcsCrsp.gccPdu.serverCoreData.version;
            if (rdpVersion == TS_UD_SC_CORE_version_Values.V1)
            {
                config.Version = "4.0";
            }
            else if (rdpVersion == TS_UD_SC_CORE_version_Values.V2)
            {
                config.Version = "8.1"; // RDP 5.0, 5.1, 5.2, 6.0, 6.1, 7.0, 7.1, 8.0, and 8.1 servers
            }
            else if (rdpVersion == TS_UD_SC_CORE_version_Values.V3)
            {
                config.Version = "10.0";
            }
            else if (rdpVersion == TS_UD_SC_CORE_version_Values.V4)
            {
                config.Version = "10.1";
            }
            else if (rdpVersion == TS_UD_SC_CORE_version_Values.V5)
            {
                config.Version = "10.2";
            }
            else if (rdpVersion == TS_UD_SC_CORE_version_Values.V6)
            {
                config.Version = "10.3";
            }
            else if (rdpVersion == TS_UD_SC_CORE_version_Values.V7)
            {
                config.Version = "10.4";
            }
            else if (rdpVersion == TS_UD_SC_CORE_version_Values.V8)
            {
                config.Version = "10.5";
            }
            else if (rdpVersion == TS_UD_SC_CORE_version_Values.V9)
            {
                config.Version = "10.6";
            }
            else if (rdpVersion == TS_UD_SC_CORE_version_Values.V10)
            {
                config.Version = "10.7";
            }
            else
            {
                DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                DetectorUtil.WriteLog("Detect RDP version failed, unknown version detected!");
            }

            detectInfo.Version = connectResponsePdu.mcsCrsp.gccPdu.serverCoreData.version;

            DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);
            DetectorUtil.WriteLog("Detect RDP version finished.");
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
                clientName,
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

        private bool ProcessLicenseSequence(Configs config, TimeSpan timeout)
        {
            try
            {
                TS_LICENSE_PDU licensePdu = rdpeleClient.ExpectPdu(timeout);

                if (licensePdu.preamble.bMsgType == bMsgType_Values.ERROR_ALERT)
                {                    
                    // If the target machine is a personal terminal server, whether the client sends the license or not, 
                    // the server always sends a license error message with the error code STATUS_VALID_CLIENT and the state transition code ST_NO_TRANSITION. 
                    if (dwErrorCode_Values.STATUS_VALID_CLIENT != licensePdu.LicensingMessage.LicenseError.Value.dwErrorCode)
                    {
                        DetectorUtil.WriteLog($"A license error message with the error code STATUS_VALID_CLIENT should be received, but the real error code is {licensePdu.LicensingMessage.LicenseError.Value.dwErrorCode}.");
                    }
                    return false;
                }

                DetectorUtil.WriteLog("Start RDP license procedure");
                if (bMsgType_Values.LICENSE_REQUEST != licensePdu.preamble.bMsgType)
                {
                    DetectorUtil.WriteLog($"A LICENSE_REQUEST message should be received from server, but the real message type is {licensePdu.preamble.bMsgType}");
                }

                rdpeleClient.SendClientNewLicenseRequest(
                    KeyExchangeAlg.KEY_EXCHANGE_ALG_RSA, (uint)Client_OS_ID.CLIENT_OS_ID_WINNT_POST_52 | (uint)Client_Image_ID.CLIENT_IMAGE_ID_MICROSOFT, config.ServerUserName, config.ClientName);
                licensePdu = rdpeleClient.ExpectPdu(timeout);
                if (bMsgType_Values.PLATFORM_CHALLENGE != licensePdu.preamble.bMsgType)
                {
                    DetectorUtil.WriteLog($"A PLATFORM_CHALLENGE message should be received from server, but the real message type is {licensePdu.preamble.bMsgType}");
                    return false;
                }

                Random random = new Random();
                CLIENT_HARDWARE_ID clientHWID = new CLIENT_HARDWARE_ID
                {
                    PlatformId = (uint)Client_OS_ID.CLIENT_OS_ID_WINNT_POST_52 | (uint)Client_Image_ID.CLIENT_IMAGE_ID_MICROSOFT,
                    Data1 = (uint)random.Next(),
                    Data2 = (uint)random.Next(),
                    Data3 = (uint)random.Next(),
                    Data4 = (uint)random.Next()
                };
                rdpeleClient.SendClientPlatformChallengeResponse(clientHWID);
                licensePdu = rdpeleClient.ExpectPdu(timeout);
                if (bMsgType_Values.NEW_LICENSE != licensePdu.preamble.bMsgType)
                {
                    DetectorUtil.WriteLog($"A NEW_LICENSE message should be received from server, but the real message type is {licensePdu.preamble.bMsgType}");
                    return false;
                }
                DetectorUtil.WriteLog("End RDP license procedure");
                return true;
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog("RDP license procedure throws exception: " + e.Message);
                return false;
            }
        }

        private void SendClientConfirmActivePDU(
            bool supportAutoReconnect,
            bool supportFastPathInput,
            bool supportFastPathOutput,
            bool supportSVCCompression)
        {
            Collection<ITsCapsSet> caps = rdpbcgrClient.CreateCapabilitySets(
                supportAutoReconnect,
                supportFastPathInput,
                supportFastPathOutput,
                supportSVCCompression);
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

        private void ClientInitiatedDisconnect()
        {
            SendMCSDisconnectProviderUltimatumPDU();
        }

        private void SendMCSDisconnectProviderUltimatumPDU()
        {
            MCS_Disconnect_Provider_Ultimatum_Pdu ultimatumPdu = rdpbcgrClient.CreateMCSDisconnectProviderUltimatumPdu(RdpbcgrTestData.RN_USER_REQUESTED);
            rdpbcgrClient.SendPdu(ultimatumPdu);
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

        private T ExpectPacket<T>(TimeSpan timeout) where T : StackPacket
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
                        else if (packet is ErrorPdu)
                        {
                            DetectorUtil.WriteLog("Expect packste throws exception: " + string.Format(((ErrorPdu)packet).ErrorMessage));
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