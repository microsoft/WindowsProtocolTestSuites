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
        [Description("Calls SamrConnect4 with domain admin account and SAM_SERVER_READ access. Expects a successful return.")]
        [TestMethod]
        public void SamrConnect4_SUCCESS()
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
                string.Format("SamrConnect4, Server:{0}, DesiredAccess: SAM_SERVER_READ.",
                _samrProtocolAdapter.pdcNetBIOSName));

            HRESULT methodStatus = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrConnect4(
                _samrProtocolAdapter.pdcNetBIOSName,
                out _serverHandle,
                0x02u,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_READ);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrConnect4 returns STATUS_SUCCESS");
            PtfAssert.AreNotEqual(IntPtr.Zero, _serverHandle, "SamrConnect4 returns a non-NULL handle.");
        }
    }
}
