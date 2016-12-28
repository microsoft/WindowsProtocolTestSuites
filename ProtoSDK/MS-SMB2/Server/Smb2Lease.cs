// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// A structure contains information about lease
    /// </summary>
    public class Smb2Lease
    {
        internal Guid leaseKey;
        internal string fileName;
        internal LeaseStateValues leaseState;
        internal LeaseStateValues breakToLeaseState;
        internal TimeSpan leaseBreakTimeout;
        internal Dictionary<FILEID, Smb2ServerOpen> leaseOpens;
        internal bool breaking;

        /// <summary>
        /// A global identifier for this lease.
        /// </summary>
        public Guid LeaseKey
        {
            get
            {
                return leaseKey;
            }
        }

        /// <summary>
        /// The name of the file backing this lease.
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        /// <summary>
        /// The current state of the lease as indicated by the underlying object store.
        /// </summary>
        public LeaseStateValues LeaseState
        {
            get
            {
                return leaseState;
            }
        }

        /// <summary>
        /// The state to which the lease is breaking.
        /// </summary>
        public LeaseStateValues BreakToLeaseState
        {
            get
            {
                return breakToLeaseState;
            }
        }

        /// <summary>
        /// The time-out value that indicates when a lease that is breaking and has not received a Lease Break Acknowledgment
        /// from the client will be acknowledged by the server to the underlying object store.
        /// </summary>
        public TimeSpan LeaseBreakTimeout
        {
            get
            {
                return leaseBreakTimeout;
            }
        }

        /// <summary>
        /// The list of opens associated with this lease.
        /// </summary>
        public ReadOnlyDictionary<FILEID, Smb2ServerOpen> LeaseOpens
        {
            get
            {
                if (leaseOpens == null)
                {
                    return null;
                }
                else
                {
                    return new ReadOnlyDictionary<FILEID, Smb2ServerOpen>(leaseOpens);
                }
            }
        }

        /// <summary>
        /// A Boolean that indicates if a lease break is in progress.
        /// </summary>
        public bool Breaking
        {
            get
            {
                return breaking;
            }
        }
    }
}
