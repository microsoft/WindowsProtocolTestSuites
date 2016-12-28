// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.DirectoryServices;
using System.Security.Principal;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrRidToSid with right Rid.")]
        public void SamrRidToSid_Alias_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string aliasName = testAliasName;
            uint relativeId;
            try
            {
                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _domainHandle,
                    aliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);
                Site.Assert.IsTrue(relativeId >= 1000, "The Rid value MUST be greater than or equal to 1000");

                Site.Log.Add(LogEntryKind.TestStep, "SamrRidToSid: obtain the SID of an account, given a RID.");
                _RPC_SID? sid;
                result = _samrProtocolAdapter.SamrRidToSid(_aliasHandle, relativeId, out sid);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrRidToSid returns:{0}", result);
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteAlias: delete the created alias.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias returns:{0}.", result);
            }
        }
    }
}
