// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// The domain GUID.
    /// </summary>
    public enum DomainGuidType
    {
        /// <summary>
        /// It is NULL.
        /// </summary>
        Null,

        /// <summary>
        /// A domain with this GUID does not exist.
        /// </summary>
        NonExistDomainGuid,

        /// <summary>
        /// The GUID of the primary domain.
        /// </summary>
        PrimaryDomainGuid,

        /// <summary>
        /// The GUID of the trusted domain.
        /// </summary>
        TrustedDomainGuid,
    }
}