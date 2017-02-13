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
        [Description("This is to test SamrSetInformationUser2 with UserAccountNameInformation.")]
        public void SamrSetInformationUser2_AccountName()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserAccountNameInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newAccountName = "SamrNewAccountName";
            expectUserInfo.AccountName.UserName = _samrProtocolAdapter.StringToRpcUnicodeString(newAccountName);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAccountNameInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserAccountNameInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAccountNameInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(newAccountName,
                utilityObject.convertToString(actualUserInfo.Value.AccountName.UserName.Buffer),
                "The AccountName has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserAdminCommentInformation.")]
        public void SamrSetInformationUser2_AdminComment()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

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

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserAdminCommentInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newAdminComment = "SamrNewAdminComment";
            expectUserInfo.AdminComment.AdminComment = _samrProtocolAdapter.StringToRpcUnicodeString(newAdminComment);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAdminCommentInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserAdminCommentInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAdminCommentInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(newAdminComment, 
                utilityObject.convertToString(actualUserInfo.Value.AdminComment.AdminComment.Buffer),
                "The AdminComment has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserControlInformation.")]
        public void SamrSetInformationUser2_Control()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserControlInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Control.UserAccountControl = (uint)(USER_ACCOUNT_CONTROL.USER_PASSWORD_NOT_REQUIRED|USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT|USER_ACCOUNT_CONTROL.USER_PASSWORD_EXPIRED);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserControlInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserControlInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserControlInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual((uint)(USER_ACCOUNT_CONTROL.USER_PASSWORD_NOT_REQUIRED | USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT | USER_ACCOUNT_CONTROL.USER_PASSWORD_EXPIRED), 
                actualUserInfo.Value.Control.UserAccountControl,
                "The UserAccountControl has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserFullNameInformation.")]
        public void SamrSetInformationUser2_FullName()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserFullNameInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newFullName = "SamrNewFullName";
            expectUserInfo.FullName.FullName = _samrProtocolAdapter.StringToRpcUnicodeString(newFullName);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserFullNameInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserFullNameInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserFullNameInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(newFullName, utilityObject.convertToString(actualUserInfo.Value.FullName.FullName.Buffer),
                "The FullName has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserGeneralInformation.")]
        public void SamrSetInformationUser2_General()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserGeneralInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newAdminComment = "SamrNewAdminComment";
            string newFullName = "SamrNewFullName";
            string newUserComment = "SamrNewUserComment";
            string newUserName = "SamrNewUserName";
            expectUserInfo.General.AdminComment = _samrProtocolAdapter.StringToRpcUnicodeString(newAdminComment);
            expectUserInfo.General.FullName = _samrProtocolAdapter.StringToRpcUnicodeString(newFullName);
            expectUserInfo.General.PrimaryGroupId = Utilities.DOMAIN_GROUP_RID_USERS;
            expectUserInfo.General.UserComment = _samrProtocolAdapter.StringToRpcUnicodeString(newUserComment);
            expectUserInfo.General.UserName = _samrProtocolAdapter.StringToRpcUnicodeString(newUserName);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserGeneralInformation, expectUserInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.6.4.1 If the value of UserInformationClass was not found in the previous three constraints, the server MUST return an error.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserAccountInformation.")]
        public void SamrSetInformationUser2_Account()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Account.AccountExpires.HighPart = 5;
            expectUserInfo.Account.AccountExpires.LowPart = 1;
            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserAccountInformation.");
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAccountInformation, expectUserInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.6.4.1 If the value of UserInformationClass was not found in the previous three constraints, the server MUST return an error.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserLogonInformation.")]
        public void SamrSetInformationUser2_Logon()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Logon.LogonCount = 10;
            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserLogonInformation.");
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserLogonInformation, expectUserInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.6.4.1 If the value of UserInformationClass was not found in the previous three constraints, the server MUST return an error.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserNameInformation.")]
        public void SamrSetInformationUser2_Name()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserNameInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newFullName = "SamrNewFullName";
            string newUserName = "SamrNewUserName";
            expectUserInfo.Name.FullName = _samrProtocolAdapter.StringToRpcUnicodeString(newFullName);
            expectUserInfo.Name.UserName = _samrProtocolAdapter.StringToRpcUnicodeString(newUserName);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserNameInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserNameInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserNameInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(newFullName, utilityObject.convertToString(actualUserInfo.Value.Name.FullName.Buffer),
                "The FullName has been set successfully.");
            Site.Assert.AreEqual(newUserName, utilityObject.convertToString(actualUserInfo.Value.Name.UserName.Buffer),
                "The UserName has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserPrimaryGroupInformation.")]
        public void SamrSetInformationUser2_PrimaryGroup()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserPrimaryGroupInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.PrimaryGroup.PrimaryGroupId = Utilities.DOMAIN_GROUP_RID_USERS;
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPrimaryGroupInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserPrimaryGroupInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPrimaryGroupInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(Utilities.DOMAIN_GROUP_RID_USERS, actualUserInfo.Value.PrimaryGroup.PrimaryGroupId,
                "The PrimaryGroupId has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserHomeInformation.")]
        public void SamrSetInformationUser2_Home()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserHomeInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newHomeDirectory = "SamrNewHomeDirectory";
            string newHomeDirectoryDrive = "SamrNewHomeDirectoryDrive";
            expectUserInfo.Home.HomeDirectory = _samrProtocolAdapter.StringToRpcUnicodeString(newHomeDirectory);
            expectUserInfo.Home.HomeDirectoryDrive = _samrProtocolAdapter.StringToRpcUnicodeString(newHomeDirectoryDrive);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserHomeInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserHomeInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserHomeInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(newHomeDirectory, utilityObject.convertToString(actualUserInfo.Value.Home.HomeDirectory.Buffer),
                "The HomeDirectory has been set successfully.");
            Site.Assert.AreEqual(newHomeDirectoryDrive, utilityObject.convertToString(actualUserInfo.Value.Home.HomeDirectoryDrive.Buffer),
                "The HomeDirectoryDrive has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserScriptInformation.")]
        public void SamrSetInformationUser2_Script()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserScriptInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newScriptPath = "SamrNewScriptPath";
            expectUserInfo.Script.ScriptPath = _samrProtocolAdapter.StringToRpcUnicodeString(newScriptPath);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserScriptInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserScriptInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserScriptInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(newScriptPath, utilityObject.convertToString(actualUserInfo.Value.Script.ScriptPath.Buffer),
                "The ScriptPath has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserProfileInformation.")]
        public void SamrSetInformationUser2_Profile()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserProfileInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newProfilePath = "SamrNewProfilePath";
            expectUserInfo.Profile.ProfilePath = _samrProtocolAdapter.StringToRpcUnicodeString(newProfilePath);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserProfileInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserProfileInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserProfileInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(newProfilePath, utilityObject.convertToString(actualUserInfo.Value.Profile.ProfilePath.Buffer),
                "The ProfilePath has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserPreferencesInformation.")]
        public void SamrSetInformationUser2_Preferences()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserPreferencesInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newUserComment = "SamrNewUserComment";
            expectUserInfo.Preferences.CodePage = 1200;
            expectUserInfo.Preferences.CountryCode = 44;
            expectUserInfo.Preferences.Reserved1 = _samrProtocolAdapter.StringToRpcUnicodeString("");
            expectUserInfo.Preferences.UserComment = _samrProtocolAdapter.StringToRpcUnicodeString(newUserComment);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPreferencesInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserPreferencesInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPreferencesInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(1200, actualUserInfo.Value.Preferences.CodePage,
                "The CodePage has been set successfully.");
            Site.Assert.AreEqual(44, actualUserInfo.Value.Preferences.CountryCode,
                "The CountryCode has been set successfully.");
            Site.Assert.IsNull(actualUserInfo.Value.Preferences.Reserved1.Buffer,
                "Reserved1: Ignored by the client and server and MUST be a zero-length string when sent and returned.");
            Site.Assert.AreEqual(newUserComment, utilityObject.convertToString(actualUserInfo.Value.Preferences.UserComment.Buffer),
                "The UserComment has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 with UserWorkStationsInformation.")]
        public void SamrSetInformationUser2_WorkStations()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserWorkStationsInformation.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newWorkStations = "SamrNewWorkStations";
            expectUserInfo.WorkStations.WorkStations = _samrProtocolAdapter.StringToRpcUnicodeString(newWorkStations);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserWorkStationsInformation, expectUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with UserWorkStationsInformation.");
            _SAMPR_USER_INFO_BUFFER? actualUserInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserWorkStationsInformation, out actualUserInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0}.", result);

            Site.Assert.AreEqual(newWorkStations, utilityObject.convertToString(actualUserInfo.Value.WorkStations.WorkStations.Buffer),
                "The WorkStations has been set successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrSetInformationUser2 for user with Rid as DOMAIN_USER_RID_KRBTGT.")]
        public void SamrSetInformationUser2_DOMAIN_USER_RID_KRBTGT()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: Open a user with Rid as DOMAIN_USER_RID_KRBTGT.");
            HRESULT result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, Utilities.USER_ALL_ACCESS, Utilities.DOMAIN_USER_RID_KRBTGT, out _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "Open the user with Rid as DOMAIN_USER_RID_KRBTGT successfully.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 for user with Rid as DOMAIN_USER_RID_KRBTGT.");
            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            string newFullName = "SamrNewFullName";
            string newUserName = "SamrNewUserName";
            expectUserInfo.Name.FullName = _samrProtocolAdapter.StringToRpcUnicodeString(newFullName);
            expectUserInfo.Name.UserName = _samrProtocolAdapter.StringToRpcUnicodeString(newUserName);
            result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserNameInformation, expectUserInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.1.8.4 If the objectSid attribute has a RID of DOMAIN_USER_RID_KRBTGT and there is already a value present in the sAMAccountName attribute, the server MUST return an error status.");
        }
    }
}
