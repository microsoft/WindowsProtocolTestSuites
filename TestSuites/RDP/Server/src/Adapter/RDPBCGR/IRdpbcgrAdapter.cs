// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    #region Delegate

    public delegate void ServerX224ConnectionConfirmHandler(Server_X_224_Connection_Confirm_Pdu x224Confirm);
    public delegate void ServerX224NegotiateFailurePDUHandler(Server_X_224_Negotiate_Failure_Pdu x224Failure);
    public delegate void ServerMCSConnectResponseHandler(Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsConnectResponse);
    public delegate void ServerMCSAttachUserConfirmHandler(Server_MCS_Attach_User_Confirm_Pdu attachUserConfirm);
    public delegate void ServerMCSChannelJoinConfirmHandler(Server_MCS_Channel_Join_Confirm_Pdu channelJoinConfirm);
    public delegate void ServerLicenseErrorPDUHandler(Server_License_Error_Pdu_Valid_Client licenseErrorPdu);
    public delegate void ServerDemandActivePDUHandler(Server_Demand_Active_Pdu demandActivePdu);
    public delegate void ServerSynchronizePDUHandler(Server_Synchronize_Pdu synchronizePdu);
    public delegate void ServerCooperateControlPDUHandler(Server_Control_Pdu_Cooperate controlPdu);
    public delegate void ServerGrantedControlPDUHandler(Server_Control_Pdu_Granted_Control controlPdu);
    public delegate void ServerFontMapPDUHandler(Server_Font_Map_Pdu fontMapPdu);
    public delegate void ServerShutdownRequestDeniedPDUHandler(Server_Shutdown_Request_Denied_Pdu shutdownRequest);
    public delegate void ServerMCSDisconnectProviderUltimatumHandler(MCS_Disconnect_Provider_Ultimatum_Pdu disconnectProvider);
    public delegate void ServerDeactivateAllPDUHandler(Server_Deactivate_All_Pdu deactiveAllPdu);
    public delegate void ServerVirtualChannelPDUHandler(Virtual_Channel_RAW_Server_Pdu virtualChannelPdu);
    public delegate void ServerSlowPathOutputUpdatePDUHandler(SlowPathOutputPdu updatePdu);
    public delegate void ServerFastPathUpdatePDUHandler(TS_FP_UPDATE_PDU updatePdu);
    public delegate void ServerRedirectionPacketHandler(Server_Redirection_Pdu redirectionPdu);
    public delegate void EnhancedSecurityServerRedirectionPacketHandler(Enhanced_Security_Server_Redirection_Pdu redirectionPdu);
    public delegate void ServerAutoDetectRequestHandler(Server_Auto_Detect_Request_PDU autoDetectRequest);
    public delegate void ServerInitiateMultitransportRequestHandler(Server_Initiate_Multitransport_Request_PDU multitransportReq);
    public delegate void ServerHeartbeatPDUHandler(Server_Heartbeat_PDU heartbeatPdu);
    public delegate void ServerSaveSessionInfoPDUHandler(Server_Save_Session_Info_Pdu saveSessionInfoPdu);

    #endregion Delegate

    public interface IRdpbcgrAdapter : IAdapter
    {
        #region Event

        /// <summary>
        /// Event will be raised when a Server X224 Connection Confirm PDU is received
        /// </summary>
        event ServerX224ConnectionConfirmHandler OnServerX224ConnectionConfirmReceived;

        /// <summary>
        /// Event will be raised when a Server X224 Negotiate Failure PDU is received
        /// </summary>
        event ServerX224NegotiateFailurePDUHandler OnServerX224NegotiateFailurePDUReceived;

        /// <summary>
        /// Event will be raised when a Server MCS Connect Response PDU is received
        /// </summary>
        event ServerMCSConnectResponseHandler OnServerMCSConnectResponseReceived;

        /// <summary>
        /// Event will be raised when a Server MCS Attach User Confirm PDU is received
        /// </summary>
        event ServerMCSAttachUserConfirmHandler OnServerMCSAttachUserConfirmReceived;

        /// <summary>
        /// Event will be raised when a Server MCS Channel Join Confirm PDU is received
        /// </summary>
        event ServerMCSChannelJoinConfirmHandler OnServerMCSChannelJoinConfirmReceived;

        /// <summary>
        /// Event will be raised when a Server License Error PDU is received
        /// </summary>
        event ServerLicenseErrorPDUHandler OnServerLicenseErrorPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Demand Active PDU is received
        /// </summary>
        event ServerDemandActivePDUHandler OnServerDemandActivePDUReceived;

        /// <summary>
        /// Event will be raised when a Server Synchronize PDU is received
        /// </summary>
        event ServerSynchronizePDUHandler OnServerSynchronizePDUReceived;

        /// <summary>
        /// Event will be raised when a Server Control PDU (Cooperate) is received
        /// </summary>
        event ServerCooperateControlPDUHandler OnServerCooperateControlPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Control PDU (Granted Control) is received
        /// </summary>
        event ServerGrantedControlPDUHandler OnServerGrantedControlPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Font Map PDU is received
        /// </summary>
        event ServerFontMapPDUHandler OnServerFontMapPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Shutdown Request Denied PDU is received
        /// </summary>
        event ServerShutdownRequestDeniedPDUHandler OnServerShutdownRequestDeniedReceived;

        /// <summary>
        /// Event will be raised when a Server MCS Disconnect Provider Ultimatum PDU is received
        /// </summary>
        event ServerMCSDisconnectProviderUltimatumHandler OnServerMCSDisconnectProviderUltimatumReceived;

        /// <summary>
        /// Event will be raised when a Server Deactivate All PDU is received
        /// </summary>
        event ServerDeactivateAllPDUHandler OnServerDeactivateAllPDUReceived;

        /// <summary>
        /// Event will be raised when a  is received
        /// </summary>
        event ServerVirtualChannelPDUHandler OnServerVirtualChannelPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Graphics Update PDU or Server Pointer Update PDU is received
        /// </summary>
        event ServerSlowPathOutputUpdatePDUHandler OnServerSlowPathOutputUpdatePDUReceived;

        /// <summary>
        /// Event will be raised when a Server Fast-Path Update PDU  is received
        /// </summary>
        event ServerFastPathUpdatePDUHandler OnServerFastPathUpdatePDUReceived;

        /// <summary>
        /// Event will be raised when a Server Redirection Packet is received
        /// </summary>
        event ServerRedirectionPacketHandler OnServerRedirectionPacketReceived;

        /// <summary>
        /// Event will be raised when a Enhanced Security Server Redirection Packet is received
        /// </summary>
        event EnhancedSecurityServerRedirectionPacketHandler OnEnhancedSecurityServerRedirectionPacketReceived;

        /// <summary>
        /// Event will be raised when a Server Auto-Detect Request PDU is received
        /// </summary>
        event ServerAutoDetectRequestHandler OnServerAutoDetectRequestReceived;

        /// <summary>
        /// Event will be raised when a Server Initiate Multitransport Request PDU is received
        /// </summary>
        event ServerInitiateMultitransportRequestHandler OnServerInitiateMultitransportRequestReceived;

        /// <summary>
        /// Event will be raised when a Server Heartbeat PDU is received
        /// </summary>
        event ServerHeartbeatPDUHandler OnServerHeartbeatPDUReceived;

        /// <summary>
        /// Event will be raised when a Server Save Session Info PDU is received
        /// </summary>
        event ServerSaveSessionInfoPDUHandler OnServerSaveSessionInfoPDUReceived;

        #endregion Event

        #region Properties

        /// <summary>
        /// Whether the RDP server support bitmap cache host 
        /// </summary>
        bool IsBitmapCacheHostSupport { get; }

        #endregion Properties

        #region Connection Methods

        /// <summary>
        /// Establish transport connection with RDP Server
        /// </summary>
        /// <param name="encryptedProtocol">Enctypted protocol</param>
        void ConnectToServer(EncryptedProtocol encryptedProtocol);

        /// <summary>
        /// Disconnect transport connection with RDP Server
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Perform a client-initiated disconnection.
        /// </summary>
        void ClientInitiatedDisconnect();

        /// <summary>
        /// Send a Client X224 Connection Request PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="requestedProtocols">Flags indicate supported security protocols</param>
        /// <param name="isRdpNegReqPresent">Whether RdpNegReq is present</param>
        /// <param name="isRoutingTokenPresent">Whether RoutingToken is present</param>
        /// <param name="isCookiePresent">Whether Cookie is present</param>
        /// <param name="isRdpCorrelationInfoPresent">Whether RdpCorrelationInfo is present</param>
        void SendClientX224ConnectionRequest(NegativeType invalidType, 
            requestedProtocols_Values requestedProtocols,
            bool isRdpNegReqPresent = true, 
            bool isRoutingTokenPresent = false, 
            bool isCookiePresent = false, 
            bool isRdpCorrelationInfoPresent = false);

        /// <summary>
        /// Send a Client MCS Connect Initial PDU with GCC Conference Create Request
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="SVCNames">Array of static virtual channels' name</param>
        /// <param name="supportEGFX">Set the support of RDPEGFX</param>
        /// <param name="supportAutoDetect">Set the support of auto-detect</param>
        /// <param name="supportHeartbeatPDU">Set the support of Heartbeat PDU</param>
        /// <param name="supportMultitransportReliable">Set the support of reliable multitransport</param>
        /// <param name="supportMultitransportLossy">Set the support of lossy multitransport</param>
        /// <param name="isMonitorDataPresent">Whether the Client Monitor Data is present</param>
        void SendClientMCSConnectInitialPDU(NegativeType invalidType, 
            string[] SVCNames, 
            bool supportEGFX, 
            bool supportAutoDetect, 
            bool supportHeartbeatPDU, 
            bool supportMultitransportReliable, 
            bool supportMultitransportLossy, 
            bool isMonitorDataPresent);

        /// <summary>
        /// Send a Client MCS Erect Domain Request PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        void SendClientMCSErectDomainRequest(NegativeType invalidType);

        /// <summary>
        /// Send a Client MCS Attach User Request PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        void SendClientMCSAttachUserRequest(NegativeType invalidType);

        /// <summary>
        /// Send a Client MCS Channel Join Request PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="channelId">Channel ID</param>
        void SendClientMCSChannelJoinRequest(NegativeType invalidType, long channelId);

        /// <summary>
        /// Send a Client Security Exchange PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        void SendClientSecurityExchangePDU(NegativeType invalidType);

        /// <summary>
        /// Send a Client Info PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="highestCompressionTypeSupported">Indicate the highest compression type supported</param>
        /// <param name="autoLogon">Indicate wether auto logon using username and password of info packet</param>
        /// <param name="isReconnect">Whether this is in a reconnection sequence</param>
        void SendClientInfoPDU(NegativeType invalidType, CompressionType highestCompressionTypeSupported, bool isReconnect = false, bool autoLogon = true);

        /// <summary>
        /// Send a Client Confirm Active PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="supportAutoReconnect">Set the support of auto-reconnect</param>
        /// <param name="supportFastPathInput">Set the support of fast-path input</param>
        /// <param name="supportFastPathOutput">Set the support of fast-path output</param>
        /// <param name="supportSurfaceCommands">Set the support of surface commands</param>
        /// <param name="supportSVCCompression">Set the support of static virtual channel data compression</param>
        /// <param name="supportRemoteFXCodec">Set the support of RemoteFX codecs</param>
        void SendClientConfirmActivePDU(NegativeType invalidType,
            bool supportAutoReconnect, 
            bool supportFastPathInput, 
            bool supportFastPathOutput, 
            bool supportSurfaceCommands, 
            bool supportSVCCompression, 
            bool supportRemoteFXCodec);

        /// <summary>
        /// Send a Client Synchronize PDU
        /// </summary>
        void SendClientSynchronizePDU();

        /// <summary>
        /// Send a Client Control PDU (Cooperate)
        /// </summary>
        void SendClientControlCooperatePDU();

        /// <summary>
        /// Send a Client Control PDU (Request Control)
        /// </summary>
        void SendClientControlRequestPDU();

        /// <summary>
        /// Send a Client Persistent Key List PDU
        /// </summary>
        void SendClientPersistentKeyListPDU();

        /// <summary>
        /// Send a Client Font List PDU
        /// </summary>
        void SendClientFontListPDU();

        /// <summary>
        /// Send a Client Shutdown Request PDU
        /// </summary>
        void SendClientShutdownRequestPDU();

        #endregion Connection Methods

        #region Input/Output Methods

        /// <summary>
        /// Send a Client Input Event PDU 
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="inputEvents">Array of input events to be sent</param>
        void SendClientInputEventPDU(NegativeType invalidType, TS_INPUT_EVENT[] inputEvents);

        #region TS_INPUT_EVENT generation

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_SYNC_EVENT
        /// </summary>
        /// <param name="scrollLock">Wether the Scroll Lock indicator light SHOULD be on</param>
        /// <param name="numLock">Wether the Num Lock indicator light SHOULD be on</param>
        /// <param name="capsLock">Wether the Caps Lock indicator light SHOULD be on</param>
        /// <param name="kanaLock">Wether the Kana Lock indicator light SHOULD be on</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_SYNC_EVENT</returns>
        TS_INPUT_EVENT GenerateSynchronizeEvent(bool scrollLock, bool numLock, bool capsLock, bool kanaLock);

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_KEYBOARD_EVENT
        /// </summary>
        /// <param name="keyboardFlags">The flags describing this keyboard event</param>
        /// <param name="keyCode">The scancode of the key</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_KEYBOARD_EVENT</returns>
        TS_INPUT_EVENT GenerateKeyboardEvent(keyboardFlags_Values keyboardFlags, ushort keyCode);

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_UNICODE_KEYBOARD_EVENT
        /// </summary>
        /// <param name="keyboardFlags">The flags describing the Unicode keyboard event, must be KBDFLAGS_RELEASE (0x8000)</param>
        /// <param name="unicodeCode">The Unicode character input code</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_UNICODE_KEYBOARD_EVENT</returns>
        TS_INPUT_EVENT GenerateUnicodeKeyboardEvent(keyboardFlags_Values keyboardFlags, ushort unicodeCode);

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_POINTER_EVENT
        /// </summary>
        /// <param name="pointerFlags">The flags describing the pointer event.</param>
        /// <param name="xPos">The x-coordinate of the pointer relative to the top-left corner of the server's desktop</param>
        /// <param name="yPos">The y-coordinate of the pointer relative to the top-left corner of the server's desktop</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_POINTER_EVENT</returns>
        TS_INPUT_EVENT GenerateMouseEvent(pointerFlags_Values pointerFlags, ushort xPos, ushort yPos);

        /// <summary>
        /// Generate a TS_INPUT_EVENT structure with a TS_POINTERX_EVENT
        /// </summary>
        /// <param name="pointerFlags">The flags describing the extended mouse event</param>
        /// <param name="xPos">The x-coordinate of the pointer</param>
        /// <param name="yPos">The Y-coordinate of the pointer</param>
        /// <returns>TS_INPUT_EVENT structure with a TS_POINTERX_EVENT</returns>
        TS_INPUT_EVENT GenerateExtendedMouseEvent(TS_POINTERX_EVENT_pointerFlags_Values pointerFlags, ushort xPos, ushort yPos);

        #endregion TS_INPUT_EVENT generation

        /// <summary>
        /// Send a Client Fast-Path Input Event PDU 
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="inputEvents">Array of input events to be sent</param>
        void SendClientFastPathInputEventPDU(NegativeType invalidType, TS_FP_INPUT_EVENT[] inputEvents);

        #region TS_FP_INPUT_EVENT generation

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_KEYBOARD_EVENT
        /// </summary>
        /// <param name="eventFlag">The flags describing the keyboard event</param>
        /// <param name="keyCode">The scancode of the key</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_KEYBOARD_EVENT</returns>
        TS_FP_INPUT_EVENT GenerateFPKeyboardEvent(TS_FP_KEYBOARD_EVENT_Eventflags eventFlag, byte keyCode);

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_POINTER_EVENT
        /// </summary>
        /// <param name="pointerFlags">The flags describing the pointer event</param>
        /// <param name="xPos">The x-coordinate of the pointer</param>
        /// <param name="yPos">The y-coordinate of the pointer</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_POINTER_EVENT</returns>
        TS_FP_INPUT_EVENT GenerateFPMouseEvent(pointerFlags_Values pointerFlags, ushort xPos, ushort yPos);

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_POINTERX_EVENT
        /// </summary>
        /// <param name="pointerFlags">The flags describing the pointer event</param>
        /// <param name="xPos">The x-coordinate of the pointer</param>
        /// <param name="yPos">The y-coordinate of the pointer</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_POINTERX_EVENT</returns>
        TS_FP_INPUT_EVENT GenerateFPExtendedMouseEvent(TS_POINTERX_EVENT_pointerFlags_Values pointerFlags, ushort xPos, ushort yPos);

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_SYNC_EVENT
        /// </summary>
        /// <param name="scrollLock">Wether the Scroll Lock indicator light SHOULD be on</param>
        /// <param name="numLock">Wether the Num Lock indicator light SHOULD be on</param>
        /// <param name="capsLock">Wether the Caps Lock indicator light SHOULD be on</param>
        /// <param name="kanaLock">Wether the Kana Lock indicator light SHOULD be on</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_SYNC_EVENT</returns>
        TS_FP_INPUT_EVENT GenerateFPSynchronizeEvent(bool scrollLock, bool numLock, bool capsLock, bool kanaLock);

        /// <summary>
        /// Generate a TS_FP_INPUT_EVENT structure with a TS_FP_UNICODE_KEYBOARD_EVENT
        /// </summary>
        /// <param name="eventFlag">The flags describing the keyboard event, must be FASTPATH_INPUT_KBDFLAGS_RELEASE (0x01)</param>
        /// <param name="unicodeCode">The Unicode character input code</param>
        /// <returns>TS_FP_INPUT_EVENT structure with a TS_FP_UNICODE_KEYBOARD_EVENT</returns>
        TS_FP_INPUT_EVENT GenerateFPUnicodeKeyboardEvent(TS_FP_KEYBOARD_EVENT_Eventflags eventFlag, ushort unicodeCode);

        #endregion TS_FP_INPUT_EVENT generation

        /// <summary>
        /// Send a Client Refresh Rect PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="rects">Array of screen area Inclusive Rectangles to redraw</param>
        void SendClientRefreshRectPDU(NegativeType invalidType, TS_RECTANGLE16[] rects);

        /// <summary>
        /// Send a Client Suppress Output PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="allowDisplayUpdates">Indicates whether the client wants to receive display updates from the server</param>
        /// <param name="rect">The coordinates of the desktop rectangle</param>
        void SendClientSuppressOutputPDU(NegativeType invalidType, AllowDisplayUpdates_SUPPRESS_OUTPUT allowDisplayUpdates, TS_RECTANGLE16 rect);

        #endregion Input/Output Methods

        #region Virtual Channel Methods

        /// <summary>
        /// Send data through specified static virtual channel
        /// </summary>
        /// <param name="channelId">ID of static virtual channel</param>
        /// <param name="virtualChannelData">Data to sent</param>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        void SendVirtualChannelPDU(UInt16 channelId, byte[] virtualChannelData, NegativeType invalidType);

        #endregion Virtual Channel Methods

        #region Auto-detect Methods

        /// <summary>
        /// Send a Client Auto-Detect Response PDU with a RDP_RTT_RESPONSE
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="sequenceNumber">The message sequence number</param>
        void SendClientAutoDetectResponsePDUWithRTTResponse(NegativeType invalidType, ushort sequenceNumber);

        /// <summary>
        /// Send a Client Auto-Detect Response PDU with a RDP_BW_RESULTS
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="sequenceNumber">The message sequence number</param>
        /// <param name="timeDelta">The time delta, in milliseconds, between the receipt of the Bandwidth Measure Start and the Bandwidth Measure Stop messages</param>
        /// <param name="byteCount">The total data received in the Bandwidth Measure Payload messages</param>
        /// <param name="isDuringConnection">Whether in a connect-time auto-detection phase</param>
        void SendClientAutoDetectResponsePDUWithBWResults(NegativeType invalidType, ushort sequenceNumber, uint timeDelta, uint byteCount, bool isDuringConnection);

        /// <summary>
        /// Send a Client Auto-Detect Response PDU with a RDP_NETCHAR_SYNC
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="sequenceNumber">The message sequence number</param>
        /// <param name="bandwidth">The previously detected bandwidth in kilobits per second</param>
        /// <param name="rtt">The previously detected round-trip time in milliseconds</param>
        void SendClientAutoDetectResponsePDUWithNetworkSync(NegativeType invalidType, ushort sequenceNumber, uint bandwidth, uint rtt);

        #endregion Auto-detect Methods

        #region Multitransport Bootstrapping

        /// <summary>
        /// Send a Client Initiate Multitransport Error PDU
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        /// <param name="requestId">The ID which is the same as he requestId field of the associated Initiate Multitransport Request PDU </param>
        void SendClientInitiateMultitransportErrorPDU(NegativeType invalidType, uint requestId);
        
        #endregion Multitransport Bootstrapping

        #region Expect Methods

        /// <summary>
        /// Expect to receive a packet whose type is T
        /// </summary>
        /// <typeparam name="T">Type of packet, which must be inherited from StackPacket</typeparam>
        /// <param name="timeout">Timeout</param>
        /// <returns>Received packet if succeed, otherwise, return null</returns>
        T ExpectPacket<T>(TimeSpan timeout) where T : StackPacket;

        /// <summary>
        /// Expect the RDP server drop the connection
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>True if RDP server dropped the connection, otherwise, return false</returns>
        bool ExpectDisconnetion(TimeSpan timeout);

        /// <summary>
        /// Wait until user logon or timeout
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <param name="expectCookie">Also expect a Server Save Session Info PDU with auto-reconnect cookie</param>
        void WaitForLogon(TimeSpan timeout, bool expectCookie = false);

        #endregion Expect Methods
        
        #region Verify Capability

        /// <summary>
        /// Whether RDP server support Auto Reconnect
        /// Check extraFlags flag of TS_GENERAL_CAPABILITYSET
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        bool IsServerSupportAutoReconnect();

        /// <summary>
        /// Whether RDP Server support fastpath input
        /// Check inputFlags flag of TS_INPUT_CAPABILITYSET
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        bool IsServerSupportFastpathInput();

        /// <summary>
        /// Whether server support RDP-UDP FEC reliable transport
        /// Check flags field of TS_UD_SC_MULTITRANSPORT
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        bool IsServerSupportUDPFECR();
        
        /// <summary>
        /// Whether server support RDP-UDP FEC lossy transport
        /// Check flags field of TS_UD_SC_MULTITRANSPORT
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        bool IsServerSupportUDPFECL();

        /// <summary>
        /// Whether server support tunneling of static virtual channel traffic over UDP
        /// Check flags field of TS_UD_SC_MULTITRANSPORT
        /// </summary>
        /// <returns>Return true if supported, otherwise, return false</returns>
        bool IsSupportUDPPreffered();

        #endregion Verify Capability

        #region sequences

        /// <summary>
        /// Establish a RDP connection with RDP Server
        /// </summary>
        /// <param name="requestedProtocols">Flags indicate supported security protocols</param>
        /// <param name="SVCNames">Array of static virtual channels' name</param>
        /// <param name="highestCompressionTypeSupported">Indicate the highest compression type supported</param>
        /// <param name="isReconnect">Whether this is in a reconnection sequence</param>
        /// <param name="autoLogon">Whether auto logon using username and password in client info PDU</param>
        /// <param name="supportEGFX">Set the support of RDPEGFX</param>
        /// <param name="supportAutoDetect">Set the support of auto-detect</param>
        /// <param name="supportHeartbeatPDU">Set the support of Heartbeat PDU</param>
        /// <param name="supportMultitransportReliable">Set the support of reliable multitransport</param>
        /// <param name="supportMultitransportLossy">Set the support of lossy multitransport</param>
        /// <param name="supportAutoReconnect">Set the support of auto-reconnect</param>
        /// <param name="supportFastPathInput">Set the support of fast-path input</param>
        /// <param name="supportFastPathOutput">Set the support of fast-path output</param>
        /// <param name="supportSurfaceCommands">Set the support of surface commands</param>
        /// <param name="supportSVCCompression">Set the support of static virtual channel data compression</param>
        /// <param name="supportRemoteFXCodec">Set the support of RemoteFX codecs</param>
        void EstablishRDPConnection(requestedProtocols_Values requestedProtocols, 
            string[] SVCNames, 
            CompressionType highestCompressionTypeSupported, 
            bool isReconnect = false,
            bool autoLogon = false,
            bool supportEGFX = false,
            bool supportAutoDetect = false,
            bool supportHeartbeatPDU = false,
            bool supportMultitransportReliable = false,
            bool supportMultitransportLossy = false,
            bool supportAutoReconnect = false,
            bool supportFastPathInput = false,
            bool supportFastPathOutput = false,
            bool supportSurfaceCommands = false,
            bool supportSVCCompression = false,
            bool supportRemoteFXCodec = false);

        /// <summary>
        /// Complete the channel join sequence
        /// </summary>
        void ChannelJoinRequestAndConfirm();

        /// <summary>
        /// Generate static virtual channel traffics
        /// </summary>
        /// <param name="invalidType">Invalid Type used for negative test case</param>
        void GenerateStaticVirtualChannelTraffics(NegativeType invalidType);

        /// <summary>
        /// Send Client Input Event PDUs with all kinds of input events
        /// </summary>
        void GenerateSlowPathInputs();

        /// <summary>
        /// Send Client Fast-Path Input Event PDU with all kinds of input events
        /// </summary>
        void GenerateFastPathInputs();

        /// <summary>
        /// Expect and verifies fast-path output events during a specific timespan
        /// </summary>
        /// <param name="timeout">TimeSpan for waiting FP outputs</param>
        void ExpectFastpathOutputs(TimeSpan timeout);

        /// <summary>
        /// Used to verify whether a RDP connection is still exist. 
        /// This function can only be used after RDP connection established and user logon.
        ///  Send a Shutdown Request PDU to the server.
        ///  Expect a Shutdown Request Denied PDU from server.
        /// </summary>
        /// <param name="timeout">Timeout when expect Shutdown Request Denied PDU</param>
        /// <returns></returns>
        bool VerifyRDPConnection(TimeSpan timeout);

        /// <summary>
        /// Process the network auto-detect sequence 
        /// </summary>
        void ProcessAutoDetectSequence();

        #endregion sequences
    }
}
