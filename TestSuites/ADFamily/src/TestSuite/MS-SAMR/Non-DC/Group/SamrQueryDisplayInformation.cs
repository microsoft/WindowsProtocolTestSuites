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
        [TestCategory("SAMR-Group")]
        [Description("Non-DC Test: This is to test SamrQueryDisplayInformation with DomainDisplayGroup, large EntryCount, large PreferredMaximumLength, zero index.")]
        public void SamrQueryDisplayInformationForGroup_DomainDisplayGroup_IndexZero_NonDC()
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
                HRESULT result = _samrProtocolAdapter.SamrQueryDisplayInformation(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup,
                    index, entryCount, preferedMaximumLength, out totalAvailable, out totalReturned, out buffer);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrQueryDisplayInformation returns:{0}.", result);
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

    }
}
