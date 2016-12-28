// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// Account name.
    /// </summary>
    public enum AccounterNameType
    {
        /// <summary>
        /// Account name is null.
        /// </summary>
        Null,

        /// <summary>
        /// Invalid account.
        /// </summary>
        InvalidAccount,

        /// <summary>
        /// Account for users whose primary account is in another domain. 
        /// This account provides user the access to the domain, but not to any domain that trusts the domain.
        /// </summary>
        AnotherDomainUserAccount,

        /// <summary>
        /// Normal domain user account.
        /// </summary>
        NormalDomainUserAccount,

        /// <summary>
        /// Computer account for a domain member.
        /// </summary>
        DomainMemberComputerAccount,

        /// <summary>
        /// Computer account for a domain member end with a period
        /// </summary>
        DomainMemberComputerAccountEndWithPeriod,

        /// <summary>
        /// Computer account for a BDC.
        /// </summary>
        BdcComputerAccount,

        /// <summary>
        /// Computer account for an RODC.
        /// </summary>
        RodcComputerAccount,
    }
}
