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
        [Description("Calls SamrEnumerateDomainsInSamServer and expects a successful return.")]
        public void SamrEnumerateDomainsInSamServer_SUCCESS()
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
                string.Format("SamrConnect5, Server:{0}, Desired Access: SAM_SERVER_ENUMERATE_DOMAINS.", _samrProtocolAdapter.PDCNetbiosName));
            hResult = (HRESULT)_samrProtocolAdapter.SamrConnect5(
                _samrProtocolAdapter.PDCNetbiosName,
                (uint)SERVER_ACCESS_MASK.SAM_SERVER_ENUMERATE_DOMAINS,
                out _serverHandle);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrConnect5 must return STATUS_SUCCESS.");
            uint? enumerationContext = 0;
            uint countReturned;
            _SAMPR_ENUMERATION_BUFFER? enumerationBuffer;
            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateDomainsInSamServer, PreferedMaximumLength: 1024.");
            hResult = (HRESULT)SAMRProtocolAdapter.RpcAdapter.SamrEnumerateDomainsInSamServer(
                _serverHandle,
                ref enumerationContext,
                out enumerationBuffer,
                1024,
                out countReturned);
            PtfAssert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrEnumerateDomainsInSamServer must return STATUS_SUCCESS.");
            PtfAssert.AreNotEqual<uint>(0, countReturned, "The CountReturned is not zero.");
            PtfAssert.IsNotNull(enumerationBuffer, "EnumerationBuffer is not null.");
            PtfAssert.AreEqual<uint>(countReturned, enumerationBuffer.Value.EntriesRead, "Verify the EntriesRead property.");
            
            bool builtInDomainFound = false;
            bool accountDomainFound = false;
            foreach (var entry in enumerationBuffer.Value.Buffer)
            {
                string name = DtypUtility.ToString(entry.Name);
                if (string.Compare(name, "BUILTIN", true) == 0) builtInDomainFound = true;
                if (string.Compare(name, _samrProtocolAdapter.primaryDomainNetBIOSName, true) == 0) accountDomainFound = true;
                PtfAssert.AreEqual<uint>(0, entry.RelativeId, "[MS-SAMR]3.1.5.2.1 Buffer.Buffer.RelativeId is 0.");
            }
            PtfAssert.IsTrue(builtInDomainFound, 
                "[MS-SAMR]3.1.5.2.1 The server MUST enable a client to obtain a listing, without duplicates, " +
                "of the following two values: the name attribute of the account domain object and the name" +
                " attribute of the built-in domain object.");
            PtfAssert.IsTrue(accountDomainFound,
                "[MS-SAMR]3.1.5.2.1 The server MUST enable a client to obtain a listing, without duplicates, " +
                "of the following two values: the name attribute of the account domain object and the name" +
                " attribute of the built-in domain object.");
                    
        }
    }
}
