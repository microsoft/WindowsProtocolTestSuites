// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// Different kinds of data contained in NETLOGON_CONTROL_DATA_INFORMATION.
    /// </summary>
    public enum NetlogonControlDataInformationType
    {
        /// <summary>
        /// It is Null.
        /// </summary>
        Null,

        /// <summary>
        /// No valid domain name contained in the trust list of NETLOGON_CONTROL_DATA_INFORMATION.
        /// </summary>
        NoValidDomainNameContained,

        /// <summary>
        /// Valid data contained in NETLOGON_CONTROL_DATA_INFORMATION.
        /// </summary>
        Valid,
    }
}
