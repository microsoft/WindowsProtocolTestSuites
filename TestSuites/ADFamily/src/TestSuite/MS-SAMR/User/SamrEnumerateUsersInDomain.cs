// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
        [Description("This is to test SamrEnumerateUsersInDomain.")]
        public void SamrEnumerateUsersInDomain()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");

            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
            _samrProtocolAdapter.VerifyEnumerateUsersInDomainResults(buffer, (uint)AdtsUserAccountControl.ADS_UF_NORMAL_ACCOUNT);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrEnumerateUsersInDomain.")]
        public void SamrEnumerateUsersInDomain_AddDeleteUser()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 1;
            uint countReturned = 0;

            uint grantedAccess = 0;
            uint relativeId = 0;

            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");

                // Start a new enumeration session
                HRESULT enumResult = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
                Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, enumResult, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
                Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
                _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);

                buffer = null;
                uint countReturnedAdded = 0;

                // Add a new user to the domain
                HRESULT opResult = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle, testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, opResult, "SamrCreateUser2InDomain succeeded.");

                // Continue the enumeration with the existing session
                var enumeratedUsersAfterUserCreation = new List<_SAMPR_RID_ENUMERATION>();
                while (enumResult == HRESULT.STATUS_MORE_ENTRIES)
                {
                    enumResult = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturnedAdded);
                    Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturnedAdded, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
                    Site.Log.Add(LogEntryKind.TestStep, "CountReturnedAdded: {0}.", countReturnedAdded);
                    _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
                    enumeratedUsersAfterUserCreation.AddRange(buffer.Value.Buffer);
                }
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, enumResult, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");

                // Check whether the newly added user was enumerated
                bool flag = enumeratedUsersAfterUserCreation.Any(user => user.RelativeId == relativeId);
                Site.Assert.IsTrue(flag, "3.1.5.2.2 If an object that satisfies Enumerate-Filter is added between successive Enumerate method calls in a session, and said object has a RID that is greater than the RIDs of all objects returned in previous calls, the server MUST return said object before the enumeration is complete.");
            }
            finally
            {
                enumerationContext = 0;

                Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");

                // Start a new enumeration session
                HRESULT enumResult = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
                Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, enumResult, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
                Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
                _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);

                buffer = null;
                uint countReturnedDeleted = 0;

                // Delete the newly added user in the domain
                HRESULT opResult = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, opResult, "SamrDeleteUser returns:{0}.", opResult);

                // Continue the enumeration with the existing session
                var enumeratedUsersAfterUserDeletion = new List<_SAMPR_RID_ENUMERATION>();
                while (enumResult == HRESULT.STATUS_MORE_ENTRIES)
                {
                    enumResult = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturnedDeleted);
                    Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturnedDeleted, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");
                    Site.Log.Add(LogEntryKind.TestStep, "CountReturnedDeleted: {0}", countReturnedDeleted);
                    _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
                    enumeratedUsersAfterUserDeletion.AddRange(buffer.Value.Buffer);
                }
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, enumResult, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");

                // Check whether the newly added user was enumerated
                bool flag = enumeratedUsersAfterUserDeletion.Any(user => user.RelativeId == relativeId);
                Site.Assert.IsFalse(flag, "3.1.5.2.2 If an object that satisfies Enumerate-Filter is deleted between successive Enumerate method calls in a session, and said object has not already been returned by a previous method call in the session, the server MUST NOT return said object before the enumeration is complete.");
            }
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrEnumerateUsersInDomain.")]
        public void SamrEnumerateUsersInDomain_STATUS_MORE_ENTRIES()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 200;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, result, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrEnumerateUsersInDomain with invalid handle.")]
        public void SamrEnumerateUsersInDomain_InvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

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
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrEnumerateUsersInDomain with no required access.")]
        public void SamrEnumerateUsersInDomain_STATUS_ACCESS_DENIED()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

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
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrEnumerateUsersInDomain.")]
        public void SamrEnumerateUsersInDomain_IgnoreBits()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateUsersInDomain enumerates all users.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT result = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)(USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT | USER_ACCOUNT_CONTROL.USER_ACCOUNT_AUTO_LOCKED | USER_ACCOUNT_CONTROL.USER_PASSWORD_EXPIRED), out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.2.2 The server MUST ignore the UF_LOCKOUT and UF_PASSWORD_EXPIRED bits in the UserAccountControl parameter.");
            Site.Assert.AreEqual(buffer.Value.EntriesRead, countReturned, "3.1.5.2.2 On output, CountReturned MUST equal Buffer.EntriesRead.");

            _samrProtocolAdapter.VerifyConstraintsInCommonProcessingOfEnumeration(buffer.Value.Buffer, preferedMaximumLength);
            _samrProtocolAdapter.VerifyEnumerateUsersInDomainResults(buffer, (uint)AdtsUserAccountControl.ADS_UF_NORMAL_ACCOUNT);
        }
    }
}
