// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.DirectoryServices;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestCategory("MS-SAMR")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Creates temp user SAMRTestUser and changes the password using SamrUnicodeChangePasswordUser2. A successful return is expected.")]
        [TestMethod]
        public void SamrUnicodeChangePasswordUser2_SUCCESS()
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

            Site.Log.Add(LogEntryKind.TestStep, "Initialize: Create Samr Bind to the server.");
            _samrProtocolAdapter.SamrBind(
                GetPdcDnsName(),
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword,
                false,
                true);

            SamrCryptography samrCryptography = new SamrCryptography(oldPassword, newPassword);
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrUnicodeChangePasswordUser2, OldPassword:{0}, NewPassword:{1}.", oldPassword, newPassword));
            HRESULT hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrUnicodeChangePasswordUser2(
                SAMRProtocolAdapter.RpcAdapter.Handle,
                DtypUtility.ToRpcUnicodeString(_samrProtocolAdapter.pdcNetBIOSName),
                DtypUtility.ToRpcUnicodeString(testUserName),
                samrCryptography.GetNewPasswordEncryptedWithOldNt(),
                samrCryptography.GetOldNtEncryptedWithNewNt(),
                0x01,
                samrCryptography.GetNewPasswordEncryptedWithOldLm(PasswordType.Unicode),
                samrCryptography.GetOldLmOwfPasswordEncryptedWithNewNt());

            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrOemChangePasSamrUnicodeChangePasswordUser2swordUser2 returns success.");
        }
    }
}
