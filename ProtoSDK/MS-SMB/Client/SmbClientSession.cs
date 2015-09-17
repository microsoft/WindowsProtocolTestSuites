// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the session of smb. inherit form smb session. 
    /// </summary>
    public class SmbClientSession  : CifsClientPerSession
    {
        #region Fields

        /// <summary>
        /// The SMB connection associated with this tree connection.
        /// </summary>
        private SmbClientConnection connection;

        /// <summary>
        /// The session key state. This can be either Unavailable or Available.
        /// </summary>
        private SessionKeyStateValue sessionKeyState;

        #endregion

        #region Properties

        /// <summary>
        /// The SMB connection associated with this tree connection.
        /// </summary>
        public SmbClientConnection Connection
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


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        internal SmbClientSession()
            : base()
        {
        }


        /// <summary>
        /// Deep copy constructor. if need to copy the connection instance, you must call the Clone method. its sub 
        /// class inherit from this, and need to provide more features. 
        /// </summary>
        protected SmbClientSession(SmbClientSession session)
            : base(session)
        {
            this.connection = session.connection;
            this.sessionKeyState = session.sessionKeyState;
        }


        /// <summary>
        /// Constructor with base class.
        /// </summary>
        internal SmbClientSession(CifsClientPerSession session)
            : base(session)
        {
        }


        /// <summary>
        /// clone this instance. using for the context to copy the instances. if need to inherit from this class, this 
        /// method must be overrided. 
        /// </summary>
        /// <returns>a copy of this instance </returns>
        protected override CifsClientPerSession Clone()
        {
            return new SmbClientSession(this);
        }


        #endregion

        #region Properties in base class Session

        /// <summary>
        /// a copy of tree connects of session.
        /// A table of tree connects that have been established by this authenticated session to shares on this
        /// server. This table MUST be uniquely indexed by TreeConnect.TreeId and MUST allow enumeration of all
        /// entries in the table.
        /// </summary>
        public ReadOnlyCollection<SmbClientTreeConnect> TreeConnectTable
        {
            get
            {
                Collection<SmbClientTreeConnect> treeconnects = new Collection<SmbClientTreeConnect>();
                
                foreach(TreeConnect treeconnect in this.treeConnectTable)
                {
                    treeconnects.Add(new SmbClientTreeConnect(this, treeconnect as CifsClientPerTreeConnect));
                }

                return new ReadOnlyCollection<SmbClientTreeConnect>(treeconnects);
            }
        }


        #endregion
    }
}
