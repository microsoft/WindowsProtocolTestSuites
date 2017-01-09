// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;

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
        [Description("Non-DC Test: This is to test SamrQueryDisplayInformation2 with DomainDisplayGroup, large EntryCount, large PreferredMaximumLength, zero index.")]
        public void SamrQueryDisplayInformation3ForGroup_DomainDisplayGroup_IndexZero_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
               out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            uint relativeId;
            try
            {
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);

                uint index = 0;
                uint entryCount = uint.MaxValue;
                uint preferedMaximumLength = uint.MaxValue;
                uint totalAvailable, totalReturned;
                _SAMPR_DISPLAY_INFO_BUFFER buffer;
                HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup,
                    index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
                Site.Assert.IsTrue(totalAvailable >= totalReturned, "The parameter totalAvailable should be greater than or equal to totalReturned.");
                Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
                Site.Assert.IsNotNull(buffer.GroupInformation, "The accounts returned should not be null.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: delete the created group.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeeded.");
            } 
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQueryDisplayInformation3 with DomainDisplayGroup with invalid handle.")]
        public void SamrQueryDisplayInformation3ForGroup_invalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3 with invalid handle.");
            uint index = 0;
            uint entryCount = uint.MaxValue;
            uint preferedMaximumLength = uint.MaxValue;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_serverHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup,
                index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQueryDisplayInformation3 without DOMAIN_LIST_ACCOUNTS access.")]
        public void SamrQueryDisplayInformation3ForGroup_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3 without DOMAIN_LIST_ACCOUNTS access.");
            uint index = 0;
            uint entryCount = uint.MaxValue;
            uint preferedMaximumLength = uint.MaxValue;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser,
                index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "DomainHandle.GrantedAccess MUST have the required access DOMAIN_LIST_ACCOUNTS. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQueryDisplayInformation3 with DomainDisplayGroup, large EntryCount, large PreferredMaximumLength, non-zero index.")]
        public void SamrQueryDisplayInformation3ForGroup_IndexNonzero_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            uint relativeId;
            try
            {
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);

                Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2: obtains an index into an ascending account-name-sorted list of accounts.");
                uint index;
                string prefix = testGroupName;
                HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup, prefix, out index);
                Site.Assert.AreNotEqual<uint>(0, index, "The returned index: {0} should be non-zero.", index);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrGetDisplayEnumerationIndex2 returns:{0}.", result);

                Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
                uint entryCount = uint.MaxValue;
                uint preferedMaximumLength = uint.MaxValue;
                uint totalAvailable, totalReturned;
                _SAMPR_DISPLAY_INFO_BUFFER buffer;
                result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup,
                    index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
                Site.Assert.IsTrue(totalAvailable >= totalReturned, "The parameter totalAvailable should be greater than or equal to totalReturned.");
                Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
                Site.Assert.IsNotNull(buffer.GroupInformation, "The accounts returned should not be null.");
                Site.Assert.AreEqual(testGroupName, utilityObject.convertToString(buffer.GroupInformation.Buffer[0].AccountName.Buffer), "Let L be a list of accounts, sorted by sAMAccountName, that match the above criteria. If the Index parameter is nonzero, the server MUST return objects starting from the position in L implied by the implementation-specific cookie (carried in the Index parameter).");
    
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: delete the created group.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeeded.");
            } 
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQueryDisplayInformation3 with DomainDisplayGroup, large EntryCount, specified PreferredMaximumLength.")]
        public void SamrQueryDisplayInformation3ForGroup_STATUS_MORE_ENTRIES_PreferedMaximumLength_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            uint relativeId;
            try
            {
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);

                Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
                uint index = 0;
                uint entryCount = uint.MaxValue;
                uint preferedMaximumLength = 1;
                uint totalAvailable, totalReturned;
                _SAMPR_DISPLAY_INFO_BUFFER buffer;
                HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup,
                    index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
                Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
                Site.Assert.IsTrue(totalAvailable >= totalReturned, "The parameter totalAvailable should be greater than or equal to totalReturned.");
                Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
                Site.Assert.IsNotNull(buffer.GroupInformation, "The accounts returned should not be null.");
                Site.Assert.AreNotEqual<uint>(entryCount, buffer.GroupInformation.EntriesRead, "The PreferredMaximumLength value overrides EntryCount if this value is reached before EntryCount is reached.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: delete the created group.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeeded.");
            } 
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQueryDisplayInformation3 with DomainDisplayGroup, specified EntryCount, large PreferredMaximumLength.")]
        public void SamrQueryDisplayInformation3ForGroup_STATUS_MORE_ENTRIES_EntryCount_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn,
                out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            string groupName = testGroupName;
            uint relativeId;
            try
            {
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);

                Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
                uint index = 0;
                uint entryCount = 1;
                uint preferedMaximumLength = uint.MaxValue;
                uint totalAvailable, totalReturned;
                _SAMPR_DISPLAY_INFO_BUFFER buffer;
                HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup,
                    index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
                Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
                Site.Assert.IsTrue(totalAvailable >= totalReturned, "The parameter totalAvailable should be greater than or equal to totalReturned.");
                Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
                Site.Assert.IsNotNull(buffer.GroupInformation, "The accounts returned should not be null.");
                Site.Assert.AreEqual<uint>(entryCount, buffer.GroupInformation.EntriesRead, "The number of entries returned should be equal to the number of accounts that the client is requesting on output.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: delete the created group.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeeded.");
            } 
        }
    }
}
