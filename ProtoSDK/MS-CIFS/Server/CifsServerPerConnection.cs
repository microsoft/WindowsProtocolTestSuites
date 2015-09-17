// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the class of CifsServerPerConnection which is used to contain the properties of CIFS ClientPerConnection.
    /// </summary>
    public class CifsServerPerConnection : IFileServiceServerConnection
    {
        #region Fields
        private Dictionary<ushort, IFileServiceServerSession> sessionTable;
        private Dictionary<ushort, SmbFamilyPacket> pendingRequestTable;
        private Capabilities clientCapabilites;
        private object identity;
        private bool isSigningActive;
        private string clientName;

        private uint serverNextReceiveSequenceNumber;
        private Dictionary<ushort, ulong> serverSendSequenceNumbers;
        private string selectedDialect;
        private byte[] signingSessionKey;
        private byte[] signingChallengeResponse;

        private string nativeLanMan;
        private string nativeOS;
        private bool opLockSupport;
        private bool sessionSetupReceived;
        private uint clientMaxBufferSize;

        private byte[] ntlmChallenge;
        private ushort clientMaxMpxCount;

        private ushort nextSessionId;
        private ushort nextTreeId;
        private ushort nextFileId;
        private ushort multiplexId;

        private FileTime negotiateTime;

        #endregion

        #region Properties of TD

        /// <summary>
        /// The Capabilities flags of the client
        /// </summary>
        public Capabilities ClientCapabilites
        {
            get
            {
                return this.clientCapabilites;
            }
            internal set
            {
                this.clientCapabilites = value;
            }
        }


        /// <summary>
        /// The identification to the connection.
        /// </summary>
        public object Identity
        {
            get
            {
                return this.identity;
            }
            set
            {
                this.identity = value;
            }
        }


        /// <summary>
        /// A Boolean that indicates whether or not message signing is active for this SMB connection.
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
        /// A client identifier. For NetBIOS-based transports, this is the NetBIOS name of the client. For other 
        /// transports, this is a transport-specific identifier that provides a unique name or address for the client.
        /// </summary>
        public string ClientName
        {
            get
            {
                return this.clientName;
            }
            set
            {
                this.clientName = value;
            }
        }


        /// <summary>
        /// all the sessions opened in this connection.
        /// </summary>
        public ReadOnlyCollection<IFileServiceServerSession> SessionTable
        {
            get
            {
                lock (this.sessionTable)
                {
                    return new ReadOnlyCollection<IFileServiceServerSession>(
                        new List<IFileServiceServerSession>(this.sessionTable.Values));
                }
            }
        }


        /// <summary>
        /// A list of request being processed by the server.
        /// </summary>
        public ReadOnlyCollection<SmbFamilyPacket> PendingRequestTable
        {
            get
            {
                lock (this.pendingRequestTable)
                {
                    return new ReadOnlyCollection<SmbFamilyPacket>(
                        new List<SmbFamilyPacket>(this.pendingRequestTable.Values));
                }
            }
        }


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
        /// A 16-byte array containing the session key that is being used for signing packets,
        /// if signing is active.
        /// </summary>
        public byte[] SigningSessionKey
        {
            get
            {
                return this.signingSessionKey;
            }
            set
            {
                this.signingSessionKey = value;
            }
        }


        /// <summary>
        /// A variable-length byte array containing the challenge response to use for signing,
        /// if signing is active. If SMB signing is activated on the connection (IsSigningActive
        /// becomes TRUE), the client response to the server challenge from the first non-null,
        /// non-guest session is used for signing all traffic on the SMB connection.
        /// </summary>
        public byte[] SigningChallengeResponse
        {
            get
            {
                return this.signingChallengeResponse;
            }
            set
            {
                this.signingChallengeResponse = value;
            }
        }


        /// <summary>
        /// A byte array containing the cryptographic challenge sent to the client during protocol negotiation.
        /// The challenge is sent in the SMB_COM_NEGOTIATE response.
        /// </summary>
        public byte[] NTLMChallenge
        {
            get
            {
                return this.ntlmChallenge;
            }
            set
            {
                this.ntlmChallenge = value;
            }
        }


        /// <summary>
        /// A string that represents the native LAN manager type of the client, as reported by the client.
        /// </summary>
        public string NativeLanMan
        {
            get
            {
                return this.nativeLanMan;
            }
            set
            {
                this.nativeLanMan = value;
            }
        }


        /// <summary>
        /// A string that represents the native operating system of the CIFS client, as reported by the client.
        /// </summary>
        public string NativeOS
        {
            get
            {
                return this.nativeOS;
            }
            set
            {
                this.nativeOS = value;
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
        /// A sequence number for the next signed request being sent.
        /// </summary>
        public uint ServerNextReceiveSequenceNumber
        {
            get
            {
                return this.serverNextReceiveSequenceNumber;
            }
            set
            {
                this.serverNextReceiveSequenceNumber = value;
            }
        }


        /// <summary>
        /// A list of the expected sequence numbers for the responses of outstanding signed requests, indexed by
        /// multiplex Id.
        /// </summary>
        public ReadOnlyDictionary<ushort, ulong> ServerSendSequenceNumbers
        {
            get
            {
                lock (this.serverSendSequenceNumbers)
                {
                    return new ReadOnlyDictionary<ushort, ulong>(this.serverSendSequenceNumbers);
                }
            }
        }


        /// <summary>
        /// A Boolean that indicates whether a negotiation packet has been sent 
        /// for this connection.
        /// </summary>
        public bool SessionSetupReceived
        {
            get
            {
                return this.sessionSetupReceived;
            }
            set
            {
                this.sessionSetupReceived = value;
            }
        }


        /// <summary>
        /// The negotiated maximum size, in bytes, for SMB messages sent between 
        /// the client and the server.
        /// </summary>
        public uint ClientMaxBufferSize
        {
            get
            {
                return this.clientMaxBufferSize;
            }
            set
            {
                this.clientMaxBufferSize = value;
            }
        }


        /// <summary>
        /// The multiplex id.
        /// </summary>
        public ushort MultiplexId
        {
            get
            {
                return this.multiplexId;
            }
            set
            {
                this.multiplexId = value;
            }
        }


        /// <summary>
        /// SystemTime filed in negotiate response. Used for NTLM V2 authentication.
        /// </summary>
        public FileTime NegotiateTime
        {
            get
            {
                return this.negotiateTime;
            }
            set
            {
                this.negotiateTime = value;
            }
        }

        #endregion

        #region Properties in negotiate request

        /// <summary>
        /// The maximum number of outstanding SMB operations the server supports.
        /// </summary>
        public ushort ClientMaxMpxCount
        {
            get
            {
                return this.clientMaxMpxCount;
            }
            set
            {
                this.clientMaxMpxCount = value;
            }
        }
        #endregion

        #region Access the sequence Number

        /// <summary>
        /// update the sequence number.
        /// </summary>
        /// <param name="request">the last request packet.</param>
        /// <exception cref="System.InvalidOperationException">the msg should be request.</exception>
        public void UpdateSequenceNumber(SmbPacket request)
        {
            // only request can be used to UpdateSequenceNumber:
            if (request.PacketType != SmbPacketType.SingleRequest
                && request.PacketType != SmbPacketType.BatchedRequest)
            {
                throw new InvalidOperationException("the packet used to UpdateSequenceNumber should be request.");
            }

            // if the message is not signed, return.
            if (!request.IsSignRequired)
            {
                return;
            }

            if (request.SmbHeader.Command == SmbCommand.SMB_COM_NT_CANCEL)
            {
                this.serverNextReceiveSequenceNumber++;
            }
            else
            {
                this.serverSendSequenceNumbers[request.SmbHeader.Mid] = this.serverNextReceiveSequenceNumber + 1;
                this.serverNextReceiveSequenceNumber += 2;
            }
        }


        /// <summary>
        /// remove the sequence number.
        /// </summary>
        /// <param name="response">the last response packet.</param>
        /// <exception cref="System.InvalidOperationException">the msg should be response.</exception>
        public void RemoveSequenceNumber(SmbPacket response)
        {
            this.serverSendSequenceNumbers.Remove(response.SmbHeader.Mid);
        }

        #endregion

        #region Generate UID, TID, FID unique per connection

        /// <summary>
        /// A UID(UserId) represents an authenticated SMB session.
        /// </summary>
        /// <returns>The UserId</returns>
        public ushort GenerateUID()
        {
            return ++this.nextSessionId;
        }


        /// <summary>
        /// A file handle represents an open file on the server.
        /// </summary>
        /// <returns>The FID</returns>
        public ushort GenerateFID()
        {
            return ++this.nextFileId;
        }


        /// <summary>
        /// A search handle represents an open file on the server.
        /// </summary>
        /// <returns>The SID</returns>
        public ushort GenerateSID()
        {
            return ++this.nextFileId;
        }


        /// <summary>
        /// A TID represents an open connection to a share, otherwise known as a tree connect.
        /// </summary>
        /// <returns>The TID</returns>
        public ushort GenerateTID()
        {
            return ++this.nextTreeId;
        }

        #endregion

        #region Access SessionTable Methods

        /// <summary>
        /// Add a session into this SessionTable.
        /// </summary>
        /// <param name="session">The session to add.</param>
        /// <exception cref="System.ArgumentNullException">the session is null</exception>
        /// <exception cref="System.ArgumentException">the session already exists</exception>
        public void AddSession(IFileServiceServerSession session)
        {
            lock (this.sessionTable)
            {
                this.sessionTable.Add((ushort)session.SessionId, session);
            }
        }


        /// <summary>
        /// Remove a session from this SessionTable.
        /// </summary>
        /// <param name="sessionId">The session Id.</param>
        public void RemoveSession(ushort sessionId)
        {
            lock (this.sessionTable)
            {
                this.sessionTable.Remove(sessionId);
            }
        }


        /// <summary>
        /// Get the session from this SessionTables.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public CifsServerPerSession GetSession(ushort sessionId)
        {
            lock (this.sessionTable)
            {
                if (this.sessionTable.ContainsKey(sessionId))
                {
                    return this.sessionTable[sessionId] as CifsServerPerSession;
                }
                return null;
            }
        }

        #endregion

        #region Access PendingRequestTable Methods

        /// <summary>
        /// Add a request into this PendingRequestTable.
        /// </summary>
        /// <param name="request">The request to add.</param>
        /// <exception cref="System.ArgumentNullException">the request is null</exception>
        /// <exception cref="System.ArgumentException">the request already exists</exception>
        public void AddPendingRequest(SmbPacket request)
        {
            lock (this.pendingRequestTable)
            {
                this.pendingRequestTable.Add(request.SmbHeader.Mid, request);
            }
        }


        /// <summary>
        /// Remove a pending request from this PendingRequestTable.
        /// </summary>
        /// <param name="mid">The multiplex Id of the request to remove.</param>
        public void RemovePendingRequest(ushort mid)
        {
            lock (this.pendingRequestTable)
            {
                this.pendingRequestTable.Remove(mid);
            }
        }


        /// <summary>
        /// Query a pending request from this PendingRequestTable.
        /// </summary>
        /// <param name="mid">The multiplex Id of the request to query.</param>
        /// <returns>Return the request packet per mid. If not exist, return null.</returns>
        public SmbPacket GetPendingRequest(ushort mid)
        {
            lock (this.pendingRequestTable)
            {
                if (this.pendingRequestTable.ContainsKey(mid))
                {
                    return this.pendingRequestTable[mid] as SmbPacket;
                }
            }
            return null;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsServerPerConnection()
        {
            this.signingSessionKey = new byte[0];
            this.signingChallengeResponse = new byte[0];
            this.ntlmChallenge = new byte[8];
            new Random().NextBytes(this.ntlmChallenge);
            this.sessionTable = new Dictionary<ushort, IFileServiceServerSession>();
            this.pendingRequestTable = new Dictionary<ushort,SmbFamilyPacket>();
            this.serverNextReceiveSequenceNumber = 2;
            this.serverSendSequenceNumbers = new Dictionary<ushort, ulong>();
            this.negotiateTime = new FileTime();
        }

        #endregion
    }
}