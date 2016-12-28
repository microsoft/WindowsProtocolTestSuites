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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrOpenUser and SamrCloseHandle with different DesiredAccess.")]
        public void SamrOpenCloseUser_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);
            uint relativeId = 0;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                    testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, User_ACCESS_MASK.USER_ALL_ACCESS);
                uint grantedAccess;
                HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain succeeded.");
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);
                Site.Assert.IsTrue(relativeId >= 1000, "The Rid value MUST be greater than or equal to 1000");
                Site.Assert.AreEqual((uint)User_ACCESS_MASK.USER_ALL_ACCESS, grantedAccess,
                    "3.1.5.4.4 The return parameter of GrantedAccess MUST be set to DesiredAccess if DesiredAccess contains only valid access masks for the user object.");
                
                Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: get rid for {0}.", testUserName);
                List<string> userNames = new List<string>();
                userNames.Add(testUserName);
                List<uint> userRids = new List<uint>();
                result = _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, userNames, out userRids);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrLookupNamesInDomain returns:{0}.", result);
                Site.Assume.IsTrue(userRids.Count == 1, "{0} user(s) with name {1} found.", userRids.Count, testUserName);
                relativeId = userRids[0];

                IntPtr userHandle = IntPtr.Zero;
                foreach (Common_ACCESS_MASK acc in Enum.GetValues(typeof(Common_ACCESS_MASK)))
                {
                    Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain a handle to a user with DesiredAccess as {0}.", acc.ToString());
                    result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)acc, userRids[0], out userHandle);
                    Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenUser returns:{0}.", result);
                    Site.Assert.AreNotEqual(IntPtr.Zero, userHandle, "The returned user handle is: {0}.", userHandle);

                    Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened user handle.");
                    result = _samrProtocolAdapter.SamrCloseHandle(ref userHandle);
                    Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                    Site.Assert.AreEqual(IntPtr.Zero, userHandle, "The opened user handle has been closed.");
                }
                foreach (Generic_ACCESS_MASK acc in Enum.GetValues(typeof(Generic_ACCESS_MASK)))
                {
                    Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain a handle to a user with DesiredAccess as {0}.", acc.ToString());
                    result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)acc, userRids[0], out userHandle);
                    Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenUser returns:{0}.", result);
                    Site.Assert.AreNotEqual(IntPtr.Zero, userHandle, "The returned user handle is: {0}.", userHandle);

                    Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened user handle.");
                    result = _samrProtocolAdapter.SamrCloseHandle(ref userHandle);
                    Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                    Site.Assert.AreEqual(IntPtr.Zero, userHandle, "The opened user handle has been closed.");
                }
                foreach (User_ACCESS_MASK acc in Enum.GetValues(typeof(User_ACCESS_MASK)))
                {
                    Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain a handle to a user with DesiredAccess as {0}.", acc.ToString());
                    result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)acc, userRids[0], out userHandle);
                    Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenUser returns:{0}.", result);
                    Site.Assert.AreNotEqual(IntPtr.Zero, userHandle, "The returned user handle is: {0}.", userHandle);

                    Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close the opened user handle.");
                    result = _samrProtocolAdapter.SamrCloseHandle(ref userHandle);
                    Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCloseHandle returns:{0}.", result);
                    Site.Assert.AreEqual(IntPtr.Zero, userHandle, "The opened user handle has been closed.");
                }
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain the handle to the created user with USER_ALL_ACCESS.");
                HRESULT result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)User_ACCESS_MASK.USER_ALL_ACCESS, relativeId, out _userHandle);
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
                result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser succeeded.");
            }         
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrOpenUser with DomainHandle.HandleType not equal to Domain.")]
        public void SamrOpenUser_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrOpenUser without required access.")]
        public void SamrOpenUser_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrOpenUser with no such user.")]
        public void SamrOpenUser_NoSuchUser_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            uint invaldRid = 5000;
            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: If no such object exists, the server MUST return an error code.");
            HRESULT result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)User_ACCESS_MASK.USER_ALL_ACCESS, invaldRid, out _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_NO_SUCH_USER, result, "SamrOpenUser returns an error:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "The returned user handle should be zero.");
        }

    }
}
