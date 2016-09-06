// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A set of bit flags that list properties of the AccountName account. 
    /// A flag is TRUE (or set) if its value is equal to 1. If the flag is set, 
    /// then the account MUST have that property; otherwise, the property is ignored.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NrpcAllowableAccountControlBits : uint
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// A:<para/>
        /// Account for users whose primary account is in another domain. 
        /// This account provides user access to the domain, but not to 
        /// any domain that trusts the domain.
        /// </summary>
        PrimaryAccountIsInAnotherDomain = 0x100,

        /// <summary>
        /// B:<para/>
        /// Normal domain user account.
        /// </summary>
        NormalDomainUserAccount = 0x200,

        /// <summary>
        /// C:<para/>
        /// Interdomain trust account.
        /// </summary>
        InterdomainTrustAccount = 0x800,

        /// <summary>
        /// D:<para/>
        /// Computer account for a domain member.
        /// </summary>
        ComputerAccount = 0x1000,

        /// <summary>
        /// E:<para/>
        /// Computer account for a BDC.
        /// </summary>
        BdcComputerAccount = 0x2000,

        /// <summary>
        /// F:<para/>
        /// Computer account for an RODC.
        /// </summary>
        RodcComputerAccount = 0x2000000,
    }
}
