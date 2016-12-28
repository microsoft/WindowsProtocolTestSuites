// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// A structure contains information about context
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class Smb2ServerContext : IDisposable
    {
        #region Field

        internal bool requireMessageSigning;
        internal Dictionary<string, Smb2ServerShare> shareList;
        internal Dictionary<ulong, Smb2ServerOpen> globalOpenTable;
        internal Dictionary<ulong, Smb2ServerSession> globalSessionTable;
        internal Dictionary<int, Smb2ServerConnection> connectionList;
        internal Guid serverGuid;
        internal DateTime serverStartTime;
        internal bool isDfsCapable;
        internal object contextLocker = new object();

        //2.1 dialect feature
        private Dictionary<Guid, Smb2LeaseTable> globalLeaseTableList;

        internal Smb2TransportType transportType;
        private bool disposed;

        #endregion

        #region Properties

        /// <summary>
        /// A Boolean that, if set, indicates that this node requires that messages MUST be signed 
        /// if the message is sent with a user security context that is neither anonymous nor guest.
        /// If not set, this node does not require that any messages be signed, 
        /// but MAY still choose to do so if the other node requires it
        /// </summary>
        public bool RequireMessageSigning
        {
            get
            {
                return requireMessageSigning;
            }
        }

        /// <summary>
        /// A list of available shares for the system. 
        /// The structure of a share is as specified in section 3.3.1.7 and is uniquely indexed by the share name.
        /// </summary>
        public ReadOnlyDictionary<string, Smb2ServerShare> ShareList
        {
            get
            {
                return new ReadOnlyDictionary<string, Smb2ServerShare>(shareList);
            }
        }

        /// <summary>
        /// A table containing all the files opened by remote clients on the server, indexed by Open.DurableFileId.
        /// The structure of an open is as specified in section 3.3.1.11. The table MUST support enumeration of all 
        /// entries in the table.
        /// </summary>
        [CLSCompliant(false)]
        public ReadOnlyDictionary<ulong, Smb2ServerOpen> GlobalOpenTable
        {
            get
            {
                return new ReadOnlyDictionary<ulong, Smb2ServerOpen>(globalOpenTable);
            }
        }

        /// <summary>
        /// A list of all the active sessions established to this server, indexed by the Session.SessionId.
        /// The server MUST also be able to search the list by security principal,
        /// and the list MUST allow for multiple sessions with the same security principal on different connections
        /// </summary>
        [CLSCompliant(false)]
        public ReadOnlyDictionary<ulong, Smb2ServerSession> GlobalSessionTable
        {
            get
            {
                return new ReadOnlyDictionary<ulong, Smb2ServerSession>(globalSessionTable);
            }
        }

        /// <summary>
        /// A list of all open connections on the server, indexed by the connection endpoint addresses.
        /// </summary>
        public ReadOnlyDictionary<int, Smb2ServerConnection> ConnectionList
        {
            get
            {
                return new ReadOnlyDictionary<int, Smb2ServerConnection>(connectionList);
            }
        }

        /// <summary>
        /// A global identifier for this server
        /// </summary>
        public Guid ServerGuid
        {
            get
            {
                return serverGuid;
            }
        }

        /// <summary>
        /// The start time of the SMB2 server
        /// </summary>
        public DateTime ServerStartTime
        {
            get
            {
                return serverStartTime;
            }
        }

        /// <summary>
        /// A Boolean that, if set, indicates that the server supports the Distributed File System.
        /// </summary>
        public bool IsDfsCapable
        {
            get
            {
                return isDfsCapable;
            }
        }

        /// <summary>
        /// A list of all the lease tables as described in 3.3.1.12, indexed by the ClientGuid.
        /// </summary>
        public ReadOnlyDictionary<Guid, Smb2LeaseTable> GlobalLeaseTableList
        {
            get
            {
                return new ReadOnlyDictionary<Guid, Smb2LeaseTable>(globalLeaseTableList);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2ServerContext()
        {
            globalOpenTable = new Dictionary<ulong, Smb2ServerOpen>();
            globalSessionTable = new Dictionary<ulong, Smb2ServerSession>();
            serverGuid = Guid.NewGuid();
            connectionList = new Dictionary<int, Smb2ServerConnection>();
            serverStartTime = DateTime.Now;

            shareList = new Dictionary<string, Smb2ServerShare>();
            globalLeaseTableList = new Dictionary<Guid, Smb2LeaseTable>();
        }

        #endregion

        #region Protocol Related Functions

        /// <summary>
        /// Update context based on the endpoint and the packet
        /// </summary>
        /// <param name="smb2Event">contain the update information</param>
        internal void UpdateContext(Smb2Event smb2Event)
        {
            lock (contextLocker)
            {
                switch (smb2Event.Type)
                {
                    case Smb2EventType.Connected:
                        HandleConnectedEvent(smb2Event);
                        break;
                    case Smb2EventType.PacketReceived:
                        HandlePacketReceivedEvent(smb2Event);
                        break;
                    case Smb2EventType.PacketSent:
                        HandlePacketSentEvent(smb2Event);
                        break;
                    case Smb2EventType.Disconnected:
                        HandleDisconnectedEvent(smb2Event);
                        break;
                }
            }
        }


        /// <summary>
        /// Handle new connection event
        /// </summary>
        /// <param name="smb2Event">contain the update information</param>
        private void HandleConnectedEvent(Smb2Event smb2Event)
        {
            Smb2ServerConnection connection = new Smb2ServerConnection();

            connection.commandSequenceWindow = new List<ulong>();
            //when a new connection established, the sequncewindow will contain one sequnce number.
            connection.GrandCredit(1);
            connection.asyncCommandList = new Dictionary<ulong, AsyncCommand>();
            connection.requestList = new Dictionary<ulong,Smb2Packet>();
            connection.clientCapabilities = 0;
            connection.negotiateDialect = 0xffff;
            connection.dialect = "Unknown";
            connection.shouldSign = false;
            connection.connectionId = smb2Event.ConnectionId;

            connectionList.Add(smb2Event.ConnectionId, connection);
        }


        /// <summary>
        /// Handle PacketReceived event
        /// </summary>
        /// <param name="smb2Event">contain the update information</param>
        private void HandlePacketReceivedEvent(Smb2Event smb2Event)
        {
            //An SMB2 CANCEL Request is the only request received by the server that
            //is not signed and does not contain a sequence number that must be checked.
            //Thus, the server MUST NOT process the received packet as specified in sections 3.3.5.2.2 and 3.3.5.2.3.
            if (smb2Event.Packet is Smb2CancelRequestPacket)
            {
                return;
            }

            if (smb2Event.Packet is Smb2CompoundPacket)
            {
                Smb2CompoundPacket compoundPacket = smb2Event.Packet as Smb2CompoundPacket;

                foreach (Smb2SinglePacket innerPacket in compoundPacket.Packets)
                {
                    innerPacket.OuterCompoundPacket = compoundPacket;
                    Smb2Event compoundPacketReceivedEvent = new Smb2Event();
                    compoundPacketReceivedEvent.ConnectionId = smb2Event.ConnectionId;
                    compoundPacketReceivedEvent.Packet = innerPacket;
                    compoundPacketReceivedEvent.Type = smb2Event.Type;

                    HandlePacketReceivedEvent(compoundPacketReceivedEvent);
                }
            }
            else
            {
                bool sequenceIdAllowed = VerifyMessageId(smb2Event.Packet, smb2Event.ConnectionId);

                if (!sequenceIdAllowed)
                {
                    throw new InvalidOperationException("Received a packet whose messageId is not valid");
                }

                SetSessionKeyInPacket(smb2Event.ConnectionId, smb2Event.Packet);

                bool isMatch = smb2Event.Packet.VerifySignature();

                if (!isMatch)
                {
                    throw new InvalidOperationException("signature is not correct.");
                }

                ulong messageId = 0;

                Smb2SinglePacket singlePacket = smb2Event.Packet as Smb2SinglePacket;

                if (singlePacket != null)
                {
                    messageId = singlePacket.Header.MessageId;
                }

                connectionList[smb2Event.ConnectionId].requestList.Add(messageId, smb2Event.Packet);

                if (singlePacket != null)
                {
                    switch (singlePacket.Header.Command)
                    {
                        case Smb2Command.QUERY_DIRECTORY:
                            HandleReceiveQueryDirectoryRequestEvent(smb2Event);
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Handle the event of receiving query directory request
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleReceiveQueryDirectoryRequestEvent(Smb2Event smb2Event)
        {
            Smb2QueryDirectoryRequestPacket packet = smb2Event.Packet as Smb2QueryDirectoryRequestPacket;

            byte[] fileNameArray = new byte[0];

            string fileName = string.Empty;

            if (packet.PayLoad.FileNameLength != 0)
            {
                fileNameArray = new byte[packet.PayLoad.FileNameLength];

                Array.Copy(packet.PayLoad.Buffer, packet.PayLoad.FileNameOffset - Smb2Consts.FileNameOffsetInQueryDirectoryRequest,
                    fileNameArray, 0, fileNameArray.Length);

                fileName = Encoding.Unicode.GetString(fileNameArray);
            }

            Smb2ServerOpen open = globalSessionTable[packet.GetSessionId()].openTable[packet.GetFileId()];

            if ((packet.PayLoad.Flags & QUERY_DIRECTORY_Request_Flags_Values.REOPEN)
                == QUERY_DIRECTORY_Request_Flags_Values.REOPEN)
            {
                open.enumerationLocation = 0;
                open.enumerationSearchPattern = string.Empty;
            }

            if ((packet.PayLoad.Flags & QUERY_DIRECTORY_Request_Flags_Values.RESTART_SCANS)
                == QUERY_DIRECTORY_Request_Flags_Values.RESTART_SCANS)
            {
                open.enumerationLocation = 0;
            }

            if (open.enumerationLocation == 0 && string.IsNullOrEmpty(open.enumerationSearchPattern))
            {
                open.enumerationSearchPattern = fileName;
            }

            if ((packet.PayLoad.Flags & QUERY_DIRECTORY_Request_Flags_Values.INDEX_SPECIFIED)
                == QUERY_DIRECTORY_Request_Flags_Values.INDEX_SPECIFIED)
            {
                open.enumerationLocation = (int)packet.PayLoad.FileIndex;

                if (string.IsNullOrEmpty(fileName))
                {
                    open.enumerationSearchPattern = fileName;
                }
            }
        }


        /// <summary>
        /// Handle event of receiving packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandlePacketSentEvent(Smb2Event smb2Event)
        {
            Smb2CompoundPacket compoundPacket = smb2Event.Packet as Smb2CompoundPacket;

            if (compoundPacket != null)
            {
                foreach (Smb2Packet innerPacket in compoundPacket.Packets)
                {
                    Smb2Event compoundPacketEvent = new Smb2Event();

                    compoundPacketEvent.ConnectionId = smb2Event.ConnectionId;
                    compoundPacketEvent.Packet = innerPacket;
                    compoundPacketEvent.Type = smb2Event.Type;

                    HandlePacketSentEvent(compoundPacketEvent);
                }
            }
            else
            {
                Smb2SinglePacket singlePacket = smb2Event.Packet as Smb2SinglePacket;

                GrandCredit(singlePacket, smb2Event.ConnectionId);

                if ((singlePacket.Header.Flags & Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND)
                    == Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND)
                {
                    HandleSendFinnalAsyncResponseEvent(smb2Event);
                }

                Smb2ErrorResponsePacket errorResponse = singlePacket as Smb2ErrorResponsePacket;

                if (errorResponse != null)
                {
                    HandleSendErrorResponseEvent(smb2Event);
                    return;
                }

                switch (singlePacket.Header.Command)
                {
                    case Smb2Command.NEGOTIATE:
                        HandleSendNegotiateResponseEvent(smb2Event);
                        break;
                    case Smb2Command.SESSION_SETUP:
                        HandleSendSessionSetupResponseEvent(smb2Event);
                        break;
                    case Smb2Command.LOGOFF:
                        HandleSendLogOffResponseEvent(smb2Event);
                        break;
                    case Smb2Command.TREE_CONNECT:
                        HandleSendTreeConnectResponseEvent(smb2Event);
                        break;
                    case Smb2Command.TREE_DISCONNECT:
                        HandleSendTreeDisconnectResponseEvent(smb2Event);
                        break;
                    case Smb2Command.CREATE:
                        HandleSendCreateResponseEvent(smb2Event);
                        break;
                    case Smb2Command.CLOSE:
                        HandleSendCloseResponseEvent(smb2Event);
                        break;
                    case Smb2Command.OPLOCK_BREAK:
                        HandleSendOplockBreakResponseEvent(smb2Event);
                        break;
                    case Smb2Command.LOCK:
                        HandleSendLockResponseEvent(smb2Event);
                        break;
                    case Smb2Command.IOCTL:
                        HandleSendIOCtlResponseEvent(smb2Event);
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// Handle sending error response event
        /// </summary>
        /// <param name="smb2Event"></param>
        private void HandleSendErrorResponseEvent(Smb2Event smb2Event)
        {
            Smb2ErrorResponsePacket packet = smb2Event.Packet as Smb2ErrorResponsePacket;

            // It is an interim response packet
            if (packet.Header.Status == (uint)Smb2Status.STATUS_PENDING)
            {
                ulong asyncId = Smb2Utility.AssembleToAsyncId(packet.Header.ProcessId, packet.Header.TreeId);
                AsyncCommand asyncCommand = new AsyncCommand();
                asyncCommand.asyncId = asyncId;
                asyncCommand.requestPacket = smb2Event.Packet;

                connectionList[smb2Event.ConnectionId].asyncCommandList.Add(asyncId, asyncCommand);
            }
        }


        /// <summary>
        /// Handle sending final async response event
        /// </summary>
        /// <param name="smb2Event"></param>
        private void HandleSendFinnalAsyncResponseEvent(Smb2Event smb2Event)
        {
            Smb2SinglePacket packet = smb2Event.Packet as Smb2SinglePacket;

            ulong asyncId = Smb2Utility.AssembleToAsyncId(packet.Header.ProcessId, packet.Header.TreeId);

            connectionList[smb2Event.ConnectionId].asyncCommandList.Remove(asyncId);
        }


        /// <summary>
        /// Grand credit to client
        /// </summary>
        /// <param name="packet">The response packet</param>
        /// <param name="connectionId">Used to find the connection</param>
        private void GrandCredit(Smb2SinglePacket packet, int connectionId)
        {
            connectionList[connectionId].GrandCredit(packet.Header.CreditRequest_47_Response);
        }


        /// <summary>
        /// Handle event of sending negotiate response packet based on receiving a smb2 negotiate request
        /// packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendNegotiateResponseEvent(Smb2Event smb2Event)
        {
            Smb2Packet packet = FindRequestPacket(smb2Event.ConnectionId, 0);

            if (packet is SmbNegotiateRequestPacket)
            {
                HandleSendSmb2NegotiateResponsev1Event(smb2Event);
            }
            else
            {
                HandleSendSmb2NegotiateResponsev2Event(smb2Event);
            }
        }


        /// <summary>
        /// Handle event of sending smb2 negotiate response packet based on receiving smb negotiate request packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendSmb2NegotiateResponsev1Event(Smb2Event smb2Event)
        {
            Smb2NegotiateResponsePacket packet = smb2Event.Packet as Smb2NegotiateResponsePacket;
            int connectionId = smb2Event.ConnectionId;

            if (connectionList.ContainsKey(connectionId) && connectionList[connectionId].negotiateDialect != 0xffff)
            {
                //The protocol version has been negotiated, the event is not valid, but sdk can't throw exception for
                //this situation, because maybe user means to do that.
                return;
            }

            if (packet.PayLoad.DialectRevision == DialectRevision_Values.V1)
            {
                connectionList[connectionId].negotiateDialect = Smb2Consts.NegotiateDialect2_02;
                connectionList[connectionId].dialect = Smb2Consts.NegotiateDialect2_02String;
            }
            else if (packet.PayLoad.DialectRevision == DialectRevision_Values.V3)
            {
                connectionList[connectionId].negotiateDialect = Smb2Consts.NegotiateDialect2_XX;
            }
            else
            {
                //do nothing to invalid dialectRevision
            }
        }


        /// <summary>
        /// Handle event of sending smb2 negotiate response packet based on receiving a smb2 negotiate
        /// request packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendSmb2NegotiateResponsev2Event(Smb2Event smb2Event)
        {
            Smb2NegotiateResponsePacket packet = smb2Event.Packet as Smb2NegotiateResponsePacket;
            int connectionId = smb2Event.ConnectionId;

            if (connectionList.ContainsKey(connectionId) && (connectionList[connectionId].negotiateDialect == 0x0202 ||
                connectionList[connectionId].negotiateDialect == 0x0210))
            {
                // if the negotiate is complete before, td says this connection MUST disconnect, but this is not sdk's duty to 
                // disconnect it, user must do that. so here we just ignore it.
                return;
            }

            if (packet.PayLoad.DialectRevision == DialectRevision_Values.V1)
            {
                connectionList[connectionId].negotiateDialect = Smb2Consts.NegotiateDialect2_02;
                connectionList[connectionId].dialect = Smb2Consts.NegotiateDialect2_02String;
            }
            else if (packet.PayLoad.DialectRevision == DialectRevision_Values.V2)
            {
                connectionList[connectionId].negotiateDialect = Smb2Consts.NegotiateDialect2_10;
                connectionList[connectionId].dialect = Smb2Consts.NegotiateDialect2_10String;
            }
            else
            {
                //the other value is not correct, but for the negtive test, we ignore it here.
            }
        }


        /// <summary>
        /// Handle the event of sending session setup response packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendSessionSetupResponseEvent(Smb2Event smb2Event)
        {
            Smb2SessionSetupResponsePacket packet = smb2Event.Packet as Smb2SessionSetupResponsePacket;

            Smb2SessionSetupRequestPacket requestPacket = FindRequestPacket(smb2Event.ConnectionId, packet.Header.MessageId)
                as Smb2SessionSetupRequestPacket;

            if (requestPacket.PayLoad.PreviousSessionId != 0)
            {
                HandleReAuthenticateEvent(packet, smb2Event.ConnectionId);
            }
            else
            {
                HandleNewAuthenticateEvent(packet, smb2Event.ConnectionId);
            }
        }


        /// <summary>
        /// Handle the event of sending log off response packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendLogOffResponseEvent(Smb2Event smb2Event)
        {
            Smb2LogOffResponsePacket packet = smb2Event.Packet as Smb2LogOffResponsePacket;

            globalSessionTable.Remove(packet.GetSessionId());
        }


        /// <summary>
        /// Handle the event of sending treeConnect response packet
        /// </summary>
        /// <param name="smb2Event">Contain event information</param>
        private void HandleSendTreeConnectResponseEvent(Smb2Event smb2Event)
        {
            Smb2TreeConnectResponsePacket packet = smb2Event.Packet as Smb2TreeConnectResponsePacket;
            Smb2ServerTreeConnect treeConnect = new Smb2ServerTreeConnect();

            treeConnect.treeId = packet.GetTreeId();
            globalSessionTable[packet.GetSessionId()].treeConnectTable.Add(treeConnect.treeId, treeConnect);
        }


        /// <summary>
        /// Handle the event of sending tree disconnect response packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendTreeDisconnectResponseEvent(Smb2Event smb2Event)
        {
            Smb2TreeDisconnectResponsePacket packet = smb2Event.Packet as Smb2TreeDisconnectResponsePacket;

            globalSessionTable[packet.GetSessionId()].treeConnectTable.Remove(packet.GetTreeId());
        }


        /// <summary>
        /// Handle the event of authenticate
        /// </summary>
        /// <param name="packet">The session setup response packet</param>
        /// <param name="connectionId">Used to find the connection</param>
        private void HandleNewAuthenticateEvent(Smb2SessionSetupResponsePacket packet, int connectionId)
        {
            if (!globalSessionTable.ContainsKey(packet.GetSessionId()))
            {
                Smb2ServerSession session = new Smb2ServerSession();

                session.connection = connectionList[connectionId];
                session.state = SessionState.InProgress;
                session.securityContext = null;
                session.sessionId = packet.GetSessionId();
                session.openTable = new Dictionary<FILEID, Smb2ServerOpen>();
                session.treeConnectTable = new Dictionary<uint, Smb2ServerTreeConnect>();

                globalSessionTable.Add(session.sessionId, session);
            }

            Smb2SessionSetupRequestPacket requestPacket = FindRequestPacket(connectionId, packet.Header.MessageId)
                as Smb2SessionSetupRequestPacket;

            if (packet.Header.Status == 0)
            {
                if (connectionList[connectionId].clientCapabilities == 0)
                {
                    connectionList[connectionId].clientCapabilities = requestPacket.PayLoad.Capabilities;
                }

                if (((packet.PayLoad.SessionFlags & SessionFlags_Values.SESSION_FLAG_IS_GUEST) == SessionFlags_Values.SESSION_FLAG_IS_GUEST)
                    || ((packet.PayLoad.SessionFlags & SessionFlags_Values.SESSION_FLAG_IS_NULL) == SessionFlags_Values.SESSION_FLAG_IS_NULL))
                {
                    //should sign set to false. do not need to set it manually.
                }
                else
                {
                    if (((requestPacket.PayLoad.SecurityMode & SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED)
                        == SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED) && (this.requireMessageSigning ||
                        connectionList[connectionId].shouldSign))
                    {
                        globalSessionTable[packet.GetSessionId()].shouldSign = true;
                    }
                }

                globalSessionTable[packet.GetSessionId()].sessionKey = connectionList[connectionId].gss.SessionKey;
                globalSessionTable[packet.GetSessionId()].state = SessionState.Valid;
                //Set it to null because if another authentiate request arrives, gss must be
                //set to a new one. set to null as a flag to indicate gss must re-construct.

                //release gss, set to null
                connectionList[connectionId].ReleaseSspiServer();
            }
        }


        /// <summary>
        /// Handle re-authenticate event
        /// </summary>
        /// <param name="packet">The session setup response packet</param>
        /// <param name="connectionId">Used to find the connection</param>
        private void HandleReAuthenticateEvent(Smb2SessionSetupResponsePacket packet, int connectionId)
        {
            if (packet.Header.Status == 0)
            {
                globalSessionTable[packet.GetSessionId()].state = SessionState.Valid;
            }
        }


        /// <summary>
        /// handle the event of sending back createResponse packet to client
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendCreateResponseEvent(Smb2Event smb2Event)
        {
            Smb2CreateResponsePacket packet = smb2Event.Packet as Smb2CreateResponsePacket;
            Smb2CreateRequestPacket requestPacket = FindRequestPacket(smb2Event.ConnectionId, packet.Header.MessageId)
                as Smb2CreateRequestPacket;

            #region Handle SMB2_CREATE_DURABLE_HANDLE_RECONNECT create context

            CREATE_CONTEXT_Values[] responseCreateContexts = packet.GetCreateContexts();
            CREATE_CONTEXT_Values[] requestCreateContexts = requestPacket.GetCreateContexts();

            if (requestCreateContexts != null)
            {
                foreach (CREATE_CONTEXT_Values createContext in requestCreateContexts)
                {
                    CreateContextTypeValue createContextType = Smb2Utility.GetContextType(createContext);

                    if (createContextType == CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT)
                    {
                        Smb2ServerOpen existOpen = globalOpenTable[packet.PayLoad.FileId.Persistent];
                        existOpen.connection = connectionList[smb2Event.ConnectionId];
                        existOpen.fileId = packet.PayLoad.FileId.Volatile;
                        globalSessionTable[packet.GetSessionId()].openTable.Add(packet.PayLoad.FileId, existOpen);

                        //The "Successful Open Initialization" and "Oplock Acquisition" phases MUST be skipped
                        return;
                    }
                }
            }

            #endregion

            #region Successful Open Initialization phase

            Smb2ServerOpen open = new Smb2ServerOpen();

            open.fileId = packet.PayLoad.FileId.Volatile;
            open.durableFileId = packet.PayLoad.FileId.Persistent;
            open.session = globalSessionTable[packet.GetSessionId()];
            open.connection = connectionList[smb2Event.ConnectionId];

            //we do not open the underlying object store actually, so just set the open handle to 0. 
            open.localOpen = 0;

            //It MUST be equal to the DesiredAccess specified in the request, 
            //except in the case where MAXIMUM_ALLOWED is included in the DesiredAccess
            //BECAUSE we do not implement underlying object store, we do not know the finnal grantedAccess,
            //so assuming it equals to request access
            open.grantedAccess = requestPacket.PayLoad.DesiredAccess.ACCESS_MASK;

            open.oplockLevel = OplockLevel_Values.OPLOCK_LEVEL_NONE;
            open.oplockState = OplockState.None;
            open.oplockTimeout = new TimeSpan(0, 0, 0);
            open.isDurable = false;
            open.durableOpenTimeout = new TimeSpan(0, 0, 0);
            open.durableOwner = null;
            open.enumerationLocation = 0;
            open.enumerationSearchPattern = null;

            //Open.CurrentEaIndex is set to 1.
            open.currentEaIndex = 1;
            //Open.CurrentQuotaIndex is set to 1.
            open.currentQuotaIndex = 1;
            open.treeConnect = globalSessionTable[packet.GetSessionId()].treeConnectTable[packet.GetTreeId()];
            open.treeConnect.openCount++;
            open.lockCount = 0;
            open.pathName = requestPacket.RetreivePathName();
            open.lockSequenceArray = new byte[Smb2Consts.LockSequenceCountInServerOpen];

            globalOpenTable.Add(packet.PayLoad.FileId.Persistent, open);
            globalSessionTable[packet.GetSessionId()].openTable.Add(packet.PayLoad.FileId, open);

            #endregion

            #region Oplock Acquisition phase

            globalSessionTable[packet.GetSessionId()].openTable[packet.PayLoad.FileId].oplockLevel = packet.PayLoad.OplockLevel;

            #endregion

            #region SMB2_CREATE_REQUEST_LEASE Create Context

            if (responseCreateContexts == null)
            {
                return;
            }

            foreach (CREATE_CONTEXT_Values createContext in responseCreateContexts)
            {
                CreateContextTypeValue createContextType = Smb2Utility.GetContextType(createContext);

                if (createContextType == CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE)
                {
                    if (connectionList[smb2Event.ConnectionId].dialect != Smb2Consts.NegotiateDialect2_10String)
                    {
                        //In case sdk user do not do the thing described in td
                        return;
                    }

                    if (!globalLeaseTableList.ContainsKey(connectionList[smb2Event.ConnectionId].clientGuid))
                    {
                        Smb2LeaseTable leaseTable = new Smb2LeaseTable();

                        leaseTable.clientGuid = connectionList[smb2Event.ConnectionId].clientGuid;
                        leaseTable.leaseList = new Dictionary<Guid,Smb2Lease>();

                        globalLeaseTableList.Add(leaseTable.ClientGuid, leaseTable);
                    }

                    byte[] leaseContextBuffer = Smb2Utility.GetDataFieldInCreateContext(createContext);
                    CREATE_RESPONSE_LEASE leaseContext = TypeMarshal.ToStruct<CREATE_RESPONSE_LEASE>(leaseContextBuffer);
                    Guid leaseKey = new Guid(leaseContext.LeaseKey);

                    if (!globalLeaseTableList[connectionList[smb2Event.ConnectionId].clientGuid].LeaseList.ContainsKey(leaseKey))
                    {
                        Smb2Lease lease = new Smb2Lease();
                        lease.leaseKey = leaseKey;
                        lease.fileName = requestPacket.RetreivePathName();
                        lease.leaseBreakTimeout = new TimeSpan(0, 0, 0);
                        lease.leaseOpens = new Dictionary<FILEID, Smb2ServerOpen>();
                        lease.leaseState = LeaseStateValues.SMB2_LEASE_NONE;
                        lease.breaking = false;

                        globalLeaseTableList[connectionList[smb2Event.ConnectionId].clientGuid].leaseList.Add(leaseKey, lease);
                    }

                    globalLeaseTableList[connectionList[smb2Event.ConnectionId].clientGuid].LeaseList[leaseKey].leaseState =
                        leaseContext.LeaseState;
                    globalLeaseTableList[connectionList[smb2Event.ConnectionId].clientGuid].LeaseList[leaseKey].leaseOpens.Add(
                        packet.PayLoad.FileId, open);

                    globalSessionTable[packet.GetSessionId()].openTable[packet.PayLoad.FileId].oplockLevel = OplockLevel_Values.SMB2_OPLOCK_LEVEL_LEASE;
                    globalSessionTable[packet.GetSessionId()].openTable[packet.PayLoad.FileId].lease =
                        globalLeaseTableList[connectionList[smb2Event.ConnectionId].clientGuid];
                }
                else if (createContextType == CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST)
                {
                    globalSessionTable[packet.GetSessionId()].openTable[packet.PayLoad.FileId].isDurable = true;
                }
            }

            #endregion
        }


        /// <summary>
        /// Handle the event of sending close response
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendCloseResponseEvent(Smb2Event smb2Event)
        {
            Smb2CloseResponsePacket packet = smb2Event.Packet as Smb2CloseResponsePacket;
            Smb2CloseRequestPacket requestPacket = FindRequestPacket(smb2Event.ConnectionId, packet.Header.MessageId)
                as Smb2CloseRequestPacket;

            FILEID fileId = requestPacket.GetFileId();

            if (fileId.Persistent == ulong.MaxValue && fileId.Volatile == ulong.MaxValue)
            {
                fileId = Smb2Utility.ResolveFileIdInCompoundResponse(fileId, packet);
            }

            Smb2ServerOpen open = globalSessionTable[packet.GetSessionId()].openTable[fileId];
            globalSessionTable[packet.GetSessionId()].openTable.Remove(fileId);
            globalOpenTable.Remove(fileId.Persistent);
            open.treeConnect.openCount--;

            Guid foundLeaseGuid = new Guid();

            if (open.lease != null)
            {
                foreach (KeyValuePair<Guid, Smb2Lease> lease in open.lease.leaseList)
                {
                    foreach (FILEID id in lease.Value.leaseOpens.Keys)
                    {
                        if ((id.Volatile == fileId.Volatile) && (id.Persistent == fileId.Persistent))
                        {
                            foundLeaseGuid = lease.Key;
                            break;
                        }
                    }
                }
            }

            if (open.lease != null)
            {
                open.lease.leaseList[foundLeaseGuid].leaseOpens.Remove(fileId);

                if (open.lease.leaseList[foundLeaseGuid].leaseOpens.Count == 0)
                {
                    if (open.lease.leaseList[foundLeaseGuid].breaking)
                    {
                        open.lease.leaseList[foundLeaseGuid].leaseState = LeaseStateValues.SMB2_LEASE_NONE;
                    }
                }
            }
        }


        /// <summary>
        /// Handle the event of sending OplockBreak notification or response packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendOplockBreakResponseEvent(Smb2Event smb2Event)
        {
            Smb2SinglePacket singlePacket = smb2Event.Packet as Smb2SinglePacket;

            if (singlePacket.Header.MessageId == ulong.MaxValue)
            {
                Smb2OpLockBreakNotificationPacket oplockNotification = singlePacket as Smb2OpLockBreakNotificationPacket;

                if (oplockNotification != null)
                {
                    //oplock notification
                    globalOpenTable[oplockNotification.PayLoad.FileId.Persistent].oplockLevel = (OplockLevel_Values)oplockNotification.PayLoad.OplockLevel;
                    globalOpenTable[oplockNotification.PayLoad.FileId.Persistent].oplockState = OplockState.Breaking;
                }
                else
                {
                    if (connectionList[smb2Event.ConnectionId].dialect == Smb2Consts.NegotiateDialect2_10String)
                    {
                        //lease break notification
                        Smb2LeaseBreakNotificationPacket leaseBreakNotification = singlePacket as Smb2LeaseBreakNotificationPacket;

                        Guid clientGuid = connectionList[smb2Event.ConnectionId].clientGuid;
                        Guid leaseKey = new Guid(leaseBreakNotification.PayLoad.LeaseKey);

                        Smb2Lease lease = globalLeaseTableList[clientGuid].leaseList[leaseKey];
                        lease.breaking = true;
                        lease.breakToLeaseState = leaseBreakNotification.PayLoad.NewLeaseState;

                        foreach (Smb2ServerOpen open in lease.leaseOpens.Values)
                        {
                            open.oplockState = OplockState.Breaking;
                        }
                    }
                }
            }
            else
            {
                Smb2OpLockBreakResponsePacket oplockResponse = singlePacket as Smb2OpLockBreakResponsePacket;

                if (oplockResponse != null)
                {
                    //oplock response
                    Smb2ServerOpen open = globalSessionTable[oplockResponse.GetSessionId()].openTable[oplockResponse.GetFileId()];

                    if (oplockResponse.PayLoad.OplockLevel == OPLOCK_BREAK_Response_OplockLevel_Values.OPLOCK_LEVEL_II)
                    {
                        open.oplockLevel = OplockLevel_Values.OPLOCK_LEVEL_II;
                        open.oplockState = OplockState.Held;
                    }
                    else if (oplockResponse.PayLoad.OplockLevel == OPLOCK_BREAK_Response_OplockLevel_Values.OPLOCK_LEVEL_NONE)
                    {
                        open.oplockLevel = OplockLevel_Values.OPLOCK_LEVEL_NONE;
                        open.oplockState = OplockState.None;
                    }
                    else
                    {
                        //invalid oplock level, but we do nothing here because maybe it is a nagtive test.
                    }
                }
                else
                {
                    if (connectionList[smb2Event.ConnectionId].dialect == Smb2Consts.NegotiateDialect2_10String)
                    {
                        //lease break response
                        Smb2LeaseBreakResponsePacket leaseBreakResponse = singlePacket as Smb2LeaseBreakResponsePacket;

                        Smb2Lease lease = globalLeaseTableList[connectionList[smb2Event.ConnectionId].clientGuid].leaseList[
                            new Guid(leaseBreakResponse.PayLoad.LeaseKey)];

                        lease.leaseState = leaseBreakResponse.PayLoad.LeaseState;
                        lease.breaking = false;
                    }
                }
            }
        }


        /// <summary>
        /// Handle the event of sending lock response packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendLockResponseEvent(Smb2Event smb2Event)
        {
            Smb2LockResponsePacket packet = smb2Event.Packet as Smb2LockResponsePacket;

            Smb2LockRequestPacket requestPacket = FindRequestPacket(smb2Event.ConnectionId, packet.Header.MessageId)
                as Smb2LockRequestPacket;

            Smb2ServerOpen open = globalOpenTable[requestPacket.PayLoad.FileId.Persistent];

            bool isUnlock = (requestPacket.PayLoad.Locks[0].Flags & LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK)
                == LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;

            if (open.isResilient && (connectionList[smb2Event.ConnectionId].dialect == Smb2Consts.NegotiateDialect2_10String))
            {
                //The LockSequence field of the SMB2 lock request MUST be set to ((BucketIndex + 1) << 4) + BucketSequence
                int lockSequenceIndex = ((int)requestPacket.PayLoad.LockSequence >> 4) - 1;

                if (lockSequenceIndex < open.lockSequenceArray.Length)
                {
                    open.lockSequenceArray[lockSequenceIndex] = (byte)(requestPacket.PayLoad.LockSequence & 0xf);
                }
            }

            for (int i = 0; i < requestPacket.PayLoad.Locks.Length; i++)
            {
                if (isUnlock)
                {
                    open.lockCount--;
                }
                else
                {
                    open.lockCount++;
                }
            }
        }


        /// <summary>
        /// Handle the event of sending IOCtl response packet
        /// </summary>
        /// <param name="smb2Event">Contains event information</param>
        private void HandleSendIOCtlResponseEvent(Smb2Event smb2Event)
        {
            Smb2IOCtlResponsePacket packet = smb2Event.Packet as Smb2IOCtlResponsePacket;

            Smb2IOCtlRequestPacket requestPacket = FindRequestPacket(smb2Event.ConnectionId, packet.Header.MessageId)
                as Smb2IOCtlRequestPacket;

            switch ((CtlCode_Values)packet.PayLoad.CtlCode)
            {
                case CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY:
                    Smb2ServerOpen open = globalOpenTable[packet.PayLoad.FileId.Persistent];
                    open.isDurable = false;
                    open.isResilient = true;

                    byte[] resiliencyArray = new byte[requestPacket.PayLoad.InputCount];

                    Array.Copy(requestPacket.PayLoad.Buffer, requestPacket.PayLoad.InputOffset - Smb2Consts.InputOffsetInIOCtlRequest,
                        resiliencyArray, 0, resiliencyArray.Length);

                    NETWORK_RESILIENCY_Request resiliency = TypeMarshal.ToStruct<NETWORK_RESILIENCY_Request>(resiliencyArray);

                    //resiliency is in 1 millisecond, and timespan accept the value in 100 nanosecond
                    open.resilientOpenTimeout = new TimeSpan(10000 * resiliency.Timeout);
                    break;
            }
        }


        /// <summary>
        /// Indicate whether the packet need to be signed
        /// </summary>
        /// <param name="sessionId">The sessionId of the packet</param>
        /// <returns>True indicates the packet need to be signed, otherwise false</returns>
        internal bool ShouldPacketBeSigned(ulong sessionId)
        {
            return globalSessionTable[sessionId].shouldSign;
        }


        /// <summary>
        /// Verify if the messageId in the packet is valid
        /// </summary>
        /// <param name="packet">The received packet</param>
        /// <param name="connectionId">Used to find the connection</param>
        /// <returns>True indicate it is a valid packet, otherwise false</returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private bool VerifyMessageId(Smb2Packet packet, int connectionId)
        {
            ulong messageId = 0;

            List<ulong> messageIds = new List<ulong>();
            bool allMessageIdValid = true;

            if (packet is SmbNegotiateRequestPacket)
            {
                messageId = (packet as SmbNegotiateRequestPacket).Header.Mid;
                messageIds.Add(messageId);
            }
            else
            {
                Smb2SinglePacket singlePacket = packet as Smb2SinglePacket;

                if (singlePacket is Smb2CancelRequestPacket)
                {
                    return true;
                }
                else
                {
                    messageId = singlePacket.Header.MessageId;

                    messageIds.Add(messageId);

                    int messageIdIndex = connectionList[connectionId].commandSequenceWindow.IndexOf(messageId);

                    if (messageIdIndex == -1)
                    {
                        return false;
                    }

                    uint maxLen = 0;

                    if (transportType == Smb2TransportType.Tcp && connectionList[connectionId].dialect
                        == Smb2Consts.NegotiateDialect2_10String && singlePacket.Header.CreditCharge != 0)
                    {
                        switch (singlePacket.Header.Command)
                        {
                            case Smb2Command.READ:
                                Smb2ReadRequestPacket readRequest = singlePacket as Smb2ReadRequestPacket;
                                maxLen = readRequest.PayLoad.Length;
                                break;
                            case Smb2Command.WRITE:
                                Smb2WriteRequestPacket writeRequet = singlePacket as Smb2WriteRequestPacket;
                                maxLen = writeRequet.PayLoad.Length;
                                break;
                            case Smb2Command.CHANGE_NOTIFY:
                                Smb2ChangeNotifyRequestPacket changeNotifyRequest = singlePacket as Smb2ChangeNotifyRequestPacket;
                                maxLen = changeNotifyRequest.PayLoad.OutputBufferLength;
                                break;
                            case Smb2Command.QUERY_DIRECTORY:
                                Smb2QueryDirectoryRequestPacket queryDirectory = singlePacket as Smb2QueryDirectoryRequestPacket;
                                maxLen = queryDirectory.PayLoad.OutputBufferLength;
                                break;
                        }

                        //CreditCharge >= (max(SendPayloadSize, Expected ResponsePayloadSize) ¨C 1)/ 65536 + 1
                        int expectedCreditCharge = 1 + ((int)maxLen - 1) / 65536;

                        if (expectedCreditCharge > singlePacket.Header.CreditCharge)
                        {
                            throw new InvalidOperationException(string.Format("The CreditCharge in header is not valid. The expected value is {0}, "
                                + "and the actual value is {1}", expectedCreditCharge, singlePacket.Header.CreditCharge));
                        }

                        for (int i = 1; i < singlePacket.Header.CreditCharge; i++)
                        {
                            if ((messageIdIndex + i) < connectionList[connectionId].commandSequenceWindow.Count)
                            {
                                messageIds.Add(connectionList[connectionId].commandSequenceWindow[messageIdIndex + i]);
                            }
                            else
                            {
                                allMessageIdValid = false;
                                break;
                            }
                        }
                    }
                }
            }

            foreach (ulong item in messageIds)
            {
                if (connectionList[connectionId].commandSequenceWindow.Contains(item))
                {
                    connectionList[connectionId].RemoveMessageId(item);
                }
                else
                {
                    allMessageIdValid = false;
                }
            }

            return allMessageIdValid;
        }


        /// <summary>
        /// Set the sessionKey field of the packet
        /// </summary>
        /// <param name="connectionId">Used to find the connection</param>
        /// <param name="packet">The packet</param>
        private void SetSessionKeyInPacket(int connectionId, Smb2Packet packet)
        {
            Smb2SinglePacket singlePacket = packet as Smb2SinglePacket;

            if (singlePacket != null)
            {
                if ((singlePacket.Header.Flags & Packet_Header_Flags_Values.FLAGS_SIGNED)
                    != Packet_Header_Flags_Values.FLAGS_SIGNED)
                {
                    return;
                }

                singlePacket.SessionKey = globalSessionTable[singlePacket.GetSessionId()].sessionKey;
            }
            else
            {
                //it is smb negotiate packet, do not need verify signature.
            }
        }


        /// <summary>
        /// Find request packet based on the connectionId and the messageId
        /// </summary>
        /// <param name="connectionId">Used to find the connection</param>
        /// <param name="messageId">Used to find the message</param>
        /// <returns>The founded message</returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal Smb2Packet FindRequestPacket(int connectionId, ulong messageId)
        {
            return connectionList[connectionId].requestList[messageId];
        }


        /// <summary>
        /// Handle disconnected event
        /// </summary>
        /// <param name="smb2Event">contain the update information</param>
        private void HandleDisconnectedEvent(Smb2Event smb2Event)
        {
            List<ulong> sessionIds = new List<ulong>();

            foreach (KeyValuePair<ulong, Smb2ServerSession> sessionItem in globalSessionTable)
            {
                if (sessionItem.Value.connection.connectionId == smb2Event.ConnectionId)
                {
                    sessionIds.Add(sessionItem.Key);

                    sessionItem.Value.treeConnectTable.Clear();

                    foreach (KeyValuePair<FILEID, Smb2ServerOpen> openItem in sessionItem.Value.openTable)
                    {
                        if (openItem.Value.isResilient || (openItem.Value.oplockLevel == OplockLevel_Values.OPLOCK_LEVEL_BATCH &&
                            openItem.Value.oplockState == OplockState.Held && openItem.Value.isDurable))
                        {
                            openItem.Value.connection = null;
                            openItem.Value.treeConnect = null;
                            openItem.Value.session = null;
                        }
                        else
                        {
                            globalOpenTable.Remove(openItem.Key.Persistent);
                        }
                    }
                }
            }

            foreach (ulong sessionId in sessionIds)
            {
                globalSessionTable.Remove(sessionId);
            }

            connectionList[smb2Event.ConnectionId].Dispose();
            connectionList.Remove(smb2Event.ConnectionId);
        }

        #endregion

        #region Implement IDispose interface

        /// <summary>
        /// Release all resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release all resources
        /// </summary>
        /// <param name="disposing">Indicate if calling this function manually</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free managed resources & other reference types
                    if (connectionList != null)
                    {
                        foreach (Smb2ServerConnection connection in connectionList.Values)
                        {
                            connection.Dispose();
                        }
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed.

                disposed = true;
            }
        }


        /// <summary>
        /// Deconstructor
        /// </summary>
        ~Smb2ServerContext()
        {
            Dispose(false);
        }

        #endregion
    }
}
