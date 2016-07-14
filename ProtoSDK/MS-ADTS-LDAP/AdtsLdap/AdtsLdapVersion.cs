// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// LDAP Versions
    /// </summary>
    /// Suppress EnumsShouldHaveZeroValue warning.
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum AdtsLdapVersion
    {
        /// <summary>
        /// LDAP Version 2, as specified in [RFC 1777]
        /// </summary>
        V2 = 2,

        /// <summary>
        /// LDAP Version 3, as specified in [RFC 2251]
        /// </summary>
        V3 = 3
    }
}
