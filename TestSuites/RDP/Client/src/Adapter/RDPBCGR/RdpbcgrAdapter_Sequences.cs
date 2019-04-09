// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrAdapter
    {
        /// <summary>
        ///  Expect a client initiated RDP connection sequence.
        /// </summary>
        /// <param name="serverSelectedProtocol">The server selected security protocol.</param>
        /// <param name="enMethod">The server selected security method.</param>
        /// <param name="enLevel">The server selected security level.</param>
        /// <param name="isExtendedClientDataSupported">Indicates if server supports Extended Client Data Blocks.</param>
        /// <param name="expectAutoReconnect">Indicates if expect an Auto-Connect sequence.</param>
        /// <param name="rdpServerVersion">The RDP Sever version</param>
        /// <param name="multiTransportTypeFlags">Flags of Multitransport Channel Data</param>
        /// <param name="supportRDPEGFX">Whether support RDPEGFX</param>
        /// <param name="supportRestrictedAdminMode">Whether support restricted admin mode</param>
        public void EstablishRDPConnection(
            selectedProtocols_Values serverSelectedProtocol,
            EncryptionMethods enMethod,
            EncryptionLevel enLevel,
            bool isExtendedClientDataSupported,
            bool expectAutoReconnect,
            TS_UD_SC_CORE_version_Values rdpServerVersion,
            MULTITRANSPORT_TYPE_FLAGS multiTransportTypeFlags = MULTITRANSPORT_TYPE_FLAGS.None,
            bool supportRDPEGFX = false,
            bool supportRestrictedAdminMode = false)
        {
            #region Logging
            this.site.Log.Add(LogEntryKind.Comment, @"EstablishRDPConnection(
                Selected Protocol = {0},
                Encyrption Method = {1},
                Encyrption Level = {2},
                Extended Client Data Supported = {3},
                Auto-Reconnection Expected = {4},
                RDP Version Code= {5}).",
                serverSelectedProtocol.ToString(), enMethod.ToString(), enLevel.ToString(), isExtendedClientDataSupported, expectAutoReconnect, rdpServerVersion.ToString());
            #endregion

            //Update server config context.
            serverConfig.selectedProtocol = serverSelectedProtocol;
            serverConfig.encryptionMethod = enMethod;
            serverConfig.encryptionLevel = enLevel;
            serverConfig.isExtendedClientDataSupported = isExtendedClientDataSupported;

            #region Connection Initiation
            //5.4.2.1   Negotiation-Based Approach
            //Once the External Security Protocol (section 5.4.5) handshake has successfully run to completion, 
            //the RDP messages resume, continuing with the MCS Connect Initial PDU (section 2.2.1.3). 
            //if (serverConfig.encryptedProtocol != EncryptedProtocol.NegotiationCredSsp)
            //{

            ExpectPacket<Client_X_224_Connection_Request_Pdu>(sessionContext, pduWaitTimeSpan);

            RDP_NEG_RSP_flags_Values RDP_NEG_RSP_flags = RDP_NEG_RSP_flags_Values.None;
            if (serverConfig.isExtendedClientDataSupported)
            {
                RDP_NEG_RSP_flags |= RDP_NEG_RSP_flags_Values.EXTENDED_CLIENT_DATA_SUPPORTED;
            }
            if (supportRDPEGFX)
            {
                RDP_NEG_RSP_flags |= RDP_NEG_RSP_flags_Values.DYNVC_GFX_PROTOCOL_SUPPORTED;
            }
            if (supportRestrictedAdminMode)
            {
                RDP_NEG_RSP_flags |= RDP_NEG_RSP_flags_Values.RESTRICTED_ADMIN_MODE_SUPPORTED;
            }
            Server_X_224_Connection_Confirm(serverConfig.selectedProtocol, RDP_NEG_RSP_flags);

            //} 
            #endregion

            #region RDSTLS authentication
            if (serverSelectedProtocol == selectedProtocols_Values.PROTOCOL_RDSTLS)
            {
                this.site.Log.Add(LogEntryKind.Comment, "Negotiation type is RDSTLS.");

                sessionContext.IsAuthenticatingRDSTLS = true;

                this.site.Log.Add(LogEntryKind.Comment, "Sending RDSTLS capability PDU.");

                SendRDSTLSCapabilityPDU();

                if (expectAutoReconnect)
                {
                    ExpectPacket<RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie>(sessionContext, pduWaitTimeSpan);
                }
                else
                {
                    ExpectPacket<RDSTLS_AuthenticationRequestPDUwithPasswordCredentials>(sessionContext, pduWaitTimeSpan);
                }

                sessionContext.IsAuthenticatingRDSTLS = false;

                this.site.Log.Add(LogEntryKind.Comment, "Sending RDSTLS authentication response PDU.");

                SendRDSTLSAuthenticationResponsePDU();
            }
            #endregion


            #region Basic Setting Exchange
            ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(sessionContext, pduWaitTimeSpan);

            Server_MCS_Connect_Response(
                enMethod,
                enLevel,
                rdpServerVersion,
                NegativeType.None,
                multiTransportTypeFlags,
                false,
                SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED,
                ConstValue.IO_CHANNEL_ID,
                ConstValue.MCS_MESSAGE_CHANNEL_ID);

            #endregion

            #region Channel Connection
            ExpectPacket<Client_MCS_Erect_Domain_Request>(sessionContext, pduWaitTimeSpan);

            ExpectPacket<Client_MCS_Attach_User_Request>(sessionContext, pduWaitTimeSpan);

            MCSAttachUserConfirm(NegativeType.None);

            //Join Channel
            int channelNum = 2;
            if (sessionContext.VirtualChannelIdStore != null)
            {
                channelNum += sessionContext.VirtualChannelIdStore.Length;
            }
            if (sessionContext.IsServerMessageChannelDataSend)
                channelNum++;
            for (int i = 0; i < channelNum; i++)
            {
                ExpectPacket<Client_MCS_Channel_Join_Request>(sessionContext, pduWaitTimeSpan);
                MCSChannelJoinConfirm(lastRequestJoinChannelId, NegativeType.None);
            }
            #endregion

            #region RDP Security Commencement
            if (serverConfig.encryptedProtocol == EncryptedProtocol.Rdp)
            {
                ExpectPacket<Client_Security_Exchange_Pdu>(sessionContext, pduWaitTimeSpan);
            }
            #endregion

            #region Secure Setting Exchange
            ExpectPacket<Client_Info_Pdu>(sessionContext, pduWaitTimeSpan);
            if (expectAutoReconnect)
            {
                site.Assert.IsNotNull(tsInfoPacket.extraInfo, "TS_EXTENDED_INFO_PACKET should be provided in Auto-Reconnect sequence.");
                site.Assert.AreNotEqual<ushort>(0, tsInfoPacket.extraInfo.cbAutoReconnectLen, "The autoReconnectCookie should be provided in Auto-Reconnect sequence.");
            }
            #endregion

            #region Licensing
            Server_License_Error_Pdu_Valid_Client(NegativeType.None);
            #endregion

            #region Capabilities Exchange
            Server_Demand_Active(NegativeType.None);

            //Once the Confirm Active PDU has been sent, the client can start sending input PDUs (see section 2.2.8) to the server.
            ExpectPacket<Client_Confirm_Active_Pdu>(sessionContext, pduWaitTimeSpan);
            #endregion

            #region Connection Finalization
            WaitForPacket<Client_Synchronize_Pdu>(sessionContext, pduWaitTimeSpan);

            ServerSynchronize();

            ServerControlCooperate();

            WaitForPacket<Client_Control_Pdu_Cooperate>(sessionContext, pduWaitTimeSpan);

            WaitForPacket<Client_Control_Pdu_Request_Control>(sessionContext, pduWaitTimeSpan);

            ServerControlGrantedControl();

            if (serverConfig.CapabilitySetting.BitmapCacheHostSupportCapabilitySet)
            {
                ITsCapsSet cap = this.clientCapSet.FindCapSet(capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_REV2);
                if (cap != null)
                {
                    TS_BITMAPCACHE_CAPABILITYSET_REV2 bitmapCacheV2 = (TS_BITMAPCACHE_CAPABILITYSET_REV2)cap;
                    if ((bitmapCacheV2.CacheFlags & CacheFlags_Values.PERSISTENT_KEYS_EXPECTED_FLAG) != 0)
                    {
                        WaitForPacket<Client_Persistent_Key_List_Pdu>(sessionContext, pduWaitTimeSpan);
                    }
                }
            }

            WaitForPacket<Client_Font_List_Pdu>(sessionContext, pduWaitTimeSpan);

            ServerFontMap();
            #endregion
        }


        /// <summary>
        /// Start a server Initiated disconnection sequence
        /// </summary>
        /// <param name="sendDisonnectPdu">Indicates if server send MCS Disconnect Provider Ultimatum PDU to client.</param>
        /// <param name="adminInitiated">Indicates it's an Administrator-Initiated disconnection or User-Initiated disconnection.</param>
        /// <param name="invalidType">Indicates the invalid type for negative cases</param>
        public void ServerInitiatedDisconnect(bool sendDisonnectPdu, bool adminInitiated, NegativeType invalidType)
        {
            ServerDeactivateAll();
            if (sendDisonnectPdu)
            {
                Server_MCSDisconnectProviderUltimatum(adminInitiated, invalidType);
            }
            rdpbcgrServerStack.Disconnect(sessionContext);
            sessionState = ServerSessionState.TransportConnected;
        }

        /// <summary>
        /// Expect a client initiated disconnection sequence.
        /// </summary>
        /// <param name="waitTimeSpan">The maximum time duration to wait.</param>
        /// <param name="respondDeniedPdu">Indicates if send the Shutdown Request PDU to client.</param>
        /// <param name="expectDisconnectPdu">Indicates if expect a client MCS Disconnect Provider Ultimatum PDU.</param>
        public void ExpectClientInitiatedDisconnect(TimeSpan waitTimeSpan, bool respondDeniedPdu, bool expectDisconnectPdu)
        {
            WaitForPacket<Client_Shutdown_Request_Pdu>(sessionContext, waitTimeSpan);

            if (respondDeniedPdu)
            {
                ServerShutdownRequestDenied();
                if (expectDisconnectPdu)
                {
                    WaitForPacket<MCS_Disconnect_Provider_Ultimatum_Pdu>(sessionContext, waitTimeSpan);
                }
            }
            else
            {
                rdpbcgrServerStack.Disconnect(sessionContext);
            }
            sessionState = ServerSessionState.TransportConnected;
        }

        /// <summary>
        /// Start a Deactivate-Reactivate sequence.
        /// </summary>
        public void DeactivateReactivate()
        {
            ServerDeactivateAll();

            #region Capabilities Exchange
            Server_Demand_Active(NegativeType.None);

            WaitForPacket<Client_Confirm_Active_Pdu>(sessionContext, pduWaitTimeSpan);
            #endregion

            #region Connection Finalization
            ExpectPacket<Client_Synchronize_Pdu>(sessionContext, pduWaitTimeSpan);

            ServerSynchronize();

            ServerControlCooperate();

            ExpectPacket<Client_Control_Pdu_Cooperate>(sessionContext, pduWaitTimeSpan);

            ExpectPacket<Client_Control_Pdu_Request_Control>(sessionContext, pduWaitTimeSpan);

            ServerControlGrantedControl();

            WaitForPacket<Client_Font_List_Pdu>(sessionContext, pduWaitTimeSpan);

            ServerFontMap();
            #endregion
        }

        /// <summary>
        /// Expect SUT start the channel join sequence
        /// </summary>
        public void ChannelJoinRequestAndConfirm(NegativeType invalidType)
        {
            int channelNum = 2;
            if (sessionContext.VirtualChannelIdStore != null)
            {
                channelNum += sessionContext.VirtualChannelIdStore.Length;
            }
            if (sessionContext.IsServerMessageChannelDataSend)
                channelNum++;
            for (int i = 0; i < channelNum; i++)
            {
                //Expect a Client MCS Channel Join Request PDU.
                ExpectPacket<Client_MCS_Channel_Join_Request>(sessionContext, pduWaitTimeSpan);

                //Respond a Server MCS Channel Join Confirm PDU.
                MCSChannelJoinConfirm(lastRequestJoinChannelId, invalidType);
                if (invalidType != NegativeType.None)
                {
                    //Expect SUT to drop the connection
                    this.site.Log.Add(LogEntryKind.Comment, "Expect SUT to drop the connection");
                    this.WaitForDisconnection(pduWaitTimeSpan);
                    break;
                }
            }
        }

    }
}
