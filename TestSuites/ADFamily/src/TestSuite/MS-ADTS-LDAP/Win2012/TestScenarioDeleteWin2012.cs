// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Net;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    /// <summary>
    /// MS-ADTS-LDAP traditional testcase
    /// </summary>
    [TestClass]
    public class TestScenarioDeleteWin2012 : TestClassBase
    {
        #region Variables

        /// <summary>
        /// AD_LDAPModelAdapter instance
        /// </summary>
        public static AD_LDAPModelAdapter adLdapModelAdapter = null;

        /// <summary>
        /// Temporary creation of Site Variable
        /// </summary>
        public static ITestSite TestScenarioDeleteWin2012TestSite = null;

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

            // Initializing the ITestSite object
            if (null == TestScenarioDeleteWin2012TestSite)
            {
                TestScenarioDeleteWin2012TestSite = TestClassBase.BaseTestSite;
                TestScenarioDeleteWin2012TestSite.DefaultProtocolDocShortName = "MS-ADTS-LDAP";
            }
            adLdapModelAdapter = AD_LDAPModelAdapter.Instance(TestScenarioDeleteWin2012TestSite);
            adLdapModelAdapter.Initialize();
        }

        /// <summary>
        /// Class cleanup
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            adLdapModelAdapter.Reset();

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
        }

        /// <summary>
        /// Test clean up
        /// </summary>
        protected override void TestCleanup()
        {
            string addr = adLdapModelAdapter.PDCIPAddress;
            string port = adLdapModelAdapter.ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(adLdapModelAdapter.DomainAdministratorName,
                    adLdapModelAdapter.DomainUserPassword,
                    adLdapModelAdapter.PrimaryDomainDnsName));
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
        public void LDAP_Delete_Constraints_nTDSDSA()
        {
            #region variables

            bool failed = false;
            int errorCode;
            string serverDN = "cn=" + adLdapModelAdapter.PDCNetbiosName + ",cn=servers,cn=default-first-site-name,cn=sites,cn=configuration," + adLdapModelAdapter.rootDomainNC;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(adLdapModelAdapter.PDCIPAddress),
              new NetworkCredential(adLdapModelAdapter.DomainAdministratorName,
                  adLdapModelAdapter.DomainUserPassword,
                  adLdapModelAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Delete constraint for class nTDSDSA

            System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(serverDN);
            delReq.Controls.Add(new TreeDeleteControl());
            System.DirectoryServices.Protocols.DeleteResponse delRep = null;
            try
            {
                delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.UnwillingToPerform)
                {
                    errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_CANT_DELETE_DSA_OBJ) failed = true;
                }
            }

            BaseTestSite.Assert.IsTrue(
                failed,
                @"If the object being deleted is the DC's nTDSDSA object or any of its ancestors, 
                unwillingToPerform / ERROR_DS_CANT_DELETE_DSA_OBJ is returned.");

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Delete_Constraints_CrossRef_CantOnNonLeaf()
        {
            #region variables

            int errorCode;
            bool failed = false;
            string fullDomainName = adLdapModelAdapter.PrimaryDomainDnsName;
            string netBiosDomainName = adLdapModelAdapter.PrimaryDomainNetBiosName;

            // This "default domain NC" CrossRef object has nCName attribute DN1 which is ancestor of
            // "config NC" CrossRef object's nCName attribute DN2, DN2 is ancestor of
            // "schema NC" CrossRef object's nCName attribute DN3, which is actually the leaf node
            // Ancestor relationship: DN1 -> DN2 -> DN3
            string[] crossRefDN = new string[3];
            crossRefDN[0] = "cn=" + netBiosDomainName + ",cn=partitions,cn=configuration," + adLdapModelAdapter.rootDomainNC;
            crossRefDN[1] = "cn=enterprise configuration,cn=partitions,cn=configuration," + adLdapModelAdapter.rootDomainNC;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(adLdapModelAdapter.PDCIPAddress),
              new NetworkCredential(adLdapModelAdapter.DomainAdministratorName,
                  adLdapModelAdapter.DomainUserPassword,
                  adLdapModelAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region crossRef

            for (int i = 0; i < 2; i++)
            {
                System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(crossRefDN[i]);
                delReq.Controls.Add(new TreeDeleteControl());
                System.DirectoryServices.Protocols.DeleteResponse delRep = null;
                try
                {
                    delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
                }
                catch (DirectoryOperationException e)
                {
                    if (e.Response.ResultCode == ResultCode.NotAllowedOnNonLeaf)
                    {
                        errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                        if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_CANT_ON_NON_LEAF) failed = true;
                    }
                }
                BaseTestSite.Assert.IsTrue(
                    failed,
                    @"If the crossRef object is a child of the CN=Partitions child of the config NC and the nCName attribute of 
                    the crossRef object is set to the value DN1 and there exists another crossRef object with the same parent where
                    the nCName attribute of the second crossRef object is set to the value DN2, and the object referred to by DN1 
                    is an ancestor of the object referred to by DN2, then notAllowedOnNonLeaf / ERROR_DS_CANT_ON_NON_LEAF is returned.");
                failed = false;
            }

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Delete_Constraints_CrossRef_NCStillHasDsas()
        {
            #region variables

            int errorCode;
            bool failed = false;
            string fullDomainName = adLdapModelAdapter.PrimaryDomainDnsName;
            string netBiosDomainName = adLdapModelAdapter.PrimaryDomainNetBiosName;

            // This "default domain NC" CrossRef object has nCName attribute DN1 which is ancestor of
            // "config NC" CrossRef object's nCName attribute DN2, DN2 is ancestor of
            // "schema NC" CrossRef object's nCName attribute DN3, which is actually the leaf node
            // Ancestor relationship: DN1 -> DN2 -> DN3
            // "schema NC" is hosted by this connnected domain controller
            string[] crossRefDN = new string[1];
            crossRefDN[0] = "cn=enterprise schema,cn=partitions,cn=configuration," + adLdapModelAdapter.rootDomainNC;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(adLdapModelAdapter.PDCIPAddress),
              new NetworkCredential(adLdapModelAdapter.DomainAdministratorName,
                  adLdapModelAdapter.DomainUserPassword,
                  adLdapModelAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region crossRef

            for (int i = 0; i < 1; i++)
            {
                System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(crossRefDN[i]);
                delReq.Controls.Add(new TreeDeleteControl());
                System.DirectoryServices.Protocols.DeleteResponse delRep = null;
                try
                {
                    delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
                }
                catch (DirectoryOperationException e)
                {
                    if (e.Response.ResultCode == ResultCode.UnwillingToPerform)
                    {
                        errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                        if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_NC_STILL_HAS_DSAS) failed = true;
                    }
                }
                BaseTestSite.Assert.IsTrue(
                    failed,
                    @"Else if the crossRef object is a child of the CN=Partitions child of the config NC, 
                    and the crossRef object’s NC is hosted by some domain controller, unwillingToPerform / ERROR_DS_NC_STILL_HAS_DSAS
                    is returned.");
                failed = false;
            }

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Delete_Constraints_Protected()
        {
            #region variables

            int errorCode;
            bool failed = false;
            string protectedObjDN = "cn=RID Set,cn=" + adLdapModelAdapter.PDCNetbiosName + ",ou=domain controllers," + adLdapModelAdapter.rootDomainNC;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(adLdapModelAdapter.PDCNetbiosName),
              new NetworkCredential(adLdapModelAdapter.DomainAdministratorName,
                  adLdapModelAdapter.DomainUserPassword,
                  adLdapModelAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region delete protected

            System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(protectedObjDN);
            delReq.Controls.Add(new TreeDeleteControl());
            System.DirectoryServices.Protocols.DeleteResponse delRep = null;
            try
            {
                delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
            }
            catch (DirectoryOperationException e)
            {
                if (e.Response.ResultCode == ResultCode.UnwillingToPerform)
                {
                    errorCode = int.Parse(e.Response.ErrorMessage.Split(':')[0], System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    if ((Win32ErrorCode_32)errorCode == Win32ErrorCode_32.ERROR_DS_CANT_DELETE) failed = true;
                }
            }
            BaseTestSite.Assert.IsTrue(
                failed,
                @"If the object being deleted is protected (see section 3.1.1.5.5.3, Protected Objects) and does not fall into
                the two categories above, unwillingToPerform / ERROR_DS_CANT_DELETE is returned.");
            failed = false;

            #endregion
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Delete_DynamicObject_Requirements()
        {
            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");

            #region Local variables

            string testUser = "testDynUser";
            string testUserDN = "CN=" + testUser + ",CN=Users," + adLdapModelAdapter.rootDomainNC;
            string testUserGuid = null;
            bool isExist = false;
            System.DirectoryServices.Protocols.DeleteRequest delReq;
            System.DirectoryServices.Protocols.DeleteResponse delRep;

            #endregion

            using (LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(adLdapModelAdapter.PDCIPAddress),
                new NetworkCredential(
                    adLdapModelAdapter.DomainAdministratorName,
                    adLdapModelAdapter.DomainUserPassword,
                    adLdapModelAdapter.PrimaryDomainDnsName)))
            {
                con.SessionOptions.Sealing = false;
                con.SessionOptions.Signing = false;

                #region Delete the dynamic object to initialize the test environment

                try
                {
                    delReq = new System.DirectoryServices.Protocols.DeleteRequest(testUserDN);
                    delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
                    BaseTestSite.Assert.AreEqual(ResultCode.Success, delRep.ResultCode, "[Initialize] Deleting the dynamic object should success.");
                }
                catch (Exception e)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "[Initialize] Deleting the dynamic object failed: {0}", e.Message);
                }

                #endregion

                #region Add the dynamic object to be deleted

                ManagedAddRequest addReq = new ManagedAddRequest(testUserDN);
                addReq.Attributes.Add(new DirectoryAttribute("objectClass", new string[] { "dynamicObject", "user" }));
                addReq.Attributes.Add(new DirectoryAttribute("entryTTL", "1800"));
                addReq.Attributes.Add(new DirectoryAttribute("sAMAccountName", testUser));
                System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
                BaseTestSite.Assert.AreEqual(ResultCode.Success, addRep.ResultCode, "Adding the dynamic object should success.");

                System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                    testUserDN,
                    "(objectClass=user)",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    new string[] { "objectClass" });
                System.DirectoryServices.Protocols.SearchResponse searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
                DirectoryAttribute attr = searchRep.Entries[0].Attributes["objectClass"];
                object[] values = attr.GetValues(Type.GetType("System.String"));
                foreach (string value in values)
                {
                    if (value.Contains("dynamicObject")) isExist = true;
                }
                BaseTestSite.Assert.IsTrue(isExist, "Dynamic entry should be created successfully.");

                testUserGuid = Utilities.GetUserGuid(
                    adLdapModelAdapter.PDCNetbiosName,
                    adLdapModelAdapter.PrimaryDomainDnsName,
                    adLdapModelAdapter.ADDSPortNum,
                    adLdapModelAdapter.DomainAdministratorName,
                    adLdapModelAdapter.DomainUserPassword,
                    testUser);

                #endregion

                #region delete the dynamic object and verify requirements

                delReq = new System.DirectoryServices.Protocols.DeleteRequest(testUserDN);
                delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);
                BaseTestSite.Assert.AreEqual(ResultCode.Success, delRep.ResultCode, "Deleting the dynamic object should success.");
                SearchResult tombStoneResult = Utilities.GetDeletedObject(testUserGuid, adLdapModelAdapter.defaultNC, adLdapModelAdapter.currentWorkingDC.FQDN, adLdapModelAdapter.currentPort);
                if (tombStoneResult == null) isExist = false;
                if (adLdapModelAdapter.isTDI72765fixed)
                {
                    BaseTestSite.Assert.IsFalse(
                        isExist,
                        @"Dynamic objects are objects that are automatically deleted after a period of time. When they are deleted (automatically or manually),
                    they do not transform into any other state, such as a tombstone, deleted-object, or recycled-object. ");
                }

                #endregion
            }
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Delete_Truncate_RDN()
        {
            //This case is implemented to test the RDN size limitations on a deleted object
            //MS-ADTS section 3.1.5.5:
            //If O is the object that is deleted, the delete-mangled RDN is the concatenation of O!name,
            //the character with value 0x0A, the string "DEL:", and the dashed string representation ([RFC4122] section 3)
            //of O!objectGUID. During this concatenation, if required, the O!name part is truncated to ensure that the 
            //length of the delete-mangled RDN does not violate the RDN size constraint in section 3.1.1.5.1.2.
            //MS-ADTS section 3.1.1.5.1.2:
            //The RDN must not contain a character with value 0xA
            //The RDN must not contain a charater with value 0x0
            //The RDN size must be less than 255 characters

            //However, When trying to form a RDN that will exceed the size limitation when deleted:
            //MS-ADA1 section 2.110: Attribute cn has rangeUpper: 64
            //That said, the length of a delete-mangled RDN can only be up to 107 characters (not including the terminating NUL character): 
            //{delete-mangled RDN: length} = {rangeUpper:64} + {'\\' :2} + {0x0A :1} + {'DEL:':4} + {dashed-string-Guid:36} = 107 < 255.
            //Therefore, the delete-managled RDN can never be larger than 255 characters.

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(adLdapModelAdapter.PDCIPAddress),
              new NetworkCredential(adLdapModelAdapter.DomainAdministratorName,
                  adLdapModelAdapter.DomainUserPassword,
                  adLdapModelAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #region variables
            const int maxRDNLen = 255;
            const int maxCNLen = 64;
            string delObjName = new string('a', maxCNLen);
            string delObjDN = "CN=" + delObjName + ",CN=Users," + adLdapModelAdapter.rootDomainNC;
            string tombStoneDN = null;
            string tombStoneRDN = null;

            #endregion

            #region Add the object for delete testing
            try
            {
                ManagedAddRequest addReq = new ManagedAddRequest(delObjDN, "user");
                System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            }
            catch { }
            #endregion

            System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(delObjDN);
            System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);

            DirectoryEntry deletedEntry = Utilities.BuildDeletedEntry(
                string.Format(CultureInfo.InvariantCulture, "{0}.{1}", adLdapModelAdapter.PDCNetbiosName, adLdapModelAdapter.PrimaryDomainDnsName),
                AD_LDAPModelAdapter.DELETED_OBJECTS_CONTAINER_GUID,
                adLdapModelAdapter.rootDomainNC);
            SearchResult tombStoneResult = Utilities.GetTombstone(deletedEntry, delObjDN.Split(',')[0].Split('=')[1].Trim());
            if (tombStoneResult != null)
            {
                foreach (string key in tombStoneResult.Properties.PropertyNames)
                {
                    foreach (object value in tombStoneResult.Properties[key])
                    {
                        if (key.ToLower(CultureInfo.InvariantCulture) == "distinguishedname")
                        {
                            tombStoneDN = value.ToString();
                        }
                    }
                }
            }
            tombStoneRDN = tombStoneDN.Split(',')[0].Split('=')[1].Trim();
            BaseTestSite.Assert.IsTrue(tombStoneRDN.Length <= maxRDNLen, "MS-ADTS section 3.1.1.5.1.2: The RDN size must be less than 255 characters");
        }

        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("Main")]
        public void LDAP_Delete_Tombstone_Requirements()
        {
            #region variables

            string testGroup = "testGroup";
            string testGroupDN = "CN=" + testGroup + ",CN=Users," + adLdapModelAdapter.rootDomainNC;
            string testUser = "testUser";
            string testUserDN = "CN=" + testUser + ",CN=Users," + adLdapModelAdapter.rootDomainNC;
            bool isExist = false;

            #endregion

            #region connect

            BaseTestSite.Assume.IsTrue(EnvironmentConfig.ServerVer >= ServerVersion.Win2012, "Server OS version should be not less than Windows Server 2012");
            string addr = adLdapModelAdapter.PDCIPAddress;
            string port = adLdapModelAdapter.ADDSPortNum;
            LdapConnection con = new LdapConnection(
                new LdapDirectoryIdentifier(addr, int.Parse(port)),
                new NetworkCredential(adLdapModelAdapter.DomainAdministratorName,
                  adLdapModelAdapter.DomainUserPassword,
                  adLdapModelAdapter.PrimaryDomainDnsName));
            con.SessionOptions.Sealing = false;
            con.SessionOptions.Signing = false;

            #endregion

            #region Add the object for delete testing

            try
            {
                System.DirectoryServices.Protocols.DeleteRequest req = new System.DirectoryServices.Protocols.DeleteRequest(testUserDN);
                System.DirectoryServices.Protocols.DeleteResponse rep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(req);
            }
            catch { }
            ManagedAddRequest addReq = new ManagedAddRequest(testUserDN, "user");
            System.DirectoryServices.Protocols.AddResponse addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);
            try
            {
                System.DirectoryServices.Protocols.DeleteRequest req = new System.DirectoryServices.Protocols.DeleteRequest(testGroupDN);
                System.DirectoryServices.Protocols.DeleteResponse rep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(req);
            }
            catch { }
            addReq = new ManagedAddRequest(testGroupDN, "group");
            addReq.Attributes.Add(new DirectoryAttribute("member", testUserDN));
            addReq.Attributes.Add(new DirectoryAttribute("description", testUserDN));
            addRep = (System.DirectoryServices.Protocols.AddResponse)con.SendRequest(addReq);

            System.DirectoryServices.Protocols.SearchRequest searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                testGroupDN,
                "(objectClass=group)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "member", "description" });
            System.DirectoryServices.Protocols.SearchResponse searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);
            DirectoryAttribute attr = searchRep.Entries[0].Attributes["member"];
            object[] values = attr.GetValues(Type.GetType("System.String"));
            foreach (string value in values)
            {
                if (value.Contains(testUserDN)) isExist = true;
            }
            BaseTestSite.Assert.IsTrue(isExist, @"Entry referencing the to-be-deleted entry should exist before deletion.");
            isExist = false;
            attr = searchRep.Entries[0].Attributes["description"];
            values = attr.GetValues(Type.GetType("System.String"));
            foreach (string value in values)
            {
                if (value.Contains(testUserDN)) isExist = true;
            }
            BaseTestSite.Assert.IsTrue(isExist, @"Entry referencing the to-be-deleted entry should exist before deletion.");

            #endregion

            #region check the deleted entry

            System.DirectoryServices.Protocols.DeleteRequest delReq = new System.DirectoryServices.Protocols.DeleteRequest(testUserDN);
            System.DirectoryServices.Protocols.DeleteResponse delRep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(delReq);

            DirectoryEntry deletedEntry = Utilities.BuildDeletedEntry(
                string.Format(CultureInfo.InvariantCulture, "{0}.{1}", adLdapModelAdapter.PDCNetbiosName, adLdapModelAdapter.PrimaryDomainDnsName),
                AD_LDAPModelAdapter.DELETED_OBJECTS_CONTAINER_GUID,
                adLdapModelAdapter.rootDomainNC);
            SearchResult tombStoneResult = Utilities.GetTombstone(deletedEntry, testUserDN.Split(',')[0].Split('=')[1].Trim());
            BaseTestSite.Assert.IsNotNull(tombStoneResult, "deleted entry: {0} should be found in AD.", deletedEntry);

            #region linked attributes in deleted entry

            isExist = false;
            foreach (string key in tombStoneResult.Properties.PropertyNames)
            {
                foreach (object value in tombStoneResult.Properties[key])
                {
                    if (key.ToLower(CultureInfo.InvariantCulture) == "objectCategory")
                    {
                        if (value != null) isExist = true;
                    }
                    if (key.ToLower(CultureInfo.InvariantCulture) == "sAMAccountType")
                    {
                        if (value != null) isExist = true;
                    }
                }
            }
            BaseTestSite.Assert.IsFalse(
                isExist,
                @"A tombstone does not retain the attribute values of the original object for the attributes objectCategory and sAMAccountType
                or for any linked attributes even if these attributes would otherwise be retained according to the preceding bullet point. 
                In other words, when an object is deleted and transformed into a tombstone, objectCategory values, sAMAccountType values, 
                and any linked attribute values on it are always removed.");

            #endregion

            #endregion

            #region check the entry referencing the deleted entry

            searchReq = new System.DirectoryServices.Protocols.SearchRequest(
                testGroupDN,
                "(objectClass=group)",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "member", "description" });
            searchRep = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(searchReq);

            #region linked attribute in referencing entry

            attr = searchRep.Entries[0].Attributes["member"];
            if (attr == null) isExist = false;
            BaseTestSite.Assert.IsFalse(
                isExist,
                @"NC replicas do not contain objects with linked attribute values referencing tombstones. 
                In other words, when an object is deleted and transformed into a tombstone, any linked attribute 
                values on other objects referencing it are also removed."
                );

            #endregion

            #region non linked attribute in referencing entry

            isExist = false;
            attr = searchRep.Entries[0].Attributes["description"];
            values = attr.GetValues(Type.GetType("System.String"));
            foreach (string value in values)
            {
                if (value.Contains(testUserDN)) isExist = true;
            }
            BaseTestSite.Assert.IsTrue(
                isExist,
                @"If any NC replicas contain other objects with nonlinked attribute values referencing a tombstone,
                then those attribute values on those objects are retained.  In other words, when an object is deleted
                and transformed into a tombstone, any nonlinked attribute values on other objects referencing it are not removed."
                );

            #endregion

            #endregion

            #region clean up

            if (Utilities.IsObjectExist(testUserDN, addr, port))
            {
                System.DirectoryServices.Protocols.DeleteRequest req = new System.DirectoryServices.Protocols.DeleteRequest(testUserDN);
                try
                {
                    System.DirectoryServices.Protocols.DeleteResponse rep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(req);
                }
                catch
                {
                }
            }
            if (Utilities.IsObjectExist(testGroupDN, addr, port))
            {
                System.DirectoryServices.Protocols.DeleteRequest req = new System.DirectoryServices.Protocols.DeleteRequest(testGroupDN);
                try
                {
                    System.DirectoryServices.Protocols.DeleteResponse rep = (System.DirectoryServices.Protocols.DeleteResponse)con.SendRequest(req);
                }
                catch
                {
                }
            }

            #endregion
        }
    }
}

