// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2CreateDurableHandleResponseV2 : Smb2CreateContextResponse
    {
        public uint Timeout;

        public CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags Flags;
    }
}
