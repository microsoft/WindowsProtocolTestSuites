// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using System.IO;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocol.TestSuites.ActiveDirectory;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase31 and TestCase32.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region ExtendedRights
        /// <summary>
        /// This method validates the requirements under 
        /// ExtendedRights Scenario.
        /// </summary>
        public void ValidateExtendedRights()
        {
            DirectoryEntry requiredEntry = new DirectoryEntry();
            if (!adAdapter.GetObjectByDN("CN=Extended-Rights,CN=Configuration," + adAdapter.rootDomainDN, out requiredEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Extended-Rights,CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            DirectoryEntry childEntry = new DirectoryEntry();

            childEntry = requiredEntry.Children.Find("CN=Change-Rid-Master");
            //MS-ADTS-Schema_R554
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "d58d5f36-0a98-11d1-adbb-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                554,
                @"For the Change-Rid-Master Extended Right the rightsguid attribute must be 
                d58d5f36-0a98-11d1-adbb-00c04fd8d5cd.");

            //MS-ADTS-Schema_R555
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "6617188d-8f3c-11d0-afda-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                555,
                @"For the Change-Rid-Master Extended Right the appliesTo attribute must be 
                6617188d-8f3c-11d0-afda-00c04fd930c9.");

            //This object exists in schema version 56 or greater
            if (serverOS >= OSVersion.WinSvr2012)
            {
                //MS-ADTS-Schema_DS-Clone-Domain-Controller
                childEntry = requiredEntry.Children.Find("CN=DS-Clone-Domain-Controller");

                DataSchemaSite.Assert.AreEqual<string>(
                    "3e0f7e18-2c7a-4c10-ba82-4d926db99a3e",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    @"For the DS-Clone-Domain-Controller Extended Right the rightsguid attribute must be 
                    3e0f7e18-2c7a-4c10-ba82-4d926db99a3e.");

                DataSchemaSite.Assert.AreEqual<string>(
                    "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    @"For the DS-Clone-Domain-Controller Extended Right the appliesTo attribute must be 
                    19195a5b-6da0-11d0-afd3-00c04fd930c9.");
    
                //MS-ADTS-Schema_Certificate-AutoEnrollment
                childEntry = requiredEntry.Children.Find("CN=Certificate-AutoEnrollment");
                
                DataSchemaSite.Assert.AreEqual<string>(
                    "a05b8cc2-17bc-4802-a710-e7c15ab866a2",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    @"For the Certificate-AutoEnrollment Extended Right the rightsguid attribute must be 
                    a05b8cc2-17bc-4802-a710-e7c15ab866a2.");
                
                DataSchemaSite.Assert.AreEqual<string>(
                    "e5209ca2-3bba-11d2-90cc-00c04fd91ab1",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    @"For the Certificate-AutoEnrollment Extended Right the appliesTo attribute must be 
                    e5209ca2-3bba-11d2-90cc-00c04fd91ab1.");                

                //MS-ADTS-Schema_Validated-MS-DS-Additional-DNS-Host-Name
                childEntry = requiredEntry.Children.Find("CN=Validated-MS-DS-Additional-DNS-Host-Name");

                DataSchemaSite.Assert.AreEqual<string>(
                    "80863791-dbe9-4eb8-837e-7f0ab55d9ac7",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    @"For the Validated-MS-DS-Additional-DNS-Host-Name Extended Right the rightsguid attribute must be 
                80863791-dbe9-4eb8-837e-7f0ab55d9ac7.");

                DataSchemaSite.Assert.AreEqual<string>(
                    "bf967a86-0de6-11d0-a285-00aa003049e2",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    @"For the Validated-MS-DS-Additional-DNS-Host-Name Extended Right the appliesTo attribute must be 
                bf967a86-0de6-11d0-a285-00aa003049e2.");

                //MS-ADTS-Schema_Validated-MS-DS-Behavior-Version
                childEntry = requiredEntry.Children.Find("CN=Validated-MS-DS-Behavior-Version");

                DataSchemaSite.Assert.AreEqual<string>(
                    "d31a8757-2447-4545-8081-3bb610cacbf2",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    @"For the Validated-MS-DS-Behavior-Version Extended Right the rightsguid attribute must be 
                d31a8757-2447-4545-8081-3bb610cacbf2.");

                DataSchemaSite.Assert.AreEqual<string>(
                    "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    @"For the Validated-MS-DS-Behavior-Version Extended Right the appliesTo attribute must be 
                f0f8ffab-1191-11d0-a060-00aa006c33ed.");
            }

            //This object exists in schema version 69 or greater
            if (serverOS >= OSVersion.WinSvr2012R2)
            {
                //MS-ADTS-Schema_DS-Read-Partition-Secrets
                childEntry = requiredEntry.Children.Find("CN=DS-Read-Partition-Secrets");

                DataSchemaSite.Assert.AreEqual<string>(
                    "084c93a2-620d-4879-a836-f0ae47de0e89",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    @"For the DS-Read-Partition-Secrets Extended Right the rightsguid attribute must be 
                084c93a2-620d-4879-a836-f0ae47de0e89.");

                DataSchemaSite.Assert.AreEqual<string>(
                    "26f11b08-a29d-4869-99bb-ef0b99fd883e",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    @"For the DS-Read-Partition-Secrets Extended Right the appliesTo attribute must be 
                26f11b08-a29d-4869-99bb-ef0b99fd883e.");

                //MS-ADTS-Schema_DS-Write-Partition-Secrets
                childEntry = requiredEntry.Children.Find("CN=DS-Write-Partition-Secrets");

                DataSchemaSite.Assert.AreEqual<string>(
                    "94825A8D-B171-4116-8146-1E34D8F54401",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    @"For the DS-Write-Partition-Secrets Extended Right the rightsguid attribute must be 
                94825A8D-B171-4116-8146-1E34D8F54401.");

                DataSchemaSite.Assert.AreEqual<string>(
                    "26f11b08-a29d-4869-99bb-ef0b99fd883e",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    @"For the DS-Write-Partition-Secrets Extended Right the appliesTo attribute must be 
                26f11b08-a29d-4869-99bb-ef0b99fd883e.");

                //MS-ADTS-Schema_DS-Set-Owner
                childEntry = requiredEntry.Children.Find("CN=DS-Set-Owner");

                DataSchemaSite.Assert.AreEqual<string>(
                    "4125c71f-7fac-4ff0-bcb7-f09a41325286",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    @"For the DS-Set-Owner Extended Right the rightsguid attribute must be 
                4125c71f-7fac-4ff0-bcb7-f09a41325286.");

                DataSchemaSite.Assert.AreEqual<string>(
                    "26f11b08-a29d-4869-99bb-ef0b99fd883e",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    @"For the DS-Set-Owner Extended Right the appliesTo attribute must be 
                26f11b08-a29d-4869-99bb-ef0b99fd883e.");

                //MS-ADTS-Schema_DS-Bypass-Quota
                childEntry = requiredEntry.Children.Find("CN=DS-Bypass-Quota");

                DataSchemaSite.Assert.AreEqual<string>(
                    "88a9933e-e5c8-4f2a-9dd7-2527416b8092",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    @"For the DS-Bypass-Quota Extended Right the rightsguid attribute must be 
                88a9933e-e5c8-4f2a-9dd7-2527416b8092.");

                DataSchemaSite.Assert.AreEqual<string>(
                    "26f11b08-a29d-4869-99bb-ef0b99fd883e",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    @"For the DS-Bypass-Quota Extended Right the appliesTo attribute must be 
                26f11b08-a29d-4869-99bb-ef0b99fd883e.");
            }

            childEntry = requiredEntry.Children.Find("CN=Manage-Optional-Features");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4526
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "7c0e2a7c-a419-48e4-a995-10180aad54dd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                4526,
                @"For the DS-Manage-Optional-Features Extended Right the rightsGuid attribute must be a 
                7c0e2a7c-a419-48e4-a995-10180aad54dd.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4527
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ef9e60e0-56f7-11d1-a9c6-0000f80367c1",
                childEntry.Properties["appliesTo"].Value.ToString(),
                4527,
                @"For the DS-Manage-Optional-Features Extended Right the appliesTo attribute must be 
                ef9e60e0-56f7-11d1-a9c6-0000f80367c1.");
            
            childEntry = requiredEntry.Children.Find("CN=Run-Protect-Admin-Groups-Task");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4528
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "7726b9d5-a4b4-4288-a6b2-dce952e80a7f",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                4528,
                @"For the Run-Protect-Admin-Groups-Task Extended Right the rightsguid attribute must be a 
                7726b9d5-a4b4-4288-a6b2-dce952e80a7f.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4529
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                4529,
                @"For the Run-Protect-Admin-Groups-Task Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Manage-Optional-Features");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4530
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "7c0e2a7c-a419-48e4-a995-10180aad54dd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                4530,
                @"For the Manage-Optional-Features Extended Right the rightsguid attribute must be a 
                7c0e2a7c-a419-48e4-a995-10180aad54dd.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4531
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ef9e60e0-56f7-11d1-a9c6-0000f80367c1",
                childEntry.Properties["appliesTo"].Value.ToString(),
                4531,
                @"For the Manage-Optional-Features Extended Right the appliesTo attribute must be 
                ef9e60e0-56f7-11d1-a9c6-0000f80367c1.");

            childEntry = requiredEntry.Children.Find("CN=Read-Only-Replication-Secret-Synchronization ");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4532
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "1131f6ae-9c07-11d1-f79f-00c04fc2dcd2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                4532,
                @"For the Read-Only-Replication-Secret-Synchronization Extended Right the rightsguid attribute must 
                be a 1131f6ae-9c07-11d1-f79f-00c04fc2dcd2.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4533
            string tmp = String.Empty;
            foreach (object str in childEntry.Properties["appliesTo"])
            {
                tmp += (string)str + ", ";
            }
            tmp = tmp.Substring(0, tmp.Length - ", ".Length);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 19195a5b-6da0-11d0-afd3-00c04fd930c9",
                tmp,
                4533,
                @"For theRead-Only-Replication-Secret-Synchronization Extended Right the appliesTo attribute must 
                be bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Do-Garbage-Collection");

            //MS-ADTS-Schema_R556
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "fec364e0-0a98-11d1-adbb-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                556,
                @"For the Do-Garbage-Collection Extended Right the rightsguid attribute must be 
                fec364e0-0a98-11d1-adbb-00c04fd8d5cd.");

            //MS-ADTS-Schema_R557
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                childEntry.Properties["appliesTo"].Value.ToString(),
                557,
                @"For the Do-Garbage-Collection Extended Right the appliesTo attribute must be 
                f0f8ffab-1191-11d0-a060-00aa006c33ed.");

            childEntry = requiredEntry.Children.Find("CN=Recalculate-Hierarchy");

            //MS-ADTS-Schema_R558
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "0bc1554e-0a99-11d1-adbb-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                558,
                @"For the Recalculate-Hierarchy Extended Right the rightsguid attribute must be 
                0bc1554e-0a99-11d1-adbb-00c04fd8d5cd.");

            //MS-ADTS-Schema_R559
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                childEntry.Properties["appliesTo"].Value.ToString(),
                559,
                @"For the Recalculate-Hierarchy Extended Right the appliesTo attribute must be 
                f0f8ffab-1191-11d0-a060-00aa006c33ed.");

            childEntry = requiredEntry.Children.Find("CN=Allocate-Rids");

            //MS-ADTS-Schema_R560
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "1abd7cf8-0a99-11d1-adbb-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                560,
                "For the Allocate-Rids Extended Right the rightsguid attribute must be 1abd7cf8-0a99-11d1-adbb-00c04fd8d5cd.");
            //MS-ADTS-Schema_R561
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                childEntry.Properties["appliesTo"].Value.ToString(),
                561,
                "For the Allocate-Rids Extended Right the appliesTo attribute must be f0f8ffab-1191-11d0-a060-00aa006c33ed.");

            childEntry = requiredEntry.Children.Find("CN=Change-PDC");
            //MS-ADTS-Schema_R562
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bae50096-4752-11d1-9052-00c04fc2d4cf",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                562,
                "For the Change-PDC Extended Right the rightsguid attribute must be bae50096-4752-11d1-9052-00c04fc2d4cf.");

            //MS-ADTS-Schema_R563
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                563,
                "For the Change-PDC Extended Right the appliesTo attribute must be 19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Add-GUID");
            //MS-ADTS-Schema_R564
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "440820ad-65b4-11d1-a3da-0000f875ae0d",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                564,
                "For the Add-GUID Extended Right the rightsguid attribute must be 440820ad-65b4-11d1-a3da-0000f875ae0d.");

            //MS-ADTS-Schema_R565
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                565,
                "For the Add-GUID Extended Right the appliesTo attribute must be 19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Change-Domain-Master");

            //MS-ADTS-Schema_R566
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "014bf69c-7b3b-11d1-85f6-08002be74fab",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                566,
                @"For the Change-Domain-Master Extended Right the rightsguid attribute must be 
                014bf69c-7b3b-11d1-85f6-08002be74fab.");

            //MS-ADTS-Schema_R567
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ef9e60e0-56f7-11d1-a9c6-0000f80367c1",
                childEntry.Properties["appliesTo"].Value.ToString(),
                567,
                @"For the Change-Domain-Master Extended Right the appliesTo attribute must be 
                ef9e60e0-56f7-11d1-a9c6-0000f80367c1.");

            childEntry = requiredEntry.Children.Find("CN= Public-Information");

            //MS-ADTS-Schema_R568
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "e48d0154-bcf8-11d1-8702-00c04fb96050",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                568,
                @"For the Public-Information Extended Right the rightsguid attribute must be 
                e48d0154-bcf8-11d1-8702-00c04fb96050.");

            string multipleGuidPublicInfo = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidPublicInfo += o.ToString() + ",";
            }
            multipleGuidPublicInfo = multipleGuidPublicInfo.Substring(0, multipleGuidPublicInfo.Length - 1);

            //MS-ADTS-Schema_R569
            if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidPublicInfo,
                    569,
                    "For the Public-Information Extended Right the appliesTo attribute must be 4828CC14-1437-45bc-9B07-"
                + "AD6F015E5F28, bf967a86-0de6-11d0-a285-00aa003049e2, bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            // This is a server2k8 r2 Doc change        
            else if (serverOS <= OSVersion.WinSvr2012R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidPublicInfo,
                    4603,
                    "For the Public-Information Extended Right the appliesTo attribute must be "
                + "4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a86-0de6-11d0-a285-00aa003049e2, "
                + "bf967aba-0de6-11d0-a285-00aa003049e2, ce206244-5827-4a86-ba1c-1c0c386c1b64[For AD DS only].");
            }
            else
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "7b8b558a-93a5-4af7-adca-c017e67f1057,4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "ce206244-5827-4a86-ba1c-1c0c386c1b64,bf967a86-0de6-11d0-a285-00aa003049e2,"
                    + "bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidPublicInfo,
                    4603,
                    "For the Public-Information Extended Right the appliesTo attribute must be "
                    + "7b8b558a-93a5-4af7-adca-c017e67f1057, 4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2, bf967aba-0de6-11d0-a285-00aa003049e2, "
                    + "ce206244-5827-4a86-ba1c-1c0c386c1b64[For AD DS only] (Windows Server 2016).");
            }

            childEntry = requiredEntry.Children.Find("CN=msmq-Receive-Dead-Letter");

            //MS-ADTS-Schema_R570
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4b6e08c0-df3c-11d1-9c86-006008764d0e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                570,
                @"For the msmq-Receive-Dead-Letter Extended Right the rightsguid attribute must be 
                4b6e08c0-df3c-11d1-9c86-006008764d0e.");

            //MS-ADTS-Schema_R571
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9a0dc344-c100-11d1-bbc5-0080c76670c0",
                childEntry.Properties["appliesTo"].Value.ToString(),
                571,
                @"For the msmq-Receive-Dead-Letter Extended Right the appliesTo attribute must be 
                9a0dc344-c100-11d1-bbc5-0080c76670c0.");

            childEntry = requiredEntry.Children.Find("CN=msmq-Peek-Dead-Letter");
            //MS-ADTS-Schema_R572
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4b6e08c1-df3c-11d1-9c86-006008764d0e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                572,
                @"For the msmq-Peek-Dead-Letter Extended Right the rightsguid attribute must be 
                4b6e08c1-df3c-11d1-9c86-006008764d0e.");

            //MS-ADTS-Schema_R573
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9a0dc344-c100-11d1-bbc5-0080c76670c0",
                childEntry.Properties["appliesTo"].Value.ToString(),
                573,
                @"For the msmq-Peek-Dead-Letter Extended Right the appliesTo attribute must be 
                9a0dc344-c100-11d1-bbc5-0080c76670c0.");

            childEntry = requiredEntry.Children.Find("CN=msmq-Receive-computer-Journal");
            //MS-ADTS-Schema_R574
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4b6e08c2-df3c-11d1-9c86-006008764d0e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                574,
                @"For the msmq-Receive-computer-Journal Extended Right the rightsguid attribute must 
                be 4b6e08c2-df3c-11d1-9c86-006008764d0e.");

            //MS-ADTS-Schema_R575
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9a0dc344-c100-11d1-bbc5-0080c76670c0",
                childEntry.Properties["appliesTo"].Value.ToString(),
                575,
                @"For the msmq-Receive-computer-Journal Extended Right the appliesTo attribute must 
                be 9a0dc344-c100-11d1-bbc5-0080c76670c0.");

            childEntry = requiredEntry.Children.Find("CN=msmq-Peek-computer-Journal");
            //MS-ADTS-Schema_R576
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4b6e08c3-df3c-11d1-9c86-006008764d0e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                576,
                @"For the msmq-Peek-computer-Journal Extended Right the rightsguid attribute must be 
                4b6e08c3-df3c-11d1-9c86-006008764d0e.");

            //MS-ADTS-Schema_R577
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9a0dc344-c100-11d1-bbc5-0080c76670c0",
                childEntry.Properties["appliesTo"].Value.ToString(),
                577,
                @"For the msmq-Peek-computer-Journal Extended Right the appliesTo attribute must 
                be 9a0dc344-c100-11d1-bbc5-0080c76670c0.");

            childEntry = requiredEntry.Children.Find("CN=msmq-Receive");
            //MS-ADTS-Schema_R578
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "06bd3200-df3e-11d1-9c86-006008764d0e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                578,
                "For the msmq-Receive Extended Right the rightsguid attribute must be 06bd3200-df3e-11d1-9c86-006008764d0e.");

            //MS-ADTS-Schema_R579
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9a0dc343-c100-11d1-bbc5-0080c76670c0",
                childEntry.Properties["appliesTo"].Value.ToString(),
                579,
                "For the msmq-Receive Extended Right the appliesTo attribute must be 9a0dc343-c100-11d1-bbc5-0080c76670c0.");

            childEntry = requiredEntry.Children.Find("CN=msmq-Peek");
            //MS-ADTS-Schema_R580
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "06bd3201-df3e-11d1-9c86-006008764d0e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                580,
                "For the msmq-Peek Extended Right the rightsguid attribute must be 06bd3201-df3e-11d1-9c86-006008764d0e.");

            //MS-ADTS-Schema_R581
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9a0dc343-c100-11d1-bbc5-0080c76670c0",
                childEntry.Properties["appliesTo"].Value.ToString(),
                581,
                "For the msmq-Peek Extended Right the appliesTo attribute must be 9a0dc343-c100-11d1-bbc5-0080c76670c0.");

            childEntry = requiredEntry.Children.Find("CN=msmq-Send");
            //MS-ADTS-Schema_R582
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "06bd3202-df3e-11d1-9c86-006008764d0e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                582,
                "For the msmq-Send Extended Right the rightsguid attribute must be 06bd3202-df3e-11d1-9c86-006008764d0e.");

            string multipleGuidForMSMQSend = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidForMSMQSend += o.ToString() + ",";
            }
            multipleGuidForMSMQSend = multipleGuidForMSMQSend.Substring(0, multipleGuidForMSMQSend.Length - 1);

            //MS-ADTS-Schema_R583
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "46b27aac-aafa-4ffb-b773-e5bf621ee87b,"
                + "9a0dc343-c100-11d1-bbc5-0080c76670c0",
                multipleGuidForMSMQSend,
                583,
                "For the msmq-Send Extended Right the appliesTo attribute must be 46b27aac-aafa-4ffb-b773-e5bf621ee87b "
            + "and 9a0dc343-c100-11d1-bbc5-0080c76670c0.");

            childEntry = requiredEntry.Children.Find("CN=msmq-Receive-journal");

            //MS-ADTS-Schema_R584
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "06bd3203-df3e-11d1-9c86-006008764d0e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                584,
                "For the msmq-Receive-journal Extended Right the rightsguid attribute must be 06bd3203-df3e-11d1-9c86-006008764d0e.");

            //MS-ADTS-Schema_R585
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9a0dc343-c100-11d1-bbc5-0080c76670c0",
                childEntry.Properties["appliesTo"].Value.ToString(),
                585,
                "For the msmq-Receive-journal Extended Right the appliesTo attribute must be 9a0dc343-c100-11d1-bbc5-0080c76670c0.");

            childEntry = requiredEntry.Children.Find("CN=msmq-Open-Connector");
            //MS-ADTS-Schema_R586
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "b4e60130-df3f-11d1-9c86-006008764d0e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                586,
                "For the msmq-Open-Connector Extended Right the rightsguid attribute must be b4e60130-df3f-11d1-9c86-006008764d0e.");

            //MS-ADTS-Schema_R587
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967ab3-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                587,
                "For the msmq-Open-Connector Extended Right the appliesTo attribute must be bf967ab3-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=Apply-Group-Policy");
            //MS-ADTS-Schema_R588
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "edacfd8f-ffb3-11d1-b41d-00a0c968f939",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                588,
                "For the Apply-Group-Policy Extended Right the rightsguid attribute must be edacfd8f-ffb3-11d1-b41d-00a0c968f939.");

            //MS-ADTS-Schema_R589
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f30e3bc2-9ff0-11d1-b603-0000f80367c1",
                childEntry.Properties["appliesTo"].Value.ToString(),
                589,
                @"For the Apply-Group-Policy Extended Right the appliesTo attribute must be 
                f30e3bc2-9ff0-11d1-b603-0000f80367c1.");

            childEntry = requiredEntry.Children.Find("CN=RAS-Information");
            //MS-ADTS-Schema_R590
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "037088f8-0ae1-11d2-b422-00a0c968f939",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                590,
                @"For the RAS-Information Extended Right the rightsguid attribute must be 
                037088f8-0ae1-11d2-b422-00a0c968f939.");

            string multipleGuidRASInfo = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidRASInfo += o.ToString() + ",";
            }
            multipleGuidRASInfo = multipleGuidRASInfo.Substring(0, multipleGuidRASInfo.Length - 1);

            //MS-ADTS-Schema_R591
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidRASInfo,
                591,
                @"For the RAS-Information Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28 and bf967aba-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=DS-Install-Replica");
            //MS-ADTS-Schema_R592
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9923a32a-3607-11d2-b9be-0000f87a36b2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                592,
                @"For the DS-Install-Replica Extended Right the rightsguid attribute must be 
                9923a32a-3607-11d2-b9be-0000f87a36b2.");

            //MS-ADTS-Schema_R593
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                593,
                @"For the DS-Install-Replica Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Change-Infrastructure-Master");

            //MS-ADTS-Schema_R594
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "cc17b1fb-33d9-11d2-97d4-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                594,
                @"For the Change-Infrastructure-Master Extended Right the rightsguid attribute must be 
                cc17b1fb-33d9-11d2-97d4-00c04fd8d5cd.");

            //MS-ADTS-Schema_R595
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "2df90d89-009f-11d2-aa4c-00c04fd7d83a",
                childEntry.Properties["appliesTo"].Value.ToString(),
                595,
                @"For the Change-Infrastructure-Master Right the appliesTo attribute must be 
                2df90d89-009f-11d2-aa4c-00c04fd7d83a.");

            childEntry = requiredEntry.Children.Find("CN=Update-Schema-Cache");

            //MS-ADTS-Schema_R596
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "be2bb760-7f46-11d2-b9ad-00c04f79f805",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                596,
                @"For the Update-Schema-Cache Extended Right the rightsguid attribute must be 
                be2bb760-7f46-11d2-b9ad-00c04f79f805.");

            //MS-ADTS-Schema_R597
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                597,
                "For the Update-Schema-Cache Right the appliesTo attribute must be bf967a8f-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=Recalculate-Security-Inheritance");
            //MS-ADTS-Schema_R598
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "62dd28a8-7f46-11d2-b9ad-00c04f79f805",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                598,
                @"For the Recalculate-Security-Inheritance Extended Right the rightsguid attribute must be 
                62dd28a8-7f46-11d2-b9ad-00c04f79f805.");

            //MS-ADTS-Schema_R599
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                childEntry.Properties["appliesTo"].Value.ToString(),
                599,
                @"For the Recalculate-Security-Inheritance Right the appliesTo attribute must be 
                f0f8ffab-1191-11d0-a060-00aa006c33ed.");

            childEntry = requiredEntry.Children.Find("CN=DS-Check-Stale-Phantoms");
            //MS-ADTS-Schema_R600
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "69ae6200-7f46-11d2-b9ad-00c04f79f805",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                600,
                @"For the DS-Check-Stale-Phantoms Extended Right the rightsguid attribute must be 
                69ae6200-7f46-11d2-b9ad-00c04f79f805.");

            //MS-ADTS-Schema_R601
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                childEntry.Properties["appliesTo"].Value.ToString(),
                601,
                @"For the DS-Check-Stale-Phantoms Right the appliesTo attribute must be 
                f0f8ffab-1191-11d0-a060-00aa006c33ed.");

            childEntry = requiredEntry.Children.Find("CN=Certificate-Enrollment");
            //MS-ADTS-Schema_R602
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "0e10c968-78fb-11d2-90d4-00c04f79dc55",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                602,
                @"For the Certificate-Enrollment Extended Right the rightsguid attribute must be 
                0e10c968-78fb-11d2-90d4-00c04f79dc55.");

            //MS-ADTS-Schema_R603
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "e5209ca2-3bba-11d2-90cc-00c04fd91ab1",
                childEntry.Properties["appliesTo"].Value.ToString(),
                603,
                @"For the Certificate-Enrollment Right the appliesTo attribute must be 
                e5209ca2-3bba-11d2-90cc-00c04fd91ab1.");

            childEntry = requiredEntry.Children.Find("CN=Self-Membership");
            //MS-ADTS-Schema_R604
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf9679c0-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                604,
                @"For the Self-Membership Extended Right the rightsguid attribute must be 
                bf9679c0-0de6-11d0-a285-00aa003049e2.");

            //MS-ADTS-Schema_R605
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a9c-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                605,
                @"For the Self-Membership Extended Right the appliesTo attribute must be 
                bf967a9c-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=Validated-DNS-Host-Name");
            //MS-ADTS-Schema_R606
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "72e39547-7b18-11d1-adef-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                606,
                @"For the Validated-DNS-Host-Name Extended Right the rightsguid attribute must be 
                72e39547-7b18-11d1-adef-00c04fd8d5cd.");

            //MS-ADTS-Schema_R607
            if (serverOS >= OSVersion.WinSvr2012)
            {
                string tmpValue = String.Empty;
                foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                {
                    tmpValue += o.ToString() + ",";
                }
                tmpValue = tmpValue.Substring(0, tmpValue.Length - 1);

                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "7b8b558a-93a5-4af7-adca-c017e67f1057,ce206244-5827-4a86-ba1c-1c0c386c1b64,bf967a86-0de6-11d0-a285-00aa003049e2",
                    tmpValue,
                    10607,
                    @"For the Validated-DNS-Host-Name Extended Right the appliesTo attribute must be 
                      7b8b558a-93a5-4af7-adca-c017e67f1057,ce206244-5827-4a86-ba1c-1c0c386c1b64,
                      bf967a86-0de6-11d0-a285-00aa003049e2.");
            }
            else if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "bf967a86-0de6-11d0-a285-00aa003049e2",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    607,
                    @"For the Validated-DNS-Host-Name Extended Right the appliesTo attribute must be 
                    bf967a86-0de6-11d0-a285-00aa003049e2.");
            }
            // This is a server2k8 r2 Doc change
            else
            {
                string tmpValue = String.Empty;
                foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                {
                    tmpValue += o.ToString() + ",";
                }
                tmpValue = tmpValue.Substring(0, tmpValue.Length - 1);

                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "ce206244-5827-4a86-ba1c-1c0c386c1b64,bf967a86-0de6-11d0-a285-00aa003049e2",
                    tmpValue,
                    10607,
                    @"For the Validated-DNS-Host-Name Extended Right the appliesTo attribute must be "
                    + "bf967a86-0de6-11d0-a285-00aa003049e2 and ce206244-5827-4a86-ba1c-1c0c386c1b64"
                    + "[for Windows server 2008 R2].");
            }
            childEntry = requiredEntry.Children.Find("CN=Validated-SPN");

            //MS-ADTS-Schema_R608
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f3a64788-5306-11d1-a9c5-0000f80367c1",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                608,
                @"For the Validated-SPN Extended Right the rightsguid attribute must be 
                f3a64788-5306-11d1-a9c5-0000f80367c1.");

            //MS-ADTS-Schema_R609
            if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "bf967a86-0de6-11d0-a285-00aa003049e2",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    609,
                    @"For the Validated-SPN Extended Right the appliesTo attribute must be 
                    bf967a86-0de6-11d0-a285-00aa003049e2.");
            }
            // This is a server2k8 r2 Doc change
            else if (serverOS <= OSVersion.WinSvr2012R2)
            {
                string tmpValue = String.Empty;
                foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                {
                    tmpValue += o.ToString() + ",";
                }
                tmpValue = tmpValue.Substring(0, tmpValue.Length - 1);

                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2",
                    tmpValue,
                    10609,
                    @"For the Validated-SPN Extended Right the appliesTo attribute must be "
                    + "bf967a86-0de6-11d0-a285-00aa003049e2 and ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2008 R2].");
            }
            else
            {
                string tmpValue = String.Empty;
                foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                {
                    tmpValue += o.ToString() + ",";
                }
                tmpValue = tmpValue.Substring(0, tmpValue.Length - 1);

                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "7b8b558a-93a5-4af7-adca-c017e67f1057,"
                    + "ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2",
                    tmpValue,
                    10609,
                    @"For the Validated-SPN Extended Right the appliesTo attribute must be "
                    + "7b8b558a-93a5-4af7-adca-c017e67f1057, bf967a86-0de6-11d0-a285-00aa003049e2 and ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2016].");
            }
            childEntry = requiredEntry.Children.Find("CN=Generate-RSoP-Planning");

            //MS-ADTS-Schema_R610
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "b7b1b3dd-ab09-4242-9e30-9980e5d322f7",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                610,
                @"For the Generate-RSoP-Planning Extended Right the rightsguid attribute must be 
                b7b1b3dd-ab09-4242-9e30-9980e5d322f7.");

            string multipleGuidGenerateRSoP = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidGenerateRSoP += o.ToString() + ",";
            }
            multipleGuidGenerateRSoP = multipleGuidGenerateRSoP.Substring(0, multipleGuidGenerateRSoP.Length - 1);

            //MS-ADTS-Schema_R611
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9,"
                + "bf967aa5-0de6-11d0-a285-00aa003049e2",
                multipleGuidGenerateRSoP,
                611,
                @"For the Generate-RSoP-Planning Extended Right the appliesTo attribute must be "
                + "19195a5b-6da0-11d0-afd3-00c04fd930c9 and bf967aa5-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=Refresh-Group-Cache");

            //MS-ADTS-Schema_R612
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9432c620-033c-4db7-8b58-14ef6d0bf477",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                612,
                @"For the Refresh-Group-Cache Extended Right the rightsguid attribute must be "
                + "9432c620-033c-4db7-8b58-14ef6d0bf477.");

            //MS-ADTS-Schema_R613
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                childEntry.Properties["appliesTo"].Value.ToString(),
                613,
                @"For the Refresh-Group-Cache Extended Right the appliesTo attribute must be "
                + "f0f8ffab-1191-11d0-a060-00aa006c33ed.");

            if (serverOS >= OSVersion.WinSvr2008)
            {
                childEntry = requiredEntry.Children.Find("CN=Reload-SSL-Certificate");
                //MS-ADTS-Schema_R614
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "1a60ea8d-58a6-4b20-bcdc-fb71eb8a9ff8",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    614,
                    @"For the Reload-SSL-Certificate Extended Right the rightsguid attribute must be 
                    1a60ea8d-58a6-4b20-bcdc-fb71eb8a9ff8.");

                //MS-ADTS-Schema_R615
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    615,
                    @"For the Reload-SSL-Certificate Extended Right the appliesTo attribute must be 
                    f0f8ffab-1191-11d0-a060-00aa006c33ed.");
            }

            childEntry = requiredEntry.Children.Find("CN=SAM-Enumerate-Entire-Domain");
            //MS-ADTS-Schema_R616
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "91d67418-0135-4acc-8d79-c08e857cfbec",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                616,
                @"For the SAM-Enumerate-Entire-Domain Extended Right the rightsguid attribute must be 
                91d67418-0135-4acc-8d79-c08e857cfbec.");

            //MS-ADTS-Schema_R617
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967aad-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                617,
                @"For the SAM-Enumerate-Entire-Domain Extended Right the appliesTo attribute must be 
                bf967aad-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=Generate-RSoP-Logging");
            //MS-ADTS-Schema_R618
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "b7b1b3de-ab09-4242-9e30-9980e5d322f7",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                618,
                @"For the Generate-RSoP-Logging Extended Right the rightsguid attribute must be 
                b7b1b3de-ab09-4242-9e30-9980e5d322f7.");

            string multipleGuidGenerateRsoPLog = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidGenerateRsoPLog += o.ToString() + ",";
            }
            multipleGuidGenerateRsoPLog = multipleGuidGenerateRsoPLog.Substring(0, multipleGuidGenerateRsoPLog.Length - 1);

            //MS-ADTS-Schema_R619
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9,"
                + "bf967aa5-0de6-11d0-a285-00aa003049e2",
                multipleGuidGenerateRsoPLog,
                619,
                @"For the Generate-RSoP-Logging Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9 and bf967aa5-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=Domain-Other-Parameters");
            //MS-ADTS-Schema_R620
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "b8119fd0-04f6-4762-ab7a-4986c76b3f9a",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                620,
                @"For the Domain-Other-Parameters Extended Right the rightsguid attribute must be 
                b8119fd0-04f6-4762-ab7a-4986c76b3f9a.");

            //MS-ADTS-Schema_R621
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                621,
                @"For the Domain-Other-Parameters Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=DNS-Host-Name-Attributes");
            //MS-ADTS-Schema_R622
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "72e39547-7b18-11d1-adef-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                622,
                @"For the DNS-Host-Name-Attributes Extended Right the rightsguid attribute must be 
                72e39547-7b18-11d1-adef-00c04fd8d5cd.");

            //MS-ADTS-Schema_R623
            if (serverOS >= OSVersion.WinSvr2012)
            {
                string tmpValue = String.Empty;
                foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                {
                    tmpValue += o.ToString() + ",";
                }
                tmpValue = tmpValue.Substring(0, tmpValue.Length - 1);

                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "7b8b558a-93a5-4af7-adca-c017e67f1057,"
                    + "ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2",
                    tmpValue,
                    10623,
                    @"For the DNS-Host-Name-Attributes Extended Right the appliesTo attribute must be
                    7b8b558a-93a5-4af7-adca-c017e67f1057,ce206244-5827-4a86-ba1c-1c0c386c1b64,
                    bf967a86-0de6-11d0-a285-00aa003049e2.");
            }
            else if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "bf967a86-0de6-11d0-a285-00aa003049e2",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    623,
                    @"For the DNS-Host-Name-Attributes Extended Right the appliesTo attribute must be 
                    bf967a86-0de6-11d0-a285-00aa003049e2.");
            }
            // This is a server2k8 r2 Doc change
            else
            {
                string tmpValue = String.Empty;
                foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                {
                    tmpValue += o.ToString() + ",";
                }
                tmpValue = tmpValue.Substring(0, tmpValue.Length - 1);

                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2",
                    tmpValue,
                    10623,
                    @"For the DNS-Host-Name-Attributes Extended Right the appliesTo attribute must be
                    bf967a86-0de6-11d0-a285-00aa003049e2 and 
                    ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2008 R2].");
            }

            childEntry = requiredEntry.Children.Find("CN=Create-Inbound-Forest-Trust");
            //MS-ADTS-Schema_R624
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "e2a36dc9-ae17-47c3-b58b-be34c55ba633",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                624,
                @"For the Create-Inbound-Forest-Trust Extended Right the rightsguid attribute must be 
                e2a36dc9-ae17-47c3-b58b-be34c55ba633.");

            //MS-ADTS-Schema_R625
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                625,
                @"For the Create-Inbound-Forest-Trust Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=DS-Replication-Get-Changes-All");
            //MS-ADTS-Schema_R626
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "1131f6ad-9c07-11d1-f79f-00c04fc2dcd2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                626,
                @"For the DS-Replication-Get-Changes-All Extended Right the rightsguid attribute must be 
                1131f6ad-9c07-11d1-f79f-00c04fc2dcd2.");

            string multipleGuidDSRepl = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidDSRepl += o.ToString() + ",";
            }

            multipleGuidDSRepl = multipleGuidDSRepl.Substring(0, multipleGuidDSRepl.Length - 1);
            //MS-ADTS-Schema_R627
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,"
                + "bf967a87-0de6-11d0-a285-00aa003049e2,19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidDSRepl,
                627,
                @"For the DS-Replication-Get-Changes-All Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                and 19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Migrate-SID-History");
            //MS-ADTS-Schema_R628
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "BA33815A-4F93-4c76-87F3-57574BFF8109",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                628,
                @"For the Migrate-SID-History Extended Right the rightsguid attribute must be 
                BA33815A-4F93-4c76-87F3-57574BFF8109.");

            //MS-ADTS-Schema_R629
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                629,
                @"For the Migrate-SID-History Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Reanimate-Tombstones");
            //MS-ADTS-Schema_R630
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "45EC5156-DB7E-47bb-B53F-DBEB2D03C40F",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                630,
                @"For the Reanimate-Tombstones Extended Right the rightsguid attribute must be 
                45EC5156-DB7E-47bb-B53F-DBEB2D03C40F.");

            string multipleGuidReanimate = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidReanimate += o.ToString() + ",";
            }
            multipleGuidReanimate = multipleGuidReanimate.Substring(0, multipleGuidReanimate.Length - 1);
            //MS-ADTS-Schema_R631
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,"
                + "bf967a87-0de6-11d0-a285-00aa003049e2,19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidReanimate,
                631,
                @"For the Reanimate-Tombstones Extended Right the appliesTo attribute must be
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                and 19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Allowed-To-Authenticate");
            //MS-ADTS-Schema_R632
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "68B1D179-0D15-4d4f-AB71-46152E79A7BC",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                632,
                @"For the Allowed-To-Authenticate Extended Right the rightsguid attribute must be 
                68B1D179-0D15-4d4f-AB71-46152E79A7BC.");

            string multipleGuidAllowed = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidAllowed += o.ToString() + ",";
            }

            multipleGuidAllowed = multipleGuidAllowed.Substring(0, multipleGuidAllowed.Length - 1);
            //MS-ADTS-Schema_R633
            if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828cc14-1437-45bc-9b07-ad6f015e5f28,"
                    + "bf967aba-0de6-11d0-a285-00aa003049e2,bf967a86-0de6-11d0-a285-00aa003049e2",
                    multipleGuidAllowed,
                    633,
                    @"For the Allowed-To-Authenticate Extended Right the appliesTo 
                    attribute must be 4828cc14-1437-45bc-9b07-ad6f015e5f28, 
                    bf967aba-0de6-11d0-a285-00aa003049e2, and bf967a86-0de6-11d0-a285-00aa003049e2.");
            }
            // This is a server2k8 r2 Doc change
            else if (serverOS <= OSVersion.WinSvr2012R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828cc14-1437-45bc-9b07-ad6f015e5f28,"
                     + "bf967aba-0de6-11d0-a285-00aa003049e2,ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                     + "bf967a86-0de6-11d0-a285-00aa003049e2",
                    multipleGuidAllowed,
                    10633,
                    @"For the Allowed-To-Authenticate Extended Right the appliesTo 
                    attribute must be 4828cc14-1437-45bc-9b07-ad6f015e5f28, 
                    bf967aba-0de6-11d0-a285-00aa003049e2, bf967a86-0de6-11d0-a285-00aa003049e2,
                    ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2008 R2].");
            }
            else
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "7b8b558a-93a5-4af7-adca-c017e67f1057,4828cc14-1437-45bc-9b07-ad6f015e5f28,"
                    + "bf967aba-0de6-11d0-a285-00aa003049e2,ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2",
                    multipleGuidAllowed,
                    10633,
                    @"For the Allowed-To-Authenticate Extended Right the appliesTo 
                    attribute must be 7b8b558a-93a5-4af7-adca-c017e67f1057, 4828cc14-1437-45bc-9b07-ad6f015e5f28, 
                    bf967aba-0de6-11d0-a285-00aa003049e2, bf967a86-0de6-11d0-a285-00aa003049e2,
                    ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2016].");
            }

            childEntry = requiredEntry.Children.Find("CN=DS-Execute-Intentions-Script");
            //MS-ADTS-Schema_R634
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "2f16c4a5-b98e-432c-952a-cb388ba33f2e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                634,
                @"For the DS-Execute-Intentions-Script Extended Right the rightsguid attribute must be 
                2f16c4a5-b98e-432c-952a-cb388ba33f2e.");

            //MS-ADTS-Schema_R635
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ef9e60e0-56f7-11d1-a9c6-0000f80367c1",
                childEntry.Properties["appliesTo"].Value.ToString(),
                635,
                @"For the DS-Execute-Intentions-Script Extended Right the appliesTo attribute must be 
                ef9e60e0-56f7-11d1-a9c6-0000f80367c1.");

            childEntry = requiredEntry.Children.Find("CN=DS-Replication-Monitor-Topology");
            //MS-ADTS-Schema_R636
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f98340fb-7c5b-4cdb-a00b-2ebdfa115a96",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                636,
                @"For the DS-Replication-Monitor-Topology Extended Right the rightsguid attribute must be 
                f98340fb-7c5b-4cdb-a00b-2ebdfa115a96.");

            string multipleGuidReplMonitor = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidReplMonitor += o.ToString() + ",";
            }

            multipleGuidReplMonitor = multipleGuidReplMonitor.Substring(0, multipleGuidReplMonitor.Length - 1);
            //MS-ADTS-Schema_R637
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,"
                + "bf967a87-0de6-11d0-a285-00aa003049e2,19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidReplMonitor,
                637,
                @"For the DS-Replication-Monitor-Topology Extended Right 
                the appliesTo attribute must be bf967a8f-0de6-11d0-a285-00aa003049e2, 
                bf967a87-0de6-11d0-a285-00aa003049e2, and 19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Update-Password-Not-Required-Bit");
            //MS-ADTS-Schema_R638
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "280f369c-67c7-438e-ae98-1d46f3c6f541",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                638,
                @"For the Update-Password-Not-Required-Bit Extended Right the rightsguid attribute must be 
                280f369c-67c7-438e-ae98-1d46f3c6f541.");

            //MS-ADTS-Schema_R639
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                639,
                @"For the Update-Password-Not-Required-Bit Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Unexpire-Password");
            //MS-ADTS-Schema_R640
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ccc2dc7d-a6ad-4a7a-8846-c04e3cc53501",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                640,
                @"For the Unexpire-Password Extended Right the rightsguid attribute must be 
                ccc2dc7d-a6ad-4a7a-8846-c04e3cc53501.");

            //MS-ADTS-Schema_R641
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                641,
                @"For the Unexpire-Password Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=Enable-Per-User-Reversibly-Encrypted-Password");
            //MS-ADTS-Schema_R642
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "05c74c5e-4deb-43b4-bd9f-86664c2a7fd5",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                642,
                @"For the Enable-Per-User-Reversibly-Encrypted-Password Extended Right the rightsguid attribute must be 
                05c74c5e-4deb-43b4-bd9f-86664c2a7fd5.");

            //MS-ADTS-Schema_R643
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                643,
                @"For the Enable-Per-User-Reversibly-Encrypted-Password Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=DS-Query-Self-Quota");
            //MS-ADTS-Schema_R644
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4ecc03fe-ffc0-4947-b630-eb672a8a9dbc",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                644,
                @"For theDS-Query-Self-Quota Extended Right the rightsguid attribute must be 
                4ecc03fe-ffc0-4947-b630-eb672a8a9dbc.");

            //MS-ADTS-Schema_R645
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "da83fc4f-076f-4aea-b4dc-8f4dab9b5993",
                childEntry.Properties["appliesTo"].Value.ToString(),
                645,
                @"For the DS-Query-Self-Quota Extended Right the appliesTo attribute must be 
                da83fc4f-076f-4aea-b4dc-8f4dab9b5993.");

            if (serverOS >= OSVersion.WinSvr2008)
            {
                childEntry = requiredEntry.Children.Find("CN=Private-Information");
                //MS-ADTS-Schema_R646
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "91e647de-d96f-4b70-9557-d63ff4f3ccd8",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    646,
                    @"For the Private-Information Extended Right the rightsguid attribute must be 
                    91e647de-d96f-4b70-9557-d63ff4f3ccd8.");

                string multipleGuidPrivateInfo = String.Empty;

                foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                {
                    multipleGuidPrivateInfo += o.ToString() + ",";
                }
                multipleGuidPrivateInfo = multipleGuidPrivateInfo.Substring(0, multipleGuidPrivateInfo.Length - 1);
                //MS-ADTS-Schema_R647
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "bf967aba-0de6-11d0-a285-00aa003049e2,"
                    + "4828cc14-1437-45bc-9b07-ad6f015e5f28",
                    multipleGuidPrivateInfo,
                    647,
                    @"For the Private-Information Extended Right the appliesTo attribute must be
                    bf967aba-0de6-11d0-a285-00aa003049e2 and 4828cc14-1437-45bc-9b07-ad6f015e5f28.");

                childEntry = requiredEntry.Children.Find("CN=MS-TS-GatewayAccess");
                //MS-ADTS-Schema_R648
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "ffa6f046-ca4b-4feb-b40d-04dfee722543",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    648,
                    @"For the MS-TS-GatewayAccess Extended Right the rightsguid attribute must be 
                    ffa6f046-ca4b-4feb-b40d-04dfee722543.");

                //MS-ADTS-Schema_R649
                if (serverOS < OSVersion.WinSvr2008R2)
                {
                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        "bf967a86-0de6-11d0-a285-00aa003049e2",
                        childEntry.Properties["appliesTo"].Value.ToString(),
                        649,
                        @"For the MS-TS-GatewayAccess Extended Right the appliesTo attribute must be 
                        bf967a86-0de6-11d0-a285-00aa003049e2.");
                }
                // This is a server2k8 r2 Doc change
                else if (serverOS <= OSVersion.WinSvr2012R2)
                {
                    string tmpValue = String.Empty;

                    foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                    {
                        tmpValue += o.ToString() + ",";
                    }
                    tmpValue = tmpValue.Substring(0, tmpValue.Length - 1);

                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        "ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                        + "bf967a86-0de6-11d0-a285-00aa003049e2",
                        tmpValue,
                        10649,
                        @"For the MS-TS-GatewayAccess Extended Right the appliesTo attribute must be
                        bf967a86-0de6-11d0-a285-00aa003049e2 and 
                        ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2008 R2].");
                }
                else
                {
                    string tmpValue = String.Empty;

                    foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                    {
                        tmpValue += o.ToString() + ",";
                    }
                    tmpValue = tmpValue.Substring(0, tmpValue.Length - 1);

                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        "7b8b558a-93a5-4af7-adca-c017e67f1057,"
                        + "ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                        + "bf967a86-0de6-11d0-a285-00aa003049e2",
                        tmpValue,
                        10649,
                        @"For the MS-TS-GatewayAccess Extended Right the appliesTo attribute must be
                        7b8b558a-93a5-4af7-adca-c017e67f1057, 
                        bf967a86-0de6-11d0-a285-00aa003049e2 and 
                        ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2016].");
                }

                childEntry = requiredEntry.Children.Find("CN=Terminal-Server-License-Server");
                //MS-ADTS-Schema_R650
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "5805bc62-bdc9-4428-a5e2-856a0f4c185e",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    650,
                    @"For the Terminal-Server-License-Server Extended Right the rightsguid attribute must be "
                + "5805bc62-bdc9-4428-a5e2-856a0f4c185e.");

                string multipleGuidTerminalServer = String.Empty;

                foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                {
                    multipleGuidTerminalServer += o.ToString() + ",";
                }

                multipleGuidTerminalServer = multipleGuidTerminalServer.Substring(0, multipleGuidTerminalServer.Length - 1);
                //MS-ADTS-Schema_R651
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "bf967aba-0de6-11d0-a285-00aa003049e2,"
                    + "4828cc14-1437-45bc-9b07-ad6f015e5f28",
                    multipleGuidTerminalServer,
                    651,
                    @"For the Terminal-Server-License-Server Extended Right the appliesTo attribute must be 
                    bf967aba-0de6-11d0-a285-00aa003049e2 and 4828cc14-1437-45bc-9b07-ad6f015e5f28.");
            }

            childEntry = requiredEntry.Children.Find("CN=Domain-Administer-Server");
            //MS-ADTS-Schema_R652
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ab721a52-1e2f-11d0-9819-00aa0040529b",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                652,
                @"For the Domain-Administer-Server Extended Right the rightsguid attribute must be 
                ab721a52-1e2f-11d0-9819-00aa0040529b.");

            //MS-ADTS-Schema_R653
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967aad-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                653,
                @"For the Domain-Administer-Server Extended Right the appliesTo attribute must be 
                bf967aad-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=User-Change-Password");
            //MS-ADTS-Schema_R654
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ab721a53-1e2f-11d0-9819-00aa0040529b",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                654,
                @"For the User-Change-Password Extended Right the rightsguid attribute must be 
                ab721a53-1e2f-11d0-9819-00aa0040529b.");

            string multipleGuidUserChangePwd = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidUserChangePwd += o.ToString() + ",";
            }
            multipleGuidUserChangePwd = multipleGuidUserChangePwd.Substring(0, multipleGuidUserChangePwd.Length - 1);

            //MS-ADTS-Schema_R655
            if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidUserChangePwd,
                    655,
                    @"For the User-Change-Password Extended Right the appliesTo attribute must be
                    4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a86-0de6-11d0-a285-00aa003049e2,
                    and bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            //For Windows server 2008 R2
            else
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64," +
                   "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                   multipleGuidUserChangePwd,
                   4604,
                   @"For the User-Change-Password Extended Right the appliesTo attribute must be
                   4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a86-0de6-11d0-a285-00aa003049e2,
                   bf967aba-0de6-11d0-a285-00aa003049e2,ce206244-5827-4a86-ba1c-1c0c386c1b64
                   [for Windows server 2008 R2].");
            }

            childEntry = requiredEntry.Children.Find("CN=User-Force-Change-Password");
            //MS-ADTS-Schema_R656
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "00299570-246d-11d0-a768-00aa006e0529",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                656,
                @"For the User-Force-Change-Password Extended Right the rightsguid attribute must be 
                00299570-246d-11d0-a768-00aa006e0529.");

            string multipleGuidUserForceChangePwd = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidUserForceChangePwd += o.ToString() + ",";
            }
            multipleGuidUserForceChangePwd = multipleGuidUserForceChangePwd.Substring(0, multipleGuidUserForceChangePwd.Length - 1);
            //MS-ADTS-Schema_R657
            if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidUserForceChangePwd,
                    657,
                    @"For the User-Force-Change-Password Extended Right the appliesTo attribute must be "
                + "4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a86-0de6-11d0-a285-00aa003049e2, bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            //For Windows server 2008 R2
            else
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidUserForceChangePwd,
                    4605,
                    @"For the User-Force-Change-Password Extended Right the appliesTo attribute must be 
                    4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a86-0de6-11d0-a285-00aa003049e2,
                    bf967aba-0de6-11d0-a285-00aa003049e2,ce206244-5827-4a86-ba1c-1c0c386c1b64
                    [for Windows server 2008 R2].");
            }
            childEntry = requiredEntry.Children.Find("CN=Send-As");
            //MS-ADTS-Schema_R658
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ab721a54-1e2f-11d0-9819-00aa0040529b",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                658,
                "For the Send-As Extended Right the rightsguid attribute must be ab721a54-1e2f-11d0-9819-00aa0040529b.");

            string multipleGuidSendAs = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidSendAs += o.ToString() + ",";
            }
            multipleGuidSendAs = multipleGuidSendAs.Substring(0, multipleGuidSendAs.Length - 1);

            //MS-ADTS-Schema_R659
            if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidSendAs,
                    659,
                    @"For the Send-As Extended Right the appliesTo attribute must be
                    4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a86-0de6-11d0-a285-00aa003049e2,
                    and bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            // This is a server2k8 r2 Doc change
            else if (serverOS <= OSVersion.WinSvr2012R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidSendAs,
                    10659,
                    @"For the Send-As Extended Right the appliesTo attribute must be 
                    4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a86-0de6-11d0-a285-00aa003049e2, 
                    bf967aba-0de6-11d0-a285-00aa003049e2, 
                    ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2008 R2].");
            }
            else
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "7b8b558a-93a5-4af7-adca-c017e67f1057,4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "ce206244-5827-4a86-ba1c-1c0c386c1b64,bf967a86-0de6-11d0-a285-00aa003049e2,"
                    + "bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidSendAs,
                    10659,
                    @"For the Send-As Extended Right the appliesTo attribute must be 
                    7b8b558a-93a5-4af7-adca-c017e67f1057, 4828CC14-1437-45bc-9B07-AD6F015E5F28, 
                    bf967a86-0de6-11d0-a285-00aa003049e2, bf967aba-0de6-11d0-a285-00aa003049e2, 
                    ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2016].");
            }

            childEntry = requiredEntry.Children.Find("CN=Receive-As");
            //MS-ADTS-Schema_R660
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ab721a56-1e2f-11d0-9819-00aa0040529b",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                660,
                @"For the Receive-As Extended Right the rightsguid attribute must be 
                ab721a56-1e2f-11d0-9819-00aa0040529b.");

            string multipleGuidReceiveAs = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidReceiveAs += o.ToString() + ",";
            }

            multipleGuidReceiveAs = multipleGuidReceiveAs.Substring(0, multipleGuidReceiveAs.Length - 1);
            //MS-ADTS-Schema_R661
            if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidReceiveAs,
                    661,
                    @"For the Receive-As Extended Right the appliesTo attribute must 
                    be 4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a86-0de6-11d0-a285-00aa003049e2,
                    and bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            // This is a server2k8 r2 Doc change
            else if (serverOS <= OSVersion.WinSvr2012R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                   multipleGuidReceiveAs,
                   10661,
                   @"For the Receive-As Extended Right the appliesTo attribute must
                   be 4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a86-0de6-11d0-a285-00aa003049e2,
                   bf967aba-0de6-11d0-a285-00aa003049e2, 
                   ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2008 R2].");
            }
            else
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "7b8b558a-93a5-4af7-adca-c017e67f1057,4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "ce206244-5827-4a86-ba1c-1c0c386c1b64,bf967a86-0de6-11d0-a285-00aa003049e2,"
                    + "bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidReceiveAs,
                    10661,
                    @"For the Receive-As Extended Right the appliesTo attribute must
                    be 7b8b558a-93a5-4af7-adca-c017e67f1057, 4828CC14-1437-45bc-9B07-AD6F015E5F28, 
                    bf967a86-0de6-11d0-a285-00aa003049e2, bf967aba-0de6-11d0-a285-00aa003049e2, 
                    ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2016].");
            }

            childEntry = requiredEntry.Children.Find("CN=Send-To");
            //MS-ADTS-Schema_R662
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ab721a55-1e2f-11d0-9819-00aa0040529b",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                662,
                "For the Send-To Extended Right the rightsguid attribute must be ab721a55-1e2f-11d0-9819-00aa0040529b.");

            //MS-ADTS-Schema_R663
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a9c-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                663,
                "For the Send-To Extended Right the appliesTo attribute must be bf967a9c-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=Domain-Password");
            //MS-ADTS-Schema_R664
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "c7407360-20bf-11d0-a768-00aa006e0529",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                664,
                @"For the Domain-Password Extended Right the rightsguid attribute must be 
                c7407360-20bf-11d0-a768-00aa006e0529.");

            string multipleGuidDomainPwd = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidDomainPwd += o.ToString() + ",";
            }

            multipleGuidDomainPwd = multipleGuidDomainPwd.Substring(0, multipleGuidDomainPwd.Length - 1);
            //MS-ADTS-Schema_R665
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9,"
                + "19195a5a-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidDomainPwd,
                665,
                @"For the Domain-Password Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9 and 19195a5a-6da0-11d0-afd3-00c04fd930c9.");

            childEntry = requiredEntry.Children.Find("CN=General-Information");
            //MS-ADTS-Schema_R666
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "59ba2f42-79a2-11d0-9020-00c04fc2d3cf",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                666,
                @"For the General-Information Extended Right the rightsguid attribute must be 
                59ba2f42-79a2-11d0-9020-00c04fc2d3cf.");

            string multipleGuidGeneralInfo = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidGeneralInfo += o.ToString() + ",";
            }

            multipleGuidGeneralInfo = multipleGuidGeneralInfo.Substring(0, multipleGuidGeneralInfo.Length - 1);
            //MS-ADTS-Schema_R667
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidGeneralInfo,
                667,
                @"For the General-Information Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28 and bf967aba-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=User-Account-Restrictions");
            //MS-ADTS-Schema_R668
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4c164200-20c0-11d0-a768-00aa006e0529",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                668,
                @"For the User-Account-Restrictions Extended Right the rightsguid attribute must be 
                4c164200-20c0-11d0-a768-00aa006e0529.");

            string multipleGuidUserAccount = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidUserAccount += o.ToString() + ",";
            }
            multipleGuidUserAccount = multipleGuidUserAccount.Substring(0, multipleGuidUserAccount.Length - 1);
            //MS-ADTS-Schema_R669
            if (serverOS >= OSVersion.WinSvr2012)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "7b8b558a-93a5-4af7-adca-c017e67f1057,"
                    + "4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidUserAccount,
                    10669,
                    @"For the User-Account-Restrictions Extended Right the appliesTo
                    attribute must be 7b8b558a-93a5-4af7-adca-c017e67f1057,
                    4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,
                    bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            else if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidUserAccount,
                    669,
                    @"For the User-Account-Restrictions Extended Right the appliesTo 
                    attribute must be 4828CC14-1437-45bc-9B07-AD6F015E5F28, 
                    bf967a86-0de6-11d0-a285-00aa003049e2, and bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            // This is a server2k8 r2 Doc change
            else
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidUserAccount,
                    10669,
                    @"For the User-Account-Restrictions Extended Right the appliesTo
                    attribute must be 4828CC14-1437-45bc-9B07-AD6F015E5F28,
                    bf967a86-0de6-11d0-a285-00aa003049e2, bf967aba-0de6-11d0-a285-00aa003049e2, 
                    ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2008 R2].");
            }

            childEntry = requiredEntry.Children.Find("CN=User-Logon");
            //MS-ADTS-Schema_R670
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "5f202010-79a5-11d0-9020-00c04fc2d4cf",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                670,
                "For the User-Logon Extended Right the rightsguid attribute must be 5f202010-79a5-11d0-9020-00c04fc2d4cf.");

            string multipleGuidUserLogon = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidUserLogon += o.ToString() + ",";
            }
            multipleGuidUserLogon = multipleGuidUserLogon.Substring(0, multipleGuidUserLogon.Length - 1);
            //MS-ADTS-Schema_R671
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidUserLogon,
                671,
                @"For the User-Logon Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28 and bf967aba-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=Membership");
            //MS-ADTS-Schema_R672
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bc0ac240-79a9-11d0-9020-00c04fc2d4cf",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                672,
                @"For the Membership Extended Right the rightsguid attribute must be 
                bc0ac240-79a9-11d0-9020-00c04fc2d4cf.");

            string multipleGuidMembership = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidMembership += o.ToString() + ",";
            }
            multipleGuidMembership = multipleGuidMembership.Substring(0, multipleGuidMembership.Length - 1);
            //MS-ADTS-Schema_R673
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidMembership,
                673,
                @"For the Membership Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28 and bf967aba-0de6-11d0-a285-00aa003049e2.");

            childEntry = requiredEntry.Children.Find("CN=Open-Address-Book");
            //MS-ADTS-Schema_R674
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "a1990816-4298-11d1-ade2-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                674,
                @"For the Open-Address-Book Extended Right the rightsguid attribute must be 
                a1990816-4298-11d1-ade2-00c04fd8d5cd.");

            //MS-ADTS-Schema_R675
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "3e74f60f-3e73-11d1-a9c0-0000f80367c1",
                childEntry.Properties["appliesTo"].Value.ToString(),
                675,
                @"For the Open-Address-Book Extended Right the appliesTo attribute must be 
                3e74f60f-3e73-11d1-a9c0-0000f80367c1.");
        }

        #endregion

        #region LDSExtendedRights Validation.

        /// <summary>
        /// This method validates the requirements under 
        /// LDSExtendedRights Scenario.
        /// </summary>
        public void ValidateLDSExtendedRights()
        {
            DirectoryEntry requiredEntry = new DirectoryEntry();
            if (!adAdapter.GetLdsObjectByDN("CN=Extended-Rights,CN=Configuration," + adAdapter.LDSRootObjectName, out requiredEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Extended-Rights,CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }
            DirectoryEntry childEntry = new DirectoryEntry();

            //MS-ADTS-Schema_R556 and MS-ADTS-Schema_R557
            childEntry = requiredEntry.Children.Find("CN=Do-Garbage-Collection");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "fec364e0-0a98-11d1-adbb-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                556,
                @"For the Do-Garbage-Collection Extended Right the rightsguid attribute must be 
                fec364e0-0a98-11d1-adbb-00c04fd8d5cd.");

            //MS-ADTS-Schema_R564 and MS-ADTS-Schema_R565
            childEntry = requiredEntry.Children.Find("CN=Add-GUID");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "440820ad-65b4-11d1-a3da-0000f875ae0d",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                564,
                "For the Add-GUID Extended Right the rightsguid attribute must be 440820ad-65b4-11d1-a3da-0000f875ae0d.");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                565,
                "For the Add-GUID Extended Right the appliesTo attribute must be 19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            //MS-ADTS-Schema_R568 and MS-ADTS-Schema_R569
            childEntry = requiredEntry.Children.Find("CN= Public-Information");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "e48d0154-bcf8-11d1-8702-00c04fb96050",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                568,
                @"For the Public-Information Extended Right the rightsguid attribute must be 
                e48d0154-bcf8-11d1-8702-00c04fb96050.");

            string multipleGuidPublicInfo = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidPublicInfo += o.ToString() + ",";
            }
            multipleGuidPublicInfo = multipleGuidPublicInfo.Substring(0, multipleGuidPublicInfo.Length - 1);

            //MS-ADTS-Schema_R592 and MS-ADTS-Schema_R593
            childEntry = requiredEntry.Children.Find("CN=DS-Install-Replica");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "9923a32a-3607-11d2-b9be-0000f87a36b2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                592,
                @"For the DS-Install-Replica Extended Right the rightsguid attribute must be 
                9923a32a-3607-11d2-b9be-0000f87a36b2.");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                593,
                @"For the DS-Install-Replica Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            //MS-ADTS-Schema_R596 and MS-ADTS-Schema_R597
            childEntry = requiredEntry.Children.Find("CN=Update-Schema-Cache");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "be2bb760-7f46-11d2-b9ad-00c04f79f805",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                596,
                @"For the Update-Schema-Cache Extended Right the rightsguid attribute must be 
                be2bb760-7f46-11d2-b9ad-00c04f79f805.");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                597,
                "For the Update-Schema-Cache Right the appliesTo attribute must be bf967a8f-0de6-11d0-a285-00aa003049e2.");

            //MS-ADTS-Schema_R598 and MS-ADTS-Schema_R599
            childEntry = requiredEntry.Children.Find("CN=Recalculate-Security-Inheritance");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "62dd28a8-7f46-11d2-b9ad-00c04f79f805",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                598,
                @"For the Recalculate-Security-Inheritance Extended Right the rightsguid attribute must be 
                62dd28a8-7f46-11d2-b9ad-00c04f79f805.");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                childEntry.Properties["appliesTo"].Value.ToString(),
                599,
                @"For the Recalculate-Security-Inheritance Right the appliesTo attribute must be 
                f0f8ffab-1191-11d0-a060-00aa006c33ed.");

            //MS-ADTS-Schema_R604 and MS-ADTS-Schema_R605
            childEntry = requiredEntry.Children.Find("CN=Self-Membership");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf9679c0-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                604,
                @"For the Self-Membership Extended Right the rightsguid attribute must be 
                bf9679c0-0de6-11d0-a285-00aa003049e2.");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a9c-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                605,
                @"For the Self-Membership Extended Right the appliesTo attribute must be 
                bf967a9c-0de6-11d0-a285-00aa003049e2.");

            //MS-ADTS-Schema_R626 and MS-ADTS-Schema_R627
            childEntry = requiredEntry.Children.Find("CN=DS-Replication-Get-Changes-All");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "1131f6ad-9c07-11d1-f79f-00c04fc2dcd2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                626,
                @"For the DS-Replication-Get-Changes-All Extended Right the rightsguid attribute must be 
                1131f6ad-9c07-11d1-f79f-00c04fc2dcd2.");

            string multipleGuidDSRepl = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidDSRepl += o.ToString() + ",";
            }
            multipleGuidDSRepl = multipleGuidDSRepl.Substring(0, multipleGuidDSRepl.Length - 1);

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,"
                + "bf967a87-0de6-11d0-a285-00aa003049e2,19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidDSRepl,
                627,
                @"For the DS-Replication-Get-Changes-All Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                and 19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            //MS-ADTS-Schema_R630 and MS-ADTS-Schema_R631
            childEntry = requiredEntry.Children.Find("CN=Reanimate-Tombstones");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "45EC5156-DB7E-47bb-B53F-DBEB2D03C40F",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                630,
                @"For the Reanimate-Tombstones Extended Right the rightsguid attribute must be 
                45EC5156-DB7E-47bb-B53F-DBEB2D03C40F.");

            string multipleGuidReanimate = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidReanimate += o.ToString() + ",";
            }
            multipleGuidReanimate = multipleGuidReanimate.Substring(0, multipleGuidReanimate.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,"
                + "bf967a87-0de6-11d0-a285-00aa003049e2,19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidReanimate,
                631,
                @"For the Reanimate-Tombstones Extended Right the appliesTo attribute must be
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                and 19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            //MS-ADTS-Schema_R634 and MS-ADTS-Schema_R635
            childEntry = requiredEntry.Children.Find("CN=DS-Execute-Intentions-Script");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "2f16c4a5-b98e-432c-952a-cb388ba33f2e",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                634,
                @"For the DS-Execute-Intentions-Script Extended Right the rightsguid attribute must be 
                2f16c4a5-b98e-432c-952a-cb388ba33f2e.");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ef9e60e0-56f7-11d1-a9c6-0000f80367c1",
                childEntry.Properties["appliesTo"].Value.ToString(),
                635,
                @"For the DS-Execute-Intentions-Script Extended Right the appliesTo attribute must be 
                ef9e60e0-56f7-11d1-a9c6-0000f80367c1.");

            //MS-ADTS-Schema_R640 and MS-ADTS-Schema_R641
            childEntry = requiredEntry.Children.Find("CN=Unexpire-Password");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ccc2dc7d-a6ad-4a7a-8846-c04e3cc53501",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                640,
                @"For the Unexpire-Password Extended Right the rightsguid attribute must be 
                ccc2dc7d-a6ad-4a7a-8846-c04e3cc53501.");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                childEntry.Properties["appliesTo"].Value.ToString(),
                641,
                @"For the Unexpire-Password Extended Right the appliesTo attribute must be 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            //MS-ADTS-Schema_R644 and MS-ADTS-Schema_R645
            childEntry = requiredEntry.Children.Find("CN=DS-Query-Self-Quota");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4ecc03fe-ffc0-4947-b630-eb672a8a9dbc",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                644,
                @"For theDS-Query-Self-Quota Extended Right the rightsguid attribute must be 
                4ecc03fe-ffc0-4947-b630-eb672a8a9dbc.");

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "da83fc4f-076f-4aea-b4dc-8f4dab9b5993",
                childEntry.Properties["appliesTo"].Value.ToString(),
                645,
                @"For the DS-Query-Self-Quota Extended Right the appliesTo attribute must be 
                da83fc4f-076f-4aea-b4dc-8f4dab9b5993.");

            //MS-ADTS-Schema_R654 and MS-ADTS-Schema_R655
            childEntry = requiredEntry.Children.Find("CN=User-Change-Password");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "ab721a53-1e2f-11d0-9819-00aa0040529b",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                654,
                @"For the User-Change-Password Extended Right the rightsguid attribute must be 
                ab721a53-1e2f-11d0-9819-00aa0040529b.");

            string multipleGuidUserChangePwd = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidUserChangePwd += o.ToString() + ",";
            }
            multipleGuidUserChangePwd = multipleGuidUserChangePwd.Substring(0, multipleGuidUserChangePwd.Length - 1);


            //MS-ADTS-Schema_R656 and MS-ADTS-Schema_R657
            childEntry = requiredEntry.Children.Find("CN=User-Force-Change-Password");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>("00299570-246d-11d0-a768-00aa006e0529",
                childEntry.Properties["rightsGuid"].Value.ToString(), 656,
                @"For the User-Force-Change-Password Extended Right the rightsguid attribute must be 
                00299570-246d-11d0-a768-00aa006e0529.");

            string multipleGuidUserForceChangePwd = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidUserForceChangePwd += o.ToString() + ",";
            }
            multipleGuidUserForceChangePwd = multipleGuidUserForceChangePwd.Substring(0, multipleGuidUserForceChangePwd.Length - 1);

            //MS-ADTS-Schema_R666 and MS-ADTS-Schema_R667
            childEntry = requiredEntry.Children.Find("CN=General-Information");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "59ba2f42-79a2-11d0-9020-00c04fc2d3cf",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                666,
                @"For the General-Information Extended Right the rightsguid attribute must be 
                59ba2f42-79a2-11d0-9020-00c04fc2d3cf.");

            string multipleGuidGeneralInfo = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidGeneralInfo += o.ToString() + ",";
            }
            multipleGuidGeneralInfo = multipleGuidGeneralInfo.Substring(0, multipleGuidGeneralInfo.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidGeneralInfo,
                667,
                @"For the General-Information Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28 and bf967aba-0de6-11d0-a285-00aa003049e2.");

            //MS-ADTS-Schema_R668 and MS-ADTS-Schema_R669
            childEntry = requiredEntry.Children.Find("CN=User-Account-Restrictions");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4c164200-20c0-11d0-a768-00aa006e0529",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                668,
                @"For the User-Account-Restrictions Extended Right the rightsguid attribute must be 
                4c164200-20c0-11d0-a768-00aa006e0529.");

            string multipleGuidUserAccount = String.Empty;

            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidUserAccount += o.ToString() + ",";
            }
            multipleGuidUserAccount = multipleGuidUserAccount.Substring(0, multipleGuidUserAccount.Length - 1);

            //MS-ADTS-Schema_R670 and MS-ADTS-Schema_R671
            childEntry = requiredEntry.Children.Find("CN=User-Logon");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "5f202010-79a5-11d0-9020-00c04fc2d4cf",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                670,
                "For the User-Logon Extended Right the rightsguid attribute must be 5f202010-79a5-11d0-9020-00c04fc2d4cf.");

            string multipleGuidUserLogon = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidUserLogon += o.ToString() + ",";
            }

            multipleGuidUserLogon = multipleGuidUserLogon.Substring(0, multipleGuidUserLogon.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidUserLogon,
                671,
                @"For the User-Logon Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28 and bf967aba-0de6-11d0-a285-00aa003049e2.");

            //MS-ADTS-Schema_R672 and MS-ADTS-Schema_R673
            childEntry = requiredEntry.Children.Find("CN=Membership");
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bc0ac240-79a9-11d0-9020-00c04fc2d4cf",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                672,
                @"For the Membership Extended Right the rightsguid attribute must be 
                bc0ac240-79a9-11d0-9020-00c04fc2d4cf.");

            string multipleGuidMembership = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidMembership += o.ToString() + ",";
            }
            multipleGuidMembership = multipleGuidMembership.Substring(0, multipleGuidMembership.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>("4828CC14-1437-45bc-9B07-AD6F015E5F28," +
                "bf967aba-0de6-11d0-a285-00aa003049e2", multipleGuidMembership, 673,
                @"For the Membership Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28 and bf967aba-0de6-11d0-a285-00aa003049e2.");

            if (serverOS == OSVersion.WinSvr2008R2)
            {
                childEntry = requiredEntry.Children.Find("CN=Reload-SSL-Certificate");
                //MS-ADTS-Schema_R614
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "1a60ea8d-58a6-4b20-bcdc-fb71eb8a9ff8",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    614,
                    @"For the Reload-SSL-Certificate Extended Right the rightsguid attribute must be 
                    1a60ea8d-58a6-4b20-bcdc-fb71eb8a9ff8.");

                //MS-ADTS-Schema_R615
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "f0f8ffab-1191-11d0-a060-00aa006c33ed",
                    childEntry.Properties["appliesTo"].Value.ToString(),
                    615,
                    @"For the Reload-SSL-Certificate Extended Right the appliesTo attribute must be 
                    f0f8ffab-1191-11d0-a060-00aa006c33ed.");
            }
        }

        #endregion

        #region DS ExtendedRights section 7.1.2.7.63 to 7.1.2.7.70 Validation

        public void ValidateExtendedRights2()
        {
            DirectoryEntry requiredEntry = new DirectoryEntry();
            if (!adAdapter.GetObjectByDN("CN=Extended-Rights,CN=Configuration," + adAdapter.rootDomainDN, out requiredEntry))
            {
                DataSchemaSite.Assert.IsTrue(
                    false,
                    "CN=Extended-Rights,CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            DirectoryEntry childEntry = new DirectoryEntry();

            #region Personal-Information Extended Right

            childEntry = requiredEntry.Children.Find("CN=Personal-Information");
            //MS-AD_Schema_R883
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "77B5B886-944A-11d1-AEBD-0000F80367C1",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                883,
                @"For the Personal-Information Extended Right the rightsguid attribute must be a 
                77B5B886-944A-11d1-AEBD-0000F80367C1.");

            //MS-AD_Schema_R884
            string multipleGuidPersonalInfo = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidPersonalInfo += o.ToString() + ",";
            }
            multipleGuidPersonalInfo = multipleGuidPersonalInfo.Substring(0, multipleGuidPersonalInfo.Length - 1);
            if (serverOS >= OSVersion.Win2016)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "7b8b558a-93a5-4af7-adca-c017e67f1057,641E87A4-8326-4771-BA2D-C706DF35E35A,4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,bf967a86-0de6-11d0-a285-00aa003049e2,5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidPersonalInfo,
                    10884,
                    @"For the Personal-Information Extended Right the appliesTo attribute must be 
                    7b8b558a-93a5-4af7-adca-c017e67f1057,641E87A4-8326-4771-BA2D-C706DF35E35A,
                    4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,
                    bf967a86-0de6-11d0-a285-00aa003049e2,5cb41ed0-0e4c-11d0-a286-00aa003049e2,
                    bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            else if (serverOS >= OSVersion.WinSvr2012)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "641E87A4-8326-4771-BA2D-C706DF35E35A,4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,bf967a86-0de6-11d0-a285-00aa003049e2,5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidPersonalInfo,
                    10884,
                    @"For the Personal-Information Extended Right the appliesTo attribute must be 
                    641E87A4-8326-4771-BA2D-C706DF35E35A,4828CC14-1437-45bc-9B07-AD6F015E5F28,
                    ce206244-5827-4a86-ba1c-1c0c386c1b64,bf967a86-0de6-11d0-a285-00aa003049e2,
                    5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            else if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a86-0de6-11d0-a285-00aa003049e2,"
                    + "5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidPersonalInfo,
                    884,
                    @"For the Personal-Information Extended Right the appliesTo attribute must be 
                    4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a86-0de6-11d0-a285-00aa003049e2,
                    5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            //For Windows server 2008 R2
            else
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,ce206244-5827-4a86-ba1c-1c0c386c1b64,"
                    + "bf967a86-0de6-11d0-a285-00aa003049e2,5cb41ed0-0e4c-11d0-a286-00aa003049e2,"
                    + "bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidPersonalInfo,
                    10884,
                    @"For the Personal-Information Extended Right the appliesTo attribute must be 
                    4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a86-0de6-11d0-a285-00aa003049e2,
                    5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2,
                    ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2008 R2].");
            }

            #endregion

            #region Email-Information Extended Right

            childEntry = requiredEntry.Children.Find("CN=Email-Information");
            //MS-AD_Schema_R885
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "E45795B2-9455-11d1-AEBD-0000F80367C1",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                885,
                @"For the Email-Information Extended Right the rightsguid attribute must be a 
                E45795B2-9455-11d1-AEBD-0000F80367C1.");

            //MS-AD_Schema_R886
            string multipleGuidEmailInfo = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidEmailInfo += o.ToString() + ",";
            }
            multipleGuidEmailInfo = multipleGuidEmailInfo.Substring(0, multipleGuidEmailInfo.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a9c-0de6-11d0-a285-00aa003049e2,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidEmailInfo,
                886,
                @"For the Email-Information Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a9c-0de6-11d0-a285-00aa003049e2, 
                bf967aba-0de6-11d0-a285-00aa003049e2.");

            #endregion

            #region Web-Information Extended Right

            childEntry = requiredEntry.Children.Find("CN=Web-Information");
            //MS-AD_Schema_R887
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "E45795B3-9455-11d1-AEBD-0000F80367C1",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                887,
                @"For the Web-Information Extended Right the rightsguid attribute must be a 
                E45795B3-9455-11d1-AEBD-0000F80367C1.");

            //MS-AD_Schema_R888
            string multipleGuidWebInfo = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidWebInfo += o.ToString() + ",";
            }
            multipleGuidWebInfo = multipleGuidWebInfo.Substring(0, multipleGuidWebInfo.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,5cb41ed0-0e4c-11d0-a286-00aa003049e2,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidWebInfo,
                888,
                @"For the Web-Information Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28, 5cb41ed0-0e4c-11d0-a286-00aa003049e2, 
                bf967aba-0de6-11d0-a285-00aa003049e2.");

            #endregion

            #region DS-Replication-Get-Changes Extended Right

            childEntry = requiredEntry.Children.Find("CN=DS-Replication-Get-Changes");
            //MS-AD_Schema_R889
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "1131f6aa-9c07-11d1-f79f-00c04fc2dcd2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                889,
                @"For the DS-Replication-Get-Changes Extended Right the rightsguid attribute must be a 
                1131f6aa-9c07-11d1-f79f-00c04fc2dcd2.");

            //MS-AD_Schema_R890
            string multipleGuidGetchanges = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidGetchanges += o.ToString() + ",";
            }
            multipleGuidGetchanges = multipleGuidGetchanges.Substring(0, multipleGuidGetchanges.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,bf967a87-0de6-11d0-a285-00aa003049e2,"
                + "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidGetchanges,
                890,
                @"For the DS-Replication-Get-Changes Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            #endregion

            #region DS-Replication-Synchronize Extended Right

            childEntry = requiredEntry.Children.Find("CN=DS-Replication-Synchronize");
            //MS-AD_Schema_R891
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "1131f6ab-9c07-11d1-f79f-00c04fc2dcd2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                891,
                @"For the DS-Replication-Synchronize Extended Right the rightsguid attribute must be a 
                1131f6ab-9c07-11d1-f79f-00c04fc2dcd2.");

            //MS-AD_Schema_R892
            string multipleGuidSynchronize = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidSynchronize += o.ToString() + ",";
            }
            multipleGuidSynchronize = multipleGuidSynchronize.Substring(0, multipleGuidSynchronize.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,bf967a87-0de6-11d0-a285-00aa003049e2,"
                + "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidSynchronize,
                892,
                @"For the DS-Replication-Synchronize Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            #endregion

            #region DS-Replication-Manage-Topology Extended Right

            childEntry = requiredEntry.Children.Find("CN=DS-Replication-Manage-Topology");
            //MS-AD_Schema_R893
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "1131f6ac-9c07-11d1-f79f-00c04fc2dcd2",
                childEntry.Properties["rightsGuid"].Value.ToString(), 893,
                @"For the DS-Replication-Manage-Topology Extended Right the rightsguid attribute must be a 
                1131f6ac-9c07-11d1-f79f-00c04fc2dcd2.");

            //MS-AD_Schema_R894
            string multipleGuidManageTopology = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidManageTopology += o.ToString() + ",";
            }
            multipleGuidManageTopology = multipleGuidManageTopology.Substring(0, multipleGuidManageTopology.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,bf967a87-0de6-11d0-a285-00aa003049e2,"
                + "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidManageTopology,
                894,
                @"For the DS-Replication-Manage-Topology Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            #endregion

            #region Change-Schema-Master Extended Right

            childEntry = requiredEntry.Children.Find("CN=Change-Schema-Master");
            //MS-AD_Schema_R895
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "e12b56b6-0a95-11d1-adbb-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                895,
                @"For the Change-Schema-Master Extended Right the rightsguid attribute must be a 
                e12b56b6-0a95-11d1-adbb-00c04fd8d5cd.");

            //MS-AD_Schema_R896
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                896,
                @"For the Change-Schema-Master Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2.");

            #endregion

            #region DS-Replication-Get-Changes-In-Filtered-Set Extended Right
            if (serverOS >= OSVersion.WinSvr2008)
            {
                childEntry = requiredEntry.Children.Find("CN=DS-Replication-Get-Changes-In-Filtered-Set");
                //MS-AD_Schema_R897
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "89e95b76-444d-4c62-991a-0facbeda640c",
                    childEntry.Properties["rightsGuid"].Value.ToString(),
                    897,
                    @"For the DS-Replication-Get-Changes-In-Filtered-Set Extended Right the rightsguid attribute must be 
                    a 89e95b76-444d-4c62-991a-0facbeda640c.");

                //MS-AD_Schema_R898
                bool isR898Satisfied = false;
                string multipleGuidChngsInFilteredSet = String.Empty;
                foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
                {
                    multipleGuidChngsInFilteredSet += o.ToString() + ",";
                }
                multipleGuidChngsInFilteredSet = multipleGuidChngsInFilteredSet.Substring(0, multipleGuidChngsInFilteredSet.Length - 1);

                foreach (string item in
                    "19195a5b-6da0-11d0-afd3-00c04fd930c9,bf967a87-0de6-11d0-a285-00aa003049e2,bf967a8f-0de6-11d0-a285-00aa003049e2".Split(','))
                {
                    if (multipleGuidChngsInFilteredSet.Contains(item))
                    {
                        isR898Satisfied = true;
                    }
                    else
                    {
                        isR898Satisfied = false;
                        break;
                    }
                }

                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isR898Satisfied,
                    898,
                    @"For the DS-Replication-Get-Changes-In-Filtered-Set Extended Right the appliesTo attribute must be 
                    19195a5b-6da0-11d0-afd3-00c04fd930c9, bf967a87-0de6-11d0-a285-00aa003049e2, 
                    bf967a8f-0de6-11d0-a285-00aa003049e2.");
            }

            #endregion
        }

        #endregion

        #region LDS ExtendedRights section 7.1.2.7.63 to 7.1.2.7.70 Validation

        public void ValidateLDSExtendedRights2()
        {
            DirectoryEntry requiredEntry = new DirectoryEntry();
            if (!adAdapter.GetLdsObjectByDN("CN=Extended-Rights,CN=Configuration," + adAdapter.LDSRootObjectName, out requiredEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Extended-Rights,CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }
            DirectoryEntry childEntry = new DirectoryEntry();

            #region Personal-Information Extended Right

            childEntry = requiredEntry.Children.Find("CN=Personal-Information");
            //MS-AD_Schema_R883
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "77B5B886-944A-11d1-AEBD-0000F80367C1",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                883,
                @"For the Personal-Information Extended Right the rightsguid attribute must be a 
                77B5B886-944A-11d1-AEBD-0000F80367C1.");

            //MS-AD_Schema_R884
            string multipleGuidPersonalInfoOnLds = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidPersonalInfoOnLds += o.ToString() + ",";
            }
            multipleGuidPersonalInfoOnLds = multipleGuidPersonalInfoOnLds.Substring(0, multipleGuidPersonalInfoOnLds.Length - 1);
            if (serverOS < OSVersion.WinSvr2008R2)
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a86-0de6-11d0-a285-00aa003049e2,"
                    + "5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidPersonalInfoOnLds,
                    884,
                    @"For the Personal-Information Extended Right the appliesTo attribute must be 
                    4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a86-0de6-11d0-a285-00aa003049e2,
                    5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2.");
            }
            // This is a server2k8 r2 Doc change
            else
            {
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a86-0de6-11d0-a285-00aa003049e2,"
                    + "5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2",
                    multipleGuidPersonalInfoOnLds,
                    11884,
                    @"For the Personal-Information Extended Right the appliesTo attribute must be 
                    4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a86-0de6-11d0-a285-00aa003049e2,
                    5cb41ed0-0e4c-11d0-a286-00aa003049e2,bf967aba-0de6-11d0-a285-00aa003049e2,
                    ce206244-5827-4a86-ba1c-1c0c386c1b64[for Windows server 2008 R2].");
            }

            #endregion

            #region Email-Information Extended Right

            childEntry = requiredEntry.Children.Find("CN=Email-Information");
            //MS-AD_Schema_R885
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "E45795B2-9455-11d1-AEBD-0000F80367C1",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                885,
                @"For the Email-Information Extended Right the rightsguid attribute must be a 
                E45795B2-9455-11d1-AEBD-0000F80367C1.");

            //MS-AD_Schema_R886
            string multipleGuidEmailInfoOnLds = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidEmailInfoOnLds += o.ToString() + ",";
            }
            multipleGuidEmailInfoOnLds = multipleGuidEmailInfoOnLds.Substring(0, multipleGuidEmailInfoOnLds.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,bf967a9c-0de6-11d0-a285-00aa003049e2,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidEmailInfoOnLds,
                886,
                @"For the Email-Information Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28, bf967a9c-0de6-11d0-a285-00aa003049e2, 
                bf967aba-0de6-11d0-a285-00aa003049e2.");

            #endregion

            #region Web-Information Extended Right

            childEntry = requiredEntry.Children.Find("CN=Web-Information");
            //MS-AD_Schema_R887
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "E45795B3-9455-11d1-AEBD-0000F80367C1",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                887,
                @"For the Web-Information Extended Right the rightsguid attribute must be a 
                E45795B3-9455-11d1-AEBD-0000F80367C1.");

            //MS-AD_Schema_R888
            string multipleGuidWebInfoOnLds = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidWebInfoOnLds += o.ToString() + ",";
            }
            multipleGuidWebInfoOnLds = multipleGuidWebInfoOnLds.Substring(0, multipleGuidWebInfoOnLds.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "4828CC14-1437-45bc-9B07-AD6F015E5F28,5cb41ed0-0e4c-11d0-a286-00aa003049e2,"
                + "bf967aba-0de6-11d0-a285-00aa003049e2",
                multipleGuidWebInfoOnLds,
                888,
                @"For the Web-Information Extended Right the appliesTo attribute must be 
                4828CC14-1437-45bc-9B07-AD6F015E5F28, 5cb41ed0-0e4c-11d0-a286-00aa003049e2, 
                bf967aba-0de6-11d0-a285-00aa003049e2.");

            #endregion

            #region DS-Replication-Get-Changes Extended Right

            childEntry = requiredEntry.Children.Find("CN=DS-Replication-Get-Changes");

            //MS-AD_Schema_R889
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "1131f6aa-9c07-11d1-f79f-00c04fc2dcd2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                889,
                @"For the DS-Replication-Get-Changes Extended Right the rightsguid attribute must be a 
                1131f6aa-9c07-11d1-f79f-00c04fc2dcd2.");

            //MS-AD_Schema_R890
            string multipleGuidGetchangesOnLds = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidGetchangesOnLds += o.ToString() + ",";
            }
            multipleGuidGetchangesOnLds = multipleGuidGetchangesOnLds.Substring(0, multipleGuidGetchangesOnLds.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,bf967a87-0de6-11d0-a285-00aa003049e2,"
                + "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidGetchangesOnLds,
                890,
                @"For the DS-Replication-Get-Changes Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            #endregion

            #region DS-Replication-Synchronize Extended Right

            childEntry = requiredEntry.Children.Find("CN=DS-Replication-Synchronize");
            //MS-AD_Schema_R891
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "1131f6ab-9c07-11d1-f79f-00c04fc2dcd2",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                891,
                @"For the DS-Replication-Synchronize Extended Right the rightsguid attribute must be a 
                1131f6ab-9c07-11d1-f79f-00c04fc2dcd2.");

            //MS-AD_Schema_R892
            string multipleGuidSynchronizeOnLds = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidSynchronizeOnLds += o.ToString() + ",";
            }
            multipleGuidSynchronizeOnLds = multipleGuidSynchronizeOnLds.Substring(0, multipleGuidSynchronizeOnLds.Length - 1);
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,bf967a87-0de6-11d0-a285-00aa003049e2,"
                + "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidSynchronizeOnLds,
                892,
                @"For the DS-Replication-Synchronize Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            #endregion

            #region DS-Replication-Manage-Topology Extended Right

            childEntry = requiredEntry.Children.Find("CN=DS-Replication-Manage-Topology");
            //MS-AD_Schema_R893
            DataSchemaSite.CaptureRequirementIfAreEqual<string>("1131f6ac-9c07-11d1-f79f-00c04fc2dcd2",
                childEntry.Properties["rightsGuid"].Value.ToString(), 893,
                @"For the DS-Replication-Manage-Topology Extended Right the rightsguid attribute must be a 
                1131f6ac-9c07-11d1-f79f-00c04fc2dcd2.");

            //MS-AD_Schema_R894
            string multipleGuidManageTopologyOnLds = String.Empty;
            foreach (object o in (IEnumerable)childEntry.Properties["appliesTo"].Value)
            {
                multipleGuidManageTopologyOnLds += o.ToString() + ",";
            }
            multipleGuidManageTopologyOnLds = multipleGuidManageTopologyOnLds.Substring(0, multipleGuidManageTopologyOnLds.Length - 1);

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2,bf967a87-0de6-11d0-a285-00aa003049e2,"
                + "19195a5b-6da0-11d0-afd3-00c04fd930c9",
                multipleGuidManageTopologyOnLds,
                894,
                @"For the DS-Replication-Manage-Topology Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2, bf967a87-0de6-11d0-a285-00aa003049e2, 
                19195a5b-6da0-11d0-afd3-00c04fd930c9.");

            #endregion

            #region Change-Schema-Master Extended Right

            childEntry = requiredEntry.Children.Find("CN=Change-Schema-Master");
            //MS-AD_Schema_R895
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "e12b56b6-0a95-11d1-adbb-00c04fd8d5cd",
                childEntry.Properties["rightsGuid"].Value.ToString(),
                895,
                @"For the Change-Schema-Master Extended Right the rightsguid attribute must be a 
                e12b56b6-0a95-11d1-adbb-00c04fd8d5cd.");

            //MS-AD_Schema_R896
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "bf967a8f-0de6-11d0-a285-00aa003049e2",
                childEntry.Properties["appliesTo"].Value.ToString(),
                896,
                @"For the Change-Schema-Master Extended Right the appliesTo attribute must be 
                bf967a8f-0de6-11d0-a285-00aa003049e2.");

            #endregion
        }

        #endregion
    }
}