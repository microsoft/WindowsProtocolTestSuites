// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// base update for LDAP related updates
    /// </summary>
    public abstract class LdapBaseUpdate
    {
        /// <summary>
        /// ldap adapter
        /// </summary>
        protected static ILdapAdapter ldapAdapter;

        /// <summary>
        /// set ldap adapter
        /// </summary>
        public static void SetLdapAdapter(ILdapAdapter ad)
        {
            ldapAdapter = ad;
        }
    }
}
