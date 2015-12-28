// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2ServerTreeConnect
    {
        public uint TreeId;

        public Smb2ServerSession Session;

        public Smb2ServerShare Share;

        public uint OpenCount;

        public uint TreeGlobalId;

        // CreationTime
    }
}
