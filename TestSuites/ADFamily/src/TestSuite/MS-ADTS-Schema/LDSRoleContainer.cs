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
    /// This file is the source file for Validation of the TestCase36.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region RolesContainer Validation.
        /// <summary>
        /// Method validates the requirements under RolesContainer Scenario.
        /// </summary>
        public void ValidateLDSRoleContainer()
        {
            DirectoryEntry dirEntryConfig = new DirectoryEntry();
            DirectoryEntry dirEntryApp = new DirectoryEntry();
            DirectoryEntry dirEntrySch = new DirectoryEntry();
            DirectoryEntry dirEntry = new DirectoryEntry();

            if (!adAdapter.GetLdsObjectByDN("CN=Roles," + adAdapter.LDSApplicationNC, out dirEntryApp))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Roles," + adAdapter.LDSApplicationNC
                    + " Object is not found in server");
            }
            if (!adAdapter.GetLdsObjectByDN("CN=Roles,CN=Configuration," + adAdapter.LDSRootObjectName,
                out dirEntryConfig))
            {
                DataSchemaSite.Assert.IsTrue(
                    false, 
                    "CN=Roles,CN=Configuration," 
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }

            //MS-ADTS-Schema_R768
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirEntryApp != null 
                && dirEntryConfig != null, 
                768,
                "For the Roles Container the Parent must be Application NC root or config NC root.");

            //MS-ADTS-Schema_R769
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirEntryApp.Properties["objectClass"].Contains((object)"container"), 
                769,
                "The objectClass attribute of Roles Container must be container.");

            //MS-ADTS-Schema_R770
            string systemFlag = dirEntryConfig.Properties["systemFlags"].Value.ToString();
            int systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlag, 
                systemFlagVal.ToString(), 
                770,
                "The systemFlags attribute of Roles Container must be FLAG_DISALLOW_DELETE.");


            DirectoryEntries rolesChilds = dirEntryConfig.Children;
            bool isParentRoles = true;

            foreach (DirectoryEntry child in rolesChilds)
            {
                if (!child.Parent.Name.ToString().Equals("CN=Roles"))
                    isParentRoles = false;
            }
            //MS-ADTS-Schema_R771
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isParentRoles, 
                771,
                "For each child of the Roles Container the Parent must be Roles Container.");

            //MS-ADTS-Schema_R772
            rolesChilds = dirEntryConfig.Children;
            List<DirectoryEntry> wellKnownRolesChilds = new List<DirectoryEntry>();
            bool isGroup = true;
            foreach (DirectoryEntry child in rolesChilds)
            {
                if (child.Properties["cn"].ToString().Equals("Administrators") || child.Properties["cn"].ToString().Equals("Instances")
                    || child.Properties["cn"].ToString().Equals("Readers") || child.Properties["cn"].ToString().Equals("Users"))
                {
                    wellKnownRolesChilds.Add(child);
                    if (!child.Properties["objectClass"].Contains((object)"group"))
                        isGroup = false;
                }
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isGroup, 
                772, 
                "The objectClass attribute for each child of the Roles Container must be group.");

            if (serverOS >= OSVersion.WinSvr2008)
            {
                //MS-ADTS-Schema_R773
                if (!adAdapter.GetLdsObjectByDN(
                    "CN=Administrators,CN=Roles,CN=Configuration," 
                    + adAdapter.LDSRootObjectName, 
                    out dirEntry))
                {
                    DataSchemaSite.Assume.IsTrue(
                        false, 
                        "CN=Administrators,CN=Roles,CN=Configuration," 
                        + adAdapter.LDSRootObjectName
                        + " Object is not found in server");
                }
                byte[] objSid = (byte[])dirEntry.Properties["objectSid"].Value;
                dirEntry.RefreshCache(new string[] { "primaryGrouptoken" });
                PropertyValueCollection primaryGroup = dirEntry.Properties["primaryGroupToken"];
                SecurityIdentifier sid = new SecurityIdentifier(objSid, 0);
                string objectSid = sid.ToString();
                objectSid=objectSid.Substring(objectSid.LastIndexOf('-')+1);
                 
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    objectSid,
                    primaryGroup.Value.ToString(),
                    773, 
                    @"The objectSid attribute for each child of the Roles Container must be a SID with two SubAuthority 
                    values,consisting of the objectSid of the NC root followed by the RID.");
            }

            //MS-ADTS-Schema_R778
            if (!adAdapter.GetLdsObjectByDN(
                "CN=Administrators,CN=Roles,CN=Configuration," 
                + adAdapter.LDSRootObjectName, 
                out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Administrators,CN=Roles,CN=Configuration," 
                    + adAdapter.LDSRootObjectName 
                    + " Object is not found in server");
            }
            PropertyValueCollection memberSec = dirEntry.Properties["member"];
            bool isForeignSecurityPrincipalsContained = false;
            foreach (var prop in memberSec)
            {
                if (prop.ToString().Contains("CN=ForeignSecurityPrincipals"))
                {
                    isForeignSecurityPrincipalsContained = true;
                    break;
                }
            }

            DataSchemaSite.CaptureRequirementIfIsTrue(
                isForeignSecurityPrincipalsContained,
                778, 
                @"The member attribute of Administrators Group Object must be that at least one foreignSecurityPrincipal
                is configured into this group by the administrator when creating a forest.");

            //MS-ADTS-Schema_R774
            foreach (DirectoryEntry dir1 in wellKnownRolesChilds)
            {
                PropertyValueCollection groupType = dir1.Properties["groupType"];
                //The {ACCOUNT_GROUP | SECURITY_ENABLED} flag value is equivalent to "0x80000002" with reference 
                //from ADTS groupFlags.
                int groupTypeValue = (int)groupType.Value;
                string groupValueType = "0x80000002";
                int modelGroup = Convert.ToInt32(groupValueType, 16);
                if(groupTypeValue==modelGroup)
                isGroup = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isGroup, 
                774,
                @"The groupType attribute for each child of theRoles Container must be 
                ACCOUNT_GROUP | SECURITY_ENABLED.");


            //MS-ADTS-Schema_R776
            dirEntry.RefreshCache(new string[] { "primaryGroupToken" });
            string primary = dirEntry.Properties["primaryGroupToken"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "519", 
                primary, 
                776,
                "The RID attribute of Administrators Group Object must be 519 (in the config NC).");

            //MS-ADTS-Schema_R777
            if (!adAdapter.GetLdsObjectByDN(
                "CN=Administrators,CN=Roles," + adAdapter.LDSApplicationNC, 
                out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Administrators,CN=Roles," + adAdapter.LDSApplicationNC 
                    + " Object is not found in server");
            }
            dirEntry.RefreshCache(new string[] { "primaryGroupToken" });
            primary = dirEntry.Properties["primaryGroupToken"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "512", 
                primary, 
                777,
                "The RID attribute of Administrators Group Object must be 512 (in an application NC).");

            //MS-ADTS-Schema_R779
            if (!adAdapter.GetLdsObjectByDN("CN=Readers,CN=Roles," + adAdapter.LDSApplicationNC, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Readers,CN=Roles," + adAdapter.LDSApplicationNC 
                    + " Object is not found in server");
            }
            dirEntry.RefreshCache(new string[] { "primaryGroupToken" });
            primary = dirEntry.Properties["primaryGroupToken"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "514", 
                primary, 
                779,
                "The RID attribute of Readers Group Object must be 514.");

            dirEntry.RefreshCache(new string[] { "member" });
            PropertyValueCollection member = dirEntry.Properties["member"];
            object memberValue = member.Value;

            //MS-ADTS-Schema_R775
            DataSchemaSite.CaptureRequirementIfIsNull(
                memberValue, 
                775, 
                @"Unless otherwise noted, the initial membership of the member attribute,for each child of the Roles 
                Container must be empty.");

            //MS-ADTS-Schema_R779
            if (!adAdapter.GetLdsObjectByDN(
                "CN=Readers,CN=Roles,CN=Configuration,"
                + adAdapter.LDSRootObjectName, 
                out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Readers,CN=Roles,CN=Configuration," 
                    + adAdapter.LDSRootObjectName 
                    + " Object is not found in server");
            }
            dirEntry.RefreshCache(new string[] { "primaryGroupToken" });
            primary = dirEntry.Properties["primaryGroupToken"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "514", 
                primary, 
                779, 
                "The RID attribute of Readers Group Object must be 514.");

            //MS-ADTS-Schema_R780
            if (!adAdapter.GetLdsObjectByDN("CN=Users,CN=Roles," + adAdapter.LDSApplicationNC, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Users,CN=Roles," + adAdapter.LDSApplicationNC
                    +  " Object is not found in server");
            }
            dirEntry.RefreshCache(new string[] { "primaryGroupToken" });
            primary = dirEntry.Properties["primaryGroupToken"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "513", 
                primary, 
                780, 
                "The RID attribute of Users Group Object must be 513.");

            if (!adAdapter.GetLdsObjectByDN(
                "CN=Users,CN=Roles,CN=Configuration," 
                + adAdapter.LDSRootObjectName, 
                out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Users,CN=Roles,CN=Configuration," 
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }
            dirEntry.RefreshCache(new string[] { "primaryGroupToken" });
            primary = dirEntry.Properties["primaryGroupToken"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "513", 
                primary, 
                780, 
                "The RID attribute of Users Group Object must be 513.");

            //MS-ADTS-Schema_R782
            if (!adAdapter.GetLdsObjectByDN(
                "CN=Instances,CN=Roles,CN=Configuration," 
                + adAdapter.LDSRootObjectName, 
                out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Instances,CN=Roles,CN=Configuration," 
                    + adAdapter.LDSRootObjectName 
                    +  " Object is not found in server");
            }
            dirEntry.RefreshCache(new string[] { "primaryGroupToken" });
            primary = dirEntry.Properties["primaryGroupToken"].Value.ToString();
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "518", 
                primary, 
                782, 
                "The RID attribute of Instances Group Object must be 518.");
        }

        #endregion
    }
}