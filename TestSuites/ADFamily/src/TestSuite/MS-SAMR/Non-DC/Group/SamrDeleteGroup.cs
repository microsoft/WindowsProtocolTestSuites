// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.DirectoryServices;
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
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrDeleteGroup.")]
        public void SamrDeleteGroup_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);
            string groupName = testGroupName;
            uint relativeId;
            Site.Log.Add(LogEntryKind.TestStep, "Create a group with name \"{0}\".", groupName);
            CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
            HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeed.");

            Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: lookup the group: {0}.", groupName);
            List<string> groupNames = new List<string>();
            groupNames.Add(groupName);
            List<uint> groupRids = new List<uint>();
            result = _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, groupNames, out groupRids);
            Site.Assert.AreEqual(HRESULT.STATUS_NONE_MAPPED, result, "The group: {0} should not be found again.", groupName);          
        }


        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrDeleteGroup whose Rid is less than 1000.")]
        public void SamrDeleteGroup_SmallRid_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: obtain the handle to the domain users group.");
            HRESULT result = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS, Utilities.DOMAIN_GROUP_RID_USERS, out _groupHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the domain users group.");
            result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.1 If the RID of G's objectSid attribute value is less than 1000, an error MUST be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrDeleteGroup with GroupHandle.HandleType not equal to Group.")]
        public void SamrDeleteGroup_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup with invalid handle.");
            HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _domainHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.1 The server MUST return an error if GroupHandle.HandleType is not equal to Group.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrDeleteGroup with no required access.")]
        public void SamrDeleteGroup_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);
            string groupName = testGroupName;
            uint relativeId = 0;
            try
            {
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId, (uint)Group_ACCESS_MASK.GROUP_READ);
              
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
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
