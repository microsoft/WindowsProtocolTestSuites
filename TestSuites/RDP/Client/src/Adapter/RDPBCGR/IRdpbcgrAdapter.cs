// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    #region delegate
    public delegate void X224ConnectioRequestHandler(Client_X_224_Connection_Request_Pdu x224Request);

    public delegate void McsConnectRequestHandler(Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request mcsConnectRequest);

    public delegate void McsErectDomainRequestHandler(Client_MCS_Erect_Domain_Request erectDomainRequest);

    public delegate void McsAttachUserRequestHandler(Client_MCS_Attach_User_Request attachUserRequest);

    public delegate void McsChannelJoinRequestHandler(Client_MCS_Channel_Join_Request channelJoinRequest);

    public delegate void SecurityExchangeRequestHandler(Client_Security_Exchange_Pdu securityExchangeRequest);

    public delegate void ClientInfoRequestHandler(Client_Info_Pdu clientInfoPdu);

    public delegate void ConfirmActiveRequestHandler(Client_Confirm_Active_Pdu confirmActivePdu);

    public delegate void ClientSyncRequestHandler(Client_Synchronize_Pdu syncPdu);

    public delegate void ControlCooperateRequestHandler(Client_Control_Pdu_Cooperate cooperatePdu);

    public delegate void ControlRequestControlRequestHandler(Client_Control_Pdu_Request_Control requestControlPdu);

    public delegate void PersistentKeyListRequestHandler(Client_Persistent_Key_List_Pdu keyListPdu);

    public delegate void FontListRequestHandler(Client_Font_List_Pdu fontListPdu);

    public delegate void ShutdownRequestHandler(Client_Shutdown_Request_Pdu shutdownPdu);

    public delegate void McsDisconnectUltimatumRequestHandler(MCS_Disconnect_Provider_Ultimatum_Pdu disconnectPdu);

    public delegate void InputRequestHandler(TS_INPUT_PDU inputPdu);

    public delegate void FastpathInputRequestHandler(TS_FP_INPUT_PDU fpInputPdu);

    public delegate void RefreshRectRequestHandler(Client_Refresh_Rect_Pdu refreshPdu);

    public delegate void SuppressOutputRequestHandler(Client_Suppress_Output_Pdu suppressPdu);

    public delegate void VirtualChannelRequestHandler(Virtual_Channel_RAW_Pdu vcPdu);

    public delegate void TS_FRAME_ACKNOWLEDGE_PDUHandler(TS_FRAME_ACKNOWLEDGE_PDU ackPdu);

    public delegate void RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsHandler(RDSTLS_AuthenticationRequestPDUwithPasswordCredentials pdu);

    public delegate void RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieHandler(RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie pdu);
    #endregion

    public interface IRdpbcgrAdapter : IAdapter
    {
        #region Events
        event X224ConnectioRequestHandler X224ConnectionRequest;
        event McsConnectRequestHandler McsConnectRequest;
        event McsErectDomainRequestHandler McsErectDomainRequest;
        event McsAttachUserRequestHandler McsAttachUserRequest;
        event McsChannelJoinRequestHandler McsChannelJoinRequest;
        event SecurityExchangeRequestHandler SecurityExchangeRequest;
        event ClientInfoRequestHandler ClientInfoRequest;
        event ConfirmActiveRequestHandler ConfirmActiveRequest;
        event ClientSyncRequestHandler ClientSyncRequest;
        event ControlCooperateRequestHandler ControlCooperateRequest;
        event ControlRequestControlRequestHandler ControlRequestControlRequest;
        event PersistentKeyListRequestHandler PersistentKeyListRequest;
        event FontListRequestHandler FontListRequest;
        event ShutdownRequestHandler ShutdownRequest;
        event McsDisconnectUltimatumRequestHandler McsDisconnectUltimatumClientRequest;
        event InputRequestHandler InputRequest;
        event FastpathInputRequestHandler FastpathInputRequest;
        event RefreshRectRequestHandler RefreshRectRequest;
        event SuppressOutputRequestHandler SuppressOutputRequest;
        event VirtualChannelRequestHandler VirtualChannelRequest;
        event TS_FRAME_ACKNOWLEDGE_PDUHandler TS_FRAME_ACKNOWLEDGE_PDUReceived;
        event RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsHandler RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsReceived;
        event RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieHandler RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieReceived;
        #endregion

        #region Properties
        RdpbcgrServerSessionContext SessionContext { get; }
        RdpbcgrServer ServerStack { get; }
        ServerCapabilitySetting CapabilitySetting { get; }
        SimulatedScreen SimulatedScreen { get; }
        UInt16 RDPDRChannelId { get; }
        #endregion

        #region Methods

        #region Connection Methods

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
        void SetServerCapability(
            bool supportServerBitmapCacheHost, //Bitmap Cache Host Support Capability Set
            bool supportFastPathInput, bool supportScancode, bool supportExtendedMouse, bool supportUnicodeKeyboard, //Input Capability Set
            bool supportAutoReconnect, bool supportRefreshRect, bool supportSuppressOoutput, //General Capbility Set
            bool supportVCComression, bool presentVCChunksize //Virtual Channel Capability Set
            );

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
        void SetServerCapability(
            bool supportServerBitmapCacheHost, //Bitmap Cache Host Support Capability Set
            bool supportFastPathInput, bool supportScancode, bool supportExtendedMouse, bool supportUnicodeKeyboard, //Input Capability Set
            bool supportAutoReconnect, bool supportRefreshRect, bool supportSuppressOoutput, //General Capbility Set
            bool supportVCComression, bool presentVCChunksize, //Virtual Channel Capability Set
            bool supportMultifragmentUpdateCapabilitySet, uint maxRequestSize, //Multifragment Update Capability Set
            bool supportLargePointerCapabilitySet, //Large Pointer Capability Set
            bool supportFrameAcknowledgeCapabilitySet, uint maxUnacknowledgedFrameCount,//TS_FRAME_ACKNOWLEDGE_CAPABILITYSET
            bool supportSurfaceCommandsCapabilitySet, CmdFlags_Values cmdFlag,  //Surface Commands Capability Set 
            bool supportBitmapCodecsCapabilitySet, bool presentNSCodec, bool presentRemoteFXCodec //Bitmap Codecs Capability Set
            );

        /// <summary>
        /// Generate Caps Sets according to capability setting from SetServerCapability
        /// </summary>
        /// <returns></returns>
        Collection<ITsCapsSet> GenerateServerCapSet();

        /// <summary>
        /// Start RDP listening.
        /// </summary>
        void StartRDPListening(EncryptedProtocol protocol);

        /// <summary>
        /// Stop RDP listening.
        /// </summary>
        void StopRDPListening();

        /// <summary>
        /// Expect a transport level (TCP) connection
        /// </summary>
        /// <param name="sessionType">The type of session to be established.</param>
        void ExpectTransportConnection(RDPSessionType sessionType);

        /// <summary>
        /// Send Server X_224 Connection Confirm Pdu to client
        /// </summary>
        /// <param name="protocol">Indicates the selected security protocol</param>
        /// <param name="bSupportExtClientData">Indicates Extended Client Data is supported</param>
        /// <param name="bSetRdpNegData">Indicates RdpNegData field is set</param>
        /// <param name="invalidType">Indicates the type of invalid field</param>
        /// <param name="bSupportEGFX">Indicates the server supports MS-RDPEGFX</param>
        /// <param name="bSupportRestrictedAdminMode">Indicates the server supports Restricted admin mode</param>
        /// <param name="bReservedSet">Indicates the value of NEGRSP_FLAG_RESERVED in the flags field of RDP Negotiation Response</param>
        /// <param name="bSupportRestrictedAuthenticationMode">Indicates the server supports restricted authentication mode</param>
        void Server_X_224_Connection_Confirm(selectedProtocols_Values protocol, bool bSupportExtClientData, bool bSetRdpNegData, NegativeType invalidType, bool bSupportEGFX = false, bool bSupportRestrictedAdminMode = false, bool bReservedSet = false, bool bSupportRestrictedAuthenticationMode = false);

        /// <summary>
        /// Send X.224 Connection Confirm PDU. It is sent as a response of X.224 Connection Request.
        /// </summary>
        /// <param name="protocol">Indicates the selected security protocol</param>
        /// <param name="flags">Specify flags field of RDP_NEG_RSP</param>
        void Server_X_224_Connection_Confirm(selectedProtocols_Values protocol, RDP_NEG_RSP_flags_Values flags);

        /// <summary>
        /// Send Server X_224 Connection Confirm Pdu to client
        /// </summary>
        /// <param name="failReason">Indicates the X_224 negotiate failure reason</param>
        void Server_X_224_Negotiate_Failure(failureCode_Values failReason);

        /// <summary>
        /// The MCS Connect Response PDU is an RDP Connection Sequence PDU sent from server to client 
        /// during the Basic Settings Exchange phase of the RDP Connection Sequence 
        /// (see section 1.3.1.1 for an overview of the RDP Connection Sequence phases).
        /// It is sent as a response to the MCS Connect Initial PDU (section 2.2.1.3). 
        /// </summary>
        /// <param name="enMothod">Server selected encryption method.</param>
        /// <param name="enLevel">Server selected encryption level.</param>
        /// <param name="rdpVersion">The RDP Server version</param>
        /// <param name="hasEarlyCapabilityFlags">Indicates the existing of the earlyCapabilityFlags</param>
        /// <param name="earlyCapabilityFlagsValue">The value of earlyCapabilityFlags</param>
        /// <param name="mcsChannelId_Net">MCSChannelId value for Server Network Data</param>
        /// <param name="mcsChannelId_MSGChannel">MCSChannelId value for Server Message Channel Data</param>
        void Server_MCS_Connect_Response(
            EncryptionMethods enMothod,
            EncryptionLevel enLevel,
            TS_UD_SC_CORE_version_Values rdpVersion,
            NegativeType invalidType,
            MULTITRANSPORT_TYPE_FLAGS multiTransportTypeFlags = MULTITRANSPORT_TYPE_FLAGS.None,
            bool hasEarlyCapabilityFlags = false,
            SC_earlyCapabilityFlags_Values earlyCapabilityFlagsValue = SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED,
            UInt16 mcsChannelId_Net = ConstValue.IO_CHANNEL_ID,
            UInt16 mcsChannelId_MSGChannel = ConstValue.MCS_MESSAGE_CHANNEL_ID);

        /// <summary>
        /// Send MCS Attach User Confirm Pdu to client.
        /// </summary>
        void MCSAttachUserConfirm(NegativeType invalidType);

        /// <summary>
        /// Send MCS Channel Join Confirm Pdu to client.
        /// </summary>
        /// <param name="channelId">The channel id that client requested to join.</param>
        void MCSChannelJoinConfirm(long channelId, NegativeType invalidType);

        /// <summary>
        /// Send the Licensing PDU to client.
        /// </summary>
        /// <param name="invalidType">invalid type in pdu</param>
        [MethodHelp("Send the Licensing PDU to client.")]
        void Server_License_Error_Pdu_Valid_Client(NegativeType invalidType);

        /// <summary>
        /// Send Server Demand Active Pdu to client.
        /// </summary>
        void Server_Demand_Active(NegativeType invalidType);

        /// <summary>
        /// Send Server Demand Active Pdu to client
        /// </summary>
        /// <param name="capSetCollection">Specify capability sets</param>
        void Server_Demand_Active(Collection<ITsCapsSet> capSetCollection);

        /// <summary>
        /// Send Server Synchronize Pdu to client.
        /// </summary>
        void ServerSynchronize();

        /// <summary>
        /// Send Server Control Cooperate Pdu to client.
        /// </summary>
        void ServerControlCooperate();

        /// <summary>
        /// Send Server Control Granted Control Pdu to client.
        /// </summary>
        void ServerControlGrantedControl();

        /// <summary>
        /// Send Server Font Map Pdu to client.
        /// </summary>
        void ServerFontMap();

        #endregion

        #region Disconenction Methods

        /// <summary>
        /// Send Server Deactivate All Pdu to client.
        /// </summary>
        void ServerDeactivateAll();

        /// <summary>
        /// Send a MCS Disconnect Provider Ultimatum to client.
        /// </summary>
        /// <param name="adminInitiated">Indicates if it's an admin-Initiated disconnection.</param>
        /// <param name="invalidType">Indicates the invalid type for negative cases</param>
        void Server_MCSDisconnectProviderUltimatum(bool adminInitiated, NegativeType invalidType);

        /// <summary>
        /// Send a Shutdown Request Denied Pdu to client.
        /// </summary>
        void ServerShutdownRequestDenied();

        #endregion

        #region Server Error Reporting and Status Updates
        /// <summary>
        /// Send Server Set Error Info PDU to client.
        /// </summary>
        /// <param name="value">The error code.</param>
        void ServerSetErrorInfoPdu(errorInfo_Values value);
        #endregion

        #region Keyboard Status PDUs
        /// <summary>
        /// Send Server Set Keyboard Indicators PDU to client.
        /// </summary>
        /// <param name="scroll">Indicates that the Scroll Lock indicator light is on or not.</param>
        /// <param name="num">Indicates that the Num Lock indicator light is on or not.</param>
        /// <param name="caps">Indicates that the Caps Lock indicator light is on or not.</param>
        /// <param name="kana">Indicates that the Kana Lock indicator light is on or not.</param>
        void ServerKeybordIndicators(bool scroll, bool num, bool caps, bool kana);

        /// <summary>
        /// Send Server Set Keyboard IME Status PDU to client.
        /// </summary>
        void ServerSetKeyboardIme();
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
        void FPUpdateBitmap(ushort left, ushort top, ushort width, ushort height);

        /// <summary>
        /// Send Fast-Path Pointer Position Update to client.
        /// </summary>
        /// <param name="x">The x-coordinate relative to the top-left corner of the server's desktop.</param>
        /// <param name="y">The y-coordinate relative to the top-left corner of the server's desktop.</param>
        void FPPointerPosition(int xPos, int yPos);

        /// <summary>
        /// Send Fast-Path System Pointer Hidden Update to client.
        /// </summary>
        void FPSystemPointerHidden();

        /// <summary>
        /// Send Fast-Path System Pointer Default Update to client.
        /// </summary>
        void FPSystemPointerDefault();

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
        void FPColorPointer(ushort cacheIndex, ushort hotSpotX, ushort hotSpotY, ushort width, ushort height,
            byte[] xorMaskData = null, byte[] andMaskData = null);

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
        void FPNewPointer(ushort xorBpp, ushort cacheIndex, ushort hotSpotX, ushort hotSpotY, ushort width, ushort height,
            byte[] xorMaskData = null, byte[] andMaskData = null);

        /// <summary>
        /// Send Fast-Path Cached Pointer Update to client
        /// </summary>
        /// <param name="cacheIndex">Cache entry in the pointer cache</param>
        void FPCachedPointer(ushort cacheIndex);

        /// <summary>
        /// Send Fast-Path Surface Commands Update to client.
        /// </summary>
        /// <param name="left">Left bound of the rectangle.</param>
        /// <param name="top">Top bound of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        void FPSurfaceCommand(ushort left, ushort top, ushort width, ushort height);

        /// <summary>
        /// The TS_FP_SURFCMDS structure encapsulates one or more Surface Command (section 2.2.9.1.2.1.10.1) structures.
        /// </summary>
        /// <param name="surfaceCommands">An array of Surface Command (section 2.2.9.1.2.1.10.1) structures 
        /// containing a collection of commands to be processed by the client.</param>
        void SendSurfaceCommandsUpdate(TS_FP_SURFCMDS surfaceCommands);

        /// <summary>
        /// The Frame Marker Command is used to group multiple surface commands 
        /// so that these commands can be processed and presented to the user as a single entity, a frame.
        /// </summary>
        /// <param name="frameAction">A 16-bit, unsigned integer. Identifies the beginning and end of a frame.</param>
        /// <param name="frameId">A 32-bit, unsigned integer. The ID identifying the frame.</param>
        void SendFrameMarkerCommand(frameAction_Values frameAction, uint frameId);

        /// <summary>
        /// The Stream Surface Bits Command is used to transport encoded bitmap data destined 
        /// for a rectangular region of the current target surface from an RDP server to an RDP client.
        /// </summary>
        /// <param name="streamCmd">Stream Surface Bits Command.</param>
        void SendStreamSurfaceBitsCommand(TS_SURFCMD_STREAM_SURF_BITS streamCmd);

        #endregion

        #region Slow-Path
        /// <summary>
        /// Send a Slow-Path output PDU to client.
        /// </summary>
        /// <param name="invalidType">The invalid Type.</param>
        void SendSlowPathOutputPdu(SlowPathTest_InvalidType invalidType);

        /// <summary>
        /// Send Server Play Sound PDU to client.
        /// </summary>
        /// <param name="frequency">Frequency of the beep the client MUST play.</param>
        /// <param name="duration">Duration of the beep the client MUST play.</param>
        void PlaySound(uint frequency, uint duration);
        #endregion

        #endregion

        #region Logon and Authorization Notifications
        /// <summary>
        /// Send Server Save Session Info PDU to client.
        /// </summary>
        /// <param name="notificationType">The Logon Notification type.</param>
        /// <param name="errorType">The Logon Error type. Only used when Notification Type is LogonError.</param>
        void ServerSaveSessionInfo(LogonNotificationType notificationType, ErrorNotificationType_Values errorType);

        /// <summary>
        /// Send an Early User Authorization Result PDU
        /// </summary>
        /// <param name="authorizationResult">The Authorization result value</param>
        void SendEarlyUserAuthorizationResultPDU(Authorization_Result_value authorizationResult);
        #endregion

        #region Auto-Reconnect
        /// <summary>
        /// Send Server Auto-Reconnect Status PDU.
        /// </summary>
        void ServerAutoReconnectStatusPdu();
        #endregion

        #region Display Update Notifications
        /// <summary>
        /// Send Monitor Layout Pdu to client. 
        /// </summary>
        void MonitorLayoutPdu();
        #endregion

        #region Virtual Channel

        /// <summary>
        /// Send virtual channel data. 
        /// </summary>
        /// <param name="channelId">The channel Id of the virtual channel. </param>
        /// <param name="virtualChannelData">The virtual channel data to be sent. </param>
        /// <param name="invalidType">Invalid Type</param>
        void SendVirtualChannelPDU(UInt16 channelId, byte[] virtualChannelData, StaticVirtualChannel_InvalidType invalidType);

        #endregion

        #region Server Redirection

        /// <summary>
        /// Sending Server Redirection Pdu to client.
        /// </summary>
        /// <param name="presentRoutingToken">Indicates if present routing token (LoadBalanceInfo).</param>
        void SendServerRedirectionPdu(bool presentRoutingToken);

        void SendServerRedirectionPduRDSTLS();

        #endregion

        #region Auto-Detection
        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_RTT_REQUEST
        /// </summary>
        void SendAutoDetectRequestPdu_RTTRequest(ushort sequenceNumber, bool isInConnectTime);

        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_BW_START
        /// </summary>
        void SendAutoDetectRequestPdu_BWStart(ushort sequenceNumber, bool isInConnectTime);

        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_BW_PAYLOAD
        /// </summary>
        void SendAutoDetectRequestPdu_BWPayload(ushort sequenceNumber, ushort payloadLength, byte[] payload);

        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_BW_STOP
        /// </summary>
        void SendAutoDetectRequestPdu_BWStop(ushort sequenceNumber, ushort payloadLength, byte[] payload, bool isInConnectTime);

        /// <summary>
        /// Send a Server Auto-Detect Request PDU with RDP_NETCHAR_RESULT
        /// </summary>
        void SendAutoDetectRequestPdu_NetcharResult(ushort sequenceNumber);

        /// <summary>
        /// Send a Tunnel Data PDU with RDP_BW_START in its subheader
        /// </summary>
        /// <param name="requestedProtocol"></param>
        /// <param name="sequenceNumber"></param>
        void SendTunnelDataPdu_BWStart(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber);

        /// <summary>
        /// Send a Tunnel Data PDU with RDP_BW_STOP in its subheader
        /// </summary>
        /// <param name="requestedProtocol"></param>
        /// <param name="sequenceNumber"></param>
        void SendTunnelDataPdu_BWStop(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber);

        /// <summary>
        /// Send a Tunnel Data PDU with RDP_NETCHAR_RESULT in its subheader
        /// </summary>
        /// <param name="requestedProtocol"></param>
        /// <param name="sequenceNumber"></param>
        void SendTunnelDataPdu_NetcharResult(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber);

        /// <summary>
        /// Wait for a Tunnel Data PDU with RDP_BW_RESULTS and check its sequenceNumber.
        /// </summary>
        /// <param name="requestedProtocol">Which tunnel to be used, reliable or lossy</param>
        /// <param name="sequenceNumber"></param>
        /// <param name="timeout"></param>
        void WaitForAndCheckTunnelDataPdu_BWResult(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber, TimeSpan timeout);

        /// <summary>
        /// Check whether the latest received Auto-Detect Response PDU is a valid PDU
        /// </summary>
        /// <param name="responseType">The responseType the Auto-Detect Response PDU should be</param>
        /// <param name="sequenceNumber">The sequenceNumber the Auto-Detect Response PDU should be</param>
        void CheckAutoDetectResponsePdu(AUTO_DETECT_RESPONSE_TYPE responseType, ushort sequenceNumber);

        /// <summary>
        /// Check whether the auto-detect Response structure is valid
        /// </summary>
        /// <param name="autodetectRspPduData">The auto-detect Response structure need to be checked</param>
        /// <param name="responseType">The responseType the Auto-Detect Response PDU should be</param>
        /// <param name="sequenceNumber">The sequenceNumber the Auto-Detect Response PDU should be</param>
        void CheckAutoDetectResponse(NETWORK_DETECTION_RESPONSE autodetectRspPduData, AUTO_DETECT_RESPONSE_TYPE responseType, ushort sequenceNumber);

        #endregion

        #region Multitransport 

        /// <summary>
        /// Send a Server Initiate Multitransport Request PDU
        /// </summary>
        /// <param name="requestedProtocol"></param>
        void SendServerInitiateMultitransportRequestPDU(uint requestId, Multitransport_Protocol_value requestedProtocol, byte[] securityCookie);

        /// <summary>
        /// Create a multitransport Channel over UDP
        /// </summary>
        /// <param name="requestedProtocol">Request protocol, indicate the channel is reliable or lossy</param>
        /// <param name="timeout"></param>
        void CreateMultitransportChannelConnection(Multitransport_Protocol_value requestedProtocol, TimeSpan timeout);

        /// <summary>
        /// Send data from multitransport tunnel
        /// </summary>
        /// <param name="requestedProtocol">Indicates which tunnel is used, reliable or lossy</param>
        /// <param name="data">Data want to sent</param>
        /// <param name="subHdDataArr">Array of Auto-detect data, will be send as subheader of TunnelDataPdu</param>
        void SendTunnelData(Multitransport_Protocol_value requestedProtocol, byte[] data, byte[][] subHdDataArr = null);


        #endregion Multitransport 

        #region Connection Health Monitoring

        /// <summary>
        /// Send a Server Heartbeat PDU
        /// </summary>
        /// <param name="period">specifies the time (in seconds) between Heartbeat PDUs</param>
        /// <param name="warningCount">specifies how many missed heartbeats SHOULD trigger a client-side warning</param>
        /// <param name="reconnectCount">specifies how many missed heartbeats SHOULD trigger a client-side reconnection attempt</param>
        void SendServerHeartbeatPDU(byte period, byte warningCount, byte reconnectCount);

        /// <summary>
        /// Check whether client support Heartbeat
        /// If supported, RNS_UD_CS_SUPPORT_HEARTBEAT_PDU should be set on earlyCapabilityFlags field of TS_UD_CS_CORE
        /// </summary>
        void CheckHeartbeatSupport();

        #endregion Connection Health Monitoring

        #region Others

        /// <summary>
        /// Method to get the id of the specified static virtual channel.
        /// </summary>
        /// <param name="channelName">The specified channel name.</param>
        /// <returns>The channel id of the specified channel. If not found, return 0.</returns>
        ushort GetStaticVirtualChannelId(string channelName);

        /// <summary>
        /// Method to turn off the RDPBCGR requirements verification.
        /// </summary>
        /// <param name="turnOff">Indicates if to turn off.</param>
        void TurnVerificationOff(bool turnOff);

        #endregion

        #endregion

        #region Sequences

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
        void EstablishRDPConnection(
            selectedProtocols_Values serverSelectedProtocol,
            EncryptionMethods enMethod,
            EncryptionLevel enLevel,
            bool isExtendedClientDataSupported,
            bool expectAutoReconnect,
            TS_UD_SC_CORE_version_Values rdpServerVersion,
            MULTITRANSPORT_TYPE_FLAGS multiTransportTypeFlags = MULTITRANSPORT_TYPE_FLAGS.None,
            bool supportRDPEGFX = false,
            bool supportRestrictedAdminMode = false);

        /// <summary>
        /// Start a server Initiated disconnection sequence
        /// </summary>
        /// <param name="sendDisonnectPdu">Indicates if server send MCS Disconnect Provider Ultimatum PDU to client.</param>
        /// <param name="admin">Indicates it's an Administrator-Initiated disconnection or User-Initiated disconnection.</param>
        /// <param name="invalidType">Indicates the invalid type for negative cases</param>
        void ServerInitiatedDisconnect(bool sendDisonnectPdu, bool admin, NegativeType invalidType);

        /// <summary>
        /// Expect a client initiated disconnection sequence.
        /// </summary>
        /// <param name="timeout">The maximum time duration to wait.</param>
        /// <param name="respondDeniedPdu">Indicates if send the Shutdown Request PDU to client.</param>
        /// <param name="expectDisconnectPdu">Indicates if expect a client MCS Disconnect Provider Ultimatum PDU.</param>
        void ExpectClientInitiatedDisconnect(TimeSpan timeout, bool respondDeniedPdu, bool expectDisconnectPdu);

        /// <summary>
        /// Start a Deactivate-Reactivate sequence.
        /// </summary>
        void DeactivateReactivate();

        /// <summary>
        /// Expect SUT start the channel join sequence
        /// </summary>
        void ChannelJoinRequestAndConfirm(NegativeType invalidType);

        #endregion

        #region Receive Methods

        /// <summary>
        /// Expect a specified type PDU from RDP client, 
        /// if the next received packet is not T, this call will be failed.
        /// </summary>
        /// <typeparam name="T">The expected type of pdu</typeparam>
        /// <param name="timeout">Timeout.</param>
        void ExpectPacket<T>(TimeSpan timeout);

        /// <summary>
        /// Wait for a specified type PDU from RDP client, 
        /// if the next received packet is not T, continue to wait until timeout.
        /// </summary>
        /// <typeparam name="T">The expected type of pdu</typeparam>
        /// <param name="timeout">Timeout.</param>
        void WaitForPacket<T>(TimeSpan timeout);

        /// <summary>
        /// Wait for a specified type PDU from RDP client, 
        /// if the next received packet is not T, continue to wait until timeout.
        /// </summary>
        /// <typeparam name="T">The expected type of pdu</typeparam>
        /// <param name="timeout">Timeout.</param>
        /// <returns>true if received, otherwise, return false.</returns>
        bool WaitForClientPacket<T>(TimeSpan timeout);

        /// <summary>
        /// Wait for a Slow-Path Input PDU with the specified event type.
        /// </summary>
        /// <param name="expectedEventType">The expected event type.</param>
        /// <param name="timeout">Timeout.</param>
        /// <returns>true if received, otherwise, return false.</returns>
        bool WaitForSlowPathInputPdu(TS_INPUT_EVENT_messageType_Values expectedEventType, TimeSpan timeout);

        /// <summary>
        /// Wait for a Fast-Path Input PDU with the specified event type.
        /// </summary>
        /// <param name="expectedEventType">The expected event type.</param>
        /// <param name="timeout">Timeout.</param>
        /// <returns>true if received, otherwise, return false.</returns>
        bool WaitForFastPathInputPdu(eventCode_Values expectedEventType, TimeSpan timeout);

        /// <summary>
        /// Wait for a Virtual Channel PDU which transported in the specified channel.
        /// </summary>
        /// <param name="channelId">The specified channel id.</param>
        /// <param name="timeout">Timeout.</param>
        void WaitForVirtualChannelPdu(UInt16 channelId, out byte[] virtualChannelData, TimeSpan timeout);

        /// <summary>
        /// Wait for the RDP client terminates the connection.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        bool WaitForDisconnection(TimeSpan timeout);

        #endregion
    }
}
