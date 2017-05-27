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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrSetInformationAlias with AliasAdminCommentInformation.")]
        public void SamrSetInformationAlias_AdminComment_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string aliasName = testAliasName;
            uint relativeId;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                    testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _domainHandle,
                    aliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationAlias: update attributes on an alias object.");
                _SAMPR_ALIAS_INFO_BUFFER expectAliasInfo = new _SAMPR_ALIAS_INFO_BUFFER();
                expectAliasInfo.AdminComment.AdminComment = _samrProtocolAdapter.StringToRpcUnicodeString("TestAdminComment");
                result = _samrProtocolAdapter.SamrSetInformationAlias(_aliasHandle, _ALIAS_INFORMATION_CLASS.AliasAdminCommentInformation, expectAliasInfo);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationAlias returns:{0}.", result);

                Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationAlias: obtain attributes from an alias object.");
                _SAMPR_ALIAS_INFO_BUFFER? actualAliasInfo;
                result = _samrProtocolAdapter.SamrQueryInformationAlias(_aliasHandle, _ALIAS_INFORMATION_CLASS.AliasAdminCommentInformation, out actualAliasInfo);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationAlias returns:{0}.", result);

                Site.Assert.AreEqual(utilityObject.convertToString(expectAliasInfo.AdminComment.AdminComment.Buffer),
                    utilityObject.convertToString(actualAliasInfo.Value.AdminComment.AdminComment.Buffer),
                    "The AdminComment has been set successfully.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: delete the created alias.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeeded.");
            }
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrSetInformationAlias with AliasNameInformation.")]
        public void SamrSetInformationAlias_Name_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string aliasName = testAliasName;
            uint relativeId;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                    testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _domainHandle,
                    aliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationAlias: update attributes on an alias object.");
                _SAMPR_ALIAS_INFO_BUFFER expectAliasInfo = new _SAMPR_ALIAS_INFO_BUFFER();
                expectAliasInfo.Name.Name = _samrProtocolAdapter.StringToRpcUnicodeString("TestSamAccountName");
                result = _samrProtocolAdapter.SamrSetInformationAlias(_aliasHandle, _ALIAS_INFORMATION_CLASS.AliasNameInformation, expectAliasInfo);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationAlias returns:{0}.", result);

                Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationAlias: obtain attributes from an alias object.");
                _SAMPR_ALIAS_INFO_BUFFER? actualAliasInfo;
                result = _samrProtocolAdapter.SamrQueryInformationAlias(_aliasHandle, _ALIAS_INFORMATION_CLASS.AliasNameInformation, out actualAliasInfo);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationAlias returns:{0}.", result);

                Site.Assert.AreEqual(utilityObject.convertToString(expectAliasInfo.Name.Name.Buffer),
                    utilityObject.convertToString(actualAliasInfo.Value.Name.Name.Buffer),
                    "The Name has been set successfully.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: delete the created alias.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeeded.");
            }
        }
    }
}
