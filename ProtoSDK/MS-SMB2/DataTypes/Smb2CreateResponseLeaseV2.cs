// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2CreateResponseLeaseV2 : Smb2CreateContextResponse
    {
        public Guid LeaseKey;

        public LeaseStateValues LeaseState;

        public LeaseFlagsValues Flags;

        public ulong LeaseDuration;

        public Guid ParentLeaseKey;

        public ushort Epoch;

        public ushort Reserved;
    }
}
