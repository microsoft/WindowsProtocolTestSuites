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
        static string GetPdcDnsName()
        {
            return _samrProtocolAdapter.PDCNetbiosName + "." + _samrProtocolAdapter.PrimaryDomainDnsName;
        }

        [TestCategory("MS-SAMR")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("SAMR-Password")]
        [Description("Invokes SamrGetUserDomainPasswordInformation. A successful return is expected.")]
        [TestMethod]
        public void SamrGetUserDomainPasswordInformation_SUCCESS()
        {
            HRESULT hResult;
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            ConnectAndOpenUser(
                GetPdcDnsName(),
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.ClientUserName,
                out _userHandle,
                (uint)DOMAIN_ACCESS_MASK.DOMAIN_READ_PASSWORD_PARAMETERS);
            _USER_DOMAIN_PASSWORD_INFORMATION passwordInformation;
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                string.Format("Invoke SamrGetUserDomainPasswordInformation."));
            hResult = _samrProtocolAdapter.SamrGetUserDomainPasswordInformation(_userHandle, out passwordInformation);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrGetUserDomainPasswordInformation returns success.");

            var attributes = Microsoft.Protocols.TestSuites.ActiveDirectory.Common.Utilities.GetAttributesFromEntry(
                _samrProtocolAdapter.primaryDomainDN,
                new string[] { "minPwdLength", "pwdProperties" },
                GetPdcDnsName(),
                _samrProtocolAdapter.ADDSPortNum);

            PtfAssert.AreEqual(
                (int)attributes["minPwdLength"],
                passwordInformation.MinPasswordLength,
                "The output parameter PasswordInformation.MinPasswordLength MUST be set to the Effective-MinimumPasswordLength attribute value (see section 3.1.1.5).");

            PtfAssert.AreEqual(
                (uint)(int)attributes["pwdProperties"],
                passwordInformation.PasswordProperties,
                "The output parameter PasswordInformation.PasswordProperties MUST be set to the pwdProperties attribute value on the account domain object.");

        }
    }
}
