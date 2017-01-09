// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// The role of the computer in a domain.
    /// </summary>
    public enum DomainComputerRoleType
    {
        /// <summary>
        /// The computer is a primary domain controller.
        /// </summary>
        Pdc,

        /// <summary>
        /// The computer is a backup domain controller.
        /// </summary>
        Bdc,

        /// <summary>
        /// The computer is a read-only domain controller.
        /// </summary>
        Rodc,

        /// <summary>
        /// The computer is a normal domain member, not a DC.
        /// </summary>
        NormalClient,
    }
}
