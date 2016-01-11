// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// LDAP connection types
    /// </summary>
    /// Suppress EnumsShouldHaveZeroValue warning.
    [SuppressMessage("Microsoft.Design","CA1008:EnumsShouldHaveZeroValue")]
    public enum AdtsLdapConnectionType
    {
        /// <summary>
        /// Tcp connection
        /// </summary>
        Tcp = 1,

        /// <summary>
        /// Udp connection
        /// </summary>
        Udp = 2,

        /// <summary>
        /// TLS/SSL connection
        /// </summary>
        TlsOrSsl = 3
    }
}
