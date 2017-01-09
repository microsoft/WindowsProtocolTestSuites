// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.DirectoryServices;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestCategory("MS-SAMR")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates temp user SAMRTestUser and changes the password using SamrChangePasswordUser. A successful return is expected.")]
        [TestMethod]
        public void SamrChangePasswordUser_SUCCESS()
        {
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            string oldPassword = "Password01!";
            string newPassword = "drowssaP02!";


            CreateTempUser createTempUser = new CreateTempUser(
                _samrProtocolAdapter,
                testUserName,
                oldPassword,
                AdtsUserAccountControl.ADS_UF_NORMAL_ACCOUNT);
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                string.Format("Create test user, username:{0} password:{1}.", testUserName, oldPassword));
            Common.UpdatesStorage.GetInstance().PushUpdate(createTempUser);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                string.Format("Set user must change password to user:{0}.", testUserName));
            Common.Utilities.UserMustChangePassword(
                _samrProtocolAdapter.pdcFqdn,
                _samrProtocolAdapter.ADDSPortNum,
                _samrProtocolAdapter.primaryDomainUserContainerDN,
                testUserName);

            ConnectAndOpenUser(
                GetPdcDnsName(),
                _samrProtocolAdapter.primaryDomainFqdn,
                testUserName,
                out _userHandle);

            SamrCryptography samrCryptography = new SamrCryptography(oldPassword, newPassword);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrChangePasswordUser, OldPassword:{0}, NewPassword:{1}.", oldPassword, newPassword));
            HRESULT hresult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrChangePasswordUser(
                                            _userHandle,
                                            1,
                                            samrCryptography.GetOldLmEncryptedWithNewLm(),
                                            samrCryptography.GetNewLmEncryptedWithOldLm(),
                                            1,
                                            samrCryptography.GetOldNtEncryptedWithNewNt(),
                                            samrCryptography.GetNewNtEncryptedWithOldNt(),
                                            0,
                                            samrCryptography.GetNewNtEncryptedWithNewLm(),
                                            0,
                                            samrCryptography.GetNewLmEncryptedWithNewNt());
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hresult, "SamrChangePasswordUser returns success.");
        }
    }
}
