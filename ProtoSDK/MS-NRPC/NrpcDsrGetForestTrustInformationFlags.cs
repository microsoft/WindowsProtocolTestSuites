// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A set of bit flags that specify additional applications for the forest trust information. 
    /// A flag is TRUE (or set) if its value is equal to 1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NrpcDsrGetForestTrustInformationFlags : uint
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// A: Update a trusted domain object (TDO) with the information returned in ForestTrustInfo.
        /// </summary>
        UpdateTdo = 1
    }
}
