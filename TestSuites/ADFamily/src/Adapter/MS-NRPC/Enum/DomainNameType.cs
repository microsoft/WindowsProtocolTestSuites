// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// Domain name.
    /// </summary>
    public enum DomainNameType
    {
        /// <summary>
        /// It is NULL.
        /// </summary>
        Null,

        /// <summary>
        /// The first character of the domain name is the null-terminator character.
        /// </summary>
        EmptyDomainName,

        /// <summary>
        /// The domain with this name, which is in NetBIOS format, does not exist.
        /// </summary>
        NonExistDomainName,

        /// <summary>
        /// The domain name is neither in the NetBIOS format nor in the FQDN format.
        /// </summary>
        InvalidFormatDomainName,

        /// <summary>
        /// The NetBIOS format name of the primary domain.
        /// </summary>
        NetBiosFormatDomainName,

        /// <summary>
        /// The FQDN format name of the primary domain.
        /// </summary>
        FqdnFormatDomainName,

        /// <summary>
        /// The FQDN format name of the trusted domain.
        /// </summary>
        TrustedDomainName,
    }
}
