// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;


namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// The enum of FSMO roles.
    /// </summary>
    [Flags]
    public enum FSMORoles : byte
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0x00,

        /// <summary>
        /// The Domain Naming Master Role
        /// </summary>
        DomainNaming = 0x01,

        /// <summary>
        /// The Schema Master Role
        /// </summary>
        Schema = 0x02,

        /// <summary>
        /// The infrastructure Master Role
        /// </summary>
        Infrastructure = 0x04,

        /// <summary>
        /// The Rid Allocation Master Role
        /// </summary>
        RidAllocation = 0x08,

        /// <summary>
        /// The PDC master role
        /// </summary>
        PDC = 0x10
    }
}
