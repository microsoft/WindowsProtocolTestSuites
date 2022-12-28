// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and set password using Internal5. A successful return is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal4_NoWhichFields()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPassword();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal4 = new _SAMPR_USER_INTERNAL4_INFORMATION()
            {
                UserPassword = encrypted
            };


            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal4Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal4Information, expectUserInfo);

            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and set password using Internal5. A successful return is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal4_NTPasswordPresent()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPassword();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal4 = new _SAMPR_USER_INTERNAL4_INFORMATION()
            {
                I1 = new _SAMPR_USER_ALL_INFORMATION(),
                UserPassword = encrypted
            };
            expectUserInfo.Internal4.I1.WhichFields = (uint)USER_ALLValue.USER_ALL_NTPASSWORDPRESENT;

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal4Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal4Information, expectUserInfo);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and set password using Internal5. A successful return is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal4_LMPasswordPresent()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPassword();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal4 = new _SAMPR_USER_INTERNAL4_INFORMATION()
            {
                I1 = new _SAMPR_USER_ALL_INFORMATION(),
                UserPassword = encrypted
            };
            expectUserInfo.Internal4.I1.WhichFields = (uint)USER_ALLValue.USER_ALL_NTPASSWORDPRESENT;


            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal4Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal4Information, expectUserInfo);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and set password using Internal5. A successful return is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal5_PasswordExpired()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPassword();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal5 = new _SAMPR_USER_INTERNAL5_INFORMATION()
            {
                PasswordExpired = 1,
                UserPassword = encrypted
            };

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal5Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal5Information, expectUserInfo);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and set password using Internal5. A successful return is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal5_PasswordNotExpired()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPassword();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal5 = new _SAMPR_USER_INTERNAL5_INFORMATION()
            {
                PasswordExpired = 0,
                UserPassword = encrypted
            };

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal5Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal5Information, expectUserInfo);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and attempt to set password using Internal4New without setting the WhichFields. An error is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal4New_NoWhichFields()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPasswordNew();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal4New = new _SAMPR_USER_INTERNAL4_INFORMATION_NEW()
            {
                UserPassword = encrypted
            };

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal4Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal4InformationNew, expectUserInfo);

            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and set password using Internal4New. A successful return is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal4New_NTPasswordPresent()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPasswordNew();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal4New = new _SAMPR_USER_INTERNAL4_INFORMATION_NEW()
            {
                I1 = new _SAMPR_USER_ALL_INFORMATION(),
                UserPassword = encrypted
            };
            expectUserInfo.Internal4New.I1.WhichFields = (uint)USER_ALLValue.USER_ALL_NTPASSWORDPRESENT;

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal4Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal4InformationNew, expectUserInfo);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and set password using Internal4New. A successful return is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal4New_LMPasswordPresent()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPasswordNew();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal4New = new _SAMPR_USER_INTERNAL4_INFORMATION_NEW()
            {
                I1 = new _SAMPR_USER_ALL_INFORMATION(),
                UserPassword = encrypted
            };
            expectUserInfo.Internal4New.I1.WhichFields = (uint)USER_ALLValue.USER_ALL_NTPASSWORDPRESENT;


            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal4Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal4InformationNew, expectUserInfo);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and set password using Internal5New. A successful return is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal5New_PasswordExpired()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPasswordNew();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal5New = new _SAMPR_USER_INTERNAL5_INFORMATION_NEW()
            {
                PasswordExpired = 1,
                UserPassword = encrypted
            };

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal5Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal5InformationNew, expectUserInfo);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates user and set password using Internal5New. A successful return is expected.")]
        [TestMethod]
        public void SamrSetInformationUser2_Internal5New_PasswordNotExpired()
        {
            GetUserHandle();

            var encrypted = GetSamprEncryptedUserPasswordNew();

            _SAMPR_USER_INFO_BUFFER expectUserInfo = new _SAMPR_USER_INFO_BUFFER();
            expectUserInfo.Internal5New = new _SAMPR_USER_INTERNAL5_INFORMATION_NEW()
            {
                PasswordExpired = 0,
                UserPassword = encrypted
            };

            Site.Log.Add(LogEntryKind.TestStep, "SamrSetInformationUser2 with UserInternal5Information.");
            var result = _samrProtocolAdapter.SamrSetInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserInternal5InformationNew, expectUserInfo);

            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrSetInformationUser2 returns:{0}.", result);
        }

        private _SAMPR_ENCRYPTED_USER_PASSWORD GetSamprEncryptedUserPassword()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Getting session key");
            var sessionKey = SAMRProtocolAdapter.RpcAdapter.SessionKey;

            var oldPassword = "Password01!";
            var newPassword = "drowssaP02!";
            var cryptoTool = new SamrCryptography(oldPassword, newPassword);
            _SAMPR_ENCRYPTED_USER_PASSWORD encrypted = cryptoTool.GetNewPasswordEncryptedWithSessionKey(sessionKey);

            Site.Log.Add(LogEntryKind.TestStep, "Got encrypted password");
            return encrypted;
        }

        private _SAMPR_ENCRYPTED_USER_PASSWORD_NEW GetSamprEncryptedUserPasswordNew()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Getting session key");
            var sessionKey = SAMRProtocolAdapter.RpcAdapter.SessionKey;

            var oldPassword = "Password01!";
            var newPassword = "drowssaP02!";

            Random rnd = new Random();
            byte[] salt = new byte[16];
            rnd.NextBytes(salt);

            var cryptoTool = new SamrCryptography(oldPassword, newPassword);
            _SAMPR_ENCRYPTED_USER_PASSWORD_NEW encrypted = cryptoTool.GetPasswordEncryptedWithSalt(salt, sessionKey);

            Site.Log.Add(LogEntryKind.TestStep, "Got encrypted password");
            return encrypted;
        }

        private void GetUserHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle, needSessionKey: true);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, Common_ACCESS_MASK.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryInformationUser2: obtain attributes from a user object.");
            _SAMPR_USER_INFO_BUFFER? userInfo;
            result = _samrProtocolAdapter.SamrQueryInformationUser2(_userHandle, _USER_INFORMATION_CLASS.UserAllInformation, out userInfo);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryInformationUser2 query UserAllInformation with MAXIMUM_ALLOWED successfully.");
            Site.Assert.IsNotNull(userInfo, "The returned userInfo should not be null.");
        }
    }
}
