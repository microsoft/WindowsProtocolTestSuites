// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Modeling;
using System.Collections.Generic;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;


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
        [Description("Non-DC Test: This is to test SamrDeleteAlias.")]
        public void SamrDeleteAlias_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);
            
            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                    testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                _domainHandle,
                testAliasName,
                (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                out _aliasHandle,
                out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);
            Site.Assert.IsTrue(relativeId >= 1000, "The Rid value MUST be greater than or equal to 1000");

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteAlias: delete the created alias.");
            result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: lookup the alias: {0}.", testAliasName);
            List<string> aliasNames = new List<string>();
            aliasNames.Add(testAliasName);
            List<uint> aliasRids = new List<uint>();
            result = _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, aliasNames, out aliasRids);
            Site.Assert.AreEqual(HRESULT.STATUS_NONE_MAPPED, result, "The alias: {0} should not be found again.", testAliasName);          
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrDeleteAlias with AliasHandle.HandleType not equal to Alias.")]
        public void SamrDeleteAlias_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteAlias with invalid handle.");
            HRESULT result = _samrProtocolAdapter.SamrDeleteAlias(ref _domainHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.1 The server MUST return an error if AliasHandle.HandleType is not equal to Alias.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrDeleteAlias with no required access.")]
        public void SamrDeleteAlias_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);
            string aliasName = testAliasName;
            uint relativeId = 0;
            try
            {
                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                 _domainHandle,
                 aliasName,
                 (uint)ALIAS_ACCESS_MASK.ALIAS_READ,
                 out _aliasHandle,
                 out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);
                Site.Assert.IsTrue(relativeId >= 1000, "The Rid value MUST be greater than or equal to 1000");

                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteAlias: delete the created alias.");
                result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "3.1.5.7.1 AliasHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: obtain the handle to the created alias with ALIAS_ALL_ACCESS.");
                HRESULT result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS, relativeId, out _aliasHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: delete the created alias.");
                result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeeded.");
            }
        }

        
    }
}
