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
    /// This file is the source file for Validation of the TestCase8 and TestCase9.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region ValidateUniqueID Validation.

        /// <summary>
        /// This method validates the requirements under 
        /// UniqueID Scenario.
        /// </summary>
        public void ValidateUniqueID()
        {
           //If the flag fANR is set on the searchFlags attribute of the attributeSchema, then fATTINDEX must also be set.
            HashSet<string> attrIDs = new HashSet<string>();
            HashSet<string> schemaIDs = new HashSet<string>();
            HashSet<string> attrLDAPDisplayNameIDs = new HashSet<string>();
            HashSet<string> linkIDs = new HashSet<string>();
            ModelObject attributeObject, classObject;
            bool uniqueAttrID = false, uniqueSchemaID = false, uniqueGovernsID = false, uniqueSttrlDAPDisplayName = false,
                uniqueClasslDAPDisplayName = false, uniquelinkID = false, flagfANR = false;
            foreach (IObjectOnServer serverObject in adAdapter.GetAllSchemaAttributes())
            {
                if (!dcModel.TryGetAttribute(serverObject.Name, out attributeObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "schema attribute '{0}' exists on server but not in model", 
                        serverObject.Name);
                    continue;
                }
                Value attrIDValue = attributeObject["attributeID"];
                Value schemaIDGUIDValue = attributeObject["schemaIDGUID"];
                Value lDAPDisplayNameValue = attributeObject["lDAPDisplayName"];
                Value linkIDValue = attributeObject["linkID"];
                if (attrIDs.Contains(attrIDValue.ToString()))
                {
                    uniqueAttrID = true;
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "attributeID of '{0}' is already exists", 
                        serverObject.Name);
                }
                if (schemaIDs.Contains(schemaIDGUIDValue.ToString()))
                {
                    uniqueSchemaID = true;
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning, 
                        "schemaIDGUID of '{0}' is already exists", 
                        serverObject.Name);
                }
                if (attrLDAPDisplayNameIDs.Contains(lDAPDisplayNameValue.ToString()))
                {
                    uniqueSttrlDAPDisplayName = true;
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "lDAPDisplayName of '{0}' is already exists", 
                        serverObject.Name);
                }
                if (linkIDValue != null)
                    if (linkIDs.Contains(linkIDValue.ToString()))
                    {
                        uniquelinkID = true;
                        DataSchemaSite.Log.Add(
                            LogEntryKind.Comment, 
                            "linkID of '{0}' is already exists", 
                            serverObject.Name);
                    }                
                attrIDs.Add(attrIDValue.ToString());
                schemaIDs.Add(schemaIDGUIDValue.ToString());
                attrLDAPDisplayNameIDs.Add(lDAPDisplayNameValue.ToString());
                if (linkIDValue != null)
                    linkIDs.Add(linkIDValue.ToString());

                Value valueOfSearchFlag = attributeObject["searchFlags"];
                Type flagType = IntegerSymbols.GetSymbolEnumType("SearchFlags");
                string searchFlag = Enum.Format(flagType, (uint)((int)valueOfSearchFlag), "g");
                if(searchFlag.Contains("fANR"))
                {
                    if (!searchFlag.Contains("fATTINDEX"))
                    {
                        flagfANR = true;
                    }
                }
            }

            //MS-ADTS-Schema_R76
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueAttrID, 
                76, 
                @"The attributeID attribute of the attributeSchema objects described in MS-ADA1, MS-ADA2, MS-ADA3 is
                Unique OID's that identifies this attribute.");

            //MS-ADTS-Schema_R80
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueSchemaID, 
                80, 
                @"The schemaIDGUID attribute of the attributeSchema objects described in MS-ADA1, MS-ADA2, MS-ADA3 is 
                Unique GUID that identifies this attribute.");

            //MS-ADTS-Schema_R102
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueSttrlDAPDisplayName, 
                102, 
                @"The lDAPDisplayName attribute of the attributeSchema objects described in MS-ADA1, MS-ADA2, 
                MS-ADA3 is Unique that identifies this attribute.");

            //MS-ADTS-Schema_R86
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniquelinkID, 
                86, 
                "The value of the linkID attribute of the attributeSchema, " +
                "is unique among all values of this attribute on objects in MS-ADA1, MS-ADA2 and MS-ADA3, " + 
                "regardless of forest functional level.");
            //MS-ADTS-Schema_R125
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !flagfANR, 
                125,
                @"If the flag fANR is set on the searchFlags attribute of the attributeSchema, 
                then fATTINDEX must also be set.");

            HashSet<string> governsIDs = new HashSet<string>();
            HashSet<string> classLDAPDisplayNameIDs = new HashSet<string>();
            foreach (IObjectOnServer serverObject in adAdapter.GetAllSchemaClasses())
            {
                if (!dcModel.TryGetClass(serverObject.Name, out classObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "schema class '{0}' exists on server but not in model", 
                        serverObject.Name);
                    continue;
                }
                Value governsIDValue = classObject["governsID"];
                Value lDAPDisplayNameValue = classObject["lDAPDisplayName"];
                if (governsIDs.Contains(governsIDValue.ToString()))
                {
                    uniqueGovernsID = true;
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "governsID of '{0}' is already exists", 
                        serverObject.Name);
                }
                if (classLDAPDisplayNameIDs.Contains(lDAPDisplayNameValue.ToString()))
                {
                    uniqueClasslDAPDisplayName = true;
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "classlDAPDisplayName of '{0}' is already exists", 
                        serverObject.Name);
                }
                governsIDs.Add(governsIDValue.ToString());
                classLDAPDisplayNameIDs.Add(lDAPDisplayNameValue.ToString());
            }
            //MS-ADTS-Schema_R160
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueGovernsID, 
                160, 
                @"The governsID attribute of the classSchema objects described in MS-ADSC is unique that 
                identifies the class.");

            //MS-ADTS-Schema_R187
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueClasslDAPDisplayName, 
                187, 
                @"The lDAPDisplayName attribute of the classSchema objects described in MS-ADSC is unique name 
                that identifies the class.");
        }

        #endregion

        #region ValidateLDSUniqueID implementation.

        /// <summary>
        /// This method validates the requirements under 
        /// LDSUniqueID Scenario.
        /// </summary>
        public void ValidateLDSUniqueID()
        {
            ModelObject attributeObject, classObject;
            HashSet<string> attrIDsForLDS = new HashSet<string>();
            HashSet<string> schemaIDsForLDS = new HashSet<string>();
            HashSet<string> attrLDAPDisplayNameIDsLDS = new HashSet<string>();
            HashSet<string> linkIDsLDS = new HashSet<string>();
            bool uniqueLDSAttrID = false, uniqueLDSSchemaID = false, uniqueLDSattrlDAPDisplayName = false,
                uniqueLDSlinkID = false, uniqueLDSGovernsID = false,
                uniqueclassLDSlDAPDisplayName = false, flagfANR =false;
            
            foreach (IObjectOnServer serverObject in adAdapter.GetAllLdsSchemaAttributes())
            {
                if (!adamModel.TryGetAttribute(serverObject.Name, out attributeObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "schema attribute '{0}' exists on server but not in model", 
                        serverObject.Name);
                    continue;
                }
                Value attrIDValue = attributeObject["attributeID"];
                Value schemaIDGUIDValue = attributeObject["schemaIDGUID"];
                Value lDAPDisplayNameValue = attributeObject["lDAPDisplayName"];
                Value linkIDValue = attributeObject["linkID"];
                if (attrIDsForLDS.Contains(attrIDValue.ToString()))
                {
                    uniqueLDSAttrID = true;
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "attributeID of '{0}' is already exists", 
                        serverObject.Name);
                }
                //The issue here is that since the OS LDIF files don't specify a schemaGuid, one will be generated at random during ADLDS install which is not specified in model.
                if (schemaIDGUIDValue != null)
                {
                    if (schemaIDsForLDS.Contains(schemaIDGUIDValue.ToString()))
                    {
                        uniqueLDSSchemaID = true;
                        DataSchemaSite.Log.Add(
                            LogEntryKind.Warning,
                            "schemaIDGUID of '{0}' is already exists",
                            serverObject.Name);
                    }
                }
                if (attrLDAPDisplayNameIDsLDS.Contains(lDAPDisplayNameValue.ToString()))
                {
                    uniqueLDSattrlDAPDisplayName = true;
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "lDAPDisplayName of '{0}' is already exists", 
                        serverObject.Name);
                }
                if (linkIDValue != null)
                    if (linkIDsLDS.Contains(linkIDValue.ToString()))
                    {
                        uniqueLDSlinkID = true;
                        DataSchemaSite.Log.Add(
                            LogEntryKind.Warning,
                            "linkID of '{0}' is already exists", 
                            serverObject.Name);
                    }
                attrIDsForLDS.Add(attrIDValue.ToString());
                schemaIDsForLDS.Add(attrIDValue.ToString());
                attrLDAPDisplayNameIDsLDS.Add(lDAPDisplayNameValue.ToString());
                if (linkIDValue != null)
                    linkIDsLDS.Add(linkIDValue.ToString());

                Value valueOfSearchFlag = attributeObject["searchFlags"];
                Type flagType = IntegerSymbols.GetSymbolEnumType("SearchFlags");
                string searchFlag = Enum.Format(flagType, (uint)((int)valueOfSearchFlag), "g");
                if (searchFlag.Contains("fANR"))
                {
                    if (!searchFlag.Contains("fATTINDEX"))
                    {
                        flagfANR = true;
                    }
                }
            }

            //MS-ADTS-Schema_R77
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueLDSAttrID, 
                77, 
                @"The attributeID attribute of the 
                attributeSchema objects described in MS-ADLS is Unique OID's that identifies this attribute.");

            //MS-ADTS-Schema_R81
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueLDSSchemaID, 
                81, 
                @"The schemaIDGUID attribute of the 
                attributeSchema objects described in MS-ADLS is Unique GUID that identifies this attribute.");

            //MS-ADTS-Schema_R103
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueLDSattrlDAPDisplayName, 
                103, 
                @"The lDAPDisplayName attribute of the 
                attributeSchema objects described in MS-ADLS is Unique that identifies this attribute.");

            //MS-ADTS-Schema_R87
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueLDSlinkID, 
                87, 
                @"The value of the linkID attribute of the attributeSchema, is unique among all values of this attribute 
                on objects in MS-ADLS, regardless of forest functional level");

            //MS-ADTS-Schema_R125
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !flagfANR, 
                125,
                @"If the flag fANR is set on the searchFlags attribute of the attributeSchema, 
                then fATTINDEX must also be set.");

            HashSet<string> governsIDsLDS = new HashSet<string>();
            HashSet<string> classLDAPDisplayNameIDsLDS = new HashSet<string>();
            foreach (IObjectOnServer serverObject in adAdapter.GetAllLdsSchemaClasses())
            {
                if (!adamModel.TryGetClass(serverObject.Name, out classObject))
                {
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "schema class '{0}' exists on server but not in model", 
                        serverObject.Name);
                    continue;
                }
                Value governsIDValue = classObject["governsID"];
                Value lDAPDisplayNameValue = classObject["lDAPDisplayName"];
                if (governsIDsLDS.Contains(governsIDValue.ToString()))
                {
                    uniqueLDSGovernsID = true;
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "governsID of '{0}' is already exists", 
                        serverObject.Name);
                }
                if (classLDAPDisplayNameIDsLDS.Contains(lDAPDisplayNameValue.ToString()))
                {
                    uniqueclassLDSlDAPDisplayName = true;
                    DataSchemaSite.Log.Add(
                        LogEntryKind.Warning,
                        "lDAPDisplayName of '{0}' is already exists", 
                        serverObject.Name);
                }
                governsIDsLDS.Add(governsIDValue.ToString());
                classLDAPDisplayNameIDsLDS.Add(lDAPDisplayNameValue.ToString());
            }
            //MS-ADTS-Schema_R161
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueLDSGovernsID, 
                161, 
                @"The governsID attribute of the classSchema objects  
                described in MS-ADLS is unique that identifies the class.");
            //MS-ADTS-Schema_R188
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !uniqueclassLDSlDAPDisplayName, 
                188, 
                @"The lDAPDisplayName attribute of the 
                classSchema objects described in MS-ADLS is unique name that identifies the class.");
        }

        #endregion
    }
}
 


