// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Modeling;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    public partial class SAMR_TestSuite : TestClassBase
    {
        /// <summary>
        /// Test method of SamrOpenUser and SamrCloseHandle with different DesiredAccess
        /// </summary>
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("Alias")]
        public void SamrOpenCloseAlias()
        {
            aliasTestPrerequisite();
            createAlias();
            Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: translate a set of account names into a set of RIDs.");
            List<string> groupNames = new List<string>();
            groupNames.Add(testAliasName);
            List<uint> groupRids = new List<uint>();
            HRESULT result = _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, groupNames, out groupRids);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrLookupNamesInDomain returns:{0}.", result);
            Site.Assume.IsTrue(groupRids.Count == 1, "{0} group(s) with name {1} found.", groupRids.Count, testAliasName);

            foreach (Common_ACCESS_MASK acc in Enum.GetValues(typeof(Common_ACCESS_MASK)))
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: obtain a handle to a group with DesiredAccess as {0}.", acc.ToString());
                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)acc, groupRids[0], out _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenAlias returns:{0}.", result);
                Site.Assert.IsNotNull(_groupHandle, "The returned group handle is: {0}.", _aliasHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened group handle.");
                result = _samrProtocolAdapter.SamrCloseHandle(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                Site.Assert.AreEqual(IntPtr.Zero, _groupHandle, "The opened group handle has been closed.");
            }
            foreach (Group_ACCESS_MASK acc in Enum.GetValues(typeof(Group_ACCESS_MASK)))
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenAlias: obtain a handle to a group with DesiredAccess as {0}.", acc.ToString());
                result = _samrProtocolAdapter.SamrOpenAlias(_domainHandle, (uint)acc, groupRids[0], out _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpengroup returns:{0}.", result);
                Site.Assert.IsNotNull(_groupHandle, "The returned group handle is: {0}.", _aliasHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened group handle.");
                result = _samrProtocolAdapter.SamrCloseHandle(ref _aliasHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                Site.Assert.AreEqual(IntPtr.Zero, _groupHandle, "The opened group handle has been closed.");
            }
        }
    }
}
