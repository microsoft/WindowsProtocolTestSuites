// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Maintain the important parameters of the session during RDPBCGR transport, 
    /// including the main sent or received PDUs, Channel Manager, the selected Encryption Algorithm etc.
    /// </summary>
    public class RdpbcgrServerSessionContext : IDisposable
    {
        #region Field Members
        #region PDU store    
        // In Client_X_224_Connection_Request_Pdu
        private Client_X_224_Connection_Request_Pdu x224ConnectionRequestPdu;

        // In Server_X_224_Connection_Confirm_Pdu
        private RDP_NEG_RSP x224ConnectionConfirmPdu;

        // In Server_X_224_Negotiate_Failure_Pdu
        private RDP_NEG_FAILURE x224NegotiateFailurePdu;

        // In Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request
        private MCSConnectInitial mcsConnectInitialPdu;

        // In MCS Connect Response PDU with GCC Conference Create Response
        private Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsConnectResponsePdu;

        // In MCS Attach User Confirm PDU
        private ushort userChannelId;
        private ushort serverChannelId;

        // 32 byte Server Random Number, in 
        // Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response Pdu
        private ushort ioChannelId;
        private ushort mcsMsgChannelId;
        private byte[] serverRandom;
        private byte[] clientRandom;
        private Client_Security_Exchange_Pdu securityExchangePdu;

        // In Client Info Pdu
        private TS_INFO_PACKET clientInfo;

        //In Server Auto-Detect Request PDU
        private Dictionary<ushort, NETWORK_DETECTION_REQUEST> serverAutoDetectRequestData;

        // Buffer for unprocessed packets
        private List<StackPacket> unprocessedPacketBuffer;

        //In Client Auto-Detect Response PDU
        private Client_Auto_Detect_Response_PDU clientAutoDetectResponsePdu;

        //In Server Initiate Multitransport Request PDU
        private Dictionary<uint, Server_Initiate_Multitransport_Request_PDU> serverInitiateMultitransportRequestPduDictionary;

        //In Client Initiate Multitransport Response PDU
        private Client_Initiate_Multitransport_Response_PDU clientInitiateMultitransportResponsePdu;

        //Auto-detect result
        private List<uint> autoDetectedRTTList;

        private uint autoDetectedBandwidth = 102400;

        // In Server_License_Error_Pdu_Valid_Client
        private Server_License_Error_Pdu_Valid_Client licenseErrorPdu;

        // In Server_Demand_Active_Pdu
        private TS_DEMAND_ACTIVE_PDU demandActivePdu;

        // In Client_Confirm_Active_Pdu
        private TS_CONFIRM_ACTIVE_PDU comfirmActivePdu;

        // In MCS_Disconnect_Provider_Ultimatum_Pdu
        private int lastDisconnectReason;

        // In Server_Set_Error_Info_Pdu
        private errorInfo_Values lastErrorInfo;

        // In Server_Status_Info_Pdu
        private StatusCode_Values lastStatusInfo;

        // In Server_Save_Session_Info_Pdu
        private TS_LOGON_INFO logonInfoV1;
        private TS_LOGON_INFO_VERSION_2 logonInfoV2;
        private ARC_SC_PRIVATE_PACKET autoReconnectCookie;
        private TS_LOGON_ERRORS_INFO logonErrorsInfo;
        #endregion PDU store

        #region Private members
        private RdpbcgrServer server;
        private object identity;
        private object localIdentity;
        private byte[] privateExponent;
        private uint pduCountToUpdate;
        private uint encryptionCount;
        private uint decryptionCount;
        private object contextLock;
        private ServerStaticVirtualChannelManager channelManager;
        private EncryptionAlgorithm encryptionAlgorithm;
        private bool isWaitingLicenseErrorPdu;
        private byte[] x509ServerExponent;
        private byte[] x509ServerModulus;

        /// <summary>
        /// Context should be updated by the switch configuration. 
        /// True for updating context automatically.
        /// False for not updating context but wait for upper layer TSD to do so.
        /// </summary>
        private bool isSwitchOn;

        /// <summary>
        /// Used to decompress slow-path or fast-path output update PDUs.
        /// </summary> 
        private Decompressor ioDecompressor;

        private Compressor ioCompressor;

        private Queue<UInt16> virtualChannelIdFactory;

        private bool isClientToServerEncrypted = true;
        private bool isAuthenticatingRDSTLS;
        #endregion
        #endregion


        #region Constructor
        /// <summary>
        /// Perform all operation and store all the info for the RDPBCGR server session context managing.
        /// </summary>
        public RdpbcgrServerSessionContext()
        {
            contextLock = new object();
            ClearAll();
            pduCountToUpdate = ConstValue.PDU_COUNT_TO_UPDATE_SESSION_KEY;
            isSwitchOn = true;
            virtualChannelIdFactory = new Queue<ushort>();
            serverAutoDetectRequestData = new Dictionary<ushort, NETWORK_DETECTION_REQUEST>();
            unprocessedPacketBuffer = new List<StackPacket>();
            serverInitiateMultitransportRequestPduDictionary = new Dictionary<uint, Server_Initiate_Multitransport_Request_PDU>();
            autoDetectedRTTList = new List<uint>();
            serverChannelId = ConstValue.SERVER_CHANNEL_ID;

            virtualChannelIdFactory.Enqueue(ConstValue.RDPDR_CHANNEL_ID);
            virtualChannelIdFactory.Enqueue(ConstValue.CLIPRDR_CHANNEL_ID);
            virtualChannelIdFactory.Enqueue(ConstValue.RDPSND_CHANNEL_ID);

            // Store channel ids that maybe used by subsequent rdp requirement.
            for (UInt16 channelId = ConstValue.RDP_STORED_ID; channelId <= ConstValue.MAX_CHANNLE_ID; ++channelId)
            {
                virtualChannelIdFactory.Enqueue(channelId);
            }

            isAuthenticatingRDSTLS = false;
        }
        #endregion constructor


        #region Properties
        /// <summary>
        /// The owner this context belongs to.
        /// </summary>
        public RdpbcgrServer Server
        {
            get
            {
                return server;
            }

            set
            {
                server = value;
            }
        }

        /// <summary>
        /// Static virtual channel Manager
        /// </summary>
        public ServerStaticVirtualChannelManager SVCManager
        {
            get
            {
                return channelManager;
            }
        }

        public Client_X_224_Connection_Request_Pdu X224ConnectionRequestPdu
        {
            get
            {
                return x224ConnectionRequestPdu;
            }
        }

        /// <summary>
        /// The endpoint of the session
        /// </summary>
        public object Identity
        {
            get
            {
                return this.identity;
            }
            set
            {
                this.identity = value;
            }
        }

        /// <summary>
        /// The local endpoint of the session
        /// </summary>
        public object LocalIdentity
        {
            get
            {
                return this.localIdentity;
            }
            set
            {
                this.localIdentity = value;
            }
        }

        /// <summary>
        /// Context should be updated by the switch configuration. 
        /// True for updating context automatically.
        /// False for not updating context but wait for upper layer TSD to do so.
        /// </summary>
        public bool IsSwitchOn
        {
            get
            {
                lock (contextLock)
                {
                    return isSwitchOn;
                }
            }
            set
            {
                lock (contextLock)
                {
                    isSwitchOn = value;
                }
            }
        }

        /// <summary>
        /// Whether the client-to-server message is encrypted
        /// </summary> 
        public bool IsClientToServerEncrypted
        {
            get
            {
                return isClientToServerEncrypted;
            }

            set
            {
                this.isClientToServerEncrypted = value;
            }
        }

        /// <summary>
        /// Whether ConnectInitialPdu contain clientMessageChannelData field
        /// </summary>
        public bool IsClientMessageChannelDataRecieved
        {
            get
            {
                if (this.mcsConnectInitialPdu.gccPdu.clientMessageChannelData == null)
                    return false;
                else
                    return true;
            }

        }

        /// <summary>
        /// Whether ConnectResponse contain a ServerMessageChannelData
        /// </summary>
        public bool IsServerMessageChannelDataSend
        {
            get
            {
                if (this.mcsConnectResponsePdu.mcsCrsp.gccPdu.serverMessageChannelData == null)
                    return false;
                else
                    return true;
            }
        }
        /// <summary>
        /// Whether ConnectInitialPdu contain clientMultitransportChannelData field
        /// </summary>
        public bool IsClientMultitransportChannelDataRecieved
        {
            get
            {
                if (this.mcsConnectInitialPdu.gccPdu.clientMultitransportChannelData == null)
                    return false;
                else
                    return true;
            }

        }
        /// <summary>
        /// An id queue contains available ids.
        /// </summary>
        public Queue<UInt16> VirtualChannelIdFactory
        {
            get
            {
                return virtualChannelIdFactory;
            }
        }


        /// <summary>
        /// Set a PDU count. After sending or receiving that count of PDUs, 
        /// the encryption and the decryption keys are updated.
        /// The default value is 4096.
        /// </summary> 
        public uint PduCountofUpdateSessionKey
        {
            get
            {
                lock (contextLock)
                {
                    return pduCountToUpdate;
                }
            }
            set
            {
                lock (contextLock)
                {
                    pduCountToUpdate = value;
                }
            }
        }


        /// <summary>
        /// 32-byte Client Random Number can be generated by RdpbcgrUtility.GenerateRandom().
        /// It is used to generate the session key.
        /// </summary>
        public byte[] ClientRandomNumber
        {
            get
            {
                lock (contextLock)
                {
                    return RdpbcgrUtility.CloneByteArray(clientRandom);
                }
            }
            set
            {
                lock (contextLock)
                {
                    clientRandom = RdpbcgrUtility.CloneByteArray(value);
                }
            }
        }


        /// <summary>
        /// 32-byte Server Random Number got from server MCS Connect Response PDU 
        /// with GCC Conference Create Response used to generate the session key.
        /// </summary>
        public byte[] ServerRandomNumber
        {
            get
            {
                lock (contextLock)
                {
                    return serverRandom;
                }
            }
            set
            {
                lock (contextLock)
                {
                    serverRandom = RdpbcgrUtility.CloneByteArray(value);
                }
            }
        }


        /// <summary>
        /// Server exponent got from server MCS Connect Response PDU with GCC Conference 
        /// Create Response used to encrypt client random.
        /// User can set this value for x509 certificate.
        /// </summary>
        public byte[] ServerExponent
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectResponsePdu != null
                        && mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate != null)
                    {
                        if (mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.dwVersion ==
                            SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1)
                        {
                            return BitConverter.GetBytes(((PROPRIETARYSERVERCERTIFICATE)
                                (mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.certData))
                                .PublicKeyBlob.pubExp);
                        }
                        else if (mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.dwVersion ==
                            SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_2)
                        {
                            return RdpbcgrUtility.CloneByteArray(x509ServerExponent);
                        }
                        // no else
                    }

                    return null;
                }
            }

            set
            {
                lock (contextLock)
                {
                    x509ServerExponent = RdpbcgrUtility.CloneByteArray(value);
                }
            }
        }


        /// <summary>
        /// Server private exponent used to decrypt client random.
        /// </summary>
        public byte[] ServerPrivateExponent
        {
            get
            {
                lock (contextLock)
                {
                    return privateExponent;
                }
            }

            set
            {
                lock (contextLock)
                {
                    privateExponent = RdpbcgrUtility.CloneByteArray(value);
                }
            }
        }


        /// <summary>
        /// Server modulus got from server MCS Connect Response PDU with GCC Conference 
        /// Create Response used to encrypt client random.
        /// User can set this value for x509 certificate.
        /// </summary>
        public byte[] ServerModulus
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectResponsePdu != null
                        && mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate != null)
                    {
                        if (mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.dwVersion ==
                            SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1)
                        {
                            PROPRIETARYSERVERCERTIFICATE cert = (PROPRIETARYSERVERCERTIFICATE)
                                mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.certData;
                            byte[] tempModulus = RdpbcgrUtility.CloneByteArray(cert.PublicKeyBlob.modulus);
                            Array.Resize<byte>(ref tempModulus, tempModulus.Length - 8);//remove the 8 bytes padding
                            return tempModulus;
                        }
                        else if (mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.dwVersion ==
                            SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_2)
                        {
                            return RdpbcgrUtility.CloneByteArray(x509ServerModulus);
                        }
                        // no else
                    }

                    return null;
                }
            }

            set
            {
                lock (contextLock)
                {
                    x509ServerModulus = RdpbcgrUtility.CloneByteArray(value);
                    PROPRIETARYSERVERCERTIFICATE cert = (PROPRIETARYSERVERCERTIFICATE)
                                mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.certData;
                    cert.PublicKeyBlob.modulus = RdpbcgrUtility.CloneByteArray(value);
                    mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.certData = cert;
                }
            }
        }


        /// <summary>
        /// Generated according to section 5.3.5 Initial Session Key Generation.
        /// Used to decrypted I/O data stream.
        /// </summary>
        public byte[] ClientDecryptKey
        {
            get
            {
                lock (contextLock)
                {
                    if (encryptionAlgorithm != null)
                    {
                        return RdpbcgrUtility.CloneByteArray(encryptionAlgorithm.CurrentClientDecryptKey);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// Generated according to section 5.3.5 Initial Session Key Generation.
        /// Used to encrypted I/O data stream.
        /// </summary>
        public byte[] ClientEncryptKey
        {
            get
            {
                lock (contextLock)
                {
                    if (encryptionAlgorithm != null)
                    {
                        return RdpbcgrUtility.CloneByteArray(encryptionAlgorithm.CurrentClientEncryptKey);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// The X224 connection request PDU received from client.
        /// </summary>
        public Client_X_224_Connection_Request_Pdu X224ConnectionReqPdu
        {
            get
            {
                lock (contextLock)
                {
                    if (x224ConnectionRequestPdu == null)
                    {
                        return null;
                    }
                    else
                    {
                        return x224ConnectionRequestPdu;
                    }
                }
            }
        }


        /// <summary>
        /// The requested protocol from client, used to determine the encryption method.
        /// </summary>
        public requestedProtocols_Values ClientRequestedProtocol
        {
            get
            {
                lock (contextLock)
                {
                    if (x224ConnectionConfirmPdu == null || x224ConnectionRequestPdu.rdpNegData == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return x224ConnectionRequestPdu.rdpNegData.requestedProtocols;
                    }
                }
            }

            set
            {
                if (x224ConnectionRequestPdu == null)
                {
                    x224ConnectionRequestPdu = new Client_X_224_Connection_Request_Pdu();
                    x224ConnectionRequestPdu.rdpNegData = new RDP_NEG_REQ();
                    x224ConnectionRequestPdu.rdpNegData.requestedProtocols = value;
                }
                else
                {
                    x224ConnectionRequestPdu.rdpNegData.requestedProtocols = value;
                }
            }
        }


        /// <summary>
        /// The requested protocol server selected in x224 confirm PDU.
        /// </summary>
        public uint ServerSelectedProtocol
        {
            get
            {
                lock (contextLock)
                {
                    if (x224ConnectionConfirmPdu == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return (uint)x224ConnectionConfirmPdu.selectedProtocol;
                    }
                }
            }
        }


        /// <summary>
        /// The requested protocol to be used for following PDUs.
        /// Got from server MCS Connect Response PDU with GCC Conference.
        /// </summary>
        public requestedProtocols_Values RequestedProtocol
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectResponsePdu != null)
                    {
                        return mcsConnectResponsePdu.mcsCrsp.gccPdu.serverCoreData.clientRequestedProtocols;
                    }
                    else if (x224ConnectionRequestPdu != null && x224ConnectionRequestPdu.rdpNegData != null)
                    {
                        return x224ConnectionRequestPdu.rdpNegData.requestedProtocols;
                    }
                    else
                    {
                        return requestedProtocols_Values.PROTOCOL_RDP_FLAG;
                    }
                }
            }
        }

        /// <summary>
        /// Get the flags of client MCS connect intial PDU.
        /// </summary>
        public MULTITRANSPORT_TYPE_FLAGS MultitransportTypeFlagsInMCSConnectIntialPdu
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientMultitransportChannelData.flags;
                    }
                    else
                    {
                        return MULTITRANSPORT_TYPE_FLAGS.None;
                    }
                }
            }
        }

        /// <summary>
        /// The failure code of x224 connection.
        /// Got from Server X.224 Connection RDP Negotiation Failure PDU.
        /// </summary>
        public failureCode_Values FailureCode
        {
            get
            {
                lock (contextLock)
                {
                    if (x224NegotiateFailurePdu != null)
                    {
                        return x224NegotiateFailurePdu.failureCode;
                    }
                    else
                    {
                        return failureCode_Values.NO_FAILURE;
                    }
                }
            }
        }


        /// <summary>
        /// The clientName got from Client MCS Connect Initial PDU with GCC Conference Create Request.
        /// </summary>
        public string ClientName
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                        && mcsConnectInitialPdu.gccPdu != null
                        && mcsConnectInitialPdu.gccPdu.clientCoreData != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientCoreData.clientName;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// The clientBuild got from Client MCS Connect Initial PDU with GCC Conference Create Request.
        /// </summary>
        public uint ClientBuild
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                        && mcsConnectInitialPdu.gccPdu != null
                        && mcsConnectInitialPdu.gccPdu.clientCoreData != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientCoreData.clientBuild;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }


        /// <summary>
        /// The supportedColorDepths got from Client MCS Connect Initial PDU with GCC Conference Create Request.
        /// </summary>
        public UInt16Class supportedColorDepths
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                        && mcsConnectInitialPdu.gccPdu != null
                        && mcsConnectInitialPdu.gccPdu.clientCoreData != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientCoreData.supportedColorDepths;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// The clientDigProductId got from Client MCS Connect Initial PDU with GCC Conference Create Request.
        /// </summary>
        public string ClientDigProductId
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                        && mcsConnectInitialPdu.gccPdu != null
                        && mcsConnectInitialPdu.gccPdu.clientCoreData != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientCoreData.clientDigProductId;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// The connectionType got from Client MCS Connect Initial PDU with GCC Conference Create Request.
        /// </summary>
        public ByteClass ClientNetworkConnectionType
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                        && mcsConnectInitialPdu.gccPdu != null
                        && mcsConnectInitialPdu.gccPdu.clientCoreData != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientCoreData.connnectionType;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public UInt16Class ClientEarlyCapabilityFlags
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                       && mcsConnectInitialPdu.gccPdu != null
                       && mcsConnectInitialPdu.gccPdu.clientCoreData != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientCoreData.earlyCapabilityFlags;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public UInt16 DesktopWidth
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                       && mcsConnectInitialPdu.gccPdu != null
                       && mcsConnectInitialPdu.gccPdu.clientCoreData != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientCoreData.desktopWidth;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public UInt16 DesktopHeight
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                       && mcsConnectInitialPdu.gccPdu != null
                       && mcsConnectInitialPdu.gccPdu.clientCoreData != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientCoreData.desktopHeight;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        /// <summary>
        /// The encryption method selected by the server.
        /// Got from server MCS Connect Response PDU with GCC Conference.
        /// </summary>
        public EncryptionMethods RdpEncryptionMethod
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectResponsePdu != null)
                    {
                        return mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.encryptionMethod;
                    }
                    else
                    {
                        return EncryptionMethods.ENCRYPTION_METHOD_NONE;
                    }
                }
            }
        }


        /// <summary>
        /// The encryption level selected by the server.
        /// Got from server MCS Connect Response PDU with GCC Conference.
        /// </summary>
        public EncryptionLevel RdpEncryptionLevel
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectResponsePdu != null)
                    {
                        return mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.encryptionLevel;
                    }
                    else
                    {
                        return EncryptionLevel.ENCRYPTION_LEVEL_NONE;
                    }
                }
            }
        }


        /// <summary>
        /// The encryption method selected by the server.
        /// Got from server MCS Connect Response PDU with GCC Conference.
        /// </summary>
        public EncryptionMethods ClientEncryptionMethod
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null)
                    {
                        return (EncryptionMethods)mcsConnectInitialPdu.gccPdu.clientSecurityData.encryptionMethods;
                    }
                    else
                    {
                        return EncryptionMethods.ENCRYPTION_METHOD_NONE;
                    }
                }
            }
        }


        /// <summary>
        /// The user channel Id got from MCS Attach User Confirm PDU.
        /// </summary>
        public ushort UserChannelId
        {
            get
            {
                lock (contextLock)
                {
                    return userChannelId;
                }
            }
            set
            {
                lock (contextLock)
                {
                    userChannelId = value;
                }
            }
        }


        /// <summary>
        /// The default sever channel id.
        /// </summary>
        public ushort ServerChannelId
        {
            get
            {
                lock (contextLock)
                {
                    return serverChannelId;
                }
            }

            set
            {
                serverChannelId = value;
            }
        }


        /// <summary>
        /// The IO channel Id got from server MCS Connect Response PDU with GCC Conference.
        /// </summary>
        public ushort IOChannelId
        {
            get
            {
                lock (contextLock)
                {
                    return ioChannelId;
                }
            }
        }

        /// <summary>
        /// The Mcs Message channel Id got from server MCS Connect Response PDU with GCC Conference.
        /// </summary>
        public ushort McsMsgChannelId
        {
            get
            {
                lock (contextLock)
                {
                    return mcsMsgChannelId;
                }
            }
        }


        /// <summary>
        /// The security exchange PDU from client contains security info.
        /// </summary>
        public Client_Security_Exchange_Pdu SecurityExchangePdu
        {
            get
            {
                lock (contextLock)
                {
                    return this.securityExchangePdu;
                }
            }
        }


        /// <summary>
        /// Error code of license pdu got from Server License Error PDU.
        /// </summary>
        public dwErrorCode_Values LicenseErrorCode
        {
            get
            {
                lock (contextLock)
                {
                    if (licenseErrorPdu != null && licenseErrorPdu.preamble.bMsgType == bMsgType_Values.ERROR_ALERT)
                    {
                        return licenseErrorPdu.validClientMessage.dwErrorCode;
                    }
                    else
                    {
                        return dwErrorCode_Values.ERR_NO_ERROR_CODE;
                    }
                }
            }
        }


        /// <summary>
        /// The share identifier for the packet got from Demand Active PDU.
        /// </summary>
        public uint ShareId
        {
            get
            {
                lock (contextLock)
                {
                    if (demandActivePdu != null)
                    {
                        return demandActivePdu.shareId;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }


        /// <summary>
        /// The virtual channel Ids allocated, got from server MCS Connect Response PDU with GCC Conference.
        /// </summary>
        public ushort[] VirtualChannelIdStore
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectResponsePdu != null
                        && mcsConnectResponsePdu.mcsCrsp.gccPdu.serverNetworkData.channelIdArray != null)
                    {
                        return (ushort[])mcsConnectResponsePdu.mcsCrsp.gccPdu.serverNetworkData.channelIdArray.Clone();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// The virtual channel names and options, got from Client MCS Connect Initial Pdu with 
        /// GCC Conference Create Request.
        /// </summary>
        public CHANNEL_DEF[] VirtualChannelDefines
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                        && mcsConnectInitialPdu.gccPdu != null
                        && mcsConnectInitialPdu.gccPdu.clientNetworkData != null
                        && mcsConnectInitialPdu.gccPdu.clientNetworkData.channelDefArray != null)
                    {
                        return (CHANNEL_DEF[])
                            mcsConnectInitialPdu.gccPdu.clientNetworkData.channelDefArray.ToArray().Clone();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// The virtual channel defines list
        /// </summary>
        public List<CHANNEL_DEF> VirtualChannelDefinesList
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu != null
                        && mcsConnectInitialPdu.gccPdu != null
                        && mcsConnectInitialPdu.gccPdu.clientNetworkData != null
                        && mcsConnectInitialPdu.gccPdu.clientNetworkData.channelDefArray != null)
                    {
                        return mcsConnectInitialPdu.gccPdu.clientNetworkData.channelDefArray;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            set
            {
                lock (contextLock)
                {
                    if (mcsConnectInitialPdu == null)
                    {
                        mcsConnectInitialPdu = new MCSConnectInitial();
                        mcsConnectInitialPdu.gccPdu.clientNetworkData.channelDefArray = value;
                    }
                }
            }
        }


        /// <summary>
        /// Indicates the highest compression package type supported.
        /// Got from Client Info PDU.
        /// </summary>
        public CompressionType CompressionTypeSupported
        {
            get
            {
                lock (contextLock)
                {
                    if (clientInfo != null
                        && ((clientInfo.flags & flags_Values.INFO_COMPRESSION) == flags_Values.INFO_COMPRESSION))
                    {
                        // CompressionTypeMask 0x00001E00 in the clientInfo.flags
                        // So >> 9 bits to get the mask
                        return (CompressionType)((uint)(clientInfo.flags & flags_Values.CompressionTypeMask) >> 9);
                    }

                    return CompressionType.PACKET_COMPR_TYPE_NONE;
                }
            }
        }


        /// <summary>
        /// The maximum allowed size of a virtual channel chunk.
        /// Got from Demand Active PDU.
        /// </summary>
        public UInt32 VCChunkSize
        {
            get
            {
                lock (contextLock)
                {
                    //look for type CAPSTYPE_VIRTUALCHANNEL in demandActivePdu.capabilitySets
                    //default is CHANNEL_CHUNK_LENGTH
                    if (demandActivePdu != null && demandActivePdu.capabilitySets != null)
                    {
                        foreach (ITsCapsSet capSet in demandActivePdu.capabilitySets)
                        {
                            if (capSet.GetCapabilityType() == capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL)
                            {
                                TS_VIRTUALCHANNEL_CAPABILITYSET channelCapSet =
                                    (TS_VIRTUALCHANNEL_CAPABILITYSET)capSet;
                                if (channelCapSet.VCChunkSize != 0)
                                {
                                    return channelCapSet.VCChunkSize;
                                }
                            }
                        }
                    }

                    return ConstValue.CHANNEL_CHUNK_LENGTH_DEFAULT;
                }
            }
        }


        /// <summary>
        /// The compression type client-to-server virtual channels support.
        /// Got from Demand Active PDU.
        /// </summary>
        public CompressionType VirtualChannelCSCompressionType
        {
            get
            {
                lock (contextLock)
                {
                    //look for type CAPSTYPE_VIRTUALCHANNEL in demandActivePdu.capabilitySets
                    //default is CHANNEL_CHUNK_LENGTH
                    if (demandActivePdu != null && demandActivePdu.capabilitySets != null)
                    {
                        foreach (ITsCapsSet capSet in demandActivePdu.capabilitySets)
                        {
                            if (capSet.GetCapabilityType() == capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL)
                            {
                                TS_VIRTUALCHANNEL_CAPABILITYSET channelCapSet =
                                    (TS_VIRTUALCHANNEL_CAPABILITYSET)capSet;
                                if ((channelCapSet.flags
                                    & TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_COMPR_CS_8K)
                                    == TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_COMPR_CS_8K)
                                {
                                    return CompressionType.PACKET_COMPR_TYPE_8K;
                                }
                            }
                        }
                    }

                    return CompressionType.PACKET_COMPR_TYPE_NONE;
                }
            }
        }


        /// <summary>
        /// The compression type server-to-client virtual channels support.
        /// Got from Confirm Active Pdu.
        /// </summary>
        public CompressionType VirtualChannelSCCompressionType
        {
            get
            {
                lock (contextLock)
                {
                    //look for type CAPSTYPE_VIRTUALCHANNEL in comfirmActivePdu.capabilitySets
                    //default is CHANNEL_CHUNK_LENGTH
                    if (comfirmActivePdu != null && comfirmActivePdu.capabilitySets != null)
                    {
                        foreach (ITsCapsSet capSet in comfirmActivePdu.capabilitySets)
                        {
                            if (capSet.GetCapabilityType() == capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL)
                            {
                                TS_VIRTUALCHANNEL_CAPABILITYSET channelCapSet =
                                    (TS_VIRTUALCHANNEL_CAPABILITYSET)capSet;
                                if ((channelCapSet.flags
                                    & TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_COMPR_SC)
                                    == TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_COMPR_SC)
                                {
                                    return CompressionTypeSupported;
                                }
                            }
                        }
                    }

                    return CompressionType.PACKET_COMPR_TYPE_NONE;
                }
            }
        }

        /// <summary>
        /// ColorPointerCacheSize in TS_POINTER_CAPABILITYSET of comfirmActivePdu
        /// </summary>
        public ushort? ColorPointerCacheSize
        {
            get
            {
                lock (contextLock)
                {
                    //look for type TS_POINTER_CAPABILITYSET in comfirmActivePdu.capabilitySets
                    if (comfirmActivePdu != null && comfirmActivePdu.capabilitySets != null)
                    {
                        foreach (ITsCapsSet capSet in comfirmActivePdu.capabilitySets)
                        {
                            if (capSet.GetCapabilityType() == capabilitySetType_Values.CAPSTYPE_POINTER)
                            {
                                TS_POINTER_CAPABILITYSET pointerCapSet =
                                    (TS_POINTER_CAPABILITYSET)capSet;
                                return pointerCapSet.colorPointerCacheSize;
                            }
                        }
                    }

                    return null;
                }
            }
        }

        /// <summary>
        /// PointerCacheSize in TS_POINTER_CAPABILITYSET of comfirmActivePdu
        /// </summary>
        public ushort? PointerCacheSize
        {
            get
            {
                lock (contextLock)
                {
                    //look for type TS_POINTER_CAPABILITYSET in comfirmActivePdu.capabilitySets
                    if (comfirmActivePdu != null && comfirmActivePdu.capabilitySets != null)
                    {
                        foreach (ITsCapsSet capSet in comfirmActivePdu.capabilitySets)
                        {
                            if (capSet.GetCapabilityType() == capabilitySetType_Values.CAPSTYPE_POINTER)
                            {
                                TS_POINTER_CAPABILITYSET pointerCapSet =
                                    (TS_POINTER_CAPABILITYSET)capSet;
                                return pointerCapSet.pointerCacheSize;
                            }
                        }
                    }

                    return null;
                }
            }
        }

        /// <summary>
        /// Whether the RDP client support Fast-path Output 
        /// </summary>
        public bool IsFastPathOutputSupported
        {
            get
            {
                lock (contextLock)
                {
                    //look for type TS_POINTER_CAPABILITYSET in comfirmActivePdu.capabilitySets
                    if (comfirmActivePdu != null && comfirmActivePdu.capabilitySets != null)
                    {
                        foreach (ITsCapsSet capSet in comfirmActivePdu.capabilitySets)
                        {
                            if (capSet.GetCapabilityType() == capabilitySetType_Values.CAPSTYPE_GENERAL)
                            {
                                TS_GENERAL_CAPABILITYSET generalCapSet =
                                    (TS_GENERAL_CAPABILITYSET)capSet;
                                if (generalCapSet.extraFlags.HasFlag(extraFlags_Values.FASTPATH_OUTPUT_SUPPORTED))
                                    return true;
                                return false;
                            }
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// The Capability Sets got from Confirm Active PDU.
        /// </summary>
        public Collection<ITsCapsSet> ConfirmCapabilitySets
        {
            get
            {
                lock (contextLock)
                {
                    if (comfirmActivePdu != null && comfirmActivePdu.capabilitySets != null)
                    {
                        TS_CONFIRM_ACTIVE_PDU clonePdu = comfirmActivePdu.Clone();
                        return clonePdu.capabilitySets;
                    }

                    return null;
                }
            }
        }

        /// <summary>
        /// To indicate if is waiting to recieve a license error PDU.
        /// </summary>
        internal bool IsWaitingLicenseErrorPdu
        {
            get
            {
                lock (contextLock)
                {
                    return isWaitingLicenseErrorPdu;
                }
            }
            set
            {
                lock (contextLock)
                {
                    isWaitingLicenseErrorPdu = value;
                }
            }
        }


        /// <summary>
        /// The LogonId field of auto reconnect cookie which is got from Save Session Info PDU.
        /// </summary>
        public UInt32 LogonId
        {
            get
            {
                lock (contextLock)
                {
                    if (autoReconnectCookie != null)
                    {
                        return autoReconnectCookie.LogonId;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }


        /// <summary>
        /// The ArcRandomBits field of auto reconnect cookie which is got from Save Session Info PDU.
        /// </summary>
        public byte[] ArcRandomBits
        {
            get
            {
                lock (contextLock)
                {
                    if (autoReconnectCookie != null)
                    {
                        return RdpbcgrUtility.CloneByteArray(autoReconnectCookie.ArcRandomBits);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// The encrypted PDU number.
        /// </summary>
        public uint EncryptionCount
        {
            get
            {
                lock (contextLock)
                {
                    return encryptionCount;
                }
            }
        }


        /// <summary>
        /// The decrypted PDU number.
        /// </summary>
        public uint DecryptionCount
        {
            get
            {
                lock (contextLock)
                {
                    return decryptionCount;
                }
            }
        }


        /// <summary>
        /// The reason of last disconnection got from MCS Disconnect Provider Ultimatum PDU.
        /// </summary>
        public int LastDisconnectReason
        {
            get
            {
                lock (contextLock)
                {
                    return lastDisconnectReason;
                }
            }
        }


        /// <summary>
        /// The error information of last Server Set Error Info PDU.
        /// </summary>
        public errorInfo_Values LastErrorInfo
        {
            get
            {
                lock (contextLock)
                {
                    return lastErrorInfo;
                }
            }
        }


        /// <summary>
        /// The status information of last Server Status Info PDU.
        /// </summary>
        public StatusCode_Values LastStatusInfo
        {
            get
            {
                lock (contextLock)
                {
                    return lastStatusInfo;
                }
            }
        }


        /// <summary>
        /// The logon information of version 1 got from Server Save Session Info PDU.
        /// </summary>
        public TS_LOGON_INFO LogonInfoV1
        {
            get
            {
                lock (contextLock)
                {
                    return logonInfoV1;
                }
            }
        }


        /// <summary>
        /// The logon information of version 2 got from Server Save Session Info PDU.
        /// </summary>
        public TS_LOGON_INFO_VERSION_2 LogonInfoV2
        {
            get
            {
                lock (contextLock)
                {
                    TS_LOGON_INFO_VERSION_2 logonInfo = logonInfoV2;
                    logonInfo.Pad = RdpbcgrUtility.CloneByteArray(logonInfoV2.Pad);
                    return logonInfo;
                }
            }
        }


        /// <summary>
        /// The logon error notification got from Server Save Session Info PDU.
        /// </summary>
        public TS_LOGON_ERRORS_INFO LogonErrorNotification
        {
            get
            {
                lock (contextLock)
                {
                    if (logonErrorsInfo != null)
                    {
                        TS_LOGON_ERRORS_INFO errorInfo = new TS_LOGON_ERRORS_INFO();
                        errorInfo.ErrorNotificationData = logonErrorsInfo.ErrorNotificationData;
                        errorInfo.ErrorNotificationType = logonErrorsInfo.ErrorNotificationType;
                        return errorInfo;
                    }

                    return null;
                }
            }
        }

        /// <summary>
        /// Get the AutoDetectRspPduData in the latest Client Auto-Detect Response PDU
        /// </summary>
        public NETWORK_DETECTION_RESPONSE AutoDetectRspPduData
        {
            get
            {
                lock (contextLock)
                {
                    if (clientAutoDetectResponsePdu != null)
                    {
                        return clientAutoDetectResponsePdu.autodetectRspPduData.Clone();
                    }

                    return null;
                }
            }
        }

        /// <summary>
        /// Get the detected network characteristic: BaseRTT
        /// </summary>
        public uint AutoDetectBaseRTT
        {
            get
            {
                lock (contextLock)
                {
                    if (this.autoDetectedRTTList != null && autoDetectedRTTList.Count != 0)
                    {
                        uint baseRtt = 0;
                        foreach (uint rtt in autoDetectedRTTList)
                        {
                            if (rtt > baseRtt)
                                baseRtt = rtt;
                        }
                        return baseRtt;
                    }
                    return 0;
                }
            }
        }

        /// <summary>
        /// Get the detected network characteristic: AutoDetectAverageRTT
        /// </summary>
        public uint AutoDetectAverageRTT
        {
            get
            {
                lock (contextLock)
                {
                    if (this.autoDetectedRTTList != null && autoDetectedRTTList.Count != 0)
                    {
                        uint totalRtt = 0;
                        foreach (uint rtt in autoDetectedRTTList)
                        {
                            totalRtt += rtt;
                        }

                        return (uint)(totalRtt / autoDetectedRTTList.Count);
                    }
                    return 0;
                }
            }
        }

        /// <summary>
        /// Get the detected network characteristic: BandWith
        /// </summary>
        public uint AutoDetectBandWith
        {
            get
            {
                lock (contextLock)
                {
                    return this.autoDetectedBandwidth;
                }
            }
        }

        /// <summary>
        /// Get the Client Initiate Multitransport Response PDU
        /// </summary>
        public Client_Initiate_Multitransport_Response_PDU ClientInitiateMultitransportResponsePDU
        {
            get
            {
                lock (contextLock)
                {
                    return this.clientInitiateMultitransportResponsePdu;
                }
            }
        }

        /// <summary>
        /// Indicating whether the RDSTLS authentication is ongoing.
        /// </summary>
        public bool IsAuthenticatingRDSTLS
        {
            get
            {
                lock (contextLock)
                {
                    return isAuthenticatingRDSTLS;
                }

            }
            set
            {
                lock (contextLock)
                {
                    isAuthenticatingRDSTLS = value;
                }
            }
        }

        #endregion public properties


        #region Methods
        /// <summary>
        /// Update some members of context with the specified pdu.
        /// </summary>
        /// <param name="pdu">The sending or receiving pdu.</param>
        public void UpdateContext(StackPacket pdu)
        {
            lock (contextLock)
            {
                if (!isSwitchOn)
                {
                    // Don't update context but wait for upper layer TSD to do so.
                    return;
                }

                if (pdu.GetType() == typeof(Client_X_224_Connection_Request_Pdu))
                {
                    x224ConnectionRequestPdu = (Client_X_224_Connection_Request_Pdu)pdu.Clone();
                }
                else if (pdu.GetType() == typeof(Server_X_224_Connection_Confirm_Pdu))
                {
                    x224ConnectionConfirmPdu = ((Server_X_224_Connection_Confirm_Pdu)pdu.Clone()).rdpNegData;
                }
                else if (pdu.GetType() == typeof(Server_X_224_Negotiate_Failure_Pdu))
                {
                    x224NegotiateFailurePdu = ((Server_X_224_Negotiate_Failure_Pdu)pdu.Clone()).rdpNegFailure;
                }
                else if (pdu.GetType() == typeof(Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request))
                {
                    mcsConnectInitialPdu = ((Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request)
                        pdu.Clone()).mcsCi;
                }
                else if (pdu is Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response)
                {
                    mcsConnectResponsePdu = (Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response)
                        pdu.Clone();
                    serverRandom = RdpbcgrUtility.CloneByteArray(mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverRandom);
                    ioChannelId = mcsConnectResponsePdu.mcsCrsp.gccPdu.serverNetworkData.MCSChannelId;
                    if (mcsConnectResponsePdu.mcsCrsp.gccPdu.serverMessageChannelData != null)
                        mcsMsgChannelId = mcsConnectResponsePdu.mcsCrsp.gccPdu.serverMessageChannelData.MCSChannelID;
                    encryptionAlgorithm = new EncryptionAlgorithm(RdpEncryptionMethod);
                }
                else if (pdu.GetType() == typeof(Server_MCS_Attach_User_Confirm_Pdu))
                {
                    if (((Server_MCS_Attach_User_Confirm_Pdu)pdu).attachUserConfirm.initiator != null)
                    {
                        userChannelId = (ushort)((Server_MCS_Attach_User_Confirm_Pdu)pdu).attachUserConfirm.initiator.Value;
                    }
                }
                else if (pdu.GetType() == typeof(Client_Security_Exchange_Pdu))
                {
                    securityExchangePdu = (Client_Security_Exchange_Pdu)pdu.Clone();
                    clientRandom = RdpbcgrUtility.CloneByteArray(
                        ((Client_Security_Exchange_Pdu)pdu).securityExchangePduData.clientRandom);
                    GenerateSessionKey();
                }
                else if (pdu.GetType() == typeof(Client_Info_Pdu))
                {
                    clientInfo = ((Client_Info_Pdu)pdu.Clone()).infoPacket;
                    if (clientInfo != null
                        && (clientInfo.flags & flags_Values.INFO_COMPRESSION) == flags_Values.INFO_COMPRESSION)
                    {
                        ioDecompressor = new Decompressor(SlidingWindowSize.EightKB);
                        ioCompressor = new Compressor(SlidingWindowSize.EightKB);
                    }
                    isWaitingLicenseErrorPdu = true;
                }
                else if (pdu.GetType() == typeof(Server_Auto_Detect_Request_PDU))
                {

                    NETWORK_DETECTION_REQUEST requestData = ((Server_Auto_Detect_Request_PDU)pdu).autoDetectReqData.Clone();
                    if (requestData.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_IN_CONNECTTIME || requestData.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_AFTER_CONNECTTIME)
                    {
                        RDP_RTT_REQUEST rttRequest = (RDP_RTT_REQUEST)requestData;
                        rttRequest.sendTime = DateTime.Now;
                        this.serverAutoDetectRequestData.Add(rttRequest.sequenceNumber, rttRequest);
                    }
                }
                else if (pdu.GetType() == typeof(Client_Auto_Detect_Response_PDU))
                {
                    clientAutoDetectResponsePdu = (Client_Auto_Detect_Response_PDU)pdu.Clone();
                    NETWORK_DETECTION_RESPONSE responseData = clientAutoDetectResponsePdu.autodetectRspPduData;
                    if (responseData.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_RTT_RESPONSE)
                    {
                        RDP_RTT_REQUEST rttRequest = (RDP_RTT_REQUEST)serverAutoDetectRequestData[responseData.sequenceNumber];
                        if (rttRequest != null)
                        {
                            TimeSpan interval = DateTime.Now - rttRequest.sendTime;
                            this.autoDetectedRTTList.Add((uint)interval.TotalMilliseconds);
                            serverAutoDetectRequestData.Remove(responseData.sequenceNumber);
                        }
                    }
                    else if (responseData.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT || responseData.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_DURING_CONNECT)
                    {
                        RDP_BW_RESULTS bwResult = (RDP_BW_RESULTS)responseData;
                        if (bwResult.timeDelta != 0)
                            this.autoDetectedBandwidth = (uint)(bwResult.byteCount / bwResult.timeDelta);
                    }
                    else if (responseData.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_NETCHAR_SYNC)
                    {
                        RDP_NETCHAR_SYNC netSync = (RDP_NETCHAR_SYNC)responseData;
                        this.autoDetectedRTTList.Add(netSync.rtt);
                        this.autoDetectedBandwidth = netSync.bandwidth;
                    }
                }
                else if (pdu.GetType() == typeof(Server_License_Error_Pdu_Valid_Client))
                {
                    licenseErrorPdu = (Server_License_Error_Pdu_Valid_Client)pdu.Clone();
                    serverChannelId = licenseErrorPdu.commonHeader.initiator;
                }
                else if (pdu.GetType() == typeof(Server_Initiate_Multitransport_Request_PDU))
                {
                    Server_Initiate_Multitransport_Request_PDU requestPDU = (Server_Initiate_Multitransport_Request_PDU)pdu;
                    serverInitiateMultitransportRequestPduDictionary.Add(requestPDU.requestId, requestPDU);
                }
                else if (pdu.GetType() == typeof(Client_Initiate_Multitransport_Response_PDU))
                {
                    clientInitiateMultitransportResponsePdu = (Client_Initiate_Multitransport_Response_PDU)pdu;
                }
                else if (pdu.GetType() == typeof(Server_Demand_Active_Pdu))
                {
                    serverChannelId = ((Server_Demand_Active_Pdu)pdu.Clone()).commonHeader.initiator;
                    demandActivePdu = ((Server_Demand_Active_Pdu)pdu.Clone()).demandActivePduData;

                }
                else if (pdu.GetType() == typeof(Client_Confirm_Active_Pdu))
                {
                    comfirmActivePdu = ((Client_Confirm_Active_Pdu)pdu.Clone()).confirmActivePduData;
                    if (channelManager == null)
                    {
                        channelManager = new ServerStaticVirtualChannelManager(this);
                    }
                }
                else if (pdu.GetType() == typeof(MCS_Disconnect_Provider_Ultimatum_Server_Pdu))
                {
                    lastDisconnectReason = (int)((MCS_Disconnect_Provider_Ultimatum_Server_Pdu)
                        pdu).disconnectProvider.reason.Value;
                }
                else if (pdu.GetType() == typeof(MCS_Disconnect_Provider_Ultimatum_Pdu))
                {
                    lastDisconnectReason = (int)((MCS_Disconnect_Provider_Ultimatum_Pdu)
                        pdu).disconnectProvider.reason.Value;
                }
                else if (pdu.GetType() == typeof(Server_Set_Error_Info_Pdu))
                {
                    lastErrorInfo = ((Server_Set_Error_Info_Pdu)pdu).errorInfoPduData.errorInfo;
                }
                else if (pdu.GetType() == typeof(Server_Status_Info_Pdu))
                {
                    lastStatusInfo = ((Server_Status_Info_Pdu)pdu).statusCode;
                }
                else if (pdu.GetType() == typeof(Server_Save_Session_Info_Pdu))
                {
                    Server_Save_Session_Info_Pdu saveSessionInfoPdu = (Server_Save_Session_Info_Pdu)pdu.Clone();

                    switch (saveSessionInfoPdu.saveSessionInfoPduData.infoType)
                    {
                        case infoType_Values.INFOTYPE_LOGON:
                            logonInfoV1 = (TS_LOGON_INFO)saveSessionInfoPdu.saveSessionInfoPduData.infoData;
                            break;

                        case infoType_Values.INFOTYPE_LOGON_LONG:
                            logonInfoV2 = (TS_LOGON_INFO_VERSION_2)saveSessionInfoPdu.saveSessionInfoPduData.infoData;
                            break;

                        case infoType_Values.INFOTYPE_LOGON_EXTENDED_INF:
                            TS_LOGON_INFO_EXTENDED infoExtended =
                                (TS_LOGON_INFO_EXTENDED)saveSessionInfoPdu.saveSessionInfoPduData.infoData;
                            switch (infoExtended.FieldsPresent)
                            {
                                case FieldsPresent_Values.LOGON_EX_AUTORECONNECTCOOKIE
                                   | FieldsPresent_Values.LOGON_EX_LOGONERRORS:
                                    autoReconnectCookie = (ARC_SC_PRIVATE_PACKET)infoExtended.LogonFields[0].FieldData;
                                    logonErrorsInfo = (TS_LOGON_ERRORS_INFO)infoExtended.LogonFields[1].FieldData;
                                    break;

                                case FieldsPresent_Values.LOGON_EX_AUTORECONNECTCOOKIE:
                                    autoReconnectCookie = (ARC_SC_PRIVATE_PACKET)infoExtended.LogonFields[0].FieldData;
                                    break;

                                case FieldsPresent_Values.LOGON_EX_LOGONERRORS:
                                    logonErrorsInfo = (TS_LOGON_ERRORS_INFO)infoExtended.LogonFields[0].FieldData;
                                    break;

                                default:
                                    break;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Add a unprocessed PDU into buffer of this RDP session
        /// </summary>
        /// <param name="pdu"></param>
        public void AddPacketToBuffer(StackPacket pdu)
        {
            lock (unprocessedPacketBuffer)
            {
                unprocessedPacketBuffer.Add(pdu);
            }

        }

        /// <summary>
        /// Get a unprocessed packet from buffer
        /// </summary>
        /// <param name="isSVCPacket">Whether need a SVC packet</param>
        /// <returns></returns>
        public StackPacket GetPacketFromBuffer(bool onlySVCPacket = false)
        {
            if (unprocessedPacketBuffer.Count > 0)
            {
                lock (unprocessedPacketBuffer)
                {
                    if (unprocessedPacketBuffer.Count > 0)
                    {
                        if (onlySVCPacket)
                        {
                            for (int i = 0; i < unprocessedPacketBuffer.Count; i++)
                            {
                                if (unprocessedPacketBuffer[i] is Virtual_Channel_RAW_Pdu
                                        || unprocessedPacketBuffer[i] is ErrorPdu
                                        || unprocessedPacketBuffer[i] is MCS_Disconnect_Provider_Ultimatum_Pdu)
                                {
                                    // if the packet is ErrorPdu or MCS_Disconnect_Provider_Ultimatum_Pdu, there's some error in the connection
                                    // should return this two types of PDU, so as to notify the error
                                    StackPacket pdu = unprocessedPacketBuffer[i];
                                    unprocessedPacketBuffer.RemoveAt(i);
                                    return pdu;
                                }
                            }
                        }
                        else
                        {
                            StackPacket pdu = unprocessedPacketBuffer[0];
                            unprocessedPacketBuffer.RemoveAt(0);
                            return pdu;
                        }
                    }

                }
            }
            return null;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (this.SVCManager != null)
            {
                this.SVCManager.Dispose();
            }
        }

        /// <summary>
        /// Clear all the members.
        /// </summary>
        internal void ClearAll()
        {
            ClearForReconnect();

            lock (contextLock)
            {
                autoReconnectCookie = null;
                clientRandom = null;
                serverRandom = null;
                lastDisconnectReason = 0;
                lastErrorInfo = errorInfo_Values.ERRINFO_NONE;
                lastStatusInfo = StatusCode_Values.TS_STATUS_NO_STATUS;
                pduCountToUpdate = ConstValue.PDU_COUNT_TO_UPDATE_SESSION_KEY;
                identity = null;
            }
        }


        /// <summary>
        /// Clear all the members except the reconnection relative members.
        /// </summary>
        internal void ClearForReconnect()
        {
            lock (contextLock)
            {
                x224ConnectionRequestPdu = null;
                x224ConnectionConfirmPdu = null;
                x224NegotiateFailurePdu = null;
                mcsConnectInitialPdu = null;
                mcsConnectResponsePdu = null;
                userChannelId = 0;
                ioChannelId = 0;
                serverChannelId = 0;
                clientInfo = null;
                securityExchangePdu = null;
                licenseErrorPdu = null;
                demandActivePdu = null;
                comfirmActivePdu = null;
                logonInfoV1 = new TS_LOGON_INFO();
                logonInfoV2 = new TS_LOGON_INFO_VERSION_2();
                logonErrorsInfo = null;
                encryptionCount = 0;
                decryptionCount = 0;

                if (encryptionAlgorithm != null)
                {
                    encryptionAlgorithm.Dispose();
                    encryptionAlgorithm = null;
                }

                if (ioDecompressor != null)
                {
                    ioDecompressor.Dispose();
                    ioDecompressor = null;
                }

                if (ioCompressor != null)
                {
                    ioCompressor.Dispose();
                    ioCompressor = null;
                }

                isAuthenticatingRDSTLS = false;
            }
        }


        /// <summary>
        /// Update session key for encryption by the member field encryptionAlgorithm.
        /// </summary>
        internal void UpdateSessionKey()
        {
            lock (contextLock)
            {
                encryptionAlgorithm.UpdateSessionKey();
                encryptionCount = 0;
                decryptionCount = 0;
            }
        }


        /// <summary>
        /// Encrypt data and generate data signature by using the member field encryptionAlgorithm.
        /// </summary>
        /// <param name="originalData">Data to be encrypted. This argument can be null.</param>
        /// <param name="isSalted">Specify if data signature generated with salted MAC.</param>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="dataSignature">The generated data signature.</param>
        internal void ServerEncrypt(byte[] originalData, bool isSalted, out byte[] encryptedData, out byte[] dataSignature)
        {
            lock (contextLock)
            {
                dataSignature = encryptionAlgorithm.GenerateDataSignature(originalData, isSalted);
                encryptedData = encryptionAlgorithm.ServerEncrypt(originalData);
                encryptionCount++;
            }
        }


        /// <summary>
        /// Decrypt data and validate the data signature by using the member field encryptionAlgorithm.
        /// </summary>
        /// <param name="originalData">Data to be decrypted. This argument can be null.</param>
        /// <param name="dataSignature">The data signature to be validated. This argument can be null.</param>
        /// <param name="isSalted">Specify if data signature generated with salted MAC.</param>
        /// <param name="decryptedData">The decrypted data.</param>
        /// <returns>Whether the data signature is valid.</returns>
        internal bool ServerDecrypt(byte[] originalData, byte[] dataSignature, bool isSalted, out byte[] decryptedData)
        {
            lock (contextLock)
            {
                decryptedData = encryptionAlgorithm.ServerDecrypt(originalData);
                decryptionCount++;

                return encryptionAlgorithm.ValidateDataSignature(dataSignature, decryptedData, isSalted);
            }
        }


        /// <summary>
        /// Generate session key for encryption by using the member field encryptionAlgorithm.
        /// </summary>
        internal void GenerateSessionKey()
        {
            lock (contextLock)
            {
                encryptionAlgorithm.GenerateSessionKey(clientRandom, ServerRandomNumber);
            }
        }


        /// <summary>
        /// Split complete virtual data into chunks by the member field channelManager.
        /// This method is called when sending Virtual Channel PDU.
        /// </summary>
        /// <param name="channelId">The channel id. If the channel id is invalid, 
        /// then the return value is null.</param>
        /// <param name="virtualChannelData">The complete channel data. This argument can be null.</param>
        /// <returns>The chunk data.</returns>
        internal ChannelChunk[] SplitToChunks(long channelId, byte[] virtualChannelData)
        {
            lock (contextLock)
            {
                return channelManager.SplitToChunks((UInt16)channelId, virtualChannelData);
            }
        }


        /// <summary>
        /// Reassemble chunk data by the member field channelManager.
        /// This method is called when processing Virtual Channel PDU.
        /// </summary>
        /// <param name="pdu">The channel pdu includes header and data. This argument can be null.</param>
        /// <returns>If the reassemble is complete, then return the Virtual_Channel_Complete_Pdu.
        /// Else return null.</returns>
        internal Virtual_Channel_Complete_Pdu ReassembleChunkData(Virtual_Channel_RAW_Pdu pdu)
        {
            lock (contextLock)
            {
                return channelManager.ReassembleChunkData(pdu);
            }
        }

        /// <summary>
        /// Decompress payload of slow path or fast path output.
        /// </summary>
        /// <param name="data">The data to be decompressed.</param>
        /// <param name="type">The type to decompress.</param>
        /// <returns>The decompressed data.</returns>
        internal byte[] Decompress(compressedType_Values type, byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            lock (contextLock)
            {
                CompressMode flag = CompressMode.None;

                if ((type & compressedType_Values.PACKET_AT_FRONT) == compressedType_Values.PACKET_AT_FRONT)
                {
                    flag |= CompressMode.SetToFront;
                }

                if ((type & compressedType_Values.PACKET_COMPRESSED) == compressedType_Values.PACKET_COMPRESSED)
                {
                    flag |= CompressMode.Compressed;
                }

                if ((type & compressedType_Values.PACKET_FLUSHED) == compressedType_Values.PACKET_FLUSHED)
                {
                    flag |= CompressMode.Flush;
                }

                if (ioDecompressor != null && flag != CompressMode.None)
                {
                    return ioDecompressor.Decompress(data, flag);
                }

                // no compression
                return data;
            }
        }


        /// <summary>
        /// Compress payload of slow path or fast path output.
        /// </summary>
        /// <param name="data">The data to be compressed.</param>
        /// <param name="type">The type to compress.</param>
        /// <returns>The compressed data.</returns>
        internal byte[] Compress(compressedType_Values type, byte[] data, int maxBit = 16)
        {
            if (data == null)
            {
                return null;
            }

            lock (contextLock)
            {
                CompressMode flag = CompressMode.None;

                if ((type & compressedType_Values.PACKET_AT_FRONT) == compressedType_Values.PACKET_AT_FRONT)
                {
                    flag |= CompressMode.SetToFront;
                }

                if ((type & compressedType_Values.PACKET_COMPRESSED) == compressedType_Values.PACKET_COMPRESSED)
                {
                    flag |= CompressMode.Compressed;
                }

                if ((type & compressedType_Values.PACKET_FLUSHED) == compressedType_Values.PACKET_FLUSHED)
                {
                    flag |= CompressMode.Flush;
                }

                if (ioCompressor != null && flag != CompressMode.None)
                {
                    return ioCompressor.Compress(data, out flag);
                }

                // no compression
                return data;
            }
        }

        #endregion
    }
}
