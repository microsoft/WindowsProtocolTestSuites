// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// session of smb server
    /// </summary>
    public class SmbServerSession : IFileServiceServerSession
    {
        #region Fields of TD

        /// <summary>
        /// Indicates the state of the Session – Expired, InProgress or Completed.
        /// </summary>
        private SessionState authenticationState;

        /// <summary>
        /// The UID associated with this Session.
        /// </summary>
        private ushort uid;

        /// <summary>
        /// The user's security context associated with this session, as obtained from the authentication packages
        /// after successful authentication.
        /// </summary>
        private byte[] securityContext;

        /// <summary>
        /// The 16-byte session key associated with this session, as obtained from the authentication packages after 
        /// successful authentication.
        /// </summary>
        private byte[] sessionKey;

        /// <summary>
        /// The session key state. This can be either Unavailable or Available.
        /// </summary>
        private SessionKeyStateValue sessionKeyState;

        /// <summary>
        /// The connection on which the session was established as specified in section 3.3.5.5.1.
        /// </summary>
        private SmbServerConnection connection;

        #endregion

        #region Fields of Sdk

        /// <summary>
        /// the treeconnect table.
        /// </summary>
        private Collection<SmbServerTreeConnect> treeconnectTable;

        #endregion

        #region Properties of TD

        /// <summary>
        /// Indicates the state of the Session – Expired, InProgress or Completed.
        /// </summary>
        public SessionState AuthenticationState
        {
            get
            {
                return this.authenticationState;
            }
            set
            {
                this.authenticationState = value;
            }
        }


        /// <summary>
        /// The UID associated with this Session.
        /// </summary>
        public ushort Uid
        {
            get
            {
                return this.uid;
            }
            set
            {
                this.uid = value;
            }
        }


        /// <summary>
        /// Session Id of the Session.
        /// </summary>
        public long SessionId
        {
            get
            {
                return this.uid;
            }
            set
            {
                this.uid = (ushort)value;
            }
        }


        /// <summary>
        /// The user's security context associated with this session, as obtained from the authentication packages
        /// after successful authentication.
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
        /// The session key state. This can be either Unavailable or Available.
        /// </summary>
        public SessionKeyStateValue SessionKeyState
        {
            get
            {
                return this.sessionKeyState;
            }
            set
            {
                this.sessionKeyState = value;
            }
        }


        /// <summary>
        /// The 16-byte session key associated with this session, as obtained from the authentication packages after 
        /// successful authentication.
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
        /// The 16-byte session key associated with this session which is used in SMB only.
        /// </summary>
        public byte[] SessionKey4Smb
        {
            get
            {
                return FileServiceUtils.ProtectSessionKey(this.sessionKey);
            }
        }

        /// <summary>
        /// The connection on which the session was established as specified in section 3.3.5.5.1.
        /// </summary>
        public SmbServerConnection Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
            }
        }


        #endregion

        #region Properties of Sdk

        /// <summary>
        /// get the tree connect table
        /// </summary>
        public ReadOnlyCollection<SmbServerTreeConnect> TreeConnectList
        {
            get
            {
                return new ReadOnlyCollection<SmbServerTreeConnect>(this.treeconnectTable);
            }
        }


        /// <summary>
        /// get the tree connect table
        /// </summary>
        public ReadOnlyCollection<IFileServiceServerTreeConnect> TreeConnectTable
        {
            get
            {
                Collection<IFileServiceServerTreeConnect> ret = new Collection<IFileServiceServerTreeConnect>();
                foreach (SmbServerTreeConnect treeconnect in this.treeconnectTable)
                {
                    ret.Add(treeconnect);
                }
                return new ReadOnlyCollection<IFileServiceServerTreeConnect>(ret);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal SmbServerSession()
        {
            this.authenticationState = (SessionState)0;
            this.treeconnectTable = new Collection<SmbServerTreeConnect>();
        }


        #endregion

        #region Access TreeConnects

        /// <summary>
        /// get the treeconnect.
        /// </summary>
        /// <param name="treeId">the identity of treeconnect</param>
        /// <returns>the matched treeconnect</returns>
        internal SmbServerTreeConnect GetTreeConnect(ushort treeId)
        {
            lock (this.treeconnectTable)
            {
                foreach (SmbServerTreeConnect treeconnect in this.treeconnectTable)
                {
                    if (treeconnect.TreeId == treeId)
                    {
                        return treeconnect;
                    }
                }

                return null;
            }
        }


        /// <summary>
        /// add treeconnect to session
        /// </summary>
        /// <param name="treeconnect">the treeconnect to add</param>
        /// <exception cref="InvalidOperationException">the treeconnect has exist in the session!</exception>
        internal void AddTreeConnect(SmbServerTreeConnect treeconnect)
        {
            lock (this.treeconnectTable)
            {
                if (this.treeconnectTable.Contains(treeconnect))
                {
                    throw new InvalidOperationException("the treeconnect has exist in the session!");
                }

                this.treeconnectTable.Add(treeconnect);
            }
        }


        /// <summary>
        /// remove the treeconnect. if does not exists, do nothing.
        /// </summary>
        /// <param name="treeconnect">the treeconnect to remove</param>
        internal void RemoveTreeConnect(SmbServerTreeConnect treeconnect)
        {
            lock (this.treeconnectTable)
            {
                if (this.treeconnectTable.Contains(treeconnect))
                {
                    this.treeconnectTable.Remove(treeconnect);
                }
            }
        }


        #endregion


        /// <summary>
        /// Inherit from FileServiceServerSession, equals to Connection.
        /// </summary>
        IFileServiceServerConnection IFileServiceServerSession.Connection
        {
            get
            {
                return this.connection;
            }
        }
    }
}
