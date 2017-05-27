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
using System.Security.AccessControl;
using System.Security.Principal;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        /// <summary>
        /// Test method of SamrCreateGroupInDomain and SamrDeleteGroup
        /// </summary>
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrCreateGroupInDomain with GROUP_TYPE_SECURITY_ACCOUNT and GROUP_ALL_ACCESS")]
        public void SamrCreateGroupInDomain_GROUP_TYPE_SECURITY_ACCOUNT_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            uint relativeId;
            Site.Log.Add(LogEntryKind.TestStep, "Create a group with name \"{0}\".", groupName);
            CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
            HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeed.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrCreateGroupInDomain with DomainHandle.HandleType not equal to Domain.")]
        public void SamrCreateGroupInDomain_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrCreateGroupInDomain without required access.")]
        public void SamrCreateGroupInDomain_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: create a group without required access.");
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(
                    _domainHandle,
                    testGroupName,
                    (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                    out _groupHandle,
                    out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrCreateGroupInDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _groupHandle, "3.1.5.4.1 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrCreateGroupInDomain with Builtin Domain.")]
        public void SamrCreateGroupInDomain_BuiltinDomain_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, "Builtin", out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

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
