// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// Either a TCP or NetBIOS over TCP connection between an SMB 2.0 Protocol
    /// client and an SMB 2.0 Protocol server. only TCP is supported in the SMB2
    /// StackSdk.
    /// </summary>
    public class Connection
    {
        #region fields

        private int globalIndex;
        private int connectionId;
        private Collection<MessageIdStatus> commandSequenceWindow;
        private Dictionary<ulong, StackPacket> requestList;
        private uint clientCapabilities;
        private bool negotiateReceived;

        /// <summary>
        /// used to contain all sessions of a connection.
        /// </summary>
        protected Collection<Session> sessionTable;

        #endregion


        #region properties

        /// <summary>
        /// A global integer index in the Global Table of Context. It is be global unique. The value is 1
        /// for the first instance, and it always increases by 1 when a new instance is created.
        /// </summary>
        public int GlobalIndex
        {
            get
            {
                return this.globalIndex;
            }
            set
            {
                this.globalIndex = value;
            }
        }


        /// <summary>
        /// the connection identity.
        /// </summary>
        public int ConnectionId
        {
            get
            {
                return this.connectionId;
            }
            set
            {
                this.connectionId = value;
            }
        }


        /// <summary>
        /// A list of the sequence numbers that are valid to receive from the client at this time. 
        /// For more information, see section 3.3.1.2.
        /// </summary>
        public Collection<MessageIdStatus> CommandSequenceWindow
        {
            get
            {
                return this.commandSequenceWindow;
            }
            set
            {
                this.commandSequenceWindow = value;
            }
        }


        /// <summary>
        /// A list of all client requests being processed. Each request MUST include a Boolean value 
        /// that indicates whether it is being handled asynchronously.
        /// </summary>
        public Dictionary<ulong, StackPacket> RequestList
        {
            get
            {
                return this.requestList;
            }
            set
            {
                this.requestList = value;
            }
        }


        /// <summary>
        /// The capabilities of the client of this connection in a form that MUST follow the syntax as
        /// specified in section 2.2.5.
        /// </summary>
        public uint ClientCapabilities
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


        /// <summary>
        /// A Boolean indicating whether a negotiate request has been received on this transport connection.
        /// </summary>
        public bool NegotiateReceived
        {
            get
            {
                return this.negotiateReceived;
            }
            set
            {
                this.negotiateReceived = value;
            }
        }

        #endregion


        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Connection()
        {
            this.GlobalIndex = 0;
            this.ConnectionId = 0;
            this.sessionTable = new Collection<Session>();
            this.CommandSequenceWindow = new Collection<MessageIdStatus>();
            this.RequestList = new Dictionary<ulong, StackPacket>();
            this.ClientCapabilities = 0;
            this.NegotiateReceived = false;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionId">the identification of the connection.</param>
        public Connection(int connectionId)
        {
            this.GlobalIndex = 0;
            this.ConnectionId = connectionId;
            this.sessionTable = new Collection<Session>();
            this.CommandSequenceWindow = new Collection<MessageIdStatus>();
            this.RequestList = new Dictionary<ulong, StackPacket>();
            this.ClientCapabilities = 0;
            this.NegotiateReceived = false;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public Connection(Connection connection)
        {
            if (connection != null)
            {
                this.GlobalIndex = connection.GlobalIndex;
                this.ConnectionId = connection.ConnectionId;
                this.sessionTable = new Collection<Session>();
                foreach (Session session in connection.sessionTable)
                {
                    this.sessionTable.Add(new Session(session));
                }
                this.CommandSequenceWindow = new Collection<MessageIdStatus>();
                foreach (MessageIdStatus messageIdStatus in connection.CommandSequenceWindow)
                {
                    this.CommandSequenceWindow.Add(new MessageIdStatus(messageIdStatus));
                }
                this.RequestList = new Dictionary<ulong, StackPacket>();
                foreach (KeyValuePair<ulong, StackPacket> request in connection.RequestList)
                {
                    this.RequestList.Add(request.Key, request.Value.Clone());
                }
                this.ClientCapabilities = connection.ClientCapabilities;
                this.NegotiateReceived = connection.NegotiateReceived;
            }
            else
            {
                this.GlobalIndex = 0;
                this.ConnectionId = 0;
                this.sessionTable = new Collection<Session>();
                this.CommandSequenceWindow = new Collection<MessageIdStatus>();
                this.RequestList = new Dictionary<ulong, StackPacket>();
                this.ClientCapabilities = 0;
                this.NegotiateReceived = false;
            }
        }

        #endregion
    }
}