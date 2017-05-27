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

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase16 and TestCase17.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region Test SchemaModification in DS
        /// <summary>
        /// This method validates the requirements under 
        /// SchemaModifications Scenario.
        /// </summary>
        public void ValidateSchemaModifications()
        {
            DirectoryEntry dirEntry = new DirectoryEntry();
            //schema objects cannot be deleted, therefore, use timestamp for 
            //the schema's name when creating a new schema object to avoid name conflict
            string timestamp = System.DateTime.Now.ToFileTime().ToString();
            string newClassName = "CN=ValidClass" + timestamp;
            List<string> mayAndMustContain = new List<string>();
            List<string> auxAndSystemAux = new List<string>();
            List<string> modifyAuxAndSystemAux = new List<string>();
            List<string> possAndSystemPoss = new List<string>();
            List<string> modifypossAndSystemPoss = new List<string>();
            PropertyValueCollection values = null;
            ModelObject classObject;
           
            #region Add Request for class schema in DS
            bool objectCreated = false;
            LdapConnection connection = new LdapConnection(new LdapDirectoryIdentifier(adAdapter.PDCNetbiosName + "." + adAdapter.PrimaryDomainDnsName));
            if (
                adAdapter.GetObjectByDN(newClassName
                + ",CN=Schema,CN=Configuration,"
                + adAdapter.rootDomainDN, out dirEntry))
                objectCreated = true;
            AddRequestForClassSchema(newClassName, adAdapter.PDCNetbiosName, adAdapter.rootDomainDN, objectCreated);

            #endregion

            #region Modify classSchema For Modify Request in DS

            string dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            bool isMustContain = false, isAuxiliaryClass = false, isObjectClassCategory = false, isModifyTop = false,
                isSubSchema = false, isSearchFlagSet = false, isFilteredAttrSet = false, isSubClassOf = false,
                isdefaultSecurity = false;
            bool validLdapDisplay = false, isSubClas = false, isSub = false, isRid = false, isaux = false, isposs = false,
                attrExists = false, classExists = false, uniqueID = false;
            DirectoryEntry dirEntryForGreater = new DirectoryEntry();
            if (!adAdapter.GetObjectByDN("CN=Country,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN, out dirEntryForGreater))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Country,CN=Schema,CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            dn = "CN=domainRelatedObject,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "lDAPDisplayName", "New Class is Modified");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                //MS-ADTS-Schema_R221
                // [Since the request that modifies lDAPDisplayName fails, the R221 can be captured directly.]
                DataSchemaSite.CaptureRequirement(
                    221,
                    @"A Modify request on a classSchema object fails, if the value of lDAPDisplayName is"
                    + " syntactically invalid.");

            }
            validLdapDisplay = true;
            try
            {
                ModifyRequest modifyForAuxiliaryClass = new ModifyRequest(dn, DirectoryAttributeOperation.Add,
                    "auxiliaryClass", newClassName.Split('=')[1]);
                connection.SendRequest(modifyForAuxiliaryClass);
            }
            catch (DirectoryOperationException)
            {
                try
                {
                    isMustContain = true;
                    ModifyRequest modifyForMustRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Add,
                        "mustContain", "Backward");
                    connection.SendRequest(modifyForMustRequest);
                }
                catch (DirectoryOperationException)
                {
                    isAuxiliaryClass = true;
                }
                //MS-ADTS-Schema_R232
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isAuxiliaryClass,
                    232,
                    @"In order to reduce the possibility of schema updates by one
                        application breaking another application,
                        A Modify does not add an auxiliary class
                        to the auxiliaryClass or systemAuxiliaryClass of an existing class,
                        if doing so would effectively add either
                        mustContain or systemMustContain attributes to the class.");

                try
                {
                    string dnTop = "CN=Top,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
                    DirectoryEntry dirEntryTop = new DirectoryEntry();
                    if (!adAdapter.GetObjectByDN(dnTop, out dirEntryTop))
                    {
                        DataSchemaSite.Assume.IsTrue(false, dnTop + " Object is not found in server");
                    }
                    if (!dirEntryTop.Properties["auxiliaryClass"].Contains("msDS-BindableObject"))
                    {
                        ModifyRequest modifyTop = new ModifyRequest(dnTop, DirectoryAttributeOperation.Add,
                            "auxiliaryClass", "msDS-BindableObject");
                        connection.SendRequest(modifyTop);
                    }
                }
                catch (DirectoryOperationException)
                {
                    isModifyTop = true;
                }
                try
                {
                    ModifyRequest modifyObjectClassCategory = new ModifyRequest(dn,
                        DirectoryAttributeOperation.Replace, "objectClassCategory", "1");
                    connection.SendRequest(modifyObjectClassCategory);
                }
                catch (DirectoryOperationException)
                {
                    isObjectClassCategory = true;
                }
                try
                {
                    dn = "CN=Aggregate,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
                    ModifyRequest modifySubSchema = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "cn", "AggregateSchema");
                    connection.SendRequest(modifySubSchema);
                }
                catch (DirectoryOperationException)
                {
                    isSubSchema = true;
                }
                try
                {
                    // MsTPM-OwnerInformation is contains searchFlag with fRODCFilteredAttribute bit set.
                    dn = "msTPM-OwnerInformation,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
                    ModifyRequest searchFlags = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "searchFlags", "128");

                    if (serverOS >= OSVersion.WinSvr2008)
                    {
                        //Attribute cannot be a member of a filtered attribute set if systemOnly is true for attributeSchema
                        if (dirEntryForGreater.Properties["systemOnly"].Value.Equals(true))
                        {
                            isFilteredAttrSet = true;
                            //MS-ADTS-Schema_R237
                            DataSchemaSite.CaptureRequirementIfIsTrue(
                                isFilteredAttrSet,
                                237,
                                "In order to reduce the possibility of schema updates by one application breaking"
                                + " another application, if the DC functionality level is DS_BEHAVIOR_WIN2008 or "
                                + " higher, and the attributeSchema object cannot"
                                + " be a member of the filtered attribute set");
                        }
                    }
                    connection.SendRequest(searchFlags);
                }
                catch (DirectoryOperationException)
                {
                    isSearchFlagSet = true;
                }
                try
                {
                    dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;

                    ModifyRequest subClassof = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "subClassOf", "classSchema;top");
                    connection.SendRequest(subClassof);
                }
                catch (DirectoryOperationException)
                {
                    isSubClassOf = true;
                }
                try
                {
                    dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
                    ModifyRequest defaultSecurity = new ModifyRequest(dn, DirectoryAttributeOperation.Add,
                        "defaultSecurityDescriptor", "Invalid SDDL string");
                    connection.SendRequest(defaultSecurity);
                }
                catch (DirectoryOperationException)
                {
                    isdefaultSecurity = true;
                }
            }

            //MS-ADTS-Schema_R219
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isdefaultSecurity,
                219,
                @"An Add request on a classSchema object fails if Attribute defaultSecurityDescriptor,  
                    if present, is not a valid SDDL string");

            //MS-ADTS-Schema_R228
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isSubClassOf,
                228,
                @"A Modify request on a classSchema object fails, if dynamicObject class is referenced by the 
                    subClassOf attribute of a class.");

            isSub = true;
            //MS-ADTS-Schema_R230
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isdefaultSecurity,
                230,
                @"A Modify request on a classSchema object fails if Attribute defaultSecurityDescriptor, 
                    if present, is not a valid SDDL string.");

            //MS-ADTS-Schema_R231
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isMustContain,
                231,
                "In order to reduce the possibility of schema updates by one application breaking another application,"
                + "a Modify adds no attributes to the mustContain or systemMustContain of an existing class.");


            //MS-ADTS-Schema_R233
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClassCategory,
                233,
                "In order to reduce the possibility of schema updates by one application breaking another application,"
                + "a Modify does not change the objectClassCategory of an existing class.");

            //MS-ADTS-Schema_R234
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isModifyTop,
                234,
                "In order to reduce the possibility of schema updates by one application breaking another application,"
                + "a Modify does not change class top, except to add back link attributes as may-contains, either by"
                + " adding back link attributes to mayContain of top, or by adding auxiliary classes to auxiliaryClass"
                + " of top whose only effect on top is adding back link attributes as may-contains.");

            //MS-ADTS-Schema_R235
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isSubSchema,
                235,
                @"In order to reduce the possibility of schema updates by one application breaking another
                    application, a Modify does not change the subSchema object.");

            //MS-ADTS-Schema_R236
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isSearchFlagSet,
                236,
                "In order to reduce the possibility of schema updates by one application breaking another "
                + "application, a Modify does not change fRODCFilteredAttribute"
                + " bit of the searchFlags attribute of an attributeSchema object.");

            //MS-ADTS-Schema_R222
            bool invalidGovernsId = true;
            bool invalidLdapDisplayName = true;
            bool invalidSchemaIDGUID = true;
            dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "governsID", "1.2.840.113556.1.3.23");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                invalidGovernsId = false;
            }
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "lDAPDisplayName", "Country");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                invalidLdapDisplayName = false;
            }
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "schemaIDGUID",
                    "bf967a8c-0de6-11d0-a285-00aa003049e2");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                invalidSchemaIDGUID = false;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !(invalidSchemaIDGUID || invalidLdapDisplayName || invalidGovernsId),
                222,
                @"A Modify request on a classSchema object fails, if the values of governsID,  
                lDAPDisplayName, and schemaIDGUID are not 'Unique'.");

            uniqueID = true;
            //MS-ADTS-Schema_R229
            dn = "CN=Country,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            adAdapter.GetObjectByDN(dn, out dirEntry);
            string rDNAttIDValue = dirEntry.Properties["rDNAttID"].Value.ToString();
            dcModel.TryGetAttributeContext(rDNAttIDValue, out attrContext);

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "StringUnicodeSyntax",
                attrContext.syntax.Name.ToString(),
                229,
                @"A Modify request on a classSchema object fails, if the attribute specified 
                    in the rDNAttID attribute does not have syntax String(Unicode).");

            isRid = true;
            //MS-ADTS-Schema_R225
            dn = "CN=Country,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            bool notsysaux = false, invalidaux = false;
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "systemAuxiliaryClass", "dMD");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                notsysaux = true;
            }
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "auxiliaryClass", "dMD");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                invalidaux = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                notsysaux && invalidaux,
                225,
                @"A Modify request on a classSchema object fails, if at least one class in the systemAuxiliaryClass and 
            auxiliaryClass attributes has either 88 class or auxiliary class specified as their objectClassCategory.");
            isaux = true;

            //MS-ADTS-Schema_R226
            values = dirEntry.Properties["possSuperiors"];
            foreach (string eachValue in values)
            {
                modifypossAndSystemPoss.Add(eachValue);
            }
            values = dirEntry.Properties["systemPossSuperiors"];
            foreach (string eachValue in values)
            {
                modifypossAndSystemPoss.Add(eachValue);
            }
            if (modifypossAndSystemPoss.Count != 0)
            {
                bool possOrSysPoss = false;
                foreach (string eachValue in modifypossAndSystemPoss)
                {
                    if (dcModel.TryGetClass(eachValue, out classObject))
                    {
                        DataSchemaSite.Log.Add(
                            LogEntryKind.Warning,
                            "schema class '{0}' exists on server but not in model",
                            eachValue);
                        continue;
                    }
                    if (classObject["objectClassCategory"].ToString() == "0"
                        || classObject["objectClassCategory"].ToString() == "1")
                    {
                        possOrSysPoss = true;
                    }
                }
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    !possOrSysPoss,
                    226,
                    @"A Modify request on a classSchema object fails, if at least
                        one class in the systemPossSuperiors and possSuperiors attributes
                        has either 88 class or structural class specified as their objectClassCategory.");
            }
            isposs = true;
            //MS-ADTS-Schema_R223
            bool invalidSysMayContain = false, invalidMayContain = false, invalidSystemMustContain = false,
     invalidMustContain = false;
            dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest mayContain = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "systemMayContain", "SomeAttribute");
                connection.SendRequest(mayContain);
            }
            catch (DirectoryOperationException)
            {
                invalidSysMayContain = true;
            }
            try
            {
                ModifyRequest mayContain = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "mayContain", "SomeAttribute");
                connection.SendRequest(mayContain);
            }
            catch (DirectoryOperationException)
            {
                invalidMayContain = true;
            }
            try
            {
                ModifyRequest mustContain = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "mustContain", "SomeAttribute");
                connection.SendRequest(mustContain);
            }
            catch (DirectoryOperationException)
            {
                invalidMustContain = true;
            }
            try
            {
                ModifyRequest mustContain = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "systemMustContain", "SomeAttribute");
                connection.SendRequest(mustContain);
            }
            catch (DirectoryOperationException)
            {
                invalidSystemMustContain = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidMustContain
                && invalidMayContain
                && invalidSystemMustContain
                && invalidSysMayContain,
                223,
                "A Modify request on a classSchema object fails, if at least one attribute that"
                + " is referenced in the systemMayContain, mayContain, systemMustContain and"
                + " mustContain lists does not exist and is not active.");

            attrExists = true;
            //MS-ADTS-Schema_R224
            //A Modify request on a classSchema object fails,  
            //if at least one class that is referenced in the subClassOf, systemAuxiliaryClass, auxiliaryClass, 
            //systemPossSuperiors and possSuperiors lists does not exist and is not active.
            bool invalidSubClassOf = false, invalidsystemAuxiliaryClass = false, invalidAuxiliaryClass = false,
                invalidSystemPossSuperiors = false, invalidPossSuperiors = false;
            try
            {
                ModifyRequest auxClass = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "systemAuxiliaryClass", "SomeClass");
                connection.SendRequest(auxClass);
            }
            catch (DirectoryOperationException)
            {
                invalidsystemAuxiliaryClass = true;
            }
            try
            {
                ModifyRequest auxClass = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "auxiliaryClass", "SomeClass");
                connection.SendRequest(auxClass);
            }
            catch (DirectoryOperationException)
            {
                invalidAuxiliaryClass = true;
            }
            try
            {
                ModifyRequest possClass = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "systemPossSuperiors", "SomeClass");
                connection.SendRequest(possClass);
            }
            catch (DirectoryOperationException)
            {
                invalidSystemPossSuperiors = true;
            }
            try
            {
                ModifyRequest possClass = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "possSuperiors", "SomeClass");
                connection.SendRequest(possClass);
            }
            catch (DirectoryOperationException)
            {
                invalidPossSuperiors = true;
            }
            try
            {
                ModifyRequest possClass = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "subClassOf", "SomeClass");
                connection.SendRequest(possClass);
            }
            catch (DirectoryOperationException)
            {
                invalidSubClassOf = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidSubClassOf
                && invalidsystemAuxiliaryClass
                && invalidSystemPossSuperiors
                && invalidPossSuperiors
                && invalidAuxiliaryClass,
                224,
                "A Modify request on a classSchema object fails, if at least one class that is referenced "
                + "in the subClassOf, systemAuxiliaryClass, auxiliaryClass, systemPossSuperiors and"
                + " possSuperiors lists does not exist and is not active.");

            classExists = true;
            //MS-ADTS-Schema_R227
            //A Modify request on a classSchema object fails, if the superclass chain of a class does not follow 
            //at least one of the rules for inheritance as specified in section 3.1.1.2.4.2
            invalidSubClassOf = false;
            try
            {
                ModifyRequest possClass = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "subClassOf", "configuration");
                connection.SendRequest(possClass);
            }
            catch (DirectoryOperationException)
            {
                invalidSubClassOf = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidSubClassOf,
                227,
                "A Modify request "
                + "on a classSchema object fails, if the superclass chain of a class does not follow"
                + " at least one of the rules for inheritance as specified in section 3.1.1.2.4.2.");

            isSubClas = true;
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isSub
                && isaux
                && isRid
                && isSubClas
                && isposs
                && classExists
                && attrExists
                && uniqueID
                && validLdapDisplay,
                220,
                "A Modify request on a classSchema object succeeds only if the resulting object"
                + " passes all of the following tests. The value of lDAPDisplayName is syntactically valid."
                + "The values of governsID, lDAPDisplayName, and schemaIDGUID are Unique."
                + "All attributes that are referenced in the systemMayContain, mayContain, systemMustContain,"
                + " and mustContain lists exist and are active."
                + "All classes that are referenced in the subClassOf, systemAuxiliaryClass, auxiliaryClass,"
                + " systemPossSuperiors, and possSuperiors lists exist and are active."
                + "All classes in the systemAuxiliaryClass and auxiliaryClass attributes have either 88 class"
                + " or auxiliary class specified as their objectClassCategory."
                + "All classes in the systemPossSuperiors and possSuperiors attributes have either 88 class"
                + " or structural class specified as their objectClassCategory."
                + "The superclass chain of a class follows the rules for inheritance."
                + "The dynamicObject class is not referenced by the subClassOf attribute of a class."
                + "The attribute specified in the rDNAttID attribute has syntax String(Unicode)."
                + "Attribute defaultSecurityDescriptor, if present, is a valid SDDL string.");

            #endregion

            #region  Add Request for attributeSchema in DS

            string newAttributeName = "CN=ValidAttribute";
            if (
                adAdapter.GetObjectByDN(
                newAttributeName
                + ",CN=Schema,CN=Configuration,"
                + adAdapter.rootDomainDN,
                out dirEntry))
                objectCreated = true;
            AddRequestForAttributeSchema(newAttributeName, adAdapter.PDCNetbiosName, adAdapter.rootDomainDN, objectCreated);

            #endregion

            #region Modify attributeSchema in DS

            bool validLdap = false, unique = false, validLink = false, validSyntax = false,
                isFnr = false, validRange = false;
            dn = "CN=audio,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            bool isModifyAttr = false;

            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "lDAPDisplayName", "Backward attribute is Modified");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                //MS-ADTS-Schema_R203
                // [Since the request that modifies lDAPDisplayName fails, R203 is captured.]
                DataSchemaSite.CaptureRequirement(
                    203,
                    "A Modify request on an attributeSchema object fails, if the value of lDAPDisplayName is"
                    + " syntactically invalid.");
            }

            validLdap = true;
            //MS-ADTS-Schema_R204
            bool notuniqueAttributeId = false, notuniqueLdapDisplayName = false, notuniqueSchemaIdGuid = false;
            dn = newAttributeName + ",CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "attributeId", "1.2.840.113556.1.4.159");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                notuniqueAttributeId = true;
            }
            try
            {
                ModifyRequest modRequest = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "lDAPDisplayName", "accountExpires");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                notuniqueLdapDisplayName = true;
            }
            try
            {
                ModifyRequest modRequest = new ModifyRequest(
                    dn,
                    DirectoryAttributeOperation.Replace,
                    "schemaIDGUID", "bf967915-0de6-11d0-a285-00aa003049e2");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                notuniqueSchemaIdGuid = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                notuniqueSchemaIdGuid
                && notuniqueLdapDisplayName
                && notuniqueAttributeId,
                204,
                "A Modify request on an attributeSchema object fails, if the values of attributeID, "
                + "lDAPDisplayName, mAPIID (if present) and schemaIDGUID are not 'Unique'.");

            unique = true;
            //MS-ADTS-Schema_R205
            bool nonUniqueLinkID = false;
            dn = "CN=Frs-Computer-Reference,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "linkID", "104");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                nonUniqueLinkID = true;

            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                nonUniqueLinkID,
                205,
                "A Modify request on an attributeSchema object fails if a nonzero linkID is"
                + " not unique among all values of the linkID attribute on objects"
                + " in the schema NC, regardless of forest functional level.");
            validLink = true;
            bool invalidAttributeSyntax = false;
            bool invalidOmSyntax = false;
            bool invalidOmObjectClass = false;
            dn = newAttributeName + ",CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "attributeSyntax", "2.5.5.18");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                invalidAttributeSyntax = true;
            }
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "oMSyntax", "3");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                invalidOmSyntax = true;
            }
            dn = "CN=Assistant,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "oMObjectClass", "1.3.12.2.1011.28.0.703");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                invalidOmObjectClass = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidAttributeSyntax
                && invalidOmSyntax
                && invalidOmObjectClass,
                206,
                "A Modify request on an attributeSchema object fails if a the values of attributeSyntax,"
                + " oMSyntax and oMObjectClass do not match defined syntax (section 3.1.1.2.2).");
            validSyntax = true;

            //MS-ADTS-Schema_R207
            dn = "CN=DMD-Location" + ",CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            adAdapter.GetObjectByDN(dn, out dirEntry);

            if (dcModel.TryGetAttributeContext(dirEntry.Properties["lDAPDisplayName"].Value.ToString(), out attrContext))
            {
                if (attrContext.syntax.Name.ToString() != "StringIA5Syntax"
                    && attrContext.syntax.Name.ToString() != "StringUnicodeSyntax"
                    && attrContext.syntax.Name.ToString() != "StringTeletexSyntax"
                    && attrContext.syntax.Name.ToString() != "StringCaseSyntax"
                    && attrContext.syntax.Name.ToString() != "StringPrintableSyntax")
                {
                    try
                    {
                        ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                            "searchFlags", SearchFlags.fANR.ToString());
                        connection.SendRequest(modRequest);
                    }
                    catch (DirectoryOperationException)
                    {
                        // [Since the request that modifies searchFlags fails, R207 is captured.]
                        DataSchemaSite.CaptureRequirement(
                        207,
                        "A Modify request on an attributeSchema object fails"
                        + " if flag fANR is present in the searchFlags attribute"
                        + " if the syntax is other than String(Unicode), String(IA5),"
                        + " String(Printable), String(Teletex) and String(Case).");
                    }
                }
            }

            isFnr = true;
            //MS-ADTS-Schema_R208
            dn = newAttributeName + ",CN=Schema,CN=configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "rangeLower", "256");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                // [Since the request that modifies rangeLower fails, R208 is captured.]
                DataSchemaSite.CaptureRequirement(
                    208,
                    "A Modify request on an attributeSchema object fails, if rangeLower and rangeUpper " +
                    "are not present, or rangeLower is bigger than rangeUpper.");
            }
            validRange = true;
            DataSchemaSite.CaptureRequirementIfIsTrue(
                validRange
                && validLink
                && validLdap
                && unique
                && isFnr
                && validSyntax,
                202,
                "A Modify request on an attributeSchema object succeeds only if the resulting object passes all"
                + " of the following tests: The value of lDAPDisplayName is syntactically valid."
                + "The values of attributeID, lDAPDisplayName, mAPIID (if present) and schemaIDGUID are Unique"
                + "A nonzero linkID, if any, is unique among all values of the linkID attribute on objects"
                + " in the schema NC, regardless of forest functional level. If a linkID is an odd number, "
                + "it is not one, and an object exists whose linkID is the even number one smaller."
                + "The values of attributeSyntax, oMSyntax, and oMObjectClass match some defined syntax."
                + "Flag fANR is only present in the searchFlags attribute if the syntax is String(Unicode),"
                + " String(IA5), String(Printable), String(Teletex) or String(Case)."
                + "If rangeLower and rangeUpper are present, rangeLower is smaller than or equal to rangeUpper.");

            #endregion

            #region AddorModify Common Attributes

            //MS-ADTS-Schema_R238
            isModifyAttr = false;
            if (!adAdapter.GetObjectByDN("CN=Division,CN=Schema,CN=configuration," + adAdapter.rootDomainDN, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Division,CN=Schema,CN=configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            string systemFlag = dirEntry.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_SCHEMA_BASE_OBJECT");
            if ((int)dirEntry.Properties["systemFlags"].Value != (systemFlagVal))
                dn = "CN=Division,CN=Schema,CN=configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "lDAPDisplayName", "schemaDivision");
                connection.SendRequest(modRequest);
                isModifyAttr = true;
            }
            catch (DirectoryOperationException)
            {

                DataSchemaSite.CaptureRequirementIfIsFalse(
                    isModifyAttr,
                    238,
                    @"A schema objects that include FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks if, 
                        A Modify does not change the lDAPDisplayName or cn of an attributeSchema or classSchema object, 
                        or the defaultObjectCategory of a classSchema object.");
            }

            //MS-ADTS-Schema_R239
            dn = "CN=dMD,CN=Schema,CN=configuration," + adAdapter.rootDomainDN;
            string dnSchema = "CN=account,CN=schema,CN=configuration," + adAdapter.rootDomainDN;
            if (!adAdapter.GetObjectByDN(dn, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, dn + " Object is not found in server");
            }
            systemFlagVal = ParseSystemFlagsValue("FLAG_SCHEMA_BASE_OBJECT");
            if ((int)dirEntry.Properties["systemFlags"].Value == (systemFlagVal))
            {
                try
                {
                    ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "lDAPDisplayName", "dMDSchema");
                    ModifyRequest modRequestSchema = new ModifyRequest(dnSchema, DirectoryAttributeOperation.Replace,
                        "lDAPDisplayName", "schemaAccount");

                    connection.SendRequest(modRequest);

                }
                catch (DirectoryOperationException)
                {
                    // [Since the request that modifies dMDSchema and schemaAccount fails, R239 is captured.]
                    DataSchemaSite.CaptureRequirement(
                        239,
                        "A schema objects that include FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks "
                        + "if,A Modify does not change the classSchema objects attributeSchema, classSchema, subSchema "
                        + "and dMD.");
                }
            }

            //MS-ADTS-Schema_R241   
            dn = "CN=Member,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest Class = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "attributeSecurityGUID", "00 0A 08 00 09 00 06 85 08");
                connection.SendRequest(Class);
            }
            catch (DirectoryOperationException)
            {
                // [Since the request that modifies attributeSecurityGUID fails, R241 is captured.]
                DataSchemaSite.CaptureRequirement(
                    241,
                    "A schema objects that include"
                    + " FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks if,"
                    + " A Modify does not change the attributeSecurityGUID on serverRole attributeSchema object.");
            }

            //MS-ADTS-Schema_R242
            dn = "CN=Account-Expires,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            if (!adAdapter.GetObjectByDN(dn, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, dn + " Object is not found in server");
            }
            systemFlag = dirEntry.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_SCHEMA_BASE_OBJECT");
            if (systemFlag == (systemFlagVal.ToString()))
            {
                try
                {
                    ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "attributeSecurityGUID", "00 0A 08 00 09 00 06 85 08");
                    connection.SendRequest(modRequest);
                    isModifyAttr = true;
                }
                catch (DirectoryOperationException)
                {
                    // [Since the request that modifies attributeSecurityGUID fails, R242 is captured.]
                    DataSchemaSite.CaptureRequirement(
                        242,
                        @"A schema objects that include FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks if, 
                        A Modify does not change the attributeSecurityGUID's of accountExpires, 
                        memberOf attributeSchema objects.");
                }
            }

            //MS-ADTS-Schema_R240
            if (systemFlag == (systemFlagVal.ToString()))
            {
                if ((SearchFlags)Enum.Parse(typeof(SearchFlags), dirEntry.Properties["searchFlags"].Value.ToString(), true) != SearchFlags.fCONFIDENTIAL)
                {
                    try
                    {
                        ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                            "searchFlags", ((int)SearchFlags.fCONFIDENTIAL).ToString());
                        connection.SendRequest(modRequest);
                        isModifyAttr = true;
                    }
                    catch (DirectoryOperationException)
                    {
                        //[Since the request that modifies searchFlag fails, R240 is captured.]
                        DataSchemaSite.CaptureRequirement(
                            240,
                            @"A schema objects that include FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks if, A 
                        Modify does not change the fCONFIDENTIAL bit of the searchFlags attribute of an 
                        attributeSchema object.");
                    }
                }
            }

            //MS-ADTS-Schema_R243   
            dn = "CN=Account-Name-History,CN=Schema,CN=Configuration," + adAdapter.rootDomainDN;
            try
            {
                ModifyRequest Class = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "attributeSecurityGUID", "00 0A 08 00 09 00 06 85 08");
                connection.SendRequest(Class);
            }
            catch (DirectoryOperationException)
            {
                // [Since the request that modifies the attributeSecurityGUID fails, R243 is captured.]
                DataSchemaSite.CaptureRequirement(
                    243,
                    "A schema objects that include"
                    + " FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks if,"
                    + " A Modify does not change the attributeSecurityGUID on serverRole attributeSchema object.");
            }
            #endregion

        }

        #endregion

        #region Test SchemaModification in LDS

        /// <summary>
        /// This method validates the requirements under 
        /// LDSSchemaModifications Scenario.
        /// </summary>
        public void ValidateLDSSchemaModifications()
        {
            DirectoryEntry dirEntry = new DirectoryEntry();
            List<string> mayAndMustContain = new List<string>();
            List<string> auxAndSystemAux = new List<string>();
            List<string> modifyAuxAndSystemAux = new List<string>();
            List<string> possAndSystemPoss = new List<string>();
            List<string> modifypossAndSystemPoss = new List<string>();
            PropertyValueCollection values = null;
            //schema objects cannot be deleted, therefore, use timestamp for 
            //the schema's name when creating a new schema object to avoid name conflict
            string timestamp = System.DateTime.Now.ToFileTime().ToString();
            string newClassName = "CN=ValidClass" + timestamp;
            #region Add Request for class schema in LDS
            LdapConnection connection = new LdapConnection(
                  new LdapDirectoryIdentifier(adAdapter.adamServerPort),
                  new System.Net.NetworkCredential(
                      adAdapter.ClientUserName,
                      adAdapter.ClientUserPassword,
                      adAdapter.PrimaryDomainDnsName),
                  AuthType.Ntlm | AuthType.Basic);
            //MS-ADTS-Schema_R209
            bool objectCreated = false;
            if (adAdapter.GetLdsObjectByDN(newClassName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName, out dirEntry))
                objectCreated = true;
            AddRequestForClassSchema(newClassName, adAdapter.adamServerPort, adAdapter.LDSRootObjectName, objectCreated, true);
            #endregion

            #region Modify classSchema For Modify Request in LDS

            string dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            bool isMustContain = false, isAuxiliaryClass = false, isObjectClassCategory = false, isModifyTop = false,
                isSubSchema = false, isSearchFlagSet = false, isFilteredAttrSet = false, isSubClassOf = false,
                isdefaultSecurity = false;
            bool validLdapDisplay = false, isSubClas = false, isSub = false, isRid = false, isaux = false,
                isposs = false, attrExists = false, classExists = false, uniqueID = false;
            DirectoryEntry dirEntryForGreater = new DirectoryEntry();
            if (!adAdapter.GetLdsObjectByDN("CN=NC-Name,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName,
                out dirEntryForGreater))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=NC-Name,CN=Schema,CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "ldapDisplayName", "New Class is Modified");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                // [Since the request that modifies ldapDisplayName fails, the R221 can be captured directly.]
                DataSchemaSite.CaptureRequirement(
                    221,
                    @"A Modify request on a classSchema object fails, if the value of lDAPDisplayName is"
                    + " syntactically invalid.");
            }
            validLdapDisplay = true;
            try
            {
                ModifyRequest modifyForAuxiliaryClass = new ModifyRequest(dn, DirectoryAttributeOperation.Add,
                    "auxiliaryClass", "domainRelatedObject");
                connection.SendRequest(modifyForAuxiliaryClass);
            }
            catch (DirectoryOperationException)
            {
                try
                {
                    isMustContain = true;
                    ModifyRequest modifyForMustRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Add,
                        "mustContain", "Backward");
                    connection.SendRequest(modifyForMustRequest);
                }
                catch (DirectoryOperationException)
                {
                    isAuxiliaryClass = true;
                }
                try
                {
                    string dnTop = "CN=Top,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
                    DirectoryEntry dirEntryTop = new DirectoryEntry();
                    if (!adAdapter.GetLdsObjectByDN(dnTop, out dirEntryTop))
                    {
                        DataSchemaSite.Assume.IsTrue(false, dnTop + " Object is not found in server");
                    }
                    if (!dirEntryTop.Properties["auxiliaryClass"].Contains("msDS-BindableObject"))
                    {
                        ModifyRequest modifyTop = new ModifyRequest(dnTop, DirectoryAttributeOperation.Add,
                            "auxiliaryClass", "msDS-BindableObject");
                        connection.SendRequest(modifyTop);
                    }
                }
                catch (DirectoryOperationException)
                {
                    isModifyTop = true;
                }
                try
                {

                    ModifyRequest modifyObjectClassCategory = new ModifyRequest(dn,
                        DirectoryAttributeOperation.Replace, "objectClassCategory", "1");
                    connection.SendRequest(modifyObjectClassCategory);
                }
                catch (DirectoryOperationException)
                {
                    isObjectClassCategory = true;
                }
                try
                {
                    dn = "CN=Aggregate,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
                    ModifyRequest modifySubSchema = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "cn", "AggregateSchema");
                    connection.SendRequest(modifySubSchema);
                }
                catch (DirectoryOperationException)
                {
                    isSubSchema = true;
                }
                try
                {
                    // Ms-PKI-AccountCredentials is contains searchFlag with fRODCFilteredAttribute bit set.
                    dn = "CN=ms-PKI-AccountCredentials,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
                    ModifyRequest searchFlags = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "searchFlags", "128");

                    if (serverOS >= OSVersion.WinSvr2008)
                    {
                        //Attribute cannot be a member of a filtered attribute set if systemOnly is true for attributeSchema
                        if (dirEntryForGreater.Properties["systemOnly"].Value.Equals(true))
                        {
                            isFilteredAttrSet = true;
                            //MS-ADTS-Schema_R237
                            DataSchemaSite.CaptureRequirementIfIsTrue(
                                isFilteredAttrSet,
                                237,
                                "In order to reduce the possibility of schema updates by one application"
                                + " breaking another application, if the DC functionality level"
                                + " is DS_BEHAVIOR_WIN2008 or higher, and the attributeSchema object cannot"
                                + " be a member of the filtered attribute set.");
                        }
                    }
                    connection.SendRequest(searchFlags);
                }
                catch (DirectoryOperationException)
                {
                    isSearchFlagSet = true;
                }
                //MS-ADTS-Schema_R236
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isSearchFlagSet,
                    236,
                    "In order to reduce the possibility of schema updates by one "
                    + "application breaking another application, a Modify does "
                    + "not change fRODCFilteredAttribute bit of the searchFlags "
                    + "attribute of an attributeSchema object.");

                try
                {
                    dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;

                    ModifyRequest subClassof = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "subClassOf", "dynamicObject");
                    connection.SendRequest(subClassof);
                }
                catch (DirectoryOperationException)
                {
                    isSubClassOf = true;
                }
                //MS-ADTS-Schema_R228
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    isSubClassOf,
                    228,
                    @"A Modify request on a classSchema object fails, if dynamicObject class is referenced by the 
                    subClassOf attribute of a class.");
                isSub = true;
                try
                {
                    dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
                    ModifyRequest defaultSecurity = new ModifyRequest(dn, DirectoryAttributeOperation.Add,
                        "defaultSecurityDescriptor",
                        "Invalid SDDL string");
                    connection.SendRequest(defaultSecurity);
                }
                catch (DirectoryOperationException)
                {
                    isdefaultSecurity = true;
                }
            }
            //MS-ADTS-Schema_R230
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isdefaultSecurity,
                230,
                @"A Modify request on a classSchema object fails if Attribute defaultSecurityDescriptor, 
                    if present, is not a valid SDDL string.");

            //MS-ADTS-Schema_R231
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isMustContain,
                231,
                "In order to reduce the possibility of schema updates by one application breaking another application,"
                + "a Modify adds no attributes to the mustContain or systemMustContain of an existing class.");

            //MS-ADTS-Schema_R232
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isAuxiliaryClass,
                232,
                @"In order to reduce the possibility of schema updates by one
                        application breaking another application,
                        A Modify does not add an auxiliary class
                        to the auxiliaryClass or systemAuxiliaryClass of an existing class,
                        if doing so would effectively add either
                        mustContain or systemMustContain attributes to the class.");

            //MS-ADTS-Schema_R233
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClassCategory,
                233,
                "In order to reduce the possibility of schema updates by one application breaking another application,"
                + "a Modify does not change the objectClassCategory of an existing class.");

            //MS-ADTS-Schema_R234
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isModifyTop,
                234,
                "In order to reduce the possibility of schema updates by one application breaking another application,"
                + "a Modify does not change class top, except to add back link attributes as may-contains, either by"
                + " adding back link attributes to mayContain of top, or by adding auxiliary classes to auxiliaryClass"
                + " of top whose only effect on top is adding back link attributes as may-contains.");

            //MS-ADTS-Schema_R235
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isSubSchema,
                235,
                @"In order to reduce the possibility of schema updates by one application breaking another
                    application, a Modify does not change the subSchema object.");

            //MS-ADTS-Schema_R222
            bool validGovernsId = true;
            bool validLdapDisplayName = true;
            bool validSchemaIDGUID = true;
            dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(dn, DirectoryAttributeOperation.Add,
                    "governsID", "1.2.840.113556.1.3.23");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                validGovernsId = false;
            }
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(dn, DirectoryAttributeOperation.Add,
                    "lDAPDisplayName", "Country");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                validLdapDisplayName = false;
            }
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(dn, DirectoryAttributeOperation.Add,
                    "schemaIDGUID", "bf967a8c-0de6-11d0-a285-00aa003049e2");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                validSchemaIDGUID = false;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !(validSchemaIDGUID
                || validLdapDisplayName
                || validGovernsId),
                222,
                @"A Modify request on a classSchema object fails, if the values of governsID,  
                lDAPDisplayName, and schemaIDGUID are not 'Unique'.");

            uniqueID = true;
            //MS-ADTS-Schema_R229
            dn = "CN=Country,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            adAdapter.GetLdsObjectByDN(dn, out dirEntry);
            string rDNAttIDValue = dirEntry.Properties["rDNAttID"].Value.ToString();
            dcModel.TryGetAttributeContext(rDNAttIDValue, out attrContext);

            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                "StringUnicodeSyntax",
                attrContext.syntax.Name.ToString(),
                229,
                @"A Modify request on a classSchema object fails, if the attribute specified 
                    in the rDNAttID attribute does not have syntax String(Unicode).");

            isRid = true;
            //MS-ADTS-Schema_R225
            dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            bool notsysaux = false, invalidaux = false;
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "systemAuxiliaryClass", "dMD");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                notsysaux = true;
            }
            try
            {
                ModifyRequest defaultSecurity = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "auxiliaryClass", "dMD");
                connection.SendRequest(defaultSecurity);
            }
            catch (DirectoryOperationException)
            {
                invalidaux = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                notsysaux && invalidaux,
                225,
                @"A Modify request on a classSchema object fails, if at least one class in the systemAuxiliaryClass and 
            auxiliaryClass attributes has either 88 class or auxiliary class specified as their objectClassCategory.");

            isaux = true;
            //MS-ADTS-Schema_R226
            values = dirEntry.Properties["possSuperiors"];
            foreach (string eachValue in values)
            {
                modifypossAndSystemPoss.Add(eachValue);
            }
            values = dirEntry.Properties["systemPossSuperiors"];
            foreach (string eachValue in values)
            {
                modifypossAndSystemPoss.Add(eachValue);
            }
            if (modifypossAndSystemPoss.Count != 0)
            {
                bool possOrSysPoss = false;
                foreach (string eachValue in modifypossAndSystemPoss)
                {
                    if (dcModel.TryGetClass(eachValue, out classObject))
                    {
                        DataSchemaSite.Log.Add(LogEntryKind.Warning,
                            "schema class '{0}' exists on server but not in model", eachValue);
                        continue;
                    }
                    if (
                        classObject["objectClassCategory"].ToString() == "0"
                        || classObject["objectClassCategory"].ToString() == "1")
                    {
                        possOrSysPoss = true;
                    }
                }
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    !possOrSysPoss,
                    226,
                    @"A Modify request on a classSchema object fails, if at least
                        one class in the systemPossSuperiors and possSuperiors attributes
                        has either 88 class or structural class specified as their objectClassCategory.");
            }

            isposs = true;
            //MS-ADTS-Schema_R223
            bool invalidSysMayContain = false, invalidMayContain = false, invalidSystemMustContain = false,
     invalidMustContain = false;
            dn = newClassName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            try
            {
                ModifyRequest mayContain = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "systemMayContain", "SomeAttribute");
                connection.SendRequest(mayContain);
            }
            catch (DirectoryOperationException)
            {
                invalidSysMayContain = true;
            }
            try
            {
                ModifyRequest mayContain = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "mayContain", "SomeAttribute");
                connection.SendRequest(mayContain);
            }
            catch (DirectoryOperationException)
            {
                invalidMayContain = true;
            }
            try
            {
                ModifyRequest mustContain = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "mustContain", "SomeAttribute");
                connection.SendRequest(mustContain);
            }
            catch (DirectoryOperationException)
            {
                invalidMustContain = true;
            }
            try
            {
                ModifyRequest mustContain = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "systemMustContain", "SomeAttribute");
                connection.SendRequest(mustContain);
            }
            catch (DirectoryOperationException)
            {
                invalidSystemMustContain = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidMustContain
                && invalidMayContain
                && invalidSystemMustContain
                && invalidSysMayContain,
                223,
                "A Modify request on a classSchema object fails, if at least one attribute that"
                + " is referenced in the systemMayContain, mayContain, systemMustContain and"
                + " mustContain lists does not exist and is not active.");

            attrExists = true;
            //MS-ADTS-Schema_R224
            //A Modify request on a classSchema object fails, if at least one class that is referenced in the 
            //subClassOf, systemAuxiliaryClass, auxiliaryClass, systemPossSuperiors and possSuperiors lists does 
            //not exist and is not active.
            bool invalidSubClassOf = false, invalidsystemAuxiliaryClass = false, invalidAuxiliaryClass = false,
                invalidSystemPossSuperiors = false, invalidPossSuperiors = false;
            try
            {
                ModifyRequest auxClass = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "systemAuxiliaryClass", "SomeClass");
                connection.SendRequest(auxClass);
            }
            catch (DirectoryOperationException)
            {
                invalidsystemAuxiliaryClass = true;
            }
            try
            {
                ModifyRequest auxClass = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "auxiliaryClass", "SomeClass");
                connection.SendRequest(auxClass);
            }
            catch (DirectoryOperationException)
            {
                invalidAuxiliaryClass = true;
            }
            try
            {
                ModifyRequest possClass = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "systemPossSuperiors", "SomeClass");
                connection.SendRequest(possClass);
            }
            catch (DirectoryOperationException)
            {
                invalidSystemPossSuperiors = true;
            }
            try
            {
                ModifyRequest possClass = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "possSuperiors", "SomeClass");
                connection.SendRequest(possClass);
            }
            catch (DirectoryOperationException)
            {
                invalidPossSuperiors = true;
            }
            try
            {
                ModifyRequest possClass = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "subClassOf", "SomeClass");
                connection.SendRequest(possClass);
            }
            catch (DirectoryOperationException)
            {
                invalidSubClassOf = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidSubClassOf
                && invalidsystemAuxiliaryClass
                && invalidSystemPossSuperiors
                && invalidPossSuperiors
                && invalidAuxiliaryClass,
                224,
                "A Modify request on a classSchema object fails, if at least one class that is referenced "
                + "in the subClassOf, systemAuxiliaryClass, auxiliaryClass, systemPossSuperiors and"
                + " possSuperiors lists does not exist and is not active.");

            classExists = true;
            //MS-ADTS-Schema_R227
            //A Modify request on a classSchema object fails, if the superclass chain of a class does not follow 
            //at least one of the rules for inheritance as specified in section 3.1.1.2.4.2
            invalidSubClassOf = false;
            try
            {
                ModifyRequest possClass = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "subClassOf", "configuration");
                connection.SendRequest(possClass);
            }
            catch (DirectoryOperationException)
            {
                invalidSubClassOf = true;
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    invalidSubClassOf,
                    227,
                    "A Modify request on a "
                    + "classSchema object fails, if the superclass chain of a class does not follow at least "
                    + "one of the rules for inheritance as specified in section 3.1.1.2.4.2.");
            }

            isSubClas = true;
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isSub
                && isaux
                && isRid
                && isSubClas
                && isposs
                && classExists
                && attrExists
                && uniqueID
                && validLdapDisplay,
                220,
                "A Modify request on a classSchema object succeeds only if the resulting object"
                + " passes all of the following tests. The value of lDAPDisplayName is syntactically valid."
                + "The values of governsID, lDAPDisplayName, and schemaIDGUID are Unique."
                + "All attributes that are referenced in the systemMayContain, mayContain, systemMustContain,"
                + " and mustContain lists exist and are active."
                + "All classes that are referenced in the subClassOf, systemAuxiliaryClass, auxiliaryClass,"
                + " systemPossSuperiors, and possSuperiors lists exist and are active."
                + "All classes in the systemAuxiliaryClass and auxiliaryClass attributes have either 88 class"
                + " or auxiliary class specified as their objectClassCategory."
                + "All classes in the systemPossSuperiors and possSuperiors attributes have either 88 class"
                + " or structural class specified as their objectClassCategory."
                + "The superclass chain of a class follows the rules for inheritance."
                + "The dynamicObject class is not referenced by the subClassOf attribute of a class."
                + "The attribute specified in the rDNAttID attribute has syntax String(Unicode)."
                + "Attribute defaultSecurityDescriptor, if present, is a valid SDDL string.");

            #endregion

            #region  Add Request for attributeSchema in LDS

            string newAttributeName = "CN=ValidAttribute";
            if (
                adAdapter.GetLdsObjectByDN(
                newAttributeName
                + ",CN=Schema,CN=Configuration,"
                + adAdapter.LDSRootObjectName,
                out dirEntry))
                objectCreated = true;
            AddRequestForAttributeSchema(newAttributeName, adAdapter.adamServerPort, adAdapter.LDSRootObjectName, objectCreated,true);

            #endregion

            #region Modify attributeSchema in LDS

            bool validLdap = false, unique = false, validLink = false, validSyntax = false, isFnr = false,
                validRange = false;

            //MS-ADTS-Schema_R203
            dn = newAttributeName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            bool isModifyAttr = true;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "lDAPDisplayName", "Attribute");
                connection.SendRequest(modRequest);

            }
            catch (DirectoryOperationException)
            {
                // [Since the request that modifies lDAPDisplayName fails, R203 is captured.]
                DataSchemaSite.CaptureRequirement(
                    203,
                    "A Modify request on an attributeSchema object fails, if the value of lDAPDisplayName is"
                    + " syntactically invalid.");
            }
            validLdap = true;
            //MS-ADTS-Schema_R204
            bool uniqueAttributeId = true, uniqueLdapDisplayName = true, uniqueSchemaIdGuid = true;
            dn = newAttributeName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "attributeId", "1.2.840.113556.1.4.159");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                uniqueAttributeId = false;
            }
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "lDAPDisplayName", "accountExpires");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                uniqueLdapDisplayName = false;
            }
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "schemaIDGUID", "bf967915-0de6-11d0-a285-00aa003049e2");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                uniqueSchemaIdGuid = false;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                !(uniqueSchemaIdGuid
                || uniqueLdapDisplayName
                || uniqueAttributeId),
                204,
                "A Modify request on an attributeSchema object fails, if the " +
                "values of attributeID, lDAPDisplayName, mAPIID (if present) and schemaIDGUID are not 'Unique'.");
            unique = true;
            dn = "CN=Managed-By,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            bool nonUniqueLinkID = false;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace, "linkID",
                    "104");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                nonUniqueLinkID = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                nonUniqueLinkID,
                205,
               "A Modify request on an attributeSchema object fails "
               + "if a nonzero linkID is not unique among all values of the"
               + " linkID attribute on objects in the schema NC, regardless of forest functional level.");

            validLink = true;
            //MS-ADTS-Schema_R206
            bool invalidAttributeSyntax = false;
            bool invalidOmSyntax = false;
            bool invalidOmObjectClass = false;
            dn = newAttributeName + ",CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "attributeSyntax", "2.5.5.18");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                invalidAttributeSyntax = true;
            }
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "oMSyntax", "3");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                invalidOmSyntax = true;
            }
            dn = "CN=Assistant,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "oMObjectClass", "1.3.12.2.1011.28.0.703");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                invalidOmObjectClass = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidAttributeSyntax
                && invalidOmSyntax
                && invalidOmObjectClass,
                206,
                "A Modify request on an attributeSchema object fails if a the values of attributeSyntax,"
                + " oMSyntax and oMObjectClass do not match defined syntax (section 3.1.1.2.2).");

            validSyntax = true;
            //MS-ADTS-Schema_R207
            dn = "CN=DMD-Location,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            adAdapter.GetLdsObjectByDN(dn, out dirEntry);
            if (dcModel.TryGetAttributeContext(dirEntry.Properties["lDAPDisplayName"].Value.ToString(), out attrContext))
            {
                if (attrContext.syntax.Name.ToString() != "StringIA5Syntax"
                    && attrContext.syntax.Name.ToString() != "StringUnicodeSyntax"
                    && attrContext.syntax.Name.ToString() != "StringTeletexSyntax"
                    && attrContext.syntax.Name.ToString() != "StringCaseSyntax"
                    && attrContext.syntax.Name.ToString() != "StringPrintableSyntax")
                {
                    try
                    {
                        ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                            "searchFlags", SearchFlags.fANR.ToString());
                        connection.SendRequest(modRequest);
                    }
                    catch (DirectoryOperationException)
                    {
                        // [Since the request that modifies searchFlags fails, R207 is captured.]
                        DataSchemaSite.CaptureRequirement(
                        207,
                        "A Modify request on an attributeSchema object fails"
                        + " if flag fANR is present in the searchFlags attribute"
                        + " if the syntax is other than String(Unicode), String(IA5),"
                        + " String(Printable), String(Teletex) and String(Case).");
                    }
                }
            }

            isFnr = true;
            dn = newAttributeName + ",CN=Schema,CN=configuration," + adAdapter.LDSRootObjectName;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace, "rangeLower", "256");
                connection.SendRequest(modRequest);
            }
            catch (DirectoryOperationException)
            {
                // [Since the request that modifies rangeLower fails, R208 is captured.]
                DataSchemaSite.CaptureRequirement(
                    208,
                    "A Modify request on an attributeSchema object fails, if rangeLower and rangeUpper " +
                    "are not present, or rangeLower is bigger than rangeUpper.");
            }
            validRange = true;
            DataSchemaSite.CaptureRequirementIfIsTrue(
                validRange
                && validLink
                && validLdap
                && unique
                && isFnr
                && validSyntax,
                202,
                "A Modify request on an attributeSchema object succeeds only if the resulting object passes all"
                + " of the following tests: The value of lDAPDisplayName is syntactically valid."
                + "The values of attributeID, lDAPDisplayName, mAPIID (if present) and schemaIDGUID are Unique"
                + "A nonzero linkID, if any, is unique among all values of the linkID attribute on objects"
                + " in the schema NC, regardless of forest functional level. If a linkID is an odd number, "
                + "it is not one, and an object exists whose linkID is the even number one smaller."
                + "The values of attributeSyntax, oMSyntax, and oMObjectClass match some defined syntax."
                + "Flag fANR is only present in the searchFlags attribute if the syntax is String(Unicode),"
                + " String(IA5), String(Printable), String(Teletex) or String(Case)."
                + "If rangeLower and rangeUpper are present, rangeLower is smaller than or equal to rangeUpper.");

            #endregion

            #region Add or Modify Common Attributes

            //MS-ADTS-Schema_R238
            if (!adAdapter.GetLdsObjectByDN("CN=Container,CN=Schema,CN=configuration," + adAdapter.LDSRootObjectName, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Container,CN=Schema,CN=configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }
            string systemFlag = dirEntry.Properties["systemFlags"].Value.ToString();
            int systemFlagVal = ParseSystemFlagsValue("FLAG_SCHEMA_BASE_OBJECT");
            isModifyAttr = false;
            if ((int)dirEntry.Properties["systemFlags"].Value != (systemFlagVal))
                dn = "CN=Container,CN=Schema,CN=configuration," + adAdapter.LDSRootObjectName;

            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "lDAPDisplayName", "schemaDivision");
                connection.SendRequest(modRequest);
                isModifyAttr = true;
            }
            catch (DirectoryOperationException)
            {
                DataSchemaSite.CaptureRequirementIfIsFalse(
                    isModifyAttr,
                    238,
                    @"A schema objects that include FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks if, 
                        A Modify does not change the lDAPDisplayName or cn of an attributeSchema or classSchema object, 
                        or the defaultObjectCategory of a classSchema object.");
            }

            //MS-ADTS-Schema_R239
            dn = "CN=DMD,CN=Schema,CN=configuration," + adAdapter.LDSRootObjectName;
            string dnSchema = "CN=account,CN=schema,CN=configuration," + adAdapter.LDSRootObjectName;
            if (!adAdapter.GetLdsObjectByDN(dn, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, dn + " Object is not found in server");
            }
            systemFlagVal = ParseSystemFlagsValue("FLAG_SCHEMA_BASE_OBJECT");
            if ((int)dirEntry.Properties["systemFlags"].Value == (systemFlagVal))
                try
                {
                    ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "lDAPDisplayName", "dMDSchema");
                    ModifyRequest modRequestSchema = new ModifyRequest(dnSchema,
                        DirectoryAttributeOperation.Replace, "lDAPDisplayName", "schemaAccount");

                    connection.SendRequest(modRequest);

                }
                catch (DirectoryOperationException)
                {
                    // [Since the request that modifies dMDSchema and schemaAccount fails, R239 is captured.]
                    DataSchemaSite.CaptureRequirement(
                        239,
                        "A schema objects that include FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks "
                        + "if,A Modify does not change the classSchema objects attributeSchema, classSchema, subSchema "
                        + "and dMD.");
                }

            //MS-ADTS-Schema_R240
            dn = "CN=Account-Expires,CN=Schema,CN=configuration," + adAdapter.LDSRootObjectName;
            if (!adAdapter.GetLdsObjectByDN(dn, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, dn + " Object is not found in server");
            }

            if ((SearchFlags)Enum.Parse(typeof(SearchFlags), dirEntry.Properties["searchFlags"].Value.ToString(), true) != SearchFlags.fCONFIDENTIAL)
                try
                {
                    ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                        "searchFlags", ((int)SearchFlags.fCONFIDENTIAL).ToString());
                    connection.SendRequest(modRequest);
                    isModifyAttr = true;
                }
                catch (DirectoryOperationException)
                {
                    // [Since the request that modifies searchFlag fails, R240 is captured.]
                    DataSchemaSite.CaptureRequirement(
                        240,
                        @"A schema objects that include FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks if, A 
                        Modify does not change the fCONFIDENTIAL bit of the searchFlags attribute of an 
                        attributeSchema object.");
                }

            //MS-ADTS-Schema_R241   
            dn = "CN=Member,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            try
            {
                ModifyRequest Class = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "attributeSecurityGUID", "00 0A 08 00 09 00 06 85 08");
                connection.SendRequest(Class);
            }
            catch (DirectoryOperationException)
            {
                // [Since the request that modifies attributeSecurityGUID fails, R241 is captured.]
                DataSchemaSite.CaptureRequirement(
                    241,
                    "A schema objects that include"
                    + " FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks if,"
                    + " A Modify does not change the attributeSecurityGUID on serverRole attributeSchema object.");
            }

            //MS-ADTS-Schema_R242
            dn = "CN=Account-Expires,CN=Schema,CN=Configuration," + adAdapter.LDSRootObjectName;
            if (!adAdapter.GetLdsObjectByDN(dn, out dirEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, dn + " Object is not found in server");
            }
            systemFlag = dirEntry.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_SCHEMA_BASE_OBJECT");
            if (systemFlag != (systemFlagVal.ToString()))

                isModifyAttr = false;
            try
            {
                ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                    "attributeSecurityGUID", "00 0A 08 00 09 00 06 85 08");
                connection.SendRequest(modRequest);
                isModifyAttr = true;

            }
            catch (DirectoryOperationException)
            {
                // [Since the request that modifies attributeSecurityGUID fails, R242 is captured.]
                DataSchemaSite.CaptureRequirement(
                    242,
                    @"A schema objects that include FLAG_SCHEMA_BASE_OBJECT in the systemFlags attribute checks if, 
                        A Modify does not change the attributeSecurityGUID's of accountExpires, 
                        memberOf attributeSchema objects.");
            }

            #endregion

        #endregion

        }
        /// <summary>
        /// This funtion adds a new attribute to the schema NC
        /// </summary>
        /// <param name="newAttributeName">Name of the new attribute</param>
        /// <param name="serverName">Name of the server to connect</param>
        /// <param name="domainDN"> Distinguished name of the domain</param>
        /// <param name="objectCreated"> Flag to identify whether object is created or not</param>
        void AddRequestForAttributeSchema(string newAttributeName, string serverName, string domainDN, bool objectCreated, bool isLDS = false)
        {
            string serverFullName = serverName;
            if (serverName.IndexOf('.') == -1)
            {
                serverFullName = serverName + "." + adAdapter.PrimaryDomainDnsName;
            }
            LdapConnection connection = null;
            if (!isLDS)
            {
                connection = new LdapConnection(
                   new LdapDirectoryIdentifier(serverFullName),
                   new System.Net.NetworkCredential(
                       adAdapter.DomainAdministratorName,
                       adAdapter.DomainUserPassword,
                       adAdapter.PrimaryDomainDnsName),
                   AuthType.Kerberos | AuthType.Basic);
            }
            else
            {
                connection = new LdapConnection(
                  new LdapDirectoryIdentifier(serverFullName),
                  new System.Net.NetworkCredential(
                      adAdapter.ClientUserName,
                      adAdapter.ClientUserPassword,
                      adAdapter.PrimaryDomainDnsName),
                  AuthType.Ntlm | AuthType.Basic);
            }
            bool inValidSyntax = false;
            //MS-ADTS-Schema_R195
            //This try will throw exception if attribute is not created
            if (!objectCreated)
            {
                try
                {

                    AddRequest request = new AddRequest();
                    request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                    request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.830.914596.3.8.132"));
                    request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", newAttributeName.Split('=')[1]));
                    request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.1"));
                    request.Attributes.Add(new DirectoryAttribute("oMSyntax", "127"));
                    request.Attributes.Add(new DirectoryAttribute("rangeLower", "0"));
                    request.Attributes.Add(new DirectoryAttribute("rangeUpper", "128"));
                    request.Attributes.Add(new DirectoryAttribute("searchFlags", "0"));
                    request.Attributes.Add(new DirectoryAttribute("objectClass", "top", "attributeSchema"));
                    AddResponse response = (AddResponse)connection.SendRequest(request);
                    //This statement will executed only if the attribute is successfully created.
                    objectCreated = true;
                }
                catch (DirectoryOperationException)
                {
                    //Attribute already exists.
                }
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectCreated,
                195,
                "An Add request on an attributeSchema object succeeds"
                + " only if the resulting object passes all of the following tests:"
                + "The value of lDAPDisplayName is syntactically valid."
                + "The values of attributeID, lDAPDisplayName, mAPIID (if present) and schemaIDGUID are 'Unique'"
                + "A nonzero linkID, if any, is unique among all values"
                + " of the linkID attribute on objects in the schema NC," +
                " regardless of forest functional level. If a linkID is an odd number,"
                + " it is not one, and an object exists whose linkID is the even number one smaller."
                + "The values of attributeSyntax, oMSyntax, and oMObjectClass match some defined syntax."
                + "Flag fANR is only present in the searchFlags attribute if the syntax is String(Unicode), String(IA5),"
                + " String(Printable), String(Teletex) or String(Case)."
                + "If rangeLower and rangeUpper are present, rangeLower is smaller than or equal to rangeUpper.");

            //MS-ADTS-Schema_R196
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.4.1244"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalid attribute"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.16"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "65"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                inValidSyntax = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                inValidSyntax,
                196,
                "An Add request on an attributeSchema object fails, if the value of lDAPDisplayName "
                + "is syntactically invalid.");

            //MS-ADTS-Schema_R197
            newAttributeName = "CN=invalidAttribute";
            bool invalidAttributeId = false;
            bool invalidLdapDisplayName = false, invalidSchemaIDGUID = false;
            //this throws exception because attributeId is not unique
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.4.1244"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidattribute"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.16"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "65"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidAttributeId = true;
            }
            //this throws exception because lDAPDisplayname is not unique
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.8.1243"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "cn"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.16"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "65"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidLdapDisplayName = true;
            }
            //this throws exception because schemaIdGuid is not unique
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.8.1243"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidattribute"));
                request.Attributes.Add(new DirectoryAttribute("schemaIdGuid", "bf9679b5-0de6-11d0-a285-00aa003049e2"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.16"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "65"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidSchemaIDGUID = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(invalidSchemaIDGUID &&
                invalidLdapDisplayName && invalidAttributeId, 197,
                "An Add request on an attributeSchema object fails, if the values of attributeID, lDAPDisplayName," +
                " mAPIID (if present) and schemaIDGUID are not 'Unique'.");

            //MS-ADTS-Schema_R198
            //this throws exception because linkID is not unique
            bool nonUniqueLinkID = false;
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.8.1243"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidattribute"));
                request.Attributes.Add(new DirectoryAttribute("linkID", "43"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.16"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "65"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                nonUniqueLinkID = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(nonUniqueLinkID, 198,
                    "An Add request on an attributeSchema object fails if a nonzero linkID is not unique " +
                    "among all values of the linkID attribute on objects in the schema NC,"
                    + " regardless of forest functional level.");
            //MS-ADTS-Schema_R199
            //This try throws Exception because attributeSyntax is not valid
            bool invalidAttributeSyntax = false, invalidOmSyntax = false, invalidOmObjectClass = false;
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.8.1243"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidattribute"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.18"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "65"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidAttributeSyntax = true;
            }
            //This try throws Exception because omSyntax is not valid
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.8.1243"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidattribute"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.16"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "3"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidOmSyntax = true;
            }
            //This try throws Exception because omObjectClass is not valid
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.8.1243"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidattribute"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.14"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "127"));
                request.Attributes.Add(new DirectoryAttribute("oMObjectClass", "1.3.12.2.1011.28.0.703"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidOmObjectClass = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidOmObjectClass
                && invalidOmSyntax
                && invalidAttributeSyntax,
                199,
                "An Add request on an attributeSchema object fails if a the values of attributeSyntax, "
                + "oMSyntax and oMObjectClass do not match defined syntax (section 3.1.1.2.2).");

            //MS-ADTS-Schema_R200
            //This try throws Exception because fANR is present for searchflags in interger
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.8.1243"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidattribute"));
                request.Attributes.Add(new DirectoryAttribute("linkID", "43"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.16"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "65"));
                request.Attributes.Add(new DirectoryAttribute("searchFlags", "fANR"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                // [R200 is captured,since the Add request fails when the flag FANR is presnt in searchFlags and the syntax is other than String(Unicode), String(IA5), String(Printable), String(Teletex) and String(Case).]
                DataSchemaSite.CaptureRequirement(
                    200,
                    @"An Add request on an attributeSchema object fails if flag fANR is present in the searchFlags 
                    attribute if the syntax is other than String(Unicode), String(IA5), String(Printable), 
                    String(Teletex) and String(Case).");
            }

            //MS-ADTS-Schema_R201
            //An Add request on an attributeSchema object fails, if rangeLower and rangeUpper are not present,
            //or rangeLower is bigger than rangeUpper.
            try
            {
                AddRequest request = new AddRequest();
                request.DistinguishedName = newAttributeName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("attributeId", "1.2.840.113556.1.8.1243"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidattribute"));
                request.Attributes.Add(new DirectoryAttribute("linkID", "43"));
                request.Attributes.Add(new DirectoryAttribute("attributeSyntax", "2.5.5.16"));
                request.Attributes.Add(new DirectoryAttribute("oMSyntax", "65"));
                request.Attributes.Add(new DirectoryAttribute("rangeLower", "128"));
                request.Attributes.Add(new DirectoryAttribute("rangeUpper", "0"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                // [R201 is captured, since the Add request on an attributeSchema object fails when the rangerLower is bigger than rangeUpper in the case they are present.]
                DataSchemaSite.CaptureRequirement(
                    201,
                    "An Add request on an attributeSchema object fails, if rangeLower and rangeUpper are present,"
                    + " and rangeLower is bigger than rangeUpper.");
            }
        }

        /// <summary>
        /// This funtion adds a new class to the schema NC
        /// </summary>
        /// <param name="newClassName">Name of the new class</param>
        /// <param name="serverName">Name of the server to connect</param>
        /// <param name="domainDN">Distinguished name of the domain</param>
        /// <param name="objectCreated"> Flag to identify whether object is created or not</param>
        /// <param name="isLDS"> Flag to identify whether add request is for LDS or not</param>
        void AddRequestForClassSchema(string newClassName, string serverName, string domainDN, bool objectCreated, bool isLDS = false)
        {
            string serverFullName = serverName;
            if (serverName.IndexOf('.') == -1)
            {
                serverFullName = serverName +"." + adAdapter.PrimaryDomainDnsName;
            }
            LdapConnection connection = null;
            if (!isLDS)
            {
                connection = new LdapConnection(
                   new LdapDirectoryIdentifier(serverFullName),
                   new System.Net.NetworkCredential(
                       adAdapter.DomainAdministratorName,
                       adAdapter.DomainUserPassword,
                       adAdapter.PrimaryDomainDnsName),
                   AuthType.Kerberos | AuthType.Basic);
            }
            else
            {
                connection = new LdapConnection(
                  new LdapDirectoryIdentifier(serverFullName),
                  new System.Net.NetworkCredential(
                      adAdapter.ClientUserName,
                      adAdapter.ClientUserPassword,
                      adAdapter.PrimaryDomainDnsName),
                  AuthType.Ntlm | AuthType.Basic);
            }

            bool inValidSyntax = false;

            //MS-ADTS-Schema_R209
            //This try will throw exception if the object is not created
            if (!objectCreated)
            {
                try
                {
                    AddRequest request = new AddRequest();
                    request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                    //governsId must be unique, therefore, create a random governsId whenever a new schema object is created
                    Random rand = new Random();
                    int randGovernsId = rand.Next(485386, 938436);
                    request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.6.1." + randGovernsId.ToString() + ".1.11.130"));
                    request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", newClassName.Split('=')[1]));
                    request.Attributes.Add(new DirectoryAttribute("systemMayContain", "department"));
                    request.Attributes.Add(new DirectoryAttribute("mayContain", "businessCategory", "comment"));
                    request.Attributes.Add(new DirectoryAttribute("systemMustContain", "instanceType"));
                    request.Attributes.Add(new DirectoryAttribute("mustContain", "department"));
                    request.Attributes.Add(new DirectoryAttribute("systemAuxiliaryClass", "SecurityPrincipal"));
                    request.Attributes.Add(new DirectoryAttribute("auxiliaryClass", "SecurityPrincipal"));
                    request.Attributes.Add(new DirectoryAttribute("systemPossSuperiors", "container"));
                    request.Attributes.Add(new DirectoryAttribute("possSuperiors", "group", "organizationalPerson"));
                    request.Attributes.Add(new DirectoryAttribute("objectClass", "classSchema"));
                    request.Attributes.Add(new DirectoryAttribute("objectClassCategory", "3"));
                    request.Attributes.Add(new DirectoryAttribute("rDNAttID", "cn"));
                    AddResponse response = (AddResponse)connection.SendRequest(request);
                    System.Threading.Thread.Sleep(30000);
                    objectCreated = true;
                    //This statement executes only if the class is successfully created
                }
                catch (DirectoryOperationException)
                {
                    //Class is already present
                }
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(objectCreated, 209, "An Add request on a classSchema object succeeds only if"
                   + "the resulting object passes all of the following tests."
                   + "The value of lDAPDisplayName is syntactically valid."
                   + "The values of governsID, lDAPDisplayName, and schemaIDGUID"
                   + " are 'Unique'.All attributes that are referenced in the systemMayContain,"
                   + " mayContain, systemMustContain, and mustContain lists exist and are active."
                   + "All classes that are referenced in the subClassOf, systemAuxiliaryClass,"
                   + "auxiliaryClass, systemPossSuperiors, and possSuperiors lists exist and are active."
                   + "All classes in the systemAuxiliaryClass and auxiliaryClass attributes have either "
                   + "88 class or auxiliary class specified as their objectClassCategory."
                   + "All classes in the systemPossSuperiors and possSuperiors attributes have either"
                   + " 88 class or structural class specified as their objectClassCategory."
                   + "The superclass chain of a class follows the rules for inheritance. "
                   + "The dynamicObject class is not referenced by the subClassOf attribute of a class."
                   + "The attribute specified in the rDNAttID attribute has syntax String(Unicode)."
                   + "Attribute defaultSecurityDescriptor, if present, is a valid SDDL string.");

            DirectoryEntry requiredEntry = new DirectoryEntry();
            if (isLDS)
            {
                if (!adAdapter.GetLdsObjectByDN(newClassName + ",CN=Schema,CN=Configuration," + domainDN, out requiredEntry))
                {
                    DataSchemaSite.Assert.IsTrue(
                        false,
                        newClassName + ",CN=Schema,CN=Configuration," + domainDN
                        + " Object is not found in server");
                }
            }
            else
            {
                if (!adAdapter.GetObjectByDN(newClassName + ",CN=Schema,CN=Configuration," + domainDN, out requiredEntry))
                {
                    DataSchemaSite.Assert.IsTrue(
                        false,
                        newClassName + ",CN=Schema,CN=Configuration," + domainDN
                        + " Object is not found in server");
                }
            }
            
			//MS-ADTS-Schema_Auto Generated Attributes
            DataSchemaSite.Assert.AreEqual<string>(
                "top",
                requiredEntry.Properties["subClassOf"].Value.ToString(),
                @"For the Auto-Generated Attributes, if a classSchema object is created with an LDAP Add operation,
                and the subClassOf attribute is not included, it must refer to class top");

            DataSchemaSite.Assert.IsTrue(
                bool.Parse(requiredEntry.Properties["showInAdvancedViewOnly"].Value.ToString()),
                @"For the Auto-Generated Attributes, if a classSchema object is created with an LDAP Add operation,
                and the showInAdvancedViewOnly attribute is not included, it must be set to TRUE");

            DataSchemaSite.Assert.AreEqual<string>(
                newClassName + ",CN=Schema,CN=Configuration," + domainDN,
                requiredEntry.Properties["defaultObjectCategory"].Value.ToString(),
                @"For the Auto-Generated Attributes, if a classSchema object is created with an LDAP Add operation,
                and the defaultObjectCategory attribute is not included, it must refer to the new classSchema object itself");

            //MS-ADTS-Schema_R210
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalid Class"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                inValidSyntax = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                inValidSyntax,
                210,
                "An Add request on a classSchema object fails, if the value of lDAPDisplayName is syntactically invalid.");

            //MS-ADTS-Schema_R211
            bool invalidGovernsId = false, invalidLdapDisplayName = false, invalidSchemaIDGUID = false;
            // This try will throws exception because governsid is not unique 
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.198"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidClass"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidGovernsId = true;
            }
            // This try will throws exception because lDAPDisplayName is not unique 
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "country"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidLdapDisplayName = true;
            }
            // This try will throws exception because schemaIdGuid is not unique 
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("schemaIdGuid", "1ed3a473-9b1b-418a-bfa0-3a37b95a5306"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidSchemaIDGUID = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(invalidLdapDisplayName && invalidGovernsId &&
                invalidSchemaIDGUID, 211, "An Add request on a classSchema object fails, if the values of governsID," +
                " lDAPDisplayName, and schemaIDGUID are not 'Unique'");
            invalidLdapDisplayName = false;
            invalidSchemaIDGUID = false;
            //MS-ADTS-Schema_R212
            bool invalidSysMayContain = false, invalidMayContain = false, invalidSystemMustContain = false,
                invalidMustContain = false;
            // This try will throws exception because assist class does not exist 
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("systemMayContain", "assist"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidSysMayContain = true;
            }
            // This try will throws exception because assist class does not exist
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("mayContain", "assist"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidMayContain = true;
            }
            // This try will throws exception because assist class does not exist
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("systemMustContain", "assist"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidSystemMustContain = true;
            }
            // This try will throws exception because assist class does not exist
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("mustContain", "assist"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidMustContain = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(invalidMustContain && invalidMayContain &&
                invalidSysMayContain && invalidSystemMustContain, 212, "An Add request on a classSchema object f" +
                "ails,  if at least one attribute that is referenced in the systemMayContain, mayContain, systemMustContain " +
                "and mustContain lists does not exist and is not active.");
            //MS-ADTS-Schema_R213
            bool invalidSubClassOf = false, invalidsystemAuxiliaryClass = false, invalidAuxiliaryClass = false,
                invalidSystemPossSuperiors = false, invalidPossSuperiors = true;
            // This try will throws exception because someClass class does not exist
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("subClassOf", "someClass"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidSubClassOf = true;
            }
            // This try will throws exception because someClass class does not exist
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("systemAuxiliaryClass", "someClass"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidsystemAuxiliaryClass = true;
            }
            // This try will throws exception because someClass class does not exist
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("auxiliaryClass", "someClass"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidAuxiliaryClass = true;
            }
            // This try will throws exception because someClass class does not exist
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("systemPossSuperiors", "someClass"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidSystemPossSuperiors = true;
            }
            // This try will throws exception because someClass class does not exist
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("possSuperiors", "someClass"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidPossSuperiors = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidPossSuperiors
                && invalidSystemPossSuperiors
                && invalidSubClassOf
                && invalidsystemAuxiliaryClass
                && invalidAuxiliaryClass,
                213,
                "An Add request on a classSchema object fails, "
                + "if at least one class that is referenced in the subClassOf, systemAuxiliaryClass, auxiliaryClass,"
                + " systemPossSuperiors and possSuperiors lists does not exist and is not active.");

            //MS-ADTS-Schema_R214
            invalidAuxiliaryClass = false;
            invalidPossSuperiors = false;
            invalidsystemAuxiliaryClass = false;
            invalidSystemPossSuperiors = false;
            invalidSubClassOf = false;
            // This try will throws exception because server class is not 88 or auxiliary class
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("systemAuxiliaryClass", "server"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidsystemAuxiliaryClass = true;
            }
            // This try will throws exception because server class is not 88 or auxiliary class
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("auxiliaryClass", "server"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidAuxiliaryClass = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidsystemAuxiliaryClass
                && invalidAuxiliaryClass,
                214,
                @"An Add request on a classSchema object fails, if at least one class in the systemAuxiliaryClass and 
                auxiliaryClass attributes is not either 88 class or auxiliary class specified as their 
                objectClassCategory.");

            //MS-ADTS-Schema_R215
            // This try will throws exception because msDS-BindableObject class is not 88 or structural class
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("systemPossSuperiors", "msDS-BindableObject"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidSystemPossSuperiors = true;
            }
            // This try will throws exception because msDS-BindableObject class is not 88 or structural class
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("possSuperiors", "msDS-BindableObject"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidPossSuperiors = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidPossSuperiors
                && invalidSystemPossSuperiors,
                215,
                @"An Add request on a classSchema object fails, if at least one class in the systemPossSuperiors and 
                possSuperiors attributes has either 88 class or structural class specified as their 
                objectClassCategory.");

            //MS-ADTS-Schema_R216
            // This try will throws exception because msDS-BindableObject class is auxiliary class
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("objectClassCategory", "1"));
                request.Attributes.Add(new DirectoryAttribute("subClassOf", "msDS-BindableObject"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidSubClassOf = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidSubClassOf,
                216,
                "An Add request on a classSchema object fails, "
                + "if the superclass chain of a class does not follow at least one of the rules for inheritance as "
                + "specified in section 3.1.1.2.4.2.");

            //MS-ADTS-Schema_R217
            invalidSubClassOf = false;
            // This try will throws exception because subClassOf attribute is dynamic class
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("objectClassCategory", "1"));
                request.Attributes.Add(new DirectoryAttribute("subClassOf", "dynamicObject"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidSubClassOf = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidSubClassOf,
                217,
                "An Add request on a classSchema object fails, if dynamicObject class is referenced by the "
                + "subClassOf attribute of a class.");

            //MS-ADTS-Schema_R218
            bool invalidRdnAttID = false;
            //An Add request on a classSchema object fails, if the attribute specified in the rDNAttID attribute 
            //does not have syntax String(Unicode).
            try
            {
                newClassName = "CN=InvalidClass";
                AddRequest request = new AddRequest();
                request.DistinguishedName = newClassName + ",CN=Schema,CN=Configuration," + domainDN;
                request.Attributes.Add(new DirectoryAttribute("governsId", "1.2.840.113556.10.50.197"));
                request.Attributes.Add(new DirectoryAttribute("lDAPDisplayName", "invalidclass"));
                request.Attributes.Add(new DirectoryAttribute("objectClassCategory", "1"));
                request.Attributes.Add(new DirectoryAttribute("rDNAttID", "c n"));
                AddResponse response = (AddResponse)connection.SendRequest(request);
            }
            catch (DirectoryOperationException)
            {
                invalidRdnAttID = true;
            }
            DataSchemaSite.CaptureRequirementIfIsTrue(
                invalidRdnAttID,
                218,
                "An Add request on a classSchema object fails,"
                + " if the attribute specified in the rDNAttID attribute does not have syntax String(Unicode).");
        }
    }
}



