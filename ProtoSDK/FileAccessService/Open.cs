// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// A runtime object that corresponds to a currently established access 
    /// to a specific file or named pipe from a specific client to a specific
    /// server, using a specific user security context. Both clients and 
    /// servers maintain opens that represent active accesses.
    /// </summary>
    public class Open
    {
        #region fields

        private int globalIndex;
        private ulong fileIdPersistent;
        private ulong fileIdVolatile;
        private ulong treeConnectId;
        private ulong sessionId;
        private int connectionId;
        private ulong localOpen;
        private ulong grantedAccess;
        private byte oplockLevel;
        private ulong oplockState;
        private ulong oplockTimeout;
        private bool isDurable;
        private ulong durableOpenTimeout;
        private ulong durableOwner;
        private string enumerationLocation;
        private string enumerationSearchPattern;

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
        /// A numeric value that uniquely identifies the open handle to a file or a pipe within the scope 
        /// of a session over which the handle was opened. A 64-bit representation of this value, combined 
        /// with Open.DurableFileId as described below, combine to form the SMB2_FILEID described in 
        /// section 2.2.14.1. This value is the volatile portion of the identifier.
        /// </summary>
        public ulong Persistent
        {
            get
            {
                return this.fileIdPersistent;
            }
            set
            {
                this.fileIdPersistent = value;
            }
        }


        /// <summary>
        ///  A numeric value that uniquely identifies the open handle to a file or a pipe within the scope 
        ///  of all opens granted by the server, as described by the GlobalOpenTable. A 64-bit representation 
        ///  of this value combined with Open.FileId, as described above, form the SMB2_FILEID described 
        ///  in section 2.2.14.1. This value is the persistent portion of the identifier.
        /// </summary>
        public ulong Volatile
        {
            get
            {
                return this.fileIdVolatile;
            }
            set
            {
                this.fileIdVolatile = value;
            }
        }


        /// <summary>
        /// A reference to the treeconnect.
        /// </summary>
        public ulong TreeConnectId
        {
            get
            {
                return this.treeConnectId;
            }
            set
            {
                this.treeConnectId = value;
            }
        }


        /// <summary>
        /// A reference to the authenticated session, as specified in section 3.3.1.8, over which this open 
        /// was performed. If the file is not attached to a session at this time, its value MUST be 0.
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
        /// A reference to the connection, as specified in section 3.3.1.7, that created this open. If the 
        /// file is not attached to a connection at this time, this value MUST be 0.
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
        /// An open of a file or named pipe in the underlying local resource that is used to perform the 
        /// local operations, such as reading or writing, to the underlying object.
        /// </summary>
        public ulong LocalOpen
        {
            get
            {
                return this.localOpen;
            }
            set
            {
                this.localOpen = value;
            }
        }


        /// <summary>
        /// The access granted on this open, as defined in section 2.2.13.1.
        /// </summary>
        /// 
        public ulong GrantedAccess
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


        /// <summary>
        /// The current oplock level for this open. This value MUST be either None, Level2, 
        /// Exclusive, Batch, or Directory.
        /// </summary>
        public byte OplockLevel
        {
            get
            {
                return this.oplockLevel;
            }
            set
            {
                this.oplockLevel = value;
            }
        }


        /// <summary>
        /// The current oplock state of the file. This value MUST be Held, Breaking, or None.
        /// </summary>
        public ulong OplockState
        {
            get
            {
                return this.oplockState;
            }
            set
            {
                this.oplockState = value;
            }
        }


        /// <summary>
        /// The time-out value that indicates when an oplock that is breaking and has not received 
        /// an acknowledgment from the client will be acknowledged by the server.
        /// </summary>
        public ulong OplockTimeout
        {
            get
            {
                return this.oplockTimeout;
            }
            set
            {
                this.oplockTimeout = value;
            }
        }


        /// <summary>
        /// A Boolean that indicates whether this open has requested durable operation.
        /// </summary>
        public bool IsDurable
        {
            get { return this.isDurable; }
            set { this.isDurable = value; }
        }


        /// <summary>
        /// A time-out value that indicates when a handle that has been preserved for durability 
        /// will be closed by the system if a client has not reclaimed it.
        /// </summary>
        public ulong DurableOpenTimeout
        {
            get
            {
                return this.durableOpenTimeout;
            }
            set
            {
                this.durableOpenTimeout = value;
            }
        }


        /// <summary>
        /// A security descriptor that holds the original opener of the file. This allows the server 
        /// to determine if a caller that is trying to reestablish a durable open is allowed to do so.
        /// </summary>
        public ulong DurableOwner
        {
            get
            {
                return this.durableOwner;
            }
            set
            {
                this.durableOwner = value;
            }
        }


        /// <summary>
        /// For directories, this value indicates the current location in a directory enumeration and 
        /// allows for the continuing of an enumeration across multiple requests. For files, this value
        /// is unused.
        /// </summary>
        public string EnumerationLocation
        {
            get
            {
                return this.enumerationLocation;
            }
            set
            {
                this.enumerationLocation = value;
            }
        }


        /// <summary>
        /// For directories, this value holds the search pattern that is used in directory enumeration 
        /// and allows for the continuing of an enumeration across multiple requests. For files, this 
        /// value is unused.
        /// </summary>
        public string EnumerationSearchPattern
        {
            get
            {
                return this.enumerationSearchPattern;
            }
            set
            {
                this.enumerationSearchPattern = value;
            }
        }

        #endregion


        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Open()
        {
            this.GlobalIndex = 0;
            this.Persistent = 0;
            this.Volatile = 0;
            this.TreeConnectId = 0;
            this.SessionId = 0;
            this.ConnectionId = 0;
            this.LocalOpen = 0;
            this.GrantedAccess = 0;
            this.OplockLevel = 0;
            this.OplockState = 0;
            this.OplockTimeout = 0;
            this.IsDurable = false;
            this.DurableOpenTimeout = 0;
            this.DurableOwner = 0;
            this.EnumerationLocation = "";
            this.EnumerationSearchPattern = "";
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public Open(Open open)
        {
            if (open != null)
            {
                this.GlobalIndex = open.GlobalIndex;
                this.Persistent = open.Persistent;
                this.Volatile = open.Volatile;
                this.TreeConnectId = open.TreeConnectId;
                this.SessionId = open.SessionId;
                this.ConnectionId = open.ConnectionId;
                this.LocalOpen = open.LocalOpen;
                this.GrantedAccess = open.GrantedAccess;
                this.OplockLevel = open.OplockLevel;
                this.OplockState = open.OplockState;
                this.OplockTimeout = open.OplockTimeout;
                this.IsDurable = open.IsDurable;
                this.DurableOpenTimeout = open.DurableOpenTimeout;
                this.DurableOwner = open.DurableOwner;
                this.EnumerationLocation = open.EnumerationLocation;
                this.EnumerationSearchPattern = open.EnumerationSearchPattern;
            }
            else
            {
                this.GlobalIndex = 0;
                this.Persistent = 0;
                this.Volatile = 0;
                this.TreeConnectId = 0;
                this.SessionId = 0;
                this.ConnectionId = 0;
                this.LocalOpen = 0;
                this.GrantedAccess = 0;
                this.OplockLevel = 0;
                this.OplockState = 0;
                this.OplockTimeout = 0;
                this.IsDurable = false;
                this.DurableOpenTimeout = 0;
                this.DurableOwner = 0;
                this.EnumerationLocation = "";
                this.EnumerationSearchPattern = "";
            }
        }

        #endregion
    }
}