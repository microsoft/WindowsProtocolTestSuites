// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Threading;
using System.Security.Principal;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Modeling;
using System.Security.AccessControl;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    /// <summary>
    /// Test suite to test the implementation for MS-APDS protocol.
    /// </summary>
    [TestClass]
    public partial class SAMR_TestSuite : TestClassBase
    {
        private const string testUserName = "SAMRTestUser";
        private const string testGroupName = "SAMRTestGroup";
        private const string testAliasName = "SAMRTestAlias";
        private const string builtinDomainName = "Builtin";
		private const string AdministratorSid = "S-1-5-32-544";
        private const string WorldSid = "S-1-1-0";
        private const string AccountOperatorsSid = "S-1-5-32-548";

        #region Member Variable Declarations

        /// <summary>
        /// The handle pointing to the server object.
        /// </summary>
        private IntPtr _serverHandle = new IntPtr();

        /// <summary>
        /// The handle pointing to the object object.
        /// </summary>
        private IntPtr _domainHandle = new IntPtr();

        /// <summary>
        /// The handle pointing to the group object.
        /// </summary>
        private IntPtr _groupHandle = new IntPtr();

        /// <summary>
        /// The handle pointing to the alias object.
        /// </summary>
        private IntPtr _aliasHandle = new IntPtr();

        /// <summary>
        /// The handle pointing to the user object.
        /// </summary>
        private IntPtr _userHandle = new IntPtr();

        #endregion

        /// <summary>
        /// This is an instance of SAMR protocol adapter
        /// </summary>
        private static SAMRProtocolAdapter _samrProtocolAdapter;

        /// <summary>
        /// This is an instance of Utilities, which contains helper functions that 
        /// adapter requires
        /// </summary>
        public Utilities utilityObject = null;

        /// <summary>
        /// Uses ClassInitialize to run code before running the first test in the class
        /// </summary>
        /// <param name="testContext">Test context base on the base class.</param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // initialize test context to your ptf config file.
            TestClassBase.Initialize(context, "AD_ServerTestSuite");
        }

        /// <summary>
        /// Uses ClassCleanup to run code after all tests in a class have run
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        /// <summary>
        /// Initialize the test cases
        /// </summary>
        protected override void TestInitialize()
        {
            base.TestInitialize();
            _samrProtocolAdapter = SAMRProtocolAdapter.Instance(Site);
            utilityObject = new Utilities();
            Microsoft.Protocols.TestSuites.ActiveDirectory.Common.UpdatesStorage.GetInstance().Initialize(BaseTestSite);
            //Microsoft.Protocols.TestSuites.Debug.WindowsDebugAdapter.AttachTTT("PDC.contoso.com", "lsass", BaseTestSite.TestProperties["CurrentTestCaseName"].ToString(), Debug.DumpLevel.AfterAttachMemoryOnly);
            //Thread.Sleep(60000);
        }

        /// <summary>
        /// Clear up the test cases
        /// </summary>
        protected override void TestCleanup()
        {
            CloseAllHandles();
            base.TestCleanup();
            if (null != _samrProtocolAdapter)
            {
                _samrProtocolAdapter.Dispose();
            }
            Microsoft.Protocols.TestSuites.ActiveDirectory.Common.UpdatesStorage.GetInstance().Clear();
            Common.Utilities.RemoveUser(_samrProtocolAdapter.pdcFqdn, _samrProtocolAdapter.ADDSPortNum,
                    _samrProtocolAdapter.primaryDomainUserContainerDN, testUserName);
            //Microsoft.Protocols.TestSuites.Debug.WindowsDebugAdapter.DettachTTT("PDC.contoso.com");
            //Thread.Sleep(60000);
        }

        /// <summary>
        /// Close all the handles
        /// </summary>
        private void CloseAllHandles()
        {
            if (_userHandle != IntPtr.Zero)
            {
                CloseHandle(ref _userHandle);
            }
            if (_groupHandle != IntPtr.Zero)
            {
                CloseHandle(ref _groupHandle);
            }
            if (_aliasHandle != IntPtr.Zero)
            {
                CloseHandle(ref _aliasHandle);
            }
            if (_domainHandle != IntPtr.Zero)
            {
                CloseHandle(ref _domainHandle);
            }
            if (_serverHandle != IntPtr.Zero)
            {
                CloseHandle(ref _serverHandle);
            }
        }

        /// <summary>
        /// Combined 3 operations: SamrConnect5, SamrLookupDomainInSamServer, SamrOpenDomain
        /// </summary>
        /// <param name="serverName">Variable of type string that Lockout name of the server for which handle needs to be obtained.</param>
        /// <param name="domainName">Variable of string domainName which indicate that the domain name to look up.</param>
        /// <param name="serverHandle">Out parameter of type pointer HANDLE that Lockout the server handle.</param>
        /// <param name="domainHandle">Out parameter of type pointer HANDLE representing the opened domain handle.</param>
        private void ConnectAndOpenDomain(string serverName, string domainName, out IntPtr serverHandle, out IntPtr domainHandle, uint domainDesiredAccess = Utilities.DOMAIN_ALL_ACCESS)
        {

            Site.Log.Add(LogEntryKind.TestStep, "Initialize: Create Samr Bind to the server.");
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.pdcNetBIOSName,
                _samrProtocolAdapter.primaryDomainFqdn,
                _samrProtocolAdapter.DomainAdministratorName,
                _samrProtocolAdapter.DomainUserPassword,
                false,
                true);

            //local Variables.
            HRESULT methodStatus;
            _RPC_SID domainSid;

            Site.Log.Add(LogEntryKind.TestStep, "SamrConnect5: obtain a handle to the server:{0}.", serverName);
            methodStatus = _samrProtocolAdapter.SamrConnect5(
                serverName,
                Utilities.SAM_SERVER_ALL_ACCESS,
                out serverHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrConnect5 returns:{0}.", methodStatus);
            Site.Assert.IsNotNull(serverHandle, "The returned server handle is: {0}.", serverHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrLookupDomainInSamServer: obtain the sid of a domain object, given the object's name.");
            methodStatus = _samrProtocolAdapter.SamrLookupDomainInSamServer(
                serverHandle,
                domainName,
                out domainSid);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrLookupDomainInSamServer returns:{0}.", methodStatus);

            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenDomain: obtain a handle to a domain object, given SID.");
            methodStatus = _samrProtocolAdapter.SamrOpenDomain(
                serverHandle,
                domainDesiredAccess,
                domainSid,
                out domainHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrOpenDomain returns:{0}.", methodStatus);
            Site.Assert.IsNotNull(domainHandle, "The returned domain handle is: {0}.", domainHandle);
        }
        /// <summary>
        /// NonDC version: Combined 3 operations: SamrConnect5, SamrLookupDomainInSamServer, SamrOpenDomain
        /// </summary>
        /// <param name="serverName">Variable of type string that Lockout name of the server for which handle needs to be obtained.</param>
        /// <param name="domainName">Variable of string domainName which indicate that the domain name to look up.</param>
        /// <param name="serverHandle">Out parameter of type pointer HANDLE that Lockout the server handle.</param>
        /// <param name="domainHandle">Out parameter of type pointer HANDLE representing the opened domain handle.</param>
        /// <param name="domainDesiredAccess">Variable of type uint that specifies the required access for DomainHandle.GrantedAccess.</param>
        private void ConnectAndOpenDomain_NonDC(string serverName, string domainName, out IntPtr serverHandle, out IntPtr domainHandle, uint domainDesiredAccess = Utilities.DOMAIN_ALL_ACCESS)
        {
            Site.Log.Add(LogEntryKind.TestStep, "Initialize: Create Samr Bind to the server.");
            _samrProtocolAdapter.SamrBind(
                _samrProtocolAdapter.domainMemberFqdn,
                _samrProtocolAdapter.domainMemberNetBIOSName,
                _samrProtocolAdapter.DMAdminName,
                _samrProtocolAdapter.DMAdminPassword,
                false,
                false);

            HRESULT methodStatus;
            _RPC_SID domainSid;

            Site.Log.Add(LogEntryKind.TestStep, "SamrConnect5: obtain a handle to the server:{0}.", serverName);
            methodStatus = _samrProtocolAdapter.SamrConnect5(
                serverName,
                Utilities.SAM_SERVER_ALL_ACCESS,
                out serverHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrConnect5 returns:{0}.", methodStatus);
            Site.Assert.IsNotNull(serverHandle, "The returned server handle is:{0}.", serverHandle);

            Site.Log.Add(LogEntryKind.TestStep, "SamrLookupDomainInSamServer: obtain the sid of a domain object, given the object's name.");
            methodStatus = _samrProtocolAdapter.SamrLookupDomainInSamServer(
                serverHandle,
                domainName,
                out domainSid);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrLookupDomainInSamServer returns:{0}.", methodStatus);

            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenDomain: obtain a handle to a domain object, given SID.");
            methodStatus = _samrProtocolAdapter.SamrOpenDomain(
                serverHandle,
                domainDesiredAccess,
                domainSid,
                out domainHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrOpenDomain returns:{0}.", methodStatus);
            Site.Assert.IsNotNull(domainHandle, "The returned domain handle is:{0}.", domainHandle);
        }

        /// <summary>
        /// Combined 2 operations: SamrLookupNamesInDomain, SamrOpenGroup
        /// </summary>
        /// <param name="domainHandle">Variable of type pointer HANDLE representing the opened domain handle.</param>
        /// <param name="groupName">Variable of type string Lockout the group to be opened.</param>
        /// <param name="groupHandle">Out parameter of type pointer HANDLE representing the opened group handle.</param>
       private void LookupAndOpenGroup(
            IntPtr domainHandle,
            string groupName,
            out IntPtr groupHandle)
        {
            //local Variables.
            HRESULT methodStatus;

            Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: translate a set of account names into a set of RIDs.");
            List<string> groupNames = new List<string>();
            groupNames.Add(groupName);
            List<uint> groupRids = new List<uint>();
            methodStatus = _samrProtocolAdapter.SamrLookupNamesInDomain(
                domainHandle,
                groupNames,
                out groupRids);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrLookupNamesInDomain returns error code:{0}.", methodStatus);
            Site.Assume.IsTrue(groupRids.Count == 1, "{0} group(s) with name {1} found.", groupRids.Count, groupName);

            Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup: obtain a handle to a group object, given SID.");
            methodStatus = _samrProtocolAdapter.SamrOpenGroup(
                domainHandle,
                Utilities.GROUP_ALL_ACCESS,
                groupRids[0],
                out groupHandle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrOpenGroup returns error code:{0}.", methodStatus);
            Site.Assert.IsNotNull(groupHandle, "The returned handle is: {0}.", groupHandle);
        }
      
       /// <summary>
       /// Method for Non-DC configuration: Create a group in domain DM
       /// </summary>
       /// <param name="groupName">Variable of type string which is used as the name of the group.</param>
       /// <param name="_serverHandle">Variable of type pointer HANDLE that Lockout the server handle.</param>
       /// <param name="_domainHandle">Variable of type pointer HANDLE that Lockout the domain handle.</param>
       /// <param name="_groupHandle">Out parameter of type pointer HANDLE representing the created group handle.</param>
       /// <param name="relativeId">Out parameter of type uint representing the relativeId of the created group object.</param>
       /// <param name="groupDesiredAccess">Variable of type uint that specifies the required access for GroupHandle.GrantedAccess.</param>
       public void CreateGroup_NonDC(string groupName, IntPtr _serverHandle, IntPtr _domainHandle, out IntPtr _groupHandle, out uint relativeId, uint groupDesiredAccess = (uint)Group_ACCESS_MASK.GROUP_ALL_ACCESS)
       {
           Site.Log.Add(LogEntryKind.TestStep, "Record the current DACL of domain DM.");
           _SAMPR_SR_SECURITY_DESCRIPTOR? sd;
           HRESULT hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out sd);
           Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
           Site.Assert.IsNotNull(sd, "The SecurityDescriptor returned by SamrQuerySecurityObject is not null.");
           CommonSecurityDescriptor ObtainedSecurityDescriptor = new CommonSecurityDescriptor(false, true, sd.Value.SecurityDescriptor, 0);
           string oldDACL = ObtainedSecurityDescriptor.GetSddlForm(AccessControlSections.Access);
           Site.Log.Add(LogEntryKind.TestStep, "Old DACL: {0}", oldDACL);

           try
           {
               Site.Log.Add(LogEntryKind.TestStep, "Modify the DACL value of domain DM to get the access to create group in this domain.");
               CommonSecurityDescriptor commonsd = new CommonSecurityDescriptor(false, true, "D:(A;;0xf07ff;;;WD)(A;;0xf07ff;;;BA)(A;;0xf07ff;;;AO)");
               byte[] buffer = new byte[commonsd.BinaryLength];
               commonsd.GetBinaryForm(buffer, 0);
               _SAMPR_SR_SECURITY_DESCRIPTOR securityDescriptor = new _SAMPR_SR_SECURITY_DESCRIPTOR()
               {
                   SecurityDescriptor = buffer,
                   Length = (uint)buffer.Length
               };
               Site.Log.Add(LogEntryKind.TestStep,
                   "SamrSetSecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION. ");
               hResult = _samrProtocolAdapter.SamrSetSecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, securityDescriptor);
               Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrSetSecurityObject must return STATUS_SUCCESS.");
               hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out sd);
               Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
               ObtainedSecurityDescriptor = new CommonSecurityDescriptor(false, true, sd.Value.SecurityDescriptor, 0);
               Site.Log.Add(LogEntryKind.TestStep, "New DACL: {0}", ObtainedSecurityDescriptor.GetSddlForm(AccessControlSections.Access));

               Site.Log.Add(LogEntryKind.TestStep, "Reopen the domain DM");
               _RPC_SID domainSid;
               hResult = _samrProtocolAdapter.SamrLookupDomainInSamServer(
                   _serverHandle,
                   _samrProtocolAdapter.domainMemberFqdn,
                   out domainSid);
               Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrLookupDomainInSamServer returns:{0}.", hResult);
               Site.Log.Add(LogEntryKind.TestStep, "SamrOpenDomain: obtain a handle to a domain object, given SID.");
               hResult = _samrProtocolAdapter.SamrOpenDomain(
                   _serverHandle,
                   Utilities.MAXIMUM_ALLOWED,
                   domainSid,
                   out _domainHandle);
               Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrOpenDomain returns:{0}.", hResult);
               Site.Assert.IsNotNull(hResult, "The returned domain handle is:{0}.", _domainHandle);

               Site.Log.Add(LogEntryKind.TestStep, "SamrCreateGroupInDomain: create a group with Name:{0}, AccountType:{1}, and DesiredAccess:{2}",
                   testGroupName, GROUP_TYPE.GROUP_TYPE_SECURITY_ACCOUNT, groupDesiredAccess);
               HRESULT result = _samrProtocolAdapter.SamrCreateGroupInDomain(
                   _domainHandle,
                   groupName,
                   (uint)groupDesiredAccess,
                   out _groupHandle,
                   out relativeId);
               Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, result, "SamrCreateGroupInDomain succeeded.");
               Site.Assert.AreNotEqual(IntPtr.Zero, _groupHandle, "The returned group handle is: {0}.", _groupHandle);

               Site.Assert.IsTrue(relativeId >= 1000, "The Rid value MUST be greater than or equal to 1000");
           }
           finally
           {
               Site.Log.Add(LogEntryKind.TestStep, "Change back the DACL of domain DM.");
               CommonSecurityDescriptor commonsd = new CommonSecurityDescriptor(false, true, oldDACL);
               byte[] buffer = new byte[commonsd.BinaryLength];
               commonsd.GetBinaryForm(buffer, 0);
               _SAMPR_SR_SECURITY_DESCRIPTOR securityDescriptor = new _SAMPR_SR_SECURITY_DESCRIPTOR()
               {
                   SecurityDescriptor = buffer,
                   Length = (uint)buffer.Length
               };
               Site.Log.Add(LogEntryKind.TestStep,
                   "SamrSetSecurityObject, SecurityInformation: OWNER_SECURITY_INFORMATION. ");
               hResult = _samrProtocolAdapter.SamrSetSecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, securityDescriptor);
               Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrSetSecurityObject must return STATUS_SUCCESS.");
               hResult = _samrProtocolAdapter.SamrQuerySecurityObject(_domainHandle, SecurityInformation.DACL_SECURITY_INFORMATION, out sd);
               Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, hResult, "SamrQuerySecurityObject must return STATUS_SUCCESS.");
               ObtainedSecurityDescriptor = new CommonSecurityDescriptor(false, true, sd.Value.SecurityDescriptor, 0);
               Site.Assert.AreEqual(oldDACL, ObtainedSecurityDescriptor.GetSddlForm(AccessControlSections.Access), "The DACL of domain DM should be changed back");
           }
       }

       /// <summary>
       /// Combined 6 operations: SamrBind, SamrConnect5, SamrLookupDomainInSamServer, SamrOpenDomainSamrLookupNamesInDomain, SamrOpenUser
       /// </summary>
       /// <param name="domainHandle">Variable of type pointer HANDLE representing the opened domain handle.</param>
       /// <param name="userName">Variable of type string Lockout the user to be opened.</param>
       /// <param name="userHandle">Out parameter of type pointer HANDLE representing the opened user handle.</param>
       private void ConnectAndOpenUser(
            string serverName, 
            string domainName,
            string userName,
            out IntPtr userHandle,
            uint desiredAccess = (uint)Common_ACCESS_MASK.MAXIMUM_ALLOWED)
       {
           //local Variables.
           HRESULT methodStatus;

           Site.Log.Add(LogEntryKind.TestStep, "SamrBind-->SamrConnect5-->SamrLookupDomainInSamServer-->SamrOpenDomain");
           ConnectAndOpenDomain(serverName, domainName, out _serverHandle, out _domainHandle);
            
           Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: translate a set of account names into a set of RIDs.");
           List<string> userNames = new List<string>();
           userNames.Add(userName);
           List<uint> userRids = new List<uint>();
           methodStatus = _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, userNames, out userRids);
           Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrLookupNamesInDomain returns:{0}.", methodStatus);
           Site.Assume.IsTrue(userRids.Count == 1, "{0} user(s) with name {1} found.", userRids.Count, userName);

           Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser with desiredAccess as {0}.", desiredAccess);
           methodStatus = _samrProtocolAdapter.SamrOpenUser(_domainHandle, desiredAccess, userRids[0], out userHandle);
           Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrOpenUser succeeded.");
           Site.Assert.IsNotNull(userHandle, "The returned user handle is: {0}.", userHandle);
       }

       /// <summary>
       /// NonDC Version: Combined 6 operations: SamrBind, SamrConnect5, SamrLookupDomainInSamServer, SamrOpenDomainSamrLookupNamesInDomain, SamrOpenUser
       /// </summary>
       /// <param name="domainHandle">Variable of type pointer HANDLE representing the opened domain handle.</param>
       /// <param name="userName">Variable of type string Lockout the user to be opened.</param>
       /// <param name="userHandle">Out parameter of type pointer HANDLE representing the opened user handle.</param>
       private void ConnectAndOpenUser_NonDC(
            string serverName,
            string domainName,
            string userName,
            out IntPtr userHandle,
            uint desiredAccess = (uint)Common_ACCESS_MASK.MAXIMUM_ALLOWED)
       {
           //local Variables.
           HRESULT methodStatus;

           Site.Log.Add(LogEntryKind.TestStep, "SamrBind-->SamrConnect5-->SamrLookupDomainInSamServer-->SamrOpenDomain");
           ConnectAndOpenDomain_NonDC(serverName, domainName, out _serverHandle, out _domainHandle, Utilities.MAXIMUM_ALLOWED);

           Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: translate a set of account names into a set of RIDs.");
           List<string> userNames = new List<string>();
           userNames.Add(userName);
           List<uint> userRids = new List<uint>();
           methodStatus = _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, userNames, out userRids);
           Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrLookupNamesInDomain returns:{0}.", methodStatus);
           Site.Assume.IsTrue(userRids.Count == 1, "{0} user(s) with name {1} found.", userRids.Count, userName);

           Site.Log.Add(LogEntryKind.TestStep, "SamrOpenUser with desiredAccess as {0}.", desiredAccess);
           methodStatus = _samrProtocolAdapter.SamrOpenUser(_domainHandle, desiredAccess, userRids[0], out userHandle);
           Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrOpenUser succeeded.");
           Site.Assert.IsNotNull(userHandle, "The returned user handle is: {0}.", userHandle);
       }

       /// <summary>
       /// Combined 6 operations: SamrBind, SamrConnect5, SamrLookupDomainInSamServer, SamrOpenDomainSamrLookupNamesInDomain, SamrOpenGroup
       /// </summary>
       /// <param name="domainHandle">Variable of type pointer HANDLE representing the opened domain handle.</param>
       /// <param name="groupName">Variable of type string Lockout the group to be opened.</param>
       /// <param name="userHandle">Out parameter of type pointer HANDLE representing the opened group handle.</param>
       private void ConnectAndOpenGroup(
            string serverName,
            string domainName,
            string groupName,
            out IntPtr groupHandle,
            uint desiredAccess = (uint)Common_ACCESS_MASK.MAXIMUM_ALLOWED)
       {
           //local Variables.
           HRESULT methodStatus;

           Site.Log.Add(LogEntryKind.TestStep, "SamrBind-->SamrConnect5-->SamrLookupDomainInSamServer-->SamrOpenDomain");
           ConnectAndOpenDomain(serverName, domainName, out _serverHandle, out _domainHandle);

           Site.Log.Add(LogEntryKind.TestStep, "SamrLookupNamesInDomain: translate a set of account names into a set of RIDs.");
           List<string> groupNames = new List<string>();
           groupNames.Add(groupName);
           List<uint> groupRids = new List<uint>();
           methodStatus = _samrProtocolAdapter.SamrLookupNamesInDomain(_domainHandle, groupNames, out groupRids);
           Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrLookupNamesInDomain returns:{0}.", methodStatus);
           Site.Assume.IsTrue(groupRids.Count == 1, "{0} user(s) with name {1} found.", groupRids.Count, groupName);

           Site.Log.Add(LogEntryKind.TestStep, "SamrOpenGroup with desiredAccess as {0}.", desiredAccess);
           methodStatus = _samrProtocolAdapter.SamrOpenGroup(_domainHandle, desiredAccess, groupRids[0], out groupHandle);
           Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrOpenGroup succeeded.");
           Site.Assert.IsNotNull(groupHandle, "The returned group handle is: {0}.", groupHandle);
       }

        /// <summary>
        /// Combined 1 operation: SamrCloseHandle
        /// </summary>
        /// <param name="handle">Variable of type pointer HANDLE representing the handle to be closed.</param>
        private void CloseHandle(ref IntPtr handle)
        {
            //local Variables.
            HRESULT methodStatus;

            Site.Log.Add(LogEntryKind.TestStep, "SamrCloseHandle: close a context handle obtained.");
            methodStatus = _samrProtocolAdapter.SamrCloseHandle(ref handle);
            Site.Assert.AreEqual(HRESULT.STATUS_SUCCESS, methodStatus, "SamrCloseHandle returns error code:{0}.", methodStatus);
        }
    }

    public class CreateTempUser : Common.IUpdate
    {
        SAMRProtocolAdapter _samrProtocolAdapter;
        AdtsUserAccountControl _userAccountControl;
        public CreateTempUser(
            SAMRProtocolAdapter adapter,
            string username,
            string password,
            AdtsUserAccountControl userAccountControl = AdtsUserAccountControl.ADS_UF_NORMAL_ACCOUNT| AdtsUserAccountControl.ADS_UF_DONT_EXPIRE_PASSWD)
        {
            _userAccountControl = userAccountControl;
            _samrProtocolAdapter = adapter;
            Username = username;
            Password = password;
        }
        public string Password;
        public string Username;

        public bool Invoke()
        {
            return Common.Utilities.NewUser(
                _samrProtocolAdapter.pdcFqdn,
                _samrProtocolAdapter.ADDSPortNum,
                _samrProtocolAdapter.primaryDomainUserContainerDN,
                Username,
                Password,
                null,
                null,
                _userAccountControl);
        }

        public bool Revert()
        {
            return Common.Utilities.RemoveUser(
                _samrProtocolAdapter.pdcFqdn,
                _samrProtocolAdapter.ADDSPortNum,
                _samrProtocolAdapter.primaryDomainUserContainerDN,
                Username);
        }
    }
}
