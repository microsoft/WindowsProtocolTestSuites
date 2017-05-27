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
        /// <summary>
        /// Test method of SamrCreateAliasInDomain and SamrDeleteAlias
        /// </summary>
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrCreateAliasInDomain and SamrDeleteAlias.")]
        public void SamrCreateAliasInDomainAndDelete_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);
            try
            {
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
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteAlias: delete the created alias.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias returns:{0}.", result);
            }
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrCreateAliasInDomain with DomainHandle.HandleType not equal to Domain.")]
        public void SamrCreateAliasInDomain_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create a alias using server handle instead of domain handle.");
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _serverHandle,
                    testAliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _aliasHandle, "3.1.5.4.1 The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrCreateAliasInDomain without required access.")]
        public void SamrCreateAliasInDomain_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create a alias without required access.");
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                   _domainHandle,
                   testAliasName,
                   (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                   out _aliasHandle,
                   out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrCreateAliasInDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _aliasHandle, "3.1.5.4.1 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrCreateAliasInDomain with Builtin Domain.")]
        public void SamrCreateAliasInDomain_BuiltinDomain_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, "Builtin", out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create a alias with Builtin Domain.");
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                   _domainHandle,
                   testAliasName,
                   (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                   out _aliasHandle,
                   out relativeId);
            Site.Log.Add(LogEntryKind.Debug, "SamrCreateAliasInDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result,
                "3.1.5.4.1 If DomainHandle.Object refers to the built-in domain, the server MUST abort the request and return a failure code.");
        }
    }
}
