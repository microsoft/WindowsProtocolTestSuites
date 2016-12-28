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
        [Description("Invokes SamrGetDomainPasswordInformation. A successful return is expected.")]
        [TestMethod]
        public void SamrGetDomainPasswordInformation_SUCCESS()
        {
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;

            Site.Log.Add(LogEntryKind.TestStep, "Initialize: Create Samr Bind to the server.");
            _samrProtocolAdapter.SamrBind(
                GetPdcDnsName(),
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword,
                false,
                true);

            _USER_DOMAIN_PASSWORD_INFORMATION passwordInformation;

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                string.Format("Invoke SamrGetUserDomainPasswordInformation."));
            HRESULT hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrGetDomainPasswordInformation(
                SAMRProtocolAdapter.RpcAdapter.Handle,
                DtypUtility.ToRpcUnicodeString(""),
                out passwordInformation);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrGetUserDomainPasswordInformation returns success.");

            var attributes = Microsoft.Protocols.TestSuites.ActiveDirectory.Common.Utilities.GetAttributesFromEntry(
                _samrProtocolAdapter.primaryDomainDN,
                new string[] { "minPwdLength", "pwdProperties" },
                GetPdcDnsName(),
                _samrProtocolAdapter.ADDSPortNum);

            PtfAssert.AreEqual(
                (int)attributes["minPwdLength"],
                passwordInformation.MinPasswordLength,
                "[MS-SAMR] 3.1.5.13.3 The output parameter PasswordInformation.MinPasswordLength MUST be set to the minPwdLength attribute value on the account domain object.");

            PtfAssert.AreEqual(
                (uint)(int)attributes["pwdProperties"],
                passwordInformation.PasswordProperties,
                "[MS-SAMR] 3.1.5.13.3 The output parameter PasswordInformation.PasswordProperties MUST be set to the pwdProperties attribute value on the account domain object.");

        }
    }
}
