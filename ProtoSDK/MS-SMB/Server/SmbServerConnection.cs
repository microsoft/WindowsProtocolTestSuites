// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// describes the connection of smb server.
    /// </summary>
    public class SmbServerConnection : IFileServiceServerConnection
    {
        #region Fields of TD

        /// <summary>
        /// Capability flags of the client, as specified in section 2.2.3.
        /// </summary>
        private Capabilities clientCapabilities;

        /// <summary>
        /// Sequence number for the next signed request being received.
        /// </summary>
        private uint serverNextReceiveSequenceNumber;

        /// <summary>
        /// The encryption key received, sent by the server during a nonextended security negotiation for use in 
        /// implicit NTLM authentication.
        /// </summary>
        private byte[] ntlmEncryptionKey;

        /// <summary>
        /// A table of Sessions established on this connection. The table is searchable based on Session.Uid or Session
        /// .SecurityContext.
        /// </summary>
        private Collection<SmbServerSession> sessionTable;

        /// <summary>
        /// A null-terminated Unicode UTF-16 IP address string, or NetBIOS host name of the client machine.
        /// </summary>
        private string clientName;

        /// <summary>
        /// key is the mid, value is the smb packet.
        /// A list of outstanding SMB requests on this connection. Each element in the list is a tuple of the form
        /// [Request, Mid, Pid], where the Request describes an outstanding SMB request as described in section 
        /// 3.3.1.11, the Mid and the Pid refer to the corresponding values obtained from the SMB header of the message
        /// sent by the client.
        /// </summary>
        private Dictionary<ushort, SmbPacket> requestList;

        #endregion

        #region Fields of SmbSdk

        /// <summary>
        /// gssApi 
        /// </summary>
        private ServerSecurityContext gssApi;

        /// <summary>
        /// the is signning active flag
        /// </summary>
        private bool isSigningActive;

        /// <summary>
        /// the send sequence number list
        /// </summary>
        private Dictionary<ServerSendSequenceNumberKey, uint> serverSendSequenceNumber;

        /// <summary>
        /// Capability flags of the server
        /// </summary>
        private Capabilities serverCapabilities;

        /// <summary>
        /// the dialects from client negotiate
        /// </summary>
        private string[] negotiatedDialects;

        /// <summary>
        /// the identity of connection. the down layer transport use this to identify a connection.
        /// </summary>
        private object identity;

        /// <summary>
        /// the capabilities of smbserver. 
        /// </summary>
        private SmbServerCapability capability;

        /// <summary>
        /// the message id.
        /// </summary>
        private ushort messageId;

        /// <summary>
        /// the process id.
        /// </summary>
        private uint processId;

        /// <summary>
        /// This field MUST specify the maximum rights the guest account has on this share based on the security 
        /// enforced by the share 
        /// </summary>
        private uint maximalShareAccessRights;

        /// <summary>
        /// This field MUST specify the maximum rights the guest account has on this share based on the security 
        /// enforced by the share 
        /// </summary>
        private uint guestMaximalShareAccessRights;

        /// <summary>
        /// The following new OptionalSupport field flags are the new extensions added to the CIFS protocol 
        /// </summary>
        private ushort optionalSupport;

        /// <summary>
        /// The time of server system. Must response with the NtlmEncryptionKey when implicit ntlm session setup.
        /// This field is set by nlmp sdk, the output field of NTLM-challenge packet.
        /// </summary>
        private ulong systemTime;

        #endregion

        #region Properties of TD

        /// <summary>
        /// A list of outstanding SMB requests on this connection. Each element in the list is a tuple of the form
        /// [Request, Mid, Pid], where the Request describes an outstanding SMB request as described in section 
        /// 3.3.1.11, the Mid and the Pid refer to the corresponding values obtained from the SMB header of the message
        /// sent by the client.
        /// </summary>
        public ReadOnlyCollection<SmbPacket> RequestList
        {
            get
            {
                return null;
            }
        }


        /// <summary>
        /// A null-terminated Unicode UTF-16 IP address string, or NetBIOS host name of the client machine.
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
        /// A table of Sessions established on this connection. The table is searchable based on Session.Uid or Session
        /// .SecurityContext.
        /// </summary>
        public ReadOnlyCollection<SmbServerSession> SessionList
        {
            get
            {
                return new ReadOnlyCollection<SmbServerSession>(this.sessionTable);
            }
        }


        /// <summary>
        /// A table of Sessions established on this connection.
        /// </summary>
        public ReadOnlyCollection<IFileServiceServerSession> SessionTable
        {
            get
            {
                Collection<IFileServiceServerSession> ret = new Collection<IFileServiceServerSession>();
                foreach (SmbServerSession session in this.sessionTable)
                {
                    ret.Add(session);
                }
                return new ReadOnlyCollection<IFileServiceServerSession>(ret);
            }
        }


        /// <summary>
        /// A list of request being processed by the server.
        /// </summary>
        public ReadOnlyCollection<SmbFamilyPacket> PendingRequestTable
        {
            get
            {
                Collection<SmbFamilyPacket> ret = new Collection<SmbFamilyPacket>();
                foreach (SmbFamilyPacket request in this.requestList.Values)
                {
                    ret.Add(request);
                }
                return new ReadOnlyCollection<SmbFamilyPacket>(ret);
            }
        }


        /// <summary>
        /// A table of Opens in this connection.
        /// </summary>
        public ReadOnlyCollection<SmbServerOpen> OpenList
        {
            get
            {
                Collection<SmbServerOpen> opens = new Collection<SmbServerOpen>();

                foreach (SmbServerSession session in this.SessionList)
                {
                    foreach (SmbServerTreeConnect treeconnect in session.TreeConnectList)
                    {
                        foreach (SmbServerOpen open in treeconnect.OpenList)
                        {
                            opens.Add(open);
                        }
                    }
                }

                return new ReadOnlyCollection<SmbServerOpen>(opens);
            }
        }


        /// <summary>
        /// The encryption key received, sent by the server during a nonextended security negotiation for use in 
        /// implicit NTLM authentication. the 8 bytes challenge. when authenticate is implicit NTLM, server pass 
        /// this to client as the EncryptionKey field of negotiate response. server stores this in the challenge field
        /// of negotiate response packet.
        /// </summary>
        public byte[] NtlmEncryptionKey
        {
            get
            {
                return ntlmEncryptionKey;
            }
        }


        /// <summary>
        /// Sequence number for the next signed request being received
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
        /// Capability flags of the client
        /// </summary>
        public Capabilities ClientCapabilities
        {
            get
            {
                return this.clientCapabilities;
            }
            set
            {
                this.clientCapabilities = value;
            }
        }


        #endregion

        #region Properties of SmbSdk

        /// <summary>
        /// GssApi 
        /// </summary>
        public ServerSecurityContext GssApi
        {
            get
            {
                return this.gssApi;
            }
            set
            {
                this.gssApi = value;
            }
        }


        /// <summary>
        /// The time of server system. Must response with the NtlmEncryptionKey when implicit ntlm session setup.
        /// This field is set by nlmp sdk, the output field of NTLM-challenge packet.
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
        /// The following new OptionalSupport field flags are the new extensions added to the CIFS protocol 
        /// </summary>
        public ushort OptionalSupport
        {
            get
            {
                return this.optionalSupport;
            }
            set
            {
                this.optionalSupport = value;
            }
        }


        /// <summary>
        /// This field MUST specify the maximum rights the guest account has on this share based on the security 
        /// enforced by the share 
        /// </summary>
        public uint GuestMaximalShareAccessRights
        {
            get
            {
                return this.guestMaximalShareAccessRights;
            }
            set
            {
                this.guestMaximalShareAccessRights = value;
            }
        }


        /// <summary>
        /// This field MUST specify the maximum rights the guest account has on this share based on the security 
        /// enforced by the share 
        /// </summary>
        public uint MaximalShareAccessRights
        {
            get
            {
                return this.maximalShareAccessRights;
            }
            set
            {
                this.maximalShareAccessRights = value;
            }
        }


        /// <summary>
        /// the message id.
        /// </summary>
        public ushort MessageId
        {
            get
            {
                return this.messageId;
            }
            set
            {
                this.messageId = value;
            }
        }


        /// <summary>
        /// the process id.
        /// </summary>
        public uint ProcessId
        {
            get
            {
                return this.processId;
            }
            set
            {
                this.processId = value;
            }
        }


        /// <summary>
        /// the capabilities of smbserver. 
        /// </summary>
        public SmbServerCapability Capability
        {
            get
            {
                return this.capability;
            }
        }


        /// <summary>
        /// indicates whether or not message signing is active for this SMB connection
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
        /// the dialects from client negotiate
        /// </summary>
        public string[] NegotiatedDialects
        {
            get
            {
                return this.negotiatedDialects;
            }
            set
            {
                this.negotiatedDialects = value;
            }
        }


        /// <summary>
        /// indicates whether or not message signing is active for this SMB connection
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
        /// A list of the expected sequence numbers for the responses of outstanding signed requests,
        /// indexed by PID/MID pair.
        /// </summary>
        public Dictionary<ServerSendSequenceNumberKey, uint> ServerSendSequenceNumber
        {
            get
            {
                return this.serverSendSequenceNumber;
            }
            set
            {
                this.serverSendSequenceNumber = value;
            }
        }


        /// <summary>
        ///  A set of server capabilities. 
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


        #endregion

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        internal SmbServerConnection()
        {
            this.requestList = new Dictionary<ushort, SmbPacket>();
            this.sessionTable = new Collection<SmbServerSession>();
            this.serverSendSequenceNumber = new Dictionary<ServerSendSequenceNumberKey, uint>();

            // on initialize, ServerNextReceiveSequenceNumber is set to the following value.
            this.serverNextReceiveSequenceNumber = 2;

            // Windows clients and servers expect 8-byte encryption keys.
            this.ntlmEncryptionKey = new byte[8];

            this.isSigningActive = true;
            this.clientCapabilities = (Capabilities)0;

            this.serverCapabilities = Capabilities.CAP_EXTENDED_SECURITY | Capabilities.CAP_LWIO 
                | Capabilities.CAP_LARGE_READX | Capabilities.CAP_LARGE_WRITE | Capabilities.CAP_INFOLEVEL_PASSTHRU
                | Capabilities.CAP_NT_FIND | Capabilities.CAP_LOCK_AND_READ | Capabilities.CAP_LEVEL_II_OPLOCKS
                | Capabilities.CAP_STATUS32 | Capabilities.CAP_RPC_REMOTE_APIS | Capabilities.CAP_NT_SMBS
                | Capabilities.CAP_LARGE_FILES | Capabilities.CAP_UNICODE;

            this.capability = new SmbServerCapability();
        }


        #endregion

        #region Access Sessions

        /// <summary>
        /// get a session from connection
        /// </summary>
        /// <param name="uid">the uid of session</param>
        /// <returns>the session</returns>
        internal SmbServerSession GetSession(ushort uid)
        {
            lock (this.sessionTable)
            {
                foreach (SmbServerSession session in this.sessionTable)
                {
                    if (session.Uid == uid)
                    {
                        return session;
                    }
                }

                return null;
            }
        }


        /// <summary>
        /// add a session to the session table of connection
        /// </summary>
        /// <param name="session">the session to add</param>
        /// <exception cref="InvalidOperationException">
        /// the session has exist in the connection, can not add it!
        /// </exception>
        internal void AddSession(SmbServerSession session)
        {
            lock (this.sessionTable)
            {
                if (this.sessionTable.Contains(session))
                {
                    throw new InvalidOperationException("the session has exist in the connection, can not add it!");
                }

                this.sessionTable.Add(session);
            }
        }


        /// <summary>
        /// remove the session. if session does not exists, do nothing.
        /// </summary>
        /// <param name="session">the session to remove</param>
        internal void RemoveSession(SmbServerSession session)
        {
            lock (this.sessionTable)
            {
                if (this.sessionTable.Contains(session))
                {
                    this.sessionTable.Remove(session);
                }
            }
        }


        #endregion

        #region Access Request Packet List

        /// <summary>
        /// remove the request packet according to the response packet
        /// </summary>
        /// <param name="response">the response packet</param>
        internal void RemoveRequestPacket(SmbPacket response)
        {
            lock (this.requestList)
            {
                if (this.requestList.ContainsKey(response.SmbHeader.Mid))
                {
                    this.requestList.Remove(response.SmbHeader.Mid);
                }
            }
        }


        /// <summary>
        /// add request packet to the request list
        /// </summary>
        /// <param name="request">the request packet</param>
        internal void AddRequestPacket(SmbPacket request)
        {
            lock (this.requestList)
            {
                this.requestList[request.SmbHeader.Mid] = request;
            }
        }


        /// <summary>
        /// get the request packet according the response packet
        /// </summary>
        /// <param name="mid">the request packet mid</param>
        /// <returns>the request packet</returns>
        internal SmbPacket GetRequestPacket(ushort mid)
        {
            lock (this.requestList)
            {
                if (this.requestList.ContainsKey(mid))
                {
                    return this.requestList[mid];
                }

                return null;
            }
        }


        #endregion

        #region Common Methods

        /// <summary>
        /// get the Netbios SesionId.<para/>
        /// if the transport is not Netbios, sdk will throw exception.
        /// </summary>
        /// <exception cref="InvalidCastException">
        /// the transport is not Netbios, you must not get the netbios information of client
        /// </exception>
        /// <returns>the session id of Netbios</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public int GetClientNetbiosSessionId()
        {
            if (!(this.identity is int))
            {
                throw new InvalidCastException(
                    "the transport is not Netbios, you must not get the netbios information of client");
            }

            return Convert.ToInt32(this.identity);
        }


        /// <summary>
        /// get the TCP IPEndPoint of client.<para/>
        /// if the transport is not TCP, sdk will throw exception.
        /// </summary>
        /// <returns>the TCP IPEndPoint of client.</returns>
        /// <exception cref="InvalidCastException">
        /// the transport is not TCP, you must not get the IPEndPoint of client
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IPEndPoint GetClientIPEndPoint()
        {
            IPEndPoint clientIPEndPoint = this.identity as IPEndPoint;

            if (clientIPEndPoint == null)
            {
                throw new InvalidCastException(
                    "the transport is not TCP, you must not get the IPEndPoint of client");
            }

            return clientIPEndPoint;
        }


        /// <summary>
        /// get the treeconnect of connection
        /// </summary>
        /// <param name="treeId">the identity of treeconnect</param>
        /// <returns>the identitied treeconnect</returns>
        internal SmbServerTreeConnect GetTreeConnect(ushort treeId)
        {
            foreach (SmbServerSession session in this.SessionList)
            {
                SmbServerTreeConnect treeconnect = session.GetTreeConnect(treeId);

                if (treeconnect != null)
                {
                    return treeconnect;
                }
            }

            return null;
        }


        /// <summary>
        /// get the smb server preferred dialect index.
        /// </summary>
        /// <param name="dialectIndex">the selected dialect index</param>
        /// <param name="wordCount">the word count of packet</param>
        internal void GetPreferedDialectIndex(out ushort dialectIndex, out byte wordCount)
        {
            dialectIndex = 0x00;
            wordCount = 0x01;

            if (this.NegotiatedDialects == null || this.NegotiatedDialects.Length == 0)
            {
                // invalid dialect.
                dialectIndex = 0xFFFF;
            }
            else
            {
                // if exists NT LM 0.12, select this dialect. or, select the last dialect.
                for (int i = 0; i < this.NegotiatedDialects.Length; i++)
                {
                    if (DialectNameString.NTLANMAN == this.NegotiatedDialects[i])
                    {
                        // if the NTLANMAN dialect is select, its word count must be the following value.
                        wordCount = 17;
                        dialectIndex = (ushort)i;
                        break;
                    }
                    dialectIndex = (ushort)i;
                }
            }
        }


        /// <summary>
        /// dispose the gssapi
        /// </summary>
        internal void DisposeGssApi()
        {
            if (this.GssApi == null)
            {
                return;
            }

            SspiServerSecurityContext sspiSecurityContext = this.GssApi as SspiServerSecurityContext;
            if (sspiSecurityContext != null)
            {
                sspiSecurityContext.Dispose();
            }
        }

        #endregion
    }
}
