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
        [TestCategory("Alias")]
        [Description("This is to test SamrGetDisplayEnumerationIndex2 with DomainDisplayGroup.")]
        public void SamrGetDisplayEnumerationIndex2_Alias()
        {
            aliasTestPrerequisite();
            createAlias();

            Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex2: obtains an index into an ascending account-name-sorted list of accounts.");
            uint index;
            string prefix = "Domain";
            HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex2(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup, prefix, out index);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrGetDisplayEnumerationIndex2 returns:{0}.", result);
        }
    }
}
