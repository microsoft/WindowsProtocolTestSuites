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
        [Description("Non_DC Test: This is to test SamrOpenAlias and SamrCloseHandle with different DesiredAccess.")]
        public void SamrOpenCloseAlias_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);
            string aliasName = testAliasName;
            uint relativeId;
            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                    testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

            HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                _domainHandle,
                aliasName,
                (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                out _aliasHandle,
                out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
            Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: get rid for {0}.", testAliasName);
            List<string> aliasNames = new List<string>();
            aliasNames.Add(testAliasName);
            List<uint> aliasRids = new List<uint>();
            result = _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, aliasNames, out aliasRids);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrLookupNamesInDomain returns:{0}.", result);
            Site.Assume.IsTrue(aliasRids.Count == 1, "{0} alias(s) with name {1} found.", aliasRids.Count, testAliasName);

            foreach (Common_ACCESS_MASK acc in Enum.GetValues(typeof(Common_ACCESS_MASK)))
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: obtain a handle to an alias with DesiredAccess as {0}.", acc.ToString());
                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)acc, aliasRids[0], out _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenAlias returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened alias handle.");
                result = _samrProtocolAdapter.SamrCloseHandle(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                Site.Assert.AreEqual(IntPtr.Zero, _aliasHandle, "The opened alias handle has been closed.");
            }
            foreach (Generic_ACCESS_MASK acc in Enum.GetValues(typeof(Generic_ACCESS_MASK)))
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: obtain a handle to an alias with DesiredAccess as {0}.", acc.ToString());
                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)acc, aliasRids[0], out _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenAlias returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened alias handle.");
                result = _samrProtocolAdapter.SamrCloseHandle(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                Site.Assert.AreEqual(IntPtr.Zero, _aliasHandle, "The opened alias handle has been closed.");
            }
            foreach (ALIAS_ACCESS_MASK acc in Enum.GetValues(typeof(ALIAS_ACCESS_MASK)))
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: obtain a handle to an alias with DesiredAccess as {0}.", acc.ToString());
                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)acc, aliasRids[0], out _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenalias returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened alias handle.");
                result = _samrProtocolAdapter.SamrCloseHandle(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                Site.Assert.AreEqual(IntPtr.Zero, _aliasHandle, "The opened alias handle has been closed.");
            }
            result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS, aliasRids[0], out _aliasHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenalias returns:{0}.", result);
            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteAlias: delete the created alias.");
            result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeed.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Alias")]
        [Description("Non-DC Test: This is to test SamrOpenAlias with DomainHandle.HandleType not equal to Domain.")]
        public void SamrOpenAlias_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            try
            {
                string aliasName = testAliasName;
                uint relativeId;
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                        testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _domainHandle,
                    aliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);

                IntPtr aliasHandle;
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
                result = _samrProtocolAdapter.SamrOpenAlias(_serverHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS, relativeId, out aliasHandle);
                Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenAlias returns an error:{0}.", result.ToString("X"));
                Site.Assert.AreEqual(IntPtr.Zero, aliasHandle, "The returned alias handle should be zero.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteAlias: delete the created alias.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeed.");
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
        [Description("Non-DC Test: This is to test SamrOpenAlias without required access.")]
        public void SamrOpenAlias_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);
            
            _RPC_SID domainSid;
            try
            {
                string aliasName = testAliasName;
                uint relativeId;
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateAliasInDomain: create an alias with Name:{0}, and DesiredAccess:{1}",
                        testAliasName, ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS);

                HRESULT result = _samrProtocolAdapter.SamrCreateAliasInDomain(
                    _domainHandle,
                    aliasName,
                    (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS,
                    out _aliasHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateAliasInDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_aliasHandle, "The returned alias handle is: {0}.", _aliasHandle);
                
                Site.Log.Add(LogEntryKind.TestStep, "Reopen the domain with DOMAIN_READ access");
                Site.Log.Add(LogEntryKind.TestStep, "SamrLookupDomainInSamServer: obtain the sid of a domain object, given the object's name.");
                result = _samrProtocolAdapter.SamrLookupDomainInSamServer(
                    _serverHandle,
                    _samrProtocolAdapter.domainMemberFqdn,
                    out domainSid);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrLookupDomainInSamServer returns:{0}.", result);
                result = _samrProtocolAdapter.SamrOpenDomain(
                    _serverHandle,
                    Utilities.DOMAIN_READ,
                    domainSid,
                    out _domainHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_domainHandle, "The returned domain handle is:{0}.", result);

                IntPtr aliasHandle;
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: DomainHandle.GrantedAccess MUST have the required access.");
                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS, relativeId, out aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrOpenAlias returns an error:{0}.", result.ToString("X"));
                Site.Assert.AreEqual(IntPtr.Zero, aliasHandle, "The returned alias handle should be zero.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "Reopen the domain with MAXIMUM_ALLOWED access");
                Site.Log.Add(LogEntryKind.TestStep, "SamrLookupDomainInSamServer: obtain the sid of a domain object, given the object's name.");
                HRESULT result = _samrProtocolAdapter.SamrLookupDomainInSamServer(
                    _serverHandle,
                    _samrProtocolAdapter.domainMemberFqdn,
                    out domainSid);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrLookupDomainInSamServer returns:{0}.", result);
                result = _samrProtocolAdapter.SamrOpenDomain(
                    _serverHandle,
                    Utilities.MAXIMUM_ALLOWED,
                    domainSid,
                    out _domainHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenDomain returns:{0}.", result);
                Site.Assert.IsNotNull(_domainHandle, "The returned domain handle is:{0}.", result);

                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteAlias: delete the created alias.");
                result = _samrProtocolAdapter.SamrDeleteAlias(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteAlias succeed.");
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
        [Description("Non-DC Test: This is to test SamrOpenAlias with no such alias.")]
        public void SamrOpenAlias_NoSuchAlias_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            uint invaldRid = 5000;
            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: If no such object exists, the server MUST return an error code.");
            HRESULT result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)ALIAS_ACCESS_MASK.ALIAS_ALL_ACCESS, invaldRid, out _aliasHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_NO_SUCH_ALIAS, result, "SamrOpenAlias returns an error:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _aliasHandle, "The returned alias handle should be zero.");

        }
    }
}
