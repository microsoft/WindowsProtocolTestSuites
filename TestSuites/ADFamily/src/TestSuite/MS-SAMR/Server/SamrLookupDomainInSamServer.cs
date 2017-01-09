// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Server")]
        [Description("Calls SamrLookupDomainInSamServer to find the primary domain in the server and expects a successful return.")]
        public void SamrLookupDomainInSamServer_PrimaryDomain()
        {
            HRESULT hResult;
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
                string.Format("SamrConnect5, Server:{0}, Desired Access: SAM_SERVER_LOOKUP_DOMAIN.", _samrProtocolAdapter.PDCNetbiosName));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.PDCNetbiosName,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_LOOKUP_DOMAIN,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect5 must return STATUS_SUCCESS.");

            Site.Log.Add(LogEntryKind.TestStep,
                string.Format("SamrLookupDomainInSamServer, Name: {0}.", _samrProtocolAdapter.primaryDomainNetBIOSName));
            _RPC_SID? domainID;
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrLookupDomainInSamServer(
                _serverHandle,
                DtypUtility.ToRpcUnicodeString(_samrProtocolAdapter.primaryDomainNetBIOSName),
                out domainID);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrLookupDomainInSamServer must return STATUS_SUCCESS.");
            PtfAssert.IsNotNull(domainID, "DomainId is not null.");
            string domainSid = DtypUtility.ToSddlString(domainID.Value);
            PtfAssert.AreEqual(
                _samrProtocolAdapter.PrimaryDomainSID,
                domainSid,
                "The objectSid of the primary domain must be returnd.");
        }
    }
}
