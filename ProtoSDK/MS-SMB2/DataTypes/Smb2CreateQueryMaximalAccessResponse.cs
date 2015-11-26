// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2CreateQueryMaximalAccessResponse : Smb2CreateContextResponse
    {
        public uint QueryStatus;

        public _ACCESS_MASK MaximalAccess;
    }
}
