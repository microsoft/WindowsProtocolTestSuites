// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the class of CifsServerPerOpenSearch which is used to contain the properties of Per Open Search.
    /// </summary>
    public class CifsServerPerOpenSearch
    {
        #region Fields
        
        private IFileServiceServerTreeConnect treeConnect;
        private ushort findSid;
        private ushort mid;
        private uint pid;
        private int searchGlobalId;

        #endregion

        #region Properties

        /// <summary>
        /// The SMB connection associated with this open.
        /// </summary>
        public CifsServerPerConnection Connection
        {
            get
            {
                return this.Session.Connection as CifsServerPerConnection;
            }
        }


        /// <summary>
        /// The SMB session associated with this open.
        /// </summary>
        public CifsServerPerSession Session
        {
            get
            {
                return this.treeConnect.Session as CifsServerPerSession;
            }
        }


        /// <summary>
        /// The SMB tree connect associated with this open.
        /// </summary>
        public IFileServiceServerTreeConnect TreeConnect
        {
            get
            {
                return this.treeConnect;
            }
        }


        /// <summary>
        /// The search handle (SID) identifying a search opened using the TRANS2_FIND_FIRST2 subcommand.
        /// </summary>
        public ushort FindSID
        {
            get
            {
                return this.findSid;
            }
        }


        /// <summary>
        /// The Multiplex ID (MID) of the client process that opened the search.
        /// </summary>
        public ushort MID
        {
            get
            {
                return this.mid;
            }
        }

        
        /// <summary>
        /// The Process ID (PID) of the client process that opened the search.
        /// </summary>
        public uint PID
        {
            get
            {
                return this.pid;
            }
        }

        /// <summary>
        /// The TreeConnect ID (TID) of the tree connect within which the search takes place.
        /// </summary>
        public ushort TID
        {
            get
            {
                return (ushort)this.treeConnect.TreeConnectId;
            }
        }


        /// <summary>
        /// The Session identified by the User ID (UID) that initiated the search.
        /// </summary>
        public ushort UID
        {
            get
            {
                return (ushort)this.Session.SessionId;
            }
        }


        /// <summary>
        /// A numeric value obtained by registration with the Server Service Remote Protocol.
        /// </summary>
        public int SearchGlobalId
        {
            get
            {
                return this.searchGlobalId;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CifsServerPerOpenSearch(
            IFileServiceServerTreeConnect treeConnect,
            ushort findSid,
            ushort mid,
            uint pid,
            int searchGlobalId)
        {
            this.treeConnect = treeConnect;
            this.findSid = findSid;
            this.mid = mid;
            this.pid = pid;
            this.searchGlobalId = searchGlobalId;
        }

        #endregion
    }
}