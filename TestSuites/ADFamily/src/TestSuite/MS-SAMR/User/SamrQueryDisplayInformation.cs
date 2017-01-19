// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.DirectoryServices;

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
        [Description("This is to test SamrQueryDisplayInformation with DomainDisplayUser, large EntryCount, large PreferredMaximumLength, zero index.")]
        public void SamrQueryDisplayInformationForUser_DomainDisplayUser_IndexZero()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation: obtains a listing of accounts in ascending name-sorted order.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser,
                0, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation returns:{0}.", result);
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
        [Description("This is to test SamrQueryDisplayInformation with DomainDisplayOemUser, large EntryCount, large PreferredMaximumLength, zero index.")]
        public void SamrQueryDisplayInformationForUser_DomainDisplayOemUser_IndexZero()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation: obtains a listing of accounts in ascending name-sorted order.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayOemUser,
                0, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation returns:{0}.", result);
            if (_samrProtocolAdapter.PDCOSVersion >= Common.ServerVersion.Win2016)
            {
                Site.Assert.AreEqual<uint>(0, totalAvailable, "A call with DisplayInformationClass set to DomainDisplayOemUser or DomainDisplayOemGroup MUST behave identically to a call with DisplayInformationClass set to DomainDisplayUser or DomainDisplayGroup, respectively, with the following exceptions: (1) The RPC_UNICODE_STRING structures in the Oem cases of DisplayInformationClass MUST be translated to RPC_STRING structures using the OEM code page. (2) The value returned in TotalAvailable MUST be set to zero.");
            }
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.UserInformation, "The accounts returned should not be null.");
            _samrProtocolAdapter.VerifyQueryDisplayInformationForDomainDisplayOemUser(buffer.OemUserInformation);
        }
    }
}
