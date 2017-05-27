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
    /// This file is the source file for Validation of the TestCase28 and TestCase29.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region ServiceAndQueryPolicyContainer Implementation.

        // <summary>
        /// This method validates the requirements under 
        /// ServiceAndQueryPolicyContainer Scenario.
        /// </summary>
        public void ValidateServiceAndQueryPolicyContainer()
        {
            // For AD/DS ;
            //This for holding DirectroyEntry.
            DirectoryEntry requiredEntry = new DirectoryEntry();
            string rootDomain = adAdapter.rootDomainDN;
            string schemaNC = "CN=Schema," + rootDomain;
            string ConfigNC = "CN=Configuration," + rootDomain;
            string parentAttribute;
            PropertyValueCollection objectClass;

            //The following requirements are present in AD/DS only.
            //MS-ADTS-Schema_R480           
            if(!adAdapter.GetObjectByDN("CN=DisplaySpecifiers," + ConfigNC, out requiredEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=DisplaySpecifiers," 
                    + ConfigNC
                    + " Object is not found in server");
            }
            parentAttribute = requiredEntry.Parent.Name.ToString();
            DataSchemaSite.CaptureRequirementIfIsTrue(
                parentAttribute.Equals("CN=Configuration"), 
                480,
                "The parent of the Display Specifiers Container must be Config NC root.");

            //MS-ADTS-Schema_R481
            objectClass = requiredEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"container"), 
                481,
                "The ObjectClass attribute of the Display Specifier Container must be container.");
            
            //MS-ADTS-Schema_R497
            if(!adAdapter.GetObjectByDN("CN=Physical Locations," + ConfigNC, out requiredEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Physical Locations," 
                    + ConfigNC
                    + " Object is not found in server");
            }
            parentAttribute = requiredEntry.Parent.Name.ToString();
            DataSchemaSite.CaptureRequirementIfIsTrue(
                parentAttribute.Equals("CN=Configuration"), 
                497, 
                "The Parent of the Physical Locations must be config NC root object.");

            //MS-ADTS-Schema_R498
            objectClass = requiredEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"physicalLocation"), 
                498, 
                "The objectClass attribute of the Physical Locations must be physicalLocation.");

            if(!adAdapter.GetObjectByDN("CN=Services," + ConfigNC, out requiredEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Services," 
                    + ConfigNC
                    + " Object is not found in server");
            }

            //This method is for DS/LDS common requirement call for Services and QueryPolicy Container.
            LDSAndDSCommonCallForServices(requiredEntry);  
        }

        #endregion

        #region LDSServiceAndQueryPolicyContainer Implementation.

        /// <summary>
        /// This method validates the requirements under 
        /// LDSServiceAndQueryPolicyContainer Scenario.
        /// </summary>
        public void ValidateLDSServiceAndQueryPolicyContainer()
        {
            DirectoryEntry requiredEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();
            string configNCForLDS = "CN=Configuration," + adAdapter.LDSRootObjectName,parentAttribute;
            PropertyValueCollection objectClass;
            if(!adAdapter.GetLdsObjectByDN("CN=Services," + configNCForLDS, out requiredEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Services," 
                    + configNCForLDS
                    + " Object is not found in server");
            }

            //This method is for LDS/DS common requirement call for Services and QueryPolicy Container.
            LDSAndDSCommonCallForServices(requiredEntry);

            //This only present in AD/LDS.
            //MS-ADTS-Schema_R494
            if(!adAdapter.GetLdsObjectByDN("CN=Directory Service,CN=Windows NT,CN=Services," 
                + configNCForLDS, out requiredEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false, 
                    "CN=Directory Service,CN=Windows NT,CN=Services," 
                    + configNCForLDS 
                    + " Object is not found in server");
            }
            childEntry = requiredEntry.Children.Find("CN=SCP Publication Service");
            
            DataSchemaSite.CaptureRequirementIfIsNotNull(
                childEntry,
                494, 
                "SCP Publication Service Object is present only in AD/LDS.");

            //MS-ADTS-Schema_R495
            parentAttribute = childEntry.Parent.Name.ToString();
            DataSchemaSite.CaptureRequirementIfIsTrue(parentAttribute.Equals(
                "CN=Directory Service"), 
                495, 
                @"The Parent of the SCP Publication Service Object  which is a type of Windows NT Service must be 
                Directory Service.");

            //MS-ADTS-Schema_R496
            objectClass = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"msDS-ServiceConnectionPointPublicationService"), 
                496,
                @"The ObjectClass attribute of the SCP Publication Service Object which is a type of Windows NT Service 
                must be msDS-ServiceConnectionPointPublicationService.");
        }

        /// <summary>
        /// This method validates the requirements under 
        /// ServiceAndQueryPolicyContainer for both AD/DS and LDS Scenario's
        /// </summary>
        public void LDSAndDSCommonCallForServices(DirectoryEntry reqEntry)
        {

            PropertyValueCollection objectClass;
            DirectoryEntry requiredEntry = new DirectoryEntry();
            DirectoryEntry childEntry = new DirectoryEntry();
            requiredEntry = reqEntry;
            //MS-ADTS-Schema_R482            
            string parentAttribute = requiredEntry.Parent.Name.ToString();
            DataSchemaSite.CaptureRequirementIfIsTrue(
                parentAttribute.Equals("CN=Configuration"), 
                482,
                "The parent of the Services Container must be Config NC root object.");

            //MS-ADTS-Schema_R483
            objectClass = requiredEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains((object)"container"), 
                483,
                "The ObjectClass attribute of the Services Container must be container.");

            //MS-ADTS-Schema_R484            
            childEntry = requiredEntry.Children.Find("CN=Windows NT");
            parentAttribute = childEntry.Parent.Name.ToString();
            DataSchemaSite.CaptureRequirementIfIsTrue(
                parentAttribute.Equals("CN=Services"), 
                484,
                "The Parent of the Windows NT Service must be Services.");

            //MS-ADTS-Schema_R485
            PropertyValueCollection objectClassForWinNT = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClassForWinNT.Contains((object)"container"), 
                485,
                "The ObjectClass attribute of the Windows NT Service must be container.");

            //MS-ADTS-Schema_R486
            childEntry = childEntry.Children.Find("CN=Directory Service");
            requiredEntry = childEntry;
            string parentAttributeDirectory = childEntry.Parent.Name.ToString();
            DataSchemaSite.CaptureRequirementIfIsTrue(
                parentAttributeDirectory.Equals("CN=Windows NT"), 
                486,
                "The Parent of the Directory Service which is a type of Windows NT Service must be Windows NT.");

            //MS-ADTS-Schema_R487
            PropertyValueCollection objectClassForDirService = childEntry.Properties["objectClass"];

            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClassForDirService.Contains((object)"nTDSService"),
                487, 
                @"The ObjectClass attribute of the Directory Service which is a type of Windows NT Service must 
                be nTDSService.");

            //MS-ADTS-Schema_R102689
            PropertyValueCollection dSHeuristicsForDirService = childEntry.Properties["dSHeuristics"];

            if (dSHeuristicsForDirService.Value == null)
            {
                DataSchemaSite.CaptureRequirementIfIsNull(
                    dSHeuristicsForDirService.Value,
                    102689,
                    "[In Directory Service] dSHeuristics: By default, this attribute is not set.");
            }
            else
            {
                DataSchemaSite.Log.Add(LogEntryKind.Comment, "dSHeuristics has been changed by other test suites, the value is: {0}", dSHeuristicsForDirService.Value);
            }

            //MS-ADTS-Schema_R490            
            childEntry = childEntry.Children.Find("CN=Query-Policies");
            parentAttribute = childEntry.Parent.Name.ToString();
            DataSchemaSite.CaptureRequirementIfIsTrue(
                parentAttribute.Equals("CN=Directory Service"), 
                490,
                "The Parent of the Query-Policies which is a type of Windows NT Service must be Directory Service.");

            //MS-ADTS-Schema_R491
            PropertyValueCollection objectClassQueryPolicy = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClassQueryPolicy.Contains((object)"container"), 
                491,
                "The ObjectClass attribute of the Query-Policies which is a type of Windows NT Service must be container.");
            
            //MS-ADTS-Schema_R492           
            childEntry = childEntry.Children.Find("CN=Default Query Policy");
            parentAttribute = childEntry.Parent.Name.ToString();
            DataSchemaSite.CaptureRequirementIfIsTrue(
                parentAttribute.Equals("CN=Query-Policies"), 
                492, 
                "The Parent of the Default Query Policy must be Query-Policies.");

            //MS-ADTS-Schema_R493
            objectClassQueryPolicy = childEntry.Properties["objectClass"];
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClassQueryPolicy.Contains((object)"queryPolicy"), 
                493,
                "The ObjectClass attribute of the Default Query Policy must be queryPolicy.");
    }
        #endregion
    }
}