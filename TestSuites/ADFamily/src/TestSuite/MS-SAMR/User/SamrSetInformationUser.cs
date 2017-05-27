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
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser with UserAdminCommentInformation.")]
        public void SamrSetInformationUser_AdminComment()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                   testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
                uint grantedAccess, relativeId;
                HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);
                Site.Assert.IsTrue(_samrProtocolAdapter.VerifyRelativeID(relativeId), "The Rid value MUST be within the Rid-Range");
                Site.Assert.AreEqual((uint)User_ACCESS_MASK.USER_ALL_ACCESS, grantedAccess,
                    "The return parameter of GrantedAccess MUST be set to DesiredAccess if DesiredAccess contains only valid access masks for the user object ");

                Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser: update attributes on a user object.");
                _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
                expectUserInfo.AdminComment.AdminComment = _samrProtocolAdapter.StringToRpcUnicodeString("TestAdminComment");
                result = _samrProtocolAdapter.SamrSetInformationUser(_userHandle, _USER_INFORMATION_CLASS.UserAdminCommentInformation, expectUserInfo);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser returns:{0}.", result);

                Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser: obtain attributes from a user object.");
                _SAMPR_USER_INFO_BUFFER? actualUserInfo;
                result = _samrProtocolAdapter.SamrQueryInformationUser(_userHandle, _USER_INFORMATION_CLASS.UserAdminCommentInformation, out actualUserInfo);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser returns:{0}.", result);

                Site.Assert.AreEqual(utilityObject.convertToString(expectUserInfo.AdminComment.AdminComment.Buffer),
                    utilityObject.convertToString(actualUserInfo.Value.AdminComment.AdminComment.Buffer),
                    "The AdminComment has been set successfully.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
            }
        }

    }
}
