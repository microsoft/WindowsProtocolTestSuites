// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Conflict
{
    /// <summary>
    /// Type of the possible conflicting request
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// SMB2 Lock request, the SMB2_LOCKFLAG_EXCLUSIVE_LOCK flag is set
        /// </summary>
        ExclusiveLock,

        /// <summary>
        /// SMB2 Create request with lease v1 context
        /// </summary>
        Lease,

        /// <summary>
        /// [MS-SMB2] 2.2.13 
        /// FILE_DELETE_ON_CLOSE  The file MUST be automatically deleted when the last open request on this file is closed. 
        /// The enum value is used in below scenario:
        /// Create two opens for the two clients, and FILE_DELETE_ON_CLOSE flag is set for the first SMB2 Create request 
        /// Then the first client closes the open. The file will be deleted after the second client closes the open.
        /// </summary>
        UncommitedDelete,

        /// <summary>
        /// The enum value is used in below scenario:
        /// Create one open for the first client, and FILE_DELETE_ON_CLOSE flag is set.
        /// Then the first client closes the open. The file will be deleted.
        /// </summary>
        Delete,

        /// <summary>
        /// SMB2 Write request
        /// </summary>
        Write,

        /// <summary>
        /// SMB2 Read request
        /// </summary>
        Read
    }

    /// <summary>
    /// The current state of the specified file.
    /// </summary>
    public enum FileState
    {
        /// <summary>
        /// Initial state
        /// </summary>
        Initial,

        /// <summary>
        /// Indicates the file is locked.
        /// </summary>
        Locked,

        /// <summary>
        /// A lease to this file is granted to one client.
        /// </summary>
        LeaseGranted,

        /// <summary>
        /// The file will be deleted after the last open on this file is closed
        /// </summary>
        ToBeDeleted,

        /// <summary>
        /// The file is deleted
        /// </summary>
        Deleted,
    }

    /// <summary>
    /// Indicates if the lease break notification is received.
    /// </summary>
    public enum LeaseBreakState
    {
        /// <summary>
        /// Indicates no lease break notification is received.
        /// </summary>
        NoLeaseBreak,

        /// <summary>
        /// Indicates lease break notification is received.
        /// </summary>
        LeaseBreakExisted,
    }
}
