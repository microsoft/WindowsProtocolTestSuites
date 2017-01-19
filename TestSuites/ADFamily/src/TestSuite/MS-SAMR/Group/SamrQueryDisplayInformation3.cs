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
        [TestCategory("BVT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryDisplayInformation2 with DomainDisplayGroup, large EntryCount, large PreferredMaximumLength, zero index.")]
        public void SamrQueryDisplayInformation3ForGroup_DomainDisplayGroup_IndexZero()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);
            // set search index as zero to start from the beginning.
            uint index = 0;
            // the total number of groups in the test environment should be less than entryCount.
            uint entryCount = uint.MaxValue;
            uint preferedMaximumLength = uint.MaxValue;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup,
                index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
            Site.Assert.AreEqual<uint>(0, totalAvailable, "The parameter totalAvailable is always equal to 0 when DisplayInformationClass is other than DomainDisplayUser.");
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.GroupInformation, "The accounts returned should not be null.");
            _samrProtocolAdapter.VerifyQueryDisplayInformationForDomainDisplayGroup(buffer.GroupInformation);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryDisplayInformation3 with DomainDisplayGroup with invalid handle.")]
        public void SamrQueryDisplayInformation3ForGroup_invalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

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
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryDisplayInformation3 without DOMAIN_LIST_ACCOUNTS access.")]
        public void SamrQueryDisplayInformation3ForGroup_STATUS_ACCESS_DENIED()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

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
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryDisplayInformation3 with DomainDisplayGroup, large EntryCount, large PreferredMaximumLength.")]
        public void SamrQueryDisplayInformation3ForGroup_IndexNonzero()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2: obtains an index into an ascending account-name¨Csorted list of accounts.");
            uint index;
            string prefix = _samrProtocolAdapter.DomainAdminGroup;
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
            Site.Assert.AreEqual<uint>(0, totalAvailable, "The parameter totalAvailable is always equal to 0 when DisplayInformationClass is other than DomainDisplayUser.");
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.GroupInformation, "The accounts returned should not be null.");
            Site.Assert.AreEqual(_samrProtocolAdapter.DomainAdminGroup, utilityObject.convertToString(buffer.GroupInformation.Buffer[0].AccountName.Buffer), "Let L be a list of accounts, sorted by sAMAccountName, that match the above criteria. If the Index parameter is nonzero, the server MUST return objects starting from the position in L implied by the implementation-specific cookie (carried in the Index parameter).");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryDisplayInformation3 with DomainDisplayGroup, large EntryCount, large PreferredMaximumLength.")]
        public void SamrQueryDisplayInformation3ForGroup_STATUS_MORE_ENTRIES_PreferedMaximumLength()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
            uint index = 0;
            uint entryCount = uint.MaxValue;
            // the returned bytes can never be smaller than 1 for even 1 entry
            uint preferedMaximumLength = 1;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup,
                index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
            Site.Assert.AreEqual<uint>(0, totalAvailable, "The parameter totalAvailable is always equal to 0 when DisplayInformationClass is other than DomainDisplayUser.");
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.GroupInformation, "The accounts returned should not be null.");
            Site.Assert.AreNotEqual<uint>(entryCount, buffer.GroupInformation.EntriesRead, "The PreferredMaximumLength value overrides EntryCount if this value is reached before EntryCount is reached.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-Group")]
        [Description("This is to test SamrQueryDisplayInformation3 with DomainDisplayGroup, large EntryCount, large PreferredMaximumLength.")]
        public void SamrQueryDisplayInformation3ForGroup_STATUS_MORE_ENTRIES_EntryCount()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
            uint index = 0;
            uint entryCount = 1;
            uint preferedMaximumLength = uint.MaxValue;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup,
                index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_MORE_ENTRIES, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
            Site.Assert.AreEqual<uint>(0, totalAvailable, "The parameter totalAvailable is always equal to 0 when DisplayInformationClass is other than DomainDisplayUser.");
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.GroupInformation, "The accounts returned should not be null.");
            Site.Assert.AreEqual<uint>(entryCount, buffer.GroupInformation.EntriesRead, "The number of entries returned should be equal to the number of accounts that the client is requesting on output.");
        }
    }
}
