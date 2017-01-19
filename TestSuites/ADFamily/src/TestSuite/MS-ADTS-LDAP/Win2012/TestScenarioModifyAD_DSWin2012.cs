// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Principal;
using System.Threading;
using System.Text;
using System.Net;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Security.AccessControl;


namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    /// <summary>
    /// MS-ADTS-LDAP traditional testcase
    /// </summary>
    [TestClass]
    public class TestScenarioModifyAD_DSWin2012 : TestClassBase
    {
        #region Test Suite Initialization

        /// <summary>
        /// Class initialization
        /// </summary>
        /// <param name="testContext">test context</param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
            EnvironmentConfig.ServerVer = (ServerVersion)AD_LDAPModelAdapter.Instance(BaseTestSite).PDCOSVersion;
        }

        /// <summary>
        /// Class cleanup
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Case Initialization and clean up

        /// <summary>
        /// Test initialize
        /// </summary>
        protected override void TestInitialize()
        {
            base.TestInitialize();
            Site.DefaultProtocolDocShortName = "MS-ADTS-LDAP";
            AD_LDAPModelAdapter.Instance(Site).Initialize();
            Utilities.DomainAdmin = AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName;
            Utilities.DomainAdminPassword = AD_LDAPModelAdapter.Instance(Site).DomainUserPassword;
            Utilities.TargetServerFqdn = AD_LDAPModelAdapter.Instance(Site).PDCNetbiosName + "." + AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName + ":" + AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
        }

        /// <summary>
        /// Test clean up
        /// </summary>
        protected override void TestCleanup()
        {
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            System.DirectoryServices.Protocols.ModifyRequest mod = new System.DirectoryServices.Protocols.ModifyRequest("",
                DirectoryAttributeOperation.Add, "schemaupgradeinprogress", "0");
            con.SendRequest(mod);
            base.TestCleanup();
        }

        #endregion

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Modify_EnforceSchemaConstrains_Range()
        {
            #region variables

            //set employeeID attribute out of range, upperRange is 16
            const int upperRange = 16;
            string attrName = "employeeID";
            string attrValueOutOfRange = new string('1', upperRange + 10);
            string userName = "testModifyConstraints";
            string userDN = "CN=" + userName + ",CN=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            int errorCode;
            bool failed = false;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(addr, int.Parse(port)),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Modify Enforce Schema Constraints RangeUpper

            if (!Utilities.IsObjectExist(userDN, addr, port))
            {
                ManagedAddRequest addReq = new ManagedAddRequest(userDN, "user");
                System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }
            System.DirectoryServices.Protocols.ModifyRequest modReq = new System.DirectoryServices.Protocols.ModifyRequest(
                userDN,
                DirectoryAttributeOperation.Add,
                attrName,
                attrValueOutOfRange);
            System.DirectoryServices.Protocols.ModifyResponse modRep = null;
            try
            {
                modRep = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(modReq);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.ConstraintViolation)
                {
                    errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_RANGE_CONSTRAINT) failed = true;
                }
            }
            BaseTestSite.Assert.IsTrue(
                    failed,
                    @"All attribute values must be compliant with the rangeUpper and rangeLower constraints 
                    of the schema (see section 3.1.1.2.3). If a supplied value violates a rangeUpper or rangeLower
                    constraint, then the Add fails with constraintViolation / ERROR_DS_RANGE_CONSTRAINT.");
            #endregion

            #region delete the test user

            System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(userDN);
            System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);

            #endregion
        }


        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Modify_Constraints_MultipleDescriptions()
        {
            #region variables

            string userName = "testModifyConstraints";
            string userDN = "CN=" + userName + ",CN=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            int errorCode;
            bool failed = false;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(addr, int.Parse(port)),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add an object for modify constraint test

            if (!Utilities.IsObjectExist(userDN, addr, port))
            {
                ManagedAddRequest addReq = new ManagedAddRequest(userDN, "user");
                System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }

            #endregion

            #region Modify constraint for class user

            System.DirectoryServices.Protocols.ModifyRequest modReq = new System.DirectoryServices.Protocols.ModifyRequest(
                    userDN,
                    DirectoryAttributeOperation.Add,
                    "description",
                    new string[] { "aaa", "bbb" });
            System.DirectoryServices.Protocols.ModifyResponse modRep = null;
            try
            {
                modRep = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(modReq);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.AttributeOrValueExists)
                {
                    errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_SINGLE_VALUE_CONSTRAINT) failed = true;
                }
            }
            BaseTestSite.Assert.IsTrue(
                    failed,
                    @"If the modify operation adds or replaces values of the description attribute on a SAM-specific object
                    (section 3.1.1.5.2.3), and results in more than one value in the attribute, then the modification fails 
                    with attributeOrValueExists / ERROR_DS_SINGLE_VALUE_CONSTRAINT.");

            #endregion

            #region Delete the user for modify test

            System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(
                    "CN=testModifyConstraints,CN=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC);
            System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Modify_Constraints_DisallowedAttributes()
        {
            #region variables

            //The values of the attributes are not important, but should be complied with the attribute syntax
            string attrValue = "100";
            int attrNum;
            int errorCode;
            bool failed = false;
            string userName = "tempUser";
            string userDN = "CN=" + userName + ",CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string groupName = "tempGroup";
            string groupDN = "CN=" + groupName + ",CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string testObjName = "tempObj";
            string testObjDN = "CN=" + testObjName + ",CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add a user, a group and a non SAM-specific object(classStore) to test modify constraints

            if (!Utilities.IsObjectExist(userDN, addr, port))
            {
                ManagedAddRequest addReq = new ManagedAddRequest(userDN, "user");
                System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }
            if (!Utilities.IsObjectExist(groupDN, addr, port))
            {
                ManagedAddRequest addReq = new ManagedAddRequest(groupDN, "group");
                System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }
            if (!Utilities.IsObjectExist(testObjDN, addr, port))
            {
                ManagedAddRequest addReq = new ManagedAddRequest(testObjDN, "classStore");
                System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }

            #endregion

            #region Modify constraint for class user
            attrNum = 15;
            System.DirectoryServices.Protocols.DirectoryAttributeModification[] modAttr1 = new DirectoryAttributeModification[attrNum];
            for (int i = 0; i < attrNum; i++)
            {
                modAttr1[i] = new DirectoryAttributeModification();
                modAttr1[i].Operation = DirectoryAttributeOperation.Replace;
                modAttr1[i].Add(attrValue);
            }
            modAttr1[0].Name = "badPasswordTime";
            modAttr1[1].Name = "badPwdCount";
            modAttr1[2].Name = "dBCSPwd";
            modAttr1[3].Name = "lastLogoff";
            modAttr1[4].Name = "lastLogon";
            modAttr1[5].Name = "lastLogonTimestamp";
            modAttr1[6].Name = "lmPwdHistory";
            modAttr1[7].Name = "logonCount";
            modAttr1[8].Name = "memberOf";
            modAttr1[9].Name = "msDS-User-Account-Control-Computed";
            modAttr1[10].Name = "ntPwdHistory";
            modAttr1[11].Name = "rid";
            modAttr1[12].Name = "sAMAccountType";
            modAttr1[13].Name = "supplementalCredentials";
            modAttr1[14].Name = "isCriticalSystemObject";
            modAttr1[14].Clear();
            modAttr1[14].Add("TRUE");

            for (int i = 0; i < attrNum; i++)
            {
                System.DirectoryServices.Protocols.ModifyRequest modReq = new System.DirectoryServices.Protocols.ModifyRequest(userDN, modAttr1[i]);
                System.DirectoryServices.Protocols.ModifyResponse modRep = null;
                try
                {
                    modRep = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(modReq);
                }
                catch (DirectoryOperationException e)
                {
                    if (e.Response.ResultCode == ResultCode.UnwillingToPerform)
                    {
                        errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                        if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_ATTRIBUTE_OWNED_BY_SAM) failed = true;
                    }
                }
                BaseTestSite.Assert.IsTrue(
                    failed,
                    @"In AD DS, the following attributes are disallowed in a Modify for an object of class user:
                    badPasswordTime, badPwdCount, dBCSPwd, isCriticalSystemObject, lastLogoff, lastLogon, 
                    lastLogonTimestamp, lmPwdHistory, logonCount, memberOf, msDS-User-Account-Control-Computed, 
                    ntPwdHistory, objectSid, rid, sAMAccountType, and supplementalCredentials. If one of these 
                    attributes is specified in an Add, the Add returns unwillingToPerform / ERROR_DS_ATTRIBUTE_OWNED_BY_SAM.");
                failed = false;
            }

            #endregion

            #region Modify constraint for class group
            attrNum = 5;
            System.DirectoryServices.Protocols.DirectoryAttributeModification[] modAttr2 = new DirectoryAttributeModification[attrNum];
            for (int i = 0; i < attrNum; i++)
            {
                modAttr2[i] = new DirectoryAttributeModification();
                modAttr2[i].Operation = DirectoryAttributeOperation.Replace;
                modAttr2[i].Add(attrValue);
            }
            modAttr2[0].Name = "memberOf";
            modAttr2[1].Name = "rid";
            modAttr2[1].Clear();
            modAttr2[1].Add("512");
            modAttr2[2].Name = "sAMAccountType";
            modAttr2[2].Clear();
            modAttr2[2].Add("805306370");
            modAttr2[3].Name = "userPassword";
            modAttr2[4].Name = "isCriticalSystemObject";
            modAttr2[4].Clear();
            modAttr2[4].Add("TRUE");

            for (int i = 0; i < attrNum; i++)
            {
                System.DirectoryServices.Protocols.ModifyRequest modReq = new System.DirectoryServices.Protocols.ModifyRequest(groupDN, modAttr2[i]);
                System.DirectoryServices.Protocols.ModifyResponse modRep = null;
                try
                {
                    modRep = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(modReq);
                }
                catch (DirectoryOperationException e)
                {
                    if (e.Response.ResultCode == ResultCode.UnwillingToPerform)
                    {
                        errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                        if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_ATTRIBUTE_OWNED_BY_SAM) failed = true;
                    }
                }
                BaseTestSite.Assert.IsTrue(
                    failed,
                    @"In AD DS, the following attributes are disallowed in a Modify for an object of class group:
                    isCriticalSystemObject, memberOf, objectSid, rid, sAMAccountType, and userPassword. 
                    If one of these attributes is specified in an Add, the Add returns unwillingToPerform / ERROR_DS_ATTRIBUTE_OWNED_BY_SAM.");
                failed = false;
            }

            #endregion

            #region Modify constraint for class not a SAM-specific object class

            attrNum = 7;
            System.DirectoryServices.Protocols.DirectoryAttributeModification[] modAttr3 = new DirectoryAttributeModification[attrNum];
            for (int i = 0; i < attrNum; i++)
            {
                modAttr3[i] = new DirectoryAttributeModification();
                modAttr3[i].Operation = DirectoryAttributeOperation.Replace;
                modAttr3[i].Add(attrValue);
            }
            modAttr3[0].Name = "lmPwdHistory";
            modAttr3[1].Name = "ntPwdHistory";
            modAttr3[2].Name = "samAccountName";
            modAttr3[3].Name = "sAMAccountType";
            modAttr3[3].Clear();
            modAttr3[3].Add("805306370");
            modAttr3[4].Name = "supplementalCredentials";
            modAttr3[5].Name = "unicodePwd";
            modAttr3[6].Name = "isCriticalSystemObject";
            modAttr3[6].Clear();
            modAttr3[6].Add("TRUE");

            for (int i = 0; i < attrNum; i++)
            {
                System.DirectoryServices.Protocols.ModifyRequest modReq = new System.DirectoryServices.Protocols.ModifyRequest(testObjDN, modAttr3[i]);
                System.DirectoryServices.Protocols.ModifyResponse modRep = null;
                try
                {
                    modRep = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(modReq);
                }
                catch (DirectoryOperationException e)
                {
                    if (e.Response.ResultCode == ResultCode.UnwillingToPerform)
                    {
                        errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                        if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_ILLEGAL_MOD_OPERATION) failed = true;
                    }
                }
                BaseTestSite.Assert.IsTrue(
                    failed,
                    @"In AD DS, the following attributes are disallowed in an Add for an object whose
                    class is not a SAM-specific object class (see 3.1.1.5.2.3): isCriticalSystemObject,
                    lmPwdHistory, ntPwdHistory, objectSid, samAccountName, sAMAccountType, supplementalCredentials,
                    and unicodePwd. If one of these attributes is specified in an Add, the Add returns
                    unwillingToPerform / ERROR_DS_ILLEGAL_MOD_OPERATION.");
                failed = false;
            }

            #endregion

            #region Delete all the test user, groups and not SAM-specific objects

            System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(userDN);
            System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);

            delReq = new System.DirectoryServices.Protocols.DeleteRequest(groupDN);
            delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);

            delReq = new System.DirectoryServices.Protocols.DeleteRequest(testObjDN);
            delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Modify_ObjectClass_Updates()
        {
            #region variables

            bool failed = false;
            string userDN = "CN=" + AD_LDAPModelAdapter.Instance(Site).testUser7Name + ",CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            int errorCode;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2003, "Server OS version should be not less than Windows Server 2003");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));

            #endregion

            #region Modify Object Class Update for class user

            System.DirectoryServices.Protocols.ModifyRequest modReq = new System.DirectoryServices.Protocols.ModifyRequest(
                    userDN,
                    DirectoryAttributeOperation.Replace,
                    "objectClass",
                    "computer");
            System.DirectoryServices.Protocols.ModifyResponse modRep = null;
            try
            {
                modRep = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(modReq);
            }
            catch (DirectoryOperationException e)
            {
                if (EnvironmentConfig.ServerVer == ServerVersion.Win2003)
                {
                    if (e.Response.ResultCode == ResultCode.UnwillingToPerform)
                    {
                        errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                        if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_ILLEGAL_MOD_OPERATION) failed = true;
                    }
                }
                else if (EnvironmentConfig.ServerVer >= ServerVersion.Win2008)
                {
                    if (e.Response.ResultCode == ResultCode.ObjectClassViolation)
                    {
                        errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                        if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_ILLEGAL_MOD_OPERATION) failed = true;
                    }
                }
                else failed = false;
            }
            BaseTestSite.Assert.IsTrue(
                failed,
                @"If the DC functional level is DS_BEHAVIOR_WIN2003, unwillingToPerform / ERROR_DS_ILLEGAL_MOD_OPERATION is returned.
                If the DC functional level is DS_BEHAVIOR_WIN2008 or greater, objectClassViolation / ERROR_DS_ILLEGAL_MOD_OPERATION is returned.");

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Modify_msDS_AdditionalDNSHostName()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");

            AD_LDAPModelAdapter.Instance(Site).SetConnectAndBind(
                ADImplementations.AD_DS,
                AD_LDAPModelAdapter.Instance(Site).PDCNetbiosName);

            string dn = "cn=" + AD_LDAPModelAdapter.Instance(Site).testComputer1Name + ",cn=Computers," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;

            Utilities.SetAccessRights(
                dn,
                AD_LDAPModelAdapter.Instance(Site).testUserName,
                AD_LDAPModelAdapter.Instance(Site).currentWorkingDC.Domain.NetbiosName,
                ActiveDirectoryRights.WriteProperty,
                AccessControlType.Allow);
            Utilities.RemoveControlAcessRights(
                dn,
                AD_LDAPModelAdapter.Instance(Site).testUserName,
                AD_LDAPModelAdapter.Instance(Site).currentWorkingDC.Domain.NetbiosName,
                ValidatedWrite.Validated_MS_DS_Additional_DNS_Host_Name,
                ActiveDirectoryRights.Self,
                AccessControlType.Deny);
            Utilities.SetControlAcessRights(
                dn,
                AD_LDAPModelAdapter.Instance(Site).testUserName,
                AD_LDAPModelAdapter.Instance(Site).currentWorkingDC.Domain.NetbiosName,
                ValidatedWrite.Validated_MS_DS_Additional_DNS_Host_Name,
                ActiveDirectoryRights.Self,
                AccessControlType.Allow);

            DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}:{1}/{2}",
                AD_LDAPModelAdapter.Instance(Site).currentWorkingDC.NetbiosName,
                AD_LDAPModelAdapter.Instance(Site).currentPort,
                dn),
                AD_LDAPModelAdapter.Instance(Site).testUserName,
                 AD_LDAPModelAdapter.Instance(Site).testUserPwd);

            string testDNS = AD_LDAPModelAdapter.Instance(Site).testComputer1Name + "." + AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName;
            entry.Properties["msds-additionaldnshostname"].Value = testDNS;
            entry.CommitChanges();
            entry.RefreshCache();
            BaseTestSite.Assert.Pass("Should success when requestor {0} has Validated-msDS-Additional-DNS-Host-Name Extended Right on computer object {1}", AD_LDAPModelAdapter.Instance(Site).testUserName, dn);

            Utilities.RemoveControlAcessRights(
                dn,
                AD_LDAPModelAdapter.Instance(Site).testUserName,
                AD_LDAPModelAdapter.Instance(Site).currentWorkingDC.Domain.NetbiosName,
                ValidatedWrite.Validated_MS_DS_Additional_DNS_Host_Name,
                ActiveDirectoryRights.Self,
                AccessControlType.Allow);
            Utilities.SetControlAcessRights(
                dn,
                AD_LDAPModelAdapter.Instance(Site).testUserName,
                AD_LDAPModelAdapter.Instance(Site).currentWorkingDC.Domain.NetbiosName,
                ValidatedWrite.Validated_MS_DS_Additional_DNS_Host_Name,
                ActiveDirectoryRights.Self,
                AccessControlType.Deny);

            entry = new DirectoryEntry(string.Format("LDAP://{0}:{1}/{2}",
                AD_LDAPModelAdapter.Instance(Site).currentWorkingDC.NetbiosName,
                AD_LDAPModelAdapter.Instance(Site).currentPort,
                dn),
                AD_LDAPModelAdapter.Instance(Site).testUserName,
                 AD_LDAPModelAdapter.Instance(Site).testUserPwd);
            entry.Properties["msds-additionaldnshostname"].Value = testDNS;
            try
            {
                entry.CommitChanges();
                BaseTestSite.Assert.Fail("Should fail when requestor {0} does not have Validated-msDS-Additional-DNS-Host-Name Extended Right on computer object {1}", AD_LDAPModelAdapter.Instance(Site).testUser7Name, dn);
            }
            catch
            {
                BaseTestSite.Assert.Pass("Should fail when requestor {0} does not have Validated-msDS-Additional-DNS-Host-Name Extended Right on computer object {1}", AD_LDAPModelAdapter.Instance(Site).testUser7Name, dn);
            }
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Modify_SecurityDescriptor_ProcessingSpecifics()
        {
            #region variables

            string netBIOSName = AD_LDAPModelAdapter.Instance(Site).PrimaryDomainNetBiosName;
            string operUser = "operUser";
            string operUserDN = "CN=" + operUser + ",CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string testUser = "testUser";
            string testUserDN = "CN=" + testUser + ",CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string userPwd = "Password01!";
            bool failed = false;
            ActiveDirectorySecurity securityDescriptor = new ActiveDirectorySecurity();
            string testUserOwner = null;

            #endregion

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less then Windows Server 2012");
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            string port = AD_LDAPModelAdapter.Instance(Site).ADDSPortNum;

            try
            {
                using (LdapConnection con = new LdapConnection(
                    new LdapDirectoryIdentifier(addr, int.Parse(port)),
                    new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                        AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                        AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName)))
                {
                    con.SessionOptions.Sealing = false;
                    con.SessionOptions.Signing = false;

                    #region add a user object for operating the ntSecurityDescriptor modify

                    if (!Utilities.IsObjectExist(operUserDN, addr, port))
                    {
                        Utilities.NewUser(addr, port, "CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC, operUser, userPwd);
                    }

                    #endregion

                    #region add a test user object to be modified

                    if (!Utilities.IsObjectExist(testUserDN, addr, port))
                    {
                        Utilities.NewUser(addr, port, "CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC, testUser, userPwd);
                    }

                    #endregion

                    #region get ntSecurityDescriptor for the test user object to be modified

                    System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                        testUserDN,
                        "(objectClass=user)",
                        System.DirectoryServices.Protocols.SearchScope.Subtree,
                        "ntSecurityDescriptor");
                    System.DirectoryServices.Protocols.SearchResponse searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
                    BaseTestSite.Assert.AreEqual(
                        1,
                        searchRep.Entries[0].Attributes.Count,
                        @"Without the presence of this control, the server returns an SD only when the SD attribute name is explicitly mentioned in the requested attribute list.");
                    DirectoryAttribute attr = searchRep.Entries[0].Attributes["ntSecurityDescriptor"];
                    object[] values = attr.GetValues(Type.GetType("System.Byte[]"));
                    byte[] value = (byte[])values[0];
                    securityDescriptor.SetSecurityDescriptorBinaryForm(value);

                    //GetsSecurityDescriptorOwner method will return the owner part of Secuirty Descriptor
                    testUserOwner = Utilities.GetSecurityDescriptorOwner(securityDescriptor);

                    #endregion
                }

                using (LdapConnection con = new LdapConnection(
                    new LdapDirectoryIdentifier(addr, int.Parse(port)),
                    new NetworkCredential(operUser, userPwd, AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName)))
                {
                    #region modify the test user

                    IdentityReference testUserId = new NTAccount(testUserOwner);
                    securityDescriptor.SetOwner(testUserId);
                    byte[] value = securityDescriptor.GetSecurityDescriptorBinaryForm();

                    DirectoryAttributeModification mod = new DirectoryAttributeModification();
                    mod.Name = "ntSecurityDescriptor";
                    mod.Operation = DirectoryAttributeOperation.Replace;
                    mod.Add(value);
                    System.DirectoryServices.Protocols.ModifyRequest modReq = new System.DirectoryServices.Protocols.ModifyRequest(testUserDN, mod);
                    try
                    {
                        System.DirectoryServices.Protocols.ModifyResponse modRep = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(modReq);
                        if (modRep.ResultCode == ResultCode.Success) failed = false;
                    }
                    catch (DirectoryOperationException e)
                    {
                        if (e.Response.ResultCode == ResultCode.ConstraintViolation)
                        {
                            int errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                            if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_INVALID_OWNER) failed = true;
                        }
                    }

                    BaseTestSite.Assert.IsTrue(
                        failed,
                        @"Microsoft Windows Server 2008 R2 operating system and above impose a restriction on modifying the OWNER field.
                    If a modify operation attempts to set the OWNER SID to a value to which it is currently set, the operation will 
                    fail with a constraintViolation / ERROR_INVALID_OWNER unless at least one of the following conditions applies.
                    Let U be the user performing the modify operation:
                    §	U.SID equals OWNER SID.
                    §	Let G be a group in U.Groups whose SID is being set in the OWNER field. G.Attributes contains SE_GROUP_OWNER but not SE_GROUP_USE_FOR_DENY_ONLY.
                    §	U.Privileges contains SE_RESTORE_PRIVILEGE.
                    This restriction is processed before the security checks described in section 6.1.3.4.");

                    #endregion
                }
            }
            finally
            {
                using (LdapConnection con = new LdapConnection(
                    new LdapDirectoryIdentifier(addr, int.Parse(port)),
                    new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                        AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                        AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName)))
                {
                    #region clean up

                    System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(testUserDN);
                    System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
                    delReq = new System.DirectoryServices.Protocols.DeleteRequest(operUserDN);
                    delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);

                    #endregion
                }
            }
        }
    }
}