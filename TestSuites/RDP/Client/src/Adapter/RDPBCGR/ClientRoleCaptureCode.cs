// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrAdapter
    {
        #region "Check PDUs"

        /// <summary>
        /// 2.2.1.1
        /// </summary>
        /// <param name="clientPdu"></param>
        public void VerifyPdu(Client_X_224_Connection_Request_Pdu clientPdu)
        {
            //Site.Log.Add(TestTools.LogEntryKind.Debug, "CheckX224ConnectionRequestPdu", clientPdu);
            site.CaptureRequirementIfAreEqual<byte>(0, clientPdu.x224Crq.classOptions, 9,
               @"[In Client X.224 Connection Request PDU] x224Crq (7 bytes): An X.224 Class 0 Connection Request "
               + @"transport protocol data unit (TPDU), as specified in [X224] section 13.3.");

            if (clientPdu != null && clientPdu.routingToken != null)
            {
                int len = clientPdu.routingToken.Length;
                site.CaptureRequirementIfIsTrue(clientPdu.routingToken[len - 2] == 0x0D && clientPdu.routingToken[len - 1] == 0x0A, 10,
                    @"[In Client X.224 Connection Request PDU]routingToken (variable): An optional and variable-length "
                        + @"routing token (used for load balancing) terminated by a 0x0D0A two-byte sequence.");
            }

            if (clientPdu.rdpNegData != null)
            {
                site.CaptureRequirementIfAreEqual<type_Values>(type_Values.V1, clientPdu.rdpNegData.type, 22,
                    @"[In RDP Negotiation Request] type ( 1 byte): it MUST be set to 0x01 (TYPE_RDP_NEG_REQ) to indicate "
                    + @"that the packet is a Negotiation Request.");

                bool isRDPNegDataSatisfied = clientPdu.rdpNegData.flags == RDP_NEG_REQ_flags_Values.V1 || clientPdu.rdpNegData.flags == RDP_NEG_REQ_flags_Values.CORRELATION_INFO_PRESENT;
                site.CaptureRequirementIfIsTrue(isRDPNegDataSatisfied, 24,
                    @"[In RDP Negotiation Request] flags (1 byte): An 8-bit, unsigned integer that contains protocol flags. "
                    + @"this flags field  MUST be set to 0x00 or CORRELATION_INFO_PRESENT(0x08).");

                site.CaptureRequirementIfAreEqual<length_Values>(length_Values.V1, clientPdu.rdpNegData.length, 26,
                    @"[In RDP Negotiation Request] length (2 bytes): length MUST be set to 0x0008 (8 bytes).");



                uint bitmask = ~(uint)(requestedProtocols_Values.PROTOCOL_RDP_FLAG | requestedProtocols_Values.PROTOCOL_HYBRID_FLAG | requestedProtocols_Values.PROTOCOL_SSL_FLAG | requestedProtocols_Values.PROTOCOL_HYBRID_EX | requestedProtocols_Values.PROTOCOL_RDSTLS);
                site.CaptureRequirementIfIsTrue(
                    (((uint)clientPdu.rdpNegData.requestedProtocols & bitmask) == 0), 28,
                    @"In RDP Negotiation Request the requestedProtocols can take combining of the following flags. 1. PROTOCOL_RDP_FLAG"
                    + @" 0x00000000 Legacy RDP encryption 2. PROTOCOL_SSL_FLAG 0x00000001  TLS 1.0. 3. PROTOCOL_HYBRID_FLAG 0x00000002  CredSSP. "
                    + @" 4. PROTOCOL_RDSTLS 0x00000004  RDSTLS.");


                if ((clientPdu.rdpNegData.requestedProtocols & requestedProtocols_Values.PROTOCOL_HYBRID_FLAG) != 0)
                {
                    site.CaptureRequirementIfAreNotEqual<requestedProtocols_Values>(0, clientPdu.rdpNegData.requestedProtocols & requestedProtocols_Values.PROTOCOL_SSL_FLAG, 32,
                        @"In RDP Negotiation Request (RDP_NEG_REQ), requestedProtocols (4 bytes) : If PROTOCOL_HYBRID_FLAG (0x00000002)"
                        + @" flag is set, then the PROTOCOL_SSL (0x00000001) SHOULD also be set because Transport Layer Security (TLS) "
                        + @"is a subset of CredSSP.");
                }
            }
        }

        /// <summary>
        /// 2.2.1.3
        /// </summary>
        /// <param name="connectInitialPdu"></param>
        public void VerifyPdu(Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request connectInitialPdu)
        {
            //Site.Log.Add(TestTools.LogEntryKind.Debug, "CheckMCSConnectInitialPdu");
            //<?>
            CaptureRequirement(connectInitialPdu.mcsCi.gccPdu != null, 41);
            //<?>
            //site.CaptureRequirementIfAreEqual<byte>(0, connectInitialPdu.x224Data.type, 66, 
            // @"[In Client MCS Connect Initial PDU with GCC Conference Create Request] x224Data (3 bytes): An X.224 Class 0 Data PDU, as specified in [X224] section 13.7.");

            if (!serverConfig.isExtendedClientDataSupported)
            {
                //<capture> also capture R265
                site.CaptureRequirementIfIsNull(connectInitialPdu.mcsCi.gccPdu.clientMonitorData, 78,
                    @"In  Client MCS Connect Initial PDU with GCC Conference Create Request, clientMonitorData (variable): This field MUST "
                    + @"NOT be included if the server did not advertise support for Extended Client Data Blocks by using the EXTENDED_CLIENT_DATA_SUPPORTED"
                    + @" flag (0x00000001) as described in section 2.2.1.2.1.");
            }
            bool isR81Satisfied = connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_CORE || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_SECURITY
                || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_NET || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_CLUSTER
                || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_MONITOR || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.SC_CORE
                || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.SC_SECURITY || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.SC_NET;
            site.CaptureRequirementIfIsTrue(isR81Satisfied, 81,
                @"In TS_UD_HEADER, the type field can take one of the following values: CS_CORE 0xC001,CS_SECURITY 0xC002,CS_NET 0xC003,CS_CLUSTER 0xC004,"
                + @"SC_CORE 0x0C01,SC_SECURITY 0x0C02,SC_NET 0xOC03");
            //<capture> also caputre R93
            site.CaptureRequirementIfAreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_CORE, connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type, 82,
                @"In TS_UD_HEADER, if the type is CS_CORE0xC001 then the data block which follows User Data Header (TS_UD_HEADER) contains Client Core Data");
            site.CaptureRequirementIfAreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_SECURITY, connectInitialPdu.mcsCi.gccPdu.clientSecurityData.header.type, 83,
                @"In TS_UD_HEADER, if the type is CS_SECURITY 0xC002 then the data block which follows User Data Header (TS_UD_HEADER) contains Client Security Data");
            if (connectInitialPdu.mcsCi.gccPdu.clientNetworkData != null)
            {
                site.CaptureRequirementIfAreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_NET, connectInitialPdu.mcsCi.gccPdu.clientNetworkData.header.type, 84,
                    @"In TS_UD_HEADER, if the type is CS_NET 0xC003 then the data block which follows contains User Data Header (TS_UD_HEADER) Client Network Data.");
            }

            if (connectInitialPdu.mcsCi.gccPdu.clientClusterData != null)
            {
                site.CaptureRequirementIfAreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_CLUSTER, connectInitialPdu.mcsCi.gccPdu.clientClusterData.header.type, 85,
                    @"In TS_UD_HEADER, if the type is CS_CLUSTER 0xC004 then the data block which follows User Data Header (TS_UD_HEADER) contains Client Cluster Data.");
            }

            if (connectInitialPdu.mcsCi.gccPdu.clientMonitorData != null)
            {
                site.CaptureRequirementIfAreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_MONITOR, connectInitialPdu.mcsCi.gccPdu.clientMonitorData.header.type, 86,
                    @"In User Data Header (TS_UD_HEADER), type (2 bytes): If the type is CS_MONITOR 0xC005 then the data block that follows contains Client Monitor"
                    + @" Data (section 2.2.1.3.6).");
                VerifyStructure(connectInitialPdu.mcsCi.gccPdu.clientMonitorData);
            }
            if (connectInitialPdu.mcsCi.gccPdu.clientCoreData != null)
            {
                VerifyStructure(connectInitialPdu.mcsCi.gccPdu.clientCoreData);
            }
            if (connectInitialPdu.mcsCi.gccPdu.clientSecurityData != null)
            {
                VerifyStructure(connectInitialPdu.mcsCi.gccPdu.clientSecurityData);
            }
            if (connectInitialPdu.mcsCi.gccPdu.clientNetworkData != null)
            {
                VerifyStructure(connectInitialPdu.mcsCi.gccPdu.clientNetworkData);
            }
            if (connectInitialPdu.mcsCi.gccPdu.clientClusterData != null)
            {
                VerifyStructure(connectInitialPdu.mcsCi.gccPdu.clientClusterData);
            }

        }

        /// <summary>
        /// 2.2.1.5
        /// </summary>
        /// <param name="erectPdu"></param>
        public void VerifyPdu(Client_MCS_Erect_Domain_Request erectPdu)
        {
        }

        /// <summary>
        /// 2.2.1.6
        /// </summary>
        /// <param name="attachUserPdu"></param>
        public void VerifyPdu(Client_MCS_Attach_User_Request attachUserPdu)
        {
        }

        /// <summary>
        /// 2.2.1.8
        /// </summary>
        /// <param name="channelJoinPdu"></param>
        public void VerifyPdu(Client_MCS_Channel_Join_Request channelJoinPdu)
        {
        }

        /// <summary>
        /// 2.2.1.10
        /// </summary>
        /// <param name="securityPdu"></param>
        public void VerifyPdu(Client_Security_Exchange_Pdu securityPdu)
        {
            //<?>
            CaptureRequirement(securityPdu.securityExchangePduData.length != 0, 407);
            //Refer to test suite bug #8341151
            //site.CaptureRequirementIfIsTrue((securityPdu.commonHeader.securityHeader.flags & TS_SECURITY_HEADER_flags_Values.SEC_EXCHANGE_PKT) == TS_SECURITY_HEADER_flags_Values.SEC_EXCHANGE_PKT, 411, 
            //  "In Security Exchange PDU Data (TS_SECURITY_PACKET) the flags field of the security header MUST contain the SEC_EXCHANGE_PKT flag (0x0001).");

            //following capture is wrong, as after decoding the length would changed
            //site.CaptureRequirementIfAreEqual<uint>((uint)securityPdu.securityExchangePduData.clientRandom.Length, securityPdu.securityExchangePduData.length, 413, 
            //"In Security Exchange PDU Data (TS_SECURITY_PACKET) the length field indicates the size in bytes of the buffer containing the encrypted client random value, not including the header length.");
        }

        /// <summary>
        /// 2.2.1.11
        /// </summary>
        /// <param name="clientInfo"></param>
        public void VerifyPdu(Client_Info_Pdu clientInfo)
        {
            site.CaptureRequirementIfIsTrue(clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER ||
                clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER1 ||
                clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER2, 422,
                @"[In Client Info PDU Data (CLIENT_INFO_PDU)]securityHeader (variable):   This field MUST contain one"
                + @" of the following headers:[Basic Security Header,Non-FIPS Security Header, FIPS Security Header ].");

            if (serverConfig.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                bool isR423Satisfied = (clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER) &&
                    (!(clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER1)) &&
                    (!(clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER2));
                site.CaptureRequirementIfIsTrue(isR423Satisfied, 423,
                    @"[In Client Info PDU Data (CLIENT_INFO_PDU)]securityHeader (variable):The securityHeader in "
                    + @"CLIENT_INFO_PDU structure is a Basic Security Header if the Encryption Level  selected by"
                    + @" the server is ENCRYPTION_LEVEL_NONE (0).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                //<bug>sdk bug 
                //site.CaptureRequirementIfIsInstanceOfType(clientInfo.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 424,
                //    @"[In Client Info PDU Data (CLIENT_INFO_PDU)]securityHeader (variable):The securityHeader in CLIENT_INFO_PDU structure is a Non-FIPS Security Header (section 2.2.8.1.1.2.2) if the Encryption LevelMethod selected by the server (see sections 5.3.2 and 2.2.1.4.3) is ENCRYPTION_LEVEL_LOW (1METHOD_40BIT (0x00000001), ENCRYPTION_LEVEL_CLIENT_COMPATIBLE (2METHOD_56BIT (0x00000008), or ENCRYPTION_LEVEL_HIGH (3METHOD_128BIT (0x00000002).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(clientInfo.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 425,
                    @"[In Client Info PDU Data (CLIENT_INFO_PDU)]securityHeader (variable):The securityHeader in "
                    + @"CLIENT_INFO_PDU structure is a FIPS Security Header (section 2.2.8.1.1.2.3) if the Encryption"
                    + @" Level Method  selected by the server  is ENCRYPTION_LEVELMethod _FIPS ( 4 0x00000010).");
            }

            if (serverConfig.encryptedProtocol == EncryptedProtocol.Rdp)
            {
                //SDK Bug, SecurityHeader.Flag is parsed incorrectly for Client Info PDU
                //site.CaptureRequirementIfAreEqual<TS_SECURITY_HEADER_flags_Values>( TS_SECURITY_HEADER_flags_Values.SEC_INFO_PKT, (clientInfo.commonHeader.securityHeader.flags & TS_SECURITY_HEADER_flags_Values.SEC_INFO_PKT), 426,
                //    @"[In Client Info PDU Data (CLIENT_INFO_PDU)]In CLIENT_INFO_PDU structure, the flags field of "
                //    + @"the security header MUST contain the SEC_INFO_PKT flag (section 2.2.8.1.1.2.1).");

                site.CaptureRequirementIfIsNotNull(clientInfo.infoPacket, 430,
                    @"[In Info Packet (TS_INFO_PACKET)]The Info Packet  SHOULD be encrypted (see sections 5.3 and"
                    + @" 5.4 for an overview of RDP security mechanisms).");
            }
            VerifyStructure(clientInfo.infoPacket);
        }

        /// <summary>
        /// 2.2.1.13.2
        /// </summary>
        /// <param name="confirmActivePdu"></param>
        public void VerifyPdu(Client_Confirm_Active_Pdu confirmActivePdu)
        {
            //Bug to be confirmed
            if (this.serverConfig.encryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE || this.serverConfig.encryptionMethod != EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                bool isR701Satisfied = confirmActivePdu.commonHeader.securityHeader.GetType() == typeof(TS_SECURITY_HEADER1) ||
                    confirmActivePdu.commonHeader.securityHeader.GetType() == typeof(TS_SECURITY_HEADER2);
                site.CaptureRequirementIfIsTrue(isR701Satisfied, 701,
                    @"In Client Confirm Active PDU, if the Encryption Level selected by the server is greater"
                    + @" than ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is "
                    + @"greater than ENCRYPTION_METHOD_NONE (0) then this field MUST contain one of the "
                    + @"following headers:");
            }

            if (this.serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                this.serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                this.serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(confirmActivePdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 702,
                    @"[In Client Confirm Active PDU]securityHeader (variable):  The securityHeader in Server"
                    + @" Demand Active PDU is a Non-FIPS Security Header (section 2.2.8.1.1.2.2) if the "
                    + @"Encryption LevelMethod selected by the server (see sections 5.3.2 and 2.2.1.4.3) is "
                    + @"ENCRYPTION_LEVEL_CLIENT_COMPATIBLE (2)METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT"
                    + @" (0x00000008), or ENCRYPTION_LEVEL_HIGH (3METHOD_128BIT (0x00000002).");
            }
            else if (this.serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(confirmActivePdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 703,
                    @"[In Client Confirm Active PDU]securityHeader (variable):The securityHeader in Server "
                    + @"Demand Active PDU is a FIPS Security Header,if the Encryption LevelMethod selected "
                    + @"by the server is ENCRYPTION_METHOD_FIPS (0x00000010).");
            }

            //verify all the capability set:
            foreach (ITsCapsSet cap in confirmActivePdu.confirmActivePduData.capabilitySets)
            {
                switch (cap.GetCapabilityType())
                {
                    case capabilitySetType_Values.CAPSTYPE_BITMAP:
                        VerifyStructure((TS_BITMAP_CAPABILITYSET)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_ORDER:
                        VerifyStructure((TS_ORDER_CAPABILITYSET)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_BITMAPCACHE:
                        VerifyStructure((TS_BITMAPCACHE_CAPABILITYSET)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_REV2:
                        VerifyStructure((TS_BITMAPCACHE_CAPABILITYSET_REV2)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_POINTER:
                        VerifyStructure((TS_POINTER_CAPABILITYSET)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_INPUT:
                        VerifyStructure((TS_INPUT_CAPABILITYSET)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_BRUSH:
                        VerifyStructure((TS_BRUSH_CAPABILITYSET)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_GLYPHCACHE:
                        VerifyStructure((TS_GLYPHCACHE_CAPABILITYSET)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_OFFSCREENCACHE:
                        VerifyStructure((TS_OFFSCREEN_CAPABILITYSET)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL:
                        VerifyStructure((TS_VIRTUALCHANNEL_CAPABILITYSET)cap);
                        break;
                    case capabilitySetType_Values.CAPSTYPE_SOUND:
                        VerifyStructure((TS_SOUND_CAPABILITYSET)cap);
                        break;
                }
            }
        }

        /// <summary>
        /// 2.2.1.14
        /// </summary>
        /// <param name="clientSyncPdu"></param>
        public void VerifyPdu(Client_Synchronize_Pdu clientSyncPdu)
        {
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(clientSyncPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 730,
                    @"In Client Synchronize PDU, securityHeader (variable):  The securityHeader in Server"
                    + @" Demand Active PDU is a Non-FIPS Security Header (section 2.2.8.1.1.2.2) if the "
                    + @"Encryption LevelMethod selected by the server (see sections 5.3.2 and 2.2.1.4.3) is"
                    + @" ENCRYPTION_METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT (0x00000008), "
                    + @"ENCRYPTIONMETHOD_128BIT (0x00000002)");

            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(clientSyncPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 731,
                    @"In Client Synchronize PDU, securityHeader (variable):The securityHeader in Server Demand"
                    + @" Active PDU is a FIPS Security Header,if the Encryption LevelMethod selected by the server"
                    + @" is ENCRYPTION_METHOD_FIPS (0x00000010).");

            }
            site.CaptureRequirementIfAreEqual<int>(7, (clientSyncPdu.synchronizePduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xf), 736,
                @"In TS_SYNCHONIZE_PDU the type subfield of the pduType field of the Share Control Header "
                + @"MUST be set to PDUTYPE_DATAPDU (7).");
            site.CaptureRequirementIfAreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_SYNCHRONIZE, clientSyncPdu.synchronizePduData.shareDataHeader.pduType2, 737,
                @"In TS_SYNCHONIZE_PDU the type subfield of the pduType2 field of the Share Data Header MUST"
                + @" be set to PDUTYPE2_SYNCHRONIZE (31).");
            site.CaptureRequirementIfAreEqual<messageType_Values>(messageType_Values.V1, clientSyncPdu.synchronizePduData.messageType, 739,
                @"In TS_SYNCHONIZE_PDU, the messageType field MUST be set to SYNCMSGTYPE_SYNC (1).");
        }

        /// <summary>
        /// 2.2.1.15
        /// </summary>
        /// <param name="clientCoopPdu"></param>
        public void VerifyPdu(Client_Control_Pdu_Cooperate clientCoopPdu)
        {
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(clientCoopPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 747,
                    @"[In Client Control PDU - Cooperate]securityHeader (variable):  The securityHeader in "
                    + @"Server Demand Active PDU is a Non-FIPS Security Header (section 2.2.8.1.1.2.2) if "
                    + @"the Encryption LevelMethod selected by the server (see sections 5.3.2 and 2.2.1.4.3) "
                    + @"is ENCRYPTION_LEVEL_CLIENT_COMPATIBLE (2)METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT"
                    + @" (0x00000008), or ENCRYPTION_LEVEL_HIGH (3METHOD_128BIT (0x00000002).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(clientCoopPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 748,
                    @"[In Client Control PDU - Cooperate]securityHeader (variable):The securityHeader in Server"
                    + @" Demand Active PDU is a FIPS Security Header,if the Encryption LevelMethod selected by "
                    + @"the server is ENCRYPTION_METHOD_FIPS (0x00000010).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                site.CaptureRequirementIfIsNull(clientCoopPdu.commonHeader.securityHeader, 749,
                    @" Client Control PDU - Cooperate ,If Enhanced RDP Security is in effect or the Encryption "
                    + @"Method selected by the server is ENCRYPTION_METHOD_NONE (0), then securityHeader MUST NOT"
                    + @" be  included in the Client Control PDU - Cooperate.");
            }
            site.CaptureRequirementIfIsTrue(clientCoopPdu.controlPduData.controlId == 0 && clientCoopPdu.controlPduData.grantId == 0, 751,
                @"In controlPduData of Client Control PDU - Cooperate, The grantId and controlId fields of the "
                + @"Control PDU Data MUST both be set to zero.");

            site.CaptureRequirementIfAreEqual<action_Values>(action_Values.CTRLACTION_COOPERATE, clientCoopPdu.controlPduData.action, 752,
                @"In controlPduData of Client Control PDU - Cooperate, the action field MUST be set to "
                + @"CTRLACTION_COOPERATE (0x0004).");

            site.CaptureRequirementIfAreEqual<int>(7, (clientCoopPdu.controlPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xf), 755,
                @"In TS_CONTROL_PDU, the type subfield of the pduType field of the Share Control Header of TS_CONTROL_PDU"
                + @" structure MUST be set to PDUTYPE_DATAPDU (7). ");

            site.CaptureRequirementIfAreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_CONTROL, clientCoopPdu.controlPduData.shareDataHeader.pduType2, 756,
                @" In TS_CONTROL_PDU, the pduType2 field of the Share Data Header  of TS_CONTROL_PDU structure MUST be set"
                + @" to PDUTYPE2_CONTROL (20).");
            bool isR758Satisfied = clientCoopPdu.controlPduData.action == action_Values.CTRLACTION_REQUEST_CONTROL ||
                clientCoopPdu.controlPduData.action == action_Values.CTRLACTION_GRANTED_CONTROL ||
                clientCoopPdu.controlPduData.action == action_Values.CTRLACTION_DETACH ||
                clientCoopPdu.controlPduData.action == action_Values.CTRLACTION_COOPERATE;
            site.CaptureRequirementIfIsTrue(isR758Satisfied, 758,
                @"In TS_CONTROL_PDU, the action field must take one of the following values  1.CTRLACTION_REQUEST_CONTROL"
                + @" 0x0001 - Request control 2.CTRLACTION_GRANTED_CONTROL 0x0002 - Granted control 3.CTRLACTION_DETACH "
                + @"0x0003 - Detach 4.CTRLACTION_COOPERATE 0x0004 - Cooperate");
        }

        /// <summary>
        /// 2.2.1.16
        /// </summary>
        /// <param name="clientCtlRequestPdu"></param>
        public void VerifyPdu(Client_Control_Pdu_Request_Control clientCtlRequestPdu)
        {
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(clientCtlRequestPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 771,
                    @"[In Client Control PDU - Request Control]securityHeader (variable):  The securityHeader in Server "
                    + @"Demand Active PDU is a Non-FIPS Security Header (section 2.2.8.1.1.2.2) if the Encryption LevelMethod"
                    + @" selected by the server (see sections 5.3.2 and 2.2.1.4.3) is ENCRYPTION_LEVEL_CLIENT_COMPATIBLE (2)"
                    + @"METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT (0x00000008), or ENCRYPTION_LEVEL_HIGH (3METHOD_128BIT"
                    + @" (0x00000002).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(clientCtlRequestPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 772,
                    @"[In Client Control PDU - Request Control]securityHeader (variable):The securityHeader in Server Demand Active "
                    + @"PDU is a FIPS Security Header,if the Encryption LevelMethod selected by the server is ENCRYPTION_METHOD_FIPS (0x00000010).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                site.CaptureRequirementIfIsNull(clientCtlRequestPdu.commonHeader.securityHeader, 773,
                    @"For Client Control PDU - Request Control, if Enhanced RDP Security is in effect or the Encryption Method selected "
                    + @"by the server is ENCRYPTION_METHOD_NONE (0), then securityHeader MUST NOT be included in the Client Control PDU"
                    + @" - Request Control.");
            }
            site.CaptureRequirementIfIsTrue(clientCtlRequestPdu.controlPduData.controlId == 0 && clientCtlRequestPdu.controlPduData.grantId == 0, 775,
                @"In Client Control PDU - Request Control, the grantId and controlId fields of the Control PDU Data MUST both be set to zero.");

            site.CaptureRequirementIfAreEqual<action_Values>(action_Values.CTRLACTION_REQUEST_CONTROL, clientCtlRequestPdu.controlPduData.action, 776,
                @"In Client Control PDU - Request Control,the action field of the Control PDU Data MUST be set to CTRLACTION_REQUEST_CONTROL (0x0001).");
        }

        /// <summary>
        /// 2.2.1.17
        /// </summary>
        /// <param name="keyListPdu"></param>
        public void VerifyPdu(Client_Persistent_Key_List_Pdu keyListPdu)
        {
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(keyListPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 785,
                    @"In Client Persistent Key List PDU, securityHeader (variable):  The securityHeader  is a Non-FIPS Security Header"
                    + @" (section 2.2.8.1.1.2.2) if the Encryption LevelMethod selected by the server (see sections 5.3.2 and 2.2.1.4.3)"
                    + @" is ENCRYPTION_LEVEL_CLIENT_COMPATIBLE (2)METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT (0x00000008), or "
                    + @"ENCRYPTION_LEVEL_HIGH (3METHOD_128BIT (0x00000002).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(keyListPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 786,
                    @"In Client Persistent Key List PDU, securityHeader (variable):The securityHeaderis a FIPS Security Header,if the"
                    + @" Encryption LevelMethod selected by the server is ENCRYPTION_METHOD_FIPS (0x00000010).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                site.CaptureRequirementIfIsNull(keyListPdu.commonHeader.securityHeader, 787,
                    @"In Client Persistent Key List PDU, if Enhanced RDP Security is in effect or the Encryption Method selected by "
                    + @"the server is ENCRYPTION_METHOD_NONE (0), then the securityHeader MUST NOT be included in the Client"
                    + @" Persistent Key List PDU.");
            }

            site.CaptureRequirementIfAreEqual<int>(7, (keyListPdu.persistentKeyListPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xf), 793,
                @"In Client Persistent Key List PDU, the type subfield of the pduType field of the Share Control Header of Client "
                + @"Persistent Key List PDU MUST be set to PDUTYPE_DATAPDU (7). ");

            site.CaptureRequirementIfAreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_BITMAPCACHE_PERSISTENT_LIST, keyListPdu.persistentKeyListPduData.shareDataHeader.pduType2, 794,
                @"In Client Persistent Key List PDU, the pduType2 field of the Share Data Header in Client Persistent Key List PDU"
                + @" MUST be set to PDUTYPE2_BITMAPCACHE_PERSISTENT_LIST (43).");

            //2.22.17.1
            //CaptureRequirement(keyListPdu.persistentKeyListPduData.numEntriesCache0 == keyListPdu.persistentKeyListPduData.
        }

        /// <summary>
        /// 2.2.1.18
        /// </summary>
        /// <param name="fontListPdu"></param>
        public void VerifyPdu(Client_Font_List_Pdu fontListPdu)
        {
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(fontListPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 845,
                    @"In Client Font List PDU, securityHeader (variable): The securityHeader in Server Demand Active PDU is a Non-FIPS"
                    + @" Security Header (section 2.2.8.1.1.2.2) if the Encryption LevelMethod selected by the server (see sections 5.3.2"
                    + @" and 2.2.1.4.3) is ENCRYPTION_LEVEL_CLIENT_COMPATIBLE (2)METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT "
                    + @"(0x00000008), or ENCRYPTION_LEVEL_HIGH (3METHOD_128BIT (0x00000002).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(fontListPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 846,
                    @"In Client Font List PDU, securityHeader (variable): The securityHeader in Server Demand Active PDU is a FIPS Security"
                    + @" Header,if the Encryption LevelMethod selected by the server is ENCRYPTION_METHOD_FIPS (0x00000010).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                site.CaptureRequirementIfIsNull(fontListPdu.commonHeader.securityHeader, 847,
                    @"In the Client Font List PDU, if Enhanced RDP Security is in effect or the Encryption Method selected by the server "
                    + @"is ENCRYPTION_METHOD_NONE (0), then securityHeader MUST NOT be included in the Client Font List PDU.");
            }

            site.CaptureRequirementIfAreEqual<int>(7, (fontListPdu.fontListPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xf), 851,
                @"In TS_FONT_LIST_PDU, the type subfield of the pduType field of the Share Control Header MUST be set to PDUTYPE_DATAPDU (7).");

            site.CaptureRequirementIfAreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_FONTLIST, fontListPdu.fontListPduData.shareDataHeader.pduType2, 852,
                @"In TS_FONT_LIST_PDU, the pduType2 field of the Share Data Header MUST be set to PDUTYPE2_FONTLIST (39).");

            //2.2.1.18.1
            site.CaptureRequirementIfAreEqual<ushort>(0, fontListPdu.fontListPduData.numberFonts, 854,
                @"In TS_FONT_LIST_PDU, numberFonts field SHOULD be set to 0.");
            site.CaptureRequirementIfAreEqual<ushort>(0, fontListPdu.fontListPduData.totalNumFonts, 856,
                @"In TS_FONT_LIST_PDU, totalNumFonts field SHOULD be set to 0.");
            site.CaptureRequirementIfAreEqual<ushort>(0x0003, fontListPdu.fontListPduData.listFlags, 858,
                @"In TS_FONT_LIST_PDU, listFlags field SHOULD be set to 0x0003 which is the logical OR'ed value of FONTLIST_FIRST (0x0001)"
                + @" and FONTLIST_LAST (0x0002).");
            site.CaptureRequirementIfAreEqual<ushort>(0x0032, fontListPdu.fontListPduData.entrySize, 860,
                @"In TS_FONT_LIST_PDU, entrySize field SHOULD be set to 0x0032 (50 bytes).");
        }

        /// <summary>
        /// 2.2.2.1
        /// </summary>
        /// <param name="shutdownPdu"></param>
        public void VerifyPdu(Client_Shutdown_Request_Pdu shutdownPdu)
        {
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(shutdownPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 919,
                    @"[In Client Shutdown Request PDU,securityHeader (variable) is a Non-FIPS Security Header (section 2.2.8.1.1.2.2)"
                    + @" if the Encryption Method selected by the server is ENCRYPTION_METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT"
                    + @" (0x00000008), or ENCRYPTION_METHOD_128BIT (0x00000002).");

            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(shutdownPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 920,
                    @"[In Client Shutdown Request PDU,securityHeader (variable) is a FIPS Security Header (section 2.2.8.1.1.2.3) if the "
                    + @"Encryption Method selected by the server is ENCRYPTION_METHOD_FIPS (0x00000010).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                site.CaptureRequirementIfIsNull(shutdownPdu.commonHeader.securityHeader, 921,
                    @"[In Client Shutdown Request PDU,securityHeader (variable):If the Encryption Level (sections 5.3.2 and 2.2.1.4.3)"
                    + @" selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method (sections 5.3.2 and 2.2.1.4.3) "
                    + @"selected by the server is ENCRYPTION_METHOD_NONE (0), then this header is not included in the PDU.");
            }
            site.CaptureRequirementIfAreEqual<byte>(7, (byte)(shutdownPdu.shutdownRequestPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xF), 925,
                @"In Shutdown Request PDU Data (TS_SHUTDOWN_REQ_PDU), shareDataHeader (18 bytes): The type subfield of the pduType field"
                + @" of the Share Control Header (section 2.2.8.1.1.1.1) MUST be set to PDUTYPE_DATAPDU (7).");
            site.CaptureRequirementIfAreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_SHUTDOWN_REQUEST, shutdownPdu.shutdownRequestPduData.shareDataHeader.pduType2, 926,
                @"In Shutdown Request PDU Data (TS_SHUTDOWN_REQ_PDU), shareDataHeader (18 bytes): The pduType2 field of the Share Data "
                + @"Header MUST be set to PDUTYPE2_SHUTDOWN_REQUEST (36).");
        }

        /// <summary>
        /// 2.2.2.3
        /// </summary>
        /// <param name="disconnectPdu"></param>
        public void VerifyPdu(MCS_Disconnect_Provider_Ultimatum_Pdu disconnectPdu)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPdu"></param>
        public void VerifyPdu(TS_INPUT_PDU inputPdu)
        {
            int versionLow = inputPdu.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow >> 4 & 0x0F;
            site.CaptureRequirementIfAreEqual<int>(1, versionLow, 1542,
                @"In TS_SHARECONTROLHEADER structure, for PduType's subfield, versionLow field MUST be set to TS_PROTOCOL_VERSION (0x1).");
            // not capture 1543
            //CaptureRequirement(inputPdu.shareDataHeader.shareControlHeader.pduType.versionHigh == 0, 1543);
            site.CaptureRequirementIfAreEqual<versionHigh_Values>(versionHigh_Values.V1, inputPdu.shareDataHeader.shareControlHeader.pduType.versionHigh, 1544,
                @"In TS_SHARECONTROLHEADER, for PduType's subfield, the versionHigh field must be 1 byte which is the most "
                + @"significant byte of pduType field.");

            site.CaptureRequirementIfIsTrue(inputPdu.shareDataHeader.streamId == streamId_Values.STREAM_UNDEFINED ||
                inputPdu.shareDataHeader.streamId == streamId_Values.STREAM_LOW ||
                inputPdu.shareDataHeader.streamId == streamId_Values.STREAM_MED ||
                inputPdu.shareDataHeader.streamId == streamId_Values.STREAM_HI,
                1554,
                @"In TS_SHAREDATAHEADER structure,  the streamId can be the following: STREAM_UNDEFINED 0x00 ,STREAM_LOW"
                + @" 0x01,STREAM_MED 0x02,STREAM_HI 0x04");

            //R1612 cannot be verified
            if (inputPdu.commonHeader.securityHeader != null && (inputPdu.commonHeader.securityHeader.flags & TS_SECURITY_HEADER_flags_Values.SEC_FLAGSHI_VALID) == TS_SECURITY_HEADER_flags_Values.SEC_FLAGSHI_VALID)
                CaptureRequirement(inputPdu.commonHeader.securityHeader.flagsHi != 0, 1611);

            if (inputPdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
            {
                TS_SECURITY_HEADER2 header2 = (TS_SECURITY_HEADER2)inputPdu.commonHeader.securityHeader;
                site.CaptureRequirementIfAreEqual<TS_SECURITY_HEADER2_length_Values>(TS_SECURITY_HEADER2_length_Values.V1, header2.length, 1619,
                    @"The TS_SECURITY_HEADER2 structure,the length field MUST be set to 0x0010 (16 bytes) for legacy reasons.");
                //have defined ConstValue.TSFIPS_VERSION1 in sdk
                site.CaptureRequirementIfAreEqual<byte>(1, header2.version, 1621,
                    @"In TS_SECURITY_HEADER2 structure,the version field SHOULD be set to TSFIPS_VERSION1 (0x01).");
            }


            //2.2.8.1.1.3
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(inputPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 1630,
                    @"In Client Input Event PDU (TS_INPUT_PDU), If Standard RDP Security is in effect and the "
                    + @"Encryption Method selected by the server is greater than ENCRYPTION_METHOD_NONE (0),"
                    + @" then this securityHeader field will contain Non-FIPS Security Header (section 2.2.8.1.1.2.2)"
                    + @" if the Encryption Level Method selected by the server (see sections 5.3.2 and 2.2.1.4.3)"
                    + @" is ENCRYPTION_LEVEL_LOW (1METHOD_40BIT (0x00000001), ENCRYPTION_LEVEL_CLIENT_COMPATIBLE"
                    + @" (2METHOD_56BIT (0x00000008), or ENCRYPTION_LEVEL_HIGH (3). METHOD_128BIT (0x00000002).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(inputPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 1631,
                    @"In Client Input Event PDU (TS_INPUT_PDU),If Standard RDP Security is in effect and the "
                    + @"Encryption Method selected by the server is greater than ENCRYPTION_METHOD_NONE (0),"
                    + @" then this securityHeader field will contain FIPS Security Header (section 2.2.8.1.1.2.3)"
                    + @" if the Encryption LevelMethod selected by the server (see sections 5.3.2 and 2.2.1.4.3) "
                    + @"is ENCRYPTION_METHOD_FIPS (0x00000010). ");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                site.CaptureRequirementIfIsNull(inputPdu.commonHeader.securityHeader, 1632,
                    @"In Client Input Event PDU (TS_INPUT_PDU),If Enhanced RDP Security is in effect or the "
                    + @"Encryption Method selected by the server  is ENCRYPTION_METHOD_NONE (0), then this "
                    + @"securityHeader field MUST NOT be included in the PDU.");
            }

            //2.2.8.1.1.3.1            
            site.CaptureRequirementIfAreEqual<ShareControlHeaderType>(ShareControlHeaderType.PDUTYPE_DATAPDU, (ShareControlHeaderType)(inputPdu.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0x0F), 1635,
                @"In Client Input Event PDU (TS_INPUT_PDU),the shareDataHeader field contains information about"
                + @" the packet. The type subfield of the pduType field of the Share Control Header MUST be set"
                + @" to PDUTYPE_DATAPDU (7). ");
            site.CaptureRequirementIfAreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_INPUT, inputPdu.shareDataHeader.pduType2, 1636,
                @"In Client Input Event PDU (TS_INPUT_PDU),the pduType2 field of the Share Data Header MUST be "
                + @"set to PDUTYPE2_INPUT (28).");

            //2.2.8.1.1.3.1.1
            foreach (TS_INPUT_EVENT e in inputPdu.slowPathInputEvents)
            {
                VerifyStructure(e);
            }
        }

        /// <summary>
        /// 2.2.8.1.2 Client Fast-Path Input Event PDU (TS_FP_INPUT_PDU)
        /// </summary>
        /// <param name="fpInputPdu"></param>
        public void VerifyPdu(TS_FP_INPUT_PDU fpInputPdu)
        {
            int actionCode;
            int numberEvents;
            int encryptionFlags;
            TS_FP_INPUT_EVENT e;

            actionCode = fpInputPdu.fpInputHeader.actionCode & 0x3;
            numberEvents = (fpInputPdu.fpInputHeader.actionCode & 0x3c) >> 2;
            encryptionFlags = (fpInputPdu.fpInputHeader.actionCode & 0xc0) >> 6;

            CaptureRequirement(actionCode == 0 || actionCode == 3, 1700);
            if (fpInputPdu.numberEvents != 0)
            {
                CaptureRequirement(numberEvents == 0, 1704);
            }

            if (this.serverConfig.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                site.CaptureRequirementIfAreEqual<TS_FP_FIPS_INFO_length_Values>(TS_FP_FIPS_INFO_length_Values.V1, fpInputPdu.fipsInformation.length, 1713,
                    @"In TS_FP_INPUT_PDU structure, the fipsInformation is present when the Encryption Level "
                    + @"selected by the server is ENCRYPTION_LEVEL_FIPS (4).In TS_FP_INPUT_PDU structure, the"
                    + @" fipsInformation is present when the Encryption Level selected by the server is "
                    + @"ENCRYPTION_LEVEL_FIPS (4).");
            }

            Console.WriteLine("encryptionFlags= " + encryptionFlags);
            if (fpInputPdu.dataSignature == null)
                Console.WriteLine("fpInputPdu.dataSignature is null");

            if (encryptionFlags == (int)encryptionFlags_Values.FASTPATH_INPUT_ENCRYPTED)
            {
                site.CaptureRequirementIfIsNotNull(fpInputPdu.dataSignature, 1715,
                    @"In TS_FP_INPUT_PDU structure, the dataSignature MUST be present if the FASTPATH_INPUT_ENCRYPTED"
                    + @" flag is set in the fpInputHeader field.");
            }

            if (numberEvents == 0)
            {
                //or fpInputPdu.numberEvents!=0
                site.CaptureRequirementIfIsNotNull(fpInputPdu.numberEvents, 1717,
                    @"In TS_FP_INPUT_PDU structure, the numberEvents field is present if the numberEvents bit field in"
                    + @" the fast-path header byte is zero.");
            }

            if (fpInputPdu.numberEvents == 0)
            {
                site.CaptureRequirementIfAreEqual<int>(numberEvents, fpInputPdu.fpInputEvents.Count, 1718,
                      @"In TS_FP_INPUT_PDU structure, the number of events present in fpInputEvents array is "
                      + @"given by the numberEvents bit field in the fast-path header byte, or by the numberEvents"
                      + @" field in the Fast-Path Input Event PDU (if it is present).");
            }
            else
            {
                site.CaptureRequirementIfAreEqual<int>(fpInputPdu.numberEvents, fpInputPdu.fpInputEvents.Count, 1718,
                    @"In TS_FP_INPUT_PDU structure, the number of events present in fpInputEvents array is given"
                    + @" by the numberEvents bit field in the fast-path header byte, or by the numberEvents field "
                    + @"in the Fast-Path Input Event PDU (if it is present).");
            }

            if (this.serverConfig.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                site.CaptureRequirementIfAreEqual<TS_FP_FIPS_INFO_length_Values>(TS_FP_FIPS_INFO_length_Values.V1, fpInputPdu.fipsInformation.length, 1721,
                    @"In TS_FP_FIPS_INFO structure, the length field MUST be set to 0x0010 (16 bytes).");
                //ConstValue.TSFIPS_VERSION1
                site.CaptureRequirementIfAreEqual<byte>(1, fpInputPdu.fipsInformation.version, 1723,
                    @"In TS_FP_FIPS_INFO structure, the version field SHOULD be set to TSFIPS_VERSION1 (0x01).");
            }

            if (numberEvents == 0)
                numberEvents = fpInputPdu.numberEvents;

            for (int i = 0; i < fpInputPdu.fpInputEvents.Count; i++)
            {
                e = fpInputPdu.fpInputEvents[i];
                VerifyStructure(e);
            }
        }

        /// <summary>
        /// 2.2.11.2
        /// </summary>
        /// <param name="refreshPdu"></param>
        public void VerifyPdu(Client_Refresh_Rect_Pdu refreshPdu)
        {
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(refreshPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 2158,
                    @"In Client Refresh Rect PDU, The securityHeader in Server Set Keyboard Indicators PDU is a "
                    + @"Non-FIPS Security Header (section 2.2.8.1.1.2.2) if the Encryption Level Method selected"
                    + @" by the server (see sections 5.3.2 and 2.2.1.4.3) is ENCRYPTION_LEVEL_CLIENT_COMPATIBLE "
                    + @"(2)METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT (0x00000008), or ENCRYPTION_LEVEL_HIGH"
                    + @" (3METHOD_128BIT (0x00000002). ");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(refreshPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 2159,
                    @"In Client Refresh Rect PDU, The securityHeader in Server Set Keyboard Indicators PDU is a "
                    + @"FIPS Security Header if the Encryption Level Method selected by the server is "
                    + @"ENCRYPTION_METHOD_FIPS (0x00000010).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                site.CaptureRequirementIfIsNull(refreshPdu.commonHeader.securityHeader, 2160,
                    @"In Client Refresh Rect PDU, If Enhanced RDP Security  is in effect or the Encryption Method"
                    + @" selected by the server  is ENCRYPTION_METHOD_NONE (0), then securityHeader  MUST NOT be"
                    + @" included in the PDU.");
            }
            site.CaptureRequirementIfAreEqual<ShareControlHeaderType>((ShareControlHeaderType)(refreshPdu.refreshRectPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xF), ShareControlHeaderType.PDUTYPE_DATAPDU, 2163,
                @"In Refresh Rect PDU Data,the type subfield of the pduType field of the Share Control Header MUST"
                + @" be set to PDUTYPE_DATAPDU (7).");
            site.CaptureRequirementIfAreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_REFRESH_RECT, refreshPdu.refreshRectPduData.shareDataHeader.pduType2, 2164,
                @"In Refresh Rect PDU Data,the pduType2 field of the Share Data Header MUST be set to PDUTYPE2_REFRESH_RECT (33).");
            site.CaptureRequirementIfAreEqual<int>(refreshPdu.refreshRectPduData.numberOfAreas, refreshPdu.refreshRectPduData.areasToRefresh.Count, 2168,
                @"In Refresh Rect PDU Data(TS_REFRESH_RECT_PDU), the areasToRefresh field is an array of TS_RECTANGLE16"
                + @" structures (variable number of bytes). Array of screen area Inclusive Rectangles to redraw. The number"
                + @" of rectangles is given by the numberOfAreas field.");
        }

        /// <summary>
        /// 2.2.11.3
        /// </summary>
        /// <param name="suppressPdu"></param>
        public void VerifyPdu(Client_Suppress_Output_Pdu suppressPdu)
        {
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.CaptureRequirementIfIsInstanceOfType(suppressPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1), 2174,
                    @"[In Client Suppress Output PDU]The securityHeader in Server Set Keyboard Indicators PDU is a Non-FIPS "
                    + @"Security Header (section 2.2.8.1.1.2.2) if the Encryption Level Method selected by the server (see "
                    + @"sections 5.3.2 and 2.2.1.4.3) is ENCRYPTION_LEVEL_CLIENT_COMPATIBLE (2)METHOD_40BIT (0x00000001), "
                    + @"ENCRYPTION_METHOD_56BIT (0x00000008), or ENCRYPTION_LEVEL_HIGH (3METHOD_128BIT (0x00000002). ");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.CaptureRequirementIfIsInstanceOfType(suppressPdu.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2), 2175,
                    @"In Client Suppress Output PDU, the securityHeader in Server Set Keyboard Indicators PDU is a FIPS "
                    + @"Security Header if the Encryption Level Method selected by the server is ENCRYPTION_METHOD_FIPS"
                    + @" (0x00000010).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                site.CaptureRequirementIfIsNull(suppressPdu.commonHeader.securityHeader, 2176,
                    @"In Client Suppress Output PDU,If Enhanced RDP Security  is in effect or the Encryption Method selected"
                    + @" by the server is ENCRYPTION_METHOD_NONE (0), then this securityHeader MUST NOT be included in the PDU.");
            }
            site.CaptureRequirementIfAreEqual<ShareControlHeaderType>(ShareControlHeaderType.PDUTYPE_DATAPDU, (ShareControlHeaderType)(suppressPdu.suppressOutputPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xF), 2179,
                @"In Suppress Output PDU Data, the type subfield of the pduType field of the Share Control Header (section 2.2.8.1.1.1.1)"
                + @" MUST be set to PDUTYPE_DATAPDU (7).");
            site.CaptureRequirementIfAreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_SUPPRESS_OUTPUT, suppressPdu.suppressOutputPduData.shareDataHeader.pduType2, 2180,
                @"In Suppress Output PDU Data, the type subfield of the pduType2 field of the Share Data Header MUST be set to "
                + @"PDUTYPE2_SUPPRESS_OUTPUT (35).");
            bool isR2182Satisfied = suppressPdu.suppressOutputPduData.allowDisplayUpdates == AllowDisplayUpdates_SUPPRESS_OUTPUT.ALLOW_DISPLAY_UPDATES || suppressPdu.suppressOutputPduData.allowDisplayUpdates == AllowDisplayUpdates_SUPPRESS_OUTPUT.SUPPRESS_DISPLAY_UPDATES;
            site.CaptureRequirementIfIsTrue(isR2182Satisfied, 2182,
                @"In Suppress Output PDU Data, allowDisplayUpdates can be of two flags: SUPPRESS_DISPLAY_UPDATES 0x00, ALLOW_DISPLAY_UPDATES 0x01");
            if (suppressPdu.suppressOutputPduData.allowDisplayUpdates == AllowDisplayUpdates_SUPPRESS_OUTPUT.ALLOW_DISPLAY_UPDATES)
            {
                bool isR2187Satisified = suppressPdu.suppressOutputPduData.desktopRect.right - suppressPdu.suppressOutputPduData.desktopRect.left != 0 &&
                    suppressPdu.suppressOutputPduData.desktopRect.bottom - suppressPdu.suppressOutputPduData.desktopRect.top != 0;
                site.CaptureRequirementIfIsTrue(isR2187Satisified, 2187,
                    @"In Suppress Output PDU Data, the desktopRect field contains the coordinates of the desktop rectangle if"
                    + @" the allowDisplayUpdates field is set to ALLOW_DISPLAY_UPDATES (1).");
            }
            else
            {
                bool isR2188Satisified = suppressPdu.suppressOutputPduData.desktopRect.right - suppressPdu.suppressOutputPduData.desktopRect.left == 0 &&
                    suppressPdu.suppressOutputPduData.desktopRect.bottom - suppressPdu.suppressOutputPduData.desktopRect.top == 0;
                site.CaptureRequirementIfIsTrue(isR2188Satisified, 2188,
                    @"In Suppress Output PDU Data, the desktopRect field MUST NOT be included in the PDU, if the "
                    + @"allowDisplayUpdates field is set to SUPPRESS_DISPLAY_UPDATES (0).");
            }
        }

        #endregion "Check PDUs"

        #region "Check structure"

        public void VerifyStructure(TS_UD_CS_CORE core)
        {
            Site.CaptureRequirementIfIsTrue(core.header.type == TS_UD_HEADER_type_Values.CS_CORE,
                    93,
                    "In Client Core Data, The User Data Header type field MUST be set to CS_CORE (0xC001).");
            //<capture> also capture 97
            Site.CaptureRequirementIfIsTrue(
                core.version == version_Values.V1
                || core.version == version_Values.V2
                || core.version == version_Values.V3
                || core.version == version_Values.V4
                || core.version == version_Values.V5
                || core.version == version_Values.V6,
                98,
                string.Format("In Client Core Data, RDP client version number should be one of {0}, {1}, {2}, {3}, {4}, {5}.",
                version_Values.V1.ToString(),
                version_Values.V2.ToString(),
                version_Values.V3.ToString(),
                version_Values.V4.ToString(),
                version_Values.V5.ToString(),
                version_Values.V6.ToString())
                );
            Site.CaptureRequirementIfIsTrue(core.desktopWidth >= 0,
                100,
                "In Client Core Data, desktopWidth must be positive.");
            Site.CaptureRequirementIfIsTrue(core.desktopHeight >= 0,
                103,
                "In Client Core Data, desktopHeight must be positive.");
            Site.CaptureRequirementIfIsTrue((int)core.colorDepth == 51712 || core.colorDepth == colorDepth_Values.RNS_UD_COLOR_8BPP,
                107,
                "In Client Core Data (TS_UD_CS_CORE), colorDepth (2 bytes): colorDepth field can have the following "
                + @"values :\r\n1.RNS_UD_COLOR_4BPP 0xCA00.\r\n2.RNS_UD_COLOR_8BPP 0xCA01.");
            Site.CaptureRequirementIfIsTrue(core.SASSequence == 0xAA03,
                111,
                "In Client Core Data, SASSequence SHOULD be set to RNS_UD_SAS_DEL (0xAA03).");
            Site.CaptureRequirementIfIsTrue(core.clientName.Length <= 15,
                118,
                "In Client Core Data,  clientName field contains up to 15 Unicode characters plus a null terminator.");
            Site.CaptureRequirementIfIsTrue(core.keyboardType >= keyboardType_Values.None && core.keyboardType <= keyboardType_Values.V7,
                120,
                "In Client Core Data,  keyboardType can take one of the following values:");
            Site.CaptureRequirementIfIsTrue(core.imeFileName.Length <= 31,
                133,
                "In Client Core Data, the imeFileName field contains up to 31 Unicode characters plus a null terminator.");
            bool isR136Satisfied = (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_4BPP || (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_8BPP
                || (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_16BPP_555 || (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_16BPP_565
                || (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_24BPP;
            site.CaptureRequirementIfIsTrue(isR136Satisfied, 136,
                @"In Client Core Data, postBeta2ColorDepth can take one of the following values: RNS_UD_COLOR_4BPP 0xCA00,"
                + @" RNS_UD_COLOR_8BPP 0xCA01, RNS_UD_COLOR_16BPP_555 0xCA02, RNS_UD_COLOR_16BPP_565 0xCA03, RNS_UD_COLOR_24BPP 0xCA04");
            Site.CaptureRequirementIfIsTrue((int)core.clientProductId.actualData == 1, 145,
                "In Client Core Data,  clientProductId SHOULD be initialized to 1.");
            Site.CaptureRequirementIfAreEqual<int>(0, (int)core.serialNumber.actualData, 149,
                "In Client Core Data, serialNumber SHOULD be initialized to 0. ");
            bool isR154Satisfied = (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V1 || (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V2
                || (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V3 || (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V4
                || (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V5;
            site.CaptureRequirementIfIsTrue(isR154Satisfied, 154,
                @"In Client Core Data, highColorDepth can have one of the following values 4,8,15,16,24");

            site.CaptureRequirementIfIsTrue(core.supportedColorDepths.actualData <= 15, 164,
                @"In Client Core Data, supportedColorDepths can have one of the following values RNS_UD_24BPP_SUPPORT 0x0001, "
                + @"RNS_UD_16BPP_SUPPORT 0x0002, RNS_UD_15BPP_SUPPORT 0x0004, RNS_UD_32BPP_SUPPORT 0x0008");

            ushort earlyCapabilityFlags = (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_ERRINFO_PDU | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_WANT_32BPP_SESSION
                | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_STATUSINFO_PDU | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_STRONG_ASYMMETRIC_KEYS | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_UNUSED | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_VALID_CONNECTION_TYPE | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_MONITOR_LAYOUT_PDU
                | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_NETWORK_AUTODETECT | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_DYNVC_GFX_PROTOCOL | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_DYNAMIC_TIME_ZONE | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_HEARTBEAT_PDU;
            ushort negEarlyCapabilityFlags = (ushort)~earlyCapabilityFlags;
            bool isR173Satisfy = ((ushort)core.earlyCapabilityFlags.actualData & negEarlyCapabilityFlags) == 0;

            site.CaptureRequirementIfIsTrue(isR173Satisfy, 173,
                    @"[In Client Core Data (TS_UD_CS_CORE)] earlyCapabilityFlags (2 bytes):In Client Core Data, the earlyCapabilityFlags"
                    + @" can be any of the following values: RNS_UD_CS_SUPPORT_ERRINFO_PDU 0x0001, RNS_UD_CS_WANT_32BPP_SESSION 0x0002, "
                    + @"RNS_UD_CS_SUPPORT_STATUSINFO_PDU 0x0004, RNS_UD_CS_STRONG_ASYMMETRIC_KEYS 0x0008, RNS_UD_CS_VALID_CONNECTION_TYPE"
                    + @" 0x0020, RNS_UD_CS_SUPPORT_MONITOR_LAYOUT_PDU 0x0040");


        }


        public void VerifyStructure(TS_UD_CS_SEC sec)
        {
            Site.CaptureRequirementIfAreEqual<TS_UD_HEADER_type_Values>(sec.header.type, TS_UD_HEADER_type_Values.CS_SECURITY, 209,
                "In Client Security Data (TS_UD_CS_SEC), User Data Header type field MUST be set to CS_SECURITY (0xC002).");
            uint encryptionMethods = (uint)encryptionMethod_Values._40BIT_ENCRYPTION_FLAG | (uint)encryptionMethod_Values._56BIT_ENCRYPTION_FLAG
                | (uint)encryptionMethod_Values._128BIT_ENCRYPTION_FLAG | (uint)encryptionMethod_Values.FIPS_ENCRYPTION_FLAG;
            uint negEncryptionMethods = ~encryptionMethods;
            if (sec.encryptionMethods != 0)
            {
                // non-French client
                bool isR213Satisfied = ((uint)sec.encryptionMethods & negEncryptionMethods) == 0;
                Site.CaptureRequirementIfIsTrue(isR213Satisfied, 213,
                    @"In Client Security Data, encryptionMethods MUST be specified at least one of the following values:  \r\n40BIT_ENCRYPTION_FLAG "
                    + @"0x00000001,\r\n128BIT_ENCRYPTION_FLAG 0x0000000,\r\n256BIT_ENCRYPTION_FLAG 0x00000008,\r\nFIPS_ENCRYPTION_FLAG 0x00000010");

                Site.CaptureRequirementIfAreEqual<uint>((uint)sec.extEncryptionMethods, 0,
                    222,
                    "In Client Security Data for non-French locale clients, the extEncryptionMethods field MUST be set to 0. ");
            }
            else
            {
                // French client
                bool isExtEncryptionMethodsSatisfied = ((uint)sec.extEncryptionMethods & negEncryptionMethods) == 0;
                Site.Assert.IsTrue(isExtEncryptionMethodsSatisfied,
                    @"In French locale clients, encryptionMethods MUST be set to zero and extEncryptionMethods MUST be specified at least one of the following values:"
                    + @"\r\n40BIT_ENCRYPTION_FLAG 0x00000001,\r\n128BIT_ENCRYPTION_FLAG 0x0000000,\r\n256BIT_ENCRYPTION_FLAG 0x00000008,\r\nFIPS_ENCRYPTION_FLAG 0x00000010");
            }
        }

        public void VerifyStructure(TS_UD_CS_NET net)
        {
            Site.CaptureRequirementIfAreEqual<TS_UD_HEADER_type_Values>(net.header.type, TS_UD_HEADER_type_Values.CS_NET,
                225,
                "In Client Network Data (TS_UD_CS_NET), the User Data Header type field MUST be set to CS_NET (0xC003).");
            Site.CaptureRequirementIfIsTrue(net.channelCount <= 31,
                228,
                "[In Client Network Data (TS_UD_CS_NET)]channelCount (4 bytes): In Client Network Data, the maximum allowed value for channelCount is 31.");
            Site.CaptureRequirementIfAreEqual<int>(net.channelDefArray.Count, (int)net.channelCount,
                230,
                "In Client Network Data, the number of CHANNEL_DEF structures which follows is given by the channelCount field.");
            const uint options = (uint)Channel_Options.INITIALIZED | (uint)Channel_Options.ENCRYPT_RDP
                | (uint)Channel_Options.ENCRYPT_SC | (uint)Channel_Options.ENCRYPT_CS
                | (uint)Channel_Options.PRI_HIGH | (uint)Channel_Options.PRI_MED
                | (uint)Channel_Options.PRI_LOW | (uint)Channel_Options.COMPRESS_RDP
                | (uint)Channel_Options.COMPRESS | (uint)Channel_Options.SHOW_PROTOCOL
                | (uint)Channel_Options.REMOTE_CONTROL_PERSISTENT;
            const uint negOptions = ~options;
            for (int i = 0; i < net.channelCount; i++)
            {
                site.CaptureRequirementIfIsTrue(((uint)net.channelDefArray[i].options & negOptions) == 0, 234,
                    @"In Channel Definition Structure (CHANNEL_DEF), the options field can take one of the following values"
                    + @" CHANNEL_OPTION_INITIALIZED 0x80000000, CHANNEL_OPTION_ENCRYPT_RDP 0x40000000, CHANNEL_OPTION_ENCRYPT_SC"
                    + @" 0x20000000, CHANNEL_OPTION_ENCRYPT_CS 0x10000000, CHANNEL_OPTION_PRI_HIGH 0x08000000, CHANNEL_OPTION_PRI_MED"
                    + @" 0x04000000,CHANNEL_OPTION_PRI_LOW 0x02000000, CHANNEL_OPTION_COMPRESS_RDP 0x00800000, CHANNEL_OPTION_COMPRESS"
                    + @" 0x00400000, CHANNEL_OPTION_SHOW_PROTOCOL 0x00200000, REMOTE_CONTROL_PERSISTENT 0x00100000");

            }
        }

        public void VerifyStructure(TS_UD_CS_CLUSTER cluster)
        {
            Site.CaptureRequirementIfAreEqual(cluster.header.type, TS_UD_HEADER_type_Values.CS_CLUSTER,
                248,
                "In Client Cluster Data (TS_UD_CS_CLUSTER), the User Data Header type field MUST be set to CS_CLUSTER (0xC004).");
            const uint flags = (uint)Flags_Values.REDIRECTION_SUPPORTED | (uint)Flags_Values.ServerSessionRedirectionVersionMask
                | (uint)Flags_Values.REDIRECTED_SESSIONID_FIELD_VALID | (uint)Flags_Values.REDIRECTED_SMARTCARD;
            const uint negFlags = ~flags;
            site.CaptureRequirementIfIsTrue(((uint)cluster.Flags & negFlags) == 0, 250,
                @"In Client Cluster Data (TS_UD_CS_CLUSTER), the flags field can take any of the following values: REDIRECTION_SUPPORTED"
                + @" 0x00000001, ServerSessionRedirectionVersionMask 0x0000003C, REDIRECTED_SESSIONID_FIELD_VALID 0x00000002, "
                + @"REDIRECTED_SMARTCARD 0x00000040");

        }

        public void VerifyStructure(TS_UD_CS_MONITOR monitor)
        {
            uint nTotalWidth = 0;
            uint nTotalHeight = 0;

            for (int i = 0; i < monitor.monitorCount; i++)
            {
                nTotalWidth += (monitor.monitorDefArray[i].right - monitor.monitorDefArray[i].left + 1);
                nTotalHeight += (monitor.monitorDefArray[i].bottom - monitor.monitorDefArray[i].top + 1);

                Site.CaptureRequirementIfIsTrue((monitor.monitorDefArray[i].right - monitor.monitorDefArray[i].left + 1) >= 200 &&
                (monitor.monitorDefArray[i].bottom - monitor.monitorDefArray[i].top + 1) >= 200,
                    268,
                    "The minimum permitted size of the virtual desktop is 200 x 200 pixels.");
            }

            Site.CaptureRequirementIfIsTrue(nTotalWidth <= 32766,
                266,
                "The maximum width of the virtual desktop resulting from the union of the monitors contained in the monitorDefArray"
                + @" field MUST NOT exceed 32766 pixels.");
            Site.CaptureRequirementIfIsTrue(nTotalHeight <= 32766,
                267,
                "Similarly, the maximum height of the virtual desktop resulting from the union of the monitors contained in the "
                + @"monitorDefArray field MUST NOT exceed 32766 pixels.");
            Site.CaptureRequirementIfIsTrue(monitor.header.type == TS_UD_HEADER_type_Values.CS_MONITOR,
                271,
                "[In Client Monitor Data (TS_UD_CS_MONITOR)]header (4 bytes):The User Data Header type field MUST be set to "
                + @"CS_MONITOR (0xC005).");
            Site.CaptureRequirementIfIsTrue(monitor.monitorCount <= 15,
                275,
                "[In Client Monitor Data (TS_UD_CS_MONITOR)]monitorCount (4 bytes):  The number of display monitor definitions "
                + @"in the monitorDefArray field (the maximum allowed is 16).");
            Site.CaptureRequirementIfIsTrue(monitor.monitorCount == monitor.monitorDefArray.Count,
                277,
                "[In Client Monitor Data (TS_UD_CS_MONITOR)]monitorDefArray (variable):  The number of TS_MONITOR_DEF structures"
                + @" is given by the monitorCount field.");

        }

        /// <summary>
        /// 2.2.1.11.1
        /// </summary>
        /// <param name="info"></param>
        public void VerifyStructure(TS_INFO_PACKET info)
        {
            //Unicode = 2 * Ansi
            site.CaptureRequirementIfAreEqual<int>(info.Domain.Length * 2, (int)info.cbDomain, 464,
                @"In the TS_INFO_PACKET structure, cbDomain represents the size in bytes of the character data in the Domain field."
                + @" This size excludes the length of the mandatory null terminator.");
            site.CaptureRequirementIfAreEqual<int>(info.UserName.Length * 2, (int)info.cbUserName, 466,
                @"In the TS_INFO_PACKET structure, cbUserName represents the size in bytes of the character data in the UserName "
                + @"field. This size excludes the length of the mandatory null terminator.");
            site.CaptureRequirementIfAreEqual<int>(info.Password.Length * 2, (int)info.cbPassword, 468,
                @"In the TS_INFO_PACKET structure, cbPassword represents the size in bytes of the character data in the Password "
                + @"field. This size excludes the length of the mandatory null terminator.");
            site.CaptureRequirementIfAreEqual<int>(info.AlternateShell.Length * 2, (int)info.cbAlternateShell, 470,
                @"In the TS_INFO_PACKET structure, cbAlternateShell represents the size in bytes of the character data in the "
                + @"AlternateShell field. This size excludes the length of the mandatory null terminator.");
            site.CaptureRequirementIfAreEqual<int>(info.WorkingDir.Length * 2, (int)info.cbWorkingDir, 472,
                @"In the TS_INFO_PACKET structure, cbWorkingDir represents the size in bytes of the character data in the WorkingDir"
                + @" field. This size excludes the length of the mandatory null terminator.");
            //<bug> Domain not contains /0
            //site.CaptureRequirementIfIsTrue(info.Domain.Contains("\0"), 476,
            //    @"In the TS_INFO_PACKET structure, Domain field MUST contain at least a null terminator character in ANSI or Unicode"
            //    + @" format (depending on the presence of the INFO_UNICODE flag)");
            //site.CaptureRequirementIfIsTrue(info.UserName.Contains("\0"), 480,
            //    @"In the TS_INFO_PACKET structure, UserName field MUST contain at least a null terminator character in ANSI or Unicode"
            //    + @" format (depending on the presence of the INFO_UNICODE flag)");
            //site.CaptureRequirementIfIsTrue(info.Password.Contains("\0"), 483,
            //    @"In the TS_INFO_PACKET structure, Password field MUST contain at least a null terminator character in ANSI or Unicode"
            //    + @" format (depending on the presence of the INFO_UNICODE flag)");
            site.CaptureRequirementIfIsTrue(info.AlternateShell.Length <= 512, 485,
                @"In the TS_INFO_PACKET structure, for AlternateShell, the maximum allowed length is 512 bytes (including the mandatory"
                + @" null terminator).");
            //site.CaptureRequirementIfIsTrue(info.AlternateShell.Contains("\0"), 487,
            //    @"In the TS_INFO_PACKET structure, AlternateShell field MUST contain at least a null terminator character in ANSI or "
            //    + @"Unicode format (depending on the presence of the INFO_UNICODE flag)");
            //site.CaptureRequirementIfIsTrue(info.WorkingDir.Contains("\0"), 491,
            //    @"In the TS_INFO_PACKET structure, WorkingDir field MUST contain at least a null terminator character in ANSI or "
            //    + @"Unicode format (depending on the presence of the INFO_UNICODE flag)");
            if (info.extraInfo != null)
            {
                //bugbug:
                //AF_INET 0x00002
                //AF_INET6 0x0017
                //<hardcode>
                if (!this.Site.Properties["RDP.Version"].Equals("8.1"))
                {
                    site.CaptureRequirementIfIsTrue(info.extraInfo.clientAddressFamily == clientAddressFamily_Values.V1 || (int)info.extraInfo.clientAddressFamily == 0x17, 495,
                        @"[In Extended Info Packet (TS_EXTENDED_INFO_PACKET)]clientAddressFamily (2 bytes):clientAddressFamily can have the "
                        + @"following values: 1.AF_INET 0x00002 2.AF_INET6 0x0017");
                }

                Console.WriteLine("RS503, clientDir.Length = " + info.extraInfo.clientDir.Length);
                Console.WriteLine("RS503, cbClientDir=" + info.extraInfo.cbClientDir);

                bool isR505Satisfied = info.extraInfo.cbClientDir <= 512;
                site.CaptureRequirementIfIsTrue(isR505Satisfied, 505,
                    @"[In Extended Info Packet (TS_EXTENDED_INFO_PACKET)] clientDir (variable):The maximum allowed length is 512 bytes "
                    + @"(including the mandatory null terminator).");

                bool isR528Satisfied = info.extraInfo.cbAutoReconnectLen <= 128;
                site.CaptureRequirementIfIsTrue(isR528Satisfied, 528,
                    @"[Extended Info Packet (TS_EXTENDED_INFO_PACKET)]autoReconnectCookie (28 bytes):the maximum allowed length "
                    + @"is 128 bytes.");
            }
        }
        /// <summary>
        /// 2.2.1.11.1.1.1
        /// </summary>
        /// <param name="zoneInfo"></param>
        public void VerifyStructure(TS_TIME_ZONE_INFORMATION zoneInfo)
        {
            site.CaptureRequirementIfIsTrue(System.Runtime.InteropServices.Marshal.SizeOf(zoneInfo.DaylightName) == 64, 547,
                @"In TS_TIME_ZONE_INFORMATION structure, DaylightName field must be 64 bytes");
            site.CaptureRequirementIfIsTrue(System.Runtime.InteropServices.Marshal.SizeOf(zoneInfo.DaylightDate) == 16, 549,
                @"In TS_TIME_ZONE_INFORMATION structure, DaylightDate must be 16 bytes");
            site.CaptureRequirementIfIsTrue(System.Runtime.InteropServices.Marshal.SizeOf(zoneInfo.DaylightBias) == 4, 553,
                @"In TS_TIME_ZONE_INFORMATION structure, DaylightBias must be 4 bytes");
        }
        /// <summary>
        /// 2.2.1.13.2.1
        /// </summary>
        /// <param name="confirmActive"></param>
        public void VerifyStructure(TS_CONFIRM_ACTIVE_PDU confirmActive)
        {
            site.CaptureRequirementIfAreEqual<ShareControlHeaderType>(ShareControlHeaderType.PDUTYPE_CONFIRMACTIVEPDU, (ShareControlHeaderType)(confirmActive.shareControlHeader.pduType.typeAndVersionLow & 0x15), 708,
                @"In TS_CONFIRM_ACTIVE_PDU structure, the type subfield of the pduType field of the Share Control Header MUST be set to"
                + @" PDUTYPE_CONFIRMACTIVEPDU (3).");
            site.CaptureRequirementIfAreEqual<originatorId_Values>(originatorId_Values.V1, confirmActive.originatorId, 711,
                @"In TS_CONFIRM_ACTIVE_PDU structure, the originatorId MUST be set to the server channel ID (in Microsoft RDP server "
                + @"implementations, this value is always 0x03EA)");
            site.CaptureRequirementIfAreEqual<ushort>((ushort)confirmActive.sourceDescriptor.Length, confirmActive.lengthSourceDescriptor, 713,
                @"In TS_CONFIRM_ACTIVE_PDU structure, the lengthSourceDescriptor gives the size in bytes of the sourceDescriptor field.");
            site.CaptureRequirementIfAreEqual<ushort>(confirmActive.numberCapabilities, (ushort)confirmActive.capabilitySets.Count, 718,
                @"In TS_CONFIRM_ACTIVE_PDU structure, numberCapabilities gives the number of capability sets included "
                + @"in the Confirm Active PDU.");
        }

        /// <summary>
        /// 2.2.7.1.1
        /// </summary>
        /// <param name="general"></param>
        public void VerifyStructure(TS_GENERAL_CAPABILITYSET general)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_GENERAL, general.capabilitySetType, 1043,
                @"In TS_GENERAL_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_GENERAL (1).");

            CaptureRequirement(general.osMajorType == osMajorType_Values.OSMAJORTYPE_MACINTOSH ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_OS2 ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_UNIX ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_WINDOWS ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_IOS ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_OSX ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_ANDROID ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_CHROME_OS,
                1047);
            CaptureRequirement(general.osMinorType >= osMinorType_Values.OSMINORTYPE_WINDOWS_31X && general.osMinorType <= osMinorType_Values.OSMINORTYPE_WINDOWS_RT, 1054);
            CaptureRequirement(general.protocolVersion == protocolVersion_Values.V1, 1065);
            CaptureRequirement(general.generalCompressionTypes == generalCompressionTypes_Values.V1, 1069);
            CaptureRequirement(general.updateCapabilityFlag == updateCapabilityFlag_Values.V1, 1080);
            CaptureRequirement(general.remoteUnshareFlag == remoteUnshareFlag_Values.V1, 1082);
        }

        /// <summary>
        /// 2.2.7.1.2
        /// </summary>
        /// <param name="bitmap"></param>
        public void VerifyStructure(TS_BITMAP_CAPABILITYSET bitmap)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_BITMAP, bitmap.capabilitySetType, 1096,
                @"In TS_BITMAP_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_BITMAP (2).");
            site.CaptureRequirementIfAreEqual<ushort>(1, bitmap.receive1BitPerPixel, 1104,
                @"In TS_BITMAP_CAPABILITYSET structure, the receive1BitPerPixel field indicates whether the client "
                + @"can receive 1 bit-per-pixel. This field is ignored and SHOULD be set to TRUE (0x0001).");
            site.CaptureRequirementIfAreEqual<ushort>(1, bitmap.receive4BitsPerPixel, 1106,
                @"In TS_BITMAP_CAPABILITYSET structure, the receive4BitsPerPixel field indicates whether the client"
                + @" can receive 4 bit-per-pixel. This field is ignored and SHOULD be set to TRUE (0x0001).");
            site.CaptureRequirementIfAreEqual<ushort>(1, bitmap.receive8BitsPerPixel, 1110,
                @"In TS_BITMAP_CAPABILITYSET structure, the receive8BitsPerPixel field indicates whether the client"
                + @" can receive 8 bit-per-pixel. This field is ignored during capability negotiation and MUST be set to "
                + @"TRUE (0x0001).");
            site.CaptureRequirementIfAreEqual<ushort>(1, bitmap.bitmapCompressionFlag, 1120,
                @"In TS_BITMAP_CAPABILITYSET structure, bitmapCompressionFlag Indicates whether the client supports "
                + @"bitmap compression.This field MUST be set to TRUE (0x0001) because support for compressed bitmaps"
                + @" is required for a connection to proceed.");
            site.CaptureRequirementIfAreEqual<byte>(0, bitmap.highColorFlags, 1123,
                @"In TS_BITMAP_CAPABILITYSET structure, highColorFlags field is ignored during capability negotiation "
                + @"and MUST be set to 0.");
            site.CaptureRequirementIfAreEqual<ushort>(1, bitmap.multipleRectangleSupport, 1131,
                @"In TS_BITMAP_CAPABILITYSET structure, multipleRectangleSupport Indicates whether the client supports"
                + @" the use of multiple bitmap rectangles. This field MUST be set to TRUE (0x0001) because multiple "
                + @"rectangle support is required for a connection to proceed.");
        }

        /// <summary>
        /// 2.2.7.1.3
        /// </summary>
        /// <param name="order"></param>
        public void VerifyStructure(TS_ORDER_CAPABILITYSET order)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_ORDER, order.capabilitySetType, 1137,
                @"In TS_ORDER_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_ORDER (3).");
            bool isR1141Satisfied = order.terminalDescriptor[0] == 0 &&
                order.terminalDescriptor[1] == 0 &&
                order.terminalDescriptor[2] == 0 &&
                order.terminalDescriptor[3] == 0 &&
                order.terminalDescriptor[4] == 0 &&
                order.terminalDescriptor[5] == 0 &&
                order.terminalDescriptor[6] == 0 &&
                order.terminalDescriptor[7] == 0 &&
                order.terminalDescriptor[8] == 0 &&
                order.terminalDescriptor[9] == 0 &&
                order.terminalDescriptor[10] == 0 &&
                order.terminalDescriptor[11] == 0 &&
                order.terminalDescriptor[12] == 0 &&
                order.terminalDescriptor[13] == 0 &&
                order.terminalDescriptor[14] == 0 &&
                order.terminalDescriptor[15] == 0;
            site.CaptureRequirementIfIsTrue(isR1141Satisfied, 1141,
                @"In TS_ORDER_CAPABILITYSET structure, the terminalDescriptor field is ignored during capability "
                + @"negotiation and SHOULD be set to all zeroes.");
            site.CaptureRequirementIfAreEqual<ushort>(1, order.maximumOrderLevel, 1152,
                @"In TS_ORDER_CAPABILITYSET structure, the maximumOrderLevel field is ignored during capability"
                + @" negotiation and SHOULD be set to ORD_LEVEL_1_ORDERS (1).");
            site.CaptureRequirementIfAreEqual<ushort>(0, order.numberFonts, 1155,
                @"In TS_ORDER_CAPABILITYSET structure, the numberFonts field is ignored during capability "
                + @"negotiation and SHOULD be set to 0.");
            site.CaptureRequirementIfIsTrue((order.orderFlags & orderFlags_Values.NEGOTIATEORDERSUPPORT) == orderFlags_Values.NEGOTIATEORDERSUPPORT, 1159,
                @"In TS_ORDER_CAPABILITYSET structure, if the orderFlags is  NEGOTIATEORDERSUPPORT 0x0002 that "
                + @"indicates support for negotiating drawing orders in the orderSupport field. This flag MUST be set in the orderFlags field.");
            site.CaptureRequirementIfIsTrue((order.orderFlags & orderFlags_Values.ZEROBOUNDSDELTASSUPPORT) == orderFlags_Values.ZEROBOUNDSDELTASSUPPORT, 1160,
                @"In TS_ORDER_CAPABILITYSET structure, if the orderFlags is  ZEROBOUNDSDELTASSUPPORT 0x0008 that"
                + @" indicates support for the order encoding flag for zero bounds delta coordinates. This flag"
                + @" MUST be set in the orderFlags field.");
        }

        /// <summary>
        /// 2.2.7.1.4.1
        /// </summary>
        /// <param name="cache"></param>
        public void VerifyStructure(TS_BITMAPCACHE_CAPABILITYSET cache)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_BITMAPCACHE, cache.capabilitySetType, 1215,
                @"In TS_BITMAPCACHE_CAPABILITYSET structure, the capabilitySetType field MUST be set to "
                + @"CAPSTYPE_BITMAPCACHE (4).");
            site.CaptureRequirementIfIsTrue(cache.Cache1Entries <= 200, 1231,
            @"In TS_BITMAPCACHE_CAPABILITYSET structure, the Cache0Entries gives the number of entries in"
            + @" Bitmap Cache 0 (maximum allowed value is 200 entries).");
            site.CaptureRequirementIfIsTrue(cache.Cache2Entries <= 600, 1236,
                @"In TS_BITMAPCACHE_CAPABILITYSET structure, Cache1Entries gives the number of entries in"
                + @" Bitmap Cache 1 (maximum allowed value is 600 entries).");
            site.CaptureRequirementIfIsTrue(cache.Cache3Entries <= 65535, 1241,
                @"In TS_BITMAPCACHE_CAPABILITYSET structure, Cache2Entries gives the number of entries in"
                + @" Bitmap Cache 2 (maximum allowed value is 65535 entries).");
        }

        /// <summary>
        /// 2.2.7.1.4.2
        /// </summary>
        /// <param name="cache2"></param>
        public void VerifyStructure(TS_BITMAPCACHE_CAPABILITYSET_REV2 cache2)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_REV2, cache2.capabilitySetType, 1248,
                @"In TS_BITMAPCACHE_CAPABILITYSET_REV2 structure capabilitySetType field MUST be set to "
                + @"CAPSTYPE_BITMAPCACHE_REV2 (19).");
            bool isR1252Satisfied = cache2.CacheFlags == CacheFlags_Values.ALLOW_CACHE_WAITING_LIST_FLAG ||
                cache2.CacheFlags == CacheFlags_Values.PERSISTENT_KEYS_EXPECTED_FLAG ||
                cache2.CacheFlags == (CacheFlags_Values.ALLOW_CACHE_WAITING_LIST_FLAG | CacheFlags_Values.PERSISTENT_KEYS_EXPECTED_FLAG) ||
                cache2.CacheFlags == CacheFlags_Values.None;
            site.CaptureRequirementIfIsTrue(isR1252Satisfied, 1252,
                @"In TS_BITMAPCACHE_CAPABILITYSET_REV2 structure,  the CacheFlags can be of the following"
                + @" PERSISTENT_KEYS_EXPECTED_FLAG 0x0001, ALLOW_CACHE_WAITING_LIST_FLAG 0x0002, None 0x0000");

            site.CaptureRequirementIfIsTrue(cache2.NumCellCaches <= 5, 1258,
                @"In TS_BITMAPCACHE_CAPABILITYSET_REV2 structure, the NumCellCaches field is the number of"
                + @" bitmap caches (with a maximum allowed value of 5). ");

            if (cache2.NumCellCaches >= 1)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache1CellInfo.NumEntriesAndK & 0xfffffff) <= 600, 1260);
            }

            if (cache2.NumCellCaches >= 2)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache2CellInfo.NumEntriesAndK & 0xfffffff) <= 600, 1263);
            }

            if (cache2.NumCellCaches >= 3)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache3CellInfo.NumEntriesAndK & 0xfffffff) <= 65536, 1266);
            }

            if (cache2.NumCellCaches >= 4)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache4CellInfo.NumEntriesAndK & 0xfffffff) <= 4096, 1269);
            }

            if (cache2.NumCellCaches >= 5)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache5CellInfo.NumEntriesAndK & 0xfffffff) <= 2048, 1272);
            }
        }

        /// <summary>
        /// 2.2.7.1.5
        /// </summary>
        /// <param name="pointer"></param>
        public void VerifyStructure(TS_POINTER_CAPABILITYSET pointer)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_POINTER, pointer.capabilitySetType, 1284,
                @"In TS_POINTER_CAPABILITY_SET structure, capabilitySetType gives the type of the capability"
                + @" set. This field MUST be set to CAPSTYPE_POINTER (8).");
        }

        /// <summary>
        /// 2.2.7.1.6 
        /// </summary>
        /// <param name="input"></param>
        public void VerifyStructure(TS_INPUT_CAPABILITYSET input)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_INPUT, input.capabilitySetType, 1298,
                @"In TS_INPUT_CAPABILITY_SET structure, the capabilitySetType field MUST be set to "
                + @"CAPSTYPE_INPUT (13).");
            bool isR1313Satisfied = input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V1 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V2 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V3 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V4 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V5 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V6 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V7 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.None;
            site.CaptureRequirementIfIsTrue(isR1313Satisfied, 1313,
                @"In TS_INPUT_CAPABILITY_SET  structure, the keyboardType field  can take the values 1,2,3,4,5,6,7,0");

            String imeFileName = input.imeFileName;
            Console.WriteLine("RS1328, imeFileName= " + imeFileName);
            Console.WriteLine("RS1328, imeFileName.Length = " + imeFileName.Length.ToString());
            //<?> should check is Unicode characters?
            //site.CaptureRequirementIfIsTrue(input.imeFileName.Length <= 32, 1328,
            //    @"In TS_INPUT_CAPABILITY_SET structure, the imeFileNamefield contains up to 31 Unicode characters plus a null terminator.");
        }

        /// <summary>
        /// 2.2.7.1.7
        /// </summary>
        /// <param name="brush"></param>
        public void VerifyStructure(TS_BRUSH_CAPABILITYSET brush)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_BRUSH, brush.capabilitySetType, 1332,
                @"In TS_BRUSH_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_BRUSH (15).");
            bool isR1336Satisfied = brush.brushSupportLevel == brushSupportLevel_Values.BRUSH_DEFAULT ||
                brush.brushSupportLevel == brushSupportLevel_Values.BRUSH_COLOR_8x8 ||
                brush.brushSupportLevel == brushSupportLevel_Values.BRUSH_COLOR_FULL;
            site.CaptureRequirementIfIsTrue(isR1336Satisfied, 1336,
                @"In TS_BRUSH_CAPABILITYSET structure, the brushSupportLevel can be the following: BRUSH_DEFAULT"
                + @" 0x00000000 ,BRUSH_COLOR_8x8 0x00000001,BRUSH_COLOR_FULL 0x00000002");
        }

        /// <summary>
        /// 2.2.7.1.8
        /// </summary>
        /// <param name="glyph"></param>
        public void VerifyStructure(TS_GLYPHCACHE_CAPABILITYSET glyph)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_GLYPHCACHE, glyph.capabilitySetType, 1342,
                @"In TS_GLYPHCACHE_CAPABILITYSET structure, capabilitySetType field MUST be set to CAPSTYPE_GLYPHCACHE (16).");
            site.CaptureRequirementIfIsTrue(glyph.FragCache.CacheEntries <= 256, 1346,
                @"In TS_GLYPHCACHE_CAPABILITYSET structure, the maximum number of entries allowed in a FragCache is"
                + @" 256, and the largest allowed maximum size of an element is 256 bytes.");
            bool isR1348Satisfied = glyph.GlyphSupportLevel == GlyphSupportLevel_Values.GLYPH_SUPPORT_NONE ||
                glyph.GlyphSupportLevel == GlyphSupportLevel_Values.GLYPH_SUPPORT_PARTIAL ||
                glyph.GlyphSupportLevel == GlyphSupportLevel_Values.GLYPH_SUPPORT_FULL ||
                glyph.GlyphSupportLevel == GlyphSupportLevel_Values.GLYPH_SUPPORT_ENCODE;
            site.CaptureRequirementIfIsTrue(isR1348Satisfied, 1348,
                @"In TS_GLYPHCACHE_CAPABILITYSET  structure, the GlyphSupportLevel  field  can be the following "
                + @"GLYPH_SUPPORT_NONE 0,GLYPH_SUPPORT_PARTIAL 1,GLYPH_SUPPORT_FULL 2,GLYPH_SUPPORT_ENCODE 3");
        }

        /// <summary>
        /// 2.2.7.1.9 
        /// Check the requirements in structure TS_OFFSCREEN_CAPABILITYSET
        /// </summary>
        /// <param name="offscreen"></param>
        public void VerifyStructure(TS_OFFSCREEN_CAPABILITYSET offscreen)
        {
            bool isSatisfied = offscreen.offscreenSupportLevel == offscreenSupportLevel_Values.TRUE ||
                offscreen.offscreenSupportLevel == offscreenSupportLevel_Values.FALSE;
            Site.Assert.IsTrue(isSatisfied, "offscreenSupportLevel field of TS_OFFSCREEN_CAPABILITYSET must be FALSE (0x00000000) or TRUE (0x00000001).");
        }

        /// <summary>
        /// 2.2.7.1.10
        /// Check the requirements in structure TS_VIRTUALCHANNEL_CAPABILITYSET.
        /// </summary>
        /// <param name="vc">The TS_VIRTUALCHANNEL_CAPABILITYSET structure</param>
        public void VerifyStructure(TS_VIRTUALCHANNEL_CAPABILITYSET vc)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL, vc.capabilitySetType, 1378,
                @"In TS_VIRTUALCHANNEL_CAPABILITYSET structure, capabilitySetType field MUST be set to "
                + @"CAPSTYPE_VIRTUALCHANNEL (20).");
        }

        /// <summary>
        /// 2.2.7.1.11
        /// Check the requirements in structure TS_SOUND_CAPABILITYSET.
        /// </summary>
        /// <param name="sound">The TS_SOUND_CAPABILITYSET structure.</param>
        public void VerifyStructure(TS_SOUND_CAPABILITYSET sound)
        {
            site.CaptureRequirementIfAreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_SOUND, sound.capabilitySetType, 1387,
                @"In TS_SOUND_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_SOUND (12).");
        }


        /// <summary>
        /// 2.2.8.1.1.3.1.1
        /// </summary>
        /// <param name="input"></param>
        public void VerifyStructure(TS_INPUT_EVENT input)
        {
            bool isR1645Satisfied = input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSE ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSEX ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SCANCODE ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SYNC ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNICODE ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNUSED;

            site.CaptureRequirementIfIsTrue(isR1645Satisfied, 1645,
                @"In TS_INPUT_EVENT structure,the messageType can be the following:INPUT_EVENT_SYNC 0x0000,"
                + @" INPUT_EVENT_SCANCODE 0x0004, INPUT_EVENT_UNICODE 0x0005, INPUT_EVENT_MOUSE 0x8001, "
                + @"INPUT_EVENT_MOUSEX 0x8002");

            switch (input.messageType)
            {
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSE:
                    site.CaptureRequirementIfIsInstanceOfType(input.slowPathInputData, typeof(TS_POINTER_EVENT), 1649,
                        @"In TS_INPUT_EVENT structure, when the messageType is INPUT_EVENT_MOUSE 0x8001 "
                        + @"then the input event is a Mouse Event.");
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSEX:
                    site.CaptureRequirementIfIsInstanceOfType(input.slowPathInputData, typeof(TS_POINTERX_EVENT), 1650,
                        @"In TS_INPUT_EVENT structure, when the messageType is INPUT_EVENT_MOUSEX 0x8002"
                        + @" then the input event is an Extended Mouse Event.");
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SCANCODE:
                    site.CaptureRequirementIfIsInstanceOfType(input.slowPathInputData, typeof(TS_KEYBOARD_EVENT), 1647,
                        @"In TS_INPUT_EVENT structure, when the messageType is INPUT_EVENT_SCANCODE 0x0004"
                        + @" then the input event is a Keyboard Event.");
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SYNC:
                    site.CaptureRequirementIfIsInstanceOfType(input.slowPathInputData, typeof(TS_SYNC_EVENT), 1646,
                        @"In TS_INPUT_EVENT structure, when the messageType is INPUT_EVENT_SYNC 0x0000 then"
                        + @" the input event is a Synchronize Event");
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNICODE:
                    site.CaptureRequirementIfIsInstanceOfType(input.slowPathInputData, typeof(TS_UNICODE_KEYBOARD_EVENT), 1648,
                        @"In TS_INPUT_EVENT structure, when the messageType is INPUT_EVENT_UNICODE 0x0005 "
                        + @"then the input event is a Unicode Keyboard Event.");
                    break;
            }

        }

        /// <summary>
        /// 2.2.8.1.2.2
        /// </summary>
        /// <param name="input"></param>
        public void VerifyStructure(TS_FP_INPUT_EVENT input)
        {
            int code;
            int flag;

            code = input.eventHeader.eventFlagsAndCode >> 5;
            flag = input.eventHeader.eventFlagsAndCode & 0x1F;
            bool isR1730Satisfied = code == 0 || code == 1 || code == 2 || code == 3 || code == 4;
            site.CaptureRequirementIfIsTrue(isR1730Satisfied, 1730,
                @"In TS_FP_INPUT_EVENT structure, eventCode can be the following: FASTPATH_INPUT_EVENT_SCANCODE"
                + @" 0x0,FASTPATH_INPUT_EVENT_MOUSE 0x1, FASTPATH_INPUT_EVENT_MOUSEX 0x2, FASTPATH_INPUT_EVENT_SYNC"
                + @" 0x3, FASTPATH_INPUT_EVENT_UNICODE 0x4");

            switch (code)
            {
                case 0:
                    site.CaptureRequirementIfIsInstanceOfType(input.eventData, typeof(TS_FP_KEYBOARD_EVENT), 1739,
                        @"In the eventHeader of the TS_FP_KEYBOARD_EVENT structure, the eventCode bit field (3 "
                        + @"bits in size) MUST be set to FASTPATH_INPUT_EVENT_SCANCODE (0).");
                    break;
                case 1:
                    site.CaptureRequirementIfIsInstanceOfType(input.eventData, typeof(TS_FP_POINTER_EVENT), 1750,
                        @"In the eventHeader of the TS_FP_POINTER_EVENT structure, the eventCode bit field (3 "
                        + @"bits in size) MUST be set to FASTPATH_INPUT_EVENT_MOUSE (1).");
                    site.CaptureRequirementIfAreEqual<int>(0, flag, 1751,
                        @"In the eventHeader of the TS_FP_POINTER_EVENT structure, the eventFlags bit field (5"
                        + @" bits in size) MUST be zeroed out.");
                    break;
                case 2:
                    site.CaptureRequirementIfIsInstanceOfType(input.eventData, typeof(TS_FP_POINTERX_EVENT), 1757,
                        @"In the eventHeader of the TS_FP_POINTERX_EVENT structure, the eventCode bit field (3"
                        + @" bits in size) MUST be set to FASTPATH_INPUT_EVENT_POINTERX (2).");
                    site.CaptureRequirementIfAreEqual<int>(0, flag, 1758,
                        @"In the eventHeader of the TS_FP_POINTERX_EVENT structure, the eventFlags bit field (5"
                        + @" bits in size) MUST be zeroed out.");
                    break;
                case 3:
                    site.CaptureRequirementIfIsTrue(input.eventData == null && code == 3, 1765,
                        @"In the eventHeader of the TS_FP_SYNC_EVENT structure, the eventCode bit field (3 bits"
                        + @" in size) MUST be set to FASTPATH_INPUT_EVENT_SYNC (3).");
                    break;
                case 4:
                    site.CaptureRequirementIfIsInstanceOfType(input.eventData, typeof(TS_FP_UNICODE_KEYBOARD_EVENT), 1744,
                        @"In the eventHeader of the TS_FP_UNICODE_KEYBOARD_EVENT structure, the eventCode bit field"
                        + @" (3 bits in size) MUST be set to FASTPATH_INPUT_EVENT_UNICODE (4).");
                    site.CaptureRequirementIfAreEqual<int>(0, flag, 1745,
                        @"In the eventHeader of the TS_FP_UNICODE_KEYBOARD_EVENT structure, the eventFlags bit field"
                        + @" (5 bits in size) MUST be zeroed out.");
                    break;
            }
        }

        #endregion "Check structure"

        #region "Helper functions"
        /// <summary>
        /// This function will load the RS info and output log information.
        /// It wraps the Site.CaptureRequirement function.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="id"></param>
        public void CaptureRequirement(bool value, int id)
        {
            if (value)
            {
                Console.WriteLine("Capture " + id.ToString());
            }
            else
            {
                Console.WriteLine("Capture " + id.ToString() + " , failed");
            }
        }

        #endregion "Helper functions"
    }
}
