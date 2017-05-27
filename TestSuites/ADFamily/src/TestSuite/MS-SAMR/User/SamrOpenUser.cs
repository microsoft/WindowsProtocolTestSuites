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
        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrOpenUser and SamrCloseHandle with different DesiredAccess.")]
        public void SamrOpenCloseUser()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);
            
            Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: get rid for {0}.", _samrProtocolAdapter.DomainAdministratorName);
            List<string> userNames = new List<string>();
            userNames.Add(_samrProtocolAdapter.DomainAdministratorName);
            List<uint> userRids = new List<uint>();
            HRESULT result = _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, userNames, out userRids);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrLookupNamesInDomain returns:{0}.", result);
            Site.Assume.IsTrue(userRids.Count == 1, "{0} user(s) with name {1} found.", userRids.Count, _samrProtocolAdapter.DomainAdministratorName);

            foreach (Common_ACCESS_MASK acc in Enum.GetValues(typeof(Common_ACCESS_MASK)))
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain a handle to a user with DesiredAccess as {0}.", acc.ToString());
                result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)acc, userRids[0], out _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenUser returns:{0}.", result);
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened user handle.");
                result = _samrProtocolAdapter.SamrCloseHandle(ref _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "The opened user handle has been closed.");
            }
            foreach (Generic_ACCESS_MASK acc in Enum.GetValues(typeof(Generic_ACCESS_MASK)))
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain a handle to a user with DesiredAccess as {0}.", acc.ToString());
                result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)acc, userRids[0], out _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenUser returns:{0}.", result);
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened user handle.");
                result = _samrProtocolAdapter.SamrCloseHandle(ref _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "The opened user handle has been closed.");
            }
            foreach (User_ACCESS_MASK acc in Enum.GetValues(typeof(User_ACCESS_MASK)))
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain a handle to a user with DesiredAccess as {0}.", acc.ToString());
                result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)acc, userRids[0], out _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenUser returns:{0}.", result);
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened user handle.");
                result = _samrProtocolAdapter.SamrCloseHandle(ref _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "The opened user handle has been closed.");
            }
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrOpenUser with DomainHandle.HandleType not equal to Domain.")]
        public void SamrOpenUser_InvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
            HRESULT result = _samrProtocolAdapter.SamrOpenUser(_serverHandle, (uint)User_ACCESS_MASK.USER_ALL_ACCESS, Utilities.DOMAIN_USER_RID_ADMIN, out _userHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenUser returns an error:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "The returned user handle should be zero.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrOpenUser without required access.")]
        public void SamrOpenUser_STATUS_ACCESS_DENIED()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: DomainHandle.GrantedAccess MUST have the required access.");
            HRESULT result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)User_ACCESS_MASK.USER_ALL_ACCESS, Utilities.DOMAIN_USER_RID_ADMIN, out _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrOpenUser returns an error:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "The returned user handle should be zero.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrOpenUser with no such user.")]
        public void SamrOpenUser_NoSuchUser()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            uint invaldRid = 5000;
            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: If no such object exists, the server MUST return an error code.");
            HRESULT result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)User_ACCESS_MASK.USER_ALL_ACCESS, invaldRid, out _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_NO_SUCH_USER, result, "SamrOpenUser returns an error:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "The returned user handle should be zero.");
        }

    }
}
