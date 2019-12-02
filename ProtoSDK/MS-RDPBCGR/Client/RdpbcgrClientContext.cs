// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Maintain the important parameters during RDPBCGR transport, 
    /// including the main sent or received PDUs, Channel Manager, the selected Encryption Algorithm etc.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
        "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class RdpbcgrClientContext : IDisposable
    {
        #region members
        #region PDU store
        /// <summary>
        /// In Client_X_224_Connection_Request_Pdu
        /// </summary>
        private Client_X_224_Connection_Request_Pdu x224ConnectionRequestPdu;

        /// <summary>
        /// In Server_X_224_Connection_Confirm_Pdu
        /// </summary>
        private RDP_NEG_RSP x224ConnectionConfirmPdu;

        /// <summary>
        /// In Server_X_224_Negotiate_Failure_Pdu
        /// </summary>
        private RDP_NEG_FAILURE x224NegotiateFailurePdu;

        /// <summary>
        /// In Early_User_Authorization_Result_PDU.
        /// </summary>
        private Early_User_Authorization_Result_PDU earlyUserAuthorizationResultPDU;

        /// <summary>
        /// In Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request
        /// </summary>
        private MCSConnectInitial mcsConnectInitialPdu;

        /// <summary>
        /// In MCS Connect Response PDU with GCC Conference Create Response
        /// </summary>
        private Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsConnectResponsePdu;

        /// <summary>
        /// In MCS Attach User Confirm PDU
        /// </summary>
        private long userChannelId;

        /// <summary>
        /// 32 byte Client Random Number, in Security Exchange PDU
        /// </summary>
        private byte[] clientRandom;

        /// <summary>
        /// In Client Info PDU
        /// </summary>
        private TS_INFO_PACKET clientInfo;

        /// <summary>
        /// In Server_License_Error_PDU_Valid_Client
        /// </summary>
        private Server_License_Error_Pdu_Valid_Client licenseErrorPdu;

        /// <summary>
        /// In Server_Demand_Active_PDU
        /// </summary>
        private TS_DEMAND_ACTIVE_PDU demandActivePdu;

        /// <summary>
        /// In Client_Confirm_Active_PDU
        /// </summary>
        private TS_CONFIRM_ACTIVE_PDU comfirmActivePdu;

        /// <summary>
        /// In MCS_Disconnect_Provider_Ultimatum_PDU
        /// </summary>
        private int lastDisconnectReason;

        /// <summary>
        /// In Server_Set_Error_Info_PDU
        /// </summary>
        private errorInfo_Values lastErrorInfo;

        /// <summary>
        /// In Server_Status_Info_PDU
        /// </summary>
        private StatusCode_Values lastStatusInfo;

        /// <summary>
        /// In Server_Save_Session_Info_PDU
        /// </summary>
        private TS_LOGON_INFO logonInfoV1;
        private TS_LOGON_INFO_VERSION_2 logonInfoV2;
        private ARC_SC_PRIVATE_PACKET autoReconnectCookie;
        private TS_LOGON_ERRORS_INFO logonErrorsInfo;
        #endregion PDU store

        #region private members
        private uint pduCountToUpdate;
        private uint encryptionCount;
        private uint decryptionCount;
        private bool isWaitingLicenseErrorPdu;
        private object contextLock;
        private ClientStaticVirtualChannelManager channelManager;
        private EncryptionAlgorithm encryptionAlgorithm;
        private byte[] publicExponent;
        private byte[] modulus;
        private object remoteIdentity;
        private object localIdentity;

        private uint requestIdReliable;
        private uint requestIdLossy;
        private byte[] cookieReliable;
        private byte[] cookieLossy;

        // Buffer for unprocessed packets
        private List<StackPacket> unprocessedPacketBuffer;

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

        private RdpbcgrClient client;
        private bool isAuthenticatingRDSTLS;
        private bool isExpectingEarlyUserAuthorizationResultPDU;

        #endregion private members

        #region properties
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
        /// Indicating whether RDSTLS authentication is ongoing.
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

        /// <summary>
        /// Indicating whether the client is expecting Early User Authorization Result PDU.
        /// </summary>
        public bool IsExpectingEarlyUserAuthorizationResultPDU
        {
            get
            {
                lock (contextLock)
                {
                    return isExpectingEarlyUserAuthorizationResultPDU;
                }
            }
            set
            {
                lock (contextLock)
                {
                    isExpectingEarlyUserAuthorizationResultPDU = value;
                }
            }
        }


        /// <summary>
        /// Static virtual channel Manager
        /// </summary>
        public ClientStaticVirtualChannelManager SVCManager
        {
            get
            {
                return channelManager;
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
                    if (mcsConnectResponsePdu != null)
                    {
                        return RdpbcgrUtility.CloneByteArray(mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverRandom);
                    }
                    else
                    {
                        return null;
                    }
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
                    if (publicExponent != null)
                    {
                        return publicExponent;
                    }

                    if (mcsConnectResponsePdu != null
                        && mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate != null)
                    {
                        RdpbcgrDecoder.DecodePubicKey(
                            mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate,
                            out publicExponent, out modulus);
                        return publicExponent;
                        // no else
                    }

                    return null;
                }
            }
            set
            {
                lock (contextLock)
                {
                    publicExponent = RdpbcgrUtility.CloneByteArray(value);
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
                    if (modulus != null)
                    {
                        return modulus;
                    }

                    if (mcsConnectResponsePdu != null
                        && mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate != null)
                    {
                        RdpbcgrDecoder.DecodePubicKey(
                            mcsConnectResponsePdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate,
                            out publicExponent, out modulus);
                        return modulus;
                        // no else
                    }

                    return null;
                }
            }
            set
            {
                lock (contextLock)
                {
                    modulus = RdpbcgrUtility.CloneByteArray(value);
                }
            }
        }

        /// <summary>
        /// Generated according to section 5.3.5 Initial Session Key Generation.
        /// Used to decrypt I/O data stream.
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
        /// Used to encrypt I/O data stream.
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
        /// The user channel Id got from MCS Attach User Confirm PDU.
        /// </summary>
        public long UserChannelId
        {
            get
            {
                lock (contextLock)
                {
                    return userChannelId;
                }
            }
        }

        public long ServerChannelId
        {
            get
            {
                lock (contextLock)
                {
                    if (demandActivePdu != null)
                    {
                        return demandActivePdu.shareControlHeader.pduSource;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// The IO channel Id got from server MCS Connect Response PDU with GCC Conference.
        /// </summary>
        public long IOChannelId
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectResponsePdu != null)
                    {
                        return mcsConnectResponsePdu.mcsCrsp.gccPdu.serverNetworkData.MCSChannelId;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// The ID of the MCS channel which transports the Multitransport Bootstrapping PDUs (sections 2.2.15.1 and 2.2.15.2) and Network Characteristics Detection PDUs (sections 2.2.14.3 and 2.2.14.4).
        /// </summary>
        public long? MessageChannelId
        {
            get
            {
                lock (contextLock)
                {
                    if (mcsConnectResponsePdu != null && mcsConnectResponsePdu.mcsCrsp.gccPdu.serverMessageChannelData != null)
                    {
                        return mcsConnectResponsePdu.mcsCrsp.gccPdu.serverMessageChannelData.MCSChannelID;
                    }
                    else
                    {
                        return null;
                    }
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
        /// The virtual channel names and options, got from Client MCS Connect Initial PDU with 
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

        public bool IsBitmapCacheHostSupport
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
                            if (capSet.GetCapabilityType() == capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_HOSTSUPPORT)
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// The compression type server-to-client virtual channels support.
        /// Got from Confirm Active PDU.
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
        /// The Capability Sets got from Demand Active PDU.
        /// </summary>
        public Collection<ITsCapsSet> demandActivemCapabilitySets
        {
            get
            {
                lock (contextLock)
                {
                    if (demandActivePdu != null && demandActivePdu.capabilitySets != null)
                    {
                        return demandActivePdu.capabilitySets;
                    }

                    return null;
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
        /// The endpoint of the session
        /// </summary>
        public object RemoteIdentity
        {
            get
            {
                return this.remoteIdentity;
            }
            set
            {
                this.remoteIdentity = value;
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
        /// RequestId for create reliable multitransport channel
        /// </summary>
        public uint RequestIdReliable
        {
            get
            {
                return requestIdReliable;
            }
        }

        /// <summary>
        /// RequestId for create lossy multitransport channel
        /// </summary>
        public uint RequestIdLossy
        {
            get
            {
                return requestIdLossy;
            }
        }

        /// <summary>
        /// Security Cookie for create reliable multitransport channel
        /// </summary>
        public byte[] CookieReliable
        {
            get
            {
                return cookieReliable;
            }
        }

        /// <summary>
        /// Security Cookie for create lossy multitransport channel
        /// </summary>
        public byte[] CookieLossy
        {
            get
            {
                return cookieLossy;
            }
        }

        /// <summary>
        /// RDPBGR Client
        /// </summary>
        public RdpbcgrClient Client
        {
            get
            {
                return client;
            }
        }

        #endregion public properties
        #endregion members

        #region constructor
        /// <summary>
        /// Create a RdpbcgrClientContext to store important data.
        /// </summary>
        public RdpbcgrClientContext(RdpbcgrClient bcgrClient)
        {
            contextLock = new object();
            ClearAll();
            pduCountToUpdate = ConstValue.PDU_COUNT_TO_UPDATE_SESSION_KEY;
            this.client = bcgrClient;
            isSwitchOn = true;
            unprocessedPacketBuffer = new List<StackPacket>(); ;
        }
        #endregion constructor


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
                else if (pdu.GetType() == typeof(Early_User_Authorization_Result_PDU))
                {
                    earlyUserAuthorizationResultPDU = pdu.Clone() as Early_User_Authorization_Result_PDU;
                }
                else if (pdu.GetType() == typeof(Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request))
                {
                    mcsConnectInitialPdu = ((Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request)
                        pdu.Clone()).mcsCi;
                }
                else if (pdu.GetType() == typeof(Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response))
                {
                    mcsConnectResponsePdu = (Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response)
                        pdu.Clone();
                    encryptionAlgorithm = new EncryptionAlgorithm(RdpEncryptionMethod);
                }
                else if (pdu.GetType() == typeof(Server_MCS_Attach_User_Confirm_Pdu))
                {
                    userChannelId = (long)((Server_MCS_Attach_User_Confirm_Pdu)pdu).attachUserConfirm.initiator.Value;
                }
                else if (pdu.GetType() == typeof(Client_Security_Exchange_Pdu))
                {
                    clientRandom = RdpbcgrUtility.CloneByteArray(
                        ((Client_Security_Exchange_Pdu)pdu).securityExchangePduData.clientRandom);
                }
                else if (pdu.GetType() == typeof(Client_Info_Pdu))
                {
                    clientInfo = ((Client_Info_Pdu)pdu.Clone()).infoPacket;
                    if (clientInfo != null
                        && (clientInfo.flags & flags_Values.INFO_COMPRESSION) == flags_Values.INFO_COMPRESSION)
                    {
                        ioDecompressor = new Decompressor((SlidingWindowSize)CompressionTypeSupported);
                    }
                }
                else if (pdu.GetType() == typeof(Server_License_Error_Pdu_Valid_Client))
                {
                    licenseErrorPdu = (Server_License_Error_Pdu_Valid_Client)pdu.Clone();
                }
                else if (pdu.GetType() == typeof(Server_Initiate_Multitransport_Request_PDU))
                {
                    Server_Initiate_Multitransport_Request_PDU initRequset = pdu as Server_Initiate_Multitransport_Request_PDU;
                    if (initRequset.requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR)
                    {
                        requestIdReliable = initRequset.requestId;
                        cookieReliable = initRequset.securityCookie;
                    }
                    else
                    {
                        requestIdLossy = initRequset.requestId;
                        cookieLossy = initRequset.securityCookie;
                    }
                }
                else if (pdu.GetType() == typeof(Server_Demand_Active_Pdu))
                {
                    demandActivePdu = ((Server_Demand_Active_Pdu)pdu.Clone()).demandActivePduData;
                }
                else if (pdu.GetType() == typeof(Client_Confirm_Active_Pdu))
                {
                    comfirmActivePdu = ((Client_Confirm_Active_Pdu)pdu.Clone()).confirmActivePduData;
                    if (channelManager == null)
                    {
                        channelManager = new ClientStaticVirtualChannelManager(this);
                    }
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
        /// Clear all the members.
        /// </summary>
        public void ClearAll()
        {
            ClearForReconnect();

            lock (contextLock)
            {
                autoReconnectCookie = null;
                clientRandom = null;
                lastDisconnectReason = 0;
                lastErrorInfo = errorInfo_Values.ERRINFO_NONE;
                lastStatusInfo = StatusCode_Values.TS_STATUS_NO_STATUS;
                pduCountToUpdate = ConstValue.PDU_COUNT_TO_UPDATE_SESSION_KEY;
                isAuthenticatingRDSTLS = false;
                isExpectingEarlyUserAuthorizationResultPDU = false;
            }
        }


        /// <summary>
        /// Clear all the members except the reconnection relative members.
        /// </summary>
        public void ClearForReconnect()
        {
            lock (contextLock)
            {
                x224ConnectionRequestPdu = null;
                x224ConnectionConfirmPdu = null;
                x224NegotiateFailurePdu = null;
                earlyUserAuthorizationResultPDU = null;
                mcsConnectInitialPdu = null;
                mcsConnectResponsePdu = null;
                userChannelId = 0;
                clientInfo = null;
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

                isAuthenticatingRDSTLS = false;
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
        /// <param name="filter">A function to filter out the packets.</param>
        /// <returns></returns>
        public StackPacket GetPacketFromBuffer(Func<StackPacket, bool> filter)
        {
            if (unprocessedPacketBuffer.Count > 0)
            {
                lock (unprocessedPacketBuffer)
                {
                    if (unprocessedPacketBuffer.Count > 0)
                    {
                        for (int i = 0; i < unprocessedPacketBuffer.Count; i++)
                        {
                            var packet = unprocessedPacketBuffer[i];
                            if (filter(packet))
                            {
                                unprocessedPacketBuffer.RemoveAt(i);
                                return packet;
                            }
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
        /// Generate session key for encryption by the member field encryptionAlgorithm.
        /// </summary>
        internal void GenerateSessionKey()
        {
            lock (contextLock)
            {
                encryptionAlgorithm.GenerateSessionKey(clientRandom, ServerRandomNumber);
            }
        }


        /// <summary>
        /// Encrypt data and generate data signature by the member field encryptionAlgorithm.
        /// </summary>
        /// <param name="originalData">Data to be encrypted. This argument can be null.</param>
        /// <param name="isSalted">Specify if data signature generated with salted MAC.</param>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="dataSignature">The generated data signature.</param>
        internal void Encrypt(byte[] originalData, bool isSalted, out byte[] encryptedData, out byte[] dataSignature)
        {
            lock (contextLock)
            {
                dataSignature = encryptionAlgorithm.GenerateDataSignature(originalData, isSalted);
                encryptedData = encryptionAlgorithm.Encrypt(originalData);
                encryptionCount++;
            }
        }


        /// <summary>
        /// Decrypt data and validate the data signature by the member field encryptionAlgorithm.
        /// </summary>
        /// <param name="originalData">Data to be decrypted. This argument can be null.</param>
        /// <param name="dataSignature">The data signature to be validated. This argument can be null.</param>
        /// <param name="isSalted">Specify if data signature generated with salted MAC.</param>
        /// <param name="decryptedData">The decrypted data.</param>
        /// <returns>Whether the data signature is valid.</returns>
        internal bool Decrypt(byte[] originalData, byte[] dataSignature, bool isSalted, out byte[] decryptedData)
        {
            lock (contextLock)
            {
                decryptedData = encryptionAlgorithm.Decrypt(originalData);
                decryptionCount++;

                return encryptionAlgorithm.ValidateDataSignature(dataSignature, decryptedData, isSalted);
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
        /// Split complete virtual data into chunks by the member field channelManager.
        /// This method is called when sending Virtual Channel PDU.
        /// </summary>
        /// <param name="channelId">The channel id. If the channel id is invalid, 
        /// then the return value is null.</param>
        /// <param name="virtualChannelData">The complete channel data. This argument can be null.</param>
        /// <returns>The chunk data.</returns>
        public ChannelChunk[] SplitToChunks(long channelId, byte[] virtualChannelData)
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
        /// <returns>If the reassemble is complete, then return the Virtual_Channel_Complete_PDU.
        /// Else return null.</returns>
        public Virtual_Channel_Complete_Server_Pdu ReassembleChunkData(Virtual_Channel_RAW_Server_Pdu pdu)
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
        internal byte[] Decompress(byte[] data, compressedType_Values type)
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
    }
}
