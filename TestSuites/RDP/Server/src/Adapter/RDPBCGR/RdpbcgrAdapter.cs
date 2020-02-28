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
using Microsoft.Protocols.TestSuites.Rdp;
using System.Security.Authentication;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpele;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    #region Delegate

    public delegate void ServerX224ConnectionConfirmHandler(Server_X_224_Connection_Confirm_Pdu x224Confirm);
    public delegate void ServerX224NegotiateFailurePDUHandler(Server_X_224_Negotiate_Failure_Pdu x224Failure);
    public delegate void ServerEarlyUserAuthorizationResultPDUHandler(Early_User_Authorization_Result_PDU earlyUserAuthorizationResultPDU);
    public delegate void ServerMCSConnectResponseHandler(Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsConnectResponse);
    public delegate void ServerMCSAttachUserConfirmHandler(Server_MCS_Attach_User_Confirm_Pdu attachUserConfirm);
    public delegate void ServerMCSChannelJoinConfirmHandler(Server_MCS_Channel_Join_Confirm_Pdu channelJoinConfirm);
    public delegate void ServerLicenseErrorPDUHandler(Server_License_Error_Pdu_Valid_Client licenseErrorPdu);
    public delegate void ServerDemandActivePDUHandler(Server_Demand_Active_Pdu demandActivePdu);
    public delegate void ServerSynchronizePDUHandler(Server_Synchronize_Pdu synchronizePdu);
    public delegate void ServerCooperateControlPDUHandler(Server_Control_Pdu_Cooperate controlPdu);
    public delegate void ServerGrantedControlPDUHandler(Server_Control_Pdu_Granted_Control controlPdu);
    public delegate void ServerFontMapPDUHandler(Server_Font_Map_Pdu fontMapPdu);
    public delegate void ServerShutdownRequestDeniedPDUHandler(Server_Shutdown_Request_Denied_Pdu shutdownRequest);
    public delegate void ServerMCSDisconnectProviderUltimatumHandler(MCS_Disconnect_Provider_Ultimatum_Pdu disconnectProvider);
    public delegate void ServerDeactivateAllPDUHandler(Server_Deactivate_All_Pdu deactiveAllPdu);
    public delegate void ServerVirtualChannelPDUHandler(Virtual_Channel_RAW_Server_Pdu virtualChannelPdu);
    public delegate void ServerSlowPathOutputUpdatePDUHandler(SlowPathOutputPdu updatePdu);
    public delegate void ServerFastPathUpdatePDUHandler(TS_FP_UPDATE_PDU updatePdu);
    public delegate void ServerRedirectionPacketHandler(Server_Redirection_Pdu redirectionPdu);
    public delegate void EnhancedSecurityServerRedirectionPacketHandler(Enhanced_Security_Server_Redirection_Pdu redirectionPdu);
    public delegate void ServerAutoDetectRequestHandler(Server_Auto_Detect_Request_PDU autoDetectRequest);
    public delegate void ServerInitiateMultitransportRequestHandler(Server_Initiate_Multitransport_Request_PDU multitransportReq);
    public delegate void ServerHeartbeatPDUHandler(Server_Heartbeat_PDU heartbeatPdu);
    public delegate void ServerSaveSessionInfoPDUHandler(Server_Save_Session_Info_Pdu saveSessionInfoPdu);

    #endregion Delegate

    public partial class RdpbcgrAdapter : ManagedAdapterBase
    {
        #region Variables

        private const string SVCNameForRDPEDYC = "DRDYNVC";
        public RdpbcgrClient rdpbcgrClientStack;
        public RdpeleClient rdpeleClient;
        private TestConfig testConfig;

        private int sendInterval = 100;

        private List<StackPacket> receiveBuffer;

        private bool isLogon = false;

        private DateTime bandwidthDetectStartTime = DateTime.Now;
        private uint byteCount = 0;

        private uint baseRTT = 0;
        private uint bandwidth = 0;
        private uint averageRTT = 0;

        private bool serverSupportUDPFECR = false;
        private bool serverSupportUDPFECL = false;
        private bool serverSupportUDPPrefferred = false;
        private EncryptedProtocol encryptedProtocol;
        #endregion Variables

        #region Properties

        /// <summary>
        /// Whether the RDP server support bitmap cache host 
        /// </summary>
        public bool IsBitmapCacheHostSupport
        {
            get
            {
                return rdpbcgrClientStack.Context.IsBitmapCacheHostSupport;
            }
        }

        public bool IsWindowsImplementation
        {
            get
            {
                return testConfig.isWindowsImplementation;
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// Event will be raised when a Server X224 Connection Confirm PDU is received
        /// </summary>
        public event ServerX224ConnectionConfirmHandler OnServerX224ConnectionConfirmReceived;

        /// <summary>
        /// Event will be raised when a Server X224 Negotiate Failure PDU is received
        /// </summary>
        public event ServerX224NegotiateFailurePDUHandler OnServerX224NegotiateFailurePDUReceived;

        /// <summary>
        /// Event will be raised when a Server Early User Authorization Result PDU is received
        /// </summary>
        public event ServerEarlyUserAuthorizationResultPDUHandler OnServerEarlyAuthorizationResultPDUHandler;

        /// <summary>
        /// Event will be raised when a Server MCS Connect Response PDU is received
        /// </summary>
        public event ServerMCSConnectResponseHandler OnServerMCSConnectResponseReceived;

        /// <summary>
        /// Event will be raised when a Server MCS Attach User Confirm PDU is received
        /// </summary>
        public event ServerMCSAttachUserConfirmHandler OnServerMCSAttachUserConfirmReceived;

        /// <summary>
        /// Event will be raised when a Server MCS Channel Join Confirm PDU is received
        /// </summary>
        public event ServerMCSChannelJoinConfirmHandler OnServerMCSChannelJoinConfirmReceived;

        /// <summary>
        /// Event will be raised when a Server License Error PDU is received
        /// </summary>
        public event ServerLicenseErrorPDUHandler OnServerLicenseErrorPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Demand Active PDU is received
        /// </summary>
        public event ServerDemandActivePDUHandler OnServerDemandActivePDUReceived;

        /// <summary>
        /// Event will be raised when a Server Synchronize PDU is received
        /// </summary>
        public event ServerSynchronizePDUHandler OnServerSynchronizePDUReceived;

        /// <summary>
        /// Event will be raised when a Server Control PDU (Cooperate) is received
        /// </summary>
        public event ServerCooperateControlPDUHandler OnServerCooperateControlPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Control PDU (Granted Control) is received
        /// </summary>
        public event ServerGrantedControlPDUHandler OnServerGrantedControlPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Font Map PDU is received
        /// </summary>
        public event ServerFontMapPDUHandler OnServerFontMapPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Shutdown Request Denied PDU is received
        /// </summary>
        public event ServerShutdownRequestDeniedPDUHandler OnServerShutdownRequestDeniedReceived;

        /// <summary>
        /// Event will be raised when a Server MCS Disconnect Provider Ultimatum PDU is received
        /// </summary>
        public event ServerMCSDisconnectProviderUltimatumHandler OnServerMCSDisconnectProviderUltimatumReceived;

        /// <summary>
        /// Event will be raised when a Server Deactivate All PDU is received
        /// </summary>
        public event ServerDeactivateAllPDUHandler OnServerDeactivateAllPDUReceived;

        /// <summary>
        /// Event will be raised when a  is received
        /// </summary>
        public event ServerVirtualChannelPDUHandler OnServerVirtualChannelPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Graphics Update PDU or Server Pointer Update PDU is received
        /// </summary>
        public event ServerSlowPathOutputUpdatePDUHandler OnServerSlowPathOutputUpdatePDUReceived;

        /// <summary>
        /// Event will be raised when a Server Fast-Path Update PDU  is received
        /// </summary>
        public event ServerFastPathUpdatePDUHandler OnServerFastPathUpdatePDUReceived;

        /// <summary>
        /// Event will be raised when a Server Redirection Packet is received
        /// </summary>
        public event ServerRedirectionPacketHandler OnServerRedirectionPacketReceived;

        /// <summary>
        /// Event will be raised when a Enhanced Security Server Redirection Packet is received
        /// </summary>
        public event EnhancedSecurityServerRedirectionPacketHandler OnEnhancedSecurityServerRedirectionPacketReceived;

        /// <summary>
        /// Event will be raised when a Server Auto-Detect Request PDU is received
        /// </summary>
        public event ServerAutoDetectRequestHandler OnServerAutoDetectRequestReceived;

        /// <summary>
        /// Event will be raised when a Server Initiate Multitransport Request PDU is received
        /// </summary>
        public event ServerInitiateMultitransportRequestHandler OnServerInitiateMultitransportRequestReceived;

        /// <summary>
        /// Event will be raised when a Server Heartbeat PDU is received
        /// </summary>
        public event ServerHeartbeatPDUHandler OnServerHeartbeatPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Save Session Info PDU is received
        /// </summary>
        public event ServerSaveSessionInfoPDUHandler OnServerSaveSessionInfoPDUReceived;

        #endregion Event

        public RdpbcgrAdapter(TestConfig testConfig)
        {
            this.testConfig = testConfig;
        }

        /// <summary>
        /// Init the RdpbcgrAdapter
        /// </summary>
        /// <param name="testSite">The test site this adapter belong to</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            receiveBuffer = new List<StackPacket>();
            rdpbcgrClientStack = new RdpbcgrClient(testConfig.domain,
                testConfig.serverName,
                testConfig.userName,
                testConfig.password,
                testConfig.localAddress,
                testConfig.serverPort);
            rdpbcgrClientStack.TlsVersion = testConfig.tlsVersion;
            rdpeleClient = new RdpeleClient(rdpbcgrClientStack);
            isLogon = false;
        }

        /// <summary>
        /// Reset the Adapter
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            receiveBuffer.Clear();

            #region Unregister Events

            OnServerX224ConnectionConfirmReceived = null;
            OnServerEarlyAuthorizationResultPDUHandler = null;
            OnServerMCSConnectResponseReceived = null;
            OnServerMCSAttachUserConfirmReceived = null;
            OnServerMCSChannelJoinConfirmReceived = null;
            OnServerLicenseErrorPDUReceived = null;
            OnServerDemandActivePDUReceived = null;
            OnServerSynchronizePDUReceived = null;
            OnServerCooperateControlPDUReceived = null;
            OnServerGrantedControlPDUReceived = null;
            OnServerFontMapPDUReceived = null;
            OnServerShutdownRequestDeniedReceived = null;
            OnServerMCSDisconnectProviderUltimatumReceived = null;
            OnServerDeactivateAllPDUReceived = null;
            OnServerVirtualChannelPDUReceived = null;
            OnServerSlowPathOutputUpdatePDUReceived = null;
            OnServerFastPathUpdatePDUReceived = null;
            OnServerRedirectionPacketReceived = null;
            OnServerAutoDetectRequestReceived = null;
            OnServerInitiateMultitransportRequestReceived = null;
            OnServerHeartbeatPDUReceived = null;
            OnServerSaveSessionInfoPDUReceived = null;

            #endregion Unregister Events

            if (rdpbcgrClientStack != null)
            {
                rdpbcgrClientStack.Disconnect();
            }

            isLogon = false;
        }
        #region Methods in IRdpbcgrAdapter

        #region Connection Methods

        /// <summary>
        /// Establish transport connection with RDP Server
        /// </summary>
        /// <param name="encryptedProtocol">Enctypted protocol</param>
        public void ConnectToServer(EncryptedProtocol encryptedProtocol)
        {
            this.encryptedProtocol = encryptedProtocol;

            rdpbcgrClientStack.Connect(encryptedProtocol);
        }

        /// <summary>
        /// Disconnect transport connection with RDP Server
        /// </summary>
        public void Disconnect()
        {
            if (rdpbcgrClientStack != null)
            {
                rdpbcgrClientStack.Disconnect();
            }
            isLogon = false;
        }

        /// <summary>
        /// Perform a client-initiated disconnection.
        /// </summary>
        public void ClientInitiatedDisconnect()
        {
            if (isLogon)
            {
                Site.Log.Add(LogEntryKind.Comment, "User have been logged on in the connection, perform a client initiated disconnection before disconnecting transport connection.");
                this.SendClientShutdownRequestPDU();
                this.ExpectPacket<Server_Shutdown_Request_Denied_Pdu>(testConfig.timeout);
                MCS_Disconnect_Provider_Ultimatum_Pdu ultimatumPdu = rdpbcgrClientStack.CreateMCSDisconnectProviderUltimatumPdu(RdpConstValue.RN_USER_REQUESTED);
                this.SendPdu(ultimatumPdu);
            }
            this.Disconnect();
        }

        /// <summary>
        /// Send a Client X224 Connection Request PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="requestedProtocols">Flags indicate supported security protocols</param>
        /// <param name="isRdpNegReqPresent">Whether RdpNegReq is present</param>
        /// <param name="isRoutingTokenPresent">Whether RoutingToken is present</param>
        /// <param name="isCookiePresent">Whether Cookie is present</param>
        /// <param name="isRdpCorrelationInfoPresent">Whether RdpCorrelationInfo is present</param>
        public void SendClientX224ConnectionRequest(NegativeType invalidType,
            requestedProtocols_Values requestedProtocols,
            bool isRdpNegReqPresent = true,
            bool isRoutingTokenPresent = false,
            bool isCookiePresent = false,
            bool isRdpCorrelationInfoPresent = false)
        {
            Client_X_224_Connection_Request_Pdu x224ConnectReqPdu = rdpbcgrClientStack.CreateX224ConnectionRequestPdu(requestedProtocols);

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

            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.X224ConnectionRequest_LengthTooSmall)
            {
                // set invalid length in TPKT header, less than 11 bytes.
                x224ConnectReqPdu.tpktHeader.length = 10; ;
            }
            else if (invalidType == NegativeType.X224ConnectionRequest_InvalidClass)
            {
                // set invalid class option, not 0.
                x224ConnectReqPdu.x224Crq.classOptions = 1;
            }

            if (invalidType == NegativeType.None)
            {
                // Check whether selected protocol is expected.
                OnServerX224ConnectionConfirmReceived += RdpbcgrAdapter_OnServerX224ConnectionConfirmReceived;
            }

            SendPdu(x224ConnectReqPdu);
        }

        private void RdpbcgrAdapter_OnServerX224ConnectionConfirmReceived(Server_X_224_Connection_Confirm_Pdu x224Confirm)
        {
            switch (encryptedProtocol)
            {
                case EncryptedProtocol.Rdp:
                    {
                        Site.Assume.AreEqual(selectedProtocols_Values.PROTOCOL_RDP_FLAG, x224Confirm.rdpNegData.selectedProtocol, "The selected protocol should be RDP when RDP is configured as the security protocol.");
                    }
                    break;

                case EncryptedProtocol.NegotiationTls:
                    {
                        Site.Assume.AreEqual(selectedProtocols_Values.PROTOCOL_SSL_FLAG, x224Confirm.rdpNegData.selectedProtocol, "The selected protocol should be SSL when TLS is configured as the security protocol.");
                    }
                    break;

                case EncryptedProtocol.NegotiationCredSsp:
                case EncryptedProtocol.DirectCredSsp:
                    {
                        bool isCredSspSelected = x224Confirm.rdpNegData.selectedProtocol == selectedProtocols_Values.PROTOCOL_HYBRID_FLAG || x224Confirm.rdpNegData.selectedProtocol == selectedProtocols_Values.PROTOCOL_HYBRID_EX;

                        Site.Assume.IsTrue(isCredSspSelected, "The selected protocol should be HYBRID or HYBRID_EX when CredSSP is configured as the security protocol.");

                        if (encryptedProtocol == EncryptedProtocol.NegotiationCredSsp && x224Confirm.rdpNegData.selectedProtocol == selectedProtocols_Values.PROTOCOL_HYBRID_EX)
                        {
                            // Check the early user authorization result.
                            OnServerEarlyAuthorizationResultPDUHandler += RdpbcgrAdapter_OnServerEarlyAuthorizationResultPDUHandler;
                        }
                    }
                    break;

                default:
                    throw new InvalidOperationException("Unexpected security protocol!");
            }
        }

        private void RdpbcgrAdapter_OnServerEarlyAuthorizationResultPDUHandler(Early_User_Authorization_Result_PDU authorizationResult)
        {
            Site.Assert.AreEqual(Authorization_Result_value.AUTHZ_SUCCESS, authorizationResult.authorizationResult, "The authorizationResult should be AUTHZ_SUCCESS when user has permission to access the server.");
        }

        /// <summary>
        /// Send a Client MCS Connect Initial PDU with GCC Conference Create Request
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="SVCNames">Array of static virtual channels' name</param>
        /// <param name="supportEGFX">Set the support of RDPEGFX</param>
        /// <param name="supportAutoDetect">Set the support of auto-detect</param>
        /// <param name="supportHeartbeatPDU">Set the support of Heartbeat PDU</param>
        /// <param name="supportMultitransportReliable">Set the support of reliable multitransport</param>
        /// <param name="supportMultitransportLossy">Set the support of lossy multitransport</param>
        /// <param name="isMonitorDataPresent">Whether the Client Monitor Data is present</param>
        public void SendClientMCSConnectInitialPDU(NegativeType invalidType,
            string[] SVCNames,
            bool supportEGFX,
            bool supportAutoDetect,
            bool supportHeartbeatPDU,
            bool supportMultitransportReliable,
            bool supportMultitransportLossy,
            bool isMonitorDataPresent)
        {
            Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request request = rdpbcgrClientStack.CreateMCSConnectInitialPduWithGCCConferenceCreateRequestPdu(Dns.GetHostName(),
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

            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                // Invalid TPKT Length
                request.tpktHeader.length -= 1;
            }
            else if (invalidType == NegativeType.MCSConnectInitialPDU_InvalidH221NonStandardkey)
            {
                // Set an invalid H.221 nonstandard key, not "Duca"
                request.mcsCi.gccPdu.h221Key = "DnMc";
            }
            else if (invalidType == NegativeType.MCSConnectInitialPDU_InvalidEncodedLength)
            {
                // Create a wrapper, this wrapper override the ToBytes method to set an invalid encoded length
                request = new Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request_Ex(request, rdpbcgrClientStack.Context);
            }
            else if (invalidType == NegativeType.MCSConnectInitialPDU_CoreData_DesktopWidthHeightTooLarge)
            {
                // Set too large value on DesktopWidth and DesktopHeight
                request.mcsCi.gccPdu.clientCoreData.desktopWidth = 1000;
                request.mcsCi.gccPdu.clientCoreData.desktopHeight = 1000;
            }
            else if (invalidType == NegativeType.MCSConnectInitialPDU_CoreData_InvalidColorDepth)
            {
                // colorDepth field does not contain a valid color-depth and the postBeta2ColorDepth field is not present
                request.mcsCi.gccPdu.clientCoreData.colorDepth = colorDepth_Values.None;
                request.mcsCi.gccPdu.clientCoreData.postBeta2ColorDepth = null;
            }
            else if (invalidType == NegativeType.MCSConnectInitialPDU_CoreData_InvalidPostBeta2ColorDepth)
            {
                // postBeta2ColorDepth field does not contain a valid color-depth and the highColorDepth field is not present,
                request.mcsCi.gccPdu.clientCoreData.postBeta2ColorDepth = new UInt16Class((ushort)postBeta2ColorDepth_Values.None);
                request.mcsCi.gccPdu.clientCoreData.highColorDepth = null;
            }
            else if (invalidType == NegativeType.MCSConnectInitialPDU_CoreData_InvalidServerSelectedProtocol)
            {
                // serverSelectedProtocol field does not contain the same value that the server transmitted to the client in the RDP Negotiation Response 
                if (request.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol != null)
                {
                    if (request.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol.actualData == (uint)selectedProtocols_Values.PROTOCOL_RDP_FLAG)
                    {
                        request.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol.actualData = (uint)selectedProtocols_Values.PROTOCOL_SSL_FLAG;
                    }
                    else if (request.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol.actualData == (uint)selectedProtocols_Values.PROTOCOL_SSL_FLAG)
                    {
                        request.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol.actualData = (uint)selectedProtocols_Values.PROTOCOL_HYBRID_FLAG;
                    }
                    else if (request.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol.actualData == (uint)selectedProtocols_Values.PROTOCOL_HYBRID_FLAG)
                    {
                        request.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol.actualData = (uint)selectedProtocols_Values.PROTOCOL_HYBRID_EX;
                    }
                    else if (request.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol.actualData == (uint)selectedProtocols_Values.PROTOCOL_HYBRID_EX)
                    {
                        request.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol.actualData = (uint)selectedProtocols_Values.PROTOCOL_RDP_FLAG;
                    }
                }
            }
            else if (invalidType == NegativeType.MCSConnectInitialPDU_SecurityData_InvalidEncryptionMethods)
            {
                // No valid flag is present in encryptionMethods and extEncryptionMethods fields of Client Security Data 
                request.mcsCi.gccPdu.clientSecurityData.encryptionMethods = encryptionMethod_Values.None;
                request.mcsCi.gccPdu.clientSecurityData.extEncryptionMethods = 0;
            }
            else if (invalidType == NegativeType.MCSConnectInitialPDU_InvalidChannelCount)
            {
                // Invalid channelCount field in Client Network Data 
                request.mcsCi.gccPdu.clientNetworkData.channelCount = 32;
            }

            SendPdu(request);
        }

        /// <summary>
        /// Send a Client MCS Erect Domain Request PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        public void SendClientMCSErectDomainRequest(NegativeType invalidType)
        {
            Client_MCS_Erect_Domain_Request request = rdpbcgrClientStack.CreateMCSErectDomainRequestPdu();

            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                // Invalid TPKT length
                request.tpktHeader.length -= 1;
            }
            else if (invalidType == NegativeType.MCSErectDomainRequest_SubHeightNotPresent
                || invalidType == NegativeType.MCSErectDomainRequest_SubintervalNotPresent)
            {
                // Create a wrapper, which override ToBytes method to make SubHeight or Subinterval not present
                request = new Client_MCS_Erect_Domain_Request_Ex(request, rdpbcgrClientStack.Context, invalidType);
            }

            SendPdu(request);
        }

        /// <summary>
        /// Send a Client MCS Attach User Request PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        public void SendClientMCSAttachUserRequest(NegativeType invalidType)
        {
            Client_MCS_Attach_User_Request request = rdpbcgrClientStack.CreateMCSAttachUserRequestPdu();
            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                request.tpktHeader.length -= 1;
            }

            SendPdu(request);
        }

        /// <summary>
        /// Send a Client MCS Channel Join Request PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="channelId">Channel ID</param>
        public void SendClientMCSChannelJoinRequest(NegativeType invalidType, long channelId)
        {
            Client_MCS_Channel_Join_Request request = rdpbcgrClientStack.CreateMCSChannelJoinRequestPdu(channelId);
            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                request.tpktHeader.length -= 1;
            }

            SendPdu(request);
        }

        /// <summary>
        /// Send a Client Security Exchange PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        public void SendClientSecurityExchangePDU(NegativeType invalidType)
        {
            // Create random data
            byte[] clientRandom = RdpbcgrUtility.GenerateRandom(RdpConstValue.CLIENT_RANDOM_SIZE);

            Client_Security_Exchange_Pdu exchangePdu = rdpbcgrClientStack.CreateSecurityExchangePdu(clientRandom);
            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                exchangePdu.commonHeader.tpktHeader.length -= 1;
            }
            else if (invalidType == NegativeType.InvalidFlagsInSecurityHeader)
            {
                // Not set SEC_EXCHANGE_PKT flag in flags field of securityHeader
                exchangePdu.commonHeader.securityHeader.flags ^= TS_SECURITY_HEADER_flags_Values.SEC_EXCHANGE_PKT;
            }
            else if (invalidType == NegativeType.SecurityExchangePDU_InvalidLength)
            {
                // Set length field an invalid value
                exchangePdu.securityExchangePduData.length -= 1;
            }

            SendPdu(exchangePdu);
        }

        /// <summary>
        /// Send a Client Info PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="highestCompressionTypeSupported">Indicate the highest compression type supported</param>
        /// <param name="isReconnect">Whether this is in a reconnection sequence</param>
        /// <param name="autoLogon">Indicate wether auto logon using username and password of info packet</param>
        public void SendClientInfoPDU(NegativeType invalidType, CompressionType highestCompressionTypeSupported, bool isReconnect = false, bool autoLogon = true)
        {
            Client_Info_Pdu pdu = rdpbcgrClientStack.CreateClientInfoPdu(
                RdpbcgrTestData.DefaultClientInfoPduFlags, testConfig.domain, testConfig.userName, testConfig.password, testConfig.localAddress, null, null, isReconnect);
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

            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                pdu.commonHeader.tpktHeader.length -= 1;
            }
            else if (invalidType == NegativeType.InvalidFlagsInSecurityHeader)
            {
                // Not set SEC_INFO_PKT flag in flags of securityHeader
                pdu.commonHeader.securityHeader.flags ^= TS_SECURITY_HEADER_flags_Values.SEC_INFO_PKT;
            }
            else if (invalidType == NegativeType.InvalidMACSignature)
            {
                // Set an invalid MAC signature
                pdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER1)
                {
                    ((TS_SECURITY_HEADER1)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
                {
                    ((TS_SECURITY_HEADER2)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
            }
            else if (invalidType == NegativeType.InvalidMCSLength)
            {
                // Create a wrapper, which override ToBytes method to set invalid MCS length
                pdu = new Client_Info_Pdu_Ex(pdu, rdpbcgrClientStack.Context);
            }

            SendPdu(pdu);
        }

        public void ProcessLicenseSequence(TimeSpan timeout)
        {
            TS_LICENSE_PDU licensePdu = rdpeleClient.ExpectPdu(timeout);

            if (licensePdu.preamble.bMsgType == bMsgType_Values.ERROR_ALERT)
            {
                // If the target machine is a personal terminal server, whether the client sends the license or not, 
                // the server always sends a license error message with the error code STATUS_VALID_CLIENT and the state transition code ST_NO_TRANSITION. 
                Site.Assert.AreEqual(dwErrorCode_Values.STATUS_VALID_CLIENT, licensePdu.LicensingMessage.LicenseError.Value.dwErrorCode,
                    $"A license error message with the error code STATUS_VALID_CLIENT should be received, the real error code is {licensePdu.LicensingMessage.LicenseError.Value.dwErrorCode}.");
                return;
            }

            Site.Log.Add(LogEntryKind.Debug, "Start RDP license procedure");
            Site.Assert.AreEqual(bMsgType_Values.LICENSE_REQUEST, licensePdu.preamble.bMsgType, $"A LICENSE_REQUEST message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");

            rdpeleClient.SendClientNewLicenseRequest(
                KeyExchangeAlg.KEY_EXCHANGE_ALG_RSA, (uint)Client_OS_ID.CLIENT_OS_ID_WINNT_POST_52 | (uint)Client_Image_ID.CLIENT_IMAGE_ID_MICROSOFT, testConfig.userName, testConfig.localAddress);
            licensePdu = rdpeleClient.ExpectPdu(timeout);
            Site.Assert.AreEqual(bMsgType_Values.PLATFORM_CHALLENGE, licensePdu.preamble.bMsgType, $"A PLATFORM_CHALLENGE message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");
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
            Site.Assert.AreEqual(bMsgType_Values.NEW_LICENSE, licensePdu.preamble.bMsgType, $"A NEW_LICENSE message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");

            Site.Log.Add(LogEntryKind.Debug, "End RDP license procedure");
        }

        /// <summary>
        /// Send a Client Confirm Active PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="supportAutoReconnect">Set the support of auto-reconnect</param>
        /// <param name="supportFastPathInput">Set the support of fast-path input</param>
        /// <param name="supportFastPathOutput">Set the support of fast-path output</param>
        /// <param name="supportSurfaceCommands">Set the support of surface commands</param>
        /// <param name="supportSVCCompression">Set the support of static virtual channel data compression</param>
        /// <param name="supportRemoteFXCodec">Set the support of RemoteFX codecs</param>
        public void SendClientConfirmActivePDU(NegativeType invalidType,
            bool supportAutoReconnect,
            bool supportFastPathInput,
            bool supportFastPathOutput,
            bool supportSurfaceCommands,
            bool supportSVCCompression,
            bool supportRemoteFXCodec)
        {
            // Create capability sets
            RdpbcgrCapSet capSet = new RdpbcgrCapSet();
            Collection<ITsCapsSet> caps = capSet.CreateCapabilitySets(supportAutoReconnect,
                supportFastPathInput,
                supportFastPathOutput,
                supportSurfaceCommands,
                supportSVCCompression,
                supportRemoteFXCodec);

            Client_Confirm_Active_Pdu pdu = rdpbcgrClientStack.CreateConfirmActivePdu(caps);

            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                pdu.commonHeader.tpktHeader.length -= 1;
            }
            else if (invalidType == NegativeType.InvalidMACSignature)
            {
                pdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER1)
                {
                    ((TS_SECURITY_HEADER1)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
                {
                    ((TS_SECURITY_HEADER2)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
            }
            else if (invalidType == NegativeType.InvalidLengthInShareHeader)
            {
                // Set invalid length in the shareheader
                pdu.confirmActivePduData.shareControlHeader.totalLength -= 1;
            }
            else if (invalidType == NegativeType.InvalidMCSLength)
            {
                // Create a wrapper, which override ToBytes method to set invalid MCS length
                pdu = new Client_Confirm_Active_Pdu_Ex(pdu, rdpbcgrClientStack.Context);
            }

            SendPdu(pdu);
        }

        /// <summary>
        /// Send a Client Synchronize PDU
        /// </summary>
        public void SendClientSynchronizePDU()
        {
            Client_Synchronize_Pdu syncPdu = rdpbcgrClientStack.CreateSynchronizePdu();
            SendPdu(syncPdu);
        }

        /// <summary>
        /// Send a Client Control PDU (Cooperate)
        /// </summary>
        public void SendClientControlCooperatePDU()
        {
            Client_Control_Pdu_Cooperate controlCooperatePdu = rdpbcgrClientStack.CreateControlCooperatePdu();
            SendPdu(controlCooperatePdu);
        }

        /// <summary>
        /// Send a Client Control PDU (Request Control)
        /// </summary>
        public void SendClientControlRequestPDU()
        {
            Client_Control_Pdu_Request_Control requestControlPdu = rdpbcgrClientStack.CreateControlRequestPdu();
            SendPdu(requestControlPdu);
        }

        /// <summary>
        /// Send a Client Persistent Key List PDU
        /// </summary>
        public void SendClientPersistentKeyListPDU()
        {
            Client_Persistent_Key_List_Pdu persistentKeyListPdu = rdpbcgrClientStack.CreatePersistentKeyListPdu();
            SendPdu(persistentKeyListPdu);
        }

        /// <summary>
        /// Send a Client Font List PDU
        /// </summary>
        public void SendClientFontListPDU()
        {
            Client_Font_List_Pdu fontListPdu = rdpbcgrClientStack.CreateFontListPdu();
            SendPdu(fontListPdu);
        }

        /// <summary>
        /// Send a Client Shutdown Request PDU
        /// </summary>
        public void SendClientShutdownRequestPDU()
        {
            Client_Shutdown_Request_Pdu request = rdpbcgrClientStack.CreateShutdownRequestPdu();
            SendPdu(request);
        }

        #endregion Connection Methods

        #region Input/Output Methods

        /// <summary>
        /// Send a Client Input Event PDU 
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="inputEvents">Array of input events to be sent</param>
        public void SendClientInputEventPDU(NegativeType invalidType, TS_INPUT_EVENT[] inputEvents)
        {
            TS_INPUT_PDU pdu = rdpbcgrClientStack.CreateSlowPathInputEventPDU(inputEvents);
            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                pdu.commonHeader.tpktHeader.length -= 1;
            }
            else if (invalidType == NegativeType.InvalidMACSignature)
            {
                pdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER1)
                {
                    ((TS_SECURITY_HEADER1)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
                {
                    ((TS_SECURITY_HEADER2)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
            }
            else if (invalidType == NegativeType.InvalidLengthInShareHeader)
            {
                pdu.shareDataHeader.shareControlHeader.totalLength -= 1;
            }
            else if (invalidType == NegativeType.InvalidMCSLength)
            {
                pdu = new TS_INPUT_PDU_Ex(pdu, rdpbcgrClientStack.Context);
            }
            else if (invalidType == NegativeType.InvalidEventType)
            {
                // Set an invalid type in messageType field of the first input event
                TS_INPUT_EVENT inputEvent = pdu.slowPathInputEvents[0];
                inputEvent.messageType = (TS_INPUT_EVENT_messageType_Values)0xFF;
                pdu.slowPathInputEvents[0] = inputEvent;
            }

            SendPdu(pdu);
        }

        #region TS_INPUT_EVENT generation

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_SYNC_EVENT
        /// </summary>
        /// <param name="scrollLock">Wether the Scroll Lock indicator light SHOULD be on</param>
        /// <param name="numLock">Wether the Num Lock indicator light SHOULD be on</param>
        /// <param name="capsLock">Wether the Caps Lock indicator light SHOULD be on</param>
        /// <param name="kanaLock">Wether the Kana Lock indicator light SHOULD be on</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_SYNC_EVENT</returns>
        public TS_INPUT_EVENT GenerateSynchronizeEvent(bool scrollLock, bool numLock, bool capsLock, bool kanaLock)
        {
            TS_INPUT_EVENT inputEvent = new TS_INPUT_EVENT();
            TS_SYNC_EVENT synchronizeEvent = new TS_SYNC_EVENT();
            synchronizeEvent.pad2Octets = 0;
            synchronizeEvent.toggleFlags = toggleFlags_Values.None;
            if (scrollLock)
            {
                synchronizeEvent.toggleFlags |= toggleFlags_Values.TS_SYNC_SCROLL_LOCK;
            }
            if (numLock)
            {
                synchronizeEvent.toggleFlags |= toggleFlags_Values.TS_SYNC_NUM_LOCK;
            }
            if (capsLock)
            {
                synchronizeEvent.toggleFlags |= toggleFlags_Values.TS_SYNC_CAPS_LOCK;
            }
            if (kanaLock)
            {
                synchronizeEvent.toggleFlags |= toggleFlags_Values.TS_SYNC_KANA_LOCK;
            }
            inputEvent.eventTime = 0;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SYNC;
            inputEvent.slowPathInputData = synchronizeEvent;
            return inputEvent;
        }

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_KEYBOARD_EVENT
        /// </summary>
        /// <param name="keyboardFlags">The flags describing this keyboard event</param>
        /// <param name="keyCode">The scancode of the key</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_KEYBOARD_EVENT</returns>
        public TS_INPUT_EVENT GenerateKeyboardEvent(keyboardFlags_Values keyboardFlags, ushort keyCode)
        {
            TS_INPUT_EVENT inputEvent = new TS_INPUT_EVENT();
            TS_KEYBOARD_EVENT keyboardEvent = new TS_KEYBOARD_EVENT();
            keyboardEvent.keyboardFlags = keyboardFlags;
            keyboardEvent.keyCode = keyCode;
            keyboardEvent.pad2Octets = 0;
            inputEvent.eventTime = 0;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SCANCODE;
            inputEvent.slowPathInputData = keyboardEvent;
            return inputEvent;
        }

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_UNICODE_KEYBOARD_EVENT
        /// </summary>
        /// <param name="keyboardFlags">The flags describing the Unicode keyboard event, must be KBDFLAGS_RELEASE (0x8000)</param>
        /// <param name="unicodeCode">The Unicode character input code</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_UNICODE_KEYBOARD_EVENT</returns>
        public TS_INPUT_EVENT GenerateUnicodeKeyboardEvent(keyboardFlags_Values keyboardFlags, ushort unicodeCode)
        {
            TS_INPUT_EVENT inputEvent = new TS_INPUT_EVENT();
            TS_UNICODE_KEYBOARD_EVENT unicodeEvent = new TS_UNICODE_KEYBOARD_EVENT();
            unicodeEvent.keyboardFlags = keyboardFlags;
            unicodeEvent.unicodeCode = unicodeCode;
            unicodeEvent.pad2Octets = 0;
            inputEvent.eventTime = 0;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNICODE;
            inputEvent.slowPathInputData = unicodeEvent;
            return inputEvent;
        }

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_POINTER_EVENT
        /// </summary>
        /// <param name="pointerFlags">The flags describing the pointer event.</param>
        /// <param name="xPos">The x-coordinate of the pointer relative to the top-left corner of the server's desktop</param>
        /// <param name="yPos">The y-coordinate of the pointer relative to the top-left corner of the server's desktop</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_POINTER_EVENT</returns>
        public TS_INPUT_EVENT GenerateMouseEvent(pointerFlags_Values pointerFlags, ushort xPos, ushort yPos)
        {
            TS_INPUT_EVENT inputEvent = new TS_INPUT_EVENT();
            TS_POINTER_EVENT mouseEvent = new TS_POINTER_EVENT();
            mouseEvent.pointerFlags = pointerFlags;
            mouseEvent.xPos = xPos;
            mouseEvent.yPos = yPos;
            inputEvent.eventTime = 0;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSE;
            inputEvent.slowPathInputData = mouseEvent;
            return inputEvent;
        }

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_POINTERX_EVENT
        /// </summary>
        /// <param name="pointerFlags">The flags describing the extended mouse event</param>
        /// <param name="xPos">The x-coordinate of the pointer</param>
        /// <param name="yPos">The Y-coordinate of the pointer</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_POINTERX_EVENT</returns>
        public TS_INPUT_EVENT GenerateExtendedMouseEvent(TS_POINTERX_EVENT_pointerFlags_Values pointerFlags, ushort xPos, ushort yPos)
        {
            TS_INPUT_EVENT inputEvent = new TS_INPUT_EVENT();
            TS_POINTERX_EVENT exmouseEvent = new TS_POINTERX_EVENT();
            exmouseEvent.pointerFlags = pointerFlags;
            exmouseEvent.xPos = xPos;
            exmouseEvent.yPos = yPos;
            inputEvent.eventTime = 0;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSEX;
            inputEvent.slowPathInputData = exmouseEvent;
            return inputEvent;
        }

        #endregion TS_INPUT_EVENT generation

        /// <summary>
        /// Send a Client Fast-Path Input Event PDU 
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="inputEvents">Array of input events to be sent</param>
        public void SendClientFastPathInputEventPDU(NegativeType invalidType, TS_FP_INPUT_EVENT[] inputEvents)
        {
            TS_FP_INPUT_PDU pdu = rdpbcgrClientStack.CreateFastPathInputEventPDU(inputEvents);

            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidMACSignature)
            {
                pdu.dataSignature = new byte[8];
            }
            else if (invalidType == NegativeType.FastPathInput_InvalidSecFlags)
            {
                pdu.fpInputHeader.flags |= encryptionFlags_Values.FASTPATH_INPUT_ENCRYPTED;
            }
            else if (invalidType == NegativeType.InvalidEventType)
            {
                TS_FP_INPUT_EVENT fpInputEvent = pdu.fpInputEvents[0];
                fpInputEvent.eventHeader.eventFlagsAndCode = (byte)(((int)0x07 << 5));
                pdu.fpInputEvents[0] = fpInputEvent;
            }

            SendPdu(pdu);
        }

        #region TS_FP_INPUT_EVENT generation

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_KEYBOARD_EVENT
        /// </summary>
        /// <param name="eventFlag">The flags describing the keyboard event</param>
        /// <param name="keyCode">The scancode of the key</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_KEYBOARD_EVENT</returns>
        public TS_FP_INPUT_EVENT GenerateFPKeyboardEvent(TS_FP_KEYBOARD_EVENT_Eventflags eventFlag, byte keyCode)
        {
            TS_FP_INPUT_EVENT fpInputEvent = new TS_FP_INPUT_EVENT();
            TS_FP_KEYBOARD_EVENT keyboardEvent = new TS_FP_KEYBOARD_EVENT();
            keyboardEvent.keyCode = keyCode;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)(((int)eventCode_Values.FASTPATH_INPUT_EVENT_SCANCODE << 5)
                | ((int)eventFlag & 0x1F));
            fpInputEvent.eventData = keyboardEvent;
            return fpInputEvent;
        }

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_POINTER_EVENT
        /// </summary>
        /// <param name="pointerFlags">The flags describing the pointer event</param>
        /// <param name="xPos">The x-coordinate of the pointer</param>
        /// <param name="yPos">The y-coordinate of the pointer</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_POINTER_EVENT</returns>
        public TS_FP_INPUT_EVENT GenerateFPMouseEvent(pointerFlags_Values pointerFlags, ushort xPos, ushort yPos)
        {
            TS_FP_INPUT_EVENT fpInputEvent = new TS_FP_INPUT_EVENT();
            TS_FP_POINTER_EVENT mouseEvent = new TS_FP_POINTER_EVENT();
            mouseEvent.pointerFlags = (ushort)pointerFlags;
            mouseEvent.xPos = xPos;
            mouseEvent.yPos = yPos;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)((int)eventCode_Values.FASTPATH_INPUT_EVENT_MOUSE << 5);
            fpInputEvent.eventData = mouseEvent;
            return fpInputEvent;
        }

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_POINTERX_EVENT
        /// </summary>
        /// <param name="pointerFlags">The flags describing the pointer event</param>
        /// <param name="xPos">The x-coordinate of the pointer</param>
        /// <param name="yPos">The y-coordinate of the pointer</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_POINTERX_EVENT</returns>
        public TS_FP_INPUT_EVENT GenerateFPExtendedMouseEvent(TS_POINTERX_EVENT_pointerFlags_Values pointerFlags, ushort xPos, ushort yPos)
        {
            TS_FP_INPUT_EVENT fpInputEvent = new TS_FP_INPUT_EVENT();
            TS_FP_POINTERX_EVENT extendedMouseEvent = new TS_FP_POINTERX_EVENT();
            extendedMouseEvent.pointerFlags = (ushort)pointerFlags;
            extendedMouseEvent.xPos = (ushort)xPos;
            extendedMouseEvent.yPos = (ushort)yPos;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)((int)eventCode_Values.FASTPATH_INPUT_EVENT_MOUSEX << 5);
            fpInputEvent.eventData = extendedMouseEvent;
            return fpInputEvent;
        }

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_SYNC_EVENT
        /// </summary>
        /// <param name="scrollLock">Wether the Scroll Lock indicator light SHOULD be on</param>
        /// <param name="numLock">Wether the Num Lock indicator light SHOULD be on</param>
        /// <param name="capsLock">Wether the Caps Lock indicator light SHOULD be on</param>
        /// <param name="kanaLock">Wether the Kana Lock indicator light SHOULD be on</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_SYNC_EVENT</returns>
        public TS_FP_INPUT_EVENT GenerateFPSynchronizeEvent(bool scrollLock, bool numLock, bool capsLock, bool kanaLock)
        {
            TS_FP_INPUT_EVENT fpInputEvent = new TS_FP_INPUT_EVENT();
            TS_FP_SYNC_EVENT synchronizeEvent = new TS_FP_SYNC_EVENT();
            TS_FP_SYNC_EVENT_Eventflags eventFlags = TS_FP_SYNC_EVENT_Eventflags.None;
            if (scrollLock)
            {
                eventFlags |= TS_FP_SYNC_EVENT_Eventflags.FASTPATH_INPUT_SYNC_SCROLL_LOCK;
            }
            if (numLock)
            {
                eventFlags |= TS_FP_SYNC_EVENT_Eventflags.FASTPATH_INPUT_SYNC_NUM_LOCK;
            }
            if (capsLock)
            {
                eventFlags |= TS_FP_SYNC_EVENT_Eventflags.FASTPATH_INPUT_SYNC_CAPS_LOCK;
            }
            if (kanaLock)
            {
                eventFlags |= TS_FP_SYNC_EVENT_Eventflags.FASTPATH_INPUT_SYNC_KANA_LOCK;
            }
            fpInputEvent.eventHeader.eventFlagsAndCode = (byte)(((int)eventCode_Values.FASTPATH_INPUT_EVENT_SYNC << 5)
                | ((int)eventFlags & 0x1F));
            fpInputEvent.eventData = synchronizeEvent;
            return fpInputEvent;
        }

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_UNICODE_KEYBOARD_EVENT
        /// </summary>
        /// <param name="eventFlag">The flags describing the keyboard event, must be FASTPATH_INPUT_KBDFLAGS_RELEASE (0x01)</param>
        /// <param name="unicodeCode">The Unicode character input code</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_UNICODE_KEYBOARD_EVENT</returns>
        public TS_FP_INPUT_EVENT GenerateFPUnicodeKeyboardEvent(TS_FP_KEYBOARD_EVENT_Eventflags eventFlag, ushort unicodeCode)
        {
            TS_FP_INPUT_EVENT fpInputEvent = new TS_FP_INPUT_EVENT();
            TS_FP_UNICODE_KEYBOARD_EVENT unicodeEvent = new TS_FP_UNICODE_KEYBOARD_EVENT();
            unicodeEvent.unicodeCode = unicodeCode;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)((int)eventCode_Values.FASTPATH_INPUT_EVENT_UNICODE << 5
                | ((int)eventFlag & 0x1F));
            fpInputEvent.eventData = unicodeEvent;
            return fpInputEvent;
        }

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_QOETIMESTAMP_EVENT
        /// </summary>
        /// <param name="timestamp">The timestamp indicates when the current input batch was encoded by the client</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_QOETIMESTAMP_EVENT</returns>
        public TS_FP_INPUT_EVENT GenerateQoETimestampEvent(uint timestamp)
        {
            TS_FP_INPUT_EVENT fpInputEvent = new TS_FP_INPUT_EVENT();
            TS_FP_QOETIMESTAMP_EVENT qoeTimestampEvent = new TS_FP_QOETIMESTAMP_EVENT();
            qoeTimestampEvent.timestamp = timestamp;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)((int)eventCode_Values.FASTPATH_INPUT_EVENT_QOE_TIMESTAMP << 5);
            fpInputEvent.eventData = qoeTimestampEvent;
            return fpInputEvent;
        }

        #endregion TS_FP_INPUT_EVENT generation

        /// <summary>
        /// Send a Client Refresh Rect PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="rects">Array of screen area Inclusive Rectangles to redraw</param>
        public void SendClientRefreshRectPDU(NegativeType invalidType, TS_RECTANGLE16[] rects)
        {
            Client_Refresh_Rect_Pdu pdu = rdpbcgrClientStack.CreateRefreshRectPdu(rects);

            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                pdu.commonHeader.tpktHeader.length -= 1;
            }
            else if (invalidType == NegativeType.InvalidMACSignature)
            {
                pdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER1)
                {
                    ((TS_SECURITY_HEADER1)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
                {
                    ((TS_SECURITY_HEADER2)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
            }
            else if (invalidType == NegativeType.InvalidLengthInShareHeader)
            {
                pdu.refreshRectPduData.shareDataHeader.shareControlHeader.totalLength -= 1;
            }

            SendPdu(pdu);

        }

        /// <summary>
        /// Send a Client Suppress Output PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="allowDisplayUpdates">Indicates whether the client wants to receive display updates from the server</param>
        /// <param name="rect">The coordinates of the desktop rectangle</param>
        public void SendClientSuppressOutputPDU(NegativeType invalidType, AllowDisplayUpdates_SUPPRESS_OUTPUT allowDisplayUpdates, TS_RECTANGLE16 rect)
        {
            Client_Suppress_Output_Pdu pdu = rdpbcgrClientStack.CreateSuppressOutputPdu(allowDisplayUpdates, rect);

            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                pdu.commonHeader.tpktHeader.length -= 1;
            }
            else if (invalidType == NegativeType.InvalidMACSignature)
            {
                pdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER1)
                {
                    ((TS_SECURITY_HEADER1)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
                if (pdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
                {
                    ((TS_SECURITY_HEADER2)pdu.commonHeader.securityHeader).dataSignature = new byte[8];
                }
            }
            else if (invalidType == NegativeType.InvalidLengthInShareHeader)
            {
                pdu.suppressOutputPduData.shareDataHeader.shareControlHeader.totalLength -= 1;
            }

            SendPdu(pdu);
        }

        #endregion Input/Output Methods

        #region Virtual Channel Methods

        /// <summary>
        /// Send data through specified static virtual channel
        /// </summary>
        /// <param name="channelId">ID of static virtual channel</param>
        /// <param name="virtualChannelData">Data to sent</param>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        public void SendVirtualChannelPDU(UInt16 channelId, byte[] virtualChannelData, NegativeType invalidType)
        {
            // Create virtual channel PDU(s), if length of virtualChannelData is too large, split it to multiple virtual channel PDUs
            Collection<Virtual_Channel_RAW_Pdu> virtualChannelPduList = rdpbcgrClientStack.CreateCompleteVirtualChannelPdu(
                            channelId,
                            virtualChannelData
                            );

            if (invalidType == NegativeType.None)
            {
                // If invalidType is none, send virtual channel PDU(s) one by one
                foreach (Virtual_Channel_RAW_Pdu rawPdu in virtualChannelPduList)
                {
                    SendPdu(rawPdu);
                }
            }
            else if (invalidType == NegativeType.InvalidTPKTLength)
            {
                // Set the length field of TPKT header to invalid value (less than actual value).
                Virtual_Channel_RAW_Pdu rawPdu = virtualChannelPduList[0];
                rawPdu.commonHeader.tpktHeader.length = (ushort)(rawPdu.commonHeader.tpktHeader.length - 1);
                SendPdu(rawPdu);
            }
            else if (invalidType == NegativeType.InvalidMACSignature)
            {
                // Not present the signature when the SEC_ENCRYPT flag in securityHeader when Enhanced RDP Security is in effect.
                Virtual_Channel_RAW_Pdu rawPdu = virtualChannelPduList[0];
                rawPdu.commonHeader.securityHeader = new TS_SECURITY_HEADER();
                rawPdu.commonHeader.securityHeader.flagsHi = 0;
                rawPdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                SendPdu(rawPdu);
            }
            else if (invalidType == NegativeType.InvalidMCSLength)
            {
                // Create a wrapper, which override the ToBytes method to set an invalid MCS length
                Virtual_Channel_RAW_Pdu rawPdu = new Virtual_Channel_RAW_Pdu_Ex(virtualChannelPduList[0], rdpbcgrClientStack.Context);
                SendPdu(rawPdu);
            }
        }

        #endregion Virtual Channel Methods

        #region Auto-detect Methods

        /// <summary>
        /// Send a Client Auto-Detect Response PDU with a RDP_RTT_RESPONSE
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="sequenceNumber">The message sequence number</param>
        public void SendClientAutoDetectResponsePDUWithRTTResponse(NegativeType invalidType, ushort sequenceNumber)
        {
            RDP_RTT_RESPONSE rttResponse = RdpbcgrUtility.GenerateRTTMeasureResponse(sequenceNumber);
            Client_Auto_Detect_Response_PDU pdu = rdpbcgrClientStack.CreateAutoDetectResponsePdu(rttResponse);
            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                pdu.commonHeader.tpktHeader.length -= 1;
            }
            SendPdu(pdu);
        }

        /// <summary>
        /// Send a Client Auto-Detect Response PDU with a RDP_BW_RESULTS
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="sequenceNumber">The message sequence number</param>
        /// <param name="timeDelta">The time delta, in milliseconds, between the receipt of the Bandwidth Measure Start and the Bandwidth Measure Stop messages</param>
        /// <param name="byteCount">The total data received in the Bandwidth Measure Payload messages</param>
        /// <param name="isDuringConnection">Whether in a connect-time auto-detection phase</param>
        public void SendClientAutoDetectResponsePDUWithBWResults(NegativeType invalidType, ushort sequenceNumber, uint timeDelta, uint byteCount, bool isDuringConnection)
        {
            AUTO_DETECT_RESPONSE_TYPE respType = AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_DURING_CONNECT;
            if (!isDuringConnection)
            {
                respType = AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT;
            }
            RDP_BW_RESULTS bwResponse = RdpbcgrUtility.GenerateBandwidthMeasureResults(respType, sequenceNumber, timeDelta, byteCount);

            Client_Auto_Detect_Response_PDU pdu = rdpbcgrClientStack.CreateAutoDetectResponsePdu(bwResponse);
            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                pdu.commonHeader.tpktHeader.length -= 1;
            }

            SendPdu(pdu);
        }

        /// <summary>
        /// Send a Client Auto-Detect Response PDU with a RDP_NETCHAR_SYNC
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="sequenceNumber">The message sequence number</param>
        /// <param name="bandwidth">The previously detected bandwidth in kilobits per second</param>
        /// <param name="rtt">The previously detected round-trip time in milliseconds</param>
        public void SendClientAutoDetectResponsePDUWithNetworkSync(NegativeType invalidType, ushort sequenceNumber, uint bandwidth, uint rtt)
        {
            RDP_NETCHAR_SYNC netSync = RdpbcgrUtility.GenerateNetworkCharacteristicsSync(sequenceNumber, bandwidth, rtt);
            Client_Auto_Detect_Response_PDU pdu = rdpbcgrClientStack.CreateAutoDetectResponsePdu(netSync);
            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                pdu.commonHeader.tpktHeader.length -= 1;
            }
            SendPdu(pdu);
        }

        #endregion Auto-detect Methods

        #region Multitransport Bootstrapping

        /// <summary>
        /// Send a Client Initiate Multitransport Error PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="requestId">The ID which is the same as he requestId field of the associated Initiate Multitransport Request PDU </param>
        public void SendClientInitiateMultitransportErrorPDU(NegativeType invalidType, uint requestId)
        {
            Client_Initiate_Multitransport_Response_PDU pdu = rdpbcgrClientStack.CreateInitiateMultitransportResponsePdu(requestId);
            // Make invalid Packet if invalidType is not None
            if (invalidType == NegativeType.InvalidTPKTLength)
            {
                pdu.commonHeader.tpktHeader.length -= 1;
            }

            SendPdu(pdu);
        }

        #endregion Multitransport Bootstrapping

        #region Expect Methods

        /// <summary>
        /// Expect to receive a packet whose type is T
        /// </summary>
        /// <typeparam name="T">Type of packet, which must be inherited from StackPacket</typeparam>
        /// <param name="timeout">Timeout</param>
        /// <returns>Received packet if succeed, otherwise, return null</returns>
        public T ExpectPacket<T>(TimeSpan timeout) where T : StackPacket
        {
            this.Site.Log.Add(LogEntryKind.Debug, "Expect RDP server to send a {0}.", typeof(T).Name);
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
                            // Print out the error message if there is an exception when expecting the pdu.
                            this.Site.Assert.Fail("An Exception happened when expecting the packet: {0}", ((ErrorPdu)packet).ErrorMessage);
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
            this.Site.Log.Add(LogEntryKind.Debug, "Timeout when expecting a {0}.", typeof(T).Name);
            return null;
        }

        /// <summary>
        /// Expect the RDP server drop the connection
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>True if RDP server dropped the connection, otherwise, return false</returns>
        public bool ExpectDisconnetion(TimeSpan timeout)
        {
            try
            {
                rdpbcgrClientStack.ExpectDisconnect(timeout);

                return true;
            }
            catch (TimeoutException)
            {
                Site.Log.Add(LogEntryKind.Comment, "Timed out waiting for disconnection from SUT.");

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Wait until user logon or timeout
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <param name="expectCookie">Also expect a Server Save Session Info PDU with auto-reconnect cookie</param>
        public void WaitForLogon(TimeSpan timeout, bool expectCookie = false)
        {
            bool receivedLogon = false;
            bool receivedCookie = false;
            DateTime endtime = DateTime.Now + timeout;
            while (timeout.TotalMilliseconds > 0)
            {
                Server_Save_Session_Info_Pdu saveSessionInfoPdu = ExpectPacket<Server_Save_Session_Info_Pdu>(timeout);
                if (saveSessionInfoPdu != null &&
                    (saveSessionInfoPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON || saveSessionInfoPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON_LONG || saveSessionInfoPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON_PLAINNOTIFY))
                {
                    receivedLogon = true;
                }
                if (saveSessionInfoPdu != null && saveSessionInfoPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON_EXTENDED_INF)
                {
                    receivedCookie = true;
                }
                if (receivedLogon && (!expectCookie || receivedCookie))
                {
                    Site.Log.Add(LogEntryKind.Comment, "Received Server_Save_Session_Info_Pdu from RDP server to notify user logon.");
                    return;
                }
                timeout = endtime - DateTime.Now;
            }
            Site.Assert.Fail("Timeout when waiting server send Server_Save_Session_Info_Pdu to notify user logon. ReceivedLogon is {0}, Received Cookie is {1}.", receivedLogon, receivedCookie);
        }
        #endregion Expect Methods

        #region Verify Capability

        /// <summary>
        /// Whether RDP server support Auto Reconnect
        /// Check extraFlags flag of TS_GENERAL_CAPABILITYSET
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        public bool IsServerSupportAutoReconnect()
        {
            ITsCapsSet capset = this.GetServerCapSet(capabilitySetType_Values.CAPSTYPE_GENERAL);
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

        /// <summary>
        /// Whether RDP Server support fastpath input
        /// Check inputFlags flag of TS_INPUT_CAPABILITYSET
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        public bool IsServerSupportFastpathInput()
        {
            ITsCapsSet capset = this.GetServerCapSet(capabilitySetType_Values.CAPSTYPE_INPUT);
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

        public bool IsServerSupportFastpathInputQoeTimestampEvent()
        {
            ITsCapsSet capset = this.GetServerCapSet(capabilitySetType_Values.CAPSTYPE_INPUT);
            if (capset != null)
            {
                TS_INPUT_CAPABILITYSET inputCap = (TS_INPUT_CAPABILITYSET)capset;
                if (inputCap.inputFlags.HasFlag(inputFlags_Values.TS_INPUT_FLAG_QOE_TIMESTAMPS))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whether server support RDP-UDP FEC reliable transport
        /// Check flags field of TS_UD_SC_MULTITRANSPORT
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        public bool IsServerSupportUDPFECR()
        {
            return serverSupportUDPFECR;
        }

        /// <summary>
        /// Whether server support RDP-UDP FEC lossy transport
        /// Check flags field of TS_UD_SC_MULTITRANSPORT
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        public bool IsServerSupportUDPFECL()
        {
            return serverSupportUDPFECL;
        }

        /// <summary>
        /// Whether server support tunneling of static virtual channel traffic over UDP
        /// Check flags field of TS_UD_SC_MULTITRANSPORT
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        public bool IsSupportUDPPreffered()
        {
            return serverSupportUDPPrefferred;
        }

        #endregion Verify Capability

        #endregion Methods in IRdpbcgrAdapter

        #region Private methods

        /// <summary>
        /// This method is used to send packet to SUT.
        /// </summary>
        /// <param name="packet">The packet to be sent.</param>
        private void SendPdu(StackPacket packet)
        {
            this.Site.Log.Add(LogEntryKind.Debug, "Send a {0} to SUT.", packet.GetType().Name);
            rdpbcgrClientStack.SendPdu(packet);
            System.Threading.Thread.Sleep(sendInterval);//To avoid the combination with other sending request.
        }

        /// <summary>
        /// Get a capability set.
        /// </summary>
        /// <param name="capsetType">Type of Capability Set</param>
        /// <returns></returns>
        private ITsCapsSet GetServerCapSet(capabilitySetType_Values capsetType)
        {
            Collection<ITsCapsSet> capsets = this.rdpbcgrClientStack.Context.demandActivemCapabilitySets;
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

        #endregion Private methods
    }
}
