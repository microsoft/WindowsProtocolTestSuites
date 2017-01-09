// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrDeleteUser.")]
        public void SamrDeleteUser_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, User_ACCESS_MASK.USER_ALL_ACCESS);
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain succeeded.");
            Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser succeeded.");
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "3.1.5.7.3 The server MUST delete the SamContextHandle ADM element (section 3.1.1.10) represented by UserHandle, and then MUST return 0 for the value of UserHandle and a return code of STATUS_SUCCESS.");    
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrDeleteUser whose Rid is less than 1000.")]
        public void SamrDeleteUser_SmallRid_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain the handle to the administrator.");
            HRESULT result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)User_ACCESS_MASK.USER_ALL_ACCESS, Utilities.DOMAIN_USER_RID_ADMIN, out _userHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrOpenUser succeeded.");
            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the administrator.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.3 If the RID of U's objectSid attribute value is less than 1000, an error MUST be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrDeleteUser with UserHandle.HandleType not equal to User.")]
        public void SamrDeleteUser_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser with invalid handle.");
            HRESULT result = _samrProtocolAdapter.SamrDeleteUser(ref _domainHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.3 The server MUST return an error if UserHandle.HandleType is not equal to User.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrDeleteUser with no required access.")]
        public void SamrDeleteUser_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            uint grantedAccess, relativeId=0;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                    testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, User_ACCESS_MASK.USER_READ);
                HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_READ,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain succeeded.");
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
                result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "3.1.5.7.3 UserHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
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
    }
}
