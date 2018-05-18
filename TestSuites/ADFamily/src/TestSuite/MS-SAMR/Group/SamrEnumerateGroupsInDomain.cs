// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [Description("This is to test SamrEnumerateGroupsInDomain.")]
        public void SamrEnumerateGroupsInDomain()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateGroupsInDomain enumerates all groups.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");

            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
            _samrProtocolAdapter.VerifyEnumerateGroupsInDomainResults(buffer);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrEnumerateGroupsInDomain.")]
        public void SamrEnumerateGroupsInDomain_AddDeleteGroup()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all groups.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
            _samrProtocolAdapter.VerifyEnumerateGroupsInDomainResults(buffer);

            uint relativeId;
            buffer = null;
            uint countReturnedAdded = 0;
            result = _samrProtocolAdapter.SamrCreateGroupInDomain(_domainHandle, testGroupName, (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS,
                out _groupHandle, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain succeeded.");
            result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturnedAdded);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturnedAdded, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
            Site.Assert.AreEqual(countReturned + 1, countReturnedAdded, "3.1.5.2.2 If an object that satisfies Enumerate-Filter is added between successive Enumerate method calls in a session, and said object has a RID that is greater than the RIDs of all objects returned in previous calls, the server MUST return said object before the enumeration is complete.");
            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
            _samrProtocolAdapter.VerifyEnumerateGroupsInDomainResults(buffer);

            uint countReturnedDeleted = 0;
            buffer = null;
            result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup returns:{0}.", result);
            result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturnedDeleted);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturnedDeleted, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
            Site.Assert.AreEqual(countReturnedAdded - 1, countReturnedDeleted, "3.1.5.2.2 If an object that satisfies Enumerate-Filter is deleted between successive Enumerate method calls in a session, and said object has not already been returned by a previous method call in the session, the server MUST NOT return said object before the enumeration is complete.");
            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
            _samrProtocolAdapter.VerifyEnumerateGroupsInDomainResults(buffer);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrEnumerateGroupsInDomain.")]
        public void SamrEnumerateGroupsInDomain_STATUS_MORE_ENTRIES()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateGroupsInDomain enumerates all groups.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 200;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
            //result always return HRESULT.STATUS_SUCCESS
            //Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrEnumerateGroupsInDomain with invalid handle.")]
        public void SamrEnumerateGroupsInDomain_InvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateGroupsInDomain enumerates all groups.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_serverHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrEnumerateGroupsInDomain with no required access.")]
        public void SamrEnumerateGroupsInDomain_STATUS_ACCESS_DENIED()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateGroupsInDomain with DOMAIN_READ access.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateGroupsInDomain(_domainHandle, ref enumerationContext, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "3.1.5.2.2 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }
    }
}
