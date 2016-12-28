// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrRidToSid with right Rid.")]
        public void SamrRidToSid_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            uint grantedAccess, relativeId = 0;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                    testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, User_ACCESS_MASK.USER_ALL_ACCESS);
                HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain succeeded.");
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrRidToSid: obtain the SID of an account, given a RID.");
                _RPC_SID? sid;
                result = _samrProtocolAdapter.SamrRidToSid(_userHandle, relativeId, out sid);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrRidToSid succeeded.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrDeleteUser succeeded.");
            }
        }

    }
}
