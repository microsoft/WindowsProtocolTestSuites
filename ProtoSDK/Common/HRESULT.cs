// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// The HRESULT numbering space is vendor-extensible.
    /// Vendors can supply their own values for this field, as long as the C bit (0x20000000) is set, indicating it is a customer code.
    /// </summary>
    [Flags]
    public enum HRESULT : UInt32
    {
        /// <summary>
        /// S (1 bit): Severity. If set, indicates a failure result. If clear, indicates a success result.
        /// </summary>
        S = 0x8000000,

        /// <summary>
        /// R (1 bit): Reserved. If the N bit is clear, this bit MUST be set to 0. If the N bit is set, this bit is defined by the NTSTATUS numbering space.
        /// </summary>
        R = 0x4000000,

        /// <summary>
        /// C (1 bit): Customer. This bit specifies if the value is customer-defined or Microsoft-defined. The bit is set for customer-defined values and clear for Microsoft-defined values.
        /// </summary>
        C = 0x2000000,

        /// <summary>
        /// N (1 bit): If set, indicates that the error code is an NTSTATUS value, except that this bit is set.
        /// </summary>
        N = 0x1000000,

        /// <summary>
        /// X (1 bit):  Reserved.  SHOULD be set to 0.
        /// </summary>
        X = 0x0800000

    }
}
