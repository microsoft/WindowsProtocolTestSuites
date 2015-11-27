// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.MixedOplockLease
{
    /// <summary>
    /// Indicates if there is oplock/lease break
    /// </summary>
    public enum ModelBreakType
    {
        /// <summary>
        /// No lease or oplock break
        /// </summary>
        NoBreak,

        /// <summary>
        /// Indicates there is an oplock break
        /// </summary>
        OplockBreak,

        /// <summary>
        /// Indicates there is a lease break
        /// </summary>
        LeaseBreak,
    }

    /// <summary>
    /// Type of different LeaseState field in the lease context
    /// The test case uses the same four combinations with windows client.
    /// </summary>
    public enum ModelLeaseStateType
    {
        /// <summary>
        /// Initial state
        /// </summary>
        Lease_None,

        /// <summary>
        /// SMB2_LEASE_READ_CACHING
        /// </summary>
        Lease_R,

        /// <summary>
        /// SMB2_LEASE_READ_CACHING | SMB2_LEASE_HANDLE_CACHING
        /// </summary>
        Lease_RH,

        /// <summary>
        /// SMB2_LEASE_READ_CACHING | SMB2_LEASE_WRITE_CACHING
        /// </summary>
        Lease_RW,

        /// <summary>
        /// SMB2_LEASE_READ_CACHING | SMB2_LEASE_WRITE_CACHING | SMB2_LEASE_HANDLE_CACHING
        /// </summary>
        Lease_RWH
    }
}
