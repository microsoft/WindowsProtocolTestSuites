// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// A local resource that is offered by an SMB 2.0 Protocol server for
    /// access by SMB 2.0 Protocol clients over the network. The SMB 2.0 
    /// Protocol defines three types of shares: file (or disk) shares, 
    /// which represent a directory tree and its included files; pipe 
    /// shares, which expose access to named pipes; and print shares, 
    /// which provide access to print resources on the server. A pipe 
    /// share as defined by the SMB 2.0 Protocol MUST always have the 
    /// name "IPC$". A pipe share MUST only allow named pipe operations
    /// and DFS referral requests to itself.
    /// </summary>
    public class Share
    {
        #region fields

        private int globalIndex;
        private string name;
        private string localPath;
        private long connectSecurity;
        private long fileSecurity;
        private long cscFlags;
        private bool isDfs;
        private bool doAccessBasedDirectoryEnumeration;
        private bool allowNamespaceCaching;
        private bool forceSharedDelete;
        private bool restrictExclusiveOpens;

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
        /// A unique name for the shared resource on this server.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }


        /// <summary>
        /// A path that describes the local resource that is being shared. This MUST be a store that either
        /// provides named pipe functionality, or that offers storage and/or retrieval of files. In the case 
        /// of the latter, it MAY be a device that accepts a file and then processes it in some format, such 
        /// as a printer.
        /// </summary>
        public string LocalPath
        {
            get
            {
                return this.localPath;
            }
            set
            {
                this.localPath = value;
            }
        }


        /// <summary>
        /// An authorization policy such as an access control list that describes which users are allowed
        /// to connect to this share.
        /// </summary>
        public long ConnectSecurity
        {
            get
            {
                return this.connectSecurity;
            }
            set
            {
                this.connectSecurity = value;
            }
        }


        /// <summary>
        /// An authorization policy such as an access control list that describes what actions users that
        /// connect to this share are allowed to perform on the shared resource.
        /// </summary>
        public long FileSecurity
        {
            get
            {
                return this.fileSecurity;
            }
            set
            {
                this.fileSecurity = value;
            }
        }


        /// <summary>
        /// The configured offline caching policy for this share. This value MUST be manual caching, 
        /// automatic caching of files, automatic caching of files and programs, or no offline caching. 
        /// For more information, see section 2.2.10. For more information about offline caching, see [OFFLINE].
        /// </summary>
        public long CscFlags
        {
            get
            {
                return this.cscFlags;
            }
            set
            {
                this.cscFlags = value;
            }
        }


        /// <summary>
        /// A Boolean that, if set, indicates that this share is configured for DFS. For more information, 
        /// see [MSDFS].
        /// </summary>
        public bool IsDfs
        {
            get
            {
                return this.isDfs;
            }
            set
            {
                this.isDfs = value;
            }
        }


        /// <summary>
        /// A Boolean that, if set, indicates that the results of directory enumerations on this share
        /// must be trimmed to include only the files and directories that the calling user has access to.
        /// </summary>
        public bool DoAccessBasedDirectoryEnumeration
        {
            get
            {
                return this.doAccessBasedDirectoryEnumeration;
            }
            set
            {
                this.doAccessBasedDirectoryEnumeration = value;
            }
        }


        /// <summary>
        /// A Boolean that, if set, indicates that clients are allowed to cache directory enumeration 
        /// results for better performance.
        /// </summary>
        public bool AllowNamespaceCaching
        {
            get
            {
                return this.allowNamespaceCaching;
            }
            set
            {
                this.allowNamespaceCaching = value;
            }
        }


        /// <summary>
        /// A Boolean that, if set, indicates that all opens on this share MUST include FILE_SHARE_DELETE
        /// in the sharing access.
        /// </summary>
        public bool ForceSharedDelete
        {
            get
            {
                return this.forceSharedDelete;
            }
            set
            {
                this.forceSharedDelete = value;
            }
        }


        /// <summary>
        /// A Boolean that, if set, indicates that users who request read-only access to a file 
        /// are not allowed to deny other readers.
        /// </summary>
        public bool RestrictExclusiveOpens
        {
            get
            {
                return this.restrictExclusiveOpens;
            }
            set
            {
                this.restrictExclusiveOpens = value;
            }
        }

        #endregion


        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Share()
        {
            this.GlobalIndex = 0;
            this.Name = "";
            this.LocalPath = "";
            this.ConnectSecurity = 0;
            this.FileSecurity = 0;
            this.CscFlags = 0;
            this.IsDfs = false;
            this.DoAccessBasedDirectoryEnumeration = false;
            this.AllowNamespaceCaching = false;
            this.ForceSharedDelete = false;
            this.RestrictExclusiveOpens = false;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public Share(Share share)
        {
            if (share != null)
            {
                this.GlobalIndex = share.GlobalIndex;
                this.Name = share.Name;
                this.LocalPath = share.LocalPath;
                this.ConnectSecurity = share.ConnectSecurity;
                this.FileSecurity = share.FileSecurity;
                this.CscFlags = share.CscFlags;
                this.IsDfs = share.IsDfs;
                this.DoAccessBasedDirectoryEnumeration = share.DoAccessBasedDirectoryEnumeration;
                this.AllowNamespaceCaching = share.AllowNamespaceCaching;
                this.ForceSharedDelete = share.ForceSharedDelete;
                this.RestrictExclusiveOpens = share.RestrictExclusiveOpens;
            }
            else
            {
                this.GlobalIndex = 0;
                this.Name = "";
                this.LocalPath = "";
                this.ConnectSecurity = 0;
                this.FileSecurity = 0;
                this.CscFlags = 0;
                this.IsDfs = false;
                this.DoAccessBasedDirectoryEnumeration = false;
                this.AllowNamespaceCaching = false;
                this.ForceSharedDelete = false;
                this.RestrictExclusiveOpens = false;
            }
        }

        #endregion
    }
}