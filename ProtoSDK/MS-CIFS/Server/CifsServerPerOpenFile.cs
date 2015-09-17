// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the class of CifsServerPerOpenFile which is used to contain the properties of CIFS Per Open.
    /// </summary>
    public class CifsServerPerOpenFile : IFileServiceServerOpen
    {
        #region Fields
        private IFileServiceServerTreeConnect treeConnect;
        private string pathName;
        private long fileId;
        private int fileGlobalId;

        private uint pid;
        private Dictionary<int, object> locks;
        private OplockLevelValue opLock;
        private uint grantedAccess;

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
        /// A variable-length string that contains the Unicode path name on which the open is performed.
        /// </summary>
        public string PathName
        {
            get
            {
                return this.pathName;
            }
        }


        /// <summary>
        /// For Cifs/Smb, this represents the unique (per-connection) 16-bit FID identifying this open.
        /// For Smb2, this represents FILEID.volatile
        /// </summary>
        public long FileId
        {
            get
            {
                return this.fileId;
            }
        }


        /// <summary>
        /// A numeric value obtained by registration with the Server Service Remote Protocol.
        /// </summary>
        public int FileGlobalId
        {
            get
            {
                return this.fileGlobalId;
            }
        }


        /// <summary>
        /// A list of byte-range locks on this open. Each entry MUST include the PID that created the lock. Each entry
        /// MUST indicate whether it is a shared (read-only) or an exclusive (read-write) lock. Each entry MUST also
        /// indicate if it is using 32- or 64-bit file offsets and MUST be accordingly formatted as either
        /// LOCKING_ANDX_RANGE32 or LOCKING_ANDX_RANGE64.
        /// </summary>
        public Dictionary<int, object> Locks
        {
            get
            {
                return this.locks;
            }
        }


        /// <summary>
        /// An element indicating the type of OpLock, if any, that has been granted on this open. This value MUST be one
        /// of None, Exclusive, Batch, or Level II.
        /// </summary>
        public OplockLevelValue OpLock
        {
            get
            {
                return this.opLock;
            }
            set
            {
                this.opLock = value;
            }
        }


        /// <summary>
        /// The unique (per connection) 32-bit PID provided in the client request that created this open. The PID is
        /// described in section 2.2.1.6.3.
        /// </summary>
        public uint PID
        {
            get
            {
                return this.pid;
            }
        }


        /// <summary>
        /// The access granted on this open.
        /// </summary>
        public uint GrantedAccess
        {
            get
            {
                return this.grantedAccess;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// public Constructor
        /// </summary>
        /// <param name="treeConnect">The SMB tree connect associated with this open.</param>
        /// <param name="pathName">A variable-length string that contains the Unicode path name on which the open 
        /// is performed.</param>
        /// <param name="fileId">The unique (per-connection) 16-bit FID identifying this open.</param>
        /// <param name="opLock">An element indicating the type of OpLock, if any, that has been granted on this open.
        /// This value MUST be one of None, Exclusive, Batch, or Level II.</param>
        /// <param name="grantedAccess">The access granted on this open.</param>
        /// <param name="fileGlobalId">A numeric value obtained by registration with the Server Service Remote Protocol.
        /// </param>
        /// <param name="pid">The unique (per connection) 32-bit PID provided in the client request that created this
        /// open.</param>
        public CifsServerPerOpenFile(
            IFileServiceServerTreeConnect treeConnect,
            string pathName,
            long fileId,
            uint grantedAccess,
            OplockLevelValue opLock,
            int fileGlobalId,
            uint pid)
        {
            this.treeConnect = treeConnect;
            this.pathName = pathName;
            this.fileId = fileId;
            this.fileGlobalId = fileGlobalId;
            this.grantedAccess = grantedAccess;
            this.opLock = opLock;
            this.pid = pid;
            this.locks = new Dictionary<int, object>();
        }

        #endregion
    }
}