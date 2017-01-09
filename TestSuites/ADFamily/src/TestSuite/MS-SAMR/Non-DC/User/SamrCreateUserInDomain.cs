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
        [Description("Non-DC Test: This is to test SamrCreateUserInDomain with USER_ALL_ACCESS.")]
        public void SamrCreateUserInDomain_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUserInDomain: create a user with Name:{0} and DesiredAccess:{1}",
                   testUserName, User_ACCESS_MASK.USER_ALL_ACCESS);
                uint relativeId;
                HRESULT result = _samrProtocolAdapter.SamrCreateUserInDomain(_domainHandle,
                    testUserName, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUserInDomain succeeded.");
                Site.Assert.IsNotNull(_userHandle, "The returned user handle is: {0}.", _userHandle);
                Site.Assert.IsTrue(relativeId >= 1000, "The Rid value MUST be greater than or equal to 1000");                   
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
