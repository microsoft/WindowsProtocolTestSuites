// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// The bit mapped values that define the properties of the direction of trust between the local domain
    /// and the named domain. One or more of the valid flags can be set. If all bits are 0, the trust is said
    /// to be disabled.
    /// </summary>
    [Flags]
    public enum LsaTrustDirection : int
    {
        /// <summary>
        /// The trust is inbound
        /// </summary>
        Inbound = 0x00000001,

        /// <summary>
        /// The trust is outbound
        /// </summary>
        Outbound = 0x00000002,
    }
}
