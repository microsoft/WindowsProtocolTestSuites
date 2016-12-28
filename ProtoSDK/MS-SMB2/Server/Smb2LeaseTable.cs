// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// A structure contains information about LeaseTable
    /// </summary>
    public class Smb2LeaseTable
    {
        internal Guid clientGuid;
        internal Dictionary<Guid, Smb2Lease> leaseList;

        /// <summary>
        /// A global identifier to associate which connections MUST use this LeaseTable
        /// </summary>
        public Guid ClientGuid
        {
            get
            {
                return clientGuid;
            }
        }

        /// <summary>
        /// A list of lease structures, as defined in section 3.3.1.13, indexed by LeaseKey.
        /// </summary>
        public ReadOnlyDictionary<Guid, Smb2Lease> LeaseList
        {
            get
            {
                if (leaseList == null)
                {
                    return null;
                }
                else
                {
                    return new ReadOnlyDictionary<Guid, Smb2Lease>(leaseList);
                }
            }
        }
    }
}
