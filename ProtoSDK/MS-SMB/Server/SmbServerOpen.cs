// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// open of smb server
    /// </summary>
    public class SmbServerOpen : IFileServiceServerOpen
    {
        #region Fields of TD

        /// <summary>
        /// A reference to the authenticated session, as specified in section 3.3.1.9 on which this file or pipe was 
        /// opened.
        /// </summary>
        private SmbServerSession session;

        /// <summary>
        /// The unique 16-bit SMB Fid identifying this Open, as described in [CIFS] section 4.2.1. The FID MUST be 
        /// unique on this connection.
        /// </summary>
        private ushort smbFid;

        /// <summary>
        /// A reference to the TreeConnect, as specified in section 3.3.1.12, over which this open was performed. If 
        /// the file is not attached to a TreeConnect at this time, its value MUST be null.
        /// </summary>
        private SmbServerTreeConnect treeConnect;

        /// <summary>
        /// A numeric value that indicates the number of locks that are held by a current Open.
        /// </summary>
        private int lockCount;

        /// <summary>
        /// A variable-length string that contains the Unicode path name that the Open is performed.
        /// </summary>
        private string pathName;

        /// <summary>
        /// A variable-length string that contains the Unicode path name that the Open is performed.
        /// </summary>
        private NtTransactDesiredAccess grantedAccess;

        #endregion

        #region Properties of TD

        /// <summary>
        /// A reference to the authenticated session, as specified in section 3.3.1.9 on which this file or pipe was 
        /// opened.
        /// </summary>
        public SmbServerSession Session
        {
            get
            {
                return this.session;
            }
            set
            {
                this.session = value;
            }
        }


        /// <summary>
        /// The unique 16-bit SMB Fid identifying this Open, as described in [CIFS] section 4.2.1. The FID MUST be 
        /// unique on this connection.
        /// </summary>
        public ushort SmbFid
        {
            get
            {
                return this.smbFid;
            }
            set
            {
                this.smbFid = value;
            }
        }


        /// <summary>
        /// The unique 16-bit SMB Fid identifying this Open, as described in [CIFS] section 4.2.1. The FID MUST be 
        /// unique on this connection.
        /// </summary>
        public long FileId
        {
            get
            {
                return this.smbFid;
            }
            set
            {
                this.smbFid = (ushort)value;
            }
        }


        /// <summary>
        /// A reference to the TreeConnect, as specified in section 3.3.1.12, over which this open was performed. If 
        /// the file is not attached to a TreeConnect at this time, its value MUST be null.
        /// </summary>
        public SmbServerTreeConnect TreeConnect
        {
            get
            {
                return this.treeConnect;
            }
            set
            {
                this.treeConnect = value;
            }
        }


        /// <summary>
        /// A numeric value that indicates the number of locks that are held by a current Open.
        /// </summary>
        public int LockCount
        {
            get
            {
                return this.lockCount;
            }
            set
            {
                this.lockCount = value;
            }
        }


        /// <summary>
        /// A variable-length string that contains the Unicode path name that the Open is performed.
        /// </summary>
        public string PathName
        {
            get
            {
                return this.pathName;
            }
            set
            {
                this.pathName = value;
            }
        }


        /// <summary>
        /// A variable-length string that contains the Unicode path name that the Open is performed.
        /// </summary>
        public NtTransactDesiredAccess GrantedAccess
        {
            get
            {
                return this.grantedAccess;
            }
            set
            {
                this.grantedAccess = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal SmbServerOpen()
        {
            this.grantedAccess = (NtTransactDesiredAccess)0x00;
        }


        #endregion


        /// <summary>
        /// Inherit from FileServiceServerOpen, equals to TreeConnect.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        IFileServiceServerTreeConnect IFileServiceServerOpen.TreeConnect
        {
            get
            {
                return this.treeConnect;
            }
        }
    }
}
