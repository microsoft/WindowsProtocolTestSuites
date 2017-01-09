// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

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
        [Description("Non-DC Test: This is to test SamrGetDisplayEnumerationIndex2 with DomainDisplayUser.")]
        public void SamrGetDisplayEnumerationIndex2_User_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2: obtains an index into an ascending account-name-sorted list of accounts.");
            uint index;
            string prefix = "admin";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser, prefix, out index);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrGetDisplayEnumerationIndex2 succeeded.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrGetDisplayEnumerationIndex2 with DomainHandle.HandleType not equal to Domain.")]
        public void SamrGetDisplayEnumerationIndex2_User_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2 with invalid handle.");
            uint index;
            string prefix = "admin";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_serverHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser, prefix, out index);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.3.4 The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrGetDisplayEnumerationIndex2 with no DOMAIN_LIST_ACCOUNTS access.")]
        public void SamrGetDisplayEnumerationIndex2_User_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2 with no DOMAIN_LIST_ACCOUNTS access.");
            uint index;
            string prefix = "admin";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser, prefix, out index);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "3.1.5.3.4 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrGetDisplayEnumerationIndex2 with invalid DisplayInformationClass.")]
        public void SamrGetDisplayEnumerationIndex2_User_InvalidDisplayInformationClass_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2 with invalid DisplayInformationClass.");
            uint index;
            string prefix = "admin";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayOemUser, prefix, out index);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.3.4 If DisplayInformationClass is not one of the following values, the server MUST return an error code: DomainDisplayUser, DomainDisplayMachine, DomainDisplayGroup.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrGetDisplayEnumerationIndex2 of STATUS_NO_MORE_ENTRIES.")]
        public void SamrGetDisplayEnumerationIndex2_User_STATUS_NO_MORE_ENTRIES_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2: obtains an index into an ascending account-name-sorted list of accounts.");
            uint index;
            string prefix = "ADMIN";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser, prefix, out index);

            Site.Log.Add(LogEntryKind.TestStep, "SamrEnumerateGroupsInDomain enumerates all users.");
            uint? enumerationContext = 0;
            _SAMPR_ENUMERATION_BUFFER? buffer = null;
            uint preferedMaximumLength = 3000;
            uint countReturned = 0;
            HRESULT res = _samrProtocolAdapter.SamrEnumerateUsersInDomain(_domainHandle, ref enumerationContext, (uint)USER_ACCOUNT_CONTROL.USER_NORMAL_ACCOUNT, out buffer, preferedMaximumLength, out countReturned);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, res, "3.1.5.2.2 The server MUST enable a client to obtain a listing, without duplicates, of all database objects that satisfy the criteria of Enumerate-Filter.");

            List<string> users = new List<string>(utilityObject.convertToString((_SAMPR_ENUMERATION_BUFFER)buffer, buffer.Value.EntriesRead));

            bool flag = false;
            foreach (string user in users)
            {
                if (user.Substring(0, prefix.Length).ToUpper() == prefix.ToUpper())
                {
                    flag = true;
                }
            }

            if (flag)
            {
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "STATUS_SUCCESS is returned if there is such user.");
            }
            else
            {
                Site.Assert.AreEqual(HRESULT.STATUS_NO_MORE_ENTRIES, result, "3.1.5.3.4 If no such element exists, the server MUST return STATUS_NO_MORE_ENTRIES.");
            }
        }

    }
}
