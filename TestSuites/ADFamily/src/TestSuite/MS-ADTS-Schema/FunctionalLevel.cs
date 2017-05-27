// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Text;

using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase24 and TestCase25.
    /// </summary>
    public partial class DataSchemaTestSuite
    {

        #region FunctionalLevel Implementation.

        /// <summary>
        /// This method validates the requirements under 
        ///  msDS-Behavior-Version Functional Levels such as Forest Functional level
        ///  DC Functional Level, Domain Functional level Scenario.
        ///  And the sub sections of Extended Rights 7.1.2.7.63 to 7.1.2.7.70
        /// </summary>  
        public void ValidateFunctionalLevel()
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

            #region msDS-Behavior-Version

            //MS-AD_Schema_R948
            //Here the Partitions Container is the crossRefContainer object.
            if (!adAdapter.GetObjectByDN("CN=Partitions," + configNC, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(false, "CN=Partitions," + configNC + " Object is not found in server");
            }

            string msDSBehaviorVersion = dirPartitions.Properties["msDS-Behavior-Version"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfIsNotNull(
                msDSBehaviorVersion,
                948,
                "The msDS-Behavior-Version for the forest is written on the crossRefContainer object.");

            //MS-AD_Schema_R954
            if (serverOS == OSVersion.WinSvr2003)
            {
                //Here has made choice by "if-else",if it is not Server2k3,
                //else it may be Win7,server2k8,Win2008R2 
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "2",
                    msDSBehaviorVersion,
                    954,
                    @"If the msDS-Behavior-Version for the forest with identifier DS_BEHAVIOR_WIN2003 and
                    Domain controller operating systems that are allowed in the forest are Windows Server 2003 , 
                    Windows Server 2008 , Windows Server 2008 R2 then the value is 2.");
            }
            else if (serverOS == OSVersion.WinSvr2008)
            {
                //Server2k8 contains both R1 and R2, and R2 is related with Win7,
                //For server2k8 verified here must except the Win7.
                //MS-AD_Schema_R955
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "3",
                    msDSBehaviorVersion,
                    955,
                    @"If the msDS-Behavior-Version for the forest with identifier DS_BEHAVIOR_WIN2008 and
                    Domain controller operating systems that are allowed in the forest are Windows Server 2008 , 
                    Windows Server 2008 R2 then the value is 3.");
            }
            else if (serverOS == OSVersion.WinSvr2008R2)
            {

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4539
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4",
                    msDSBehaviorVersion,
                    4539,
                    @"If the msDS-Behavior-Version for the forest with identifier DS_BEHAVIOR_WIN2008R2 and Domain 
                    controller operating systems that are allowed in the forest is Windows Server 2008 R2 then 
                    the value is 4.");
            }
            else if (serverOS == OSVersion.WinSvr2012)
            {
                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4539
                DataSchemaSite.Assert.AreEqual<string>(
                    "5",
                    msDSBehaviorVersion,
                    @"If the msDS-Behavior-Version for the forest with identifier DS_BEHAVIOR_WIN2012 and Domain 
                    controller operating systems that are allowed in the forest is Windows Server 2012 then 
                    the value is 5.");
            }
            else if (serverOS == OSVersion.WinSvr2012R2)
            {
                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4539
                DataSchemaSite.Assert.AreEqual<string>(
                    "6",
                    msDSBehaviorVersion,
                    @"If the msDS-Behavior-Version for the forest with identifier DS_BEHAVIOR_WIN2012R2 and Domain 
                    controller operating systems that are allowed in the forest is Windows Server 2012 R2 then 
                    the value is 6.");
            }

            if (!adAdapter.GetObjectByDN(currDomain, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(false, currDomain + " Object is not found in server");
            }

            string msDSBehaviorVersionOfDomainNC = dirPartitions.Properties["msDS-Behavior-Version"].Value.ToString();

            if (serverOS == OSVersion.WinSvr2003)
            {
                //Here has made choice by "if-else",if it is not Server2k3,
                //else it may be Win7,server2k8,Win2008R2 
                //MS-AD_Schema_R946
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "2",
                    msDSBehaviorVersionOfDomainNC,
                    946,
                    "If the msDS-Behavior-Version for domains with identifier DS_BEHAVIOR_WIN2003 and Domain "
                + "controller operating systems that are allowed in the domain are Windows Server 2003, "
                + "Windows Server 2008, Windows Server 2008 R2 then the value is 2.");
            }
            else if (serverOS == OSVersion.WinSvr2008)
            {
                //Server2k8 contains both R1 and R2, and R2 is related with Win7,
                //For server2k8 verified here must except the Win7.              
                //MS-AD_Schema_R947
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "3",
                    msDSBehaviorVersionOfDomainNC,
                    947,
                    @"If the msDS-Behavior-Version for domains with identifier DS_BEHAVIOR_WIN2008 and Domain controller
                    operating systems that are allowed in the domain are Windows Server 2008,Windows Server 2008 R2 
                    then the value is 3.");
            }
            else if (serverOS == OSVersion.WinSvr2008R2)
            {
                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4538
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4",
                    msDSBehaviorVersionOfDomainNC,
                    4538,
                    @"If the msDS-Behavior-Version for domains with identifier DS_BEHAVIOR_WIN2008R2 and Domain 
                    controller operating systems that are allowed in the domain is Windows Server 2008 R2 then the  
                    value is 4.");
            }
            else if (serverOS == OSVersion.WinSvr2012)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "5",
                    msDSBehaviorVersionOfDomainNC,
                    4538,
                    @"If the msDS-Behavior-Version for domains with identifier DS_BEHAVIOR_WIN2012 and Domain 
                    controller operating systems that are allowed in the domain is Windows Server 2012 then the  
                    value is 5.");
            }
            else if (serverOS == OSVersion.WinSvr2012R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "6",
                    msDSBehaviorVersionOfDomainNC,
                    4538,
                    @"If the msDS-Behavior-Version for domains with identifier DS_BEHAVIOR_WIN2012R2 and Domain 
                    controller operating systems that are allowed in the domain is Windows Server 2012 R2 then the  
                    value is 6.");
            }

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
                if ((child.Properties["Name"].Value.ToString().ToLower()) == ("Servers".ToLower()))
                {
                    childEntry = child.Children.Find("CN=" + adAdapter.PDCNetbiosName);
                    DirectoryEntries childChildren = childEntry.Children;
                    foreach (DirectoryEntry child1 in childChildren)
                    {
                        if (child1.Properties["objectClass"].Contains((object)"nTDSDSA"))
                        {
                            string msDSBehaviorVersionOfnTDSDSA = child1.Properties["msDS-Behavior-Version"].Value.ToString();

                            if (serverOS == OSVersion.WinSvr2003)
                            {
                                //Here has made choice by "if-else",if it is not Server2k3,
                                //else it may be Win7,server2k8,Win2008R2 
                                //MS-AD_Schema_R938
                                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                                    "2",
                                    msDSBehaviorVersionOfnTDSDSA,
                                    938,
                                    "If the msDS-Behavior-Version is written on the nTDSDSA object representing" +
                                    "a DC with identifier DS_BEHAVIOR_WIN2003 then the value is 2.");
                            }
                            else if (serverOS == OSVersion.WinSvr2008)
                            {
                                //Server2k8 contains both R1 and R2, and R2 is related with Win7,
                                //For server2k8 verified here must except the Win7.
                                //MS-AD_Schema_R939
                                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                                    "3",
                                    msDSBehaviorVersionOfnTDSDSA,
                                    939,
                                    "If the msDS-Behavior-Version is written on the nTDSDSA object representing" +
                                    "a DC with identifier DS_BEHAVIOR_WIN2008 then the value is 3.");
                            }
                            else if (serverOS == OSVersion.WinSvr2008R2)
                            {
                                //Verify MS-AD_Schema requirement:MS-AD_Schema_R4537
                                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                                    "4",
                                    msDSBehaviorVersionOfnTDSDSA,
                                    4537,
                                    @"If the msDS-Behavior-Version is written on the nTDSDSA object representing a DC 
                                    with identifier DS_BEHAVIOR_WIN2008R2 then the value is 4.");
                            }
                            else if (serverOS == OSVersion.WinSvr2012)
                            {
                                //Server2012 is function level 5
                                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                                    "5",
                                    msDSBehaviorVersionOfnTDSDSA,
                                    4537,
                                    @"If the msDS-Behavior-Version is written on the nTDSDSA object representing a DC 
                                    with identifier DS_BEHAVIOR_WIN2012 then the value is 5.");
                            }
                            else if (serverOS == OSVersion.WinSvr2012R2)
                            {
                                //Server2012 is function level 5
                                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                                    "6",
                                    msDSBehaviorVersionOfnTDSDSA,
                                    4537,
                                    @"If the msDS-Behavior-Version is written on the nTDSDSA object representing a DC 
                                    with identifier DS_BEHAVIOR_WIN2012R2 then the value is 6.");
                            }
                        }
                    }
                }
            }
            // Here the Critical System Object is LostAndFound Container
            if (!adAdapter.GetObjectByDN("CN=LostAndFound," + currDomain, out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(false, "CN=LostAndFound," + currDomain + " Object is not found in server");
            }
            systemFlag = dirPartitions.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE|FLAG_DOMAIN_DISALLOW_RENAME|FLAG_DOMAIN_DISALLOW_MOVE");
            //MS-AD_Schema_R900
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                900,
                @"The critical domain objects attribute systemFlag bit value is
                FLAG_DISALLOW_DELETE, FLAG_DOMAIN_DISALLOW_RENAME, FLAG_DOMAIN_DISALLOW_MOVE");

            #endregion

            #region Validation of SubSchema Requirements MS-AD_Schema_R865, R866, R958, R959

            SearchResponse subSchemaResponse = null;
            GetLDAPObject(
                null,
                adAdapter.PDCNetbiosName,
                "objectclass=*",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "subschemasubentry" },
                out subSchemaResponse);

            if (subSchemaResponse.Entries.Count > 0)
            {
                string val = ((string[])subSchemaResponse.Entries[0].Attributes["subschemasubentry"].GetValues(typeof(string)))[0];

                bool condition1 = (val != null);

                subSchemaResponse = null;
                GetLDAPObject(
                    val,
                    adAdapter.PDCNetbiosName,
                    "objectclass=subschema",
                    System.DirectoryServices.Protocols.SearchScope.Base,
                    new string[] { "cn", "objectClass", "objectClasses", "attributeTypes", "dITContentRules" },
                    out subSchemaResponse);

                //Capture req is true
                bool condition2 = subSchemaResponse.Entries[0].Attributes.Count == 5;
                subSchemaResponse = null;
                GetLDAPObject(
                    val,
                    adAdapter.PDCNetbiosName,
                    "objectclass=subschema",
                    System.DirectoryServices.Protocols.SearchScope.Base,
                    new string[] { "modifyTimeStamp", "createTimeStamp" },
                    out subSchemaResponse);

                bool condition3 = subSchemaResponse.Entries[0].Attributes.Count == 1;
                //MS-AD_Schema_R866
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    !subSchemaResponse.Entries[0].Attributes.Contains("createTimeStamp"),
                    866,
                    @"The subSchema 
                    object does not support the createTimeStamp attribute even though its object class derives 
                    from top, which contains the createTimeStamp attribute as part of systemMayContain. In Active 
                    Directory the subSchema class is defined to be structural rather than auxiliary.");

                subSchemaResponse = null;
                GetLDAPObject(
                    val,
                    adAdapter.PDCNetbiosName,
                    "objectclass=subschema",
                    System.DirectoryServices.Protocols.SearchScope.Base,
                    new string[] { "matchingRules", "matchingRuleUse", "dITStructureRules", "nameForms", "ldapSyntaxes" },
                    out subSchemaResponse);

                bool condition4 = subSchemaResponse.Entries[0].Attributes.Count == 0;

                condition1 = condition1 && condition2 && condition3 && condition4;
                //MS-AD_Schema_R865
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    condition1,
                    865,
                    @"Active Directory exposes a subSchema 
                    object that is pointed to by the subschemaSubentry attribute on the rootDSE. subSchema object 
                    contains the required cn, objectClass, objectClasses, and attributeTypes attributes. Additionally, 
                    it contains the dITContentRules attribute. It does not contain the matchingRules, matchingRuleUse, 
                    dITStructureRules, nameForms, or ldapSyntaxes attributes. It contains the modifyTimeStamp 
                    attribute but not the createTimeStamp attribute.");

                #region Querying subSchema's attributeTypes, extendedAttributeInfo attributes

                subSchemaResponse = null;
                GetLDAPObject(
                    val,
                    adAdapter.PDCNetbiosName,
                    "objectclass=subschema",
                    System.DirectoryServices.Protocols.SearchScope.Base,
                    new string[] { "attributeTypes", "extendedAttributeInfo" },
                    out subSchemaResponse);

                if (subSchemaResponse.Entries.Count > 0)
                {
                    condition1 = false;

                    List<string> ls = new List<string>(
                        (string[])subSchemaResponse.Entries[0].Attributes["attributeTypes"].GetValues(typeof(string)));

                    foreach (string item in ls)
                    {
                        if (item.Contains("sAMAccountName"))
                        {
                            condition1 = true;
                            break;
                        }
                    }

                    if (condition1)
                    {
                        ls = new List<string>(
                            (string[])subSchemaResponse.Entries[0].Attributes["extendedAttributeInfo"].GetValues(typeof(string)));

                        foreach (string item in ls)
                        {
                            if (item.Contains("sAMAccountName"))
                            {
                                condition1 = true;
                                break;
                            }
                        }
                    }
                    else
                        condition1 = false;

                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        condition1,
                        958,
                        @"If the forest functional level is 
                        DS_BEHAVIOR_WIN2003 or greater, the attributeTypes, extendedAttributeInfo, attributes on the 
                        subSchema object contains attribute 'sAMAccountName' which is not a defunct attribute, and 
                        an active attribute.");
                }

                #endregion

                #region Querying subSchema's dITContentRules, extendedClassInfo, objectClasses attributes

                subSchemaResponse = null;
                GetLDAPObject(
                    val,
                    adAdapter.PDCNetbiosName,
                    "objectclass=subschema",
                    System.DirectoryServices.Protocols.SearchScope.Base,
                    new string[] { "dITContentRules", "extendedClassInfo", "objectClasses" },
                    out subSchemaResponse);

                if (subSchemaResponse.Entries.Count > 0)
                {
                    condition1 = false;

                    List<string> ls = new List<string>(
                        (string[])subSchemaResponse.Entries[0].Attributes["dITContentRules"].GetValues(typeof(string)));

                    foreach (string item in ls)
                    {
                        if (item.Contains("posixGroup"))
                        {
                            condition1 = true;
                            break;
                        }
                    }

                    if (condition1)
                    {
                        ls = new List<string>(
                            (string[])subSchemaResponse.Entries[0].Attributes["extendedClassInfo"].GetValues(typeof(string)));

                        foreach (string item in ls)
                        {
                            if (item.Contains("posixGroup"))
                            {
                                condition1 = true;
                                break;
                            }
                        }

                        if (condition1)
                        {
                            ls = new List<string>(
                                (string[])subSchemaResponse.Entries[0].Attributes["objectClasses"].GetValues(typeof(string)));

                            foreach (string item in ls)
                            {
                                if (item.Contains("posixGroup"))
                                {
                                    condition1 = true;
                                    break;
                                }
                            }
                        }
                        else
                            condition1 = false;
                    }
                    else
                        condition1 = false;

                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        condition1,
                        959,
                        @"If the forest functional level is 
                        DS_BEHAVIOR_WIN2003 or greater, the dITContentRules, extendedClassInfo, and objectClasses 
                        attributes on the subSchema object contains class 'posixGroup' which is not a defunct class, 
                        and only an active class.");
                }

                #endregion

            }

            #endregion

            //Validates requirements related to critical object
            CriticalObject();
            //Domain Controller objects.
            if (serverOS >= OSVersion.WinSvr2008)
            {
                DomainControllerObjects();
            }
            //ValidateGetClassNames
            ValidateGetClassNames();
            //Validation of DS ExtendedRights Requirements for sub sections 7.1.2.7.63 to 7.1.2.7.70
            if (isDS)
            {
                ValidateExtendedRights2();
            }
            //Validation of LDS ExtendedRights Requirements for sub sections 7.1.2.7.63 to 7.1.2.7.70
            bool isLDS = adAdapter.RunLDSTestCases;
            if (isLDS)
            {
                ValidateLDSExtendedRights2();
            }

            //validation of special Attibute: msDS-AuthenticatedAtDC
            validateAuthenticatedAtDC();
        }

        #endregion

        #region CriticalObject
        /// <summary>
        /// Validate critical object
        /// </summary>
        public void CriticalObject()
        {
            DirectoryEntry userAccountControlEntry = new DirectoryEntry();
            if (!adAdapter.GetObjectByDN("OU=Domain Controllers," + adAdapter.rootDomainDN, out userAccountControlEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "Object is not found");
            }

            DirectoryEntries userAccountControlSearchEntry = userAccountControlEntry.Children;
            //If TO is a computer Object:
            //If TO!userAccountControl does not have the ADS_UF_PARTIAL_SECRETS_ACCOUNT bit set,
            //TO!msDS-IsUserCachableAtRodc has no value.
            foreach (DirectoryEntry userAccountControlEntries in userAccountControlSearchEntry)
            {
                #region MS-AD_Schema_R901, R902

                if (userAccountControlEntries.SchemaClassName == "computer")
                {
                    PropertyValueCollection uAC = userAccountControlEntries.Properties["userAccountControl"];

                    //ADS_UF_SERVER_TRUST_ACCOUNT in userAccountControl.
                    int userAcc_ADS_UF_SERVER_TRUST_ACCOUNT_Value = (int)uAC.Value;
                    int user_ADS_UF_SERVER_TRUST_ACCOUNT_AccControlFlag = ParseUserAccountControlValue("ADS_UF_SERVER_TRUST_ACCOUNT");

                    //ADS_UF_TRUSTED_FOR_DELEGATION in userAccountControl.
                    int userAcc_ADS_UF_TRUSTED_FOR_DELEGATION_Value = (int)uAC.Value;
                    int user_ADS_UF_TRUSTED_FOR_DELEGATION_AccControlFlag = ParseUserAccountControlValue("ADS_UF_TRUSTED_FOR_DELEGATION");

                    // If ADS_UF_SERVER_TRUST_ACCOUNT and ADS_UF_TRUSTED_FOR_DELEGATION are set in userAccountControl

                    //MS-AD_Schema_R901
                    if (
                        ((userAcc_ADS_UF_SERVER_TRUST_ACCOUNT_Value & user_ADS_UF_SERVER_TRUST_ACCOUNT_AccControlFlag) == 0)
                        && ((userAcc_ADS_UF_TRUSTED_FOR_DELEGATION_Value & user_ADS_UF_TRUSTED_FOR_DELEGATION_AccControlFlag) == 0))
                    {
                        //validate this requirements since both these bits are set.
                        DataSchemaSite.CaptureRequirement(
                            901,
                            @"The critical domain objects attribute 
                            userAccountControl bits: ADS_UF_SERVER_TRUST_ACCOUNT, ADS_UF_TRUSTED_FOR_DELEGATION");
                    }
                }

                if(userAccountControlEntries.SchemaClassName == "group")
                {
                    groupType = userAccountControlEntries.Properties["groupType"];
                    GroupTypeFlags gType = (GroupTypeFlags)Convert.ToInt32(groupType);

                    bool isGroupType = true;
                    if ((gType & (GroupTypeFlags.GROUP_TYPE_RESOURCE_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED | GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP)) == 0)
                        isGroupType = false;
                    //MS-AD_Schema_R902
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isGroupType,
                        902,
                        @"The critical domain objects attribute groupType bits: GROUP_TYPE_RESOURCE_GROUP,
                        GROUP_TYPE_SECURITY_ENABLED, GROUP_TYPE_ACCOUNT_GROUP");
                    break;
                }

                #endregion
            }
        }

        #endregion

        #region Domain Controllers
        /// <summary>
        /// Validates requirements related to DC.
        /// </summary>
        public void DomainControllerObjects()
        {

            //Dn of DomainController
            string distinguishName = "LDAP://CN="
                + adAdapter.PDCNetbiosName
                + ",OU=Domain Controllers,"
                + adAdapter.rootDomainDN;

            //Instance of DirectoryEntry eith dn.
            DirectoryEntry compEntry = new DirectoryEntry(distinguishName);

            #region Validate MS-AD_Schema_R905, MS-AD_Schema_R906

            //Check for objectClass
            groupType = compEntry.Properties["objectClass"];
            string userAccountControl = compEntry.Properties["userAccountControl"].Value.ToString();

            //Search all the group types returned 
            foreach (string groupTypeValue in groupType)
            {
                //If Domain controllers  objectClass is computer
                if (groupTypeValue == "computer")
                {
                    //MS-AD_Schema_R905
                    //Validate the req since the the value is computer
                    DataSchemaSite.CaptureRequirement(905, "the value is computer.");
                }
            }

            int userAccCtrlFlagValue = ParseUserAccountControlValue("ADS_UF_SERVER_TRUST_ACCOUNT|ADS_UF_TRUSTED_FOR_DELEGATION");
            //ADS_UF_TRUSTED_FOR_DELEGATION in userAccountControl.
            //MS-AD_Schema_R906
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                userAccCtrlFlagValue.ToString(),
                userAccountControl,
                906,
                @"Domain controller object's userAccountControl is 
                { ADS_UF_SERVER_TRUST_ACCOUNT | ADS_UF_TRUSTED_FOR_DELEGATION }");

            #endregion

            #region Validate MS-AD_Schema_R907

            //Get primaryGroupID 
            groupType = compEntry.Properties["primaryGroupID"];

            int groupTypeValueActual = (int)groupType.Value;

            int groupTypeValueExpected = 516;

            //Validate R907 if primaryGroupID is 516.
            DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                groupTypeValueExpected,
                groupTypeValueActual,
                907,
                @"Domain controller object's primaryGroupId is 516.");

            #endregion

            #region Validate MS-AD_Schema_R909

            //Get primaryGroupID 
            groupType = compEntry.Properties["servicePrincipalName"];

            //If the returned groupType is not null
            //Validate the MS-AD_Schema_R909
            DataSchemaSite.CaptureRequirementIfIsNotNull(
                groupType,
                909,
                "Domain Controllers servicePrincipalName attribute contains all of the SPNs "
            + "for a normal (not read-only) DC.");

            #endregion

            #region Validate MS-AD_Schema_R910

            groupType = compEntry.Properties["dNSHostName"];
            string dnshostname = (string)groupType.Value;
            string expectedDnsHostName = adAdapter.PDCNetbiosName + "." + adAdapter.PrimaryDomainDnsName;

            //Validate MS-AD_Schema_R910
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                expectedDnsHostName.ToLower(),
                dnshostname.ToLower(),
                910,
                @"Domain Controller object's dNSHostName equals fully qualified DNS name of the DC.");

            #endregion

            #region Validate MS-AD_Schema_objectCategory

            //Get the objectCategory into the object
            groupType = compEntry.Properties["objectCategory"];
            string compClassDN = "LDAP://CN=Computer,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            DirectoryEntry compClassEntry = new DirectoryEntry(compClassDN);
            string defaultObjectCategory = compClassEntry.Properties["defaultObjectCategory"].Value.ToString();
            DataSchemaSite.Assert.AreEqual<string>(
                defaultObjectCategory,
                groupType.Value.ToString(),
                @"Domain controller object's objectCategory contains 
                the distinguished name of the classSchema object for 
                the computer class. This is the value of the defaultObjectCategory 
                attribute of the computer class.");

            #endregion

            #region Validation of MS-AD_Schema_R926, R927, R928, R930, R931, R932, R933

            SearchResponse crossRedResponse = null;
            SearchResponse ntdsdsResponse = null;
            bool condition1 = false;
            bool condition2 = false;
            bool condition3 = false;
            int instanceType = 0;
            const int NTDSDS_OPT_IS_GC = 1;
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

            distinguishName = "CN=Partitions,CN=Configuration," + adAdapter.rootDomainDN;
            GetLDAPObject(
                distinguishName,
                adAdapter.PDCNetbiosName,
                "objectclass=crossref",
                System.DirectoryServices.Protocols.SearchScope.OneLevel,
                new string[] { "msDS-NC-Replica-Locations" },
                out crossRedResponse);

            #region Validate MS-AD_Schema_R926 , R927



            foreach (SearchResultEntry entry in crossRedResponse.Entries)
            {
				if(entry.Attributes["msDS-NC-Replica-Locations"] != null)
				{
					string[] res = (string[])entry.Attributes["msDS-NC-Replica-Locations"].GetValues(typeof(string));

					string crossRefValue = res[0];

                    if (crossRefValue.Contains(adAdapter.PDCNetbiosName.ToUpper()))
					{

						distinguishName = "CN=NTDS Settings,CN="
                            + adAdapter.PDCNetbiosName
							+ ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                            + adAdapter.rootDomainDN;

						GetLDAPObject(
							distinguishName,
                            adAdapter.PDCNetbiosName,
							"objectclass=ntdsdsa",
							System.DirectoryServices.Protocols.SearchScope.Base,
							new string[] { "distinguishedname", "msDS-hasMasterNCs", "msDS-HasInstantiatedNCs" },
							out ntdsdsResponse);

						string ntdsdsaRefValue = ntdsdsResponse.Entries[0].DistinguishedName;

						DataSchemaSite.CaptureRequirementIfAreEqual<string>(crossRefValue, ntdsdsaRefValue, 926, @"A DC is 
						instructed to host an application NC replica if: The attribute msDS-NC-Replica-Locations on the 
						crossRef representing the NC contains the dsname of the nTDSDSA object representing the DC.");

						string[] masterValue = (string[])ntdsdsResponse.Entries[0].Attributes["msDS-HasInstantiatedNCs"].GetValues(typeof(string));
						condition1 = new List<string>(
							(string[])ntdsdsResponse.Entries[0].Attributes["msDS-hasMasterNCs"].GetValues(typeof(string))).Contains(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN);

                        DirectoryEntry AppNCEntry = null;
                        if (!adAdapter.GetObjectByDN(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN, out AppNCEntry))
                        {
                            DataSchemaSite.Assume.IsTrue(false, adAdapter.PartitionPath + "," + adAdapter.rootDomainDN + " Object is not found in server");
                        }
                        instanceType = int.Parse(AppNCEntry.Properties["instancetype"].Value.ToString());

						condition2 = false;
						foreach (string val in masterValue)
						{
							if (
								val.Contains(instanceType.ToString("X")
								+ ":"
								+ adAdapter.PartitionPath
								+ ","
								+ adAdapter.rootDomainDN))
							{
								condition2 = true;
								break;
							}
						}

						condition1 = condition1 && condition2;
						DataSchemaSite.CaptureRequirementIfIsTrue(
							condition1,
							927,
							@"A DC is hosting an application NC replica when 
							1. The attribute msDS-hasMasterNCs on the nTDSDSA object representing the DC contains the dsname 
							   of the NC root representing the NC. and 
							2. The attribute msDS-HasInstantiatedNCs on the nTDSDSA object representing the DC contains an 
								Object(DN-Binary) value such that the DN field is the dsname of the NC root representing the  
								NC,and the Data field contains the value of the instanceType attribute on the NC root object 
								on the DC.");
						break;
					}
				}
            }
            #endregion

            #region Validate MS-AD_Schema_R928

            distinguishName = "CN=" + adAdapter.PDCNetbiosName + ",OU=Domain Controllers," + adAdapter.rootDomainDN;
            GetLDAPObject(
                distinguishName,
                adAdapter.PDCNetbiosName,
                "objectclass=computer",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "cn" },
                out crossRedResponse);

            DataSchemaSite.CaptureRequirementIfIsTrue(
                crossRedResponse.Entries.Count > 0,
                928,
                @"A DC is instructed 
                to host a regular domain NC replica if the domain controller object representing the DC is in the 
                domain NC.");

            #endregion

            #region Validate MS-AD_Schema_R930

            distinguishName = "CN=NTDS Settings,CN="
                + adAdapter.PDCNetbiosName
                + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                + adAdapter.rootDomainDN;

            GetLDAPObject(
                distinguishName,
                adAdapter.PDCNetbiosName,
                "objectclass=ntdsdsa",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "msDS-hasMasterNCs", "hasMasterNCs", "msDS-HasInstantiatedNCs", "msDS-HasDomainNCs" },
                out ntdsdsResponse);

            instanceType = int.Parse(new DirectoryEntry("LDAP://" + adAdapter.rootDomainDN).Properties["instancetype"].Value.ToString());

            condition1 = condition2 = condition3 = false;
            condition1 = new List<string>(
                (string[])ntdsdsResponse.Entries[0].Attributes["msDS-hasMasterNCs"].GetValues(typeof(string))).Contains(adAdapter.rootDomainDN)
                && new List<string>((string[])ntdsdsResponse.Entries[0].Attributes["hasMasterNCs"].GetValues(typeof(string))).Contains(adAdapter.rootDomainDN);

            string[] instantiatedNCValue = (string[])ntdsdsResponse.Entries[0].Attributes["msDS-HasInstantiatedNCs"].GetValues(typeof(string));

            foreach (string value in instantiatedNCValue)
            {
                if (value.Contains(instanceType.ToString("X") + ":" + adAdapter.rootDomainDN))
                {
                    condition2 = true;
                    break;
                }
            }

            condition3 = new List<string>(
                (string[])ntdsdsResponse.Entries[0].Attributes["msDS-HasDomainNCs"].GetValues(typeof(string))).Contains(adAdapter.rootDomainDN);

            condition1 = condition1 && condition2 && condition3;

            DataSchemaSite.CaptureRequirementIfIsTrue(
                condition1,
                930,
                @"A DC is hosting a regular domain NC replica 
                when the attribute msDS-hasMasterNCs and attribute hasMasterNCs on the nTDSDSA object representing 
                the DC contain the dsname of the NC root representing the domain NC, the attribute 
                msDS-HasInstantiatedNCs on the nTDSDSA object representing the DC contains an Object(DN-Binary) 
                value such that the DN field is the dsname of the domain NC root representing the domain NC, and the 
                Data field contains the value of the instanceType attribute on the domain NC root object on the DC 
                and the attribute msDS-HasDomainNCs on the nTDSDSA object representing the DC equals the dsname of 
                the domain NC root. A DC hosts only one full domain NC replica.");

            #endregion

            #region Validate MS-AD_Schema_R931

            distinguishName = "CN=NTDS Settings,CN="
                + adAdapter.PDCNetbiosName
                + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                + adAdapter.rootDomainDN;
            GetLDAPObject(
                distinguishName,
                adAdapter.PDCNetbiosName,
                "objectclass=ntdsdsa",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "msDS-hasMasterNCs", "hasMasterNCs", "msDS-HasInstantiatedNCs", 
                "msDS-HasDomainNCs", "options", "hasPartialReplicaNCs" },
                out ntdsdsResponse);

            condition1 = condition2 = condition3 = false;

            condition1 = new List<string>(
                (string[])ntdsdsResponse.Entries[0].Attributes["msDS-hasMasterNCs"].GetValues(typeof(string))).Contains(
                "CN=Configuration," + adAdapter.rootDomainDN)
                && new List<string>(
                (string[])ntdsdsResponse.Entries[0].Attributes["hasMasterNCs"].GetValues(typeof(string))).Contains(
                "CN=Configuration," + adAdapter.rootDomainDN)
                && new List<string>(
                (string[])ntdsdsResponse.Entries[0].Attributes["msDS-hasMasterNCs"].GetValues(typeof(string))).Contains(
                "CN=Schema,CN=Configuration," + adAdapter.rootDomainDN)
                && new List<string>(
                (string[])ntdsdsResponse.Entries[0].Attributes["hasMasterNCs"].GetValues(typeof(string))).Contains(
                "CN=Schema,CN=Configuration," + adAdapter.rootDomainDN);

            instantiatedNCValue = (string[])ntdsdsResponse.Entries[0].Attributes["msDS-HasInstantiatedNCs"].GetValues(typeof(string));

            instanceType = int.Parse(new DirectoryEntry("LDAP://"
                + "CN=Configuration,"
                + adAdapter.rootDomainDN).Properties["instancetype"].Value.ToString());

            foreach (string value in instantiatedNCValue)
            {
                if (value.Contains(instanceType.ToString("X") + ":" + "CN=Configuration," + adAdapter.rootDomainDN))
                {
                    condition2 = true;
                    break;
                }
            }

            if (condition2)
            {
                instanceType = int.Parse(new DirectoryEntry("LDAP://"
                    + "CN=Schema,CN=Configuration,"
                    + adAdapter.rootDomainDN).Properties["instancetype"].Value.ToString());

                foreach (string value in instantiatedNCValue)
                {
                    if (value.Contains(instanceType.ToString("X") + ":" + "CN=Schema,CN=Configuration," + adAdapter.rootDomainDN))
                    {
                        condition2 = true;
                        break;
                    }
                }
            }
            else
            {
                condition2 = false;
            }

            condition1 = condition1 && condition2;

            DataSchemaSite.CaptureRequirementIfIsTrue(
                condition1,
                931,
                @"A DC is hosting the schema and Config NC 
                replicas when the  attribute msDS-hasMasterNCs and attribute hasMasterNCs on the nTDSDSA object 
                representing the DC contain the dsname of both the NC roots representing the Schema and Config NCs 
                and the attribute msDS-HasInstantiatedNCs on the nTDSDSA object representing the DC contains two 
                Object(DN-Binary) values such that the DN fields are the dsname of the NC root representing the 
                config and schema NCs, and the binary fields contain the values of the instanceType attribute on the 
                config and schema NC root objects on the DC.");

            #endregion

            #region Validate MS-AD_Schema_R932

            condition1 = false;

            int optionsFlag = int.Parse(ntdsdsResponse.Entries[0].Attributes["options"].GetValues(typeof(string))[0].ToString());

            condition1 = ((optionsFlag & NTDSDS_OPT_IS_GC) == NTDSDS_OPT_IS_GC);

            DataSchemaSite.CaptureRequirementIfIsTrue(
                condition1,
                932,
                @"A DC is instructed to host a partial NC replica 
                of every domain NC in the forest if the options attribute of the nTDSDSA object representing that DC 
                has NTDSDS_OPT_IS_GC flag.");

            #endregion

            #region Validate MS-AD_Schema_R933

            if (ntdsdsResponse.Entries[0].Attributes.Contains("hasPartialReplicaNCs"))
            {
                string[] partialReplicas = (string[])ntdsdsResponse.Entries[0].Attributes["hasPartialReplicaNCs"].GetValues(typeof(string));

                condition1 = false;

                foreach (string replica in partialReplicas)
                {
                    if (replica.ToLower().Equals(adAdapter.childDomainDN.ToLower()))
                    {
                        condition1 = true;
                        break;
                    }
                }

                distinguishName = "CN=NTDS Settings,CN="
                    + adAdapter.PDCNetbiosName
                    + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,"
                    + adAdapter.rootDomainDN;
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

        #region Validation of MS-AD_Schema_R873 and R874

        /// <summary>
        /// Validates the requiremnets which gets the class names
        /// </summary>
        public void ValidateGetClassNames()
        {
            //Current Roort domain
            string currDomain = adAdapter.rootDomainDN;
            //With Configuration NC
            string configNC = "CN=Configuration," + currDomain;
            //Schema and Configuration NC
            string SchemaNC = "CN=Schema," + configNC;
            string distinguishName = String.Empty;
            int expectedResult = 0;
            int actualResult = 0;

            //DS  
            isDS = adAdapter.RunDSTestCases;
            if (isDS)
            {
                //Distinguishname in case of DS.
                distinguishName = SchemaNC;
                string[] classNames = { "top", "country", "locality", "organization", "organizationalUnit", "person", 
                    "organizationalPerson", "organizationalRole", "groupOfNames", "residentialPerson", 
                    "applicationProcess", "applicationEntity", "dSA", "device", "certificationAuthority", 
                    "account", "document", "room", "documentSeries", "domain", "rFC822LocalPart", "domainRelatedObject", 
                    "friendlyCountry", "simpleSecurityObject", "subSchema", "inetOrgPerson", "posixAccount", 
                    "shadowAccount", "posixGroup", "ipService", "ipProtocol", "oncRpc", "ipHost", "ipNetwork", 
                    "nisNetgroup", "nisMap", "nisObject", "ieee802Device", "bootableDevice" };
                actualResult = GetClassNames(distinguishName, classNames, true);

                //Validate MS-AD_Schema_R873
                DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                    expectedResult,
                    actualResult,
                    873,
                    "The class that supported in AD DS are the followings: top,country,locality,organization,"
                + "organizationalUnit,person,organizationalPerson,organizationalRole,groupOfNames,residentialPerson,"
                + "applicationProcess,applicationEntity,dSA,device,certificationAuthority,account,document,room,"
                + "documentSeries,domain,LocalPart,domainRelatedObject,friendlyCountry,simpleSecurityObject,subSchema,"
                + "inetOrgPerson,posixAccount,shadowAccount,posixGroup,ipService,ipProtocol,oncRpc,ipHost,ipNetwork,"
                + "nisNetgroup,nisMap,nisObject,ieee802Device,bootableDevice.");
            }
            //LDS
            bool isLDS = adAdapter.RunLDSTestCases;
            if (isLDS)
            {
                distinguishName = "CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
                string[] classNames = { "dMD", "top", "country", "locality", "organization", "organizationalUnit", "subSchema", "domain" };
                actualResult = GetClassNames(distinguishName, classNames, false);

                //Validate MS-AD_Schema_R874
                DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                    expectedResult,
                    actualResult,
                    874,
                    "The class that supported in AD LDS are the followings: device,dMD,top,country,locality,"
                + "organization,organizationalUnit,subSchema,domain.");
            }
        }

        #endregion

        #region GetClassNames
        /// <summary>
        /// Gets all the class names for the given distinguish names
        /// If every thing matches returns 0 else returns 1.
        /// </summary>
        /// <param name="distinguishName">Distinguish name of the classNames</param>
        /// <param name="dsOrLds">If need to connect ds, set it true; If need to connect lds, set it false</param>
        /// <returns>Returns 0 if all the classes are matched else returns 1 </returns>
        private int GetClassNames(string distinguishName, string[] classNames, bool dsOrLds)
        {
            int classNamescount = -1;
            List<string> tempName = new List<string>();
            try
            {
                string server = adAdapter.PDCNetbiosName;
                if (serverOS >= OSVersion.WinSvr2008R2)
                    server += "." + adAdapter.PrimaryDomainDnsName;
                //Ldap Connection 

                LdapConnection connection = null;
                if (dsOrLds)
                {
                    connection = new LdapConnection(new LdapDirectoryIdentifier(server));
                }
                else
                {
                    connection = new LdapConnection(server + ":" + adAdapter.ADLDSPortNum);
                }
                //Bind
                connection.Bind();
                tempName.AddRange(classNames);
                foreach (string classname in classNames)
                {
                    SearchRequest request = new SearchRequest(
                        distinguishName,
                        "objectclas=classschema",
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        new string[] { "ldapdisplayname" });
                    SearchResponse response = (SearchResponse)connection.SendRequest(request);
                    if (response.Entries.Count > 0)
                    {
                        string s = (string)response.Entries[0].Attributes["ldapdisplayname"].GetValues(classname.GetType())[0];
                        tempName.Remove(s);
                    }
                }
                classNamescount = 0;
                return classNamescount;
            }
            catch (Exception)
            {
                classNamescount = 1;
                return classNamescount;
            }
        }

        #endregion


        #region validateAuthenticatedAtDC

        /// <summary>
        /// Validates the requirements about the special attribute
        /// </summary>
        private void validateAuthenticatedAtDC()
        {
            bool isMaintainedByComputers = true, isMaintainedByUsers = true, isMaintainedByDCs = true;
            List<string> rodcsAuthenticatedModel = new List<string>();

            DirectoryEntry domainControlEntry = new DirectoryEntry();
            if (!adAdapter.GetObjectByDN("OU=Domain Controllers," + adAdapter.rootDomainDN, out domainControlEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "Object is not found");
            }
            DirectoryEntries domainControlEntries = domainControlEntry.Children;
            foreach (DirectoryEntry domainController in domainControlEntries)
            {
                domainController.RefreshCache(new string[] { "msDS-isRODC", "distinguishedName" });
                if (bool.Parse(domainController.Properties["msDS-isRODC"].Value.ToString()) == true)
                {
                    rodcsAuthenticatedModel.Add(domainController.Properties["distinguishedName"].Value.ToString().ToLower());
                }
            }
            List<string> rodcsAuthDCActual = new List<string>();
            foreach (DirectoryEntry domainController in domainControlEntries)
            {
                rodcsAuthDCActual = new List<string>();
                domainController.RefreshCache(new string[] { "msDS-AuthenticatedAtDC" });
                if ((domainController.Properties["msDS-AuthenticatedAtDC"] != null) && (domainController.Properties["msDS-AuthenticatedAtDC"].Value != null))
                {
                    foreach (string value in domainController.Properties["msDS-AuthenticatedAtDC"])
                    {
                        rodcsAuthDCActual.Add(value.ToLower());
                    }
                    isMaintainedByDCs = EqualList(rodcsAuthenticatedModel, rodcsAuthDCActual);
                }
                if (isMaintainedByDCs == false)
                    break;
            }

            List<string> rodcsAuthComputerActual = new List<string>();
            DirectoryEntry computersEntry = new DirectoryEntry();
            if (!adAdapter.GetObjectByDN("CN=Computers," + adAdapter.rootDomainDN, out computersEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "Object is not found");
            }
            DirectoryEntries computerEntries = computersEntry.Children;
            foreach (DirectoryEntry computer in computerEntries)
            {
                rodcsAuthComputerActual = new List<string>();
                computer.RefreshCache(new string[] { "msDS-AuthenticatedAtDC" });
                if ((computer.Properties["msDS-AuthenticatedAtDC"] != null) && (computer.Properties["msDS-AuthenticatedAtDC"].Value != null))
                {
                    foreach (string value in computer.Properties["msDS-AuthenticatedAtDC"])
                    {
                        rodcsAuthComputerActual.Add(value.ToLower());
                    }
                    isMaintainedByComputers = EqualList(rodcsAuthenticatedModel, rodcsAuthComputerActual);
                    if (!isMaintainedByComputers)
                        DataSchemaSite.Log.Add(LogEntryKind.Warning, "Maybe need restart computer {0} to have it authenticated", computer.Name);
                }
                if (isMaintainedByComputers == false)
                    break;
            }

            DirectoryEntry userAccountControlEntry = new DirectoryEntry();
            if (!adAdapter.GetObjectByDN("CN=Users," + adAdapter.rootDomainDN, out userAccountControlEntry))
            {
                DataSchemaSite.Assert.IsTrue(false, "Object is not found");
            }
            List<string> rodcsAuthUserActual = new List<string>();
            DirectoryEntries userAccountControlEntries = userAccountControlEntry.Children;
            foreach (DirectoryEntry user in userAccountControlEntries)
            {
                rodcsAuthUserActual = new List<string>();
                user.RefreshCache(new string[] { "msDS-AuthenticatedAtDC" });
                if (user.Properties["msDS-AuthenticatedAtDC"].Value != null)
                {
                    foreach (string value in user.Properties["msDS-AuthenticatedAtDC"])
                    {
                        rodcsAuthUserActual.Add(value.ToLower());
                    }
                    isMaintainedByUsers = EqualList(rodcsAuthenticatedModel, rodcsAuthUserActual);
                }
                if (isMaintainedByUsers == false)
                    break;
            }

            DataSchemaSite.CaptureRequirementIfIsTrue(isMaintainedByComputers
                && isMaintainedByDCs
                && isMaintainedByUsers,
                956,
                @"msDS-AuthenticatedAtDC is maintained by the DC on user and computer objects.
                The attribute contains a list of computer objects, corresponding to the RODCs
                at which the user or computer has authenticated.");

            bool isBacklink = false;
            bool isForlink = false;
            DirectoryEntry authenDCEntry = new DirectoryEntry();
            if (adAdapter.GetObjectByDN("CN=ms-DS-AuthenticatedAt-DC,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN,
                out authenDCEntry))
            {
                authenDCEntry.RefreshCache(new string[] { "adminDescription" });
                foreach (string value in authenDCEntry.Properties["adminDescription"])
                {
                    if (value.ToLower().Contains("Forwardlink for ms-DS-AuthenticatedTo-Accountlist".ToLower()))
                    {
                        isForlink = true;
                    }
                }
            }
            else
            {
                DataSchemaSite.Assert.Fail("Object CN=ms-DS-AuthenticatedAt-DC,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN + " is not existed.");
            }

            if (adAdapter.GetObjectByDN("CN=ms-DS-AuthenticatedTo-Accountlist,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN,
                out authenDCEntry))
            {
                authenDCEntry.RefreshCache(new string[] { "adminDescription" });
                foreach (string value in authenDCEntry.Properties["adminDescription"])
                {
                    if (value.ToLower().Contains("Backlink for ms-DS-AuthenticatedAt-DC".ToLower()))
                    {
                        isBacklink = true;
                    }
                }
            }
            else
            {
                DataSchemaSite.Assert.Fail("Object CN=ms-DS-AuthenticatedTo-Accountlist,CN=Schema,CN=Configuration,"
                    + adAdapter.rootDomainDN + " is not existed.");
            }

            DataSchemaSite.CaptureRequirementIfIsTrue(isForlink
                && isBacklink,
                957,
                @"msDS-AuthenticatedAtD is a forward link attribute whose corresponding back link is the
                msDS-AuthenticatedToAccountlist attribute.When a writable DC authenticates a user or computer to an RODC,
                that writable DC adds the DN of the RODC's computer object to the list in the msDS-AuthenticatedAtDC attribute
                of the user or computer that was authenticated.");
        }

        #endregion

        #region EqualList

        /// <summary>
        /// EqualList is used to equal two lists.
        /// </summary>
        /// <param name="listOne"></param>
        /// <param name="listTwo"></param>
        /// <returns>true if the two lists are equal, or else false.</returns>
        private bool EqualList(List<string> listOne, List<string> listTwo)
        {
            bool isEqual = true;
            if (listOne.Count != listTwo.Count)
            {
                return isEqual = false;
            }
            listOne.Sort();
            listTwo.Sort();
            string[] stringOne = listOne.ToArray();
            string[] stringTwo = listTwo.ToArray();
            for (int i = 0; i < listOne.Count; i++)
            {
                if (!stringOne[i].Equals(stringTwo[i]))
                {
                    return isEqual = false;
                }
            }
            return isEqual;
        }

        #endregion

    }
}