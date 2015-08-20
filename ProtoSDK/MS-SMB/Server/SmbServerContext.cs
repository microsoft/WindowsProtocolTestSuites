// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// contain all server data 
    /// </summary>
    public class SmbServerContext : FileServiceServerContext
    {
        #region Fields of TD

        /// <summary>
        /// A list of available shares that are present on this server indexed by the share name, as specified in 
        /// section 3.3.1.7.
        /// </summary>
        private Dictionary<string, SmbServerShare> shareList;

        /// <summary>
        /// Global capability bits supported by this server.
        /// </summary>
        private volatile Capabilities globalCapabilities;

        #endregion

        #region Fields of SmbSdk

        /// <summary>
        /// if true, stack will update context automatically
        /// </summary>
        private bool isUpdateContext;

        /// <summary>
        /// the connection list
        /// </summary>
        private Collection<SmbServerConnection> connectionList;

        #endregion

        #region Properties of TD

        /// <summary>
        /// Global capability bits supported by this server
        /// </summary>
        public Capabilities GlobalCapabilities
        {
            get
            {
                return this.globalCapabilities;
            }
            set
            {
                this.globalCapabilities = value;
            }
        }


        /// <summary>
        /// A list of available shares that are present on this server indexed by the share name, as specified in 
        /// section 3.3.1.7.
        /// </summary>
        public Dictionary<string, SmbServerShare> ShareList
        {
            get
            {
                return this.shareList;
            }
            set
            {
                this.shareList = value;
            }
        }


        #endregion

        #region Properties of SmbSdk

        /// <summary>
        /// if true, stack will update context automatically
        /// </summary>
        public bool IsUpdateContext
        {
            get
            {
                return this.isUpdateContext;
            }
            set
            {
                this.isUpdateContext = value;
            }
        }


        /// <summary>
        /// get all connections.
        /// </summary>
        public ReadOnlyCollection<SmbServerConnection> ConnectionList
        {
            get
            {
                return new ReadOnlyCollection<SmbServerConnection>(this.connectionList);
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal SmbServerContext()
        {
            this.shareList = new Dictionary<string, SmbServerShare>();
            this.globalCapabilities = (Capabilities)0x00;
            this.isUpdateContext = true;
            this.connectionList = new Collection<SmbServerConnection>();
        }


        #endregion

        #region Access Connectoins

        /// <summary>
        /// get the connection by connection identity
        /// </summary>
        /// <param name="identity">the identity of connection</param>
        /// <returns>the connection object.</returns>
        internal SmbServerConnection GetConnection(object identity)
        {
            foreach (SmbServerConnection connection in this.connectionList)
            {
                if (connection.Identity.Equals(identity))
                {
                    return connection;
                }
            }

            return null;
        }


        /// <summary>
        /// Add Connection to context
        /// </summary>
        /// <param name="connection">the connection</param>
        /// <exception cref="InvalidOperationException">
        /// the connection already exists in the connection list, can not add duplicate connection
        /// </exception>
        internal void AddConnection(SmbServerConnection connection)
        {
            lock (this.connectionList)
            {
                if(this.connectionList.Contains(connection))
                {
                    throw new InvalidOperationException(
                        "the connection already exists in the connection list, can not add duplicate connection");
                }

                this.connectionList.Add(connection);
            }
        }


        /// <summary>
        /// Remove connection from the context. if connection does not exists, do nothing.
        /// </summary>
        /// <param name="connection">the connection to remove</param>
        internal void RemoveConnection(SmbServerConnection connection)
        {
            lock (this.connectionList)
            {
                if (this.connectionList.Contains(connection))
                {
                    this.connectionList.Remove(connection);
                }
            }
        }


        #endregion

        #region Update Context

        /// <summary>
        /// update the context of smb server
        /// </summary>
        /// <param name="connection">the connection of the client</param>
        /// <param name="packet">the packet to update the context</param>
        internal void UpdateRoleContext(SmbServerConnection connection, SmbPacket packet)
        {
            if (connection == null)
            {
                return;
            }

            // packet status
            SmbStatus packetStatus = (SmbStatus)packet.SmbHeader.Status;

            // filter error packet
            if (packetStatus != SmbStatus.STATUS_SUCCESS &&
                    packetStatus != SmbStatus.STATUS_MORE_PROCESSING_REQUIRED &&
                    packetStatus != SmbStatus.STATUS_BUFFER_OVERFLOW)
            {
                return;
            }

            // process the response packet.
            if (packet.PacketType == SmbPacketType.BatchedResponse
                || packet.PacketType == SmbPacketType.SingleResponse)
            {
                ResponsePacketUpdateRoleContext(connection, packet);
                return;
            }

            // process the request packet.
            if (packet.PacketType == SmbPacketType.BatchedRequest
                || packet.PacketType == SmbPacketType.SingleRequest)
            {
                RequestPacketUpdateRoleContext(connection, packet);
                return;
            }
        }


        /// <summary>
        /// update the context with response packet
        /// </summary>
        /// <param name="connection">the connection of endpoint</param>
        /// <param name="packet">the packet to update the context</param>
        private void ResponsePacketUpdateRoleContext(SmbServerConnection connection, SmbPacket packet)
        {
            SmbHeader smbHeader = packet.SmbHeader;

            SmbPacket requestPacket = connection.GetRequestPacket(smbHeader.Mid);
            if (requestPacket == null)
            {
                return;
            }

            switch (smbHeader.Command)
            {
                case SmbCommand.SMB_COM_SESSION_SETUP_ANDX:
                    if (smbHeader.Uid == 0)
                    {
                        break;
                    }
                    else
                    {
                        SmbServerSession session = new SmbServerSession();
                        session.Uid = smbHeader.Uid;
                        session.AuthenticationState = SessionState.Complete;
                        session.Connection = connection;
                        session.SessionKey = connection.GssApi.SessionKey;

                        connection.AddSession(session);
                    }

                    break;

                case SmbCommand.SMB_COM_LOGOFF_ANDX:
                    if (requestPacket.SmbHeader.Uid == 0)
                    {
                        break;
                    }
                    else
                    {
                        connection.RemoveSession(connection.GetSession(requestPacket.SmbHeader.Uid));
                    }

                    break;

                case SmbCommand.SMB_COM_TREE_CONNECT_ANDX:
                    {
                        SmbTreeConnectAndxRequestPacket request = requestPacket as SmbTreeConnectAndxRequestPacket;

                        SmbServerTreeConnect treeconnect = new SmbServerTreeConnect();
                        treeconnect.TreeId = smbHeader.Tid;
                        treeconnect.Session = connection.GetSession(smbHeader.Uid);
                        if (treeconnect.Session == null)
                        {
                            break;
                        }
                        if ((request.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) != 0)
                        {
                            treeconnect.Path = Encoding.Unicode.GetString(request.SmbData.Path);
                        }
                        else
                        {
                            treeconnect.Path = Encoding.ASCII.GetString(request.SmbData.Path);
                        }
                        treeconnect.Path = treeconnect.Path.TrimEnd('\0');
                        treeconnect.Session.AddTreeConnect(treeconnect);
                    }

                    break;

                case SmbCommand.SMB_COM_TREE_DISCONNECT:
                    if (requestPacket.SmbHeader.Uid != 0)
                    {
                        SmbServerSession session = connection.GetSession(requestPacket.SmbHeader.Uid);
                        if (session == null)
                        {
                            break;
                        }
                        session.RemoveTreeConnect(session.GetTreeConnect(requestPacket.SmbHeader.Tid));
                    }

                    break;

                case SmbCommand.SMB_COM_NT_CREATE_ANDX:
                    {
                        SmbNtCreateAndxResponsePacket response = packet as SmbNtCreateAndxResponsePacket;
                        SmbNtCreateAndxRequestPacket request = requestPacket as SmbNtCreateAndxRequestPacket;

                        SmbServerOpen open = new SmbServerOpen();
                        open.SmbFid = response.SmbParameters.FID;
                        open.PathName = SmbMessageUtils.GetString(request.SmbData.FileName,
                            SmbFlags2.SMB_FLAGS2_UNICODE == (request.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE));
                        open.PathName = open.PathName.TrimEnd('\0');
                        open.Session = connection.GetSession(smbHeader.Uid);
                        open.TreeConnect = connection.GetTreeConnect(smbHeader.Tid);
                        if (open.TreeConnect == null)
                        {
                            break;
                        }
                        open.TreeConnect.AddOpen(open);
                    }

                    break;

                case SmbCommand.SMB_COM_OPEN_ANDX:
                    {
                        SmbOpenAndxResponsePacket response = packet as SmbOpenAndxResponsePacket;
                        SmbOpenAndxRequestPacket request = requestPacket as SmbOpenAndxRequestPacket;

                        SmbServerOpen open = new SmbServerOpen();
                        open.SmbFid = response.SmbParameters.FID;
                        open.PathName = SmbMessageUtils.GetString(request.SmbData.FileName,
                            SmbFlags2.SMB_FLAGS2_UNICODE == (request.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE));
                        open.Session = connection.GetSession(smbHeader.Uid);
                        open.TreeConnect = connection.GetTreeConnect(smbHeader.Tid);
                        if (open.TreeConnect == null)
                        {
                            break;
                        }
                        open.TreeConnect.AddOpen(open);
                    }

                    break;

                case SmbCommand.SMB_COM_CLOSE:
                    {
                        SmbCloseRequestPacket closeRequest = requestPacket as SmbCloseRequestPacket;

                        SmbServerTreeConnect treeconnect = connection.GetTreeConnect(requestPacket.SmbHeader.Tid);
                        if (treeconnect == null)
                        {
                            break;
                        }

                        treeconnect.RemoveOpen(treeconnect.GetOpen(closeRequest.SmbParameters.FID));
                    }

                    break;

                default:
                    break;
            }

            connection.RemoveRequestPacket(packet);
        }


        /// <summary>
        /// update the context with request packet
        /// </summary>
        /// <param name="connection">the connection of endpoint</param>
        /// <param name="packet">the packet to update the context</param>
        private void RequestPacketUpdateRoleContext(SmbServerConnection connection, SmbPacket packet)
        {
            connection.AddRequestPacket(packet);

            // update the message id
            connection.MessageId = packet.SmbHeader.Mid;

            // update the process id
            connection.ProcessId = (uint)(packet.SmbHeader.PidHigh << 16);
            connection.ProcessId += packet.SmbHeader.PidLow;

            // update the message sign sequence number
            if (packet.SmbHeader.SecurityFeatures != 0
                && connection.GssApi != null && connection.GssApi.SessionKey != null)
            {
                if (packet.SmbHeader.Command == SmbCommand.SMB_COM_NT_CANCEL)
                {
                    connection.ServerNextReceiveSequenceNumber++;
                }
                else
                {
                    ServerSendSequenceNumberKey key = new ServerSendSequenceNumberKey();

                    key.PidHigh = packet.SmbHeader.PidHigh;
                    key.PidLow = packet.SmbHeader.PidLow;
                    key.Mid = packet.SmbHeader.Mid;

                    connection.ServerSendSequenceNumber[key] = connection.ServerNextReceiveSequenceNumber + 1;
                    connection.ServerNextReceiveSequenceNumber += 2;
                }
            }

            // process each special command
            switch (packet.SmbHeader.Command)
            {
                case SmbCommand.SMB_COM_NEGOTIATE:
                    SmbNegotiateRequestPacket request = packet as SmbNegotiateRequestPacket;
                    byte[] dialects = request.SmbData.Dialects;

                    List<string> negotiateDialects = new List<string>();

                    for (int i = 0; i < dialects.Length; i++)
                    {
                        if (dialects[i] == 0x02)
                        {
                            continue;
                        }

                        string dialect = "";

                        for (; i < dialects.Length && dialects[i] != 0x00; i++)
                        {
                            dialect += (char)dialects[i];
                        }

                        negotiateDialects.Add(dialect);
                    }

                    connection.NegotiatedDialects = negotiateDialects.ToArray();

                    break;

                case SmbCommand.SMB_COM_TREE_CONNECT_ANDX:

                    // down-case the packet
                    Cifs.SmbTreeConnectAndxRequestPacket treeconnect = packet as Cifs.SmbTreeConnectAndxRequestPacket;

                    // get the specified session.
                    SmbServerSession treeconnectSession = connection.GetSession((ushort)treeconnect.SmbHeader.Uid);
                    if (treeconnectSession == null)
                    {
                        return;
                    }

                    // Calculate the one-way hash
                    byte[] sessionKey = FileServiceUtils.ProtectSessionKey(treeconnectSession.SessionKey);

                    // update the session key state.
                    treeconnectSession.SessionKeyState = SessionKeyStateValue.Available;

                    // if server does not support SMB_EXTENDED_SIGNATURES, return.
                    if (SmbClientTreeConnect.TreeConnectAndxExtendedSignatures !=
                        (treeconnect.SmbParameters.Flags & SmbClientTreeConnect.TreeConnectAndxExtendedSignatures))
                    {
                        return;
                    }

                    treeconnectSession.SessionKey = sessionKey;

                    break;

                default:
                    return;
            }

            return;
        }


        #endregion
    }
}
