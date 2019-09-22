// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Security.Principal;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase37 and TestCase38.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        //common variables.
        PropertyValueCollection groupType;
        GroupTypeFlags gType;

        #region WellKnownSecurityDomainPrincipal implementation.

        /// <summary>
        /// TestCase29 method validates the requirements under 
        /// WellKnownSecurityDomainPrincipal Scenario.
        /// </summary>
        public void ValidateWellKnownSecurityDomainPrincipal()
        {
            DirectoryEntry dirEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();
            DirectoryEntry rootEntry = new DirectoryEntry();
            string currDomain = adAdapter.rootDomainDN;
            string configNC = "CN=Configuration," + currDomain;
            string schemaNC = "CN=Schema," + configNC;


            bool isObjectSid = true;
            SecurityIdentifier sid;
            PropertyValueCollection rid, ridUser;
            byte[] objectSid;
            string expectedValue = String.Empty;
            string actualValue = String.Empty;



            if (!adAdapter.GetObjectByDN("CN=Users," + currDomain, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, "CN=Users," + currDomain + " Object is not found in server");
            }

            // Get the object of Root Domain NC.
            if (!adAdapter.GetObjectByDN(currDomain, out rootEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, currDomain + " Object is not found in server");
            }

            //Get the objectSid value of the object of root domain NC.
            objectSid = (byte[])rootEntry.Properties["objectSid"].Value;
            sid = new SecurityIdentifier(objectSid, 0);

            expectedValue = sid.ToString();
            int length = expectedValue.Length;
            DirectoryEntries rolesChilds = dirEntry.Children;

            //For each child,
            foreach (DirectoryEntry child in rolesChilds)
            {
                //Get the sid of the child object.
                objectSid = (byte[])child.Properties["objectSid"].Value;
                sid = new SecurityIdentifier(objectSid, 0);

                //Get the rid of this object.
                string temp = sid.ToString();
                temp = temp.Substring(temp.LastIndexOf('-'));

                //Add rid with expected value.
                expectedValue = expectedValue + temp;

                //Get the actual value.
                actualValue = sid.ToString();

                //Compare.
                if (actualValue.Equals(expectedValue))
                {
                    isObjectSid = true;
                }
                else
                {
                    isObjectSid = false;
                    break;
                }
                //Reset the expected value.
                expectedValue = expectedValue.Substring(0, length);
            }
            //MS-ADTS-Schema_R811
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectSid,
                811,
                @"The objectSid attribute of Well-Known Domain-Relative Security Principals must be a SID
                consisting of the objectSid of the domain NC root followed by the RID specified for each child.");

            //MS-ADTS-Schema_R812
            childEntry = dirEntry.Children.Find($"CN={adAdapter.DomainAdministratorName}");
            rid = childEntry.Properties["primaryGroupID"];
            childEntry = dirEntry.Children.Find("CN=Domain Users");
            childEntry.RefreshCache(new string[] { "primaryGroupToken" });
            ridUser = childEntry.Properties["primaryGroupToken"];

            DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                (int)ridUser.Value,
                (int)rid.Value,
                812,
                @"The primaryGroupID attribute of class user Well-Known Domain-Relative Security Principals must be  
                RID, which refers to another Well-Known domain relative security principal, by RID");

            //The method to call common requirements for AD/DS and LDS.
            LDSAndDSCommonCall(dirEntry);

            if (!adAdapter.GetObjectByDN(currDomain, out childEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, currDomain + " Object is not found in server");
            }
            PropertyValueCollection nTMixedDomain = childEntry.Properties["nTMixedDomain"];
            //If nTMixedDomain value is 0 that is not mixed else mixed domain            
            if ((int)nTMixedDomain.Value == 0)
            {
                //MS-ADTS-Schema_R843
                childEntry = dirEntry.Children.Find("CN=Schema Admins");
                PropertyValueCollection groupType = childEntry.Properties["groupType"];
                gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

                DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                    GroupTypeFlags.GROUP_TYPE_UNIVERSAL_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                    gType,
                    843,
                    @"If the forest root domain is not mixed :The groupType attribute of Schema Admins Well-Known 
                    Domain-Relative Security Principals is {GROUP_TYPE_UNIVERSAL_GROUP | GROUP_TYPE_SECURITY_ENABLED}
                    This means that in groupType field the two above bits are set, which means that the groupType 
                    is 0x80000008.");

                if (serverOS >= OSVersion.WinSvr2008)
                {
                    //MS-ADTS-Schema_R832
                    childEntry = dirEntry.Children.Find("CN=Enterprise Admins");
                    groupType = childEntry.Properties["groupType"];
                    gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);
                    DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                        GroupTypeFlags.GROUP_TYPE_UNIVERSAL_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                        gType,
                        832,
                        @"If the forest root domain is not mixed :The groupType attribute of Enterprise Administrators 
                        Well-Known Domain-Relative Security Principals is 
                        {GROUP_TYPE_UNIVERSAL_GROUP | GROUP_TYPE_SECURITY_ENABLED} This means that in groupType field 
                        the two above bits are set, which means that the groupType is 0x80000008.");
                }
            }
            else
            {
                //MS-ADTS-Schema_R842, MS-ADTS-Schema_R831
                childEntry = dirEntry.Children.Find("CN=Schema Admins");
                PropertyValueCollection groupType = childEntry.Properties["groupType"];
                gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

                DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                    GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                    gType,
                    842,
                    @"If the forest root domain is not mixed :The groupType attribute of Schema Admins Well-Known 
                        Domain-Relative Security Principals is {GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED} 
                        This means that in groupType field the two above bits are set, which means that the groupType 
                        is 0x80000002.");
                if (serverOS == OSVersion.WinSvr2008)
                {
                    //MS-ADTS-Schema_R831
                    childEntry = dirEntry.Children.Find("CN=Enterprise Admins");
                    groupType = childEntry.Properties["groupType"];
                    gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

                    DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                        GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                        gType,
                        831,
                        @"If the forest root domain is not mixed :The groupType attribute of Enterprise 
                            Administrators Well-Known Domain-Relative Security Principals is {GROUP_TYPE_ACCOUNT_GROUP | 
                            GROUP_TYPE_SECURITY_ENABLED} This means that in groupType field the two above bits are set,
                            which means that the groupType is 0x80000002.");
                }
            }
        }

        #endregion

        #region LDSWellKnownSecurityDomainPrincipal implementation.

        /// <summary>
        /// TestCase30 method validates the requirements under 
        /// LDSWellKnownSecurityDomainPrincipal Scenario.
        /// </summary>
        public void ValidateLDSWellKnownSecurityDomainPrincipal()
        {
            DirectoryEntry dirEntry = new DirectoryEntry();
            string currDomain = adAdapter.rootDomainDN;
            string configNCForLDS = "CN=Configuration," + adAdapter.LDSRootObjectName;
            if (!adAdapter.GetLdsObjectByDN(
                "CN=NTDS Settings,CN="
                + adAdapter.LDSServerInstance 
                + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,"
                + configNCForLDS,
                out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=NTDS Settings,CN="
                    + adAdapter.LDSServerInstance
                    + ",CN=Servers,CN=Default-First-Site-Name,CN=Sites,"
                    + configNCForLDS
                    + " Object is not found in server");
            }

            //MS-ADTS-Schema_R446           
            dirEntry.RefreshCache(new string[] { "msDS-PortLDAP" });
            PropertyValueCollection msDSLDAP = dirEntry.Properties["msDS-PortLDAP"];
            string msLDAPPort = msDSLDAP.Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                adAdapter.ADLDSPortNum,
                msLDAPPort,
                446,
                "In AD/LDS msDS-PortLDAP attribute of the nTDSDSA Object stores the LDAP port for the instance");

            //MS-ADTS-Schema_R447
            PropertyValueCollection msDSSLPort = dirEntry.Properties["msDS-PortSSL"];
            string msSSLPort = msDSSLPort.Value.ToString();
            int portNumberValue = int.Parse(adAdapter.ADLDSPortNum) + 1;
            DataSchemaSite.CaptureRequirementIfAreEqual<int>(
                portNumberValue,
                int.Parse(msSSLPort),
                447,
                "In AD/LDS msDS-PortSSL attribute of the nTDSDSA Object stores the SSL port for the instance");

            //MS-ADTS-Schema_R448
            PropertyValueCollection msDSSerAccount = dirEntry.Properties["msDS-ServiceAccount"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                msDSSerAccount.Value.ToString().Contains("CN=ForeignSecurityPrincipals"),
                448,
                @"In AD/LDS msDS-ServiceAccount attribute of the nTDSDSA Object stores  the foreignSecurityPrincipal 
                object representing the service account running this DC");

            //MS-ADTS-Schema_R453
            PropertyValueCollection msDSNamingContext = dirEntry.Properties["msDS-DefaultNamingContext"];

            //If The value is set
            DataSchemaSite.CaptureRequirementIfIsNotNull(
                msDSNamingContext,
                453,
                @"In AD/LDS msDS-DefaultNamingContext attribute of the nTDSDSA object specifies, the NC that should 
                be returned as the default NC by the defaultNamingContext attribute of the root DSE.");

            //The method to call common requirements for AD/DS and LDS.
            if (!adAdapter.GetObjectByDN("CN=Users," + currDomain, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Users,"
                    + currDomain
                    + " Object is not found in server");
            }
            LDSAndDSCommonCall(dirEntry);
        }

        #endregion

        #region CommonRequirements

        /// <summary>
        /// This method validates the requirements under 
        /// WellKnownSecurityDomainPrincipal for both AD/DS and LDS Scenario's.
        /// The common requirements are logged here.
        /// </summary>
        public void LDSAndDSCommonCall(DirectoryEntry directEntry)
        {
            GroupTypeFlags gType;

            //Holding Directory entries.
            DirectoryEntry dirEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();
            dirEntry = directEntry;
            //MS-ADTS-Schema_R810
            DirectoryEntries rolesChilds = dirEntry.Children;
            bool isParentRoles = true;
            foreach (DirectoryEntry child in rolesChilds)
            {
                if (!child.Parent.Name.ToString().Equals("CN=Users"))
                    isParentRoles = false;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isParentRoles,
                810,
                "For the Well-Known Domain-Relative Security Principals the Parent must be Users container");

            //MS-ADTS-Schema_R813
            childEntry = dirEntry.Children.Find($"CN={adAdapter.DomainAdministratorName}");
            PropertyValueCollection objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"user"),
                813,
                "The objectClass attribute of Administrator Well-Known Domain-Relative Security Principals must be user");

            //MS-ADTS-Schema_R814
            childEntry = dirEntry.Children.Find("CN=Guest");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"user"),
                814,
                "The objectClass attribute of Guest Well-Known Domain-Relative Security Principals must be user");

            //MS-ADTS-Schema_R815
            childEntry.RefreshCache(new string[] { "primaryGroupID" });
            string primary = childEntry.Properties["primaryGroupID"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "514",
                primary,
                815,
                @"The primaryGroupID attribute of Guest Well-Known Domain-Relative Security Principals must 
                be 514 (Domain Guests)");

            //MS-ADTS-Schema_R816
            childEntry = dirEntry.Children.Find("CN=krbtgt");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"user"),
                816,
                @"The objectClass attribute of Key Distribution Center Service Account Well-Known Domain-Relative 
                Security Principals must be user");

            //MS-ADTS-Schema_R817
            childEntry.RefreshCache(new string[] { "primaryGroupID" });
            primary = childEntry.Properties["primaryGroupID"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "513",
                primary,
                817,
                @"The primaryGroupID attribute of Key Distribution Center Service Account Well-Known Domain-Relative 
                Security Principals must be 513 (Domain Users)");

            //MS-ADTS-Schema_R818
            childEntry = dirEntry.Children.Find("CN=Cert Publishers");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"group"),
                818,
                @"The objectClass attribute of Key Distribution Center Service Account Well-Known Domain-Relative 
                Security Principals must be user");

            //MS-ADTS-Schema_R819
            groupType = childEntry.Properties["groupType"];
            gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

            DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                GroupTypeFlags.GROUP_TYPE_RESOURCE_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                gType,
                819,
                @"The groupType attribute of Cert Publishers Well-Known Domain-Relative Security Principals is 
                {GROUP_TYPE_RESOURCE_GROUP | GROUP_TYPE_SECURITY_ENABLED}.This means that in groupType field the two 
                above bits are set, which means that the groupType is 0x80000004");

            //MS-ADTS-Schema_R820
            childEntry = dirEntry.Children.Find("CN=Domain Admins");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"group"),
                820,
                @"The objectClass attribute of Domain Administrators Well-Known Domain-Relative Security Principals 
                must be group");

            //MS-ADTS-Schema_R821
            groupType = childEntry.Properties["groupType"];
            gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

            DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                gType,
                821,
                @"The groupType attribute of Domain Administrators, Well-Known Domain-Relative Security Principals 
                is  {GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED}.This means that in groupType field  
                the two above bits are set, which means that the groupType is 0x80000002");

            //MS-ADTS-Schema_R822
            childEntry = dirEntry.Children.Find("CN=Domain Computers");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"group"),
                822,
                @"The objectClass attribute of Domain Computers Well-Known Domain-Relative Security 
                Principals must be group");

            //MS-ADTS-Schema_R823
            groupType = childEntry.Properties["groupType"];
            gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

            DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                gType,
                823,
                @"The groupType attribute of Domain Computers Well-Known Domain-Relative Security Principals is 
                {GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED}.This means that in groupType field the
                two above bits are set, which means that the groupType is 0x80000002");

            //MS-ADTS-Schema_R824
            childEntry = dirEntry.Children.Find("CN=Domain Controllers");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"group"),
                824,
                @"The objectClass attribute of Domain Controllers, Well-Known Domain-Relative Security Principals 
                must be group");

            //MS-ADTS-Schema_R825
            groupType = childEntry.Properties["groupType"];
            gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

            DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                gType,
                825,
                @"The groupType attribute of Domain Controllers Well-Known Domain-Relative Security Principals 
                is {GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED}.This means that in groupType field  
                the two above bits are set, which means that the groupType is 0x80000002");

            //MS-ADTS-Schema_R826
            childEntry = dirEntry.Children.Find("CN=Domain Guests");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"group"),
                826,
                @"The objectClass attribute of Domain Guests Well-Known Domain-Relative Security Principals must 
                be group");

            //MS-ADTS-Schema_R827
            groupType = childEntry.Properties["groupType"];
            gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

            DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                gType,
                827,
                @"The groupType attribute of Domain Guests Well-Known Domain-Relative Security Principals is 
                {GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED}.This means that in groupType field  
                the two above bits are set, which means that the groupType is 0x80000002");

            //MS-ADTS-Schema_R828
            childEntry = dirEntry.Children.Find("CN=Domain Users");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"group"),
                828,
                "The objectClass attribute of Domain Users Well-Known Domain-Relative Security Principals must be group");

            //MS-ADTS-Schema_R829
            groupType = childEntry.Properties["groupType"];
            gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

            DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                gType,
                829,
                @"The groupType attribute of Domain Users Well-Known Domain-Relative Security Principals must
                be either of  GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED");

            //MS-ADTS-Schema_R833
            childEntry = dirEntry.Children.Find("CN=Group Policy Creator Owners");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"group"),
                833,
                @"The objectClass attribute of Group Policy Creator Owners Well-Known Domain-Relative Security 
                Principals must be group");

            //MS-ADTS-Schema_R834
            groupType = childEntry.Properties["groupType"];
            gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

            DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                gType,
                834,
                @"The groupType attribute of Group Policy Creator Owners Well-Known Domain-Relative Security Principals 
                is {GROUP_TYPE_ACCOUNT_GROUP | GROUP_TYPE_SECURITY_ENABLED}.This means that in groupType field the two 
                above bits are set, which means that the groupType is 0x80000002");

            //MS-ADTS-Schema_R835
            childEntry = dirEntry.Children.Find("CN=RAS and IAS Servers");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"group"),
                835,
                @"The objectClass attribute of RAS and IAS Servers Well-Known Domain-Relative Security Principals 
                must be group");

            //MS-ADTS-Schema_R836
            groupType = childEntry.Properties["groupType"];
            gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

            DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                GroupTypeFlags.GROUP_TYPE_RESOURCE_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                gType,
                836,
                @"The groupType attribute of RAS and IAS Servers Well-Known Domain-Relative Security Principals 
                is {GROUP_TYPE_RESOURCE_GROUP | GROUP_TYPE_SECURITY_ENABLED}.This means that in groupType field  
                the two above bits are set, which means that the groupType is 0x80000004");

            //MS-ADTS-Schema_R841
            childEntry = dirEntry.Children.Find("CN=Schema Admins");
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"group"),
                841,
                @"The objectClass attribute of Schema Admins Well-Known Domain-Relative Security Principals 
                must be group");

            if (serverOS >= OSVersion.WinSvr2008)
            {
                //MS-ADTS-Schema_R830
                childEntry = dirEntry.Children.Find("CN=Enterprise Admins");
                objectClass = childEntry.Properties["objectClass"];
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    objectClass.Contains((object)"group"),
                    830,
                    @"The objectClass attribute of Enterprise Administrators Well-Known Domain-Relative Security  
                    Principals must be group.");

                //MS-ADTS-Schema_R837
                childEntry = dirEntry.Children.Find("CN=Read-only Domain Controllers");
                objectClass = childEntry.Properties["objectClass"];
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    objectClass.Contains((object)"group"),
                    837,
                    @"The objectClass attribute of Read-Only Domain Controllers Well-Known 
                    Domain-Relative Security Principals must be group");

                //MS-ADTS-Schema_R838
                groupType = childEntry.Properties["groupType"];
                gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

                DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                    GroupTypeFlags.GROUP_TYPE_ACCOUNT_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                    gType,
                    838,
                    @"The groupType attribute of Read-Only Domain Controllers Well-Known Domain-Relative  
                    Security Principals is {GROUP_TYPE_ACCOUNT_GROUP|GROUP_TYPE_SECURITY_ENABLED}
                    This means that in groupType field the two above bits are set, which means that the  
                    groupType is 0x80000002.");

                //MS-ADTS-Schema_R839
                childEntry = dirEntry.Children.Find("CN=Enterprise Read-only Domain Controllers");
                objectClass = childEntry.Properties["objectClass"];
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    objectClass.Contains((object)"group"),
                    839,
                    @"The objectClass attribute of Enterprise Read-Only Domain Controllers Well-Known Domain-Relative 
                    Security Principals must be group");

                //MS-ADTS-Schema_R840
                groupType = childEntry.Properties["groupType"];
                gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

                DataSchemaSite.CaptureRequirementIfAreEqual<GroupTypeFlags>(
                    GroupTypeFlags.GROUP_TYPE_UNIVERSAL_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                    gType,
                    840,
                    @"The groupType attribute of Enterprise Read-Only Domain Controllers Well-Known Domain-Relative 
                    Security Principals is {GROUP_TYPE_UNIVERSAL_GROUP|GROUP_TYPE_SECURITY_ENABLED} This means that in 
                    groupType field the two above bits are set, which means that the groupType is 0x80000008.");

                //MS-ADTS-Schema_Allowed RODC Password Replication Group
                childEntry = dirEntry.Children.Find("CN=Allowed RODC Password Replication Group");
                objectClass = childEntry.Properties["objectClass"];
                DataSchemaSite.Assert.IsTrue(
                    objectClass.Contains((object)"group"),
                    @"The objectClass attribute of Allowed RODC Password Replication Group Well-Known Domain-Relative 
                    Security Principals must be group");

                //MS-ADTS-Schema_Allowed RODC Password Replication Group
                groupType = childEntry.Properties["groupType"];
                gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

                DataSchemaSite.Assert.AreEqual<GroupTypeFlags>(
                    GroupTypeFlags.GROUP_TYPE_RESOURCE_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                    gType,
                    @"The groupType attribute of Allowed RODC Password Replication Group Well-Known Domain-Relative 
                    Security Principals is {GROUP_TYPE_RESOURCE_GROUP|GROUP_TYPE_SECURITY_ENABLED} This means that in 
                    groupType field the two above bits are set, which means that the groupType is 0x80000004.");

                //MS-ADTS-Schema_Denied RODC Password Replication Group
                childEntry = dirEntry.Children.Find("CN=Denied RODC Password Replication Group");
                objectClass = childEntry.Properties["objectClass"];
                DataSchemaSite.Assert.IsTrue(
                    objectClass.Contains((object)"group"),
                    @"The objectClass attribute of Denied RODC Password Replication Group Well-Known Domain-Relative 
                    Security Principals must be group");

                //MS-ADTS-Schema_Denied RODC Password Replication Group
                groupType = childEntry.Properties["groupType"];
                gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

                DataSchemaSite.Assert.AreEqual<GroupTypeFlags>(
                    GroupTypeFlags.GROUP_TYPE_RESOURCE_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED,
                    gType,
                    @"The groupType attribute of Denied RODC Password Replication Group Well-Known Domain-Relative 
                    Security Principals is {GROUP_TYPE_RESOURCE_GROUP|GROUP_TYPE_SECURITY_ENABLED} This means that in 
                    groupType field the two above bits are set, which means that the groupType is 0x80000004.");
            }
        }

        #endregion
    }
}