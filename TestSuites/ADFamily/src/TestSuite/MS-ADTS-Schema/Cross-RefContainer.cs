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
    /// This file is the source file for Validation of the TestCase22 and TestCase23.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region CrossRefContainer Validation.
        /// <summary>
        /// Method validates the requirements under CrossRefContainer Scenario.
        /// </summary>
        public void ValidateCrossRefContainer()
        {
            //Variables required for Directory Entries.
            DirectoryEntry dirPartitions = new DirectoryEntry();
            DirectoryEntry domainEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();
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
            string configNC = "CN=Configuration," + adAdapter.rootDomainDN;
            string SchemaNC = "CN=Schema," + configNC;
            //Returns Fully Qualified DNS Name for the Current Domain.
            string strReturn = adAdapter.PrimaryDomainDnsName;

            if (!adAdapter.GetObjectByDN("CN=Partitions," + configNC, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(false, "CN=Partitions," + configNC + " Object is not found in server");
            }
            string parent = dirPartitions.Parent.Name, systemFlag;
            bool isPresent = false;

            int systemFlagVal;

            //MS-ADTS-Schema_R402
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Configuration",
                parent,
                402,
                "The Parent of the Cross-Ref-Container Container must be Config NC root.");

            //MS-ADTS-Schema_R403
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "Partitions",
                dirPartitions.Properties["name"].Value.ToString(),
                403,
                @"The name attribute of the Cross-Ref-Container Container must be Partitions.");

            //MS-ADTS-Schema_R404
            PropertyValueCollection objectClass = dirPartitions.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"crossRefContainer"),
                404,
                "The objectClass attribute of the Cross-Ref-Container Container must be crossRefContainer.");
            string fsmoRoleOwner = dirPartitions.Properties["fSMORoleOwner"].Value.ToString();

            //MS-ADTS-Schema_R406
            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                406,
                "The systemFlags attribute of the Cross-Ref-Container Container must be FLAG_DISALLOW_DELETE.");

            //MS-ADTS-Schema_R405
            if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out domainEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, adAdapter.rootDomainDN + " Object is not found in server");
            }
            string domainFSMO = domainEntry.Properties["fSMORoleOwner"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                fsmoRoleOwner,
                domainFSMO,
                405,
                "The fSMORoleOwner attribute of the Cross-Ref-Container Container must references the Domain "
            + "Naming Master FSMO role owner.");

            //MS-ADTS-Schema_R408
            //Query the Partitions for CrossRef container get the childrens and validate the requirments as such.
            bool isCrossRef = true;
            bool isObjectClassCrossRef = true;
            DirectoryEntries Childs = dirPartitions.Children;

            foreach (DirectoryEntry child in Childs)
            {
                if (!child.Parent.Name.Equals("CN=Partitions"))
                {
                    isCrossRef = false;
                }
                PropertyValueCollection objClass = child.Properties["objectClass"];
                objClass.RemoveAt(0);
                if (objClass.Count != 1 || !objClass.Contains((object)"crossRef"))
                {
                    isObjectClassCrossRef = false;
                }
                if (dirPartitions.Parent.Name.ToString().Equals("CN=Configuration"))
                {
                    isPresent = true;
                }
            }
            //Validate the parent of the crossref objects,objectClass,nCName and
            //dnsRoot for all childs and crossRef container in the configNC.
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isCrossRef,
                408,
                "The parent of the Cross-Ref Objects must be crossRefContainer object.");

            //MS-ADTS-Schema_R851
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isPresent & isCrossRef,
                851,
               "For any NC in the forest crossRef,NC root objects must be exist.");

            // MS-ADTS-Schema_R409
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClassCrossRef,
                409,
                "The objectClass attribute of the Cross-Ref Objects must be crossRef.");

            //This is the crossRef container or crossRefObject 
            //MS-ADTS-Schema_R410
            childEntry = dirPartitions.Children.Find("CN=Enterprise Configuration");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                configNC.ToLower(),
                childEntry.Properties["nCName"].Value.ToString().ToLower(),
                410,
                "The nCName attribute of the Configuration crossRef Object must equal to the config NC root.");

            //MS-ADTS-Schema_R411
            string dnsRoot = childEntry.Properties["dnsRoot"].Value.ToString().ToLower();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                strReturn.ToLower(),
                dnsRoot.ToLower(),
                411,
                "In AD/DS the dnsroot attribute of the Configuration crossRef Object must be the forest "
            + "root's fully qualified DNS name.");

            //This is the crossRef child or crossRefObject 
            //MS-ADTS-Schema_R413
            childEntry = dirPartitions.Children.Find("CN=Enterprise Schema");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                SchemaNC.ToLower(),
                childEntry.Properties["nCName"].Value.ToString().ToLower(),
                413,
                "The nCName attribute of the Schema crossRef Object must equal to the Schema NC root.");

            //MS-ADTS-Schema_R414
            dnsRoot = childEntry.Properties["dnsRoot"].Value.ToString().ToLower();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                strReturn.ToLower(),
                dnsRoot.ToLower(),
                414,
                "In AD/DS the dnsroot attribute of the Schema crossRef Object must be the forest "
            + "root's fully qualified DNS name.");

            //MS-ADTS-Schema_R852
            if (!adAdapter.GetObjectByDN(SchemaNC, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(false, SchemaNC + " Object is not found in server");
            }
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                childEntry.Properties["nCName"].Value.ToString(),
                dirPartitions.Properties["distinguishedName"].Value.ToString(),
                852,
                "The nCName attribute of the crossRef object must equal the dsname of the NC root object.");

            if (!adAdapter.GetObjectByDN("CN=Partitions," + configNC, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(false, "CN=Partitions," + configNC + " Object is not found in server");
            }
            foreach (DirectoryEntry partitionEntry in dirPartitions.Children)
            {
                if (partitionEntry.Properties["objectCategory"].Value.ToString().ToLower().Contains("cn=cross-ref"))
                {
                    if (partitionEntry.Properties["nCName"].Value.ToString().Contains(adAdapter.PartitionPath))
                    {
                        partitionEntry.RefreshCache(new string[] { "msDS-SDReferenceDomain" });
                        if (partitionEntry.Properties.Contains("msDS-SDReferenceDomain") && partitionEntry.Properties["msDS-SDReferenceDomain"].Value != null)
                        {
                            //MS-ADTS-Schema_R422
                            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                                adAdapter.rootDomainDN.ToLower(),
                                partitionEntry.Properties["msDS-SDReferenceDomain"].Value.ToString().ToLower(),
                                422,
                                "In AD/DS msDS-SDReferenceDomain attribute of the Application NC crossRef object,"
                            + "references an NC root object for a domain. All security descriptors in this application NC "
                            + "must use the NC represented as the reference domain for resolution.");
                        }
                        else
                        {
                            DataSchemaSite.Log.Add(LogEntryKind.Warning, "The value of msDS-SDReferenceDomain attribute of the Application NC crossRef object is null");
                        }
                    }
                    if (partitionEntry.Properties["nCName"].Value.ToString().Equals(adAdapter.rootDomainDN))
                    {
                        //MS-ADTS-Schema_R416
                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            partitionEntry.Properties["name"].Value.ToString(),
                            partitionEntry.Properties["nETBIOSName"].Value.ToString(),
                            416,
                            "The name attribute of the Domain crossRef Object must be the Netbios name of the domain.");

                        //MS-ADTS-Schema_R417
                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            adAdapter.rootDomainDN,
                            partitionEntry.Properties["nCName"].Value.ToString(),
                            417,
                            "The nCName attribute of the Domain crossRef Object must be reference to the Domain NC root.");

                        //MS-ADTS-Schema_R418
                        if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out domainEntry))
                        {
                            DataSchemaSite.Assume.IsTrue(false, adAdapter.rootDomainDN + " Object is not found in server");
                        }
                        DirectoryEntry domainChildEntry = new DirectoryEntry();

                        DataSchemaSite.CaptureRequirementIfIsTrue(
                           adAdapter.rootDomainDN.ToLower().Contains(partitionEntry.Properties["nETBIOSName"].Value.ToString().ToLower()),
                            418,
                            "The nETBIOSName attribute of the Domain crossRef Object must be the Netbios name of the domain.");

                        //MS-ADTS-Schema trustParent
                        DataSchemaSite.Assert.IsNull(
                            partitionEntry.Properties["trustParent"].Value,
                            "The trustParent attribute is not present on the root domain NC's crossRef object.");

                        //MS-ADTS-Schema_R419
                        systemFlag = partitionEntry.Properties["systemFlags"].Value.ToString();
                        systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_NOT_REPLICATED|FLAG_ATTR_REQ_PARTIAL_SET_MEMBER");

                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            systemFlagVal.ToString(),
                            systemFlag,
                            419,
                            @"The systemFlags attribute of the Domain crossRef Object must be FLAG_CR_NTDS_NC | FLAG_CR_NTDS_DOMAIN.");
                    }
                    if (partitionEntry.Properties["nCName"].Value.ToString().Equals(adAdapter.childDomainDN))
                    {
                        //MS-ADTS-Schema_R416
                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            partitionEntry.Properties["name"].Value.ToString(),
                            partitionEntry.Properties["nETBIOSName"].Value.ToString(),
                            416,
                            "The name attribute of the Domain crossRef Object must be the Netbios name of the domain.");

                        //MS-ADTS-Schema_R417
                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            adAdapter.childDomainDN,
                            partitionEntry.Properties["nCName"].Value.ToString(),
                            417,
                            "The nCName attribute of the Domain crossRef Object must be reference to the Domain NC root.");

                        //MS-ADTS-Schema_R418
                        if (!adAdapter.GetObjectByDN(adAdapter.childDomainDN, out domainEntry))
                        {
                            DataSchemaSite.Assume.IsTrue(false, adAdapter.childDomainDN + " Object is not found in server");
                        }
                        DirectoryEntry domainChildEntry = new DirectoryEntry();

                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            adAdapter.childDomainDN.ToLower().Contains(partitionEntry.Properties["nETBIOSName"].Value.ToString().ToLower()),
                            418,
                            "The nETBIOSName attribute of the Domain crossRef Object must be the Netbios name of the domain.");

                        //MS-ADTS-Schema trustParent
                        DirectoryEntry parentEntry = new DirectoryEntry();
                        string netbiosDomain = adAdapter.PrimaryDomainNetBiosName; 
                        parentEntry = dirPartitions.Children.Find(string.Format("CN={0}", netbiosDomain));                        
                        DataSchemaSite.Assert.AreEqual<string>(
                            parentEntry.Properties["distinguishedName"].Value.ToString(),
                            partitionEntry.Properties["trustParent"].Value.ToString(),
                            "For child NCs, the trustParent attribute value references the parent NC's crossRef object.");

                        //MS-ADTS-Schema_R419
                        systemFlag = partitionEntry.Properties["systemFlags"].Value.ToString();
                        systemFlagVal = ParseSystemFlagsValue("FLAG_ATTR_NOT_REPLICATED|FLAG_ATTR_REQ_PARTIAL_SET_MEMBER");

                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            systemFlagVal.ToString(),
                            systemFlag,
                            419,
                            @"The systemFlags attribute of the Domain crossRef Object must be FLAG_CR_NTDS_NC | FLAG_CR_NTDS_DOMAIN.");
                    }
                }
            }
            //MS-ADTS-Schema_R420
            dnsRoot = childEntry.Properties["dnsRoot"].Value.ToString().ToLower();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                strReturn.ToLower(),
                dnsRoot.ToLower(),
                420,
                "In AD/DS the the value for dnsRoot for an application NC crossRef is derived by syntactically "
            + "converting the DN portion of the crossRef's nCName into a fully qualified DNS name.");
        }

        #endregion

        #region LDSCrossRefContainer Implementation.

        /// <summary>
        /// Method validates the requirements under LDSCrossRefContainer Scenario.
        /// </summary>
        public void ValidateLDSCrossRefContainer()
        {
            DirectoryEntry dirPartitions = new DirectoryEntry();
            DirectoryEntry domainEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();
            DirectoryEntry requiredEntry = new DirectoryEntry();
            string systemFlag;
            int systemFlagVal;
            //Variables for holding the paths for NCs.
            if (!adAdapter.GetLdsObjectByDN("CN=Partitions,CN=Configuration," + adAdapter.LDSRootObjectName, out dirPartitions))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Partitions,CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + "Object is not found in server");
            }
            string parentAttribute = dirPartitions.Parent.Name.ToString();

            //MS-ADTS-Schema_R402
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Configuration",
                parentAttribute,
                402,
                "The Parent of the Cross-Ref-Container Container must be Config NC root.");

            //MS-ADTS-Schema_R403
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "Partitions",
                dirPartitions.Properties["name"].Value.ToString(),
                403,
                "The name attribute of the Cross-Ref-Container Container must be Partitions.");

            PropertyValueCollection objectClass = dirPartitions.Properties["objectClass"];

            //MS-ADTS-Schema_R404
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"crossRefContainer"),
                404,
                "The objectClass attribute of the Cross-Ref-Container Container must be crossRefContainer.");

            //MS-ADTS-Schema_R406
            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                406,
                "The systemFlags attribute of the Cross-Ref-Container Container must be FLAG_DISALLOW_DELETE.");

            bool isCrossRef = true;
            bool isObjectClassCrossRef = true;
            DirectoryEntries Childs = dirPartitions.Children;

            foreach (DirectoryEntry child in Childs)
            {
                if (!child.Parent.Name.Equals("CN=Partitions"))
                {
                    isCrossRef = false;
                }
                PropertyValueCollection objClass = child.Properties["objectClass"];
                objClass.RemoveAt(0);
                if (objClass.Count != 1 || !objClass.Contains((object)"crossRef"))
                {
                    isObjectClassCrossRef = false;
                }
            }

            //MS-ADTS-Schema_R408
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isCrossRef,
                408,
                "The parent of the Cross-Ref Objects must be crossRefContainer object.");

            //MS-ADTS-Schema_R409
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClassCrossRef,
                409,
                "The objectClass attribute of the Cross-Ref Objects must be crossRef.");

            childEntry = dirPartitions.Children.Find("CN=Enterprise Configuration");
            //MS-ADTS-Schema_R410
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Configuration," + adAdapter.LDSRootObjectName,
                childEntry.Properties["nCName"].Value.ToString(),
                410,
                "The nCName attribute of the Configuration crossRef Object must equal to the config NC root.");

            //MS-ADTS-Schema_R412
            DataSchemaSite.CaptureRequirementIfIsFalse(
                childEntry.Properties.Contains("dnsRoot"),
                412,
                "In AD/LDS the dnsroot attribute is not presented for the Configuration crossRef Object. ");

            childEntry = dirPartitions.Children.Find("CN=Enterprise Schema");
            //MS-ADTS-Schema_R413
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName,
                childEntry.Properties["nCName"].Value.ToString(),
                413,
                "The nCName attribute of the Schema crossRef Object must equal to the Schema NC root.");

            //MS-ADTS-Schema_R415
            DataSchemaSite.CaptureRequirementIfIsFalse(
                childEntry.Properties.Contains("dnsRoot"),
                415,
                "In AD/LDS the dnsroot attribute is not presented for the Schema crossRef Object.");

            childEntry = dirPartitions.Children.Find("CN=Enterprise Configuration");
            //MS-ADTS-Schema_R421
            DataSchemaSite.CaptureRequirementIfIsFalse(
                childEntry.Properties.Contains("dnsRoot"),
                421,
                "In AD/LDS the dnsroot attribute is not presented for Application NC crossRef Object.");

            //MS-ADTS-Schema_R852
            childEntry = dirPartitions.Children.Find("CN=Enterprise Schema");
            if (!adAdapter.GetLdsObjectByDN("CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName,
                 out dirPartitions))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Schema,CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                childEntry.Properties["nCName"].Value.ToString(),
                dirPartitions.Properties["distinguishedName"].Value.ToString(),
                852,
                "The nCName attribute of the crossRef object must equal the dsname of the NC root object.");
        }

        #endregion
    }
}