// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// A structure returned by the KDC to provide extended error information.
    /// </summary>
    public struct KERB_EXT_ERROR
    {
        /// <summary>
        /// An NTSTATUS value.
        /// </summary>
        public UInt32 status;

        /// <summary>
        /// Set to zero and MUST be ignored on receipt.
        /// </summary>
        public UInt32 reserved;

        /// <summary>
        /// Set to 0x00000001.
        /// </summary>
        public UInt32 flags;
    }
}
