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
    /// The computer class represents an endpoint information
    /// </summary>
    public class Computer
    {
        /// <summary>
        /// The full qualified name of the computer
        /// </summary>
        public string FQDN;

        /// <summary>
        /// The netbios name of the computer
        /// </summary>
        public string NetbiosName;

        /// <summary>
        /// The domain that this computer is a member of
        /// </summary>
        public Domain Domain;

        /// <summary>
        /// The ip address of this computer
        /// </summary>
        public IPAddress IPAddress;

        /// <summary>
        /// The operating system version of this domain controller
        /// </summary>
        public ServerVersion OSVersion;

        /// <summary>
        /// The constructor of the computer with 0 arguments
        /// </summary>
        public Computer() {}

        /// <summary>
        /// The constructor of the computer when computer not domain member
        /// </summary>
        /// <param name="netbiosname">The netbios name of the computer</param>
        /// <param name="ipaddress">The ipaddress of the computer</param>
        public Computer(string netbiosname, string ipaddress)
        {
            NetbiosName = netbiosname;
            IPAddress.TryParse(ipaddress, out IPAddress);
        }

        /// <summary>
        /// The constructor of the computer when computer is a domain member
        /// </summary>
        /// <param name="domain">The name of the domain that the computer is in</param>
        /// <param name="netbiosname">The netbios name of the computer</param>
        /// <param name="ipaddress">The ipaddress of the computer</param>
        public Computer(Domain domain, string netbiosname, string ipaddress)
        {
            Domain = domain;
            NetbiosName = netbiosname;
            IPAddress.TryParse(ipaddress, out IPAddress);
            FQDN = String.Concat(netbiosname, '.', domain.FQDN);
        }

        /// <summary>
        /// The constructor of the computer when computer is a domain member
        /// </summary>
        /// <param name="domain">The name of the domain that the computer is in</param>
        /// <param name="netbiosname">The netbios name of the computer</param>
        /// <param name="ipaddress">The ipaddress of the computer</param>
        /// <param name="osVersion">The os version of the computer</param>
        public Computer(Domain domain, string netbiosname, string ipaddress, ServerVersion osversion)
        {
            Domain = domain;
            NetbiosName = netbiosname;
            IPAddress.TryParse(ipaddress, out IPAddress);
            FQDN = String.Concat(netbiosname, '.', domain.FQDN);
            OSVersion = osversion;
        }
    }

    public class ComputerCollection : Collection<Computer>
    {
    }
}
