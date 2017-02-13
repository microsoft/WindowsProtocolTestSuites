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
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrDeleteUser.")]
        public void SamrDeleteUser()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

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
            string userPath = string.Format("LDAP://{0}/CN={1},{2}", _samrProtocolAdapter.pdcFqdn, testUserName, _samrProtocolAdapter.primaryDomainUserContainerDN);
            Site.Assert.IsFalse(DirectoryEntry.Exists(userPath), "3.1.5.7.3 U MUST be removed from the database.");       
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrDeleteUser whose Rid is less than 1000.")]
        public void SamrDeleteUser_SmallRid()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);
            
            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain the handle to the administrator.");
            HRESULT result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)User_ACCESS_MASK.USER_ALL_ACCESS, Utilities.DOMAIN_USER_RID_ADMIN, out _userHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the administrator.");
            result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.3 If the RID of U's objectSid attribute value is less than 1000, an error MUST be returned.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrDeleteUser with UserHandle.HandleType not equal to User.")]
        public void SamrDeleteUser_InvalidHandle()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser with invalid handle.");
            HRESULT result = _samrProtocolAdapter.SamrDeleteUser(ref _domainHandle);
            Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.3 The server MUST return an error if UserHandle.HandleType is not equal to User.");
        }

        [TestMethod]
        [TestCategory("MS-SAMR")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrDeleteUser with no required access.")]
        public void SamrDeleteUser_STATUS_ACCESS_DENIED()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

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
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain the handle to the created user.");
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
        [TestCategory("SAMR-User")]
        [Description("This is to test SamrDeleteUser for user which is a parent to another object.")]
        public void SamrDeleteUser_WithChildObject()
        {
            ConnectAndOpenDomain(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.PrimaryDomainDnsName, out _serverHandle, out _domainHandle);

            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(_samrProtocolAdapter.PDCIPAddress, int.Parse(_samrProtocolAdapter.ADDSPortNum)),
                new NetworkCredential(_samrProtocolAdapter.DomainAdministratorName,
                    _samrProtocolAdapter.DomainUserPassword, _samrProtocolAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            string treeRootDN = "CN=testRootDN," + _samrProtocolAdapter.primaryDomainUserContainerDN;
            string treeEntry1 = "CN=testEntry1," + treeRootDN;
            try
            {
                Site.Log.Add(LogEntryKind.TestStep, "Add test user with child object.");
                ManagedAddRequest add = new ManagedAddRequest(treeRootDN, "user");
                System.DirectoryServices.Protocols.AddResponse response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);
                add = new ManagedAddRequest(treeEntry1, "classStore");
                response = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(add);

                System.DirectoryServices.Protocols.SearchRequest searchreq = new System.DirectoryServices.Protocols.SearchRequest(treeRootDN, "(ObjectClass=*)", System.DirectoryServices.Protocols.SearchScope.Base);
                System.DirectoryServices.Protocols.SearchResponse searchresp = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchreq);

                byte[] values = (byte[])searchresp.Entries[0].Attributes["objectSid"].GetValues(Type.GetType("System.Byte[]"))[0];
                SecurityIdentifier Sid = new SecurityIdentifier(values, 0);
                string[] sidArray = Sid.ToString().Split('-');
                string rid = sidArray[sidArray.Length - 1];
                Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser: obtain the handle to the created user.");
                HRESULT result = _samrProtocolAdapter.SamrOpenUser(_domainHandle, (uint)User_ACCESS_MASK.USER_ALL_ACCESS, uint.Parse(rid), out _userHandle);


                Site.Log.Add(LogEntryKind.TestStep, "SamrDeleteUser: delete the created user.");
                result = _samrProtocolAdapter.SamrDeleteUser(ref _userHandle);
                Site.Assert.AreNotEqual(HRESULT.STATUS_SUCCESS, result, "3.1.5.7.3 In the DC configuration, if U is a parent to another object, an error MUST be returned.");
            }
            finally
            {
                System.DirectoryServices.Protocols.DeleteRequest delreq = new System.DirectoryServices.Protocols.DeleteRequest(treeRootDN);
                System.DirectoryServices.Protocols.TreeDeleteControl treeDelCtrl = new TreeDeleteControl();
                delreq.Controls.Add(treeDelCtrl);
                System.DirectoryServices.Protocols.DeleteResponse delresp = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delreq);
            }
        }
    }
}
