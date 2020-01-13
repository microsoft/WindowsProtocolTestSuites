// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Server")]
        [Description("Calls SamrCloseHandle to close a server handle. Expects a successful return.")]
        [TestMethod]
        public void SamrCloseHandle_Server()
        {
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrBind, Server:{0}, Domain:{1}, User:{2}, Password{3}.",
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword));
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword,
                false,
                true);

            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrConnect, Server:{0}, DesiredAccess: SAM_SERVER_READ.",
                _samrProtocolAdapter.pdcNetBIOSName));

            HRESULT hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrConnect(
                _samrProtocolAdapter.pdcNetBIOSName,
                out _serverHandle,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_READ);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect returns STATUS_SUCCESS");
            Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle");
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrCloseHandle(
                ref _serverHandle);
            PtfAssert.AreEqual<IntPtr>(IntPtr.Zero, _serverHandle, "[MS-SAMR] 3.1.5.13.1 ... MUST return 0 for the value SamHandle.");
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrCloseHandle returns STATUS_SUCCESS");

        }
    }
}
