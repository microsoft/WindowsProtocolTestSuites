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
        [Description("Calls SamrConnect5 with domain admin account and SAM_SERVER_READ access. Expects a successful return.")]
        [TestMethod]
        public void SamrConnect5_SUCCESS()
        {
            IChecker PtfAssert = TestClassBase.BaseTestSite.Assert;
            Site.Log.Add(LogEntryKind.TestStep, 
                string.Format("SamrBind: Server:{0}, Domain:{1}, User:{2}, Password{3}.",
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
                string.Format("SamrConnect5: Server:{0}, DesiredAccess: SAM_SERVER_READ.",
                _samrProtocolAdapter.pdcNetBIOSName));

            SAMPR_REVISION_INFO[] inRevisionInfo = new SAMPR_REVISION_INFO[1];
            inRevisionInfo[0] = new SAMPR_REVISION_INFO();
            inRevisionInfo[0].V1.Revision = _SAMPR_REVISION_INFO_V1_Revision_Values.V3;
            inRevisionInfo[0].V1.SupportedFeatures = SupportedFeatures_Values.V1;

            uint outVersion;
            SAMPR_REVISION_INFO outRevisionInfo;
            HRESULT methodStatus = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrConnect5(
                _samrProtocolAdapter.pdcNetBIOSName,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_READ,
                0x01u,
                inRevisionInfo[0],
                out outVersion,
                out outRevisionInfo,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "[MS-SAMR] 3.1.5.1.1 Otherwise, the server MUST return STATUS_SUCCESS.");
            PtfAssert.AreEqual(1u, outVersion, 
                "[MS-SAMR] 3.1.5.1.1 The server MUST set OutVersion to 1 and OutRevisionInfo.Revision to 3.");
            PtfAssert.AreEqual(3u, (uint)outRevisionInfo.V1.Revision, 
                "[MS-SAMR] 3.1.5.1.1 The server MUST set OutVersion to 1 and OutRevisionInfo.Revision to 3.");
            PtfAssert.AreEqual(0u, (uint)outRevisionInfo.V1.SupportedFeatures, "[MS-SAMR] 3.1.5.1.1 The remaining fields of OutRevisionInfo MUST be set to zero.");
            PtfAssert.AreNotEqual(IntPtr.Zero, _serverHandle, "SamrConnect5 returns a non-NULL handle.");
        }
    }
}
