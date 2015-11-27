// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2CreateDurableHandleReconnectV2 : Smb2CreateContextRequest
    {
        public FILEID FileId;

        public Guid CreateGuid;

        public CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags Flags;
    }
}
