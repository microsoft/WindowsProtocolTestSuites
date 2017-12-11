// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    /// <summary>
    /// The RDPBCGRVersion is likely useless, maybe deleted later.
    /// </summary>
    public enum RdpVersion
    {
        Version40 = 0x00080001,
        Version50 = 0x00080004,
        Version51 = 0x00080004,
        Version52 = 0x00080004,
        Version60 = 0x00080004,
        Version61 = 0x00080004,
        Version70 = 0x00080004,
        Version71 = 0x00080004
    }

    /// <summary>
    /// To indicate the current state of a RDP session
    /// </summary>
    public enum ServerSessionState
    {
        /// <summary>
        /// Transport not connected
        /// </summary>
        TransportDisconnected = 0x00000000,

        /// <summary>
        /// Transport connected
        /// </summary>
        TransportConnected = 0x00000001,

        /// <summary>
        /// Server received the X224 Connection Request
        /// </summary>
        X224ConnectionRequestReceived = 0x00000100,

        /// <summary>
        /// Server sends the X224 Connection response
        /// </summary>
        X224ConnectionResponseSent = 0x00000101,

        /// <summary>
        /// MCS Connect Initial Received
        /// </summary>
        MCSConnectInitialReceived = 0x00000200,

        /// <summary>
        /// MCS Connect Response Sent
        /// </summary>
        MCSConnectResponseSent = 0x00000201,

        /// <summary>
        /// MCS Erect Domain Request Received
        /// </summary>
        MCSErectDomainRequestReceived = 0x00000400,

        /// <summary>
        /// MCS Attach User Request Received
        /// </summary>
        MCSAttachUserRequestReceived = 0x00000401,

        /// <summary>
        /// MCS Attach User Confirm Sent
        /// </summary>
        MCSAttachUserConfirmSent = 0x00000402,

        /// <summary>
        /// MCSChannelJoinRequestReceived
        /// </summary>
        MCSChannelJoinRequestReceived = 0x00000404,

        /// <summary>
        /// MCS Channel Join Confirm Sent
        /// </summary>
        MCSChannelJoinConfirmSent = 0x00000408,

        /// <summary>
        /// Client Security Exchange Received
        /// </summary>
        ClientSecurityExchangeReceived = 0x00000800,

        /// <summary>
        /// Client Info Pdu Received
        /// </summary>
        ClientInfoPduReceived = 0x00000801,

        /// <summary>
        /// Server License Error Pdu Sent
        /// </summary>
        ServerLicenseErroPduSent = 0x00001000,

        /// <summary>
        /// Server Demand Active Sent
        /// </summary>
        ServerDemandActiveSent = 0x00002000,

        /// <summary>
        /// Client Confirm Active Received
        /// </summary>
        ClientConfirmActiveReceived = 0x00002001,

        /// <summary>
        /// Client Synchronize Received
        /// </summary>
        ClientSynchronizeReceived = 0x00004000,

        /// <summary>
        /// Client Control Cooperate Received
        /// </summary>
        ClientControlCooperateReceived = 0x00004001,

        /// <summary>
        /// Client Control Request Control Received
        /// </summary>
        ClientControlRequestControlReceived = 0x00004002,

        /// <summary>
        /// Client Persistent Key List Received
        /// </summary>
        ClientPersistenKeyListReceived = 0x00004003,

        /// <summary>
        /// Client Font List Received
        /// </summary>
        ClientFontListReceived = 0x00004004,

        /// <summary>
        /// RDP connection have been established
        /// </summary>
        RDPConnected = 0x00010000,
    }

    /// <summary>
    /// Indicates the type of RDP session to be established.
    /// </summary>
    public enum RDPSessionType
    {
        /// <summary>
        /// Normal Session.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Auto Reconnection Session.
        /// </summary>
        AutoReconnection,

        /// <summary>
        /// Server Redirection Session.
        /// </summary>
        Redirection
    }

    /// <summary>
    /// Indicates the server state in Connection Finalization phase
    /// </summary>
    public enum ConnectionFinalization_ServerState
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Connection Finalization phase not started
        /// </summary>
        NotStarted = 0x00008000,

        /// <summary>
        /// Server Synchronize Sent
        /// </summary>
        ServerSynchonizeSent = 0x00008001,

        /// <summary>
        /// Server Control Cooperate Sent
        /// </summary>
        ServerControlCooperateSent = 0x00008002,

        /// <summary>
        /// Server Control Granted Control Sent
        /// </summary>
        ServerControlGrantedControlSent = 0x00008004,

        /// <summary>
        /// Server Control Granted Control Sent
        /// </summary>
        ServerFontMapSent = 0x00008008
    }

    /// <summary>
    /// The three reasons for sending Save Session Info PDU are:
    /// 1.	Notifying the client that the user has logged on (the username and domain which were used, as well as the ID of the session to which the user connected, may be included in this notification).
    /// 2.	Transmitting an auto-reconnect cookie to the client (see section 1.3.1.5 for an overview of automatic reconnection).
    /// 3.	Informing the client of an error or warning that occurred while the user was logging on.
    /// </summary>
    public enum LogonNotificationType
    {
        UserLoggedOn,
        AutoReconnectCookie,
        LogonError
    }

    /// <summary>
    /// This class is used to store test data.
    /// </summary>
    public class RdpbcgrTestData
    {
        /// <summary>
        /// The session identifier for reconnection test.
        /// </summary>
        public const uint Test_LogonId = 1;

        /// <summary>
        /// Byte buffer containing a 16-byte, random number generated as a key for secure reconnection.
        /// </summary>
        public static byte[] Test_ArcRadndomBits = new byte[]{
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f};

        /// <summary>
        /// The session identifier which used for server redirection test.
        /// </summary>
        public const uint Test_Redirection_SessionId = 1;

        /// <summary>
        /// The routing token is used in redirection test.
        /// </summary>
        public const string Test_Redirection_RoutingToken = "TestRoutingToken\r\n";

        /// <summary>
        /// The User Name which used for server redirection test.
        /// </summary>
        public static string Test_UserName = "TestUser";

        /// <summary>
        /// The Domain Name which used for server redirection test.
        /// </summary>
        public static string Test_Domain = "TestDomain";

        /// <summary>
        /// The User Password which used for server redirection test.
        /// </summary>
        public static string Test_Password = "TestPassword";

        /// <summary>
        /// The redirection GUID for RDSTLS redirection test.
        /// </summary>
        public static string Test_RedirectionGuid = "SUTRedirectionGuid";

        /// <summary>
        /// The full qualified domain name of driver computer for RDSTLS redirection test.
        /// </summary>
        public static string Test_FullQualifiedDomainName = "DriverComputerFullQualifiedDomainName";

        /// <summary>
        /// The NetBios name of driver computer for RDSTLS redirection test.
        /// </summary>
        public static string Test_NetBiosName = "DriverComputerNetBiosName";
    }

    /// <summary>
    /// The invalid types used for Static Virtual Channel negative testing.
    /// </summary>
    public enum StaticVirtualChannel_InvalidType
    {
        /// <summary>
        /// Valid Type.
        /// </summary>
        None,

        /// <summary>
        /// The length field of tpktHeader is set to an invalid value.
        /// </summary>
        InvalidTPKTLength,

        /// <summary>
        /// The length field of x224Data is set to an invalid value.
        /// </summary>
        InvalidX224Length,

        /// <summary>
        /// The length field of mcsPdu is set to an invalid value.
        /// </summary>
        InvalidMCSLength,

        /// <summary>
        /// The signature field of SecurityHeader is set to an incorrect value.
        /// </summary>
        InvalidSignature,

        /// <summary>
        /// Presents the SEC_ENCRYPT flag in securityHeader when Enhanced RDP Security is in effect.
        /// </summary>
        InvalidEncryptFlag,

        /// <summary>
        /// The CHANNEL_FLAGS_SHOW_PROTOCOL (0x00000010) flag is not set in a virtual channel data chunk. 
        /// </summary>
        InvalidFlag
    }

    /// <summary>
    /// The invalid types used for Slow-Path negative testing testing.
    /// </summary>
    public enum SlowPathTest_InvalidType
    {
        /// <summary>
        /// Valid Type.
        /// </summary>
        None,

        /// <summary>
        /// The length field of tpktHeader is set to an invalid value.
        /// </summary>
        InvalidTPKTLength,

        /// <summary>
        /// The length field of x224Data is set to an invalid value.
        /// </summary>
        InvalidX224Length,

        /// <summary>
        /// The length field of mcsPdu is set to an invalid value.
        /// </summary>
        InvalidMCSLength,

        /// <summary>
        /// The signature field of SecurityHeader is set to an incorrect value.
        /// </summary>
        InvalidSignature,

        /// <summary>
        /// Presents the SEC_ENCRYPT flag in securityHeader when Enhanced RDP Security is in effect.
        /// </summary>
        InvalidEncryptFlag,

        /// <summary>
        /// The totalLenth field within shareDataHeader is set to an invalid value. 
        /// </summary>
        InvalidTotalLength
    }

    /// <summary>
    /// Specify various invalid types
    /// </summary>
    public enum NegativeType
    {
        /// <summary>
        /// There is no invalid fields in the Pdu
        /// </summary>
        None,
        /// <summary>
        /// Tpkt header has invalid value
        /// </summary>
        InvalidTPKTHeader,
        /// <summary>
        /// X224 packet contains invalid field, for example lengthIndicator is less than the actual value
        /// </summary>
        InvalidX224,
        /// <summary>
        /// rdpNegData has invalid structure
        /// </summary>
        InvalidRdpNegData,
        /// <summary>
        /// result field in mcsCrsp of MCS Connect Response PDU with GCC Conference Create Response. Refer to Section 3.2.5.3.4
        /// </summary>
        InvalidResult,
        /// <summary>
        /// Not equal to "MUST be" value of the ANSI character string "McDn"
        /// </summary>
        InvalidH221,
        /// <summary>
        /// the length field of User Data Header to an invalid value (less than the actual value)
        /// </summary>
        InvalidEncodedLength,
        /// <summary>
        /// When using external security protocols, the length field of User Data Header is set to an invalid value (less than the actual value)
        /// </summary>
        InvalidEncodedLengthExternalSecurityProtocols,
        /// <summary>
        /// invalid ClientRequestedProtocols in Server MCS connect Response PDU
        /// </summary>
        InvalidClientRequestedProtocols,
        /// <summary>
        /// invalid encryptionmethod field in Server Security Data
        /// </summary>
        InvalidEncryptionMethod,
        /// <summary>
        /// invalid encryptionlevel field in Server Security Data
        /// </summary>
        InvalidEncryptionLevel,
        /// <summary>
        /// invalid serverRandomLen field in Server Security Data
        /// </summary>
        InvalidServerRandomLen,
        /// <summary>
        /// invalid serverCertificate value in Server Security Data
        /// </summary>
        InvalidServerCertificate,
        /// <summary>
        /// invalid value of channelCount of Server Network Data
        /// </summary>
        InvalidChannelCount,

        #region Channel Connection
        /// <summary>
        /// invalid TPK header length (not 11)
        /// </summary>
        InvalidTPKLength,
        /// <summary>
        /// No initiator field presented
        /// </summary>
        InvalidInitiatorField,
        /// <summary>
        /// No channelId field presented
        /// </summary>
        InvalidEmptyChannelIdField,
        /// <summary>
        /// mismatched channelId field is set
        /// </summary>
        InvalidMismatchChannelIdField,
        #endregion

        #region Security Setting Exchange
        /// <summary>
        /// invalid length field in MCS Header
        /// </summary>
        InvalidMCSLength,
        /// <summary>
        /// SEC_LICENSE_PKT (0x0080) flag is not presented
        /// </summary>
        InvalidFlagInSecurityHeader,
        /// <summary>
        /// Invalid MAC Signature in security header
        /// </summary>
        InvalidSignatureInSecurityHeader,
        /// <summary>
        /// invalid dwErrorCode value (ERR_INVALID_MAC)
        /// </summary>
        InvalidMessage,
        #endregion
        #region Capability Exchange
        /// <summary>
        /// invalid Pdu type
        /// </summary>
        InvalidPduType,
        /// <summary>
        /// invalid PDU length
        /// </summary>
        InvalidPduLength,

        #endregion
    }

    /// <summary>
    /// Wrapper of SlowPathOutputPdu to set MCS encoded length to an invalid value.
    /// </summary>
    public class SlowPathOutputPduEx : SlowPathOutputPdu
    {
        public SlowPathOutputPduEx(SlowPathOutputPdu orgPdu, RdpbcgrServerSessionContext serverSessionContext) : base(serverSessionContext)
        {
            this.commonHeader = orgPdu.commonHeader;

            if (orgPdu.slowPathUpdates != null)
            {
                Collection<RdpbcgrSlowPathUpdatePdu> updates = new Collection<RdpbcgrSlowPathUpdatePdu>();
                for (int i = 0; i < orgPdu.slowPathUpdates.Length; ++i)
                {
                    updates.Add(orgPdu.slowPathUpdates[i]);
                }

                this.slowPathUpdates = new RdpbcgrSlowPathUpdatePdu[updates.Count];
                for (int i = 0; i < updates.Count; ++i)
                {
                    this.slowPathUpdates[i] = updates[i];
                }
            }
        }

        /// <summary>
        /// Set the MCS encoded length field to an invalid value (less than the actual).
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] bytes = base.ToBytes();
            bytes[13] = (byte)(bytes[13] - 1);
            return bytes;
        }
    }

    /// <summary>
    /// Wrapper of Virtual_Channel_RAW_Server_Pdu to set MCS encoded length to an invalid value.
    /// </summary>
    public class Virtual_Channel_RAW_Server_Pdu_Ex : Virtual_Channel_RAW_Server_Pdu
    {
        public Virtual_Channel_RAW_Server_Pdu_Ex(Virtual_Channel_RAW_Server_Pdu orgPdu, RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
            this.commonHeader = orgPdu.commonHeader;
            this.channelPduHeader = orgPdu.channelPduHeader;
            this.virtualChannelData = orgPdu.virtualChannelData;
        }

        /// <summary>
        /// Set the MCS encoded length field to an invalid value (less than the actual).
        /// </summary>
        /// <returns></returns>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();
            rawData[13] = (byte)(rawData[13] - 1);
            return rawData;
        }
    }

    /// <summary>
    /// Wrapper of Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response to set MCS encoded length field to an invalid value.
    /// </summary>
    public class Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response_Ex : Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response
    {
        public Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response_Ex(Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response orgPdu, RdpbcgrServerSessionContext serverSessionContext, NegativeType invalidType)
            : base(serverSessionContext)
        {
            this.tpktHeader = orgPdu.tpktHeader;
            this.x224Data = orgPdu.x224Data;
            this.mcsCrsp = orgPdu.mcsCrsp;
            this.invalidType = invalidType;
        }

        /// <summary>
        /// Set the MCS encoded length field to an invalid value (less than the actual). or be used when ASN workaround doesn't work.
        /// </summary>
        /// <returns></returns>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();

            if (invalidType == NegativeType.InvalidEncodedLength)
            {
                //The first encoded length field is at offset 10, 11. Reduce 1 from least byte if least byte is not zero, otherwise, reduce 1 from higest byte.
                if (rawData[11] > 0)
                {
                    rawData[11] = (byte)(rawData[11] - 1);
                }
                else
                {
                    rawData[10] = (byte)(rawData[10] - 1);
                }
            }
            else if (invalidType == NegativeType.InvalidEncodedLengthExternalSecurityProtocols)
            {
                rawData[9] = (byte)(rawData[9] - 1);
            }

            return rawData;
        }

        private NegativeType invalidType = NegativeType.None;
    }

    public class Server_Demand_Active_Pdu_Ex : Server_Demand_Active_Pdu
    {
        public Server_Demand_Active_Pdu_Ex(Server_Demand_Active_Pdu orgPdu, RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
            this.commonHeader = orgPdu.commonHeader;
            //this.commonHeader.tpktHeader = orgPdu.commonHeader.tpktHeader;
            //this.commonHeader.x224Data = orgPdu.commonHeader.x224Data;
            //this.commonHeader.securityHeader = orgPdu.commonHeader.securityHeader;            
            this.demandActivePduData = orgPdu.demandActivePduData;
        }

        /// <summary>
        /// Set the length field in MCSHeader to an invalid value (less than the actual).
        /// </summary>
        /// <returns></returns>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();

            rawData[13] = (byte)(rawData[13] - 1);

            return rawData;
        }
    }

    public class Server_License_Error_Pdu_Valid_Client_Ex : Server_License_Error_Pdu_Valid_Client
    {
        public Server_License_Error_Pdu_Valid_Client_Ex(Server_License_Error_Pdu_Valid_Client orgPdu, RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
            this.commonHeader = orgPdu.commonHeader;
            this.preamble = orgPdu.preamble;
            this.validClientMessage = orgPdu.validClientMessage;
        }

        /// <summary>
        /// Set the length field in MCSHeader to an invalid value (less than the actual).
        /// </summary>
        /// <returns></returns>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();

            rawData[13] = (byte)(rawData[13] - 1);

            return rawData;
        }
    }
}
