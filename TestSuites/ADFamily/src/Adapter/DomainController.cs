// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Common
{
    /// <summary>
    /// The domain controller class represents a domain controller in an Active Directory.
    /// </summary>
    public class DomainController : Computer
    {
        /// <summary>
        /// The constructor of the domain controller
        /// </summary>
        /// <param name="domain">The name of the domain that the domain controller is in</param>
        /// <param name="netbiosname">The netbiosname of the domain controller</param>
        /// <param name="ipaddress">The ip address of the domain controller</param>
        /// <param name="osversion">The os version of the domain controller</param>
        public DomainController(Domain domain, string netbiosname, string ipaddress, ServerVersion osversion)
            : base(domain, netbiosname, ipaddress, osversion)
        { }

        /// <summary>
        /// The constructor of the domain controller
        /// </summary>
        /// <param name="domain">The name of the domain that the domain controller is in</param>
        /// <param name="netbiosname">The netbiosname of the domain controller</param>
        /// <param name="ipaddress">The ip address of the domain controller</param>
        public DomainController(Domain domain, string netbiosname, string ipaddress) 
            : base(domain, netbiosname, ipaddress)
        { }
    }

    public class DomainControllerCollection : Collection<DomainController> 
    {
    }
}
