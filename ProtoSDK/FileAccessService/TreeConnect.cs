// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// A connection by a specific session on an SMB 2.0 Protocol client to a specific 
    /// share on an SMB 2.0 Protocol server over an SMB 2.0 Protocol connection. There 
    /// could be multiple tree connects over a single SMB 2.0 Protocol connection. The
    /// TreeId field in the SMB2 packet header distinguishes the various tree connects.
    /// </summary>
    public class TreeConnect
    {
        #region fields

        private int globalIndex;
        private ulong treeId;
        private ulong sessionId;
        private int connectionId;
        private string share;

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
        /// A numeric value that uniquely identifies a tree connect within the scope of the session
        /// over which it was established. This value is typically represented as a 32-bit TreeId 
        /// in the SMB2 header.
        /// </summary>
        public ulong TreeId
        {
            get
            {
                return this.treeId;
            }
            set
            {
                this.treeId = value;
            }
        }


        /// <summary>
        /// A pointer to the authenticated session that established this tree connect.
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
        ///  A pointer to the share that this tree connect was established for.
        /// </summary>
        public string Share
        {
            get
            {
                return this.share;
            }
            set
            {
                this.share = value;
            }
        }

        #endregion


        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TreeConnect()
        {
            this.GlobalIndex = 0;
            this.TreeId = 0;
            this.SessionId = 0;
            this.ConnectionId = 0;
            this.Share = "";
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionId">the connectionId of the treeconnect.</param>
        /// <param name="sessionId">the sessionId of the treeconnect.</param>
        /// <param name="treeId">the treeId of the treeconnect.</param>
        public TreeConnect(int connectionId, ulong sessionId, ulong treeId)
        {
            this.GlobalIndex = 0;
            this.TreeId = treeId;
            this.SessionId = sessionId;
            this.ConnectionId = connectionId;
            this.Share = "";
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public TreeConnect(TreeConnect treeConnect)
        {
            if (treeConnect != null)
            {
                this.GlobalIndex = treeConnect.GlobalIndex;
                this.TreeId = treeConnect.TreeId;
                this.SessionId = treeConnect.SessionId;
                this.ConnectionId = treeConnect.ConnectionId;
                this.Share = treeConnect.Share;
            }
            else
            {
                this.GlobalIndex = 0;
                this.TreeId = 0;
                this.SessionId = 0;
                this.ConnectionId = 0;
                this.Share = "";
            }
        }

        #endregion
    }
}