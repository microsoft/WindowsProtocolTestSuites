// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class FsccFileFullEaInformation
    {
        public FileFullEaInformation_Flags_Values Flags;

        public string EaName;

        public byte[] EaValue;
    }
}
