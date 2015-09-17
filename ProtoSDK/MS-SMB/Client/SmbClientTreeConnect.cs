// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the tree connect of smb. inherit form smb tree connect. 
    /// </summary>
    public class SmbClientTreeConnect  : CifsClientPerTreeConnect
    {
        #region Fields

        /// <summary>
        /// The MaximalShareAccessRights value as returned in 
        /// the SMB_COM_TREE_CONNECT_ANDX server response (section 2.2.7).
        /// </summary>
        private uint maximalShareAccessRights;

        /// <summary>
        /// The GuestMaximalShareAccessRights value as returned in the 
        /// SMB_COM_TREE_CONNECT_ANDX server response (section 2.2.7).
        /// </summary>
        private uint guestMaximalShareAccessRights;

        /// <summary>
        /// The SMB connection associated with this tree connection.
        /// </summary>
        private SmbClientConnection connection;

        /// <summary>
        /// the session to get the opentable
        /// </summary>
        private SmbClientSession session;

        /// <summary>
        /// SMB_EXTENDED_SIGNATURES<para/>
        /// If set, then the server is using signing key protection (see section 3.3.5.4), as requested by the client.
        /// </summary>
        internal const ushort SmbExtendedSignatures = 0x0020;

        /// <summary>
        /// TREE_CONNECT_ANDX_EXTENDED_SIGNATURES<para/>
        /// If set, then the client is requesting signing key protection,
        /// as specified in sections 3.2.4.2.5 and 3.2.5.4.
        /// </summary>
        internal const ushort TreeConnectAndxExtendedSignatures = 0x0004;

        #endregion

        #region Properties

        /// <summary>
        /// The share name corresponding to this tree connection.
        /// </summary>
        public new string ShareName
        {
            get
            {
                return base.ShareName;
            }
            set
            {
                base.ShareName = value;
            }
        }


        /// <summary>
        /// The MaximalShareAccessRights value as returned in 
        /// the SMB_COM_TREE_CONNECT_ANDX server response (section 2.2.7).
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
        /// The GuestMaximalShareAccessRights value as returned in the 
        /// SMB_COM_TREE_CONNECT_ANDX server response (section 2.2.7).
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


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="session">the session to get the opentable</param>
        protected SmbClientTreeConnect(SmbClientSession session)
            : base()
        {
            this.session = session;
        }


        /// <summary>
        /// Deep copy constructor. if need to copy the connection instance, you must call the Clone method. its sub 
        /// class inherit from this, and need to provide more features. 
        /// </summary>
        /// <param name="session">the session to get the opentable</param>
        /// <param name="treeconnect">the treeconnect to copy from</param>
        protected SmbClientTreeConnect(SmbClientSession session, SmbClientTreeConnect treeconnect)
            : base(treeconnect)
        {
            this.session = session;
            this.maximalShareAccessRights = treeconnect.maximalShareAccessRights;
            this.guestMaximalShareAccessRights = treeconnect.guestMaximalShareAccessRights;
            this.connection = treeconnect.connection;
        }


        /// <summary>
        /// Constructor with base class.
        /// </summary>
        /// <param name="session">the session to get the opentable</param>
        /// <param name="treeconnect">the treeconnect to copy from</param>
        internal SmbClientTreeConnect(SmbClientSession session, CifsClientPerTreeConnect treeconnect)
            : base(treeconnect)
        {
            this.session = session;
        }


        /// <summary>
        /// clone this instance. using for the context to copy the instances. if need to inherit from this class, this 
        /// method must be overrided. 
        /// </summary>
        /// <returns>a copy of this instance </returns>
        protected override CifsClientPerTreeConnect Clone()
        {
            return new SmbClientTreeConnect(this.session, this);
        }


        #endregion

        #region Properties in base class TreeConnect

        /// <summary>
        /// a copy of files of session.
        /// A table of opens of files or named pipes, as specified in section 3.3.1.10, that have been opened
        /// by this authenticated session. This table MUST be uniquely indexed by Open.FileId and MUST support
        /// enumeration of all entries in the table.
        /// </summary>
        public ReadOnlyCollection<SmbClientOpen> OpenFileTable
        {
            get
            {
                Collection<SmbClientOpen> opens = new Collection<SmbClientOpen>();
                foreach (CifsClientPerOpenFile open in this.session.OpenFileList)
                {
                    if (open.TreeConnectId == this.TreeId)
                    {
                        opens.Add(new SmbClientOpen(open));
                    }
                }
                return new ReadOnlyCollection<SmbClientOpen>(opens);
            }
        }


        #endregion
    }
}
