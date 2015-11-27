// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Modeling;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2ServerLeaseTable
    {
        public Guid ClientGuid;

        public MapContainer<SequenceContainer<byte>, Smb2ServerLease> LeaseList;
    }
}
