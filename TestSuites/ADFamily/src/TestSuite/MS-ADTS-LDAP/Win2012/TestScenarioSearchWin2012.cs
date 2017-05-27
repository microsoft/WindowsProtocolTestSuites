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
    public class TestScenarioSearchWin2012 : TestClassBase
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
            Utilities.TargetServerFqdn = AD_LDAPModelAdapter.Instance(Site).PDCNetbiosName + "." + AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName + ":" + AD_LDAPModelAdapter.Instance(Site).ADDSPortNum + "/";
        }

        /// <summary>
        /// Test clean up
        /// </summary>
        protected override void TestCleanup()
        {
            string addr = AD_LDAPModelAdapter.Instance(Site).PDCIPAddress;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr),
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
        public void LDAP_Search_SearchFilters()
        {
            #region variables

            string testUser = "testDynUser";
            string testUserDN = "CN=" + testUser + ",CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
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

            #region Add a dynamic object for search testing

            if (Utilities.IsObjectExist(testUserDN, addr, port))
            {
                System.DirectoryServices.Protocols.DeleteRequest req = new System.DirectoryServices.Protocols.DeleteRequest(testUserDN);
                System.DirectoryServices.Protocols.DeleteResponse rep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(req);
            }
            ManagedAddRequest addReq = new ManagedAddRequest(testUserDN);
            addReq.Attributes.Add(new DirectoryAttribute("objectClass", new string[] { "dynamicObject", "user" }));
            addReq.Attributes.Add(new DirectoryAttribute("entryTTL", "1800"));
            addReq.Attributes.Add(new DirectoryAttribute("sAMAccountName", testUser));
            System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);

            #endregion

            #region search filter
            //entryTTL is a constructed attribute
            System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                testUserDN,
                "(entryTTL=*)",
                System.DirectoryServices.Protocols.SearchScope.Subtree);
            try
            {
                System.DirectoryServices.Protocols.SearchResponse searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.InappropriateMatching)
                {
                    errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_FILTER_USES_CONTRUCTED_ATTRS) failed = true;
                }
            }
            BaseTestSite.Assert.IsTrue(
                failed,
                @"Active Directory does not support constructed attributes (defined in section 3.1.1.4.5) in search filters.
                When a search operation is performed with such a search filter, Active Directory fails with inappropriateMatching
                ([RFC2251] section 4.1.10).");

            #endregion

            #region clean up
            if (Utilities.IsObjectExist(testUserDN, addr, port))
            {
                System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(testUserDN);
                System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            }
            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Search_SD_Flags_Control()
        {
            #region variables

            string testUser = "testUser";
            string testUserDN = "CN=" + testUser + ",CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            bool isExist = false;

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

            #region Add an object for search testing

            if (!Utilities.IsObjectExist(testUserDN, addr, port))
            {
                ManagedAddRequest addReq = new ManagedAddRequest(testUserDN, "user");
                System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }

            #endregion

            #region with SD FLags Control

            System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                    testUserDN,
                    "(objectClass=user)",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "ntSecurityDescriptor");
            System.DirectoryServices.Protocols.SecurityDescriptorFlagControl sdFlagsCtrl = new System.DirectoryServices.Protocols.SecurityDescriptorFlagControl(
                System.DirectoryServices.Protocols.SecurityMasks.Owner);
            searchReq.Controls.Add(sdFlagsCtrl);
            System.DirectoryServices.Protocols.SearchResponse searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            DirectoryAttribute attr = searchRep.Entries[0].Attributes["ntSecurityDescriptor"];
            object[] values = attr.GetValues(Type.GetType("System.String"));
            if (values != null) isExist = true;
            BaseTestSite.Assert.IsTrue(
                isExist,
                @"If the LDAP_SERVER_SD_FLAGS_OID control is present in an LDAP search request, the server returns an SD with the parts specified in the control
                when the SD attribute name is explicitly mentioned in the requested attribute list.");

            isExist = false;
            searchReq.Attributes.Clear();
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["ntSecurityDescriptor"];
            values = attr.GetValues(Type.GetType("System.String"));
            if (values != null) isExist = true;
            BaseTestSite.Assert.IsTrue(
                isExist,
                @"If the LDAP_SERVER_SD_FLAGS_OID control is present in an LDAP search request, the server returns an SD with the parts specified in the control
                when the requested attribute list is empty.");

            isExist = false;
            searchReq.Attributes.Add("*");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["ntSecurityDescriptor"];
            values = attr.GetValues(Type.GetType("System.String"));
            if (values != null) isExist = true;
            BaseTestSite.Assert.IsTrue(
                isExist,
                @"If the LDAP_SERVER_SD_FLAGS_OID control is present in an LDAP search request, the server returns an SD with the parts specified in the control
                when all attributes are requested.");

            #endregion

            #region without SD Flags Control

            isExist = false;
            searchReq.Controls.Clear();
            searchReq.Attributes.Clear();
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            attr = searchRep.Entries[0].Attributes["ntSecurityDescriptor"];
            if (attr == null)
            {
                isExist = false;
            }
            else
            {
                isExist = true;
            }
            BaseTestSite.Assert.IsFalse(
                isExist,
                @"Without the presence of this control, the server returns an SD only when the SD attribute name is explicitly mentioned in the requested attribute list.");

            searchReq.Attributes.Add("ntSecurityDescriptor");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            BaseTestSite.Assert.AreEqual(
                1,
                searchRep.Entries[0].Attributes.Count,
                @"Without the presence of this control, the server returns an SD only when the SD attribute name is explicitly mentioned in the requested attribute list.");
            attr = searchRep.Entries[0].Attributes["ntSecurityDescriptor"];
            values = attr.GetValues(Type.GetType("System.String"));
            if (values != null) isExist = true;
            BaseTestSite.Assert.IsTrue(
                isExist,
                @"Without the presence of this control, the server returns an SD only when the SD attribute name is explicitly mentioned in the requested attribute list.");

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("RODC")]
        [TestCategory("DomainWin2012")]
		[TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Search_ConstructedAttributes_isUserCachableAtRodc()
        {
            if (string.IsNullOrWhiteSpace(AD_LDAPModelAdapter.Instance(Site).RODCNetbiosName))
                BaseTestSite.Assert.Fail("Test case requires a RODC but \"RODCName\" ptfconfig property value is invalid");

            #region variables

            string RODCName = AD_LDAPModelAdapter.Instance(Site).RODCNetbiosName;
            string RODCDN = "CN=" + RODCName + ",OU=Domain Controllers," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            //Let D be the DN of the user principal specified using LDAP Control LDAP_SERVER_DN_INPUT_OID. 
            //If the DN of a security principal is not explicitly specified, D is the DN of the current requester.
            string userName = AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName;
            string userDN = "CN=" + userName + ",CN=Users," + AD_LDAPModelAdapter.Instance(Site).rootDomainNC;
            bool isCachable = false;

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

            #region search with LDAP_SERVER_DN_INPUT_OID

            System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                    RODCDN,
                    "(objectClass=computer)",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "msDS-isUserCachableAtRodc");
            //Let D be the DN of the user principal specified using LDAP Control LDAP_SERVER_DN_INPUT_OID. 
            //If the DN of a security principal is not explicitly specified, D is the DN of the current requester.
            System.DirectoryServices.Protocols.SearchResponse searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            DirectoryAttribute attr = searchRep.Entries[0].Attributes["msDS-isUserCachableAtRodc"];
            object[] values = attr.GetValues(Type.GetType("System.String"));
            isCachable = Convert.ToBoolean(Convert.ToInt16(values[0].ToString(), CultureInfo.InvariantCulture));

            //Get expected result by GetRevealSecretsPolicyForUser(TO!distinguishedName, D) defined in MS-DRSR section 4.1.10.5.14
            bool expectedCachable = GetRevealSecretsPolicyForUser(RODCDN, userDN);

            BaseTestSite.Assert.AreEqual(
                expectedCachable,
                isCachable,
                @"TO!msDS-IsUserCachableAtRodc = GetRevealSecretsPolicyForUser(TO!distinguishedName, D) (procedure GetRevealSecretsPolicyForUser is defined in [MS-DRSR] section 4.1.10.5.14).");

            #endregion

        }

        private bool GetRevealSecretsPolicyForUser(string rodcObjDN, string userObjDN)
        {
            #region variables

            System.DirectoryServices.Protocols.SearchResultEntry rodcEntry;
            System.DirectoryServices.Protocols.SearchResultEntry userEntry;

            #endregion

            #region connect and get rodc and user object

            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(AD_LDAPModelAdapter.Instance(Site).PDCNetbiosName),
              new NetworkCredential(AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                  AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                  AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;
            System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                    rodcObjDN,
                    "(objectClass=computer)",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "msDS-KrbTgtLink",
                    "msDS-NeverRevealGroup",
                    "msDS-RevealOnDemandGroup");
            System.DirectoryServices.Protocols.SearchResponse searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            rodcEntry = searchRep.Entries[0];
            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                    userObjDN,
                    "(objectClass=user)",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "userAccountControl",
                    "objectSid");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            userEntry = searchRep.Entries[0];

            #endregion

            #region body
            //An RODC can always cache secrets of its own account
            if (rodcObjDN == userObjDN) return true;

            //An RODC can always cache secrets of its own secondary Kerberos TGT account
            //But not other secondary Kerberos TGT accounts.
            DirectoryAttribute attr = rodcEntry.Attributes["msDS-KrbTgtLink"];
            object[] values = attr.GetValues(Type.GetType("System.String"));
            foreach (string value in values)
            {
                if (value.Equals(userObjDN, StringComparison.CurrentCultureIgnoreCase)) return true;
            }
            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                    AD_LDAPModelAdapter.Instance(Site).rootDomainNC,
                    "(objectClass=computer)",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "msDS-KrbTgtLink");
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            foreach (System.DirectoryServices.Protocols.SearchResultEntry entry in searchRep.Entries)
            {
                if (entry.Attributes["msDS-KrbTgtLink"] != null)
                {
                    values = entry.Attributes["msDS-KrbTgtLink"].GetValues(Type.GetType("System.String"));
                    foreach (string value in values)
                    {
                        if (value.Equals(userObjDN, StringComparison.CurrentCultureIgnoreCase)) return false;
                    }
                }
            }

            //Never reveal secrets of inter-domain trust accounts
            attr = userEntry.Attributes["userAccountControl"];
            values = attr.GetValues(Type.GetType("System.String"));
            foreach (string value in values)
            {
                int userAccountControl = int.Parse(value, CultureInfo.InvariantCulture);
                if (((AdtsUserAccountControl)userAccountControl & AdtsUserAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT) != 0) return false;
            }

            //Never reveal secrets of users reachable from rodcObj!msDS-NeverRevealGroup
            attr = rodcEntry.Attributes["msDS-NeverRevealGroup"];
            values = attr.GetValues(Type.GetType("System.String"));
            foreach (string groupDN in values)
            {
                if (Utilities.IsGroupMember(
                    AD_LDAPModelAdapter.Instance(Site).PDCNetbiosName,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName,
                    AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    groupDN,
                    userObjDN))
                {
                    return false;
                }
            }

            //Cache secrets of users reachable from rodcObj!msDS-RevealOnDemandGroup
            attr = rodcEntry.Attributes["msDS-RevealOnDemandGroup"];
            values = attr.GetValues(Type.GetType("System.String"));
            foreach (string groupDN in values)
            {
                if (Utilities.IsGroupMember(
                    AD_LDAPModelAdapter.Instance(Site).PDCNetbiosName,
                    AD_LDAPModelAdapter.Instance(Site).PrimaryDomainDnsName,
                    AD_LDAPModelAdapter.Instance(Site).DomainAdministratorName,
                    AD_LDAPModelAdapter.Instance(Site).DomainUserPassword,
                    groupDN,
                    userObjDN))
                {
                    return true;
                }
            }

            return false;

            #endregion
        }
    }
}