// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using System.Globalization;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    /// <summary>
    /// Server role LDAP capture code
    /// </summary>
    public partial class AD_LDAPModelAdapter : ADCommonServerAdapter, IAD_LDAPModelAdapter
    {
        /// <summary>
        /// Verify deleted objects lastKnownParent and msDS-LastKnownRDN for deleted objects
        /// </summary>
        /// <param name="deletedObjectDN">The deleted objects distinguished name</param>
        /// <param name="objectNameBeforeDeleted">The name of the object before deleted</param>
        /// <param name="lastKnownParent">Retrieved from lastKnownParent attribute</param>
        /// <param name="lastKnownRDN">Retrieved from msDS-LastKnownRDN attribute</param>
        private void VerifyDeletedObjectsRDNAndParent(
            string deletedObjectDN,
            string objectNameBeforeDeleted,
            string lastKnownParent,
            string lastKnownRDN)
        {
            if (Utilities.IsOptionalFeatureEnabled(
                forestScopePartialDN + ',' + configurationNC,
                recycleBinPartialDN + ',' + configurationNC))
            {
                string rdn = deletedObjectDN.Split(',')[0];
                // objectDN = objectRDN + parentDN. If we want to get parentDN, we used to extract it from its dn and rdn
                string parentDN = deletedObjectDN.Substring(rdn.Length + 1, deletedObjectDN.Length - (rdn.Length + 1));
                Site.CaptureRequirementIfAreEqual<string>(
                        parentDN.ToLower(CultureInfo.InvariantCulture),
                        lastKnownParent.ToLower(CultureInfo.InvariantCulture),
                        4444,
                        @"[When the delete operation results in the transformation of an object into a deleted-object]
                          For originating updates:The lastKnownParent attribute value is set to the DN of the current 
                          parent object.");
                Site.CaptureRequirementIfAreEqual<string>(
                    objectNameBeforeDeleted.ToLower(CultureInfo.InvariantCulture),
                    lastKnownRDN.ToLower(CultureInfo.InvariantCulture),
                    4445,
                    @"[When the delete operation results in the transformation of an object into a deleted-object]For 
                      originating updates:The msDS-LastKnownRDN attribute value is set to the RDN of the object before 
                      the deletion transformation.");
            }
        }

        /// <summary>
        /// Verify deleted objects dn format
        /// </summary>
        /// <param name="deletedObjectDN">The deleted objects distinguished name</param>
        /// <param name="objectNameBeforeDeletion">The name of the object before deletion</param>
        /// <param name="objectGuid">The objectGuid of the object</param>
        private void VerifyDeletedObjectsDN(
            string deletedObjectDN,
            string objectNameBeforeDeletion,
            string objectGuid)
        {
            bool isRDNFormatCorrect = false, isParentDeletedObjectsContainer = false;

            // Verify RDN from dn
            string rdn = deletedObjectDN.Split(',')[0];
            isRDNFormatCorrect = rdn
                .Equals(string.Format("CN={0}\\0ADEL:{1}", objectNameBeforeDeletion, objectGuid), StringComparison.InvariantCultureIgnoreCase);

            // Verify parent from dn
            isParentDeletedObjectsContainer = deletedObjectDN.ToLower(CultureInfo.InvariantCulture)
                .Contains((deletedObjectsContainerDNForDs.ToLower(CultureInfo.InvariantCulture)));

            if (!Utilities.IsOptionalFeatureEnabled(
                forestScopePartialDN + ',' + configurationNC,
                recycleBinPartialDN + ',' + configurationNC))
            {
                Site.Log.Add(
                    LogEntryKind.Debug,
                    "The distingsuished name of the deleted object is: {0}, the name is: {1}, guid is: {2}",
                    deletedObjectDN,
                    objectNameBeforeDeletion,
                    objectGuid);
                Site.CaptureRequirementIfIsTrue(
                    isRDNFormatCorrect && isParentDeletedObjectsContainer,
                    4632,
                    "[When the Recycle Bin optional feature is not enabled]Specifically, its[tombstone's] RDN is changed to a \"delete-mangled RDN\".In most cases, it[tombstone] is moved into the Deleted Objects container of its NC, as described in section 3.1.1.5.5.");
                Site.CaptureRequirementIfIsTrue(
                    isParentDeletedObjectsContainer,
                    1004632,
                    "[When the Recycle Bin optional feature is not enabled,specifically,tombstone's RDN is changed to a \"delete-mangled RDN\" and, ]in most cases, it[tombstone] is moved into the Deleted Objects container of its NC, as described in section 3.1.1.5.5.");
                Site.CaptureRequirementIfIsTrue(
                    isParentDeletedObjectsContainer,
                    4403,
                    @"[Tombstone Requirements]Except as described in section 3.1.1.5.5.6, tombstones exist only in the 
                      Deleted Objects container of an NC.");
                Site.CaptureRequirementIfIsTrue(
                    isRDNFormatCorrect,
                    4405,
                    "[Tombstone Requirements]Except as described in section 3.1.1.5.5.6, tombstones have \"delete-mangled RDNs\".");
            }
            else
            {
                Site.CaptureRequirementIfIsTrue(
                    isRDNFormatCorrect,
                    4442,
                    @"[When the delete operation results in the transformation of an object into a deleted-object]For 
                      originating updates: The RDN for the deleted-object is the object's delete-mangled RDN, as 
                      specified in Delete Operation in section 3.1.1.5.");
                //Here no need to check for replicated updates because each operation will base on the refreshed AD. 
                //This sentence is used for ldp.exe like visual tool, 
                //For deletion, no image will be updated unless your manually refresh the tree view.
                //But for code, the refresh does not seem mandatory because each operation is 
                //Based on the latest updates of AD.
                Site.CaptureRequirementIfIsTrue(
                    isRDNFormatCorrect,
                    4443,
                    @"[When the delete operation results in the transformation of an object into a deleted-object]For 
                      replicated updates, the received RDN for the deleted-object is set on the object.");
                Site.CaptureRequirementIfIsTrue(
                    isParentDeletedObjectsContainer,
                    4416,
                    @"[Deleted-Object Requirements]Except as described in section 3.1.1.5.5.6, deleted-objects exist 
                      in the Deleted Objects container of an NC.");
                Site.CaptureRequirementIfIsTrue(
                    isRDNFormatCorrect,
                    4417,
                    "[Deleted-Object Requirements]Except as described in section 3.1.1.5.5.6, deleted-objects have \"delete-mangled RDNs\".");
                Site.CaptureRequirementIfIsTrue(
                    isParentDeletedObjectsContainer,
                    4433,
                    @"[Recycled-Object Requirements]Except as described in section 3.1.1.5.5.6, recycled-objects exist 
                      in the Deleted Objects container of an NC.");
                Site.CaptureRequirementIfIsTrue(
                    isRDNFormatCorrect,
                    4434,
                    "[Recycled-Object Requirements]Except as described in section 3.1.1.5.5.6, recycled-objects have \"delete-mangled RDNs\".");
            }
        }

        /// <summary>
        /// Verify link of deleted objects
        /// </summary>
        /// <param name="objectClass">objectClass is always a generic link attribute</param>
        /// <param name="controls">The controls used for the search operation</param>
        /// <param name="isDeleted">Retrieved from the isDeleted attribute of a deleted object</param>
        /// <param name="isRecycled">Retrieved from the isRecycled attribute of a deleted object</param>
        private void VerifyDeletedObjectLink(string objectClass, string controls, string isDeleted, string isRecycled)
        {
            if (controls.Contains(ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID))
            {
                if (controls.Contains(ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID))
                {
                    Site.CaptureRequirementIfAreNotEqual<string>(
                        string.Empty,
                        objectClass,
                        4238,
                        @"If [the LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is] used in conjunction with 
                        LDAP_SERVER_SHOW_DELETED_OID, link attributes that are stored on deleted-objects are also
                        visible to the search operation.");
                }
                if (controls.Contains(ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID))
                {
                    Site.CaptureRequirementIfAreNotEqual<string>(
                        string.Empty,
                        objectClass,
                        4239,
                        @"If [the LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is] used in conjunction with
                        LDAP_SERVER_SHOW_RECYCLED_OID, link attributes that are stored on deleted-objects are also 
                        visible to the search operation.");
                }
            }

            if (!Utilities.IsOptionalFeatureEnabled(forestScopePartialDN + ',' + configurationNC, recycleBinPartialDN + ',' + configurationNC))
            {
                if (isDeleted.Equals(bool.TrueString))
                {
                    Site.CaptureRequirementIfIsTrue(
                        controls.Contains(ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID) || 
                        controls.Contains(ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID),
                        46,
                        @"[When the Recycle Bin optional feature is not enabled] A tombstone is not returned by a normal 
                          LDAP Search request, but only by a Search request with extended control 
                          LDAP_SERVER_SHOW_DELETED_OID or LDAP_SERVER_SHOW_RECYCLED_OID.");
                }
                if (controls.Contains(ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID))
                {
                    Site.CaptureRequirementIfAreEqual<string>(
                        bool.TrueString,
                        isDeleted,
                        4643,
                        @"[When the Recycle Bin optional feature is not enabled] A tombstone is returned by a LDAP 
                          Search request with extended control LDAP_SERVER_SHOW_DELETED_OID.");
                }
                if (controls.Contains(ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID))
                {
                    Site.CaptureRequirementIfAreEqual<string>(
                        bool.TrueString,
                        isDeleted,
                        4644,
                        @"[When the Recycle Bin optional feature is not enabled] A tombstone is returned by a LDAP 
                          Search request with extended control LDAP_SERVER_SHOW_RECYCLED_OID.");
                }
            }
            else
            {
                if (isRecycled.Equals(bool.TrueString))
                {
                    Site.CaptureRequirementIfIsTrue(
                        controls.Contains(ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID),
                        304185,
                        @"[When the Recycle Bin optional feature is enabled, in the second stage] A recycled-object is 
                          also generally not an object from the LDAP perspective: a recycled-object is returned  by a 
                          Search request with extended control LDAP_SERVER_SHOW_RECYCLED_OID, as described in section 3.1.1.3.");
                }
            }
        }

        /// <summary>
        /// Verify extended operation
        /// </summary>
        /// <param name="extendedOperation">The extendedOperation type</param>
        /// <param name="result">Returned results from server</param>
        private void VerifyRefreshEntryTTL(string extendedOperation, ExtendedOperationResponse result)
        {
            if (extendedOperation == ExtendedOperation.LDAP_TTL_REFRESH_OID)
            {
                Site.CaptureRequirementIfAreEqual<ExtendedOperationResponse>(
                    ExtendedOperationResponse.Valid,
                    result,
                    4266,
                    @"[In LDAP_TTL_REFRESH_OID, this extended operation is to refresh a specific dynamic object that 
                      has already been created.] The refresh operation is treated as a modify operation 
                      (section 3.1.1.5.3) of the entryTTL attribute (section 3.1.1.4.5.12).");
            }
        }

        /// <summary>
        /// Verify VerifyDeletedObject
        /// </summary>
        /// <param name="isDeleted">Retrieved from the isDeleted attribute of a deleted object</param>
        /// <param name="isRecycled">Retrieved from the isRecycled attribute of a deleted object</param>
        private void VerifyDeletedObject(
            string isDeleted,
            string isRecycled)
        {
            if (!Utilities.IsOptionalFeatureEnabled(
                forestScopePartialDN + ',' + configurationNC,
                recycleBinPartialDN + ',' + configurationNC))
            {
                Site.CaptureRequirementIfAreEqual<string>(
                    "True",
                    isDeleted,
                    4381,
                    @"[Delete Operation]If the Recycle Bin optional feature is not enabled, the delete operation 
                      results in the transformation of an existing-object in the directory tree into a tombstone.");
                Site.CaptureRequirementIfAreEqual<string>(
                    "True",
                    isDeleted,
                    4556,
                    @"Tombstones are distinguished from existing-objects by the presence of isDeleted attribute 
                      with the value true.");
                Site.CaptureRequirementIfAreEqual<string>(
                    "True",
                    isDeleted,
                    4386,
                    @"[Delete Operation]Tombstones exist when the Recycle Bin optional feature is not enabled.");
                Site.CaptureRequirementIfAreEqual<string>(
                    "True",
                    isDeleted,
                    886,
                    @"The delete operation results in transformation of an existing object in the directory tree into 
                      some form of deleted object.");
            }
            else
            {
                // IsRecycled is not present indicates the object is a deleted-object, else, it is a recycled-object
                if (String.IsNullOrEmpty(isRecycled))
                {
                    Site.CaptureRequirementIfAreEqual<string>(
                        "True",
                        isDeleted,
                        4389,
                        @"[Delete Operation]Deleted-objects exist when the Recycle Bin optional feature is enabled.");
                    Site.CaptureRequirementIfAreEqual<string>(
                        "True",
                        isDeleted,
                        4406,
                        @"[Deleted-Object Requirements]The isDeleted attribute is set to true on deleted-objects.");
                    Site.CaptureRequirementIfAreEqual<string>(
                        string.Empty,
                        isRecycled,
                        4407,
                        @"[Deleted-Object Requirements]The isRecycled attribute is not present.");
                    Site.CaptureRequirementIfAreEqual<string>(
                        "True",
                        isDeleted,
                        4448,
                        @"[When the delete operation results in the transformation of an object into a deleted-object]
                          The isDeleted attribute is set to true.");
                }
                else
                {
                    Site.CaptureRequirementIfAreEqual<string>(
                        "True",
                        isRecycled,
                        4391,
                        @"[Delete Operation]Recycled-objects are distinguished from existing-objects by the presence
                          of the isRecycled attribute with the value true. ");
                    Site.CaptureRequirementIfAreEqual<string>(
                        "True",
                        isRecycled,
                        4393,
                        @"[Delete Operation]Recycled-objects exist when the Recycle Bin optional feature is enabled. ");
                    Site.CaptureRequirementIfAreEqual<string>(
                        "True",
                        isDeleted,
                        4419,
                        @"[Recycled-Object Requirements]The isDeleted attribute is set to true on recycled-objects.");
                    Site.CaptureRequirementIfAreEqual<string>(
                        "True",
                        isRecycled,
                        4420,
                        "[Recycled-Object Requirements]The isRecycled attribute is set to true on recycled-objects.");
                    Site.CaptureRequirementIfAreEqual<string>(
                        "True",
                        isDeleted,
                        4457,
                        @"[When the delete operation results in the transformation of an object into a recycled-object]
                          The isDeleted attribute is set to true.");
                    Site.CaptureRequirementIfAreEqual<string>(
                        "True",
                        isRecycled,
                        4458,
                        @"[When the delete operation results in the transformation of an object into a recycled-object]
                          The isRecycled attribute is set to true.");
                }
            }
        }

        /// <summary>
        /// Verify entryTTL attribute
        /// </summary>
        /// <param name="entryTTL">Retrieved from entryTTL attribute</param>
        /// <param name="dynamicObjectMinTTL">Retrieved from dynamicObjectMinTTL attribute</param>
        /// <param name="entryTTLToBeModified">Retried from the value that this request want to set</param>
        private void VerifyEntryTTL(int entryTTL, int dynamicObjectMinTTL, int entryTTLToBeModified)
        {
            bool isR4328Verified = false;
            if (entryTTLToBeModified < dynamicObjectMinTTL)
            {
                // entryTTLToBeModified should be very small so that the time elapsed from modification to verification 
                // will still result the entryTTL to be larger than this entryTTLToBeModified value
                if (entryTTL <= dynamicObjectMinTTL && entryTTL >= entryTTLToBeModified)
                {
                    isR4328Verified = true;
                }
            }
            Site.CaptureRequirementIfIsTrue(
                isR4328Verified,
                4328,
                @"[modify operation]If a value of the entryTTL attribute is specified in the modify request, it is 
                processed as follows: If the value of the entryTTL attribute is less than the DynamicObjectMinTTL 
                LDAP setting, then the entryTTL attribute is set to the value of the DynamicObjectMinTTL setting.");
        }

        /// <summary>
        /// Verify tombstone
        /// </summary>
        /// <param name="attributesAreNotNull">A list contains the attributes in a tombstone which is not null</param>
        /// <param name="service">Indicates whether the current connection is on AD_LDS</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable CA1801, because the parameter 'service' is used for interface implementation.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyTombstone(
            List<string> attributesAreNotNull,
            ADImplementations service)
        {
            Site.CaptureRequirementIfIsFalse(
                attributesAreNotNull.Contains("objectCategory".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("sAMAccountType".ToLower(CultureInfo.InvariantCulture)),
                4398,
                @"[Tombstone Requirements]A tombstone does not have values for the attributes objectCategory or sAMAccountType.");
            bool isAttributesReserverd = false;
            foreach (string attribute in attributesAreNotNull)
            {
                int searchFlags = Utilities.GetSearchFlagsFromSchemaAttribute(
                    schemaNC,
                    attribute);
                // Search special attribute to check if PR (fPRESERVEONDELETE) is set in this attribute defination
                if (Utilities.IsAttributeReserved(searchFlags))
                {
                    isAttributesReserverd = true;
                }
            }
            Site.CaptureRequirementIfIsTrue(
                isAttributesReserverd,
                4399,
                @"[Tombstone Requirements]A tombstone does not have values for any attributes except for the following: 
                  Attributes marked as being preserved on deletion (see section 2.2.9).");
            Site.CaptureRequirementIfIsTrue(
                attributesAreNotNull.Contains("cn"),
                4400,
                @"[Tombstone Requirements]A tombstone does not have values for any attributes except for the following: 
                  The attribute that is the RDN of the tombstone.");
            Site.CaptureRequirementIfIsTrue(
                attributesAreNotNull.Contains("nTSecurityDescriptor".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("attributeID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("attributeSyntax".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("dNReferenceUpdate".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("dNSHostName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("flatName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("governsID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("groupType".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("instanceType".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("lDAPDisplayName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("legacyExchangeDN".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("isDeleted".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("isRecycled".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("lastKnownParent".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("msDS-LastKnownRDN".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("mS-DS-CreatorSID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("mSMQOwnerID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("nCName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("objectClass".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("distinguishedName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("objectGUID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("objectSid".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("oMSyntax".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("proxiedObjectName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("name".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("replPropertyMetaData".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("sAMAccountName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("securityIdentifier".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("sIDHistory".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("subClassOf".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("systemFlags".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustPartner".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustDirection".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustType".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustAttributes".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("userAccountControl".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("uSNChanged".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("uSNCreated".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("whenCreated".ToLower(CultureInfo.InvariantCulture)),
                4401,
                @"[Tombstone Requirements]A tombstone does not have values for any attributes except for the following: 
                nTSecurityDescriptor,attributeID, attributeSyntax, dNReferenceUpdate, dNSHostName, flatName, governsID, 
                groupType, instanceType, lDAPDisplayName,legacyExchangeDN, isDeleted, isRecycled, lastKnownParent, 
                msDS-LastKnownRDN, mS-DS-CreatorSID, mSMQOwnerID, nCName, objectClass,distinguishedName, objectGUID, 
                objectSid, oMSyntax, proxiedObjectName, name, replPropertyMetaData, sAMAccountName, securityIdentifier, 
                sIDHistory, subClassOf, systemFlags, trustPartner, trustDirection, trustType, trustAttributes, 
                userAccountControl, uSNChanged, uSNCreated, whenCreated.");
        }

        /// <summary>
        /// Verify deleted-objects
        /// </summary>
        /// <param name="attributesAreNotNull">A list contains the attributes in a tombstone which is not null</param>
        private void VerifyDeletedObjects(List<string> attributesAreNotNull)
        {
            Site.CaptureRequirementIfIsFalse(
                attributesAreNotNull.Contains("objectCategory".ToLower(CultureInfo.InvariantCulture)) || attributesAreNotNull.Contains("sAMAccountType".ToLower(CultureInfo.InvariantCulture)),
                4411,
                "[Deleted-Object Requirements]A deleted-object does not have values for the attributes objectCategory or sAMAccountType.");
            Site.CaptureRequirementIfIsFalse(
                attributesAreNotNull.Contains("objectCategory".ToLower(CultureInfo.InvariantCulture)) || attributesAreNotNull.Contains("sAMAccountType".ToLower(CultureInfo.InvariantCulture)),
                4447,
                @"[When the delete operation results in the transformation of an object into a deleted-object]The 
                  attributes objectCategory and sAMAccountType are removed.");
        }

        /// <summary>
        /// Verify retained attribute by deleted-object
        /// </summary>
        /// <param name="attributesAreNotNull">A list contains the attributes in a deleted object which is not null</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private void VerifyDeletedObjectRetainedAttribute(List<string> attributesAreNotNull)
        {
            Site.CaptureRequirementIfIsTrue(
                   attributesAreNotNull.Contains("nTSecurityDescriptor".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("attributeID".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("attributeSyntax".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("dNReferenceUpdate".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("dNSHostName".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("flatName".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("governsID".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("groupType".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("instanceType".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("lDAPDisplayName".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("legacyExchangeDN".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("isDeleted".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("isRecycled".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("lastKnownParent".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("msDS-LastKnownRDN".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("mS-DS-CreatorSID".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("mSMQOwnerID".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("nCName".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("objectClass".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("distinguishedName".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("objectGUID".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("objectSid".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("oMSyntax".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("proxiedObjectName".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("name".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("replPropertyMetaData".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("sAMAccountName".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("securityIdentifier".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("sIDHistory".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("subClassOf".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("systemFlags".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("trustPartner".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("trustDirection".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("trustType".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("trustAttributes".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("userAccountControl".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("uSNChanged".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("uSNCreated".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("whenCreated".ToLower(CultureInfo.InvariantCulture)),
                   915,
                   @"During the delete operation, all attribute values are removed from the object except 
                     nTSecurityDescriptor, attributeID, attributeSyntax, dNReferenceUpdate, dNSHostName, 
                     flatName, governsID, groupType, instanceType, lDAPDisplayName, legacyExchangeDN, 
                     mS-DS-CreatorSID, mSMQOwnerID, nCName, objectClass, distinguishedName, objectGUID, 
                     objectSid, oMSyntax, proxiedObjectName, name, replPropertyMetaData, sAMAccountName, 
                     securityIdentifier, sIDHistory, subClassOf, systemFlags, trustPartner, trustDirection, 
                     trustType, trustAttributes, userAccountControl, uSNChanged, uSNCreated, whenCreated attributes.");
            Site.CaptureRequirementIfIsTrue(
                   attributesAreNotNull.Contains("instanceType".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("objectGUID".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("objectSid".ToLower(CultureInfo.InvariantCulture))
                   || attributesAreNotNull.Contains("distinguishedName".ToLower(CultureInfo.InvariantCulture)),
                   916,
                   @"During the delete operation, all attribute values are removed from the object 
                     except instanceType, objectGUID, objectSid, distinguishedName.");
        }
        /// <summary>
        /// Verify recycled-objects
        /// </summary>
        /// <param name="attributesAreNotNull">A list contains the attributes in a tombstone which is not null</param>
        /// <param name="service">Indicates whether the current connection is on AD_LDS</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable CA1801, because the parameter 'service' is used for interface implementation.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyRecycledObjects(List<string> attributesAreNotNull, ADImplementations service)
        {
            Site.CaptureRequirementIfIsFalse(
                attributesAreNotNull.Contains("objectCategory".ToLower(CultureInfo.InvariantCulture)) || attributesAreNotNull.Contains("sAMAccountType".ToLower(CultureInfo.InvariantCulture)),
                4424,
                "[Recycled-Object Requirements]A recycled-object does not have values for the attributes objectCategory or sAMAccountType.");
            bool isAttributesReserverd = false;
            foreach (string attribute in attributesAreNotNull)
            {
                int searchFlags = Utilities.GetSearchFlagsFromSchemaAttribute(this.schemaNC, attribute);
                // Search special attribute to check if PR (fPRESERVEONDELETE) is set in this attribute defination
                if (Utilities.IsAttributeReserved(searchFlags))
                {
                    isAttributesReserverd = true;
                }
            }
            Site.CaptureRequirementIfIsTrue(
                isAttributesReserverd,
                4425,
                @"[Recycled-Object Requirements]A recycled-object does not have values for any attributes except for 
                  the following:Attributes marked as being preserved on deletion (see section 2.2.9).");
            Site.CaptureRequirementIfIsTrue(
                attributesAreNotNull.Contains("cn"),
                4426,
                @"[Recycled-Object Requirements]A recycled-object does not have values for any attributes except for 
                  the following:The attribute that is the RDN of the recycled-object.");
            Site.CaptureRequirementIfIsTrue(
                attributesAreNotNull.Contains("nTSecurityDescriptor".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("attributeID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("attributeSyntax".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("dNReferenceUpdate".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("dNSHostName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("flatName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("governsID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("groupType".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("instanceType".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("lDAPDisplayName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("legacyExchangeDN".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("isDeleted".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("isRecycled".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("lastKnownParent".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("msDS-LastKnownRDN".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("mS-DS-CreatorSID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("mSMQOwnerID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("nCName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("objectClass".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("distinguishedName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("objectGUID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("objectSid".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("oMSyntax".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("proxiedObjectName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("name".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("replPropertyMetaData".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("sAMAccountName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("securityIdentifier".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("sIDHistory".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("subClassOf".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("systemFlags".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustPartner".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustDirection".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustType".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustAttributes".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("userAccountControl".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("uSNChanged".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("uSNCreated".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("whenCreated".ToLower(CultureInfo.InvariantCulture)),
                4427,
                @"[Recycled-Object Requirements]A recycled-object does not have values for any attributes except for the following:
                nTSecurityDescriptor, attributeID, attributeSyntax, dNReferenceUpdate, dNSHostName, flatName, governsID, groupType,
                instanceType, lDAPDisplayName, legacyExchangeDN, isDeleted, isRecycled, lastKnownParent, msDS-LastKnownRDN,
                mS-DS-CreatorSID, mSMQOwnerID, nCName, objectClass, distinguishedName, objectGUID, objectSid, oMSyntax,
                proxiedObjectName, name, replPropertyMetaData, sAMAccountName, securityIdentifier, sIDHistory, subClassOf,
                systemFlags, trustPartner, trustDirection, trustType, trustAttributes, userAccountControl, uSNChanged, uSNCreated,
                whenCreated.");
            Site.CaptureRequirementIfIsTrue(
                attributesAreNotNull.Contains("nTSecurityDescriptor".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("attributeID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("attributeSyntax".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("dNReferenceUpdate".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("dNSHostName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("flatName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("governsID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("groupType".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("instanceType".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("lDAPDisplayName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("legacyExchangeDN".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("isDeleted".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("isRecycled".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("lastKnownParent".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("msDS-LastKnownRDN".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("mS-DS-CreatorSID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("mSMQOwnerID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("nCName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("objectClass".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("distinguishedName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("objectGUID".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("objectSid".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("oMSyntax".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("proxiedObjectName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("name")
                || attributesAreNotNull.Contains("replPropertyMetaData".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("sAMAccountName".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("securityIdentifier".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("sIDHistory".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("subClassOf".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("systemFlags".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustPartner".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustDirection".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustType".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("trustAttributes".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("userAccountControl".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("uSNChanged".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("uSNCreated".ToLower(CultureInfo.InvariantCulture))
                || attributesAreNotNull.Contains("whenCreated".ToLower(CultureInfo.InvariantCulture)),
                4451,
                @"[When the delete operation results in the transformation of an object into a recycled-object]All attribute values
                are removed from the object, with the following exceptions: nTSecurityDescriptor, attributeID, attributeSyntax,
                dNReferenceUpdate, dNSHostName, flatName, governsID, groupType, instanceType, lDAPDisplayName, lastKnownParent,
                ms-DS-lastKnownRDN, legacyExchangeDN, mS-DS-CreatorSID, mSMQOwnerID, nCName, objectClass, distinguishedName, objectGUID,
                objectSid, oMSyntax, proxiedObjectName, name, replPropertyMetaData, sAMAccountName, securityIdentifier, sIDHistory,
                subClassOf, systemFlags, trustPartner, trustDirection, trustType, trustAttributes, userAccountControl, uSNChanged,
                uSNCreated, whenCreated.");
            Site.CaptureRequirementIfIsTrue(
                attributesAreNotNull.Contains("cn"),
                4453,
                @"[When the delete operation results in the transformation of an object into a recycled-object]All attribute values are
                removed from the object, with the following exceptions: The attribute that equals the rdnType of the object (for example,
                cn for a user object) is retained.");
            Site.CaptureRequirementIfIsTrue(
                isAttributesReserverd,
                4454,
                @"[When the delete operation results in the transformation of an object into a recycled-object]All attribute values are
                removed from the object, with the following exceptions: Any attribute that has the fPRESERVEONDELETE flag set in its 
                searchFlags is retained, except objectCategory and sAMAccountType, which are always removed, regardless of the value of
                their searchFlags.");
        }
    }
}
