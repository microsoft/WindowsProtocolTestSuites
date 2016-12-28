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
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserAllInformation.")]
        public void SamrQueryInformationUser2_UserAllInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL | Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT | Utilities.USER_READ_PREFERENCES);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle,_USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with MAXIMUM_ALLOWED successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserAllInformation using USER_READ_GENERAL.")]
        public void SamrQueryInformationUser2_UserAllInformation_USER_READ_GENERAL_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserAllInformation using USER_READ_LOGON.")]
        public void SamrQueryInformationUser2_UserAllInformation_USER_READ_LOGON_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserAllInformation using USER_READ_ACCOUNT.")]
        public void SamrQueryInformationUser2_UserAllInformation_USER_READ_ACCOUNT_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with USER_READ_ACCOUNT successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserAllInformation using USER_READ_PREFERENCES.")]
        public void SamrQueryInformationUser2_UserAllInformation_USER_READ_PREFERENCES_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_PREFERENCES);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with USER_READ_PREFERENCES successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserAccountInformation.")]
        public void SamrQueryInformationUser2_UserAccountInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL | Utilities.USER_READ_PREFERENCES | Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAccountInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAccountInformation with USER_READ_GENERAL | USER_READ_PREFERENCES | USER_READ_LOGON | USER_READ_ACCOUNT successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserGeneralInformation.")]
        public void SamrQueryInformationUser2_UserGeneralInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserGeneralInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserGeneralInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserPrimaryGroupInformation.")]
        public void SamrQueryInformationUser2_UserPrimaryGroupInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPrimaryGroupInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserPrimaryGroupInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserNameInformation.")]
        public void SamrQueryInformationUser2_UserNameInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserNameInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserNameInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserAccountNameInformation.")]
        public void SamrQueryInformationUser2_UserAccountNameInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAccountNameInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAccountNameInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserFullNameInformation.")]
        public void SamrQueryInformationUser2_UserFullNameInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserFullNameInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserFullNameInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserAdminCommentInformation.")]
        public void SamrQueryInformationUser2_UserAdminCommentInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAdminCommentInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAdminCommentInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserPreferencesInformation.")]
        public void SamrQueryInformationUser2_UserPreferencesInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_PREFERENCES | Utilities.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserPreferencesInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserPreferencesInformation with USER_READ_GENERAL successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserLogonInformation.")]
        public void SamrQueryInformationUser2_UserLogonInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL | Utilities.USER_READ_PREFERENCES | Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserLogonInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserLogonInformation with USER_READ_GENERAL|USER_READ_PREFERENCES|USER_READ_LOGON|USER_READ_ACCOUNT successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserLogonHoursInformation.")]
        public void SamrQueryInformationUser2_UserLogonHoursInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserLogonHoursInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserLogonHoursInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserHomeInformation.")]
        public void SamrQueryInformationUser2_UserHomeInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserHomeInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserHomeInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserScriptInformation.")]
        public void SamrQueryInformationUser2_UserScriptInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserScriptInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserScriptInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserProfileInformation.")]
        public void SamrQueryInformationUser2_UserProfileInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserProfileInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserProfileInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserWorkStationsInformation.")]
        public void SamrQueryInformationUser2_UserWorkStationsInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_LOGON);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserWorkStationsInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserWorkStationsInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserControlInformation.")]
        public void SamrQueryInformationUser2_UserControlInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserControlInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserControlInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserExpiresInformation.")]
        public void SamrQueryInformationUser2_UserExpiresInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_ACCOUNT);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserExpiresInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserExpiresInformation with USER_READ_LOGON successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with UserParametersInformation.")]
        public void SamrQueryInformationUser2_UserParametersInformation_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_ACCOUNT);

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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with invalid handle.")]
        public void SamrQueryInformationUser2_WithInvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 without required access.")]
        public void SamrQueryInformationUser2_WithoutRequiredAccess_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_WRITE_ACCOUNT);

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
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in Common Processing (section 3.1.5.5.5.1).");
            Site.Assert.IsNull(userInfo, "The returned userInfo should be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryInformationUser2 with invalid information level.")]
        public void SamrQueryInformationUser2_WithInvalidInformationLevel_NonDC()
        {
            ConnectAndOpenUser_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               _samrProtocolAdapter.DMAdminName, out _userHandle, Utilities.USER_READ_GENERAL | Utilities.USER_READ_LOGON | Utilities.USER_READ_ACCOUNT | Utilities.USER_READ_PREFERENCES);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            HRESULT result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInvalidInformation, out userInfo);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "If there is no match on Information Level, the server MUST return an error.");
        }
    }
}
