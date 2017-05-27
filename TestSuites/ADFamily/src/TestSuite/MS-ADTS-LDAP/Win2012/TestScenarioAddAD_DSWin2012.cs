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


namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    /// <summary>
    /// MS-ADTS-LDAP traditional testcase
    /// </summary>
    [TestClass]
    public class TestScenarioAddAD_DSWin2012 : TestClassBase
    {
        #region Variables

        Guid validated_dns_host_name = new Guid("7c0e2a7c-a419-48e4-a995-10180aad54dd");

        Guid validated_msds_additional_dns_host_name = new Guid("80863791-dbe9-4eb8-837e-7f0ab55d9ac7");

        #endregion

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
            Site.DefaultProtocolDocShortName = "MS-AD_LDAP";
            AD_LDAPModelAdapter.Instance(Site).Initialize();
            Utilities.DomainAdmin = AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName;
            Utilities.DomainAdminPassword = AD_LDAPModelAdapter.Instance(Site).DomainUserPassword;
            Utilities.TargetServerFqdn = AD_LDAPModelAdapter.Instance(Site).PDCNetbiosName + "." + AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName + ":" + AD_LDAPModelAdapter.Instance(Site).ADDSPortNum + "/";
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
        public void LDAP_Add_EnforceSchemaConstrains_mustContain()
        {
            #region variables

            string userName = "testAddConstraints";
            string userDN = "CN=" + userName + ",CN=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            int errorCode;
            bool failed = false;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add Enforce Schema Constraints mustContain
            try
            {
                System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(userDN);
                System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            }
            catch { }
            ManagedAddRequest addReq = new ManagedAddRequest(userDN, "groupOfUniqueNames");
            // classSchema for groupOfUniqueNames mustContain requires a uniqueMember attribtue
            // Not specifying the mustContain attribute when operating LDAP add will cause objectClassViolation error.
            System.DirectoryServices.Protocols.AddResponse addRep = null;
            try
            {
                addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.ObjectClassViolation)
                {
                    errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_MISSING_REQUIRED_ATT) failed = true;
                }
            }
            BaseTestSite.Assert.IsTrue(
                    failed,
                    @"The mayContain/mustContain constraints that are applicable based on the 
                      selected objectClass values are enforced. The computation of the 
                      mayContain/mustContain set takes into consideration the complete inheritance 
                      chain of the structural objectClass and the 88 object class as well as any 
                      auxiliary classes supplied. If any attributes in the mustContain set are not 
                      provided, the Add fails with objectClassViolation / <unrestricted>. If any 
                      attributes provided are not present in either the mayContain or mustContain 
                      sets, the Add fails with objectClassViolation / <unrestricted>.");
            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Add_EnforceSchemaConstrains_Range()
        {
            #region variables

            //set employeeID attribute out of range, upperRange is 16
            const int upperRange = 16;
            string attrName = "employeeID";
            string attrValueOutOfRange = new string('1', upperRange + 10);
            string userName = "testAddConstraints";
            string userDN = "CN=" + userName + ",CN=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            int errorCode;
            bool failed = false;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add Enforce Schema Constraints RangeUpper
            try
            {
                System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(userDN);
                System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            }
            catch { }
            ManagedAddRequest addReq = new ManagedAddRequest(userDN, "user");
            addReq.Attributes.Add(new DirectoryAttribute(attrName, attrValueOutOfRange));
            System.DirectoryServices.Protocols.AddResponse addRep = null;
            try
            {
                addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
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
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Add_EnforceSchemaConstrains_isSingleValued()
        {
            #region variables

            // [MS-ADA1] section 2.217 Attibute employeeID
            // isSingleValued: TRUE
            // set employeeID attribute with multiple values, will cause constraint violation
            string attrName = "employeeID";
            string attrValue1 = "1";
            string attrValue2 = "2";
            string attrValue3 = "3";
            string userName = "testAddConstraints";
            string userDN = "CN=" + userName + ",CN=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            int errorCode;
            bool failed = false;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add Enforce Schema Constraints isSingleValued
            try
            {
                System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(userDN);
                System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            }
            catch { }
            ManagedAddRequest addReq = new ManagedAddRequest(userDN, "user");
            addReq.Attributes.Add(new DirectoryAttribute(attrName, attrValue1, attrValue2, attrValue3));
            System.DirectoryServices.Protocols.AddResponse addRep = null;
            try
            {
                addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.ConstraintViolation)
                {
                    errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_SINGLE_VALUE_CONSTRAINT) failed = true;
                }
            }
            BaseTestSite.Assert.IsTrue(
                    failed,
                    @"All attribute values must be compliant with the isSingleValued constraint of the schema 
                    (see section 3.1.1.2.3). If multiple values are provided for an attribute that is single-valued, 
                    then the Add fails with constraintViolation / <unrestricted>.");
            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Add_EnforceSchemaConstrains_rDNAttID()
        {
            #region variables

            // OrganizationalUnit rDNAttID should be "OU=" instead of "CN="
            // Specify wrong rDNAttID to cause expected errorcode
            string userName = "test RDN";
            string userDN = "CN=" + userName + "," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            int errorCode;
            bool failed = false;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add Enforce Schema Constraints rDNAttID
            try
            {
                System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(userDN);
                System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            }
            catch { }
            ManagedAddRequest addReq = new ManagedAddRequest(userDN, "organizationalUnit");
            System.DirectoryServices.Protocols.AddResponse addRep = null;
            try
            {
                addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.NamingViolation)
                {
                    errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_RDN_DOESNT_MATCH_SCHEMA) failed = true;
                }
            }
            BaseTestSite.Assert.IsTrue(
                    failed,
                    @"The attributeType of the first label of the object DN matches the rDNAttID of 
                    the structural object class or the 88 object class. Otherwise, 
                    namingViolation / ERROR_DS_RDN_DOESNT_MATCH_SCHEMA is returned. 
                    For example, it is not allowed to create an organizationalUnit with CN=test RDN; 
                    the correct RDN for an organizationalUnit object is OU=test. If there is no class C 
                    for which the attributeType is equal to C!rDNAttID, namingViolation / <unrestricted> 
                    is returned.");
            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Add_Constraints_DisallowedAttributes()
        {
            #region variables

            //The values of the attributes are not important, but should be complied with the attribute syntax
            string attrValue = "100";
            int attrNum;
            int errorCode;
            bool failed = false;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add constraint for class user
            attrNum = 15;
            System.DirectoryServices.Protocols.DirectoryAttribute[] attr = new DirectoryAttribute[attrNum];
            attr[0] = new DirectoryAttribute("badPasswordTime", attrValue);
            attr[1] = new DirectoryAttribute("badPwdCount", attrValue);
            attr[2] = new DirectoryAttribute("dBCSPwd", attrValue);
            attr[3] = new DirectoryAttribute("lastLogoff", attrValue);
            attr[4] = new DirectoryAttribute("lastLogon", attrValue);
            attr[5] = new DirectoryAttribute("lastLogonTimestamp", attrValue);
            attr[6] = new DirectoryAttribute("lmPwdHistory", attrValue);
            attr[7] = new DirectoryAttribute("logonCount", attrValue);
            attr[8] = new DirectoryAttribute("memberOf", attrValue);
            attr[9] = new DirectoryAttribute("msDS-User-Account-Control-Computed", attrValue);
            attr[10] = new DirectoryAttribute("ntPwdHistory", attrValue);
            attr[11] = new DirectoryAttribute("rid", attrValue);
            attr[12] = new DirectoryAttribute("sAMAccountType", attrValue);
            attr[13] = new DirectoryAttribute("supplementalCredentials", attrValue);
            attr[14] = new DirectoryAttribute("isCriticalSystemObject", "TRUE");

            for (int i = 0; i < attrNum; i++)
            {
                ManagedAddRequest addReq = new ManagedAddRequest(
                    "CN=testAddConstraints,CN=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC, "user");
                addReq.Attributes.Add(attr[i]);
                System.DirectoryServices.Protocols.AddResponse addRep = null;
                try
                {
                    addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
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
                        @"In AD DS, the following attributes are disallowed in an Add for objects of class user:
                        badPasswordTime, badPwdCount, dBCSPwd, isCriticalSystemObject, lastLogoff, lastLogon, 
                        lastLogonTimestamp, lmPwdHistory, logonCount, memberOf, msDS-User-Account-Control-Computed, 
                        ntPwdHistory, objectSid, rid, sAMAccountType, and supplementalCredentials. If one of these 
                        attributes is specified in an Add, the Add returns unwillingToPerform / ERROR_DS_ATTRIBUTE_OWNED_BY_SAM.");
                failed = false;
            }

            #endregion

            #region Add constraint for class group

            //The values of the attributes are not important, but should be complied with the attribute syntax
            attrNum = 5;
            System.DirectoryServices.Protocols.DirectoryAttribute[] attr2 = new DirectoryAttribute[attrNum];
            attr[0] = new DirectoryAttribute("memberOf", attrValue);
            attr[1] = new DirectoryAttribute("rid", attrValue);
            attr[2] = new DirectoryAttribute("sAMAccountType", attrValue);
            attr[3] = new DirectoryAttribute("userPassword", attrValue);
            attr[4] = new DirectoryAttribute("isCriticalSystemObject", "TRUE");

            for (int i = 0; i < attrNum; i++)
            {
                ManagedAddRequest addReq = new ManagedAddRequest(
                    "CN=testAddConstraints,CN=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC, "group");
                addReq.Attributes.Add(attr[i]);
                System.DirectoryServices.Protocols.AddResponse addRep = null;
                try
                {
                    addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
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
                        @"In AD DS, the following attributes are disallowed in an Add for objects of class group:
                        isCriticalSystemObject, memberOf, objectSid, rid, sAMAccountType, and userPassword. 
                        If one of these attributes is specified in an Add, the Add returns unwillingToPerform / ERROR_DS_ATTRIBUTE_OWNED_BY_SAM.");
                failed = false;
            }

            #endregion

            #region Add constraint for class not a SAM-specific object class

            //The values of the attributes are not important, but should be complied with the attribute syntax
            attrNum = 7;
            System.DirectoryServices.Protocols.DirectoryAttribute[] attr3 = new DirectoryAttribute[attrNum];
            attr[0] = new DirectoryAttribute("lmPwdHistory", attrValue);
            attr[1] = new DirectoryAttribute("ntPwdHistory", attrValue);
            attr[2] = new DirectoryAttribute("samAccountName", attrValue);
            attr[3] = new DirectoryAttribute("sAMAccountType", attrValue);
            attr[4] = new DirectoryAttribute("supplementalCredentials", attrValue);
            attr[5] = new DirectoryAttribute("unicodePwd", attrValue);
            attr[6] = new DirectoryAttribute("isCriticalSystemObject", "TRUE");

            for (int i = 0; i < attrNum; i++)
            {
                ManagedAddRequest addReq = new ManagedAddRequest(
                    "CN=testAddConstraints,CN=users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC, "classStore");
                addReq.Attributes.Add(attr[i]);
                System.DirectoryServices.Protocols.AddResponse addRep = null;
                try
                {
                    addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
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
        }

        //investigating
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_AD_DS_Add_Constraints_ComputerObject()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");

            #region Connect and bind to server

            SocketTransportConfig transportConfig = new SocketTransportConfig();
            transportConfig.RemoteIpAddress = IPAddress.Parse(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress);
            transportConfig.RemoteIpPort = int.Parse(AD_LDAPModelAdapter.Instance(Site).ADDSPortNum, CultureInfo.InvariantCulture);
            transportConfig.BufferSize = AD_LDAPModelAdapter.Instance(Site).transportBufferSize;
            transportConfig.Type = StackTransportType.Tcp;
            transportConfig.Role = Role.Client;
            AdtsLdapClient ldapClientStack = new AdtsLdapClient(AdtsLdapVersion.V3, transportConfig);
            ldapClientStack.Connect();

            //The user do not have RIGHT_DS_CREATE_CHILD access rights
            String userName = AD_LDAPModelAdapter.Instance(Site).testUser7Name;
            String password = AD_LDAPModelAdapter.Instance(Site).testUser7Pwd;
            String netbiosDomain = AD_LDAPModelAdapter.Instance(Site).PrimaryDomainNetBiosName;
            TimeSpan timeout = AD_LDAPModelAdapter.Instance(Site).timeout;

            //Using SSL binding
            //Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.AccountCredential transportCredential = new Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.AccountCredential(
            //    Site.Properties["FullDomainName"], userName, password);
            //Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.ClientSecurityContextAttribute contextAttributes = Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.ClientSecurityContextAttribute.Connection;
            //Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.SspiClientSecurityContext securityContext = new Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.SspiClientSecurityContext(
            //            Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.SecurityPackageType.Kerberos,
            //            transportCredential,
            //            "LDAP/" + Site.Properties["ServerComputerName"],
            //            contextAttributes,
            //            Microsoft.Protocols.TestTools.StackSdk.Security.Sspi.SecurityTargetDataRepresentation.SecurityNetworkDrep);
            //securityContext.Initialize(null);
            //AdtsBindRequestPacket bindRequest = ldapClientStack.CreateSaslBindRequest(securityContext, false);

            AdtsBindRequestPacket bindRequest = ldapClientStack.CreateSimpleBindRequest(userName, password, netbiosDomain);
            //send bind request
            ldapClientStack.SendPacket(bindRequest);
            AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
            AdtsBindResponsePacket bindResponse = (AdtsBindResponsePacket)response;

            //check the connectiong between client and server
            Site.Assert.AreEqual<long>(
                LDAPResult_resultCode.success,
                (long)((BindResponse)bindResponse.GetInnerRequestOrResponse()).resultCode.Value,
                "Bind response result should be LDAPResult_resultCode.success.");

            #endregion

            #region Add a Computer Object

            string computerName = "testAddConstraints";
            string computerObjectDN = "CN=" + computerName + ",CN=Computers," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            KeyValuePair<string, string[]>[] attrs = new KeyValuePair<string, string[]>[5];
            attrs[0] = new KeyValuePair<string, string[]>("objectClass", new string[] { "computer" });
            attrs[1] = new KeyValuePair<string, string[]>("dNSHostName", new string[] { computerName + "." + AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName });
            attrs[2] = new KeyValuePair<string, string[]>("servicePrincipalName", new string[] { "host/" + computerName + "." + AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName, "host/" + computerName });
            attrs[3] = new KeyValuePair<string, string[]>("sAMAccountName", new string[] { computerName + "$" });
            //attrs[4] = new KeyValuePair<string, string[]>("userAccountControl", new string[]{ "4098" });
            //If the account is created with UF_ACCOUNTDISABLE set in userAccountControl, unicodePwd is not required.
            //attrs[5] = new KeyValuePair<string, string[]>("unicodePwd", new string[] { "Password01!" });

            AdtsAddRequestPacket addRequest = ldapClientStack.CreateAddRequest(computerObjectDN, attrs);
            ldapClientStack.SendPacket(addRequest);
            response = ldapClientStack.ExpectPacket(timeout);
            AdtsAddResponsePacket addResponse = (AdtsAddResponsePacket)response;
            string ldapErrorCode = Enum.GetName(typeof(ResultCode), ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.AddResponse)
                     addResponse.GetInnerRequestOrResponse()).resultCode.Value).ToString();
            //BaseTestSite.Assert.AreEqual<string>(
            //         "some error code",
            //         ldapErrorCode,
            //         @"");
            #endregion

            #region Unbind and Disconnect

            AdtsUnbindRequestPacket unbindRequest = ldapClientStack.CreateUnbindRequest();
            ldapClientStack.SendPacket(unbindRequest);
            ldapClientStack.Disconnect();
            ldapClientStack = null;

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Add_Processing_Specifics_SystemFlags()
        {
            #region variables

            string siteObjDN = "CN=testSite,CN=Sites,CN=Configuration," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string serversContainerObjDN = "CN=testServers," + siteObjDN;
            string serverObjDN = "CN=testServer," + serversContainerObjDN;
            string ntdsSettingsObjDN = "CN=NTDS Settings," + serverObjDN;
            string nTDSConnection = "CN=testnTDSConnection," + ntdsSettingsObjDN;
            string ipObjDN = "CN=IP,CN=Inter-Site Transports,CN=Sites,CN=Configuration," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string siteLinkObjDN = "CN=testSiteLink," + ipObjDN;
            string siteLinkBridgeDN = "CN=testSiteLinkBridge," + ipObjDN;
            string subnetContainerObjDN = "CN=Subnets,CN=Sites,CN=Configuration," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            string subnetObjDN = "CN=192.168.0.0/24," + subnetContainerObjDN;
            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCIPAddress),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Site Object
            ManagedAddRequest addReq = new ManagedAddRequest(siteObjDN, "site");
            System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                addRep.ResultCode,
                @"Add Site: {0} should succeed.",
                siteObjDN);
            System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                siteObjDN,
                "(objectClass=Site)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "systemFlags");
            System.DirectoryServices.Protocols.SearchResponse searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            DirectoryAttribute attr = searchRep.Entries[0].Attributes["systemFlags"];
            object[] values = attr.GetValues(Type.GetType("System.String"));
            int flags = Convert.ToInt32(values[0], CultureInfo.InvariantCulture);
            BaseTestSite.Assert.AreEqual(
                SystemFlags.FLAG_DISALLOW_MOVE_ON_DELETE | SystemFlags.FLAG_CONFIG_ALLOW_RENAME,
                (SystemFlags)flags & (SystemFlags.FLAG_DISALLOW_MOVE_ON_DELETE | SystemFlags.FLAG_CONFIG_ALLOW_RENAME),
                @"The DC sets additional bits in the systemFlags value of the object created:
                site object: FLAG_DISALLOW_MOVE_ON_DELETE and FLAG_CONFIG_ALLOW_RENAME.");
            #endregion

            #region ServersContainer Object
            addReq = new ManagedAddRequest(serversContainerObjDN, "serversContainer");
            addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                addRep.ResultCode,
                @"Add ServersContainer: {0} should succeed.",
                serversContainerObjDN);
            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                serversContainerObjDN,
                "(objectClass=serversContainer)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "systemFlags");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["systemFlags"];
            values = attr.GetValues(Type.GetType("System.String"));
            flags = Convert.ToInt32(values[0], CultureInfo.InvariantCulture);
            BaseTestSite.Assert.AreEqual(
                SystemFlags.FLAG_DISALLOW_MOVE_ON_DELETE,
                (SystemFlags)flags & SystemFlags.FLAG_DISALLOW_MOVE_ON_DELETE,
                @"The DC sets additional bits in the systemFlags value of the object created:
                serversContainer object: FLAG_DISALLOW_MOVE_ON_DELETE.");
            #endregion

            #region Server Object
            addReq = new ManagedAddRequest(serverObjDN, "server");
            addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                addRep.ResultCode,
                @"Add server: {0} should succeed.",
                serverObjDN);
            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                serverObjDN,
                "(objectClass=server)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "systemFlags");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["systemFlags"];
            values = attr.GetValues(Type.GetType("System.String"));
            flags = Convert.ToInt32(values[0], CultureInfo.InvariantCulture);
            BaseTestSite.Assert.AreEqual(
                SystemFlags.FLAG_DISALLOW_MOVE_ON_DELETE | SystemFlags.FLAG_CONFIG_ALLOW_RENAME | SystemFlags.FLAG_CONFIG_ALLOW_LIMITED_MOVE,
                (SystemFlags)flags & (SystemFlags.FLAG_DISALLOW_MOVE_ON_DELETE | SystemFlags.FLAG_CONFIG_ALLOW_RENAME | SystemFlags.FLAG_CONFIG_ALLOW_LIMITED_MOVE),
                @"The DC sets additional bits in the systemFlags value of the object created:
                server object: FLAG_DISALLOW_MOVE_ON_DELETE, FLAG_CONFIG_ALLOW_RENAME, and FLAG_CONFIG_ALLOW_LIMITED_MOVE.");
            #endregion

            #region nTDSDSA Object
            System.DirectoryServices.Protocols.ModifyRequest modReq = new System.DirectoryServices.Protocols.ModifyRequest("",
                 DirectoryAttributeOperation.Add, "schemaupgradeinprogress", "1");
            System.DirectoryServices.Protocols.ModifyResponse modRep = (System.DirectoryServices.Protocols.ModifyResponse)con.SendRequest(modReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, modRep.ResultCode, "Should return success when set SchemaUpgradeInProgress to 1");
            addReq = new ManagedAddRequest(ntdsSettingsObjDN, "nTDSDSA");
            addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                addRep.ResultCode,
                @"Add nTDSDSA: {0} should succeed.",
                ntdsSettingsObjDN);
            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                ntdsSettingsObjDN,
                "(objectClass=nTDSDSA)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "systemFlags");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["systemFlags"];
            values = attr.GetValues(Type.GetType("System.String"));
            flags = Convert.ToInt32(values[0], CultureInfo.InvariantCulture);
            BaseTestSite.Assert.AreEqual(
                SystemFlags.FLAG_DISALLOW_MOVE_ON_DELETE,
                (SystemFlags)flags & (SystemFlags.FLAG_DISALLOW_MOVE_ON_DELETE),
                @"The DC sets additional bits in the systemFlags value of the object created:
                nTDSDSA object: FLAG_DISALLOW_MOVE_ON_DELETE.");
            #endregion

            #region nTDSConnection Object
            addReq = new ManagedAddRequest(nTDSConnection, "nTDSConnection");
            addReq.Attributes.Add(new DirectoryAttribute("options", "1"));
            addReq.Attributes.Add(new DirectoryAttribute("fromServer", ntdsSettingsObjDN));
            addReq.Attributes.Add(new DirectoryAttribute("enabledConnection", "TRUE"));
            addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);

            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                addRep.ResultCode,
                @"Add nTDSConnection: {0} should succeed.",
                nTDSConnection);
            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                nTDSConnection,
                "(objectClass=nTDSConnection)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "systemFlags");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["systemFlags"];
            values = attr.GetValues(Type.GetType("System.String"));
            flags = Convert.ToInt32(values[0], CultureInfo.InvariantCulture);
            BaseTestSite.Assert.AreEqual(
                SystemFlags.FLAG_CONFIG_ALLOW_RENAME,
                (SystemFlags)flags & (SystemFlags.FLAG_CONFIG_ALLOW_RENAME),
                @"The DC sets additional bits in the systemFlags value of the object created:
                nTDSConnection object: FLAG_CONFIG_ALLOW_RENAME.");
            #endregion

            #region SiteLink Object
            addReq = new ManagedAddRequest(siteLinkObjDN, "siteLink");

            addReq.Attributes.Add(new DirectoryAttribute("siteList", siteObjDN));
            addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                addRep.ResultCode,
                @"Add SiteLink: {0} should succeed.",
                siteLinkObjDN);
            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                siteLinkObjDN,
                "(objectClass=SiteLink)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "systemFlags");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["systemFlags"];
            values = attr.GetValues(Type.GetType("System.String"));
            flags = Convert.ToInt32(values[0], CultureInfo.InvariantCulture);
            BaseTestSite.Assert.AreEqual(
                SystemFlags.FLAG_CONFIG_ALLOW_RENAME,
                (SystemFlags)flags & SystemFlags.FLAG_CONFIG_ALLOW_RENAME,
                @"The DC sets additional bits in the systemFlags value of the object created:
                siteLink object: FLAG_CONFIG_ALLOW_RENAME.");
            #endregion

            #region SiteLinkBridge Object
            addReq = new ManagedAddRequest(siteLinkBridgeDN, "siteLinkBridge");
            addReq.Attributes.Add(new DirectoryAttribute("siteLinkList", siteLinkObjDN));
            addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                addRep.ResultCode,
                @"Add SiteLinkBridge: {0} should succeed.",
                siteLinkBridgeDN);
            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                siteLinkBridgeDN,
                "(objectClass=SiteLinkBridge)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "systemFlags");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["systemFlags"];
            values = attr.GetValues(Type.GetType("System.String"));
            flags = Convert.ToInt32(values[0], CultureInfo.InvariantCulture);
            BaseTestSite.Assert.AreEqual(
                SystemFlags.FLAG_CONFIG_ALLOW_RENAME,
                (SystemFlags)flags & SystemFlags.FLAG_CONFIG_ALLOW_RENAME,
                @"The DC sets additional bits in the systemFlags value of the object created:
                siteLinkBridge object: FLAG_CONFIG_ALLOW_RENAME.");
            #endregion

            #region not above Object with Subnets Container Parent
            addReq = new ManagedAddRequest(subnetObjDN, "subnet");
            addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                addRep.ResultCode,
                @"Add subnet: {0} should succeed.",
                subnetObjDN);
            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                subnetObjDN,
                "(objectClass=Subnet)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                "systemFlags");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["systemFlags"];
            values = attr.GetValues(Type.GetType("System.String"));
            flags = Convert.ToInt32(values[0], CultureInfo.InvariantCulture);
            BaseTestSite.Assert.AreEqual(
                SystemFlags.FLAG_CONFIG_ALLOW_RENAME,
                (SystemFlags)flags & SystemFlags.FLAG_CONFIG_ALLOW_RENAME,
                @"The DC sets additional bits in the systemFlags value of the object created:
                subnet object: FLAG_CONFIG_ALLOW_RENAME.");
            #endregion

            #region not above Object with Sites Container Parent except the Subnets Container and the Inter-Site-Transports Container
            #endregion

            #region clean up

            System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(siteObjDN);
            delReq.Controls.Add(new TreeDeleteControl());
            System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            delReq = new System.DirectoryServices.Protocols.DeleteRequest(siteLinkObjDN);
            delReq.Controls.Add(new TreeDeleteControl());
            delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            delReq = new System.DirectoryServices.Protocols.DeleteRequest(siteLinkBridgeDN);
            delReq.Controls.Add(new TreeDeleteControl());
            delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            delReq = new System.DirectoryServices.Protocols.DeleteRequest(subnetObjDN);
            delReq.Controls.Add(new TreeDeleteControl());
            delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);

            #endregion
        }
    }
}

