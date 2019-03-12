// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    /// <summary>
    /// Specify various invalid types
    /// </summary>
    public enum NegativeType
    {
        #region Common

        /// <summary>
        /// There is no invalid fields in the Pdu
        /// </summary>
        None,

        /// <summary>
        /// Length in TPKT header has invalid value
        /// </summary>
        InvalidTPKTLength,

        /// <summary>
        /// Flags field in SecurityHeader is invalid
        /// </summary>
        InvalidFlagsInSecurityHeader,

        /// <summary>
        /// Invalid MAC Signature in security header
        /// </summary>
        InvalidMACSignature,

        /// <summary>
        /// Invalid length field in MCS Header
        /// </summary>
        InvalidMCSLength,

        /// <summary>
        /// Invalid length field in ShareHeader
        /// </summary>
        InvalidLengthInShareHeader,

        #endregion Common

        #region Connection

        #region Connection Initiation
        /// <summary>
        /// The length in TPKT header is less than 11 bytes
        /// </summary>
        X224ConnectionRequest_LengthTooSmall,

        /// <summary>
        /// The class is not 0
        /// </summary>
        X224ConnectionRequest_InvalidClass,

        #endregion Connection Initiation

        #region Basic Setting Exchange

        MCSConnectInitialPDU_InvalidH221NonStandardkey,

        /// <summary>
        /// Encoded length of Client MCS Connect Initial PDU with GCC Conference Create Request message is invalid 
        /// </summary>
        MCSConnectInitialPDU_InvalidEncodedLength,

        /// <summary>
        /// DesktopWidth and DesktopHeight in Core data are too large
        /// </summary>
        MCSConnectInitialPDU_CoreData_DesktopWidthHeightTooLarge,

        /// <summary>
        /// ColorDepth in Core data is invalid
        /// </summary>
        MCSConnectInitialPDU_CoreData_InvalidColorDepth,

        /// <summary>
        /// PostBeta2ColorDepth in core data is invalid
        /// </summary>
        MCSConnectInitialPDU_CoreData_InvalidPostBeta2ColorDepth,

        /// <summary>
        /// ServerSelectedProtocol in core data is invalid
        /// </summary>
        MCSConnectInitialPDU_CoreData_InvalidServerSelectedProtocol,

        /// <summary>
        /// Encryption methods in security data is invalid
        /// </summary>
        MCSConnectInitialPDU_SecurityData_InvalidEncryptionMethods,

        /// <summary>
        /// ChannelCount in Network data is invalid, larger than 31
        /// </summary>
        MCSConnectInitialPDU_InvalidChannelCount,

        #endregion Basic Setting Exchange

        #region Channel Connection

        /// <summary>
        /// SubHeight of MCSErectDomainRequest is not present
        /// </summary>
        MCSErectDomainRequest_SubHeightNotPresent,

        /// <summary>
        /// Subinterval of MCSErectDomainRequest is not present
        /// </summary>
        MCSErectDomainRequest_SubintervalNotPresent,

        #endregion Channel Connection

        SecurityExchangePDU_InvalidLength,

        #endregion Connection

        #region Others
        /// <summary>
        /// SecurityVerifier of Client info PDU is not set a right value
        /// </summary>
        AutoReconnection_InvalidSecurityVerifier,

        /// <summary>
        /// secFlags of fpInputHeader is set to FASTPATH_INPUT_ENCRYPTED (2) when Enhanced RDP Security is in effect.
        /// </summary>
        FastPathInput_InvalidSecFlags,

        /// <summary>
        /// Invalid Event type in FastPath/SlowPath input
        /// </summary>
        InvalidEventType,

        #endregion Others

    }

    /// <summary>
    /// Test data for RDPBCGR
    /// </summary>
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
        /// <summary>
        /// The size of the client random number.
        /// </summary>
        public const int ClientRandomSize = 32;

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

    #region Type defined for Negative Test

    /// <summary>
    /// This class is a wrapper of Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request
    /// The ToBytes method in this class make an invalid encoded length
    /// </summary>
    public class Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request_Ex : Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request
    {
        public Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request_Ex(Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request originalPacket, RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
            this.tpktHeader = originalPacket.tpktHeader;
            this.x224Data = originalPacket.x224Data;
            this.mcsCi = originalPacket.mcsCi;

        }

        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();
            // TBD: Do something herer to change the encoded length
            return rawData;
        }
    }
    
    /// <summary>
    /// This class is a wrapper of Client_MCS_Erect_Domain_Request.
    /// It is used for negative test cases
    /// </summary>
    public class Client_MCS_Erect_Domain_Request_Ex : Client_MCS_Erect_Domain_Request
    {
        NegativeType invalidType;
        public Client_MCS_Erect_Domain_Request_Ex(Client_MCS_Erect_Domain_Request originalPacket, RdpbcgrClientContext clientContext, NegativeType invalidType)
            : base(clientContext)
        {
            this.tpktHeader = originalPacket.tpktHeader;
            this.x224Data = originalPacket.x224Data;
            this.subHeight = originalPacket.subHeight;
            this.subInterval = originalPacket.subInterval;
            this.invalidType = invalidType;
        }

        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();
            // TBD: Do something here
            return rawData;
        }
    }

    /// <summary>
    /// This class is a wrapper of Client_Info_Pdu
    /// The ToBytes method in this class make an invalid MCS length
    /// </summary>
    public class Client_Info_Pdu_Ex : Client_Info_Pdu
    {
        public Client_Info_Pdu_Ex(Client_Info_Pdu originalPacket, RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
            this.commonHeader = originalPacket.commonHeader;
            this.infoPacket = originalPacket.infoPacket;
        }

        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();
            // TBD: Do something herer to change the MCS length
            return rawData;
        }
    }

    /// <summary>
    /// This class is a wrapper of Client_Confirm_Active_Pdu
    /// The ToBytes method in this class make an invalid MCS length
    /// </summary>
    public class Client_Confirm_Active_Pdu_Ex : Client_Confirm_Active_Pdu
    {
        public Client_Confirm_Active_Pdu_Ex(Client_Confirm_Active_Pdu originalPacket, RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
            this.commonHeader = originalPacket.commonHeader;
            this.confirmActivePduData = originalPacket.confirmActivePduData;
        }

        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();
            // TBD: Do something herer to change the MCS length
            return rawData;
        }
    }

    /// <summary>
    /// This class is a wrapper of TS_INPUT_PDU
    /// The ToBytes method in this class make an invalid MCS length
    /// </summary>
    public class TS_INPUT_PDU_Ex : TS_INPUT_PDU
    {
        public TS_INPUT_PDU_Ex(TS_INPUT_PDU originalPacket, RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
            this.commonHeader = originalPacket.commonHeader;
            this.shareDataHeader = originalPacket.shareDataHeader;
            this.numberEvents = originalPacket.numberEvents;
            this.pad2Octets = originalPacket.pad2Octets;
            this.slowPathInputEvents = originalPacket.slowPathInputEvents;
        }

        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();
            // TBD: Do something herer to change the MCS length
            return rawData;
        }
    }

    /// <summary>
    /// This class is a wrapper of Virtual_Channel_RAW_Pdu
    /// The ToBytes method in this class make an invalid MCS length
    /// </summary>
    public class Virtual_Channel_RAW_Pdu_Ex : Virtual_Channel_RAW_Pdu
    {
        public Virtual_Channel_RAW_Pdu_Ex(Virtual_Channel_RAW_Pdu originalPacket, RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
            this.commonHeader = originalPacket.commonHeader;
            this.channelPduHeader = originalPacket.channelPduHeader;
            this.virtualChannelData = originalPacket.virtualChannelData;
        }
        public override byte[] ToBytes()
        {
            byte[] rawData = base.ToBytes();
            // TBD: Do something herer to change the MCS length
            return rawData;
        }
    }
    #endregion Type defined for Negative Test
}