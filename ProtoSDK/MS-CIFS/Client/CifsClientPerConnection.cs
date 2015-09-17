// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the class of CifsClientPerConnection which is used to contain the properties of CIFS ClientPerConnection.
    /// </summary>
    public class CifsClientPerConnection : Connection
    {
        #region fields

        // common ADM:
        private string selectedDialect;
        private bool isSigningActive;
        private byte[] connectionSigningSessionKey;
        private byte[] connectionSigningChallengeResponse;

        // client ADM:
        private bool shareLevelAccessControl;
        private bool serverChallengeResponse;
        private SignStateValue serverSigningState;
        private ulong clientNextSendSequenceNumber;
        private Dictionary<ushort, ulong> clientResponseSequenceNumberList;
        private byte[] ntlmEncryptionKey;
        private Capabilities serverCapabilities;
        private bool negotiateSent;
        private uint maxBufferSize;
        private ushort sessionID;
        private Dictionary<ushort, byte> opLockTable;

        // in negotiate response:
        private SecurityModes securityMode;
        private ushort maxMpxCount;
        private ushort maxNumberVcs;
        private uint maxRawSize;
        private uint sessionKey;
        private ulong systemTime;
        private short serverTimeZone;
        private ulong challenge;
        private byte[] domainName;

        // stack sdk design:
        private string serverNetbiosName;
        private string clientNetbiosName;
        private StackTransportState connectionState;
        private Dictionary<ulong, SmbPacket> pendingResponseList;
        private ushort nextMessageId;

        #endregion


        #region Properties in common ADM

        /// <summary>
        /// A variable that stores the SMB Protocol dialect selected for use on this connection. 
        /// Details of dialects prior to NT LAN Manager ("NT LM 0.12") are described in other documents.
        /// </summary>
        public string SelectedDialect
        {
            get
            {
                return this.selectedDialect;
            }
            set
            {
                this.selectedDialect = value;
            }
        }


        /// <summary>
        /// A BOOLEAN that indicates whether or not message signing is 
        /// active for this SMB connection.
        /// </summary>
        public bool IsSigningActive
        {
            get
            {
                return this.isSigningActive;
            }
            set
            {
                this.isSigningActive = value;
            }
        }


        /// <summary>
        /// A 16-byte array containing the session key that is being used for signing packets,
        /// if signing is active.
        /// </summary>
        public byte[] ConnectionSigningSessionKey
        {
            get
            {
                return this.connectionSigningSessionKey;
            }
            set
            {
                this.connectionSigningSessionKey = value;
            }
        }


        /// <summary>
        /// A variable-length byte array containing the challenge response to use for signing,
        /// if signing is active. If SMB signing is activated on the connection (IsSigningActive
        /// becomes TRUE), the client response to the server challenge from the first non-null,
        /// non-guest session is used for signing all traffic on the SMB connection.
        /// </summary>
        public byte[] ConnectionSigningChallengeResponse
        {
            get
            {
                return this.connectionSigningChallengeResponse;
            }
            set
            {
                this.connectionSigningChallengeResponse = value;
            }
        }

        #endregion


        #region Properties in client ADM

        /// <summary>
        /// A Boolean that determines whether the target server requires share passwords (share
        /// level access control) instead of user accounts (user level access control). Share 
        /// level and user level access control are mutually exclusive. The server MUST support 
        /// one or the other, but not both. Share level and user level access control are mutually
        /// exclusive; the server MUST support one or the other, but not both. The server's access 
        /// control level is indicated in the 0x01 bit of the SecurityMode field in the 
        /// SMB_COM_NEGOTIATE_PROTOCOL response.
        /// </summary>
        public bool ShareLevelAccessControl
        {
            get
            {
                return this.shareLevelAccessControl;
            }
            set
            {
                this.shareLevelAccessControl = value;
            }
        }


        /// <summary>
        /// A Boolean value that indicates whether or not the server supports challenge/response
        /// authentication.
        /// </summary>
        public bool ServerChallengeResponse
        {
            get
            {
                return this.serverChallengeResponse;
            }
            set
            {
                this.serverChallengeResponse = value;
            }
        }


        /// <summary>
        /// A value that indicates the signing policy of the server. 
        /// This value can be Disabled, Enabled, or Required.
        /// </summary>
        public SignStateValue ServerSigningState
        {
            get
            {
                return this.serverSigningState;
            }
            set
            {
                this.serverSigningState = value;
            }
        }


        /// <summary>
        /// A sequence number for the next signed request being sent.
        /// </summary>
        public ulong ClientNextSendSequenceNumber
        {
            get
            {
                return this.clientNextSendSequenceNumber;
            }
            set
            {
                this.clientNextSendSequenceNumber = value;
            }
        }


        /// <summary>
        /// A list of the expected sequence numbers for the responses of outstanding 
        /// signed requests, indexed by message ID (Mid value).
        /// </summary>
        public Dictionary<ushort, ulong> ClientResponseSequenceNumberList
        {
            get
            {
                return this.clientResponseSequenceNumberList;
            }
            set
            {
                this.clientResponseSequenceNumberList = value;
            }
        }


        /// <summary>
        /// A byte array containing the encryption key received from the server 
        /// during a non-extended security negotiate for use in implicit NTLM 
        /// authentication and remembered for authentication.
        /// </summary>
        public byte[] NTLMEncryptionKey
        {
            get
            {
                return this.ntlmEncryptionKey;
            }
            set
            {
                this.ntlmEncryptionKey = value;
            }
        }


        /// <summary>
        ///  The capabilities of the server, as specified in section 2.2.3. 
        ///  The capabilities indirectly reflect the negotiated dialect for this connection.
        /// </summary>
        public Capabilities ServerCapabilities
        {
            get
            {
                return this.serverCapabilities;
            }
            set
            {
                this.serverCapabilities = value;
            }
        }


        /// <summary>
        /// A Boolean that indicates whether a negotiation packet has been sent 
        /// for this connection.
        /// </summary>
        public bool NegotiateSent
        {
            get
            {
                return this.negotiateSent;
            }
            set
            {
                this.negotiateSent = value;
            }
        }


        /// <summary>
        /// The negotiated maximum size, in bytes, for SMB messages sent between 
        /// the client and the server.
        /// </summary>
        public uint MaxBufferSize
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
        /// Only used if the underlying transport is connectionless. This is an SMB Connection
        /// identifier; a server-unique identifier for the connection between the client and 
        /// the server.
        /// </summary>
        public ushort SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = value;
            }
        }


        /// <summary>
        ///  table of FIDs for which OpLocks have been granted. This table is indexed
        ///  by FIDs, and includes the type of OpLock granted to each FID. The client
        ///  will perform client-side caching for each listed FID, as permitted by the 
        ///  type of OpLock granted.
        /// </summary>
        public Dictionary<ushort, byte> OpLockTable
        {
            get
            {
                return this.opLockTable;
            }
            set
            {
                this.opLockTable = value;
            }
        }

        #endregion


        #region Properties in negotiate response

        /// <summary>
        /// indicating the security modes supported or required by the server.
        /// </summary>
        public SecurityModes SecurityMode
        {
            get
            {
                return this.securityMode;
            }
            set
            {
                this.securityMode = value;
            }
        }


        /// <summary>
        /// The maximum number of outstanding SMB operations the server supports.
        /// </summary>
        public ushort MaxMpxCount
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
        /// The maximum number of virtual circuits that MAY be established between the client and the server
        /// as part of the same SMB session.
        /// </summary>
        public ushort MaxNumberVcs
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
        /// The maximum raw buffer size, in bytes, available on the server. This value specifies the maximum 
        /// message size that the client MUST not exceed when sending an SMB_COM_WRITE_RAW client request, and 
        /// the maximum message size that the server MUST not exceed when sending an SMB_COM_READ_RAW response.
        /// </summary>
        public uint MaxRawSize
        {
            get
            {
                return this.maxRawSize;
            }
            set
            {
                this.maxRawSize = value;
            }
        }


        /// <summary>
        /// A unique token identifying the SMB connection. This value is generated by the server for each SMB 
        /// connection. If the client needs to create an additional virtual circuit and attach it to the same 
        /// SMB connection, the client MUST provide the SessionKey in the SMB_COM_SESSION_SETUP_ANDX.
        /// </summary>
        public uint SessionKey
        {
            get
            {
                return this.sessionKey;
            }
            set
            {
                this.sessionKey = value;
            }
        }


        /// <summary>
        /// The number of 100-nanosecond intervals that have elapsed since January 1, 1601,
        /// in Coordinated Universal Time (UTC) format.
        /// </summary>
        public ulong SystemTime
        {
            get
            {
                return this.systemTime;
            }
            set
            {
                this.systemTime = value;
            }
        }


        /// <summary>
        /// represents the server's time zone, in minutes, from UTC. The time zone of the server MUST be expressed 
        /// in minutes, plus or minus, from UTC.
        /// </summary>
        public short ServerTimeZone
        {
            get
            {
                return this.serverTimeZone;
            }
            set
            {
                this.serverTimeZone = value;
            }
        }


        /// <summary>
        /// An array of unsigned bytes that MUST be ChallengeLength bytes long and MUST represent the server 
        /// challenge. This array MUST NOT be null-terminated..
        /// </summary>
        public ulong Challenge
        {
            get
            {
                return this.challenge;
            }
            set
            {
                this.challenge = value;
            }
        }


        /// <summary>
        /// The name of the NT domain or workgroup to which the server belongs.
        /// </summary>
        public byte[] DomainName
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

        #endregion


        #region Properties in stack sdk design


        /// <summary>
        /// A table of authenticated sessions that have been established on this transport connection. 
        /// The table MUST be uniquely indexed by Session.SessionId, and MUST support enumeration of 
        /// every entry in the table.
        /// </summary>
        protected internal Collection<Session> SessionTable
        {
            get
            {
                return this.sessionTable;
            }
            set
            {
                this.sessionTable = value;
            }
        }


        /// <summary>
        /// the smb server netbios name
        /// </summary>
        public string ServerNetbiosName
        {
            get
            {
                return this.serverNetbiosName;
            }
            set
            {
                this.serverNetbiosName = value;
            }
        }


        /// <summary>
        /// the local netbios name
        /// </summary>
        public string ClientNetbiosName
        {
            get
            {
                return this.clientNetbiosName;
            }
            set
            {
                this.clientNetbiosName = value;
            }
        }


        /// <summary>
        /// the current connection state.
        /// </summary>
        public StackTransportState ConnectionState
        {
            get
            {
                return this.connectionState;
            }
            set
            {
                this.connectionState = value;
            }
        }


        /// <summary>
        /// get the min MessageId in sequence window. If the secquence has been consumed out, 
        /// the return is the max consumed messageId added 1.
        /// </summary>
        public ushort MessageId
        {
            get
            {
                return this.nextMessageId;
            }
        }

        #endregion


        #region Constructor


        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsClientPerConnection()
            : base()
        {
            // common ADM:
            this.SelectedDialect = string.Empty;
            this.IsSigningActive = false;
            this.ConnectionSigningSessionKey = new byte[0];
            this.ConnectionSigningChallengeResponse = new byte[0];

            // client ADM:
            this.ShareLevelAccessControl = true;
            this.ServerChallengeResponse = true;
            this.ServerSigningState = SignStateValue.DISABLED;
            this.ClientNextSendSequenceNumber = 2;
            this.ClientResponseSequenceNumberList = new Dictionary<ushort, ulong>();
            //The initial response sequence number.
            this.ClientResponseSequenceNumberList.Add(2, 3);
            this.NTLMEncryptionKey = new byte[0];
            this.ServerCapabilities = 0;
            this.NegotiateSent = false;
            this.MaxBufferSize = 0;
            this.SessionID = 0;
            this.OpLockTable = new Dictionary<ushort, byte>();

            // negotiate response:
            this.SecurityMode = SecurityModes.NONE;
            this.MaxMpxCount = 0;
            this.MaxNumberVcs = 0;
            this.MaxRawSize = 0;
            this.SessionKey = 0;
            this.SystemTime = 0;
            this.ServerTimeZone = 0;
            this.Challenge = 0;
            this.DomainName = new byte[0];

            // stack sdk design:
            this.ServerNetbiosName = string.Empty;
            this.ClientNetbiosName = string.Empty;
            this.ConnectionState = StackTransportState.Init;
            this.pendingResponseList = new Dictionary<ulong, SmbPacket>();
            this.SessionTable = new Collection<Session>();
            this.nextMessageId = 1;
        }


        /// <summary>
        /// Deep copy constructor.
        /// if need to copy the connection instance, you must call the Clone method.
        /// its sub class inherit from this, and need to provide more features.
        /// </summary>
        public CifsClientPerConnection(CifsClientPerConnection connection)
            : base(connection)
        {
            lock (connection)
            {
                // common ADM:
                this.SelectedDialect = connection.SelectedDialect;
                this.IsSigningActive = connection.IsSigningActive;
                if (connection.ConnectionSigningSessionKey != null)
                {
                    this.ConnectionSigningSessionKey = new byte[connection.ConnectionSigningSessionKey.Length];
                    Array.Copy(connection.ConnectionSigningSessionKey, this.ConnectionSigningSessionKey,
                        connection.ConnectionSigningSessionKey.Length);
                }
                if (connection.ConnectionSigningChallengeResponse != null)
                {
                    this.ConnectionSigningChallengeResponse = new byte[
                        connection.ConnectionSigningChallengeResponse.Length];
                    Array.Copy(connection.ConnectionSigningChallengeResponse, this.ConnectionSigningChallengeResponse,
                        connection.ConnectionSigningChallengeResponse.Length);
                }

                // client ADM:
                this.ShareLevelAccessControl = connection.ShareLevelAccessControl;
                this.ServerChallengeResponse = connection.ServerChallengeResponse;
                this.ServerSigningState = connection.ServerSigningState;
                this.ClientNextSendSequenceNumber = connection.ClientNextSendSequenceNumber;
                if (connection.ClientResponseSequenceNumberList != null)
                {
                    this.ClientResponseSequenceNumberList = new Dictionary<ushort, ulong>();
                    foreach (KeyValuePair<ushort, ulong> obj in connection.ClientResponseSequenceNumberList)
                    {
                        this.ClientResponseSequenceNumberList.Add(obj.Key, obj.Value);
                    }
                }
                if (connection.NTLMEncryptionKey != null)
                {
                    this.NTLMEncryptionKey = new byte[connection.NTLMEncryptionKey.Length];
                    Array.Copy(connection.NTLMEncryptionKey, this.NTLMEncryptionKey,
                        connection.NTLMEncryptionKey.Length);
                }
                this.ServerCapabilities = connection.ServerCapabilities;
                this.NegotiateSent = connection.NegotiateSent;
                this.MaxBufferSize = connection.MaxBufferSize;
                this.SessionID = connection.SessionID;
                if (connection.OpLockTable != null)
                {
                    this.OpLockTable = new Dictionary<ushort, byte>();
                    foreach (KeyValuePair<ushort, byte> obj in connection.OpLockTable)
                    {
                        this.OpLockTable.Add(obj.Key, obj.Value);
                    }
                }

                // negotiate response:
                this.SecurityMode = connection.SecurityMode;
                this.MaxMpxCount = connection.MaxMpxCount;
                this.MaxNumberVcs = connection.MaxNumberVcs;
                this.MaxRawSize = connection.MaxRawSize;
                this.SessionKey = connection.SessionKey;
                this.SystemTime = connection.SystemTime;
                this.ServerTimeZone = connection.ServerTimeZone;
                this.Challenge = connection.Challenge;
                if (connection.DomainName != null)
                {
                    this.DomainName = new byte[connection.DomainName.Length];
                    Array.Copy(connection.DomainName, this.DomainName, connection.DomainName.Length);
                }

                // stack sdk design:
                this.ServerNetbiosName = string.Empty;
                this.ClientNetbiosName = string.Empty;
                this.ConnectionState = StackTransportState.Init;
                this.pendingResponseList = new Dictionary<ulong, SmbPacket>();
                this.SessionTable = new Collection<Session>();
                this.nextMessageId = 1;

                // stack sdk design:
                this.ServerNetbiosName = connection.ServerNetbiosName;
                this.ClientNetbiosName = connection.ClientNetbiosName;
                this.ConnectionState = connection.ConnectionState;
                if (connection.pendingResponseList != null)
                {
                    this.pendingResponseList = new Dictionary<ulong, SmbPacket>();
                    foreach (KeyValuePair<ulong, SmbPacket> pair in connection.pendingResponseList)
                    {
                        this.pendingResponseList.Add(pair.Key, pair.Value.Clone() as SmbPacket);
                    }
                }
                if (connection.sessionTable != null)
                {
                    this.sessionTable = new Collection<Session>();
                    foreach (CifsClientPerSession session in connection.sessionTable)
                    {
                        this.sessionTable.Add(session.Clone());
                    }
                }
                this.nextMessageId = connection.nextMessageId;
            }
        }


        /// <summary>
        /// clone this instance.
        /// using for the context to copy the instances.
        /// if need to inherit from this class, this method must be overridden.
        /// </summary>
        /// <returns>a copy of this instance</returns>
        protected internal virtual CifsClientPerConnection Clone()
        {
            return new CifsClientPerConnection(this);
        }


        #endregion


        #region access outstanding request list


        /// <summary>
        /// add a request into the table of OutstandingRequests.
        /// </summary>
        /// <param name="packet">the Outstanding Request.</param>
        /// <returns>false:the packet is not REQUEST or it is Cancel packet or the MessageId already exists in the table.
        /// otherwise true.</returns>
        internal bool AddOutstandingRequest(SmbPacket packet)
        {
            ulong mid = (ulong)(packet.SmbHeader.Mid);

            lock (this.RequestList)
            {
                if (this.RequestList.ContainsKey(mid))
                {
                    return false;
                }
                else
                {
                    this.RequestList.Add(mid, packet);
                    return true;
                }
            }
        }


        /// <summary>
        /// get all requests from the table of OutstandingRequests.
        /// </summary>
        /// <returns>all OutstandingRequests in this connection will be returned.</returns>
        internal Collection<SmbPacket> GetOutstandingRequests()
        {
            Collection<SmbPacket> ret = new Collection<SmbPacket>();

            lock (this.RequestList)
            {
                foreach (SmbPacket request in this.RequestList.Values)
                {
                    ret.Add(request.Clone() as SmbPacket);
                }
            }

            return ret;
        }


        /// <summary>
        /// get a request from the table of OutstandingRequests.
        /// </summary>
        /// <param name="mid">the messageId of the request to get.</param>
        /// <returns>if found, a copy of the outstanding request will be returned. 
        /// otherwise, null will be returned.</returns>
        internal SmbPacket GetOutstandingRequest(ulong mid)
        {
            SmbPacket packet = null;

            lock (this.RequestList)
            {
                if (this.RequestList.ContainsKey(mid))
                {
                    packet = this.RequestList[mid].Clone() as SmbPacket;
                }
            }

            return packet;
        }


        /// <summary>
        /// remove a request from the table of OutstandingRequests.
        /// </summary>
        /// <param name="mid">the messageId of the request to be removed.</param>
        internal void RemoveOutstandingRequest(ulong mid)
        {
            lock (this.RequestList)
            {
                this.RequestList.Remove(mid);
            }
        }

        #endregion


        #region update the sequence window

        /// <summary>
        /// update the sequence window.
        /// </summary>
        internal void UpdateSequenceWindow()
        {
            this.nextMessageId++;
        }

        #endregion


        #region access the sequence Number

        /// <summary>
        /// update the sequence number.
        /// </summary>
        /// <param name="msg">the last request packet.</param>
        /// <exception cref="System.InvalidOperationException">the msg should be request.</exception>
        public void UpdateSequenceNumber(SmbPacket msg)
        {
            // only request can be used to UpdateSequenceNumber:
            if (msg.PacketType != SmbPacketType.SingleRequest
                && msg.PacketType != SmbPacketType.BatchedRequest)
            {
                throw new InvalidOperationException("the packet used to UpdateSequenceNumber should be request.");
            }

            // if the message is not signed, return.
            if (!msg.IsSignRequired)
            {
                return;
            }

            // update client sequence number
            if (msg.SmbHeader.Command == SmbCommand.SMB_COM_NT_CANCEL)
            {
                this.ClientNextSendSequenceNumber++;
            }
            else
            {
                this.clientResponseSequenceNumberList[msg.SmbHeader.Mid] = this.ClientNextSendSequenceNumber + 1;
                this.ClientNextSendSequenceNumber += 2;
            }
        }


        /// <summary>
        /// remove the sequence number.
        /// </summary>
        /// <param name="msg">the last response packet.</param>
        /// <exception cref="System.InvalidOperationException">the msg should be response.</exception>
        public void RemoveSequenceNumber(SmbPacket msg)
        {
            // only response can be used to RemoveSequenceNumber:
            if (msg.PacketType != SmbPacketType.SingleResponse
                && msg.PacketType != SmbPacketType.BatchedResponse)
            {
                throw new InvalidOperationException("the packet used to RemoveSequenceNumber should be response.");
            }

            if (this.ClientResponseSequenceNumberList.ContainsKey(msg.SmbHeader.Mid))
            {
                this.ClientResponseSequenceNumberList.Remove(msg.SmbHeader.Mid);
            }
        }

        #endregion
    }
}