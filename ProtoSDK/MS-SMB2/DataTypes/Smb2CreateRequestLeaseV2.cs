// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2CreateRequestLeaseV2 : Smb2CreateContextRequest
    {
        public Guid LeaseKey;

        public LeaseStateValues LeaseState;

        public uint LeaseFlags;

        public ulong LeaseDuration;

        public Guid ParentLeaseKey;

        public uint Epoch;
    }
}
