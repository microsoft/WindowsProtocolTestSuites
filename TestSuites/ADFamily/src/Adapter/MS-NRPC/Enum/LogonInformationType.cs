// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// LogonInformation used in NetrLogonSamLogonEx,NetrLogonSamLogonWithFlags
    /// and NetrLogonSamLogon.
    /// </summary>
    public enum LogonInformationType
    {
        /// <summary>
        /// It is Null.
        /// </summary>
        Null,

        /// <summary>
        /// Valid logon information.
        /// </summary>
        Valid,
    }
}
