// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// FileAccessCollection provides a container for all SMB2 protocol related
    /// context in an SMB2 server or client.
    /// </summary>
    public class FileAccessCollection
    {
        #region fields

        private Collection<Share> shareList;
        private Collection<Open> globalOpenTable;
        private Collection<TreeConnect> globalTreeConnectTable;
        private Collection<Session> globalSessionTable;
        private Collection<Connection> connectionList;
        private int nextShareGlobalIndex;
        private int nextOpenGlobalIndex;
        private int nextTreeConnectGlobalIndex;
        private int nextSessionGlobalIndex;
        private int nextConnectionGlobalIndex;

        #endregion


        #region properties

        /// <summary>
        /// A list of available shares for the system. The structure of a share is as specified
        /// in section 3.3.1.6. The list MUST be uniquely indexed by the share name.
        /// </summary>
        public Collection<Share> ShareList
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


        /// <summary>
        /// A table containing all the files opened by remote clients on the server. The structure
        /// of an open is as specified in section 3.3.1.10. The table MUST be uniquely indexed by 
        /// Open.DurableFileId and MUST support enumeration of all entries in the table.
        /// </summary>
        public Collection<Open> GlobalOpenTable
        {
            get
            {
                return this.globalOpenTable;
            }
            set
            {
                this.globalOpenTable = value;
            }
        }


        /// <summary>
        /// A list of all treeconnect connected by clients on the server.
        /// </summary>
        public Collection<TreeConnect> GlobalTreeConnectTable
        {
            get
            {
                return this.globalTreeConnectTable;
            }
            set
            {
                this.globalTreeConnectTable = value;
            }
        }


        /// <summary>
        /// A list of all the authenticated sessions established to this server, indexed by the 
        /// SessionId. The server MUST also be able to search the list by security principal, and
        /// the list MUST allow for multiple sessions with the same security principal on different
        /// connections.
        /// </summary>
        public Collection<Session> GlobalSessionTable
        {
            get
            {
                return this.globalSessionTable;
            }
            set
            {
                this.globalSessionTable = value;
            }
        }


        /// <summary>
        /// A list of all open connections on the server, indexed by the connection endpoint addresses.
        /// </summary>
        public Collection<Connection> ConnectionList
        {
            get
            {
                return this.connectionList;
            }
            set
            {
                this.connectionList = value;
            }
        }


        /// <summary>
        /// to Save the global index next Share instance. It should be increased by 1 when a new Share
        /// is created.
        /// </summary>
        public int NextShareGlobalIndex
        {
            get
            {
                return this.nextShareGlobalIndex;
            }
            set
            {
                this.nextShareGlobalIndex = value;
            }
        }


        /// <summary>
        /// to Save the global index next Open instance. It should be increased by 1 when a new Open
        /// is created.
        /// </summary>
        public int NextOpenGlobalIndex
        {
            get
            {
                return this.nextOpenGlobalIndex;
            }
            set
            {
                this.nextOpenGlobalIndex = value;
            }
        }


        /// <summary>
        /// to Save the global index next TreeConnect instance. It should be increased by 1 when a new
        /// TreeConnect is created.
        /// </summary>
        public int NextTreeConnectGlobalIndex
        {
            get
            {
                return this.nextTreeConnectGlobalIndex;
            }
            set
            {
                this.nextTreeConnectGlobalIndex = value;
            }
        }


        /// <summary>
        /// to Save the global index next Session instance. It should be increased by 1 when a new Session
        /// is created.
        /// </summary>
        public int NextSessionGlobalIndex
        {
            get
            {
                return this.nextSessionGlobalIndex;
            }
            set
            {
                this.nextSessionGlobalIndex = value;
            }
        }


        /// <summary>
        /// to Save the Connection index next Share instance. It should be increased by 1 when a new
        /// Connection is created.
        /// </summary>
        public int NextConnectionGlobalIndex
        {
            get
            {
                return this.nextConnectionGlobalIndex;
            }
            set
            {
                this.nextConnectionGlobalIndex = value;
            }
        }

        #endregion


        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileAccessCollection()
        {
            this.ShareList = new Collection<Share>();
            this.GlobalOpenTable = new Collection<Open>();
            this.GlobalTreeConnectTable = new Collection<TreeConnect>();
            this.GlobalSessionTable = new Collection<Session>();
            this.ConnectionList = new Collection<Connection>();
            this.NextShareGlobalIndex = 1;
            this.NextOpenGlobalIndex = 1;
            this.NextTreeConnectGlobalIndex = 1;
            this.NextSessionGlobalIndex = 1;
            this.NextConnectionGlobalIndex = 1;
        }

        #endregion
    }
}