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
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrCreateUserInDomain with USER_ALL_ACCESS.")]
        public void SamrCreateUserInDomain()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

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
                Site.Assert.IsTrue(_samrProtocolAdapter.VerifyRelativeID(relativeId), "3.1.1.9.2.1 The Rid value MUST be within the Rid-Range");
                _samrProtocolAdapter.VerifyConstraintsForUserOrComputer(testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT);
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
