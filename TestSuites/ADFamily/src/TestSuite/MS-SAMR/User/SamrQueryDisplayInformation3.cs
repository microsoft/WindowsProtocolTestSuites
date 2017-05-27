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
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryDisplayInformation3 with DomainDisplayUser, large EntryCount, large PreferredMaximumLength, zero index.")]
        public void SamrQueryDisplayInformation3ForUser_DomainDisplayUser_IndexZero()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser,
                0, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
            Site.Assert.IsTrue(totalReturned>0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.UserInformation, "The accounts returned should not be null.");
            _samrProtocolAdapter.VerifyQueryDisplayInformationForDomainDisplayUser(buffer.UserInformation);  
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryDisplayInformation3 with DomainDisplayOemUser, large EntryCount, large PreferredMaximumLength, zero index.")]
        public void SamrQueryDisplayInformation3ForUser_DomainDisplayOemUser_IndexZero()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayOemUser,
                0, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
            if (_samrProtocolAdapter.PDCOSVersion >= Common.ServerVersion.Win2016)
            {
                Site.Assert.AreEqual<uint>(0, totalAvailable, "A call with DisplayInformationClass set to DomainDisplayOemUser or DomainDisplayOemGroup MUST behave identically to a call with DisplayInformationClass set to DomainDisplayUser or DomainDisplayGroup, respectively, with the following exceptions: (1) The RPC_UNICODE_STRING structures in the Oem cases of DisplayInformationClass MUST be translated to RPC_STRING structures using the OEM code page. (2) The value returned in TotalAvailable MUST be set to zero.");
            }
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.UserInformation, "The accounts returned should not be null.");
            _samrProtocolAdapter.VerifyQueryDisplayInformationForDomainDisplayOemUser(buffer.OemUserInformation);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryDisplayInformation3 with DomainDisplayUser, large EntryCount, large PreferredMaximumLength.")]
        public void SamrQueryDisplayInformation3ForUser_DomainDisplayUser()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2: obtains an index into an ascending account-name¨Csorted list of accounts.");
            uint index;
            string prefix = "Administrator";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser, prefix, out index);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrGetDisplayEnumerationIndex2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser,
                index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation3 returns:{0}.", result);      
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.UserInformation, "The accounts returned should not be null.");
            _samrProtocolAdapter.VerifyQueryDisplayInformationForDomainDisplayUser(buffer.UserInformation);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryDisplayInformation3 with DomainDisplayOemUser, large EntryCount, large PreferredMaximumLength.")]
        public void SamrQueryDisplayInformation3ForUser_DomainDisplayOemUser()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2: obtains an index into an ascending account-name¨Csorted list of accounts.");
            uint index;
            string prefix = "Administrator";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser, prefix, out index);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrGetDisplayEnumerationIndex2 returns:{0}.", result);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayOemUser,
                index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation3 returns:{0}.", result);
            if (_samrProtocolAdapter.PDCOSVersion >= Common.ServerVersion.Win2016)
            {
                Site.Assert.AreEqual<uint>(0, totalAvailable, "A call with DisplayInformationClass set to DomainDisplayOemUser or DomainDisplayOemGroup MUST behave identically to a call with DisplayInformationClass set to DomainDisplayUser or DomainDisplayGroup, respectively, with the following exceptions: (1) The RPC_UNICODE_STRING structures in the Oem cases of DisplayInformationClass MUST be translated to RPC_STRING structures using the OEM code page. (2) The value returned in TotalAvailable MUST be set to zero.");
            }
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.OemUserInformation, "The accounts returned should not be null.");
            _samrProtocolAdapter.VerifyQueryDisplayInformationForDomainDisplayOemUser(buffer.OemUserInformation);
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryDisplayInformation3 with invalid handle.")]
        public void SamrQueryDisplayInformation3ForUser_InvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3 with invalid handle.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_serverHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser,
                0, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrQueryDisplayInformation3 without DOMAIN_LIST_ACCOUNTS access.")]
        public void SamrQueryDisplayInformation3ForUser_STATUS_ACCESS_DENIED()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3 without DOMAIN_LIST_ACCOUNTS access.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation3(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser,
                0, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "DomainHandle.GrantedAccess MUST have the required access DOMAIN_LIST_ACCOUNTS. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

    }
}
