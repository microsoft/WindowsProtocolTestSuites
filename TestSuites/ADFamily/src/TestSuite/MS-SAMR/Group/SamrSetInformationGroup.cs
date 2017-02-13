// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrSetInformationGroup with GroupAdminCommentInformation.")]
        public void SamrSetInformationGroup_AdminComment()
        {
            Site.Log.Add(LogEntryKind.TestStep, "SamrBind-->SamrConnect5-->SamrLookupDomainInSamServer-->SamrOpenDomain");
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: create a group with Name:{0}, and DesiredAccess:{1}",
               testGroupName, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(_domainHandle,
                testGroupName, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                out _groupHandle, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned group handle is: {0}.", _groupHandle);
            Site.Assert.IsTrue(_samrProtocolAdapter.VerifyRelativeID(relativeId), "The Rid value MUST be within the Rid-Range");

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationGroup: update attributes on a group object.");
            _SAMPR_GROUP_INFO_BUFFER expectGroupInfo = new _SAMPR_GROUP_INFO_BUFFER();
            expectGroupInfo.AdminComment.AdminComment = _samrProtocolAdapter.StringToRpcUnicodeString("TestAdminComment");
            result = _samrProtocolAdapter.SamrSetInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupAdminCommentInformation, expectGroupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationGroup returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup: obtain attributes from a group object.");
            _SAMPR_GROUP_INFO_BUFFER? actualGroupInfo;
            result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupAdminCommentInformation, out actualGroupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0}.", result);

            Site.Assert.AreEqual(utilityObject.convertToString(expectGroupInfo.AdminComment.AdminComment.Buffer),
                utilityObject.convertToString(actualGroupInfo.Value.AdminComment.AdminComment.Buffer),
                "The AdminComment has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
            result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrSetInformationGroup with GroupAttributeInformation.")]
        public void SamrSetInformationGroup_Attribute()
        {
            Site.Log.Add(LogEntryKind.TestStep, "SamrBind-->SamrConnect5-->SamrLookupDomainInSamServer-->SamrOpenDomain");
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: create a group with Name:{0}, and DesiredAccess:{1}",
               testGroupName, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(_domainHandle,
                testGroupName, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                out _groupHandle, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned group handle is: {0}.", _groupHandle);
            Site.Assert.IsTrue(_samrProtocolAdapter.VerifyRelativeID(relativeId), "The Rid value MUST be within the Rid-Range");

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationGroup: update attributes on a group object.");
            _SAMPR_GROUP_INFO_BUFFER expectGroupInfo = new _SAMPR_GROUP_INFO_BUFFER();
            expectGroupInfo.Attribute.Attributes = Utilities.SE_GROUP_MANDATORY | Utilities.SE_GROUP_ENABLED_BY_DEFAULT | Utilities.SE_GROUP_ENABLED;
            result = _samrProtocolAdapter.SamrSetInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupAttributeInformation, expectGroupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationGroup returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup: obtain attributes from a group object.");
            _SAMPR_GROUP_INFO_BUFFER? actualGroupInfo;
            result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupAttributeInformation, out actualGroupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0}.", result);

            Site.Assert.AreEqual<uint>(expectGroupInfo.Attribute.Attributes, actualGroupInfo.Value.Attribute.Attributes,
                "The Attribute has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
            result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrSetInformationGroup with GroupNameInformation.")]
        public void SamrSetInformationGroup_Name()
        {
            Site.Log.Add(LogEntryKind.TestStep, "SamrBind-->SamrConnect5-->SamrLookupDomainInSamServer-->SamrOpenDomain");
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: create a group with Name:{0}, and DesiredAccess:{1}",
               testGroupName, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(_domainHandle,
                testGroupName, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                out _groupHandle, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned group handle is: {0}.", _groupHandle);
            Site.Assert.IsTrue(_samrProtocolAdapter.VerifyRelativeID(relativeId), "The Rid value MUST be within the Rid-Range");

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationGroup: update attributes on a group object.");
            _SAMPR_GROUP_INFO_BUFFER expectGroupInfo = new _SAMPR_GROUP_INFO_BUFFER();
            expectGroupInfo.Name.Name = _samrProtocolAdapter.StringToRpcUnicodeString("TestSamAccountName");
            result = _samrProtocolAdapter.SamrSetInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupNameInformation, expectGroupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationGroup returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup: obtain attributes from a group object.");
            _SAMPR_GROUP_INFO_BUFFER? actualGroupInfo;
            result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupNameInformation, out actualGroupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0}.", result);

            Site.Assert.AreEqual(utilityObject.convertToString(expectGroupInfo.Name.Name.Buffer),
                utilityObject.convertToString(actualGroupInfo.Value.Name.Name.Buffer),
                "The Name has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
            result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup returns:{0}.", result);
        }
    }
}
