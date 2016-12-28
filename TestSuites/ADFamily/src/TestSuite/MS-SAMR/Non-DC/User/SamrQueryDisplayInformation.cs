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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryDisplayInformation with DomainDisplayUser, large EntryCount, large PreferredMaximumLength, zero index.")]
        public void SamrQueryDisplayInformationForUser_DomainDisplayUser_IndexZero_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation: obtains a listing of accounts in ascending name-sorted order.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayUser,
                0, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation returns:{0}.", result);
            Site.Assert.IsTrue(totalAvailable >= totalReturned, "The length of total available accounts should be larger than or equal to the length of total returned.");
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.UserInformation, "The accounts returned should not be null.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrQueryDisplayInformation with DomainDisplayOemUser, large EntryCount, large PreferredMaximumLength, zero index.")]
        public void SamrQueryDisplayInformationForUser_DomainDisplayOemUser_IndexZero_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation: obtains a listing of accounts in ascending name-sorted order.");
            uint entryCount = 100;
            uint preferedMaximumLength = 3000;
            uint totalAvailable, totalReturned;
            _SAMPR_DISPLAY_INFO_BUFFER buffer;
            HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayOemUser,
                0, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation returns:{0}.", result);
            Site.Assert.IsTrue(totalReturned > 0, "The number of bytes returned should be larger than 0.");
            Site.Assert.IsNotNull(buffer.UserInformation, "The accounts returned should not be null.");
        }
    }
}
