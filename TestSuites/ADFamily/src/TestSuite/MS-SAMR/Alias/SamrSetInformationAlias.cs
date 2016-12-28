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
        [TestCategory("Alias")]
        [Description("This is to test SamrSetInformationAlias with GroupAdminCommentInformation.")]
        public void SamrSetInformationAlias_AdminComment()
        {
            aliasTestPrerequisite();
            createAlias();

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationAlias: update attributes on a user object.");
            _SAMPR_ALIAS_INFO_BUFFER expectGroupInfo = new _SAMPR_ALIAS_INFO_BUFFER();
            expectGroupInfo.AdminComment.AdminComment = _samrProtocolAdapter.StringToRpcUnicodeString("TestAdminComment");
            HRESULT result = _samrProtocolAdapter.SamrSetInformationAlias(_aliasHandle, _ALIAS_INFORMATION_CLASS.AliasAdminCommentInformation, expectGroupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationAlias returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationAlias: obtain attributes from an alias object.");
            _SAMPR_ALIAS_INFO_BUFFER? actualGroupInfo;
            result = _samrProtocolAdapter.SamrQueryInformationAlias(_aliasHandle, _ALIAS_INFORMATION_CLASS.AliasAdminCommentInformation, out actualGroupInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationAlias returns:{0}.", result);

            Site.Assert.AreEqual(utilityObject.convertToString(expectGroupInfo.AdminComment.AdminComment.Buffer),
                utilityObject.convertToString(actualGroupInfo.Value.AdminComment.AdminComment.Buffer),
                "The AdminComment has been set successfully.");
        }

    }
}
