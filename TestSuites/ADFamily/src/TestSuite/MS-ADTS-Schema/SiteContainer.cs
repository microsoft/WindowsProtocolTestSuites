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
using System.Linq;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase24 and TestCase25.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region SitesContainer Implementation.
        // <summary>
        /// This method validates the requirements under 
        /// SitesContainer Scenario.
        /// </summary>  
        public void ValidateSitesContainer()
        {
            //Declaring the DirectoryEntry variables for holding the objects.
            DirectoryEntry dirPartitions = new DirectoryEntry();
            DirectoryEntry domainEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();

            string currDomain = adAdapter.rootDomainDN;
            string configNC = "CN=Configuration," + currDomain;
            string SchemaNC = "CN=Schema," + configNC;
            string systemFlag = String.Empty;
            int systemFlagVal;

            //MS-ADTS-Schema_R423
            if (!adAdapter.GetObjectByDN("CN=Sites," + configNC, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(false, "CN=Sites," + configNC + " Object is not found in server");
            }
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Configuration",
                dirPartitions.Parent.Name.ToString(),
                423,
                "Each forest contains a Sites container in the Config NC.");

            //MS-ADTS-Schema_R425
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Configuration",
                dirPartitions.Parent.Name.ToString(),
                425,
                "The parent of the Sites Container must be Config NC root object.");

            //MS-ADTS-Schema_R426
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirPartitions.Properties["objectClass"].Contains((object)"sitesContainer"),
                426,
                "The objectClass attribute of the Sites Container must be sitesContainer.");

            //MS-ADTS-Schema_R428
            DirectoryEntries dirEntriesForValue = dirPartitions.Children;
            bool isParentRoles = false;
            foreach (DirectoryEntry child in dirEntriesForValue)
            {
                if (child.Parent.Name.ToString().Equals("CN=Sites"))
                {
                    isParentRoles = true;
                }
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isParentRoles,
                428,
                "The parent of the Site Object must be Sites container.");

            //MS-ADTS-Schema_R429
            childEntry = dirPartitions.Children.Find("CN=Default-First-Site-Name");
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childEntry.Properties["objectClass"].Contains((object)"site"),
                429,
                "The objectClass attribute of the Site Object must be Site.");

            //MS-ADTS-Schema_R427

            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE|FLAG_DISALLOW_MOVE_ON_DELETE");
            if (systemFlag != (systemFlagVal.ToString()))
            {
                isParseSystemFlagsValue = false;
            }
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                427,
                @"The SytemFlags attribute of the Sites Container must be 
                either of FLAG_DISALLOW_DELETE|FLAG_DISALLOW_MOVE_ON_DELETE.");

            if (systemFlag != (systemFlagVal.ToString()))
            {
                isParseSystemFlagsValue = false;
            }

            //MS-ADTS-Schema_R430
            systemFlag = childEntry.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_CONFIG_ALLOW_RENAME|FLAG_DISALLOW_MOVE_ON_DELETE");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                430,
                @"The SytemFlags attribute of the Site Object must be 
                either of FLAG_CONFIG_ALLOW_RENAME|FLAG_DISALLOW_MOVE_ON_DELETE.");
            if (systemFlag != (systemFlagVal.ToString()))
            {
                isParseSystemFlagsValue = false;
            }

            //MS-ADTS-Schema_R431
            DirectoryEntry sitesEntry = new DirectoryEntry();
            if (!adAdapter.GetObjectByDN("CN=Sites,CN=Configuration," + currDomain, out sitesEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Sites,CN=Configuration,"
                    + currDomain
                    + " Object is not found in server");
            }
            sitesEntry = sitesEntry.Children.Find("CN=Default-First-Site-Name");
            DirectoryEntries sitesChildren = sitesEntry.Children;
            foreach (DirectoryEntry child in sitesChildren)
            {
                PropertyValueCollection v = child.Properties["objectClass"];
                if (child.Properties["objectClass"].Contains((object)"nTDSSiteSettings"))
                {
                    //MS-ADTS-Schema_R432
                    // [Since objectClass (sitesEntry) contains nTDSSiteSettings, R432 is captured.]
                    DataSchemaSite.CaptureRequirement(
                        432,
                        "The objectClass attribute of the NTDS Site Settings Object must be nTDSSiteSettings.");
                }
            }

            //MS-ADTS-Schema_R431
            DataSchemaSite.Log.Add(LogEntryKind.Debug, "Verify MS-ADTS-Schema_R431");

            DataSchemaSite.CaptureRequirementIfIsTrue(
                sitesEntry.Parent.Name.ToString().Contains("CN=Sites"),
                431,
                "The parent of the NTDS Site Settings Object must be site object.");

            //MS-ADTS-Schema_R457
            if (!adAdapter.GetObjectByDN("CN=Subnets,CN=Sites," + configNC, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Subnets,CN=Sites,"
                    + configNC
                    + " Object is not found in server");
            }
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Sites",
                dirPartitions.Parent.Name.ToString(),
                457,
                "Each forest contains a Subnets container in the config NC.");

            //MS-ADTS-Schema_R458
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Sites",
                dirPartitions.Parent.Name.ToString(),
                458,
                "The Parent of the Subnet container must be Sites container.");

            //MS-ADTS-Schema_R459
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirPartitions.Properties["objectClass"].Contains((object)"subnetContainer"),
                459,
                "The ObjectClass attribute of the Subnet container must be subnetContainer.");

            //MS-ADTS-Schema_R460
            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                460,
                "The SystemFlags attribute of the Subnet container must be FLAG_DISALLOW_DELETE.");
            if (systemFlag != (systemFlagVal.ToString()))
                isParseSystemFlagsValue = false;

            //MS-ADTS-Schema_R465
            if (!adAdapter.GetObjectByDN("CN=Inter-Site Transports,CN=Sites," + configNC, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Inter-Site Transports,CN=Sites,"
                    + configNC
                    + " Object is not found in server");
            }
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Sites",
                dirPartitions.Parent.Name.ToString(),
                465,
                "The Parent of the IP Transport Container must be Inter-Site Transports container.");

            //MS-ADTS-Schema_R466
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirPartitions.Properties["objectClass"].Contains((object)"interSiteTransportContainer"),
                466,
                "The ObjectClass attribute of the Inter-Site Transports Container must be interSiteTransportContainer.");

            //MS-ADTS-Schema_R467
            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                467,
                "The SystemFlags attribute of the Inter-Site Transports Container must be FLAG_DISALLOW_DELETE.");
            if (systemFlag != (systemFlagVal.ToString()))
                isParseSystemFlagsValue = false;

            //MS-ADTS-Schema_R468 
            childEntry = dirPartitions.Children.Find("CN=IP");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Inter-Site Transports",
                childEntry.Parent.Name.ToString(),
                468,
                "The Parent of the IP Transport Container must be Inter-Site Transports container.");

            //MS-ADTS-Schema_R469
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childEntry.Properties["objectClass"].Contains((object)"interSiteTransport"),
                469,
                "The ObjectClass attribute of the IP Transport Container must be interSiteTransport.");
            List<DirectoryAttribute> atts = new List<DirectoryAttribute>();
            string ret = AdLdapClient.Instance().ConnectAndBind(
                adAdapter.PDCNetbiosName,
                adAdapter.PDCIPAddr,
                Convert.ToInt32(adAdapter.ADDSPortNum),
                adAdapter.DomainAdministratorName,
                adAdapter.DomainUserPassword,
                adAdapter.PrimaryDomainNetBiosName,
                AuthType.Basic | AuthType.Kerberos);
            if (ret.Equals("Success_STATUS_SUCCESS"))
            {
                atts.Add(new DirectoryAttribute("objectClass", new String[] { "top", "site" }));
                string testDistinguishedName = "CN=test,CN=Sites,CN=Configuration," + adAdapter.rootDomainDN;
                AdLdapClient.Instance().AddObject(testDistinguishedName,atts,null);

                atts.Clear();
                atts.Add(new DirectoryAttribute("siteList", new String[] { "CN=test,CN=Sites,CN=Configuration," + adAdapter.rootDomainDN }));
                atts.Add(new DirectoryAttribute("objectClass", new String[] { "top", "siteLink" }));
                string testLinkDistinguishedName = "CN=testlink,CN=IP,CN=Inter-Site Transports,CN=Sites,CN=Configuration," + adAdapter.rootDomainDN;
                AdLdapClient.Instance().AddObject(testLinkDistinguishedName, atts, null);

                atts.Clear();
                atts.Add(new DirectoryAttribute("siteLinkList", new String[] 
                { "CN=DEFAULTIPSITELINK,CN=IP,CN=Inter-Site Transports,CN=Sites,CN=Configuration," + adAdapter.rootDomainDN, 
                    "CN=testLink,CN=IP,CN=Inter-Site Transports,CN=Sites,CN=Configuration," + adAdapter.rootDomainDN }));
                atts.Add(new DirectoryAttribute("objectClass", new String[] { "top", "siteLinkBridge" }));
                string testLinkBridgeDistinguishedName = "CN=testLinkBridge,CN=IP,CN=Inter-Site Transports,CN=Sites,CN=Configuration," + adAdapter.rootDomainDN;
                string result = AdLdapClient.Instance().AddObject(testLinkBridgeDistinguishedName, atts, null);
                DataSchemaSite.Assert.AreEqual<string>("Success_STATUS_SUCCESS", result, "create dynamic object {0} succeeded", testLinkBridgeDistinguishedName);

                if (!adAdapter.GetObjectByDN("CN=Subnets,CN=Sites," + configNC, out dirPartitions))
                {
                    DataSchemaSite.Assume.IsTrue(
                        false,
                        "CN=Subnets,CN=Sites,"
                        + configNC
                        + " Object is not found in server");
                }
                //MS-ADTS-Schema_R462,MS-ADTS-Schema_R463 and MS-ADTS-Schema_R464
                //The Parent of the Subnet Object  must be Subnets container.
                DirectoryEntries subnetChilds;
                subnetChilds = dirPartitions.Children;
                foreach (DirectoryEntry child in subnetChilds)
                {
                    DataSchemaSite.Log.Add(LogEntryKind.Debug, "Subnet name is a valid subnet name if:");
                    string name = child.Properties["name"].Value.ToString();
                    int count = name.Split('/').Length - 1;
                    DataSchemaSite.Assert.AreEqual(1, count, "1. There should be only one occurrence of the character \"/\" in subnet name:{0}.", name);
                    
                    int i = name.IndexOf('/');
                    string s1 = name.Substring(0, i);
                    DataSchemaSite.Assert.AreNotEqual(0, s1.IndexOf('0'), "2. Let i be the index of the character \"/\" in name, the substring name[0, i-1]:{0} does not have any leading zeros.", s1);
                    System.Net.IPAddress subnetIP = null;
                    bool isValid = false;
                    isValid = System.Net.IPAddress.TryParse(s1, out subnetIP);
                    DataSchemaSite.Assert.IsTrue(isValid, "2. The substring name[0, i-1]:{0} should be either a valid IPv4 address in dotted decimal notation or a valid IPv6 address in colon-hexadecimal form or compressed form.", s1);

                    string s2 = name.Substring(i + 1);
                    DataSchemaSite.Assert.AreNotEqual(0, s2.IndexOf('0'), "3. The substring name[i+1, l-1]:{0} does not have any leading zeros.", s2);
                    uint n;
                    isValid = false;
                    isValid = uint.TryParse(s2, out n);
                    DataSchemaSite.Assert.IsTrue(isValid, "3. The substring name[i+1, l-1] should be able to be converted to an unsigned integer n:{0}.", n);

                    isValid = false;
                    if (subnetIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        if (n > 0 && n <= 32) isValid = true; 
                    }
                    else if (subnetIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        if (n > 0 && n <= 128) isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                    DataSchemaSite.Assert.IsTrue(isValid, "4. When the address is in IPv4 format, 0 < n:{0} <= 32. When the address is in IPv6 format, 0 < n:{0} <= 128.", n);

                    uint[] bitMask = new uint[] { 0x00000000, 0x00000080, 0x000000C0, 0x000000E0, 0x000000F0, 0x000000F8, 0x000000FC, 0x000000FE, 0x000000FF, 0x000080FF, 0x0000C0FF, 0x0000E0FF, 0x0000F0FF, 0x0000F8FF, 0x0000FCFF, 0x0000FEFF, 0x0000FFFF, 0x0080FFFF, 0x00C0FFFF, 0x00E0FFFF, 0x00F0FFFF, 0x00F8FFFF, 0x00FCFFFF, 0x00FEFFFF, 0x00FFFFFF, 0x80FFFFFF, 0xC0FFFFFF, 0xE0FFFFFF, 0xF0FFFFFF, 0xF8FFFFFF, 0xFCFFFFFF, 0xFEFFFFFF, 0xFFFFFFFF};
                    if (subnetIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        uint ipInt = BitConverter.ToUInt32(subnetIP.GetAddressBytes().Reverse().ToArray(), 0);
                        uint res = ipInt & (~bitMask[n]);
                        DataSchemaSite.Assert.AreEqual<uint>(0, res, "5. Let b be the binary representation of the address, when the address is in IPv4 format, b & (~BitMask[n]) = 0.");
                        DataSchemaSite.Assert.IsTrue(ipInt!=bitMask[n], "6. When the address is in IPv4 format, b != BitMask[n].");
                    }

                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "CN=Subnets",
                    child.Parent.Name.ToString(),
                    462,
                    "The Parent of the Subnet Object must be Subnets container.");

                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        "subnet",
                        child.SchemaClassName,
                        463,
                        "The ObjectClass attribute of the Subnet Object  must be Subnet.");

                    systemFlag = child.Properties["systemFlags"].Value.ToString();
                    systemFlagVal = ParseSystemFlagsValue("FLAG_CONFIG_ALLOW_RENAME");
                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        systemFlagVal.ToString(),
                        systemFlag,
                        464,
                        "The SystemFlags attribute of the Subnet must be FLAG_CONFIG_ALLOW_RENAME.");
                }
                //MS-ADTS-Schema_R476,MS-ADTS-Schema_R477 and MS-ADTS-Schema_R478
                subnetChilds = childEntry.Children;
                foreach (DirectoryEntry child in subnetChilds)
                {
                    if (child.SchemaClassName.ToLower() == "siteLinkBridge".ToLower())
                    {
                        DataSchemaSite.CaptureRequirementIfIsTrue(
                            child.Properties["objectClass"].Contains("siteLinkBridge"),
                            476,
                            "The ObjectClass attribute of the Site Link Bridge Object must be siteLinkBridge.");
                        systemFlag = child.Properties["systemFlags"].Value.ToString();
                        systemFlagVal = ParseSystemFlagsValue("FLAG_CONFIG_ALLOW_RENAME");
                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                            systemFlagVal.ToString(),
                            systemFlag,
                            477,
                            "The SystemFlags attribute of the Site Link Bridge Object must be FLAG_CONFIG_ALLOW_RENAME.");
                        DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        "CN=IP",
                        child.Parent.Name.ToString(),
                        478,
                        @"The Parent of the Site Link Bridge Object must be 
                        Either IP Transport container or SMTP Transport container.");
                    }
                }

                if (!AdLdapClient.Instance().DeleteObject(testLinkBridgeDistinguishedName, null).Equals("Success_STATUS_SUCCESS"))
                {
                    DataSchemaSite.Assume.Fail("The created dynamic object" + testLinkBridgeDistinguishedName + " can not be deleted.");
                }
                if(!AdLdapClient.Instance().DeleteObject(testLinkDistinguishedName,null).Equals("Success_STATUS_SUCCESS"))
                {
                    DataSchemaSite.Assume.Fail("The created dynamic object" + testLinkDistinguishedName + " can not be deleted.");
                }
                if(!AdLdapClient.Instance().DeleteObject(testDistinguishedName,null).Equals("Success_STATUS_SUCCESS"))
                {
                    DataSchemaSite.Assume.Fail("The created dynamic object" + testDistinguishedName + " can not be deleted.");
                }
            }
            else
            {
                DataSchemaSite.Assume.Fail("It's failed to connect and bind to AD/DS ");
            }
            AdLdapClient.Instance().Unbind();

            //MS-ADTS-Schema_R472
            domainEntry = childEntry.Children.Find("CN=DEFAULTIPSITELINK");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=IP",
                domainEntry.Parent.Name.ToString(),
                472,
                @"The Parent of the Site Link Bridge Object must be 
                Either IP Transport container or SMTP Transport container.");

            //MS-ADTS-Schema_R473
            DataSchemaSite.CaptureRequirementIfIsTrue(
                domainEntry.Properties["objectClass"].Contains((object)"siteLink"),
                473,
                "The ObjectClass attribute of the Site Link Object must be siteLink.");

            //MS-ADTS-Schema_R474
            systemFlag = domainEntry.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_CONFIG_ALLOW_RENAME");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                474,
                "The SystemFlags attribute of the Site Link Object must be FLAG_CONFIG_ALLOW_RENAME.");
            if (systemFlag != (systemFlagVal.ToString()))
                isParseSystemFlagsValue = false;

            if (!adAdapter.GetObjectByDN("CN=Inter-Site Transports,CN=Sites," + configNC, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Inter-Site Transports,CN=Sites,"
                    + configNC
                    + " Object is not found in server");
            }
            //MS-ADTS-Schema_R470
            childEntry = dirPartitions.Children.Find("CN=SMTP");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Inter-Site Transports",
                childEntry.Parent.Name.ToString(),
                470,
                "The Parent of the SMTP Transport Container must be Inter-Site Transports container.");

            //MS-ADTS-Schema_R471
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childEntry.Properties["objectClass"].Contains((object)"interSiteTransport"),
                471,
                "The ObjectClass attribute of the SMTP Transport Container must be interSiteTransport.");
        }

        #endregion

        #region LDSSitesContainer Implementation.

        // <summary>
        /// This method validates the requirements under 
        /// LDSSitesContainer Scenario.
        /// </summary>  
        public void ValidateLDSSitesContainer()
        {
            DirectoryEntry dirPartitions = new DirectoryEntry();
            DirectoryEntry domainEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();
            string systemFlag;
            int systemFlagVal;
            string configNCForLDS = "CN=Configuration," + adAdapter.LDSRootObjectName;
            string SchemaNC = "CN=Schema," + configNCForLDS;

            //MS-ADTS-Schema_R423
            if (!adAdapter.GetLdsObjectByDN("CN=Sites," + configNCForLDS, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Sites,"
                    + configNCForLDS
                    + " Object is not found in server");
            }
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Configuration",
                dirPartitions.Parent.Name.ToString(),
                423,
                "Each forest contains a Sites container in the Config NC.");

            //MS-ADTS-Schema_R425           
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Configuration",
                dirPartitions.Parent.Name.ToString(),
                425,
                "The parent of the Sites Container must be Config NC root object.");


            //MS-ADTS-Schema_R426
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirPartitions.Properties["objectClass"].Contains((object)"sitesContainer"),
                426,
                "The objectClass attribute of the Sites Container must be sitesContainer.");

            //MS-ADTS-Schema_R427
            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE|FLAG_DISALLOW_MOVE_ON_DELETE");
            if (systemFlag != (systemFlagVal.ToString()))
                isParseSystemFlagsValue = false;
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                427,
                @"The SytemFlags attribute of the Sites Container must be 
                either of FLAG_DISALLOW_DELETE|FLAG_DISALLOW_MOVE_ON_DELETE.");
            if (systemFlag != (systemFlagVal.ToString()))
                isParseSystemFlagsValue = false;

            //MS-ADTS-Schema_R428
            DirectoryEntries dirEntriesForValue = dirPartitions.Children;
            bool isParentRoles = false;
            foreach (DirectoryEntry child in dirEntriesForValue)
            {
                if (child.Properties["objectCategory"].Value.ToString().ToLower().Contains("cn=site"))
                {
                    isParentRoles = true;
                }
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isParentRoles,
                428,
                "The parent of the Site Object must be Sites container.");

            //MS-ADTS-Schema_R429
            childEntry = dirPartitions.Children.Find("CN=Default-First-Site-Name");
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childEntry.Properties["objectClass"].Contains((object)"site"),
                429,
                "The objectClass attribute of the Site Object must be Site.");

            //MS-ADTS-Schema_R430
            systemFlag = childEntry.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_CONFIG_ALLOW_RENAME|FLAG_DISALLOW_MOVE_ON_DELETE");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                430,
                @"The SytemFlags attribute of the Site Object must be 
                either of FLAG_CONFIG_ALLOW_RENAME|FLAG_DISALLOW_MOVE_ON_DELETE.");
            if (systemFlag != (systemFlagVal.ToString()))
                isParseSystemFlagsValue = false;

            //MS-ADTS-Schema_R457
            if (!adAdapter.GetLdsObjectByDN("CN=Subnets,CN=Sites," + configNCForLDS, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Subnets,CN=Sites,"
                    + configNCForLDS
                    + " Object is not found in server");
            }
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Sites",
                dirPartitions.Parent.Name.ToString(),
                457,
                "Each forest contains a Subnets container in the config NC.");

            //MS-ADTS-Schema_R458
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Sites", dirPartitions.Parent.Name.ToString(),
                458,
                "The Parent of the Subnet container must be Sites container.");

            //MS-ADTS-Schema_R459
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirPartitions.Properties["objectClass"].Contains((object)"subnetContainer"),
                459,
                "The ObjectClass attribute of the Subnet container must be subnetContainer.");

            //MS-ADTS-Schema_R460

            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                460,
                "The SystemFlags attribute of the Subnet container must be FLAG_DISALLOW_DELETE.");
            if (systemFlag != (systemFlagVal.ToString()))
                isParseSystemFlagsValue = false;

            //MS-ADTS-Schema_R465
            if (!adAdapter.GetLdsObjectByDN("CN=Inter-Site Transports,CN=Sites," + configNCForLDS, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Inter-Site Transports,CN=Sites,"
                    + configNCForLDS
                    + " Object is not found in server.");
            }
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Sites", dirPartitions.Parent.Name.ToString(),
                465,
                "The Parent of the IP Transport Container must be Inter-Site Transports container.");

            //MS-ADTS-Schema_R466
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirPartitions.Properties["objectClass"].Contains((object)"interSiteTransportContainer"),
                466,
                "The ObjectClass attribute of the Inter-Site Transports Container must be interSiteTransportContainer.");

            //MS-ADTS-Schema_R467
            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                467,
                "The SystemFlags attribute of the Inter-Site Transports Container must be FLAG_DISALLOW_DELETE.");
            if (systemFlag != (systemFlagVal.ToString()))
                isParseSystemFlagsValue = false;

            //MS-ADTS-Schema_R468 
            childEntry = dirPartitions.Children.Find("CN=IP");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=Inter-Site Transports",
                childEntry.Parent.Name.ToString(),
                468,
                "The Parent of the IP Transport Container must be Inter-Site Transports container.");

            //MS-ADTS-Schema_R469
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childEntry.Properties["objectClass"].Contains((object)"interSiteTransport"),
                469,
                "The ObjectClass attribute of the IP Transport Container must be interSiteTransport.");

            //MS-ADTS-Schema_R472
            domainEntry = childEntry.Children.Find("CN=DEFAULTIPSITELINK");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=IP",
                domainEntry.Parent.Name.ToString(),
                472,
                @"The Parent of the Site Link Bridge Object must be 
                Either IP Transport container or SMTP Transport container.");

            //MS-ADTS-Schema_R473
            DataSchemaSite.CaptureRequirementIfIsTrue(
                domainEntry.Properties["objectClass"].Contains((object)"siteLink"),
                473,
                "The ObjectClass attribute of the Site Link Object must be siteLink.");
        }

        #endregion
    }
}