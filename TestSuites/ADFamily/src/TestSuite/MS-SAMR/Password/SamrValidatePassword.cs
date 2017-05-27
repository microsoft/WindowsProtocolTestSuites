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
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestCategory("MS-SAMR")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Test SamrValidatePassword ValidateAuthentication. A successful return is expected.")]
        [TestMethod]
        public void SamrValidatePassword_Auth_Success()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;

            ConnectAndOpenDomain(
                GetPdcDnsName(),
                _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle,
                out _domainHandle);

            _SAM_VALIDATE_INPUT_ARG inputArg = new _SAM_VALIDATE_INPUT_ARG();
            inputArg.ValidateAuthenticationInput = new _SAM_VALIDATE_AUTHENTICATION_INPUT_ARG();

            DateTime lockOutTime = DateTime.Now - TimeSpan.FromDays(5);
            DateTime lastSetTime = DateTime.Now - TimeSpan.FromDays(6);
            _FILETIME lockout = DtypUtility.ToFileTime(lockOutTime);
            _FILETIME lastset = DtypUtility.ToFileTime(lastSetTime);
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                string.Format("Create InputArg for SamrValidatePassword, LockoutTime:{0}, PasswordLastSet:{1}, PasswordMatched:{2}, BadPasswordCount: {3}, PasswordHistoryLength: {4}.",
                lockOutTime.ToString(),
                lastSetTime.ToString(),
                1,
                1,
                0));
            inputArg.ValidateAuthenticationInput.InputPersistedFields.LockoutTime.QuadPart = (((long)lockout.dwHighDateTime) << 32) | lockout.dwLowDateTime;
            inputArg.ValidateAuthenticationInput.InputPersistedFields.PasswordLastSet.QuadPart = (((long)lastset.dwHighDateTime) << 32) | lastset.dwLowDateTime;
            inputArg.ValidateAuthenticationInput.PasswordMatched = 1;
            inputArg.ValidateAuthenticationInput.InputPersistedFields.BadPasswordCount = 1;
            inputArg.ValidateAuthenticationInput.InputPersistedFields.PasswordHistoryLength = 0;
            inputArg.ValidateAuthenticationInput.InputPersistedFields.PasswordHistory = new _SAM_VALIDATE_PASSWORD_HASH[] { };

            BaseTestSite.Log.Add(LogEntryKind.TestStep, string.Format("Invoke SamrValidatePassword."));

            _SAM_VALIDATE_OUTPUT_ARG? outputArg;
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrValidatePassword(
                _domainHandle,
                _PASSWORD_POLICY_VALIDATION_TYPE.SamValidateAuthentication,
                inputArg,
                out outputArg
                );
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrGetUserDomainPasswordInformation returns success.");
            PtfAssert.AreEqual(0, outputArg.Value.ValidateAuthenticationOutput.ChangedPersistedFields.LockoutTime.QuadPart,
                "[MS-SAMR] 3.1.5.13.7.1 LockoutTime MUST be set to 0 (and continue processing).");
            PtfAssert.AreEqual(_SAM_VALIDATE_VALIDATION_STATUS.SamValidateSuccess, outputArg.Value.ValidateAuthenticationOutput.ValidationStatus,
                "[MS-SAMR] 3.1.5.13.7.1 ValidationStatus MUST be set to SamValidateSuccess.");
            PtfAssert.AreEqual(0u, outputArg.Value.ValidateAuthenticationOutput.ChangedPersistedFields.BadPasswordCount,
                "[MS-SAMR] 3.1.5.13.7.1 If BadPasswordCount is nonzero, BadPasswordCount MUST be set to 0.");
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Test SamrValidatePassword SamValidatePasswordChange. A successful return is expected.")]
        [TestMethod]
        public void SamrValidatePassword_Change_Success()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;

            ConnectAndOpenDomain(
                GetPdcDnsName(),
                _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle,
                out _domainHandle);

            _SAM_VALIDATE_INPUT_ARG inputArg = new _SAM_VALIDATE_INPUT_ARG();
            inputArg.ValidatePasswordChangeInput = new _SAM_VALIDATE_PASSWORD_CHANGE_INPUT_ARG();

            DateTime lockOutTime = DateTime.Now - TimeSpan.FromDays(5);
            DateTime lastSetTime = DateTime.Now - TimeSpan.FromDays(6);
            _FILETIME lockout = DtypUtility.ToFileTime(lockOutTime);
            _FILETIME lastset = DtypUtility.ToFileTime(lastSetTime);
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                string.Format("Create InputArg for SamValidatePasswordChange, LockoutTime:{0}, PasswordLastSet:{1}, PasswordMatched:{2}, BadPasswordCount: {3}, PasswordHistoryLength: {4}.",
                lockOutTime.ToString(),
                lastSetTime.ToString(),
                1,
                1,
                0));
            inputArg.ValidatePasswordChangeInput.InputPersistedFields.LockoutTime.QuadPart = (((long)lockout.dwHighDateTime) << 32) | lockout.dwLowDateTime;
            inputArg.ValidatePasswordChangeInput.InputPersistedFields.PasswordLastSet.QuadPart = (((long)lastset.dwHighDateTime) << 32) | lastset.dwLowDateTime;
            inputArg.ValidatePasswordChangeInput.PasswordMatch = 1;
            inputArg.ValidatePasswordChangeInput.ClearPassword = DtypUtility.ToRpcUnicodeString("drowssaP02!");
            inputArg.ValidatePasswordChangeInput.InputPersistedFields.BadPasswordCount = 1;
            inputArg.ValidatePasswordChangeInput.InputPersistedFields.PasswordHistoryLength = 0;
            inputArg.ValidatePasswordChangeInput.InputPersistedFields.PasswordHistory = new _SAM_VALIDATE_PASSWORD_HASH[] { };
            inputArg.ValidatePasswordChangeInput.HashedPassword = new _SAM_VALIDATE_PASSWORD_HASH()
            {
                Hash = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF },
                Length = 4
            };

            BaseTestSite.Log.Add(LogEntryKind.TestStep, string.Format("Invoke SamrValidatePassword."));

            _SAM_VALIDATE_OUTPUT_ARG? outputArg;
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrValidatePassword(
                _domainHandle,
                _PASSWORD_POLICY_VALIDATION_TYPE.SamValidatePasswordChange,
                inputArg,
                out outputArg
                );
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrValidatePassword returns success.");
            PtfAssert.AreEqual(_SAM_VALIDATE_VALIDATION_STATUS.SamValidateSuccess, outputArg.Value.ValidatePasswordChangeOutput.ValidationStatus,
                "[MS-SAMR]3.1.5.13.7.2 ValidationStatus MUST be set to SamValidateSuccess.");
            PtfAssert.AreEqual(0, outputArg.Value.ValidatePasswordChangeOutput.ChangedPersistedFields.LockoutTime.QuadPart,
                "[MS-SAMR]3.1.5.13.7.2 If LockoutTime plus DomainLockoutDuration is less than or equal to the current time, LockoutTime MUST be set to 0.");
            PtfAssert.AreEqual(0u, outputArg.Value.ValidatePasswordChangeOutput.ChangedPersistedFields.BadPasswordCount,
                "[MS-SAMR]3.1.5.13.7.2 BadPasswordCount is set to 0.");

        }

        [TestCategory("MS-SAMR")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Password")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [Description("Test SamrValidatePassword SamValidatePasswordReset. A successful return is expected.")]
        [TestMethod]
        public void SamrValidatePassword_Reset_Success()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;

            ConnectAndOpenDomain(
                GetPdcDnsName(),
                _samrProtocolAdapter.PrimaryDomainDnsName,
                out _serverHandle,
                out _domainHandle);

            _SAM_VALIDATE_INPUT_ARG inputArg = new _SAM_VALIDATE_INPUT_ARG();
            inputArg.ValidatePasswordResetInput = new _SAM_VALIDATE_PASSWORD_RESET_INPUT_ARG();

            DateTime lockOutTime = DateTime.Now - TimeSpan.FromDays(5);
            DateTime lastSetTime = DateTime.Now - TimeSpan.FromDays(6);
            _FILETIME lockout = DtypUtility.ToFileTime(lockOutTime);
            _FILETIME lastset = DtypUtility.ToFileTime(lastSetTime);
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                string.Format("Create InputArg for SamValidatePasswordReset, LockoutTime:{0}, PasswordLastSet:{1}, PasswordMustChangeAtNextLogon: {2}, PasswordHistoryLength: {3}.",
                lockOutTime.ToString(),
                lastSetTime.ToString(),
                1,
                0));
            inputArg.ValidatePasswordResetInput.InputPersistedFields.LockoutTime.QuadPart = (((long)lockout.dwHighDateTime) << 32) | lockout.dwLowDateTime;
            inputArg.ValidatePasswordResetInput.InputPersistedFields.PasswordLastSet.QuadPart = (((long)lastset.dwHighDateTime) << 32) | lastset.dwLowDateTime;
            inputArg.ValidatePasswordResetInput.ClearPassword = DtypUtility.ToRpcUnicodeString("drowssaP02!");
            inputArg.ValidatePasswordResetInput.InputPersistedFields.BadPasswordCount = 1;
            inputArg.ValidatePasswordResetInput.PasswordMustChangeAtNextLogon = 1;
            inputArg.ValidatePasswordResetInput.InputPersistedFields.PasswordHistoryLength = 0;
            inputArg.ValidatePasswordResetInput.ClearLockout = 1;
            inputArg.ValidatePasswordResetInput.InputPersistedFields.PasswordHistory = new _SAM_VALIDATE_PASSWORD_HASH[] { };
            inputArg.ValidatePasswordResetInput.HashedPassword = new _SAM_VALIDATE_PASSWORD_HASH()
            {
                Hash = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF },
                Length = 4
            };

            BaseTestSite.Log.Add(LogEntryKind.TestStep, string.Format("Invoke SamrValidatePassword."));

            _SAM_VALIDATE_OUTPUT_ARG? outputArg;
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrValidatePassword(
                _domainHandle,
                _PASSWORD_POLICY_VALIDATION_TYPE.SamValidatePasswordReset,
                inputArg,
                out outputArg
                );
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrValidatePassword returns success.");
            PtfAssert.AreEqual(_SAM_VALIDATE_VALIDATION_STATUS.SamValidateSuccess, outputArg.Value.ValidatePasswordResetOutput.ValidationStatus,
                "[MS-SAMR]3.1.5.13.7.3 ValidationStatus MUST be set to SamValidateSuccess.");
            PtfAssert.AreEqual(0, outputArg.Value.ValidatePasswordResetOutput.ChangedPersistedFields.PasswordLastSet.QuadPart,
                "[MS-SAMR]3.1.5.13.7.3 If PasswordMustChangeAtNextLogon is nonzero, PasswordLastSet MUST be set to 0.");
            PtfAssert.AreEqual(0, outputArg.Value.ValidatePasswordResetOutput.ChangedPersistedFields.LockoutTime.QuadPart,
                "[MS-SAMR]3.1.5.13.7.3 LockoutTime MUST be set to 0.");
            PtfAssert.AreEqual(0u, outputArg.Value.ValidatePasswordResetOutput.ChangedPersistedFields.BadPasswordCount,
                "[MS-SAMR]3.1.5.13.7.3 If ValidatePasswordResetInput.InputPersistedFields.BadPasswordCount is nonzero, BadPasswordCount MUST be set to 0.");

        }
    }
}
