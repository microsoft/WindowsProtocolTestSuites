// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2CreateDurableHandleRequestV2 : Smb2CreateContextRequest
    {
        public uint Timeout;

        public CREATE_DURABLE_HANDLE_REQUEST_V2_Flags Flags;

        public Guid CreateGuid;
    }
}
