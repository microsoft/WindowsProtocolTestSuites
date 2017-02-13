// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.DirectoryServices;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("BVT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrDeleteGroup.")]
        public void SamrDeleteGroup()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: Create a group with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                testGroupName, GROUP_TYPE.GROUP_TYPE_SECURITY_ACCOUNT, Group_ACCESS_MASK.GROUP_ALL_ACCESS);
            uint relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(
                _domainHandle,
                testGroupName,
                (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                out _groupHandle,
                out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain succeeded.");
            Site.Assert.AreNotEqual(IntPtr.Zero, _groupHandle, "The returned user handle is: {0}.", _groupHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
            result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeed.");
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "3.1.5.7.1 The server MUST delete the SamContextHandle ADM element (section 3.1.1.10) represented by GroupHandle, and then MUST return 0 for the value of GroupHandle and a return code of STATUS_SUCCESS.");
            string groupPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, testGroupName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            Site.Assert.IsFalse(DirectoryEntry.Exists(groupPath), "3.1.5.7.1 G MUST be removed from the database.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrDeleteGroup whose Rid is less than 1000.")]
        public void SamrDeleteGroup_SmallRid()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: obtain the handle to the domain users group.");
            HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS, Utilities.DOMAIN_GROUP_RID_USERS, out _groupHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the domain users group.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _groupHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.1 If the RID of G's objectSid attribute value is less than 1000, an error MUST be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrDeleteGroup with GroupHandle.HandleType not equal to Group.")]
        public void SamrDeleteGroup_InvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup with invalid handle.");
            HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _domainHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.1 The server MUST return an error if GroupHandle.HandleType is not equal to Group.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrDeleteGroup with no required access.")]
        public void SamrDeleteGroup_STATUS_ACCESS_DENIED()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            uint relativeId = 0;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: Create a group with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                    testGroupName, GROUP_TYPE.GROUP_TYPE_SECURITY_ACCOUNT, Group_ACCESS_MASK.GROUP_READ);
                HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(
                    _domainHandle,
                    testGroupName,
                    (uint)Group_ACCESS_MASK.GROUP_READ,
                    out _groupHandle,
                    out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain succeeded.");
                Site.Assert.AreNotEqual(IntPtr.Zero, _groupHandle, "The returned user handle is: {0}.", _groupHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
                result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "3.1.5.7.1 GroupHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: obtain the handle to the created group.");
                HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS, relativeId, out _groupHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: delete the created group.");
                result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeeded.");
            }
        }
    }
}
