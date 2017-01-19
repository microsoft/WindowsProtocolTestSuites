// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
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
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrGetGroupsForUser.")]
        public void SamrGetGroupsForUser()
        {
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetGroupsForUser: obtain a listing of groups that a user is a member of.");
            _SAMPR_GET_GROUPS_BUFFER? groups;
            _samrProtocolAdapter.SamrGetGroupsForUser(_userHandle,out groups);
    
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, 
                _samrProtocolAdapter.DomainAdministratorName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            DirectoryEntry entry = new DirectoryEntry(userPath);
            List<string> memberOfDomain = new List<string>();
            List<uint> expectGroupRids = new List<uint>();
            List<uint> actualGroupRids = new List<uint>();
            foreach (var group in entry.Properties["memberOf"])
            {
                if (!group.ToString().ToLower().Contains("cn=builtin"))
                {
                    memberOfDomain.Add(group.ToString().Split(',')[0].Split('=')[1]);
                }
            }
            _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, memberOfDomain, out expectGroupRids);
            expectGroupRids.Add(Convert.ToUInt32(entry.Properties["primaryGroupID"].Value));

            Site.Assert.AreEqual((uint)expectGroupRids.Count, groups.Value.MembershipCount,
                "3.1.5.9.1 The returned Groups.MembershipCount MUST be set to the cardinality that the union determined from step 3.");
            foreach (_GROUP_MEMBERSHIP group in groups.Value.Groups)
            {
                actualGroupRids.Add(group.RelativeId);
                Site.Assert.AreEqual<uint>(Utilities.SE_GROUP_MANDATORY | Utilities.SE_GROUP_ENABLED_BY_DEFAULT | Utilities.SE_GROUP_ENABLED, group.Attributes, 
                    "3.1.5.9.1 Group {0}: On query, the returned value MUST be a logical union of the following bits: SE_GROUP_MANDATORY, SE_GROUP_ENABLED_BY_DEFAULT, and SE_GROUP_ENABLED.", group.RelativeId);
            }

            Site.Assert.IsTrue(Enumerable.SequenceEqual(expectGroupRids.OrderBy(t => t), actualGroupRids.OrderBy(t => t)), 
                "3.1.5.9.1 RelativeId MUST contain the RID of the SID of the dsname member value.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrGetGroupsForUser with no USER_LIST_GROUPS access.")]
        public void SamrGetGroupsForUser_STATUS_ACCESS_DENIED()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Connect and open a user handle with USER_READ access.");
            ConnectAndOpenUser(_samrProtocolAdapter.PDCNetbiosName, _samrProtocolAdapter.PrimaryDomainDnsName,
                _samrProtocolAdapter.DomainAdministratorName, out _userHandle, (uint)User_ACCESS_MASK.USER_READ_GENERAL);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetGroupsForUser: obtain a listing of groups that a user is a member of.");
            _SAMPR_GET_GROUPS_BUFFER? groups;
            HRESULT result = _samrProtocolAdapter.SamrGetGroupsForUser(_userHandle, out groups);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "UserHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrGetGroupsForUser with invalid handle.")]
        public void SamrGetGroupsForUser_InvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetGroupsForUser: obtain a listing of groups that a user is a member of.");
            _SAMPR_GET_GROUPS_BUFFER? groups;
            HRESULT result = _samrProtocolAdapter.SamrGetGroupsForUser(_serverHandle, out groups);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "The server MUST return an error if UserHandle.HandleType is not equal to User.");
        }

    }
}
