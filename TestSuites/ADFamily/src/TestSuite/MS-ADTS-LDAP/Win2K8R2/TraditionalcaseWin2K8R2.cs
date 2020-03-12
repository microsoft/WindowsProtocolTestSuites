// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;


namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    /// <summary>
    /// MS-ADTS-LDAP traditional testcase
    /// </summary>
    [TestClass]
    public class TraditionalcaseWin2K8R2 : TestClassBase
    {
        #region Variables

        /// <summary>
        /// AD_LDAPModelAdapter instance
        /// </summary>
        public static AD_LDAPModelAdapter adLdapModelAdapter = null;

        /// <summary>
        /// Temporary creation of Site Variable
        /// </summary>
        public static ITestSite TraditionalcaseWin2K8R2TestSite = null;

        /// <summary>
        /// Attribute and value used in add operation
        /// </summary>
        private Sequence<string> addAttrValSeq;

        /// <summary>
        /// Attribute and value used in modify operation
        /// </summary>
        private Map<string, Sequence<string>> modAttrValMap;

        /// <summary>
        /// Error status returned by add operation
        /// </summary>
        private ConstrOnAddOpErrs addErrorStatus;

        /// <summary>
        /// Error status returned by modify operation
        /// </summary>
        private ConstrOnModOpErrs modErrorStatus;

        /// <summary>
        /// Error status returned by delete operation
        /// </summary>
        private ConstrOnDelOpErr delErrorStatus;

        #endregion

        /// <summary>
        /// Response of search
        /// </summary>
        ICollection<AdtsSearchResultEntryPacket> searchResponse;

        Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry resultEntry;

        PartialAttributeList attributeList;

        PartialAttributeList_element[] attributeList_element;

        Asn1SetOf<AttributeValue> mvale;

        AttributeValue attributevalue;


        #region Test Suite Initialization

        /// <summary>
        /// Class initialization
        /// </summary>
        /// <param name="testContext">test context</param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);

            // Initializing the ITestSite object
            if (null == TraditionalcaseWin2K8R2TestSite)
            {
                TraditionalcaseWin2K8R2TestSite = TestClassBase.BaseTestSite;
                TraditionalcaseWin2K8R2TestSite.DefaultProtocolDocShortName = "MS-ADTS-LDAP";
            }
            adLdapModelAdapter = AD_LDAPModelAdapter.Instance(TraditionalcaseWin2K8R2TestSite);
            adLdapModelAdapter.Initialize();
        }

        /// <summary>
        /// Class cleanup
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Set the life time of deleted-object/recycled object back to original
            adLdapModelAdapter.SetADObjectLifeTime(1, 1);

            adLdapModelAdapter.Reset();
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Case Initialization and clean up

        /// <summary>
        /// Test initialize
        /// </summary>
        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        /// <summary>
        /// Test clean up
        /// </summary>
        protected override void TestCleanup()
        {
            base.TestCleanup();

            adLdapModelAdapter.CleanUpEnvironment();
        }

        #endregion

        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_DeleteOperationWithRecycleBinEnabled()
        {
            Site.Assume.IsTrue(adLdapModelAdapter.RecycleBinEnabled, "This test case requires recycle bin to be enabled. Environment will be broken because if once recycle bin is enabled, it cannot be disabled.");

            #region Local Variables

            string testUser0Dn = adLdapModelAdapter.testUser0DNForDs;
            string testUser1Dn = adLdapModelAdapter.testUser1DNForDs;

            // attributes on deleted objects
            bool isDelete = false;
            bool isRecycle = false;
            byte[] guidByte = null;
            byte[] sidByte = null;

            #endregion

            #region Add object testUser0

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser0Dn).Add("objectClass: user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add object testUser1

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser1Dn).Add("objectClass: user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Enable recycle bin if not enabled

            if (!Utilities.IsOptionalFeatureEnabled(adLdapModelAdapter.forestScopePartialDN + ',' + adLdapModelAdapter.configurationNC, adLdapModelAdapter.recycleBinPartialDN + ',' + adLdapModelAdapter.configurationNC))
            {
                modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("enableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
                adLdapModelAdapter.ModifyRecycleBin(
                    modAttrValMap,
                    RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    ADImplementations.AD_DS,
                    ServerVersion.Win2008R2,
                    false,
                    out modErrorStatus);
                Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");
            }

            #endregion

            #region Delete testUser0 Stage 1 - tombstone

            #region Get GUID and Sid for testUser0 before delete

            var attributes = Utilities.GetAttributesFromEntry(
                testUser0Dn,
                new string[] { "objectGUID", "objectSid" },
                adLdapModelAdapter.PDCNetbiosName + "." + adLdapModelAdapter.PrimaryDomainDnsName,
                adLdapModelAdapter.ADDSPortNum);

            Guid testUser0GuidBeforeDelete = new Guid((byte[])attributes["objectGUID"]);
            SecurityIdentifier testUser0SidBeforeDelete = new SecurityIdentifier(Utilities.ConvertSidToStringSid((byte[])attributes["objectSid"]));

            #endregion

            #region Delete object testUser0

            adLdapModelAdapter.DeleteOperation(
                testUser0Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUser0 with LDAP_SERVER_SHOW_DELETED_OID control

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUser0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID);
            // When we search a deleted-object using LDAP_SERVER_SHOW_DELETED_OID and the response is not null, then this requirement is verified.
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4646, "[When the Recycle Bin optional feature is enabled, in the first stage] A deleted-object is generally not an object from the LDAP perspective: a deleted-object is returned by a Search request with extended control LDAP_SERVER_SHOW_DELETED_OID OID, as described in section 3.1.1.3.");
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4231, "When LDAP_SERVER_SHOW_DELETED_OID is used, the deleted-objects are visible.");
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4258, "When LDAP_SERVER_SHOW_DELETED_OID control is used, the deleted-objects are visible.");

            #endregion

            #region Search testUser0 with LDAP_SERVER_SHOW_RECYCLED_OID and LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUser0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID + ";" + ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID);
            // When we search a deleted-object using LDAP_SERVER_SHOW_RECYCLED_OID and the response is not null, then this requirement is verified.
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4647, "[When the Recycle Bin optional feature is enabled, in the first stage] A deleted-object is generally not an object from the LDAP perspective: a deleted-object is returned by a Search request with extended control LDAP_SERVER_SHOW_RECYCLED_OID, as described in section 3.1.1.3.");
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4234, "When LDAP_SERVER_SHOW_RECYCLED_OID is used, the deleted-objects are visible.");
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4261, "When LDAP_SERVER_SHOW_RECYCLED_OID control is used, the deleted-objects are visible.");
            if (adLdapModelAdapter.currentWorkingDC.OSVersion >= ServerVersion.Win2008R2)
            {
                // When we search a recycled-object using LDAP_SERVER_SHOW_RECYCLED_OID and the response is not null, then this requirement is verified.
                Site.CaptureRequirementIfIsNotNull(resultEntry, 4230, "The LDAP extended control LDAP_SERVER_SHOW_RECYCLED_OID is supported in Windows Server 2008 R2.");
                // When we search a recycled-object using LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and the response is not null, then this requirement is verified.
                Site.CaptureRequirementIfIsNotNull(resultEntry, 4229, "The LDAP extended control LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID is supported in Windows Server 2008 R2.");
            }

            PartialAttributeList partialAttributeList = resultEntry.attributes;
            PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
            Asn1SetOf<AttributeValue> setOfAttributeValue = new Asn1SetOf<AttributeValue>();
            AttributeValue attributeValue = new AttributeValue();
            for (int i = 0; i < attributeListElement.Length; i++)
            {
                string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                setOfAttributeValue = attributeListElement[i].vals;
                attributeValue = setOfAttributeValue.Elements[0];
                if (title == "isDeleted")
                {
                    isDelete = Convert.ToBoolean(Encoding.ASCII.GetString(attributeValue.ByteArrayValue).ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                }
                if (title == "isRecycled")
                {
                    isRecycle = Convert.ToBoolean(Encoding.ASCII.GetString(attributeValue.ByteArrayValue).ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                }
                if (title == "objectGUID")
                {
                    guidByte = attributeValue.ByteArrayValue as byte[];
                }
                if (title == "objectSid")
                {
                    sidByte = attributeValue.ByteArrayValue as byte[];
                }
            }
            string testUser0DnAfterDelete = Encoding.ASCII.GetString(resultEntry.objectName.ByteArrayValue);
            Guid testUser0GuidAfterDelete = new Guid(guidByte);
            SecurityIdentifier testUser0SidAfterDelete = new SecurityIdentifier(sidByte, 0);

            Site.CaptureRequirementIfIsTrue(testUser0GuidAfterDelete.Equals(testUser0GuidBeforeDelete) && testUser0SidAfterDelete.Equals(testUser0SidBeforeDelete), 4408, "[Deleted-Object Requirements]The deleted-object retains the objectGUID and objectSid (if any) attributes of the original object.");
            Site.CaptureRequirementIfIsTrue(testUser0GuidAfterDelete.Equals(testUser0GuidBeforeDelete) && !testUser0DnAfterDelete.Equals(testUser0Dn), 4176, "[When the Recycle Bin optional feature is enabled, in the first stage] The deleted-object also resembles the state of the object before deletion; it has the same objectGUID but a different DN.");
            Site.CaptureRequirementIfIsTrue(testUser0DnAfterDelete.Contains("\\0ADEL"), 4177, "[When the Recycle Bin optional feature is enabled, in the first stage] Specifically, its[the deleted-object's] RDN is changed to a \"delete-mangled RDN\".");
            Site.CaptureRequirementIfIsTrue(testUser0DnAfterDelete.Contains("CN=Deleted Objects"), 4178, "[When the Recycle Bin optional feature is enabled, in the first stage, the deleted-object's RDN is changed to a \"delete-mangled RDN\" and] in most cases, it[the deleted-object] is moved into the Deleted Objects container of its NC, as described in section 3.1.1.5.5. ");
            Site.CaptureRequirementIfIsTrue(testUser0DnAfterDelete.Contains("\\0ADEL") && testUser0DnAfterDelete.Contains("CN=Deleted Objects"), 104173, "[When the Recycle Bin optional feature is enabled, object deletion is performed in three stages]Specifically, its RDN[The deleted-object's RDN] is changed to a \"delete-mangled RDN\".");
            Site.CaptureRequirementIfIsTrue(testUser0DnAfterDelete.Contains("CN=Deleted Objects"), 204173, "[When the Recycle Bin optional feature is enabled, object deletion is performed in three stages, specifically, the deleted-object's RDN is changed to a \"delete-mangled RDN\" and,] in most cases, it is moved into the Deleted Objects container of its NC, as described in section 3.1.1.5.5.");
            Site.CaptureRequirementIfIsTrue(isDelete && !isRecycle, 4174, "[When the Recycle Bin optional feature is enabled] In the first stage, the object being deleted is transformed into a deleted-object.");
            Site.CaptureRequirementIfIsTrue(isDelete && !isRecycle, 4382, "[Delete Operation]If the Recycle Bin optional feature is enabled and the requester has specified an existing-object as the object to be transformed, the deletion operation results in transformation of the existing-object in the directory tree into a deleted-object.");

            #endregion

            #region Try to modify displayname of testUser0

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("displayname: name1", new Sequence<string>().Add("distinguishedName: " + testUser0Dn)));
            adLdapModelAdapter.ModifyOperation(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual(ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION, modErrorStatus, 4323, "Any other modifications [except the operation in R1956,R4584,R4585,R4586] of these objects[objects with isDeleted = true is allowed] fail with unwillingToPerform / ERROR_DS_ILLEGAL_MOD_OPERATION.");

            #endregion

            #endregion

            #region Delete testUser1 Stage 1 - tombstone

            #region Delete object testUser1

            adLdapModelAdapter.DeleteOperation(
                testUser1Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUser1 with LDAP_SERVER_SHOW_DELETED_OID control

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUser1Dn,
                ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID);

            Site.CaptureRequirementIfIsNotNull(resultEntry, 4233, "When LDAP_SERVER_SHOW_DELETED_OID is used, the recycled-objects are not visible.");
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4260, "When LDAP_SERVER_SHOW_DELETED_OID control is used, the recycled-objects are not visible.");

            #endregion

            #endregion

            #region Delete object container

            #region Search delete object container with LDAP_SERVER_SHOW_RECYCLED_OID control

            string deletedObjectContainer = "CN=Deleted Objects," + adLdapModelAdapter.rootDomainNC;

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                deletedObjectContainer,
                ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID);
            Site.Assert.IsNotNull(resultEntry, "Search operation should be successful.");

            isRecycle = false;
            isDelete = false;
            partialAttributeList = resultEntry.attributes;
            attributeListElement = partialAttributeList.Elements;
            setOfAttributeValue = new Asn1SetOf<AttributeValue>();
            attributeValue = new AttributeValue();
            for (int i = 0; i < attributeListElement.Length; i++)
            {
                string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                setOfAttributeValue = attributeListElement[i].vals;
                attributeValue = setOfAttributeValue.Elements[0];
                if (title == "isDeleted")
                {
                    isDelete = Convert.ToBoolean(Encoding.ASCII.GetString(attributeValue.ByteArrayValue).ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                }
                if (title == "isRecycled")
                {
                    isRecycle = Convert.ToBoolean(Encoding.ASCII.GetString(attributeValue.ByteArrayValue).ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                }
            }
            Site.CaptureRequirementIfIsTrue(isDelete && !isRecycle, 4410, "[Deleted-Object Requirements]The deleted-object remains in the database and is available for outbound replication for at least the deleted-object lifetime interval (see section 7.1.1) after its deletion, with the following exception: The Deleted Objects container (which is itself a deleted-object if the Recycle Bin optional feature is enabled) always remains available, and is never transformed into a recycled-object.");

            #endregion

            #region Try to modify displayname of delete object container

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("displayname: name", new Sequence<string>().Add("distinguishedName: " + deletedObjectContainer)));
            adLdapModelAdapter.ModifyOperation(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(
                ConstrOnModOpErrs.insufficientAccessRights,
                modErrorStatus,
                4606,
                @"During the modify operation, Modifying an object with isDeleted = true (a tombstone) is allowed if the following conditions is true:
                 The object being modified is the Deleted Objects container.");

            #endregion

            #endregion

            #region Delete testUser0 Stage 2 - recycle bin

            #region Delete testUser0 which is in tombstone

            adLdapModelAdapter.DeleteOperation(
                testUser0Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUser0 with LDAP_SERVER_SHOW_RECYCLED_OID control

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUser0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID);
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4263, "When LDAP_SERVER_SHOW_RECYCLED_OID control is used, the recycled-objects are visible.");

            isRecycle = false;
            isDelete = false;
            partialAttributeList = resultEntry.attributes;
            attributeListElement = partialAttributeList.Elements;
            setOfAttributeValue = new Asn1SetOf<AttributeValue>();
            attributeValue = new AttributeValue();
            for (int i = 0; i < attributeListElement.Length; i++)
            {
                string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                setOfAttributeValue = attributeListElement[i].vals;
                attributeValue = setOfAttributeValue.Elements[0];
                if (title == "isDeleted")
                {
                    isDelete = Convert.ToBoolean(Encoding.ASCII.GetString(attributeValue.ByteArrayValue).ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                }
                if (title == "isRecycled")
                {
                    isRecycle = Convert.ToBoolean(Encoding.ASCII.GetString(attributeValue.ByteArrayValue).ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                }
                if (title == "objectGUID")
                {
                    guidByte = attributeValue.ByteArrayValue as byte[];
                }
                if (title == "objectSid")
                {
                    sidByte = attributeValue.ByteArrayValue as byte[];
                }
            }
            Guid testUser0GuidAfterRecycle = new Guid(guidByte);
            SecurityIdentifier testUser0SidAfterRecycle = new SecurityIdentifier(sidByte, 0);
            string testUser0DnAfterRecycle = Encoding.ASCII.GetString(resultEntry.objectName.ByteArrayValue);

            Site.CaptureRequirementIfIsTrue(testUser0GuidAfterRecycle.Equals(testUser0GuidBeforeDelete) && testUser0SidAfterRecycle.Equals(testUser0SidBeforeDelete), 4421, "[Recycled-Object Requirements]The recycled-object retains the objectGUID and objectSid (if any) attributes of the original object.");
            Site.CaptureRequirementIfIsTrue(testUser0GuidAfterRecycle.Equals(testUser0GuidBeforeDelete) && !testUser0DnAfterRecycle.Equals(testUser0Dn), 4182, "[When the Recycle Bin optional feature is enabled, in the second stage] The recycled-object also resembles the state of the object before deletion; it has the same objectGUID but a different DN.");
            Site.CaptureRequirementIfIsTrue(testUser0DnAfterRecycle.Contains("\\0ADEL"), 4183, "[When the Recycle Bin optional feature is enabled, in the second stage] Specifically, its [the recycled-object's] RDN has been changed.");
            Site.CaptureRequirementIfIsTrue(testUser0DnAfterRecycle.Contains("CN=Deleted Objects"), 4184, "[When the Recycle Bin optional feature is enabled, in the second stage] in most cases, the object [the recycled-object] moved, as described in the first stage.");
            //Here verifies whether the recycled-object is part of the replica's state. If its distinguished name contains the domain NC, we could tell it is in domain replica. 
            Site.CaptureRequirementIfIsTrue(testUser0DnAfterRecycle.ToLower(CultureInfo.InvariantCulture).Contains(adLdapModelAdapter.rootDomainNC.ToLower(CultureInfo.InvariantCulture)), 4181, "[When the Recycle Bin optional feature is enabled, in the second stage] A recycled-object is a special object, part of a replica's state.");
            if (adLdapModelAdapter.currentWorkingDC.OSVersion >= ServerVersion.Win2008R2)
            {
                Site.CaptureRequirementIfIsTrue(isDelete && isRecycle, 4236, "When LDAP_SERVER_SHOW_RECYCLED_OID is used, the recycled-objects are visible.");
            }

            #endregion

            #endregion

            #region Delete testUser1 Stage 2 - recycle bin

            #region Delete testUser1 which is in tombstone

            adLdapModelAdapter.DeleteOperation(
                testUser1Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUser1 with LDAP_SERVER_SHOW_RECYCLED_OID and LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUser1Dn,
                ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID + ";" + ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID);
            Site.Assert.IsNotNull(resultEntry, "Search operation should be successful.");

            isDelete = false;
            isRecycle = false;
            partialAttributeList = resultEntry.attributes;
            attributeListElement = partialAttributeList.Elements;
            setOfAttributeValue = new Asn1SetOf<AttributeValue>();
            attributeValue = new AttributeValue();
            for (int i = 0; i < attributeListElement.Length; i++)
            {
                string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                setOfAttributeValue = attributeListElement[i].vals;
                attributeValue = setOfAttributeValue.Elements[0];
                if (title == "isDeleted")
                {
                    isDelete = Convert.ToBoolean(Encoding.ASCII.GetString(attributeValue.ByteArrayValue).ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                }
                if (title == "isRecycled")
                {
                    isRecycle = Convert.ToBoolean(Encoding.ASCII.GetString(attributeValue.ByteArrayValue).ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                }
            }
            Site.CaptureRequirementIfIsTrue(isDelete && isRecycle, 4383, "[Delete Operation]If the Recycle Bin optional feature is enabled and the requester has specified a deleted-object as the object to be transformed, the operation results in transformation of a deleted-object in the directory tree into a recycled-object.");
            // The delete operation is performed in writable NC replica in adapter.
            Site.CaptureRequirementIfIsTrue(isDelete && isRecycle, 404185, "Note that this transformation from deleted-object to recycled-object in a writable NC replica.");

            #endregion

            #region Delete testUser1 after it is recycled

            // Delete object testUser1Dn
            adLdapModelAdapter.DeleteOperation(
                testUser1Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.UnwillingToPerform_ERROR_DS_CANT_DELETE, delErrorStatus, 4440, "[constraints are enforced for the delete operation]If the object being deleted is a recycled-object, then unwillingToPerform / ERROR_DS_CANT_DELETE is returned.");

            #endregion

            #endregion

            #region Try to disable recycle bin

            if (Utilities.IsOptionalFeatureEnabled(adLdapModelAdapter.forestScopePartialDN + ',' + adLdapModelAdapter.configurationNC, adLdapModelAdapter.recycleBinPartialDN + ',' + adLdapModelAdapter.configurationNC))
            {
                uint flags = uint.Parse(Utilities.GetAttributeFromEntry(
                    adLdapModelAdapter.recycleBinPartialDN + ',' + adLdapModelAdapter.configurationNC,
                    "msDS-OptionalFeatureFlags",
                    adLdapModelAdapter.PDCNetbiosName + "." + adLdapModelAdapter.PrimaryDomainDnsName,
                    adLdapModelAdapter.ADDSPortNum).ToString());

                Site.Log.Add(LogEntryKind.Debug, "Whether an optional feature can be disabled is specified in the msDS-OptionalFeatureFlags attribute on the object representing the optional feature. If the feature can be disabled, the attribute contains the bit flag DISABLABLE_OPTIONAL_FEATURE. Absence of this flag means that the feature cannot be disabled once it has been enabled. Flags: {0}", (OptionalFeature)flags);
                Site.CaptureRequirementIfAreNotEqual(OptionalFeature.DISABLABLE_OPTIONAL_FEATURE, (OptionalFeature)flags & OptionalFeature.DISABLABLE_OPTIONAL_FEATURE, 4499, "[Recycle Bin Optional Feature]The Recycle Bin optional feature cannot be disabled once it is enabled.");

                modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
                // Disable recycle bin optional feature
                adLdapModelAdapter.ModifyRecycleBin(
                    modAttrValMap,
                    RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    ADImplementations.AD_DS,
                    ServerVersion.Win2008R2,
                    false,
                    out modErrorStatus);
                Site.CaptureRequirementIfAreNotEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, 4195, "disableOptionalFeature rootDSE attribute is supported by Windows Server 2008 R2 AD DS and Windows Server 2008 R2 AD LDS.");
                Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED, modErrorStatus, 4219, "[In disableOptionalFeature]If the feature is not marked as being disablable, the server will return the LDAP error unwillingToPerform.");
            }

            #endregion
        }

        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("SDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_DisableOptionalFeatureWithInvalidParameter()
        {
            Site.Assume.IsTrue(adLdapModelAdapter.RecycleBinEnabled, "This test case requires recycle bin to be enabled. Environment will be broken because if once recycle bin is enabled, it cannot be disabled.");

            #region Connect and Bind

            adLdapModelAdapter.SetConnectAndBind(ADImplementations.AD_DS, adLdapModelAdapter.PDCNetbiosName);

            #endregion

            #region Enable recycle bin if not enabled

            if (!Utilities.IsOptionalFeatureEnabled(adLdapModelAdapter.forestScopePartialDN + ',' + adLdapModelAdapter.configurationNC, adLdapModelAdapter.recycleBinPartialDN + ',' + adLdapModelAdapter.configurationNC))
            {
                modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("enableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
                adLdapModelAdapter.ModifyRecycleBin(
                    modAttrValMap,
                    RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    ADImplementations.AD_DS,
                    ServerVersion.Win2008R2,
                    false,
                    out modErrorStatus);
                Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");
            }

            #endregion

            #region Try to disable recycle bin

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
            // The specified scope is forest-wide and this operation is not performed against the DC that holds the Partition Naming Master role
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED, modErrorStatus, 4220, "[In disableOptionalFeature]If the specified optional feature is not already enabled in the specified scope, the server will return the LDAP error noSuchAttribute.");

            #endregion

            #region Try to disable recycle bin with invalid GUID

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableOptionalFeature: invalidGUID", new Sequence<string>().Add("distinguishedName: null")));
            // Disable recycle bin with invalid GUID
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.OperationsError_ERROR_DS_OBJ_NOT_FOUND, modErrorStatus, 4215, "[In disableOptionalFeature]If the server does not recognize the GUID as identifying a known feature, the server will return the LDAP error operationsError.");

            #endregion

            #region Try to disable recycle bin with invalid DN

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableOptionalFeature: invalidDN", new Sequence<string>().Add("distinguishedName: null")));
            // The server does not recognize the DN as belonging to that of an object that represents a scope
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED, modErrorStatus, 4216, "[In disableOptionalFeature]If the DN represents an existing object but the object does not represent a scope, the server will return the error unwillingToPerform / ERROR_DS_NOT_SUPPORTED.");

            #endregion

            #region Try to disable recycle bin with invalid scope

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableOptionalFeature: invalidScope", new Sequence<string>().Add("distinguishedName: null")));
            // The feature is not marked as being valid for the specified scope
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED, modErrorStatus, 4217, "[In disableOptionalFeature]If the feature is not marked as being valid for the specified scope, the server will return the LDAP error unwillingToPerform.");

            #endregion

            #region Try to disable recycle bin against invalid DC

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableOptionalFeature: DCNotPNM", new Sequence<string>().Add("distinguishedName: null")));
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED, modErrorStatus, 4218, "[In disableOptionalFeature]If the specified scope is forest-wide, and this operation is not performed against the DC that holds the Domain Naming Master role, the server will return the LDAP error unwillingToPerform.");

            #endregion
        }

        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_DeleteOperationWithRecycleBinDisabled()
        {
            Site.Assume.IsFalse(adLdapModelAdapter.RecycleBinEnabled, "This test case requires recycle bin to be disabled. Environment will be broken because if once recycle bin is enabled, it cannot be disabled.");

            #region Local Variables

            string testUser0Dn = adLdapModelAdapter.testUser0DNForDs;
            string testUser1Dn = adLdapModelAdapter.testUser1DNForDs;
            string testUser2Dn = adLdapModelAdapter.testUser2DNForDs;
            string testGroup0Dn = adLdapModelAdapter.testUserGroup0DNForDs;

            // attributes on deleted objects
            byte[] sidByte = null;

            #endregion

            #region Add object testUser0

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser0Dn).Add("objectClass: user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add container object testUser1 (not SAM-specific)

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser1Dn).Add("objectClass: container");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add SAM-specific object testUser2Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser2Dn).Add("objectClass: samDomain;user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add object testGroup0Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testGroup0Dn).Add("objectClass: group").Add("member: " + testUser0Dn);
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Delete testUser0 Stage 1 - tombstone

            #region Get GUID and Sid for testUser0 before delete

            var attributes = Utilities.GetAttributesFromEntry(
                testUser0Dn,
                new string[] { "objectGUID", "objectSid" },
                adLdapModelAdapter.PDCNetbiosName + "." + adLdapModelAdapter.PrimaryDomainDnsName,
                adLdapModelAdapter.ADDSPortNum);

            Guid testUser0GuidBeforeDelete = new Guid((byte[])attributes["objectGUID"]);
            SecurityIdentifier testUser0SidBeforeDelete = new SecurityIdentifier(Utilities.ConvertSidToStringSid((byte[])attributes["objectSid"]));

            #endregion

            #region Delete object testUser0

            adLdapModelAdapter.DeleteOperation(
                testUser0Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUser0 with LDAP_SERVER_SHOW_DELETED_OID control

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUser0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID);
            // When we search a deleted-object using LDAP_SERVER_SHOW_DELETED_OID and the response is not null, then this requirement is verified.
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4232, "When LDAP_SERVER_SHOW_DELETED_OID is used, the tombstones are visible.");
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4259, "When LDAP_SERVER_SHOW_DELETED_OID control is used, the tombstones are visible.");

            attributeList = resultEntry.attributes;
            attributeList_element = attributeList.Elements;
            mvale = new Asn1SetOf<AttributeValue>();
            attributevalue = new AttributeValue();
            for (int i = 0; i < attributeList_element.Length; i++)
            {
                string title = Encoding.ASCII.GetString(attributeList_element[i].type.ByteArrayValue);
                mvale = attributeList_element[i].vals;
                attributevalue = mvale.Elements[0];
                if (title == "objectSid")
                {
                    sidByte = attributevalue.ByteArrayValue as byte[];
                }
            }
            string testUser0DnAfterDelete = Encoding.ASCII.GetString(resultEntry.objectName.ByteArrayValue);
            SecurityIdentifier testUser0SidAfterDelete = new SecurityIdentifier(sidByte, 0);
            Site.CaptureRequirementIfAreEqual(testUser0SidAfterDelete, testUser0SidBeforeDelete, 890, "During the Delete operation, the tombstone retains the objectGUID and objectSid (if any) attributes of the original object.");

            var member = Utilities.GetAttributeFromEntry(
                testGroup0Dn,
                "member",
                adLdapModelAdapter.PDCNetbiosName + "." + adLdapModelAdapter.PrimaryDomainDnsName,
                adLdapModelAdapter.ADDSPortNum);
            Site.CaptureRequirementIfIsNull(member, 59, "When an object o is transformed into a tombstone, any forward link reference to object o is removed from the attribute that contains it.");

            #endregion

            #endregion

            #region Delete testUser1 Stage 1 - tombstone

            #region Delete object testUser1Dn

            adLdapModelAdapter.DeleteOperation(
                testUser1Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUser1Dn with LDAP_SERVER_SHOW_RECYCLED_OID control and PR searchFlag (fPRESERVEONDELETE) set

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUser1Dn,
                ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID);
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4235, "When LDAP_SERVER_SHOW_RECYCLED_OID is used, the tombstones are visible.");
            Site.CaptureRequirementIfIsNotNull(resultEntry, 4262, "When LDAP_SERVER_SHOW_RECYCLED_OID control is used, the tombstones are visible.");

            bool isAttributeWithPRExist = false;
            attributeList = resultEntry.attributes;
            attributeList_element = attributeList.Elements;
            mvale = new Asn1SetOf<AttributeValue>();
            attributevalue = new AttributeValue();
            for (int i = 0; i < attributeList_element.Length; i++)
            {
                string title = Encoding.ASCII.GetString(attributeList_element[i].type.ByteArrayValue);
                mvale = attributeList_element[i].vals;
                attributevalue = mvale.Elements[0];
                if ((title == "instanceType"))
                {
                    isAttributeWithPRExist = true;
                }
            }
            string testUser1DnAfterDelete = Encoding.ASCII.GetString(resultEntry.objectName.ByteArrayValue);
            Site.CaptureRequirementIfIsTrue(isAttributeWithPRExist, 4, "If the search flag PR (fPRESERVEONDELETE) is set, it specifies that the attribute must be preserved on objects after deletion of the object (that is, when the object is transformed to a tombstone, deleted-object, or recycled-object).");

            #endregion

            #region Delete testUser1 which is in tombstone

            adLdapModelAdapter.DeleteOperation(
                testUser1Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.UnwillingToPerform_ERROR_DS_CANT_DELETE, delErrorStatus, 4438, "[constraints are enforced for the delete operation]Deletions of tombstone objects fail with unwillingToPerform / ERROR_DS_CANT_DELETE if the DC functional level is DS_BEHAVIOR_WIN2008R2 or higher.");

            #endregion

            #endregion

            #region Delete testUser2 Stage 1 - tombstone

            #region Delete object testUser2Dn

            adLdapModelAdapter.DeleteOperation(
                testUser2Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Delete SAM-specific testUser2 which is in tombstone

            adLdapModelAdapter.DeleteOperation(
                testUser2Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.NoSuchObject, delErrorStatus, 4439, "[constraints are enforced for the delete operation]However, if the object is a tombstone of a SAM-specific object (section 3.1.1.5.2.3), then noSuchObject is returned instead.");

            #endregion

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_DeleteOperationWithRecycleBinEnabledOnNCRoot()
        {
            Site.Assume.IsTrue(adLdapModelAdapter.RecycleBinEnabled, "This test case requires recycle bin to be enabled. Environment will be broken because if once recycle bin is enabled, it cannot be disabled.");

            #region local Variables

            string quotaContainerDn = adLdapModelAdapter.quotaContainerDNForDs;
            string rootNcDn = adLdapModelAdapter.rootDomainNC;
            string treeDeleteObject = adLdapModelAdapter.testUserTreeRoot0DNForDs;
            string leafDeleteObject = adLdapModelAdapter.testUserTreeLeaf0DNForDs;

            #endregion

            #region Connect and Bind

            adLdapModelAdapter.SetConnectAndBind(ADImplementations.AD_DS, adLdapModelAdapter.PDCNetbiosName);

            #endregion

            #region Enable recycle bin if not enabled

            if (!Utilities.IsOptionalFeatureEnabled(adLdapModelAdapter.forestScopePartialDN + ',' + adLdapModelAdapter.configurationNC, adLdapModelAdapter.recycleBinPartialDN + ',' + adLdapModelAdapter.configurationNC))
            {
                modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("enableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
                adLdapModelAdapter.ModifyRecycleBin(
                    modAttrValMap,
                    RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    ADImplementations.AD_DS,
                    ServerVersion.Win2008R2,
                    false,
                    out modErrorStatus);
                Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");
            }

            #endregion

            #region Delete object quotaContainerDn

            adLdapModelAdapter.DeleteOperation(
                quotaContainerDn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.CaptureRequirementIfAreNotEqual(ConstrOnDelOpErr.success, delErrorStatus, 4459, "[When the delete operation results in the transformation of an object into a recycled-object]The object is moved into the Deleted Objects container in its NC, except in the following scenarios, when it MUST remain in its current place: 1. The object is an NC root. 2. The object's systemFlags value has the FLAG_DISALLOW_MOVE_ON_DELETE bit set.");

            #endregion

            #region Delete object RootNC

            adLdapModelAdapter.DeleteOperation(
                rootNcDn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.CaptureRequirementIfAreNotEqual(ConstrOnDelOpErr.success, delErrorStatus, 4449, "The object is moved into the Deleted Objects container in its NC, except in the following scenarios, when it MUST remain in its current place: 1. The object is an NC root. 2. The object's systemFlags value has FLAG_DISALLOW_MOVE_ON_DELETE bit set.");
            Site.CaptureRequirementIfAreNotEqual(ConstrOnDelOpErr.success, delErrorStatus, 4459, "[When the delete operation results in the transformation of an object into a recycled-object]The object is moved into the Deleted Objects container in its NC, except in the following scenarios, when it MUST remain in its current place: 1. The object is an NC root. 2. The object's systemFlags value has the FLAG_DISALLOW_MOVE_ON_DELETE bit set.");

            #endregion

            #region Add tree to be deleted

            addAttrValSeq = new Sequence<string>()
                .Add("distinguishedName: " + treeDeleteObject)
                .Add("objectClass: container");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add leaf to be deleted

            addAttrValSeq = new Sequence<string>()
                .Add("distinguishedName: " + leafDeleteObject)
                .Add("objectClass: user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Delete tree object Stage 1 - tombstone

            #region Delete tree object

            adLdapModelAdapter.TreeDeleteOperation(
                treeDeleteObject,
                RightsOnObjects.RIGHT_DS_DELETE_TREE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be success.");

            #endregion

            #region Delete tree object which is in tombstone

            adLdapModelAdapter.TreeDeleteOperation(
                treeDeleteObject,
                RightsOnObjects.RIGHT_DS_DELETE_TREE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.NoSuchObject, delErrorStatus, "[constraints are enforced for the delete operation]However, if the object is a tombstone of a SAM-specific object (section 3.1.1.5.2.3), then noSuchObject is returned instead.");

            #endregion

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_EnableOptionalFeatureWithInvalidParameter()
        {
            Site.Log.Add(LogEntryKind.Debug, "Environment will be broken because if once recycle bin is enabled, it cannot be disabled.");

            #region Connect and Bind

            adLdapModelAdapter.SetConnectAndBind(ADImplementations.AD_DS, adLdapModelAdapter.PDCNetbiosName);

            #endregion

            #region Try to enable recycle bin with invalid GUID

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableOptionalFeature: invalidGUID", new Sequence<string>().Add("distinguishedName: null")));
            // Enable recycle bin bin with invalid GUID
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.OperationsError_ERROR_DS_OBJ_NOT_FOUND, modErrorStatus, 4224, "[In enableOptionalFeature]If the server does not recognize the GUID as identifying a known feature, the server will return the LDAP error invalidParameter.");

            #endregion

            #region Try to enable recycle bin with invalid DN

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableOptionalFeature: invalidDN", new Sequence<string>().Add("distinguishedName: null")));
            // The server does not recognize the DN as belonging to that of an object that represents a scope
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED, modErrorStatus, 4225, "[In enableOptionalFeature]If the server does not recognize the DN as belonging to that of an object that represents a scope, the server will return the LDAP error invalidParameter.");

            #endregion

            #region Try to enable recycle bin with invalid scope

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableOptionalFeature: invalidScope", new Sequence<string>().Add("distinguishedName: null")));
            // The feature is not marked as being valid for the specified scope
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED, modErrorStatus, 4226, "[In enableOptionalFeature]If the feature is not marked as being valid for the specified scope, the server will return the LDAP error unwillingToPerform.");

            #endregion

            #region Try to enable recycle bin with invalid DC

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("disableOptionalFeature: DCNotPNM", new Sequence<string>().Add("distinguishedName: null")));
            // The specified scope is forest-wide and this operation is not performed against the DC that holds the Partition Naming Master role
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_NOT_SUPPORTED, modErrorStatus, 4227, "[In enableOptionalFeature]If the specified scope is forest-wide and this operation is not performed against the DC that holds the Domain Naming Master role, the server will return the LDAP error unwillingToPerform.");

            #endregion

            #region Enable recycle bin if not enabled

            if (!Utilities.IsOptionalFeatureEnabled(adLdapModelAdapter.forestScopePartialDN + ',' + adLdapModelAdapter.configurationNC, adLdapModelAdapter.recycleBinPartialDN))
            {
                modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("enableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
                // Enable recycle bin
                adLdapModelAdapter.ModifyRecycleBin(
                    modAttrValMap,
                    RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    ADImplementations.AD_DS,
                    ServerVersion.Win2008R2,
                    false,
                    out modErrorStatus);
                Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");
            }

            #endregion

            #region Try to enable recycle bin again expect unsuccessfully when recycle already enabled

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("enableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
            // The specified optional feature is already enabled in the specified scope
            adLdapModelAdapter.ModifyRecycleBin(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS, modErrorStatus, 4228, "[In enableOptionalFeature]If the specified optional feature is already enabled in the specified scope, the server will return the LDAP error attributeOrValueExists.");

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_UndeleteOperationRecycleBin()
        {
            Site.Log.Add(LogEntryKind.Debug, "Environment will be broken because if once recycle bin is enabled, it cannot be disabled.");

            #region LocalVariables

            string testComputer0Dn = adLdapModelAdapter.testComputer0DNForDs;

            #endregion

            #region Add object testComputer0

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + adLdapModelAdapter.testComputer0DNForDs).Add("objectClass: computer");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Enable recycle bin if not enabled

            if (!Utilities.IsOptionalFeatureEnabled(adLdapModelAdapter.forestScopePartialDN + ',' + adLdapModelAdapter.configurationNC, adLdapModelAdapter.recycleBinPartialDN))
            {
                #region Try to restore undeleted object with recycle bin disabled

                adLdapModelAdapter.RestoreDeletedObject(
                    testComputer0Dn,
                    testComputer0Dn,
                    RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    ADImplementations.AD_DS,
                    ServerVersion.Win2008R2,
                    false,
                    out modErrorStatus);
                Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ, modErrorStatus, 4368, "[During undelete operation, if the Recycle Bin optional feature is not enabled] if the DC functional level is DS_BEHAVIOR_WIN2008R2, the error returned is noSuchAttribute/ERROR_DS_ATT_IS_NOT_ON_OBJ.");

                #endregion

                modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("enableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
                // Enable recycle bin
                adLdapModelAdapter.ModifyRecycleBin(
                    modAttrValMap,
                    RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    ADImplementations.AD_DS,
                    ServerVersion.Win2008R2,
                    false,
                    out modErrorStatus);
                Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");
            }

            #endregion

            #region Try to restore undeleted object with recycle bin enabled

            adLdapModelAdapter.RestoreDeletedObject(
                testComputer0Dn,
                testComputer0Dn,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ, modErrorStatus, 4370, "[During undelete operation, if the Recycle Bin optional feature is enabled] if the DC functional level is DS_BEHAVIOR_WIN2008R2, the error returned is noSuchAttribute/ERROR_DS_ATT_IS_NOT_ON_OBJ.");

            #endregion

            #region Delete object testComputer0

            string testComputer0DnAfterDelete = Utilities.GetDeletedObjectDN(
                testComputer0Dn,
                adLdapModelAdapter.deletedObjectsContainerDNForDs,
                adLdapModelAdapter.currentWorkingDC.FQDN,
                adLdapModelAdapter.currentPort);

            adLdapModelAdapter.DeleteOperation(
                testComputer0Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Try to restore deleted object with recycle bin enabled

            adLdapModelAdapter.RestoreDeletedObject(
                testComputer0DnAfterDelete,
                testComputer0Dn,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, 4605, "During the modify operation, Modifying an object with isDeleted = true (a tombstone) is allowed if the following conditions is true: the Recycle Bin optional feature is enabled, the object does not have isRecycled = true, and the operation is an undelete operation. Note that the undelete operation is a special case of the modify operation. See section 3.1.1.8.1 for more information on the Recycle Bin optional feature. See section 3.1.1.5.3.7 for more information on the undelete operation.");
            // We verify undelete operation part here.
            Site.CaptureRequirementIfAreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, 710, "During the modify operation, System-only attribute modifications (including the case of adding an auxiliary class with a must-have system-only attribute) are disallowed, as well as modifications of all back link attributes; with the following exceptions:" +
                                "->If the DC functional level is DS_BEHAVIOUR_WIN2003 or greater, then modifications of the objectClass attribute are permitted, subject to additional constraints (section 3.1.1.5.3.5)." +
                                "->If the DC functional level is DS_BEHAVIOUR_WIN2003 or greater, then modifications of msDS-Behavior-Version are permitted, subject to additional constraints (section 3.1.1.5.3.4)." +
                                "->Modifications of msDS-AdditionalDnsHostName are permitted." +
                                "->Modifications of systemFlags are permitted only in the following case: the modify is on an attributeSchema object in the schema container, and the change is to set (but not reset) the FLAG_ATTR_IS_RDN bit." +
                                "-> Modifications of wellKnownObjects are permitted, subject to additional constraints. See section 3.1.1.5.3.6, wellKnownObjects Updates, for more information." +
                                "-> Modifications of isDeleted and distinguishedName are permitted only when the modify operation is Undelete (section 3.1.1.5.3.7)." +
                                "-> Modifications of mAPID are permitted, subject to the constraints described in section 3.1.1.2.3.");

            #endregion

            #region Clean up by deleting the object testComputer0

            adLdapModelAdapter.DeleteOperation(
                testComputer0Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null, ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_UpdateWellKnownObject()
        {
            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_DS,
                adLdapModelAdapter.PDCNetbiosName);

            ///This function is used to verify R4365 and R4367.

            #region possSuperiors schema constraint

            ///If the DC functional level is DS_BEHAVIOR_WIN2008 or greater, then the object named by the new value must 
            ///Satisfy the possSuperiors schema constraint for the objectClass corresponding to the WKO reference being updated.
            ///If this constraint is not satisfied, the server returns unwillingToPerform / ERROR_DS_ILLEGAL_SUPERIOR. 
            adLdapModelAdapter.ModifyWellKnownObject(
                "wellKnowObject: invalidNewValue",
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            if (modErrorStatus == ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_SUPERIOR)
            {
                Site.CaptureRequirement(4365, "[In AD/DS, when a wellKnownObjects value is modified by an originating update]If this constraint[the possSuperiors schema constraint for the objectClass corresponding to the WKO reference being updated] is not satisfied, the server returns unwillingToPerform / ERROR_DS_ILLEGAL_SUPERIOR.");
            }

            #endregion

            #region RemovedValueMatchingTest

            ///The removed value matches the corresponding existing value of the WKO reference. If not, then unwillingToPerform / 
            ///ERROR_DS_UNWILLING_TO_PERFORM is returned. 
            adLdapModelAdapter.ModifyWellKnownObject(
                "wellKnowObject: invalidRemoveValue",
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            if (modErrorStatus == ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_UNWILLING_TO_PERFORM)
            {
                Site.CaptureRequirement(4367, "[In AD/DS, when a wellKnownObjects value is modified by an originating update] If not [The removed value does not match the corresponding existing value of the WKO reference], then unwillingToPerform / ERROR_DS_UNWILLING_TO_PERFORM is returned.");
            }

            #endregion
        }

        [Ignore]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_AD_DS_Add_NC_Success_CrossRef_Exists()
        {
            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_DS,
                adLdapModelAdapter.PDCNetbiosName);

            string crossRefDN = "cn=testAppNC,cn=partitions," + adLdapModelAdapter.configurationNC;
            string appNC = "DC=testAppNC," + adLdapModelAdapter.rootDomainNC;

            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "delete item crossRef object in case it exists: " + crossRefDN);
                AdLdapClient.Instance().DeleteObject(crossRefDN, null);
            }
            catch
            {
            }

            List<DirectoryAttribute> attrs = new List<DirectoryAttribute>();
            attrs.Add(new DirectoryAttribute("systemflags:5"));
            attrs.Add(new DirectoryAttribute("dnsroot:" + adLdapModelAdapter.PDCNetbiosName + "." + adLdapModelAdapter.PrimaryDomainDnsName));
            attrs.Add(new DirectoryAttribute("ncname:" + appNC));
            attrs.Add(new DirectoryAttribute("enabled:FALSE"));
            attrs.Add(new DirectoryAttribute("objectclass:crossref"));
            string err = AdLdapClient.Instance().AddObject(crossRefDN, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("Success_STATUS_SUCCESS", err, "crossRef {0} should be successfully ", crossRefDN);
            attrs.Clear();
            attrs.Add(new DirectoryAttribute("instancetype:5"));
            attrs.Add(new DirectoryAttribute("objectclass:domainDNS"));
            err = AdLdapClient.Instance().AddObject(appNC, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("Success_STATUS_SUCCESS", err, "add request should return Success_STATUS_SUCCESS");
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Add_NC_Failed_CrossRef_Exists_Enabled()
        {
            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_DS,
                adLdapModelAdapter.PDCNetbiosName);

            string crossRefDN = "cn=testAppNC,cn=partitions," + adLdapModelAdapter.configurationNC;
            string appNC = "DC=testAppNC," + adLdapModelAdapter.rootDomainNC;

            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "delete item crossRef object in case it exists: " + crossRefDN);
                AdLdapClient.Instance().DeleteObject(crossRefDN, null);
            }
            catch
            {
            }

            List<DirectoryAttribute> attrs = new List<DirectoryAttribute>();
            attrs.Add(new DirectoryAttribute("systemflags:5"));
            attrs.Add(new DirectoryAttribute("dnsroot:" + adLdapModelAdapter.PDCNetbiosName + "." + adLdapModelAdapter.PrimaryDomainDnsName));
            attrs.Add(new DirectoryAttribute("ncname:" + appNC));
            attrs.Add(new DirectoryAttribute("enabled:TRUE"));
            attrs.Add(new DirectoryAttribute("objectclass:crossref"));
            string err = AdLdapClient.Instance().AddObject(crossRefDN, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("Success_STATUS_SUCCESS", err, "crossRef {0} should be successfully ", crossRefDN);
            attrs.Clear();
            attrs.Add(new DirectoryAttribute("instancetype:5"));
            attrs.Add(new DirectoryAttribute("objectclass:domainDNS"));
            err = AdLdapClient.Instance().AddObject(appNC, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("EntryAlreadyExists_UnKnownError", err, "add request should return EntryAlreadyExists_UnKnownError when crossRef object is Enabled");
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Add_NC_Failed_CrossRef_Exists_Invalid_DNSRoot()
        {
            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_DS,
                adLdapModelAdapter.PDCNetbiosName);

            string crossRefDN = "cn=testAppNC,cn=partitions," + adLdapModelAdapter.configurationNC;
            string appNC = "DC=testAppNC," + adLdapModelAdapter.rootDomainNC;

            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "delete item crossRef object in case it exists: " + crossRefDN);
                AdLdapClient.Instance().DeleteObject(crossRefDN, null);
            }
            catch
            {
            }

            List<DirectoryAttribute> attrs = new List<DirectoryAttribute>();
            attrs.Add(new DirectoryAttribute("systemflags:5"));
            attrs.Add(new DirectoryAttribute("dnsroot:badDNS"));
            attrs.Add(new DirectoryAttribute("ncname:" + appNC));
            attrs.Add(new DirectoryAttribute("enabled:FALSE"));
            attrs.Add(new DirectoryAttribute("objectclass:crossref"));
            string err = AdLdapClient.Instance().AddObject(crossRefDN, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("Success_STATUS_SUCCESS", err, "crossRef {0} should be successfully ", crossRefDN);
            attrs.Clear();
            attrs.Add(new DirectoryAttribute("instancetype:5"));
            attrs.Add(new DirectoryAttribute("objectclass:domainDNS"));
            err = AdLdapClient.Instance().AddObject(appNC, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("UnwillingToPerform_ERROR_DS_MASTERDSA_REQUIRED", err, "add request should return UnwillingToPerform_ERROR_DS_MASTERDSA_REQUIRED when crossRef object has invalid dns root");
        }

        [Ignore]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_AD_LDS_Add_NC_Success_CrossRef_Exists()
        {
            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_LDS,
                adLdapModelAdapter.PDCNetbiosName);

            string crossRefDN = "cn=testAppNC,cn=partitions," + adLdapModelAdapter.configurationNC;
            string appNC = "DC=testAppNC," + adLdapModelAdapter.rootDomainNC;

            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "delete item crossRef object in case it exists: " + crossRefDN);
                AdLdapClient.Instance().DeleteObject(crossRefDN, null);
            }
            catch
            {
            }

            List<DirectoryAttribute> attrs = new List<DirectoryAttribute>();
            attrs.Add(new DirectoryAttribute("systemflags", "5"));
            attrs.Add(new DirectoryAttribute("dnsroot", adLdapModelAdapter.PDCNetbiosName + "." + adLdapModelAdapter.PrimaryDomainDnsName + ":" + adLdapModelAdapter.ADLDSPortNum + ":" + adLdapModelAdapter.ADLDSSSLPortNum));
            attrs.Add(new DirectoryAttribute("ncname", appNC));
            attrs.Add(new DirectoryAttribute("enabled", "FALSE"));
            attrs.Add(new DirectoryAttribute("objectclass", "crossref"));
            string err = AdLdapClient.Instance().AddObject(crossRefDN, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("Success_STATUS_SUCCESS", err, "crossRef {0} should be successfully ", crossRefDN);
            attrs.Clear();
            attrs.Add(new DirectoryAttribute("instancetype", "5"));
            attrs.Add(new DirectoryAttribute("objectclass", "domainDNS"));
            err = AdLdapClient.Instance().AddObject(appNC, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("Success_STATUS_SUCCESS", err, "add request should return Success_STATUS_SUCCESS");
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Modify_DNSHostName()
        {
            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_DS,
                adLdapModelAdapter.PDCNetbiosName);

            // Recreate TestComputer1 computer account object.
            // It was deleted in the TestCleanup method.
            // It should be recreated, because it will be used further in this case.
            adLdapModelAdapter.NewComputer(adLdapModelAdapter.testComputer1Name);

            string dn = "cn=" + adLdapModelAdapter.testComputer1Name + ",cn=computers," + adLdapModelAdapter.rootDomainNC;

            Utilities.SetAccessRights(
                dn,
                adLdapModelAdapter.testUserName,
                adLdapModelAdapter.currentWorkingDC.Domain.NetbiosName,
                ActiveDirectoryRights.WriteProperty,
                AccessControlType.Allow);
            Utilities.RemoveControlAcessRights(
                dn,
                adLdapModelAdapter.testUserName,
                adLdapModelAdapter.currentWorkingDC.Domain.NetbiosName,
                ValidatedWrite.Validated_DNS_Host_Name,
                ActiveDirectoryRights.Self,
                AccessControlType.Deny);
            Utilities.SetControlAcessRights(
                dn,
                adLdapModelAdapter.testUserName,
                adLdapModelAdapter.currentWorkingDC.Domain.NetbiosName,
                ValidatedWrite.Validated_DNS_Host_Name,
                ActiveDirectoryRights.Self,
                AccessControlType.Allow);

            DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}:{1}/{2}",
                adLdapModelAdapter.currentWorkingDC.NetbiosName,
                adLdapModelAdapter.currentPort,
                dn),
                adLdapModelAdapter.testUserName,
                 adLdapModelAdapter.testUserPwd);

            string testDNS = adLdapModelAdapter.testComputer1Name + "." + adLdapModelAdapter.PrimaryDomainDnsName;
            entry.Properties["dnshostname"].Value = testDNS;
            entry.CommitChanges();
            entry.RefreshCache();
            BaseTestSite.Assert.Pass("Should success when requestor {0} has Validated-DNS-Host-Name Extended Right on computer object {1}", adLdapModelAdapter.testUserName, dn);

            Utilities.RemoveControlAcessRights(
                dn,
                adLdapModelAdapter.testUserName,
                adLdapModelAdapter.currentWorkingDC.Domain.NetbiosName,
                ValidatedWrite.Validated_DNS_Host_Name,
                ActiveDirectoryRights.Self,
                AccessControlType.Allow);
            Utilities.SetControlAcessRights(
                dn,
                adLdapModelAdapter.testUserName,
                adLdapModelAdapter.currentWorkingDC.Domain.NetbiosName,
                ValidatedWrite.Validated_DNS_Host_Name,
                ActiveDirectoryRights.Self,
                AccessControlType.Deny);

            entry = new DirectoryEntry(string.Format("LDAP://{0}:{1}/{2}",
                adLdapModelAdapter.currentWorkingDC.NetbiosName,
                adLdapModelAdapter.currentPort,
                dn),
                adLdapModelAdapter.testUserName,
                 adLdapModelAdapter.testUserPwd);
            entry.Properties["dnshostname"].Value = testDNS;
            try
            {
                entry.CommitChanges();
                BaseTestSite.Assert.Fail("Should fail when requestor {0} does not have Validated-DNS-Host-Name Extended Right on computer object {1}", adLdapModelAdapter.testUser7Name, dn);
            }
            catch
            {
                BaseTestSite.Assert.Pass("Should fail when requestor {0} does not have Validated-DNS-Host-Name Extended Right on computer object {1}", adLdapModelAdapter.testUser7Name, dn);
            }
        }

        [Ignore]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Manual")]
        public void LDAP_AD_LDS_Add_NC_Failed_CrossRef_Exists_Enabled()
        {
            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_LDS,
                adLdapModelAdapter.PDCNetbiosName);

            string crossRefDN = "cn=testAppNC,cn=partitions," + adLdapModelAdapter.configurationNC;
            string appNC = "DC=testAppNC," + adLdapModelAdapter.rootDomainNC;

            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "delete item crossRef object in case it exists: " + crossRefDN);
                AdLdapClient.Instance().DeleteObject(crossRefDN, null);
            }
            catch
            {
            }

            List<DirectoryAttribute> attrs = new List<DirectoryAttribute>();
            attrs.Add(new DirectoryAttribute("systemflags", "5"));
            attrs.Add(new DirectoryAttribute("dnsroot", adLdapModelAdapter.PDCNetbiosName + "." + adLdapModelAdapter.PrimaryDomainDnsName + ":" + adLdapModelAdapter.ADLDSPortNum + ":" + adLdapModelAdapter.ADLDSSSLPortNum));
            attrs.Add(new DirectoryAttribute("ncname", appNC));
            attrs.Add(new DirectoryAttribute("enabled", "TRUE"));
            attrs.Add(new DirectoryAttribute("objectclass", "crossref"));
            string err = AdLdapClient.Instance().AddObject(crossRefDN, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("Success_STATUS_SUCCESS", err, "crossRef {0} should be successfully ", crossRefDN);
            attrs.Clear();
            attrs.Add(new DirectoryAttribute("instancetype", "5"));
            attrs.Add(new DirectoryAttribute("objectclass", "domainDNS"));
            err = AdLdapClient.Instance().AddObject(appNC, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("EntryAlreadyExists_UnKnownError", err, "add request should return EntryAlreadyExists_UnKnownError when crossRef object is Enabled");
        }

        [Ignore]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Manual")]
        public void LDAP_AD_LDS_Add_NC_Failed_CrossRef_Exists_Invalid_DNSRoot()
        {
            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_LDS,
                adLdapModelAdapter.PDCNetbiosName);

            string crossRefDN = "cn=testAppNC,cn=partitions," + adLdapModelAdapter.configurationNC;
            string appNC = "DC=testAppNC," + adLdapModelAdapter.rootDomainNC;

            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "delete item crossRef object in case it exists: " + crossRefDN);
                AdLdapClient.Instance().DeleteObject(crossRefDN, null);
            }
            catch
            {
            }

            List<DirectoryAttribute> attrs = new List<DirectoryAttribute>();
            attrs.Add(new DirectoryAttribute("systemflags", "5"));
            attrs.Add(new DirectoryAttribute("dnsroot", "badDNS"));
            attrs.Add(new DirectoryAttribute("ncname", appNC));
            attrs.Add(new DirectoryAttribute("enabled", "FALSE"));
            attrs.Add(new DirectoryAttribute("objectclass", "crossref"));
            string err = AdLdapClient.Instance().AddObject(crossRefDN, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("Success_STATUS_SUCCESS", err, "crossRef {0} should be successfully ", crossRefDN);
            attrs.Clear();
            attrs.Add(new DirectoryAttribute("instancetype", "5"));
            attrs.Add(new DirectoryAttribute("objectclass", "domainDNS"));
            err = AdLdapClient.Instance().AddObject(appNC, attrs, null);
            BaseTestSite.Assert.AreEqual<string>("UnwillingToPerform_ERROR_DS_MASTERDSA_REQUIRED", err, "add request should return UnwillingToPerform_ERROR_DS_MASTERDSA_REQUIRED when crossRef object has invalid dns root");
        }

        [Ignore]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Manual")]
        public void LDAP_Add_ComputerObject_NoPrivilege()
        {
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(
                    Site.Properties["WritableDC1.IPAddress"]),
                    new NetworkCredential(
                        adLdapModelAdapter.testUserName,
                        adLdapModelAdapter.testUserPwd,
                        adLdapModelAdapter.PrimaryDomainDnsName));
            ManagedAddRequest req = new ManagedAddRequest("cn=badcomputer,cn=computers," + adLdapModelAdapter.rootDomainNC, "computer");
            bool asExpected = false;
            try
            {
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(req);
            }
            catch (Exception e)
            {
                if (e.Message == "The user has insufficient access rights.")
                    asExpected = true;
            }
            Site.Assert.IsTrue(asExpected, "Normal domain member user should not be allowed to create computer object if it does not have SE_MACHINE_ACCOUNT_NAME privilege");
        }

        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_TestDeactivatedLink()
        {
            Site.Assume.IsTrue(adLdapModelAdapter.RecycleBinEnabled, "This test case requires recycle bin to be enabled. Environment will be broken because if once recycle bin is enabled, it cannot be disabled.");

            #region Local Variables

            string testUser3Dn = adLdapModelAdapter.testUser3DNForDs;
            string testUser4Dn = adLdapModelAdapter.testUser4DNForDs;
            string testUser5Dn = adLdapModelAdapter.testUser5DNForDs;
            string testUserGroup0Dn = adLdapModelAdapter.testUserGroup0DNForDs;

            string member = string.Empty;
            string assistant = string.Empty;
            string memberof = string.Empty;
            DirectoryControl[] controls = null;

            #endregion

            #region Add object testUser3Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser3Dn).Add("objectClass: user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add object testUser4Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser4Dn).Add("objectClass: user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add object testUser5Dn with assistant testUser4Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser5Dn).Add("objectClass: user").Add("assistant:" + testUser4Dn);
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add object testUserGroup0Dn with member testUser3Dn and testUser4Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUserGroup0Dn).Add("objectClass: group").Add("member: " + testUser3Dn).Add("member: " + testUser4Dn);
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Enable recycle bin if not enabled

            if (!Utilities.IsOptionalFeatureEnabled(adLdapModelAdapter.forestScopePartialDN + ',' + adLdapModelAdapter.configurationNC, adLdapModelAdapter.recycleBinPartialDN))
            {
                modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("enableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
                // Enable recycle bin
                adLdapModelAdapter.ModifyRecycleBin(
                    modAttrValMap,
                    RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    ADImplementations.AD_DS,
                    ServerVersion.Win2008R2,
                    false,
                    out modErrorStatus);
                Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");
            }

            #endregion

            #region Get objectDN for testUser4 and testUserGroup0 after they are deleted

            string testUser4DnAfterDelete = Utilities.GetDeletedObjectDN(
                testUser4Dn,
                adLdapModelAdapter.deletedObjectsContainerDNForDs,
                adLdapModelAdapter.currentWorkingDC.FQDN,
                adLdapModelAdapter.currentPort);
            string testUserGroup0DnAfterDelete = Utilities.GetDeletedObjectDN(
                testUserGroup0Dn,
                adLdapModelAdapter.deletedObjectsContainerDNForDs,
                adLdapModelAdapter.currentWorkingDC.FQDN,
                adLdapModelAdapter.currentPort);

            #endregion

            #region Delete object testUser4Dn

            adLdapModelAdapter.DeleteOperation(
                testUser4Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search object testUser5Dn for nonlinked attribute "assistant" testUser4Dn

            AdLdapClient.Instance().SearchObject(
                testUser5Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                null,
                out searchResponse);
            assistant = string.Empty;
            if (searchResponse != null)
            {
                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                    (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                    entrypacket.GetInnerRequestOrResponse();
                    PartialAttributeList pattributeList = entry.attributes;
                    PartialAttributeList_element[] pattributeList_element = pattributeList.Elements;
                    Asn1SetOf<AttributeValue> smvale = new Asn1SetOf<AttributeValue>();
                    for (int i = 0; i < pattributeList_element.Length; i++)
                    {
                        string title = Encoding.ASCII.GetString(pattributeList_element[i].type.ByteArrayValue);
                        smvale = pattributeList_element[i].vals;
                        if (title == "assistant")
                        {
                            for (int j = 0; j < smvale.Elements.Length; j++)
                            {
                                assistant = Encoding.ASCII.GetString(smvale.Elements[j].ByteArrayValue);
                            }
                            break;
                        }
                    }
                }
            }
            Site.CaptureRequirementIfIsTrue(assistant.ToLower(CultureInfo.InvariantCulture).Contains(testUser4DnAfterDelete.ToLower(CultureInfo.InvariantCulture)), 4431, "[Recycled-Object Requirements]The nonlinked references to the object are retained when the deleted-object is transformed into a recycled-object (that is, other objects that referred to the original deleted-object via nonlinked DN references continue pointing to the recycled-object indefinitely after the object is transformed, until these values are removed explicitly).");


            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DELETED_OID

            // Search testUserGroup0Dn using LDAP_SERVER_SHOW_DELETED_OID control
            controls = new DirectoryControl[] { new DirectoryControl(ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, null, true, true) };
            AdLdapClient.Instance().SearchObject(
                testUserGroup0Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                controls,
                out searchResponse);
            member = string.Empty;
            if (searchResponse != null)
            {
                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                    (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                    entrypacket.GetInnerRequestOrResponse();
                    PartialAttributeList partialAttributeList = entry.attributes;
                    PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
                    Asn1SetOf<AttributeValue> setOfAttributevalue = new Asn1SetOf<AttributeValue>();
                    for (int i = 0; i < attributeListElement.Length; i++)
                    {
                        string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                        setOfAttributevalue = attributeListElement[i].vals;
                        if (title == "member")
                        {
                            for (int j = 0; j < setOfAttributevalue.Elements.Length; j++)
                            {
                                member += Encoding.ASCII.GetString(setOfAttributevalue.Elements[j].ByteArrayValue);
                            }
                            break;
                        }
                    }
                }
            }
            // We verify "linked attribute values referring to deleted-objects is not returned" part here. 
            // There is another capture code to verify another part.
            Site.CaptureRequirementIfIsFalse(member.Contains(testUser4Dn), 4241, "When this control[LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID] is not used, linked attribute values referring to deleted-objects and link valued attributes stored on deleted-objects are not visible to search operation filters.");
            Site.CaptureRequirementIfIsFalse(member.Contains(testUser4Dn), 4242, "When this control[LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID] is not used, linked attribute values referring to deleted-objects and link valued attributes stored on deleted-objects are not returned as requested attributes for the search operation.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID

            //Search testUserGroup0Dn using LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control
            controls = new DirectoryControl[] { new DirectoryControl(ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, null, true, true) };
            AdLdapClient.Instance().SearchObject(
                testUserGroup0Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                controls,
                out searchResponse);
            member = string.Empty;
            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
            {
                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                entrypacket.GetInnerRequestOrResponse();
                PartialAttributeList partialAttributeList = entry.attributes;
                PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
                Asn1SetOf<AttributeValue> setOfAttributevalue = new Asn1SetOf<AttributeValue>();
                for (int i = 0; i < attributeListElement.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                    setOfAttributevalue = attributeListElement[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < setOfAttributevalue.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(setOfAttributevalue.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4243, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used, link values neither stored on nor referring to deleted-objects are visible.");
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser4DnAfterDelete.ToLower(CultureInfo.InvariantCulture)), 4244, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used, link values not stored on but referring to deleted-objects are visible.");
            // we verified "any forward link reference to o is maintained" part here. There is another capture code to verify another part.
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser4DnAfterDelete.ToLower(CultureInfo.InvariantCulture)), 4188, "When an object o is transformed into a deleted-object, any forward link reference to o is maintained, but is made invisible to LDAP operations that do not specify the LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control.");
            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_DELETED_OID

            //Search testUserGroup0Dn using LDAP_SERVER_SHOW_DELETED_OID and LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control
            controls = new DirectoryControl[]
            {
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, null, true, true),
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, null, true, true)
            };
            AdLdapClient.Instance().SearchObject(
                testUserGroup0Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                controls,
                out searchResponse);
            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
            {
                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                entrypacket.GetInnerRequestOrResponse();
                PartialAttributeList partialAttributeList = entry.attributes;
                PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
                Asn1SetOf<AttributeValue> setOfAttributeValue = new Asn1SetOf<AttributeValue>();
                for (int i = 0; i < attributeListElement.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                    setOfAttributeValue = attributeListElement[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < setOfAttributeValue.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(setOfAttributeValue.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4247, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used in conjunction with LDAP_SERVER_SHOW_DELETED_OID, link values neither stored on nor referring to deleted-objects are visible.");
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser4DnAfterDelete.ToLower(CultureInfo.InvariantCulture)), 4248, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used in conjunction with LDAP_SERVER_SHOW_DELETED_OID, link values not stored on but referring to deleted-objects are visible.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_RECYCLED_OID

            //Search testUserGroup0Dn using LDAP_SERVER_SHOW_RECYCLED_OID and LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control
            controls = new DirectoryControl[]
            {
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID, null, true, true),
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, null, true, true)
            };
            AdLdapClient.Instance().SearchObject(
                testUserGroup0Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                controls,
                out searchResponse);
            member = string.Empty;
            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
            {
                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                entrypacket.GetInnerRequestOrResponse();
                PartialAttributeList partialAttributeList = entry.attributes;
                PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
                Asn1SetOf<AttributeValue> setOfAttributeValue = new Asn1SetOf<AttributeValue>();
                for (int i = 0; i < attributeListElement.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                    setOfAttributeValue = attributeListElement[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < setOfAttributeValue.Elements.Length; j++)
                        {
                            member = member + Encoding.ASCII.GetString(setOfAttributeValue.Elements[j].ByteArrayValue);
                        }
                    }
                }
            }
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4251, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used in conjunction with LDAP_SERVER_SHOW_RECYCLED_OID, link values neither stored on nor referring to deleted-objects are visible.");
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser4DnAfterDelete.ToLower(CultureInfo.InvariantCulture)), 4252, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used in conjunction with LDAP_SERVER_SHOW_RECYCLED_OID, link values not stored on but referring to deleted-objects are visible.");

            #endregion

            #region Delete object testUserGroup0Dn

            // Delete object testUserGroup0Dn
            adLdapModelAdapter.DeleteOperation(
                testUserGroup0Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DELETED_OID

            // Search testUserGroup0Dn using LDAP_SERVER_SHOW_DELETED_OID control
            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUserGroup0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID);
            member = string.Empty;
            if (resultEntry != null)
            {
                attributeList = resultEntry.attributes;
                attributeList_element = attributeList.Elements;
                mvale = new Asn1SetOf<AttributeValue>();
                for (int i = 0; i < attributeList_element.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeList_element[i].type.ByteArrayValue);
                    mvale = attributeList_element[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < mvale.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(mvale.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }
            // We verify "but is made invisible to LDAP operations that do not specify the LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control" part here. There is another capture code to verify another part.
            Site.CaptureRequirementIfIsTrue(string.IsNullOrEmpty(member), 4188, "When an object o is transformed into a deleted-object, any forward link reference to o is maintained, but is made invisible to LDAP operations that do not specify the LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control.");
            // we verify "link valued attributes stored on deleted-objects are not returned" part here. There is another capture code to verify another part.
            Site.CaptureRequirementIfIsTrue(string.IsNullOrEmpty(member), 4241, "When this control[LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID] is not used, linked attribute values referring to deleted-objects and link valued attributes stored on deleted-objects are not visible to search operation filters.");
            Site.CaptureRequirementIfIsTrue(string.IsNullOrEmpty(member), 4242, "When this control[LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID] is not used, linked attribute values referring to deleted-objects and link valued attributes stored on deleted-objects are not returned as requested attributes for the search operation.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID

            //Search testUser0Dn using LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control
            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUserGroup0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID);
            member = string.Empty;
            if (resultEntry != null)
            {
                attributeList = resultEntry.attributes;
                attributeList_element = attributeList.Elements;
                mvale = new Asn1SetOf<AttributeValue>();
                attributevalue = new AttributeValue();
                for (int i = 0; i < attributeList_element.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeList_element[i].type.ByteArrayValue);
                    mvale = attributeList_element[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < mvale.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(mvale.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsFalse(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4245, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used, link values stored on deleted-objects but not referring to deleted-objects are not visible.");
            Site.CaptureRequirementIfIsFalse(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser4Dn.ToLower(CultureInfo.InvariantCulture)), 4246, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used, link values stored on and referring to deleted-objects are not visible.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_DELETED_OID

            //Search testUser0Dn using LDAP_SERVER_SHOW_DELETED_OID and LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control
            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUserGroup0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID + ";" + ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID);
            member = string.Empty;
            if (resultEntry != null)
            {
                attributeList = resultEntry.attributes;
                attributeList_element = attributeList.Elements;
                mvale = new Asn1SetOf<AttributeValue>();
                attributevalue = new AttributeValue();
                for (int i = 0; i < attributeList_element.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeList_element[i].type.ByteArrayValue);
                    mvale = attributeList_element[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < mvale.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(mvale.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }

            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4249, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used in conjunction with LDAP_SERVER_SHOW_DELETED_OID, link values stored on deleted-objects but not referring to deleted-objects are visible.");
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser4DnAfterDelete.ToLower(CultureInfo.InvariantCulture)), 4250, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used in conjunction with LDAP_SERVER_SHOW_DELETED_OID, link values stored on and referring to deleted-objects are visible.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_RECYCLED_OID

            //Search testUser0Dn using LDAP_SERVER_SHOW_RECYCLED_OID and LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control
            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUserGroup0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID + ";" + ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID);
            member = string.Empty;
            if (resultEntry != null)
            {
                attributeList = resultEntry.attributes;
                attributeList_element = attributeList.Elements;
                mvale = new Asn1SetOf<AttributeValue>();
                attributevalue = new AttributeValue();
                for (int i = 0; i < attributeList_element.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeList_element[i].type.ByteArrayValue);
                    mvale = attributeList_element[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < mvale.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(mvale.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4253, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used in conjunction with LDAP_SERVER_SHOW_RECYCLED_OID, link values stored on deleted-objects but not referring to deleted-objects are visible.");
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser4DnAfterDelete.ToLower(CultureInfo.InvariantCulture)), 4254, "When LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID control is used in conjunction with LDAP_SERVER_SHOW_RECYCLED_OID, link values stored on and referring to deleted-objects are visible.");

            #endregion

            #region Delete object testUser4Dn which is in tombstone

            adLdapModelAdapter.DeleteOperation(
                testUser4Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_RECYCLED_OID

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUserGroup0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID + ";" + ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID);
            member = string.Empty;
            if (resultEntry != null)
            {
                attributeList = resultEntry.attributes;
                attributeList_element = attributeList.Elements;
                mvale = new Asn1SetOf<AttributeValue>();
                attributevalue = new AttributeValue();
                for (int i = 0; i < attributeList_element.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeList_element[i].type.ByteArrayValue);
                    mvale = attributeList_element[i].vals;
                    if ((title == "member"))
                    {
                        for (int j = 0; j < mvale.Elements.Length; j++)
                        {
                            member += Convert.ToString(Encoding.ASCII.GetString(mvale.Elements[j].ByteArrayValue), CultureInfo.InvariantCulture);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsFalse(member.Contains(testUser4DnAfterDelete), 4430, "[Recycled-Object Requirements]NC replicas do not contain linked references to recycled-objects.");
            Site.CaptureRequirementIfIsFalse(member.Contains(testUser4DnAfterDelete), 4187, "When an object o is transformed into a recycled-object, any forward link reference to object o is removed from the attribute that contains it.");

            #endregion

            #region Search testUser4Dn for linked attribute "memberOf" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_RECYCLED_OID

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUser4Dn,
                ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID + ";" + ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID);
            memberof = string.Empty;
            if (resultEntry != null)
            {
                attributeList = resultEntry.attributes;
                attributeList_element = attributeList.Elements;
                mvale = new Asn1SetOf<AttributeValue>();
                attributevalue = new AttributeValue();
                for (int i = 0; i < attributeList_element.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeList_element[i].type.ByteArrayValue);
                    mvale = attributeList_element[i].vals;
                    if ((title == "memberof"))
                    {
                        for (int j = 0; j < mvale.Elements.Length; j++)
                        {
                            memberof += Convert.ToString(Encoding.ASCII.GetString(mvale.Elements[j].ByteArrayValue), CultureInfo.InvariantCulture);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsFalse(memberof.ToLower(CultureInfo.InvariantCulture).Contains(testUserGroup0DnAfterDelete.ToLower(CultureInfo.InvariantCulture)), 4456, @"[When the delete operation results in the transformation of an object into a recycled-object]All incoming linked attribute values are removed, but not as an originating update.");

            #endregion

            #region Delete object testUserGroup0 which is in tombstone

            adLdapModelAdapter.DeleteOperation(
                testUserGroup0Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_RECYCLED_OID

            resultEntry = adLdapModelAdapter.SearchDeletedObject(
                testUserGroup0Dn,
                ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID + ";" + ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID);
            member = string.Empty;
            if (resultEntry != null)
            {
                attributeList = resultEntry.attributes;
                attributeList_element = attributeList.Elements;
                mvale = new Asn1SetOf<AttributeValue>();
                attributevalue = new AttributeValue();
                for (int i = 0; i < attributeList_element.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeList_element[i].type.ByteArrayValue);
                    mvale = attributeList_element[i].vals;
                    if ((title == "member"))
                    {
                        for (int j = 0; j < mvale.Elements.Length; j++)
                        {
                            member += Convert.ToString(Encoding.ASCII.GetString(mvale.Elements[j].ByteArrayValue), CultureInfo.InvariantCulture);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsFalse(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4455, "[When the delete operation results in the transformation of an object into a recycled-object]All outgoing linked attribute values are removed, but not as an originating update.");
            Site.CaptureRequirementIfIsFalse(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4429, "[Recycled-Object Requirements]A recycled-object does not have outgoing linked attribute values (that is, it may not point to other objects via a linked attribute), even if the attribute would otherwise be allowed according to the preceding bullet point.");

            #endregion
        }

        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_TestLinkedAttributes()
        {
            Site.Assume.IsTrue(adLdapModelAdapter.RecycleBinEnabled, "This test case requires recycle bin to be enabled. Environment will be broken because if once recycle bin is enabled, it cannot be disabled.");

            #region Local variables

            string testUserGroup0Dn = adLdapModelAdapter.testUserGroup0DNForDs;
            string testUser3Dn = adLdapModelAdapter.testUser3DNForDs;
            string testUser4Dn = adLdapModelAdapter.testUser4DNForDs;
            DirectoryControl[] controls = null;
            string member = string.Empty;

            #endregion

            #region Add object testUser3Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser3Dn).Add("objectClass: user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add object testUser4Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser4Dn).Add("objectClass: user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2, null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add object testUserGroup0Dn with member testUser3Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUserGroup0Dn).Add("objectClass: group").Add("member: " + testUser3Dn).Add("displayName: group6750");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Enable recycle bin if not enabled

            if (!Utilities.IsOptionalFeatureEnabled(adLdapModelAdapter.forestScopePartialDN + ',' + adLdapModelAdapter.configurationNC, adLdapModelAdapter.recycleBinPartialDN))
            {
                modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("enableRecycleBin: true", new Sequence<string>().Add("distinguishedName: null")));
                // Enable recycle bin
                adLdapModelAdapter.ModifyRecycleBin(
                    modAttrValMap,
                    RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                    null,
                    ADImplementations.AD_DS,
                    ServerVersion.Win2008R2,
                    false,
                    out modErrorStatus);
                Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");
            }

            #endregion

            #region Delete object testUser3Dn

            string testUser3DnAfterDelete = Utilities.GetDeletedObjectDN(
                testUser3Dn,
                adLdapModelAdapter.deletedObjectsContainerDNForDs,
                adLdapModelAdapter.currentWorkingDC.FQDN,
                adLdapModelAdapter.currentPort);
            adLdapModelAdapter.DeleteOperation(
                testUser3Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Restore deleted object testUser3Dn

            adLdapModelAdapter.RestoreDeletedObject(
                testUser3DnAfterDelete,
                testUser3Dn,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_DELETED_OID

            // Search testUserGroup0Dn using LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_DELETED_OID control
            controls = new DirectoryControl[] {
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, null, true, true),
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, null, true, true)
            };
            AdLdapClient.Instance().SearchObject(
                testUserGroup0Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                controls,
                out searchResponse);
            member = string.Empty;
            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
            {
                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                entrypacket.GetInnerRequestOrResponse();
                PartialAttributeList partialAttributeList = entry.attributes;
                PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
                Asn1SetOf<AttributeValue> setOfAttributeValue = new Asn1SetOf<AttributeValue>();
                for (int i = 0; i < attributeListElement.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                    setOfAttributeValue = attributeListElement[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < setOfAttributeValue.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(setOfAttributeValue.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4544, "When a deleted-object o is transformed into an object that is not a deleted-object, a tombstone, or a recycled-object, any forward link reference to o from object p where p is not a deleted-object is made visible to LDAP operations.");
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3Dn.ToLower(CultureInfo.InvariantCulture)), 4545, "[When a deleted-object o is transformed into an object that is not a deleted-object, a tombstone, or a recycled-object] Similarly[like any forward link reference to o from object p where p is not a deleted-object is made visible to LDAP operations], any forward link reference from o to p is made visible to LDAP operations.");

            #endregion

            #region Delete object testUser3Dn which has be restored

            adLdapModelAdapter.DeleteOperation(
                testUser3Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Remove displayname of testUserGroup0Dn

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("displayName: removal", new Sequence<string>().Add("distinguishedName: " + testUserGroup0Dn)));
            adLdapModelAdapter.ModifyOperation(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_DELETED_OID

            controls = new DirectoryControl[] {
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, null, true, true),
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, null, true, true)
            };
            AdLdapClient.Instance().SearchObject(
                testUserGroup0Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                controls,
                out searchResponse);
            member = string.Empty;
            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
            {
                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                entrypacket.GetInnerRequestOrResponse();
                PartialAttributeList partialAttributeList = entry.attributes;
                PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
                Asn1SetOf<AttributeValue> setOfAttributeValue = new Asn1SetOf<AttributeValue>();
                for (int i = 0; i < attributeListElement.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                    setOfAttributeValue = attributeListElement[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < setOfAttributeValue.Elements.Length; j++)
                        {

                            member += Encoding.ASCII.GetString(setOfAttributeValue.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsTrue(member.ToLower(CultureInfo.InvariantCulture).Contains(testUser3DnAfterDelete.ToLower(CultureInfo.InvariantCulture)), 4331, "[modify operation]Note that if the update operation is an explicit list of attributes to be removed rather than a directive to completely remove the attribute, then no values that refer to deleted-objects are removed.");

            #endregion

            #region Replace the linked attribute "member" of testUserGroup0Dn testUser3Dn -> testUser4Dn

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("linkattribute: replacement: " + testUser4Dn, new Sequence<string>().Add("distinguishedName: " + testUserGroup0Dn)));
            adLdapModelAdapter.ModifyOperation(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_DELETED_OID

            controls = new DirectoryControl[] {
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, null, true, true),
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, null, true, true)
            };
            AdLdapClient.Instance().SearchObject(
                testUserGroup0Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                controls,
                out searchResponse);
            member = string.Empty;
            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
            {
                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                entrypacket.GetInnerRequestOrResponse();
                PartialAttributeList partialAttributeList = entry.attributes;
                PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
                Asn1SetOf<AttributeValue> setOfAttributeValue = new Asn1SetOf<AttributeValue>();
                for (int i = 0; i < attributeListElement.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                    setOfAttributeValue = attributeListElement[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < setOfAttributeValue.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(setOfAttributeValue.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsFalse(member.Contains(testUser3Dn), 4332, "[modify operation]If link attribute values that refer to deleted-objects are not visible to the update operation (section 3.1.1.3.4.1.25), and the update operation is a complete replacement of a link attribute, all existing values of the attribute including values that refer to deleted-objects are removed before any new values specified by the replacement are added.");

            #endregion

            #region Delete object testUser4Dn

            string distinguishedName3Deleted = Utilities.GetDeletedObjectDN(
                testUser4Dn,
                adLdapModelAdapter.deletedObjectsContainerDNForDs,
                adLdapModelAdapter.currentWorkingDC.FQDN,
                adLdapModelAdapter.currentPort);
            adLdapModelAdapter.DeleteOperation(
                testUser4Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Add a value to a single-valued attribute of testUserGroup0Dn

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("single-valuedAttribute: addValue", new Sequence<string>().Add("distinguishedName: " + testUserGroup0Dn)));
            adLdapModelAdapter.ModifyOperation(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnspecifiedError, modErrorStatus, "Modify operation should be failed.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_DELETED_OID

            controls = new DirectoryControl[] {
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, null, true, true),
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, null, true, true)
            };
            AdLdapClient.Instance().SearchObject(
                testUserGroup0Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                controls,
                out searchResponse);
            member = null;
            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
            {
                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                entrypacket.GetInnerRequestOrResponse();
                PartialAttributeList partialAttributeList = entry.attributes;
                PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
                Asn1SetOf<AttributeValue> setOfAttributeValue = new Asn1SetOf<AttributeValue>();
                for (int i = 0; i < attributeListElement.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                    setOfAttributeValue = attributeListElement[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < setOfAttributeValue.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(setOfAttributeValue.Elements[j].ByteArrayValue);
                        }
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsFalse(member.Contains(testUser4Dn), 4333, "[modify operation]If link attribute values that refer to deleted-objects are not visible to the update operation (section 3.1.1.3.4.1.25), and the update operation is the addition of a value to a single-valued attribute, and all existing values of the attribute refer to deleted-objects, then all existing values of the attribute (including values that refer to deleted-objects) are removed before the new value is added.");

            #endregion

            #region Restore deleted object testUser3Dn

            adLdapModelAdapter.RestoreDeletedObject(
                testUser3DnAfterDelete,
                testUser3Dn,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");

            #endregion

            #region Add testUser3Dn back to the linked attribute "member" of testUserGroup0Dn

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("member: " + testUser3Dn, new Sequence<string>().Add("distinguishedName: " + testUserGroup0Dn)));
            adLdapModelAdapter.ModifyOperation(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");

            #endregion

            #region Restore deleted object testUser4Dn

            adLdapModelAdapter.RestoreDeletedObject(
                distinguishedName3Deleted,
                testUser4Dn,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");

            #endregion

            #region Add testUser4Dn back to the linked attribute "member" of testUserGroup0Dn

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("member: " + testUser4Dn, new Sequence<string>().Add("distinguishedName: " + testUserGroup0Dn)));
            adLdapModelAdapter.ModifyOperation(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.success, modErrorStatus, "Modify operation should be successful.");

            #endregion

            #region Delete object testUser4Dn

            adLdapModelAdapter.DeleteOperation(
                testUser4Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Remove linked attribute of testUserGroup0Dn

            modAttrValMap = new Map<string, Sequence<string>>().Add(new KeyValuePair<string, Sequence<string>>("linkattribute: removal: " + testUser3Dn, new Sequence<string>().Add("distinguishedName: " + testUserGroup0Dn)));
            adLdapModelAdapter.ModifyOperation(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(ConstrOnModOpErrs.UnwillingToPerform_UnKnownError, modErrorStatus, "Modify operation should be failed.");

            #endregion

            #region Search testUserGroup0 for linked attribute "member" with LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID and LDAP_SERVER_SHOW_DELETED_OID

            controls = new DirectoryControl[] {
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, null, true, true),
                new DirectoryControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Common.ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, null, true, true)
            };
            AdLdapClient.Instance().SearchObject(
                testUserGroup0Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                controls,
                out searchResponse);
            member = null;
            foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
            {
                Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                entrypacket.GetInnerRequestOrResponse();
                PartialAttributeList partialAttributeList = entry.attributes;
                PartialAttributeList_element[] attributeListElement = partialAttributeList.Elements;
                Asn1SetOf<AttributeValue> setOfAttributeValue = new Asn1SetOf<AttributeValue>();
                for (int i = 0; i < attributeListElement.Length; i++)
                {
                    string title = Encoding.ASCII.GetString(attributeListElement[i].type.ByteArrayValue);
                    setOfAttributeValue = attributeListElement[i].vals;
                    if (title == "member")
                    {
                        for (int j = 0; j < setOfAttributeValue.Elements.Length; j++)
                        {
                            member += Encoding.ASCII.GetString(setOfAttributeValue.Elements[j].ByteArrayValue);
                        }
                    }
                }
            }
            Site.CaptureRequirementIfIsNull(member, 4330, "[modify operation]If link attribute values that refer to deleted-objects are not visible to the update operation (section 3.1.1.3.4.1.25), and the update operation is a complete removal of a link attribute, all existing values of the attribute are removed, including values that refer to deleted-objects.");

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestNonlinkedAttributes()
        {

            #region Local Variables

            string testUser4Dn = adLdapModelAdapter.testUser4DNForDs;
            string testUser5Dn = adLdapModelAdapter.testUser5DNForDs;

            #endregion

            #region Add object testUser4Dn

            addAttrValSeq = new Sequence<string>().Add("distinguishedName: " + testUser4Dn).Add("objectClass: user");
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Add object testUser5Dn

            addAttrValSeq = new Sequence<string>()
                .Add("distinguishedName: " + adLdapModelAdapter.testUser5DNForDs)
                .Add("objectClass: user")
                .Add("assistant:" + testUser4Dn);
            adLdapModelAdapter.AddOperation(
                addAttrValSeq,
                RightsOnParentObjects.RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,
                NCRight.RIGHT_DS_ADD_GUID,
                ServerVersion.Win2008R2,
                null,
                ADImplementations.AD_DS,
                false,
                out addErrorStatus);
            Site.Assert.AreEqual<ConstrOnAddOpErrs>(ConstrOnAddOpErrs.success, addErrorStatus, "Add operation should be successful.");

            #endregion

            #region Get deleted-object distinguished name for testUser4Dn

            string distinguishedName3Deleted = Utilities.GetDeletedObjectDN(
                testUser4Dn,
                adLdapModelAdapter.deletedObjectsContainerDNForDs,
                adLdapModelAdapter.currentWorkingDC.FQDN,
                adLdapModelAdapter.currentPort);

            #endregion

            #region Delete object testUser4Dn

            adLdapModelAdapter.DeleteOperation(
                testUser4Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion

            #region Search testUser5Dn for nonlinked attribute "assistant"

            // Search testUser0Dn using LDAP_SERVER_SHOW_DELETED_OID control
            AdLdapClient.Instance().SearchObject(
                testUser5Dn,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                null,
                null,
                out searchResponse);
            string assistant = string.Empty;
            if (searchResponse != null)
            {
                foreach (AdtsSearchResultEntryPacket entrypacket in searchResponse)
                {
                    assistant = AdLdapClient.Instance().GetAttributeValuesInString(entrypacket, "assistant")[0];
                }
            }
            Site.CaptureRequirementIfIsTrue(assistant.ToLower(CultureInfo.InvariantCulture).Contains(distinguishedName3Deleted.ToLower(CultureInfo.InvariantCulture)), 4414, "[Deleted-Object Requirements]The nonlinked references to the object are retained when the object is deleted and is transformed into a deleted-object (that is, other objects that referred to the original existing-object via nonlinked DN references continue pointing to the deleted-object indefinitely after the object is deleted, until these values are removed explicitly).");


            #endregion

            #region Delete object testUser5Dn

            adLdapModelAdapter.DeleteOperation(
                testUser5Dn,
                RightsOnParentObjects.RIGHT_DS_DELETE_CHILD,
                RightsOnObjects.RIGHT_DELETE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out delErrorStatus);
            Site.Assert.AreEqual<ConstrOnDelOpErr>(ConstrOnDelOpErr.success, delErrorStatus, "Delete operation should be successful.");

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_RootDseSearchOverUdpScenario()
        {
            int portNumber;
            string result;

            if (!string.IsNullOrEmpty(adLdapModelAdapter.ADLDSPortNum))
            {
                portNumber = Convert.ToInt32(adLdapModelAdapter.ADLDSPortNum, CultureInfo.InvariantCulture);
            }
            else
            {
                portNumber = Convert.ToInt32(adLdapModelAdapter.ADDSPortNum, CultureInfo.InvariantCulture);
            }
            Microsoft.Protocols.TestSuites.ActiveDirectory.Common.DomainController currentDC =
                adLdapModelAdapter.GetDomainController(adLdapModelAdapter.PDCNetbiosName);
            result = AdLdapClient.Instance().ConnectAndBindEx(
                currentDC.IPAddress,
                portNumber,
                StackTransportType.Udp);
            Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    string.Format("Connect and bind operation over UDP should be successful, actual result: {0}", result));

            ICollection<AdtsSearchResultEntryPacket> rssearchResponse = null;
            result = AdLdapClient.Instance().SearchObject(
                null,
                System.DirectoryServices.Protocols.SearchScope.Base,
                "(objectClass=*)",
                new string[] { "msDS-PrincipalName" },
                null,
                out rssearchResponse);
            Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    string.Format("Search rootDSE operation should be successful, actual result: {0}", result));
            Site.Log.Add(LogEntryKind.Debug, string.Format("{0} {1} found.", rssearchResponse.Count, rssearchResponse.Count > 1 ? "entries were" : "entry was"));
            foreach (AdtsSearchResultEntryPacket entrypacket in rssearchResponse)
            {
                string[] searchAttrVals = AdLdapClient.Instance().GetAttributeValuesInString(entrypacket, "msDS-PrincipalName");
                Site.Log.Add(LogEntryKind.Debug, string.Format("msDS-PrincipalName: {0}", searchAttrVals[0]));
                //On UDP, the same decoding method is used as on TCP and the result is correct. 
                //It indicates that AD encodes the results of an LDAP search performed over UDP in the same manner as it does a search performed over TCP
                Site.CaptureRequirementIfAreEqual(
                    "NT AUTHORITY\\ANONYMOUS LOGON",
                    searchAttrVals[0],
                    1516,
                    @"Active Directory supports search over UDP only for searches against rootDSE. 
                    It encodes the results of an LDAP search performed over UDP in the same manner as it does a search performed over TCP; specifically, 
                    as one or more SearchResultEntry messages followed by a SearchResultDone message, as described in [RFC2251].");
            }
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestReadPropertyAccess()
        {
            string dn = "CN=NTDS Quotas," + adLdapModelAdapter.rootDomainNC;

            //Connect and bind to the PDC
            Microsoft.Protocols.TestSuites.ActiveDirectory.Common.DomainController currentDC =
                adLdapModelAdapter.GetDomainController(adLdapModelAdapter.PDCNetbiosName);

            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_DS,
                adLdapModelAdapter.PDCNetbiosName);

            //Deny the ReadProperty access
            DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}:{1}/{2}",
                adLdapModelAdapter.currentWorkingDC.NetbiosName,
                adLdapModelAdapter.currentPort,
                dn),
                adLdapModelAdapter.testUserName,
                adLdapModelAdapter.testUserPwd);

            NTAccount accountName = new NTAccount(
                adLdapModelAdapter.currentWorkingDC.Domain.NetbiosName,
                adLdapModelAdapter.testUserName);
            IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
            ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(
                new SecurityIdentifier(acctSID.Value),
                ActiveDirectoryRights.ReadProperty,
                System.Security.AccessControl.AccessControlType.Deny);
            try
            {
                entry.ObjectSecurity.AddAccessRule(myRule);
                entry.CommitChanges();
                //Verify if the returned result is empty
                string[] attribs = { "msDS-TopQuotaUsage" };
                ICollection<AdtsSearchResultEntryPacket> response = null;
                string result = AdLdapClient.Instance().SearchObject(
                    null,
                    System.DirectoryServices.Protocols.SearchScope.Base,
                    "(objectClass=*)",
                    attribs,
                    null,
                    out response);
                Site.Assert.IsTrue(result.ToLower().Contains("success"),
                    string.Format("Search operation on rootDSE should be successful, actual result: {0}", result));
                Site.Log.Add(LogEntryKind.Debug, string.Format("{0} {1} found.", response.Count, response.Count > 1 ? "entries were" : "entry was"));
                foreach (AdtsSearchResultEntryPacket entrypacket in response)
                {
                    Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry searchResultEntry =
                        (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                        entrypacket.GetInnerRequestOrResponse();
                    Site.CaptureRequirementIfAreEqual(0, searchResultEntry.attributes.Elements.Length, 248,
                    @"While making LDAP request to msDs-TopQuotaUsage rootDSE attribute, if the caller does not have the 
                    RIGHT_DS_READ_PROPERTY access right on the Quotas container the search operation will succeed but no results will be returned.");
                }
            }
            finally
            {
                //Allow the ReadProperty access
                entry.ObjectSecurity.RemoveAccessRule(myRule);
                entry.CommitChanges();
            }
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_SmokeTest()
        {
            #region Connect and bind to server

            //Connect and bind to the PDC
            Microsoft.Protocols.TestSuites.ActiveDirectory.Common.DomainController currentDC =
                adLdapModelAdapter.GetDomainController(adLdapModelAdapter.PDCNetbiosName);

            try
            {
                adLdapModelAdapter.SetConnectAndBind(
                    ADImplementations.AD_DS,
                    adLdapModelAdapter.PDCNetbiosName);

                //check the connectiong between client and server
                Site.Assert.Pass(
                    "Bind response result should be LDAPResult_resultCode.success.");
            }
            catch
            {
                Site.Assert.Fail(
                    "Bind response result should be LDAPResult_resultCode.success.");
            }

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_RootDseModifyScenario()
        {
            adLdapModelAdapter.SetConnectAndBind(
                ADImplementations.AD_DS,
                adLdapModelAdapter.PDCNetbiosName);

            modAttrValMap = new Map<string, Sequence<string>>().Add(
                new KeyValuePair<string, Sequence<string>>(
                    "removeLingeringObject:CN=Configuration,DC=FAKELDAP,DC=com:CN=one,CN=adts_user1,CN=Users,DC=adts88",
                    new Sequence<string>().Add("distinguishedName:null")));
            adLdapModelAdapter.ModifyOperation(
                modAttrValMap,
                RightsOnAttributes.RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,
                null,
                ADImplementations.AD_DS,
                ServerVersion.Win2008R2,
                false,
                out modErrorStatus);
            Site.Assert.AreEqual<ConstrOnModOpErrs>(
                ConstrOnModOpErrs.OperationsError_ERROR_DS_OBJ_NOT_FOUND,
                modErrorStatus,
                "Modify operation should be successful.");
        }
    }
}
