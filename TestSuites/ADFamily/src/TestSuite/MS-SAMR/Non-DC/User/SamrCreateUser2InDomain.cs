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
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain with USER_NORMAL_ACCOUNT and USER_ALL_ACCESS.")]
        public void SamrCreateUser2InDomain_USER_NORMAL_ACCOUNT_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                    testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, User_ACCESS_MASK.USER_ALL_ACCESS);
                uint grantedAccess, relativeId;
                HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain succeeded.");
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);
                Site.Assert.IsTrue(relativeId >= 1000, "The Rid value MUST be greater than or equal to 1000");                
                Site.Assert.AreEqual((uint)User_ACCESS_MASK.USER_ALL_ACCESS, grantedAccess,
                    "3.1.5.4.4 The return parameter of GrantedAccess MUST be set to DesiredAccess if DesiredAccess contains only valid access masks for the user object.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
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
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain with USER_WORKSTATION_TRUST_ACCOUNT and USER_ALL_ACCESS.")]
        public void SamrCreateUser2InDomain_USER_WORKSTATION_TRUST_ACCOUNT_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                    testUserName, ACCOUNT_TYPE.USER_WORKSTATION_TRUST_ACCOUNT, User_ACCESS_MASK.USER_ALL_ACCESS);
                uint grantedAccess, relativeId;
                HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_WORKSTATION_TRUST_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain succeeded.");
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);
                Site.Assert.IsTrue(relativeId >= 1000, "The Rid value MUST be greater than or equal to 1000");
                Site.Assert.AreEqual((uint)User_ACCESS_MASK.USER_ALL_ACCESS, grantedAccess,
                    "3.1.5.4.4 The return parameter of GrantedAccess MUST be set to DesiredAccess if DesiredAccess contains only valid access masks for the user object.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
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
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain with USER_SERVER_TRUST_ACCOUNT and USER_ALL_ACCESS.")]
        public void SamrCreateUser2InDomain_USER_SERVER_TRUST_ACCOUNT_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
                    testUserName, ACCOUNT_TYPE.USER_SERVER_TRUST_ACCOUNT, User_ACCESS_MASK.USER_ALL_ACCESS);
                uint grantedAccess, relativeId;
                HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_SERVER_TRUST_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain succeeded.");
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);
                Site.Assert.IsTrue(relativeId >= 1000, "The Rid value MUST be greater than or equal to 1000");     
                Site.Assert.AreEqual((uint)User_ACCESS_MASK.USER_ALL_ACCESS, grantedAccess,
                    "3.1.5.4.4 The return parameter of GrantedAccess MUST be set to DesiredAccess if DesiredAccess contains only valid access masks for the user object.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
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
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain with existing sAMAccountName.")]
        public void SamrCreateUser2InDomain_STATUS_USER_EXISTS_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a user with Name:{0}, AccountType:{1} and DesiredAccess:{2}",
               testUserName, ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, User_ACCESS_MASK.USER_ALL_ACCESS);
            uint grantedAccess, relativeId;
            try
            {
                HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain succeeded.");
                Site.Assert.AreNotEqual(IntPtr.Zero, _userHandle, "The returned user handle is: {0}.", _userHandle);

                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a USER_NORMAL_ACCOUNT with existing sAMAccountName.");
                IntPtr newUserHandle = IntPtr.Zero;
                result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out newUserHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_USER_EXISTS, result, "3.1.1.8.4 If the sAMAccountName attribute value is NOT unique with respect to the union of all sAMAccountName and msDS-AdditionalSamAccountName attribute values for all other objects within the scope of the account and built-in domain, the server MUST return an error status.");
                Site.Assert.AreEqual(IntPtr.Zero, newUserHandle, "The returned user handle should be null.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a USER_WORKSTATION_TRUST_ACCOUNT with existing sAMAccountName.");
                result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    testUserName, (uint)ACCOUNT_TYPE.USER_WORKSTATION_TRUST_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out newUserHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_USER_EXISTS, result, "3.1.1.8.4 If the sAMAccountName attribute value is NOT unique with respect to the union of all sAMAccountName and msDS-AdditionalSamAccountName attribute values for all other objects within the scope of the account and built-in domain, the server MUST return an error status.");
                Site.Assert.AreEqual(IntPtr.Zero, newUserHandle, "The returned user handle should be null.");

                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a USER_SERVER_TRUST_ACCOUNT with existing sAMAccountName.");
                result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                     testUserName, (uint)ACCOUNT_TYPE.USER_SERVER_TRUST_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                     out newUserHandle, out grantedAccess, out relativeId);
                Site.Assert.AreEqual(HRESULT.STATUS_USER_EXISTS, result, "3.1.1.8.4 If the sAMAccountName attribute value is NOT unique with respect to the union of all sAMAccountName and msDS-AdditionalSamAccountName attribute values for all other objects within the scope of the account and built-in domain, the server MUST return an error status.");
                Site.Assert.AreEqual(IntPtr.Zero, newUserHandle, "The returned user handle should be null.");
            }
            finally
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
                HRESULT result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
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
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain with DomainHandle.HandleType not equal to Domain.")]
        public void SamrCreateUser2InDomain_InvalidHandle_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user using server handle instead of domain handle.");
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_serverHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "3.1.5.4.4 The server MUST return an error if DomainHandle.HandleType is not equal to Domain.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain without required access.")]
        public void SamrCreateUser2InDomain_STATUS_ACCESS_DENIED_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user without required access.");
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrCreateUser2InDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "3.1.5.4.4 DomainHandle.GrantedAccess MUST have the required access specified in section 3.1.2.1. Otherwise, the server MUST return STATUS_ACCESS_DENIED.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain with invalid AccountType.")]
        public void SamrCreateUser2InDomain_InvalidAccountType_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with invalid AccountType.");
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_INVALID, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateUser2InDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "3.1.5.4.4 The AccountType parameter from the message MUST be equal to exactly one value from the following list.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain with invalid DesiredAccess.")]
        public void SamrCreateUser2InDomain_InvalidDesiredAccess_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.DOMAIN_READ);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with invalid DesiredAccess.");
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_INVALID_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreEqual(HRESULT.STATUS_ACCESS_DENIED, result, "SamrCreateUser2InDomain returns error code:{0}.", result.ToString("X"));
            Site.Assert.AreEqual(IntPtr.Zero, _userHandle, "3.1.5.4.4 otherwise, the request MUST be aborted and STATUS_ACCESS_DENIED MUST be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain with Builtin Domain.")]
        public void SamrCreateUser2InDomain_BuiltinDomain_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, builtinDomainName, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

            Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: create a user with Builtin Domain.");
            uint grantedAccess, relativeId;
            HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                testUserName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                out _userHandle, out grantedAccess, out relativeId);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, 
                "3.1.5.4.4 If DomainHandle.Object refers to the built-in domain, the server MUST abort the request and return a failure code.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("Non-DC")]
        [TestCategory("SAMR-User")]
        [Description("Non-DC Test: This is to test SamrCreateUser2InDomain with invalid sAMAccountName.")]
        public void SamrCreateUser2InDomain_InvalidsAMAccountName_NonDC()
        {
            ConnectAndOpenDomain_NonDC(_samrProtocolAdapter.domainMemberFqdn, _samrProtocolAdapter.domainMemberFqdn, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);
            uint grantedAccess, relativeId;

            string[] invalidNames = new string[] { "  ", "justtest.", "justtest\"", "justtest/", "justtest\\","justtest[", "justtest]",
            "justtest:", "justtest|","justtest<","justtest>","justtest+","justtest=","justtest;","justtest?","justtest,","justtest*","justtotest20characters"};
            foreach (string invalidName in invalidNames)
            {
                Site.Log.Add(LogEntryKind.TestStep, "SamrCreateUser2InDomain: Create a user with sAMAccountName containing only blank spaces.");
                HRESULT result = _samrProtocolAdapter.SamrCreateUser2InDomain(_domainHandle,
                    invalidName, (uint)ACCOUNT_TYPE.USER_NORMAL_ACCOUNT, (uint)User_ACCESS_MASK.USER_ALL_ACCESS,
                    out _userHandle, out grantedAccess, out relativeId);
                if (invalidName.Equals("  "))
                {
                    Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.1.6 sAMAccountName MUST contain at least one non-blank character; on error, return a failure code.");
                }
                else if (invalidName.Equals("justtest."))
                {
                    Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.1.6 sAMAccountName MUST NOT end with a '.' (period) character; on error, return a failure code.");
                }
                else if (invalidName.Equals("justtest20characters"))
                {
                    Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.1.6 sAMAccountName MUST contain less than or equal to 20 characters if the object's objectClass is user; on error, return a failure code.");
                }
                else
                {
                    Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.1.6 sAMAccountName MUST NOT contain any of the following characters. On error, return a failure code.");
                }
            }           
        }

    }
}
