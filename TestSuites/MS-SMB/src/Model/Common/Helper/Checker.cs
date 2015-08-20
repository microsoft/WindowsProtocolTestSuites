// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// This class is used to check all the request message and the response message.
    /// Most message states and parameter checking will be done this, that can keep the model program code to be clear.    
    /// </summary>
    public static class Checker
    {
        /// <summary>
        /// This is used to check SMB_COM_NEGOTIATE request message.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="dialectNames">The input array of dialects.</param>
        /// <param name="signState">State of the System Under Test (the SUT) has message signing enabled or required.</param>
        public static void CheckNegotiateRequest(
            SmbConnection connection,
            int messageId,
            SmbState smbState,
            SignState signState,
            Sequence<Dialect> dialectNames)
        {
            Condition.IsTrue(smbState == SmbState.CreateNamePipeAndMailslotSucceed);
            Condition.IsTrue(smbState == SmbState.CreateNamePipeAndMailslotSucceed);
            Condition.IsTrue(!connection.isNegotiateSent);
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(connection.sentRequest.Count == 0);
            Condition.IsTrue(!connection.SutSendSequenceNumber.Contains(messageId));
            if (signState == SignState.Required)
            {
                if (Parameter.clientPlatform != Platform.NonWindows)
                {
                    // SupportsExtendedSecurity is TRUE for Windows 2000 and later. 
                    // For all other Windows SMB clients and servers, it is FALSE.
                    Condition.IsTrue(Parameter.clientPlatform != Platform.WinNt);
                }
            }
            switch (Parameter.clientPlatform)
            {
                case Platform.NonWindows:
                    Condition.IsTrue(
                        (dialectNames == new Sequence<Dialect>(
                            Dialect.DosLanMan12,
                            Dialect.DosLanMan21,
                            Dialect.XenixCore,
                            Dialect.LanMan10,
                            Dialect.LanMan12,
                            Dialect.LanMan21,
                            Dialect.MsNet103,
                            Dialect.MsNet30,
                            Dialect.NtLanMan,
                            Dialect.PcLan1,
                            Dialect.PcNet1,
                            Dialect.Wfw10))
                        || (dialectNames == new Sequence<Dialect>(Dialect.Invalid)));
                    break;
                case Platform.WinNt:
                    Condition.IsTrue(
                        (dialectNames == new Sequence<Dialect>(
                            Dialect.PcNet1,
                            Dialect.XenixCore,
                            Dialect.MsNet103,
                            Dialect.LanMan10,
                            Dialect.Wfw10,
                            Dialect.LanMan12,
                            Dialect.LanMan21,
                            Dialect.NtLanMan))
                        || (dialectNames == new Sequence<Dialect>(Dialect.Invalid)));
                    break;
                case Platform.Win2K:
                case Platform.WinXP:
                    Condition.IsTrue(
                        (dialectNames == new Sequence<Dialect>(
                            Dialect.PcNet1,
                            Dialect.LanMan10,
                            Dialect.Wfw10,
                            Dialect.LanMan12,
                            Dialect.LanMan21,
                            Dialect.NtLanMan))
                        || (dialectNames == new Sequence<Dialect>(Dialect.Invalid)));
                    break;
                case Platform.Win7:
                case Platform.WinVista:
                    Condition.IsTrue(
                        (dialectNames == new Sequence<Dialect>(
                            Dialect.PcNet1,
                            Dialect.LanMan10,
                            Dialect.Wfw10,
                            Dialect.LanMan12,
                            Dialect.LanMan21,
                            Dialect.NtLanMan))
                        || (dialectNames == new Sequence<Dialect>(Dialect.Invalid)));
                    break;
                default: break;
            }
        }


        /// <summary>
        /// This is used to check SMB_COM_NEGOTIATE reponse message.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="validMessageStatus">Valid message status.</param>
        public static void CheckNegotiateResponse(
            SmbConnection connection,
            int messageId,
            SmbState smbState,
            MessageStatus messageStatus,
            params MessageStatus[] validMessageStatus)
        {
            Condition.IsTrue(smbState == SmbState.NegotiateSent);
            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(new Set<MessageStatus>(validMessageStatus).Contains(messageStatus));
            Condition.IsTrue(connection.isNegotiateSent);
            Condition.IsTrue(connection.SutSendSequenceNumber.Contains(messageId));
        }


        /// <summary>
        /// This is used to check SMB_COM_SESSION_SETUP request message.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="capabilities">
        /// Used by the SUT to notify the client as to which features are supported by the SUT.
        /// </param>
        /// <param name="isRequireSign">It indicates whether the message signing is required.</param>
        /// <param name="isSendBufferSizeExceeds">
        /// It indicates whether the maximum the SUT buffer size sent can exceed the MaxBufferSize field or not.
        /// </param>
        /// <param name="isWriteBufferSizeExceeds">
        /// It indicates whether the maximum the SUT buffer size wrote can exceed the MaxBufferSize field or not.
        /// </param>
        public static void CheckSessionSetupRequest(
            SmbConnection connection,
            int messageId,
            int sessionId,
            SmbState smbState,
            bool isRequireSign,
            Set<Capabilities> capabilities,
            bool isSendBufferSizeExceeds,
            bool isWriteBufferSizeExceeds)
        {
            Condition.IsTrue(smbState == SmbState.NegotiateSuccess);
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(!connection.SutSendSequenceNumber.Contains(messageId));
            Condition.IsTrue(sessionId == connection.sessionId);
            Condition.IsTrue(capabilities.Contains(Capabilities.CapExtendedSecurity));

            if (connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                Condition.IsTrue(isRequireSign);
            }
            else
            {
                Condition.IsTrue(!isRequireSign);
            }

            if (connection.sutCapabilities.Contains(Capabilities.CapLargeReadx)
                && capabilities.Contains(Capabilities.CapLargeReadx))
            {
                Condition.IsTrue(isSendBufferSizeExceeds);
            }

            if (connection.sutCapabilities.Contains(Capabilities.CapLargeWritex)
                && capabilities.Contains(Capabilities.CapLargeWritex))
            {
                Condition.IsTrue(isWriteBufferSizeExceeds);
            }
        }


        /// <summary>
        /// This is used to check SMB_COM_SESSION_SETUP request message for non-extended security mode.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="isRequireSign">It indicates whether the message signing is required.</param>
        /// <param name="capabilities">
        /// Used by the SUT to notify the client as to which features are supported by the SUT.
        /// </param>
        public static void NonExtendedCheckSessionSetupRequest(
            SmbConnection connection,
            int messageId,
            int sessionId, SmbState smbState,
            bool isRequireSign,
            Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize)
        {
            Condition.IsTrue(smbState == SmbState.NegotiateSuccess);
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(!connection.SutSendSequenceNumber.Contains(messageId));
            Condition.IsTrue(sessionId == connection.sessionId);
            Condition.IsTrue(!capabilities.Contains(Capabilities.CapExtendedSecurity));

            if (connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                Condition.IsTrue(isRequireSign);
            }
            else
            {
                Condition.IsTrue(!isRequireSign);
            }
            if (connection.sutCapabilities.Contains(Capabilities.CapLargeReadx)
                && capabilities.Contains(Capabilities.CapLargeReadx))
            {
                Condition.IsTrue(isSendBufferSizeExceedMaxBufferSize);
            }

            if (connection.sutCapabilities.Contains(Capabilities.CapLargeWritex)
                && capabilities.Contains(Capabilities.CapLargeWritex))
            {
                Condition.IsTrue(isWriteBufferSizeExceedMaxBufferSize);
            }
        }


        /// <summary>
        /// This is used to check SMB_COM_SESSION_SETUP reponse message for non-extended security mode.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="securitySignatureValue">
        /// It indicates the security signature used in session setup response header.
        /// </param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="isGuestAccount">It indicates whether the client is a guest.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="validMessageStatus">Valid message status.</param>
        public static void CheckNonExtendedSessionSetupResponse(
            SmbConnection connection,
            SmbState smbState,
            int messageId,
            int sessionId,
            int securitySignatureValue,
            bool isSigned,
            bool isGuestAccount,
            MessageStatus messageStatus,
            params MessageStatus[] validMessageStatus)
        {
            Condition.IsTrue(smbState == SmbState.SessionSetupSent);
            Condition.IsTrue(!connection.sutCapabilities.Contains(Capabilities.CapExtendedSecurity));
            Condition.IsTrue(new Set<MessageStatus>(validMessageStatus).Contains(messageStatus));
            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.sentRequest[messageId].command == Command.SmbComSessionSetup);
            NonExtendedSessionSetupRequest sentRequest
                = (NonExtendedSessionSetupRequest)connection.sentRequest[messageId];

            if (!connection.sessionList.ContainsKey(sentRequest.sessionId))
            {
                Condition.IsTrue(sessionId == (sentRequest.sessionId + 1));
            }

            if (connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                if (sentRequest.isRequireSign)
                {
                    if (isSigned)
                    {
                        Condition.IsTrue(sentRequest.accountType == AccountType.Admin);
                        Condition.IsTrue(!isGuestAccount);
                        Condition.IsTrue(securitySignatureValue == 1);
                    }
                    else
                    {
                        Condition.IsTrue(sentRequest.accountType == AccountType.Guest);
                        Condition.IsTrue(isGuestAccount);
                        Condition.IsTrue(securitySignatureValue == 0);
                    }
                }
            }
            else
            {
                Condition.IsTrue(!isSigned);
                Condition.IsTrue(securitySignatureValue == 0);
            }
        }


        /// <summary>
        /// This is used to check SMB_COM_SESSION_SETUP reponse message.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="validMessageStatus">Valid message status.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="isGuestAccount">It indicates whether the client is a guest.</param>
        /// <param name="securitySignatureValue">
        /// It indicates the security signature used in session setup response header.
        /// </param>
        public static void CheckSessionSetupResponse(
            SmbConnection connection,
            SmbState smbState,
            int messageId,
            int sessionId,
            int securitySignatureValue,
            bool isSigned,
            bool isGuestAccount,
            MessageStatus messageStatus,
            params MessageStatus[] validMessageStatus)
        {
            Condition.IsTrue(smbState == SmbState.SessionSetupSent);
            Condition.IsTrue(connection.sutCapabilities.Contains(Capabilities.CapExtendedSecurity));
            Condition.IsTrue(new Set<MessageStatus>(validMessageStatus).Contains(messageStatus));
            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.sentRequest[messageId].command == Command.SmbComSessionSetup);
            SessionSetupRequest sentRequest = (SessionSetupRequest)connection.sentRequest[messageId];

            if (!connection.sessionList.ContainsKey(sentRequest.sessionId))
            {
                Condition.IsTrue(sessionId == (sentRequest.sessionId + 1));
            }
            if (connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                if (sentRequest.isRequireSign)
                {
                    if (isSigned)
                    {
                        Condition.IsTrue(sentRequest.accountType == AccountType.Admin && !isGuestAccount);
                        ModelHelper.CaptureRequirement(
                            105555,
                            @"[In server Response Extensions]If[SMB_SETUP_GUEST 0x0001] clear (0x0000), then the user
                            successfully authenticated and is logged in.");

                        Condition.IsTrue(securitySignatureValue == 1);
                    }
                    else
                    {
                        // If Action field in SessionSetupResponse is set to 1, adapter return isGuestAccount as 1; 
                        // if not, adapter return isGuestAccount as 0.
                        Condition.IsTrue(sentRequest.accountType == AccountType.Guest && isGuestAccount);
                        ModelHelper.CaptureRequirement(
                            205555,
                            @"[In server Response Extensions]If [SMB_SETUP_GUEST 0x0001]set (0x0001), 
                            then authentication failed but the server has granted guest access; the user is logged in
                            as a Guest.");

                        Condition.IsTrue(securitySignatureValue == 0);
                    }
                }
            }
            else
            {
                Condition.IsTrue(!isSigned);
                Condition.IsTrue(securitySignatureValue == 0);
            }
        }


        /// <summary>
        /// This is used to check SMB_COM_SESSION_SETUP reponse message.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="validMessageStatus">Valid message status.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="isGuestAccount">It indicates whether the client is a guest.</param>
        /// <param name="securitySignatureValue">
        /// It indicates the security signature used in session setup response header.
        /// </param>
        public static void CheckSessionSetupResponseAdditional(
            SmbConnection connection,
            SmbState smbState,
            int messageId,
            int sessionId,
            int securitySignatureValue,
            bool isSigned,
            bool isGuestAccount,
            MessageStatus messageStatus,
            params MessageStatus[] validMessageStatus)
        {
            Condition.IsTrue(smbState == SmbState.SessionSetupSent);
            Condition.IsTrue(connection.sutCapabilities.Contains(Capabilities.CapExtendedSecurity));
            Condition.IsTrue(new Set<MessageStatus>(validMessageStatus).Contains(messageStatus));
            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.sentRequest[messageId].command == Command.SmbComSessionSetupAdditional);
            SessionSetupRequestAdditional sentRequest
                = (SessionSetupRequestAdditional)connection.sentRequest[messageId];

            if (!connection.sessionList.ContainsKey(sentRequest.sessionId))
            {
                Condition.IsTrue(sessionId == (sentRequest.sessionId + 1));
            }
            if (connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                if (sentRequest.isRequireSign)
                {
                    if (isSigned)
                    {
                        Condition.IsTrue(sentRequest.accountType == AccountType.Admin && !isGuestAccount);
                        ModelHelper.CaptureRequirement(
                            105555,
                            @"[In server Response Extensions]If[SMB_SETUP_GUEST 0x0001] clear (0x0000), then the user
                            successfully authenticated and is logged in.");
                        Condition.IsTrue(securitySignatureValue == 1);
                    }
                    else
                    {
                        // If Action field in SessionSetupResponse is set to 1, adapter return isGuestAccount as 1;
                        // if not, adapter return isGuestAccount as 0.
                        Condition.IsTrue(sentRequest.accountType == AccountType.Guest && isGuestAccount);
                        ModelHelper.CaptureRequirement(
                            205555,
                            @"[In server Response Extensions]If [SMB_SETUP_GUEST 0x0001]set (0x0001), 
                            then authentication failed but the server has granted guest access; the user is logged in 
                            as a Guest.");

                        Condition.IsTrue(securitySignatureValue == 0);
                    }
                }
            }
            else
            {
                Condition.IsTrue(!isSigned);
                Condition.IsTrue(securitySignatureValue == 0);
            }
        }


        /// <summary>
        /// This is used to check SMB_COM_TREE_CONNECT_ANDX request message.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="share">This is used to indicate which share the client wants to access.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        public static void CheckTreeConnectRequest(
            SmbConnection connection,
            int messageId,
            int sessionId,
            SmbState smbState,
            bool isSigned,
            string share,
            ShareType shareType)
        {
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(!connection.SutSendSequenceNumber.Contains(messageId));
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(smbState == SmbState.SessionSetupSuccess);

            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else if (!connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                Condition.IsTrue(!isSigned);
            }

            foreach (SmbTree tree in connection.treeConnectList.Values)
            {
                Condition.IsTrue(tree.smbShare.shareName != share);
            }

            switch (shareType)
            {
                case ShareType.Disk:
                    if (Parameter.isSupportDfs)
                    {
                        Condition.IsTrue(Parameter.shareFileNames.Contains(share));
                    }
                    else
                    {
                        Condition.IsTrue(Parameter.shareFileNames.Contains(share)
                                            && share != Parameter.shareFileNames[2]);
                    }
                    break;
                case ShareType.NamedPipe:
                    Condition.IsTrue(Parameter.sharePipeNames.Contains(share));
                    break;
                case ShareType.Printer:
                    Condition.IsTrue(Parameter.sharePrinterNames.Contains(share));
                    break;
                case ShareType.CommunicationDevice:
                    Condition.IsTrue(Parameter.shareDeviceNames.Contains(share));
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// This is used to check multiple tree connect request message.
        ///</summary>
        /// <param name="connection"> SMB Connection Constructor. </param>
        /// <param name="smbState"> SMB request and reponse state. </param>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> Session id. </param>
        /// <param name="isSigned"> Indicate whether the server has message signing enabled or required.</param>
        /// <param name="share"> Share name.</param>
        /// <param name="shareType"> The type of resource the client intends to access.</param>
        public static void CheckTreeMultipleConnectRequest(
            SmbConnection connection,
            int messageId,
            int sessionId,
            SmbState smbState,
            bool isSigned,
            string share,
            ShareType shareType)
        {
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(!connection.SutSendSequenceNumber.Contains(messageId));
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId) &&
                connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(smbState == SmbState.TreeConnectSuccess);
            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else if (!connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                Condition.IsTrue(!isSigned);
            }
            switch (shareType)
            {
                case ShareType.Disk:
                    if (Parameter.isSupportDfs)
                    {
                        Condition.IsTrue(Parameter.shareFileNames.Contains(share));
                    }
                    else
                    {
                        Condition.IsTrue(Parameter.shareFileNames.Contains(share) &&
                            share != Parameter.shareFileNames[2]);
                    }
                    break;
                case ShareType.NamedPipe:
                    Condition.IsTrue(Parameter.sharePipeNames.Contains(share));
                    break;
                case ShareType.Printer:
                    Condition.IsTrue(Parameter.sharePrinterNames.Contains(share));
                    break;
                case ShareType.CommunicationDevice:
                    Condition.IsTrue(Parameter.shareDeviceNames.Contains(share));
                    break;
                default:
                    break;
            }
        }

        ///<summary>
        /// This is used to check SMB_COM_TREE_CONNECT_ANDX response message.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSecuritySignatureZero">isSecuritySignatureZero.</param>
        /// <param name="validMessageStatus">Valid message status.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>        
        /// <param name="smbState">SMB request and reponse state.</param>
        public static void CheckTreeConnectResponse(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isSecuritySignatureZero,
            ShareType shareType,
            SmbState smbState,
            MessageStatus messageStatus,
            params MessageStatus[] validMessageStatus)
        {
            Condition.IsTrue(smbState == SmbState.TreeConnectSent);
            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.sentRequest[messageId].command == Command.SmbComTreeConnect);
            Condition.IsTrue(((TreeConnectRequest)connection.sentRequest[messageId]).sessionId == sessionId);
            Condition.IsTrue((treeId - connection.treeId) == 1);
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(((TreeConnectRequest)connection.sentRequest[messageId]).shareType == shareType);

            if (connection.isSigningActive)
            {
                Condition.IsTrue(!isSecuritySignatureZero);
                Condition.IsTrue(isSigned);
            }
            else if (!connection.isSigningActive)
            {
                Condition.IsTrue(isSecuritySignatureZero);
                Condition.IsTrue(!isSigned);
            }

            Condition.IsTrue(new Set<MessageStatus>(validMessageStatus).Contains(messageStatus));
        }


        /// <summary>
        /// Check SMB_COM_NT_CREATE_ANDX request.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="name">
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="isOpenByFileId">It indicates whether the file is opened by file ID.</param>
        /// <param name="isDirectoryFile">It indicates whether the file to be created is a directory or not.</param>
        public static void CheckCreateRequest(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            ShareType shareType,
            string name,
            SmbState smbState,
            bool isOpenByFileId,
            bool isDirectoryFile)
        {
            Condition.IsTrue(smbState == SmbState.TreeConnectSuccess);
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(!connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId)
                                && connection.treeConnectList[treeId].smbShare.shareType == shareType);
            Condition.IsTrue(!String.IsNullOrEmpty(name));
            Condition.IsTrue(shareType == ShareType.NamedPipe || shareType == ShareType.Disk);

            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else if (!connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                Condition.IsTrue(!isSigned);
            }

            switch (shareType)
            {
                case ShareType.Disk:
                    Condition.IsTrue(Parameter.dirNames.Contains(name) || Parameter.fileNames.Contains(name));
                    break;
                case ShareType.NamedPipe:
                    Condition.IsTrue(Parameter.pipeNames.Contains(name));
                    break;
                default: break;
            }

            if (!Parameter.dirNames.Contains(name))
            {
                Condition.IsTrue(!isDirectoryFile);
            }

            if (isOpenByFileId)
            {
                if (Parameter.dirNames.Contains(name))
                {
                    Condition.IsTrue(isDirectoryFile);
                }
                else if (!Parameter.dirNames.Contains(name))
                {
                    Condition.IsTrue(!isDirectoryFile);
                }
            }
        }


        /// <summary>
        /// Check SMB_COM_NT_CREATE_ANDX response.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="createAction">The action taken for the specified file.</param>
        /// <param name="isFileIdZero">It indicates whether the fileId is zero.</param>
        /// <param name="isVolumeGUIDZero">It indicates whether the volumeGUIDIsZero is zero.</param>
        /// <param name="isDirectoryZero">It indicates whether the Directory field is zero or not.</param>
        /// <param name="isNoStream">isNoStream.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// Disable warning CA1502, because there are 30 situations in this command according to the technical document,
        /// the cyclomatic complexity cannot be reduced.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static void CheckCreateResponse(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fId,
            Set<CreateAction> createAction,
            bool isFileIdZero,
            bool isVolumeGUIDZero,
            bool isDirectoryZero,
            bool isNoStream,
            MessageStatus messageStatus)
        {
            Condition.IsTrue(messageStatus == MessageStatus.Success);
            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(!connection.openedFiles.ContainsKey(fId));
            Condition.IsTrue(fId == connection.fId);
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId));
            Condition.IsTrue(connection.sentRequest[messageId].command == Command.NtCreateRequest);

            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else
            {
                Condition.IsTrue(!isSigned);
            }

            CreateRequest request = (CreateRequest)connection.sentRequest[messageId];
            if (request.shareType == ShareType.Disk)
            {
                if (Parameter.isSupportStream)
                {
                    Condition.IsTrue(!isNoStream);
                }
                else
                {
                    Condition.IsTrue(isNoStream);
                }
            }

            Condition.IsTrue(request.name != Parameter.fileNames[3]);
            Condition.IsTrue(!request.isOpenByFileId);
            if (Parameter.sutPlatform != Platform.NonWindows)
            {
                if (Parameter.sutPlatform == Platform.Win2K
                       || Parameter.sutPlatform == Platform.Win2K3
                       || Parameter.sutPlatform == Platform.Win2K8
                       || Parameter.sutPlatform == Platform.Win2K8R2)
                {
                    Condition.IsTrue(isFileIdZero && isVolumeGUIDZero);
                }
                else
                {
                    Condition.IsTrue(isFileIdZero && isVolumeGUIDZero);
                }
            }

            if (Parameter.dirNames.Contains(request.name))
            {
                Condition.IsTrue(request.isDirectoryFile);
                Condition.IsTrue(!isDirectoryZero);
            }
            else
            {
                Condition.IsTrue(!request.isDirectoryFile);
                Condition.IsTrue(isDirectoryZero);
            }

            bool fileExist = false;
            if (request.shareType == ShareType.Disk)
            {
                foreach (SmbFile file in connection.openedFiles.Values)
                {
                    if (request.name == file.name)
                    {
                        fileExist = true;
                        break;
                    }
                }

                // Parameter.fileName[2] indicates "ExistTest.txt" which is already created in the SUT.
                if (request.name == Parameter.fileNames[2])
                {
                    fileExist = true;
                }
            }
            else
            {
                fileExist = true;
            }

            switch (request.createDisposition)
            {
                case CreateDisposition.FileSupersede:
                    if (fileExist)
                    {
                        Condition.IsTrue(createAction
                            == new Set<CreateAction>(CreateAction.FileSuperseded, CreateAction.FileExists));
                    }
                    else
                    {
                        Condition.IsTrue(createAction
                            == new Set<CreateAction>(CreateAction.FileCreated, CreateAction.FileDoesNotExist));
                    }
                    break;
                case CreateDisposition.FileOpen:
                    Condition.IsTrue(fileExist);
                    Condition.IsTrue(createAction
                        == new Set<CreateAction>(CreateAction.FileOpened, CreateAction.FileExists));
                    break;
                case CreateDisposition.FileCreate:
                    Condition.IsTrue(!fileExist);
                    Condition.IsTrue(createAction
                        == new Set<CreateAction>(CreateAction.FileCreated, CreateAction.FileDoesNotExist));
                    break;
                case CreateDisposition.FileOpenIf:
                    if (fileExist)
                    {
                        Condition.IsTrue(createAction
                            == new Set<CreateAction>(CreateAction.FileOpened, CreateAction.FileExists));
                    }
                    else
                    {
                        Condition.IsTrue(createAction
                            == new Set<CreateAction>(CreateAction.FileCreated, CreateAction.FileDoesNotExist));
                    }
                    break;
                case CreateDisposition.FileOverwrite:
                    Condition.IsTrue(fileExist);
                    Condition.IsTrue(createAction
                        == new Set<CreateAction>(CreateAction.FileOverwritten, CreateAction.FileExists));
                    break;
                case CreateDisposition.FileOverwriteIf:
                    if (fileExist)
                    {
                        Condition.IsTrue(createAction
                            == new Set<CreateAction>(CreateAction.FileOverwritten, CreateAction.FileExists));
                    }
                    else
                    {
                        Condition.IsTrue(createAction
                            == new Set<CreateAction>(CreateAction.FileCreated, CreateAction.FileDoesNotExist));
                    }
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Check SMB_COM_READ_ANDX request.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="name">
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </param>
        /// <param name="smbState">SMB request and reponse state.</param>
        public static void CheckReadRequest(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            ShareType shareType,
            bool isSigned,
            SmbState smbState)
        {
            Condition.IsTrue(smbState == SmbState.TreeConnectSuccess);
            Condition.IsTrue(!connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId)
                                && connection.treeConnectList[treeId].smbShare.shareType == shareType);
            Condition.IsTrue(connection.openedFiles.ContainsKey(fId));
            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else if (!connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                Condition.IsTrue(!isSigned);
            }
            Condition.IsTrue(shareType == ShareType.Disk || shareType == ShareType.NamedPipe);
        }


        /// <summary>
        /// Check SMB_COM_OPEN_ANDX response.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        public static void CheckReadResponse(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            MessageStatus messageStatus)
        {
            Condition.IsTrue(messageStatus == MessageStatus.Success);
            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId));
            Condition.IsTrue(connection.sentRequest[messageId].command == Command.NtReadRequest);
        }


        /// <summary>
        /// Check SMB_COM_WRITE_ANDX request.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="name">
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </param>
        /// <param name="smbState">SMB request and reponse state.</param>
        public static void CheckWriteRequest(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            ShareType shareType,
            bool isSigned,
            SmbState smbState)
        {
            Condition.IsTrue(smbState == SmbState.TreeConnectSuccess);
            Condition.IsTrue(!connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId)
                                && connection.treeConnectList[treeId].smbShare.shareType == shareType);
            Condition.IsTrue(connection.openedFiles.ContainsKey(fid));

            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else if (!connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                Condition.IsTrue(!isSigned);
            }
            Condition.IsTrue(shareType == ShareType.Disk || shareType == ShareType.NamedPipe);
        }


        /// <summary>
        /// Check SMB_COM_OPEN_ANDX response.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        public static void CheckWriteResponse(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            MessageStatus messageStatus)
        {
            Condition.IsTrue(messageStatus == MessageStatus.Success);

            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId));
            Condition.IsTrue(connection.sentRequest[messageId].command == Command.NtWriteRequest);
            Condition.IsTrue(!connection.sutCapabilities.Contains(Capabilities.CapLargeWritex)
                                || !connection.clientCapabilities.Contains(Capabilities.CapLargeWritex));
            Condition.IsTrue(!connection.sutCapabilities.Contains(Capabilities.CapRawMode)
                                || !connection.clientCapabilities.Contains(Capabilities.CapRawMode));
        }


        /// <summary>
        /// It is used to check parameters of request.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="smbState">SMB request and reponse state.</param>
        public static void CheckRequest(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            SmbState smbState)
        {
            Condition.IsTrue(smbState == SmbState.TreeConnectSuccess);
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(!connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId));
            Condition.IsTrue(connection.treeConnectList[treeId].sessionId == sessionId);
            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else if (!connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                Condition.IsTrue(!isSigned);
            }
        }


        /// <summary>
        /// It is used to check parameters of response.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="request">Request prop.</param>
        /// <param name="validStatus">Vail status.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        public static void CheckResponse<RequestType>(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus,
            out RequestType request,
            params MessageStatus[] validStatus) where RequestType : SmbRequest
        {
            Condition.IsTrue(new Set<MessageStatus>(validStatus).Contains(messageStatus));
            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId));
            Condition.IsTrue(connection.treeConnectList[treeId].sessionId == sessionId);
            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else
            {
                Condition.IsTrue(!isSigned);
            }

            SmbRequest genericRequest = null;
            Condition.IsTrue(connection.sentRequest.TryGetValue(messageId, out genericRequest));

            request = genericRequest as RequestType;
            Condition.IsTrue(request != null);
        }


        /// <summary>
        /// Check NT_TRANSACT_CREATE request.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="name">
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </param>
        /// <param name="smbState">SMB request and reponse state.</param>
        /// <param name="isOpenByFileId">isOpenByFileId.</param>
        /// <param name="isDirectoryFile">isDirectoryFile.</param>
        public static void CheckNtTransactCreateRequest(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            ShareType shareType,
            bool isSigned,
            string name,
            SmbState smbState)
        {
            Condition.IsTrue(smbState == SmbState.TreeConnectSuccess);
            Condition.IsTrue(messageId == connection.SutNextReceiveSequenceNumber);
            Condition.IsTrue(!connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId));

            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else if (!connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                Condition.IsTrue(!isSigned);
            }

            Condition.IsTrue(shareType == ShareType.NamedPipe || shareType == ShareType.Disk);
            switch (shareType)
            {
                case ShareType.Disk:
                    Condition.IsTrue(Parameter.dirNames.Contains(name) || Parameter.fileNames.Contains(name));
                    break;
                case ShareType.NamedPipe:
                    Condition.IsTrue(Parameter.pipeNames.Contains(name));
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Check NT_TRANSACT_CREATE response.
        /// </summary>
        /// <param name="connection">SMB Connection Constructor.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        public static void CheckNtTransactCreateResponse(
            SmbConnection connection,
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            Condition.IsTrue(messageStatus == MessageStatus.Success);
            Condition.IsTrue(connection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(connection.sessionList.ContainsKey(sessionId)
                                && connection.sessionList[sessionId].sessionState == SessionState.Valid);
            Condition.IsTrue(connection.treeConnectList.ContainsKey(treeId));
            Condition.IsTrue(connection.sentRequest[messageId].command == Command.NtTransactCreate);
            if (connection.isSigningActive)
            {
                Condition.IsTrue(isSigned);
            }
            else
            {
                Condition.IsTrue(!isSigned);
            }
        }
    }
}
