// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    public class Smb2CreateAppInstanceVersion : Smb2CreateContextRequest
    {
        /// <summary>
        /// The client MUST set this field to 24, indicating the size of this structure.
        /// </summary>
        public ushort StructureSize;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. This field MUST be set to zero.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// An unsigned 64 bit integer containing the most significant value of the version.
        /// </summary>
        public UInt64 AppInstanceVersionHigh;

        /// <summary>
        /// An unsigned 64 bit integer containing the least significant value of the version.
        /// </summary>
        public UInt64 AppInstanceVersionLow;
    }
}
