// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// CifsServerContext provides a collection for all CIFS client context and 
    /// its access. all operations to access the client context provided in 
    /// CifsServerContext are thread safe. all operations to get some data from
    /// the context return a snapshot of the current status.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class CifsServerContext : FileServiceServerContext
    {
        #region Fields
        private string serverName;
        private string domainName;
        private volatile Capabilities capabilities;
        private bool opLockSupport;
        private bool guestOkay;
        private int maxBufferSize;
        private int maxRawSize;
        private int maxMpxCount;
        private int maxNumberVcs;
        private bool shareLevelAuthentication;

        private CifsServerGlobalTables globalTables;

        private MessageSigningPolicyValues messageSigningPolicy;
        private PlaintextAuthenticationPolicyValues plaintextAuthenticationPolicy;
        private LMAuthenticationPolicyValues lmAuthenticationPolicy;
        private NTLMAuthenticationPolicyValues ntlmAuthenticationPolicy;

        private bool isContextUpdateEnabled;
        private Collection<AccountCredential> accountCredentials;
        private Collection<NlmpServerSecurityContext> nlmpServerSecurityContexts;
        private string activeAccount;

        #endregion

        #region Properties


        /// <summary>
        /// The domain name which the server belongs to
        /// </summary>
        public string DomainName
        {
            get
            {
                return this.domainName;
            }
            set
            {
                this.domainName = value;
            }
        }


        /// <summary>
        /// The server name.
        /// </summary>
        public string ServerName
        {
            get
            {
                return this.serverName;
            }
            set
            {
                this.serverName = value;
            }
        }


        /// <summary>
        /// to update context by stack automatically or not.
        /// </summary>
        public bool IsContextUpdateEnabled
        {
            get
            {
                return this.isContextUpdateEnabled;
            }
            set
            {
                this.isContextUpdateEnabled = value;
            }
        }


        /// <summary>
        /// The set of Capabilities (as described in section 1.7 and defined in section 2.2.4.52.2) supported by the
        /// server.
        /// </summary>
        public Capabilities Capabilities
        {
            get
            {
                return this.capabilities;
            }
            set
            {
                this.capabilities = value;
            }
        }


        /// <summary>
        /// A Boolean value that indicates whether or not the server supports granting OpLocks on this connection.
        /// </summary>
        public bool OpLockSupport
        {
            get
            {
                return this.opLockSupport;
            }
            set
            {
                this.opLockSupport = value;
            }
        }


        /// <summary>
        /// A Boolean value that indicates whether or not a guest authentication is allowed if user-level authentication
        /// fails. If Server.ShareLevelAuthentication is TRUE, Server.GuestOkay MUST be FALSE.
        /// </summary>
        public bool GuestOkay
        {
            get
            {
                return this.guestOkay;
            }
            set
            {
                this.guestOkay = value;
            }
        }


        /// <summary>
        /// The size, in bytes, of the largest SMB message that the server can receive.
        /// </summary>
        public int MaxBufferSize
        {
            get
            {
                return this.maxBufferSize;
            }
            set
            {
                this.maxBufferSize = value;
            }
        }


        /// <summary>
        /// The maximum raw buffer size, in bytes, available on the server. This value specifies the maximum message 
        /// size that the client MUST NOT exceed when sending an SMB_COM_WRITE_RAW client request, and the maximum
        /// message size that the server MUST NOT exceed when sending an SMB_COM_READ_RAW response. This value is
        /// significant only if CAP_RAW_MODE is negotiated.
        /// </summary>
        public int MaxRawSize
        {
            get
            {
                return this.maxRawSize;
            }
            set
            {
                this.maxRawSize= value;
            }
        }


        /// <summary>
        /// The maximum number of virtual circuits that can be established between the client and the server as part of
        /// the same SMB session
        /// </summary>
        public int MaxNumberVcs
        {
            get
            {
                return this.maxNumberVcs;
            }
            set
            {
                this.maxNumberVcs = value;
            }
        }


        /// <summary>
        /// The maximum number of outstanding commands that each client is allowed to have at any given time.
        /// </summary>
        public int MaxMpxCount
        {
            get
            {
                return this.maxMpxCount;
            }
            set
            {
                this.maxMpxCount = value;
            }
        }


        /// <summary>
        /// A Boolean that indicates whether Share-level or User-level authentication is supported. If this is TRUE,
        /// Share-level authentication MUST be used
        /// </summary>
        public bool ShareLevelAuthentication
        {
            get
            {
                return this.shareLevelAuthentication;
            }
            set
            {
                this.shareLevelAuthentication = value;
            }
        }


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
        /// A state that determines whether plain text authentication is permitted.
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


        /// <summary>
        /// GlobalSessionTable indexed by fileGlobalId.
        /// </summary>
        public ReadOnlyDictionary<int, CifsServerPerOpenFile> GlobalOpenTable
        {
            get
            {
                lock (this.globalTables)
                {
                    return new ReadOnlyDictionary<int, CifsServerPerOpenFile>(this.globalTables.GlobalOpenTable);
                }
            }
        }


        /// <summary>
        /// GlobalOpenSearchTable indexed by searchGlobalId.
        /// </summary>
        public ReadOnlyDictionary<int, CifsServerPerOpenSearch> GlobalOpenSearchTable
        {
            get
            {
                lock (this.globalTables)
                {
                    return new ReadOnlyDictionary<int, CifsServerPerOpenSearch>(
                        this.globalTables.GlobalOpenSearchTable);
                }
            }
        }


        /// <summary>
        /// GlobalSessionTable indexed by treeGlobalId.
        /// </summary>
        public ReadOnlyDictionary<int, CifsServerPerTreeConnect> GlobalTreeConnectTable
        {
            get
            {
                lock (this.globalTables)
                {
                    return new ReadOnlyDictionary<int, CifsServerPerTreeConnect>(this.globalTables.GlobalTreeConnectTable);
                }
            }
        }


        /// <summary>
        /// GlobalSessionTable indexed by sessionGlobalId.
        /// </summary>
        public ReadOnlyDictionary<int, CifsServerPerSession> GlobalSessionTable
        {
            get
            {
                lock (this.globalTables)
                {
                    return new ReadOnlyDictionary<int, CifsServerPerSession>(this.globalTables.GlobalSessionTable);
                }
            }
        }


        /// <summary>
        /// A list of all open connections on the server, indexed by the identify.
        /// </summary>
        public ReadOnlyDictionary<object, CifsServerPerConnection> ConnectionTable
        {
            get
            {
                lock (this.globalTables)
                {
                    return new ReadOnlyDictionary<object, CifsServerPerConnection>(this.globalTables.ConnectionTable);
                }
            }
        }


        /// <summary>
        /// AccountCredentials to accept the token
        /// </summary>
        public Collection<AccountCredential> AccountCredentials
        {
            get
            {
                return this.accountCredentials;
            }
            set
            {
                this.accountCredentials = value;
            }
        }


        /// <summary>
        /// Nlmp Server Security Contexts
        /// </summary>
        public Collection<NlmpServerSecurityContext> NlmpServerSecurityContexts
        {
            get
            {
                return this.nlmpServerSecurityContexts;
            }
            set
            {
                this.nlmpServerSecurityContexts = value;
            }
        }


        /// <summary>
        /// the active account
        /// </summary>
        public string ActiveAccount
        {
            get
            {
                return this.activeAccount;
            }
            set
            {
                this.activeAccount = value;
            }
        }

        #endregion

        #region Generate global Id

        /// <summary>
        /// Generate a new unique file global id.
        /// </summary>
        public int GenerateFileGlobalId()
        {
            lock (this.globalTables)
            {
                return this.globalTables.NextOpenGlobalIndex++;
            }
        }



        /// <summary>
        /// Generate a new unique search global id.
        /// </summary>
        public int GenerateSearchGlobalId()
        {
            lock (this.globalTables)
            {
                return this.globalTables.NextOpenSearchGlobalIndex++;
            }
        }


        /// <summary>
        /// Generate a new unique session global id.
        /// </summary>
        public int GenerateSessionGlobalId()
        {
            lock (this.globalTables)
            {
                return this.globalTables.NextSessionGlobalIndex++;
            }
        }


        /// <summary>
        /// Generate a new unique tree global id.
        /// </summary>
        public int GenerateTreeGlobalId()
        {
            lock (this.globalTables)
            {
                return this.globalTables.NextTreeConnectGlobalIndex++;
            }
        }

        #endregion

        #region Constructor & Dispose

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsServerContext()
        {
            this.domainName = Environment.UserDomainName;
            this.serverName = Environment.MachineName;
            this.capabilities = Capabilities.NONE;
            this.globalTables = new CifsServerGlobalTables();
            this.messageSigningPolicy = MessageSigningPolicyValues.MessageSigningDisabled;
            this.plaintextAuthenticationPolicy = PlaintextAuthenticationPolicyValues.Disabled;
            this.lmAuthenticationPolicy = LMAuthenticationPolicyValues.Disabled;
            this.ntlmAuthenticationPolicy = 
                NTLMAuthenticationPolicyValues.NtlmEnabled | NTLMAuthenticationPolicyValues.NtlmV2Enabled;
            this.isContextUpdateEnabled = true;
            this.accountCredentials = new Collection<AccountCredential>();
            this.nlmpServerSecurityContexts = new Collection<NlmpServerSecurityContext>();
            //The following default values are based on windows implementation.
            this.maxNumberVcs = 1;
            this.maxBufferSize = 16644;
            this.maxRawSize = 65536;
            this.maxMpxCount = 50;
        }

        #endregion

        #region Update Context

        /// <summary>
        /// Update the context of cifs server
        /// </summary>
        /// <param name="connection">The connection between client and server.</param>
        /// <param name="packet">The sent or received packet in stack transport.</param>
        public virtual void UpdateRoleContext(CifsServerPerConnection connection, SmbPacket packet)
        {
            if (connection == null || packet == null)
            {
                return;
            }

            if (packet is SmbReadRawResponsePacket)
            {
                if (connection.PendingRequestTable.Count != 1)
                {
                    return;
                }
                SmbPacket request = connection.PendingRequestTable[0] as SmbPacket;
                this.UpdateResponseRoleContext(connection, request, packet);
                connection.RemovePendingRequest(request.SmbHeader.Mid);
                connection.RemoveSequenceNumber(packet);
                return;
            }
            else
            {
                switch(packet.PacketType)
                {
                    case SmbPacketType.BatchedRequest:
                    case SmbPacketType.SingleRequest:
                        if (connection.IsSigningActive)
                        {
                            packet.IsSignatureCorrect = CifsMessageUtils.VerifySignature(packet,
                                connection.SigningSessionKey, connection.ServerNextReceiveSequenceNumber,
                                connection.SigningChallengeResponse);
                        }
                        this.UpdateRequestRoleConext(connection, packet);
                        break;
                    case SmbPacketType.BatchedResponse:
                    case SmbPacketType.SingleResponse:
                        SmbPacket request = connection.GetPendingRequest(packet.SmbHeader.Mid);
                        this.UpdateResponseRoleContext(connection, request, packet);
                        this.RemoveRequestAndSequence(connection, packet);
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// remove request and sequence if the response is the final response.
        /// </summary>
        /// <param name="connection">the connection on which to remove the request and sequence</param>
        /// <param name="response">the response for which to remove the request and sequence</param>
        protected virtual void RemoveRequestAndSequence(CifsServerPerConnection connection, SmbPacket response)
        {
            if (response is SmbWriteRawInterimResponsePacket
                || response is SmbNtTransactInterimResponsePacket
                || response is SmbTransactionInterimResponsePacket
                || response is SmbTransaction2InterimResponsePacket)
            {
                return;
            }

            connection.RemovePendingRequest(response.SmbHeader.Mid);
            connection.RemoveSequenceNumber(response);
        }


        /// <summary>
        /// Update the context when received request.
        /// </summary>
        /// <param name="connection">The connection between client and server.</param>
        /// <param name="requestPacket">The received packet in stack transport.</param>
        protected virtual void UpdateRequestRoleConext(
            CifsServerPerConnection connection,
            SmbPacket requestPacket)
        {
            if (requestPacket == null)
            {
                return;
            }

            bool isAndxPacket = requestPacket.SmbHeader.Protocol == CifsMessageUtils.SMB_PROTOCOL_ANDXPACKET;

            if (!isAndxPacket)
            {
                connection.MultiplexId = requestPacket.SmbHeader.Mid;
            }

            /*If the UID is valid, the server MUST enumerate all connections in the Server.ConnectionTable and MUST
             *look up Session in the Server.Connection.SessionTable where UID is equal to Server.Session.UID.
             *If a session is found, Server.Session.IdleTime MUST be set to the current time. If no session is found,
             *no action regarding idle time is taken.*/
            if (requestPacket.SmbHeader.Uid != 0)
            {
                foreach (CifsServerPerSession session in connection.SessionTable)
                {
                    if (session.SessionId == requestPacket.SmbHeader.Uid)
                    {
                        session.IdleTime = DateTime.Now;
                        break;
                    }
                }
            }

            switch (requestPacket.SmbHeader.Command)
            {
                case SmbCommand.SMB_COM_SESSION_SETUP_ANDX:
                    {
                        SmbSessionSetupAndxRequestPacket request = requestPacket as SmbSessionSetupAndxRequestPacket;
                        connection.SessionSetupReceived = true;
                        connection.ClientCapabilites = request.SmbParameters.Capabilities;
                        connection.ClientMaxBufferSize = request.SmbParameters.MaxBufferSize;
                        connection.ClientMaxMpxCount = request.SmbParameters.MaxMpxCount;
                        connection.IsSigningActive = false;
                        connection.NativeLanMan = CifsMessageUtils.ToSmbString(request.SmbData.NativeLanMan, 0, false);
                        connection.NativeOS = CifsMessageUtils.ToSmbString(request.SmbData.NativeOS, 0, false);
                    }
                    break;
                default:
                    break;
            }

            if (!isAndxPacket
                && requestPacket.SmbHeader.Command != SmbCommand.SMB_COM_TRANSACTION_SECONDARY
                && requestPacket.SmbHeader.Command != SmbCommand.SMB_COM_TRANSACTION2_SECONDARY
                && requestPacket.SmbHeader.Command != SmbCommand.SMB_COM_NT_TRANSACT_SECONDARY
                && requestPacket.SmbHeader.Command != SmbCommand.SMB_COM_NT_CANCEL)
            {
                connection.AddPendingRequest(requestPacket);
            }

            if (!isAndxPacket)
            {
                connection.UpdateSequenceNumber(requestPacket);
            }

            SmbBatchedRequestPacket batchedRequest = requestPacket as SmbBatchedRequestPacket;
            if (batchedRequest != null)
            {
                SmbPacket andxPacket = batchedRequest.AndxPacket;
                this.UpdateRequestRoleConext(connection, andxPacket);
            }
        }


        /// <summary>
        /// Update the context when sending a successful response.
        /// </summary>
        /// <param name="connection">The connection between client and server.</param>
        /// <param name="requestPacket">The request packet associated with this packet.</param>
        /// <param name="responsePacket">The response packet.</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        protected virtual void UpdateResponseRoleContext(
            CifsServerPerConnection connection,
            SmbPacket requestPacket,
            SmbPacket responsePacket)
        {
            if (requestPacket == null || responsePacket == null || responsePacket.SmbHeader.Status != 0)
            {
                return;
            }

            SmbHeader smbHeader = responsePacket.SmbHeader;

            switch (responsePacket.SmbHeader.Command)
            {
                #region Negotiate
                case SmbCommand.SMB_COM_NEGOTIATE:
                    {
                        SmbNegotiateRequestPacket request = requestPacket as SmbNegotiateRequestPacket;
                        SmbNegotiateResponsePacket response = responsePacket as SmbNegotiateResponsePacket;
                        if (request != null && response != null && request.SmbData.Dialects != null)
                        {
                            int dialectIndex = (int)(response.SmbParameters.DialectIndex);
                            byte[] dialectBytes = request.SmbData.Dialects;
                            int startIndex = 0;
                            for (int i = 0; i < dialectIndex; i++)
                            {
                                startIndex = Array.IndexOf<byte>(dialectBytes, 0, startIndex,
                                    dialectBytes.Length - startIndex) + 1;
                            }
                            connection.SelectedDialect = CifsMessageUtils.ToSmbString(dialectBytes, startIndex, true);
                            connection.NTLMChallenge = response.SmbData.Challenge;
                            connection.OpLockSupport = this.opLockSupport;
                            connection.NegotiateTime = response.SmbParameters.SystemTime;

                            if (this.ntlmAuthenticationPolicy != NTLMAuthenticationPolicyValues.Disabled)
                            {
                                //Prepare security context for the coming ntlm authentication.
                                foreach (AccountCredential accountCredential in this.accountCredentials)
                                {
                                    NlmpServerSecurityContext serverSecurityContext = new NlmpServerSecurityContext(
                                        NegotiateTypes.NTLM_NEGOTIATE_OEM | NegotiateTypes.NTLMSSP_NEGOTIATE_NTLM,
                                        new NlmpClientCredential(string.Empty, accountCredential.DomainName,
                                            accountCredential.AccountName, accountCredential.Password),
                                        !string.IsNullOrEmpty(this.domainName),
                                        this.domainName,
                                        this.serverName);
                                    serverSecurityContext.UpdateServerChallenge(
                                        BitConverter.ToUInt64(response.SmbData.Challenge, 0));
                                    this.nlmpServerSecurityContexts.Add(serverSecurityContext);
                                }
                            }

                            if ((response.SmbParameters.SecurityMode & SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED)
                                == SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED)
                            {
                                connection.IsSigningActive = true;
                            }
                        }
                    }
                    break;
                #endregion

                #region Session

                case SmbCommand.SMB_COM_SESSION_SETUP_ANDX:
                    #region SMB_COM_SESSION_SETUP_ANDX
                    {
                        SmbSessionSetupAndxRequestPacket request = requestPacket as SmbSessionSetupAndxRequestPacket;
                        SmbSessionSetupAndxResponsePacket response = responsePacket as SmbSessionSetupAndxResponsePacket;

                        if (request != null && response != null)
                        {
                            NlmpServerSecurityContext securityContext = null;
                            if (response.SmbParameters.Action != ActionValues.GuestAccess)
                            {
                                this.ActiveAccount = CifsMessageUtils.PlainTextAuthenticate(request, this.accountCredentials);

                                if (string.IsNullOrEmpty(this.ActiveAccount))
                                {
                                    securityContext = CifsMessageUtils.NTLMAuthenticate(request,
                                        this.nlmpServerSecurityContexts, connection.NegotiateTime.Time);

                                    if (securityContext != null)
                                    {
                                        connection.IsSigningActive = true;
                                        connection.SigningChallengeResponse = request.SmbData.UnicodePassword;
                                        this.ActiveAccount = securityContext.Context.ClientCredential.AccountName;
                                        connection.SigningSessionKey = NlmpUtility.GetResponseKeyNt(
                                            NlmpVersion.v1,
                                            securityContext.Context.ClientCredential.DomainName,
                                            securityContext.Context.ClientCredential.AccountName,
                                            securityContext.Context.ClientCredential.Password);
                                    }
                                }
                            }

                            CifsServerPerSession session = new CifsServerPerSession(
                                connection,
                                smbHeader.Uid,
                                securityContext,
                                DateTime.Now,
                                DateTime.Now,
                                CifsMessageUtils.ToSmbString(request.SmbData.AccountName, 0, false),
                                GenerateSessionGlobalId());
                            this.AddSession(session);
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_LOGOFF_ANDX:
                    #region SMB_COM_LOGOFF_ANDX
                    {
                        SmbLogoffAndxRequestPacket request = requestPacket as SmbLogoffAndxRequestPacket;
                        SmbLogoffAndxResponsePacket response = responsePacket as SmbLogoffAndxResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);
                        
                        if (request != null && response != null && session != null)
                        {
                            this.RemoveSession(session.SessionGlobalId);
                        }
                    }
                    #endregion
                    break;

                #endregion

                #region Tree Connect

                case SmbCommand.SMB_COM_TREE_CONNECT:
                    #region SMB_COM_TREE_CONNECT
                    {
                        SmbTreeConnectRequestPacket request = requestPacket as SmbTreeConnectRequestPacket;
                        SmbTreeConnectResponsePacket response = responsePacket as SmbTreeConnectResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        //If Core Protocol, No sessions setup, make a default session
                        if (session == null
                            && (connection.SelectedDialect == CifsMessageUtils.DIALECT_PCNETWORK_PROGRAM
                                || connection.SelectedDialect == CifsMessageUtils.DIALECT_NTLANMAN))
                        {
                            session = new CifsServerPerSession(
                                    connection,
                                    smbHeader.Uid, //should be unique, windows always be zero
                                    null,
                                    DateTime.Now,
                                    DateTime.Now,
                                    string.Empty,
                                    GenerateSessionGlobalId());
                            this.AddSession(session);
                        }


                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = new CifsServerPerTreeConnect(
                                session,
                                CifsMessageUtils.ToSmbString(request.SmbData.Path, 0, false),
                                response.SmbParameters.TID,
                                this.GenerateTreeGlobalId(),
                                DateTime.Now);
                            this.AddTreeConnect(treeConnect);
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_TREE_CONNECT_ANDX:
                    #region SMB_COM_TREE_CONNECT_ANDX
                    {
                        SmbTreeConnectAndxRequestPacket request = requestPacket as SmbTreeConnectAndxRequestPacket;

                        SmbTreeConnectAndxResponsePacket response = responsePacket as SmbTreeConnectAndxResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = new CifsServerPerTreeConnect(
                                session,
                                CifsMessageUtils.ToString(request.SmbData.Path, request.SmbHeader.Flags2),
                                smbHeader.Tid,
                                this.GenerateTreeGlobalId(),
                                DateTime.Now);
                            this.AddTreeConnect(treeConnect);
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_TREE_DISCONNECT:
                    #region SMB_COM_TREE_DISCONNECT
                    {
                        SmbTreeDisconnectRequestPacket request = requestPacket as SmbTreeDisconnectRequestPacket;
                        SmbTreeDisconnectResponsePacket response = responsePacket as SmbTreeDisconnectResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            //If core protocol, no logoff any more, we have to remove the session if there is 
                            // no treeconnects in it except this.
                            if (session.TreeConnectTable.Count == 1
                                && (connection.SelectedDialect == CifsMessageUtils.DIALECT_PCNETWORK_PROGRAM
                                    || connection.SelectedDialect == CifsMessageUtils.DIALECT_NTLANMAN))
                            {
                                this.RemoveSession(session.SessionGlobalId);
                            }
                            else
                            {
                                CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                                if (treeConnect != null)
                                {
                                    this.RemoveTreeConnect(treeConnect.TreeGlobalId);
                                }
                            }
                        }
                    }
                    #endregion
                    break;

                #endregion

                #region Open File/Search

                case SmbCommand.SMB_COM_OPEN:
                    #region SMB_COM_OPEN
                    {
                        SmbOpenRequestPacket request = requestPacket as SmbOpenRequestPacket;
                        SmbOpenResponsePacket response = responsePacket as SmbOpenResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                OplockLevelValue opLock = OplockLevelValue.None;
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPLOCK) == SmbFlags.SMB_FLAGS_OPLOCK)
                                {
                                    opLock = OplockLevelValue.Exclusive;
                                }
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPBATCH) == SmbFlags.SMB_FLAGS_OPBATCH)
                                {
                                    opLock = OplockLevelValue.Batch;
                                }
                                CifsServerPerOpenFile open = new CifsServerPerOpenFile(
                                    treeConnect,
                                    CifsMessageUtils.ToSmbString(request.SmbData.FileName, 0, false),
                                    response.SmbParameters.FID,
                                    response.SmbParameters.AccessMode,
                                    opLock,
                                    this.GenerateFileGlobalId(),
                                    request.SmbHeader.Pid);
                                this.AddOpenFile(open);
                            }
                        }
                    }
                    #endregion
                    break;

                case SmbCommand.SMB_COM_CREATE:
                    #region SMB_COM_CREATE
                    {
                        SmbCreateRequestPacket request = requestPacket as SmbCreateRequestPacket;
                        SmbCreateResponsePacket response = responsePacket as SmbCreateResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                OplockLevelValue opLock = OplockLevelValue.None;
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPLOCK) == SmbFlags.SMB_FLAGS_OPLOCK)
                                {
                                    opLock = OplockLevelValue.Exclusive;
                                }
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPBATCH) == SmbFlags.SMB_FLAGS_OPBATCH)
                                {
                                    opLock = OplockLevelValue.Batch;
                                }
                                CifsServerPerOpenFile open = new CifsServerPerOpenFile(
                                    treeConnect,
                                    CifsMessageUtils.ToSmbString(request.SmbData.FileName, 0, false),
                                    response.SmbParameters.FID,
                                    (ushort)request.SmbParameters.FileAttributes,
                                    opLock,
                                    this.GenerateFileGlobalId(),
                                    request.SmbHeader.Pid);
                                this.AddOpenFile(open);
                            }
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_CREATE_TEMPORARY:
                    #region SMB_COM_CREATE_TEMPORARY
                    {
                        SmbCreateTemporaryRequestPacket request = requestPacket as SmbCreateTemporaryRequestPacket;
                        SmbCreateTemporaryResponsePacket response = responsePacket as SmbCreateTemporaryResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                OplockLevelValue opLock = OplockLevelValue.None;
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPLOCK) == SmbFlags.SMB_FLAGS_OPLOCK)
                                {
                                    opLock = OplockLevelValue.Exclusive;
                                }
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPBATCH) == SmbFlags.SMB_FLAGS_OPBATCH)
                                {
                                    opLock = OplockLevelValue.Batch;
                                }
                                CifsServerPerOpenFile open = new CifsServerPerOpenFile(
                                    treeConnect,
                                    CifsMessageUtils.ToSmbString(response.SmbData.TemporaryFileName, 0, false),
                                    response.SmbParameters.FID,
                                    (uint)request.SmbParameters.FileAttributes,
                                    opLock,
                                    this.GenerateFileGlobalId(),
                                    request.SmbHeader.Pid);
                                this.AddOpenFile(open);
                            }
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_CREATE_NEW:
                    #region SMB_COM_CREATE_NEW
                    {
                        SmbCreateNewRequestPacket request = requestPacket as SmbCreateNewRequestPacket;
                        SmbCreateNewResponsePacket response = responsePacket as SmbCreateNewResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                OplockLevelValue opLock = OplockLevelValue.None;
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPLOCK) == SmbFlags.SMB_FLAGS_OPLOCK)
                                {
                                    opLock = OplockLevelValue.Exclusive;
                                }
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPBATCH) == SmbFlags.SMB_FLAGS_OPBATCH)
                                {
                                    opLock |= OplockLevelValue.Batch;
                                }
                                CifsServerPerOpenFile open = new CifsServerPerOpenFile(
                                    treeConnect,
                                    CifsMessageUtils.ToSmbString(request.SmbData.FileName, 0, false),
                                    response.SmbParameters.FID,
                                    (uint)request.SmbParameters.FileAttributes,
                                    opLock,
                                    this.GenerateFileGlobalId(),
                                    request.SmbHeader.Pid);
                                this.AddOpenFile(open);
                            }
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_OPEN_ANDX:
                    #region SMB_COM_OPEN_ANDX
                    {
                        SmbOpenAndxRequestPacket request = requestPacket as SmbOpenAndxRequestPacket;
                        SmbOpenAndxResponsePacket response = responsePacket as SmbOpenAndxResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                OplockLevelValue opLock = OplockLevelValue.None;
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPLOCK) == SmbFlags.SMB_FLAGS_OPLOCK)
                                {
                                    opLock = OplockLevelValue.Exclusive;
                                }
                                if ((smbHeader.Flags & SmbFlags.SMB_FLAGS_OPBATCH) == SmbFlags.SMB_FLAGS_OPBATCH)
                                {
                                    opLock |= OplockLevelValue.Batch;
                                }
                                string fileName;
                                if ((request.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                                {
                                    fileName = Encoding.Unicode.GetString(request.SmbData.FileName);
                                }
                                else
                                {
                                    fileName = Encoding.ASCII.GetString(request.SmbData.FileName);
                                }
                                CifsServerPerOpenFile open = new CifsServerPerOpenFile(
                                    treeConnect,
                                    fileName,
                                    response.SmbParameters.FID,
                                    (uint)response.SmbParameters.FileAttrs,
                                    opLock,
                                    this.GenerateFileGlobalId(),
                                    request.SmbHeader.Pid);
                                this.AddOpenFile(open);

                                //save FID for chained response like:
                                //treeConnect->openAndx->readAndx->close
                                //when "close", FID is need to close the open opened in openAndx.
                                if (response.AndxPacket != null)
                                {
                                    //borrow smbHeader.Protocol to save FID for later process.
                                    //smbHeader.Protocol also use a flag to differentiate a single packet from a
                                    //batched andx packet.
                                    //FID is ushort, impossible to impact smbHeader.Protocol's usage as 
                                    //a real packet header 0x424D53FF(0xFF, 'S', 'M', 'B')
                                    SmbHeader andxHeader = response.AndxPacket.SmbHeader;
                                    andxHeader.Protocol = response.SmbParameters.FID;
                                    response.AndxPacket.SmbHeader = andxHeader;
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_NT_CREATE_ANDX:
                    #region SMB_COM_NT_CREATE_ANDX
                    {
                        SmbNtCreateAndxRequestPacket request = requestPacket as SmbNtCreateAndxRequestPacket;
                        SmbNtCreateAndxResponsePacket response = responsePacket as SmbNtCreateAndxResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                string fileName;
                                if ((request.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                                {
                                    fileName = Encoding.Unicode.GetString(request.SmbData.FileName);
                                }
                                else
                                {
                                    fileName = Encoding.ASCII.GetString(request.SmbData.FileName);
                                }
                                CifsServerPerOpenFile open = new CifsServerPerOpenFile(
                                    treeConnect,
                                    fileName,
                                    response.SmbParameters.FID,
                                    (uint)response.SmbParameters.ExtFileAttributes,
                                    response.SmbParameters.OplockLevel,
                                    this.GenerateFileGlobalId(),
                                    request.SmbHeader.Pid);
                                this.AddOpenFile(open);
                            }
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_OPEN_PRINT_FILE:
                    #region SMB_COM_OPEN_PRINT_FILE
                    {
                        SmbOpenPrintFileRequestPacket request = requestPacket as SmbOpenPrintFileRequestPacket;
                        SmbOpenPrintFileResponsePacket response = responsePacket as SmbOpenPrintFileResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                CifsServerPerOpenFile open = new CifsServerPerOpenFile(
                                    treeConnect,
                                    CifsMessageUtils.ToSmbString(request.SmbData.Identifier, 0, false),
                                    response.SmbParameters.FID,
                                    request.SmbParameters.Mode,
                                    OplockLevelValue.None,
                                    this.GenerateFileGlobalId(),
                                    request.SmbHeader.Pid);
                                this.AddOpenFile(open);
                            }
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_TRANSACTION2:
                    #region Trans2Open2
                    {
                        SmbTrans2Open2RequestPacket request = requestPacket as SmbTrans2Open2RequestPacket;
                        SmbTrans2Open2FinalResponsePacket response = responsePacket as SmbTrans2Open2FinalResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                string fileName;
                                if ((request.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                                    SmbFlags2.SMB_FLAGS2_UNICODE)
                                {
                                    fileName = Encoding.Unicode.GetString(request.Trans2Parameters.FileName);
                                }
                                else
                                {
                                    fileName = Encoding.ASCII.GetString(request.Trans2Parameters.FileName);
                                }
                                CifsServerPerOpenFile open = new CifsServerPerOpenFile(
                                    treeConnect,
                                    fileName,
                                    response.Trans2Parameters.Fid,
                                    (uint)response.Trans2Parameters.FileAttributes,
                                    OplockLevelValue.None,
                                    this.GenerateFileGlobalId(),
                                    request.SmbHeader.Pid);
                                this.AddOpenFile(open);
                            }
                        }
                    }
                    #endregion

                    #region Trans2FindFirst2
                    {
                        SmbTrans2FindFirst2RequestPacket request = requestPacket as SmbTrans2FindFirst2RequestPacket;
                        SmbTrans2FindFirst2FinalResponsePacket response = responsePacket as SmbTrans2FindFirst2FinalResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);
                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                string fileName;
                                if ((request.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) ==
                                    SmbFlags2.SMB_FLAGS2_UNICODE)
                                {
                                    fileName = Encoding.Unicode.GetString(request.Trans2Parameters.FileName);
                                }
                                else
                                {
                                    fileName = Encoding.ASCII.GetString(request.Trans2Parameters.FileName);
                                }
                                CifsServerPerOpenSearch openSearch = new CifsServerPerOpenSearch(
                                    treeConnect,
                                    response.Trans2Parameters.SID,
                                    smbHeader.Mid,
                                    smbHeader.Pid,
                                    this.GenerateSearchGlobalId());
                                this.AddOpenSearch(openSearch);
                            }
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_WRITE_AND_CLOSE:
                    #region SMB_COM_WRITE_AND_CLOSE
                    {
                        SmbWriteAndCloseRequestPacket request = requestPacket as SmbWriteAndCloseRequestPacket;
                        SmbWriteAndCloseResponsePacket response = responsePacket as SmbWriteAndCloseResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                CifsServerPerOpenFile open = treeConnect.GetOpen(request.SmbParameters.FID);
                                if (open != null)
                                {
                                    this.RemoveOpenFile(open.FileGlobalId);
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_CLOSE:
                    #region SMB_COM_CLOSE
                    {
                        SmbCloseRequestPacket request = requestPacket as SmbCloseRequestPacket;
                        SmbCloseResponsePacket response = responsePacket as SmbCloseResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);
                        
                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                CifsServerPerOpenFile open = treeConnect.GetOpen(request.SmbParameters.FID);
                                if (open == null)
                                {
                                    open = treeConnect.GetOpen((ushort)smbHeader.Protocol);
                                }
                                if (open != null)
                                {
                                    this.RemoveOpenFile(open.FileGlobalId);
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case SmbCommand.SMB_COM_FIND_CLOSE2:
                    #region SMB_COM_FIND_CLOSE2
                    {
                        SmbFindClose2RequestPacket request = requestPacket as SmbFindClose2RequestPacket;
                        SmbFindClose2ResponsePacket response = responsePacket as SmbFindClose2ResponsePacket;
                        CifsServerPerSession session = connection.GetSession(smbHeader.Uid);

                        if (request != null && response != null && session != null)
                        {
                            CifsServerPerTreeConnect treeConnect = session.GetTreeConnect(smbHeader.Tid);
                            if (treeConnect != null)
                            {
                                CifsServerPerOpenSearch openSearch = treeConnect.GetOpenSearch(
                                    request.SmbParameters.SearchHandle); ;
                                if (openSearch != null)
                                {
                                    this.RemoveOpenSearch(openSearch.SearchGlobalId);
                                }
                            }
                        }
                    }
                    #endregion
                    break;

                #endregion

                default:
                    // No Connection/Session/Tree/Open will be updated if other types of response.
                    break;
            }

            SmbBatchedRequestPacket batchedRequest = requestPacket as SmbBatchedRequestPacket;
            SmbBatchedResponsePacket batchedResponse = responsePacket as SmbBatchedResponsePacket;

            if (batchedRequest != null && batchedResponse != null)
            {

                //pass the FID stored in the andxHeader.Protocol into response.AndxPacket
                if (batchedRequest.AndxPacket != null && batchedResponse.AndxPacket != null
                    && batchedResponse.SmbHeader.Protocol != CifsMessageUtils.SMB_PROTOCOL_ANDXPACKET
                    && batchedResponse.SmbHeader.Protocol != CifsMessageUtils.SMB_PROTOCOL_IDENTIFIER)
                {
                    SmbHeader andxHeader = batchedResponse.AndxPacket.SmbHeader;
                    andxHeader.Protocol = smbHeader.Protocol;
                    batchedResponse.AndxPacket.SmbHeader = andxHeader;
                }
                this.UpdateResponseRoleContext(connection, batchedRequest.AndxPacket, batchedResponse.AndxPacket);
            }
        }

        #endregion

        #region Access ConnectionTable

        /// <summary>
        /// Add a connection into ConnectionTable
        /// </summary>
        /// <param name="connection">The connection to be added or updated.</param>
        /// <exception cref="ArgumentNullException">The connection is null</exception>
        /// <exception cref="ArgumentException">The connection already exists</exception>
        public void AddConnection(CifsServerPerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentException("The connection is null.");
            }

            lock (this.globalTables)
            {
                this.globalTables.ConnectionTable.Add(connection.Identity, connection);
            }
        }


        /// <summary>
        /// Remove all connections and release associated resources.
        /// </summary>
        public void RemoveAllConnections()
        {
            lock (this.globalTables)
            {
                List<CifsServerPerConnection> tempTable = 
                    new List<CifsServerPerConnection>(this.globalTables.ConnectionTable.Values);
                foreach (CifsServerPerConnection connection in tempTable)
                {
                    RemoveConnection(connection.Identity);
                }
            }
        }


        /// <summary>
        /// Remove a connection from the ConnectionTable and release associated resources.
        /// </summary>
        /// <param name="identify">The identification of the connection to remove</param>
        /// <exception cref="ArgumentNullException">The identify param is null.</exception>
        public void RemoveConnection(object identify)
        {
            lock (this.globalTables)
            {
                if (this.globalTables.ConnectionTable.ContainsKey(identify))
                {
                    CifsServerPerConnection connection = this.globalTables.ConnectionTable[identify];

                    List<IFileServiceServerSession> tempTable =
                        new List<IFileServiceServerSession>(connection.SessionTable);
                    foreach (CifsServerPerSession session in tempTable)
                    {
                        RemoveSession(session.SessionGlobalId);
                    }
                    this.globalTables.ConnectionTable.Remove(identify);
                }
            }
        }

        #endregion

        #region Access GlobalSessionTable


        /// <summary>
        /// Add a session into GlobalSessionTable, and add itself into its own connection's SessionTable.
        /// </summary>
        /// <param name="session">The session to be added or updated.</param>
        /// <exception cref="System.ArgumentNullException">The session or session.connection is null.</exception>
        /// <exception cref="System.ArgumentException">The session already exists</exception>
        public void AddSession(CifsServerPerSession session)
        {
            if (session == null || session.Connection == null)
            {
                throw new ArgumentException("The session or session.connection is null.");
            }

            lock (this.globalTables)
            {
                (session.Connection as CifsServerPerConnection).AddSession(session);
                this.globalTables.GlobalSessionTable.Add(session.SessionGlobalId, session);
            }
        }


        /// <summary>
        /// Remove all sessions from the GlobalSessionTable and clear the SessionTable of every connection.
        /// </summary>
        public void RemoveAllSessions()
        {
            lock (this.globalTables)
            {
                List<CifsServerPerSession> tempTable = 
                    new List<CifsServerPerSession>(this.globalTables.GlobalSessionTable.Values);
                foreach (CifsServerPerSession session in tempTable)
                {
                    RemoveSession(session.SessionGlobalId);
                }
            }
        }


        /// <summary>
        /// Remove a session identified by the globalIndex from the globalSessionTable, and remove it from its
        /// own connection's SessionTable.
        /// </summary>
        /// <param name="globalIndex">The sessionGlobalId of the session to be removed.</param>
        /// <exception cref="KeyNotFoundException">The key not found in the collection.</exception>
        public void RemoveSession(int globalIndex)
        {
            lock (this.globalTables)
            {
                CifsServerPerSession session = this.globalTables.GlobalSessionTable[globalIndex];
                List<IFileServiceServerTreeConnect> tempTable = 
                    new List<IFileServiceServerTreeConnect>(session.TreeConnectTable);
                foreach (CifsServerPerTreeConnect treeConnect in tempTable)
                {
                    RemoveTreeConnect(treeConnect.TreeGlobalId);
                }

                (session.Connection as CifsServerPerConnection).RemoveSession((ushort)session.SessionId);
                this.globalTables.GlobalSessionTable.Remove(globalIndex);
            }
        }

        #endregion

        #region Access GlobalTreeConnectTable


        /// <summary>
        /// Add a tree connect into GlobalTreeConnectTable, and add itself to it's own session.TreeConnectTable.
        /// </summary>
        /// <param name="treeConnect">The treeConnect to be added or updated.</param>
        /// <exception cref="System.ArgumentNullException">The treeConnect or treeConnect.Sessionis null.</exception>
        /// <exception cref="System.ArgumentException">The treeConnect already exists</exception>
        public void AddTreeConnect(CifsServerPerTreeConnect treeConnect)
        {
            if (treeConnect == null || treeConnect.Session == null)
            {
                throw new ArgumentException("The treeConnect or treeConnect.Session is null.");
            }

            lock (this.globalTables)
            {
                (treeConnect.Session as CifsServerPerSession).AddTreeConnect(treeConnect);
                this.globalTables.GlobalTreeConnectTable.Add(treeConnect.TreeGlobalId, treeConnect);
            }
        }


        /// <summary>
        /// Remove all TreeConnects  from the GlobalTreeConnectTable, and clear all the sessions' TreeConnectTable.
        /// </summary>
        public void RemoveAllTreeConnects()
        {
            lock (this.globalTables)
            {
                List<CifsServerPerTreeConnect> tempTable =
                    new List<CifsServerPerTreeConnect>(this.GlobalTreeConnectTable.Values);
                foreach (CifsServerPerTreeConnect treeConnect in tempTable)
                {
                    RemoveTreeConnect(treeConnect.TreeGlobalId);
                }
            }
        }


        /// <summary>
        /// Remove a TreeConnect identified by the TreeGlobalId from the globalTreeConnectTable, and from its own 
        /// session's TreeConnectTable.
        /// </summary>
        /// <param name="globalIndex">The globalIndex of the TreeConnect to be removed.</param>
        /// <exception cref="KeyNotFoundException">The key not found in the collection.</exception>
        public void RemoveTreeConnect(int globalIndex)
        {
            lock (this.globalTables)
            {
                CifsServerPerTreeConnect treeConnect = this.globalTables.GlobalTreeConnectTable[globalIndex];
                foreach (CifsServerPerOpenFile open in treeConnect.OpenTable)
                {
                    this.RemoveOpenFile(open.FileGlobalId);
                }

                foreach (CifsServerPerOpenSearch openSearch in treeConnect.OpenSearchTable)
                {
                    this.RemoveOpenSearch(openSearch.SearchGlobalId);
                }

                (treeConnect.Session as CifsServerPerSession).RemoveTreeConnect((ushort)treeConnect.TreeConnectId);
                this.globalTables.GlobalTreeConnectTable.Remove(globalIndex);
            }
        }

        #endregion

        #region Access GlobalOpenTable

        /// <summary>
        /// Add an open into the GlobalOpenTable, and to its own TreeConnect's OpenTable.
        /// </summary>
        /// <param name="open">The open to be added or updated.</param>
        /// <exception cref="System.ArgumentNullException">The open or open.TreeConnectis null</exception>
        /// <exception cref="System.ArgumentException">The open already exists</exception>
        public void AddOpenFile(CifsServerPerOpenFile open)
        {
            if (open == null || open.TreeConnect == null)
            {
                throw new ArgumentException("The open or open.TreeConnect is null.");
            }

            lock (this.globalTables)
            {
                (open.TreeConnect as CifsServerPerTreeConnect).AddOpen(open);
                this.globalTables.GlobalOpenTable.Add(open.FileGlobalId, open);
            }
        }


        /// <summary>
        /// Remove all opens from the GlobalOpenTable, and clear all the tree connect's OpenTable.
        /// </summary>
        public void RemoveAllOpenFiles()
        {
            lock (this.globalTables)
            {
                foreach (CifsServerPerOpenFile open in this.globalTables.GlobalOpenTable.Values)
                {
                    (open.TreeConnect as CifsServerPerTreeConnect).RemoveOpen((ushort)open.FileId);
                }
                this.globalTables.GlobalOpenTable.Clear();
            }
        }


        /// <summary>
        /// Remove an open identified by the globalIndex from the globalOpenTable, and from its own tree connect's
        /// OpenTable.
        /// </summary>
        /// <param name="globalIndex">The globalIndex of the Open to be removed.</param>
        /// <exception cref="KeyNotFoundException">The key not found in the collection.</exception>
        public void RemoveOpenFile(int globalIndex)
        {
            lock (this.globalTables)
            {
                CifsServerPerOpenFile open = this.globalTables.GlobalOpenTable[globalIndex];
                (open.TreeConnect as CifsServerPerTreeConnect).RemoveOpen((ushort)open.FileId);
                this.globalTables.GlobalOpenTable.Remove(globalIndex);
            }
        }

        #endregion

        #region Access GlobalOpenSearchTable


        /// <summary>
        /// Add an open search into GlobalOpenSearchTable, and to its own tree connect's OpenSearchTable.
        /// </summary>
        /// <param name="openSearch">The open search to be added or updated.</param>
        /// <exception cref="System.ArgumentNullException">The openSearch or openSearch.TreeConnect is null.</exception>
        /// <exception cref="System.ArgumentException">The open search already exists</exception>
        public void AddOpenSearch(CifsServerPerOpenSearch openSearch)
        {
            if (openSearch == null || openSearch.TreeConnect == null)
            {
                throw new ArgumentException("The openSearch or openSearch.TreeConnect is null.");
            }

            lock (this.globalTables)
            {
                (openSearch.TreeConnect as CifsServerPerTreeConnect).AddOpenSearch(openSearch);
                this.globalTables.GlobalOpenSearchTable.Add(openSearch.SearchGlobalId, openSearch);
            }
        }


        /// <summary>
        /// Remove all opens from the GlobalOpenSearchTable, and clear all the tree connect's OpenSearchTable.
        /// </summary>
        public void RemoveAllOpenSearches()
        {
            lock (this.globalTables)
            {
                foreach (CifsServerPerOpenSearch openSearch in this.globalTables.GlobalOpenSearchTable.Values)
                {
                    (openSearch.TreeConnect as CifsServerPerTreeConnect).RemoveOpenSearch(openSearch.FindSID);
                }
                this.globalTables.GlobalOpenSearchTable.Clear();
            }
        }


        /// <summary>
        /// Remove an open search identified by the globalIndex from the GlobalOpenSearchTable, and from its own
        /// treeconnect's OpenSearchTable.
        /// </summary>
        /// <param name="globalIndex">The globalIndex of the open search to be removed.</param>
        /// <exception cref="KeyNotFoundException">The key not found in the collection.</exception>
        public void RemoveOpenSearch(int globalIndex)
        {
            lock (this.globalTables)
            {
                CifsServerPerOpenSearch openSearch = this.globalTables.GlobalOpenSearchTable[globalIndex];
                (openSearch.TreeConnect as CifsServerPerTreeConnect).RemoveOpenSearch(openSearch.FindSID);
                this.globalTables.GlobalOpenSearchTable.Remove(globalIndex);
            }
        }

        #endregion
    }
}