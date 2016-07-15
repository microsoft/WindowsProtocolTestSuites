// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A set of bit flags that specify delivery settings. 
    /// A flag is TRUE (or set) if its value is equal to 1. 
    /// Output flags MUST be the same as input. 
    /// The value is constructed from zero or more bit flags from the following table.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NrpcNetrLogonSamLogonExtraFlags : uint
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// A: Request MUST be passed to the domain controller at the root of the forest.
        /// </summary>
        RequestAtTheRootOfForest = 1,

        /// <summary>
        /// B: Request MUST be passed to the DC at the end of the first hop over a cross-forest trust.
        /// </summary>
        RequestAtTheEndOfFirstHop = 2,

        /// <summary>
        /// C: Request was passed by an RODC to a DC in a different domain.
        /// </summary>
        RequestByRodcToDifferentDomain = 4,

        /// <summary>
        /// D: Request is an NTLM authentication package request passed by an RODC.
        /// </summary>
        RequestIsNtlmByRodc = 8,
    }
}
