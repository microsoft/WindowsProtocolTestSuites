// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Common
{
    /// <summary>
    /// The domain class represents an Active Directory domain.
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// The full qualified domain name of this domain
        /// </summary>
        public string FQDN;

        /// <summary>
        /// The netbios domain name of this domain
        /// </summary>
        public string NetbiosName;

        /// <summary>
        /// The root domain naming context
        /// </summary>
        public string DomainNC;

        /// <summary>
        /// The mode that this domain is operating in
        /// </summary>
        public DomainFunctionLevel DomainFunctionLevel;

        public Domain(string fqdn, string netbios = null) 
        {
            FQDN = fqdn;
            NetbiosName = String.IsNullOrEmpty(netbios) ? fqdn.Split('.')[0] : netbios;
            DomainNC = "DC=" + fqdn.Replace(".", ",DC=");
        }

        public Domain(string fqdn, DomainFunctionLevel domainfunctionlevel, string netbios = null)
        {
            FQDN = fqdn;
            NetbiosName = String.IsNullOrEmpty(netbios) ? fqdn.Split('.')[0] : netbios;
            DomainNC = "DC=" + fqdn.Replace(".", ",DC=");
            DomainFunctionLevel = domainfunctionlevel;
        }
    }

    /// <summary>
    /// The DomainCollection class contains the Domain objects
    /// </summary>
    public class DomainCollection : Collection<Domain> { }
}

