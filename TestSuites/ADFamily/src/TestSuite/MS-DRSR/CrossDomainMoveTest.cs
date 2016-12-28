// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class CrossDomainMoveTest : DrsrTestClassBase
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            DrsrTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        [BVT]
        [TestCategory("Win2000")]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [Ignore]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling LDAP ModifyDN to move an object from parent domain to child domain. 
            The source DC are expected to complete this operation by invoking IDL_DRSInterDomainMove on target DC.")]
        public void DRSR_DRSInterDomainMove_LDAP_Move_Object_From_Parent_To_Child()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine parentDcType = EnvironmentConfig.Machine.WritableDC1;
            DsServer parentDc = (DsServer)EnvironmentConfig.MachineStore[parentDcType];
            EnvironmentConfig.Machine childDcType = EnvironmentConfig.Machine.CDC;
            DsServer childDc = (DsServer)EnvironmentConfig.MachineStore[childDcType];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            string srcObjDn = ldapAdapter.TestAddUserObj(parentDc);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSInterDomainMove: Adding a new object {0} using LDAP", srcObjDn);
            string newObjRdn = DrsrHelper.GetRDNFromDN(srcObjDn);
            string tgtParentObjDn = "CN=Users," + DrsrHelper.GetNamingContextDN(childDc.Domain, NamingContext.DomainNC);
            string newObjDn = newObjRdn + "," + tgtParentObjDn;

            bool bNewObjAdded = ldapAdapter.IsObjectExist(childDc, newObjDn);
            if (bNewObjAdded)
            {
                ResultCode rCode = ldapAdapter.DeleteObject(childDc, newObjDn);
                BaseTestSite.Assume.AreEqual<ResultCode>(
                    ResultCode.Success, 
                    rCode, 
                    "IDL_DRSInterDomainMove: Checking LDAP remove {0} return value - got: {0}, expect: {1}",
                    newObjDn,
                    (uint)rCode,
                    (uint)ResultCode.Success
                    );
            }

            using (LdapConnection connection = new LdapConnection(new LdapDirectoryIdentifier(parentDc.DnsHostName)))// connecting to the parent domain in which the object exists
            {
                connection.Credential = new System.Net.NetworkCredential(user.Username, user.Password, parentDc.Domain.DNSName);
                connection.SessionOptions.ProtocolVersion = 3;
                connection.SessionOptions.SspiFlag = connection.SessionOptions.SspiFlag | 1; //Set Delegate flag.
                connection.AuthType = AuthType.Kerberos;
                connection.Bind();
                try
                {
                    BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSInterDomainMove: Calling LDAP ModifyDN to move object {0} to {1}.", srcObjDn, newObjDn);
                    ModifyDNRequest modDnRequest = new ModifyDNRequest(srcObjDn, tgtParentObjDn, newObjRdn);
                    modDnRequest.Controls.Add(new CrossDomainMoveControl(childDc.DnsHostName)); // child domain to which the object need to move
                    ModifyDNResponse modDnResponse = (ModifyDNResponse)connection.SendRequest(modDnRequest);
                    BaseTestSite.Assert.AreEqual<ResultCode>(
                        ResultCode.Success,
                        modDnResponse.ResultCode,
                        "IDL_DRSInterDomainMove: Checking return code of LDAP ModifyDN - got: {0}, expect: {1}",
                        (uint)modDnResponse.ResultCode,
                        (uint)ResultCode.Success);

                    bool bOldObjDeleted = ldapAdapter.IsObjectExist(parentDc, srcObjDn);
                    bNewObjAdded = ldapAdapter.IsObjectExist(childDc, newObjDn);
                    BaseTestSite.Assert.IsFalse(bOldObjDeleted, "IDL_DRSInterDomainMove: LDAP ModifyDN: the old object should be deleted from source DC.");
                    BaseTestSite.Assert.IsTrue(bNewObjAdded, "IDL_DRSInterDomainMove: LDAP ModifyDN: the new object should be added into target DC.");
                }
                catch (DirectoryOperationException e)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "IDL_DRSInterDomainMove: LDAP ModifyDN: ResultCode:{0}", e.Response.ResultCode.ToString());
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "IDL_DRSInterDomainMove: LDAP ModifyDN: ErrorMessage:{0}", e.Response.ErrorMessage.ToString());
                    BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, e.Response.ResultCode,
                         "IDL_DRSInterDomainMove: LDAP ModifyDN: server should invoke the IDL_DRSInterDomainMove on target Dc and move the object.");
                }
            }
        }

        [TestCategory("Win2000")]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [Ignore]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling LDAP ModifyDN to move an object from child domain to parent domain. 
            The source DC are expected to complete this operation by invoking IDL_DRSInterDomainMove on target DC.")]
        public void DRSR_DRSInterDomainMove_LDAP_Move_Object_From_Child_To_Parent()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine parentDcType = EnvironmentConfig.Machine.WritableDC1;
            DsServer parentDc = (DsServer)EnvironmentConfig.MachineStore[parentDcType];
            EnvironmentConfig.Machine childDcType = EnvironmentConfig.Machine.CDC;
            DsServer childDc = (DsServer)EnvironmentConfig.MachineStore[childDcType];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            string srcObjDn = ldapAdapter.TestAddUserObj(childDc);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "LDAP add a new object: {0}.", srcObjDn);
            string newObjRdn = DrsrHelper.GetRDNFromDN(srcObjDn);
            string tgtParentObjDn = "CN=Users," + DrsrHelper.GetNamingContextDN(parentDc.Domain, NamingContext.DomainNC);
            string newObjDn = newObjRdn + "," + tgtParentObjDn;

            bool bNewObjAdded = ldapAdapter.IsObjectExist(parentDc, newObjDn);
            if (bNewObjAdded)
            {
               ResultCode rCode = ldapAdapter.DeleteObject(parentDc, newObjDn);
               BaseTestSite.Assume.AreEqual<ResultCode>(ResultCode.Success, rCode, "LDAP: {0} should be removed.", newObjDn);
            }

            using (LdapConnection connection = new LdapConnection(new LdapDirectoryIdentifier(childDc.DnsHostName))) // connecting to the child domain in which the object exists
            {
                connection.Credential = new System.Net.NetworkCredential(user.Username, user.Password, parentDc.Domain.DNSName);
                connection.SessionOptions.ProtocolVersion = 3;
                connection.SessionOptions.SspiFlag = connection.SessionOptions.SspiFlag | 1; //Set Delegate flag.
                connection.AuthType = AuthType.Kerberos;
                connection.Bind();
                try
                {
                    BaseTestSite.Log.Add(LogEntryKind.Comment, "LDAP ModifyDN: moving object {0} to {1}.", srcObjDn, newObjDn);
                    ModifyDNRequest modDnRequest = new ModifyDNRequest(srcObjDn, tgtParentObjDn, newObjRdn);
                    modDnRequest.Controls.Add(new CrossDomainMoveControl(parentDc.DnsHostName)); // parent domain to which the object need to move
                    ModifyDNResponse modDnResponse = (ModifyDNResponse)connection.SendRequest(modDnRequest);
                    BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, modDnResponse.ResultCode,
                        "LDAP ModifyDN: server should invoke IDL_DRSInterDomainMove on target Dc and move the object.");

                    bool bOldObjDeleted = ldapAdapter.IsObjectExist(childDc, srcObjDn);
                    bNewObjAdded = ldapAdapter.IsObjectExist(parentDc, newObjDn);
                    BaseTestSite.Assert.IsFalse(bOldObjDeleted, "LDAP ModifyDN: the old object should be deleted from source DC.");
                    BaseTestSite.Assert.IsTrue(bNewObjAdded, "LDAP ModifyDN: the new object should be added into target DC.");
                }
                catch (DirectoryOperationException e)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "LDAP ModifyDN: ResultCode:{0}", e.Response.ResultCode.ToString());
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "LDAP ModifyDN: ErrorMessage:{0}", e.Response.ErrorMessage.ToString());
                    BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, e.Response.ResultCode,
                         "LDAP ModifyDN: server should invoke the IDL_DRSInterDomainMove on target Dc and move the object.");
                }
            }
        }
    }
}
