// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the class of CifsServerPerSession which is used to contain the properties of CIFS ClientPerSession.
    /// </summary>
    public class CifsServerPerSession : IFileServiceServerSession
    {
        #region Fields

        private IFileServiceServerConnection connection;
        private Dictionary<ushort, IFileServiceServerTreeConnect> treeConnectTable;
        private long sessionId;
        private ServerSecurityContext securityContext;
        private DateTime creationTime;
        private DateTime idleTime;
        private string userName;
        private int sessionGlobalId;

        #endregion

        #region  Properties

        /// <summary>
        /// The SMB connection associated with this session.
        /// </summary>
        public IFileServiceServerConnection Connection
        {
            get
            {
                return this.connection;
            }
        }


        /// <summary>
        /// The 16-byte session key associated with this session, as obtained from the authentication packages 
        /// after successful authentication.
        /// </summary>
        public byte[] SessionKey
        {
            get
            {
                return (this.securityContext != null) ? this.securityContext.SessionKey : null;
            }
        }


        /// <summary>
        /// The 16-byte session key associated with this session which is used in SMB only.
        /// </summary>
        public byte[] SessionKey4Smb
        {
            get
            {
                throw new NotSupportedException();
            }
        }


        /// <summary>
        /// The security context of the user that established the session, as obtained from the authentication
        /// subsystem after successful authentication.
        /// </summary>
        public ServerSecurityContext SecurityContext
        {
            get
            {
                return this.securityContext;
            }
        }


        /// <summary>
        /// The time that the session was established.
        /// </summary>
        public DateTime CreationTime
        {
            get
            {
                return this.creationTime;
            }
        }


        /// <summary>
        /// The time that the session processed its most recent request.
        /// </summary>
        public DateTime IdleTime
        {
            get
            {
                return this.idleTime;
            }
            internal set
            {
                this.idleTime = value;
            }
        }


        /// <summary>
        /// The name of the user who established the session.
        /// </summary>
        public string UserName
        {
            get
            {
                return this.userName;
            }
        }


        /// <summary>
        /// All the tree connects in this session. indexed by tree id.
        /// </summary>
        public ReadOnlyCollection<IFileServiceServerTreeConnect> TreeConnectTable
        {
            get
            {
                lock (this.treeConnectTable)
                {
                    return new ReadOnlyCollection<IFileServiceServerTreeConnect>(
                        new List<IFileServiceServerTreeConnect>(this.treeConnectTable.Values));
                }
            }
        }


        /// <summary>
        /// Session Id of the Session
        /// </summary>
        public long SessionId
        {
            get
            {
                return this.sessionId;
            }
        }


        /// <summary>
        /// A numeric 32-bit value obtained by registration with the Server Service Remote Protocol.
        /// </summary>
        public int SessionGlobalId
        {
            get
            {
                return this.sessionGlobalId;
            }
        }

        #endregion

        #region Access TreeConnectTable Methods

        /// <summary>
        /// Add a tree connect file into this TreeConnectTable.
        /// </summary>
        /// <param name="treeConnect">The tree connect to add.</param>
        /// <exception cref="System.ArgumentNullException">the tree connect is null</exception>
        /// <exception cref="System.ArgumentException">the treeConnect already exists</exception>
        public void AddTreeConnect(IFileServiceServerTreeConnect treeConnect)
        {
            lock (this.treeConnectTable)
            {
                this.treeConnectTable.Add((ushort)treeConnect.TreeConnectId, treeConnect);
            }
        }


        /// <summary>
        /// Remove a tree connect file from this TreeConnectTable.
        /// </summary>
        /// <param name="treeId">The tree id.</param>
        public void RemoveTreeConnect(ushort treeId)
        {
            lock (this.treeConnectTable)
            {
                this.treeConnectTable.Remove(treeId);
            }
        }


        /// <summary>
        /// Get the tree connect from this TreeConnectTable.
        /// </summary>
        /// <param name="treeId">The tree id.</param>
        public CifsServerPerTreeConnect GetTreeConnect(ushort treeId)
        {
            lock (this.treeConnectTable)
            {
                if (this.treeConnectTable.ContainsKey(treeId))
                {
                    return this.treeConnectTable[treeId] as CifsServerPerTreeConnect;
                }
                return null;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection">The SMB connection associated with this session.</param>
        /// <param name="sessionId">Session Id of the Session</param>
        /// <param name="securityContext">The security context of the user that established the session, as obtained
        /// from the authentication subsystem after successful authentication.</param>
        /// <param name="creationTime">The time that the session was established.</param>
        /// <param name="idleTime">The time that the session processed its most recent request.</param>
        /// <param name="userName">The name of the user who established the session.</param>
        /// <param name="sessionGlobalId">A numeric 32-bit value obtained by registration with the Server Service Remote
        /// Protocol.</param>
        public CifsServerPerSession(
            IFileServiceServerConnection connection,
            long sessionId,
            ServerSecurityContext securityContext,
            DateTime creationTime,
            DateTime idleTime,
            string userName,
            int sessionGlobalId)
        {
            this.connection = connection;
            this.sessionId = sessionId;
            this.securityContext = securityContext;
            this.creationTime = creationTime;
            this.idleTime = idleTime;
            this.userName = userName;
            this.sessionGlobalId = sessionGlobalId;
            this.treeConnectTable = new Dictionary<ushort, IFileServiceServerTreeConnect>();
        }

        #endregion
    }
}