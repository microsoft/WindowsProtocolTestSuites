// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2ServerLeaseTable
    {
        public Guid ClientGuid;

        public Dictionary<List<byte>, Smb2ServerLease> LeaseList;
    }
}
