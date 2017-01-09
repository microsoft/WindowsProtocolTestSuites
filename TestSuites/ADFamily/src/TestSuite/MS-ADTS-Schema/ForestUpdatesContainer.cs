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
    /// This file is the source file for Validation of the TestCase33.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        /// <summary>
        /// A structure which is used to store forest revision
        /// </summary>
        public struct ForestRevision
        {
            //Forest minor revision
            public int minor;
            //Forest major revision
            public int major;
        }


        #region Validation of ForestUpdatesContainer.
        /// <summary>
        /// This method validates the requirements under 
        /// ForestUpdatesContainer Scenario.
        /// </summary>
        public void ValidateForestUpdatesContainer()
        {
            DirectoryEntry dirPartitions = new DirectoryEntry();

            string currDomain = adAdapter.rootDomainDN;
            string parent;
            PropertyValueCollection objectClass;

            if (!adAdapter.GetObjectByDN(
                "CN=Operations,CN=ForestUpdates,CN=Configuration,"
                + currDomain, 
                out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Operations,CN=ForestUpdates,CN=Configuration,"
                    + currDomain 
                    + " Object is not found in server");
            }
            parent = dirPartitions.Parent.Name;

            //MS-ADTS-Schema_R676
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=ForestUpdates", 
                parent, 
                676,
                @"The Parent of the Operations Container, 
                which is a type of Forest Updates Container must be equal to the Forest Updates container.");

            //MS-ADTS-Schema_R677
            objectClass = dirPartitions.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"container"), 
                677,
                @"The objectClass attribute of the Operations Container ,
                which is a type of Forest Updates Container must be container.");

            if (!adAdapter.GetObjectByDN(
                "CN=Windows2003Update,CN=ForestUpdates,CN=Configuration,"
                + currDomain, 
                out dirPartitions))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Windows2003Update,CN=ForestUpdates,CN=Configuration,"
                    + currDomain 
                    + " Object is not found in server");
            }
            parent = dirPartitions.Parent.Name;

            //MS-ADTS-Schema_R678
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "CN=ForestUpdates", 
                parent, 
                678,
                @"The Parent of the Windows2003Update Container ,
                which is a type of Forest Updates Container must be equal to the Forest Updates container.");

            //MS-ADTS-Schema_R679
            objectClass = dirPartitions.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"container"),
                679,
                @"The objectClass attribute of the Windows2003Update Container ,
                which is a type of Forest Updates Container must be container.");

            if (serverOS >= OSVersion.WinSvr2008)
            {
                if (!adAdapter.GetObjectByDN(
                    "CN=ActiveDirectoryUpdate,CN=ForestUpdates,CN=Configuration,"
                    + currDomain, 
                    out dirPartitions))
                {
                    DataSchemaSite.Assume.IsTrue(
                        false, 
                        "CN=ActiveDirectoryUpdate,CN=ForestUpdates,"
                        + "CN=Configuration," 
                        + currDomain 
                        + " Object is not found in server");
                }
                parent = dirPartitions.Parent.Name;

                //MS-ADTS-Schema_R681
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "CN=ForestUpdates", 
                    parent, 
                    681,
                    @"The Parent of the ActiveDirectoryUpdate Container ,
                    which is a type of Forest Updates Container must be equal to the Forest Updates container.");

                //MS-ADTS-Schema_R682
                objectClass = dirPartitions.Properties["objectClass"];
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    objectClass.Contains((object)"container"), 
                    682,
                    @"The objectClass attribute of the ActiveDirectoryUpdate Container ,
                    which is a type of Forest Updates Container must be container.");

                if (!adAdapter.GetObjectByDN(
                    "CN=ActiveDirectoryRodcUpdate,CN=ForestUpdates,CN=Configuration,"
                    + currDomain, 
                    out dirPartitions))
                {
                    DataSchemaSite.Assume.IsTrue(
                        false, 
                        "CN=ActiveDirectoryRodcUpdate,CN=ForestUpdates,"
                        + "CN=Configuration," 
                        + currDomain 
                        + " Object is not found in server");
                }
                parent = dirPartitions.Parent.Name;

                //MS-ADTS-Schema_R684
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    "CN=ForestUpdates", 
                    parent, 
                    684,
                    @"The Parent of the ActiveDirectoryRodcUpdate Container ,
                    which is a type of Forest Updates Container must be equal to the Forest Updates container.");

                //MS-ADTS-Schema_R685
                objectClass = dirPartitions.Properties["objectClass"];
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    objectClass.Contains((object)"container"), 
                    685,
                    @"The objectClass attribute of the ActiveDirectoryRodcUpdate Container ,
                    which is a type of Forest Updates Container must be container.");
            }

            //Check revisions
            string updatesNC = "CN=ForestUpdates,CN=Configuration," + currDomain;
            int rodcRevision = 0;
            int operationRevision = 0;

            bool isStoredOnActive = false;
            bool isStoredOnWin2003Up = false;
            bool isStoredOnActiveRodc = false;
            ForestRevision forest = new ForestRevision();
            //Get RODC revision
            adAdapter.GetObjectByDN("CN=ActiveDirectoryRodcUpdate," + updatesNC, out dirPartitions);
            if (dirPartitions != null)
            {
                if (dirPartitions.Properties["Revision"].Value != null)
                {
                     //Verify MS-AD_Schema requirement:MS-AD_Schema_R264500
                     DataSchemaSite.CaptureRequirementIfIsTrue(
                        dirPartitions.Properties["Revision"].Value.GetType() == typeof(int),
                        264500,
                        @"[In RODC Revision] The version of the RODC revision is an integer.");

                     rodcRevision = (int)dirPartitions.Properties["Revision"].Value;
                     isStoredOnActiveRodc = true;
                }  
                else
                    {
                         DataSchemaSite.Assert.Fail("Failed to get RODC Update container");
                    }

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R224533
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isStoredOnActiveRodc,
                    224533,
                    @"[In Forest Updates Container] The version of the RODC revision is stored on the revision attribute
                    of the ActiveDirectoryRodcUpdate container.");
                                
                if (2==rodcRevision) 
                {   
                    parent = dirPartitions.Parent.Name;

                   //Verify MS-AD_Schema requirement:MS-AD_Schema_R144533
                    DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                        "CN=ForestUpdates",
                        parent,
                        144533,
                        @"[In Forest Updates Container] If the version of the RODC revision is 2, the Forest Updates 
                        container includes the child container ActiveDirectoryRodcUpdate.");
                }
               
            }
            //Get forest major revision
            adAdapter.GetObjectByDN("CN=ActiveDirectoryUpdate," + updatesNC, out dirPartitions);
            if (dirPartitions != null)
            {
                if (dirPartitions.Properties["Revision"].Value != null)
                {
                    forest.major = (int)dirPartitions.Properties["Revision"].Value;
                    isStoredOnActive = true; 
                }

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R164533
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isStoredOnActive,
                    164533,
                    @"[In Forest Updates Container] The major version of the forest revision is stored on the revision 
                    attribute of the ActiveDirectoryUpdate container.");
                
            }
            //Get Operations revision
            adAdapter.GetObjectByDN("CN=Operations," + updatesNC, out dirPartitions);
            if (dirPartitions != null)
            {
                if (dirPartitions.Properties["Revision"].Value != null)
                {
                    operationRevision = (int)dirPartitions.Properties["Revision"].Value;
                }
            }
            //Get forest minor revision
            adAdapter.GetObjectByDN("CN=Windows2003Update," + updatesNC, out dirPartitions);
            if (dirPartitions != null)
            {
                if (dirPartitions.Properties["Revision"].Value != null)
                {
                    forest.minor = (int)dirPartitions.Properties["Revision"].Value;
                    isStoredOnWin2003Up = true;
                }

                //Verify MS-AD_Schema requirement:MS-AD_Schema_R194533
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isStoredOnWin2003Up,
                    194533,
                    @"[In Forest Updates Container] The minor version of the forest revision is stored on the revision 
                    attribute of the Windows2003Update container.");
            }

            ForestRevision DsBehavior = new ForestRevision();
            bool isForestForCorrectDsBehavior = false;

            // The forest revision for WinSvr2008 is 2.10
            if (serverOS == OSVersion.WinSvr2008)
            {
                DsBehavior.major = 2;
                DsBehavior.minor = 10;
            }
            // The forest revision for WinSvr2008R2 is 5.9
            else if (serverOS == OSVersion.WinSvr2008R2)
            {
                DsBehavior.major = 5;
                DsBehavior.minor = 9;
            }
            // The forest revision for Win2012 is 10.9
            else if (serverOS == OSVersion.WinSvr2012)
            {
                DsBehavior.major = 10;
                DsBehavior.minor = 9;
            }
            // The forest revision for Win2012R2 is 12.10
            else if (serverOS == OSVersion.WinSvr2012R2)
            {
                DsBehavior.major = 12;
                DsBehavior.minor = 10;
            }

            // Check forest revision
            if ((forest.major > DsBehavior.major) || (forest.major == DsBehavior.major && forest.minor >= DsBehavior.minor))
            {
                isForestForCorrectDsBehavior = true;
            }

            // Verify MS-AD_Schema requirement:MS-AD_Schema_R194500
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isForestForCorrectDsBehavior,
                194500,
                @"[In Forest Revision] The forest revision is below the minimal requirement of the current DC functional level.");

            
        }

        #endregion
    }
}