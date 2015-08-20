// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// Init State.
    /// </summary>
    [InitialMode("SmbState.Init")]
    public static partial class BaseModelProgram
    {
        /// <summary>
        /// It indicates the SMB state and initializes its state.
        /// </summary>
        private static SmbState smbState = SmbState.Init;

        /// <summary>
        /// The connection that used for SMB exchanges.
        /// </summary>
        private static SmbConnection smbConnection = new SmbConnection();

        /// <summary>
        /// SMB request.
        /// </summary>
        private static SmbRequest smbRequest;

        /// <summary>
        /// It indicates whether the maximum System Under Test (the SUT) buffer size of bytes for sendind exceeds the 
        /// MaxBufferSize field.
        /// </summary>
        private static bool isSendBufferSizeExceedMaxBufferSize;

        /// <summary>
        /// It indicates whether the maximum the SUT buffer size of bytes for writing exceeds the MaxBufferSize field.
        /// </summary>
        private static bool isWriteBufferSizeExceedMaxBufferSize;

        /// <summary>
        /// It indicates the created access of a file, including ReadOnly, WriteOnly, ReadWrite
        /// </summary>
        private static int createFileAccess;

        /// <summary>
        /// To store the client value of maximum allowed operation.
        /// </summary>
        private static bool isClientMaxAllowedSet;

        #region Accepting state

        /// <summary>
        /// The accepting state condition.
        /// </summary>
        /// <returns>
        /// Accepting Condition: 
        /// 1. After session setup established;
        /// 2. Session be closed; 
        /// 3. Tree connection is established and all the requests have received their assotiated responses.
        /// </returns>
        [AcceptingStateCondition]
        public static bool AcceptingCondition()
        {
            return ((smbState == SmbState.SessionSetupSuccess)
                        || (smbState == SmbState.Closed)
                        || (smbState == SmbState.TreeConnectSuccess && smbConnection.sentRequest.Count == 0));
        }

        #endregion

        #region Initialize the SMB transport connection.

        /// <summary>
        /// Initialize the SMB underlying transport connection.
        /// </summary>
        [Rule(Action = "SmbConnectionRequest()")]
        public static void SmbConnectionRequest()
        {
            Condition.IsTrue(smbState == SmbState.Init);
            smbState = SmbState.ConnectionRequest;
        }


        /// <summary>
        /// SMB connection response.
        /// </summary>
        /// <param name="clientPlatform">Platform of client.</param>
        /// <param name="sutPlatform">Platform of the SUT.</param>
        [Rule]
        public static void SmbConnectionResponse(Platform clientPlatform, Platform sutPlatform)
        {
            Condition.IsTrue(smbState == SmbState.ConnectionRequest);
            Condition.IsTrue(clientPlatform == Platform.Win2K
                                || clientPlatform == Platform.WinXP
                                || clientPlatform == Platform.WinVista
                                || clientPlatform == Platform.NonWindows
                                || clientPlatform == Platform.Win7);
            Condition.IsTrue(sutPlatform == Platform.WinNt
                                || sutPlatform == Platform.Win2K3
                                || sutPlatform == Platform.Win2K8
                                || sutPlatform == Platform.NonWindows
                                || sutPlatform == Platform.Win2K8R2);
            Parameter.clientPlatform = clientPlatform;
            Parameter.sutPlatform = sutPlatform;
            smbState = SmbState.ConnectionSuccess;
        }

        #endregion

        #region server Setup Configuration

        /// <summary>
        /// SUT setup.
        /// </summary>
        /// <param name="fsType">The file system type of the SUT.</param>
        /// <param name="sutSignState">It indicates what the message signing policy of the SUT.</param>
        /// <param name="isSupportDfs">It indicates whether the SUT supports DFS.</param>
        /// <param name="isSupportPreviousVersion">
        /// It indicates whether the SUT will support the previous version.
        /// </param>
        /// <param name="isMessageModePipe">
        /// It indicates the adapter to setup a message mode pipe or byte mode pipe.
        /// </param>
        [Rule]
        public static void ServerSetup(
            FileSystemType fsType,
            SignState sutSignState,
            bool isSupportDfs,
            bool isSupportPreviousVersion,
            bool isMessageModePipe)
        {
            Condition.IsTrue(smbState == SmbState.ConnectionSuccess);

            if (Parameter.sutPlatform != Platform.NonWindows)
            {
                if (Parameter.sutPlatform == Platform.Win2K3)
                {
                    // It is not possible to disable the DFS capability from the WS03 the SUT.
                    Condition.IsTrue(isSupportDfs);
                }
            }

            if (Parameter.sutPlatform != Platform.NonWindows)
            {
                if (fsType == FileSystemType.Fat)
                {
                    Parameter.isSupportQuota = false;
                    // In Windows, the Generic file system (FAT) does not support File IDs.
                    Parameter.isSupportUniqueFileId = false;
                    // FAT does not support streams.
                    Parameter.isSupportStream = false;
                    // FAT does not support the previous version.
                    Condition.IsTrue(!isSupportPreviousVersion);

                    Requirement.AssumeCaptured("Server doesn't support stream");
                }
                else
                {
                    Parameter.isSupportQuota = true;
                    // In Windows, the Generic file system (NTFS) file system supports File IDs.
                    Parameter.isSupportUniqueFileId = true;
                    // NTFS supports streams.
                    Parameter.isSupportStream = true;
                    // NTFS supports the previous version.
                    Condition.IsTrue(isSupportPreviousVersion);
                    Requirement.AssumeCaptured("Server doesn't support stream");
                }
            }

            Parameter.isSupportPreviousVersion = isSupportPreviousVersion;
            Parameter.fsType = fsType;
            Parameter.isSupportDfs = isSupportDfs;
            Parameter.sutSignState = sutSignState;
            Parameter.isMessageModePipe = isMessageModePipe;
            smbState = SmbState.ServerSetupRequest;
        }


        /// <summary>
        /// SUT Setup Response.
        /// </summary>
        /// <param name="totalBytesWritten">The totall bytes can be written to a file.</param>
        /// <param name="isSupportInfoLevelPassthrough">It indicates whether the SUT supports Info Passthrough.</param>
        /// <param name="isSupportNtSmb">It indicates whther the SUT supports NT SMBs.</param>
        /// <param name="isRapServerActive">It indicates whether the RAP server is available on the SUT machine.</param>
        /// <param name="isSupportResumeKey">It indicates whether the SUT will support resume key.</param>
        /// <param name="isSupportCopyChunk">It indicates whether the SUT will support CopyChunk.</param>
        [Rule]
        public static void ServerSetupResponse(
            int totalBytesWritten,
            bool isSupportInfoLevelPassthrough,
            bool isSupportNtSmb,
            bool isRapServerActive,
            bool isSupportResumeKey,
            bool isSupportCopyChunk)
        {
            Condition.IsTrue(smbState == SmbState.ServerSetupRequest);

            // The model abstracts the totalBytesWritten as 1.
            // And when request copy chunk, totalBytesWritten is N*1 (N is an integer >= 1).
            Condition.IsTrue(totalBytesWritten == 1);

            if (Parameter.sutPlatform == Platform.Win2K3
                    || Parameter.sutPlatform == Platform.Win2K8
                    || Parameter.sutPlatform == Platform.Win2K8R2)
            {
                Condition.IsTrue(isSupportInfoLevelPassthrough
                                    && isSupportNtSmb
                                    && isRapServerActive
                                    && isSupportResumeKey
                                    && isSupportCopyChunk);
            }

            Parameter.totalBytesWritten = totalBytesWritten;
            Parameter.isSupportInfoLevelPassThrough = isSupportInfoLevelPassthrough;
            if (!isSupportNtSmb)
            {
                Requirement.AssumeCaptured("Server doesn't support NT SMBs");
            }
            else
            {
                Requirement.AssumeCaptured("Server supports NT SMBs");
            }

            Parameter.isSupportNtSmb = isSupportNtSmb;
            Parameter.isRapServerActive = isRapServerActive;
            Parameter.isSupportResumeKey = isSupportResumeKey;
            Parameter.isSupportCopyChunk = isSupportCopyChunk;
            smbState = SmbState.ServerSetupSuccess;
        }

        #endregion

        #region Create pipes and mailslot

        /// <summary>
        /// Create the named pipe and mailslot. 
        /// Make sure the named pipe and mailslot are already available before using.
        /// </summary>
        /// <param name="pipes">All the pipes will be created on SMB the SUT.</param>
        /// <param name="mailslot">All the mailslots will be created on SMB the SUT.</param>
        /// <param name="isCreatePipeStatus">
        /// It indicates the status of create named pipe and mailslot operation.
        /// </param>
        [Rule]
        public static void CreatePipeAndMailslot(Set<string> pipes, Set<string> mailslot, out bool isCreatePipeStatus)
        {
            Condition.IsTrue(smbState == SmbState.ServerSetupSuccess);
            Condition.IsTrue(pipes == PipeNames() && mailslot == MailslotNames());
            isCreatePipeStatus = true;
            smbState = SmbState.CreateNamePipeAndMailslotSucceed;
        }

        #endregion

        #region Establish Connection

        #region Negotiation

        /// <summary>
        /// SMB_COM_NEGOTIATE request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="isSupportExtSecurity">This indicates whether the client supports extended security.</param>
        /// <param name="dialectName">The input array of dialects.</param>
        /// <param name="clientSignState">
        /// It indicates the sign state of the client: Required, Enabled, Disabled or Disabled Unless Required.
        /// </param>
        [Rule]
        public static void NegotiateRequest(
            int messageId,
            bool isSupportExtSecurity,
            SignState clientSignState,
            Sequence<Dialect> dialectName)
        {
            Checker.CheckNegotiateRequest(smbConnection, messageId, smbState, clientSignState, dialectName);
            smbRequest = new NegotiateRequest(messageId, isSupportExtSecurity, clientSignState, dialectName);
            Update.UpdateNegotiateRequest(smbConnection, smbRequest);
            smbState = SmbState.NegotiateSent;
        }


        /// <summary>
        /// SMB_COM_NEGOTIATE response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="isSignatureRequired">
        /// It indicates whether the NEGOTIATE_SECURITY_SIGNATURES_REQUIRED is set in SecurityMode field of 
        /// SMB_COM_NEGOTIATE Response.
        /// </param>
        /// <param name="isSignatureEnabled">
        /// It indicates whether the NEGOTIATE_SECURITY_SIGNATURES_ENABLED is set in SecurityMode field of 
        /// SMB_COM_NEGOTIATE Response.
        /// </param> 
        /// <param name="dialectIndex">
        /// The index of the SMB dialect was selected from the DialectName field that was passed in the 
        /// SMB_COM_NEGOTIATE client request.
        /// </param>
        /// <param name="sutCapabilities">It indicates the capabilities the SUT supports.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void NegotiateResponse(
            int messageId,
            bool isSignatureRequired,
            bool isSignatureEnabled,
            int dialectIndex,
            [Domain("SutCapabilities")] Set<Capabilities> sutCapabilities,
            MessageStatus messageStatus)
        {
            Checker.CheckNegotiateResponse(
                smbConnection,
                messageId,
                smbState,
                messageStatus,
                MessageStatus.Success);
            Condition.IsTrue(
                !((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.Contains(Dialect.Invalid));
            Condition.IsTrue(messageStatus == MessageStatus.Success);
            Condition.IsTrue(!sutCapabilities.Contains(Capabilities.None));
            Condition.IsTrue(
                (dialectIndex >= 0)
                    && (dialectIndex < ((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.Count));
            Condition.IsTrue(
                dialectIndex
                    == ((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.IndexOf(Dialect.NtLanMan));

            if (smbConnection.isClientSupportExtSecurity)
            {
                if (Parameter.sutPlatform != Platform.NonWindows)
                {
                    if (Parameter.sutPlatform == Platform.Win2K
                            || Parameter.sutPlatform == Platform.Win2K3
                            || Parameter.sutPlatform == Platform.Win2K8
                            || Parameter.sutPlatform == Platform.Win2K8R2)
                    {
                        Condition.IsTrue(sutCapabilities.Contains(Capabilities.CapExtendedSecurity));
                    }
                }
            }
            else if (!smbConnection.isClientSupportExtSecurity)
            {
                Condition.IsTrue(!sutCapabilities.Contains(Capabilities.CapExtendedSecurity));
            }

            if (Parameter.isSupportDfs)
            {
                Condition.IsTrue(sutCapabilities.Contains(Capabilities.CapDfs));
            }
            else
            {
                Condition.IsTrue(!sutCapabilities.Contains(Capabilities.CapDfs));
            }

            switch (Parameter.sutSignState)
            {
                case SignState.Required:
                    Condition.IsTrue(isSignatureRequired && isSignatureEnabled);
                    break;
                case SignState.Enabled:
                    Condition.IsTrue(!isSignatureRequired && isSignatureEnabled);
                    break;
                case SignState.Disabled:
                case SignState.DisabledUnlessRequired:
                    Condition.IsTrue(!isSignatureRequired && !isSignatureEnabled);
                    break;
                default:
                    break;
            }

            // If the result is Blocked (Client:Required & Server:Disabled, Client:Disabled & Server:Required), the
            // underlying transport connection must be closed.
            if (((smbConnection.clientSignState == SignState.Disabled)
                    && (Parameter.sutSignState == SignState.Required))
                || ((smbConnection.clientSignState == SignState.Required)
                    && (Parameter.sutSignState == SignState.Disabled)))
            {
                smbState = SmbState.Closed;
            }
            else
            {
                smbState = SmbState.NegotiateSuccess;
            }

            if (Parameter.sutPlatform != Platform.NonWindows)
            {
                if (smbConnection.isClientSupportExtSecurity
                        && sutCapabilities.Contains(Capabilities.CapExtendedSecurity))
                {
                    ModelHelper.CaptureRequirement(
                        2308,
                        "[In Receiving an SMB_COM_NEGOTIATE Request] Generating Extended Security Token: If the client " +
                        "indicated support for extended security by setting SMB_FLAGS2_EXTENDED_SECURITY in the Flags2 " +
                        "field of the SMB header of the SMB_COM_NEGOTIATE request, then the SUT sets " +
                        "CAP_EXTENDED_SECURITY in the SMB_COM_NEGOTIATE response in windows if it supports extended " +
                        "security.");
                }
            }

            // The client has received the response from the SUT.
            ModelHelper.CaptureRequirement(
                4747,
                "[In Receiving an SMB_COM_NEGOTIATE Request] Generating Extended Security Token: The SMB_COM_NEGOTIATE " +
                "response packet is sent to the client.<113>");

            Update.UpdateNegotiateResponse(smbConnection, messageId, Parameter.sutSignState, sutCapabilities);
        }


        /// <summary>
        /// SMB_COM_NEGOTIATE response for non-extended security mode.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="isSignatureRequired">
        /// It indicates whether the NEGOTIATE_SECURITY_SIGNATURES_REQUIRED is set in SecurityMode field of 
        /// SMB_COM_NEGOTIATE Response.
        /// </param>
        /// <param name="isSignatureEnabled">
        /// It indicates whether the NEGOTIATE_SECURITY_SIGNATURES_ENABLED is set in SecurityMode field of 
        /// SMB_COM_NEGOTIATE Response.
        /// </param> 
        /// <param name="dialectIndex">
        /// The index of the SMB dialect was selected from the DialectName field that was passed in the 
        /// SMB_COM_NEGOTIATE client request.
        /// </param>
        /// <param name="serverCapabilities">It indicates the capabilities the SUT supports.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void NonExtendedNegotiateResponse(
            int messageId,
            bool isSignatureRequired,
            bool isSignatureEnabled,
            int dialectIndex,
            [Domain("SutCapabilitiesForNonextendedSecurity ")] Set<Capabilities> serverCapabilities,
            MessageStatus messageStatus)
        {
            Condition.IsTrue(!serverCapabilities.Contains(Capabilities.CapExtendedSecurity));

            // The adapter will read the response according to [CIFS] section 4.1.1.
            Checker.CheckNegotiateResponse(
                smbConnection,
                messageId,
                smbState,
                messageStatus,
                MessageStatus.Success);

            if (!((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.Contains(Dialect.Invalid))
            {
                Condition.IsTrue(messageStatus == MessageStatus.Success);
                Condition.IsTrue(!serverCapabilities.Contains(Capabilities.None));

                Condition.IsTrue(
                    !((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.Contains(Dialect.Invalid));
                Condition.IsTrue(
                    (dialectIndex >= 0)
                        && (dialectIndex < ((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.Count));
                Condition.IsTrue(
                    ((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.IndexOf(Dialect.NtLanMan)
                        == dialectIndex);

                if (Parameter.isSupportDfs)
                {
                    Condition.IsTrue(serverCapabilities.Contains(Capabilities.CapDfs));
                }
                else
                {
                    Condition.IsTrue(!serverCapabilities.Contains(Capabilities.CapDfs));
                }

                switch (Parameter.sutSignState)
                {
                    case SignState.Required:
                        Condition.IsTrue(isSignatureRequired && isSignatureEnabled);
                        break;
                    case SignState.Enabled:
                        Condition.IsTrue(!isSignatureRequired && isSignatureEnabled);
                        break;
                    case SignState.Disabled:
                    case SignState.DisabledUnlessRequired:
                        Condition.IsTrue(!isSignatureRequired && !isSignatureEnabled);
                        break;
                    default:
                        break;
                }

                // If the result is Blocked (Client:Required&Server:Disabled Client:Disabled&Server:Required), 
                // the underlying transport connection must be closed.
                if (((smbConnection.clientSignState == SignState.Disabled)
                        && (Parameter.sutSignState == SignState.Required))
                    || ((smbConnection.clientSignState == SignState.Required)
                        && (Parameter.sutSignState == SignState.Disabled)))
                {
                    smbState = SmbState.Closed;
                }
                else
                {
                    smbState = SmbState.NegotiateSuccess;
                }
            }
            else if (((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.Contains(Dialect.Invalid))
            {
                // The message status equals NOT_SUPPORTED means an error happens.
                Condition.IsTrue(messageStatus == MessageStatus.Success);
                Condition.IsTrue(serverCapabilities.Contains(Capabilities.None)
                                    && !isSignatureEnabled
                                    && !isSignatureRequired);
                Condition.IsTrue(dialectIndex == -1);
                smbState = SmbState.Closed;
            }
            Update.UpdateNegotiateResponse(smbConnection, messageId, Parameter.sutSignState, serverCapabilities);
        }


        /// <summary>
        /// SMB_COM_NEGOTIATE Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorNegotiateResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.ErrorGenFailure);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.SmbComNegotiate);

            if (((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.Contains(Dialect.LanMan21))
            {
                ModelHelper.CaptureRequirement(
                    8579,
                    "[In Sending Any Error Response Message] <110> Section 3.3.4.1.1: If the negotiated dialect " +
                    "is DOS LANMAN 2.1, an ERROR_GEN_FAILURE error is returned.");
            }

            if (((NegotiateRequest)smbConnection.sentRequest[messageId]).dialectNames.Contains(Dialect.LanMan10))
            {
                ModelHelper.CaptureRequirement(
                    8580,
                    "[In Sending Any Error Response Message] <110> Section 3.3.4.1.1: If the negotiated dialect is " +
                    "prior to LANMAN 1.0, an ERROR_GEN_FAILURE error is returned.");
            }

            smbState = SmbState.Closed;
            smbConnection.sentRequest.Remove(messageId);
        }
        #endregion


        #region Session setup
        /// <summary>
        /// SMB_COM_SESSION_SETUP request.
        /// </summary>
        /// <param name="account">It indicates the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.
        /// </param>
        /// <param name="isRequireSign">It indicates whether the message signing is required.</param>
        /// <param name="capabilities">A set of client capabilities.</param>
        /// <param name="isSendBufferSizeExceeds">
        /// It indicates whether the maximum the SUT buffer size of bytes for sending exceeds the MaxBufferSize field.
        /// </param>
        /// <param name="isWriteBufferSizeExceeds">
        /// It indicates whether the maximum the SUT buffer size of bytes for writing exceeds the MaxBufferSize field.
        /// </param>
        /// <param name="flag2">This value is ignored by the SUT, and it is used for traditional test.</param>
        [Rule]
        public static void SessionSetupRequest(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("ClientCapabilities")] Set<Capabilities> capabilities,
            bool isSendBufferSizeExceeds,
            bool isWriteBufferSizeExceeds,
            bool flag2)
        {
            Condition.IsTrue(flag2);
            Checker.CheckSessionSetupRequest(
                smbConnection,
                messageId,
                sessionId,
                smbState,
                isRequireSign,
                capabilities,
                isSendBufferSizeExceeds,
                isWriteBufferSizeExceeds);
            if (capabilities.Contains(Capabilities.CapExtendedSecurity))
            {
                ModelHelper.CaptureRequirement(
                    8388,
                    "[In Sequence Diagram] Session Setup Roundtrip: If the CAP_EXTENDED_SECURITY bit is set " +
                    "(0x80000000), then the SMB the SUT does support extended security.");
            }

            smbRequest = new SessionSetupRequest(
                account,
                messageId,
                sessionId,
                securitySignature,
                isRequireSign,
                capabilities);

            Update.UpdateSessionSetupRequest(smbConnection, smbRequest);
            smbState = SmbState.SessionSetupSent;
        }


        /// <summary>
        /// Session setup request with SMB_FLAGS2_UNICODE not set in Flags2.
        /// </summary>
        /// <param name="account"> Indicate the account type to establish the session.</param>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> Set this value to 0 to request a new session setup, 
        /// or set this value to a previously established session identifier 
        /// to reauthenticate to an existing session. </param>
        /// <param name="securitySignature"> Delegate the security signature used in session setup request header.</param>
        /// <param name="isRequireSign"> Indicate whether the message signing is required.</param>
        /// <param name="capabilities"> A set of client capabilities. </param>
        /// <param name="isSendBufferSizeExceedMaxBufferSize"> Indicate whether the maximum buffer size 
        /// for sending can exceed the MaxBufferSize field.</param>
        /// <param name="isWriteBufferSizeExceedMaxBufferSize"> Indicate whether the maximum buffer size 
        /// for writing can exceed the MaxBufferSize field.</param>
        /// <param name="flag2"> This value is ignored by the server and it is used for traditional test.</param>
        [Rule]
        public static void SessionSetupNonUnicodeRequest(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("ClientCapabilities")] Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize,
            bool flag2)
        {
            Condition.IsTrue(flag2);
            Checker.CheckSessionSetupRequest(
                smbConnection,
                messageId,
                sessionId,
                smbState,
                isRequireSign,
                capabilities,
                isSendBufferSizeExceedMaxBufferSize,
                isWriteBufferSizeExceedMaxBufferSize);

            if (capabilities.Contains(Capabilities.CapExtendedSecurity))
            {
                ModelHelper.CaptureRequirement(
                    8388,
                    "[In Sequence Diagram] Session Setup Roundtrip: If the CAP_EXTENDED_SECURITY bit is set " +
                    "(0x80000000), then the SMB server does support extended security.");
            }
            smbRequest = new SessionSetupRequest(
                account,
                messageId,
                sessionId,
                securitySignature,
                isRequireSign,
                capabilities);
            Update.UpdateSessionSetupRequest(smbConnection, smbRequest);
            smbState = SmbState.SessionSetupSent;
        }

        /// <summary>
        /// SMB_COM_SESSION_SETUP for non-extended security mode.
        /// </summary>
        /// <param name="account">It indicates the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.
        /// </param>
        /// <param name="isRequireSign">It indicates whether the message signing is required.</param>
        /// <param name="capabilities">A set of client capabilities.</param>
        /// <param name="isSendBufferSizeExceeds">
        /// It indicates whether the maximum the SUT buffer size of bytes for sending exceeds the MaxBufferSize field.
        /// </param>
        /// <param name="isWriteBufferSizeExceeds">
        /// It indicates whether the maximum the SUT buffer size of bytes for writing exceeds the MaxBufferSize field.
        /// </param>
        /// <param name="flag2">This value is ignored by the SUT, and it is used for traditional test.</param>
        [Rule]
        public static void NonExtendedSessionSetupRequest(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("ClientCapabilitiesForNonextendedSecurity")] Set<Capabilities> capabilities,
            bool isSendBufferSizeExceeds,
            bool isWriteBufferSizeExceeds,
            bool flag2)
        {
            Condition.IsTrue(flag2);
            Checker.NonExtendedCheckSessionSetupRequest(
                smbConnection,
                messageId,
                sessionId,
                smbState,
                isRequireSign,
                capabilities,
                isSendBufferSizeExceeds,
                isWriteBufferSizeExceeds);

            if (!capabilities.Contains(Capabilities.CapExtendedSecurity))
            {
                ModelHelper.CaptureRequirement(
                    8385,
                    "[In Sequence Diagram]Session Setup Roundtrip: If the CAP_EXTENDED_SECURITY bit is clear " +
                    "(0x00000000), then the SMB the SUT does not support extended security. ");
            }

            BaseModelProgram.isSendBufferSizeExceedMaxBufferSize = isSendBufferSizeExceeds;
            BaseModelProgram.isWriteBufferSizeExceedMaxBufferSize = isWriteBufferSizeExceeds;
            smbRequest = new NonExtendedSessionSetupRequest(
                account,
                messageId,
                sessionId,
                securitySignature,
                isRequireSign,
                capabilities);

            Update.UpdateNonExtSessionSetupRequest(smbConnection, smbRequest);
            smbState = SmbState.SessionSetupSent;
        }


        /// <summary>
        /// SMB_COM_SESSION_SETUP response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="securitySignatureValue">
        /// It indicates the security signature used in session setup response header.
        /// </param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="isGuestAccount">
        /// It indicates whether the account is a guest account or an administer account.
        /// </param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void SessionSetupResponse(
            int messageId,
            int sessionId,
            int securitySignatureValue,
            bool isSigned,
            bool isGuestAccount,
            MessageStatus messageStatus)
        {
            Checker.CheckSessionSetupResponse(
                smbConnection,
                smbState,
                messageId,
                sessionId,
                securitySignatureValue,
                isSigned,
                isGuestAccount,
                messageStatus,
                MessageStatus.Success);

            if (smbConnection.sentRequest[messageId].command == Command.SmbComSessionSetup)
            {
                ModelHelper.CaptureRequirement(
                    8380,
                    "[In Sequence Diagram] The server MUST respond to each client request that it receives.");
                smbState = SmbState.SessionSetupSuccess;
            }

            // Authentication is completed.
            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    4143,
                    @"[In Sequence Diagram]The exchange of security tokens MUST continue until either the client or the
                    server determines that authentication has failed or both sides decide that authentication is 
                    complete.");
                ModelHelper.CaptureRequirement(
                    4784,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: If the GSS mechanism 
                    indicates that the current output token is the last output token of the authentication exchange 
                    based on the return code, as specified in [RFC2743]: Then the Status field in the SMB header of the
                    response MUST be set to STATUS_SUCCESS.");
                ModelHelper.CaptureRequirement(
                    2193,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Response]NTLM Authentication: The connection MUST 
                    remain open for the client to attempt another authentication.");
            }

            if (smbConnection.clientCapabilities.Contains(Capabilities.CapExtendedSecurity))
            {
                ModelHelper.CaptureRequirement(
                    8390,
                    "[In Sequence Diagram] Session Setup Roundtrip: The request is sent to the SMB server, and the " +
                    "server builds an extended SMB_COM_SESSION_SETUP_ANDX response. (section 2.2.4.6.2.). ");
            }

            Update.UpdateSessionSetupResponse(smbConnection, messageId, sessionId);

            if (smbConnection.isSigningActive)
            {
                ModelHelper.CaptureRequirement(
                    2341,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Signing Initialization: Otherwise[if opposite
                    to the condition: IsSigningActive is FALSE, and the response of the SMB_COM_SESSION_SETUP_ANDX 
                    operation contains STATUS_SUCCESS], server.Connection.IsSigningActive MUST be set to TRUE.");
            }
        }


        /// <summary>
        /// SMB_COM_SESSION_SETUP response for non-extended security mode.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="securitySignatureValue">
        /// It indicates the security signature used in session setup response header.
        /// </param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="isGuestAccount">
        /// It indicates whether the account is a guest account or an administer account.
        /// </param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isRs2322Implemented">It indicates whether RS2322 is implemented.</param>
        /// Disable warning CA1801 isRs2322Implemented is used in Adapter.
        [Rule]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static void NonExtendedSessionSetupResponse(
            int messageId,
            int sessionId,
            int securitySignatureValue,
            bool isSigned,
            bool isGuestAccount,
            bool isRs2322Implemented,
            MessageStatus messageStatus)
        {
            Checker.CheckNonExtendedSessionSetupResponse(
                smbConnection,
                smbState,
                messageId,
                sessionId,
                securitySignatureValue,
                isSigned,
                isGuestAccount,
                messageStatus,
                MessageStatus.Success);

            if (!smbConnection.clientCapabilities.Contains(Capabilities.CapExtendedSecurity))
            {
                ModelHelper.CaptureRequirement(
                    2322,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: Otherwise[if 
                        CAP_EXTENDED_SECURITY is not set in ClientCapabilities], it[server] MUST continue to the  
                        following NTLM authentication section.");
            }

            if (smbConnection.sentRequest[messageId].command == Command.SmbComSessionSetup)
            {
                ModelHelper.CaptureRequirement(
                    8380,
                    "[In Sequence Diagram] The server MUST respond to each client request that it receives.");
                smbState = SmbState.SessionSetupSuccess;
            }

            Update.UpdateNonExtSessionSetupResponse(smbConnection, messageId, sessionId);
        }


        /// <summary>
        /// SMB_COM_SESSION_SETUP Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorSessionSetupResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.NetworkSessionExpired);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.SmbComSessionSetup);

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                ModelHelper.CaptureRequirement(
                    4732,
                    @"[In Receiving Any Message]Session Validation and Re-authentication: [If 
                    Connection.SessionTable[UID].AuthenticationState is equal to Expired] Otherwise[if a session renewal
                    is indicated], the server MUST fail this operation with STATUS_NETWORK_SESSION_EXPIRED.");
                ModelHelper.CaptureRequirement(
                    4765,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Determine Reauth or Continuation of Previous
                    Auth: The server MUST prevent any further operations from executing on this session until 
                    authentication is complete, and fail them with STATUS_NETWORK_SESSION_EXPIRED.");
            }
            smbState = SmbState.Closed;
            smbConnection.sentRequest.Remove(messageId);
        }


        /// <summary>
        /// SMB_COM_SESSION_SETUP request.
        /// </summary>
        /// <param name="accountType">It indicates the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.
        /// </param>
        /// <param name="isRequireSign">It indicates whether the message signing is required.</param>
        /// <param name="capabilities">A set of client capabilities.</param>
        /// <param name="isSendBufferSizeExceeds">
        /// It indicates whether the maximum the SUT buffer size of bytes for sending exceeds the MaxBufferSize field.
        /// </param>
        /// <param name="isWriteBufferSizeExceeds">
        /// It indicates whether the maximum the SUT buffer size of bytes for writing exceeds the MaxBufferSize field.
        /// </param>
        /// <param name="flag2">This value is ignored by the SUT, and it is used for traditional test.</param>
        /// <param name="isGSSTokenValid">It indicates whether the GSSToken is valid.</param>
        /// <param name="isUserIdValid">It indicates whether the user id is valid.</param>
        [Rule]
        public static void SessionSetupRequestAdditional(
            AccountType accountType,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("ClientCapabilities")] Set<Capabilities> capabilities,
            bool isSendBufferSizeExceeds,
            bool isWriteBufferSizeExceeds,
            bool flag2,
            bool isGSSTokenValid,
            bool isUserIdValid)
        {
            Condition.IsTrue(flag2);
            Checker.CheckSessionSetupRequest(
                smbConnection,
                messageId,
                sessionId,
                smbState,
                isRequireSign,
                capabilities,
                isSendBufferSizeExceeds,
                isWriteBufferSizeExceeds);

            smbRequest = new SessionSetupRequestAdditional(
                accountType,
                messageId,
                sessionId,
                securitySignature,
                isRequireSign,
                capabilities,
                isGSSTokenValid,
                isUserIdValid);
            Update.UpdateSessionSetupRequestAdditional(smbConnection, smbRequest);
            smbState = SmbState.SessionSetupSent;
        }


        /// <summary>
        /// SMB_COM_SESSION_SETUP response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="securitySignatureValue">
        /// It indicates the security signature used in session setup response header.
        /// </param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="isGuestAccount">
        /// It indicates whether the account is a guest account or an administer account.
        /// </param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void SessionSetupResponseAdditional(
            int messageId,
            int sessionId,
            int securitySignatureValue,
            bool isSigned,
            bool isGuestAccount,
            MessageStatus messageStatus)
        {
            Checker.CheckSessionSetupResponseAdditional(
                smbConnection,
                smbState,
                messageId,
                sessionId,
                securitySignatureValue,
                isSigned,
                isGuestAccount,
                messageStatus,
                MessageStatus.Success);
            Condition.IsTrue(((SessionSetupRequestAdditional)smbConnection.sentRequest[messageId]).isUserIdValid);
            Condition.IsTrue(((SessionSetupRequestAdditional)smbConnection.sentRequest[messageId]).isGssValid);
            if (smbConnection.sentRequest[messageId].command == Command.SmbComSessionSetupAdditional)
            {
                ModelHelper.CaptureRequirement(
                    8380,
                    "[In Sequence Diagram] The server MUST respond to each client request that it receives.");
                ModelHelper.CaptureRequirement(
                    2193,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Response]NTLM Authentication: The connection MUST 
                    remain open for the client to attempt another authentication.");
                smbState = SmbState.SessionSetupSuccess;
            }

            // Authentication is completed.
            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    4143,
                    @"[In Sequence Diagram]The exchange of security tokens MUST continue until either the client or the
                    server determines that authentication has failed or both sides decide that authentication is 
                    complete.");

                if (((SessionSetupRequestAdditional)smbConnection.sentRequest[messageId]).isGssValid)
                {
                    ModelHelper.CaptureRequirement(
                        2329,
                        @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: If the GSS mechanism
                        indicates success, then  the server MUST create an SMB_COM_SESSION_SETUP_ANDX response 
                        (section 2.2.4.6.2).");

                    ModelHelper.CaptureRequirement(
                        4784,
                        @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: If the GSS mechanism 
                        indicates that the current output token is the last output token of the authentication exchange
                        based on the return code, as specified in [RFC2743]: Then the Status field in the SMB header of 
                        the response MUST be set to STATUS_SUCCESS.");
                }
            }

            if (smbConnection.clientCapabilities.Contains(Capabilities.CapExtendedSecurity))
            {
                ModelHelper.CaptureRequirement(
                    8390,
                    "[In Sequence Diagram] Session Setup Roundtrip: The request is sent to the SMB server, and the " +
                    "server builds an extended SMB_COM_SESSION_SETUP_ANDX response. (section 2.2.4.6.2.). ");
            }

            Update.UpdateSessionSetupResponseAdditional(smbConnection, messageId, sessionId);

            if (smbConnection.isSigningActive)
            {
                ModelHelper.CaptureRequirement(
                    2341,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Signing Initialization: Otherwise[if opposite 
                    to the condition: IsSigningActive is FALSE, and the response of the SMB_COM_SESSION_SETUP_ANDX 
                    operation contains STATUS_SUCCESS], Server.Connection.IsSigningActive MUST be set to TRUE.");
            }
        }


        /// <summary>
        /// SMB_COM_SESSION_SETUP Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorSessionSetupResponseAdditional(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.MoreProcessingRequired
                                || messageStatus == MessageStatus.StatusSmbBadUid);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.SmbComSessionSetupAdditional);

            if (messageStatus == MessageStatus.MoreProcessingRequired)
            {
                Condition.IsTrue(!((SessionSetupRequestAdditional)smbConnection.sentRequest[messageId]).isGssValid);
            }

            if (messageStatus == MessageStatus.StatusSmbBadUid)
            {
                Condition.IsTrue(!((SessionSetupRequestAdditional)smbConnection.sentRequest[messageId]).isUserIdValid);

                ModelHelper.CaptureRequirement(
                    2326,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: If the GSS mechanism 
                    indicates an error that is not STATUS_MORE_PROCESSING_REQUIRED, then the server MUST fail the client
                    request, and return only an SMB header and propagate the failure code.");
                ModelHelper.CaptureRequirement(
                    4776,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: If the GSS mechanism 
                    indicates an error that is not STATUS_MORE_PROCESSING_REQUIRED, the authentication has failed and 
                    no further processing is done on this request.");
                ModelHelper.CaptureRequirement(
                    4777,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: If the GSS mechanism 
                    indicates an error that is not STATUS_MORE_PROCESSING_REQUIRED, this error response is sent to the 
                    client.");
                ModelHelper.CaptureRequirement(
                    2333,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: Otherwise[If the GSS 
                    mechanism does not indicate that the current output token is the last output token of the 
                    authentication exchange based on the return code], the Status field in the SMB header of the 
                    response MUST be set to STATUS_MORE_PROCESSING_REQUIRED.");

                ModelHelper.CaptureRequirement(
                    80172,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Determine Reauth or Continuation of Previous 
                    Auth: If there is no session for the provided UID, then the request MUST be failed with
                    STATUS_SMB_BAD_UID.");
            }

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                ModelHelper.CaptureRequirement(
                    4765,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Determine Reauth or Continuation of Previous 
                    Auth: The server MUST prevent any further operations from executing on this session until 
                    authentication is complete, and fail them with STATUS_NETWORK_SESSION_EXPIRED.");
            }

            smbState = SmbState.Closed;
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        # region Tree Connection

        /// <summary>
        /// SMB_COM_TREE_CONNECT_ANDX request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="isRequestExtSignature">It indicates whether the client requests extended signature.</param>
        /// <param name="isRequestExtResponse">
        /// It indicates whether the client requests extended information on Tree connection response.
        /// </param>
        /// <param name="shareName">This is used to indicate which share the client wants to access.</param>
        /// <param name="shareType">The share type client intends to access.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        [Rule]
        public static void TreeConnectRequest(
            int messageId,
            int sessionId,
            bool isTidDisconnectionSet,
            bool isRequestExtSignature,
            bool isRequestExtResponse,
            [Domain("ShareDomain")] string shareName,
            ShareType shareType,
            bool isSigned)
        {
            Checker.CheckTreeConnectRequest(
                smbConnection,
                messageId,
                sessionId,
                smbState,
                isSigned,
                shareName,
                shareType);

            smbRequest = new TreeConnectRequest(
                messageId,
                sessionId,
                isTidDisconnectionSet,
                isRequestExtSignature,
                isRequestExtResponse,
                shareName,
                shareType);
            Update.UpdateTreeConnectRequest(smbConnection, smbRequest);
            smbState = SmbState.TreeConnectSent;
        }


        /// <summary>
        /// Tree multiple connect request
        /// </summary>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> The current session ID for this connection.</param>
        /// <param name="isRequestExtSignature"> Indicate whether the client requests extended signature.</param>
        /// <param name="isRequestExtResponse"> Indicate whether the client requests extended information 
        /// on Tree connection response.</param>
        /// <param name="share"> Share name.</param>
        /// <param name="shareType"> The share type client intends to access.</param>
        /// <param name="isSigned"> Indicate whether the message is signed or not for this request.</param>
        [Rule]
        public static void TreeMultipleConnectRequest(
            int messageId,
            int sessionId,
            bool isTidDisconnectionSet,
            bool isRequestExtSignature,
            bool isRequestExtResponse,
            string share,
            ShareType shareType,
            bool isSigned)
        {
            Checker.CheckTreeMultipleConnectRequest(
                smbConnection,
                messageId,
                sessionId,
                smbState,
                isSigned,
                share,
                shareType);
            smbRequest = new TreeConnectRequest(
                messageId,
                sessionId,
                isTidDisconnectionSet,
                isRequestExtSignature,
                isRequestExtResponse,
                share,
                shareType);
            Update.UpdateTreeConnectRequest(smbConnection, smbRequest);
            smbState = SmbState.TreeConnectSent;
        }

        /// <summary>
        /// SMB_COM_TREE_CONNECT_ANDX response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSecuritySignatureZero">It indicates whether the securitySignature is zero.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="isDfsShare">
        /// It indicates whether the share is managed by DFS. If SMB_SHARE_IS_IN_DFS in OptionalSupport of 
        /// SMB_COM_TREE_CONNECT_ANDX response is set, it is true; otherwise, it is false.
        /// </param>
        /// <param name="isSupportExtSignature">
        /// One flag of OptionalSupport field. If set, the SUT is using signing key protection as the client
        /// requested.
        /// </param>
        [Rule]
        public static void TreeConnectResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSecuritySignatureZero,
            ShareType shareType,
            MessageStatus messageStatus,
            bool isSigned,
            bool isDfsShare,
            bool isSupportExtSignature)
        {
            Checker.CheckTreeConnectResponse(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                isSecuritySignatureZero,
                shareType,
                smbState,
                messageStatus,
                MessageStatus.Success);

            TreeConnectRequest request = (TreeConnectRequest)smbConnection.sentRequest[messageId];

            if (request.shareName == Parameter.shareFileNames[2])
            {
                Condition.IsTrue(Parameter.isSupportDfs && isDfsShare);
            }
            else
            {
                Condition.IsTrue(!isDfsShare);
            }
            if (request.isRequestExtSignature)
            {
                Condition.IsTrue(isSupportExtSignature);
            }
            else if (!request.isRequestExtSignature)
            {
                Condition.IsTrue(!isSupportExtSignature);
            }

            Update.UpdateTreeConnectResponse(smbConnection, messageId, sessionId, treeId, shareType);

            smbState = SmbState.TreeConnectSuccess;
        }


        /// <summary>
        /// SMB_COM_TREE_CONNECT_ANDX Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isRs357Implemented">It indicates whether RS357 is implemented.</param>
        [Rule]
        public static void ErrorTreeConnectResponse(int messageId, MessageStatus messageStatus, bool isRs357Implemented)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.StatusBadNetWorkName || messageStatus == MessageStatus.InvalidSmb);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.SmbComTreeConnect);
            TreeConnectRequest request = (TreeConnectRequest)smbConnection.sentRequest[messageId];

            if (request.isTidDisconnectionSet && messageStatus == MessageStatus.StatusBadNetWorkName)
            {
                if (isRs357Implemented)
                {
                    ModelHelper.CaptureRequirement(
                        357,
                        @"[In Client Request Extensions,TREE_CONNECT_ANDX_DISCONNECT_TID]If set, the tree connect
                        specified by the TID in the SMB header of the request SHOULD be disconnected when the server 
                        sends the response.");

                    if (Parameter.sutPlatform != Platform.NonWindows)
                    {
                        ModelHelper.CaptureRequirement(
                            10357,
                            @"[In Client Request Extensions,TREE_CONNECT_ANDX_DISCONNECT_TID]If set, the tree connect
                            specified by the TID in the SMB header of the request is disconnected when the server sends
                            the response in Windows.");
                    }
                }
            }

            if (request.isTidDisconnectionSet && messageStatus == MessageStatus.InvalidSmb)
            {
                if (isRs357Implemented)
                {
                    ModelHelper.CaptureRequirement(
                        357,
                        @"[In Client Request Extensions,TREE_CONNECT_ANDX_DISCONNECT_TID]If set, the tree connect
                        specified by the TID in the SMB header of the request SHOULD be disconnected when the server 
                        sends the response.");

                    if (Parameter.sutPlatform != Platform.NonWindows)
                    {
                        ModelHelper.CaptureRequirement(
                            10357,
                            @"[In Client Request Extensions,TREE_CONNECT_ANDX_DISCONNECT_TID]If set, the tree connect
                            specified by the TID in the SMB header of the request is disconnected when the server sends
                            the response in Windows.");
                    }
                }
            }

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {

            }

            smbConnection.sessionList.Remove(request.sessionId);
            smbState = SmbState.Closed;
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #endregion

        #region Create the file
        /// <summary>
        /// SMB_COM_NT_CREATE_ANDX request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="desiredAccess">In the model, there are 3 kinds of access right: read, write, readwrite.</param>
        /// <param name="createDisposition">The action to take if a file does or does not exist.</param>
        /// <param name="impersonationLevel">
        /// This field specifies the information given to the SUT about the client and how the SUT MUST represent,
        /// or impersonate the client.
        /// </param>
        /// <param name="fileName">This is used to represent the name of the resource.</param>
        /// <param name="shareType">The share type in the current tree connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="isOpenByFileId">
        /// It indicates whether the FILE_OPEN_BY_FILE_ID is set in CreateOptions field of Create Request.
        /// </param>
        /// <param name="isDirectoryFile">
        /// It indicates whether the FILE_DIRECTORY_FILE and FILE_NON_DIRECTORY_FILE are set. 
        /// If true,FILE_DIRECTORY_FILE is set; else FILE_NON_DIRECTORY_FILE is set.
        /// </param>
        /// <param name="isMaximumAllowedSet">
        /// It indicates that the client set the maximum allowed in the request.
        /// </param>
        [Rule]
        public static void CreateRequest(
            int messageId,
            int sessionId,
            int treeId,
            [Domain("DesiredAccess")] int desiredAccess,
            CreateDisposition createDisposition,
            [Domain("ImpersonationLevel")] int impersonationLevel,
            [Domain("FileDomain")] string fileName,
            ShareType shareType,
            bool isSigned,
            bool isOpenByFileId,
            bool isDirectoryFile,
            bool isMaximumAllowedSet)
        {
            smbRequest = new CreateRequest(
                messageId,
                sessionId,
                treeId,
                desiredAccess,
                createDisposition,
                impersonationLevel,
                fileName,
                shareType,
                isSigned,
                isOpenByFileId,
                isDirectoryFile);

            Checker.CheckCreateRequest(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                shareType,
                fileName,
                smbState,
                isOpenByFileId,
                isDirectoryFile);

            if (shareType == ShareType.NamedPipe)
            {
                Condition.IsTrue((createDisposition == CreateDisposition.FileOpen)
                                    || (createDisposition == CreateDisposition.FileCreate)
                                    || (createDisposition == CreateDisposition.FileOpenIf));
            }
            // Save the client state when setting the max allowed, to cover the requirement.
            isClientMaxAllowedSet = isMaximumAllowedSet;

            Update.UpdateCreateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// SMB_COM_NT_CREATE_ANDX response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="createAction">Create action.</param>
        /// <param name="isFileIdZero">It indicates whether the fileId is zero.</param>
        /// <param name="isVolumeGUIDZero">It indicates whether the volumeGUIDIsZero is zero.</param>
        /// <param name="isDirectoryZero">It indicates whether the Directory field is zero or not.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isNoStream">
        /// It indicates whether NO_SUBSTREAMS bit in the FileStatusFlags field is set in the SMB_COM_NT_CREATE_ANDX 
        /// response.</param>
        [Rule]
        public static void CreateResponse(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            [Domain("ActionTaken")] Set<CreateAction> createAction,
            bool isFileIdZero,
            bool isVolumeGUIDZero,
            bool isDirectoryZero,
            bool isByteCountZero,
            bool isNoStream,
            MessageStatus messageStatus)
        {
            Checker.CheckCreateResponse(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                fId,
                createAction,
                isFileIdZero,
                isVolumeGUIDZero,
                isDirectoryZero,
                isNoStream,
                messageStatus);

            // The isByteCountZero is tested in adapter.
            Condition.IsTrue(isByteCountZero);

            // All input parameters are valid.
            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    30081,
                    @"[In Receiving an SMB_COM_NT_CREATE_ANDX Request] Otherwise [If normalization does not fail], the 
                    server MUST continue processing.");
            }

            if (!Parameter.isSupportStream && isNoStream)
            {
                ModelHelper.CaptureRequirement(
                    30065,
                    "[In Receiving an SMB_COM_NT_CREATE_ANDX Request, The processing of an SMB_COM_NT_CREATE_ANDX " +
                    "request is handled as specified in [MS-CIFS] section 3.3.5.50 with the following additions: ] If the " +
                    "server sends the new response, then it MUST construct a response as specified in section 2.2.4.9.2. " +
                    "with the addition of the following rules: If the underlying object store of the share in which the " +
                    "file is opened or created does not support streams, then the server MUST set the NO_SUBSTREAMS bit " +
                    "in the FileStatusFlags field.<116>");
            }

            if (Parameter.sutPlatform == Platform.Win2K
                    || Parameter.sutPlatform == Platform.Win2K3
                    || Parameter.sutPlatform == Platform.Win2K3R2
                    || Parameter.sutPlatform == Platform.Win2K8)
            {
                if (isVolumeGUIDZero)
                {
                    ModelHelper.CaptureRequirement(
                        9612,
                        "<56> Section 2.2.4.9.2: Windows 2000 server, Windows the SUT 2003, Windows the SUT 2003 R2, " +
                        "and Windows server 2008 set the VolumeGUID field to zero.");
                }

                if (isFileIdZero)
                {
                    ModelHelper.CaptureRequirement(
                        9614,
                        "<57> Section 2.2.4.9.2: Windows 2000 server, Windows the SUT 2003, Windows the SUT 2003 R2, and " +
                        "Windows server 2008 set the FileId field to zero. ");
                }

                if (Parameter.sutPlatform == Platform.Win2K3 || Parameter.sutPlatform == Platform.Win2K3R2
                    || Parameter.sutPlatform == Platform.Win2K8)
                {
                    Condition.IsTrue(isByteCountZero);
                }
            }

            CreateRequest request = (CreateRequest)smbConnection.sentRequest[messageId];
            createFileAccess = request.desiredAccess;
            Update.UpdateCreateResponse(smbConnection, messageId, fId, createAction);
        }


        /// <summary>
        /// SMB_COM_NT_CREATE_ANDX Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        /// Disable warning CA1502, because there are 28 error situations in this command according to the technical 
        /// document, the cyclomatic complexity cannot be reduced.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static void ErrorCreateResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.NtCreateRequest);
            Condition.IsTrue(messageStatus == MessageStatus.NotSupported
                                || messageStatus == MessageStatus.BadImpersonationLevel
                                || messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.ObjectPathNotFound
                                || messageStatus == MessageStatus.StatusFileIsADirectory
                                || messageStatus == MessageStatus.AccessDenied);

            CreateRequest request = (CreateRequest)smbConnection.sentRequest[messageId];
            bool isFileExists = false;
            foreach (SmbFile file in smbConnection.openedFiles.Values)
            {
                if (request.name == file.name)
                {
                    isFileExists = true;
                    break;
                }
            }

            // Parameter.fileName[2] indicates "ExistTest.txt" which is already created in the SUT.
            if (request.name == Parameter.fileNames[2])
            {
                isFileExists = true;
            }

            foreach (SmbPipe pipe in smbConnection.openedPipes.Values)
            {
                if (request.name == pipe.name)
                {
                    isFileExists = true;
                    break;
                }
            }

            foreach (SmbMailslot mailslot in smbConnection.openedMailslots.Values)
            {
                if (request.name == mailslot.pipeName)
                {
                    isFileExists = true;
                    break;
                }
            }

            switch (messageStatus)
            {
                case MessageStatus.NotSupported:
                    if (!request.isOpenByFileId)
                    {
                        if (isFileExists)
                        {
                            Condition.IsTrue(request.createDisposition == CreateDisposition.FileCreate);
                        }
                        else
                        {
                            Condition.IsTrue(request.createDisposition == CreateDisposition.FileOpen
                                                && request.createDisposition == CreateDisposition.FileOverwrite);
                        }
                    }
                    if (Parameter.sutPlatform == Platform.WinNt)
                    {
                        Condition.IsTrue(request.impersonationLevel != 3);
                    }

                    if (Parameter.dirNames.Contains(request.name))
                    {
                        Condition.IsTrue(request.isDirectoryFile);
                    }
                    else if (!Parameter.dirNames.Contains(request.name))
                    {
                        Condition.IsTrue(!request.isDirectoryFile);
                    }

                    if ((Parameter.sutPlatform != Platform.NonWindows) && !request.isOpenByFileId)
                    {
                        Condition.IsTrue(request.name != Parameter.fileNames[3]);
                    }

                    smbState = SmbState.End;
                    break;
                case MessageStatus.StatusFileIsADirectory:
                    Condition.IsTrue(!request.isOpenByFileId);
                    if (Parameter.dirNames.Contains(request.name))
                    {
                        if (isFileExists)
                        {
                            Condition.IsTrue(request.createDisposition != CreateDisposition.FileCreate);
                        }
                        else
                        {
                            Condition.IsTrue(request.createDisposition != CreateDisposition.FileOpen
                                                && request.createDisposition != CreateDisposition.FileOverwrite);
                        }

                        if (Parameter.sutPlatform == Platform.WinNt)
                        {
                            Condition.IsTrue(request.impersonationLevel != 3);
                        }
                        if (Parameter.sutPlatform != Platform.NonWindows)
                        {
                            Condition.IsTrue(request.name != Parameter.fileNames[3]);
                        }
                        Condition.IsTrue(!request.isDirectoryFile);
                    }
                    else if (!Parameter.dirNames.Contains(request.name))
                    {
                        if (isFileExists)
                        {
                            Condition.IsTrue(request.createDisposition != CreateDisposition.FileCreate);
                        }
                        else
                        {
                            Condition.IsTrue(request.createDisposition != CreateDisposition.FileOpen
                                                && request.createDisposition != CreateDisposition.FileOverwrite);
                        }

                        if (Parameter.sutPlatform == Platform.WinNt)
                        {
                            Condition.IsTrue(request.impersonationLevel != 3);
                        }

                        if (Parameter.sutPlatform != Platform.NonWindows)
                        {
                            Condition.IsTrue(request.name != Parameter.fileNames[3]);
                        }
                        Condition.IsTrue(request.isDirectoryFile);
                    }

                    smbState = SmbState.End;
                    break;
                case MessageStatus.BadImpersonationLevel:
                    Condition.IsTrue(!request.isOpenByFileId);
                    if (Parameter.dirNames.Contains(request.name))
                    {
                        Condition.IsTrue(request.isDirectoryFile);
                    }
                    else if (!Parameter.dirNames.Contains(request.name))
                    {
                        Condition.IsTrue(!request.isDirectoryFile);
                    }

                    if (Parameter.sutPlatform != Platform.NonWindows)
                    {
                        Condition.IsTrue(request.name != Parameter.fileNames[3]);
                    }

                    if (isFileExists)
                    {
                        Condition.IsTrue(request.createDisposition != CreateDisposition.FileCreate);
                    }
                    else
                    {
                        Condition.IsTrue(request.createDisposition != CreateDisposition.FileOpen
                                            && request.createDisposition != CreateDisposition.FileOverwrite);
                    }

                    // The request.impersonationLevel = 3 means the impersonationLevel is SECURITY_ANONYMOUS, 
                    // while the SUT is Windows NT, it can't perform this impersonation.
                    if (request.impersonationLevel == 0)
                    {
                        ModelHelper.CaptureRequirement(
                            30075,
                            "[In Receiving an SMB_COM_NT_CREATE_ANDX Request] If the impersonation level is" +
                            "SECURITY_ANONYMOUS, then the server MUST fail the request with" +
                            "STATUS_BAD_IMPERSONATION_LEVEL.<118>");
                    }

                    if (request.impersonationLevel > 3 || request.impersonationLevel < 0)
                    {
                        ModelHelper.CaptureRequirement(
                            1004000,
                            "<53> Section 2.2.4.9.1: Windows-based servers fail the request with " +
                            "STATUS_BAD_IMPERSONATION_LEVEL if the impersonation level is not one of SECURITY_ANONYMOUS, " +
                            "SECURITY_IDENTIFICATION, SECURITY_IMPERSONATION, or SECURITY_DELEGATION.");
                    }

                    smbState = SmbState.End;
                    break;
                case MessageStatus.ObjectPathNotFound:
                    Condition.IsTrue(!request.isOpenByFileId);

                    if (Parameter.dirNames.Contains(request.name))
                    {
                        Condition.IsTrue(request.isDirectoryFile);
                    }
                    else if (!Parameter.dirNames.Contains(request.name))
                    {
                        Condition.IsTrue(!request.isDirectoryFile);
                    }

                    if (Parameter.sutPlatform == Platform.WinNt)
                    {
                        Condition.IsTrue(request.impersonationLevel != 3);
                    }

                    if (isFileExists)
                    {
                        Condition.IsTrue(request.createDisposition != CreateDisposition.FileCreate);
                    }
                    else
                    {
                        Condition.IsTrue(request.createDisposition != CreateDisposition.FileOpen &&
                                            request.createDisposition != CreateDisposition.FileOverwrite);
                    }

                    Condition.IsTrue(Parameter.sutPlatform != Platform.NonWindows);

                    // Create "Dir1\Test1.txt"
                    Condition.IsTrue(request.name == Parameter.fileNames[3]);
                    smbState = SmbState.End;
                    break;
                case MessageStatus.NetworkSessionExpired:
                    smbConnection.sessionList.Remove(request.sessionId);
                    smbState = SmbState.Closed;
                    break;
                case MessageStatus.AccessDenied:
                    Condition.IsTrue(isClientMaxAllowedSet);
                    smbState = SmbState.End;
                    break;
                default:
                    break;
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region Read file
        /// <summary>
        /// SMB_COM_READ_ANDX Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>        
        /// <param name="fid">File ID.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        [Rule]
        public static void ReadRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            ShareType shareType,
            bool isSigned)
        {
            Checker.CheckReadRequest(smbConnection, messageId, sessionId, treeId, fid, shareType, isSigned, smbState);
            smbRequest = new ReadRequest(messageId, sessionId, treeId, fid, shareType, isSigned);
            Update.UpdateReadRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// SMB_COM_READ_ANDX response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="fid">The SMB file identifier of the target directory.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="isSendBufferSizeExceeds">
        /// It indicates whether the maximum the SUT buffer size of bytes for writing exceeds the MaxBufferSize field.
        /// </param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        /// Disable CA1801, because the parameter 'isSigned' is used for interface implementation.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static void ReadResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isSendBufferSizeExceeds,
            MessageStatus messageStatus)
        {
            Checker.CheckReadResponse(smbConnection, messageId, sessionId, treeId, messageStatus);
            if (BaseModelProgram.isSendBufferSizeExceedMaxBufferSize == isSendBufferSizeExceeds)
            {
                ModelHelper.CaptureRequirement(
                    206932,
                    "<33> Section 2.2.4.5.2.1:Windows-based clients and servers support CAP_LARGE_READX, which permits " +
                    "file transfers larger than the negotiated MaxBufferSize.");
                ModelHelper.CaptureRequirement(
                    5402,
                    @"When this capability is set by the server (and set by the client in the 
                    SMB_COM_SESSION_SETUP_ANDX request), then the maximum server buffer size for sending data can 
                    exceed the MaxBufferSize field.");
            }
            Update.UpdateReadResponse(smbConnection, messageId);
        }


        /// <summary>
        /// SMB_COM_READ_ANDX Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorReadResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.NetworkSessionExpired);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.NtReadRequest);
            ReadRequest request = (ReadRequest)smbConnection.sentRequest[messageId];

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                smbConnection.sessionList.Remove(request.sessionId);
                smbState = SmbState.Closed;
            }

            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region Write file

        /// <summary>
        /// SMB_COM_WRITE_ANDX Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="fId">This is the file ID of the opened file.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="synchronize">This flag SHOULD be ignored by both clients and servers.</param>
        [Rule]
        public static void WriteRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            ShareType shareType,
            bool isSigned,
            int synchronize)
        {
            Condition.IsTrue(synchronize == 0);
            Checker.CheckWriteRequest(smbConnection, messageId, sessionId, treeId, fId, shareType, isSigned, smbState);
            smbRequest = new WriteRequest(messageId, sessionId, treeId, shareType, isSigned);
            Update.UpdateWriteRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// SMB_COM_WRITE_ANDX response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isWrittenByteCountEqualCountField">
        /// It indicates whether the written bytes count is equal to the Count field of response.
        /// </param>
        /// <param name="isWrittenByteCountEqualCountHighField">
        /// It indicates whether the written bytes count is equal to the CountHigh field of response.
        /// </param>
        /// <param name="isWrittenByteCountLargerThanMaxBufferSize">
        /// It indicates whether the written bytes count is lager than MaxBufferSize.
        /// </param>
        [Rule]
        /// Disable CA1801, because the parameter 'isSigned' is used for interface implementation.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static void WriteResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus,
            bool isWrittenByteCountEqualCountField,
            bool isWrittenByteCountEqualCountHighField,
            bool isWrittenByteCountLargerThanMaxBufferSize)
        {
            Checker.CheckWriteResponse(smbConnection, messageId, sessionId, treeId, messageStatus);
            Condition.IsTrue(createFileAccess != 1);
            if (messageStatus == MessageStatus.Success)
            {
                if (isWrittenByteCountEqualCountField)
                {
                    ModelHelper.CaptureRequirement(
                        30041,
                        @"[In Receiving an SMB_COM_WRITE_ANDX Request] The processing of an SMB_COM_WRITE_ANDX request 
                        is handled as specified in [MS-CIFS] section 3.3.5.36 with the following additions: If the 
                        server successfully writes data to the underlying object store, then the count of bytes written
                        MUST be set in the Count fields of the response, as specified in section 2.2.4.3.2.");
                }

                if (isWrittenByteCountEqualCountHighField)
                {
                    ModelHelper.CaptureRequirement(
                        30042,
                        @"[In Receiving an SMB_COM_WRITE_ANDX Request] The processing of an SMB_COM_WRITE_ANDX request 
                        is handled as specified in [MS-CIFS] section 3.3.5.36 with the following additions: If the 
                        server successfully writes data to the underlying object store, then the count of bytes written 
                        MUST be set in the CountHigh fields of the response, as specified in section 2.2.4.3.2.");
                }

                if (BaseModelProgram.isWriteBufferSizeExceedMaxBufferSize && isWrittenByteCountLargerThanMaxBufferSize)
                {
                    ModelHelper.CaptureRequirement(
                        5407,
                        @"When this capability is set by the server (and set by the client in the 
                        SMB_COM_SESSION_SETUP_ANDX request), then the maximum server buffer size of bytes it writes can
                        exceed the MaxBufferSize field.");
                    ModelHelper.CaptureRequirement(
                        306932,
                        "<34> Section 2.2.4.5.2.1: Windows-based clients and servers support CAP_LARGE_WRITEX, which " +
                        "permits file transfers larger than the negotiated MaxBufferSize.");
                    ModelHelper.CaptureRequirement(
                        30039,
                        @"[In Receiving an SMB_COM_WRITE_ANDX Request] The processing of an SMB_COM_WRITE_ANDX request 
                        is handled as specified in [MS-CIFS] section 3.3.5.36 with the following additions: If 
                        CAP_LARGE_WRITEX is set in server.Connection.ClientCapabilities, then it is possible that the
                        count of bytes to be written is larger than the server's MaxBufferSize. ");

                    if (smbConnection.sutCapabilities.Contains(Capabilities.CapExtendedSecurity))
                    {
                        if (smbConnection.sutCapabilities.Contains(Capabilities.CapLargeWritex)
                            && smbConnection.clientCapabilities.Contains(Capabilities.CapLargeWritex))
                        {
                            ModelHelper.CaptureRequirement(
                                109957,
                                @"[In Extended Security Response]MaxBufferSize (4 bytes): The exceptions in which this 
                            maximum buffer size MUST be exceeded are:When the SMB_COM_WRITE_ANDX command is used and 
                            the client and server both support the CAP_LARGE_WRITEX capability (see the Capabilities 
                            field for more information).");
                        }
                    }
                }
            }

            Update.UpdateWriteResponse(smbConnection, messageId);
        }


        /// <summary>
        /// SMB_COM_WRITE_ANDX Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isRs5229Implemented">It indicates whether RS5229 is implemented.</param>
        [Rule]
        public static void ErrorWriteResponse(int messageId, MessageStatus messageStatus, bool isRs5229Implemented)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.StatusMaxBufferExceeded
                                || messageStatus == MessageStatus.AccessDenied);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.NtWriteRequest);
            WriteRequest request = (WriteRequest)smbConnection.sentRequest[messageId];

            if (isRs5229Implemented)
            {
                ModelHelper.CaptureRequirement(
                    5229,
                    @"[In Message Syntax]When an error occurs, unless otherwise noted in this specification, an SMB 
                    server SHOULD return an Error Response message. ");

                if (Parameter.sutPlatform != Platform.NonWindows)
                {
                    ModelHelper.CaptureRequirement(
                        105229,
                        @"[In Message Syntax]When an error occurs, unless otherwise noted in this specification, an SMB
                        server returns an Error Response message in Windows. ");
                }
            }

            if (messageStatus == MessageStatus.StatusMaxBufferExceeded)
            {
                if (!smbConnection.sutCapabilities.Contains(Capabilities.CapExtendedSecurity))
                {
                    if (smbConnection.sutCapabilities.Contains(Capabilities.CapLargeWritex)
                            && smbConnection.clientCapabilities.Contains(Capabilities.CapLargeWritex))
                    {
                        ModelHelper.CaptureRequirement(
                            9207,
                            @"[In Non-Extended Security Response]MaxBufferSize (4 bytes): The only exceptions in which
                            this maximum buffer size MUST be exceeded are:When the SMB_COM_WRITE_ANDX command is used
                            and both the client and server support the CAP_LARGE_WRITEX capability (see the 
                            Capabilities field for more information).");
                    }

                    if (smbConnection.sutCapabilities.Contains(Capabilities.CapRawMode)
                            && smbConnection.clientCapabilities.Contains(Capabilities.CapRawMode))
                    {
                        ModelHelper.CaptureRequirement(
                            9208,
                            @"[In Non-Extended Security Response]MaxBufferSize (4 bytes): The only exceptions in which 
                            this maximum buffer size MUST be exceeded are:When the SMB_COM_WRITE_RAW command is used 
                            and both the client and server support the CAP_RAW_MODE capability.");
                    }
                }
            }
            else if (messageStatus == MessageStatus.AccessDenied)
            {
                Condition.IsTrue(createFileAccess == 1);
                ModelHelper.CaptureRequirement(
                    9096,
                    @"[In File_Pipe_Printer_Access_Mask]If no access is granted for the client on this file, the server
                    MUST fail the open with STATUS_ACCESS_DENIED.");
                ModelHelper.CaptureRequirement(
                    9119,
                    @"[In Directory_Access_Mask]Directory_Access_Mask (4 bytes):MAXIMUM_ALLOWED 0x02000000:If no access
                    is granted for the client on this directory, then the server MUST fail the open with 
                    STATUS_ACCESS_DENIED.");
            }

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                ModelHelper.CaptureRequirement(
                    8553,
                    @"[In Receiving Any Message]Session Validation and Re-authentication: [If 
                    Connection.SessionTable[UID].AuthenticationState is equal to Expired] Otherwise[if a session renewal
                    is successful], the server MUST fail this operation with STATUS_NETWORK_SESSION_EXPIRED.");
            }

            smbConnection.sessionList.Remove(request.sessionId);
            smbState = SmbState.Closed;
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region Provide an unknown subcommand

        /// <summary>
        /// Bad command request error response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorInvalidCommandResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.InvalidCommand);
            Condition.IsTrue(messageStatus == MessageStatus.InvalidSmb
                                || messageStatus == MessageStatus.ErrSmbCmd);

            InvalidCommandRequest request = (InvalidCommandRequest)smbConnection.sentRequest[messageId];
            if (messageStatus == MessageStatus.InvalidSmb)
            {
                Condition.IsTrue(request.validCommand != Command.SmbComSendMessage
                                   && request.validCommand != Command.SmbComSendStartMbMessage
                                   && request.validCommand != Command.SmbComSendTextMbMessage
                                   && request.validCommand != Command.SmbComSendEndMbMessage);
            }

            else if (messageStatus == MessageStatus.ErrSmbCmd)
            {
                Condition.IsTrue(request.validCommand == Command.SmbComSendMessage
                                    || request.validCommand == Command.SmbComSendStartMbMessage
                                    || request.validCommand == Command.SmbComSendEndMbMessage
                                    || request.validCommand == Command.SmbComSendTextMbMessage);
            }

            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region Session Close

        /// <summary>
        /// Close session request.
        /// </summary>
        /// <param name="sessionId">The session Id which the session is being closed.</param>
        [Rule]
        public static void SessionClose(int sessionId)
        {
            Condition.IsTrue(smbState == SmbState.End);
            Condition.IsTrue(smbConnection.sessionList.ContainsKey(sessionId));

            ModelHelper.CaptureRequirement(
                2299,
                "[In Receiving Any Message]Signing: If the signature on the received packet is incorrect, the " +
                "server SHOULD<111> terminate the connection.");

            smbState = SmbState.Closed;
        }

        #endregion Session Close

        #region state variable on client side

        /// <summary>
        /// To store the value of bad command.
        /// </summary>
        private static FsctlInvalidCommand badCommand;

        #endregion

        /// <summary>
        /// FSCTL Bad command request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="fid">The SMB file identifier of the target directory.</param>
        /// <param name="command">This is used to tell adapter to send an invalid kind of command.</param>
        [Rule]
        public static void FsctlBadCommandRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            FsctlInvalidCommand command)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fid));
            Condition.IsTrue(smbConnection.openedFiles[fid].treeId == treeId);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            if (Parameter.sutPlatform != Platform.Win2K8R2
                    && Parameter.sutPlatform != Platform.NonWindows)
            {
                Condition.IsTrue(command == FsctlInvalidCommand.FsctlRequestOplockLevle1
                                    || command == FsctlInvalidCommand.FsctlRequestOplockLevel2
                                    || command == FsctlInvalidCommand.FsctlRequestBatchOplock
                                    || command == FsctlInvalidCommand.FsctlOplockBreakAcknowledge
                                    || command == FsctlInvalidCommand.FsctlOpBatchAckClosePending
                                    || command == FsctlInvalidCommand.FsctlOpLockBreakNotify
                                    || command == FsctlInvalidCommand.FsctlMoveFile
                                    || command == FsctlInvalidCommand.FsctlMarkHandle
                                    || command == FsctlInvalidCommand.FsctlQueryRetrievalPointers
                                    || command == FsctlInvalidCommand.FsctlPipeAssignEvent
                                    || command == FsctlInvalidCommand.FsctlGetVolumeBitmap
                                    || command == FsctlInvalidCommand.FsctlGetNtfsFileRecord
                                    || command == FsctlInvalidCommand.FsctlInvalidateVolumes
                                    || command == FsctlInvalidCommand.FsctlReadUsnJournal
                                    || command == FsctlInvalidCommand.FsctlCreateUsnJournal
                                    || command == FsctlInvalidCommand.FsctlQueryUsnJournal
                                    || command == FsctlInvalidCommand.FsctlDeleteUsnJournal
                                    || command == FsctlInvalidCommand.FsctlEnumUsnData);
            }
            smbRequest = new FSCTLBadCommandRequest(messageId, command);
            Update.UpdateRequest(smbConnection, smbRequest);

            // Store the bad command value.
            badCommand = command;
        }


        /// <summary>
        /// Bad command request error response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        /// Disable warning CA1502, because there are 35 error situations in this command according to the technical 
        /// document, the cyclomatic complexity cannot be reduced.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static void ErrorFsctlBadCommandResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.InvalidFsctlCommand);

            Condition.IsTrue(messageStatus == MessageStatus.NotSupported);
            ModelHelper.CaptureRequirement(
                9343,
                @"[In Client Request Extensions]Server implementations that receive undefined FSCTL or IOCTL operation 
                requests MUST either pass the operation code and data (if any) through to the underlying object store 
                or fail the operation by returning STATUS_NOT_SUPPORTED.<68>");

            if (Parameter.sutPlatform != Platform.NonWindows)
            {
                if (badCommand == FsctlInvalidCommand.FsctlRequestOplockLevle1
                        || badCommand == FsctlInvalidCommand.FsctlRequestOplockLevel2
                        || badCommand == FsctlInvalidCommand.FsctlRequestBatchOplock
                        || badCommand == FsctlInvalidCommand.FsctlOplockBreakAcknowledge
                        || badCommand == FsctlInvalidCommand.FsctlOpBatchAckClosePending
                        || badCommand == FsctlInvalidCommand.FsctlOpLockBreakNotify
                        || badCommand == FsctlInvalidCommand.FsctlMoveFile
                        || badCommand == FsctlInvalidCommand.FsctlMarkHandle
                        || badCommand == FsctlInvalidCommand.FsctlQueryRetrievalPointers
                        || badCommand == FsctlInvalidCommand.FsctlPipeAssignEvent
                        || badCommand == FsctlInvalidCommand.FsctlGetVolumeBitmap
                        || badCommand == FsctlInvalidCommand.FsctlGetNtfsFileRecord
                        || badCommand == FsctlInvalidCommand.FsctlInvalidateVolumes)
                {
                    ModelHelper.CaptureRequirement(
                        9568,
                        "<69> Section 2.2.7.2.1: The following FSCTLs[FSCTL_REQUEST_OPLOCK_LEVEL_1," +
                        "FSCTL_REQUEST_OPLOCK_LEVEL_2,FSCTL_REQUEST_BATCH_OPLOCK,FSCTL_OPLOCK_BREAK_ACKNOWLEDGE," +
                        "FSCTL_OPBATCH_ACK_CLOSE_PENDING,FSCTL_OPLOCK_BREAK_NOTIFY,FSCTL_MOVE_FILE,FSCTL_MARK_HANDLE," +
                        "FSCTL_QUERY_RETRIEVAL_POINTERS,FSCTL_PIPE_ASSIGN_EVENT,FSCTL_GET_VOLUME_BITMAP," +
                        "FSCTL_GET_NTFS_FILE_RECORD,FSCTL_INVALIDATE_VOLUMES]are explicitly blocked by the server and " +
                        "are failed with STATUS_NOT_SUPPORTED.");
                }

                //Win2K8R2
                if (Parameter.sutPlatform == Platform.Win2K8R2)
                {
                    if (badCommand == FsctlInvalidCommand.FsctlQueryDependentVolume
                        || badCommand == FsctlInvalidCommand.FsctlSdGlobalChange
                        || badCommand == FsctlInvalidCommand.FsctlGetBootAreaInfo
                        || badCommand == FsctlInvalidCommand.FsctlGetRetrievalPointerBase
                        || badCommand == FsctlInvalidCommand.FsctlSetPersistentVolumeState
                        || badCommand == FsctlInvalidCommand.FsctlQueryPersistentVolumeState
                        || badCommand == FsctlInvalidCommand.FsctlRequestOpLock
                        || badCommand == FsctlInvalidCommand.FsctlTxFsModifyRm
                        || badCommand == FsctlInvalidCommand.FsctlTxfsQueryRmInformation
                        || badCommand == FsctlInvalidCommand.FsctlTxFsRollForwardRedo
                        || badCommand == FsctlInvalidCommand.FsctlTxFsRollForwardUndo
                        || badCommand == FsctlInvalidCommand.FsctlTxfsStartRm
                        || badCommand == FsctlInvalidCommand.FsctlTxfsShutdownRm
                        || badCommand == FsctlInvalidCommand.FsctlTxfsReadBackupInformation
                        || badCommand == FsctlInvalidCommand.FsctlTxfsReadBackupInformation2
                        || badCommand == FsctlInvalidCommand.FsctlTxfsCreateSecondaryRm
                        || badCommand == FsctlInvalidCommand.FsctlTxfsGetMetadataInfo
                        || badCommand == FsctlInvalidCommand.FsctlTxfsGetTransactedVersion
                        || badCommand == FsctlInvalidCommand.FsctlTxfsSavePointInformation
                        || badCommand == FsctlInvalidCommand.FsctlTxfsCreateMiniVersion
                        || badCommand == FsctlInvalidCommand.FsctlTxfsTransactionActive
                        || badCommand == FsctlInvalidCommand.FsctlTxfsListTransactions)
                    {
                        ModelHelper.CaptureRequirement(
                            9571,
                            "The following FSCTLs[FSCTL_REQUEST_OPLOCK_LEVEL_1, FSCTL_REQUEST_OPLOCK_LEVEL_2," +
                            "FSCTL_REQUEST_BATCH_OPLOCK,FSCTL_REQUEST_FILTER_OPLOCK,FSCTL_OPLOCK_BREAK_ACKNOWLEDGE," +
                            "FSCTL_OPBATCH_ACK_CLOSE_PENDING, FSCTL_OPLOCK_BREAK_NOTIFY,FSCTL_MOVE_FILE," +
                            "FSCTL_MARK_HANDLE,FSCTL_QUERY_RETRIEVAL_POINTERS,FSCTL_PIPE_ASSIGN_EVENT," +
                            "FSCTL_GET_VOLUME_BITMAP,FSCTL_GET_NTFS_FILE_RECORD,FSCTL_INVALIDATE_VOLUMES," +
                            "FSCTL_READ_USN_JOURNAL,FSCTL_CREATE_USN_JOURNAL, FSCTL_QUERY_USN_JOURNAL," +
                            "FSCTL_DELETE_USN_JOURNAL,FSCTL_ENUM_USN_DATA,FSCTL_QUERY_DEPENDENT_VOLUME," +
                            "FSCTL_SD_GLOBAL_CHANGE, FSCTL_GET_BOOT_AREA_INFO,FSCTL_GET_RETRIEVAL_POINTER_BASE," +
                            "FSCTL_SET_PERSISTENT_VOLUME_STATE,FSCTL_QUERY_PERSISTENT_VOLUME_STATE, FSCTL_REQUEST_OPLOCK," +
                            "FSCTL_TXFS_MODIFY_RM,FSCTL_TXFS_QUERY_RM_INFORMATION,FSCTL_TXFS_ROLLFORW ARD_REDO," +
                            "FSCTL_TXFS_ROLLFORWARD_UNDO,FSCTL_TXFS_START_RM,FSCTL_TXFS_SHUTDOWN_RM," +
                            "FSCTL_TXFS_READ_BACKUP_INFORMATION, FSCTL_TXFS_WRITE_BACKUP_INFORMATION," +
                            "FSCTL_TXFS_WRITE_BACKUP_INFORMATION,FSCTL_TXFS_CREATE_SECONDARY_RM," +
                            "FSCTL_TXFS_GET_METADATA_INFO,FSCTL_TXFS_GET_TRANSACTED_VERSION," +
                            "FSCTL_TXFS_SAVEPOINT_INFORMATION, FSCTL_TXFS_CREATE_MINIVERSION," +
                            "FSCTL_TXFS_TRANSACTION_ACTIVE,FSCTL_TXFS_LIST_TRANSACTIONS," +
                            "FSCTL_TXFS_READ_BACKUP_INFORMATION2,FSCTL_TXFS_WRITE_BACKUP_INFORMATION2]are explicitly" +
                            "blocked by Windows server 2008 R2 and are not passed through to the object store. They are" +
                            "failed with STATUS_NOT_SUPPORTED.");
                    }
                }
            }
            smbConnection.sentRequest.Remove(messageId);
            smbState = SmbState.End;
        }

        #region Domain

        /// <summary>
        /// Info level queried by path.
        /// </summary>
        /// <returns>The information levels used for querying information by path.</returns>
        public static IEnumerable<InformationLevel> InfoLevelQueriedByPath()
        {
            // The information level supported by the SUT for quering path information.
            yield return InformationLevel.SmbQueryFileStreamInfo;
            yield return InformationLevel.SmbQueryFileAllLocationInfo;
            yield return InformationLevel.SmbQueryFileEndOfFileInfo;

            // The native Windows NT information level.
            yield return InformationLevel.FileAccessInformation;
        }


        /// <summary>
        /// Info level set by path.
        /// </summary>
        /// <returns>The information levels used for setting information by path.</returns>
        public static IEnumerable<InformationLevel> InfoLevelSetByPath()
        {
            // The information level supported by the SUT for setting path information.
            yield return InformationLevel.SmbInfoStandard;

            // The native Windows NT information level.
            yield return InformationLevel.FileAllocationInformation;
        }


        /// <summary>
        /// Info level set by FS.
        /// </summary>
        /// <returns> The information levels used for setting information by FS.</returns>
        public static IEnumerable<InformationLevel> InfoLevelSetByFs()
        {
            // The information level supported by the SUT for setting file system information.
            yield return InformationLevel.FileFsControlInformaiton;
        }

        /// <summary>
        /// Info level set by FS.
        /// </summary>
        /// <returns> The information levels used for setting information by FS.</returns>
        public static IEnumerable<InformationLevel> InfoLevelSetByFsAdditional()
        {
            // The information level supported by the SUT for setting file system information.
            yield return InformationLevel.FileFsControlInformaiton;

            // Invalid level.
            yield return InformationLevel.Invalid;
        }

        /// <summary>
        /// Info level set by FID.
        /// </summary>
        /// <returns>The information levels used for setting information by FID.</returns>
        public static IEnumerable<InformationLevel> InfoLevelSetByFid()
        {
            // The information level supported by the SUT for setting file information.
            yield return InformationLevel.SmbSetFileBasicInfo;

            // The information level not supported by the SUT, just for negative purpose.
            yield return InformationLevel.Invalid;

            // The native Windows NT information level.
            yield return InformationLevel.FileLinkInformation;
            yield return InformationLevel.FileRenameInformation;
        }


        /// <summary>
        /// Info level queried by FS.
        /// </summary>
        /// <returns>The information levels used for querying information by FS.</returns>
        public static IEnumerable<InformationLevel> InfoLevelQueriedByFs()
        {
            // The information level supported by the SUT for quering file system information.
            yield return InformationLevel.SmbQueryFsAttributeInfo;

            // The native Windows NT information level.
            yield return InformationLevel.FileFsSizeInformation;

            yield return InformationLevel.Invalid;
        }


        /// <summary>
        /// Info level find.
        /// </summary>
        /// <returns> The information levels used for querying information using Find operation.</returns>
        public static IEnumerable<InformationLevel> InfoLevelByFind()
        {
            yield return InformationLevel.SmbFindFileBothDirectoryInfo;
            yield return InformationLevel.SmbFindFileIdFullDirectoryInfo;
            yield return InformationLevel.SmbFindFileIdBothDirectoryInfo;
        }


        /// <summary>
        /// Info level queried by FID.
        /// </summary>
        /// <returns>The information levels used for querying information by FID.</returns>
        public static IEnumerable<InformationLevel> InfoLevelQueriedByFid()
        {
            // The information level supported by the SUT for quering file information.
            yield return InformationLevel.SmbInfoStandard;
            yield return InformationLevel.SmbQueryFileStreamInfo;

            // The native Windows NT information level
            yield return InformationLevel.FileAccessInformation;

            yield return InformationLevel.Invalid;
        }


        /// <summary>
        /// Client capabilities.
        /// </summary>
        /// <returns>The client capabilites.</returns>
        /// Disable CA1006, because according to current test suite design, it is no need to remove the nested type 
        /// argument.
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static IEnumerable<Set<Capabilities>> ClientCapabilities()
        {
            Set<Capabilities> capabilities = new Set<Capabilities>();

            if (smbConnection.sutCapabilities.Contains(Capabilities.CapNtSmbs))
            {
                capabilities = capabilities.Add(Capabilities.CapNtSmbs);
            }
            if (smbConnection.sutCapabilities.Contains(Capabilities.CapExtendedSecurity))
            {
                capabilities = capabilities.Add(Capabilities.CapExtendedSecurity);
            }

            yield return capabilities;
        }


        /// <summary>
        /// Share domain.
        /// </summary>
        /// <returns>All available shares that can be connected.</returns>
        public static IEnumerable<string> ShareDomain()
        {
            foreach (string share in Parameter.shareFileNames)
            {
                yield return share;
            }

            foreach (string share in Parameter.sharePipeNames)
            {
                yield return share;
            }

            foreach (string share in Parameter.sharePrinterNames)
            {
                yield return share;
            }

            foreach (string share in Parameter.shareDeviceNames)
            {
                yield return share;
            }
        }


        /// <summary>
        /// File domain.
        /// </summary>
        /// <returns>All available file names.</returns>
        public static IEnumerable<string> FileDomain()
        {
            foreach (string name in Parameter.fileNames)
            {
                yield return name;
            }
            foreach (string name in Parameter.pipeNames)
            {
                yield return name;
            }
            foreach (string name in Parameter.dirNames)
            {
                yield return name;
            }
        }


        /// <summary>
        /// Pipe names.
        /// </summary>
        /// <returns>All available pipe names.</returns>
        public static Set<string> PipeNames()
        {
            Set<string> s = new Set<string>();
            foreach (string name in Parameter.pipeNames)
            {
                s = s.Add(name);
            }
            return s;
        }

        /// <summary>
        /// Mailslot names.
        /// </summary>
        /// <returns>All available mailslot names.</returns>
        public static Set<string> MailslotNames()
        {
            Set<string> s = new Set<string>();
            foreach (string name in Parameter.mailslotNames)
            {
                s = s.Add(name);
            }
            return s;
        }


        /// <summary>
        /// Disired access.
        /// </summary>
        /// <returns>The kinds of the access rights.</returns>
        public static IEnumerable<int> DesiredAccess()
        {
            // Read Access Right.
            yield return 1;
            // Write Access Right.
            yield return 2;
            // Read & Write Access Right.
            yield return 3;
        }


        /// <summary>
        /// Impersonation level.
        /// </summary>
        /// <returns>The impersonation level.</returns>
        public static IEnumerable<int> ImpersonationLevel()
        {
            for (int i = 0; i <= 4; i++)
            {
                yield return i;
            }
        }


        /// <summary>
        /// Action taken.
        /// </summary>
        /// <returns>The action taken for the file after Open action.</returns>
        /// Disable CA1006, because according to current test suite design, it is no need to remove the nested type 
        /// argument.
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static IEnumerable<Set<CreateAction>> ActionTaken()
        {
            yield return new Set<CreateAction>(CreateAction.FileSuperseded, CreateAction.FileExists);
            yield return new Set<CreateAction>(CreateAction.FileOpened, CreateAction.FileExists);
            yield return new Set<CreateAction>(CreateAction.FileCreated, CreateAction.FileDoesNotExist);
            yield return new Set<CreateAction>(CreateAction.FileOverwritten, CreateAction.FileExists);
        }


        /// <summary>
        /// Valid command.
        /// </summary>
        /// <returns>Valid command.</returns>
        public static IEnumerable<Command> ValidCommand()
        {
            yield return Command.SmbComTransaction;
            yield return Command.TransMailSlotWrite;
            yield return Command.SmbComTransaction2;
            yield return Command.SmbComNtTransact;
            yield return Command.SmbComSendMessage;
            yield return Command.SmbComSendStartMbMessage;
            yield return Command.SmbComSendTextMbMessage;
            yield return Command.SmbComSendEndMbMessage;
        }


        /// <summary>
        /// the SUT capbilities.
        /// </summary>
        /// <returns>The the SUT capabilities.</returns>
        /// Disable CA1006, because according to current test suite design, it is no need to remove the nested type 
        /// argument.
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static IEnumerable<Set<Capabilities>> SutCapabilities()
        {
            Set<Capabilities> capabilities = new Set<Capabilities>();

            if (Parameter.isSupportDfs)
            {
                capabilities = capabilities.Add(Capabilities.CapDfs);
            }
            if (Parameter.isSupportInfoLevelPassThrough)
            {
                capabilities = capabilities.Add(Capabilities.CapInfoLevelPassThru);
            }
            if (smbConnection.isClientSupportExtSecurity)
            {
                capabilities = capabilities.Add(Capabilities.CapExtendedSecurity);
            }
            if (Parameter.isSupportNtSmb)
            {
                capabilities = capabilities.Add(Capabilities.CapNtSmbs);
            }
            yield return capabilities;
            yield return new Set<Capabilities>(Capabilities.None);
        }


        /// <summary>
        /// the SUT capbilities for non-extended security.
        /// </summary>
        /// <returns>The the SUT capabilities for non-extended security.</returns>
        /// Disable CA1006, because according to current test suite design, it is no need to remove the nested type 
        /// argument.
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static IEnumerable<Set<Capabilities>> SutCapabilitiesForNonextendedSecurity()
        {
            Set<Capabilities> capabilities = new Set<Capabilities>();
            if (Parameter.isSupportDfs)
            {
                capabilities = capabilities.Add(Capabilities.CapDfs);
            }
            if (Parameter.isSupportInfoLevelPassThrough)
            {
                capabilities = capabilities.Add(Capabilities.CapInfoLevelPassThru);
            }
            if (Parameter.isSupportNtSmb)
            {
                capabilities = capabilities.Add(Capabilities.CapNtSmbs);
            }
            yield return capabilities;
            yield return new Set<Capabilities>(Capabilities.None);
        }

        /// <summary>
        /// Client capabilities for non-extended security.
        /// </summary>
        /// <returns>The client capabilites for non-extended security..</returns>
        /// Disable CA1006, because according to current test suite design, it is no need to remove the nested type 
        /// argument.
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static IEnumerable<Set<Capabilities>> ClientCapabilitiesForNonextendedSecurity()
        {
            Set<Capabilities> capabilities = new Set<Capabilities>();
            if (smbConnection.sutCapabilities.Contains(Capabilities.CapUnicode))
            {
                capabilities = capabilities.Add(Capabilities.CapUnicode);
            }
            if (smbConnection.sutCapabilities.Contains(Capabilities.CapLargeFiles))
            {
                capabilities = capabilities.Add(Capabilities.CapLargeFiles);
            }
            if (smbConnection.sutCapabilities.Contains(Capabilities.CapNtSmbs))
            {
                capabilities = capabilities.Add(Capabilities.CapNtSmbs);
            }
            if (smbConnection.sutCapabilities.Contains(Capabilities.CapNtFind))
            {
                capabilities = capabilities.Add(Capabilities.CapNtFind);
            }
            if (smbConnection.sutCapabilities.Contains(Capabilities.CapStatus32))
            {
                capabilities = capabilities.Add(Capabilities.CapStatus32);
            }
            if (smbConnection.sutCapabilities.Contains(Capabilities.CapLevelIIOplocks))
            {
                capabilities = capabilities.Add(Capabilities.CapLevelIIOplocks);
            }
            yield return capabilities;
        }

        #endregion

    }
}
