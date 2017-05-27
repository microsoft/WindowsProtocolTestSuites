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
        [TestCategory("Alias")]
        [Description("This is to test SamrQueryDisplayInformation3 with DomainDisplayGroup, large EntryCount, large PreferredMaximumLength.")]
        public void SamrQueryDisplayInformation3ForAlias_IndexZero()
        {
            aliasTestPrerequisite();
            createAlias();

            uint index = 0;
            Site.Log.Add(LogEntryKind.TestStep, "SamrQueryDisplayInformation3: obtains a listing of accounts in ascending name-sorted order.");
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

    }
}
