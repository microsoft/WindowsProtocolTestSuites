// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// The RID.
    /// </summary>
    public enum RidType
    {
        /// <summary>
        /// RID that does not exist.
        /// </summary>
        NonExist,

        /// <summary>
        /// The RID of a non-machine account.
        /// </summary>
        RidOfNonMachineAccount,

        /// <summary>
        /// The RID of the machine account of the non-DC computer in the domain.
        /// </summary>
        RidOfNonDcMachineAccount,
    }
}
