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
        [Description("This is to test SamrQueryInformationGroup with GroupGeneralInformation.")]
        public void SamrQueryInformationGroup_GroupGeneralInformation()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);
            LookupAndOpenGroup(_domainHandle, _samrProtocolAdapter.DomainAdminGroup, out _groupHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup: obtain attributes from a group object.");
            _SAMPR_GROUP_INFO_BUFFER? groupInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupGeneralInformation, out groupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0}.", result);
            Site.Assert.IsNotNull(groupInfo, "The returned groupInfo should not be null.");

            _samrProtocolAdapter.VerifyGroupGeneralInformationFields(_samrProtocolAdapter.DomainAdminGroup, groupInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryInformationGroup with GroupNameInformation.")]
        public void SamrQueryInformationGroup_GroupNameInformation()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);
            LookupAndOpenGroup(_domainHandle, _samrProtocolAdapter.DomainAdminGroup, out _groupHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup: obtain attributes from a group object.");
            _SAMPR_GROUP_INFO_BUFFER? groupInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupNameInformation, out groupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0}.", result);
            Site.Assert.IsNotNull(groupInfo, "The returned groupInfo should not be null.");

            _samrProtocolAdapter.VerifyGroupNameInformationFields(_samrProtocolAdapter.DomainAdminGroup, groupInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryInformationGroup with GroupAttributeInformation.")]
        public void SamrQueryInformationGroup_GroupAttributeInformation()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);
            LookupAndOpenGroup(_domainHandle, _samrProtocolAdapter.DomainAdminGroup, out _groupHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup: obtain attributes from a group object.");
            _SAMPR_GROUP_INFO_BUFFER? groupInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupAttributeInformation, out groupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0}.", result);
            Site.Assert.IsNotNull(groupInfo, "The returned groupInfo should not be null.");

            _samrProtocolAdapter.VerifyGroupAttributeInformationFields(_samrProtocolAdapter.DomainAdminGroup, groupInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryInformationGroup with GroupAdminCommentInformation.")]
        public void SamrQueryInformationGroup_GroupAdminCommentInformation()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);
            LookupAndOpenGroup(_domainHandle, _samrProtocolAdapter.DomainAdminGroup, out _groupHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup: obtain attributes from a group object.");
            _SAMPR_GROUP_INFO_BUFFER? groupInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupAdminCommentInformation, out groupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0}.", result);
            Site.Assert.IsNotNull(groupInfo, "The returned groupInfo should not be null.");

            _samrProtocolAdapter.VerifyGroupAdminCommentInformationFields(_samrProtocolAdapter.DomainAdminGroup, groupInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryInformationGroup with GroupReplicationInformation.")]
        public void SamrQueryInformationGroup_GroupReplicationInformation()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);
            LookupAndOpenGroup(_domainHandle, _samrProtocolAdapter.DomainAdminGroup, out _groupHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup: obtain attributes from a group object.");
            _SAMPR_GROUP_INFO_BUFFER? groupInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupReplicationInformation, out groupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0}.", result);
            Site.Assert.IsNotNull(groupInfo, "The returned groupInfo should not be null.");

            _samrProtocolAdapter.VerifyGroupReplicationInformationFields(_samrProtocolAdapter.DomainAdminGroup, groupInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryInformationGroup with GroupGeneralInformation.")]
        public void SamrQueryInformationGroup_WithInvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup with server handle.");
            _SAMPR_GROUP_INFO_BUFFER? groupInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationGroup(_serverHandle, _GROUP_INFORMATION_CLASS.GroupGeneralInformation, out groupInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0} with server handle.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup with domain handle.");
            result = _samrProtocolAdapter.SamrQueryInformationGroup(_domainHandle, _GROUP_INFORMATION_CLASS.GroupGeneralInformation, out groupInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationGroup returns:{0} with domain handle.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryInformationGroup without required access.")]
        public void SamrQueryInformationGroup_WithoutRequiredAccess()
        {
            ConnectAndOpenGroup(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdminGroup, out _groupHandle, Utilities.GROUP_WRITE_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup using GroupGeneralInformation with GROUP_WRITE_ACCOUNT.");
            _SAMPR_GROUP_INFO_BUFFER? groupInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupGeneralInformation, out groupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "groupHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(groupInfo, "The returned groupInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup using GroupNameInformation with GROUP_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupNameInformation, out groupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "groupHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(groupInfo, "The returned groupInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup using GroupAttributeInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupAttributeInformation, out groupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "groupHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(groupInfo, "The returned groupInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationGroup using UserNameInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationGroup(_groupHandle, _GROUP_INFORMATION_CLASS.GroupReplicationInformation, out groupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "groupHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(groupInfo, "The returned groupInfo should be null.");
        }
    }
}
