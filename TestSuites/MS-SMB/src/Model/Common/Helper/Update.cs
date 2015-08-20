// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// This class is used to update all the request message and the response message.
    /// </summary>
    public sealed class Update
    {
        /// <summary>
        /// Update SMB_COM_NEGOTIATE request.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="smbRequest">It represents the SMB request.</param>
        public static void UpdateNegotiateRequest(SmbConnection connection, SmbRequest smbRequest)
        {
            NegotiateRequest req = smbRequest as NegotiateRequest;
            connection.SutSendSequenceNumber.Add(smbRequest.messageId);
            connection.sentRequest.Add(smbRequest.messageId, smbRequest);
            connection.clientSignState = req.signState;
            connection.isNegotiateSent = true;
            connection.isClientSupportExtSecurity = req.isSupportExtSecurity;
        }


        /// <summary>
        /// Update SMB_COM_NEGOTIATE response.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="signState">It indicates whether the System Under Test (the SUT) has message signing enabled or required </param>
        /// <param name="serverCapabilities">
        /// It is used by the SUT to notify the client as to which features are supported by the SUT.
        /// </param>
        public static void UpdateNegotiateResponse(
            SmbConnection connection, 
            int messageId,
            SignState signState, 
            Set<Capabilities> serverCapabilities)
        {
            connection.sutSignState = signState;
            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
            connection.SutNextReceiveSequenceNumber++;
            connection.sutCapabilities = serverCapabilities;
        }


        /// <summary>
        /// Update SMB_COM_SESSION_SETUP request.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="smbRequest">It represents the SMB request.</param>
        public static void UpdateSessionSetupRequest(SmbConnection connection, SmbRequest smbRequest)
        {
            SessionSetupRequest req = smbRequest as SessionSetupRequest;
            connection.SutSendSequenceNumber.Add(smbRequest.messageId);
            connection.sentRequest.Add(smbRequest.messageId, smbRequest);
            connection.clientCapabilities = req.capabilities;
            connection.accountType = req.accountType;
        }


        /// <summary>
        /// Update SMB_COM_SESSION_SETUP request.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="smbRequest">It represents the SMB request.</param>
        public static void UpdateSessionSetupRequestAdditional(SmbConnection connection, SmbRequest smbRequest)
        {
            SessionSetupRequestAdditional req = smbRequest as SessionSetupRequestAdditional;
            connection.SutSendSequenceNumber.Add(smbRequest.messageId);
            connection.sentRequest.Add(smbRequest.messageId, smbRequest);
            connection.clientCapabilities = req.capabilities;
            connection.accountType = req.accountType;
        }


        /// <summary>
        /// Update SMB_COM_SESSION_SETUP request for non-extended security mode.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="smbRequest">It represents the SMB request.</param>
        public static void UpdateNonExtSessionSetupRequest(SmbConnection connection, SmbRequest smbRequest)
        {
            NonExtendedSessionSetupRequest req = smbRequest as NonExtendedSessionSetupRequest;
            connection.SutSendSequenceNumber.Add(smbRequest.messageId);
            connection.sentRequest.Add(smbRequest.messageId, smbRequest);
            connection.clientCapabilities = req.capabilities;
            connection.accountType = req.accountType;
        }


        /// <summary>
        /// Update SMB_COM_SESSION_SETUP response for non-extended security mode.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        public static void UpdateNonExtSessionSetupResponse(
            SmbConnection connection, 
            int messageId, 
            int sessionId)
        {
            NonExtendedSessionSetupRequest request = (NonExtendedSessionSetupRequest)connection.sentRequest[messageId];
            connection.sessionList.Add(sessionId, new SmbSession(sessionId, SessionState.Valid));
            connection.sessionId++;
            connection.SutNextReceiveSequenceNumber++;

            if (connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                if (request.accountType == AccountType.Admin)
                {
                    connection.isSigningActive = true;
                }
                else if (request.accountType == AccountType.Guest)
                {
                    connection.isSigningActive = false;
                }
            }

            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
        }


        /// <summary>
        /// Update SMB_COM_SESSION_SETUP response.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        public static void UpdateSessionSetupResponse(
            SmbConnection connection,
            int messageId,
            int sessionId)
        {
            SessionSetupRequest request = (SessionSetupRequest)connection.sentRequest[messageId];
            connection.sessionList.Add(sessionId, new SmbSession(sessionId, SessionState.Valid));
            connection.sessionId++;
            connection.SutNextReceiveSequenceNumber++;

            if (connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                if (request.accountType == AccountType.Admin)
                {
                    connection.isSigningActive = true;                    
                }
                else if (request.accountType == AccountType.Guest)
                {
                    connection.isSigningActive = false;
                }
            }

            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
        }


        /// <summary>
        /// Update SMB_COM_SESSION_SETUP response.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        public static void UpdateSessionSetupResponseAdditional(
            SmbConnection connection, 
            int messageId,
            int sessionId)
        {
            SessionSetupRequestAdditional request = (SessionSetupRequestAdditional)connection.sentRequest[messageId];
            connection.sessionList.Add(sessionId, new SmbSession(sessionId, SessionState.Valid));
            connection.sessionId++;
            connection.SutNextReceiveSequenceNumber++;

            if (connection.isSignEnable(connection.clientSignState, connection.sutSignState))
            {
                if (request.accountType == AccountType.Admin)
                {
                    connection.isSigningActive = true;
                }
                else if (request.accountType == AccountType.Guest)
                {
                    connection.isSigningActive = false;
                }
            }

            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
        }


        /// <summary>
        /// Update SMB_COM_TREE_CONNECT_ANDX request.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="smbRequest">It represents the SMB request.</param>
        public static void UpdateTreeConnectRequest(SmbConnection connection, SmbRequest smbRequest)
        {
            connection.SutSendSequenceNumber.Add(smbRequest.messageId);
            connection.sentRequest.Add(smbRequest.messageId, smbRequest);

        }


        /// <summary>
        /// Update SMB_COM_TREE_CONNECT_ANDX response.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        public static void UpdateTreeConnectResponse(
            SmbConnection connection,
            int messageId, 
            int sessionId,
            int treeId,
            ShareType shareType)
        {
            TreeConnectRequest request = (TreeConnectRequest)connection.sentRequest[messageId];
            connection.treeConnectList.Add(
                treeId,
                new SmbTree(treeId, new SmbShare(request.shareName, shareType), sessionId));

            switch (shareType)
            {
                case ShareType.Disk:
                    Parameter.shareFileNames.Remove(request.shareName);
                    break;
                case ShareType.NamedPipe:
                    Parameter.sharePipeNames.Remove(request.shareName);
                    break;
                case ShareType.Printer:
                    Parameter.sharePrinterNames.Remove(request.shareName);
                    break;
                case ShareType.CommunicationDevice:
                    Parameter.shareDeviceNames.Remove(request.shareName);
                    break;
                default:
                    break;
            }

            connection.SutSendSequenceNumber.Remove(messageId);
            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
            connection.SutNextReceiveSequenceNumber++;
            connection.treeId++;
        }


        /// <summary>
        /// Update SMB_COM_NT_CREATE_ANDX request.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="smbRequest">It represents the SMB request.</param>
        public static void UpdateCreateRequest(SmbConnection connection, SmbRequest smbRequest)
        {
            connection.SutSendSequenceNumber.Add(smbRequest.messageId);
            connection.sentRequest.Add(smbRequest.messageId, smbRequest);

        }


        /// <summary>
        /// Update SMB_COM_NT_CREATE_ANDX response.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="createAction">The action taken for the specified file.</param>
        public static void UpdateCreateResponse(
            SmbConnection connection, 
            int messageId, 
            int fId, 
            Set<CreateAction> createAction)
        {
            CreateRequest request = (CreateRequest)connection.sentRequest[messageId];
            bool isOpened = false;
            if(createAction.Contains(CreateAction.FileCreated) || createAction.Contains(CreateAction.FileSuperseded))
            {
                isOpened = false;
            }
            else if(createAction.Contains(CreateAction.FileOpened)
                        || createAction.Contains(CreateAction.FileOverwritten ))
            {
                isOpened = true;
            }

            switch (request.shareType)
            {
                case ShareType.Disk:
                    SmbFile file = new SmbFile(
                        request.shareType, 
                        request.name, 
                        request.treeId,
                        request.desiredAccess, 
                        isOpened);
                    Console.WriteLine(fId);
                    connection.openedFiles.Add(fId, file);
                    break;
                case ShareType.NamedPipe:
                    if(Parameter.pipeNames.Contains(request.name))
                    {
                        SmbPipe pipe = new SmbPipe(
                            request.shareType, 
                            request.name, 
                            request.treeId, 
                            request.desiredAccess, 
                            isOpened);
                        connection.openedPipes.Add(fId, pipe);
                        connection.openedPipes[fId].isRequireReadModePipeState = Parameter.isMessageModePipe;
                    }
                    else if(Parameter.mailslotNames.Contains(request.name))
                    {
                        SmbMailslot mailslot = new SmbMailslot(
                            request.shareType, 
                            request.name, 
                            request.treeId,
                            request.desiredAccess, 
                            isOpened);
                        connection.openedMailslots.Add(fId,mailslot);
                    }
                    break;
                default: 
                    break;
            }

            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
            connection.SutNextReceiveSequenceNumber++;
            connection.fId++;
        }


        /// <summary>
        /// Update SMB_COM_READ_ANDX request.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="request">It represents the SMB request.</param>
        public static void UpdateReadRequest(SmbConnection connection, SmbRequest request)
        {
            connection.SutSendSequenceNumber.Add(request.messageId);
            connection.sentRequest.Add(request.messageId, request);
        }


        /// <summary>
        /// Update SMB_COM_READ_ANDX response.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        public static void UpdateReadResponse(SmbConnection connection, int messageId)
        {
            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
            connection.SutNextReceiveSequenceNumber++;
        }


        /// <summary>
        /// Update SMB_COM_WRITE_ANDX request.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="request">It represents the SMB request.</param>
        public static void UpdateWriteRequest(SmbConnection connection, SmbRequest request)
        {
            connection.SutSendSequenceNumber.Add(request.messageId);
            connection.sentRequest.Add(request.messageId, request);
        }


        /// <summary>
        /// Update SMB_COM_WRITE_ANDX response.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        public static void UpdateWriteResponse(SmbConnection connection, int messageId)
        {            
            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
            connection.SutNextReceiveSequenceNumber++;
        }


        /// <summary>
        /// Update request.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="request">It represents the SMB request.</param>
        public static void UpdateRequest(SmbConnection connection, SmbRequest request)
        {
            connection.SutSendSequenceNumber.Add(request.messageId);
            connection.sentRequest.Add(request.messageId, request);
        }


        /// <summary>
        /// Update response.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        public static void UpdateResponse(SmbConnection connection, int messageId)
        {
            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
            connection.SutNextReceiveSequenceNumber++;
        }


        /// <summary>
        /// Update NT_TRANSACT_CREATE request.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="smbRequest">It represents the SMB request.</param>
        public static void UpdateNTTransactCreateRequest(SmbConnection connection, SmbRequest smbRequest)
        {
            connection.SutSendSequenceNumber.Add(smbRequest.messageId);
            connection.sentRequest.Add(smbRequest.messageId, smbRequest);

        }


        /// <summary>
        /// Update NT_TRANSACT_CREATE response.
        /// </summary>
        /// <param name="connection">It represents the SMB connection.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        public static void UpdateNTTransactCreateResponse(SmbConnection connection, int messageId)
        {
            connection.SutSendSequenceNumber.Remove(messageId);
            connection.sentRequest.Remove(messageId);
            connection.SutNextReceiveSequenceNumber++;
        }

        #region Constructor

        /// <summary>
        /// Update private constructor.
        /// </summary>
        private Update()
        { }

        #endregion
    }
}
