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

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrAdapter
    {        

        #region Private Receive Method

        private StackPacket ExpectPdu(TimeSpan timeout)
        {
            StackPacket packet = rdpbcgrClientStack.ExpectPdu(timeout);
            if (packet != null)
            {
                ReceiveMessage(packet);
            }
            return packet;
        }

        private void ReceiveMessage(StackPacket packet)
        {
            if (packet == null)
            {
                return;
            }

            if (packet is Server_X_224_Connection_Confirm_Pdu)
            {
                ReceiveServerX224ConnectionConfirm(packet as Server_X_224_Connection_Confirm_Pdu);
            }
            else if (packet is Server_X_224_Negotiate_Failure_Pdu)
            {
                ReceiveServerX224NegotiateFailurePDU(packet as Server_X_224_Negotiate_Failure_Pdu);
            }
            else if (packet is Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response)
            {
                ReceiveServerMCSConnectResponse(packet as Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response);
            }
            else if (packet is Server_MCS_Attach_User_Confirm_Pdu)
            {
                ReceiveServerMCSAttachUserConfirm(packet as Server_MCS_Attach_User_Confirm_Pdu);
            }
            else if (packet is Server_MCS_Channel_Join_Confirm_Pdu)
            {
                ReceiveServerMCSChannelJoinConfirm(packet as Server_MCS_Channel_Join_Confirm_Pdu);
            }
            else if (packet is Server_License_Error_Pdu_Valid_Client)
            {
                ReceiveServerLicenseErrorPDU(packet as Server_License_Error_Pdu_Valid_Client);
            }
            else if (packet is Server_Demand_Active_Pdu)
            {
                ReceiveServerDemandActivePDU(packet as Server_Demand_Active_Pdu);
            }
            else if (packet is Server_Synchronize_Pdu)
            {
                ReceiveServerSynchronizePDU(packet as Server_Synchronize_Pdu);
            }
            else if (packet is Server_Control_Pdu_Cooperate)
            {
                ReceiveServerCooperateControlPDU(packet as Server_Control_Pdu_Cooperate);
            }
            else if (packet is Server_Control_Pdu_Granted_Control)
            {
                ReceiveServerGrantedControlPDU(packet as Server_Control_Pdu_Granted_Control);
            }
            else if (packet is Server_Font_Map_Pdu)
            {
                ReceiveServerFontMapPDU(packet as Server_Font_Map_Pdu);
            }
            else if (packet is Server_Shutdown_Request_Denied_Pdu)
            {
                ReceiveServerShutdownRequestDeniedPDU(packet as Server_Shutdown_Request_Denied_Pdu);
            }
            else if (packet is MCS_Disconnect_Provider_Ultimatum_Pdu)
            {
                ReceiveServerMCSDisconnectProviderUltimatum(packet as MCS_Disconnect_Provider_Ultimatum_Pdu);
            }
            else if (packet is Server_Deactivate_All_Pdu)
            {
                ReceiveServerDeactivateAllPDU(packet as Server_Deactivate_All_Pdu);
            }
            else if (packet is Virtual_Channel_RAW_Server_Pdu)
            {
                ReceiveServerVirtualChannelPDU(packet as Virtual_Channel_RAW_Server_Pdu);
            }
            else if (packet is SlowPathOutputPdu)
            {
                ReceiveServerSlowPathOutputUpdatePDU(packet as SlowPathOutputPdu);
            }
            else if (packet is TS_FP_UPDATE_PDU)
            {
                ReceiveServerFastPathUpdatePDU(packet as TS_FP_UPDATE_PDU);
            }
            else if (packet is Server_Redirection_Pdu)
            {
                ReceiveServerRedirectionPacket(packet as Server_Redirection_Pdu);
            }
            else if (packet is Enhanced_Security_Server_Redirection_Pdu)
            {
                ReceiveEnhancedSecurityServerRedirectionPacket(packet as Enhanced_Security_Server_Redirection_Pdu);
            }
            else if (packet is Server_Auto_Detect_Request_PDU)
            {
                ReceiveServerAutoDetectRequest(packet as Server_Auto_Detect_Request_PDU);
            }
            else if (packet is Server_Initiate_Multitransport_Request_PDU)
            {
                ReceiveServerInitiateMultitransportRequest(packet as Server_Initiate_Multitransport_Request_PDU);
            }
            else if (packet is Server_Heartbeat_PDU)
            {
                ReceiveServerHeartbeatPDU(packet as Server_Heartbeat_PDU);
            }
            else if (packet is Server_Save_Session_Info_Pdu)
            {
                ReceiveServerSaveSessionInfoPDU(packet as Server_Save_Session_Info_Pdu);
            }
        }

        private void ReceiveServerX224ConnectionConfirm(Server_X_224_Connection_Confirm_Pdu x224Confirm)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(x224Confirm);
            }

            if (OnServerX224ConnectionConfirmReceived != null)
            {
                OnServerX224ConnectionConfirmReceived(x224Confirm);
            }
        }

        private void ReceiveServerX224NegotiateFailurePDU(Server_X_224_Negotiate_Failure_Pdu x224Failure)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(x224Failure);
            }

            if (OnServerX224NegotiateFailurePDUReceived != null)
            {
                OnServerX224NegotiateFailurePDUReceived(x224Failure);
            }
        }

        private void ReceiveServerMCSConnectResponse(Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsConnectResponse)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(mcsConnectResponse);
            }

            if (OnServerMCSConnectResponseReceived != null)
            {
                OnServerMCSConnectResponseReceived(mcsConnectResponse);
            }
        }

        private void ReceiveServerMCSAttachUserConfirm(Server_MCS_Attach_User_Confirm_Pdu attachUserConfirm)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(attachUserConfirm);
            }

            if (OnServerMCSAttachUserConfirmReceived != null)
            {
                OnServerMCSAttachUserConfirmReceived(attachUserConfirm);
            }
        }

        private void ReceiveServerMCSChannelJoinConfirm(Server_MCS_Channel_Join_Confirm_Pdu channelJoinConfirm)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(channelJoinConfirm);
            }

            if (OnServerMCSChannelJoinConfirmReceived != null)
            {
                OnServerMCSChannelJoinConfirmReceived(channelJoinConfirm);
            }
        }

        private void ReceiveServerLicenseErrorPDU(Server_License_Error_Pdu_Valid_Client licenseErrorPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(licenseErrorPdu);
            }

            if (OnServerLicenseErrorPDUReceived != null)
            {
                OnServerLicenseErrorPDUReceived(licenseErrorPdu);
            }
        }

        private void ReceiveServerDemandActivePDU(Server_Demand_Active_Pdu demandActivePdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(demandActivePdu);
            }

            if (OnServerDemandActivePDUReceived != null)
            {
                OnServerDemandActivePDUReceived(demandActivePdu);
            }
        }

        private void ReceiveServerSynchronizePDU(Server_Synchronize_Pdu synchronizePdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(synchronizePdu);
            }

            if (OnServerSynchronizePDUReceived != null)
            {
                OnServerSynchronizePDUReceived(synchronizePdu);
            }
        }

        private void ReceiveServerCooperateControlPDU(Server_Control_Pdu_Cooperate controlPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(controlPdu);
            }

            if (OnServerCooperateControlPDUReceived != null)
            {
                OnServerCooperateControlPDUReceived(controlPdu);
            }
        }

        private void ReceiveServerGrantedControlPDU(Server_Control_Pdu_Granted_Control controlPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(controlPdu);
            }

            if (OnServerGrantedControlPDUReceived != null)
            {
                OnServerGrantedControlPDUReceived(controlPdu);
            }
        }

        private void ReceiveServerFontMapPDU(Server_Font_Map_Pdu fontMapPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(fontMapPdu);
            }
                        

            if (OnServerFontMapPDUReceived != null)
            {
                OnServerFontMapPDUReceived(fontMapPdu);
            }
        }

        private void ReceiveServerShutdownRequestDeniedPDU(Server_Shutdown_Request_Denied_Pdu shutdownRequest)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(shutdownRequest);
            }

            if (OnServerShutdownRequestDeniedReceived != null)
            {
                OnServerShutdownRequestDeniedReceived(shutdownRequest);
            }
        }

        private void ReceiveServerMCSDisconnectProviderUltimatum(MCS_Disconnect_Provider_Ultimatum_Pdu disconnectProvider)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(disconnectProvider);
            }

            if (OnServerMCSDisconnectProviderUltimatumReceived != null)
            {
                OnServerMCSDisconnectProviderUltimatumReceived(disconnectProvider);
            }
        }

        private void ReceiveServerDeactivateAllPDU(Server_Deactivate_All_Pdu deactiveAllPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(deactiveAllPdu);
            }

            if (OnServerDeactivateAllPDUReceived != null)
            {
                OnServerDeactivateAllPDUReceived(deactiveAllPdu);
            }
        }

        private void ReceiveServerVirtualChannelPDU(Virtual_Channel_RAW_Server_Pdu virtualChannelPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(virtualChannelPdu);
            }

            if (OnServerVirtualChannelPDUReceived != null)
            {
                OnServerVirtualChannelPDUReceived(virtualChannelPdu);
            }
        }

        private void ReceiveServerSlowPathOutputUpdatePDU(SlowPathOutputPdu updatePdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(updatePdu);
            }

            if (OnServerSlowPathOutputUpdatePDUReceived != null)
            {
                OnServerSlowPathOutputUpdatePDUReceived(updatePdu);
            }
        }

        private void ReceiveServerFastPathUpdatePDU(TS_FP_UPDATE_PDU updatePdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(updatePdu);
            }

            if (OnServerFastPathUpdatePDUReceived != null)
            {
                OnServerFastPathUpdatePDUReceived(updatePdu);
            }
        }

        private void ReceiveServerRedirectionPacket(Server_Redirection_Pdu redirectionPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(redirectionPdu);
            }

            if (OnServerRedirectionPacketReceived != null)
            {
                OnServerRedirectionPacketReceived(redirectionPdu);
            }
        }

        private void ReceiveEnhancedSecurityServerRedirectionPacket(Enhanced_Security_Server_Redirection_Pdu redirectionPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(redirectionPdu);
            }

            if (OnEnhancedSecurityServerRedirectionPacketReceived != null)
            {
                OnEnhancedSecurityServerRedirectionPacketReceived(redirectionPdu);
            }
        }

        private void ReceiveServerAutoDetectRequest(Server_Auto_Detect_Request_PDU autoDetectRequest)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(autoDetectRequest);
            }

            if (OnServerAutoDetectRequestReceived != null)
            {
                OnServerAutoDetectRequestReceived(autoDetectRequest);
            }
        }

        private void ReceiveServerInitiateMultitransportRequest(Server_Initiate_Multitransport_Request_PDU multitransportReq)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(multitransportReq);
            }

            if (OnServerInitiateMultitransportRequestReceived != null)
            {
                OnServerInitiateMultitransportRequestReceived(multitransportReq);
            }
        }

        private void ReceiveServerHeartbeatPDU(Server_Heartbeat_PDU heartbeatPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(heartbeatPdu);
            }

            if (OnServerHeartbeatPDUReceived != null)
            {
                OnServerHeartbeatPDUReceived(heartbeatPdu);
            }
        }

        private void ReceiveServerSaveSessionInfoPDU(Server_Save_Session_Info_Pdu saveSessionInfoPdu)
        {
            if (verifyPduEnabled)
            {
                VerifyPdu(saveSessionInfoPdu);
            }

            if(saveSessionInfoPdu != null && 
                (saveSessionInfoPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON ||saveSessionInfoPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON_LONG || saveSessionInfoPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON_PLAINNOTIFY))
            {
                // Notify logon.
                isLogon = true;
            }

            if (OnServerSaveSessionInfoPDUReceived != null)
            {
                OnServerSaveSessionInfoPDUReceived(saveSessionInfoPdu);
            }
        }
        #endregion Private Receive Method

        #region Verify PDUs

        #region Connection Initiation

        /// <summary>
        /// Verify Server_X_224_Connection_Confirm_Pdu received
        /// </summary>
        /// <param name="x224Confirm">The Server_X_224_Connection_Confirm_Pdu received</param>
        public void VerifyPdu(Server_X_224_Connection_Confirm_Pdu x224Confirm)
        {
            if (x224Confirm == null)
            {
                return;
            }

            RDP_NEG_RSP rdpNegData = x224Confirm.rdpNegData;
            if (rdpNegData != null)
            {
                Site.Assert.AreEqual<RDP_NEG_RSP_type_Values>(RDP_NEG_RSP_type_Values.V1, rdpNegData.type, "The type field of RDP_NEG_RSP MUST be set to 0x02 (TYPE_RDP_NEG_RSP).");

                byte flags = (byte)(RDP_NEG_RSP_flags_Values.EXTENDED_CLIENT_DATA_SUPPORTED | RDP_NEG_RSP_flags_Values.DYNVC_GFX_PROTOCOL_SUPPORTED | RDP_NEG_RSP_flags_Values.NEGRSP_FLAG_RESERVED | RDP_NEG_RSP_flags_Values.RESTRICTED_ADMIN_MODE_SUPPORTED | RDP_NEG_RSP_flags_Values.REDIRECTED_AUTHENTICATION_MODE_SUPPORTED);
                byte negFlags = (byte)~flags;
                Site.Assert.AreEqual<byte>(0, (byte)(((byte)(rdpNegData.flags)) & negFlags), "The flags field of RDP_NEG_RSP contains protocol flags: "
                    + "EXTENDED_CLIENT_DATA_SUPPORTED (0x01), DYNVC_GFX_PROTOCOL_SUPPORTED (0x02), NEGRSP_FLAG_RESERVED (0x04),  RESTRICTED_ADMIN_MODE_SUPPORTED (0x08).");

                Site.Assert.AreEqual<RDP_NEG_RSP_length_Values>(RDP_NEG_RSP_length_Values.V1, rdpNegData.length, "The length field of RDP_NEG_RSP MUST be set to 0x0008 (8 bytes)");

                Site.Assert.IsTrue(rdpNegData.selectedProtocol == selectedProtocols_Values.PROTOCOL_RDP_FLAG
                    || rdpNegData.selectedProtocol == selectedProtocols_Values.PROTOCOL_SSL_FLAG
                    || rdpNegData.selectedProtocol == selectedProtocols_Values.PROTOCOL_HYBRID_FLAG
                    || rdpNegData.selectedProtocol == selectedProtocols_Values.PROTOCOL_HYBRID_EX, "The selectedProtocol field of RDP_NEG_RSP specifies the selected security protocol, it MUST take one of the following value:"
                    + "PROTOCOL_RDP (0x00000000), PROTOCOL_SSL (0x00000001), PROTOCOL_HYBRID (0x00000002), PROTOCOL_HYBRID_EX (0x00000008).");
            }
        }

        /// <summary>
        /// Verify Server_X_224_Negotiate_Failure_Pdu Received
        /// </summary>
        /// <param name="x224Failure">The Server_X_224_Negotiate_Failure_Pdu received</param>
        public void VerifyPdu(Server_X_224_Negotiate_Failure_Pdu x224Failure)
        {
            if (x224Failure == null)
            {
                return;
            }

            RDP_NEG_FAILURE rdpNegData = x224Failure.rdpNegFailure;
            if (rdpNegData != null)
            {
                Site.Assert.AreEqual<RDP_NEG_FAILURE_type_Values>(RDP_NEG_FAILURE_type_Values.V1, rdpNegData.type, "In RDP Negotiation Failure, the type field MUST be set to 0x03 (TYPE_RDP_NEG_FAILURE).");
                Site.Assert.AreEqual<RDP_NEG_FAILURE_flags_Values>(RDP_NEG_FAILURE_flags_Values.V1, rdpNegData.flags, "In RDP Negotiation Failure, the flags field MUST be set to 0x00.");
                Site.Assert.AreEqual<RDP_NEG_FAILURE_length_Values>(RDP_NEG_FAILURE_length_Values.V1, rdpNegData.length, "In RDP Negotiation Failure, the length field MUST be set to 0x0008 (8 bytes).");
                Site.Assert.IsTrue(rdpNegData.failureCode == failureCode_Values.SSL_REQUIRED_BY_SERVER
                    || rdpNegData.failureCode == failureCode_Values.SSL_NOT_ALLOWED_BY_SERVER
                    || rdpNegData.failureCode == failureCode_Values.SSL_CERT_NOT_ON_SERVER
                    || rdpNegData.failureCode == failureCode_Values.INCONSISTENT_FLAGS
                    || rdpNegData.failureCode == failureCode_Values.HYBRID_REQUIRED_BY_SERVER
                    || rdpNegData.failureCode == failureCode_Values.SSL_WITH_USER_AUTH_REQUIRED_BY_SERVER, "In RDP Negotiation Failure, the failureCode field specifies the failure code: "
                    + "SSL_REQUIRED_BY_SERVER (0x00000001), SSL_NOT_ALLOWED_BY_SERVER (0x00000002), SSL_CERT_NOT_ON_SERVER (0x00000003), INCONSISTENT_FLAGS (0x00000004), HYBRID_REQUIRED_BY_SERVER (0x00000005), SSL_WITH_USER_AUTH_REQUIRED_BY_SERVER (0x00000006).");
            }

        }

        #endregion Connection Initiation

        #region Basic Setting Exchange

        /// <summary>
        /// Verify Server MCS Connect Response PDU message received
        /// </summary>
        /// <param name="mcsConnectResponse">The Server MCS Connect Response PDU received</param>
        public void VerifyPdu(Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsConnectResponse)
        {
            if (mcsConnectResponse == null)
            {
                return;
            }

            Site.Assert.IsNotNull(mcsConnectResponse.mcsCrsp, "In Server MCS Connect Response PDU with GCC Conference Create Response, mcsCrsp cannot be null.");

            Site.Assert.IsNotNull(mcsConnectResponse.mcsCrsp.gccPdu, "In Server MCS Connect Response PDU with GCC Conference Create Response, gccCCrsp cannot be null.");

            if (mcsConnectResponse.mcsCrsp.gccPdu.serverCoreData != null)
            {
                VerifyStructure(mcsConnectResponse.mcsCrsp.gccPdu.serverCoreData);
            }

            if (mcsConnectResponse.mcsCrsp.gccPdu.serverNetworkData != null)
            {
                VerifyStructure(mcsConnectResponse.mcsCrsp.gccPdu.serverNetworkData);
            }

            if (mcsConnectResponse.mcsCrsp.gccPdu.serverSecurityData != null)
            {
                VerifyStructure(mcsConnectResponse.mcsCrsp.gccPdu.serverSecurityData);
            }

            if (mcsConnectResponse.mcsCrsp.gccPdu.serverMessageChannelData != null)
            {
                VerifyStructure(mcsConnectResponse.mcsCrsp.gccPdu.serverMessageChannelData);
            }

            if (mcsConnectResponse.mcsCrsp.gccPdu.serverMultitransportChannelData != null)
            {
                VerifyStructure(mcsConnectResponse.mcsCrsp.gccPdu.serverMultitransportChannelData);
            }

        }

        #endregion Basic Setting Exchange

        #region Channel Connection

        /// <summary>
        /// Verify Server_MCS_Attach_User_Confirm_Pdu received
        /// </summary>
        /// <param name="attachUserConfirm">The Server_MCS_Attach_User_Confirm_Pdu received</param>
        public void VerifyPdu(Server_MCS_Attach_User_Confirm_Pdu attachUserConfirm)
        {
            // Nothing to verify
        }

        /// <summary>
        /// Verify Server_MCS_Channel_Join_Confirm_Pdu received
        /// </summary>
        /// <param name="channelJoinConfirm">The Server_MCS_Channel_Join_Confirm_Pdu received</param>
        public void VerifyPdu(Server_MCS_Channel_Join_Confirm_Pdu channelJoinConfirm)
        {
            // Nothing to verify
        }

        #endregion Channel Connection

        #region Licensing

        /// <summary>
        /// Verify Server_License_Error_Pdu_Valid_Client received
        /// </summary>
        /// <param name="licenseErrorPdu">The Server_License_Error_Pdu_Valid_Client received</param>
        public void VerifyPdu(Server_License_Error_Pdu_Valid_Client licenseErrorPdu)
        {
            if (licenseErrorPdu == null)
            {
                return;
            }

            Site.Assert.IsNotNull(licenseErrorPdu.commonHeader.securityHeader, "In Server_License_Error_Pdu_Valid_Client, the security header Must be present.");

            Site.Assert.IsTrue(licenseErrorPdu.commonHeader.securityHeader.flags.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_PKT), "In Server_License_Error_Pdu_Valid_Client, the flags field of the security header MUST contain the SEC_LICENSE_PKT (0x0080) flag ");

            Site.Assert.AreEqual<bMsgType_Values>(bMsgType_Values.ERROR_ALERT, licenseErrorPdu.preamble.bMsgType, "In Server_License_Error_Pdu_Valid_Client, the bMsgType field of the preamble structure MUST be set to ERROR_ALERT (0xFF).");

            Site.Assert.AreEqual<dwStateTransition_Values>(dwStateTransition_Values.ST_NO_TRANSITION, licenseErrorPdu.validClientMessage.dwStateTransition, "In Server_License_Error_Pdu_Valid_Client, the dwStateTransition field of the validClientMessage field MUST be set to ST_NO_TRANSITION (0x00000002).");

            Site.Assert.AreEqual<wBlobType_Values>(wBlobType_Values.BB_ERROR_BLOB, licenseErrorPdu.validClientMessage.bbErrorInfo.wBlobType, "In Server_License_Error_Pdu_Valid_Client, the bbErrorInfo field of the validClientMessage MUST contain an empty binary large object (BLOB) of type BB_ERROR_BLOB (0x0004).");
        }

        #endregion Licensing

        #region Capabilities Exchange

        /// <summary>
        /// Verify Server_Demand_Active_Pdu received
        /// </summary>
        /// <param name="demandActivePdu">The Server_Demand_Active_Pdu received</param>
        public void VerifyPdu(Server_Demand_Active_Pdu demandActivePdu)
        {
            if (demandActivePdu == null)
            {
                return;
            }

            if (rdpbcgrClientStack.Context.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE && rdpbcgrClientStack.Context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(demandActivePdu.commonHeader.securityHeader, "In Server_Demand_Active_Pdu, If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is ENCRYPTION_METHOD_NONE (0), then the security header MUST NOT be included in the PDU.");
            }

            // TBD: verify all capability sets in capabilitySets field one by one
        }

        #endregion Capabilities Exchange

        #region Connection Finalization

        /// <summary>
        /// Verify Server_Synchronize_Pdu received
        /// </summary>
        /// <param name="synchronizePdu">The Server_Synchronize_Pdu received</param>
        public void VerifyPdu(Server_Synchronize_Pdu synchronizePdu)
        {
            if (synchronizePdu == null)
            {
                return;
            }

            if (rdpbcgrClientStack.Context.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE && rdpbcgrClientStack.Context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(synchronizePdu.commonHeader.securityHeader, "In Server_Synchronize_Pdu, If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is ENCRYPTION_METHOD_NONE (0), then the security header MUST NOT be included in the PDU.");
            }

            Site.Assert.AreEqual<messageType_Values>(messageType_Values.V1, synchronizePdu.synchronizePduData.messageType, "In Server_Synchronize_Pdu, the messageType field of the synchronizePduData field MUST be set to SYNCMSGTYPE_SYNC (1).");
        }

        /// <summary>
        /// Verify Server_Control_Pdu_Granted_Control received
        /// </summary>
        /// <param name="controlPdu">The Server_Control_Pdu_Granted_Control received</param>
        public void VerifyPdu(Server_Control_Pdu_Granted_Control controlPdu)
        {
            if (controlPdu == null)
            {
                return;
            }

            if (rdpbcgrClientStack.Context.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE && rdpbcgrClientStack.Context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(controlPdu.commonHeader.securityHeader, "In Server_Control_Pdu_Granted_Control, If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is ENCRYPTION_METHOD_NONE (0), then the security header MUST NOT be included in the PDU.");
            }

            Site.Assert.AreEqual<ushort>((ushort)rdpbcgrClientStack.Context.UserChannelId, controlPdu.controlPduData.grantId, "In Server Control PDU - Granted Control, the grantId field of the controlPduData field MUST be set to the User Channel ID.");
            Site.Assert.AreEqual<uint>(0x03EA, controlPdu.controlPduData.controlId, "In Server Control PDU - Granted Control, the controlId field of the controlPduData field MUST be set to the server channel ID (0x03EA).");
            Site.Assert.AreEqual<action_Values>(action_Values.CTRLACTION_GRANTED_CONTROL, controlPdu.controlPduData.action, "In Server Control PDU - Granted Control, the action field of the controlPduData field MUST be set to CTRLACTION_GRANTED_CONTROL (0x0002).");
        }

        /// <summary>
        /// Verify Server_Control_Pdu_Cooperate received
        /// </summary>
        /// <param name="controlPdu">The Server_Control_Pdu_Cooperate received</param>
        public void VerifyPdu(Server_Control_Pdu_Cooperate controlPdu)
        {
            if (controlPdu == null)
            {
                return;
            }

            if (rdpbcgrClientStack.Context.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE && rdpbcgrClientStack.Context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(controlPdu.commonHeader.securityHeader, "In Server_Control_Pdu_Cooperate, If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is ENCRYPTION_METHOD_NONE (0), then the security header MUST NOT be included in the PDU.");
            }

            Site.Assert.AreEqual<ushort>(0, controlPdu.controlPduData.grantId, "In Server Control PDU - Cooperate, the grantId field of the controlPduData field MUST be set to 0.");
            Site.Assert.AreEqual<uint>(0, controlPdu.controlPduData.controlId, "In Server Control PDU - Cooperate, the controlId field of the controlPduData field MUST be set to 0.");
            Site.Assert.AreEqual<action_Values>(action_Values.CTRLACTION_COOPERATE, controlPdu.controlPduData.action, "In Server Control PDU - Cooperate, the action field of the controlPduData field MUST be set to CTRLACTION_COOPERATE (0x0004).");
        }

        /// <summary>
        /// Verify Server_Font_Map_Pdu received
        /// </summary>
        /// <param name="fontMapPdu">The Server_Font_Map_Pdu received</param>
        public void VerifyPdu(Server_Font_Map_Pdu fontMapPdu)
        {
            if (fontMapPdu == null)
            {
                return;
            }

            if (rdpbcgrClientStack.Context.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE && rdpbcgrClientStack.Context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(fontMapPdu.commonHeader.securityHeader, "In Server_Font_Map_Pdu, If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is ENCRYPTION_METHOD_NONE (0), then the security header MUST NOT be included in the PDU.");
            }

            if (verifyShouldBehaviors)
            {
                Site.Assert.AreEqual<ushort>(0, fontMapPdu.fontMapPduData.numberEntries, "In Server_Font_Map_Pdu, the numberEntries field of the fontMapPduData field should be set to zero.");
                Site.Assert.AreEqual<ushort>(0, fontMapPdu.fontMapPduData.totalNumEntries, "In Server_Font_Map_Pdu, the totalNumEntries field of the fontMapPduData field should be set to zero.");
                Site.Assert.AreEqual<ushort>(0x0003, fontMapPdu.fontMapPduData.mapFlags, "In Server_Font_Map_Pdu, the mapFlags field of the fontMapPduData field should be set to 0x0003, which is the logical OR'ed value of FONTMAP_FIRST (0x0001) and FONTMAP_LAST (0x0002).");
                Site.Assert.AreEqual<ushort>(0x0004, fontMapPdu.fontMapPduData.entrySize, "In Server_Font_Map_Pdu, the entrySize field of the fontMapPduData field should be set to 0x0004 (4 bytes).");
            }
        }

        #endregion Connection Finalization

        #region Disconnection sequences

        /// <summary>
        /// Verify Server_Shutdown_Request_Denied_Pdu received
        /// </summary>
        /// <param name="shutdownRequest">The Server_Shutdown_Request_Denied_Pdu received</param>
        public void VerifyPdu(Server_Shutdown_Request_Denied_Pdu shutdownRequest)
        {
            if (shutdownRequest == null)
            {
                return;
            }
            if (rdpbcgrClientStack.Context.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE && rdpbcgrClientStack.Context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(shutdownRequest.commonHeader.securityHeader, "In Server_Shutdown_Request_Denied_Pdu, If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is ENCRYPTION_METHOD_NONE (0), then the security header MUST NOT be included in the PDU.");
            }
        }

        /// <summary>
        /// Verify MCS Disconnect Provider Ultimatum PDU received
        /// </summary>
        /// <param name="disconnectProvider">the MCS Disconnect Provider Ultimatum PDU received</param>
        public void VerifyPdu(MCS_Disconnect_Provider_Ultimatum_Pdu disconnectProvider)
        {
            // Nothing to verify
        }

        #endregion  Disconnection sequences

        #region Deactivation-Reactivation Sequence

        /// <summary>
        /// Verify Server_Deactivate_All_Pdu received
        /// </summary>
        /// <param name="deactiveAllPdu">The Server_Deactivate_All_Pdu received</param>
        public void VerifyPdu(Server_Deactivate_All_Pdu deactiveAllPdu)
        {
            if (deactiveAllPdu == null)
            {
                return;
            }

            if (rdpbcgrClientStack.Context.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE && rdpbcgrClientStack.Context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(deactiveAllPdu.commonHeader.securityHeader, "In Server_Deactivate_All_Pdu, If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is ENCRYPTION_METHOD_NONE (0), then the security header MUST NOT be included in the PDU.");
            }
        }

        #endregion Deactivation-Reactivation Sequence

        #region Static Virtual Channels

        /// <summary>
        /// Verify virtual channel pdu received
        /// </summary>
        /// <param name="virtualChannelPdu"></param>
        public void VerifyPdu(Virtual_Channel_RAW_Server_Pdu virtualChannelPdu)
        {
            if (virtualChannelPdu == null)
            {
                return;
            }

            if (rdpbcgrClientStack.Context.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE && rdpbcgrClientStack.Context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(virtualChannelPdu.commonHeader.securityHeader, "In Virtual Channel PDU, If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is ENCRYPTION_METHOD_NONE (0), then the security header MUST NOT be included in the PDU.");
            }

            uint flags = (uint)(CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_FIRST | CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_LAST
                | CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_SHOW_PROTOCOL | CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_SUSPEND
                | CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_RESUME | CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_COMPRESSED
                | CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_AT_FRONT | CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_FLUSHED
                | CHANNEL_PDU_HEADER_flags_Values.CompressionTypeMask);
            uint negFlags = (uint)(~flags);

            Site.Assert.AreEqual<uint>(0, (uint)((uint)virtualChannelPdu.channelPduHeader.flags & negFlags), "In Virtual Channel PDU, the value of flags field in channelPduHeader is defined as following:"
                + "CHANNEL_FLAG_FIRST (0x00000001), CHANNEL_FLAG_LAST (0x00000002), CHANNEL_FLAG_SHOW_PROTOCOL (0x00000010), CHANNEL_FLAG_SUSPEND (0x00000020), CHANNEL_FLAG_RESUME (0x00000040), CHANNEL_PACKET_COMPRESSED (0x00200000), CHANNEL_PACKET_AT_FRONT (0x00400000), CHANNEL_PACKET_FLUSHED (0x00800000), CompressionTypeMask (0x000F0000).");

            uint compressType = ((uint)(CHANNEL_PDU_HEADER_flags_Values.CompressionTypeMask & virtualChannelPdu.channelPduHeader.flags)) >> 16;
            Site.Assert.IsTrue(compressType == (uint)CompressionType.PACKET_COMPR_TYPE_8K
                || compressType == (uint)CompressionType.PACKET_COMPR_TYPE_64K
                || compressType == (uint)CompressionType.PACKET_COMPR_TYPE_RDP6
                || compressType == (uint)CompressionType.PACKET_COMPR_TYPE_RDP6, "In Virtual Channel PDU, Possible compression types are as follows:"
                + "PACKET_COMPR_TYPE_8K (0x0), PACKET_COMPR_TYPE_64K (0x1), PACKET_COMPR_TYPE_RDP6 (0x2), PACKET_COMPR_TYPE_RDP61 (0x3).");
        }

        #endregion Static Virtual Channels

        #region Basic Output

        /// <summary>
        /// Verify Server Graphics Update PDU or Server Pointer Update PDU.
        /// </summary>
        /// <param name="updatePdu"></param>
        public void VerifyPdu(SlowPathOutputPdu updatePdu)
        {
            if (updatePdu == null)
            {
                return;
            }

            if (rdpbcgrClientStack.Context.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE && rdpbcgrClientStack.Context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(updatePdu.commonHeader.securityHeader, "In Graphics Update PDU or Server Pointer Update PDU, If the Encryption Level selected by the server is ENCRYPTION_LEVEL_NONE (0) and the Encryption Method selected by the server is ENCRYPTION_METHOD_NONE (0), then the security header MUST NOT be included in the PDU.");
            }

            if(updatePdu.slowPathUpdates != null)
            {
                foreach (RdpbcgrSlowPathUpdatePdu spUpdate in updatePdu.slowPathUpdates)
                {
                    VerifyStructure(spUpdate);
                }
            }
        }

        /// <summary>
        /// Verify Server Fast-Path Update PDU received
        /// </summary>
        /// <param name="updatePdu">The Server Fast-Path Update PDU received</param>
        public void VerifyPdu(TS_FP_UPDATE_PDU updatePdu)
        {
            if (updatePdu == null)
            {
                return;
            }

            if (updatePdu.fpOutputUpdates != null)
            {
                foreach (TS_FP_UPDATE update in updatePdu.fpOutputUpdates)
                {
                    VerifyStructure(update);
                }
            }
        }

        #endregion Basic Output

        #region Redirection

        /// <summary>
        /// Verify Standard Security Server Redirection PDU received
        /// </summary>
        /// <param name="redirectionPdu">The Server_Redirection_Pdu received</param>
        public void VerifyPdu(Server_Redirection_Pdu redirectionPdu)
        {
            if (redirectionPdu == null)
            {
                return;
            }

            Site.Assert.IsNotNull(redirectionPdu.commonHeader.securityHeader, "In Standard Security Server Redirection PDU, the security header Must be present.");

            Site.Assert.IsTrue(redirectionPdu.commonHeader.securityHeader.flags.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_REDIRECTION_PKT), "In Standard Security Server Redirection PDU , the flags field of the security header MUST contain the SEC_REDIRECTION_PKT (0x0400) flag.");

            VerifyStructure(redirectionPdu.serverRedirectionPdu);
        }

        /// <summary>
        /// Verify Enhanced Security Server Redirection PDU received
        /// </summary>
        /// <param name="redirectionPdu">The Enhanced_Security_Server_Redirection_Pdu received</param>
        public void VerifyPdu(Enhanced_Security_Server_Redirection_Pdu redirectionPdu)
        {
            if (redirectionPdu == null)
            {
                return;
            }

            VerifyStructure(redirectionPdu.serverRedirectionPdu);
        }

        #endregion Redirection

        #region Auto-Detect

        /// <summary>
        /// Verify Server_Auto_Detect_Request_PDU received
        /// </summary>
        /// <param name="autoDetectRequest">The Server_Auto_Detect_Request_PDU received</param>
        public void VerifyPdu(Server_Auto_Detect_Request_PDU autoDetectRequest)
        {
            if (autoDetectRequest == null)
            {
                return;
            }

            Site.Assert.IsNotNull(autoDetectRequest.commonHeader.securityHeader, "In Server_Auto_Detect_Request_PDU, the security header Must be present.");
            Site.Assert.IsTrue(autoDetectRequest.commonHeader.securityHeader.flags.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_AUTODETECT_REQ), "In Server_Auto_Detect_Request_PDU, the flags field of the security header MUST contain the SEC_AUTODETECT_REQ (0x1000) flag.");

            VerifyStructure(autoDetectRequest.autoDetectReqData);
        
        }

        #endregion Auto-Detect

        #region Multitransport Bootstrapping

        /// <summary>
        /// Verify Server_Initiate_Multitransport_Request_PDU received
        /// </summary>
        /// <param name="multitransportReq">The Server_Initiate_Multitransport_Request_PDU received</param>
        public void VerifyPdu(Server_Initiate_Multitransport_Request_PDU multitransportReq)
        {
            if (multitransportReq == null)
            {
                return;
            }

            Site.Assert.IsNotNull(multitransportReq.commonHeader.securityHeader, "In Server_Initiate_Multitransport_Request_PDU, the security header Must be present.");
            Site.Assert.IsTrue(multitransportReq.commonHeader.securityHeader.flags.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_TRANSPORT_REQ), "In Server_Initiate_Multitransport_Request_PDU, the flags field of the security header MUST contain the SEC_TRANSPORT_REQ (0x0002) flag.");
            Site.Assert.IsTrue(multitransportReq.requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR
                || multitransportReq.requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL, "In Server_Initiate_Multitransport_Request_PDU, the requestedProtocol field specifies the protocol to use in the transport: "
                + "INITITATE_REQUEST_PROTOCOL_UDPFECR (0x01), INITITATE_REQUEST_PROTOCOL_UDPFECL (0x02).");
        }

        #endregion Multitransport Bootstrapping

        #region Connection Health Monitoring

        /// <summary>
        /// Verify Server_Heartbeat_PDU received
        /// </summary>
        /// <param name="heartbeatPdu">The Server_Heartbeat_PDU received</param>
        public void VerifyPdu(Server_Heartbeat_PDU heartbeatPdu)
        {
            if (heartbeatPdu == null)
            {
                return;
            }

            Site.Assert.IsNotNull(heartbeatPdu.commonHeader.securityHeader, "In Server_Heartbeat_PDU, the security header Must be present.");
            Site.Assert.IsTrue(heartbeatPdu.commonHeader.securityHeader.flags.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_HEARTBEAT), "In Server_Heartbeat_PDU, the flags field of the security header MUST contain the SEC_HEARTBEAT (0x4000) flag.");
            
        }

        #endregion Connection Health Monitoring

        /// <summary>
        /// Verify Server_Save_Session_Info_Pdu received
        /// </summary>
        /// <param name="saveSessionPdu">The Server_Save_Session_Info_Pdu received</param>
        public void VerifyPdu(Server_Save_Session_Info_Pdu saveSessionPdu)
        {
            if (saveSessionPdu == null)
            {
                return;
            }

            if (saveSessionPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON)
            {
                Site.Assert.IsTrue(saveSessionPdu.saveSessionInfoPduData.infoData is TS_LOGON_INFO, "In Server_Save_Session_Info_Pdu, infoType field is INFOTYPE_LOGON indicates that The infoData field which follows contains a Logon Info Version 1 structure.");
                
            }
            else if (saveSessionPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON_LONG)
            {
                Site.Assert.IsTrue(saveSessionPdu.saveSessionInfoPduData.infoData is TS_LOGON_INFO_VERSION_2, "In Server_Save_Session_Info_Pdu, infoType field is INFOTYPE_LOGON_LONG indicates that The infoData field which follows contains a Logon Info Version 2 structure.");
                TS_LOGON_INFO_VERSION_2 logonInfoVersion2 = (TS_LOGON_INFO_VERSION_2)saveSessionPdu.saveSessionInfoPduData.infoData;
                Site.Assert.AreEqual<TS_LOGON_INFO_VERSION_2_Version_Values>(TS_LOGON_INFO_VERSION_2_Version_Values.SAVE_SESSION_PDU_VERSION_ONE, logonInfoVersion2.Version,
                    "In TS_LOGON_INFO_VERSION_2 structure, version field MUST be SAVE_SESSION_PDU_VERSION_ONE (0x0001).");
            }
            else if (saveSessionPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON_PLAINNOTIFY)
            {
                Site.Assert.IsTrue(saveSessionPdu.saveSessionInfoPduData.infoData is TS_PLAIN_NOTIFY, "In Server_Save_Session_Info_Pdu, infoType field is INFOTYPE_LOGON_PLAINNOTIFY indicates that The infoData field which follows contains a Plain Notify structure.");
            }
            else if (saveSessionPdu.saveSessionInfoPduData.infoType == infoType_Values.INFOTYPE_LOGON_EXTENDED_INF)
            {
                Site.Assert.IsTrue(saveSessionPdu.saveSessionInfoPduData.infoData is TS_LOGON_INFO_EXTENDED, "In Server_Save_Session_Info_Pdu, infoType field is INFOTYPE_LOGON_EXTENDED_INF indicates that The infoData field which follows contains a Logon Info Extended structure");
                TS_LOGON_INFO_EXTENDED logInfoExtend = (TS_LOGON_INFO_EXTENDED)saveSessionPdu.saveSessionInfoPduData.infoData;
                uint flags = (uint)(FieldsPresent_Values.LOGON_EX_AUTORECONNECTCOOKIE | FieldsPresent_Values.LOGON_EX_LOGONERRORS);
                uint negFlags = ~flags;
                bool isSatisfy = (negFlags & (uint)logInfoExtend.FieldsPresent) == 0;
                Site.Assert.IsTrue(isSatisfy, "In TS_LOGON_INFO_EXTENDED, the FieldsPresent field indicates which fields are present in the LogonFields field, the flags contain: "
                    + "LOGON_EX_AUTORECONNECTCOOKIE (0x00000001), LOGON_EX_LOGONERRORS (0x00000002).");

                if (logInfoExtend.FieldsPresent != FieldsPresent_Values.None)
                {
                    isSatisfy = logInfoExtend.LogonFields != null && logInfoExtend.LogonFields.Length > 0;
                    Site.Assert.IsTrue(isSatisfy, "In TS_LOGON_INFO_EXTENDED, LogonFields cannot be empty if logInfoExtend.FieldsPresent contains non-zero flag.");
                    foreach (TS_LOGON_INFO_FIELD field in logInfoExtend.LogonFields)
                    {
                        FieldsPresent_Values expectedFlag = FieldsPresent_Values.None;
                        if (field.FieldData.GetType() == typeof(ARC_SC_PRIVATE_PACKET))
                        {
                            Site.Assert.IsTrue(logInfoExtend.FieldsPresent.HasFlag(FieldsPresent_Values.LOGON_EX_AUTORECONNECTCOOKIE), "In TS_LOGON_INFO_EXTENDED, if the LogonFields field contains a ARC_SC_PRIVATE_PACKET, the FieldsPresent field MUST contain LOGON_EX_AUTORECONNECTCOOKIE flag.");
                            expectedFlag |= FieldsPresent_Values.LOGON_EX_AUTORECONNECTCOOKIE;
                            ARC_SC_PRIVATE_PACKET cookiePacket = (ARC_SC_PRIVATE_PACKET)field.FieldData;
                            Site.Assert.AreEqual<cbLen_Values>(cbLen_Values.V1, cookiePacket.cbLen, "In ARC_SC_PRIVATE_PACKET structure, the cbLen field MUST be set to 0x0000001C (28 bytes).");
                            Site.Assert.AreEqual<Version_Values>(Version_Values.AUTO_RECONNECT_VERSION_1, cookiePacket.Version, "In ARC_SC_PRIVATE_PACKET structure, the version field MUST be set to AUTO_RECONNECT_VERSION_1 (0x00000001).");
                        }
                        if (field.FieldData.GetType() == typeof(TS_LOGON_ERRORS_INFO))
                        {
                            Site.Assert.IsTrue(logInfoExtend.FieldsPresent.HasFlag(FieldsPresent_Values.LOGON_EX_LOGONERRORS), "In TS_LOGON_INFO_EXTENDED, if the LogonFields field contains a TS_LOGON_ERRORS_INFO, the FieldsPresent field MUST contain LOGON_EX_LOGONERRORS flag.");
                            expectedFlag |= FieldsPresent_Values.LOGON_EX_LOGONERRORS;
                        }
                        Site.Assert.AreEqual<FieldsPresent_Values>(expectedFlag, logInfoExtend.FieldsPresent, "In TS_LOGON_INFO_EXTENDED, the FieldsPresent field and the LogonFields field are not match, auctual FieldsPresent value is {0}, expected value is {1}.",
                            logInfoExtend.FieldsPresent, expectedFlag);
                    }
                }
            }
            else
            {
                Site.Assert.Fail("In Server_Save_Session_Info_Pdu, infoType filed MUST be one of following: "
                    + "INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), INFOTYPE_LOGON_PLAINNOTIFY (0x00000002), INFOTYPE_LOGON_EXTENDED_INFO (0x00000003).");
            }
        }

        #endregion Verify PDUs

        #region Verify Structures

        #region Structures in Server MCS Connect Response PDU

        /// <summary>
        /// Verify structure: TS_UD_SC_CORE
        /// </summary>
        /// <param name="serverCoreData"></param>
        public void VerifyStructure(TS_UD_SC_CORE serverCoreData)
        {
            if (serverCoreData == null)
            {
                return;
            }

            Site.Assert.IsTrue(serverCoreData.version == TS_UD_SC_CORE_version_Values.V1
                || serverCoreData.version == TS_UD_SC_CORE_version_Values.V2
                || serverCoreData.version == TS_UD_SC_CORE_version_Values.V3
                || serverCoreData.version == TS_UD_SC_CORE_version_Values.V4
                || serverCoreData.version == TS_UD_SC_CORE_version_Values.V5
                || serverCoreData.version == TS_UD_SC_CORE_version_Values.V6
                || serverCoreData.version == TS_UD_SC_CORE_version_Values.V7,
                "The version field of TS_UD_SC_CORE contains value: 0x00080001,0x00080004,0x00080005,0x00080006,0x00080007,0x00080008,0x00080009.");


            uint flags =(uint)(requestedProtocols_Values.PROTOCOL_RDP_FLAG | requestedProtocols_Values.PROTOCOL_SSL_FLAG | requestedProtocols_Values.PROTOCOL_HYBRID_FLAG | requestedProtocols_Values.PROTOCOL_HYBRID_EX);
            uint negFlags = (uint)(~flags);
            Site.Assert.AreEqual<uint>(0, (uint)serverCoreData.clientRequestedProtocols & negFlags, "The clientRequestedProtocols field of TS_UD_SC_CORE, which contains the flags sent by the client in the requestedProtocols field of the RDP Negotiation Request."
                + "Available flags: PROTOCOL_RDP (0x00000000), PROTOCOL_SSL (0x00000001), PROTOCOL_HYBRID (0x00000002), PROTOCOL_HYBRID_EX (0x00000008).");


            flags = (uint)(SC_earlyCapabilityFlags_Values.RNS_UD_SC_DYNAMIC_DST_SUPPORTED | SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED | SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V2);
            negFlags = (uint)(~flags);
            Site.Assert.AreEqual<uint>(0, (uint)serverCoreData.earlyCapabilityFlags & negFlags, "The earlyCapabilityFlags field of TS_UD_SC_CORE contains flags: "
                + "RNS_UD_SC_EDGE_ACTIONS_SUPPORTED (0x00000001), RNS_UD_SC_DYNAMIC_DST_SUPPORTED (0x00000002).");
            
        }

        /// <summary>
        /// Verify structure: TS_UD_SC_NET
        /// </summary>
        /// <param name="serverNetworkData"></param>
        public void VerifyStructure(TS_UD_SC_NET serverNetworkData)
        {
            if (serverNetworkData == null)
            {
                return;
            }

            if (serverNetworkData.channelIdArray != null)
            {
                Site.Assert.AreEqual<int>(serverNetworkData.channelCount, serverNetworkData.channelIdArray.Length, "In TS_UD_SC_NET structure, the channelCount field specifies number of MCS channel IDs in the channelIdArray field.");
            }
        }

        /// <summary>
        /// Veirfy structure: TS_UD_SC_SEC1
        /// </summary>
        /// <param name="serverSecurityData"></param>
        public void VerifyStructure(TS_UD_SC_SEC1 serverSecurityData)
        {
            if (serverSecurityData == null)
            {
                return;
            }

            Site.Assert.IsTrue(serverSecurityData.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE
                || serverSecurityData.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT
                || serverSecurityData.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT
                || serverSecurityData.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT
                || serverSecurityData.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS, "In TS_UD_SC_SEC1 structure, the vale of encryptionMethod field is defined as following: "
                + "ENCRYPTION_METHOD_NONE (0x00000000), ENCRYPTION_METHOD_40BIT (0x00000001), ENCRYPTION_METHOD_128BIT (0x00000002), ENCRYPTION_METHOD_56BIT (0x00000008), ENCRYPTION_METHOD_FIPS (0x00000010).");

            Site.Assert.IsTrue(serverSecurityData.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE
                || serverSecurityData.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_LOW
                || serverSecurityData.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_CLIENT_COMPATIBLE
                || serverSecurityData.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_HIGH
                || serverSecurityData.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS, "In TS_UD_SC_SEC1 structure, the vale of encryptionLevel field is defined as following: "
                + "ENCRYPTION_LEVEL_NONE (0x00000000), ENCRYPTION_LEVEL_LOW (0x00000001), ENCRYPTION_LEVEL_CLIENT_COMPATIBLE (0x00000002), ENCRYPTION_LEVEL_HIGH (0x00000003), ENCRYPTION_LEVEL_FIPS (0x00000004).");

            if (serverSecurityData.encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE
                && serverSecurityData.encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                Site.Assert.IsNull(serverSecurityData.serverRandomLen, "In TS_UD_SC_SEC1 structure, if the encryptionMethod and encryptionLevel fields are both set to zero, then the serverRandomLen field MUST NOT be present.");

                Site.Assert.IsNull(serverSecurityData.serverCertLen, "In TS_UD_SC_SEC1 structure, if the encryptionMethod and encryptionLevel fields are both set to zero, then the serverCertLen field MUST NOT be present.");

                Site.Assert.IsNull(serverSecurityData.serverRandom, "In TS_UD_SC_SEC1 structure, if the encryptionMethod and encryptionLevel fields are both set to zero, then the serverRandom field MUST NOT be present.");

                Site.Assert.IsNull(serverSecurityData.serverCertificate, "In TS_UD_SC_SEC1 structure, if the encryptionMethod and encryptionLevel fields are both set to zero, then the serverCertificate field MUST NOT be present.");
            }
            else
            {
                Site.Assert.IsNotNull(serverSecurityData.serverRandomLen, "In TS_UD_SC_SEC1 structure, if either the encryptionMethod or encryptionLevel field is non-zero, the serverRandomLen field MUST be present.");

                Site.Assert.AreEqual<uint>(0x00000020, serverSecurityData.serverRandomLen.actualData, "In TS_UD_SC_SEC1 structure, if either the encryptionMethod or encryptionLevel field is non-zero, the serverRandomLen field MUST be set to 0x00000020.");
            }
        }

        /// <summary>
        /// Verify structure: TS_UD_SC_MCS_MSGCHANNEL
        /// </summary>
        /// <param name="serverMessageChannelData"></param>
        public void VerifyStructure(TS_UD_SC_MCS_MSGCHANNEL serverMessageChannelData)
        {
            // Nothing to verify
        }

        /// <summary>
        /// Verify structure: TS_UD_SC_MULTITRANSPORT
        /// </summary>
        /// <param name="serverMultitransportChannelData"></param>
        public void VerifyStructure(TS_UD_SC_MULTITRANSPORT serverMultitransportChannelData)
        {
            uint flags = (uint)(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR | MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL | MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDP_PREFERRED);
            uint negFlags = (uint)(~flags);
            Site.Assert.AreEqual<uint>(0, (uint)((uint)serverMultitransportChannelData.flags & negFlags), "In TS_UD_SC_MULTITRANSPORT structure, the flags field contains flags as following: "
                + "TRANSPORTTYPE_UDPFECR (0x01), TRANSPORTTYPE_UDPFECL (0x04), TRANSPORTTYPE_UDP_PREFERRED (0x100).");

        }

        #endregion Structures in Server MCS Connect Response PDU

        #region Structures in Server Auto-Detect Request PDU

        /// <summary>
        /// Verify structure: NETWORK_DETECTION_REQUEST
        /// </summary>
        /// <param name="detectReqest"></param>
        public void VerifyStructure(NETWORK_DETECTION_REQUEST detectReqest)
        {
            if (detectReqest is RDP_RTT_REQUEST)
            {
                VerifyStructure((RDP_RTT_REQUEST)detectReqest);
            }
            else if (detectReqest is RDP_BW_START)
            {
                VerifyStructure((RDP_BW_START)detectReqest);
            }
            else if (detectReqest is RDP_BW_PAYLOAD)
            {
                VerifyStructure((RDP_BW_PAYLOAD)detectReqest);
            }
            else if (detectReqest is RDP_BW_STOP)
            {
                VerifyStructure((RDP_BW_STOP)detectReqest);
            }
            else if (detectReqest is RDP_NETCHAR_RESULT)
            {
                VerifyStructure((RDP_NETCHAR_RESULT)detectReqest);
            }
        }

        /// <summary>
        /// Verify structure: RDP_RTT_REQUEST
        /// </summary>
        /// <param name="rttRequest"></param>
        public void VerifyStructure(RDP_RTT_REQUEST rttRequest)
        {
            if (rttRequest == null)
            {
                return;
            }

            Site.Assert.AreEqual<byte>(0x06, rttRequest.headerLength, "In RDP_RTT_REQUEST, the headerLength field MUST be set to 0x06.");
            Site.Assert.AreEqual<HeaderTypeId_Values>(HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST, rttRequest.headerTypeId, "In RDP_RTT_REQUEST, the headerTypeId field MUST be to TYPE_ID_AUTODETECT_REQUEST (0x00).");
            Site.Assert.IsTrue(rttRequest.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_IN_CONNECTTIME
                || rttRequest.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_AFTER_CONNECTTIME, "In RDP_RTT_REQUEST, the requestType field MUST be one of the following:\n"
                + "0x0001: The RTT Measure Request message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU sent after the RDP Connection Sequence has completed.\n"
                + "0x1001: The RTT Measure Request message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU sent during the Optional Connect-Time Auto-Detection phase of the RDP Connection Sequence.");
        }

        /// <summary>
        /// Verify structure: RDP_BW_START
        /// </summary>
        /// <param name="bwStart"></param>
        public void VerifyStructure(RDP_BW_START bwStart)
        {
            if (bwStart == null)
            {
                return;
            }

            Site.Assert.AreEqual<byte>(0x06, bwStart.headerLength, "In RDP_BW_START, the headerLength field MUST be set to 0x06.");
            Site.Assert.AreEqual<HeaderTypeId_Values>(HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST, bwStart.headerTypeId, "In RDP_BW_START, the headerTypeId field MUST be to TYPE_ID_AUTODETECT_REQUEST (0x00).");
            Site.Assert.IsTrue(bwStart.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_LOSSYUDP
                || bwStart.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP
                || bwStart.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_IN_CONNECTTIME, "In RDP_BW_START, the requestType field MUST be one of the following:\n"
                + "0x0014: One of two possible meanings: \n"
                + "\tThe Bandwidth Measure Start message is encapsulated in the SubHeaderData field of an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being tunneled over a reliable UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).\n"
                + "\tThe Bandwidth Measure Start message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU (section 2.2.14.3) sent after the RDP Connection Sequence (section 1.3.1.1) has completed.\n"
                + "0x0114: The Bandwidth Measure Start message is encapsulated in the SubHeaderData field of an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being tunneled over a lossy UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).\n"
                + "0x1014: The Bandwidth Measure Start message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU sent during the Optional Connect-Time Auto-Detection phase of the RDP Connection Sequence.\n");
        }

        /// <summary>
        /// Verify structure: RDP_BW_PAYLOAD
        /// </summary>
        /// <param name="bwPayload"></param>
        public void VerifyStructure(RDP_BW_PAYLOAD bwPayload)
        {
            if (bwPayload == null)
            {
                return;
            }

            Site.Assert.AreEqual<byte>(0x08, bwPayload.headerLength, "In RDP_BW_PAYLOAD, the headerLength field MUST be set to 0x06.");
            Site.Assert.AreEqual<HeaderTypeId_Values>(HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST, bwPayload.headerTypeId, "In RDP_BW_PAYLOAD, the headerTypeId field MUST be to TYPE_ID_AUTODETECT_REQUEST (0x00).");
            Site.Assert.IsTrue(bwPayload.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_PAYLOAD, "In RDP_BW_PAYLOAD, the requestType field MUST be 0x0002.");
            
        }

        /// <summary>
        /// Verify structure: RDP_BW_STOP
        /// </summary>
        /// <param name="bwStop"></param>
        public void VerifyStructure(RDP_BW_STOP bwStop)
        {
            if (bwStop == null)
            {
                return;
            }

            Site.Assert.AreEqual<HeaderTypeId_Values>(HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST, bwStop.headerTypeId, "In RDP_BW_STOP, the headerTypeId field MUST be to TYPE_ID_AUTODETECT_REQUEST (0x00).");
            Site.Assert.IsTrue(bwStop.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_LOSSYUDP
                || bwStop.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP
                || bwStop.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_IN_CONNECTTIME, "In RDP_BW_STOP, the requestType field MUST be one of the following:\n"
                + "0x0429: One of two possible meanings: \n"
                + "\tThe Bandwidth Measure Stop message is encapsulated in the SubHeaderData field of an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being tunneled over a reliable UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).\n"
                + "\tThe Bandwidth Measure Stop message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU sent after the RDP Connection Sequence has completed.\n"
                + "0x002B: The Bandwidth Measure Stop message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU (section 2.2.14.3) sent during the Optional Connect-Time Auto-Detection phase of the RDP Connection Sequence (section 1.3.1.1). The payloadLength field is present and has a value greater than zero.\n"
                + "0x0629: The Bandwidth Measure Stop message is encapsulated in the SubHeaderData field of an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being tunneled over a lossy UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).\n");
            if (bwStop.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_IN_CONNECTTIME)
            {
                Site.Assert.AreEqual<byte>(0x08, bwStop.headerLength, "In RDP_BW_STOP, the headerLength field MUST be set to 0x08 if the requestType field is set to 0x002B.");
            }
            else
            {
                Site.Assert.AreEqual<byte>(0x06, bwStop.headerLength, "In RDP_BW_STOP, the headerLength field MUST be set to 0x06 if the requestType field is not set to 0x002B.");            
            }
        }

        /// <summary>
        /// Verify structure : RDP_NETCHAR_RESULT
        /// </summary>
        /// <param name="netcharResult"></param>
        public void VerifyStructure(RDP_NETCHAR_RESULT netcharResult)
        {
            if (netcharResult == null)
            {
                return;
            }

            Site.Assert.AreEqual<HeaderTypeId_Values>(HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST, netcharResult.headerTypeId, "In RDP_NETCHAR_RESULT, the headerTypeId field MUST be to TYPE_ID_AUTODETECT_REQUEST (0x00).");
            Site.Assert.IsTrue(netcharResult.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT
                || netcharResult.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT
                || netcharResult.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT, "In RDP_NETCHAR_RESULT, the requestType field MUST be one of the following:\n"
                + "0x0840: The baseRTT and averageRTT fields are present in the Network Characteristics Result message (the bandwidth field is not present).\n"
                + "0x0880: The bandwidth and averageRTT fields are present in the Network Characteristics Result message (the baseRTT field is not present).\n"
                + "0x08C0: The baseRTT, bandwidth and averageRTT fields are present in the Network Characteristics Result message.\n");
            if (netcharResult.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT)
            {
                Site.Assert.AreEqual<byte>(0x12, netcharResult.headerLength, "In RDP_NETCHAR_RESULT, the headerLength field MUST be set to 0x12 if the requestType field is set to 0x08C0.");
            }
            else
            {
                Site.Assert.AreEqual<byte>(0x0E, netcharResult.headerLength, "In RDP_NETCHAR_RESULT, the headerLength field MUST be set to 0x0E if the requestType field is not set to 0x08C0.");
            }
        }

        #endregion Structures in Server Auto-Detect Request PDU

        #region Verify slow-update update structures

        /// <summary>
        /// Verify structure: RdpbcgrSlowPathUpdatePdu
        /// </summary>
        /// <param name="spUpdate"></param>
        public void VerifyStructure(RdpbcgrSlowPathUpdatePdu spUpdate)
        {
            if (spUpdate == null)
            {
                return;
            }

            if (spUpdate is TS_UPDATE_PALETTE)
            {
                VerifyStructure(spUpdate as TS_UPDATE_PALETTE);
            }
            else if (spUpdate is TS_UPDATE_BITMAP)
            {
                VerifyStructure(spUpdate as TS_UPDATE_BITMAP);
            }
            else if (spUpdate is TS_UPDATE_SYNC)
            {
                VerifyStructure(spUpdate as TS_UPDATE_SYNC);
            }
            else if (spUpdate is TS_POINTER_PDU)
            {
                VerifyStructure(spUpdate as TS_POINTER_PDU);
            }
        }

        /// <summary>
        /// Verify structure: TS_UPDATE_PALETTE
        /// </summary>
        /// <param name="update"></param>
        public void VerifyStructure(TS_UPDATE_PALETTE update)
        {
            if (update == null)
            {
                return;
            }

            Site.Assert.AreEqual<updateType_Values>(updateType_Values.UPDATETYPE_PALETTE, update.paletteData.updateType, "In TS_UPDATE_PALETTE, the updateType field of the paletteData field MUST be set to UPDATETYPE_PALETTE (0x0002).");
            Site.Assert.AreEqual<uint>(256, update.paletteData.numberColors, "In TS_UPDATE_PALETTE, the numberColors field of the paletteData field MUST be set to 256.");
        }

        /// <summary>
        /// Verify structure: TS_UPDATE_BITMAP
        /// </summary>
        /// <param name="update"></param>
        public void VerifyStructure(TS_UPDATE_BITMAP update)
        {
            if (update == null)
            {
                return;
            }

            Site.Assert.AreEqual<ushort>((ushort)updateType_Values.UPDATETYPE_BITMAP, update.bitmapData.updateType, "In TS_UPDATE_BITMAP, the updateType field of the bitmapData field MUST be set to UPDATETYPE_BITMAP (0x0001).");
            if (update.bitmapData.rectangles != null)
            {
                Site.Assert.AreEqual<int>(update.bitmapData.numberRectangles, update.bitmapData.rectangles.Length, "In bitmapData field of TS_UPDATE_BITMAP, the number of rectangle bitmap data structures in rectangles field is specified by the numberRectangles field.");
                ushort flags = (ushort)(TS_BITMAP_DATA_Flags_Values.BITMAP_COMPRESSION | TS_BITMAP_DATA_Flags_Values.NO_BITMAP_COMPRESSION_HDR);
                ushort negFlags = (ushort)(~flags);
                foreach (TS_BITMAP_DATA data in update.bitmapData.rectangles)
                {
                    bool isSatisfy = (negFlags & (ushort)data.Flags) == 0;
                    Site.Assert.IsTrue(isSatisfy, "In TS_BITMAP_DATA of TS_UPDATE_BITMAP, The flags field describing the format of the bitmap data in the bitmapDataStream field: "
                        + "BITMAP_COMPRESSION (0x0001), NO_BITMAP_COMPRESSION_HDR (0x0400).");
                }
            }
        }

        /// <summary>
        /// Verify structure: TS_UPDATE_SYNC
        /// </summary>
        /// <param name="update"></param>
        public void VerifyStructure(TS_UPDATE_SYNC update)
        {
            if (update == null)
            {
                return;
            }

            Site.Assert.AreEqual<ushort>((ushort)updateType_Values.UPDATETYPE_SYNCHRONIZE, update.updateType, "In TS_UPDATE_SYNC, the updateType field MUST be set to UPDATETYPE_SYNCHRONIZE (0x0003).");            
        }

        /// <summary>
        /// Verify structure: TS_POINTER_PDU
        /// </summary>
        /// <param name="pointPdu"></param>
        public void VerifyStructure(TS_POINTER_PDU pointPdu)
        {
            if (pointPdu == null)
            {
                return;
            }

            if (pointPdu.messageType == (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_SYSTEM)
            {
                Site.Assert.IsTrue(pointPdu.pointerAttributeData is TS_SYSTEMPOINTERATTRIBUTE, "In TS_POINTER_PDU, pointerAttributeData field is a TS_SYSTEMPOINTERATTRIBUTE when messageType field is TS_PTRMSGTYPE_SYSTEM.");
                Site.Assert.IsTrue(((TS_SYSTEMPOINTERATTRIBUTE)pointPdu.pointerAttributeData).systemPointerType == (uint)systemPointerType_Values.SYSPTR_NULL
                    || ((TS_SYSTEMPOINTERATTRIBUTE)pointPdu.pointerAttributeData).systemPointerType == (uint)systemPointerType_Values.SYSPTR_DEFAULT, "In TS_SYSTEMPOINTERATTRIBUTE of TS_POINTER_PDU, the value of systemPointerType field is as following: "
                    + "SYSPTR_NULL (0x00000000), SYSPTR_DEFAULT (0x00007F00).");
            }
            else if (pointPdu.messageType == (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_POSITION)
            {
                Site.Assert.IsTrue(pointPdu.pointerAttributeData is TS_POINTERPOSATTRIBUTE, "In TS_POINTER_PDU, pointerAttributeData field is a TS_POINTERPOSATTRIBUTE when messageType field is TS_PTRMSGTYPE_POSITION.");
            }
            else if (pointPdu.messageType == (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_COLOR)
            {
                Site.Assert.IsTrue(pointPdu.pointerAttributeData is TS_COLORPOINTERATTRIBUTE, "In TS_POINTER_PDU, pointerAttributeData field is a TS_COLORPOINTERATTRIBUTE when messageType field is TS_PTRMSGTYPE_COLOR.");
            }
            else if (pointPdu.messageType == (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_CACHED)
            {
                Site.Assert.IsTrue(pointPdu.pointerAttributeData is TS_CACHEDPOINTERATTRIBUTE, "In TS_POINTER_PDU, pointerAttributeData field is a TS_CACHEDPOINTERATTRIBUTE when messageType field is TS_PTRMSGTYPE_CACHED.");
            }
            else if (pointPdu.messageType == (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_POINTER)
            {
                Site.Assert.IsTrue(pointPdu.pointerAttributeData is TS_POINTERATTRIBUTE, "In TS_POINTER_PDU, pointerAttributeData field is a TS_POINTERATTRIBUTE when messageType field is TS_PTRMSGTYPE_POINTER.");
            }
            else
            {
                Site.Assert.Fail("In TS_POINTER_PDU, the value of messageType field are defined as following: "
                    + "TS_PTRMSGTYPE_SYSTEM (0x0001), TS_PTRMSGTYPE_POSITION (0x0003), TS_PTRMSGTYPE_COLOR (0x0006), TS_PTRMSGTYPE_CACHED (0x0007), TS_PTRMSGTYPE_POINTER (0x0008).");
            }

        }

        #endregion Verify slow-update update structures

        #region Verify fast-path update structures

        /// <summary>
        /// Verify structure: TS_FP_UPDATE
        /// </summary>
        /// <param name="update"></param>
        public void VerifyStructure(TS_FP_UPDATE update)
        {
            if (update == null)
            {
                return;
            }

            byte flag = (byte)(compressedType_Values.CompressionTypeMask | compressedType_Values.PACKET_AT_FRONT
                | compressedType_Values.PACKET_COMPRESSED | compressedType_Values.PACKET_FLUSHED);
            byte negFlag = (byte)(~flag);
            bool isSatisfy =((byte)update.compressionFlags & negFlag) == 0;
            Site.Assert.IsTrue(isSatisfy, "In TS_FP_UPDATE, the compressionFlags field contains: "
                + "CompressionTypeMask (0x0F), PACKET_COMPRESSED (0x20), PACKET_AT_FRONT (0x40), PACKET_FLUSHED (0x80).");

            byte compressType = (byte)((byte)update.compressionFlags & 0x0F);
            Site.Assert.IsTrue(compressType == (byte)CompressionType.PACKET_COMPR_TYPE_8K
                || compressType == (byte)CompressionType.PACKET_COMPR_TYPE_64K
                || compressType == (byte)CompressionType.PACKET_COMPR_TYPE_RDP6
                || compressType == (byte)CompressionType.PACKET_COMPR_TYPE_RDP61, "In TS_FP_UPDATE, the compress type values in compressionFlags are: "
                + "PACKET_COMPR_TYPE_8K (0x0), PACKET_COMPR_TYPE_64K (0x1), PACKET_COMPR_TYPE_RDP6 (0x2), PACKET_COMPR_TYPE_RDP61 (0x3).");

            if (update is TS_FP_UPDATE_PALETTE)
            {
                VerifyStructure(update as TS_FP_UPDATE_PALETTE);
            }
            else if (update is TS_FP_UPDATE_BITMAP)
            {
                VerifyStructure(update as TS_FP_UPDATE_BITMAP);
            }
            else if (update is TS_FP_UPDATE_SYNCHRONIZE)
            {
                VerifyStructure(update as TS_FP_UPDATE_SYNCHRONIZE);
            }
            else if (update is TS_FP_POINTERPOSATTRIBUTE)
            {
                VerifyStructure(update as TS_FP_POINTERPOSATTRIBUTE);
            }
            else if (update is TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE)
            {
                VerifyStructure(update as TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE);
            }
            else if (update is TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE)
            {
                VerifyStructure(update as TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE);
            }
            else if (update is TS_FP_COLORPOINTERATTRIBUTE)
            {
                VerifyStructure(update as TS_FP_COLORPOINTERATTRIBUTE);
            }
            else if (update is TS_FP_POINTERATTRIBUTE)
            {
                VerifyStructure(update as TS_FP_POINTERATTRIBUTE);
            }
            else if (update is TS_FP_CACHEDPOINTERATTRIBUTE)
            {
                VerifyStructure(update as TS_FP_CACHEDPOINTERATTRIBUTE);
            }
            else if (update is TS_FP_SURFCMDS)
            {
                VerifyStructure(update as TS_FP_SURFCMDS);
            }
        }

        /// <summary>
        /// Verify structure: TS_FP_UPDATE_PALETTE
        /// </summary>
        /// <param name="update"></param>
        public void VerifyStructure(TS_FP_UPDATE_PALETTE update)
        {
            if (update == null)
            {
                return;
            }

            Site.Assert.AreEqual<updateType_Values>(updateType_Values.UPDATETYPE_PALETTE, update.paletteUpdateData.updateType, "In TS_FP_UPDATE_PALETTE, the updateType field of the paletteUpdateData field MUST be set to UPDATETYPE_PALETTE (0x0002).");
            Site.Assert.AreEqual<uint>(256, update.paletteUpdateData.numberColors, "In TS_FP_UPDATE_PALETTE, the numberColors field of the paletteUpdateData field MUST be set to 256.");
        }

        /// <summary>
        /// Verify structure: TS_FP_UPDATE_BITMAP
        /// </summary>
        /// <param name="update"></param>
        public void VerifyStructure(TS_FP_UPDATE_BITMAP update)
        {
            if (update == null)
            {
                return;
            }

            Site.Assert.AreEqual<ushort>((ushort)updateType_Values.UPDATETYPE_BITMAP, update.bitmapUpdateData.updateType, "In TS_FP_UPDATE_BITMAP, the updateType field of the bitmapUpdateData field MUST be set to UPDATETYPE_BITMAP (0x0001).");
            if (update.bitmapUpdateData.rectangles != null)
            {
                Site.Assert.AreEqual<int>(update.bitmapUpdateData.numberRectangles, update.bitmapUpdateData.rectangles.Length, "In bitmapUpdateData field of TS_FP_UPDATE_BITMAP, the number of rectangle bitmap data structures in rectangles field is specified by the numberRectangles field.");
                ushort flags = (ushort)(TS_BITMAP_DATA_Flags_Values.BITMAP_COMPRESSION | TS_BITMAP_DATA_Flags_Values.NO_BITMAP_COMPRESSION_HDR);
                ushort negFlags = (ushort)(~flags);
                foreach (TS_BITMAP_DATA data in update.bitmapUpdateData.rectangles)
                {
                    bool isSatisfy = (negFlags & (ushort)data.Flags) == 0;
                    Site.Assert.IsTrue(isSatisfy, "In TS_BITMAP_DATA of TS_FP_UPDATE_BITMAP, The flags field describing the format of the bitmap data in the bitmapDataStream field: "
                        + "BITMAP_COMPRESSION (0x0001), NO_BITMAP_COMPRESSION_HDR (0x0400).");
                }
            }
        }

        /// <summary>
        /// Verify structure: TS_FP_UPDATE_SYNCHRONIZE
        /// </summary>
        /// <param name="update"></param>
        public void VerifyStructure(TS_FP_UPDATE_SYNCHRONIZE update)
        {
            // Nothing to verify
        }

        /// <summary>
        /// Verify structure: TS_FP_POINTERPOSATTRIBUTE
        /// </summary>
        /// <param name="update"></param>
        public void VerifyStructure(TS_FP_POINTERPOSATTRIBUTE update)
        {
            // Nothing to verify
        }

        public void VerifyStructure(TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE update)
        {
            // Nothing to verify
        }

        public void VerifyStructure(TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE update)
        {
            // Nothing to verify
        }

        public void VerifyStructure(TS_FP_COLORPOINTERATTRIBUTE update)
        {
            // Nothing to verify
        }

        public void VerifyStructure(TS_FP_POINTERATTRIBUTE update)
        {
            // Nothing to verify
        }

        public void VerifyStructure(TS_FP_CACHEDPOINTERATTRIBUTE update)
        {
            // Nothing to verify
        }

        /// <summary>
        /// Verify structure: TS_FP_SURFCMDS
        /// </summary>
        /// <param name="update"></param>
        public void VerifyStructure(TS_FP_SURFCMDS update)
        {
            if (update == null)
            {
                return;
            }

            if (update.surfaceCommands != null)
            {
                foreach (TS_SURFCMD surfcmd in update.surfaceCommands)
                {
                    VerifyStructure(surfcmd);
                }
            }
        }

        /// <summary>
        /// Verify Structure: TS_SURFCMD
        /// </summary>
        /// <param name="surfcmd"></param>
        public void VerifyStructure(TS_SURFCMD surfcmd)
        {
            if (surfcmd.cmdType == cmdType_Values.CMDTYPE_SET_SURFACE_BITS)
            {
                Site.Assert.IsTrue(surfcmd is TS_SURFCMD_SET_SURF_BITS, "In TS_FP_SURFCMDS, the TS_SURFCMD is a TS_SURFCMD_SET_SURF_BITS when cmdType is CMDTYPE_SET_SURFACE_BITS (0x0001).");
            }
            else if (surfcmd.cmdType == cmdType_Values.CMDTYPE_FRAME_MARKER)
            {
                Site.Assert.IsTrue(surfcmd is TS_FRAME_MARKER, "In TS_FP_SURFCMDS, the TS_SURFCMD is a TS_FRAME_MARKER when cmdType is CMDTYPE_FRAME_MARKER (0x0004).");
                Site.Assert.IsTrue((surfcmd as TS_FRAME_MARKER).frameAction == frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN
                    || (surfcmd as TS_FRAME_MARKER).frameAction == frameAction_Values.SURFACECMD_FRAMEACTION_END, "In CMDTYPE_FRAME_MARKER, the value of frameAction field are defined as following: "
                    + "SURFACECMD_FRAMEACTION_BEGIN (0x0000), SURFACECMD_FRAMEACTION_END (0x0001).");
            }
            else if (surfcmd.cmdType == cmdType_Values.CMDTYPE_STREAM_SURFACE_BITS)
            {
                Site.Assert.IsTrue(surfcmd is TS_SURFCMD_STREAM_SURF_BITS, "In TS_FP_SURFCMDS, the TS_SURFCMD is a TS_SURFCMD_STREAM_SURF_BITS when cmdType is CMDTYPE_STREAM_SURFACE_BITS (0x0006).");
            }
            else
            {
                Site.Assert.Fail("In TS_FP_SURFCMDS, the values of TS_SURFCMD field are defined as following: "
                    + "CMDTYPE_SET_SURFACE_BITS (0x0001), CMDTYPE_FRAME_MARKER (0x0004), CMDTYPE_STREAM_SURFACE_BITS (0x0006).");
            }
        }

        #endregion Verify fast-path update structures

        /// <summary>
        /// Verify structure: RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        /// <param name="redirectionPacket"></param>
        public void VerifyStructure(RDP_SERVER_REDIRECTION_PACKET redirectionPacket)
        {
            Site.Assert.AreEqual<RDP_SERVER_REDIRECTION_PACKET_FlagsEnum>(RDP_SERVER_REDIRECTION_PACKET_FlagsEnum.SEC_REDIRECTION_PKT, redirectionPacket.Flags, "In Server Redirection Packet , the Flags field MUST be set to SEC_REDIRECTION_PKT (0x0400).");

            uint flags = (uint)(RedirectionFlags.LB_TARGET_NET_ADDRESS | RedirectionFlags.LB_LOAD_BALANCE_INFO | RedirectionFlags.LB_USERNAME
                | RedirectionFlags.LB_DOMAIN | RedirectionFlags.LB_PASSWORD | RedirectionFlags.LB_DONTSTOREUSERNAME | RedirectionFlags.LB_SMARTCARD_LOGON
                | RedirectionFlags.LB_NOREDIRECT | RedirectionFlags.LB_TARGET_FQDN | RedirectionFlags.LB_TARGET_NETBIOS_NAME | RedirectionFlags.LB_TARGET_NET_ADDRESSES
                | RedirectionFlags.LB_CLIENT_TSV_URL | RedirectionFlags.LB_SERVER_TSV_CAPABLE);
            uint negFlags = ~flags;
            bool isSatisfy = ((uint)redirectionPacket.RedirFlags & negFlags) == 0;
            Site.Assert.IsTrue(isSatisfy, "In Server Redirection Packet , the RedirFlags field contains redirection information flags: "
                + "LB_TARGET_NET_ADDRESS (0x00000001), LB_LOAD_BALANCE_INFO (0x00000002), LB_USERNAME (0x00000004), LB_DOMAIN (0x00000008), LB_PASSWORD (0x00000010), LB_DONTSTOREUSERNAME (0x00000020), LB_SMARTCARD_LOGON (0x00000040), LB_NOREDIRECT (0x00000080), LB_TARGET_FQDN (0x00000100), LB_TARGET_NETBIOS_NAME (0x00000200), LB_TARGET_NET_ADDRESSES (0x00000800), LB_CLIENT_TSV_URL (0x00001000), LB_SERVER_TSV_CAPABLE (0x00002000).");

        }

        #endregion Verify Structures
    }
}