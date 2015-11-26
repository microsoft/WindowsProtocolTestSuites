// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// NDR version enumeration.
    /// </summary>
    [Flags]
    public enum RpceNdrVersion
    {
        /// <summary>
        /// None NDR version is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// NDR
        /// </summary>
        NDR = 1,

        /// <summary>
        /// NDR64
        /// </summary>
        NDR64 = 2
    }
}
