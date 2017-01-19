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
    /// This file is the source file for Validation of the TestCase22 and TestCase23.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region OptionalFeature Validation.
        /// <summary>
        /// Method validates the requirements under OptionalFeature Scenario.
        /// </summary>
        public void ValidateOptionalFeature()
        {
            #region Variables required for Directory Entries.
            DirectoryEntry dirPartitions = new DirectoryEntry();
            DirectoryEntry domainEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();

            string configNC = "CN=Configuration," + adAdapter.rootDomainDN;
            string SchemaNC = "CN=Schema," + configNC;
            string recycleBin = "CN=Recycle Bin Feature,CN=Optional Features,CN=Directory Service,CN=Windows NT,CN=Services," + configNC;

            #endregion

            #region Verify Recycle Bin Feature enabled

            //Get recycle bin feature entry
            if (!adAdapter.GetObjectByDN(recycleBin, out dirPartitions))
            {
                DataSchemaSite.Assume.Fail(
                    "CN=Recycle Bin Feature,CN=Optional Features,CN=Directory Service,CN=Windows NT,CN=Services," 
                    + configNC 
                    + " Object is not found in server");
            }
            bool isForestbitflag = false;
            bool isRecyclebinfeature = false;
            bool isWIN2008R2 = false;
            //According to TD 2.2.16, forest feature value is 0x1
            const int FOREST_OPTIONAL_FEATURE = 0x00000001;
            //According to TD 7.1.4.2, windows 2008 R2 version is 4
            const int Win2K8R2VerionNumber = 4;

            if (FOREST_OPTIONAL_FEATURE == (int)(dirPartitions.Properties["msDS-OptionalFeatureFlags"].Value))
            {
                isForestbitflag = true;
            }
            if (dirPartitions.Name == "CN=Recycle Bin Feature")
            {
                isRecyclebinfeature = true;
            }
            if (Win2K8R2VerionNumber <= (int)(dirPartitions.Properties["msDS-RequiredForestBehaviorVersion"].Value))
            {
                isWIN2008R2 = true;
            }
            List<string> forestScope = new List<string>();
            List<string> serverScope = new List<string>();
            List<string> domainScope = new List<string>();
            foreach (string scope in dirPartitions.Properties["msDS-EnabledFeatureBL"])
            {
                //Check if this is server-scope
                if (scope.Contains("CN=Servers"))
                {
                    serverScope.Add(scope);
                }
                //Check if this is forest scope
                else if (scope.Contains("CN=Partitions"))
                {
                    forestScope.Add(scope);
                }
                else
                {
                    domainScope.Add(scope);
                }
            }
            bool allEnabledRecycleBin = true;
            foreach (string server in serverScope)
            {
                DirectoryEntry serverObj;
                adAdapter.GetObjectByDN(server, out serverObj);
                //Check if all server objects included in recycle bin feature has this feature in 
                //msDS-EnableFeature attribute
                if (!serverObj.Properties["msDS-EnabledFeature"].Contains(recycleBin))
                {
                    allEnabledRecycleBin = false;
                }
            }
            foreach (string forest in forestScope)
            {
                DirectoryEntry forestObj;
                adAdapter.GetObjectByDN(forest, out forestObj);
                //Check if all forest objects included in recycle bin feature has this feature in 
                //msDS-EnableFeature attribute
                if (!forestObj.Properties["msDS-EnabledFeature"].Contains(recycleBin))
                {
                    allEnabledRecycleBin = false;
                }
            }
            foreach (string domain in domainScope)
            {
                DirectoryEntry domainObj;
                adAdapter.GetObjectByDN(domain, out domainObj);
                //Check if all domain objects included in recycle bin feature has this feature in 
                //msDS-EnableFeature attribute
                if (!domainObj.Properties["msDS-EnabledFeature"].Contains(recycleBin))
                {
                    allEnabledRecycleBin = false;
                    break;
                }
            }
            
            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4486
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirPartitions.Properties["msDS-RequiredForestBehaviorVersion"] != null,
                4486,
                @"[Optional Features]If an optional feature requires a specific forest functional level before it can 
                be enabled, the forest functional level required is stored in the msDS-RequiredForestBehaviorVersion 
                attribute of the object representing the optional feature.");
           
            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4477
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isForestbitflag,
                4477,
                @"[Optional Features]If an optional feature is permissible for a forest-wide scope, the attribute 
                contains the bit flag FOREST_OPTIONAL_FEATURE.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4468
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4468,
                @"[Optional Features]The list of optional features enabled for a scope is stored in the 
                msDS-EnabledFeature attribute on the object representing the scope.");
        
            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4470
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4470,
                @"[Optional Features]The list of scopes in which an optional feature is enabled is stored in the 
                msDS-EnabledFeatureBL attribute on the object representing the optional feature.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4471
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4471,
                @"[Optional Features]The values stored[stored in the msDS-EnabledFeatureBL attribute] are references to 
                the objects representing the scopes where the feature is enabled.");
        
            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4472
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4472,
                @"[Optional Features]If an optional feature is enabled in some scope, then, depending on the feature, 
                it might be automatically enabled in another scope; for example, the Recycle Bin optional feature 
                (section 3.1.1.8.1).");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4475
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4475,
                @"[Optional Features]The following procedure determines whether an optional feature is enabled in a 
                scope by using the msDS-EnabledFeature attribute: procedure IsOptionalFeatureEnabled ( scope: DSNAME, 
                featureGuid: GUID): boolean Returns true if scope!msDS-EnabledFeature contains the DN of a 
                msDS-optionalFeature object o such that o!msDS-optionalFeatureGuid equals featureGuid. 
                Returns false otherwise.");
            
            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4464
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "msDS-OptionalFeature",
                dirPartitions.SchemaClassName,
                4464,
                @"[Optional Features]Optional features are represented by instances of the object class 
                msDS-OptionalFeature.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4496
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isWIN2008R2 && dirPartitions != null,
                4496,
                "[Recycle Bin Optional Feature]The Recycle Bin optional feature requires a Forest Functional "
            + "Level of DS_BEHAVIOR_WIN2008R2 or greater.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4488
            //The Recycle Bin Optional feature itsself is a new property for the Windows Server 2008R2 DS and LDS
            //And before calling the method,it has been determined the version of platform for Windows Server 2008R2.
            DataSchemaSite.CaptureRequirement(
                "MS-ADTS-Schema",
                4488,
                @"[Optional Features]Recycle Bin Optional feature is available in Windows Server 2008 R2 AD DS and 
                Windows Server 2008 R2 AD LDS.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4489
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isRecyclebinfeature,
                4489,
                @"[Recycle Bin Optional Feature]The Recycle Bin optional feature is represented by the Recycle Bin 
                Feature Object (see section 7.1.1.2.4.1.3.1).");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4500
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isWIN2008R2,
                4500,
                "[Recycle Bin Optional Feature]Any DC with a behavior version of DS_BEHAVIOR_WIN2008R2 or greater "
            + "MUST be capable of supporting the Recycle Bin optional feature.");

            DirectoryEntry parent = dirPartitions.Parent;

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4465
            DataSchemaSite.CaptureRequirementIfIsTrue(
                parent.Name == "CN=Optional Features" 
                && parent.SchemaClassName == "container" 
                && parent.Parent.Name == "CN=Directory Service",
                4465,
                @"[Optional Features]Objects representing optional features are stored in the Optional Features 
                container in the Config NC (see section 7.1.1.2.4.1.3).");

            bool isR4474AndR14473Satisfied = false;
            if (dirPartitions.Properties.Contains("msDS-OptionalFeatureGUID"))
            {
                try
                {
                    //Get the optional feature Guid
                    Guid featureGuid = new Guid((byte[])dirPartitions.Properties["msDS-OptionalFeatureGUID"][0]);
                    Guid testGuids = new Guid("766ddcd8-acd0-445e-f3b9-a7f9b6744f2a");
                    if (featureGuid != null && featureGuid != Guid.Empty)
                    {
                        isR4474AndR14473Satisfied = true;
                    }

                    //Verify MS-AD_Schema requirement:MS-AD_Schema_R4495
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        featureGuid == testGuids,
                        4495,
                        @"[Recycle Bin Optional Feature]The Recycle Bin optional feature is identified by the feature
                        GUID {766ddcd8-acd0-445e-f3b9-a7f9b6744f2a}.");

                    //Verify MS-AD_Schema requirement:MS-AD_Schema_R14473
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                    isR4474AndR14473Satisfied,
                    14473,
                    @"[Optional Features]Recycle Bin Optional Feature is uniquely identified by a GUID.");

                    //Verify MS-AD_Schema requirement:MS-AD_Schema_R4474
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR4474AndR14473Satisfied,
                        4474,
                        @"[Optional Features] The GUID is stored in the msDS-OptionalFeatureGUID attribute of the object 
                        representing the optional feature.");
                }
                catch (System.InvalidCastException e)
                {
                    DataSchemaSite.Assert.Fail(e.ToString());
                }
            }

            #endregion

            #region Verify Deleted Objects

            //Deleted objects are stored in Deleted Objects NC
            DirectoryEntry deletedObjects = new DirectoryEntry("LDAP://CN=Deleted Objects," + adAdapter.rootDomainDN);
            deletedObjects.AuthenticationType = AuthenticationTypes.FastBind;
            DirectorySearcher searcher = new DirectorySearcher(deletedObjects);
            searcher.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            searcher.Tombstone = true;
            //Get all deleted objects
            try
            {
                SearchResultCollection results = searcher.FindAll();
                bool isDeleted = false;
                int RecycledCount;
                foreach (System.DirectoryServices.SearchResult res in results)
                {
                    if (res.Path.Contains("CN=" + adAdapter.DeletedUserName))
                    {
                        if (res.Properties["isDeleted"] != null)
                        {
                            isDeleted = (bool)res.Properties["isDeleted"][0];
                        }
                        RecycledCount = res.Properties["isRecycled"].Count;
                    }
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                DataSchemaSite.Assert.Fail("Can't access deleted objects: " + e.Message);
            }
            DomainController dc = DomainController.FindOne(new DirectoryContext(DirectoryContextType.Domain));
            Guid invocationID = dc.GetReplicationMetadata("CN=Deleted Objects," 
                + adAdapter.rootDomainDN)["isDeleted"].LastOriginatingInvocationId;
            DateTime deletedTime = dc.GetReplicationMetadata("CN=Deleted Objects," 
                + adAdapter.rootDomainDN)["isDeleted"].LastOriginatingChangeTime;

            #endregion

            #region Verify Delete Settings

            int tombstoneLifetime = 0;
            int deletedObjectLifetime = 0;
            if (adAdapter.GetObjectByDN("CN=Directory Service,CN=Windows NT,CN=Services," + configNC, out dirPartitions))
            {
                if (dirPartitions.Properties["deletedObjectLifetime"].Value != null)
                {
                    deletedObjectLifetime = (int)dirPartitions.Properties["deletedObjectLifetime"][0];
                }
                if (dirPartitions.Properties["tombstoneLifetime"].Value != null)
                {
                    tombstoneLifetime = (int)dirPartitions.Properties["tombstoneLifetime"][0];
                }
            }
            else
            {
                DataSchemaSite.Assert.Fail("Can't access Directory Service NC");
            }

            #endregion
        }

        #endregion

        #region LDS OptionalFeature Validation.

        /// <summary>
        /// Method validates the requirements under LDS OptionalFeature Scenario.
        /// </summary>
        public void ValidateLDSOptionalFeature()
        {
            #region Variables required for Directory Entries.
            DirectoryEntry dirPartitions = new DirectoryEntry();
            DirectoryEntry domainEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();

            string configNC = "CN=Configuration," + adAdapter.LDSRootObjectName;
            string SchemaNC = "CN=Schema," + configNC;
            string recycleBin = "CN=Recycle Bin Feature,CN=Optional Features,CN=Directory Service,CN=Windows NT,CN=Services," + configNC;

            #endregion

            #region Verify Recycle Bin Feature enabled

            //Get recycle bin feature entry
            if (!adAdapter.GetLdsObjectByDN(recycleBin, out dirPartitions))
            {
                DataSchemaSite.Assume.Fail(
                    "CN=Recycle Bin Feature,CN=Optional Features,CN=Directory Service,CN=Windows NT,CN=Services,,"
                    + configNC 
                    + " Object is not found in server");
            }
          
            bool isForestbitflag = false;
           
            bool isRecyclebinfeature = false;
            bool isWIN2008R2 = false;
            //According to TD 2.2.16, forest feature value is 0x1
            const int FOREST_OPTIONAL_FEATURE = 0x00000001;
            //According to TD 7.1.4.2, windows 2008 R2 version is 4
            const int Win2K8R2VerionNumber = 4;

            if (FOREST_OPTIONAL_FEATURE == (int)(dirPartitions.Properties["msDS-OptionalFeatureFlags"].Value))
            {
                isForestbitflag = true;
            }
            if (dirPartitions.Name == "CN=Recycle Bin Feature")
            {
                isRecyclebinfeature = true;
            }
            if (Win2K8R2VerionNumber == (int)(dirPartitions.Properties["msDS-RequiredForestBehaviorVersion"].Value))
            {
                isWIN2008R2 = true;
            }
            List<string> forestScope = new List<string>();
            List<string> serverScope = new List<string>();
            foreach (string scope in dirPartitions.Properties["msDS-EnabledFeatureBL"])
            {
                //Check if this is server-scope
                if (scope.Contains("CN=Servers"))
                {
                    serverScope.Add(scope);
                }
                //Check if this is forest scope
                else if (scope.Contains("CN=Partitions"))
                {
                    forestScope.Add(scope);
                }
            }
            bool allEnabledRecycleBin = true;
            foreach (string server in serverScope)
            {
                DirectoryEntry serverObj;
                adAdapter.GetLdsObjectByDN(server, out serverObj);
                //Check if all server objects included in recycle bin feature has this feature in 
                //msDS-EnableFeature attribute
                if (!serverObj.Properties["msDS-EnabledFeature"].Contains(recycleBin))
                {
                    allEnabledRecycleBin = false;
                }
                //Check for nTDSDSA object
                if (serverObj.SchemaClassName != "nTDSDSA")
                {
                    allEnabledRecycleBin = false;
                }
            }
            foreach (string forest in forestScope)
            {
                DirectoryEntry forestObj;
                adAdapter.GetLdsObjectByDN(forest, out forestObj);
                //Check if all forest objects included in recycle bin feature has this feature in 
                //msDS-EnableFeature attribute
                if (!forestObj.Properties["msDS-EnabledFeature"].Contains(recycleBin))
                {
                    allEnabledRecycleBin = false;
                }
            }

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4486
            DataSchemaSite.CaptureRequirementIfIsTrue(
                dirPartitions.Properties["msDS-RequiredForestBehaviorVersion"] != null,
                4486,
                @"[Optional Features]If an optional feature requires a specific forest functional level before it can 
                be enabled, the forest functional level required is stored in the msDS-RequiredForestBehaviorVersion 
                attribute of the object representing the optional feature.");
       
            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4477
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isForestbitflag,
                4477,
                @"[Optional Features]If an optional feature is permissible for a forest-wide scope, the attribute 
                contains the bit flag FOREST_OPTIONAL_FEATURE.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4468
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4468,
                @"[Optional Features]The list of optional features enabled for a scope is stored in the 
                msDS-EnabledFeature attribute on the object representing the scope.");
     
            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4470
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4470,
                @"[Optional Features]The list of scopes in which an optional feature is enabled is stored in the 
                msDS-EnabledFeatureBL attribute on the object representing the optional feature.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4471
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4471,
                @"[Optional Features]The values stored[stored in the msDS-EnabledFeatureBL attribute] are references to 
                the objects representing the scopes where the feature is enabled.");
        
            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4472
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4472,
                @"[Optional Features]If an optional feature is enabled in some scope, then, depending on the feature, 
                it might be automatically enabled in another scope; for example, the Recycle Bin optional feature 
                (section 3.1.1.8.1).");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4475
            DataSchemaSite.CaptureRequirementIfIsTrue(
                allEnabledRecycleBin,
                4475,
                @"[Optional Features]The following procedure determines whether an optional feature is enabled in a 
                scope by using the msDS-EnabledFeature attribute: procedure IsOptionalFeatureEnabled ( scope: DSNAME, 
                featureGuid: GUID): boolean Returns true if scope!msDS-EnabledFeature contains the DN of a 
                msDS-optionalFeature object o such that o!msDS-optionalFeatureGuid equals featureGuid. 
                Returns false otherwise.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4464
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "msDS-OptionalFeature",
                dirPartitions.SchemaClassName,
                4464,
                @"[Optional Features]Optional features are represented by instances of the object class 
                msDS-OptionalFeature.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4496
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isWIN2008R2 && dirPartitions != null,
                4496,
                @"[Recycle Bin Optional Feature]The Recycle Bin optional feature requires a Forest Functional "
            + "Level of DS_BEHAVIOR_WIN2008R2 or greater.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4488
            //The Recycle Bin Optional feature itsself is a new property for the Windows Server 2008R2 DS and LDS
            //And before calling the method,it has been determined the version of platform for Windows Server 2008R2.
            DataSchemaSite.CaptureRequirement(
                "MS-ADTS-Schema",
                4488,
                @"[Optional Features]Recycle Bin Optional feature is available in Windows Server 2008 R2 AD DS and 
                Windows Server 2008 R2 AD LDS.");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4489
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isRecyclebinfeature,
                4489,
                @"[Recycle Bin Optional Feature]The Recycle Bin optional feature is represented by the Recycle Bin 
                Feature Object (see section 7.1.1.2.4.1.3.1).");

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4500
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isWIN2008R2,
                4500,
                "[Recycle Bin Optional Feature]Any DC with a behavior version of DS_BEHAVIOR_WIN2008R2 or greater "
            + "MUST be capable of supporting the Recycle Bin optional feature.");
            DirectoryEntry parent = dirPartitions.Parent;

            //Verify MS-AD_Schema requirement:MS-AD_Schema_R4465
            DataSchemaSite.CaptureRequirementIfIsTrue(
                parent.Name == "CN=Optional Features" 
                && parent.SchemaClassName == "container" 
                && parent.Parent.Name == "CN=Directory Service",
                4465,
                @"[Optional Features]Objects representing optional features are stored in the Optional Features 
                container in the Config NC (see section 7.1.1.2.4.1.3).");

            bool isR4474AndR14473Satisfied = false;
            if (dirPartitions.Properties.Contains("msDS-OptionalFeatureGUID"))
            {
                try
                {
                    //Get the optional feature Guid
                    Guid featureGUID = new Guid((byte[])dirPartitions.Properties["msDS-OptionalFeatureGUID"][0]);
                    Guid expectedGUID = new Guid("766ddcd8-acd0-445e-f3b9-a7f9b6744f2a");

                    if (featureGUID != null && featureGUID != Guid.Empty)
                    {
                        isR4474AndR14473Satisfied = true;
                    }

                    //Verify MS-AD_Schema requirement:MS-AD_Schema_R4495
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        featureGUID == expectedGUID,
                        4495,
                        @"[Recycle Bin Optional Feature]The Recycle Bin optional feature is identified by the feature
                        GUID {766ddcd8-acd0-445e-f3b9-a7f9b6744f2a}.");

                    //Verify MS-AD_Schema requirement:MS-AD_Schema_R14473
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR4474AndR14473Satisfied,
                        14473,
                        @"[Optional Features]Recycle Bin Optional Feature is uniquely identified by a GUID.");

                    //Verify MS-AD_Schema requirement:MS-AD_Schema_R4474
                    DataSchemaSite.CaptureRequirementIfIsTrue(
                        isR4474AndR14473Satisfied,
                        4474,
                        @"[Optional Features] The GUID is stored in the msDS-OptionalFeatureGUID attribute of the object 
                        representing the optional feature.");
                }
                catch (System.InvalidCastException e)
                {
                    DataSchemaSite.Assert.Fail(e.ToString());
                }
            }

            #endregion

            #region Verify Deleted Objects

            //Deleted objects are stored in Deleted Objects NC
            DirectoryEntry deletedObjects = new DirectoryEntry(
                "LDAP://"
                + adAdapter.adamServerPort
                + "/CN=Deleted Objects," 
                + configNC,adAdapter.PrimaryDomainDnsName+@"\"+adAdapter.ClientUserName,adAdapter.ClientUserPassword);
            deletedObjects.AuthenticationType = AuthenticationTypes.Secure;
            DirectorySearcher searcher = new DirectorySearcher(deletedObjects);
            searcher.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
            searcher.Tombstone = true;
            //Get all deleted objects
            try
            {
                SearchResultCollection results = searcher.FindAll();
                bool isDeleted = false;
                int RecycledCount;
                foreach (System.DirectoryServices.SearchResult res in results)
                {
                    if (res.Path.Contains("CN=" + adAdapter.DeletedGroupName))
                    {
                        if (res.Properties["isDeleted"] != null)
                        {
                            isDeleted = (bool)res.Properties["isDeleted"][0];
                        }
                        RecycledCount = res.Properties["isRecycled"].Count;
                    }
                }
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                DataSchemaSite.Log.Add(LogEntryKind.Warning, "Can't access deleted objects: " + e.Message);
            }

            AdamInstance adam = AdamInstance.GetAdamInstance(
                new DirectoryContext(DirectoryContextType.DirectoryServer, 
                    adAdapter.adamServerPort, adAdapter.ClientUserName, adAdapter.ClientUserPassword));
            Guid invocationID = adam.GetReplicationMetadata(
                "CN=Deleted Objects,CN=Configuration," 
                + adAdapter.LDSRootObjectName)["isDeleted"].LastOriginatingInvocationId;
            DateTime deletedTime = adam.GetReplicationMetadata(
                "CN=Deleted Objects,CN=Configuration," 
                + adAdapter.LDSRootObjectName)["isDeleted"].LastOriginatingChangeTime;

            #endregion

            #region Verify Delete Settings

            int tombstoneLifetime = 0;
            int deletedObjectLifetime = 0;

            if (adAdapter.GetLdsObjectByDN(
                "CN=Directory Service,CN=Windows NT,CN=Services," 
                + configNC, 
                out dirPartitions))
            {
                if (dirPartitions.Properties["deletedObjectLifetime"].Value != null)
                {
                    deletedObjectLifetime = (int)dirPartitions.Properties["deletedObjectLifetime"][0];
                }
                if (dirPartitions.Properties["tombstoneLifetime"].Value != null)
                {
                    tombstoneLifetime = (int)dirPartitions.Properties["tombstoneLifetime"][0];
                }
            }
            else
            {
                DataSchemaSite.Assert.Fail("Can't access Directory Service NC");
            }

            #endregion
        }

        #endregion
    }
}