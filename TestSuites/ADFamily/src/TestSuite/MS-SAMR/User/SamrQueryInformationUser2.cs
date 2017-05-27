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
        [Description("This is to test SamrQueryInformationUser2 with UserAllInformation.")]
        public void SamrQueryInformationUser2_UserAllInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL | Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT | Utilities.USER_READ_PREFERENCES);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle,_USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with MAXIMUM_ALLOWED successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserAllInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserAllInformation using USER_READ_GENERAL.")]
        public void SamrQueryInformationUser2_UserAllInformation_USER_READ_GENERAL()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserAllInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo, Utilities.USER_READ_GENERAL);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserAllInformation using USER_READ_LOGON.")]
        public void SamrQueryInformationUser2_UserAllInformation_USER_READ_LOGON()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserAllInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo, Utilities.USER_READ_LOGON);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserAllInformation using USER_READ_ACCOUNT.")]
        public void SamrQueryInformationUser2_UserAllInformation_USER_READ_ACCOUNT()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with USER_READ_ACCOUNT successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserAllInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo, Utilities.USER_READ_ACCOUNT);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserAllInformation using USER_READ_PREFERENCES.")]
        public void SamrQueryInformationUser2_UserAllInformation_USER_READ_PREFERENCES()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_PREFERENCES);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with USER_READ_PREFERENCES successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserAllInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo, Utilities.USER_READ_PREFERENCES);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserAccountInformation.")]
        public void SamrQueryInformationUser2_UserAccountInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL | Utilities.USER_READ_PREFERENCES | Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAccountInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAccountInformation with USER_READ_GENERAL | USER_READ_PREFERENCES | USER_READ_LOGON | USER_READ_ACCOUNT successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserAccountInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserGeneralInformation.")]
        public void SamrQueryInformationUser2_UserGeneralInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserGeneralInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserGeneralInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserGeneralInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserPrimaryGroupInformation.")]
        public void SamrQueryInformationUser2_UserPrimaryGroupInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPrimaryGroupInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserPrimaryGroupInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserPrimaryGroupInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserNameInformation.")]
        public void SamrQueryInformationUser2_UserNameInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserNameInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserNameInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserNameInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserAccountNameInformation.")]
        public void SamrQueryInformationUser2_UserAccountNameInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAccountNameInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAccountNameInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserAccountNameInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserFullNameInformation.")]
        public void SamrQueryInformationUser2_UserFullNameInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserFullNameInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserFullNameInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserFullNameInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserAdminCommentInformation.")]
        public void SamrQueryInformationUser2_UserAdminCommentInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAdminCommentInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAdminCommentInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserAdminCommentInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserPreferencesInformation.")]
        public void SamrQueryInformationUser2_UserPreferencesInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_PREFERENCES | Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPreferencesInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserPreferencesInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserPreferencesInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserLogonInformation.")]
        public void SamrQueryInformationUser2_UserLogonInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL | Utilities.USER_READ_PREFERENCES |  Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserLogonInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserLogonInformation with USER_READ_GENERAL|USER_READ_PREFERENCES|USER_READ_LOGON|USER_READ_ACCOUNT successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserLogonInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserLogonHoursInformation.")]
        public void SamrQueryInformationUser2_UserLogonHoursInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserLogonHoursInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserLogonHoursInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserLogonHoursInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserHomeInformation.")]
        public void SamrQueryInformationUser2_UserHomeInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserHomeInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserHomeInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserHomeInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserScriptInformation.")]
        public void SamrQueryInformationUser2_UserScriptInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserScriptInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserScriptInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserScriptInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserProfileInformation.")]
        public void SamrQueryInformationUser2_UserProfileInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserProfileInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserProfileInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserProfileInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserWorkStationsInformation.")]
        public void SamrQueryInformationUser2_UserWorkStationsInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserWorkStationsInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserWorkStationsInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserWorkStationsInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserControlInformation.")]
        public void SamrQueryInformationUser2_UserControlInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserControlInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserControlInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserControlInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserExpiresInformation.")]
        public void SamrQueryInformationUser2_UserExpiresInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserExpiresInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserExpiresInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserExpiresInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with UserParametersInformation.")]
        public void SamrQueryInformationUser2_UserParametersInformation()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserParametersInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserParametersInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");

            _samrProtocolAdapter.VerifyUserParametersInformationFields(_samrProtocolAdapter.DomainAdministratorName, userInfo);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with invalid handle.")]
        public void SamrQueryInformationUser2_WithInvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with server handle.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_serverHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0} with server handle.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 with domain handle.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_domainHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 returns:{0} with domain handle.", result);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 without required access.")]
        public void SamrQueryInformationUser2_WithoutRequiredAccess()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_WRITE_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserAccountInformation with USER_WRITE_ACCOUNT.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAccountInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserGeneralInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserGeneralInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserPrimaryGroupInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPrimaryGroupInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserNameInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserNameInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserAccountNameInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAccountNameInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserFullNameInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserFullNameInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserAdminCommentInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAdminCommentInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserPreferencesInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPreferencesInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserLogonInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserLogonInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserLogonHoursInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserLogonHoursInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserHomeInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserHomeInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserScriptInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserScriptInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserProfileInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserProfileInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserWorkStationsInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserWorkStationsInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserControlInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserControlInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserExpiresInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserExpiresInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2 using UserParametersInformation with USER_WRITE_ACCOUNT.");
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserParametersInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "In the DC configuration, this handle-based check MUST be relaxed if the client has ACTRL_DS_READ_PROP access on the userParameters attribute.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryInformationUser2 with invalid information level.")]
        public void SamrQueryInformationUser2_WithInvalidInformationLevel()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
    _samrProtocolAdapter.DomainAdministratorName, out _userHandle, Utilities.USER_READ_GENERAL | Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT | Utilities.USER_READ_PREFERENCES);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInvalidInformation, out userInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "If there is no match on Information Level, the server MUST return an error.");
        }
    }
}
