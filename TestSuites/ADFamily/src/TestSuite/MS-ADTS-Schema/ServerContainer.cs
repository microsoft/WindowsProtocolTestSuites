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

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase26 and TestCase27.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        //Creating common Directory Entry for specified partition
        DirectoryEntry dirPartitions = new DirectoryEntry();
        //Creating Directory Entry for Sites
        DirectoryEntry sitesEntry = new DirectoryEntry();
        //Creating Directory Entry for the parent of Servers Container
        DirectoryEntry serverParentEntry = new DirectoryEntry();
        //Creating Directory Entry for Servers Container
        DirectoryEntry serverEntry = new DirectoryEntry();
        //Creating Directory Entry
        DirectoryEntry entry = new DirectoryEntry();

        bool isParent = true;
        bool isObjectClass = true;
        bool isParseSystemFlagsValue = true;
        bool isTrue = false;
        bool isEnabledconnection = true;
        bool isOptions = true;
        bool isRefertontdsdsa = true;
        bool isSchedule = true;

        //String currDomain = adAdapter.adAdapter.rootDomainDN;
        string parent, nonRODCConnection, systemFlag, nTDSName = null, dnName = null;
        PropertyValueCollection objectClass;
        int systemFlagVal;

        #region Validation of ServerContainer

        /// <summary>
        /// This method validates the requirements under 
        /// ServerContainer  Scenario.
        /// </summary>
        public void ValidateServerContainer()
        {
            isDS = true;
            CaptureServerContainer();
        }

        #endregion

        #region Validation of LDSServerContainer

        /// <summary>
        /// This method validates the requirements under 
        /// LDSServerContainer Scenario.
        /// </summary>        
        public void ValidateLDSServerContainer()
        {
            isDS = false;
            CaptureServerContainer();
        }

        #endregion

        #region Common Validation

        /// <summary>
        /// CaptureServerContainer is a Common method for Both AD/DS and AD/LDS Server Container Requirements Validation
        /// Retrieves the required Directory Entry for a specified DN and its attributes.
        /// </summary>
        public void CaptureServerContainer()
        {
            if (isDS)
            {
                if (!adAdapter.GetObjectByDN("CN=Sites,CN=Configuration," + adAdapter.rootDomainDN, out sitesEntry))
                {
                    DataSchemaSite.Assume.IsTrue(
                        false,
                        "CN=Sites,CN=Configuration,"
                        + adAdapter.rootDomainDN
                        + " Object is not found in server");
                }
            }
            else
            {
                if (!adAdapter.GetLdsObjectByDN("CN=Sites,CN=Configuration," + adAdapter.LDSRootObjectName, out sitesEntry))
                {
                    DataSchemaSite.Assume.IsTrue(
                        false,
                        "CN=Sites,CN=Configuration,"
                        + adAdapter.LDSRootObjectName
                        + " Object is not found in server");
                }
            }

            DirectoryEntries sitesChildern = sitesEntry.Children;
            foreach (DirectoryEntry child in sitesChildern)
            {
                if (child.Properties["objectClass"].Contains((object)"site") && child.Name.Equals("CN=Default-First-Site-Name"))
                {
                    serverParentEntry = child.Children.Find("CN=Servers");
                    //MS-ADTS-Schema_R433
                    // [Since objectClass of the child is a site object, R433 is captured.]
                    DataSchemaSite.CaptureRequirement(
                        433,
                        "The parent of the Servers Container must be site object.");
                }
            }

            if (isDS)
            {
                string dnServers = serverParentEntry.Properties["distinguishedName"].Value.ToString();
                if (!adAdapter.GetObjectByDN(dnServers, out dirPartitions))
                {
                    DataSchemaSite.Assume.IsTrue(false, dnServers + " Object is not found in server");
                }
            }
            parent = serverParentEntry.Parent.Name;

            objectClass = serverParentEntry.Properties["objectClass"];
            //MS-ADTS-Schema_R434
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"serversContainer"),
                434,
                "The ObjectClass Attribute of the Servers Container must be serversContainer.");

            systemFlag = serverParentEntry.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_MOVE_ON_DELETE");
            //MS-ADTS-Schema_R435
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                435,
                "The systemFlags Attribute of the Servers Container must be FLAG_DISALLOW_MOVE_ON_DELETE.");
            DirectoryEntries serverChildren = null;
            foreach (DirectoryEntry sitechild in sitesChildern)
            {
                if (sitechild.Properties["objectClass"].Contains((object)"site") && sitechild.Name.Equals("CN=Default-First-Site-Name"))
                {
                    serverParentEntry = sitechild.Children.Find("CN=Servers");
                    serverChildren = serverParentEntry.Children;
                    foreach (DirectoryEntry child in serverChildren)
                    {
                        if (!child.Parent.Name.Equals("CN=Servers"))
                        {
                            isParent = false;
                        }
                        objectClass = child.Properties["objectClass"];
                        if (!objectClass.Contains((object)"server"))
                        {
                            isObjectClass = false;
                        }

                        systemFlag = child.Properties["systemFlags"].Value.ToString();
                        systemFlagVal = ParseSystemFlagsValue("FLAG_CONFIG_ALLOW_RENAME|FLAG_CONFIG_ALLOW_LIMITED_MOVE|FLAG_DISALLOW_MOVE_ON_DELETE");
                        if (systemFlag != (systemFlagVal.ToString()))
                        {
                            isParseSystemFlagsValue = false;
                        }

                    }
                }
            }

            //MS-ADTS-Schema_R436
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isParent,
                436,
                "The parent of the Server Object must be servers Container object.");

            //MS-ADTS-Schema_R437
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClass,
                437,
                "The ObjectClass Attribute of the Server Object must be Server.");
            //MS-ADTS-Schema_R438

            DataSchemaSite.CaptureRequirementIfIsTrue(
                isParseSystemFlagsValue,
                438,
                "The systemFlags Attribute of the Server Object must be either of" +
                "FLAG_CONFIG_ALLOW_RENAME | FLAG_CONFIG_ALLOW_LIMITED_MOVE | FLAG_DISALLOW_MOVE_ON_DELETE.");

            if (isDS)
            {
                if (!adAdapter.GetObjectByDN("OU=Domain Controllers," + adAdapter.rootDomainDN, out sitesEntry))
                {
                    DataSchemaSite.Assume.IsTrue(
                        false,
                        "OU=Domain Controllers,"
                        + adAdapter.rootDomainDN
                        + " Object is not found in server");
                }
                DirectoryEntries domainChildren = sitesEntry.Children;
                isObjectClass = true;
                foreach (DirectoryEntry childDomain in domainChildren)
                {
                    string cnName = childDomain.Properties["cn"].Value.ToString();
                    foreach (DirectoryEntry sitechild in sitesChildern)
                    {
                        if (sitechild.Properties["objectClass"].Contains((object)"site"))
                        {
                            if (sitechild.Name.Equals("CN=Default-First-Site-Name"))
                            {
                                serverParentEntry = sitechild.Children.Find("CN=Servers");
                                serverChildren = serverParentEntry.Children;
                                foreach (DirectoryEntry child in serverChildren)
                                {
                                    serverEntry = serverParentEntry.Children.Find("CN=" + cnName);
                                    DirectoryEntries serverEntryChildern = serverEntry.Children;
                                    foreach (DirectoryEntry serverEntryChild in serverEntryChildern)
                                    {
                                        PropertyValueCollection objClass = serverEntryChild.Properties["objectClass"];
                                        dnName = serverEntryChild.Properties["distinguishedName"].Value.ToString();
                                        if (dnName.StartsWith("CN=NTDS Settings"))
                                        {
                                            if (dnName.Contains(adAdapter.PDCNetbiosName))
                                            {
                                                nTDSName = dnName;
                                            }
                                            if (!objClass.Contains((object)"nTDSDSA"))
                                            {
                                                isObjectClass = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                bool hasDNSHostName = false;
                string dnsHostName = null;
                string[] hostName = null;
                string domainStr = null;
                foreach (DirectoryEntry child in serverChildren)
                {
                    dnsHostName = child.Properties["dnsHostName"].Value.ToString().ToLower();
                    hostName = dnsHostName.Split('.');
                    domainStr = dnsHostName.Substring(hostName[0].Length + 1, dnsHostName.Length - (hostName[0].Length + 1));
                    if (hostName[0].Equals(adAdapter.PDCNetbiosName.ToLower()))
                    {
                        hasDNSHostName = true;
                        break;
                    }
                }

                //MS-ADTS-Schema_R439
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    hasDNSHostName
                    && (domainStr.ToLower().Equals(adAdapter.PrimaryDomainDnsName.ToLower())),
                    439,
                    "The dNSHostName Attribute of the Server Object must be fully qualified DNS name of the DC.");

                //MS-ADTS-Schema_R440
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isObjectClass,
                    440,
                    "Each DC in a domain has an nTDSDSA object in the config NC.");

                if (!adAdapter.GetObjectByDN(nTDSName, out dirPartitions))
                {
                    DataSchemaSite.Assume.IsTrue(false, nTDSName + " Object is not found in server");
                }

                string dMDLocation = dirPartitions.Properties["dMDLocation"].Value.ToString();
                //MS-ADTS-Schema_R444
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    ("CN=Schema,CN=Configuration,"
                    + adAdapter.rootDomainDN).ToLower(), dMDLocation.ToLower(),
                    444,
                    "The dMDLocation Attribute of the nTDSDSA Object must be dsname of the schema NC root.");
            }
            else
            {
                foreach (DirectoryEntry serverEntryChild in serverChildren)
                {
                    DirectoryEntries serverEntryChildsChild = serverEntryChild.Children;
                    foreach (DirectoryEntry serverChild in serverEntryChildsChild)
                    {
                        string dnsHostName = serverEntryChild.Properties["dNSHostName"].Value.ToString().ToLower();
                        string[] hostName = dnsHostName.Split('.');
                        string domainStr = dnsHostName.Substring(hostName[0].Length + 1, dnsHostName.Length - (hostName[0].Length + 1));
                        if ((hostName[0].ToLower().Equals(adAdapter.PDCNetbiosName.ToLower())) && (domainStr.ToLower().Equals(adAdapter.PrimaryDomainDnsName.ToLower())))
                        {
                            isTrue = true;
                        }
                        dnName = serverChild.Properties["distinguishedName"].Value.ToString();
                        if (dnName.StartsWith("CN=NTDS Settings"))
                        {
                            nTDSName = dnName;
                        }
                    }
                }
                //MS-ADTS-Schema_R439
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isTrue,
                    439,
                    "The dNSHostName Attribute of the Server Object must be fully qualified DNS name of the DC.");

                if (!adAdapter.GetLdsObjectByDN(nTDSName, out dirPartitions))
                {
                    DataSchemaSite.Assume.IsTrue(false, nTDSName + " Object is not found in server");
                }

                string dMDLocation = dirPartitions.Properties["dMDLocation"].Value.ToString();
                //MS-ADTS-Schema_R444
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "CN=Schema,CN=Configuration,"
                    + adAdapter.LDSRootObjectName, dMDLocation,
                    444,
                    "The dMDLocation Attribute of the nTDSDSA Object must be dsname of the schema NC root.");
            }

            string name = dirPartitions.Properties["name"].Value.ToString();
            //MS-ADTS-Schema_R441
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "NTDS Settings",
                name,
                441,
                "The name Attribute of the nTDSDSA Object must be NTDS Settings.");

            objectClass = dirPartitions.Parent.Properties["objectClass"];
            //MS-ADTS-Schema_R442
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"server"),
                442,
                "The parent of the nTDSDSA Object must be an object with objectClass server.");

            objectClass = dirPartitions.Properties["objectClass"];
            //MS-ADTS-Schema_R443
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"nTDSDSA"),
                443,
                "The ObjectClass Attribute of the nTDSDSA Object must be nTDSDSA.");

            //MS-ADTS-Schema_R445
            PropertyValueCollection allNCs = null;
            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_MOVE_ON_DELETE");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                445,
                "The SystemFlags attribute of the nTDSDSA Object must be FLAG_DISALLOW_MOVE_ON_DELETE.");

            if (isDS)
            {
                entry = new DirectoryEntry("LDAP://" + adAdapter.PDCNetbiosName + "/rootDSE");
            }
            else
            {
                entry = new DirectoryEntry("LDAP://" + adAdapter.adamServerPort + "/rootDSE");

                try
                {
                    allNCs = entry.Properties["namingContexts"];
                }
                catch (Exception)
                {
                    entry = new DirectoryEntry("LDAP://" + adAdapter.adamServerPort + "/rootDSE");
                }
            }
            allNCs = entry.Properties["namingContexts"];//namingContexts
            PropertyValueCollection hasMasterNCs = dirPartitions.Properties["hasMasterNCs"];
            PropertyValueCollection msDSHasMasterNCs = dirPartitions.Properties["msDS-hasMasterNCs"];
            PropertyValueCollection msDSHasFullReplicasNCs;
            bool isHasMasterNCs = true, ismsDSHasMasterNCs = true, ismsDSHasFullReplicasNCs = true;
            for (int index = 0; index < hasMasterNCs.Count; index++)
            {
                if (!allNCs.Contains(hasMasterNCs[index]))
                {
                    isHasMasterNCs = false;
                }
            }
            for (int index = 0; index < msDSHasMasterNCs.Count; index++)
            {
                if (!allNCs.Contains(msDSHasMasterNCs[index]))
                {
                    ismsDSHasMasterNCs = false;
                }
            }

            //MS-ADTS-Schema_R449
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isHasMasterNCs,
                449,
                @"hasMasterNCs attribute in nTDSDSA Object Contains the dsnames of the NC root objects representing 
                the schema NC, config NC,and domain NC for the default domain of the DC. This attribute always contains 
                these three values, and only these three values.");

            //MS-ADTS-Schema_R451
            DataSchemaSite.CaptureRequirementIfIsTrue(
                ismsDSHasMasterNCs,
                451,
                @"msDS-hasMasterNCs attribute in nTDSDSA Object Contains the dsnames of the root objects of all 
                writable NC replicas hosted by the DC.");

            if (isDS)
            {
                if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out entry))
                {
                    DataSchemaSite.Assume.IsTrue(false, adAdapter.rootDomainDN + " Object is not found in server");
                }
                PropertyValueCollection msds = entry.Properties["distinguishedName"];
                PropertyValueCollection msDSHasDomainNCs = dirPartitions.Properties["msDS-HasDomainNCs"];

                //MS-ADTS-Schema_R450
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    msds.Value.ToString() == msDSHasDomainNCs.Value.ToString(),
                    450,
                    @"msDS-HasDomainNCs attribute in nTDSDSA Object Equals to the dsname of the NC root object for which 
                    the DC is hosting a regular NC replica. This attribute must have only one value. ");
            }
            if (isDS)
            {
                isParseSystemFlagsValue = true;
                isParent = true;
                isObjectClass = true;
                bool isNotExistingRODC = true;
                //The value optionsvalue=0x41 can be got from TD [MS-ADTS] 7.1.1.2.2.1.2.1.3
                const int optionsvalue = 0x41;

                foreach (DirectoryEntry serverEntryChild in serverChildren)
                {
                    if (serverEntryChild.Name.ToLower() == ("CN=" + adAdapter.RODCNetbiosName).ToLower())
                    {
                        DirectoryEntries nTDSEntries = serverEntryChild.Children;
                        foreach (DirectoryEntry nTDSEntry in nTDSEntries)
                        {
                            msDSHasFullReplicasNCs = nTDSEntry.Properties["msDS-hasFullReplicaNCs"];
                            for (int index = 0; index < hasMasterNCs.Count; index++)
                            {
                                if (!allNCs.Contains(msDSHasFullReplicasNCs[index]))
                                {
                                    ismsDSHasFullReplicasNCs = false;
                                }
                            }
                            //Checking For the Parent equal to Servers
                            if (!nTDSEntry.Properties["objectClass"].Contains((object)"nTDSDSA"))
                            {
                                isParent = false;
                            }
                            foreach (DirectoryEntry nTDSConnectionObjects in nTDSEntry.Children)
                            {
                                //Check schedule attribute
                                if (nTDSConnectionObjects.Properties["schedule"].Value != null)
                                {
                                    bool verifySuccessed = VerifyScheduleStructure((byte[])nTDSConnectionObjects.Properties["schedule"].Value);
                                    if (verifySuccessed != true)
                                    {
                                        isSchedule = false;
                                    }
                                }
                                objectClass = nTDSConnectionObjects.Properties["objectClass"];
                                if (!objectClass.Contains((object)"nTDSConnection"))
                                {
                                    isObjectClass = false;
                                }
                                string s = (string)nTDSConnectionObjects.Properties["fromServer"].Value;
                                if (!s.Contains("CN=NTDS Settings"))
                                {
                                    isRefertontdsdsa = false;
                                }
                                //Checking For the attribute options.
                                if (optionsvalue != ((int)(nTDSConnectionObjects.Properties["options"].Value)))
                                {
                                    isOptions = false;
                                }
                                //Checking For the name equals to RODC Connection.
                                name = nTDSConnectionObjects.Properties["name"].Value.ToString();
                                if ((bool)(nTDSConnectionObjects.Properties["enabledConnection"].Value) != true)
                                {
                                    isEnabledconnection = false;
                                }

                                systemFlag = nTDSConnectionObjects.Properties["systemFlags"].Value.ToString();
                                systemFlagVal = ParseSystemFlagsValue("FLAG_CONFIG_ALLOW_RENAME");
                                if (systemFlag != (systemFlagVal.ToString()))
                                {
                                    isParseSystemFlagsValue = false;
                                }
                            }
                        }
                    }
                    if (serverEntryChild.Name.ToLower() == ("CN=" + adAdapter.PDCNetbiosName.ToLower()))
                    {
                        DirectoryEntries nTDSDCEntries = serverEntryChild.Children;
                        foreach (DirectoryEntry nTDSDCEntry in nTDSDCEntries)
                        {
                            foreach (DirectoryEntry nTDSConnectionDCObjects in nTDSDCEntry.Children)
                            {
                                nonRODCConnection = nTDSConnectionDCObjects.Properties["name"].Value.ToString();
                                if ((serverOS == OSVersion.WinSvr2012 || serverOS == OSVersion.WinSvr2012R2) && nonRODCConnection.Equals("RODC Connection (SYSVOL)"))
                                {
                                    isNotExistingRODC = false;
                                }
                                else if ((serverOS == OSVersion.WinSvr2008 || serverOS == OSVersion.WinSvr2008R2) && nonRODCConnection.Equals("RODC Connection (FRS)"))
                                {
                                    isNotExistingRODC = false;
                                }
                            }
                        }

                    }
                }

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4511
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isRefertontdsdsa,
                    4511,
                    @"[Each RODC NTFRS connection object has the following attributes:]fromServer: A reference to the
                    nTDSDSA object of the source DC.");

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4512
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isSchedule,
                    4512,
                    @"[Each RODC NTFRS connection object has the following attributes:]schedule: Contains a SCHEDULE 
                    structure that specifies the time intervals when replication can be performed between the source 
                    and the destination DCs.");

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4504
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isNotExistingRODC,
                    4504,
                    @"RODC NTFRS connection objects do not exist for DCs.");

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4503
                if (serverOS >= OSVersion.WinSvr2012)
                {
                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                           "RODC Connection (SYSVOL)",
                           name,
                           4503,
                           @"An RODC NTFRS connection object exists for each RODC in the forest.");
                }
                else
                {
                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        "RODC Connection (FRS)",
                        name,
                        4503,
                        @"An RODC NTFRS connection object exists for each RODC in the forest.");
                }
                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4508
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isParent,
                    4508,
                    @"[Each RODC NTFRS connection object has the following attributes:]parent: nTDSDSA object.");

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4506
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isParent,
                    4506,
                    @"This object[RODC NTFRS Connection Object] is a child of the nTDSDSA object of the 
                    destination RODC.");

                //MS-ADTS-Schema_R455
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isObjectClass,
                    455,
                    "The objectClass attribute of the Connection object must be nTDSConnection.");

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4510
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isEnabledconnection,
                    4510,
                    @"[Each RODC NTFRS connection object has the following attributes:]enabledConnection: true.");
                //
                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4513
                //
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isParseSystemFlagsValue,
                    4513,
                    @"[Each RODC NTFRS connection object has the following attributes:]systemFlags: 
                    {FLAG_CONFIG_ALLOW_RENAME}.");

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4509
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isObjectClass,
                    4509,
                    @"[Each RODC NTFRS connection object has the following attributes:]objectClass: nTDSConnection.");

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4507
                if (serverOS >= OSVersion.WinSvr2012)
                {
                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        "RODC Connection (SYSVOL)",
                        name,
                        4507,
                        @"[Each RODC NTFRS connection object has the following attributes:]name: RODC Connection (SYSVOL).");
                }
                else
                {
                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        "RODC Connection (FRS)",
                        name,
                        4507,
                        @"[Each RODC NTFRS connection object has the following attributes:]name: RODC Connection (FRS).");
                }
                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4514
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isOptions,
                    4514,
                    @"[Each RODC NTFRS connection object has the following attributes:]options: Both of the bits from 
                    the following diagram[The 26th bit is RT, the last bit is IG, and the rest are X]. ");

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4517
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isOptions,
                    4517,
                    @"[In RODC NTFRS connection object, options attribute]X: Must be zero and ignored.");

            }

            //MS-ADTS-Schema_R452
            DataSchemaSite.CaptureRequirementIfIsTrue(
                ismsDSHasFullReplicasNCs,
                452,
                @"msDS-hasFullReplicaNCs attribute in nTDSDSA Object Contains the dsnames of the root objects of all 
                writable NC replicas hosted by the DC.");

            //MS-ADTS-Schema_R454
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isParent,
                454,
                "The parent of the Connection object must be nTDSDSA object.");

            //MS-ADTS-Schema_R456
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isParseSystemFlagsValue,
                456,
                @"The SystemFlags attribute of the Connection object
                    must be FLAG_CONFIG_ALLOW_RENAME | FLAG_CONFIG_ALLOW_MOVE.");
        }

        #endregion
    }
}