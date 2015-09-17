// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// CifsClientContext provides a collection for all CIFS client context and 
    /// its access. all operations to access the client context provided in 
    /// CifsClientContext are thread safe. all operations to get some data from
    /// the context return a snapshot of the current status.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class CifsClientContext : FileServiceClientContext
    {
        #region fields

        private CifsClientCollection contextCollection;

        // common global ADM:
        private MessageSigningPolicyValues messageSigningPolicy;

        // client global ADM:
        private PlaintextAuthenticationPolicyValues plaintextAuthenticationPolicy;
        private LMAuthenticationPolicyValues lmAuthenticationPolicy;
        private NTLMAuthenticationPolicyValues ntlmAuthenticationPolicy;

        #endregion


        #region properties

        /// <summary>
        /// A state that determines whether this node signs messages. 
        /// </summary>
        public MessageSigningPolicyValues MessageSigningPolicy
        {
            get
            {
                return this.messageSigningPolicy;
            }
            set
            {
                this.messageSigningPolicy = value;
            }
        }


        /// <summary>
        /// A state that determines whether plaintext authentication is permitted.
        /// </summary>
        public PlaintextAuthenticationPolicyValues PlaintextAuthenticationPolicy
        {
            get
            {
                return this.plaintextAuthenticationPolicy;
            }
            set
            {
                this.plaintextAuthenticationPolicy = value;
            }
        }


        /// <summary>
        /// A state that determines the LAN Manager challenge/response authentication
        /// mechanism to be used. 
        /// </summary>
        public LMAuthenticationPolicyValues LmAuthenticationPolicy
        {
            get
            {
                return this.lmAuthenticationPolicy;
            }
            set
            {
                this.lmAuthenticationPolicy = value;
            }
        }


        /// <summary>
        /// A state that determines the NT LAN Manager challenge/response authentication
        /// mechanism to be used.
        /// </summary>
        public NTLMAuthenticationPolicyValues NtlmAuthenticationPolicy
        {
            get
            {
                return this.ntlmAuthenticationPolicy;
            }
            set
            {
                this.ntlmAuthenticationPolicy = value;
            }
        }

        #endregion


        #region Constructor & Dispose

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsClientContext()
            : base()
        {
            this.contextCollection = new CifsClientCollection();
            this.messageSigningPolicy = MessageSigningPolicyValues.MessageSigningDisabled;
            this.plaintextAuthenticationPolicy = PlaintextAuthenticationPolicyValues.Disabled;
            this.lmAuthenticationPolicy = LMAuthenticationPolicyValues.Disabled;
            this.ntlmAuthenticationPolicy = NTLMAuthenticationPolicyValues.Disabled;
        }

        #endregion


        #region Update Context

        /// <summary>
        /// update the connection state. it will be invoked every time connect or disconnect occurs. 
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        /// <param name="state">the latest connection state.</param>
        public virtual void UpdateTransportState(int connectionId, StackTransportState state)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        (this.contextCollection.ConnectionList[i] as CifsClientPerConnection).ConnectionState = state;
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// this function will be invoked every time a packet is sent or received. all logics about 
        /// Client' states will be implemented here.
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        /// <param name="packet">the sent or received packet in stack transport.</param>
        public virtual void UpdateRoleContext(int connectionId, StackPacket packet)
        {
            CifsClientPerConnection connection = this.GetConnection(connectionId) as CifsClientPerConnection;

            SmbPacket smbPacket = packet as SmbPacket;

            // Do nothing if no connection is found or the packet is not SmbPacket:
            if (connection == null || smbPacket == null)
            {
                return;
            }

            UpdateRoleContext(connection, smbPacket);
        }

        /// <summary>
        /// this function will be invoked every time a packet is sent or received. all logics about 
        /// Client' states will be implemented here.
        /// </summary>
        /// <param name="connection">the connection object.</param>
        /// <param name="packet">the sent or received packet in stack transport.</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        protected virtual void UpdateRoleContext(CifsClientPerConnection connection, SmbPacket packet)
        {
            // request packet:
            if ((packet.PacketType == SmbPacketType.BatchedRequest
                || packet.PacketType == SmbPacketType.SingleRequest)
                // Skip over the OpLock Break Notification request sent from server.
                && packet.SmbHeader.Protocol != CifsMessageUtils.SMB_PROTOCOL_IDENTIFIER_ASYNC
                && packet.SmbHeader.Mid != CifsMessageUtils.INVALID_MID)
            {
                RequestPacketUpdateRoleContext(connection, packet, false);
            }
            // response packet:
            else if (packet.PacketType == SmbPacketType.BatchedResponse
                || packet.PacketType == SmbPacketType.SingleResponse)
            {
                SmbPacket request = this.GetOutstandingRequest(connection.ConnectionId, (ulong)packet.SmbHeader.Mid);
                ResponsePacketUpdateRoleContext(connection, request, packet);
                if (connection.IsSigningActive && packet.IsSignRequired)
                {
                    packet.IsSignatureCorrect = CifsMessageUtils.VerifySignature(
                        packet,
                        connection.ConnectionSigningSessionKey,
                        (uint)connection.ClientResponseSequenceNumberList[packet.SmbHeader.Mid],
                        connection.ConnectionSigningChallengeResponse);
                }
                ResponsePacketUpdateRoleContextRegular(connection, packet);
            }
            else
            {
                // Do nothing if neither request nor response.
                // No exception is thrown here because UpdateRoleContext is not responsible for checking the 
                // invalidation of the packet.
            }
        }


        /// <summary>
        /// this function will be invoked every time a packet is sent or received. all logics about 
        /// Client' states will be implemented here.
        /// request packet update the context
        /// </summary>
        /// <param name="connection">the connection object.</param>
        /// <param name="packet">the sent or received packet in stack transport.</param>
        /// <param name="isAndxPacket">the packet is andx packet or not.</param>
        protected virtual void RequestPacketUpdateRoleContext(CifsClientPerConnection connection, SmbPacket packet, bool isAndxPacket)
        {
            int connectionId = connection.ConnectionId;

            if (packet == null)
            {
                return;
            }

            #region To update Connection/Session/Tree/Open with request
            switch (packet.SmbHeader.Command)
            {
                case SmbCommand.SMB_COM_NEGOTIATE:
                    connection.NegotiateSent = true;
                    this.AddOrUpdateConnection(connection);
                    break;

                default:
                    // No Connection/Session/Tree/Open will be updated for other requests.
                    break;
            }
            #endregion

            #region AddOutstandingRequest
            if (!isAndxPacket
                && packet.SmbHeader.Command != SmbCommand.SMB_COM_TRANSACTION_SECONDARY
                && packet.SmbHeader.Command != SmbCommand.SMB_COM_TRANSACTION2_SECONDARY
                && packet.SmbHeader.Command != SmbCommand.SMB_COM_NT_TRANSACT_SECONDARY
                && packet.SmbHeader.Command != SmbCommand.SMB_COM_NT_CANCEL)
            {
                this.AddOutstandingRequest(connectionId, packet);
            }
            else
            {
                // do nothing for requests of Andx, SECONDARY and CANCEL.
            }
            #endregion

            #region UpdateSequenceNumber
            if (!isAndxPacket)
            {
                this.UpdateSequenceNumber(connectionId, packet);
            }

            SmbBatchedRequestPacket batchedPacket = packet as SmbBatchedRequestPacket;
            if (batchedPacket != null)
            {
                RequestPacketUpdateRoleContext(connection, batchedPacket.AndxPacket, true);
            }
            #endregion
        }


        /// <summary>
        /// this function will be invoked every time a packet is sent or received. all logics about 
        /// Client' states will be implemented here.
        /// response packet update the context, do common transaction.
        /// </summary>
        /// <param name="connection">the connection object.</param>
        /// <param name="request">the request corresponding with the response.</param>
        /// <param name="response">the sent or received packet in stack transport.</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected virtual void ResponsePacketUpdateRoleContext(
            CifsClientPerConnection connection,
            SmbPacket request,
            SmbPacket response)
        {
            int connectionId = connection.ConnectionId;

            if (request ==null | response == null)
            {
                return;
            }

            #region To update Connection/Session/Tree/Open with response

            // Update context with success response:
            if (response.SmbHeader.Status == 0)
            {
                CifsClientPerSession session = new CifsClientPerSession();
                CifsClientPerTreeConnect tree = new CifsClientPerTreeConnect();
                CifsClientPerOpenFile openFile = new CifsClientPerOpenFile();
                CifsClientPerOpenSearch openSearch = new CifsClientPerOpenSearch();
                switch (response.SmbHeader.Command)
                {
                    #region connection
                    case SmbCommand.SMB_COM_NEGOTIATE:
                        #region SMB_COM_NEGOTIATE
                        SmbNegotiateRequestPacket negotiateRequest = request as SmbNegotiateRequestPacket;
                        SmbNegotiateResponsePacket negotiateResponse = response as SmbNegotiateResponsePacket;
                        if (negotiateRequest == null || negotiateResponse == null)
                        {
                            break;
                        }
                        // base class Connection:
                        connection.NegotiateReceived = true;
                        // common ADM:
                        int dialectIndex = (int)(negotiateResponse.SmbParameters.DialectIndex);
                        byte[] dialectBytes = negotiateRequest.SmbData.Dialects;
                        int startIndex = 0;
                        for (int i = 0; i < dialectIndex; i++)
                        {
                            startIndex = Array.IndexOf<byte>(dialectBytes, 0, startIndex,
                                dialectBytes.Length - startIndex) + 1;
                        }
                        connection.SelectedDialect = CifsMessageUtils.ToSmbString(dialectBytes, startIndex, true);
                        // client ADM:
                        connection.ShareLevelAccessControl = ((negotiateResponse.SmbParameters.SecurityMode
                            & SecurityModes.NEGOTIATE_ENCRYPT_PASSWORDS) == SecurityModes.NONE);
                        connection.ServerChallengeResponse = ((negotiateResponse.SmbParameters.SecurityMode
                            & SecurityModes.NEGOTIATE_ENCRYPT_PASSWORDS)
                            == SecurityModes.NEGOTIATE_ENCRYPT_PASSWORDS);
                        if (connection.ServerChallengeResponse
                            && ((negotiateResponse.SmbParameters.SecurityMode
                            & SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_ENABLED)
                            == SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_ENABLED))
                        {
                            connection.ServerSigningState = SignStateValue.ENABLED;
                        }
                        if (connection.ServerSigningState == SignStateValue.ENABLED
                           && ((negotiateResponse.SmbParameters.SecurityMode
                           & SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED)
                           == SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED))
                        {
                            connection.ServerSigningState = SignStateValue.REQUIRED;
                        }
                        connection.ServerCapabilities = negotiateResponse.SmbParameters.Capabilities;
                        connection.MaxBufferSize = negotiateResponse.SmbParameters.MaxBufferSize;
                        // negotiate response:
                        connection.SecurityMode = negotiateResponse.SmbParameters.SecurityMode;
                        connection.MaxMpxCount = negotiateResponse.SmbParameters.MaxMpxCount;
                        connection.MaxNumberVcs = negotiateResponse.SmbParameters.MaxNumberVcs;
                        connection.MaxRawSize = negotiateResponse.SmbParameters.MaxRawSize;
                        connection.SessionKey = negotiateResponse.SmbParameters.SessionKey;
                        connection.SystemTime = negotiateResponse.SmbParameters.SystemTime.Time;
                        connection.ServerTimeZone = negotiateResponse.SmbParameters.ServerTimeZone;
                        if (negotiateResponse.SmbData.Challenge != null
                            && negotiateResponse.SmbData.Challenge.Length >= 8)
                        {
                            connection.Challenge =
                                BitConverter.ToUInt64(negotiateResponse.SmbData.Challenge, 0);
                        }
                        if (negotiateResponse.SmbData.DomainName != null)
                        {
                            connection.DomainName = new byte[negotiateResponse.SmbData.DomainName.Length];
                            Array.Copy(negotiateResponse.SmbData.DomainName, connection.DomainName,
                                negotiateResponse.SmbData.DomainName.Length);
                        }
                        // stack sdk design:
                        connection.ConnectionState = StackTransportState.ConnectionEstablished;
                        // update:
                        this.AddOrUpdateConnection(connection);
                        #endregion
                        break;
                    #endregion

                    #region session
                    case SmbCommand.SMB_COM_SESSION_SETUP_ANDX:
                        #region SMB_COM_SESSION_SETUP_ANDX
                        SmbSessionSetupAndxRequestPacket sessionSetupRequest =
                            request as SmbSessionSetupAndxRequestPacket;
                        SmbSessionSetupAndxResponsePacket sessionSetupResponse =
                            response as SmbSessionSetupAndxResponsePacket;
                        if (sessionSetupRequest == null || sessionSetupResponse == null)
                        {
                            break;
                        }

                        session.ConnectionId = connectionId;
                        session.SessionId = (ulong)sessionSetupResponse.SmbHeader.Uid;
                        // in request:
                        session.SessionKey = sessionSetupRequest.ImplicitNtlmSessionKey;
                        session.MaxBufferSize = sessionSetupRequest.SmbParameters.MaxBufferSize;
                        session.MaxMpxCount = sessionSetupRequest.SmbParameters.MaxMpxCount;
                        session.VcNumber = sessionSetupRequest.SmbParameters.VcNumber;
                        session.SessionKeyOfNegotiated = sessionSetupRequest.SmbParameters.SessionKey;
                        session.Capabilities = sessionSetupRequest.SmbParameters.Capabilities;
                        if ((sessionSetupRequest.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                            SmbFlags2.SMB_FLAGS2_UNICODE)
                        {
                            session.UserAccount = new CifsUserAccount(
                                Encoding.Unicode.GetString(sessionSetupRequest.SmbData.PrimaryDomain),
                                Encoding.Unicode.GetString(sessionSetupRequest.SmbData.AccountName),
                                Encoding.Unicode.GetString(sessionSetupRequest.SmbData.OEMPassword));
                            session.ClientNativeOs =
                                Encoding.Unicode.GetString(sessionSetupRequest.SmbData.NativeOS);
                            session.ClientNativeLanMan =
                                Encoding.Unicode.GetString(sessionSetupRequest.SmbData.NativeLanMan);
                        }
                        else
                        {
                            session.UserAccount = new CifsUserAccount(
                                Encoding.ASCII.GetString(sessionSetupRequest.SmbData.PrimaryDomain),
                                Encoding.ASCII.GetString(sessionSetupRequest.SmbData.AccountName),
                                Encoding.ASCII.GetString(sessionSetupRequest.SmbData.OEMPassword));
                            session.ClientNativeOs =
                                Encoding.ASCII.GetString(sessionSetupRequest.SmbData.NativeOS);
                            session.ClientNativeLanMan =
                                Encoding.ASCII.GetString(sessionSetupRequest.SmbData.NativeLanMan);
                        }
                        // in response:
                        session.Action = sessionSetupResponse.SmbParameters.Action;
                        if ((sessionSetupResponse.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                            SmbFlags2.SMB_FLAGS2_UNICODE)
                        {
                            session.ServerNativeOs =
                                Encoding.Unicode.GetString(sessionSetupResponse.SmbData.NativeOS);
                            session.ServerNativeLanMan =
                                Encoding.Unicode.GetString(sessionSetupResponse.SmbData.NativeLanMan);
                            session.ServerPrimaryDomain =
                                Encoding.Unicode.GetString(sessionSetupResponse.SmbData.PrimaryDomain);
                        }
                        else
                        {
                            session.ServerNativeOs =
                                Encoding.ASCII.GetString(sessionSetupResponse.SmbData.NativeOS);
                            session.ServerNativeLanMan =
                                Encoding.ASCII.GetString(sessionSetupResponse.SmbData.NativeLanMan);
                            session.ServerPrimaryDomain =
                                Encoding.ASCII.GetString(sessionSetupResponse.SmbData.PrimaryDomain);
                        }

                        if (connection.ShareLevelAccessControl == false)
                        {
                            connection.IsSigningActive = true;
                            connection.ConnectionSigningSessionKey = sessionSetupRequest.ImplicitNtlmSessionKey;
                            connection.ConnectionSigningChallengeResponse = sessionSetupRequest.SmbData.UnicodePassword;
                            AddOrUpdateConnection(connection);
                        }

                        // update:
                        this.AddOrUpdateSession(session);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_LOGOFF_ANDX:
                        #region SMB_COM_LOGOFF_ANDX
                        SmbLogoffAndxResponsePacket logoffResponse = response as SmbLogoffAndxResponsePacket;
                        if (logoffResponse == null)
                        {
                            break;
                        }
                        this.RemoveSession(connectionId, (ulong)logoffResponse.SmbHeader.Uid);
                        #endregion
                        break;
                    #endregion

                    #region treeconnect
                    case SmbCommand.SMB_COM_TREE_CONNECT:
                        #region SMB_COM_TREE_CONNECT
                        SmbTreeConnectRequestPacket treeConnectRequest =
                            request as SmbTreeConnectRequestPacket;
                        SmbTreeConnectResponsePacket treeConnectResponse =
                            response as SmbTreeConnectResponsePacket;
                        if (treeConnectRequest == null || treeConnectResponse == null)
                        {
                            break;
                        }
                        tree.ConnectionId = connectionId;
                        tree.SessionId = (ulong)treeConnectResponse.SmbHeader.Uid;
                        tree.TreeId = (ulong)treeConnectResponse.SmbHeader.Tid;
                        tree.ShareName = CifsMessageUtils.ToSmbString(treeConnectRequest.SmbData.Path, 0, false);
                        int index = tree.ShareName.LastIndexOf(@"\");
                        if (index > 0)
                        {
                            tree.Share = tree.ShareName.Substring(index + 1);
                        }
                        else
                        {
                            tree.Share = tree.ShareName;
                        }
                        this.AddOrUpdateTreeConnect(tree);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_TREE_CONNECT_ANDX:
                        #region SMB_COM_TREE_CONNECT_ANDX
                        SmbTreeConnectAndxRequestPacket treeConnectAndxRequest =
                            request as SmbTreeConnectAndxRequestPacket;
                        SmbTreeConnectAndxResponsePacket treeConnectAndxResponse =
                            response as SmbTreeConnectAndxResponsePacket;
                        if (treeConnectAndxRequest == null || treeConnectAndxResponse == null)
                        {
                            break;
                        }
                        tree.ConnectionId = connectionId;
                        tree.SessionId = (ulong)treeConnectAndxResponse.SmbHeader.Uid;
                        tree.TreeId = (ulong)treeConnectAndxResponse.SmbHeader.Tid;
                        if ((treeConnectAndxRequest.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                            SmbFlags2.SMB_FLAGS2_UNICODE)
                        {
                            tree.ShareName = Encoding.Unicode.GetString(treeConnectAndxRequest.SmbData.Path);
                        }
                        else
                        {
                            tree.ShareName = Encoding.ASCII.GetString(treeConnectAndxRequest.SmbData.Path);
                        }
                        int IndexOfShare = tree.ShareName.LastIndexOf(@"\");
                        if (IndexOfShare > 0)
                        {
                            tree.Share = tree.ShareName.Substring(IndexOfShare + 1);
                        }
                        else
                        {
                            tree.Share = tree.ShareName;
                        }
                        this.AddOrUpdateTreeConnect(tree);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_TREE_DISCONNECT:
                        #region SMB_COM_TREE_DISCONNECT
                        SmbTreeDisconnectResponsePacket treeDisconnectResponse =
                            response as SmbTreeDisconnectResponsePacket;
                        if (treeDisconnectResponse == null)
                        {
                            break;
                        }
                        this.RemoveTreeConnect(connectionId,
                            (ulong)treeDisconnectResponse.SmbHeader.Uid,
                            (ulong)treeDisconnectResponse.SmbHeader.Tid);
                        #endregion
                        break;
                    #endregion

                    #region openfile
                    case SmbCommand.SMB_COM_OPEN:
                        #region SMB_COM_OPEN
                        SmbOpenRequestPacket openRequest = request as SmbOpenRequestPacket;
                        SmbOpenResponsePacket openResponse = response as SmbOpenResponsePacket;
                        if (openRequest == null || openResponse == null)
                        {
                            break;
                        }
                        openFile.ConnectionId = connectionId;
                        openFile.SessionId = (ulong)openResponse.SmbHeader.Uid;
                        openFile.TreeConnectId = (ulong)openResponse.SmbHeader.Tid;
                        openFile.FileHandle = openResponse.SmbParameters.FID;
                        openFile.FileName = CifsMessageUtils.ToSmbString(openRequest.SmbData.FileName, 0, false);
                        this.AddOrUpdateOpenFile(openFile);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_CREATE:
                        #region SMB_COM_CREATE
                        SmbCreateRequestPacket createRequest = request as SmbCreateRequestPacket;
                        SmbCreateResponsePacket createResponse = response as SmbCreateResponsePacket;
                        if (createRequest == null || createResponse == null)
                        {
                            break;
                        }
                        openFile.ConnectionId = connectionId;
                        openFile.SessionId = (ulong)createResponse.SmbHeader.Uid;
                        openFile.TreeConnectId = (ulong)createResponse.SmbHeader.Tid;
                        openFile.FileHandle = createResponse.SmbParameters.FID;
                        openFile.FileName = CifsMessageUtils.ToSmbString(createRequest.SmbData.FileName, 0, false);
                        this.AddOrUpdateOpenFile(openFile);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_CREATE_TEMPORARY:
                        #region SMB_COM_CREATE_TEMPORARY
                        SmbCreateTemporaryRequestPacket createTemporaryRequest =
                            request as SmbCreateTemporaryRequestPacket;
                        SmbCreateTemporaryResponsePacket createTemporaryResponse =
                            response as SmbCreateTemporaryResponsePacket;
                        if (createTemporaryRequest == null || createTemporaryResponse == null)
                        {
                            break;
                        }
                        openFile.ConnectionId = connectionId;
                        openFile.SessionId = (ulong)createTemporaryResponse.SmbHeader.Uid;
                        openFile.TreeConnectId = (ulong)createTemporaryResponse.SmbHeader.Tid;
                        openFile.FileHandle = createTemporaryResponse.SmbParameters.FID;
                        openFile.FileName = CifsMessageUtils.ToSmbString(
                            createTemporaryResponse.SmbData.TemporaryFileName, 0, false);
                        this.AddOrUpdateOpenFile(openFile);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_CREATE_NEW:
                        #region SMB_COM_CREATE_NEW
                        SmbCreateNewRequestPacket createNewRequest = request as SmbCreateNewRequestPacket;
                        SmbCreateNewResponsePacket createNewResponse = response as SmbCreateNewResponsePacket;
                        if (createNewRequest == null || createNewResponse == null)
                        {
                            break;
                        }
                        openFile.ConnectionId = connectionId;
                        openFile.SessionId = (ulong)createNewResponse.SmbHeader.Uid;
                        openFile.TreeConnectId = (ulong)createNewResponse.SmbHeader.Tid;
                        openFile.FileHandle = createNewResponse.SmbParameters.FID;
                        openFile.FileName = CifsMessageUtils.ToSmbString(createNewRequest.SmbData.FileName, 0, false);
                        this.AddOrUpdateOpenFile(openFile);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_OPEN_ANDX:
                        #region SMB_COM_OPEN_ANDX
                        SmbOpenAndxRequestPacket openAndxRequest = request as SmbOpenAndxRequestPacket;
                        SmbOpenAndxResponsePacket openAndxResponse = response as SmbOpenAndxResponsePacket;
                        if (openAndxRequest == null || openAndxResponse == null)
                        {
                            break;
                        }
                        openFile.ConnectionId = connectionId;
                        openFile.SessionId = (ulong)openAndxResponse.SmbHeader.Uid;
                        openFile.TreeConnectId = (ulong)openAndxResponse.SmbHeader.Tid;
                        openFile.FileHandle = openAndxResponse.SmbParameters.FID;
                        if ((openAndxRequest.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                            SmbFlags2.SMB_FLAGS2_UNICODE)
                        {
                            openFile.FileName = Encoding.Unicode.GetString(openAndxRequest.SmbData.FileName);
                        }
                        else
                        {
                            openFile.FileName = Encoding.ASCII.GetString(openAndxRequest.SmbData.FileName);
                        }
                        this.AddOrUpdateOpenFile(openFile);
                        #endregion

                        //save FID for chained response like:
                        //treeConnect->openAndx->readAndx->close
                        //when "close", FID is need to close the open opened in openAndx.
                        if (openAndxResponse.AndxPacket != null)
                        {
                            //borrow smbHeader.Protocol to save FID for later process.
                            //smbHeader.Protocol also use a flag to differentiate a single packet from a
                            //batched andx packet.
                            //FID is ushort, impossible to impact smbHeader.Protocol's usage as 
                            //a real packet header 0x424D53FF(0xFF, 'S', 'M', 'B')
                            SmbHeader andxHeader = openAndxResponse.AndxPacket.SmbHeader;
                            andxHeader.Protocol = openAndxResponse.SmbParameters.FID;
                            openAndxResponse.AndxPacket.SmbHeader = andxHeader;
                        }
                        break;

                    case SmbCommand.SMB_COM_NT_CREATE_ANDX:
                        #region SMB_COM_NT_CREATE_ANDX
                        SmbNtCreateAndxRequestPacket ntCreateAndxRequest =
                            request as SmbNtCreateAndxRequestPacket;
                        SmbNtCreateAndxResponsePacket ntCreateAndxResponse =
                            response as SmbNtCreateAndxResponsePacket;
                        if (ntCreateAndxRequest == null || ntCreateAndxResponse == null)
                        {
                            break;
                        }
                        openFile.ConnectionId = connectionId;
                        openFile.SessionId = (ulong)ntCreateAndxResponse.SmbHeader.Uid;
                        openFile.TreeConnectId = (ulong)ntCreateAndxResponse.SmbHeader.Tid;
                        openFile.FileHandle = ntCreateAndxResponse.SmbParameters.FID;
                        if ((ntCreateAndxRequest.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                            SmbFlags2.SMB_FLAGS2_UNICODE)
                        {
                            openFile.FileName = Encoding.Unicode.GetString(ntCreateAndxRequest.SmbData.FileName);
                        }
                        else
                        {
                            openFile.FileName = Encoding.ASCII.GetString(ntCreateAndxRequest.SmbData.FileName);
                        }
                        this.AddOrUpdateOpenFile(openFile);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_OPEN_PRINT_FILE:
                        #region SMB_COM_OPEN_PRINT_FILE
                        SmbOpenPrintFileRequestPacket openPrintFileRequest =
                            request as SmbOpenPrintFileRequestPacket;
                        SmbOpenPrintFileResponsePacket openPrintFileResponse =
                            response as SmbOpenPrintFileResponsePacket;
                        if (openPrintFileRequest == null || openPrintFileResponse == null)
                        {
                            break;
                        }
                        openFile.ConnectionId = connectionId;
                        openFile.SessionId = (ulong)openPrintFileResponse.SmbHeader.Uid;
                        openFile.TreeConnectId = (ulong)openPrintFileResponse.SmbHeader.Tid;
                        openFile.FileHandle = openPrintFileResponse.SmbParameters.FID;
                        openFile.FileName = CifsMessageUtils.ToSmbString(
                            openPrintFileRequest.SmbData.Identifier, 0, false);
                        this.AddOrUpdateOpenFile(openFile);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_TRANSACTION2:
                        #region Trans2Open2
                        SmbTrans2Open2RequestPacket trans2Open2Request =
                            request as SmbTrans2Open2RequestPacket;
                        SmbTrans2Open2FinalResponsePacket trans2Open2Response =
                            response as SmbTrans2Open2FinalResponsePacket;
                        if (trans2Open2Request != null && trans2Open2Response != null)
                        {
                            openFile.ConnectionId = connectionId;
                            openFile.SessionId = (ulong)trans2Open2Response.SmbHeader.Uid;
                            openFile.TreeConnectId = (ulong)trans2Open2Response.SmbHeader.Tid;
                            openFile.FileHandle = trans2Open2Response.Trans2Parameters.Fid;
                            if ((trans2Open2Request.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                                SmbFlags2.SMB_FLAGS2_UNICODE)
                            {
                                openFile.FileName = Encoding.Unicode.GetString(
                                    trans2Open2Request.Trans2Parameters.FileName);
                            }
                            else
                            {
                                openFile.FileName = Encoding.ASCII.GetString(
                                    trans2Open2Request.Trans2Parameters.FileName);
                            }
                            this.AddOrUpdateOpenFile(openFile);
                            break;
                        }
                        #endregion

                        #region Trans2FindFirst2
                        SmbTrans2FindFirst2RequestPacket trans2FindFirst2Request =
                            request as SmbTrans2FindFirst2RequestPacket;
                        SmbTrans2FindFirst2FinalResponsePacket trans2FindFirst2Response =
                            response as SmbTrans2FindFirst2FinalResponsePacket;
                        if (trans2FindFirst2Request != null && trans2FindFirst2Response != null)
                        {
                            openSearch.ConnectionId = connectionId;
                            openSearch.SessionId = (ulong)trans2FindFirst2Response.SmbHeader.Uid;
                            openSearch.TreeConnectId = (ulong)trans2FindFirst2Response.SmbHeader.Tid;
                            openSearch.SearchID = trans2FindFirst2Response.Trans2Parameters.SID;
                            if ((trans2FindFirst2Request.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                                SmbFlags2.SMB_FLAGS2_UNICODE)
                            {
                                openSearch.SearchName = Encoding.Unicode.GetString(
                                    trans2FindFirst2Request.Trans2Parameters.FileName);
                            }
                            else
                            {
                                openSearch.SearchName = Encoding.ASCII.GetString(
                                    trans2FindFirst2Request.Trans2Parameters.FileName);
                            }
                            this.AddOrUpdateOpenSearch(openSearch);
                            break;
                        }
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_WRITE_AND_CLOSE:
                        #region SMB_COM_WRITE_AND_CLOSE
                        SmbWriteAndCloseRequestPacket writeAndCloseRequest =
                            request as SmbWriteAndCloseRequestPacket;
                        if (writeAndCloseRequest == null)
                        {
                            break;
                        }
                        this.RemoveOpenFile(connectionId, (ulong)response.SmbHeader.Uid,
                            (ulong)response.SmbHeader.Tid, writeAndCloseRequest.SmbParameters.FID);
                        #endregion
                        break;

                    case SmbCommand.SMB_COM_CLOSE:
                        #region SMB_COM_CLOSE
                        // Get FID from CLOSE request. 
                        // If fail, then get FID from the Batched Request.
                        // If still fail, then Get FID from the Batched Response.
                        ushort closeFId = 0;
                        SmbCloseRequestPacket closeRequest = request as SmbCloseRequestPacket;
                        if (closeRequest != null)
                        {
                            //Neither SMB_PROTOCOL_IDENTIFIER, or SMB_PROTOCOL_ANDXPACKET, then must be a FID
                            if (response.SmbHeader.Protocol != CifsMessageUtils.SMB_PROTOCOL_IDENTIFIER
                                && response.SmbHeader.Protocol != CifsMessageUtils.SMB_PROTOCOL_ANDXPACKET)
                            {
                                closeFId = (ushort)response.SmbHeader.Protocol;
                            }
                            else
                            {
                                closeFId = closeRequest.SmbParameters.FID;
                            }
                            this.RemoveOpenFile(connectionId, (ulong)response.SmbHeader.Uid,
                                (ulong)response.SmbHeader.Tid, closeFId);
                        }
                        
                        #endregion
                        break;
                    #endregion

                    #region opensearch
                    case SmbCommand.SMB_COM_FIND_CLOSE2:
                        #region SMB_COM_FIND_CLOSE2
                        SmbFindClose2RequestPacket findClose2Request =
                            request as SmbFindClose2RequestPacket;
                        if (findClose2Request == null)
                        {
                            break;
                        }
                        this.RemoveOpenSearch(connectionId, (ulong)response.SmbHeader.Uid,
                            (ulong)response.SmbHeader.Tid, findClose2Request.SmbParameters.SearchHandle);
                        #endregion
                        break;
                    #endregion

                    default:
                        // No Connection/Session/Tree/Open will be updated if other types of response.
                        break;
                }

                SmbBatchedRequestPacket smbBatchedRequest = request as SmbBatchedRequestPacket;
                SmbBatchedResponsePacket smbBatchedResponse = response as SmbBatchedResponsePacket;

                if (smbBatchedRequest != null && smbBatchedResponse != null)
                {
                    //pass the FID stored in the andxHeader.Protocol into response.AndxPacket
                    if (smbBatchedRequest.AndxPacket != null && smbBatchedResponse.AndxPacket != null
                        && response.SmbHeader.Protocol != CifsMessageUtils.SMB_PROTOCOL_ANDXPACKET
                        && response.SmbHeader.Protocol != CifsMessageUtils.SMB_PROTOCOL_IDENTIFIER)
                    {
                        SmbHeader andxHeader = smbBatchedResponse.AndxPacket.SmbHeader;
                        andxHeader.Protocol = smbBatchedResponse.SmbHeader.Protocol;
                        smbBatchedResponse.AndxPacket.SmbHeader = andxHeader;
                    }
                    this.ResponsePacketUpdateRoleContext(connection, smbBatchedRequest.AndxPacket,
                        smbBatchedResponse.AndxPacket);
                }
            }
            #endregion
        }


        /// <summary>
        /// this function will be invoked every time a packet is sent or received. all logics about 
        /// Client' states will be implemented here.
        /// response packet update the context, do regular transaction.
        /// </summary>
        /// <param name="connection">the connection object.</param>
        /// <param name="packet">the sent or received packet in stack transport.</param>
        protected virtual void ResponsePacketUpdateRoleContextRegular(Connection connection, SmbPacket packet)
        {
            int connectionId = connection.ConnectionId;

            #region RemoveOutstandingRequest
            if (packet.SmbHeader.Status != 0
                ||!(packet is SmbTransactionInterimResponsePacket
                    || packet is SmbTransaction2InterimResponsePacket
                    || packet is SmbNtTransactInterimResponsePacket
                    || packet is SmbWriteRawInterimResponsePacket))
            {
                this.RemoveOutstandingRequest(connectionId, (ulong)packet.SmbHeader.Mid);
            }
            #endregion

            #region RemoveSequenceNumber

            this.RemoveSequenceNumber(connectionId, packet);
            #endregion
        }

        #endregion


        #region Access the command sequence window of a connection

        /// <summary>
        /// Get the min usable MessageId in the command sequence window of a connection.
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        /// <returns>if the connection exists, returns min usable MessageId of the connection. otherwise
        /// 0.</returns>
        public ushort GetMessageId(int connectionId)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        ushort messageId =
                            (this.contextCollection.ConnectionList[i] as CifsClientPerConnection).MessageId;
                        this.UpdateSequenceWindow(connectionId);
                        return messageId;
                    }
                }
            }
            return 0;
        }


        /// <summary>
        /// update the sequence window of a connection.
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        private void UpdateSequenceWindow(int connectionId)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        (this.contextCollection.ConnectionList[i] as
                            CifsClientPerConnection).UpdateSequenceWindow();
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// update the sequence number of a connection.
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        /// <param name="msg">the last request packet.</param>
        private void UpdateSequenceNumber(int connectionId, SmbPacket msg)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        (this.contextCollection.ConnectionList[i] as
                            CifsClientPerConnection).UpdateSequenceNumber(msg);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// Remove the sequence number from a connection.
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        /// <param name="msg">the last response packet.</param>
        private void RemoveSequenceNumber(int connectionId, SmbPacket msg)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        (this.contextCollection.ConnectionList[i] as
                            CifsClientPerConnection).RemoveSequenceNumber(msg);
                        return;
                    }
                }
            }
        }

        #endregion


        #region Access the Outstanding Request table of a connection


        /// <summary>
        /// add a request into the table of OutstandingRequests.
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        /// <param name="packet">the Outstanding Request.</param>
        public void AddOutstandingRequest(int connectionId, SmbPacket packet)
        {
            if (packet == null)
            {
                return;
            }
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        (this.contextCollection.ConnectionList[i] as
                            CifsClientPerConnection).AddOutstandingRequest(packet);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// remove a request from the table of OutstandingRequests.
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        /// <param name="mid">the messageId of the request to be removed.</param>
        public void RemoveOutstandingRequest(int connectionId, ulong mid)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        (this.contextCollection.ConnectionList[i] as
                            CifsClientPerConnection).RemoveOutstandingRequest(mid);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// get a request from the table of OutstandingRequests.
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        /// <returns>a copy of the outstanding request will be returned.</returns>
        public Collection<SmbPacket> GetOutstandingRequests(int connectionId)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        return (this.contextCollection.ConnectionList[i] as
                            CifsClientPerConnection).GetOutstandingRequests();
                    }
                }
            }
            return new Collection<SmbPacket>();
        }


        /// <summary>
        /// get a request from the table of OutstandingRequests.
        /// </summary>
        /// <param name="connectionId">the connection identity.</param>
        /// <param name="mid">the messageId of the request to get.</param>
        /// <returns>if found, a copy of the outstanding request will be returned. 
        /// otherwise, null will be returned.</returns>
        public SmbPacket GetOutstandingRequest(int connectionId, ulong mid)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        return (this.contextCollection.ConnectionList[i] as
                            CifsClientPerConnection).GetOutstandingRequest(mid);
                    }
                }
            }
            return null;
        }

        #endregion


        #region Access connectionList


        /// <summary>
        /// if the connection identified by connectionId has been existed in connectionList, 
        /// this connection will be updated into the connectionList. otherwise, this connection
        /// will be added into the connectionList.
        /// </summary>
        /// <param name="connection">the connection to be added or updated.</param>
        public void AddOrUpdateConnection(CifsClientPerConnection connection)
        {
            if (connection == null)
            {
                return;
            }
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connection.ConnectionId)
                    {
                        // update the connection:
                        this.contextCollection.ConnectionList[i] = connection;
                        return;
                    }
                }
                // add the connection:
                connection.GlobalIndex = this.contextCollection.NextConnectionGlobalIndex;
                this.contextCollection.ConnectionList.Add(connection);
                this.contextCollection.NextConnectionGlobalIndex += 1;
            }
        }


        /// <summary>
        /// remove all connections from the connectionList.
        /// </summary>
        public void RemoveAllConnections()
        {
            lock (this.contextCollection)
            {
                RemoveAllSessions();
                this.contextCollection.ConnectionList.Clear();
            }
        }


        /// <summary>
        /// remove a connection identified by connectionId from the connectionList.
        /// </summary>
        /// <param name="connectionId">the identity of the connection to be removed.</param>
        public void RemoveConnection(int connectionId)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                    {
                        RemoveSessions(connectionId);
                        this.contextCollection.ConnectionList.RemoveAt(i);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// remove a connection identified by globalIndex from the connectionList.
        /// </summary>
        /// <param name="globalIndex">the globalIndex of the connection to be removed.</param>
        public void RemoveConnectionByGlobalIndex(int globalIndex)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.ConnectionList[i].GlobalIndex == globalIndex)
                    {
                        RemoveSessions(this.contextCollection.ConnectionList[i].ConnectionId);
                        this.contextCollection.ConnectionList.RemoveAt(i);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// get all connections in the connectionList. 
        /// </summary>
        /// <returns>A snapshot of the connectionList will be returned.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public ReadOnlyCollection<CifsClientPerConnection> GetConnections()
        {
            Collection<CifsClientPerConnection> ret = new Collection<CifsClientPerConnection>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerConnection connection in this.contextCollection.ConnectionList)
                {
                    ret.Add(connection.Clone());
                }
            }
            return new ReadOnlyCollection<CifsClientPerConnection>(ret);
        }


        /// <summary>
        /// get connection identified by connectionId from the connectionList. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the connection to get.</param>
        /// <returns>if found, A copy of the connection in the connectionList will be returned.
        /// otherwise, null will be returned.</returns>
        public CifsClientPerConnection GetConnection(int connectionId)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerConnection connection in this.contextCollection.ConnectionList)
                {
                    if (connection.ConnectionId == connectionId)
                    {
                        return connection.Clone();
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// get connection identified by the globalIndex in the connectionList. 
        /// </summary>
        /// <param name="globalIndex">the globalIndex of the connection to get.</param>
        /// <returns>A snapshot of the found connection will be returned.</returns>
        public CifsClientPerConnection GetConnectionByGlobalIndex(int globalIndex)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerConnection connection in this.contextCollection.ConnectionList)
                {
                    if (connection.GlobalIndex == globalIndex)
                    {
                        return connection.Clone();
                    }
                }
            }
            return null;
        }

        #endregion


        #region Access globalSessionTable


        /// <summary>
        /// if the session identified by the connectionId and sessionId has been existed in the
        /// globalSessionTable, this session will be updated into the globalSessionTable.
        /// otherwise, this session will be added into the globalSessionTable.
        /// </summary>
        /// <param name="session">the session to be added or updated.</param>
        /// <exception cref="System.InvalidOperationException">Failed in AddOrUpdateSession 
        /// because the Connection of the Session does not found.</exception>
        public void AddOrUpdateSession(CifsClientPerSession session)
        {
            if (session == null)
            {
                return;
            }

            lock (this.contextCollection)
            {
                if (AddOrUpdateSessionTableInConnection(session))
                {
                    AddOrUpdateSessionTableInGlobal(session);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Failed in AddOrUpdateSession because the Connection of the Session doesnot found.");

                }
            }
        }


        /// <summary>
        /// to add or update session into connection.
        /// </summary>
        /// <param name="session">the session to be added.</param>
        /// <returns>true: added successfully. false:the connection does not existed.</returns>
        private bool AddOrUpdateSessionTableInConnection(CifsClientPerSession session)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                // if the connection exists, add or update the session:
                if (this.contextCollection.ConnectionList[i].ConnectionId == session.ConnectionId)
                {
                    CifsClientPerConnection connection =
                    this.contextCollection.ConnectionList[i] as CifsClientPerConnection;

                    // if the session does exist, update it:
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == session.SessionId)
                        {
                            // update the session:
                            connection.SessionTable[j] = session;
                            return true;
                        }
                    }
                    // add the session:
                    session.GlobalIndex = this.contextCollection.NextSessionGlobalIndex;
                    connection.SessionTable.Add(session);
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// to add or update session into global session table.
        /// </summary>
        /// <param name="session">the session to be added.</param>
        private void AddOrUpdateSessionTableInGlobal(CifsClientPerSession session)
        {
            for (int i = this.contextCollection.GlobalSessionTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalSessionTable[i].ConnectionId == session.ConnectionId
                    && this.contextCollection.GlobalSessionTable[i].SessionId == session.SessionId)
                {
                    // update the session:
                    this.contextCollection.GlobalSessionTable[i] = session;
                    return;
                }
            }
            // add the session:
            session.GlobalIndex = this.contextCollection.NextSessionGlobalIndex;
            this.contextCollection.GlobalSessionTable.Add(session);
            this.contextCollection.NextSessionGlobalIndex += 1;
        }


        /// <summary>
        /// remove all sessions from the globalSessionTable.
        /// </summary>
        public void RemoveAllSessions()
        {
            lock (this.contextCollection)
            {
                RemoveTreeConnects();
                RemoveAllSessionsFromConnection();
                RemoveAllSessionsFromGlobal();
            }
        }


        /// <summary>
        /// to remove all sessions from connection.
        /// </summary>
        private void RemoveAllSessionsFromConnection()
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection =
                this.contextCollection.ConnectionList[i] as CifsClientPerConnection;
                connection.SessionTable.Clear();
            }
        }


        /// <summary>
        /// to remove all sessions from global.
        /// </summary>
        private void RemoveAllSessionsFromGlobal()
        {
            this.contextCollection.GlobalSessionTable.Clear();
        }


        /// <summary>
        /// remove sessions in a connection identified by the connectionId from the globalSessionTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the session to be removed.</param>
        public void RemoveSessions(int connectionId)
        {
            lock (this.contextCollection)
            {
                RemoveTreeConnects(connectionId);
                RemoveSessionsFromConnection(connectionId);
                RemoveSessionsFromGlobal(connectionId);
            }
        }


        /// <summary>
        /// to remove sessions from connection.
        /// </summary>
        /// <param name="connectionId">the connectionId to be removed.</param>
        private void RemoveSessionsFromConnection(int connectionId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection =
                    this.contextCollection.ConnectionList[i] as CifsClientPerConnection;
                    connection.SessionTable.Clear();
                    return;
                }
            }
        }


        /// <summary>
        /// to remove sessions from global.
        /// </summary>
        /// <param name="connectionId">the connectionId to be removed.</param>
        private void RemoveSessionsFromGlobal(int connectionId)
        {
            for (int i = this.contextCollection.GlobalSessionTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalSessionTable[i].ConnectionId == connectionId)
                {
                    this.contextCollection.GlobalSessionTable.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// remove session identified by the connectionId and sessionId from the globalSessionTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the session to be removed.</param>
        /// <param name="sessionId">the sessionId of the session to be removed.</param>
        public void RemoveSession(int connectionId, ulong sessionId)
        {
            lock (this.contextCollection)
            {
                RemoveTreeConnects(connectionId, sessionId);
                RemoveSessionFromConnection(connectionId, sessionId);
                RemoveSessionFromGlobal(connectionId, sessionId);
            }
        }


        /// <summary>
        /// to remove session from connection.
        /// </summary>
        /// <param name="connectionId">the connectionId of the session to be removed.</param>
        /// <param name="sessionId">the sessionId of the session to be removed.</param>
        private void RemoveSessionFromConnection(int connectionId, ulong sessionId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection =
                    this.contextCollection.ConnectionList[i] as CifsClientPerConnection;

                    // if the session does exist, remove it:
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == sessionId)
                        {
                            connection.SessionTable.RemoveAt(j);
                            break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove session from global table.
        /// </summary>
        /// <param name="connectionId">the connectionId of the session to be removed.</param>
        /// <param name="sessionId">the sessionId of the session to be removed.</param>
        private void RemoveSessionFromGlobal(int connectionId, ulong sessionId)
        {
            for (int i = this.contextCollection.GlobalSessionTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalSessionTable[i].ConnectionId == connectionId
                    && this.contextCollection.GlobalSessionTable[i].SessionId == sessionId)
                {
                    this.contextCollection.GlobalSessionTable.RemoveAt(i);
                    return;
                }
            }
        }


        /// <summary>
        /// remove a session identified by the globalIndex from the globalSessionTable.
        /// </summary>
        /// <param name="globalIndex">the globalIndex of the session to be removed.</param>
        public void RemoveSession(int globalIndex)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.GlobalSessionTable.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.GlobalSessionTable[i].GlobalIndex == globalIndex)
                    {
                        CifsClientPerSession session = this.contextCollection.GlobalSessionTable[i]
                            as CifsClientPerSession;
                        RemoveTreeConnects(session.ConnectionId, session.SessionId);
                    }
                }
                RemoveSessionFromConnection(globalIndex);
                RemoveSessionFromGlobal(globalIndex);
            }
        }


        /// <summary>
        /// to Remove Session From Connection.
        /// </summary>
        /// <param name="globalIndex">the global Index of the session to be removed.</param>
        private void RemoveSessionFromConnection(int globalIndex)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection =
                this.contextCollection.ConnectionList[i] as CifsClientPerConnection;

                // if the session does exist, remove it:
                for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                {
                    if (connection.SessionTable[j].GlobalIndex == globalIndex)
                    {
                        connection.SessionTable.RemoveAt(j);
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// to Remove Session From Global table.
        /// </summary>
        /// <param name="globalIndex">the global Index of the session to be removed.</param>
        private void RemoveSessionFromGlobal(int globalIndex)
        {
            for (int i = this.contextCollection.GlobalSessionTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalSessionTable[i].GlobalIndex == globalIndex)
                {
                    this.contextCollection.GlobalSessionTable.RemoveAt(i);
                    return;
                }
            }
        }


        /// <summary>
        /// get all sessions in the globalSessionTable. 
        /// </summary>
        /// <returns>A snapshot of the globalSessionTable will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerSession> GetSessions()
        {
            Collection<CifsClientPerSession> ret = new Collection<CifsClientPerSession>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerSession session in this.contextCollection.GlobalSessionTable)
                {
                    ret.Add(session.Clone());
                }
            }
            return new ReadOnlyCollection<CifsClientPerSession>(ret);
        }


        /// <summary>
        /// get sessions of a connection in the globalSessionTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the session to get.</param>
        /// <returns>A snapshot of the found sessions will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerSession> GetSessions(int connectionId)
        {
            Collection<CifsClientPerSession> ret = new Collection<CifsClientPerSession>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerSession session in this.contextCollection.GlobalSessionTable)
                {
                    if (session.ConnectionId == connectionId)
                    {
                        ret.Add(session.Clone());
                    }
                }
            }
            return new ReadOnlyCollection<CifsClientPerSession>(ret);
        }


        /// <summary>
        /// get a session identified by the connectionId and sessionId in the globalSessionTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the session to get.</param>
        /// <param name="sessionId">the sessionId of the session to get.</param>
        /// <returns>A snapshot of the found session will be returned.</returns>
        public CifsClientPerSession GetSession(int connectionId, ulong sessionId)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerSession session in this.contextCollection.GlobalSessionTable)
                {
                    if (session.ConnectionId == connectionId
                        && session.SessionId == sessionId)
                    {
                        return session.Clone();
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// get session identified by the globalIndex in the globalSessionTable. 
        /// </summary>
        /// <param name="globalIndex">the globalIndex of the session to get.</param>
        /// <returns>A snapshot of the found session will be returned.</returns>
        public CifsClientPerSession GetSession(int globalIndex)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerSession session in this.contextCollection.GlobalSessionTable)
                {
                    if (session.GlobalIndex == globalIndex)
                    {
                        return session.Clone();
                    }
                }
            }
            return null;
        }

        #endregion


        #region Access globalTreeConnectTable


        /// <summary>
        /// if the treeConnect identified by the connectionId, sessionId and treeId has been existed in 
        /// globalTreeConnectTable, this treeConnect will be updated into the globalTreeConnectTable.
        /// otherwise, this treeConnect will be added into the globalTreeConnectTable.
        /// </summary>
        /// <param name="treeConnect">the treeConnect to be added or updated.</param>
        /// <exception cref="System.InvalidOperationException">Failed in AddOrUpdateTreeConnect
        /// because the Session or Connection of the TreeConnect does not found.</exception>
        public void AddOrUpdateTreeConnect(CifsClientPerTreeConnect treeConnect)
        {
            if (treeConnect == null)
            {
                return;
            }

            lock (this.contextCollection)
            {
                if (AddOrUpdateTreeConnectInSession(treeConnect))
                {
                    AddOrUpdateTreeConnectInGlobal(treeConnect);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Failed in AddOrUpdateTreeConnect because the Session or Connection of the  TreeConnect" +
                        " does not found.");

                }
            }
        }


        /// <summary>
        /// Add Or Update a TreeConnect in the TreeConnectTable of Session.
        /// </summary>
        /// <param name="treeConnect">the treeConnect to be added or updated.</param>
        /// <returns>if the connection exists, add or update the session and return true. otherwise return
        /// false.</returns>
        private bool AddOrUpdateTreeConnectInSession(CifsClientPerTreeConnect treeConnect)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                // if the connection exists, add or update the treeConnect:
                if (connection.ConnectionId == treeConnect.ConnectionId)
                {
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                        // if the session exists, add or update the treeConnect:
                        if (session.SessionId == treeConnect.SessionId)
                        {
                            // if the treeConnect exists, update it:
                            for (int k = session.TreeConnectTable.Count - 1; k >= 0; k--)
                            {
                                if (session.TreeConnectTable[k].TreeId == treeConnect.TreeId)
                                {
                                    // update the treeConnect:
                                    session.TreeConnectTable[k] = treeConnect;
                                    return true;
                                }
                            }
                            // add the treeConnect:
                            treeConnect.GlobalIndex = this.contextCollection.NextTreeConnectGlobalIndex;
                            session.TreeConnectTable.Add(treeConnect);
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// Add Or Update a TreeConnect in the GlobalTreeConnectTable.
        /// </summary>
        /// <param name="treeConnect">the treeConnect to be added or updated.</param>
        private void AddOrUpdateTreeConnectInGlobal(CifsClientPerTreeConnect treeConnect)
        {
            for (int i = this.contextCollection.GlobalTreeConnectTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalTreeConnectTable[i].ConnectionId == treeConnect.ConnectionId
                    && this.contextCollection.GlobalTreeConnectTable[i].SessionId == treeConnect.SessionId
                    && this.contextCollection.GlobalTreeConnectTable[i].TreeId == treeConnect.TreeId)
                {
                    // update the treeConnect:
                    this.contextCollection.GlobalTreeConnectTable[i] = treeConnect;
                    return;
                }
            }
            // add the treeConnect:
            treeConnect.GlobalIndex = this.contextCollection.NextTreeConnectGlobalIndex;
            this.contextCollection.GlobalTreeConnectTable.Add(treeConnect);
            this.contextCollection.NextTreeConnectGlobalIndex += 1;
        }


        /// <summary>
        /// remove all TreeConnects  from the globalTreeConnectTable.
        /// </summary>
        public void RemoveTreeConnects()
        {
            lock (this.contextCollection)
            {
                RemoveOpenFiles();
                RemoveOpenSearchs();
                RemoveTreeConnectsFromAllConnection();
                RemoveTreeConnectsFromGlobal();
            }
        }


        /// <summary>
        /// to remove all tree connects from connection.
        /// </summary>
        private void RemoveTreeConnectsFromAllConnection()
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection =
                this.contextCollection.ConnectionList[i] as CifsClientPerConnection;

                for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                {
                    CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                    session.TreeConnectTable.Clear();
                }
            }
        }


        /// <summary>
        /// to remove all tree connects from global.
        /// </summary>
        private void RemoveTreeConnectsFromGlobal()
        {
            this.contextCollection.GlobalTreeConnectTable.Clear();
        }


        /// <summary>
        /// remove TreeConnects in a connection identified by the connectionId from the globalTreeConnectTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the TreeConnect to be removed.</param>
        public void RemoveTreeConnects(int connectionId)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.GlobalTreeConnectTable.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.GlobalTreeConnectTable[i].ConnectionId == connectionId)
                    {
                        CifsClientPerTreeConnect treeConnect = this.contextCollection.GlobalTreeConnectTable[i]
                            as CifsClientPerTreeConnect;
                        RemoveOpenFiles(treeConnect.ConnectionId);
                        RemoveOpenSearchs(treeConnect.ConnectionId);
                    }
                }
                RemoveTreeConnectsFromConnection(connectionId);
                RemoveTreeConnectsFromGlobal(connectionId);
            }
        }


        /// <summary>
        /// to remove tree connect from connection.
        /// </summary>
        /// <param name="connectionId">the connectionId of tree to be removed.</param>
        private void RemoveTreeConnectsFromConnection(int connectionId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection =
                    this.contextCollection.ConnectionList[i] as CifsClientPerConnection;

                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                        session.TreeConnectTable.Clear();
                    }
                }
            }
        }


        /// <summary>
        /// to remove tree connect from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of tree to be removed.</param>
        private void RemoveTreeConnectsFromGlobal(int connectionId)
        {
            for (int i = this.contextCollection.GlobalTreeConnectTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalTreeConnectTable[i].ConnectionId == connectionId)
                {
                    this.contextCollection.GlobalTreeConnectTable.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// remove TreeConnects in a session identified by the connectionId, sessionId from the
        /// globalTreeConnectTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the TreeConnect to be removed.</param>
        /// <param name="sessionId">the sessionId of the TreeConnect to be removed.</param>
        public void RemoveTreeConnects(int connectionId, ulong sessionId)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.GlobalTreeConnectTable.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.GlobalTreeConnectTable[i].ConnectionId == connectionId
                        && this.contextCollection.GlobalTreeConnectTable[i].SessionId == sessionId)
                    {
                        CifsClientPerTreeConnect treeConnect = this.contextCollection.GlobalTreeConnectTable[i]
                            as CifsClientPerTreeConnect;
                        RemoveOpenFiles(treeConnect.ConnectionId, treeConnect.SessionId);
                        RemoveOpenSearchs(treeConnect.ConnectionId, treeConnect.SessionId);
                    }
                }

                RemoveTreeConnectsFromSession(connectionId, sessionId);
                RemoveTreeConnectsFromGlobal(connectionId, sessionId);
            }
        }


        /// <summary>
        /// to remove tree connect from session.
        /// </summary>
        /// <param name="connectionId">the connectionId of tree to be removed.</param>
        /// <param name="sessionId">the sessionId of tree to be removed.</param>
        private void RemoveTreeConnectsFromSession(int connectionId, ulong sessionId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection =
                    this.contextCollection.ConnectionList[i] as CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == sessionId)
                        {
                            CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                            session.TreeConnectTable.Clear();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove tree connect from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of tree to be removed.</param>
        /// <param name="sessionId">the sessionId of tree to be removed.</param>
        private void RemoveTreeConnectsFromGlobal(int connectionId, ulong sessionId)
        {
            for (int i = this.contextCollection.GlobalTreeConnectTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalTreeConnectTable[i].ConnectionId == connectionId
                    && this.contextCollection.GlobalTreeConnectTable[i].SessionId == sessionId)
                {
                    this.contextCollection.GlobalTreeConnectTable.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// remove a TreeConnect identified by the connectionId, sessionId and treeId from the globalTreeConnectTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the TreeConnect to be removed.</param>
        /// <param name="sessionId">the sessionId of the TreeConnect to be removed.</param>
        /// <param name="treeId">the treeId of the TreeConnect to be removed.</param>
        public void RemoveTreeConnect(int connectionId, ulong sessionId, ulong treeId)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.GlobalTreeConnectTable.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.GlobalTreeConnectTable[i].ConnectionId == connectionId
                        && this.contextCollection.GlobalTreeConnectTable[i].SessionId == sessionId
                        && this.contextCollection.GlobalTreeConnectTable[i].TreeId == treeId)
                    {
                        CifsClientPerTreeConnect treeConnect = this.contextCollection.GlobalTreeConnectTable[i]
                            as CifsClientPerTreeConnect;
                        RemoveOpenFiles(treeConnect.ConnectionId, treeConnect.SessionId, treeId);
                        RemoveOpenSearchs(treeConnect.ConnectionId, treeConnect.SessionId, treeId);
                    }
                }

                RemoveTreeConnectFromSession(connectionId, sessionId, treeId);
                RemoveTreeConnectFromGlobal(connectionId, sessionId, treeId);
            }
        }


        /// <summary>
        /// to remove tree connect from session.
        /// </summary>
        /// <param name="connectionId">the connectionId of tree to be removed.</param>
        /// <param name="sessionId">the sessionId of tree to be removed.</param>
        /// <param name="treeId">the treeId of tree to be removed.</param>
        private void RemoveTreeConnectFromSession(int connectionId, ulong sessionId, ulong treeId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == sessionId)
                        {
                            CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                            for (int k = session.TreeConnectTable.Count - 1; k >= 0; k--)
                            {
                                if (session.TreeConnectTable[k].TreeId == treeId)
                                {
                                    session.TreeConnectTable.RemoveAt(k);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove tree connect from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of tree to be removed.</param>
        /// <param name="sessionId">the sessionId of tree to be removed.</param>
        /// <param name="treeId">the treeId of tree to be removed.</param>
        private void RemoveTreeConnectFromGlobal(int connectionId, ulong sessionId, ulong treeId)
        {
            for (int i = this.contextCollection.GlobalTreeConnectTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalTreeConnectTable[i].ConnectionId == connectionId
                    && this.contextCollection.GlobalTreeConnectTable[i].SessionId == sessionId
                    && this.contextCollection.GlobalTreeConnectTable[i].TreeId == treeId)
                {
                    this.contextCollection.GlobalTreeConnectTable.RemoveAt(i);
                    return;
                }
            }
        }


        /// <summary>
        /// remove a TreeConnect identified by the globalIndex from the globalTreeConnectTable.
        /// </summary>
        /// <param name="globalindex">the globalIndex of the TreeConnect to be removed.</param>
        public void RemoveTreeConnect(int globalindex)
        {
            lock (this.contextCollection)
            {
                for (int i = this.contextCollection.GlobalTreeConnectTable.Count - 1; i >= 0; i--)
                {
                    if (this.contextCollection.GlobalTreeConnectTable[i].GlobalIndex == globalindex)
                    {
                        CifsClientPerTreeConnect treeConnect = this.contextCollection.GlobalTreeConnectTable[i]
                            as CifsClientPerTreeConnect;
                        RemoveOpenFiles(treeConnect.ConnectionId, treeConnect.SessionId, treeConnect.TreeId);
                        RemoveOpenSearchs(treeConnect.ConnectionId, treeConnect.SessionId, treeConnect.TreeId);
                    }
                }

                RemoveTreeConnectFromSession(globalindex);
                RemoveTreeConnectFromGlobal(globalindex);
            }
        }


        /// <summary>
        /// to remove tree connect from session.
        /// </summary>
        /// <param name="globalindex">the globalindex of tree to be removed.</param>
        private void RemoveTreeConnectFromSession(int globalindex)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                {
                    CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                    for (int k = session.TreeConnectTable.Count - 1; k >= 0; k--)
                    {
                        if (session.TreeConnectTable[k].GlobalIndex == globalindex)
                        {
                            session.TreeConnectTable.RemoveAt(k);
                            return;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove tree connect from global.
        /// </summary>
        /// <param name="globalindex">the globalindex of tree to be removed.</param>
        private void RemoveTreeConnectFromGlobal(int globalindex)
        {
            for (int i = this.contextCollection.GlobalTreeConnectTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalTreeConnectTable[i].GlobalIndex == globalindex)
                {
                    this.contextCollection.GlobalTreeConnectTable.RemoveAt(i);
                    return;
                }
            }
        }


        /// <summary>
        /// get all TreeConnects in the globalTreeConnectTable. 
        /// </summary>
        /// <returns>A snapshot of the globalTreeConnectTable will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerTreeConnect> GetTreeConnects()
        {
            Collection<CifsClientPerTreeConnect> ret = new Collection<CifsClientPerTreeConnect>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerTreeConnect treeConnect in this.contextCollection.GlobalTreeConnectTable)
                {
                    ret.Add(treeConnect.Clone());
                }
            }
            return new ReadOnlyCollection<CifsClientPerTreeConnect>(ret);
        }


        /// <summary>
        /// get TreeConnects of a connection in the globalTreeConnectTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the treeConnect to get.</param>
        /// <returns>A snapshot of the found treeConnects will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerTreeConnect> GetTreeConnects(int connectionId)
        {
            Collection<CifsClientPerTreeConnect> ret = new Collection<CifsClientPerTreeConnect>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerTreeConnect treeConnect in this.contextCollection.GlobalTreeConnectTable)
                {
                    if (treeConnect.ConnectionId == connectionId)
                    {
                        ret.Add(treeConnect.Clone());
                    }
                }
            }
            return new ReadOnlyCollection<CifsClientPerTreeConnect>(ret);
        }


        /// <summary>
        /// get TreeConnects of a session identified by the connectionId and sessionId in the globalTreeConnectTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the treeConnect to get.</param>
        /// <param name="sessionId">the sessionId of the treeConnect to get.</param>
        /// <returns>A snapshot of the found treeConnects will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerTreeConnect> GetTreeConnects(int connectionId, ulong sessionId)
        {
            Collection<CifsClientPerTreeConnect> ret = new Collection<CifsClientPerTreeConnect>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerTreeConnect treeConnect in this.contextCollection.GlobalTreeConnectTable)
                {
                    if (treeConnect.ConnectionId == connectionId
                        && treeConnect.SessionId == sessionId)
                    {
                        ret.Add(treeConnect.Clone());
                    }
                }
            }
            return new ReadOnlyCollection<CifsClientPerTreeConnect>(ret);
        }


        /// <summary>
        /// get TreeConnect identified by the connectionId, sessionId and treeId in the globalTreeConnectTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the treeConnect to get.</param>
        /// <param name="sessionId">the sessionId of the treeConnect to get.</param>
        /// <param name="treeId">the treeId of the treeConnect to get.</param>
        /// <returns>A snapshot of the found treeConnect will be returned.</returns>
        public CifsClientPerTreeConnect GetTreeConnect(int connectionId, ulong sessionId, ulong treeId)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerTreeConnect treeConnect in this.contextCollection.GlobalTreeConnectTable)
                {
                    if (treeConnect.ConnectionId == connectionId
                        && treeConnect.SessionId == sessionId
                        && treeConnect.TreeId == treeId)
                    {
                        return treeConnect.Clone();
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// get TreeConnect identified by the globalIndex in the globalTreeConnectTable. 
        /// </summary>
        /// <param name="globalIndex">the globalIndex of the TreeConnect to get.</param>
        /// <returns>A snapshot of the found TreeConnect will be returned.</returns>
        public CifsClientPerTreeConnect GetTreeConnect(int globalIndex)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerTreeConnect treeConnect in this.contextCollection.GlobalTreeConnectTable)
                {
                    if (treeConnect.GlobalIndex == globalIndex)
                    {
                        return treeConnect.Clone();
                    }
                }
            }
            return null;
        }

        #endregion


        #region Access globalOpenFileTable


        /// <summary>
        /// if the open identified by the connectionId, sessionId, treeId, fileHandle has 
        /// been existed in globalOpenTable, this open will be updated into the globalOpenTable.
        /// otherwise, this open will be added into the globalOpenTable.
        /// </summary>
        /// <param name="open">the open to be added or updated.</param>
        /// <exception cref="System.InvalidOperationException">Failed in AddOrUpdateOpen because
        /// the Session or Connection of the Open does not found.</exception>
        public void AddOrUpdateOpenFile(CifsClientPerOpenFile open)
        {
            if (open == null)
            {
                return;
            }

            lock (this.contextCollection)
            {
                if (AddOrUpdateOpenFileInSession(open))
                {
                    AddOrUpdateOpenFileInGlobal(open);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Failed in AddOrUpdateOpen because the Session or Connection of the Open does not found.");
                }
            }
        }


        /// <summary>
        /// Add Or Update an Open in the OpenFileTable of the session.
        /// </summary>
        /// <param name="open">the open to be added or updated.</param>
        /// <returns>if the connection exists, add or update the session and return true. otherwise return
        /// false.</returns>
        private bool AddOrUpdateOpenFileInSession(CifsClientPerOpenFile open)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                // if the connection exists, add or update the open:
                if (connection.ConnectionId == open.ConnectionId)
                {
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                        // if the session exists, add or update the open:
                        if (session.SessionId == open.SessionId)
                        {
                            // if the open exists, update it:
                            for (int k = session.OpenFileTable.Count - 1; k >= 0; k--)
                            {
                                if (session.OpenFileTable[k].Volatile == open.Volatile
                                    && session.OpenFileTable[k].Volatile == open.Volatile)
                                {
                                    // update the open:
                                    session.OpenFileTable[k] = open;
                                    return true;
                                }
                            }
                            // add the open:
                            open.GlobalIndex = this.contextCollection.NextOpenGlobalIndex;
                            open.Volatile = (ulong)open.GlobalIndex;
                            session.OpenFileTable.Add(open);
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// Add Or Update an Open in the GlobalOpenFileTable.
        /// </summary>
        /// <param name="open">the open to be added or updated.</param>
        private void AddOrUpdateOpenFileInGlobal(CifsClientPerOpenFile open)
        {
            for (int i = this.contextCollection.GlobalOpenFileTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenFileTable[i].ConnectionId == open.ConnectionId
                    && this.contextCollection.GlobalOpenFileTable[i].SessionId == open.SessionId
                    && this.contextCollection.GlobalOpenFileTable[i].TreeConnectId == open.TreeConnectId
                    && this.contextCollection.GlobalOpenFileTable[i].Persistent == open.Persistent
                    && this.contextCollection.GlobalOpenFileTable[i].Volatile == open.Volatile)
                {
                    // update the open:
                    this.contextCollection.GlobalOpenFileTable[i] = open;
                    return;
                }
            }
            // add the open:
            open.GlobalIndex = this.contextCollection.NextOpenGlobalIndex;
            open.Volatile = (ulong)open.GlobalIndex;
            this.contextCollection.GlobalOpenFileTable.Add(open);
            this.contextCollection.NextOpenGlobalIndex += 1;
        }


        /// <summary>
        /// remove all opens from the globalOpenTable.
        /// </summary>
        public void RemoveOpenFiles()
        {
            lock (this.contextCollection)
            {
                RemoveOpenFilesFromAllConnection();
                RemoveOpenFilesFromGlobal();
            }
        }


        /// <summary>
        /// to remove all open files from connection.
        /// </summary>
        private void RemoveOpenFilesFromAllConnection()
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection =
                this.contextCollection.ConnectionList[i] as CifsClientPerConnection;

                for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                {
                    CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                    session.OpenFileTable.Clear();
                }
            }
        }


        /// <summary>
        /// to remove all open files from global.
        /// </summary>
        private void RemoveOpenFilesFromGlobal()
        {
            this.contextCollection.GlobalOpenFileTable.Clear();
        }


        /// <summary>
        /// remove opens in a connection identified by the connectionId from the globalOpenTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the open to be removed.</param>
        public void RemoveOpenFiles(int connectionId)
        {
            lock (this.contextCollection)
            {
                RemoveOpenFilesFromConnection(connectionId);
                RemoveOpenFilesFromGlobal(connectionId);
            }
        }


        /// <summary>
        /// to remove open file from connection.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        private void RemoveOpenFilesFromConnection(int connectionId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                        session.OpenFileTable.Clear();
                    }
                    return;
                }
            }
        }


        /// <summary>
        /// to remove open file from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        private void RemoveOpenFilesFromGlobal(int connectionId)
        {
            for (int i = this.contextCollection.GlobalOpenFileTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenFileTable[i].ConnectionId == connectionId)
                {
                    this.contextCollection.GlobalOpenFileTable.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// remove opens in a session identified by the connectionId, sessionId from the globalOpenTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the open to be removed.</param>
        /// <param name="sessionId">the sessionId of the open to be removed.</param>
        public void RemoveOpenFiles(int connectionId, ulong sessionId)
        {
            lock (this.contextCollection)
            {
                RemoveOpenFilesFromSession(connectionId, sessionId);
                RemoveOpenFilesFromGlobal(connectionId, sessionId);
            }
        }


        /// <summary>
        /// to remove open file from session.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        private void RemoveOpenFilesFromSession(int connectionId, ulong sessionId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == sessionId)
                        {
                            CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                            session.OpenFileTable.Clear();
                        }
                    }
                    return;
                }
            }
        }


        /// <summary>
        /// to remove open file from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        private void RemoveOpenFilesFromGlobal(int connectionId, ulong sessionId)
        {
            for (int i = this.contextCollection.GlobalOpenFileTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenFileTable[i].ConnectionId == connectionId
                    && this.contextCollection.GlobalOpenFileTable[i].SessionId == sessionId)
                {
                    this.contextCollection.GlobalOpenFileTable.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// remove opens in a treeConnect identified by the connectionId, sessionId, treeId from the 
        /// globalOpenTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the open to be removed.</param>
        /// <param name="sessionId">the sessionId of the open to be removed.</param>
        /// <param name="treeId">the treeId of the open to be removed.</param>
        public void RemoveOpenFiles(int connectionId, ulong sessionId, ulong treeId)
        {
            lock (this.contextCollection)
            {
                RemoveOpenFilesFromTree(connectionId, sessionId, treeId);
                RemoveOpenFilesFromGlobal(connectionId, sessionId, treeId);
            }
        }


        /// <summary>
        /// to remove open file from tree.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        /// <param name="treeId">the treeId of the Open to be removed.</param>
        private void RemoveOpenFilesFromTree(int connectionId, ulong sessionId, ulong treeId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == sessionId)
                        {
                            CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                            for (int k = session.OpenFileTable.Count - 1; k >= 0; k--)
                            {
                                if (session.OpenFileTable[k].TreeConnectId == treeId)
                                {
                                    session.OpenFileTable.RemoveAt(k);
                                }
                            }
                            return;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove open file from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        /// <param name="treeId">the treeId of the Open to be removed.</param>
        private void RemoveOpenFilesFromGlobal(int connectionId, ulong sessionId, ulong treeId)
        {
            for (int i = this.contextCollection.GlobalOpenFileTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenFileTable[i].ConnectionId == connectionId
                    && this.contextCollection.GlobalOpenFileTable[i].SessionId == sessionId
                    && this.contextCollection.GlobalOpenFileTable[i].TreeConnectId == treeId)
                {
                    this.contextCollection.GlobalOpenFileTable.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// remove a open identified by the connectionId, sessionId, treeId, fileHandle from the globalOpenTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the open to be removed.</param>
        /// <param name="sessionId">the sessionId of the open to be removed.</param>
        /// <param name="treeId">the treeId of the open to be removed.</param>
        /// <param name="fileHandle">the fileHandle of the open to be removed.</param>
        public void RemoveOpenFile(int connectionId, ulong sessionId, ulong treeId, ushort fileHandle)
        {
            lock (this.contextCollection)
            {
                RemoveOpenFileFromTree(connectionId, sessionId, treeId, fileHandle);
                RemoveOpenFileFromGlobal(connectionId, sessionId, treeId, fileHandle);
            }
        }


        /// <summary>
        /// to remove open file from tree.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        /// <param name="treeId">the treeId of the Open to be removed.</param>
        /// <param name="fileHandle">the fileHandle of the Open to be removed.</param>
        private void RemoveOpenFileFromTree(int connectionId, ulong sessionId, ulong treeId, ushort fileHandle)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == sessionId)
                        {
                            CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                            for (int k = session.OpenFileTable.Count - 1; k >= 0; k--)
                            {
                                if (session.OpenFileTable[k].TreeConnectId == treeId
                                    && (session.OpenFileTable[k] as CifsClientPerOpenFile).FileHandle == fileHandle)
                                {
                                    session.OpenFileTable.RemoveAt(k);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove open file from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        /// <param name="treeId">the treeId of the Open to be removed.</param>
        /// <param name="fileHandle">the fileHandle of the Open to be removed.</param>
        private void RemoveOpenFileFromGlobal(int connectionId, ulong sessionId, ulong treeId, ushort fileHandle)
        {
            for (int i = this.contextCollection.GlobalOpenFileTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenFileTable[i].ConnectionId == connectionId
                    && this.contextCollection.GlobalOpenFileTable[i].SessionId == sessionId
                    && this.contextCollection.GlobalOpenFileTable[i].TreeConnectId == treeId
                    && (this.contextCollection.GlobalOpenFileTable[i] as CifsClientPerOpenFile).FileHandle == fileHandle)
                {
                    this.contextCollection.GlobalOpenFileTable.RemoveAt(i);
                    return;
                }
            }
        }


        /// <summary>
        /// remove a open identified by the globalIndex from the globalOpenTable.
        /// </summary>
        /// <param name="globalindex">the globalIndex of the Open to be removed.</param>
        public void RemoveOpenFile(int globalindex)
        {
            lock (this.contextCollection)
            {
                RemoveOpenFileFromTree(globalindex);
                RemoveOpenFileFromGlobal(globalindex);
            }
        }


        /// <summary>
        /// to remove open file from tree.
        /// </summary>
        /// <param name="globalindex">the globalindex of the Open to be removed.</param>
        private void RemoveOpenFileFromTree(int globalindex)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                {
                    CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                    for (int k = session.OpenFileTable.Count - 1; k >= 0; k--)
                    {
                        if (session.OpenFileTable[k].GlobalIndex == globalindex)
                        {
                            session.OpenFileTable.RemoveAt(k);
                            return;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove open file from global.
        /// </summary>
        /// <param name="globalindex">the globalindex of the Open to be removed.</param>
        private void RemoveOpenFileFromGlobal(int globalindex)
        {
            for (int i = this.contextCollection.GlobalOpenFileTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenFileTable[i].GlobalIndex == globalindex)
                {
                    this.contextCollection.GlobalOpenFileTable.RemoveAt(i);
                    return;
                }
            }
        }


        /// <summary>
        /// get all opens  in the globalTreeConnectTable. 
        /// </summary>
        /// <returns>A snapshot of the globalTreeConnectTable will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerOpenFile> GetOpenFiles()
        {
            Collection<CifsClientPerOpenFile> ret = new Collection<CifsClientPerOpenFile>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenFile open in this.contextCollection.GlobalOpenFileTable)
                {
                    ret.Add(open.Clone());
                }
            }
            return new ReadOnlyCollection<CifsClientPerOpenFile>(ret);
        }


        /// <summary>
        /// get opens in a connection identified by the connectionId in the GlobalOpenFileTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to get.</param>
        /// <returns>A snapshot of the found Opens will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerOpenFile> GetOpenFiles(int connectionId)
        {
            Collection<CifsClientPerOpenFile> ret = new Collection<CifsClientPerOpenFile>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenFile open in this.contextCollection.GlobalOpenFileTable)
                {
                    if (open.ConnectionId == connectionId)
                    {
                        ret.Add(open.Clone());
                    }
                }
            }
            return new ReadOnlyCollection<CifsClientPerOpenFile>(ret);
        }


        /// <summary>
        /// get opens in a session identified by the connectionId, sessionId in the GlobalOpenFileTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to get.</param>
        /// <param name="sessionId">the sessionId of the Open to get.</param>
        /// <returns>A snapshot of the found Opens will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerOpenFile> GetOpenFiles(int connectionId, ulong sessionId)
        {
            Collection<CifsClientPerOpenFile> ret = new Collection<CifsClientPerOpenFile>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenFile open in this.contextCollection.GlobalOpenFileTable)
                {
                    if (open.ConnectionId == connectionId
                        && open.SessionId == sessionId)
                    {
                        ret.Add(open.Clone());
                    }
                }
            }
            return new ReadOnlyCollection<CifsClientPerOpenFile>(ret);
        }


        /// <summary>
        /// get opens in a treeConnect identified by the connectionId, sessionId, treeId in the 
        /// GlobalOpenFileTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to get.</param>
        /// <param name="sessionId">the sessionId of the Open to get.</param>
        /// <param name="treeId">the treeId of the Open to get.</param>
        /// <returns>A snapshot of the found Opens will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerOpenFile> GetOpenFiles(int connectionId, ulong sessionId, ulong treeId)
        {
            Collection<CifsClientPerOpenFile> ret = new Collection<CifsClientPerOpenFile>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenFile open in this.contextCollection.GlobalOpenFileTable)
                {
                    if (open.ConnectionId == connectionId
                        && open.SessionId == sessionId
                        && open.TreeConnectId == treeId)
                    {
                        ret.Add(open.Clone());
                    }
                }
            }
            return new ReadOnlyCollection<CifsClientPerOpenFile>(ret);
        }


        /// <summary>
        /// get Open identified by the connectionId, sessionId, treeId, fileHandle in the GlobalOpenFileTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to get.</param>
        /// <param name="sessionId">the sessionId of the Open to get.</param>
        /// <param name="treeId">the treeId of the Open to get.</param>
        /// <param name="fileHandle">the fileHandle of the Open to get.</param>
        /// <returns>A snapshot of the found Open will be returned.</returns>
        public CifsClientPerOpenFile GetOpenFile(int connectionId, ulong sessionId, ulong treeId, ushort fileHandle)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenFile open in this.contextCollection.GlobalOpenFileTable)
                {
                    if (open.ConnectionId == connectionId
                        && open.SessionId == sessionId
                        && open.TreeConnectId == treeId
                        && open.FileHandle == fileHandle)
                    {
                        return open.Clone();
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// get Open identified by the globalIndex in the GlobalOpenFileTable. 
        /// </summary>
        /// <param name="globalIndex">the globalIndex of the Open to get.</param>
        /// <returns>A snapshot of the found Open will be returned.</returns>
        public CifsClientPerOpenFile GetOpenFile(int globalIndex)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenFile open in this.contextCollection.GlobalOpenFileTable)
                {
                    if (open.GlobalIndex == globalIndex)
                    {
                        return open.Clone();
                    }
                }
            }
            return null;
        }

        #endregion


        #region Access globalOpenSearchTable


        /// <summary>
        /// if the open identified by the connectionId, sessionId, treeId, searchId has 
        /// been existed in globalOpenTable, this open will be updated into the globalOpenTable.
        /// otherwise, this open will be added into the globalOpenTable.
        /// </summary>
        /// <param name="open">the open to be added or updated.</param>
        /// <exception cref="System.InvalidOperationException">Failed in AddOrUpdateOpen because
        /// the Session or Connection of the Open does not found.</exception>
        public void AddOrUpdateOpenSearch(CifsClientPerOpenSearch open)
        {
            if (open == null)
            {
                return;
            }

            lock (this.contextCollection)
            {
                if (AddOrUpdateOpenSearchInSession(open))
                {
                    AddOrUpdateOpenSearchInGlobal(open);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Failed in AddOrUpdateOpen because the Session or Connection of the Open does not found.");
                }
            }
        }


        /// <summary>
        /// Add Or Update an Open in the OpenSearchTable of the session.
        /// </summary>
        /// <param name="open">the open to be added or updated.</param>
        /// <returns>if the connection exists, add or update the session and return true. otherwise return
        /// false.</returns>
        private bool AddOrUpdateOpenSearchInSession(CifsClientPerOpenSearch open)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                // if the connection exists, add or update the open:
                if (connection.ConnectionId == open.ConnectionId)
                {
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                        // if the session exists, add or update the open:
                        if (session.SessionId == open.SessionId)
                        {
                            // if the open exists, update it:
                            for (int k = session.OpenSearchTable.Count - 1; k >= 0; k--)
                            {
                                if (session.OpenSearchTable[k].Volatile == open.Volatile
                                    && session.OpenSearchTable[k].Volatile == open.Volatile)
                                {
                                    // update the open:
                                    session.OpenSearchTable[k] = open;
                                    return true;
                                }
                            }
                            // add the open:
                            open.GlobalIndex = this.contextCollection.NextOpenGlobalIndex;
                            session.OpenSearchTable.Add(open);
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// Add Or Update an Open in the GlobalOpenSearchTable.
        /// </summary>
        /// <param name="open">the open to be added or updated.</param>
        private void AddOrUpdateOpenSearchInGlobal(CifsClientPerOpenSearch open)
        {
            for (int i = this.contextCollection.GlobalOpenSearchTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenSearchTable[i].ConnectionId == open.ConnectionId
                    && this.contextCollection.GlobalOpenSearchTable[i].SessionId == open.SessionId
                    && this.contextCollection.GlobalOpenSearchTable[i].TreeConnectId == open.TreeConnectId
                    && this.contextCollection.GlobalOpenSearchTable[i].Persistent == open.Persistent
                    && this.contextCollection.GlobalOpenSearchTable[i].Volatile == open.Volatile)
                {
                    // update the open:
                    this.contextCollection.GlobalOpenSearchTable[i] = open;
                    return;
                }
            }
            // add the open:
            open.GlobalIndex = this.contextCollection.NextOpenGlobalIndex;
            this.contextCollection.GlobalOpenSearchTable.Add(open);
            this.contextCollection.NextOpenGlobalIndex += 1;
        }


        /// <summary>
        /// remove all opens from the globalOpenTable.
        /// </summary>
        public void RemoveOpenSearchs()
        {
            lock (this.contextCollection)
            {
                RemoveOpenSearchsFromAllConnection();
                RemoveOpenSearchsFromGlobal();
            }
        }


        /// <summary>
        /// to remove all open searches from connection.
        /// </summary>
        private void RemoveOpenSearchsFromAllConnection()
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection =
                this.contextCollection.ConnectionList[i] as CifsClientPerConnection;

                for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                {
                    CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                    session.OpenSearchTable.Clear();
                }
            }
        }


        /// <summary>
        /// to remove all open searches from global.
        /// </summary>
        private void RemoveOpenSearchsFromGlobal()
        {
            this.contextCollection.GlobalOpenSearchTable.Clear();
        }


        /// <summary>
        /// remove opens in a connection identified by the connectionId from the globalOpenTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the open to be removed.</param>
        public void RemoveOpenSearchs(int connectionId)
        {
            lock (this.contextCollection)
            {
                RemoveOpenSearchsFromConnection(connectionId);
                RemoveOpenSearchsFromGlobal(connectionId);
            }
        }


        /// <summary>
        /// to remove open search from connection.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        private void RemoveOpenSearchsFromConnection(int connectionId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                        session.OpenSearchTable.Clear();
                    }
                    return;
                }
            }
        }


        /// <summary>
        /// to remove open search from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        private void RemoveOpenSearchsFromGlobal(int connectionId)
        {
            for (int i = this.contextCollection.GlobalOpenSearchTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenSearchTable[i].ConnectionId == connectionId)
                {
                    this.contextCollection.GlobalOpenSearchTable.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// remove opens in a session identified by the connectionId, sessionId from the globalOpenTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the open to be removed.</param>
        /// <param name="sessionId">the sessionId of the open to be removed.</param>
        public void RemoveOpenSearchs(int connectionId, ulong sessionId)
        {
            lock (this.contextCollection)
            {
                RemoveOpenSearchsFromSession(connectionId, sessionId);
                RemoveOpenSearchsFromGlobal(connectionId, sessionId);
            }
        }


        /// <summary>
        /// to remove open search from session.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        private void RemoveOpenSearchsFromSession(int connectionId, ulong sessionId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == sessionId)
                        {
                            CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                            session.OpenSearchTable.Clear();
                        }
                    }
                    return;
                }
            }
        }


        /// <summary>
        /// to remove open search from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        private void RemoveOpenSearchsFromGlobal(int connectionId, ulong sessionId)
        {
            for (int i = this.contextCollection.GlobalOpenSearchTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenSearchTable[i].ConnectionId == connectionId
                    && this.contextCollection.GlobalOpenSearchTable[i].SessionId == sessionId)
                {
                    this.contextCollection.GlobalOpenSearchTable.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// remove opens in a treeConnect identified by the connectionId, sessionId, treeId from the 
        /// globalOpenTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the open to be removed.</param>
        /// <param name="sessionId">the sessionId of the open to be removed.</param>
        /// <param name="treeId">the treeId of the open to be removed.</param>
        public void RemoveOpenSearchs(int connectionId, ulong sessionId, ulong treeId)
        {
            lock (this.contextCollection)
            {
                RemoveOpenSearchsFromTree(connectionId, sessionId, treeId);
                RemoveOpenSearchsFromGlobal(connectionId, sessionId, treeId);
            }
        }


        /// <summary>
        /// to remove open search from tree.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        /// <param name="treeId">the treeId of the Open to be removed.</param>
        private void RemoveOpenSearchsFromTree(int connectionId, ulong sessionId, ulong treeId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == sessionId)
                        {
                            CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                            for (int k = session.OpenSearchTable.Count - 1; k >= 0; k--)
                            {
                                if (session.OpenSearchTable[k].TreeConnectId == treeId)
                                {
                                    session.OpenSearchTable.RemoveAt(k);
                                }
                            }
                            return;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove open search from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        /// <param name="treeId">the treeId of the Open to be removed.</param>
        private void RemoveOpenSearchsFromGlobal(int connectionId, ulong sessionId, ulong treeId)
        {
            for (int i = this.contextCollection.GlobalOpenSearchTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenSearchTable[i].ConnectionId == connectionId
                    && this.contextCollection.GlobalOpenSearchTable[i].SessionId == sessionId
                    && this.contextCollection.GlobalOpenSearchTable[i].TreeConnectId == treeId)
                {
                    this.contextCollection.GlobalOpenSearchTable.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// remove a open identified by the connectionId, sessionId, treeId, searchId from the globalOpenTable.
        /// </summary>
        /// <param name="connectionId">the connectionId of the open to be removed.</param>
        /// <param name="sessionId">the sessionId of the open to be removed.</param>
        /// <param name="treeId">the treeId of the open to be removed.</param>
        /// <param name="searchId">the searchId of the open to be removed.</param>
        public void RemoveOpenSearch(int connectionId, ulong sessionId, ulong treeId, ushort searchId)
        {
            lock (this.contextCollection)
            {
                RemoveOpenSearchFromTree(connectionId, sessionId, treeId, searchId);
                RemoveOpenSearchFromGlobal(connectionId, sessionId, treeId, searchId);
            }
        }


        /// <summary>
        /// to remove open search from tree.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        /// <param name="treeId">the treeId of the Open to be removed.</param>
        /// <param name="searchId">the searchId of the Open to be removed.</param>
        private void RemoveOpenSearchFromTree(int connectionId, ulong sessionId, ulong treeId, ushort searchId)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.ConnectionList[i].ConnectionId == connectionId)
                {
                    CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                    for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                    {
                        if (connection.SessionTable[j].SessionId == sessionId)
                        {
                            CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                            for (int k = session.OpenSearchTable.Count - 1; k >= 0; k--)
                            {
                                if (session.OpenSearchTable[k].TreeConnectId == treeId
                                    && (session.OpenSearchTable[k] as CifsClientPerOpenSearch).SearchID == searchId)
                                {
                                    session.OpenSearchTable.RemoveAt(k);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove open search from global.
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to be removed.</param>
        /// <param name="sessionId">the sessionId of the Open to be removed.</param>
        /// <param name="treeId">the treeId of the Open to be removed.</param>
        /// <param name="searchId">the searchId of the Open to be removed.</param>
        private void RemoveOpenSearchFromGlobal(int connectionId, ulong sessionId, ulong treeId, ushort searchId)
        {
            for (int i = this.contextCollection.GlobalOpenSearchTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenSearchTable[i].ConnectionId == connectionId
                    && this.contextCollection.GlobalOpenSearchTable[i].SessionId == sessionId
                    && this.contextCollection.GlobalOpenSearchTable[i].TreeConnectId == treeId
                    && (this.contextCollection.GlobalOpenSearchTable[i] as CifsClientPerOpenSearch).SearchID == searchId)
                {
                    this.contextCollection.GlobalOpenSearchTable.RemoveAt(i);
                    return;
                }
            }
        }


        /// <summary>
        /// remove a open identified by the globalIndex from the globalOpenTable.
        /// </summary>
        /// <param name="globalindex">the globalIndex of the Open to be removed.</param>
        public void RemoveOpenSearch(int globalindex)
        {
            lock (this.contextCollection)
            {
                RemoveOpenSearchFromTree(globalindex);
                RemoveOpenSearchFromGlobal(globalindex);
            }
        }


        /// <summary>
        /// to remove open search from tree.
        /// </summary>
        /// <param name="globalindex">the globalindex of the Open to be removed.</param>
        private void RemoveOpenSearchFromTree(int globalindex)
        {
            for (int i = this.contextCollection.ConnectionList.Count - 1; i >= 0; i--)
            {
                CifsClientPerConnection connection = this.contextCollection.ConnectionList[i] as
                    CifsClientPerConnection;
                for (int j = connection.SessionTable.Count - 1; j >= 0; j--)
                {
                    CifsClientPerSession session = connection.SessionTable[j] as CifsClientPerSession;
                    for (int k = session.OpenSearchTable.Count - 1; k >= 0; k--)
                    {
                        if (session.OpenSearchTable[k].GlobalIndex == globalindex)
                        {
                            session.OpenSearchTable.RemoveAt(k);
                            return;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// to remove open search from global.
        /// </summary>
        /// <param name="globalindex">the globalindex of the Open to be removed.</param>
        private void RemoveOpenSearchFromGlobal(int globalindex)
        {
            for (int i = this.contextCollection.GlobalOpenSearchTable.Count - 1; i >= 0; i--)
            {
                if (this.contextCollection.GlobalOpenSearchTable[i].GlobalIndex == globalindex)
                {
                    this.contextCollection.GlobalOpenSearchTable.RemoveAt(i);
                    return;
                }
            }
        }


        /// <summary>
        /// get all opens  in the globalTreeConnectTable. 
        /// </summary>
        /// <returns>A snapshot of the globalTreeConnectTable will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerOpenSearch> GetOpenSearchs()
        {
            Collection<CifsClientPerOpenSearch> ret = new Collection<CifsClientPerOpenSearch>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenSearch open in this.contextCollection.GlobalOpenSearchTable)
                {
                    ret.Add(new CifsClientPerOpenSearch(open));
                }
            }
            return new ReadOnlyCollection<CifsClientPerOpenSearch>(ret);
        }


        /// <summary>
        /// get opens in a connection identified by the connectionId in the GlobalOpenSearchTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to get.</param>
        /// <returns>A snapshot of the found Opens will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerOpenSearch> GetOpenSearchs(int connectionId)
        {
            Collection<CifsClientPerOpenSearch> ret = new Collection<CifsClientPerOpenSearch>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenSearch open in this.contextCollection.GlobalOpenSearchTable)
                {
                    if (open.ConnectionId == connectionId)
                    {
                        ret.Add(new CifsClientPerOpenSearch(open));
                    }
                }
            }
            return new ReadOnlyCollection<CifsClientPerOpenSearch>(ret);
        }


        /// <summary>
        /// get opens in a session identified by the connectionId, sessionId in the GlobalOpenSearchTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to get.</param>
        /// <param name="sessionId">the sessionId of the Open to get.</param>
        /// <returns>A snapshot of the found Opens will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerOpenSearch> GetOpenSearchs(int connectionId, ulong sessionId)
        {
            Collection<CifsClientPerOpenSearch> ret = new Collection<CifsClientPerOpenSearch>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenSearch open in this.contextCollection.GlobalOpenSearchTable)
                {
                    if (open.ConnectionId == connectionId
                        && open.SessionId == sessionId)
                    {
                        ret.Add(new CifsClientPerOpenSearch(open));
                    }
                }
            }
            return new ReadOnlyCollection<CifsClientPerOpenSearch>(ret);
        }


        /// <summary>
        /// get opens in a treeConnect identified by the connectionId, sessionId, treeId in the 
        /// GlobalOpenSearchTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to get.</param>
        /// <param name="sessionId">the sessionId of the Open to get.</param>
        /// <param name="treeId">the treeId of the Open to get.</param>
        /// <returns>A snapshot of the found Opens will be returned.</returns>
        public ReadOnlyCollection<CifsClientPerOpenSearch> GetOpenSearchs(int connectionId, ulong sessionId, ulong treeId)
        {
            Collection<CifsClientPerOpenSearch> ret = new Collection<CifsClientPerOpenSearch>();
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenSearch open in this.contextCollection.GlobalOpenSearchTable)
                {
                    if (open.ConnectionId == connectionId
                        && open.SessionId == sessionId
                        && open.TreeConnectId == treeId)
                    {
                        ret.Add(new CifsClientPerOpenSearch(open));
                    }
                }
            }
            return new ReadOnlyCollection<CifsClientPerOpenSearch>(ret);
        }


        /// <summary>
        /// get Open identified by the connectionId, sessionId, treeId, searchId in the GlobalOpenSearchTable. 
        /// </summary>
        /// <param name="connectionId">the connectionId of the Open to get.</param>
        /// <param name="sessionId">the sessionId of the Open to get.</param>
        /// <param name="treeId">the treeId of the Open to get.</param>
        /// <param name="searchId">the searchId of the Open to get.</param>
        /// <returns>A snapshot of the found Open will be returned.</returns>
        public CifsClientPerOpenSearch GetOpenSearch(int connectionId, ulong sessionId, ulong treeId, ushort searchId)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenSearch open in this.contextCollection.GlobalOpenSearchTable)
                {
                    if (open.ConnectionId == connectionId
                        && open.SessionId == sessionId
                        && open.TreeConnectId == treeId
                        && open.SearchID == searchId)
                    {
                        return new CifsClientPerOpenSearch(open);
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// get Open identified by the globalIndex in the GlobalOpenSearchTable. 
        /// </summary>
        /// <param name="globalIndex">the globalIndex of the Open to get.</param>
        /// <returns>A snapshot of the found Open will be returned.</returns>
        public CifsClientPerOpenSearch GetOpenSearch(int globalIndex)
        {
            lock (this.contextCollection)
            {
                foreach (CifsClientPerOpenSearch open in this.contextCollection.GlobalOpenSearchTable)
                {
                    if (open.GlobalIndex == globalIndex)
                    {
                        return new CifsClientPerOpenSearch(open);
                    }
                }
            }
            return null;
        }

        #endregion
    }
}