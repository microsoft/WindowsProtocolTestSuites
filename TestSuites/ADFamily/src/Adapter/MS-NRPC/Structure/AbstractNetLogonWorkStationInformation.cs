// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// Abstraction of union NETLOGON_WORKSTATION_INFORMATION.
    /// </summary>
    public struct AbstractNetLogonWorkStationInformation
    {
        /// <summary>
        /// WorkStation information. Only used when the level parameter in NetrLogonGetDomainInfo is 1.
        /// </summary>
        public AbstractNetLogonWorkStationInfo WorkStationInfo;

        /// <summary>
        /// Whether the LsaPolicyInfo is NULL. Only used when the level parameter in NetrLogonGetDomainInfo is 2.
        /// </summary>
        public bool IsLsaPolicyInfoNull;
    }
}
