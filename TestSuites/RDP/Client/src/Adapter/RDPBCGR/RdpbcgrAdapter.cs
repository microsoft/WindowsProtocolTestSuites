// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrAdapter : ManagedAdapterBase, IRdpbcgrAdapter
    {
        #region Variables

        private ITestSite site;
        private RdpbcgrServer rdpbcgrServerStack;
        private RdpbcgrServerSessionContext sessionContext;
        private RdpbcgrServerConfig serverConfig;
        private ServerSessionState sessionState = ServerSessionState.TransportDisconnected;
        private ConnectionFinalization_ServerState connectionFinalizationState = ConnectionFinalization_ServerState.NotStarted;
        private TimeSpan pduWaitTimeSpan = new TimeSpan(0, 0, 40);
        private long lastRequestJoinChannelId;
        //runtime data:
        private TS_INFO_PACKET tsInfoPacket;
        private RdpbcgrCapSet clientCapSet;

        private uint autoDetectedBandwidth;
        private uint autoDetectedBaseRTT;
        private uint autoDetectedAverageRTT;
        //private RDPBCGRCapSet m_serverCapSet = null; 
        private string certFile;
        private string certPwd;
        private int port = ConstValue.TEST_PORT;
        private IpVersion ipVersion = IpVersion.Ipv4;
        private string rdpVersionString = "7.0";
        private List<StackPacket> pduCache; //used to cache unproceed Pdus.
        private int RDPDR_Index = -1; //Index of "RDPDR" within MCS Connection Initial request.
        private UInt16 RDPDR_ChannelId; //channel Id of "RDPDR" static virtual channel.
        private IPAddress serverIPAddress;
        private bool isWindowsImplementation = true;
        private Dictionary<string, ushort> svcNameIdDic;
        private CHANNEL_DEF[] clientRequestedChannelDefList;
        private bool bVerifyRequirements = true;
        private bool isClientToServerEncrypted = true;
        private bool verifySUTDisplay = false;

        private RdpeudpServer rdpeudpServer;
        private RdpemtServer rdpemtServerReliable;
        private RdpemtServer rdpemtServerLossy;
        private uint multitransportRequestId = 0;

        private SimulatedScreen simulatedScreen;
        private IQA_Algorithm IQAAlgorithm;
        private double IQAAssessValueThreshold;
        #endregion

        #region Events

        public event X224ConnectioRequestHandler X224ConnectionRequest;
        public event McsConnectRequestHandler McsConnectRequest;
        public event McsErectDomainRequestHandler McsErectDomainRequest;
        public event McsAttachUserRequestHandler McsAttachUserRequest;
        public event McsChannelJoinRequestHandler McsChannelJoinRequest;
        public event SecurityExchangeRequestHandler SecurityExchangeRequest;
        public event ClientInfoRequestHandler ClientInfoRequest;
        public event ConfirmActiveRequestHandler ConfirmActiveRequest;
        public event ClientSyncRequestHandler ClientSyncRequest;
        public event ControlCooperateRequestHandler ControlCooperateRequest;
        public event ControlRequestControlRequestHandler ControlRequestControlRequest;
        public event PersistentKeyListRequestHandler PersistentKeyListRequest;
        public event FontListRequestHandler FontListRequest;
        public event ShutdownRequestHandler ShutdownRequest;
        public event McsDisconnectUltimatumRequestHandler McsDisconnectUltimatumClientRequest;
        public event InputRequestHandler InputRequest;
        public event FastpathInputRequestHandler FastpathInputRequest;
        public event RefreshRectRequestHandler RefreshRectRequest;
        public event SuppressOutputRequestHandler SuppressOutputRequest;
        public event VirtualChannelRequestHandler VirtualChannelRequest;
        public event TS_FRAME_ACKNOWLEDGE_PDUHandler TS_FRAME_ACKNOWLEDGE_PDUReceived;
        public event RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsHandler RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsReceived;
        public event RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieHandler RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieReceived;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public RdpbcgrAdapter()
        {
        }
        #endregion

        #region Overriden functions

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            this.site = testSite;
            pduCache = new List<StackPacket>();
            svcNameIdDic = new Dictionary<string, ushort>();
            LoadServerConfiguation();

            if (verifySUTDisplay)
            {
                simulatedScreen = new SimulatedScreen(this.site, this.IQAAlgorithm, this.IQAAssessValueThreshold);
            }
            else
            {
                simulatedScreen = null;
            }
        }

        public override void Reset()
        {
            base.Reset();
            pduCache.Clear();
            svcNameIdDic.Clear();

            if (simulatedScreen != null)
            {
                simulatedScreen.Reset();
            }

            #region Unregister Events
            X224ConnectionRequest = null;
            McsConnectRequest = null;
            McsErectDomainRequest = null;
            McsAttachUserRequest = null;
            McsChannelJoinRequest = null;
            SecurityExchangeRequest = null;
            ClientInfoRequest = null;
            ConfirmActiveRequest = null;
            ClientSyncRequest = null;
            ControlCooperateRequest = null;
            ControlRequestControlRequest = null;
            PersistentKeyListRequest = null;
            FontListRequest = null;
            ShutdownRequest = null;
            McsDisconnectUltimatumClientRequest = null;
            InputRequest = null;
            FastpathInputRequest = null;
            RefreshRectRequest = null;
            SuppressOutputRequest = null;
            VirtualChannelRequest = null;
            #endregion
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.rdpbcgrServerStack != null)
            {
                this.rdpbcgrServerStack.Dispose();
            }

        }

        #endregion

        #region Runtime information

        public TS_INFO_PACKET ClientInfo
        {
            get { return tsInfoPacket; }
        }

        #endregion "Runtime information"

        #region Properties
        public RdpbcgrServerSessionContext SessionContext
        {
            get
            {
                return sessionContext;
            }
        }
        public RdpbcgrServer ServerStack
        {
            get
            {
                return rdpbcgrServerStack;
            }
        }
        public ServerCapabilitySetting CapabilitySetting
        {
            get
            {
                return serverConfig.CapabilitySetting;
            }
        }
        public SimulatedScreen SimulatedScreen
        {
            get
            {
                return simulatedScreen;
            }
        }

        public UInt16 RDPDRChannelId
        {
            get
            {
                return RDPDR_ChannelId;
            }
        }
        #endregion

        #region Server Methods

        #region Connection Methods

        /// <summary>
        /// Start RDP listening.
        /// </summary>
        public void StartRDPListening(EncryptedProtocol protocol)
        {
            serverConfig.encryptedProtocol = protocol;
            X509Certificate2 cert = null;
            if (protocol != EncryptedProtocol.Rdp)
            {
                cert = new X509Certificate2(certFile, certPwd);
            }
            rdpbcgrServerStack = new RdpbcgrServer(port, serverConfig.encryptedProtocol, cert, isClientToServerEncrypted);
            if (ipVersion == IpVersion.Ipv6)
            {
                serverIPAddress = IPAddress.IPv6Any;
            }
            else
            {
                serverIPAddress = IPAddress.Any;
            }
            rdpbcgrServerStack.Start(serverIPAddress);
            site.Log.Add(LogEntryKind.Debug, "The IP version is {0}.", ipVersion);
            site.Log.Add(LogEntryKind.Debug, "RDP server is listening on {0}:{1}.", serverIPAddress.ToString(), port);
        }

        /// <summary>
        /// Stop RDP listening.
        /// </summary>
        public void StopRDPListening()
        {
            if (rdpemtServerReliable != null)
                rdpemtServerReliable.Dispose();

            if (rdpemtServerLossy != null)
                rdpemtServerLossy.Dispose();

            if (rdpeudpServer != null && rdpeudpServer.Running)
                rdpeudpServer.Stop();

            if (rdpbcgrServerStack != null)
            {
                rdpbcgrServerStack.Dispose();
                sessionContext = null;
            }
        }

        /// <summary>
        /// Expect a transport level (TCP) connection.
        /// </summary>
        /// <param name="sessionType">The type of session to be established.</param>
        public void ExpectTransportConnection(RDPSessionType sessionType)
        {
            this.pduCache.Clear();
            RdpbcgrServerSessionContext oldSession = sessionContext;

            TimeSpan leftTime = pduWaitTimeSpan;
            DateTime expiratedTime = DateTime.Now + pduWaitTimeSpan;
            bool isRecieved = false;
            while (!isRecieved && leftTime.CompareTo(new TimeSpan(0)) > 0)
            {
                try
                {
                    sessionContext = rdpbcgrServerStack.ExpectConnect(leftTime);
                    if (oldSession == null)
                    {
                        isRecieved = true;
                        break;
                    }
                    else if (oldSession.Identity.ToString() != sessionContext.Identity.ToString())
                    {
                        isRecieved = true;
                        break;
                    }
                }
                catch (TimeoutException)
                {
                    site.Assert.Fail("Timeout when expecting Connection");
                }
                catch (InvalidOperationException ex)
                {
                    //break;
                    site.Log.Add(LogEntryKind.Warning, "Excetpion thrown out when expect Connection. {0}", ex.Message);
                }
                finally
                {
                    System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                    leftTime = expiratedTime - DateTime.Now;
                }

            }
            if (!isRecieved)
            {
                site.Assert.Fail("Timeout when expecting Connection");
            }

            site.Log.Add(LogEntryKind.Debug, "A RDP client initiates a connection. The local endpoint is {0}. The remote endpoint is {1}", sessionContext.LocalIdentity, sessionContext.Identity.ToString());

            sessionState = ServerSessionState.TransportConnected;

            if (this.serverConfig.encryptedProtocol == EncryptedProtocol.Rdp && isWindowsImplementation && sessionType != RDPSessionType.AutoReconnection)
            {
                site.Log.Add(LogEntryKind.Debug, @"Section 3.2.5.3.2, disconnect and then restart the connection sequence, specifying only the PROTOCOL_RDP flag (0x00000000) 
                in the requestedProtocols field of the RDP Negotiation Request structure (section 2.2.1.1.1).");

                ExpectPacket<Client_X_224_Connection_Request_Pdu>(sessionContext, pduWaitTimeSpan);

                Server_X_224_Connection_Confirm(selectedProtocols_Values.PROTOCOL_RDP_FLAG, serverConfig.isExtendedClientDataSupported, true, NegativeType.None);

                RdpbcgrServerSessionContext orgSession = sessionContext;
                sessionContext = rdpbcgrServerStack.ExpectConnect(pduWaitTimeSpan);
                if (sessionContext.Identity == orgSession.Identity)
                {
                    sessionContext = rdpbcgrServerStack.ExpectConnect(pduWaitTimeSpan);
                }

                sessionState = ServerSessionState.TransportConnected;
            }
        }


        /// <summary>
        /// 2.2.1.1 - 2.2.1.2
        /// 
        /// Send X.224 Connection Confirm PDU. It is sent as a response of X.224 Connection Request.
        /// </summary>
        /// <param name="protocol">Indicates the selected security protocol</param>
        /// <param name="bSupportExtClientData">Indicates Extended Client Data is supported </param>
        /// <param name="setRdpNegData">Indicates rdpNegData is set</param>
        /// <param name="bSupportEGFX">Indicates the server supports MS-RDPEGFX</param>
        /// <param name="bSupportRestrictedAdminMode">Indicates the server supports Restricted admin mode</param>
        /// <param name="bReservedSet">Indicates the value of NEGRSP_FLAG_RESERVED in the flags field of RDP Negotiation Response</param>
        /// <param name="bSupportRestrictedAuthenticationMode">Indicates the server supports Restricted Authentication mode</param>
        public void Server_X_224_Connection_Confirm(selectedProtocols_Values protocol, bool bSupportExtClientData, bool setRdpNegData, NegativeType invalidType, bool bSupportEGFX = false, bool bSupportRestrictedAdminMode = false, bool bReservedSet = false, bool bSupportRestrictedAuthenticationMode = false)
        {
            //ExpectPacket<Client_X_224_Connection_Request_Pdu>(sessionContext, timeout);
            serverConfig.selectedProtocol = protocol;
            serverConfig.isExtendedClientDataSupported = bSupportExtClientData;

            Server_X_224_Connection_Confirm_Pdu confirmPdu
                = rdpbcgrServerStack.CreateX224ConnectionConfirmPdu(sessionContext, protocol);
            if (bSupportExtClientData)
            {
                confirmPdu.rdpNegData.flags |= RDP_NEG_RSP_flags_Values.EXTENDED_CLIENT_DATA_SUPPORTED;
            }
            if (bSupportEGFX)
            {
                confirmPdu.rdpNegData.flags |= RDP_NEG_RSP_flags_Values.DYNVC_GFX_PROTOCOL_SUPPORTED;
            }
            if (bSupportRestrictedAdminMode)
            {
                confirmPdu.rdpNegData.flags |= RDP_NEG_RSP_flags_Values.RESTRICTED_ADMIN_MODE_SUPPORTED;
            }
            if (bReservedSet)
            {
                confirmPdu.rdpNegData.flags |= RDP_NEG_RSP_flags_Values.NEGRSP_FLAG_RESERVED;
            }
            if (bSupportRestrictedAuthenticationMode)
            {
                confirmPdu.rdpNegData.flags |= RDP_NEG_RSP_flags_Values.REDIRECTED_AUTHENTICATION_MODE_SUPPORTED;
            }

            switch (invalidType)
            {
                case NegativeType.InvalidTPKTHeader:
                    confirmPdu.tpktHeader.version = 0;
                    break;
                case NegativeType.InvalidX224:
                    confirmPdu.x224Ccf.lengthIndicator--;
                    break;
                case NegativeType.InvalidRdpNegData:
                    confirmPdu.rdpNegData.type = RDP_NEG_RSP_type_Values.None;
                    break;
            }
            if (!setRdpNegData)
            {
                confirmPdu.rdpNegData = null;
            }
            SendPdu(confirmPdu);
            sessionState = ServerSessionState.X224ConnectionResponseSent;
        }

        /// <summary>
        /// Send X.224 Connection Confirm PDU. It is sent as a response of X.224 Connection Request.
        /// </summary>
        /// <param name="protocol">Indicates the selected security protocol</param>
        /// <param name="flags">Specify flags field of RDP_NEG_RSP</param>
        public void Server_X_224_Connection_Confirm(selectedProtocols_Values protocol, RDP_NEG_RSP_flags_Values flags)
        {
            serverConfig.selectedProtocol = protocol;
            serverConfig.isExtendedClientDataSupported = flags.HasFlag(RDP_NEG_RSP_flags_Values.EXTENDED_CLIENT_DATA_SUPPORTED);

            Server_X_224_Connection_Confirm_Pdu confirmPdu
               = rdpbcgrServerStack.CreateX224ConnectionConfirmPdu(sessionContext, protocol, flags);

            SendPdu(confirmPdu);
            sessionState = ServerSessionState.X224ConnectionResponseSent;
        }
        /// <summary>
        /// 2.2.1.1 - 2.2.1.2
        /// 
        /// Send X.224 Connection Confirm PDU.It is sent as a response of X.224 Connection Request. X.224 Connection Request.
        /// </summary>
        /// <param name="bSupportExtClientData">Indicates Extended Client Data is supported </param>
        public void Server_X_224_Negotiate_Failure(failureCode_Values failReason)
        {
            Server_X_224_Negotiate_Failure_Pdu confirmPdu
                 = rdpbcgrServerStack.CreateX224NegotiateFailurePdu(sessionContext, failReason);

            SendPdu(confirmPdu);
            sessionState = ServerSessionState.X224ConnectionResponseSent;
        }

        /// <summary>
        /// The MCS Connect Response PDU is an RDP Connection Sequence PDU sent from server to client 
        /// during the Basic Settings Exchange phase of the RDP Connection Sequence 
        /// (see section 1.3.1.1 for an overview of the RDP Connection Sequence phases).
        /// It is sent as a response to the MCS Connect Initial PDU (section 2.2.1.3). 
        /// </summary>
        /// <param name="enMothod">Server selected encryption method.</param>
        /// <param name="enLevel">Server selected encryption level.</param>
        /// <param name="rdpVersion">The RDP Server version</param>
        /// <param name="invalidType">Indicates the invalid type</param>
        /// <param name="multiTransportTypeFlags">Flags of Multitransport Channel Data</param>
        /// <param name="hasEarlyCapabilityFlags">Indicates the existing of the earlyCapabilityFlags</param>
        /// <param name="earlyCapabilityFlagsValue">The value of earlyCapabilityFlags</param>
        /// <param name="mcsChannelId_Net">MCSChannelId value for Server Network Data</param>
        /// <param name="mcsChannelId_MSGChannel">MCSChannelId value for Server Message Channel Data</param>
        public void Server_MCS_Connect_Response(
            EncryptionMethods enMothod,
            EncryptionLevel enLevel,
            TS_UD_SC_CORE_version_Values rdpVersion,
            NegativeType invalidType,
            MULTITRANSPORT_TYPE_FLAGS multiTransportTypeFlags = MULTITRANSPORT_TYPE_FLAGS.None,
            bool hasEarlyCapabilityFlags = false,
            SC_earlyCapabilityFlags_Values earlyCapabilityFlagsValue = SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED,
            UInt16 mcsChannelId_Net = ConstValue.IO_CHANNEL_ID,
            UInt16 mcsChannelId_MSGChannel = ConstValue.MCS_MESSAGE_CHANNEL_ID)
        {
            SERVER_CERTIFICATE cert = null;
            int certLen = 0;

            if (serverConfig.selectedProtocol == selectedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                serverConfig.certChainVersion = SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1;
                byte[] privateExp, publicExp, modulus;
                cert = rdpbcgrServerStack.GenerateCertificate(serverConfig.dwKeysize, out privateExp, out publicExp, out modulus);
                serverConfig.serverPrivateExponent = privateExp;
                certLen = 120 + serverConfig.dwKeysize / 8;
            }

            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response connectRespPdu = null;
            connectRespPdu = rdpbcgrServerStack.CreateMCSConnectResponsePduWithGCCConferenceCreateResponsePdu(
                    sessionContext,
                    enMothod,
                    enLevel,
                    cert,
                    certLen,
                    multiTransportTypeFlags,
                    hasEarlyCapabilityFlags,
                    earlyCapabilityFlagsValue,
                    mcsChannelId_Net,
                    ConstValue.MCS_MESSAGE_CHANNEL_ID);
            connectRespPdu.mcsCrsp.gccPdu.serverCoreData.version = rdpVersion;
            connectRespPdu = new Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response_Ex(connectRespPdu, sessionContext, invalidType);


            switch (invalidType)
            {
                case NegativeType.None:
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidX224:
                    connectRespPdu.x224Data.length = 1; //ConstValue.X224_DATA_TYPE_LENGTH (0x02) - 1;
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidResult:
                    connectRespPdu.mcsCrsp.result = 1; //not rt-successful (0)
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidH221:
                    connectRespPdu.mcsCrsp.gccPdu.H221Key = "DnMc"; //ConstValue.H221_KEY "McDn";
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidEncodedLength:
                    //Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response_Ex invalidPdu = new Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response_Ex(connectRespPdu, sessionContext, invalidType, asnIssueFixed);
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidEncodedLengthExternalSecurityProtocols:
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidClientRequestedProtocols:
                    connectRespPdu.mcsCrsp.gccPdu.serverCoreData.clientRequestedProtocols = (requestedProtocols_Values)0x04;//Invalid value different from any of requestedProtocols_Values enums
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidEncryptionMethod:
                    connectRespPdu.mcsCrsp.gccPdu.serverSecurityData.encryptionMethod = (EncryptionMethods)0x2000;//Invalid EncriptionMethods
                    connectRespPdu.mcsCrsp.gccPdu.serverMessageChannelData = null;
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidEncryptionLevel:
                    connectRespPdu.mcsCrsp.gccPdu.serverSecurityData.encryptionLevel = (EncryptionLevel)0x00004000; //Invalid EncriptionLevel
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidServerRandomLen:
                    byte[] serverRandom = new byte[] { 0x10, 0x11 };
                    connectRespPdu.mcsCrsp.gccPdu.serverSecurityData.serverRandomLen = new UInt32Class((uint)serverRandom.Length);//non 32
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidServerCertificate:
                    connectRespPdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate = null; //invalid cert
                    connectRespPdu.mcsCrsp.gccPdu.serverMessageChannelData = null;
                    SendPdu(connectRespPdu);
                    break;
                case NegativeType.InvalidChannelCount:
                    if (sessionContext.VirtualChannelDefines != null)
                    {
                        ushort channelCount = connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelCount;
                        connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelCount--;
                        connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelIdArray = new ushort[channelCount - 1];

                        for (int i = 0; i < connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelCount; ++i)
                        {
                            connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelIdArray[i] = sessionContext.VirtualChannelIdFactory.Dequeue();
                        }
                        if (!RdpbcgrUtility.IsEven(connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelCount))
                        {
                            connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.Pad = new byte[2];
                        }
                        else
                        {
                            connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.Pad = null;
                        }

                        int networkDataSize = Marshal.SizeOf(connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.header)
                                            + Marshal.SizeOf(connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.MCSChannelId)
                                            + Marshal.SizeOf(connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelCount)
                                            + (int)(2/*ConstValue.CHANNEL_ID_SIZE*/ * (ushort)connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelCount);
                        if (connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.Pad != null)
                        {
                            networkDataSize += (int)connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.Pad.Length;
                        }
                        connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.header.length = (ushort)networkDataSize;
                    }
                    SendPdu(connectRespPdu);
                    break;
            }

            //Update server config
            if (serverConfig.serverPrivateExponent != null)
            {
                sessionContext.ServerPrivateExponent = new byte[serverConfig.serverPrivateExponent.Length];
                Array.Copy(serverConfig.serverPrivateExponent, sessionContext.ServerPrivateExponent, serverConfig.serverPrivateExponent.Length);
            }
            serverConfig.encryptionMethod = enMothod;
            serverConfig.encryptionLevel = enLevel;
            serverConfig.rdpServerVersion = rdpVersion;

            //Store the "RDPDR" channel id.
            if (RDPDR_Index > -1)
            {
                RDPDR_ChannelId = connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelIdArray[RDPDR_Index];
            }

            //Store the mapping of static virtual channel and the channel id.
            if (clientRequestedChannelDefList != null)
            {
                svcNameIdDic.Clear();
                int maxIdx = Math.Min(clientRequestedChannelDefList.Length, connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelIdArray.Length);
                for (int i = 0; i < maxIdx; i++)
                {
                    int len = clientRequestedChannelDefList[i].name.IndexOf("\0");
                    if (len <= 0)
                        len = clientRequestedChannelDefList[i].name.Length - 1;
                    svcNameIdDic.Add(clientRequestedChannelDefList[i].name.ToUpper().Substring(0, len), connectRespPdu.mcsCrsp.gccPdu.serverNetworkData.channelIdArray[i]);
                }

            }

            sessionState = ServerSessionState.MCSConnectResponseSent;
        }

        /// <summary>
        /// Send MCS Attach User Confirm Pdu to client.
        /// </summary>
        public void MCSAttachUserConfirm(NegativeType invalidType)
        {
            Server_MCS_Attach_User_Confirm_Pdu confirmPdu = null;

            //ExpectPacket<Client_MCS_Erect_Domain_Request>(sessionContext, timeout);
            //ExpectPacket<Client_MCS_Attach_User_Request>(sessionContext, timeout);

            confirmPdu = rdpbcgrServerStack.CreateMCSAttachUserConfirmPdu(sessionContext);
            switch (invalidType)
            {
                case NegativeType.InvalidTPKLength:
                    confirmPdu.tpktHeader.length = 8; //valid value should be 11
                    break;
                case NegativeType.InvalidX224:
                    confirmPdu.x224Data.length = 1; //valid value should be ConstValue.X224_DATA_TYPE_LENGTH (0x02)
                    break;
                case NegativeType.InvalidResult:
                    confirmPdu.attachUserConfirm.result = new Result(Result.rt_no_such_channel); //not rt-successful (0)
                    break;
                case NegativeType.InvalidInitiatorField:
                    confirmPdu.attachUserConfirm.initiator = null; //does not present
                    break;
            }

            SendPdu(confirmPdu);
            sessionState = ServerSessionState.MCSAttachUserConfirmSent;

        }

        /// <summary>
        /// Send MCS Channel Join Confirm Pdu to client.
        /// </summary>
        /// <param name="channelId">The channel id that client requested to join.</param>
        public void MCSChannelJoinConfirm(long channelId, NegativeType invalidType)
        {
            //ExpectPacket<Client_MCS_Channel_Join_Request>(sessionContext, timeout);

            Server_MCS_Channel_Join_Confirm_Pdu channelJoinResponse = rdpbcgrServerStack.CreateMCSChannelJoinConfirmPdu(
                        sessionContext,
                        channelId);
            switch (invalidType)
            {
                case NegativeType.InvalidTPKLength:
                    channelJoinResponse.tpktHeader.length = 10;//valid value should be 15
                    break;
                case NegativeType.InvalidX224:
                    channelJoinResponse.x224Data.length = 1; //valid value is ConstValue.X224_DATA_TYPE_LENGTH (0x02)
                    break;
                case NegativeType.InvalidEmptyChannelIdField:
                    channelJoinResponse.channelJoinConfirm.channelId = null; //channelId field is not presented
                    break;
                case NegativeType.InvalidResult:
                    channelJoinResponse.channelJoinConfirm.result = new Result(Result.rt_no_such_channel); //not rt-successful (0)
                    break;
                case NegativeType.InvalidMismatchChannelIdField:
                    channelJoinResponse.channelJoinConfirm.channelId = new ChannelId(channelId - 1); //mismatch channelId
                    break;
            }
            SendPdu(channelJoinResponse);

            sessionState = ServerSessionState.MCSChannelJoinConfirmSent;
        }

        /// <summary>
        /// Send the Licensing PDU to client.
        /// </summary>
        public void Server_License_Error_Pdu_Valid_Client(NegativeType invalidType)
        {
            //ExpectPacket<Client_Security_Exchange_Pdu>(sessionContext, timeout);
            //ExpectPacket<Client_Info_Pdu>(sessionContext, timeout);

            Server_License_Error_Pdu_Valid_Client licensePdu = rdpbcgrServerStack.CreateLicenseErrorMessage(sessionContext);

            switch (invalidType)
            {
                case NegativeType.None:
                    SendPdu(licensePdu);
                    break;
                case NegativeType.InvalidTPKLength:
                    licensePdu.commonHeader.tpktHeader.length -= 1;
                    SendPdu(licensePdu);
                    break;
                case NegativeType.InvalidX224:
                    licensePdu.commonHeader.x224Data.length -= 1;
                    SendPdu(licensePdu);
                    break;
                case NegativeType.InvalidMCSLength:
                    //licensePdu.commonHeader.userDataLength -= 1;
                    Server_License_Error_Pdu_Valid_Client_Ex newLicensePdu = new Server_License_Error_Pdu_Valid_Client_Ex(licensePdu, sessionContext);
                    SendPdu(newLicensePdu);
                    break;
                case NegativeType.InvalidFlagInSecurityHeader:
                    licensePdu.commonHeader.securityHeader.flags ^= TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_PKT;
                    SendPdu(licensePdu);
                    break;
                case NegativeType.InvalidSignatureInSecurityHeader:
                    licensePdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                    if (licensePdu.commonHeader.securityHeader is TS_SECURITY_HEADER1)
                    {
                        ((TS_SECURITY_HEADER1)licensePdu.commonHeader.securityHeader).dataSignature = null;
                    }
                    if (licensePdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
                    {
                        ((TS_SECURITY_HEADER2)licensePdu.commonHeader.securityHeader).dataSignature = null;
                    }
                    SendPdu(licensePdu);
                    break;
                case NegativeType.InvalidMessage:
                    licensePdu.validClientMessage.dwErrorCode = dwErrorCode_Values.ERR_INVALID_MAC;
                    SendPdu(licensePdu);
                    break;
            }


            sessionState = ServerSessionState.ServerLicenseErroPduSent;
        }

        /// <summary>
        /// Set the server capability settings.
        /// </summary>
        /// <param name="supportServerBitmapCacheHost">Indicates server support Bitmap Host Cache Support Capability Set or not.</param>
        /// <param name="supportFastPathInput">Indicates server support client Fast Path Input or not.</param>
        /// <param name="supportScancode">Indicates support for using scancodes in the Keyboard Event notifications (see sections 2.2.8.1.1.3.1.1.1 and 2.2.8.1.2.2.1).</param>
        /// <param name="supportExtendedMouse">Indicates support for Extended Mouse Event notifications (see sections 2.2.8.1.1.3.1.1.4 and 2.2.8.1.2.2.4).</param>
        /// <param name="supportUnicodeKeyboard">Indicates support for Unicode Keyboard Event notifications (see sections 2.2.8.1.1.3.1.1.2 and 2.2.8.1.2.2.2).</param>
        /// <param name="supportAutoReconnect">Indicates support for Auto-Reconnect.</param>
        /// <param name="supportRefreshRect">Server-only flag that indicates whether the Refresh Rect PDU (section 2.2.11.2) is supported</param>
        /// <param name="supportSuppressOoutput">Server-only flag that indicates whether the Suppress Output PDU (section 2.2.11.3) is supported.</param>
        /// <param name="supportVCComression">Indicates server support virtual channel compression or not.</param>
        /// <param name="presentVCChunksize">Indicates present the VCChunkSize field within Virtual Channel Capability Set or not.</param>
        public void SetServerCapability(
            bool supportServerBitmapCacheHost, //Bitmap Cache Host Support Capability Set
            bool supportFastPathInput, bool supportScancode, bool supportExtendedMouse, bool supportUnicodeKeyboard, //Input Capability Set
            bool supportAutoReconnect, bool supportRefreshRect, bool supportSuppressOoutput, //General Capbility Set
            bool supportVCComression, bool presentVCChunksize //Virtual Channel Capability Set
            )
        {
            this.site.Log.Add(LogEntryKind.Comment, @"SetServerCapability(
                supportServerBitmapCacheHost = {0};
                supportFastPathInput = {1};
                supportScancode = {2};
                supportExtendedMouse = {3};
                supportUnicodeKeyboard = {4};
                supportAutoReconnect = {5};
                supportRefreshRect = {6};
                supportSuppressOoutput = {7};
                supportVCComression = {8};
                presentVCChunksize = {9}).",
             supportServerBitmapCacheHost,
             supportFastPathInput, supportScancode, supportExtendedMouse, supportUnicodeKeyboard,
             supportAutoReconnect, supportRefreshRect, supportSuppressOoutput,
             supportVCComression, presentVCChunksize
            );
            this.serverConfig.CapabilitySetting.BitmapCacheHostSupportCapabilitySet = supportServerBitmapCacheHost;
            //INPUT_FLAG_FASTPATH_INPUT is advertised by RDP 5.0 and 5.1 servers. 
            //RDP 5.2, 6.0, 6.1, and 7.0 servers advertise the INPUT_FLAG_FASTPATH_INPUT2 flag to indicate support for fast-path input.
            if (rdpVersionString.Equals("5.0") || rdpVersionString.Equals("5.1"))
            {
                this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_FPInput = supportFastPathInput;
                this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_FPInput2 = false;
            }
            else
            {
                this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_FPInput2 = supportFastPathInput;
                this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_FPInput = false;
            }
            this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_ScanCodes = supportScancode;
            this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_MouseX = supportExtendedMouse;
            this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_Unicode = supportUnicodeKeyboard;
            this.serverConfig.CapabilitySetting.GeneralCapSet_ExtraFlags_AutoReconnect = supportAutoReconnect;
            this.serverConfig.CapabilitySetting.GeneralCapSet_RefreshRectSupport = supportRefreshRect;
            this.serverConfig.CapabilitySetting.GeneralCapSet_SuppressOutputSupport = supportSuppressOoutput;
            this.serverConfig.CapabilitySetting.VCCapSet_CS_CompressionSupport = supportVCComression;
            this.serverConfig.CapabilitySetting.VCCapSet_SC_VCChunkSizePresent = presentVCChunksize;
        }

        /// <summary>
        /// Set the server capability settings.
        /// </summary>
        /// <param name="supportServerBitmapCacheHost">Indicates server support Bitmap Host Cache Support Capability Set or not.</param>
        /// <param name="supportFastPathInput">Indicates server support client Fast Path Input or not.</param>
        /// <param name="supportScancode">Indicates support for using scancodes in the Keyboard Event notifications (see sections 2.2.8.1.1.3.1.1.1 and 2.2.8.1.2.2.1).</param>
        /// <param name="supportExtendedMouse">Indicates support for Extended Mouse Event notifications (see sections 2.2.8.1.1.3.1.1.4 and 2.2.8.1.2.2.4).</param>
        /// <param name="supportUnicodeKeyboard">Indicates support for Unicode Keyboard Event notifications (see sections 2.2.8.1.1.3.1.1.2 and 2.2.8.1.2.2.2).</param>
        /// <param name="supportAutoReconnect">Indicates support for Auto-Reconnect.</param>
        /// <param name="supportRefreshRect">Server-only flag that indicates whether the Refresh Rect PDU (section 2.2.11.2) is supported</param>
        /// <param name="supportSuppressOoutput">Server-only flag that indicates whether the Suppress Output PDU (section 2.2.11.3) is supported.</param>
        /// <param name="supportVCComression">Indicates server support virtual channel compression or not.</param>
        /// <param name="presentVCChunksize">Indicates present the VCChunkSize field within Virtual Channel Capability Set or not.</param>
        /// <param name="supportMultifragmentUpdateCapabilitySet">Indicates support Multifragment Update Capability Set or not.</param>
        /// <param name="maxRequestSize">The size of the buffer used to reassemble the fragments of a Fast-Path Update (see section 2.2.9.1.2.1). </param>
        /// <param name="supportLargePointerCapabilitySet">Indicates support Large Pointer Capability Set or not.</param>
        /// <param name="supportFrameAcknowledgeCapabilitySet">Indicates support Frame Acknowledge Capability Set or not.</param>
        /// <param name="maxUnacknowledgedFrameCount">Indicates the number of in-flight TS_FRAME_ACKNOWLEDGE_PDUs that the server is prepared to accept. </param>
        /// <param name="supportSurfaceCommandsCapabilitySet">Indicates support Surface Commands Capability Set or not.</param>
        /// <param name="cmdFlag">Flags indicating which Surface Commands are supported.</param>
        /// <param name="supportBitmapCodecsCapabilitySet">Indicates support Bitmap Codecs Capability Set or not.</param>
        /// <param name="presentNSCodec">Indicates if NSCodec Bitmap Codec is supported by RDP server.</param>
        /// <param name="presentRemoteFXCodec">Indicates if RemoteFX Bitmap Codec is supported by RDP server.</param>
        public void SetServerCapability(
            bool supportServerBitmapCacheHost, //Bitmap Cache Host Support Capability Set
            bool supportFastPathInput, bool supportScancode, bool supportExtendedMouse, bool supportUnicodeKeyboard, //Input Capability Set
            bool supportAutoReconnect, bool supportRefreshRect, bool supportSuppressOoutput, //General Capbility Set
            bool supportVCComression, bool presentVCChunksize, //Virtual Channel Capability Set
            bool supportMultifragmentUpdateCapabilitySet, uint maxRequestSize, //Multifragment Update Capability Set
            bool supportLargePointerCapabilitySet, //Large Pointer Capability Set
            bool supportFrameAcknowledgeCapabilitySet, uint maxUnacknowledgedFrameCount,//TS_FRAME_ACKNOWLEDGE_CAPABILITYSET
            bool supportSurfaceCommandsCapabilitySet, CmdFlags_Values cmdFlag,  //Surface Commands Capability Set 
            bool supportBitmapCodecsCapabilitySet, bool presentNSCodec, bool presentRemoteFXCodec //Bitmap Codecs Capability Set
            )
        {
            this.serverConfig.CapabilitySetting.BitmapCacheHostSupportCapabilitySet = supportServerBitmapCacheHost;
            //INPUT_FLAG_FASTPATH_INPUT is advertised by RDP 5.0 and 5.1 servers. 
            //RDP 5.2, 6.0, 6.1, and 7.0 servers advertise the INPUT_FLAG_FASTPATH_INPUT2 flag to indicate support for fast-path input.
            if (rdpVersionString.Equals("5.0") || rdpVersionString.Equals("5.1"))
            {
                this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_FPInput = supportFastPathInput;
                this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_FPInput2 = false;
            }
            else
            {
                this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_FPInput2 = supportFastPathInput;
                this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_FPInput = false;
            }
            this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_ScanCodes = supportScancode;
            this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_MouseX = supportExtendedMouse;
            this.serverConfig.CapabilitySetting.InputCapSet_InputFlags_Unicode = supportUnicodeKeyboard;
            this.serverConfig.CapabilitySetting.GeneralCapSet_ExtraFlags_AutoReconnect = supportAutoReconnect;
            this.serverConfig.CapabilitySetting.GeneralCapSet_RefreshRectSupport = supportRefreshRect;
            this.serverConfig.CapabilitySetting.GeneralCapSet_SuppressOutputSupport = supportSuppressOoutput;
            this.serverConfig.CapabilitySetting.VCCapSet_CS_CompressionSupport = supportVCComression;
            this.serverConfig.CapabilitySetting.VCCapSet_SC_VCChunkSizePresent = presentVCChunksize;

            this.serverConfig.CapabilitySetting.MultifragmentUpdateCapabilitySet = supportMultifragmentUpdateCapabilitySet;
            this.serverConfig.CapabilitySetting.MultifragmentUpdateCapabilitySet_maxRequestSize = maxRequestSize;
            this.serverConfig.CapabilitySetting.LargePointerCapabilitySet = supportLargePointerCapabilitySet;
            this.serverConfig.CapabilitySetting.FrameAcknowledgeCapabilitySet = supportFrameAcknowledgeCapabilitySet;
            this.serverConfig.CapabilitySetting.FrameAcknowledgeCapabilitySet_maxUnacknowledgedFrameCount = maxUnacknowledgedFrameCount;
            this.serverConfig.CapabilitySetting.SurfaceCommandsCapabilitySet = supportSurfaceCommandsCapabilitySet;
            this.serverConfig.CapabilitySetting.SurfaceCommandsCapabilitySet_cmdFlag = cmdFlag;
            this.serverConfig.CapabilitySetting.BitmapCodecsCapabilitySet = supportBitmapCodecsCapabilitySet;
            this.serverConfig.CapabilitySetting.BitmapCodecsCapabilitySet_NSCodec = presentNSCodec;
            this.serverConfig.CapabilitySetting.BitmapCodecsCapabilitySet_RemoteFx = presentRemoteFXCodec;

        }

        /// <summary>
        /// Generate Caps Sets according to capability setting from SetServerCapability
        /// </summary>
        /// <returns></returns>
        public Collection<ITsCapsSet> GenerateServerCapSet()
        {
            RdpbcgrCapSet capSet = new RdpbcgrCapSet();
            capSet.SetFromConfig(serverConfig.CapabilitySetting);
            return capSet.CapabilitySets;
        }

        /// <summary>
        /// Send Server Demand Active Pdu to client.
        /// </summary>
        public void Server_Demand_Active(NegativeType invalidType)
        {
            Server_Demand_Active_Pdu demandActivePdu = null;

            Collection<ITsCapsSet> capSetCollection = GenerateServerCapSet();

            demandActivePdu = rdpbcgrServerStack.CreateDemandActivePdu(sessionContext, capSetCollection);
            switch (invalidType)
            {
                case NegativeType.None:
                    SendPdu(demandActivePdu);
                    break;
                case NegativeType.InvalidTPKLength:
                    demandActivePdu.commonHeader.tpktHeader.length--;
                    SendPdu(demandActivePdu);
                    break;
                case NegativeType.InvalidX224:
                    demandActivePdu.commonHeader.x224Data.length--;
                    SendPdu(demandActivePdu);
                    break;
                case NegativeType.InvalidMCSLength:
                    //demandActivePdu.commonHeader.userDataLength --;
                    Server_Demand_Active_Pdu_Ex newDemmandPdu = new Server_Demand_Active_Pdu_Ex(demandActivePdu, sessionContext);
                    SendPdu(newDemmandPdu);
                    break;
                case NegativeType.InvalidSignatureInSecurityHeader:
                    demandActivePdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                    if (demandActivePdu.commonHeader.securityHeader is TS_SECURITY_HEADER1)
                    {
                        ((TS_SECURITY_HEADER1)demandActivePdu.commonHeader.securityHeader).dataSignature = new byte[8];
                    }
                    if (demandActivePdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
                    {
                        ((TS_SECURITY_HEADER2)demandActivePdu.commonHeader.securityHeader).dataSignature = new byte[8];
                    }
                    SendPdu(demandActivePdu);
                    break;
                case NegativeType.InvalidPduType:
                    demandActivePdu.demandActivePduData.shareControlHeader.pduType.typeAndVersionLow &= 0xf0; //set least significant 4 bits to none
                    SendPdu(demandActivePdu);
                    break;
                case NegativeType.InvalidPduLength:
                    demandActivePdu.demandActivePduData.shareControlHeader.totalLength--;
                    SendPdu(demandActivePdu);
                    break;
            }

            sessionState = ServerSessionState.ServerDemandActiveSent;
        }


        /// <summary>
        /// Send Server Demand Active Pdu to client
        /// </summary>
        /// <param name="capSetCollection">Specify capability sets</param>
        public void Server_Demand_Active(Collection<ITsCapsSet> capSetCollection)
        {
            Server_Demand_Active_Pdu demandActivePdu = rdpbcgrServerStack.CreateDemandActivePdu(sessionContext, capSetCollection);
            SendPdu(demandActivePdu);
        }

        /// <summary>
        /// Send Server Synchronize Pdu to client.
        /// </summary>
        public void ServerSynchronize()
        {
            Server_Synchronize_Pdu synchronizePdu;
            synchronizePdu = rdpbcgrServerStack.CreateSynchronizePdu(sessionContext);
            SendPdu(synchronizePdu);
            connectionFinalizationState = ConnectionFinalization_ServerState.ServerSynchonizeSent;
        }

        /// <summary>
        /// Send Server Control Cooperate Pdu to client.
        /// </summary>
        public void ServerControlCooperate()
        {
            Server_Control_Pdu controlCooperatePdu;
            controlCooperatePdu = rdpbcgrServerStack.CreateControlCooperatePdu(sessionContext);
            SendPdu(controlCooperatePdu);
            connectionFinalizationState = ConnectionFinalization_ServerState.ServerControlCooperateSent;
        }

        /// <summary>
        /// Send Server Control Granted Control Pdu to client.
        /// </summary>
        public void ServerControlGrantedControl()
        {
            Server_Control_Pdu controlGrantedPdu;
            controlGrantedPdu = rdpbcgrServerStack.CreateControlGrantedPdu(sessionContext);
            SendPdu(controlGrantedPdu);
            connectionFinalizationState = ConnectionFinalization_ServerState.ServerControlGrantedControlSent;
        }

        /// <summary>
        /// Send Server Font Map Pdu to client.
        /// </summary>
        public void ServerFontMap()
        {
            Server_Font_Map_Pdu fontMapPdu;
            fontMapPdu = rdpbcgrServerStack.CreateFontMapPdu(sessionContext);
            SendPdu(fontMapPdu);
            connectionFinalizationState = ConnectionFinalization_ServerState.ServerFontMapSent;
            sessionState = ServerSessionState.RDPConnected;
        }

        #endregion

        #region Disconnect Methods

        /// <summary>
        /// Send Server Deactivate All Pdu to client.
        /// </summary>
        public void ServerDeactivateAll()
        {
            Server_Deactivate_All_Pdu deactivatePdu = null;
            deactivatePdu = rdpbcgrServerStack.CreateDeactivateAllPdu(sessionContext);
            SendPdu(deactivatePdu);
        }

        /// <summary>
        /// Send a MCS Disconnect Provider Ultimatum to client.
        /// </summary>
        /// <param name="adminInitiated">Indicates if it's an admin-Initiated disconnection.</param>
        public void Server_MCSDisconnectProviderUltimatum(bool adminInitiated, NegativeType invalidType)
        {
            int reason = (int)Reason.rn_user_requested;
            if (!adminInitiated) reason = (int)Reason.rn_provider_initiated;
            MCS_Disconnect_Provider_Ultimatum_Server_Pdu ultimatunPdu = null;
            ultimatunPdu = rdpbcgrServerStack.CreateMCSDisconnectProviderUltimatumPdu(sessionContext, (int)reason);

            if (invalidType.Equals(NegativeType.InvalidTPKTHeader))
            {
                ultimatunPdu.tpktHeader.length--;
            }
            else if (invalidType.Equals(NegativeType.InvalidX224))
            {
                ultimatunPdu.x224Data.length--;
            }

            SendPdu(ultimatunPdu);
        }

        /// <summary>
        /// Send a Shutdown Request Denied Pdu to client.
        /// </summary>
        public void ServerShutdownRequestDenied()
        {
            Server_Shutdown_Request_Denied_Pdu shutdownDeniedPdu =
                rdpbcgrServerStack.CreateShutdownRequestDeniedPdu(sessionContext);
            SendPdu(shutdownDeniedPdu);
        }

        #endregion "Disconnect sequence"

        #region Server Error Reporting and Status Updates
        /// <summary>
        /// Send Server Set Error Info PDU to client.
        /// </summary>
        /// <param name="value">The error Code.</param>
        public void ServerSetErrorInfoPdu(errorInfo_Values value)
        {
            Server_Set_Error_Info_Pdu errorPdu = null;

            errorPdu = rdpbcgrServerStack.CreateSetErrorInfoPdu(sessionContext, value);
            SendPdu(errorPdu);
        }
        #endregion

        #region Keyboard Status PDUs
        /// <summary>
        /// Send Server Set Keyboard Indicators PDU to client.
        /// </summary>
        /// <param name="scroll">Indicates that the Scroll Lock indicator light is on or not.</param>
        /// <param name="num">Indicates that the Num Lock indicator light is on or not.</param>
        /// <param name="caps">Indicates that the Caps Lock indicator light is on or not.</param>
        /// <param name="kana">Indicates that the Kana Lock indicator light is on or not.</param>
        public void ServerKeybordIndicators(bool scroll, bool num, bool caps, bool kana)
        {
            LedFlags_Values values = LedFlags_Values.None;
            Server_Set_Keyboard_Indicators_Pdu keybordPdu = null;

            if (scroll)
                values |= LedFlags_Values.TS_SYNC_SCROLL_LOCK;
            if (num)
                values |= LedFlags_Values.TS_SYNC_NUM_LOCK;
            if (caps)
                values |= LedFlags_Values.TS_SYNC_CAPS_LOCK;
            if (kana)
                values |= LedFlags_Values.TS_SYNC_KANA_LOCK;

            keybordPdu = rdpbcgrServerStack.CreateSetKeyboardIndicatorsPdu(sessionContext, values);

            Console.WriteLine("Send the keyboard indicators PDU.");
            SendPdu(keybordPdu);
        }

        /// <summary>
        /// Send Server Set Keyboard IME Status PDU to client.
        /// </summary>
        public void ServerSetKeyboardIme()
        {
            Server_Set_Keyboard_IME_Status_Pdu imePdu = null;

            imePdu = rdpbcgrServerStack.CreateSetKeyboardIMEStatusPdu(sessionContext, (uint)0, 0);
            SendPdu(imePdu);
        }
        #endregion

        #region Basic Output

        #region Fast-Path
        /// <summary>
        /// Send Fast-Path Bitmap Update to client.
        /// </summary>
        /// <param name="left">Left bound of the rectangle.</param>
        /// <param name="top">Top bound of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public void FPUpdateBitmap(ushort left, ushort top, ushort width, ushort height)
        {
            TS_FP_UPDATE_PDU fpOutput = null;
            TS_FP_UPDATE_BITMAP bitmap = null;

            fpOutput = RDPBCGROutput.CreateFPUpdatePDU(sessionContext, 1);
            bitmap = RDPBCGROutput.CreateFPUpdateBitmap(left, top, width, height);
            fpOutput.fpOutputUpdates[0] = bitmap;

            SendPdu(fpOutput);
        }

        /// <summary>
        /// Send Fast-Path Pointer Position Update to client.
        /// </summary>
        /// <param name="x">The x-coordinate relative to the top-left corner of the server's desktop.</param>
        /// <param name="y">The y-coordinate relative to the top-left corner of the server's desktop.</param>
        public void FPPointerPosition(int x, int y)
        {
            TS_FP_UPDATE_PDU fpOutput = null;
            TS_FP_POINTERPOSATTRIBUTE fpPos = new TS_FP_POINTERPOSATTRIBUTE();

            fpOutput = RDPBCGROutput.CreateFPUpdatePDU(sessionContext, 1);
            fpPos = RDPBCGROutput.CreateFPPointerPosAttribute(x, y);
            fpOutput.fpOutputUpdates[0] = fpPos;

            SendPdu(fpOutput);
        }

        /// <summary>
        /// Send Fast-Path System Pointer Hidden Update to client.
        /// </summary>
        public void FPSystemPointerHidden()
        {
            TS_FP_UPDATE_PDU fpOutput = null;
            TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE fpSysPointer = new TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE();

            fpOutput = RDPBCGROutput.CreateFPUpdatePDU(sessionContext, 1);
            fpSysPointer = RDPBCGROutput.CreateFPSystemPointerHiddenAttribute();
            fpOutput.fpOutputUpdates[0] = fpSysPointer;

            SendPdu(fpOutput);
        }

        /// <summary>
        /// Send Fast-Path System Pointer Default Update to client.
        /// </summary>
        public void FPSystemPointerDefault()
        {
            TS_FP_UPDATE_PDU fpOutput = null;
            TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE fpSysPointer = new TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE();

            fpOutput = RDPBCGROutput.CreateFPUpdatePDU(sessionContext, 1);
            fpSysPointer = RDPBCGROutput.CreateFPSystemPointerDefaultAttribute();
            fpOutput.fpOutputUpdates[0] = fpSysPointer;

            SendPdu(fpOutput);
        }

        /// <summary>
        /// Send Fast-Path Color Pointer Update to client.
        /// </summary>
        /// <param name="cacheIndex">Cache entry in the pointer cache </param>
        /// <param name="hotSpotX">X coordinate of hotSpot</param>
        /// <param name="hotSpotY">Y coordinate of hotSpot</param>
        /// <param name="width">Width of the pointer</param>
        /// <param name="height">Height of the pointer</param>
        /// <param name="xorMaskData">Data of XOR mask</param>
        /// <param name="andMaskData">Data of AND mask</param>
        public void FPColorPointer(ushort cacheIndex, ushort hotSpotX, ushort hotSpotY, ushort width, ushort height,
            byte[] xorMaskData = null, byte[] andMaskData = null)
        {
            TS_FP_UPDATE_PDU updatePdu = RDPBCGROutput.CreateFPUpdatePDU(sessionContext, 1);
            TS_FP_COLORPOINTERATTRIBUTE colorAttr = RDPBCGROutput.CreateFPColorPointerAttribute(cacheIndex, hotSpotX, hotSpotY, width, height,
                xorMaskData, andMaskData);
            updatePdu.fpOutputUpdates[0] = colorAttr;

            SendPdu(updatePdu);
        }

        /// <summary>
        /// Send Fast-Path New Pointer Update to client.
        /// </summary>
        /// <param name="xorBpp">Color depth in bits-per-pixel of the XOR mask</param>
        /// <param name="cacheIndex">Cache entry in the pointer cache </param>
        /// <param name="hotSpotX">X coordinate of hotSpot</param>
        /// <param name="hotSpotY">Y coordinate of hotSpot</param>
        /// <param name="width">Width of the pointer</param>
        /// <param name="height">Height of the pointer</param>
        /// <param name="xorMaskData">Data of XOR mask</param>
        /// <param name="andMaskData">Data of AND mask</param>
        public void FPNewPointer(ushort xorBpp, ushort cacheIndex, ushort hotSpotX, ushort hotSpotY, ushort width, ushort height,
            byte[] xorMaskData = null, byte[] andMaskData = null)
        {
            TS_FP_UPDATE_PDU updatePdu = RDPBCGROutput.CreateFPUpdatePDU(sessionContext, 1);
            TS_FP_POINTERATTRIBUTE pointerAttr = RDPBCGROutput.CreateFPPointerAttribute(xorBpp, cacheIndex, hotSpotX, hotSpotY, width, height,
                xorMaskData, andMaskData);
            updatePdu.fpOutputUpdates[0] = pointerAttr;

            SendPdu(updatePdu);
        }

        /// <summary>
        /// Send Fast-Path Cached Pointer Update to client
        /// </summary>
        /// <param name="cacheIndex">Cache entry in the pointer cache</param>
        public void FPCachedPointer(ushort cacheIndex)
        {
            TS_FP_UPDATE_PDU updatePdu = RDPBCGROutput.CreateFPUpdatePDU(sessionContext, 1);
            TS_FP_CACHEDPOINTERATTRIBUTE cachedPointer = RDPBCGROutput.CreateCachedPointerAttribute(cacheIndex);
            updatePdu.fpOutputUpdates[0] = cachedPointer;

            SendPdu(updatePdu);
        }
        /// <summary>
        /// Send Fast-Path Surface Commands Update to client.
        /// </summary>
        /// <param name="left">Left bound of the rectangle.</param>
        /// <param name="top">Top bound of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public void FPSurfaceCommand(ushort left, ushort top, ushort width, ushort height)
        {
            TS_FP_UPDATE_PDU fpOutput = null;
            TS_FP_SURFCMDS surfCmds = null;
            TS_SURFCMD_SET_SURF_BITS setSurfBits = null;

            fpOutput = RDPBCGROutput.CreateFPUpdatePDU(sessionContext, 1);
            setSurfBits = RDPBCGROutput.CreateSurfCmdSetSurfBits(left, top, width, height);
            surfCmds = RDPBCGROutput.CreateFPSurfCmds(setSurfBits);


            surfCmds.surfaceCommands[0] = setSurfBits;
            fpOutput.fpOutputUpdates[0] = surfCmds;

            SendPdu(fpOutput);
        }

        /// <summary>
        /// The TS_FP_SURFCMDS structure encapsulates one or more Surface Command (section 2.2.9.1.2.1.10.1) structures.
        /// </summary>
        /// <param name="surfaceCommands">An array of Surface Command (section 2.2.9.1.2.1.10.1) structures 
        /// containing a collection of commands to be processed by the client.</param>
        public void SendSurfaceCommandsUpdate(TS_FP_SURFCMDS surfaceCommands)
        {
            TS_FP_UPDATE_PDU fpOutput;
            TS_FP_UPDATE[] updates = new TS_FP_UPDATE[1];
            updates[0] = surfaceCommands;
            fpOutput = rdpbcgrServerStack.CreateFastPathUpdatePdu(sessionContext, updates);
            SendPdu(fpOutput);
        }

        /// <summary>
        /// The Frame Marker Command is used to group multiple surface commands 
        /// so that these commands can be processed and presented to the user as a single entity, a frame.
        /// </summary>
        /// <param name="frameAction">A 16-bit, unsigned integer. Identifies the beginning and end of a frame.</param>
        /// <param name="frameId">A 32-bit, unsigned integer. The ID identifying the frame.</param>
        public void SendFrameMarkerCommand(frameAction_Values frameAction, uint frameId)
        {
            TS_FRAME_MARKER frameMakerCmd = new TS_FRAME_MARKER();
            frameMakerCmd.cmdType = cmdType_Values.CMDTYPE_FRAME_MARKER;
            frameMakerCmd.frameAction = frameAction;
            frameMakerCmd.frameId = frameId;

            TS_FP_SURFCMDS surfCmds = new TS_FP_SURFCMDS();
            surfCmds.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_SURFCMDS & 0x0f)
            | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
            | ((int)compressedType_Values.None << 6));
            surfCmds.compressionFlags = compressedType_Values.None;
            surfCmds.size = 8; //size of TS_FRAME_MARKER;
            surfCmds.surfaceCommands = new TS_SURFCMD[1];
            surfCmds.surfaceCommands[0] = frameMakerCmd;

            SendSurfaceCommandsUpdate(surfCmds);
        }

        /// <summary>
        /// The Stream Surface Bits Command is used to transport encoded bitmap data destined 
        /// for a rectangular region of the current target surface from an RDP server to an RDP client.
        /// </summary>
        /// <param name="streamCmd">Stream Surface Bits Command.</param>
        public void SendStreamSurfaceBitsCommand(TS_SURFCMD_STREAM_SURF_BITS streamCmd)
        {
            TS_FP_SURFCMDS surfCmds = new TS_FP_SURFCMDS();
            surfCmds.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_SURFCMDS & 0x0f)
            | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
            | ((int)compressedType_Values.None << 6));
            surfCmds.compressionFlags = compressedType_Values.None;
            // size of cmdType + destLeft + destTop + destRight + destBottom = 10
            // size of bpp + flags + reserved + codecId + width + height + bitmapDataLength = 12
            // size of exBitmapDataHeader = 24, if bitmapData.flags contains TSBitmapDataExFlags_Values.EX_COMPRESSED_BITMAP_HEADER_PRESENT
            int subLength = 22;
            if (streamCmd.bitmapData.exBitmapDataHeader != null)
            {
                subLength += 24;
            }
            surfCmds.size = (ushort)(subLength + streamCmd.bitmapData.bitmapDataLength); //size of TS_SURFCMD_STREAM_SURF_BITS;

            surfCmds.surfaceCommands = new TS_SURFCMD[1];
            surfCmds.surfaceCommands[0] = streamCmd;
            SendSurfaceCommandsUpdate(surfCmds);
        }

        #endregion "Fast Path Output"

        #region Slow-Path
        /// <summary>
        /// Send a Slow-Path output PDU to client.
        /// </summary>
        /// <param name="invalidType">The invalid Type.</param>
        public void SendSlowPathOutputPdu(SlowPathTest_InvalidType invalidType)
        {
            SlowPathOutputPdu spOutputPdu = rdpbcgrServerStack.CreateSlowPathUpdataPdu(sessionContext);

            //Modify the SDK created pdu.
            foreach (RdpbcgrSlowPathUpdatePdu spPdu in spOutputPdu.slowPathUpdates)
            {
                if (spPdu is TS_POINTER_PDU)
                {
                    TS_POINTER_PDU updatePtr = (TS_POINTER_PDU)spPdu;
                    spOutputPdu.slowPathUpdates = new RdpbcgrSlowPathUpdatePdu[1];
                    spOutputPdu.slowPathUpdates[0] = updatePtr;
                    break;
                }
            }

            if (invalidType == SlowPathTest_InvalidType.InvalidTPKTLength)
            {
                spOutputPdu.commonHeader.tpktHeader.length = (ushort)(spOutputPdu.commonHeader.tpktHeader.length - 1);
            }
            else if (invalidType == SlowPathTest_InvalidType.InvalidX224Length)
            {
                spOutputPdu.commonHeader.x224Data.length = (byte)(spOutputPdu.commonHeader.x224Data.length - 1);
            }
            else if (invalidType == SlowPathTest_InvalidType.InvalidMCSLength)
            {
                //Set MCS length field to an invalid value in wrapper class
                SlowPathOutputPduEx exPdu = new SlowPathOutputPduEx(spOutputPdu, sessionContext);
                SendPdu(exPdu);
                return;
            }
            else if (invalidType == SlowPathTest_InvalidType.InvalidEncryptFlag)
            {
                if (spOutputPdu.commonHeader.securityHeader == null)
                {
                    spOutputPdu.commonHeader.securityHeader = new TS_SECURITY_HEADER();
                    spOutputPdu.commonHeader.securityHeader.flagsHi = 0;
                }
                spOutputPdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
            }
            else if (invalidType == SlowPathTest_InvalidType.InvalidSignature)
            {
                if (spOutputPdu.commonHeader.securityHeader != null)
                {
                    if (spOutputPdu.commonHeader.securityHeader is TS_SECURITY_HEADER)
                    {
                        TS_SECURITY_HEADER1 secHeader = new TS_SECURITY_HEADER1();
                        secHeader.flags = spOutputPdu.commonHeader.securityHeader.flags | TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                        secHeader.flagsHi = spOutputPdu.commonHeader.securityHeader.flagsHi;
                        secHeader.dataSignature = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 }; //invalid signature
                        spOutputPdu.commonHeader.securityHeader = secHeader;
                    }
                    else if (spOutputPdu.commonHeader.securityHeader is TS_SECURITY_HEADER1)
                    {
                        ((TS_SECURITY_HEADER1)spOutputPdu.commonHeader.securityHeader).dataSignature = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 }; //invalid signature
                    }
                    else if (spOutputPdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
                    {
                        ((TS_SECURITY_HEADER2)spOutputPdu.commonHeader.securityHeader).dataSignature = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8 }; //invalid signature
                    }
                }
            }
            else if (invalidType == SlowPathTest_InvalidType.InvalidTotalLength)
            {
                foreach (RdpbcgrSlowPathUpdatePdu spPdu in spOutputPdu.slowPathUpdates)
                {
                    if (spPdu is TS_POINTER_PDU)
                    {
                        TS_POINTER_PDU updatePtr = (TS_POINTER_PDU)spPdu;
                        updatePtr.shareDataHeader.shareControlHeader.totalLength = (ushort)(updatePtr.shareDataHeader.shareControlHeader.totalLength - 1);
                        spOutputPdu.slowPathUpdates = new RdpbcgrSlowPathUpdatePdu[1];
                        spOutputPdu.slowPathUpdates[0] = updatePtr;
                        break;
                    }
                }
            }
            SendPdu(spOutputPdu);
        }

        /// <summary>
        /// Send Server Play Sound PDU to client.
        /// </summary>
        /// <param name="frequency">Frequency of the beep the client MUST play.</param>
        /// <param name="duration">Duration of the beep the client MUST play.</param>
        public void PlaySound(uint frequency, uint duration)
        {
            Server_Play_Sound_Pdu playSoundPdu = rdpbcgrServerStack.CreatePlaySoundPdu(sessionContext);
            playSoundPdu.playSoundPduData.frequency = frequency;
            playSoundPdu.playSoundPduData.duration = duration;

            SendPdu(playSoundPdu);
        }
        #endregion

        #endregion

        #region Logon and Authorization Notifications

        /// <summary>
        /// Send Server Save Session Info PDU to client.
        /// </summary>
        /// <param name="notificationType">The Logon Notification type.</param>
        /// <param name="errorType">The Logon Error type. Only used when Notification Type is LogonError.</param>
        public void ServerSaveSessionInfo(LogonNotificationType notificationType, ErrorNotificationType_Values errorType)
        {
            if (this.sessionState != ServerSessionState.RDPConnected || this.connectionFinalizationState != ConnectionFinalization_ServerState.ServerFontMapSent)
            {
                return;
            }

            TS_LOGON_INFO logonInfoVersion1 = new TS_LOGON_INFO();
            TS_LOGON_INFO_VERSION_2 logonInfoVersion2 = new TS_LOGON_INFO_VERSION_2();
            TS_PLAIN_NOTIFY logonInfoPlain = new TS_PLAIN_NOTIFY();
            TS_LOGON_INFO_EXTENDED logonInfoExtended = new TS_LOGON_INFO_EXTENDED();
            Server_Save_Session_Info_Pdu saveSessionPdu = null;
            TS_GENERAL_CAPABILITYSET generalCapSet;

            //Refer to 3.3.5.10 
            if (notificationType == LogonNotificationType.UserLoggedOn)
            {
                #region User Logged On Notification
                if ((tsInfoPacket.flags & flags_Values.INFO_LOGONNOTIFY) == flags_Values.INFO_LOGONNOTIFY)
                {
                    if (clientCapSet != null)
                    {
                        generalCapSet = (TS_GENERAL_CAPABILITYSET)clientCapSet.FindCapSet(capabilitySetType_Values.CAPSTYPE_GENERAL);
                        if ((generalCapSet.extraFlags & extraFlags_Values.LONG_CREDENTIALS_SUPPORTED) == extraFlags_Values.LONG_CREDENTIALS_SUPPORTED)
                        {
                            logonInfoVersion2.Version = TS_LOGON_INFO_VERSION_2_Version_Values.SAVE_SESSION_PDU_VERSION_ONE;
                            logonInfoVersion2.Size = 576;
                            logonInfoVersion2.SessionId = 0;
                            logonInfoVersion2.cbDomain = tsInfoPacket.cbDomain;
                            logonInfoVersion2.Domain = tsInfoPacket.Domain.Trim('\0');
                            logonInfoVersion2.cbUserName = tsInfoPacket.cbUserName;
                            logonInfoVersion2.UserName = tsInfoPacket.UserName.Trim('\0');
                            logonInfoVersion2.Pad = new byte[558];

                            saveSessionPdu = rdpbcgrServerStack.CreateSaveSessionInfoPdu(sessionContext, logonInfoVersion2);
                            SendPdu(saveSessionPdu);
                        }
                        else
                        {
                            logonInfoVersion1.cbDomain = tsInfoPacket.cbDomain;
                            logonInfoVersion1.Domain = tsInfoPacket.Domain.Trim('\0');
                            logonInfoVersion1.cbUserName = tsInfoPacket.cbUserName;
                            logonInfoVersion1.UserName = tsInfoPacket.UserName.Trim('\0');
                            logonInfoVersion1.SessionId = 0;

                            saveSessionPdu = rdpbcgrServerStack.CreateSaveSessionInfoPdu(sessionContext, logonInfoVersion1);
                            SendPdu(saveSessionPdu);
                        }
                    }
                }
                else
                {
                    logonInfoPlain.Pad = new byte[576];
                    saveSessionPdu = rdpbcgrServerStack.CreateSaveSessionInfoPdu(sessionContext, logonInfoPlain);
                    SendPdu(saveSessionPdu);
                }
                #endregion
            }
            else if (notificationType == LogonNotificationType.AutoReconnectCookie)
            {
                logonInfoExtended.FieldsPresent = FieldsPresent_Values.LOGON_EX_AUTORECONNECTCOOKIE;
                logonInfoExtended.LogonFields = new TS_LOGON_INFO_FIELD[1];
                TS_LOGON_INFO_FIELD logonField = new TS_LOGON_INFO_FIELD();

                ARC_SC_PRIVATE_PACKET autoReconnectPacket = new ARC_SC_PRIVATE_PACKET();
                autoReconnectPacket.cbLen = cbLen_Values.V1;
                autoReconnectPacket.Version = Version_Values.AUTO_RECONNECT_VERSION_1;
                autoReconnectPacket.LogonId = RdpbcgrTestData.Test_LogonId;
                autoReconnectPacket.ArcRandomBits = RdpbcgrTestData.Test_ArcRadndomBits;

                logonField.cbFieldData = (uint)autoReconnectPacket.cbLen;
                logonField.FieldData = autoReconnectPacket;
                logonInfoExtended.LogonFields[0] = logonField;

                logonInfoExtended.Pad = new byte[570];
                logonInfoExtended.Length = (ushort)(576 + logonField.cbFieldData + 4);
                saveSessionPdu = rdpbcgrServerStack.CreateSaveSessionInfoPdu(sessionContext, logonInfoExtended);
                SendPdu(saveSessionPdu);
            }
            else // notificationType == LogonNotificationType.LogonError
            {
                logonInfoExtended.FieldsPresent = FieldsPresent_Values.LOGON_EX_LOGONERRORS;
                logonInfoExtended.LogonFields = new TS_LOGON_INFO_FIELD[1];
                TS_LOGON_INFO_FIELD logonField = new TS_LOGON_INFO_FIELD();

                TS_LOGON_ERRORS_INFO errorInfo = new TS_LOGON_ERRORS_INFO();
                errorInfo.ErrorNotificationType = errorType;
                errorInfo.ErrorNotificationData = ErrorNotificationData_Values.LOGON_MSG_NO_PERMISSION;

                logonField.cbFieldData = 8;
                logonField.FieldData = errorInfo;
                logonInfoExtended.LogonFields[0] = logonField;

                logonInfoExtended.Pad = new byte[570];
                logonInfoExtended.Length = (ushort)(576 + logonField.cbFieldData + 4);
                saveSessionPdu = rdpbcgrServerStack.CreateSaveSessionInfoPdu(sessionContext, logonInfoExtended);
                SendPdu(saveSessionPdu);
            }

        }

        /// <summary>
        /// Send  Early User Authorization Result PDU
        /// </summary>
        /// <param name="authorizationResult">The Authorization result value</param>
        public void SendEarlyUserAuthorizationResultPDU(Authorization_Result_value authorizationResult)
        {
            Early_User_Authorization_Result_PDU resultPdu = new Early_User_Authorization_Result_PDU(authorizationResult);
            SendPdu(resultPdu);
        }

        #endregion

        #region Auto-Reconnect
        /// <summary>
        /// Send Server Auto-Reconnect Status PDU.
        /// </summary>
        public void ServerAutoReconnectStatusPdu()
        {
            Server_Auto_Reconnect_Status_Pdu statusPdu = rdpbcgrServerStack.CreateAutoReconnectStatusPdu(sessionContext);
            SendPdu(statusPdu);
        }
        #endregion

        #region Display Update Notifications
        /// <summary>
        /// Send Monitor Layout Pdu to client. 
        /// </summary>
        public void MonitorLayoutPdu()
        {
            TS_MONITOR_LAYOUT_PDU monitor = null;

            monitor = rdpbcgrServerStack.CreateMonitorLayoutPdu(sessionContext);
            monitor.monitorDefArray[0].left = 0;
            monitor.monitorDefArray[0].top = 0;
            monitor.monitorDefArray[0].right = 1151;
            monitor.monitorDefArray[0].bottom = 863;

            SendPdu(monitor);
        }
        #endregion

        #region Virtual Channel

        /// <summary>
        /// Send virtual channel data. 
        /// </summary>
        /// <param name="channelId">The channel Id of the virtual channel. </param>
        /// <param name="virtualChannelData">The virtual channel data to be sent. </param>
        /// <param name="invalidType">Invalid Type.</param>
        public void SendVirtualChannelPDU(UInt16 channelId, byte[] virtualChannelData, StaticVirtualChannel_InvalidType invalidType)
        {
            Virtual_Channel_Complete_Server_Pdu completePdu = this.rdpbcgrServerStack.CreateCompleteVirtualChannelPdu(sessionContext, channelId, virtualChannelData);
            if (invalidType == StaticVirtualChannel_InvalidType.None)
            {
                foreach (Virtual_Channel_RAW_Server_Pdu rawPdu in completePdu.rawPdus)
                {
                    SendPdu(rawPdu);
                }
            }
            else if (invalidType == StaticVirtualChannel_InvalidType.InvalidTPKTLength)
            {
                //Set the length field of TPKT header to an invalid value (less than the actual value).
                Virtual_Channel_RAW_Server_Pdu rawPdu = completePdu.rawPdus[0];
                rawPdu.commonHeader.tpktHeader.length = (ushort)(rawPdu.commonHeader.tpktHeader.length - 1);
                SendPdu(rawPdu);
            }
            else if (invalidType == StaticVirtualChannel_InvalidType.InvalidX224Length)
            {
                //Set the length field of X224 header to an invalid value (less than the actual value).
                Virtual_Channel_RAW_Server_Pdu rawPdu = completePdu.rawPdus[0];
                rawPdu.commonHeader.x224Data.length = (byte)(rawPdu.commonHeader.x224Data.length - 1);
                SendPdu(rawPdu);
            }
            else if (invalidType == StaticVirtualChannel_InvalidType.InvalidEncryptFlag)
            {
                //Presents the SEC_ENCRYPT flag in securityHeader when Enhanced RDP Security is in effect.
                Virtual_Channel_RAW_Server_Pdu rawPdu = completePdu.rawPdus[0];
                rawPdu.commonHeader.securityHeader = new TS_SECURITY_HEADER();
                rawPdu.commonHeader.securityHeader.flagsHi = 0;
                rawPdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                SendPdu(rawPdu);
                //StackPacket receivedPdu = this.rdpbcgrServerStack.ExpectPdu(sessionContext, new TimeSpan(0, 0, 30));
            }
            else if (invalidType == StaticVirtualChannel_InvalidType.InvalidFlag)
            {
                //The CHANNEL_FLAGS_SHOW_PROTOCOL (0x00000010) flag is not set in a virtual channel data chunk. 
                Virtual_Channel_RAW_Server_Pdu rawPdu = completePdu.rawPdus[0];
                rawPdu.channelPduHeader.flags = (CHANNEL_PDU_HEADER_flags_Values)((uint)rawPdu.channelPduHeader.flags & (uint)0xFFFFFFEF);
                //Not set CHANNEL_FLAG_LAST flag to indicate it's a chunck data.
                rawPdu.channelPduHeader.flags = (CHANNEL_PDU_HEADER_flags_Values)((uint)rawPdu.channelPduHeader.flags & (uint)0xFFFFFFFD);
                rawPdu.channelPduHeader.length = rawPdu.channelPduHeader.length + 1;
                SendPdu(rawPdu);
                //StackPacket receivedPdu = this.rdpbcgrServerStack.ExpectPdu(sessionContext, new TimeSpan(0, 0, 30));
            }
            else if (invalidType == StaticVirtualChannel_InvalidType.InvalidSignature)
            {
                //Not present the signature when the SEC_ENCRYPT flag in securityHeader when Enhanced RDP Security is in effect.
                Virtual_Channel_RAW_Server_Pdu rawPdu = completePdu.rawPdus[0];
                rawPdu.commonHeader.securityHeader = new TS_SECURITY_HEADER();
                rawPdu.commonHeader.securityHeader.flagsHi = 0;
                rawPdu.commonHeader.securityHeader.flags |= TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                SendPdu(rawPdu);
            }
            else if (invalidType == StaticVirtualChannel_InvalidType.InvalidMCSLength)
            {
                //Set MCS length to an invalid value in wrapper class.
                Virtual_Channel_RAW_Server_Pdu rawPdu = completePdu.rawPdus[0];
                Virtual_Channel_RAW_Server_Pdu_Ex exPdu = new Virtual_Channel_RAW_Server_Pdu_Ex(rawPdu, sessionContext);
                SendPdu(exPdu);
            }
            else
            {
                //Set the length field of TPKT header to an invalid value (less than the actual value).
                Virtual_Channel_RAW_Server_Pdu rawPdu = completePdu.rawPdus[0];
                rawPdu.commonHeader.tpktHeader.length = (ushort)(rawPdu.commonHeader.tpktHeader.length - 1);
                SendPdu(rawPdu);
            }

        }

        #endregion

        #region Server Redirection

        /// <summary>
        /// Sending Server Redirection Pdu to client.
        /// </summary>
        /// <param name="presentRoutingToken">Indicates if present routing token (LoadBalanceInfo).</param>
        public void SendServerRedirectionPdu(bool presentRoutingToken)
        {
            UnicodeEncoding encoder = new UnicodeEncoding();
            RDP_SERVER_REDIRECTION_PACKET redirectPacket = new RDP_SERVER_REDIRECTION_PACKET();

            redirectPacket.Flags = RDP_SERVER_REDIRECTION_PACKET_FlagsEnum.SEC_REDIRECTION_PKT;
            redirectPacket.SessionID = RdpbcgrTestData.Test_Redirection_SessionId;

            if (presentRoutingToken)
            {
                byte[] routingTokenBytes = ASCIIEncoding.ASCII.GetBytes(RdpbcgrTestData.Test_Redirection_RoutingToken);
                redirectPacket.LoadBalanceInfo = routingTokenBytes;
                redirectPacket.RedirFlags |= RedirectionFlags.LB_LOAD_BALANCE_INFO;
            }
            else
            {
                string serverIP = ((System.Net.IPEndPoint)(sessionContext.LocalIdentity)).Address.ToString();
                redirectPacket.TargetNetAddress = serverIP;
                redirectPacket.RedirFlags = RedirectionFlags.LB_TARGET_NET_ADDRESS;
            }

            redirectPacket.UserName = RdpbcgrTestData.Test_UserName;
            redirectPacket.RedirFlags |= RedirectionFlags.LB_USERNAME;
            redirectPacket.RedirFlags |= RedirectionFlags.LB_DONTSTOREUSERNAME;

            redirectPacket.Domain = RdpbcgrTestData.Test_Domain;
            redirectPacket.RedirFlags |= RedirectionFlags.LB_DOMAIN;

            redirectPacket.Password = encoder.GetBytes(RdpbcgrTestData.Test_Password);
            redirectPacket.RedirFlags |= RedirectionFlags.LB_PASSWORD;

            redirectPacket.Pad = new byte[8];

            redirectPacket.UpdateLength();

            if (this.serverConfig.encryptedProtocol == EncryptedProtocol.Rdp)
            {
                Server_Redirection_Pdu redirPdu = this.rdpbcgrServerStack.CreateStandardRedirectionPdu(sessionContext, redirectPacket);
                SendPdu(redirPdu);
            }
            else
            {
                Enhanced_Security_Server_Redirection_Pdu redirPdu = this.rdpbcgrServerStack.CreateEnhancedRedirectionPdu(sessionContext, redirectPacket);
                SendPdu(redirPdu);
            }
        }

        public void SendServerRedirectionPduRDSTLS()
        {
            UnicodeEncoding encoder = new UnicodeEncoding();
            RDP_SERVER_REDIRECTION_PACKET redirectPacket = new RDP_SERVER_REDIRECTION_PACKET();

            redirectPacket.Flags = RDP_SERVER_REDIRECTION_PACKET_FlagsEnum.SEC_REDIRECTION_PKT;
            redirectPacket.SessionID = RdpbcgrTestData.Test_Redirection_SessionId;

            string serverIP = ((System.Net.IPEndPoint)(sessionContext.LocalIdentity)).Address.ToString();
            redirectPacket.TargetNetAddress = serverIP;
            redirectPacket.RedirFlags = RedirectionFlags.LB_TARGET_NET_ADDRESS;


            redirectPacket.UserName = RdpbcgrTestData.Test_UserName;
            redirectPacket.RedirFlags |= RedirectionFlags.LB_USERNAME;

            redirectPacket.Domain = RdpbcgrTestData.Test_Domain;
            redirectPacket.RedirFlags |= RedirectionFlags.LB_DOMAIN;

            redirectPacket.Password = RdpbcgrUtility.EncodeUnicodeStringToBytes(RdpbcgrTestData.Test_Password);
            redirectPacket.RedirFlags |= RedirectionFlags.LB_PASSWORD;
            redirectPacket.RedirFlags |= RedirectionFlags.LB_PASSWORD_IS_PK_ENCRYPTED;

            redirectPacket.TargetNetAddresses = new TARGET_NET_ADDRESSES();
            redirectPacket.TargetNetAddresses.address = new TARGET_NET_ADDRESS[1];
            redirectPacket.TargetNetAddresses.address[0] = new TARGET_NET_ADDRESS();
            redirectPacket.TargetNetAddresses.address[0].address = serverIP;
            redirectPacket.RedirFlags |= RedirectionFlags.LB_TARGET_NET_ADDRESSES;

            redirectPacket.RedirectionGuid = RdpbcgrUtility.EncodeUnicodeStringToBytes(RdpbcgrTestData.Test_RedirectionGuid);
            redirectPacket.RedirFlags |= RedirectionFlags.LB_REDIRECTION_GUID;

            var certificate = new X509Certificate2(certFile, certPwd);
            redirectPacket.TargetCertificate = RdpbcgrUtility.EncodeCertificate(certificate);
            redirectPacket.RedirFlags |= RedirectionFlags.LB_TARGET_CERTIFICATE;

            redirectPacket.TargetFQDN = RdpbcgrTestData.Test_FullQualifiedDomainName;
            redirectPacket.RedirFlags |= RedirectionFlags.LB_TARGET_FQDN;

            redirectPacket.TargetNetBiosName = RdpbcgrTestData.Test_NetBiosName;
            redirectPacket.RedirFlags |= RedirectionFlags.LB_TARGET_NETBIOS_NAME;


            redirectPacket.Pad = new byte[8];

            redirectPacket.UpdateLength();

            if (this.serverConfig.encryptedProtocol == EncryptedProtocol.Rdp)
            {
                Server_Redirection_Pdu redirPdu = this.rdpbcgrServerStack.CreateStandardRedirectionPdu(sessionContext, redirectPacket);
                SendPdu(redirPdu);
            }
            else
            {
                Enhanced_Security_Server_Redirection_Pdu redirPdu = this.rdpbcgrServerStack.CreateEnhancedRedirectionPdu(sessionContext, redirectPacket);
                SendPdu(redirPdu);
            }
        }

        #endregion

        #region Auto-Detection
        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_RTT_REQUEST
        /// </summary>
        public void SendAutoDetectRequestPdu_RTTRequest(ushort sequenceNumber, bool isInConnectTime)
        {
            AUTO_DETECT_REQUEST_TYPE type = AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_AFTER_CONNECTTIME;
            if (isInConnectTime)
            {
                type = AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_IN_CONNECTTIME;
            }
            RDP_RTT_REQUEST rttRequest = RdpbcgrUtility.GenerateRTTMeasureRequest(type, sequenceNumber);
            Server_Auto_Detect_Request_PDU autoDetectRequestPdu = rdpbcgrServerStack.CreateServerAutoDetectRequestPDU(sessionContext, rttRequest);

            SendPdu(autoDetectRequestPdu);
        }

        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_BW_START
        /// </summary>
        public void SendAutoDetectRequestPdu_BWStart(ushort sequenceNumber, bool isInConnectTime)
        {
            AUTO_DETECT_REQUEST_TYPE type = AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP;
            if (isInConnectTime)
            {
                type = AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_IN_CONNECTTIME;
            }
            RDP_BW_START bwStart = RdpbcgrUtility.GenerateBandwidthMeasureStart(type, sequenceNumber);
            Server_Auto_Detect_Request_PDU autoDetectRequestPdu = rdpbcgrServerStack.CreateServerAutoDetectRequestPDU(sessionContext, bwStart);

            SendPdu(autoDetectRequestPdu);
        }

        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_BW_PAYLOAD
        /// </summary>
        public void SendAutoDetectRequestPdu_BWPayload(ushort sequenceNumber, ushort payloadLength, byte[] payload)
        {
            RDP_BW_PAYLOAD bwPayload = RdpbcgrUtility.GenerateBandwidthMeasurePayload(AUTO_DETECT_REQUEST_TYPE.RDP_BW_PAYLOAD, sequenceNumber, payloadLength, payload);
            Server_Auto_Detect_Request_PDU autoDetectRequestPdu = rdpbcgrServerStack.CreateServerAutoDetectRequestPDU(sessionContext, bwPayload);

            SendPdu(autoDetectRequestPdu);
        }

        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_BW_STOP
        /// </summary>
        public void SendAutoDetectRequestPdu_BWStop(ushort sequenceNumber, ushort payloadLength, byte[] payload, bool isInConnectTime)
        {
            AUTO_DETECT_REQUEST_TYPE type = AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP;
            if (isInConnectTime)
            {
                type = AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_IN_CONNECTTIME;
            }
            RDP_BW_STOP bwStop = RdpbcgrUtility.GenerateBandwidthMeasureStop(type, sequenceNumber, payloadLength, payload);
            Server_Auto_Detect_Request_PDU autoDetectRequestPdu = rdpbcgrServerStack.CreateServerAutoDetectRequestPDU(sessionContext, bwStop);

            SendPdu(autoDetectRequestPdu);
        }

        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_NETCHAR_RESULT
        /// </summary>
        public void SendAutoDetectRequestPdu_NetcharResult(ushort sequenceNumber)
        {
            autoDetectedBaseRTT = sessionContext.AutoDetectBaseRTT;
            autoDetectedBandwidth = sessionContext.AutoDetectBandWith;
            autoDetectedAverageRTT = sessionContext.AutoDetectAverageRTT;
            RDP_NETCHAR_RESULT netResult = RdpbcgrUtility.GenerateNetworkCharacteristicsResult(AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT, sequenceNumber, autoDetectedBaseRTT, autoDetectedBandwidth, autoDetectedAverageRTT);
            Server_Auto_Detect_Request_PDU autoDetectRequestPdu = rdpbcgrServerStack.CreateServerAutoDetectRequestPDU(sessionContext, netResult);

            SendPdu(autoDetectRequestPdu);
        }

        /// <summary>
        /// Send a Tunnel Data PDU with RDP_BW_START in its subheader
        /// </summary>
        /// <param name="requestedProtocol"></param>
        /// <param name="sequenceNumber"></param>
        public void SendTunnelDataPdu_BWStart(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber)
        {
            AUTO_DETECT_REQUEST_TYPE requestType = AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                requestType = AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_LOSSYUDP;
            }
            RDP_BW_START bwStart = RdpbcgrUtility.GenerateBandwidthMeasureStart(requestType, sequenceNumber);
            byte[] reqData = rdpbcgrServerStack.EncodeNetworkDetectionRequest(bwStart, true);
            byte[][] subHdDataArr = new byte[][] { reqData };
            SendTunnelData(requestedProtocol, null, subHdDataArr);

        }

        /// <summary>
        /// Send a Tunnel Data PDU with RDP_BW_STOP in its subheader
        /// </summary>
        /// <param name="requestedProtocol"></param>
        /// <param name="sequenceNumber"></param>
        public void SendTunnelDataPdu_BWStop(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber)
        {
            AUTO_DETECT_REQUEST_TYPE requestType = AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                requestType = AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_LOSSYUDP;
            }
            RDP_BW_STOP bwStop = RdpbcgrUtility.GenerateBandwidthMeasureStop(requestType, sequenceNumber);
            byte[] reqData = rdpbcgrServerStack.EncodeNetworkDetectionRequest(bwStop, true);
            byte[][] subHdDataArr = new byte[][] { reqData };
            SendTunnelData(requestedProtocol, null, subHdDataArr);

        }


        /// <summary>
        /// Send a Tunnel Data PDU with RDP_NETCHAR_RESULT in its subheader
        /// </summary>
        /// <param name="requestedProtocol"></param>
        /// <param name="sequenceNumber"></param>
        public void SendTunnelDataPdu_NetcharResult(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber)
        {
            autoDetectedBaseRTT = sessionContext.AutoDetectBaseRTT;
            autoDetectedBandwidth = sessionContext.AutoDetectBandWith;
            autoDetectedAverageRTT = sessionContext.AutoDetectAverageRTT;
            RDP_NETCHAR_RESULT netResult = RdpbcgrUtility.GenerateNetworkCharacteristicsResult(AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT, sequenceNumber, autoDetectedBaseRTT, autoDetectedBandwidth, autoDetectedAverageRTT);
            byte[] reqData = rdpbcgrServerStack.EncodeNetworkDetectionRequest(netResult, true);
            byte[][] subHdDataArr = new byte[][] { reqData };
            SendTunnelData(requestedProtocol, null, subHdDataArr);

        }

        /// <summary>
        /// Wait for a Tunnel Data PDU with RDP_BW_RESULTS and check its sequenceNumber.
        /// </summary>
        /// <param name="requestedProtocol">Which tunnel to be used, reliable or lossy</param>
        /// <param name="sequenceNumber"></param>
        /// <param name="timeout"></param>
        public void WaitForAndCheckTunnelDataPdu_BWResult(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber, TimeSpan timeout)
        {

            bool isReceived = false;
            TimeSpan leftTime = timeout;
            DateTime expiratedTime = DateTime.Now + timeout;
            RDP_BW_RESULTS bwResult = null;

            while (!isReceived && leftTime.CompareTo(new TimeSpan(0)) > 0)
            {
                try
                {
                    RDP_TUNNEL_DATA tunnelData = ExpectTunnelData(requestedProtocol, leftTime);
                    if (tunnelData != null)
                    {
                        RDP_TUNNEL_SUBHEADER[] SubHeaders = tunnelData.TunnelHeader.SubHeaders;
                        if (SubHeaders != null)
                        {
                            foreach (RDP_TUNNEL_SUBHEADER subHeader in SubHeaders)
                            {
                                if (subHeader.SubHeaderType == RDP_TUNNEL_SUBHEADER_TYPE_Values.TYPE_ID_AUTODETECT_RESPONSE)
                                {
                                    NETWORK_DETECTION_RESPONSE detectRsp = rdpbcgrServerStack.ParseNetworkDetectionResponse(subHeader.SubHeaderData, true);
                                    {
                                        if (detectRsp.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT)
                                        {
                                            bwResult = (RDP_BW_RESULTS)detectRsp;
                                            isReceived = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (TimeoutException)
                {
                    site.Assert.Fail("Timeout when expecting a Tunnel Data PDU with RDP_BW_RESULTS");
                }
                catch (InvalidOperationException ex)
                {
                    //break;
                    site.Log.Add(LogEntryKind.Warning, "Exception thrown out when receiving client PDUs {0}.", ex.Message);
                }
                finally
                {
                    System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                    leftTime = expiratedTime - DateTime.Now;
                }
            }
            if (isReceived)
            {
                CheckAutoDetectResponse(bwResult, AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT, sequenceNumber);
            }
            else
            {
                site.Assert.Fail("Timeout when expecting a Tunnel Data PDU with RDP_BW_RESULTS");
            }
        }
        /// <summary>
        /// Check whether the latest received Auto-Detect Response PDU is a valid PDU
        /// </summary>
        /// <param name="responseType">The responseType the Auto-Detect Response PDU should be</param>
        /// <param name="sequenceNumber">The sequenceNumber the Auto-Detect Response PDU should be</param>
        public void CheckAutoDetectResponsePdu(AUTO_DETECT_RESPONSE_TYPE responseType, ushort sequenceNumber)
        {
            NETWORK_DETECTION_RESPONSE autodetectRspPduData = sessionContext.AutoDetectRspPduData;
            CheckAutoDetectResponse(autodetectRspPduData, responseType, sequenceNumber);
        }

        /// <summary>
        /// Check whether the auto-detect Response structure is valid
        /// </summary>
        /// <param name="autodetectRspPduData">The auto-detect Response structure need to be checked</param>
        /// <param name="responseType">The responseType the Auto-Detect Response PDU should be</param>
        /// <param name="sequenceNumber">The sequenceNumber the Auto-Detect Response PDU should be</param>
        public void CheckAutoDetectResponse(NETWORK_DETECTION_RESPONSE autodetectRspPduData, AUTO_DETECT_RESPONSE_TYPE responseType, ushort sequenceNumber)
        {
            if (autodetectRspPduData.responseType != responseType)
            {
                site.Assert.Fail("Expect responseType is {0}, but receive responseType: {1}", responseType, autodetectRspPduData.responseType);
            }

            if (autodetectRspPduData.sequenceNumber != sequenceNumber)
            {
                site.Assert.Fail("Expect sequence Number is {0}, but receive sequence Number: {1}", sequenceNumber, autodetectRspPduData.sequenceNumber);
            }

            if (autodetectRspPduData.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_NETCHAR_SYNC)
            {
                RDP_NETCHAR_SYNC netcharSync = (RDP_NETCHAR_SYNC)autodetectRspPduData;
                if (netcharSync.rtt != autoDetectedAverageRTT)
                {
                    site.Assert.Fail("Rtt sent is {0}, but receive rtt: {1}", autoDetectedAverageRTT, netcharSync.rtt);
                }
                if (netcharSync.bandwidth != autoDetectedBandwidth)
                {
                    site.Assert.Fail("Bandwidth sent is {0}, but receive bandwidth: {1}", autoDetectedBandwidth, netcharSync.bandwidth);
                }
            }
        }
        #endregion

        #region Multitransport

        /// <summary>
        /// Send a Server Initiate Multitransport Request PDU
        /// </summary>
        /// <param name="requestedProtocol"></param>
        public void SendServerInitiateMultitransportRequestPDU(uint requestId, Multitransport_Protocol_value requestedProtocol, byte[] securityCookie)
        {
            Server_Initiate_Multitransport_Request_PDU requestPDU = rdpbcgrServerStack.CreateServerInitiateMultitransportRequestPDU(sessionContext, requestId, requestedProtocol, securityCookie);
            SendPdu(requestPDU);
        }

        /// <summary>
        /// Create a multitransport Channel over UDP
        /// </summary>
        /// <param name="requestedProtocol">Request protocol, indicate the channel is reliable or lossy</param>
        /// <param name="timeout"></param>
        public void CreateMultitransportChannelConnection(Multitransport_Protocol_value requestedProtocol, TimeSpan timeout)
        {
            if (rdpeudpServer == null)
            { 
                rdpeudpServer = new RdpeudpServer((IPEndPoint)this.SessionContext.LocalIdentity);

                rdpeudpServer.UnhandledExceptionReceived += (ex) =>
                {
                    Site.Log.Add(LogEntryKind.Debug, $"Unhandled exception from RdpeudpServer: {ex}");
                };
            }
            if (!rdpeudpServer.Running)
                rdpeudpServer.Start();

            //Send a Server Initiate Multitransport Request PDU
            byte[] securityCookie = new byte[16];
            Random rnd = new Random();
            rnd.NextBytes(securityCookie);
            SendServerInitiateMultitransportRequestPDU(++this.multitransportRequestId, requestedProtocol, securityCookie);

            TransportMode udpTransportMode = TransportMode.Reliable;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                udpTransportMode = TransportMode.Lossy;
            }

            RdpeudpServerSocket rdpudpSocket = rdpeudpServer.Accept(((IPEndPoint)this.SessionContext.Identity).Address, udpTransportMode, timeout);
            if (rdpudpSocket == null)
            {
                site.Assert.Fail("Create UDP connection failed");
            }


            //Authendicate the UDP channel
            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);
            RdpemtServer rdpemtServer = new RdpemtServer(rdpudpSocket, cert, false);

            uint receivedRequestId;
            byte[] receivedSecurityCookie;
            if (!rdpemtServer.ExpectConnect(timeout, out receivedRequestId, out receivedSecurityCookie))
            {
                site.Assert.Fail("RDPEMT tunnel creation failed");
            }
            if (receivedRequestId != this.multitransportRequestId)
            {
                site.Assert.Fail("RequestId is not consistent. The requestId sent is {0}, But receive requestId is {1}.", this.multitransportRequestId, receivedRequestId);
            }
            if (securityCookie.Equals(receivedSecurityCookie))
            {
                site.Assert.Fail("SecurityCookie is not consistent");
            }

            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR)
            {
                rdpemtServerReliable = rdpemtServer;
            }
            else
            {
                rdpemtServerLossy = rdpemtServer;
            }

        }

        /// <summary>
        /// Send data from multitransport tunnel
        /// </summary>
        /// <param name="requestedProtocol">Indicates which tunnel is used, reliable or lossy</param>
        /// <param name="data">Data want to sent</param>
        /// <param name="subHdDataArr">Array of Auto-detect data, will be send as subheader of TunnelDataPdu</param>
        public void SendTunnelData(Multitransport_Protocol_value requestedProtocol, byte[] data, byte[][] subHdDataArr = null)
        {
            RdpemtServer rdpemtServer = null;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR)
            {
                rdpemtServer = rdpemtServerReliable;
            }
            else
            {
                rdpemtServer = rdpemtServerLossy;
            }
            List<byte[]> subHdDataList = new List<byte[]>();
            subHdDataList.AddRange(subHdDataArr);
            RDP_TUNNEL_DATA tunnelData = rdpemtServer.CreateTunnelDataPdu(data, subHdDataList);

            rdpemtServer.SendRdpemtPacket(tunnelData);
        }

        /// <summary>
        /// Receive a Tunnel Data PDU from multitransport tunnel
        /// </summary>
        /// <param name="requestedProtocol">Indicates which tunnel is used, reliable or lossy</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RDP_TUNNEL_DATA ExpectTunnelData(Multitransport_Protocol_value requestedProtocol, TimeSpan timeout)
        {
            RdpemtServer rdpemtServer = null;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR)
            {
                rdpemtServer = rdpemtServerReliable;
            }
            else
            {
                rdpemtServer = rdpemtServerLossy;
            }

            return rdpemtServer.ExpectTunnelData(timeout);
        }
        #endregion Multitransport

        #region Connection Health Monitoring

        /// <summary>
        /// Send a Server Heartbeat PDU
        /// </summary>
        /// <param name="period">specifies the time (in seconds) between Heartbeat PDUs</param>
        /// <param name="warningCount">specifies how many missed heartbeats SHOULD trigger a client-side warning</param>
        /// <param name="reconnectCount">specifies how many missed heartbeats SHOULD trigger a client-side reconnection attempt</param>
        public void SendServerHeartbeatPDU(byte period, byte warningCount, byte reconnectCount)
        {
            Server_Heartbeat_PDU heartbeatPDU = rdpbcgrServerStack.CreateServerHeartbeatPDU(sessionContext, period, warningCount, reconnectCount);
            SendPdu(heartbeatPDU);
        }

        /// <summary>
        /// Check whether client support Heartbeat
        /// If supported, RNS_UD_CS_SUPPORT_HEARTBEAT_PDU should be set on earlyCapabilityFlags field of TS_UD_CS_CORE
        /// </summary>
        public void CheckHeartbeatSupport()
        {
            UInt16Class earlyCapabilityFlags = sessionContext.ClientEarlyCapabilityFlags;
            site.Assert.IsNotNull(earlyCapabilityFlags, "Client should support Heartbeat PDU (earlyCapabilityFlags field of TS_UD_CS_CORE includes RNS_UD_CS_SUPPORT_HEARTBEAT_PDU flag).");
            bool supportHeartbeat = ((earlyCapabilityFlags_Values)earlyCapabilityFlags.actualData).HasFlag(earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_HEARTBEAT_PDU);
            site.Assert.IsTrue(supportHeartbeat, "Client should support Heartbeat PDU (earlyCapabilityFlags field of TS_UD_CS_CORE includes RNS_UD_CS_SUPPORT_HEARTBEAT_PDU flag).");
        }

        #endregion Connection Health Monitoring

        #region Others

        /// <summary>
        /// Method to get the id of the specified static virtual channel.
        /// </summary>
        /// <param name="channelName">The specified channel name.</param>
        /// <returns>The channel id of the specified channel. If not found, return -1.</returns>
        public ushort GetStaticVirtualChannelId(string channelName)
        {
            if (this.svcNameIdDic.ContainsKey(channelName.ToUpper()))
            {
                return this.svcNameIdDic[channelName];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Method to trun off the RDPBCGR requirements verification.
        /// </summary>
        ///<param name="turnOff">Indicates if to turn off, "true" means to trun off, "false" means to turn on.</param>
        public void TurnVerificationOff(bool turnOff)
        {
            if (turnOff)
            {
                bVerifyRequirements = false;
            }
            else
            {
                bVerifyRequirements = true;
            }
        }

        #endregion

        #region RDSTLS authentication
        public void SendRDSTLSCapabilityPDU()
        {
            var pdu = rdpbcgrServerStack.CreateRDSTLSCapabilityPDU(sessionContext);
            SendPdu(pdu);
        }

        public void SendRDSTLSAuthenticationResponsePDU()
        {
            var pdu = rdpbcgrServerStack.CreateRDSTLSAuthenticationResponsePDU(sessionContext);
            SendPdu(pdu);
        }
        #endregion

        #endregion

        #region Receiving Packets
        /// <summary>
        /// Expect a specified type PDU from RDP client, 
        /// if the next received packet is not T, this call will be failed.
        /// </summary>
        /// <typeparam name="T">The expected type of pdu</typeparam>
        /// <param name="session">The target session</param>
        /// <param name="waitTimeSpan">Timeout</param>
        private void ExpectPacket<T>(RdpbcgrServerSessionContext session, TimeSpan waitTimeSpan)
        {
            object receivedPdu = null;

            try
            {
                receivedPdu = this.rdpbcgrServerStack.ExpectPdu(session, waitTimeSpan);
            }
            catch (TimeoutException)
            {
                site.Assert.Fail("Timeout when expecting {0}", typeof(T));
            }
            catch (InvalidOperationException ex)
            {
                string exceptionMsg = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                site.Assert.Fail("Exception thrown out when receiving client PDUs: {0}", exceptionMsg);
            }
            if (receivedPdu == null)
            {
                site.Assert.Fail("Can't get expecting PDU {0}", typeof(T));
            }
            else
            {
                if (receivedPdu is ErrorPdu)
                {
                    byte[] errorPduData = ((ErrorPdu)receivedPdu).ToBytes();
                    site.Log.Add(LogEntryKind.Debug, "The bytes of the ErrorPDU:\n{0}", BitConverter.ToString(errorPduData));
                }
                site.Assert.IsInstanceOfType(receivedPdu, typeof(T), "Receiving {0}", typeof(T).Name);
                ReceiveLoop((StackPacket)receivedPdu);
            }
        }

        /// <summary>
        /// Expect a specified type PDU from RDP client, 
        /// if the next received packet is not T, this call will be failed.
        /// </summary>
        /// <typeparam name="T">The expected type of pdu</typeparam>
        /// <param name="timeout">Timeout</param>
        public void ExpectPacket<T>(TimeSpan timeout)
        {
            ExpectPacket<T>(sessionContext, timeout);
        }

        /// <summary>
        /// Wait for a specified type pdu from RDP client, 
        /// if the next received packet is not T, continue to wait until timeout.
        /// </summary>
        /// <typeparam name="T">The expected type of pdu</typeparam>
        /// <param name="session">The target session</param>
        /// <param name="timeout">Timeout</param>
        private void WaitForPacket<T>(RdpbcgrServerSessionContext session, TimeSpan timeout)
        {
            StackPacket receivedPdu = null;
            bool isReceived = false;
            TimeSpan leftTime = timeout;
            DateTime expiratedTime = DateTime.Now + timeout;

            foreach (StackPacket pdu in pduCache)
            {
                if (pdu is T)
                {
                    isReceived = true;
                    receivedPdu = pdu;
                    pduCache.Remove(pdu);
                    break;
                }
            }

            while (!isReceived && leftTime.CompareTo(new TimeSpan(0)) > 0)
            {
                try
                {
                    receivedPdu = this.rdpbcgrServerStack.ExpectPdu(session, leftTime);
                    if (receivedPdu is T)
                    {
                        isReceived = true;
                        break;
                    }
                    else
                    {
                        pduCache.Add(receivedPdu);
                    }
                }
                catch (TimeoutException)
                {
                    site.Assert.Fail("Timeout when expecting {0}", typeof(T));
                }
                catch (InvalidOperationException ex)
                {
                    //break;
                    site.Log.Add(LogEntryKind.Warning, "Exception thrown out when receiving client PDUs {0}.", ex.Message);
                }
                finally
                {
                    System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                    leftTime = expiratedTime - DateTime.Now;
                }
            }
            if (isReceived)
            {
                ReceiveLoop((StackPacket)receivedPdu);
            }
            else
            {
                site.Assert.Fail("Timeout when expecting {0}.", typeof(T));
            }
        }

        /// <summary>
        /// Wait for a specified type pdu from RDP client, 
        /// if the next received packet is not T, continue to wait until timeout.
        /// </summary>
        /// <typeparam name="T">The expected type of pdu</typeparam>
        /// <param name="timeout">Timeout</param>
        public void WaitForPacket<T>(TimeSpan timeout)
        {
            WaitForPacket<T>(sessionContext, timeout);
        }

        /// <summary>
        /// Wait for a specified type pdu from RDP client, 
        /// if the next received packet is not T, continue to wait until timeout.
        /// </summary>
        /// <typeparam name="T">The expected type of pdu</typeparam>
        /// <param name="timeout">Timeout.</param>
        /// <returns>true if received, otherwise, return false.</returns>
        public bool WaitForClientPacket<T>(TimeSpan timeout)
        {
            StackPacket receivedPdu = null;
            bool isReceived = false;
            TimeSpan leftTime = timeout;
            DateTime expiratedTime = DateTime.Now + timeout;

            foreach (StackPacket pdu in pduCache)
            {
                if (pdu is T)
                {
                    isReceived = true;
                    receivedPdu = pdu;
                    pduCache.Remove(pdu);
                    break;
                }
            }

            while (!isReceived && leftTime.CompareTo(new TimeSpan(0)) > 0)
            {
                try
                {
                    receivedPdu = this.rdpbcgrServerStack.ExpectPdu(sessionContext, leftTime);
                    if (receivedPdu is T)
                    {
                        isReceived = true;
                        break;
                    }
                    else
                    {
                        pduCache.Add(receivedPdu);
                    }
                }
                catch (TimeoutException)
                {
                    return false;
                }
                catch (InvalidOperationException ex)
                {
                    //break;
                    site.Log.Add(LogEntryKind.Warning, "Exception thrown out when receiving client PDUs {0}.", ex.Message);
                }
                finally
                {
                    System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                    leftTime = expiratedTime - DateTime.Now;
                }
            }
            if (isReceived)
            {
                ReceiveLoop((StackPacket)receivedPdu);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Wait for a Slow-Path Input PDU with the specified event type.
        /// </summary>
        /// <param name="expectedEventType">The expected event type.</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>true if received, otherwise, return false.</returns>
        public bool WaitForSlowPathInputPdu(TS_INPUT_EVENT_messageType_Values expectedEventType, TimeSpan timeout)
        {
            StackPacket receivedPdu = null;
            bool isReceived = false;
            TimeSpan leftTime = timeout;
            DateTime expiratedTime = DateTime.Now + timeout;

            foreach (StackPacket pdu in pduCache)
            {
                if (pdu is TS_INPUT_PDU)
                {
                    if (slowPathInputPdu_ContainEvent((TS_INPUT_PDU)pdu, expectedEventType))
                    {
                        isReceived = true;
                        receivedPdu = pdu;
                        pduCache.Remove(pdu);
                        break;
                    }
                }
            }

            while (!isReceived && leftTime.CompareTo(new TimeSpan(0)) > 0)
            {
                try
                {
                    receivedPdu = this.rdpbcgrServerStack.ExpectPdu(sessionContext, leftTime);
                    if (receivedPdu is TS_INPUT_PDU)
                    {
                        if (slowPathInputPdu_ContainEvent((TS_INPUT_PDU)receivedPdu, expectedEventType))
                        {
                            isReceived = true;
                            break;
                        }
                        else
                        {
                            site.Log.Add(LogEntryKind.Debug, "Received and cached Pdu: {0}.", receivedPdu.GetType());
                            pduCache.Add(receivedPdu);
                        }
                    }
                    else
                    {
                        site.Log.Add(LogEntryKind.Debug, "Received and cached Pdu: {0}.", receivedPdu.GetType());
                        pduCache.Add(receivedPdu);
                    }
                }
                catch (TimeoutException)
                {
                    return false;
                }
                catch (InvalidOperationException ex)
                {
                    //break;
                    site.Log.Add(LogEntryKind.Warning, "Exception thrown out when receiving client PDUs {0}.", ex.Message);
                }
                finally
                {
                    System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                    leftTime = expiratedTime - DateTime.Now;
                }
            }
            if (isReceived)
            {
                ReceiveLoop((StackPacket)receivedPdu);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Wait for a Fast-Path Input PDU with the specified event type.
        /// </summary>
        /// <param name="expectedEventType">The expected event type.</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>true if received, otherwise, return false.</returns>
        public bool WaitForFastPathInputPdu(eventCode_Values expectedEventType, TimeSpan timeout)
        {
            StackPacket receivedPdu = null;
            bool isReceived = false;
            TimeSpan leftTime = timeout;
            DateTime expiratedTime = DateTime.Now + timeout;

            foreach (StackPacket pdu in pduCache)
            {
                if (pdu is TS_FP_INPUT_PDU)
                {
                    if (fastPathInputPdu_ContainEvent((TS_FP_INPUT_PDU)pdu, expectedEventType))
                    {
                        isReceived = true;
                        receivedPdu = pdu;
                        pduCache.Remove(pdu);
                        break;
                    }
                }
            }

            while (!isReceived && leftTime.CompareTo(new TimeSpan(0)) > 0)
            {
                try
                {
                    receivedPdu = this.rdpbcgrServerStack.ExpectPdu(sessionContext, leftTime);
                    if (receivedPdu is TS_FP_INPUT_PDU)
                    {
                        if (fastPathInputPdu_ContainEvent((TS_FP_INPUT_PDU)receivedPdu, expectedEventType))
                        {
                            isReceived = true;
                            break;
                        }
                        else
                        {
                            site.Log.Add(LogEntryKind.Debug, "Received and cached Pdu: {0}.", receivedPdu.GetType());
                            pduCache.Add(receivedPdu);
                        }
                    }
                    else
                    {
                        site.Log.Add(LogEntryKind.Debug, "Received and cached Pdu: {0}.", receivedPdu.GetType());
                        pduCache.Add(receivedPdu);
                    }
                }
                catch (TimeoutException)
                {
                    return false;
                }
                catch (InvalidOperationException ex)
                {
                    //break;
                    site.Log.Add(LogEntryKind.Warning, "Exception thrown out when receiving client PDUs {0}.", ex.Message);
                }
                finally
                {
                    System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                    leftTime = expiratedTime - DateTime.Now;
                }
            }
            if (isReceived)
            {
                ReceiveLoop((StackPacket)receivedPdu);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Wait for a Virtual Channel PDU which transported in the specified channel.
        /// </summary>
        /// <param name="channelId">The specified channel id.</param>
        /// <param name="timeout">Timeout.</param>
        public void WaitForVirtualChannelPdu(UInt16 channelId, out byte[] virtualChannelData, TimeSpan timeout)
        {
            StackPacket receivedPdu = null;
            bool isReceived = false;
            TimeSpan leftTime = timeout;
            DateTime expiratedTime = DateTime.Now + timeout;
            virtualChannelData = null;

            foreach (StackPacket pdu in pduCache)
            {
                if (pdu is Virtual_Channel_RAW_Pdu)
                {
                    if (((Virtual_Channel_RAW_Pdu)pdu).commonHeader.channelId == channelId)
                    {
                        isReceived = true;
                        receivedPdu = pdu;
                        pduCache.Remove(pdu);
                        break;
                    }
                }
            }

            while (!isReceived && leftTime.CompareTo(new TimeSpan(0)) > 0)
            {
                try
                {
                    receivedPdu = this.rdpbcgrServerStack.ExpectPdu(sessionContext, leftTime);
                    if (receivedPdu is Virtual_Channel_RAW_Pdu)
                    {
                        if (((Virtual_Channel_RAW_Pdu)receivedPdu).commonHeader.channelId == channelId)
                        {
                            isReceived = true;
                            break;
                        }
                        else
                        {
                            site.Log.Add(LogEntryKind.Debug, "Received and cached Pdu: {0}.", receivedPdu.GetType());
                            pduCache.Add(receivedPdu);
                        }
                    }
                    else
                    {
                        site.Log.Add(LogEntryKind.Debug, "Received and cached Pdu: {0}.", receivedPdu.GetType());
                        pduCache.Add(receivedPdu);
                    }
                }
                catch (TimeoutException)
                {
                    site.Assert.Fail("Timeout when expecting {0} from channel: {1}", typeof(Virtual_Channel_RAW_Pdu), channelId);
                }
                catch (InvalidOperationException ex)
                {
                    //break;
                    site.Log.Add(LogEntryKind.Warning, "Exception thrown out when receiving client PDUs {0}.", ex.Message);
                }
                finally
                {
                    System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                    leftTime = expiratedTime - DateTime.Now;
                }
            }
            if (isReceived)
            {
                virtualChannelData = ((Virtual_Channel_RAW_Pdu)receivedPdu).virtualChannelData;
                ReceiveLoop((StackPacket)receivedPdu);
            }
            else
            {
                site.Assert.Fail("Timeout when expecting {0}.", typeof(Virtual_Channel_RAW_Pdu));
            }
        }

        /// <summary>
        /// Wait for the RDP client terminates the connection.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        public bool WaitForDisconnection(TimeSpan timeout)
        {
            bool isReceived = false;

            try
            {
                this.rdpbcgrServerStack.ExpectDisconnect(sessionContext, timeout);
                isReceived = true;
            }
            catch (InvalidOperationException)
            {
                site.Log.Add(LogEntryKind.Debug, "Underlayer transport throw exception indicates the connection has been terminated.");
                return true;
            }
            catch (TimeoutException timeoutEx)
            {
                site.Log.Add(LogEntryKind.Debug, timeoutEx.Message);
                return false;
            }

            return isReceived;
        }

        /// <summary>
        /// This method is used to process the received PDU.
        /// </summary>
        /// <param name="request">The received PDU</param>
        protected void ReceiveLoop(StackPacket request)
        {
            if (request == null)
                return;

            if (request is Client_X_224_Connection_Request_Pdu)
            {
                this.ReceiveX224ConnectionRequestPdu((Client_X_224_Connection_Request_Pdu)request);
            }
            else if (request is Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request)
            {
                this.ReceiveMcsConnectInitialPdu((Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request)request);
            }
            else if (request is Client_MCS_Erect_Domain_Request)
            {
                ReceiveMcsErectDomainPdu((Client_MCS_Erect_Domain_Request)request);
            }
            else if (request is Client_MCS_Attach_User_Request)
            {
                ReceiveMcsAttachUserPdu((Client_MCS_Attach_User_Request)request);
            }
            else if (request is Client_MCS_Channel_Join_Request)
            {
                ReceiveChannelJoinPdu((Client_MCS_Channel_Join_Request)request);
            }
            else if (request is Client_Security_Exchange_Pdu)
            {
                ReceiveSecurityExchangePdu((Client_Security_Exchange_Pdu)request);
            }
            else if (request is Client_Info_Pdu)
            {
                ReceiveClientInfoPdu((Client_Info_Pdu)request);
            }
            else if (request is Client_Confirm_Active_Pdu)
            {
                ReceiveConfirmActivePdu((Client_Confirm_Active_Pdu)request);
            }
            else if (request is Client_Synchronize_Pdu)
            {
                ReceiveSynchronizePdu((Client_Synchronize_Pdu)request);
            }
            else if (request is Client_Control_Pdu_Cooperate)
            {
                ReceiveControlCooperatePdu((Client_Control_Pdu_Cooperate)request);
            }
            else if (request is Client_Control_Pdu_Request_Control)
            {
                ReceiveControlRequestControlPdu((Client_Control_Pdu_Request_Control)request);
            }
            else if (request is Client_Persistent_Key_List_Pdu)
            {
                ReceivePersistentKeyListPdu((Client_Persistent_Key_List_Pdu)request);
            }
            else if (request is Client_Font_List_Pdu)
            {
                ReceiveFontListPdu((Client_Font_List_Pdu)request);
            }
            else if (request is Client_Shutdown_Request_Pdu)
            {
                ReceiveShutdownPdu((Client_Shutdown_Request_Pdu)request);
            }
            else if (request is MCS_Disconnect_Provider_Ultimatum_Pdu)
            {
                ReceiveMcsDisconnectPdu((MCS_Disconnect_Provider_Ultimatum_Pdu)request);
            }
            else if (request is TS_INPUT_PDU)
            {
                ReceiveInputPdu((TS_INPUT_PDU)request);
            }
            else if (request is TS_FP_INPUT_PDU)
            {
                ReceiveFastPathInputPdu((TS_FP_INPUT_PDU)request);
            }
            else if (request is Client_Refresh_Rect_Pdu)
            {
                ReceiveRefreshRectPdu((Client_Refresh_Rect_Pdu)request);
            }
            else if (request is Client_Suppress_Output_Pdu)
            {
                ReceiveSuppressOutputPdu((Client_Suppress_Output_Pdu)request);
            }
            else if (request is Virtual_Channel_RAW_Pdu)
            {
                ReceiveVirtualChannelPdu((Virtual_Channel_RAW_Pdu)request);
            }
            else if (request is RDSTLS_AuthenticationRequestPDUwithPasswordCredentials)
            {
                ReceivedRDSTLS_AuthenticationRequestPDUwithPasswordCredentials((RDSTLS_AuthenticationRequestPDUwithPasswordCredentials)request);
            }
            else if (request is RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie)
            {
                ReceivedRDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie((RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie)request);
            }

            TS_FRAME_ACKNOWLEDGE_PDU ackPdu = request as TS_FRAME_ACKNOWLEDGE_PDU;
            if (ackPdu != null && TS_FRAME_ACKNOWLEDGE_PDUReceived != null)
            {
                TS_FRAME_ACKNOWLEDGE_PDUReceived(ackPdu);
            }

        }

        /// <summary>
        /// This method is used to send packet to SUT.
        /// </summary>
        /// <param name="packet">The packet to be sent.</param>
        private void SendPdu(RdpbcgrServerPdu packet)
        {
            rdpbcgrServerStack.SendPdu(sessionContext, packet);
            System.Threading.Thread.Sleep(100);//To avoid the combination with other sending request.
        }
        #endregion "PDU transport"

        #region Receive Functions
        private void ReceiveX224ConnectionRequestPdu(Client_X_224_Connection_Request_Pdu request)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(request);
            }

            if (X224ConnectionRequest != null)
            {
                X224ConnectionRequest(request);
            }

            sessionState = ServerSessionState.X224ConnectionRequestReceived;
        }

        private void ReceiveMcsConnectInitialPdu(Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request initialPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(initialPdu);
            }

            if (McsConnectRequest != null)
            {
                McsConnectRequest(initialPdu);
            }
            //Store the index of  RDPDR Channel. 
            RDPDR_Index = -1;
            if (initialPdu.mcsCi != null
                        && initialPdu.mcsCi.gccPdu != null
                        && initialPdu.mcsCi.gccPdu.clientNetworkData != null
                        && initialPdu.mcsCi.gccPdu.clientNetworkData.channelDefArray != null)
            {
                clientRequestedChannelDefList = initialPdu.mcsCi.gccPdu.clientNetworkData.channelDefArray.ToArray();
                if (clientRequestedChannelDefList.Length > 0)
                {
                    for (int i = 0; i < clientRequestedChannelDefList.Length; i++)
                    {
                        if (clientRequestedChannelDefList[i].name.StartsWith("RDPDR", StringComparison.CurrentCultureIgnoreCase))
                        {
                            RDPDR_Index = i;
                            break;
                        }
                    }
                }
            }

            if (initialPdu.mcsCi != null && initialPdu.mcsCi.gccPdu != null && initialPdu.mcsCi.gccPdu.clientCoreData != null)
            {
                serverConfig.CapabilitySetting.DesktopHeight = initialPdu.mcsCi.gccPdu.clientCoreData.desktopHeight;
                serverConfig.CapabilitySetting.DesktopWidth = initialPdu.mcsCi.gccPdu.clientCoreData.desktopWidth;
            }
            sessionState = ServerSessionState.MCSConnectInitialReceived;
        }

        private void ReceiveMcsErectDomainPdu(Client_MCS_Erect_Domain_Request erectPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(erectPdu);
            }

            if (McsErectDomainRequest != null)
            {
                McsErectDomainRequest(erectPdu);
            }

            sessionState = ServerSessionState.MCSErectDomainRequestReceived;
        }

        private void ReceiveMcsAttachUserPdu(Client_MCS_Attach_User_Request attachUserPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(attachUserPdu);
            }

            if (McsAttachUserRequest != null)
            {
                McsAttachUserRequest(attachUserPdu);
            }

            sessionState = ServerSessionState.MCSAttachUserRequestReceived;
        }

        private void ReceiveChannelJoinPdu(Client_MCS_Channel_Join_Request channelJoinPdu)
        {
            lastRequestJoinChannelId = channelJoinPdu.mcsChannelId;

            if (bVerifyRequirements)
            {
                VerifyPdu(channelJoinPdu);
            }

            if (McsChannelJoinRequest != null)
            {
                McsChannelJoinRequest(channelJoinPdu);
            }

            sessionState = ServerSessionState.MCSChannelJoinRequestReceived;
        }

        private void ReceiveSecurityExchangePdu(Client_Security_Exchange_Pdu securityExchangePdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(securityExchangePdu);
            }

            if (SecurityExchangeRequest != null)
            {
                SecurityExchangeRequest(securityExchangePdu);
            }

            sessionState = ServerSessionState.ClientSecurityExchangeReceived;
        }

        private void ReceiveClientInfoPdu(Client_Info_Pdu infoPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(infoPdu);
            }

            if (ClientInfoRequest != null)
            {
                ClientInfoRequest(infoPdu);
            }

            tsInfoPacket = infoPdu.infoPacket;
            sessionState = ServerSessionState.ClientInfoPduReceived;
        }

        private void ReceiveConfirmActivePdu(Client_Confirm_Active_Pdu confirmActivePdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(confirmActivePdu);
            }

            if (ConfirmActiveRequest != null)
            {
                ConfirmActiveRequest(confirmActivePdu);
            }
            clientCapSet = new RdpbcgrCapSet();
            clientCapSet.CapabilitySets = confirmActivePdu.confirmActivePduData.capabilitySets;
            sessionState = ServerSessionState.ClientConfirmActiveReceived;
        }

        private void ReceiveSynchronizePdu(Client_Synchronize_Pdu syncPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(syncPdu);
            }

            if (ClientSyncRequest != null)
            {
                ClientSyncRequest(syncPdu);
            }

            sessionState = ServerSessionState.ClientSynchronizeReceived;
        }

        private void ReceiveControlCooperatePdu(Client_Control_Pdu_Cooperate cooperatePdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(cooperatePdu);
            }

            if (ControlCooperateRequest != null)
            {
                ControlCooperateRequest(cooperatePdu);
            }

            sessionState = ServerSessionState.ClientControlCooperateReceived;
        }

        private void ReceiveControlRequestControlPdu(Client_Control_Pdu_Request_Control requestControlPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(requestControlPdu);
            }

            if (ControlRequestControlRequest != null)
            {
                ControlRequestControlRequest(requestControlPdu);
            }
            sessionState = ServerSessionState.ClientControlRequestControlReceived;
        }

        private void ReceivePersistentKeyListPdu(Client_Persistent_Key_List_Pdu keyListPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(keyListPdu);
            }

            if (PersistentKeyListRequest != null)
            {
                PersistentKeyListRequest(keyListPdu);
            }

            sessionState = ServerSessionState.ClientPersistenKeyListReceived;
        }

        private void ReceiveFontListPdu(Client_Font_List_Pdu fontListPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(fontListPdu);
            }

            if (FontListRequest != null)
            {
                FontListRequest(fontListPdu);
            }

            sessionState = ServerSessionState.ClientFontListReceived;
        }

        private void ReceiveShutdownPdu(Client_Shutdown_Request_Pdu shutdownPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(shutdownPdu);
            }

            if (ShutdownRequest != null)
            {
                ShutdownRequest(shutdownPdu);
            }

        }

        private void ReceiveMcsDisconnectPdu(MCS_Disconnect_Provider_Ultimatum_Pdu disconnectPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(disconnectPdu);
            }

            if (McsDisconnectUltimatumClientRequest != null)
            {
                McsDisconnectUltimatumClientRequest(disconnectPdu);
            }
        }

        private void ReceiveInputPdu(TS_INPUT_PDU inputPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(inputPdu);
            }

            if (InputRequest != null)
            {
                InputRequest(inputPdu);
            }
        }

        private void ReceiveFastPathInputPdu(TS_FP_INPUT_PDU fpInputPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(fpInputPdu);
            }

            if (FastpathInputRequest != null)
            {
                FastpathInputRequest(fpInputPdu);
            }
        }

        private void ReceiveRefreshRectPdu(Client_Refresh_Rect_Pdu refreshPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(refreshPdu);
            }

            if (RefreshRectRequest != null)
            {
                RefreshRectRequest(refreshPdu);
            }
        }

        private void ReceiveSuppressOutputPdu(Client_Suppress_Output_Pdu suppressPdu)
        {
            if (bVerifyRequirements)
            {
                VerifyPdu(suppressPdu);
            }

            if (SuppressOutputRequest != null)
            {
                SuppressOutputRequest(suppressPdu);
            }
        }

        private void ReceiveVirtualChannelPdu(Virtual_Channel_RAW_Pdu vcPdu)
        {
            if (VirtualChannelRequest != null)
            {
                VirtualChannelRequest(vcPdu);
            }
        }

        private void ReceivedRDSTLS_AuthenticationRequestPDUwithPasswordCredentials(RDSTLS_AuthenticationRequestPDUwithPasswordCredentials pdu)
        {
            if (RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsReceived != null)
            {
                RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsReceived(pdu);
            }
        }

        private void ReceivedRDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie(RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie pdu)
        {
            if (RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieReceived != null)
            {
                RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieReceived(pdu);
            }
        }

        #endregion "Receive Functions"

        #region Private mothods

        private void LoadServerConfiguation()
        {
            serverConfig = new RdpbcgrServerConfig();
            port = int.Parse(site.Properties["RDP.ServerPort"]);
            if (site.Properties["RDP.IpVersion"] == "Ipv6")
            {
                ipVersion = IpVersion.Ipv6;
            }
            else
            {
                ipVersion = IpVersion.Ipv4;
            }
            certFile = site.Properties["CertificatePath"];
            certPwd = site.Properties["CertificatePassword"];
            rdpVersionString = site.Properties["RDP.Version"];

            #region WaitTime
            string strWaitTime = site.Properties["WaitTime"];
            if (strWaitTime != null)
            {
                int waitSeconds = Int32.Parse(strWaitTime);
                pduWaitTimeSpan = new TimeSpan(0, 0, waitSeconds);
            }
            #endregion

            string strIsClientToServerEncrypted = site.Properties["RDP.Security.IsClientToServerEncrypted"];
            if (strIsClientToServerEncrypted != null)
            {
                isClientToServerEncrypted = Boolean.Parse(strIsClientToServerEncrypted);
            }

            string strIsWindows = site.Properties["IsWindowsImplementation"];
            if (strIsWindows != null)
            {
                isWindowsImplementation = Boolean.Parse(strIsWindows);
            }

            string strVerifySUTDisplay = site.Properties["VerifySUTDisplay.Enable"];
            if (strVerifySUTDisplay != null)
            {
                verifySUTDisplay = Boolean.Parse(strVerifySUTDisplay);
            }

            string strVerifySUTDisplayAssessValueThreshold = site.Properties["VerifySUTDisplay.IQA.AssessValueThreshold"];

            if (Double.TryParse(strVerifySUTDisplayAssessValueThreshold, out this.IQAAssessValueThreshold))
            {
                if (this.IQAAssessValueThreshold < 0 || this.IQAAssessValueThreshold > 1)
                {
                    this.Site.Assert.Fail("VerifySUTDisplay.IQA.AssessValueThreshold should be in [0, 1]. The current value in config is: {0}.", this.IQAAssessValueThreshold);
                }
            }
            else
            {
                this.Site.Assert.Fail("VerifySUTDisplay.IQA.AssessValueThreshold is not in digital format.");
            }

            string strVerifySUTDisplayIQAAlgorithm = site.Properties["VerifySUTDisplay.IQA.Algorithm"];
            if (strVerifySUTDisplayIQAAlgorithm != null && strVerifySUTDisplayIQAAlgorithm.Equals("MS-SSIM"))
            {
                this.IQAAlgorithm = IQA_Algorithm.MSSSIM;
            }
            else if (strVerifySUTDisplayIQAAlgorithm != null && strVerifySUTDisplayIQAAlgorithm.Equals("G-SSIM"))
            {
                this.IQAAlgorithm = IQA_Algorithm.GSSIM;
            }
            else
            {
                this.IQAAlgorithm = IQA_Algorithm.SSIM;
            }

            //Update test data
            string tempStr;
            if (PtfPropUtility.GetStringPtfProperty(site, "RDP.ServerDomain", out tempStr))
            {
                RdpbcgrTestData.Test_Domain = tempStr;
            }
            if (PtfPropUtility.GetStringPtfProperty(site, "RDP.ServerUserName", out tempStr))
            {
                RdpbcgrTestData.Test_UserName = tempStr;
            }
            if (PtfPropUtility.GetStringPtfProperty(site, "RDP.ServerUserPassword", out tempStr))
            {
                RdpbcgrTestData.Test_Password = tempStr;
            }
            if (PtfPropUtility.GetStringPtfProperty(site, "SUTRedirectionGuid", out tempStr))
            {
                RdpbcgrTestData.Test_RedirectionGuid = tempStr;
            }
            if (PtfPropUtility.GetStringPtfProperty(site, "SUTFullQualifiedDomainName", out tempStr))
            {
                RdpbcgrTestData.Test_FullQualifiedDomainName = tempStr;
            }
            if (PtfPropUtility.GetStringPtfProperty(site, "SUTNetBiosName", out tempStr))
            {
                RdpbcgrTestData.Test_NetBiosName = tempStr;
            }
        }

        private bool slowPathInputPdu_ContainEvent(TS_INPUT_PDU inputPdu, TS_INPUT_EVENT_messageType_Values eventType)
        {
            if (inputPdu == null || inputPdu.numberEvents == 0) return false;
            Collection<TS_INPUT_EVENT> inputEvents = inputPdu.slowPathInputEvents;
            foreach (TS_INPUT_EVENT inputEvent in inputEvents)
            {
                site.Log.Add(LogEntryKind.Debug, "Received Slow Path Event: {0}", inputEvent.messageType);
                if (inputEvent.messageType == eventType) return true;
            }
            return false;
        }

        private bool fastPathInputPdu_ContainEvent(TS_FP_INPUT_PDU inputPdu, eventCode_Values expectedEventType)
        {
            if (inputPdu == null) return false;
            Collection<TS_FP_INPUT_EVENT> inputEvents = inputPdu.fpInputEvents;
            foreach (TS_FP_INPUT_EVENT inputEvent in inputEvents)
            {
                uint eventCode = (uint)inputEvent.eventHeader.eventFlagsAndCode >> 5;
                if (eventCode == (uint)expectedEventType) return true;
            }
            return false;
        }

        #endregion
    }
}
