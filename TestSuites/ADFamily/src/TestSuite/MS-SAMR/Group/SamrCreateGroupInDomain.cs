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
        /// <summary>
        /// Test method of SamrCreateGroupInDomain and SamrDeleteGroup
        /// </summary>
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("BVT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrCreateGroupInDomain with GROUP_TYPE_SECURITY_ACCOUNT and GROUP_ALL_ACCESS.")]
        public void SamrCreateGroupInDomain_GROUP_TYPE_SECURITY_ACCOUNT()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: create a group with Name:{0}, AccountType:{1}, and DesiredAccess:{2}",
                    testGroupName, GROUP_TYPE.GROUP_TYPE_SECURITY_ACCOUNT, Group_ACCESS_MASK.GROUP_ALL_ACCESS);
                uint relativeId;
                HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(
                    _domainHandle,
                    testGroupName,
                    (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                    out _groupHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain succeeded.");
                Site.Assert.AreNotEqual(IntPtr.Zero, _groupHandle, "The returned group handle is: {0}.", _groupHandle);
                Site.Assert.IsTrue(_samrProtocolAdapter.VerifyRelativeID(relativeId), "The Rid value MUST be within the Rid-Range");
                _samrProtocolAdapter.VerifyConstraintsForGroup(testGroupName);
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeed.");
            }
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrCreateGroupInDomain with DomainHandle.HandleType not equal to Domain.")]
        public void SamrCreateGroupInDomain_InvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: create a group using server handle instead of domain handle.");
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(
                    _serverHandle,
                    testGroupName,
                    (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                    out _groupHandle,
                    out relativeId);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _groupHandle, "3.1.5.4.1 The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrCreateGroupInDomain without required access.")]
        public void SamrCreateGroupInDomain_STATUS_ACCESS_DENIED()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: create a group without required access.");
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(
                    _domainHandle,
                    testGroupName,
                    (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                    out _groupHandle,
                    out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrCreateGroupInDomain returns error code:{0}.", result.ToString("X"));
            // [MS-SAMR] Section 3.1.2.1 Standard Handle-Based Access Checks
            // For SamrCreateGroupInDomain Operation: DomainHandle.GrantedAccess MUST have the DOMAIN_CREATE_GROUP access.
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "3.1.5.4.1 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrCreateGroupInDomain with Builtin Domain.")]
        public void SamrCreateGroupInDomain_BuiltinDomain()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, builtinDomainName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: create a group with Builtin Domain.");
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(
                    _domainHandle,
                    testGroupName,
                    (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                    out _groupHandle,
                    out relativeId);
            Site.Log.Add(LogEntryKind.Debug, "SamrCreateGroupInDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result,
                "3.1.5.4.1 If DomainHandle.Object refers to the built-in domain, the server MUST abort the request and return a failure code.");
        }
    }
}
