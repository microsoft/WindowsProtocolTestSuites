// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// An authenticated context that is established between am SMB 2.0 
    /// Protocol client and an SMB 2.0 Protocol server over an SMB 2.0 
    /// Protocol connection for a specific security principal. There 
    /// could be multiple active sessions over a single SMB 2.0 Protocol
    /// connection. The SessionId field distinguishes the various sessions.
    /// </summary>
    public class Session
    {
        #region fields

        private int globalIndex;
        private ulong sessionId;
        private int connectionId;
        private uint state;
        private byte[] securityContext;
        private byte[] sessionKey;
        private bool shouldSign;

        /// <summary>
        /// all opens of a session.
        /// </summary>
        protected Collection<Open> openTable;

        /// <summary>
        /// all tree connects of a session.
        /// </summary>
        protected Collection<TreeConnect> treeConnectTable;

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
        /// A numeric value that uniquely identifies the session within the scope of the transport 
        /// connection over which the session was established. This value, transformed into a 64-bit 
        /// number, is typically sent to clients as the SessionId in the SMB2 header.
        /// </summary>
        public ulong SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = value;
            }
        }


        /// <summary>
        /// The connection on which this session was established (see also sections 3.3.5.5.1 and 3.3.4.4).
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
        /// The current activity state of this session. This value MUST be either InProgress, Valid, 
        /// or Expired.
        /// </summary>
        public uint State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }


        /// <summary>
        /// The security context of the user that authenticated this session. This value MUST be in a form
        /// that allows for evaluating security descriptors within the server, as well as being passed to 
        /// the underlying object store to handle security evaluation that may happen there.
        /// </summary>
        public byte[] SecurityContext
        {
            get
            {
                return this.securityContext;
            }
            set
            {
                this.securityContext = value;
            }
        }


        /// <summary>
        /// The 16-byte cryptographic key for this authenticated context.
        /// </summary>
        public byte[] SessionKey
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
        /// The 16-byte cryptographic key for this authenticated context which is used in SMB only.
        /// </summary>
        public byte[] SessionKey4Smb
        {
            get
            {
                return FileServiceUtils.ProtectSessionKey(this.sessionKey);
            }
        }


        /// <summary>
        /// A Boolean that, if set, indicates that this session MUST sign communication if signing is enabled
        /// on this connection.
        /// </summary>
        public bool ShouldSign
        {
            get
            {
                return this.shouldSign;
            }
            set
            {
                this.shouldSign = value;
            }
        }

        #endregion


        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Session()
        {
            this.GlobalIndex = 0;
            this.SessionId = 0;
            this.ConnectionId = 0;
            this.State = 0;
            this.SecurityContext = new byte[0];
            this.SessionKey = new byte[16];
            this.ShouldSign = false;
            this.openTable = new Collection<Open>();
            this.treeConnectTable = new Collection<TreeConnect>();
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionId">the connection identity of the session.</param>
        /// <param name="sessionId">the session identity of the session.</param>
        public Session(int connectionId, ulong sessionId)
        {
            this.GlobalIndex = 0;
            this.SessionId = sessionId;
            this.ConnectionId = connectionId;
            this.State = 0;
            this.SecurityContext = new byte[0];
            this.SessionKey = new byte[16];
            this.ShouldSign = false;
            this.openTable = new Collection<Open>();
            this.treeConnectTable = new Collection<TreeConnect>();
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public Session(Session session)
        {
            if (session != null)
            {
                this.GlobalIndex = session.GlobalIndex;
                this.SessionId = session.SessionId;
                this.ConnectionId = session.ConnectionId;
                this.State = session.State;
                this.SecurityContext = new byte[session.SecurityContext.Length];
                Array.Copy(session.SecurityContext, this.SecurityContext, session.SecurityContext.Length);
                this.SessionKey = new byte[session.SessionKey.Length];
                Array.Copy(session.SessionKey, this.SessionKey, session.SessionKey.Length);
                this.ShouldSign = session.ShouldSign;
                this.openTable = new Collection<Open>();
                foreach (Open open in session.openTable)
                {
                    this.openTable.Add(new Open(open));
                }
                this.treeConnectTable = new Collection<TreeConnect>();
                foreach (TreeConnect treeConnect in session.treeConnectTable)
                {
                    this.treeConnectTable.Add(new TreeConnect(treeConnect));
                }
            }
            else
            {
                this.GlobalIndex = 0;
                this.SessionId = 0;
                this.ConnectionId = 0;
                this.State = 0;
                this.SecurityContext = new byte[0];
                this.SessionKey = new byte[16];
                this.ShouldSign = false;
                this.openTable = new Collection<Open>();
                this.treeConnectTable = new Collection<TreeConnect>();
            }
        }

        #endregion
    }
}