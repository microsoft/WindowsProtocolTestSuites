// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// This is used for model to verify the RS2299 which is should/may template code.
    /// </summary>
    /// <param name="isRsImplemented">If RS2299 implemented, it is true, else it is false.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void IsRs2299ImplementedResponseHandle(bool isRsImplemented);

    /// <summary>
    /// This is used for model to verify the RS4984 which is should/may template code.
    /// </summary>
    /// <param name="isRsImplemented">If RS4984 implemented, it is true, else it is false.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void IsRs4984ImplementedResponseHandle(bool isRsImplemented);

    /// <summary>
    /// SessionSetup response additional handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a 
    /// previously established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="securitySignatureValue">
    /// Indicate the security signature used in session setup response header.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required. </param>
    /// <param name="isGuestAccount">Indicate whether the account is a guest account or an admin account.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void SessionSetupResponseAdditionalHandle(
        int messageId,
        int sessionId,
        int securitySignatureValue,
        bool isSigned,
        bool isGuestAccount,
        MessageStatus messageStatus);

    /// <summary>
    /// Read response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a 
    /// previously established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">The tree identifier.</param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required. </param>
    /// <param name="isSendBufferSizeExceedMaxBufferSize">
    /// Indicate whether the bufferSize sent by the SUT exceeds the max buffer size or not.
    /// </param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void ReadResponseHandle(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        bool isSendBufferSizeExceedMaxBufferSize,
        MessageStatus messageStatus);

    /// <summary>
    /// Write response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a 
    /// previously established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">The tree identifier.</param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// <param name="isWrittenByteCountEqualCountField">
    /// Indicate whether the byte count written by the SUT equal to the count field or not.
    /// </param>
    /// <param name="isWrittenByteCountEqualCountHighField">
    /// Indicate whether the byte count written by the SUT equal to the count high field or not.
    /// </param>
    /// <param name="isWrittenByteCountLargerThanMaxBufferSize">
    /// Indicate whether the byte count written by the SUT larger than the max buffer size or not.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void WriteResponseHandle(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus,
        bool isWrittenByteCountEqualCountField,
        bool isWrittenByteCountEqualCountHighField,
        bool isWrittenByteCountLargerThanMaxBufferSize);

    /// <summary>
    /// Error write response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// <param name="isRS5229Implemented">If the RS5229 implemented, it is true, else it is false.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void ErrorWriteResponseHandle(
        int messageId,
        MessageStatus messageStatus,
        bool isRS5229Implemented);

    /// <summary>
    /// SMB connection response handler.
    /// </summary>
    /// <param name="clientPlatform">Platform of client.</param>
    /// <param name="sutPlatform">Platform of the SUT.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void SmbConnectionResponseHandler(
        Platform clientPlatform,
        Platform sutPlatform);

    /// <summary>
    /// Negotiate response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="isSignatureRequired">
    /// Indicate whether the NEGOTIATE_SECURITY_SIGNATURES_REQUIRED is set inSecurityMode field 
    /// of Negotiation Response.
    /// </param>
    /// <param name="isSignatureEnabled">
    /// Indicate whether the NEGOTIATE_SECURITY_SIGNATURES_ENABLED is set in
    /// SecurityMode field of Negotiation Response.
    /// </param>
    /// <param name="dialectIndex">
    /// The index of the SMB dialect, it was selected from the DialectName field which was passed in the 
    /// SMB_COM_NEGOTIATE client request.
    /// </param>
    /// <param name="serverCapabilities" >Indicate the capabilities that the server supports.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void NegotiateResponseHandler(
        int messageId,
        bool isSignatureRequired,
        bool isSignatureEnabled,
        int dialectIndex,
        [Domain("serverCapabilities")] Microsoft.Modeling.Set<Capabilities> serverCapabilities,
        MessageStatus messageStatus);

    /// <summary>
    /// Non Extended Security for negotiate response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="isSignatureRequired">
    /// Indicate whether the NEGOTIATE_SECURITY_SIGNATURES_REQUIRED is set inSecurityMode field 
    /// of Negotiation Response.
    /// </param>
    /// <param name="isSignatureEnabled">
    /// Indicate whether the NEGOTIATE_SECURITY_SIGNATURES_ENABLED is set in
    /// SecurityMode field of Negotiation Response.
    /// </param>
    /// <param name="dialectIndex">
    /// The index of the SMB dialect, it was selected from the DialectName field which was passed in the 
    /// SMB_COM_NEGOTIATE client request.
    /// </param>
    /// <param name="serverCapabilities" >Indicate the capabilities that the server supports.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void NonExtendedNegotiateResponseHandler(
        int messageId,
        bool isSignatureRequired,
        bool isSignatureEnabled,
        int dialectIndex,
        [Domain("serverCapabilitiesForNonextendedSecurity")] Microsoft.Modeling.Set<Capabilities> serverCapabilities,
        MessageStatus messageStatus);

    /// <summary>
    /// Non Extended Security for session setup response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a 
    /// previously established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="securitySignatureValue">
    /// Indicate the security signature used in session setup response header.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="isGuestAccount">Indicate whether the account is a guest account or an admin account.</param>
    /// <param name="isRS2322Implemented">If the RS2322 implemented, it is true, else it is false.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code is returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void NonExtendedSessionSetupResponseHandler(
        int messageId,
        int sessionId,
        int securitySignatureValue,
        bool isSigned,
        bool isGuestAccount,
        bool isRS2322Implemented,
        MessageStatus messageStatus);

    /// <summary>
    /// Session setup response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously
    /// established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="securitySignatureValue">
    /// Indicate the security signature used in session setup response header.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="isGuestAccount">Indicate whether the account is a guest account or an admin account.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void SessionSetupResponseHandler(
        int messageId,
        int sessionId,
        int securitySignatureValue,
        bool isSigned,
        bool isGuestAccount,
        MessageStatus messageStatus);

    /// <summary>
    /// Tree connect response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously
    /// established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this 
    /// document) on the server that the client is accessing.
    /// </param>
    /// <param name="isSecuritySignatureZero">Indicate whether the securitySignature is 0.</param>
    /// <param name="shareType">The type of resource the client intends to access.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="isInDfs">Indicate whether the share is managed by DFS.</param>
    /// <param name="isSupportExtSignature">
    /// One flag of OptionalSupport field. If set, the server is using signing 
    /// key protection as the client requested.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void TreeConnectResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSecuritySignatureZero,
        ShareType shareType,
        MessageStatus messageStatus,
        bool isSigned,
        bool isInDfs,
        bool isSupportExtSignature);

    /// <summary>
    /// Create response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously
    /// established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this 
    /// document) on the server that the client is accessing.
    /// </param>
    /// <param name="fid">The file identifier. </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="createAction">Create an action.</param>
    /// <param name="isFileIdZero">Indicate whether the fileId is 0.</param>
    /// <param name="isVolumeGuidZero">Indicate whether the volumeGUIDIsZero is 0.</param>
    /// <param name="isDirectoryZero">Indicate whether the Directory field is zero or not.</param>
    /// <param name="isByteCountZero">Indicate whether the byte count is zero or not.</param>
    /// <param name="isNoStream">
    /// Indicate whether NO_SUBSTREAMS bit in the FileStatusFlags field is set in the 
    /// SMB_COM_NT_CREATE_ANDX server response.
    /// </param>
    /// <param name="messageStatus"> Indicate the status code returned from server, success or fail.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void CreateResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        int fid,
        bool isSigned,
        [Domain("ActionTaken")] Microsoft.Modeling.Set<CreateAction> createAction,
        bool isFileIdZero,
        bool isVolumeGuidZero,
        bool isDirectoryZero,
        bool isByteCountZero,
        bool isNoStream,
        MessageStatus messageStatus);

    /// <summary>
    /// Error response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void ErrorResponseHandler(
        int messageId,
        MessageStatus messageStatus);

    /// <summary>
    /// Error tree connect response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// <param name="isRS357Implemented">If the RS357 implemented, it is true, else it is false.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void ErrorTreeConnectResponseHandler(
        int messageId,
        MessageStatus messageStatus,
        bool isRS357Implemented);

    /// <summary>
    /// Error TRANS2_QUERY_FS_INFORMATION response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// <param name="isRS2073Implemented">If the RS2073 implemented, it is true, else it is false.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void ErrorTrans2QueryFileInfoResponseHandler(
        int messageId,
        MessageStatus messageStatus,
        bool isRS2073Implemented);

    /// <summary>
    /// Error TRANS2_QUERY_PATH_INFORMATION response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// <param name="isRS2076Implemented">If the RS2076 implemented, it is true, else it is false.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void ErrorTrans2QueryPathInfoResponseHandler(
        int messageId,
        MessageStatus messageStatus,
        bool isRS2076Implemented);




    /// <summary>
    /// The interface of SMBAdapter.
    /// </summary>
    public partial interface ISmbAdapter : IAdapter
    {
        #region previous interface

        /// <summary>
        /// SMB connection response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event SmbConnectionResponseHandler SmbConnectionResponse;

        /// <summary>
        /// Negotiate response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event NegotiateResponseHandler NegotiateResponse;

        /// <summary>
        /// Non Extended Security for negotiate response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event NonExtendedNegotiateResponseHandler NonExtendedNegotiateResponse;

        /// <summary>
        /// Non Extended Security for session setup response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event NonExtendedSessionSetupResponseHandler NonExtendedSessionSetupResponse;

        /// <summary>
        /// Session setup response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event SessionSetupResponseHandler SessionSetupResponse;

        /// <summary>
        /// Tree connect response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event TreeConnectResponseHandler TreeConnectResponse;

        /// <summary>
        /// Create response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event CreateResponseHandler CreateResponse;

        /// <summary>
        /// Error response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event ErrorResponseHandler ErrorResponse;

        /// <summary>
        /// Error tree connect response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event ErrorTreeConnectResponseHandler ErrorTreeConnectResponse;

        /// <summary>
        /// ErrorTrans2QueryFileInfo response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event ErrorTrans2QueryFileInfoResponseHandler ErrorTrans2QueryFileInfoResponse;

        /// <summary>
        /// ErrorTrans2QueryPathInfo response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event ErrorTrans2QueryPathInfoResponseHandler ErrorTrans2QueryPathInfoResponse;



        /// <summary>
        /// SMB connection request.
        /// </summary>
        void SmbConnectionRequest();

        /// <summary>
        /// Negotiate request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="isSupportExtSecurity">This Indicate whether the client supports extended security.</param>
        /// <param name="clientSignState">
        /// Indicate the sign state of the client: Required, Enabled, Disabled or Disabled Unless Required.
        /// </param>
        /// <param name="dialectName">The input array of dialects.</param>
        void NegotiateRequest(
            int messageId,
            bool isSupportExtSecurity,
            SignState clientSignState,
            Sequence<Dialect> dialectName);

        /// <summary>
        /// Session setup request.
        /// </summary>
        /// <param name="account">Indicate the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.
        /// </param>
        /// <param name="isRequireSign">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="capabilities">A set of client capabilities.</param>
        /// <param name="isSendBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize sent by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="isWriteBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize written by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="flag2">Whether the Flag2 field of the SMB header is valid or not.</param>
        void SessionSetupRequest(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("clientCapabilities")] Microsoft.Modeling.Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize,
            bool flag2);

        /// <summary>
        /// Non Extended Security for session setup request.
        /// </summary>
        /// <param name="account">Indicate the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.
        /// </param>
        /// <param name="isRequireSign">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="capabilities">A set of client capabilities. </param>
        /// <param name="isSendBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize sent by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="isWriteBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize written by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="flag2">Whether the Flag2 field of the SMB header is valid or not.</param>
        void NonExtendedSessionSetupRequest(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("clientCapabilitiesForNonextendedSecurity")] Microsoft.Modeling.Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize,
            bool flag2);

        /// <summary>
        /// Session setup request with SMB_FLAGS2_UNICODE not set in Flags2.
        /// </summary>
        /// <param name="account">Indicate the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request re-authenticatean existing session.
        /// </param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.</param>
        /// <param name="isRequireSign">Indicate whether the message signing is required.</param>
        /// <param name="capabilities">A set of client capabilities. </param>
        /// <param name="isSendBufferSizeExceedMaxBufferSize">
        /// Indicate whether the maximum buffer size for sending can exceed the MaxBufferSize field.
        /// </param>
        /// <param name="isWriteBufferSizeExceedMaxBufferSize">
        /// Indicate whether the maximum buffer size for writing can exceed the MaxBufferSize field.
        /// </param>
        /// <param name="flag2"> This value is ignored by the server and it is used for traditional test.</param>
        void SessionSetupNonUnicodeRequest(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("clientCapabilities")] Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize,
            bool flag2);

        /// <summary>
        /// Close session request
        /// </summary>
        /// <param name="sessionId"> The session Id which the session is being closed.</param>
        void SessionClose(int sessionId);

        /// <summary>
        /// Tree connect request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">Session id. </param>
        /// <param name="isTidDisconnectionSet">Indicate whether the client sets the tid disconnection.</param>
        /// <param name="isRequestExtSignature">Indicate whether the client requests extended signature.</param>
        /// <param name="isRequestExtResponse">
        /// Indicate whether the client requests extended information on Tree connection response.
        /// </param>
        /// <param name="share">The share method.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSigned" >
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        void TreeConnectRequest(
            int messageId,
            int sessionId,
            bool isTidDisconnectionSet,
            bool isRequestExtSignature,
            bool isRequestExtResponse,
            string share,
            ShareType shareType,
            bool isSigned);

        /// <summary>
        /// Tree multiple connect request
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="isTidDisconnectionSet">Indicate whether the client sets the tid disconnection.</param>
        /// <param name="isRequestExtSignature">Indicate whether the client requests the extended signature.</param>
        /// <param name="isRequestExtResponse">
        /// Indicate whether the client requests the extended information on Tree connection response.
        /// </param>
        /// <param name="share">The share method.</param>
        /// <param name="shareType">The share type client intends to access.</param>
        /// <param name="isSigned">Indicate whether the message is signed or not for this request.</param>
        void TreeMultipleConnectRequest(
            int messageId,
            int sessionId,
            bool isTidDisconnectionSet,
            bool isRequestExtSignature,
            bool isRequestExtResponse,
            string share,
            ShareType shareType,
            bool isSigned);

        /// <summary>
        /// Create request
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">Set this value to 0 to request a new session setup, or set this value to a 
        /// previously established session identifier to reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="desiredAccess">
        /// The client wants to have access to the SUT. This value must be specified in the ACCESS_MASK 
        /// format.
        /// </param>
        /// <param name="createDisposition">The action to take if a file does or does not exist.</param>
        /// <param name="impersonationLevel">
        /// This field specifies the information given to the server about the client and how the server MUST 
        /// represent, or impersonate, the client.
        /// </param>
        /// <param name="name">File name.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isOpenByFileId">
        /// Indicate whether the FILE_OPEN_BY_FILE_ID is set in CreateOptions field of Create Request.
        /// </param>
        /// <param name="isDirectoryFile">
        /// Indicate whether the FILE_DIRECTORY_FILE and FILE_NON_DIRECTORY_FILE are set. If true,FILE_DIRECTORY_FILE 
        /// is set; else FILE_NON_DIRECTORY_FILE is set.
        /// </param>
        /// <param name="isMaximumAllowedSet">Whether the maximum allowed value is set.</param>
        void CreateRequest(
            int messageId,
            int sessionId,
            int treeId,
            [Domain("DesiredAccess")] int desiredAccess,
            CreateDisposition createDisposition,
            [Domain("ImpersonationLevel")] int impersonationLevel,
            [Domain("FileDomain")] string name,
            ShareType shareType,
            bool isSigned,
            bool isOpenByFileId,
            bool isDirectoryFile,
            bool isMaximumAllowedSet);

        #endregion

        #region Added interface

        /// <summary>
        /// Read response handle.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event ReadResponseHandle ReadResponse;

        /// <summary>
        /// Write response handle.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event WriteResponseHandle WriteResponse;

        /// <summary>
        /// Error write response handle.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event ErrorWriteResponseHandle ErrorWriteResponse;

        /// <summary>
        /// IsRS2299Implemented response handle.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event IsRs2299ImplementedResponseHandle IsRs2299ImplementedResponse;

        /// <summary>
        /// IsRS4984Implemented response handle.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event IsRs4984ImplementedResponseHandle IsRs4984ImplementedResponse;

        /// <summary>
        /// Session setup response additional handle.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event SessionSetupResponseAdditionalHandle SessionSetupResponseAdditional;

        /// <summary>
        /// IsRS2299Implemented request.
        /// </summary>
        void IsRs2299ImplementedRequest();

        /// <summary>
        /// IsRS4984Implemented request.
        /// </summary>
        void IsRs4984ImplementedRequest();

        /// <summary>
        /// Read request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="treeId">The tree identifier.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="shareType">The type of share.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        void ReadRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            ShareType shareType,
            bool isSigned);

        /// <summary>
        /// Write request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="treeId">The tree identifier.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="shareType">The type of share.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="synchronize">The synchronize method.</param>
        void WriteRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            ShareType shareType,
            bool isSigned,
            int synchronize);

        /// <summary>
        /// Session setup request additional.
        /// </summary>
        /// <param name="account">Indicate the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.
        /// </param>
        /// <param name="isRequireSign">
        /// Indicate whether the server has message signing enabled or required.
        /// </param>
        /// <param name="capabilities">A set of client capabilities.</param>
        /// <param name="isSendBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize sent by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="isWriteBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize written by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="flag2">Whether the Flag2 field of the SMB header is valid or not.</param>
        /// <param name="isGssTokenValid">Whether the GSS token in valid or not.</param>
        /// <param name="isUserIdValid">Whether the user ID is valid or not.</param>
        void SessionSetupRequestAdditional(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("clientCapabilities")] Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize,
            bool flag2,
            bool isGssTokenValid,
            bool isUserIdValid);

        /// <summary>
        /// FSCTL Bad command request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">Indicate whether the message is signed or not for this response.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="command">This is used to tell the adapter to send an invalid kind of command.</param>
        void FsctlBadCommandRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            FsctlInvalidCommand command);

        #endregion


    }
}
