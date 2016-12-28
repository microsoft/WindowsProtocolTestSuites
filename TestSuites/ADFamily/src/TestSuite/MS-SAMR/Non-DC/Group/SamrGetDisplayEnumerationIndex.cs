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
        [Description("Non-DC Test: This is to test SamrGetDisplayEnumerationIndex with DomainDisplayGroup.")]
        public void SamrGetDisplayEnumerationIndex_Group_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            try
            {
                string groupName = "GroupForTest";
                uint relativeId;
                Site.Log.Add(LogEntryKind.TestStep, "Create a group with name \"{0}\".", groupName);
                CreateGroup_NonDC(groupName, _serverHandle, _domainHandle, out _groupHandle, out relativeId);

                Site.Log.Add(LogEntryKind.TestStep, "SamrGetDisplayEnumerationIndex: obtains an index into an ascending account-name-sorted list of accounts.");
                uint index;
                string prefix = "Group";
                HRESULT result = _samrProtocolAdapter.SamrGetDisplayEnumerationIndex(_domainHandle, _DOMAIN_DISPLAY_INFORMATION.DomainDisplayGroup, prefix, out index);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrGetDisplayEnumerationIndex returns:{0}.", result);
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteGroup: delete the created group.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteGroup(ref _groupHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteGroup succeed.");
            }
        }

    }
}
