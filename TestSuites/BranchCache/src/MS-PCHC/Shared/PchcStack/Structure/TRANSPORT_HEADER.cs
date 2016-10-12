// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    /// <summary>
    /// The transport adds the TRANSPORT_HEADER in front of any response protocol message.
    /// </summary>
    public struct TRANSPORT_HEADER
    {
        /// <summary>
        /// Total message size in bytes, excluding this field [TRANSPORT_HEADER].
        /// </summary>
        public uint Size;
    }
}
