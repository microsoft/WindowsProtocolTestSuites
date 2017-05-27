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
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrEnumerateUsersInDomain.")]
        public void SamrEnumerateUsersInDomain_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");

            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrEnumerateUsersInDomain.")]
        public void SamrEnumerateUsersInDomain_AddDeleteUser_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);

            uint grantedAccess, relativeId = 0;
            try
            {
                buffer = null;
                uint countReturnedAdded = 0;
                result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle, testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain succeeded.");
                result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturnedAdded);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
                Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturnedAdded, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
                Site.Log.Add(LogEntryKind.TestStep, "CountReturnedAdded: {0}.", countReturnedAdded);
                bool flag = false;
                foreach (_SAMPR_RID_ENUMERATION rid in buffer.Value.Buffer)
                {
                    if (relativeId == rid.RelativeId)
                    {
                        flag = true;
                    }
                }
                Site.Assert.IsTrue(flag, "3.1.5.2.2 If an object that satisfies Enumerate-Filter is added between successive Enumerate method calls in a session, and said object has a RID that is greater than the RIDs of all objects returned in previous calls, the server MUST return said object before the enumeration is complete.");
                _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
            }
            finally
            {
                uint countReturnedDeleted = 0;
                buffer = null;
                result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser returns:{0}.", result);
                result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturnedDeleted);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
                Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturnedDeleted, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
                bool flag = false;
                foreach (_SAMPR_RID_ENUMERATION rid in buffer.Value.Buffer)
                {
                    if (relativeId == rid.RelativeId)
                    {
                        flag = true;
                    }
                }
                Site.Assert.IsFalse(flag, "3.1.5.2.2 If an object that satisfies Enumerate-Filter is deleted between successive Enumerate method calls in a session, and said object has not already been returned by a previous method call in the session, the server MUST NOT return said object before the enumeration is complete.");
                _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
            }
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrEnumerateUsersInDomain.")]
        public void SamrEnumerateUsersInDomain_STATUS_MORE_ENTRIES_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 1;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrEnumerateUsersInDomain with invalid handle.")]
        public void SamrEnumerateUsersInDomain_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_serverHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrEnumerateUsersInDomain with no required access.")]
        public void SamrEnumerateUsersInDomain_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain with DOMAIN_READ access.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "3.1.5.2.2 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrEnumerateUsersInDomain.")]
        public void SamrEnumerateUsersInDomain_IgnoreBits_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)(USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT | USER_ACCOUNT_CONTROL.USER_ACCOUNT_AUTO_LOCKED | USER_ACCOUNT_CONTROL.USER_PASSWORD_EXPIRED), out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST ignore the UF_LOCKOUT and UF_PASSWORD_EXPIRED bits in the UserAccountControl parameter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");

            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
        }
    }
}
