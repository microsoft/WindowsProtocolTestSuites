// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public abstract class DsDomain
    {
        /// <summary>
        /// If AD DS, it is the FQDN name of the domain; 
        /// If AD LDS, it is null.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Netbios name of AD DS
        /// </summary>
        public String NetbiosName { get; set; }

        /// <summary>
        /// DNS name of AD DS
        /// </summary>
        public String DNSName { get; set; }

        /// <summary>
        /// The admin user of the domain.
        /// </summary>
        public DsUser Admin { get; set; }

        /// <summary>
        /// The DSName of the Config NC.
        /// </summary>
        public DSNAME ConfigNC { get; set; }

        /// <summary>
        /// The DSName of the Schema NC.
        /// </summary>
        public DSNAME SchemaNC { get; set; }

        /// <summary>
        /// Function Level of Domain
        /// </summary>
        public DrsrDomainFunctionLevel FunctionLevel { get; set; }

        /// <summary>
        /// NTDS DSA object DNs of the FSMO role owner
        /// </summary>
        public Dictionary<FSMORoles, string> FsmoRoleOwners { get; set; }
    }

    /// <summary>
    /// AD DS domain
    /// </summary>
    public class AddsDomain : DsDomain
    {

        /// <summary>
        /// The NetBIOS name of the domain.
        /// </summary>
        public String NetBiosName
        {
            get
            {
                return DrsrHelper.GetNetbiosNameFromDNSName(Name);
            }
        }

        public AddsDomain()
        {
            FsmoRoleOwners = new Dictionary<FSMORoles, string>();
        }

        /// <summary>
        /// DSNAME of the domain NC.
        /// </summary>
        public DSNAME DomainNC { get; set; }

        /// <summary>
        /// An array of DSNAME for NCs other than Config NC, Schema NC, and Domain NC.
        /// </summary>
        public DSNAME[] OtherNCs { get; set; }
    }

    /// <summary>
    /// AD LDS domain
    /// </summary>
    public class AdldsDomain : DsDomain
    {
        public AdldsDomain()
        {
            FsmoRoleOwners = new Dictionary<FSMORoles, string>();
        }

        /// <summary>
        /// DSNAME of the app NCs.
        /// </summary>
        public DSNAME[] AppNCs { get; set; }

        /// <summary>
        /// An array of DSNAME for NCs other than Config NC, Schema NC, and App NCs.
        /// </summary>
        public DSNAME[] OtherNCs { get; set; }
    }
}
