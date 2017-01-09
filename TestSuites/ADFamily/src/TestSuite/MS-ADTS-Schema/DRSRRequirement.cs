// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using System.IO;
using System.Text;

using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase24 and TestCase25.
    /// </summary>
    public partial class DataSchemaTestSuite
    {

               
        #region Domain Controllers
        /// <summary>
        /// Validates requirements related to DC.
        /// </summary>
        public void DRSRRequirementValidation()
        {

            //Dn of DomainController
            string distinguishName = "LDAP://CN="
                + adAdapter.PDCNetbiosName
                + ",OU=Domain Controllers,"
                + adAdapter.rootDomainDN;

            //Instance of DirectoryEntry eith dn.
            DirectoryEntry compEntry = new DirectoryEntry(distinguishName);            

            #region Validation of MS-AD_Schema_R933

            SearchResponse ntdsdsResponse = null;
            bool condition1 = false;
            bool condition2 = false;
            int instanceType = 0;

            #region Validate MS-AD_Schema_R933

            distinguishName = "CN=NTDS Settings,CN="
                + adAdapter.PDCNetbiosName 
                + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                + adAdapter.rootDomainDN;
            GetLDAPObject(
                distinguishName,
                adAdapter.PDCNetbiosName, 
                "objectclass=ntdsdsa", 
                System.DirectoryServices.Protocols.SearchScope.Base, 
                new string[] { "msDS-hasMasterNCs", "hasMasterNCs", "msDS-HasInstantiatedNCs", "msDS-HasDomainNCs", "options", "hasPartialReplicaNCs" },
                out ntdsdsResponse);

            string[] instantiatedNCValue = (string[])ntdsdsResponse.Entries[0].Attributes["msDS-HasInstantiatedNCs"].GetValues(typeof(string));

            if (ntdsdsResponse.Entries[0].Attributes.Contains("hasPartialReplicaNCs"))
            {
                string[] partialReplicas = (string[])ntdsdsResponse.Entries[0].Attributes["hasPartialReplicaNCs"].GetValues(typeof(string));

                condition1 = false;

                foreach (string replica in partialReplicas)
                {
                    if (replica.ToLower().Contains(adAdapter.rootDomainDN.ToLower()))
                    {
                        condition1 = true;
                        break;
                    }
                }

                distinguishName = "CN=NTDS Settings,CN=" + adAdapter.PDCNetbiosName + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration," + adAdapter.rootDomainDN;
                GetLDAPObject(
                    distinguishName,
                    adAdapter.PDCNetbiosName, 
                    "objectclass=ntdsdsa", 
                    System.DirectoryServices.Protocols.SearchScope.Base, 
                    new string[] { "msDS-HasInstantiatedNCs" }, 
                    out ntdsdsResponse);

                instanceType = int.Parse(new DirectoryEntry("LDAP://" + adAdapter.rootDomainDN).Properties["instancetype"].Value.ToString());

                condition2 = false;
                foreach (string value in instantiatedNCValue)
                {
                    if (value.Contains(instanceType.ToString("X") + ":" + adAdapter.rootDomainDN))
                    {
                        condition2 = true;
                        break;
                    }
                }

                condition1 = condition1 && condition2;

                DataSchemaSite.CaptureRequirementIfIsTrue(
                    condition1, 
                    933, 
                    @"A DC hosts a partial NC replica of a domain 
                    NC when the attribute hasPartialReplicaNCs on the nTDSDSA object representing the DC contains the 
                    dsname of the NC roots representing the domain NC and the attribute msDS-HasInstantiatedNCs on the 
                    nTDSDSA object representing the DC contains an Object(DN-Binary) value such that the DN field is 
                    the dsname of the NC root representing the NC, and the Data field contains the value of the 
                    instanceType attribute on the NC root object on the DC.");
            }

            #endregion

            #endregion     
        }

        #endregion

    }
}