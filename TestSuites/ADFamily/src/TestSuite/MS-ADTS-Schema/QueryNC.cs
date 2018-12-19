// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using System.DirectoryServices.Protocols;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase20 and TestCase21.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region QueryNC

        /// <summary>
        /// This method validates the requirements under 
        /// QueryNC Scenario.
        /// </summary>
        public void ValidateQueryNC()
        {
            //Variables holding the directory entries required.
            DirectoryEntry dirEntry = new DirectoryEntry();
            DirectoryEntry schemaEntry = new DirectoryEntry();
            DirectoryEntry serverDirEntry = new DirectoryEntry();
            DirectoryEntry partitionsDirEntry = new DirectoryEntry();

            if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out dirEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, adAdapter.rootDomainDN + " Object is not found in server");
            }

            //MS-ADTS-Schema_R853
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirEntry.Properties.Contains("nTMixedDomain"),
                853,
                "The attribute nTMixedDomain is present on each domain NC root object.");

            if (!adAdapter.GetObjectByDN("CN=Configuration," + adAdapter.rootDomainDN, out dirEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            if (!adAdapter.GetObjectByDN("CN=Schema,CN=Configuration," + adAdapter.rootDomainDN,
                out schemaEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Schema,CN=Configuration,"
                    + adAdapter.rootDomainDN + " Object is not found in server");
            }

            if (!adAdapter.GetObjectByDN("CN=" + adAdapter.PDCNetbiosName + ",OU=Domain Controllers," + adAdapter.rootDomainDN,
                out dirEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN="
                    + adAdapter.PDCNetbiosName
                    + ",OU=Domain Controllers,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            if (!adAdapter.GetObjectByDN(
                "CN="
                + adAdapter.PDCNetbiosName
                + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                + adAdapter.rootDomainDN,
                out serverDirEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN="
                    + adAdapter.PDCNetbiosName
                    + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }

            //Validate the objectVersion of Schema NC
            if (serverOS == OSVersion.WinSvr2008)
            {
                DataSchemaSite.Assert.AreEqual<string>("44", schemaEntry.Properties["objectVersion"].Value.ToString(),
                    "The objectVersion of {0} should be 44.", serverOS);
            }
            else if (serverOS == OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.Assert.AreEqual<string>("47", schemaEntry.Properties["objectVersion"].Value.ToString(),
                    "The objectVersion of {0} should be 47.", serverOS);
            }
            else if (serverOS == OSVersion.WinSvr2012)
            {
                DataSchemaSite.Assert.AreEqual<string>("56", schemaEntry.Properties["objectVersion"].Value.ToString(),
                    "The objectVersion of {0} should be 56.", serverOS);
            }
            else if (serverOS == OSVersion.WinSvr2012R2)
            {
                DataSchemaSite.Assert.AreEqual<string>("69", schemaEntry.Properties["objectVersion"].Value.ToString(),
                    "The objectVersion of {0} should be 69.", serverOS);
            }
            else if (serverOS == OSVersion.Win2016)
            {
                DataSchemaSite.Assert.AreEqual<string>("87", schemaEntry.Properties["objectVersion"].Value.ToString(),
                    "The objectVersion of {0} should be 87.", serverOS);
            }
            else if (serverOS >= OSVersion.Winv1803)
            {
                DataSchemaSite.Assert.AreEqual<string>("88", schemaEntry.Properties["objectVersion"].Value.ToString(),
                    "The objectVersion of {0} should be 88.", serverOS);
            }

            //MS-ADTS-Schema_R848
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                dirEntry.Properties["dNSHostName"].Value.ToString(),
                serverDirEntry.Properties["dNSHostName"].Value.ToString(),
                848,
                @"On AD/DS, the dNSHostName attribute of the domain controller object must equal
                the dNSHostName attribute of the server object.");

            //MS-ADTS-Schema_R847
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                dirEntry.Properties["distinguishedName"].Value.ToString(),
                serverDirEntry.Properties["serverReference"].Value.ToString(),
                847,
                @"On AD/DS, the attribute serverReference on the server object must equal
                the dsname of the domain controller object.");

            //MS-ADTS-Schema_R845
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                adAdapter.PDCNetbiosName.ToLower(),
                serverDirEntry.Properties["cn"].Value.ToString().ToLower(),
                845,
                "On AD/DS, the name of the server object is the computer name of the DC.");

            //MS-ADTS-Schema_R849
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                dirEntry.Properties["dNSHostName"].Value.ToString(),
                serverDirEntry.Properties["dNSHostName"].Value.ToString(),
                849,
                @"The dNSHostName attribute of the server object must equal the DNS Hostname
                of the computer that is physically the DC.");

            //MS-ADTS-Schema_R850
            PropertyValueCollection val = dirEntry.Properties["servicePrincipalName"];
            foreach (string value in val)
            {
                // Extract the hostname
                string partvar = value.Substring(value.IndexOf('/') + 1);
                string hostname = (partvar.IndexOf('/') == -1) ? partvar : partvar.Substring(0, partvar.IndexOf('/'));
                // Ignore the port number
                if (hostname.Contains(":")) hostname = hostname.Substring(0, hostname.IndexOf(':'));

                // If it is a DNS hostname
                if (hostname.Contains(".") && !hostname.Contains("_msdcs"))
                {
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        hostname.ToLower().Equals(dirEntry.Properties["dNSHostName"].Value.ToString().ToLower()),
                        850,
                        @"On AD/DS, every value of the servicePrincipalName attribute of the domain controller object,
                        which has a DNS Hostname as the instance name should have instance name equal to the dNSHostName 
                        of the domain controller object.");
                }
            }


            if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, adAdapter.rootDomainDN + " Object is not found in server");
            }

            DirectoryEntry entry = new DirectoryEntry("LDAP://" + adAdapter.PDCNetbiosName + "/rootDSE");
            DirectoryEntry applicationEntry = new DirectoryEntry();
            DirectoryEntry ncEntry = new DirectoryEntry();
            PropertyValueCollection allNCs = entry.Properties["namingContexts"];
            HashSet<string> collectionOfNCs = new HashSet<string>();
            bool rootDomainNCAsChild = false, domainOfApplicationNC = false;

            foreach (string eachNC in allNCs)
            {
                if (collectionOfNCs.Contains(eachNC))
                {
                    collectionOfNCs.Add(eachNC);
                }
                collectionOfNCs.Add(eachNC);
            }
            if (!Utilities.IsObjectExist(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN, adAdapter.PDCNetbiosName,
                adAdapter.ADDSPortNum, adAdapter.DomainAdministratorName, adAdapter.DomainUserPassword))
            {
                //To create the Application NC in the Active Directory.
                AdLdapClient.Instance().ConnectAndBind(adAdapter.PDCNetbiosName,
                    adAdapter.PDCIPAddr, Convert.ToInt32(adAdapter.ADDSPortNum), adAdapter.DomainAdministratorName,
                    adAdapter.DomainUserPassword, adAdapter.PrimaryDomainDnsName,
                    AuthType.Basic | AuthType.Kerberos);
                List<DirectoryAttribute> attrs = new List<DirectoryAttribute>();
                attrs.Add(new DirectoryAttribute("instancetype:5"));
                attrs.Add(new DirectoryAttribute("objectclass:domainDNS"));
                AdLdapClient.Instance().AddObject(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN, attrs, null);
                AdLdapClient.Instance().Unbind();
            }
            if (!adAdapter.GetObjectByDN(
                adAdapter.PartitionPath
                + ","
                + adAdapter.rootDomainDN,
                out applicationEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    adAdapter.PartitionPath
                    + ","
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            foreach (DirectoryEntry eachChild in applicationEntry.Children)
            {
                if (eachChild.Name == adAdapter.rootDomainDN)
                {
                    domainOfApplicationNC = true;
                }
            }
            //MS-ADTS-Schema_R8
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !domainOfApplicationNC,
                8,
                "In a forest, the Domain NC must not be a child of application NC.");

            bool sidofdomainNCnotnull = false;
            bool sidofcongNcnull = false;
            bool sidofschemaNcnull = false;
            bool sidofapplicationNcnull = false;
            bool isDomainNc = false;
            bool isConfigurationNc = false;
            bool isSchemaNc = false;
            bool isApplicationNc = false;

            //Get all NC objects
            foreach (string singleNC in collectionOfNCs)
            {
                if (adAdapter.GetObjectByDN(singleNC, out ncEntry))
                {
                    //Get SID of this NC
                    object objectSid = ncEntry.Properties["objectSid"].Value;
                    if (objectSid != null)
                    {
                        System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier((byte[])objectSid, 0);
                        sidofdomainNCnotnull = true;
                    }
                    if (ncEntry.Properties["distinguishedName"].Value.ToString().Equals(adAdapter.rootDomainDN,StringComparison.OrdinalIgnoreCase))
                    {
                        isDomainNc = true;
                    }
                    else if (ncEntry.Name == "CN=Configuration")
                    {
                        isConfigurationNc = true;
                        if (objectSid == null)
                        {
                            sidofcongNcnull = true;
                        }
                    }
                    else if (ncEntry.Name == "CN=Schema")
                    {
                        isSchemaNc = true;
                        if (objectSid == null)
                        {
                            sidofschemaNcnull = true;
                        }
                    }
                    else
                    {
                        isApplicationNc = true;
                        if (objectSid == null)
                        {
                            sidofapplicationNcnull = true;
                        }
                    }
                }
            }
            //Verify MS-AD_Schema requirement: MS-AD_Schema_R4557.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isDomainNc,
                4557,
                @"[NC, NC Replica]Active Directory supports four NC types: Domain NC.");

            //Verify MS-AD_Schema requirement: MS-AD_Schema_R4558.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                sidofdomainNCnotnull,
                4558,
                @"[NC, NC Replica]The sid field of a domain NC is not NULL.");

            //Verify MS-AD_Schema requirement: MS-AD_Schema_R4559.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isConfigurationNc,
                4559,
                @"[NC, NC Replica]Active Directory supports four NC types: Config NC.");

            //Verify MS-AD_Schema requirement: MS-AD_Schema_R4560.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                sidofcongNcnull,
                4560,
                @"[NC, NC Replica]The sid field of a config NC is NULL.");

            //Verify MS-AD_Schema requirement: MS-AD_Schema_R4561.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isSchemaNc,
                4561,
                @"[NC, NC Replica]Active Directory supports four NC types: Schema NC.");

            //Verify MS-AD_Schema requirement: MS-AD_Schema_R4562.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                sidofschemaNcnull,
                4562,
                @"[NC, NC Replica]The sid field of a schema NC is NULL.");

            //Verify MS-AD_Schema requirement: MS-AD_Schema_R4564.
            DataSchemaSite.CaptureRequirementIfIsTrue(
            sidofapplicationNcnull,
            4564,
            @"[NC, NC Replica]The sid field of an application NC is NULL.");
            //
            //Verify MS-AD_Schema requirement: MS-AD_Schema_R4563.
            //
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isApplicationNc,
                4563,
                @"[NC, NC Replica]Active Directory supports four NC types: Application NC.");

            if (!adAdapter.GetObjectByDN(
                "CN=Partitions,CN=Configuration,"
                + adAdapter.rootDomainDN,
                out partitionsDirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Partitions,CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }

            foreach (DirectoryEntry eachNC in partitionsDirEntry.Children)
            {
                if (eachNC.Properties["msDS-SDReferenceDomain"].Value != null)
                {
                    if (dirEntry.Properties["distinguishedName"].Value.ToString() ==
                        eachNC.Properties["distinguishedName"].Value.ToString())
                    {
                        rootDomainNCAsChild = true;
                    }
                }
            }
            //MS-ADTS-Schema_R9
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !rootDomainNCAsChild,
                9,
                "In a forest, the root domain NC must not be a child of another domain NC in the forest.");
        }

        #endregion

        #region LDSQueryNC Validation.

        /// <summary>
        /// This method validates the requirements under 
        /// LDSQueryNC Scenario.
        /// </summary>
        public void ValidateLDSQueryNC()
        {
            DirectoryEntry dirEntry = new DirectoryEntry();
            DirectoryEntry schemaEntry = new DirectoryEntry();
            DirectoryEntry serverDirEntry = new DirectoryEntry();

            if (!adAdapter.GetLdsObjectByDN("CN=Configuration," + adAdapter.LDSRootObjectName, out dirEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            if (!adAdapter.GetLdsObjectByDN("CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName,
                out schemaEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Schema,CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            if (!adAdapter.GetLdsObjectByDN(
                "CN="
                + adAdapter.LDSServerInstance
                + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                + adAdapter.LDSRootObjectName,
                out serverDirEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN="
                    + adAdapter.LDSServerInstance
                    + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }

            //Validate the objectVersion of Schema NC
            if (serverOS == OSVersion.WinSvr2008)
            {
                DataSchemaSite.Assert.AreEqual<string>("30", schemaEntry.Properties["objectVersion"].Value.ToString(),
                    "The objectVersion of {0} should be 30.", serverOS);
            }
            else if (serverOS >= OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.Assert.AreEqual<string>("31", schemaEntry.Properties["objectVersion"].Value.ToString(),
                    "The objectVersion of {0} should be 31.", serverOS);
            }

            //MS-ADTS-Schema_R846
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                adAdapter.LDSServerInstance.ToLower(),
                serverDirEntry.Properties["cn"].Value.ToString().ToLower(),
                846,
                "On AD/LDS, the name of the server object is the computer name, followed by \"$\",followed by the "
            + "instance name of the DC.");
        }
        #endregion

    }
}