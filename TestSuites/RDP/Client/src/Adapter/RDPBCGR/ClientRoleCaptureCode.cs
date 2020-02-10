// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using System;
using System.Linq;
using System.Text;

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
            site.Log.Add(TestTools.LogEntryKind.Debug, "CheckX224ConnectionRequestPdu", clientPdu);
            site.Assert.AreEqual<byte>(0, clientPdu.x224Crq.classOptions,
               @"[In Client X.224 Connection Request PDU] x224Crq (7 bytes): An X.224 Class 0 Connection Request "
               + @"transport protocol data unit (TPDU), as specified in [X224] section 13.3.");

            if (clientPdu != null && clientPdu.routingToken != null)
            {
                int len = clientPdu.routingToken.Length;
                site.Assert.IsTrue(clientPdu.routingToken[len - 2] == 0x0D && clientPdu.routingToken[len - 1] == 0x0A,
                    @"[In Client X.224 Connection Request PDU]routingToken (variable): An optional and variable-length "
                        + @"routing token (used for load balancing) terminated by a 0x0D0A two-byte sequence.");
            }

            if (clientPdu.rdpNegData != null)
            {
                site.Assert.AreEqual<type_Values>(type_Values.V1, clientPdu.rdpNegData.type,
                    @"[In RDP Negotiation Request] type ( 1 byte): it MUST be set to 0x01 (TYPE_RDP_NEG_REQ) to indicate "
                    + @"that the packet is a Negotiation Request.");

                bool isRDPNegDataSatisfied = clientPdu.rdpNegData.flags == RDP_NEG_REQ_flags_Values.V1 || clientPdu.rdpNegData.flags == RDP_NEG_REQ_flags_Values.CORRELATION_INFO_PRESENT;
                site.Assert.IsTrue(isRDPNegDataSatisfied,
                    @"[In RDP Negotiation Request] flags (1 byte): An 8-bit, unsigned integer that contains protocol flags. "
                    + @"this flags field  MUST be set to 0x00 or CORRELATION_INFO_PRESENT(0x08).");

                site.Assert.AreEqual<length_Values>(length_Values.V1, clientPdu.rdpNegData.length,
                    @"[In RDP Negotiation Request] length (2 bytes): length MUST be set to 0x0008 (8 bytes).");



                uint bitmask = ~(uint)(requestedProtocols_Values.PROTOCOL_RDP_FLAG | requestedProtocols_Values.PROTOCOL_HYBRID_FLAG | requestedProtocols_Values.PROTOCOL_SSL_FLAG | requestedProtocols_Values.PROTOCOL_HYBRID_EX | requestedProtocols_Values.PROTOCOL_RDSTLS);
                site.Assert.IsTrue(
                    (((uint)clientPdu.rdpNegData.requestedProtocols & bitmask) == 0),
                    @"In RDP Negotiation Request the requestedProtocols can take combining of the following flags. 1. PROTOCOL_RDP_FLAG"
                    + @" 0x00000000 Legacy RDP encryption 2. PROTOCOL_SSL_FLAG 0x00000001  TLS 1.0. 3. PROTOCOL_HYBRID_FLAG 0x00000002  CredSSP. "
                    + @" 4. PROTOCOL_RDSTLS 0x00000004  RDSTLS.");


                if ((clientPdu.rdpNegData.requestedProtocols & requestedProtocols_Values.PROTOCOL_HYBRID_FLAG) != 0)
                {
                    site.Assert.AreNotEqual<requestedProtocols_Values>(0, clientPdu.rdpNegData.requestedProtocols & requestedProtocols_Values.PROTOCOL_SSL_FLAG,
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
            site.Log.Add(TestTools.LogEntryKind.Debug, "CheckMCSConnectInitialPdu");

            CaptureRequirement(connectInitialPdu.mcsCi.gccPdu != null,
                @"[In RDP Negotiation Request] The RDP Negotiation Request structure is used by a client to advertise the security protocols which it supports.");

            if (!serverConfig.isExtendedClientDataSupported)
            {
                site.Assert.IsNull(connectInitialPdu.mcsCi.gccPdu.clientMonitorData,
                    @"In  Client MCS Connect Initial PDU with GCC Conference Create Request, clientMonitorData (variable): This field MUST "
                    + @"NOT be included if the server did not advertise support for Extended Client Data Blocks by using the EXTENDED_CLIENT_DATA_SUPPORTED"
                    + @" flag (0x00000001) as described in section 2.2.1.2.1.");
            }
            bool isValidValue = connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_CORE || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_SECURITY
                || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_NET || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_CLUSTER
                || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.CS_MONITOR || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.SC_CORE
                || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.SC_SECURITY || connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type == TS_UD_HEADER_type_Values.SC_NET;
            site.Assert.IsTrue(isValidValue,
                @"In TS_UD_HEADER, the type field can take one of the following values: CS_CORE 0xC001,CS_SECURITY 0xC002,CS_NET 0xC003,CS_CLUSTER 0xC004,"
                + @"SC_CORE 0x0C01,SC_SECURITY 0x0C02,SC_NET 0xOC03");

            site.Assert.AreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_CORE, connectInitialPdu.mcsCi.gccPdu.clientCoreData.header.type,
                @"In TS_UD_HEADER, if the type is CS_CORE0xC001 then the data block which follows User Data Header (TS_UD_HEADER) contains Client Core Data");
            site.Assert.AreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_SECURITY, connectInitialPdu.mcsCi.gccPdu.clientSecurityData.header.type,
                @"In TS_UD_HEADER, if the type is CS_SECURITY 0xC002 then the data block which follows User Data Header (TS_UD_HEADER) contains Client Security Data");
            if (connectInitialPdu.mcsCi.gccPdu.clientNetworkData != null)
            {
                site.Assert.AreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_NET, connectInitialPdu.mcsCi.gccPdu.clientNetworkData.header.type,
                    @"In TS_UD_HEADER, if the type is CS_NET 0xC003 then the data block which follows contains User Data Header (TS_UD_HEADER) Client Network Data.");
            }

            if (connectInitialPdu.mcsCi.gccPdu.clientClusterData != null)
            {
                site.Assert.AreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_CLUSTER, connectInitialPdu.mcsCi.gccPdu.clientClusterData.header.type,
                    @"In TS_UD_HEADER, if the type is CS_CLUSTER 0xC004 then the data block which follows User Data Header (TS_UD_HEADER) contains Client Cluster Data.");
            }

            if (connectInitialPdu.mcsCi.gccPdu.clientMonitorData != null)
            {
                site.Assert.AreEqual<TS_UD_HEADER_type_Values>(TS_UD_HEADER_type_Values.CS_MONITOR, connectInitialPdu.mcsCi.gccPdu.clientMonitorData.header.type,
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
            CaptureRequirement(securityPdu.securityExchangePduData.length != 0,
                @"In Security Exchange PDU Data (TS_SECURITY_PACKET) the length field is a 32-bit, unsigned integer. The size in bytes of the buffer containing the encrypted client random value, not including the header length.");
            //Refer to test suite bug #8341151
            site.Assert.IsTrue((securityPdu.commonHeader.securityHeader.flags & TS_SECURITY_HEADER_flags_Values.SEC_EXCHANGE_PKT) == TS_SECURITY_HEADER_flags_Values.SEC_EXCHANGE_PKT,
              "In Security Exchange PDU Data (TS_SECURITY_PACKET) the flags field of the security header MUST contain the SEC_EXCHANGE_PKT flag (0x0001).");

        }

        /// <summary>
        /// 2.2.1.11
        /// </summary>
        /// <param name="clientInfo"></param>
        public void VerifyPdu(Client_Info_Pdu clientInfo)
        {
            site.Assert.IsTrue(clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER ||
                clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER1 ||
                clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER2,
                @"[In Client Info PDU Data (CLIENT_INFO_PDU)]securityHeader (variable):   This field MUST contain one"
                + @" of the following headers:[Basic Security Header,Non-FIPS Security Header, FIPS Security Header ].");

            // The security header of Client Info PDU is different from other PDU.
            if (serverConfig.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                bool isValidEncryptionLevel = (clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER) &&
                    (!(clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER1)) &&
                    (!(clientInfo.commonHeader.securityHeader is TS_SECURITY_HEADER2));
                site.Assert.IsTrue(isValidEncryptionLevel,
                    @"The securityHeader in "
                    + @"CLIENT_INFO_PDU structure is a Basic Security Header if the Encryption Level  selected by"
                    + @" the server is ENCRYPTION_LEVEL_NONE (0).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.Assert.IsInstanceOfType(clientInfo.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER1),
                    @"The securityHeader in CLIENT_INFO_PDU structure is a Non-FIPS Security Header (section 2.2.8.1.1.2.2) if the "
                    + @"Encryption Method selected by the server is "
                    + @"ENCRYPTION_METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT (0x00000008), "
                    + @"ENCRYPTION_METHOD_128BIT (0x00000002).");
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.Assert.IsInstanceOfType(clientInfo.commonHeader.securityHeader, typeof(TS_SECURITY_HEADER2),
                    @"The securityHeader in "
                    + @"CLIENT_INFO_PDU structure is a FIPS Security Header (section 2.2.8.1.1.2.3) if the Encryption"
                    + @" Method selected by the server is ENCRYPTION_METHOD_FIPS (0x00000010).");
            }
            else
            {
                site.Assert.Fail("Check the code, something is wrong with the test case. encryptionMethod: {0}, encryptionLevel: {1}",
                    serverConfig.encryptionMethod, serverConfig.encryptionLevel);
            }

            if (serverConfig.encryptedProtocol == EncryptedProtocol.Rdp)
            {
                //SDK Bug, SecurityHeader.Flag is parsed incorrectly for Client Info PDU
                site.Assert.AreEqual<TS_SECURITY_HEADER_flags_Values>(TS_SECURITY_HEADER_flags_Values.SEC_INFO_PKT, (clientInfo.commonHeader.securityHeader.flags & TS_SECURITY_HEADER_flags_Values.SEC_INFO_PKT),
                    @"[In Client Info PDU Data (CLIENT_INFO_PDU)]In CLIENT_INFO_PDU structure, the flags field of "
                    + @"the security header MUST contain the SEC_INFO_PKT flag (section 2.2.8.1.1.2.1).");

                site.Assert.IsNotNull(clientInfo.infoPacket,
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
            VerifySecurityHeader(confirmActivePdu.commonHeader.securityHeader, "Client Confirm Active");

            VerifyStructure(confirmActivePdu.confirmActivePduData);

            //verify all the capability set:
            foreach (ITsCapsSet cap in confirmActivePdu.confirmActivePduData.capabilitySets)
            {
                switch (cap.GetCapabilityType())
                {
                    case capabilitySetType_Values.CAPSTYPE_GENERAL:
                        VerifyStructure((TS_GENERAL_CAPABILITYSET)cap);
                        break;
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
            VerifySecurityHeader(clientSyncPdu.commonHeader.securityHeader, "Client Synchronize");

            site.Assert.AreEqual<int>(7, (clientSyncPdu.synchronizePduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xf),
                @"In TS_SYNCHONIZE_PDU the type subfield of the pduType field of the Share Control Header "
                + @"MUST be set to PDUTYPE_DATAPDU (7).");
            site.Assert.AreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_SYNCHRONIZE, clientSyncPdu.synchronizePduData.shareDataHeader.pduType2,
                @"In TS_SYNCHONIZE_PDU the type subfield of the pduType2 field of the Share Data Header MUST"
                + @" be set to PDUTYPE2_SYNCHRONIZE (31).");
            site.Assert.AreEqual<messageType_Values>(messageType_Values.V1, clientSyncPdu.synchronizePduData.messageType,
                @"In TS_SYNCHONIZE_PDU, the messageType field MUST be set to SYNCMSGTYPE_SYNC (1).");
        }

        /// <summary>
        /// 2.2.1.15
        /// </summary>
        /// <param name="clientCoopPdu"></param>
        public void VerifyPdu(Client_Control_Pdu_Cooperate clientCoopPdu)
        {
            VerifySecurityHeader(clientCoopPdu.commonHeader.securityHeader, "Client Control (Cooperate)");

            site.Assert.IsTrue(clientCoopPdu.controlPduData.controlId == 0 && clientCoopPdu.controlPduData.grantId == 0,
                @"In controlPduData of Client Control PDU - Cooperate, The grantId and controlId fields of the "
                + @"Control PDU Data MUST both be set to zero.");

            site.Assert.AreEqual<action_Values>(action_Values.CTRLACTION_COOPERATE, clientCoopPdu.controlPduData.action,
                @"In controlPduData of Client Control PDU - Cooperate, the action field MUST be set to "
                + @"CTRLACTION_COOPERATE (0x0004).");

            site.Assert.AreEqual<int>(7, (clientCoopPdu.controlPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xf),
                @"In TS_CONTROL_PDU, the type subfield of the pduType field of the Share Control Header of TS_CONTROL_PDU"
                + @" structure MUST be set to PDUTYPE_DATAPDU (7). ");

            site.Assert.AreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_CONTROL, clientCoopPdu.controlPduData.shareDataHeader.pduType2,
                @" In TS_CONTROL_PDU, the pduType2 field of the Share Data Header  of TS_CONTROL_PDU structure MUST be set"
                + @" to PDUTYPE2_CONTROL (20).");
            bool isValidValue = clientCoopPdu.controlPduData.action == action_Values.CTRLACTION_REQUEST_CONTROL ||
                clientCoopPdu.controlPduData.action == action_Values.CTRLACTION_GRANTED_CONTROL ||
                clientCoopPdu.controlPduData.action == action_Values.CTRLACTION_DETACH ||
                clientCoopPdu.controlPduData.action == action_Values.CTRLACTION_COOPERATE;
            site.Assert.IsTrue(isValidValue,
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
            VerifySecurityHeader(clientCtlRequestPdu.commonHeader.securityHeader, "Client Control (Request Control)");

            site.Assert.IsTrue(clientCtlRequestPdu.controlPduData.controlId == 0 && clientCtlRequestPdu.controlPduData.grantId == 0,
                @"In Client Control PDU - Request Control, the grantId and controlId fields of the Control PDU Data MUST both be set to zero.");

            site.Assert.AreEqual<action_Values>(action_Values.CTRLACTION_REQUEST_CONTROL, clientCtlRequestPdu.controlPduData.action,
                @"In Client Control PDU - Request Control,the action field of the Control PDU Data MUST be set to CTRLACTION_REQUEST_CONTROL (0x0001).");
        }

        /// <summary>
        /// 2.2.1.17
        /// </summary>
        /// <param name="keyListPdu"></param>
        public void VerifyPdu(Client_Persistent_Key_List_Pdu keyListPdu)
        {
            VerifySecurityHeader(keyListPdu.commonHeader.securityHeader, "Client Persistent Key List");

            site.Assert.AreEqual<int>(7, (keyListPdu.persistentKeyListPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xf),
                @"In Client Persistent Key List PDU, the type subfield of the pduType field of the Share Control Header of Client "
                + @"Persistent Key List PDU MUST be set to PDUTYPE_DATAPDU (7). ");

            site.Assert.AreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_BITMAPCACHE_PERSISTENT_LIST, keyListPdu.persistentKeyListPduData.shareDataHeader.pduType2,
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
            VerifySecurityHeader(fontListPdu.commonHeader.securityHeader, "Client Font List");

            site.Assert.AreEqual<int>(7, (fontListPdu.fontListPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xf),
                @"In TS_FONT_LIST_PDU, the type subfield of the pduType field of the Share Control Header MUST be set to PDUTYPE_DATAPDU (7).");

            site.Assert.AreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_FONTLIST, fontListPdu.fontListPduData.shareDataHeader.pduType2,
                @"In TS_FONT_LIST_PDU, the pduType2 field of the Share Data Header MUST be set to PDUTYPE2_FONTLIST (39).");

            //2.2.1.18.1
            site.Assert.AreEqual<ushort>(0, fontListPdu.fontListPduData.numberFonts,
                @"In TS_FONT_LIST_PDU, numberFonts field SHOULD be set to 0.");
            site.Assert.AreEqual<ushort>(0, fontListPdu.fontListPduData.totalNumFonts,
                @"In TS_FONT_LIST_PDU, totalNumFonts field SHOULD be set to 0.");
            site.Assert.AreEqual<ushort>(0x0003, fontListPdu.fontListPduData.listFlags,
                @"In TS_FONT_LIST_PDU, listFlags field SHOULD be set to 0x0003 which is the logical OR'ed value of FONTLIST_FIRST (0x0001)"
                + @" and FONTLIST_LAST (0x0002).");
            site.Assert.AreEqual<ushort>(0x0032, fontListPdu.fontListPduData.entrySize,
                @"In TS_FONT_LIST_PDU, entrySize field SHOULD be set to 0x0032 (50 bytes).");
        }

        /// <summary>
        /// 2.2.2.1
        /// </summary>
        /// <param name="shutdownPdu"></param>
        public void VerifyPdu(Client_Shutdown_Request_Pdu shutdownPdu)
        {
            VerifySecurityHeader(shutdownPdu.commonHeader.securityHeader, "Client Shutdown Request");
            site.Assert.AreEqual<byte>(7, (byte)(shutdownPdu.shutdownRequestPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xF),
                @"In Shutdown Request PDU Data (TS_SHUTDOWN_REQ_PDU), shareDataHeader (18 bytes): The type subfield of the pduType field"
                + @" of the Share Control Header (section 2.2.8.1.1.1.1) MUST be set to PDUTYPE_DATAPDU (7).");
            site.Assert.AreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_SHUTDOWN_REQUEST, shutdownPdu.shutdownRequestPduData.shareDataHeader.pduType2,
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
            site.Assert.AreEqual<int>(1, versionLow,
                @"In TS_SHARECONTROLHEADER structure, for PduType's subfield, versionLow field MUST be set to TS_PROTOCOL_VERSION (0x1).");

            site.Assert.AreEqual<versionHigh_Values>(versionHigh_Values.V1, inputPdu.shareDataHeader.shareControlHeader.pduType.versionHigh,
                @"In TS_SHARECONTROLHEADER, for PduType's subfield, the versionHigh field must be 1 byte which is the most "
                + @"significant byte of pduType field.");

            site.Assert.IsTrue(inputPdu.shareDataHeader.streamId == streamId_Values.STREAM_UNDEFINED ||
                inputPdu.shareDataHeader.streamId == streamId_Values.STREAM_LOW ||
                inputPdu.shareDataHeader.streamId == streamId_Values.STREAM_MED ||
                inputPdu.shareDataHeader.streamId == streamId_Values.STREAM_HI,
                @"In TS_SHAREDATAHEADER structure,  the streamId can be the following: STREAM_UNDEFINED 0x00 ,STREAM_LOW"
                + @" 0x01,STREAM_MED 0x02,STREAM_HI 0x04");

            if (inputPdu.commonHeader.securityHeader is TS_SECURITY_HEADER2)
            {
                TS_SECURITY_HEADER2 header2 = (TS_SECURITY_HEADER2)inputPdu.commonHeader.securityHeader;
                site.Assert.AreEqual<TS_SECURITY_HEADER2_length_Values>(TS_SECURITY_HEADER2_length_Values.V1, header2.length,
                    @"The TS_SECURITY_HEADER2 structure,the length field MUST be set to 0x0010 (16 bytes) for legacy reasons.");
                //have defined ConstValue.TSFIPS_VERSION1 in sdk
                site.Assert.AreEqual<byte>(1, header2.version,
                    @"In TS_SECURITY_HEADER2 structure,the version field SHOULD be set to TSFIPS_VERSION1 (0x01).");
            }

            VerifySecurityHeader(inputPdu.commonHeader.securityHeader, "Client Input Event");

            //2.2.8.1.1.3.1            
            site.Assert.AreEqual<ShareControlHeaderType>(ShareControlHeaderType.PDUTYPE_DATAPDU, (ShareControlHeaderType)(inputPdu.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0x0F),
                @"In Client Input Event PDU (TS_INPUT_PDU),the shareDataHeader field contains information about"
                + @" the packet. The type subfield of the pduType field of the Share Control Header MUST be set"
                + @" to PDUTYPE_DATAPDU (7). ");
            site.Assert.AreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_INPUT, inputPdu.shareDataHeader.pduType2,
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
            TS_FP_INPUT_EVENT e;

            var actionCode = fpInputPdu.fpInputHeader.action;
            int numberEvents = fpInputPdu.fpInputHeader.numEvents;
            var encryptionFlags = fpInputPdu.fpInputHeader.flags;

            CaptureRequirement(actionCode == actionCode_Values.FASTPATH_INPUT_ACTION_FASTPATH || actionCode == actionCode_Values.FASTPATH_INPUT_ACTION_X224,
                @"In TS_FP_INPUT_PDU structure, action is a 2-bit, unsigned integer that indicates whether the PDU is in fast-path (0) or slow-path format (3)."
                );

            if (numberEvents != 0)
            {
                CaptureRequirement(numberEvents >= 1 && numberEvents <= 15,
                    @"In fpInputHeader, numEvents is a 4-bit, unsigned integer that collapses the number of fast-path input events packed together in the fpInputEvents field into 4 bits if the number of events is in the range 1 to 15."
                    );
            }
            else
            {
                CaptureRequirement(fpInputPdu.numberEvents != 0,
                 @"In TS_FP_INPUT_PDU structure, if the number of input events in fpInputHeader is greater than 15, then the numEvents bit field in the fast-path header byte MUST be set to zero, and the numEvents optional field inserted after the dataSignature field. This allows up to 255 input events in one PDU."
                 );
            }

            int pduSize = RdpbcgrUtility.GetPduSize(fpInputPdu);
            int length = RdpbcgrUtility.CalculateFpUpdatePduLength(fpInputPdu.length1, fpInputPdu.length2);
            site.Assert.AreEqual(pduSize, length, "The length ({0}) of TS_FP_INPUT_PDU calculated by length1 & length2 must be equal to the real size ({1}) of the pdu.", length, pduSize);

            if ((0x7f & fpInputPdu.length1) == fpInputPdu.length1)
            {
                // length1's most significant bit is not set
                site.Assert.AreEqual(true, fpInputPdu.length1 >= 1 && fpInputPdu.length1 <= 127, "If the most significant bit of the length1 field is not set, then the size of the PDU is in the range 1 to 127 bytes ");
            }
            else
            {
                site.Assert.AreEqual(true, length <= 16383, "If the most significant bit of the length1 field is set, the overall PDU length SHOULD be less than or equal to 16,383 bytes.");
            }

            Console.WriteLine("encryptionFlags= " + encryptionFlags);
            if (fpInputPdu.dataSignature == null)
                Console.WriteLine("fpInputPdu.dataSignature is null");

            if (encryptionFlags == encryptionFlags_Values.FASTPATH_INPUT_ENCRYPTED)
            {
                site.Assert.IsNotNull(fpInputPdu.dataSignature,
                    @"In TS_FP_INPUT_PDU structure, the dataSignature MUST be present if the FASTPATH_INPUT_ENCRYPTED"
                    + @" flag is set in the fpInputHeader field.");
            }

            if (numberEvents == 0)
            {
                //or fpInputPdu.numberEvents!=0
                site.Assert.IsNotNull(fpInputPdu.numberEvents,
                    @"In TS_FP_INPUT_PDU structure, the numberEvents field is present if the numberEvents bit field in"
                    + @" the fast-path header byte is zero.");
            }

            if (fpInputPdu.numberEvents == 0)
            {
                site.Assert.AreEqual<int>(numberEvents, fpInputPdu.fpInputEvents.Count,
                      @"In TS_FP_INPUT_PDU structure, the number of events present in fpInputEvents array is "
                      + @"given by the numberEvents bit field in the fast-path header byte, or by the numberEvents"
                      + @" field in the Fast-Path Input Event PDU (if it is present).");
            }
            else
            {
                site.Assert.AreEqual<int>(fpInputPdu.numberEvents, fpInputPdu.fpInputEvents.Count,
                    @"In TS_FP_INPUT_PDU structure, the number of events present in fpInputEvents array is given"
                    + @" by the numberEvents bit field in the fast-path header byte, or by the numberEvents field "
                    + @"in the Fast-Path Input Event PDU (if it is present).");
            }

            if (this.serverConfig.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                site.Assert.AreEqual<TS_FP_FIPS_INFO_length_Values>(TS_FP_FIPS_INFO_length_Values.V1, fpInputPdu.fipsInformation.length,
                    @"In TS_FP_FIPS_INFO structure, the length field MUST be set to 0x0010 (16 bytes).");
                //ConstValue.TSFIPS_VERSION1
                site.Assert.AreEqual<byte>(1, fpInputPdu.fipsInformation.version,
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
            VerifySecurityHeader(refreshPdu.commonHeader.securityHeader, "Client Refresh Rect");
            site.Assert.AreEqual<ShareControlHeaderType>((ShareControlHeaderType)(refreshPdu.refreshRectPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xF), ShareControlHeaderType.PDUTYPE_DATAPDU,
                @"In Refresh Rect PDU Data,the type subfield of the pduType field of the Share Control Header MUST"
                + @" be set to PDUTYPE_DATAPDU (7).");
            site.Assert.AreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_REFRESH_RECT, refreshPdu.refreshRectPduData.shareDataHeader.pduType2,
                @"In Refresh Rect PDU Data,the pduType2 field of the Share Data Header MUST be set to PDUTYPE2_REFRESH_RECT (33).");
            site.Assert.AreEqual<int>(refreshPdu.refreshRectPduData.numberOfAreas, refreshPdu.refreshRectPduData.areasToRefresh.Count,
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
            VerifySecurityHeader(suppressPdu.commonHeader.securityHeader, "Client Suppress Output");
            site.Assert.AreEqual<ShareControlHeaderType>(ShareControlHeaderType.PDUTYPE_DATAPDU, (ShareControlHeaderType)(suppressPdu.suppressOutputPduData.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow & 0xF),
                @"In Suppress Output PDU Data, the type subfield of the pduType field of the Share Control Header (section 2.2.8.1.1.1.1)"
                + @" MUST be set to PDUTYPE_DATAPDU (7).");
            site.Assert.AreEqual<pduType2_Values>(pduType2_Values.PDUTYPE2_SUPPRESS_OUTPUT, suppressPdu.suppressOutputPduData.shareDataHeader.pduType2,
                @"In Suppress Output PDU Data, the type subfield of the pduType2 field of the Share Data Header MUST be set to "
                + @"PDUTYPE2_SUPPRESS_OUTPUT (35).");
            bool isValidValue = suppressPdu.suppressOutputPduData.allowDisplayUpdates == AllowDisplayUpdates_SUPPRESS_OUTPUT.ALLOW_DISPLAY_UPDATES || suppressPdu.suppressOutputPduData.allowDisplayUpdates == AllowDisplayUpdates_SUPPRESS_OUTPUT.SUPPRESS_DISPLAY_UPDATES;
            site.Assert.IsTrue(isValidValue,
                @"In Suppress Output PDU Data, allowDisplayUpdates can be of two flags: SUPPRESS_DISPLAY_UPDATES 0x00, ALLOW_DISPLAY_UPDATES 0x01");
            if (suppressPdu.suppressOutputPduData.allowDisplayUpdates == AllowDisplayUpdates_SUPPRESS_OUTPUT.ALLOW_DISPLAY_UPDATES)
            {
                bool isContained = suppressPdu.suppressOutputPduData.desktopRect.right - suppressPdu.suppressOutputPduData.desktopRect.left != 0 &&
                    suppressPdu.suppressOutputPduData.desktopRect.bottom - suppressPdu.suppressOutputPduData.desktopRect.top != 0;
                site.Assert.IsTrue(isContained,
                    @"In Suppress Output PDU Data, the desktopRect field contains the coordinates of the desktop rectangle if"
                    + @" the allowDisplayUpdates field is set to ALLOW_DISPLAY_UPDATES (1).");
            }
            else
            {
                bool isNotContained = suppressPdu.suppressOutputPduData.desktopRect.right - suppressPdu.suppressOutputPduData.desktopRect.left == 0 &&
                    suppressPdu.suppressOutputPduData.desktopRect.bottom - suppressPdu.suppressOutputPduData.desktopRect.top == 0;
                site.Assert.IsTrue(isNotContained,
                    @"In Suppress Output PDU Data, the desktopRect field MUST NOT be included in the PDU, if the "
                    + @"allowDisplayUpdates field is set to SUPPRESS_DISPLAY_UPDATES (0).");
            }
        }

        #endregion "Check PDUs"

        #region "Check structure"

        public void VerifyStructure(TS_UD_CS_CORE core)
        {
            Site.Assert.IsTrue(core.header.type == TS_UD_HEADER_type_Values.CS_CORE,
                    "In Client Core Data, The User Data Header type field MUST be set to CS_CORE (0xC001).");

            var clientVersions = Enum.GetValues(typeof(TS_UD_CS_CORE_version_Values)).Cast<uint>();
            Site.Assert.IsTrue(
                clientVersions.Any(version => version == (uint)core.version),
                VersionDescribeFormat());
            Site.Assert.IsTrue(core.desktopWidth >= 0,
                "In Client Core Data, desktopWidth must be positive.");
            Site.Assert.IsTrue(core.desktopHeight >= 0,
                "In Client Core Data, desktopHeight must be positive.");
            Site.Assert.IsTrue((int)core.colorDepth == 51712 || core.colorDepth == colorDepth_Values.RNS_UD_COLOR_8BPP,
                "In Client Core Data (TS_UD_CS_CORE), colorDepth (2 bytes): colorDepth field can have the following "
                + @"values :\r\n1.RNS_UD_COLOR_4BPP 0xCA00.\r\n2.RNS_UD_COLOR_8BPP 0xCA01.");
            Site.Assert.IsTrue(core.SASSequence == 0xAA03,
                "In Client Core Data, SASSequence SHOULD be set to RNS_UD_SAS_DEL (0xAA03).");
            Site.Assert.IsTrue(core.clientName.Length <= 15,
                "In Client Core Data,  clientName field contains up to 15 Unicode characters plus a null terminator.");
            Site.Assert.IsTrue(core.keyboardType >= keyboardType_Values.None && core.keyboardType <= keyboardType_Values.V7,
                "In Client Core Data,  keyboardType can take one of the following values:");
            Site.Assert.IsTrue(core.imeFileName.Length <= 31,
                "In Client Core Data, the imeFileName field contains up to 31 Unicode characters plus a null terminator.");
            bool isValidValue = (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_4BPP || (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_8BPP
                || (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_16BPP_555 || (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_16BPP_565
                || (postBeta2ColorDepth_Values)core.postBeta2ColorDepth.actualData == postBeta2ColorDepth_Values.RNS_UD_COLOR_24BPP;
            site.Assert.IsTrue(isValidValue,
                @"In Client Core Data, postBeta2ColorDepth can take one of the following values: RNS_UD_COLOR_4BPP 0xCA00,"
                + @" RNS_UD_COLOR_8BPP 0xCA01, RNS_UD_COLOR_16BPP_555 0xCA02, RNS_UD_COLOR_16BPP_565 0xCA03, RNS_UD_COLOR_24BPP 0xCA04");
            Site.Assert.IsTrue((int)core.clientProductId.actualData == 1,
                "In Client Core Data,  clientProductId SHOULD be initialized to 1.");
            Site.Assert.AreEqual<int>(0, (int)core.serialNumber.actualData,
                "In Client Core Data, serialNumber SHOULD be initialized to 0. ");
            isValidValue = (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V1 || (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V2
                || (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V3 || (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V4
                || (highColorDepth_Values)core.highColorDepth.actualData == highColorDepth_Values.V5;
            site.Assert.IsTrue(isValidValue,
                @"In Client Core Data, highColorDepth can have one of the following values 4,8,15,16,24");

            site.Assert.IsTrue(core.supportedColorDepths.actualData <= 15,
                @"In Client Core Data, supportedColorDepths can have one of the following values RNS_UD_24BPP_SUPPORT 0x0001, "
                + @"RNS_UD_16BPP_SUPPORT 0x0002, RNS_UD_15BPP_SUPPORT 0x0004, RNS_UD_32BPP_SUPPORT 0x0008");

            ushort earlyCapabilityFlags = (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_ERRINFO_PDU | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_WANT_32BPP_SESSION
                | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_STATUSINFO_PDU | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_STRONG_ASYMMETRIC_KEYS | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_UNUSED | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_VALID_CONNECTION_TYPE | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_MONITOR_LAYOUT_PDU
                | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_NETWORK_AUTODETECT | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_DYNVC_GFX_PROTOCOL | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_DYNAMIC_TIME_ZONE | (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_HEARTBEAT_PDU;
            ushort negEarlyCapabilityFlags = (ushort)~earlyCapabilityFlags;
            isValidValue = ((ushort)core.earlyCapabilityFlags.actualData & negEarlyCapabilityFlags) == 0;

            site.Assert.IsTrue(isValidValue,
                    @"[In Client Core Data (TS_UD_CS_CORE)] earlyCapabilityFlags (2 bytes):In Client Core Data, the earlyCapabilityFlags"
                    + @" can be any of the following values: RNS_UD_CS_SUPPORT_ERRINFO_PDU 0x0001, RNS_UD_CS_WANT_32BPP_SESSION 0x0002, "
                    + @"RNS_UD_CS_SUPPORT_STATUSINFO_PDU 0x0004, RNS_UD_CS_STRONG_ASYMMETRIC_KEYS 0x0008, RNS_UD_CS_VALID_CONNECTION_TYPE"
                    + @" 0x0020, RNS_UD_CS_SUPPORT_MONITOR_LAYOUT_PDU 0x0040");

        }


        public String VersionDescribeFormat()
        {
            StringBuilder versionDescribe = new StringBuilder("In Client Core Data, RDP client version number should be one of: ");
            foreach (uint versionValue in Enum.GetValues(typeof(TS_UD_CS_CORE_version_Values)))
            {
                versionDescribe.Append($"{((uint)versionValue).ToString("X08")}, ");
            }
            versionDescribe.Remove(versionDescribe.Length - 2, 2);
            versionDescribe.Append(".");
            return versionDescribe.ToString();
        }


        public void VerifyStructure(TS_UD_CS_SEC sec)
        {
            Site.Assert.AreEqual<TS_UD_HEADER_type_Values>(sec.header.type, TS_UD_HEADER_type_Values.CS_SECURITY,
                "In Client Security Data (TS_UD_CS_SEC), User Data Header type field MUST be set to CS_SECURITY (0xC002).");
            uint encryptionMethods = (uint)encryptionMethod_Values._40BIT_ENCRYPTION_FLAG | (uint)encryptionMethod_Values._56BIT_ENCRYPTION_FLAG
                | (uint)encryptionMethod_Values._128BIT_ENCRYPTION_FLAG | (uint)encryptionMethod_Values.FIPS_ENCRYPTION_FLAG;
            uint negEncryptionMethods = ~encryptionMethods;
            if (sec.encryptionMethods != 0)
            {
                // non-French client
                bool isValidValue = ((uint)sec.encryptionMethods & negEncryptionMethods) == 0;
                Site.Assert.IsTrue(isValidValue,
                    @"In Client Security Data, encryptionMethods MUST be specified at least one of the following values:  \r\n40BIT_ENCRYPTION_FLAG "
                    + @"0x00000001,\r\n128BIT_ENCRYPTION_FLAG 0x0000000,\r\n256BIT_ENCRYPTION_FLAG 0x00000008,\r\nFIPS_ENCRYPTION_FLAG 0x00000010");

                Site.Assert.AreEqual<uint>((uint)sec.extEncryptionMethods, 0,
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
            Site.Assert.AreEqual<TS_UD_HEADER_type_Values>(net.header.type, TS_UD_HEADER_type_Values.CS_NET,
                "In Client Network Data (TS_UD_CS_NET), the User Data Header type field MUST be set to CS_NET (0xC003).");
            Site.Assert.IsTrue(net.channelCount <= 31,
                "[In Client Network Data (TS_UD_CS_NET)]channelCount (4 bytes): In Client Network Data, the maximum allowed value for channelCount is 31.");
            Site.Assert.AreEqual<int>(net.channelDefArray.Count, (int)net.channelCount,
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
                site.Assert.IsTrue(((uint)net.channelDefArray[i].options & negOptions) == 0,
                    @"In Channel Definition Structure (CHANNEL_DEF), the options field can take one of the following values"
                    + @" CHANNEL_OPTION_INITIALIZED 0x80000000, CHANNEL_OPTION_ENCRYPT_RDP 0x40000000, CHANNEL_OPTION_ENCRYPT_SC"
                    + @" 0x20000000, CHANNEL_OPTION_ENCRYPT_CS 0x10000000, CHANNEL_OPTION_PRI_HIGH 0x08000000, CHANNEL_OPTION_PRI_MED"
                    + @" 0x04000000,CHANNEL_OPTION_PRI_LOW 0x02000000, CHANNEL_OPTION_COMPRESS_RDP 0x00800000, CHANNEL_OPTION_COMPRESS"
                    + @" 0x00400000, CHANNEL_OPTION_SHOW_PROTOCOL 0x00200000, REMOTE_CONTROL_PERSISTENT 0x00100000");
            }
        }

        public void VerifyStructure(TS_UD_CS_CLUSTER cluster)
        {
            Site.Assert.AreEqual(cluster.header.type, TS_UD_HEADER_type_Values.CS_CLUSTER,
                "In Client Cluster Data (TS_UD_CS_CLUSTER), the User Data Header type field MUST be set to CS_CLUSTER (0xC004).");
            const uint flags = (uint)Flags_Values.REDIRECTION_SUPPORTED | (uint)Flags_Values.ServerSessionRedirectionVersionMask
                | (uint)Flags_Values.REDIRECTED_SESSIONID_FIELD_VALID | (uint)Flags_Values.REDIRECTED_SMARTCARD;
            const uint negFlags = ~flags;
            site.Assert.IsTrue(((uint)cluster.Flags & negFlags) == 0,
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

                Site.Assert.IsTrue((monitor.monitorDefArray[i].right - monitor.monitorDefArray[i].left + 1) >= 200 &&
                (monitor.monitorDefArray[i].bottom - monitor.monitorDefArray[i].top + 1) >= 200,
                    "The minimum permitted size of the virtual desktop is 200 x 200 pixels.");
            }

            Site.Assert.IsTrue(nTotalWidth <= 32766,
                "The maximum width of the virtual desktop resulting from the union of the monitors contained in the monitorDefArray"
                + @" field MUST NOT exceed 32766 pixels.");
            Site.Assert.IsTrue(nTotalHeight <= 32766,
                "Similarly, the maximum height of the virtual desktop resulting from the union of the monitors contained in the "
                + @"monitorDefArray field MUST NOT exceed 32766 pixels.");
            Site.Assert.IsTrue(monitor.header.type == TS_UD_HEADER_type_Values.CS_MONITOR,
                "[In Client Monitor Data (TS_UD_CS_MONITOR)]header (4 bytes):The User Data Header type field MUST be set to "
                + @"CS_MONITOR (0xC005).");
            Site.Assert.IsTrue(monitor.monitorCount <= 15,
                "[In Client Monitor Data (TS_UD_CS_MONITOR)]monitorCount (4 bytes):  The number of display monitor definitions "
                + @"in the monitorDefArray field (the maximum allowed is 16).");
            Site.Assert.IsTrue(monitor.monitorCount == monitor.monitorDefArray.Count,
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
            site.Assert.AreEqual<int>((info.flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE ? info.Domain.Trim('\0').Length * 2 : info.Domain.Trim('\0').Length, (int)info.cbDomain,
                @"In the TS_INFO_PACKET structure, cbDomain represents the size in bytes of the character data in the Domain field."
                + @" This size excludes the length of the mandatory null terminator.");
            site.Assert.AreEqual<int>((info.flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE ? info.UserName.Trim('\0').Length * 2 : info.UserName.Trim('\0').Length, (int)info.cbUserName,
                @"In the TS_INFO_PACKET structure, cbUserName represents the size in bytes of the character data in the UserName "
                + @"field. This size excludes the length of the mandatory null terminator.");
            site.Assert.AreEqual<int>((info.flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE ? info.Password.Trim('\0').Length * 2 : info.Password.Trim('\0').Length, (int)info.cbPassword,
                @"In the TS_INFO_PACKET structure, cbPassword represents the size in bytes of the character data in the Password "
                + @"field. This size excludes the length of the mandatory null terminator.");
            site.Assert.AreEqual<int>((info.flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE ? info.AlternateShell.Trim('\0').Length * 2 : info.AlternateShell.Trim('\0').Length, (int)info.cbAlternateShell,
                @"In the TS_INFO_PACKET structure, cbAlternateShell represents the size in bytes of the character data in the "
                + @"AlternateShell field. This size excludes the length of the mandatory null terminator.");
            site.Assert.AreEqual<int>((info.flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE ? info.WorkingDir.Trim('\0').Length * 2 : info.WorkingDir.Trim('\0').Length, (int)info.cbWorkingDir,
                @"In the TS_INFO_PACKET structure, cbWorkingDir represents the size in bytes of the character data in the WorkingDir"
                + @" field. This size excludes the length of the mandatory null terminator.");

            site.Assert.IsTrue(info.Domain.EndsWith("\0"),
                @"In the TS_INFO_PACKET structure, Domain field MUST contain at least a null terminator character in ANSI or Unicode"
                + @" format (depending on the presence of the INFO_UNICODE flag)");
            site.Assert.IsTrue(info.UserName.EndsWith("\0"),
                @"In the TS_INFO_PACKET structure, UserName field MUST contain at least a null terminator character in ANSI or Unicode"
                + @" format (depending on the presence of the INFO_UNICODE flag)");
            site.Assert.IsTrue(info.Password.EndsWith("\0"),
                @"In the TS_INFO_PACKET structure, Password field MUST contain at least a null terminator character in ANSI or Unicode"
                + @" format (depending on the presence of the INFO_UNICODE flag)");
            site.Assert.IsTrue(info.AlternateShell.Length <= 512,
                @"In the TS_INFO_PACKET structure, for AlternateShell, the maximum allowed length is 512 bytes (including the mandatory"
                + @" null terminator).");
            site.Assert.IsTrue(info.AlternateShell.EndsWith("\0"),
                @"In the TS_INFO_PACKET structure, AlternateShell field MUST contain at least a null terminator character in ANSI or "
                + @"Unicode format (depending on the presence of the INFO_UNICODE flag)");
            site.Assert.IsTrue(info.WorkingDir.EndsWith("\0"),
                @"In the TS_INFO_PACKET structure, WorkingDir field MUST contain at least a null terminator character in ANSI or "
                + @"Unicode format (depending on the presence of the INFO_UNICODE flag)");
            if (info.extraInfo != null)
            {

                if (!this.Site.Properties["RDP.Version"].Equals("8.1"))
                {
                    site.Assert.IsTrue(info.extraInfo.clientAddressFamily == clientAddressFamily_Values.V1 || info.extraInfo.clientAddressFamily == clientAddressFamily_Values.V2,
                        @"[In Extended Info Packet (TS_EXTENDED_INFO_PACKET)]clientAddressFamily (2 bytes):clientAddressFamily can have the "
                        + @"following values: 1.AF_INET 0x00002 2.AF_INET6 0x0017");
                }

                Console.WriteLine("clientDir.Length = " + info.extraInfo.clientDir.Length);
                Console.WriteLine("cbClientDir=" + info.extraInfo.cbClientDir);

                bool isValidLen = info.extraInfo.cbClientDir <= 512;
                site.Assert.IsTrue(isValidLen,
                    @"[In Extended Info Packet (TS_EXTENDED_INFO_PACKET)] clientDir (variable):The maximum allowed length is 512 bytes "
                    + @"(including the mandatory null terminator).");

                isValidLen = info.extraInfo.cbAutoReconnectLen <= 128;
                site.Assert.IsTrue(isValidLen,
                    @"[Extended Info Packet (TS_EXTENDED_INFO_PACKET)]autoReconnectCookie (28 bytes):the maximum allowed length "
                    + @"is 128 bytes.");
            }
        }

        /// <summary>
        /// 2.2.1.13.2.1
        /// </summary>
        /// <param name="confirmActive"></param>
        public void VerifyStructure(TS_CONFIRM_ACTIVE_PDU confirmActive)
        {
            site.Assert.AreEqual<ShareControlHeaderType>(ShareControlHeaderType.PDUTYPE_CONFIRMACTIVEPDU, (ShareControlHeaderType)(confirmActive.shareControlHeader.pduType.typeAndVersionLow & 0x0F),
                @"In TS_CONFIRM_ACTIVE_PDU structure, the type subfield of the pduType field of the Share Control Header MUST be set to"
                + @" PDUTYPE_CONFIRMACTIVEPDU (3).");
            site.Assert.AreEqual(1, confirmActive.shareControlHeader.pduType.typeAndVersionLow >> 4 & 0x0F,
                @"The PDUVersion subfield MUST be set to TS_PROTOCOL_VERSION (0x1).");
            site.Assert.AreEqual<originatorId_Values>(originatorId_Values.V1, confirmActive.originatorId,
                @"In TS_CONFIRM_ACTIVE_PDU structure, the originatorId MUST be set to the server channel ID (in Microsoft RDP server "
                + @"implementations, this value is always 0x03EA)");
            site.Assert.AreEqual<ushort>((ushort)confirmActive.sourceDescriptor.Length, confirmActive.lengthSourceDescriptor,
                @"In TS_CONFIRM_ACTIVE_PDU structure, the lengthSourceDescriptor gives the size in bytes of the sourceDescriptor field.");
            site.Assert.AreEqual<ushort>(confirmActive.numberCapabilities, (ushort)confirmActive.capabilitySets.Count,
                @"In TS_CONFIRM_ACTIVE_PDU structure, numberCapabilities gives the number of capability sets included "
                + @"in the Confirm Active PDU.");
        }

        /// <summary>
        /// 2.2.7.1.1
        /// </summary>
        /// <param name="general"></param>
        public void VerifyStructure(TS_GENERAL_CAPABILITYSET general)
        {
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_GENERAL, general.capabilitySetType,
                @"In TS_GENERAL_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_GENERAL (1).");

            CaptureRequirement(general.osMajorType == osMajorType_Values.OSMAJORTYPE_MACINTOSH ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_OS2 ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_UNIX ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_WINDOWS ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_IOS ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_OSX ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_ANDROID ||
                general.osMajorType == osMajorType_Values.OSMAJORTYPE_CHROME_OS,
                @"In TS_GENERAL_CAPABILITYSET structure, the osMajorType field should take one of the following values"
                + @"1. OSMAJORTYPE_UNSPECIFIED 0x0000 - Unspecified platform"
                + @"2. OSMAJORTYPE_WINDOWS 0x0001 - Windows platform"
                + @"3. OSMAJORTYPE_OS2 0x0002 - OS,2 platform"
                + @"4. OSMAJORTYPE_MACINTOSH 0x0003 - Macintosh platform"
                + @"5. OSMAJORTYPE_UNIX 0x0004 - UNIX platform"
                + @"6. OSMAJORTYPE_IOS 0x0005 - iOS platform"
                + @"7. OSMAJORTYPE_OSX 0x0006 - OS X platform"
                + @"8. OSMAJORTYPE_ANDROID 0x0007 - Android platform"
                + @"9. OSMAJORTYPE_CHROME_OS 0x0008 - Chrome OS platform"
            );
            CaptureRequirement(general.osMinorType >= osMinorType_Values.OSMINORTYPE_WINDOWS_31X && general.osMinorType <= osMinorType_Values.OSMINORTYPE_WINDOWS_RT,
              @"In TS_GENERAL_CAPABILITYSET structure, the osMinorType field should take one of the following values:"
                 + @"1. OSMINORTYPE_UNSPECIFIED 0x0000 -Unspecified version"
                 + @"2. OSMINORTYPE_WINDOWS_31X 0x0001 - Windows 3.1x"
                 + @"3. TS_OSMINORTYPE_WINDOWS_95 0x0002 - Windows 95"
                 + @"4. TS_OSMINORTYPE_WINDOWS_NT 0x0003 - Windows NT"
                 + @"5. TS_OSMINORTYPE_OS2_V21 0x0004 - OS,2 2.1"
                 + @"6. TS_OSMINORTYPE_POWER_PC 0x0005 - PowerPC"
                 + @"7. TS_OSMINORTYPE_MACINTOSH 0x0006 - Macintosh"
                 + @"8. TS_OSMINORTYPE_NATIVE_XSERVER 0x0007 - Native X Server"
                 + @"9. TS_OSMINORTYPE_PSEUDO_XSERVER 0x0008 - Pseudo X Server"
                 + @"10. OSMINORTYPE_WINDOWS RT 0x0009 - Windows RT"
                );
            CaptureRequirement(general.protocolVersion == protocolVersion_Values.V1,
              @"In TS_GENERAL_CAPABILITYSET structure, protocolVersion field MUST be set to TS_CAPS_PROTOCOLVERSION (0x0200).");

            CaptureRequirement(general.generalCompressionTypes == generalCompressionTypes_Values.V1,
              @"In TS_GENERAL_CAPABILITYSET structure, generalCompressionTypes field MUST be set to zero.");

            CaptureRequirement(general.updateCapabilityFlag == updateCapabilityFlag_Values.V1,
              @"In TS_GENERAL_CAPABILITYSET structure, updateCapabilityFlag field MUST be set to zero.");

            CaptureRequirement(general.remoteUnshareFlag == remoteUnshareFlag_Values.V1,
              @"In TS_GENERAL_CAPABILITYSET structure, remoteUnshareFlag field MUST be set to zero.");
        }

        /// <summary>
        /// 2.2.7.1.2
        /// </summary>
        /// <param name="bitmap"></param>
        public void VerifyStructure(TS_BITMAP_CAPABILITYSET bitmap)
        {
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_BITMAP, bitmap.capabilitySetType,
                @"In TS_BITMAP_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_BITMAP (2).");
            site.Assert.AreEqual<ushort>(1, bitmap.receive1BitPerPixel,
                @"In TS_BITMAP_CAPABILITYSET structure, the receive1BitPerPixel field indicates whether the client "
                + @"can receive 1 bit-per-pixel. This field is ignored and SHOULD be set to TRUE (0x0001).");
            site.Assert.AreEqual<ushort>(1, bitmap.receive4BitsPerPixel,
                @"In TS_BITMAP_CAPABILITYSET structure, the receive4BitsPerPixel field indicates whether the client"
                + @" can receive 4 bit-per-pixel. This field is ignored and SHOULD be set to TRUE (0x0001).");
            site.Assert.AreEqual<ushort>(1, bitmap.receive8BitsPerPixel,
                @"In TS_BITMAP_CAPABILITYSET structure, the receive8BitsPerPixel field indicates whether the client"
                + @" can receive 8 bit-per-pixel. This field is ignored during capability negotiation and MUST be set to "
                + @"TRUE (0x0001).");
            site.Assert.AreEqual<ushort>(1, bitmap.bitmapCompressionFlag,
                @"In TS_BITMAP_CAPABILITYSET structure, bitmapCompressionFlag Indicates whether the client supports "
                + @"bitmap compression.This field MUST be set to TRUE (0x0001) because support for compressed bitmaps"
                + @" is required for a connection to proceed.");
            site.Assert.AreEqual<byte>(0, bitmap.highColorFlags,
                @"In TS_BITMAP_CAPABILITYSET structure, highColorFlags field is ignored during capability negotiation "
                + @"and MUST be set to 0.");
            site.Assert.AreEqual<ushort>(1, bitmap.multipleRectangleSupport,
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
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_ORDER, order.capabilitySetType,
                @"In TS_ORDER_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_ORDER (3).");
            bool isZero = order.terminalDescriptor[0] == 0 &&
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
            site.Assert.IsTrue(isZero,
                @"In TS_ORDER_CAPABILITYSET structure, the terminalDescriptor field is ignored during capability "
                + @"negotiation and SHOULD be set to all zeroes.");
            site.Assert.AreEqual<ushort>(1, order.maximumOrderLevel,
                @"In TS_ORDER_CAPABILITYSET structure, the maximumOrderLevel field is ignored during capability"
                + @" negotiation and SHOULD be set to ORD_LEVEL_1_ORDERS (1).");
            site.Assert.AreEqual<ushort>(0, order.numberFonts,
                @"In TS_ORDER_CAPABILITYSET structure, the numberFonts field is ignored during capability "
                + @"negotiation and SHOULD be set to 0.");
            site.Assert.IsTrue((order.orderFlags & orderFlags_Values.NEGOTIATEORDERSUPPORT) == orderFlags_Values.NEGOTIATEORDERSUPPORT,
                @"In TS_ORDER_CAPABILITYSET structure, if the orderFlags is  NEGOTIATEORDERSUPPORT 0x0002 that "
                + @"indicates support for negotiating drawing orders in the orderSupport field. This flag MUST be set in the orderFlags field.");
            site.Assert.IsTrue((order.orderFlags & orderFlags_Values.ZEROBOUNDSDELTASSUPPORT) == orderFlags_Values.ZEROBOUNDSDELTASSUPPORT,
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
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_BITMAPCACHE, cache.capabilitySetType,
                @"In TS_BITMAPCACHE_CAPABILITYSET structure, the capabilitySetType field MUST be set to "
                + @"CAPSTYPE_BITMAPCACHE (4).");
            site.Assert.IsTrue(cache.Cache1Entries <= 200,
            @"In TS_BITMAPCACHE_CAPABILITYSET structure, the Cache0Entries gives the number of entries in"
            + @" Bitmap Cache 0 (maximum allowed value is 200 entries).");
            site.Assert.IsTrue(cache.Cache2Entries <= 600,
                @"In TS_BITMAPCACHE_CAPABILITYSET structure, Cache1Entries gives the number of entries in"
                + @" Bitmap Cache 1 (maximum allowed value is 600 entries).");
            site.Assert.IsTrue(cache.Cache3Entries <= 65535,
                @"In TS_BITMAPCACHE_CAPABILITYSET structure, Cache2Entries gives the number of entries in"
                + @" Bitmap Cache 2 (maximum allowed value is 65535 entries).");
        }

        /// <summary>
        /// 2.2.7.1.4.2
        /// </summary>
        /// <param name="cache2"></param>
        public void VerifyStructure(TS_BITMAPCACHE_CAPABILITYSET_REV2 cache2)
        {
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_REV2, cache2.capabilitySetType,
                @"In TS_BITMAPCACHE_CAPABILITYSET_REV2 structure capabilitySetType field MUST be set to "
                + @"CAPSTYPE_BITMAPCACHE_REV2 (19).");
            bool isValidValue = cache2.CacheFlags == CacheFlags_Values.ALLOW_CACHE_WAITING_LIST_FLAG ||
                cache2.CacheFlags == CacheFlags_Values.PERSISTENT_KEYS_EXPECTED_FLAG ||
                cache2.CacheFlags == (CacheFlags_Values.ALLOW_CACHE_WAITING_LIST_FLAG | CacheFlags_Values.PERSISTENT_KEYS_EXPECTED_FLAG) ||
                cache2.CacheFlags == CacheFlags_Values.None;
            site.Assert.IsTrue(isValidValue,
                @"In TS_BITMAPCACHE_CAPABILITYSET_REV2 structure,  the CacheFlags can be of the following"
                + @" PERSISTENT_KEYS_EXPECTED_FLAG 0x0001, ALLOW_CACHE_WAITING_LIST_FLAG 0x0002, None 0x0000");

            site.Assert.IsTrue(cache2.NumCellCaches <= 5,
                @"In TS_BITMAPCACHE_CAPABILITYSET_REV2 structure, the NumCellCaches field is the number of"
                + @" bitmap caches (with a maximum allowed value of 5). ");

            if (cache2.NumCellCaches >= 1)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache1CellInfo.NumEntriesAndK & 0x7fffffff) <= 600,
                   @"In Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2)]BitmapCache0CellInfo (4 bytes): The maximum number of entries allowed in this cache is 600. This field is only valid if NumCellCaches is greater than or equal to 1.");
            }

            if (cache2.NumCellCaches >= 2)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache2CellInfo.NumEntriesAndK & 0x7fffffff) <= 600,
                   @"In Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2)]BitmapCache1CellInfo (4 bytes): The maximum number of entries allowed in this cache is 600. This field is only valid if NumCellCaches is greater than or equal to 2.");
            }

            if (cache2.NumCellCaches >= 3)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache3CellInfo.NumEntriesAndK & 0x7fffffff) <= 65536,
                    @"In Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2)]BitmapCache2CellInfo (4 bytes): The maximum number of entries allowed in this cache is 65536. This field is only valid if NumCellCaches is greater than or equal to 3.");
            }

            if (cache2.NumCellCaches >= 4)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache4CellInfo.NumEntriesAndK & 0x7fffffff) <= 4096,
                   @"In Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2)]BitmapCache3CellInfo (4 bytes): The maximum number of entries allowed in this cache is 4096. This field is only valid if NumCellCaches is greater than or equal to 4.");
            }

            if (cache2.NumCellCaches >= 5)
            {
                //<?>
                CaptureRequirement((cache2.BitmapCache5CellInfo.NumEntriesAndK & 0x7fffffff) <= 2048,
                    @"In Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2)]BitmapCache4CellInfo (4 bytes): The maximum number of entries allowed in this cache is 2048. This field is only valid if NumCellCaches is equal to 5.");
            }
        }

        /// <summary>
        /// 2.2.7.1.5
        /// </summary>
        /// <param name="pointer"></param>
        public void VerifyStructure(TS_POINTER_CAPABILITYSET pointer)
        {
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_POINTER, pointer.capabilitySetType,
                @"In TS_POINTER_CAPABILITY_SET structure, capabilitySetType gives the type of the capability"
                + @" set. This field MUST be set to CAPSTYPE_POINTER (8).");
        }

        /// <summary>
        /// 2.2.7.1.6 
        /// </summary>
        /// <param name="input"></param>
        public void VerifyStructure(TS_INPUT_CAPABILITYSET input)
        {
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_INPUT, input.capabilitySetType,
                @"In TS_INPUT_CAPABILITY_SET structure, the capabilitySetType field MUST be set to "
                + @"CAPSTYPE_INPUT (13).");
            bool isValidKeyBoardType = input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V1 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V2 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V3 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V4 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V5 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V6 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.V7 ||
                input.keyboardType == TS_INPUT_CAPABILITYSET_keyboardType_Values.None;
            site.Assert.IsTrue(isValidKeyBoardType,
                @"In TS_INPUT_CAPABILITY_SET  structure, the keyboardType field  can take the values 1,2,3,4,5,6,7,0");

            String imeFileName = input.imeFileName;
            Console.WriteLine("imeFileName= " + imeFileName);
            Console.WriteLine("imeFileName.Length = " + imeFileName.Length.ToString());

            site.Assert.IsTrue(input.imeFileName.Length <= 32,
                @"In TS_INPUT_CAPABILITY_SET structure, the imeFileNamefield contains up to 31 Unicode characters plus a null terminator.");
        }

        /// <summary>
        /// 2.2.7.1.7
        /// </summary>
        /// <param name="brush"></param>
        public void VerifyStructure(TS_BRUSH_CAPABILITYSET brush)
        {
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_BRUSH, brush.capabilitySetType,
                @"In TS_BRUSH_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_BRUSH (15).");
            bool isValidBrushSupportLevel = brush.brushSupportLevel == brushSupportLevel_Values.BRUSH_DEFAULT ||
                brush.brushSupportLevel == brushSupportLevel_Values.BRUSH_COLOR_8x8 ||
                brush.brushSupportLevel == brushSupportLevel_Values.BRUSH_COLOR_FULL;
            site.Assert.IsTrue(isValidBrushSupportLevel,
                @"In TS_BRUSH_CAPABILITYSET structure, the brushSupportLevel can be the following: BRUSH_DEFAULT"
                + @" 0x00000000 ,BRUSH_COLOR_8x8 0x00000001,BRUSH_COLOR_FULL 0x00000002");
        }

        /// <summary>
        /// 2.2.7.1.8
        /// </summary>
        /// <param name="glyph"></param>
        public void VerifyStructure(TS_GLYPHCACHE_CAPABILITYSET glyph)
        {
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_GLYPHCACHE, glyph.capabilitySetType,
                @"In TS_GLYPHCACHE_CAPABILITYSET structure, capabilitySetType field MUST be set to CAPSTYPE_GLYPHCACHE (16).");
            site.Assert.IsTrue(glyph.FragCache.CacheEntries <= 256,
                @"In TS_GLYPHCACHE_CAPABILITYSET structure, the maximum number of entries allowed in a FragCache is"
                + @" 256, and the largest allowed maximum size of an element is 256 bytes.");
            bool isValidGlyphSupportLevel = glyph.GlyphSupportLevel == GlyphSupportLevel_Values.GLYPH_SUPPORT_NONE ||
                glyph.GlyphSupportLevel == GlyphSupportLevel_Values.GLYPH_SUPPORT_PARTIAL ||
                glyph.GlyphSupportLevel == GlyphSupportLevel_Values.GLYPH_SUPPORT_FULL ||
                glyph.GlyphSupportLevel == GlyphSupportLevel_Values.GLYPH_SUPPORT_ENCODE;
            site.Assert.IsTrue(isValidGlyphSupportLevel,
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
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL, vc.capabilitySetType,
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
            site.Assert.AreEqual<capabilitySetType_Values>(capabilitySetType_Values.CAPSTYPE_SOUND, sound.capabilitySetType,
                @"In TS_SOUND_CAPABILITYSET structure, the capabilitySetType field MUST be set to CAPSTYPE_SOUND (12).");
        }


        /// <summary>
        /// 2.2.8.1.1.3.1.1
        /// </summary>
        /// <param name="input"></param>
        public void VerifyStructure(TS_INPUT_EVENT input)
        {
            bool isValidMessageType = input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSE ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSEX ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SCANCODE ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SYNC ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNICODE ||
                input.messageType == TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNUSED;

            site.Assert.IsTrue(isValidMessageType,
                @"In TS_INPUT_EVENT structure,the messageType can be the following:INPUT_EVENT_SYNC 0x0000,"
                + @" INPUT_EVENT_SCANCODE 0x0004, INPUT_EVENT_UNICODE 0x0005, INPUT_EVENT_MOUSE 0x8001, "
                + @"INPUT_EVENT_MOUSEX 0x8002");

            switch (input.messageType)
            {
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSE:
                    site.Assert.IsInstanceOfType(input.slowPathInputData, typeof(TS_POINTER_EVENT),
                        @"In TS_INPUT_EVENT structure, when the messageType is INPUT_EVENT_MOUSE 0x8001 "
                        + @"then the input event is a Mouse Event.");
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSEX:
                    site.Assert.IsInstanceOfType(input.slowPathInputData, typeof(TS_POINTERX_EVENT),
                        @"In TS_INPUT_EVENT structure, when the messageType is INPUT_EVENT_MOUSEX 0x8002"
                        + @" then the input event is an Extended Mouse Event.");
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SCANCODE:
                    site.Assert.IsInstanceOfType(input.slowPathInputData, typeof(TS_KEYBOARD_EVENT),
                        @"In TS_INPUT_EVENT structure, when the messageType is INPUT_EVENT_SCANCODE 0x0004"
                        + @" then the input event is a Keyboard Event.");
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SYNC:
                    site.Assert.IsInstanceOfType(input.slowPathInputData, typeof(TS_SYNC_EVENT),
                        @"In TS_INPUT_EVENT structure, when the messageType is INPUT_EVENT_SYNC 0x0000 then"
                        + @" the input event is a Synchronize Event");
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNICODE:
                    site.Assert.IsInstanceOfType(input.slowPathInputData, typeof(TS_UNICODE_KEYBOARD_EVENT),
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
            bool isValidEventCode = code == 0 || code == 1 || code == 2 || code == 3 || code == 4;
            site.Assert.IsTrue(isValidEventCode,
                @"In TS_FP_INPUT_EVENT structure, eventCode can be the following: FASTPATH_INPUT_EVENT_SCANCODE"
                + @" 0x0,FASTPATH_INPUT_EVENT_MOUSE 0x1, FASTPATH_INPUT_EVENT_MOUSEX 0x2, FASTPATH_INPUT_EVENT_SYNC"
                + @" 0x3, FASTPATH_INPUT_EVENT_UNICODE 0x4");

            switch (code)
            {
                case 0:
                    site.Assert.IsInstanceOfType(input.eventData, typeof(TS_FP_KEYBOARD_EVENT),
                        @"In the eventHeader of the TS_FP_KEYBOARD_EVENT structure, the eventCode bit field (3 "
                        + @"bits in size) MUST be set to FASTPATH_INPUT_EVENT_SCANCODE (0).");
                    break;
                case 1:
                    site.Assert.IsInstanceOfType(input.eventData, typeof(TS_FP_POINTER_EVENT),
                        @"In the eventHeader of the TS_FP_POINTER_EVENT structure, the eventCode bit field (3 "
                        + @"bits in size) MUST be set to FASTPATH_INPUT_EVENT_MOUSE (1).");
                    site.Assert.AreEqual<int>(0, flag,
                        @"In the eventHeader of the TS_FP_POINTER_EVENT structure, the eventFlags bit field (5"
                        + @" bits in size) MUST be zeroed out.");
                    break;
                case 2:
                    site.Assert.IsInstanceOfType(input.eventData, typeof(TS_FP_POINTERX_EVENT),
                        @"In the eventHeader of the TS_FP_POINTERX_EVENT structure, the eventCode bit field (3"
                        + @" bits in size) MUST be set to FASTPATH_INPUT_EVENT_POINTERX (2).");
                    site.Assert.AreEqual<int>(0, flag,
                        @"In the eventHeader of the TS_FP_POINTERX_EVENT structure, the eventFlags bit field (5"
                        + @" bits in size) MUST be zeroed out.");
                    break;
                case 3:
                    site.Assert.IsTrue(input.eventData == null && code == 3,
                        @"In the eventHeader of the TS_FP_SYNC_EVENT structure, the eventCode bit field (3 bits"
                        + @" in size) MUST be set to FASTPATH_INPUT_EVENT_SYNC (3).");
                    break;
                case 4:
                    site.Assert.IsInstanceOfType(input.eventData, typeof(TS_FP_UNICODE_KEYBOARD_EVENT),
                        @"In the eventHeader of the TS_FP_UNICODE_KEYBOARD_EVENT structure, the eventCode bit field"
                        + @" (3 bits in size) MUST be set to FASTPATH_INPUT_EVENT_UNICODE (4).");
                    site.Assert.AreEqual<int>(0, flag,
                        @"In the eventHeader of the TS_FP_UNICODE_KEYBOARD_EVENT structure, the eventFlags bit field"
                        + @" (5 bits in size) MUST be zeroed out.");
                    break;

            }
        }

        #endregion "Check structure"

        #region "Helper functions"
        /// <summary>
        /// This function will output log information.        
        /// </summary>
        /// <param name="value"></param>
        /// <param name="desc"></param>
        public void CaptureRequirement(bool value, string desc)
        {
            if (value)
            {
                site.Log.Add(TestTools.LogEntryKind.Checkpoint, "@Capture Succeed: " + desc);
            }
            else
            {
                site.Log.Add(TestTools.LogEntryKind.Checkpoint, "@Capture Failed: " + desc);
            }
        }

        /// <summary>
        /// Verify the security header for all PDUs except for Client Info PDU.
        /// </summary>
        /// <param name="securityHeader">The security header to be verified</param>
        /// <param name="pduName">The pdu name</param>
        private void VerifySecurityHeader(TS_SECURITY_HEADER securityHeader, string pduName)
        {
            if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT ||
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
            {
                site.Assert.IsInstanceOfType(securityHeader, typeof(TS_SECURITY_HEADER1),
                    @"The securityHeader in "
                    + @"{0} PDU is a Non-FIPS Security Header (section 2.2.8.1.1.2.2) if the "
                    + @"Encryption Method selected by the server is "
                    + @"ENCRYPTION_METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_56BIT (0x00000008), "
                    + @"ENCRYPTION_METHOD_128BIT (0x00000002).", pduName);
            }
            else if (serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                site.Assert.IsInstanceOfType(securityHeader, typeof(TS_SECURITY_HEADER2),
                    @"The securityHeader in "
                    + @"{0} PDU is a FIPS Security Header,if the Encryption Method selected by "
                    + @"the server is ENCRYPTION_METHOD_FIPS (0x00000010).", pduName);
            }
            else if (serverConfig.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE &&
                serverConfig.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                site.Assert.IsNull(securityHeader,
                    @"{0} PDU: " +
                    @"If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption  "
                    + @"Method selected by the server is ENCRYPTION_METHOD_NONE (0), then securityHeader MUST NOT "
                    + @"be included in the PDU.", pduName);
            }
            else
            {
                site.Assert.Fail("Check the code, something is wrong with the test case. encryptionMethod: {0}, encryptionLevel: {1}",
                    serverConfig.encryptionMethod, serverConfig.encryptionLevel);
            }
        }
        #endregion "Helper functions"
    }
}
