// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// Test class for Administration Tool Support methods.
    /// </summary>
    [TestClass]
    public class AdministrationToolSupportTest : DrsrTestClassBase
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
            DumpLevel = 1;
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
            DumpLevel = 0;
        }
        #endregion

        #region DRSADDEntry
        [BVT]
        [Priority(0)]
        [TestCategory("Win2003")]
        [ServerFSMORole(FSMORoles.DomainNaming)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling IDL_DRSAddEntry to create a crossRef object.")]
        public void DRSR_DRSAddEntry_V2_Success_Create_crossRef()
        {
            DrsrTestChecker.Check();
            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.DC_With_DomainNamingMasterRole;
                    DsServer dcServer = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
                    DRS_AddEntry_Versions reqVer = DRS_AddEntry_Versions.V2;
                    DRS_SecBufferDesc[] pClientCreds = null;
                    uint? outVersion;
                    DRS_MSG_ADDENTRYREPLY? outMessage;
                    const string newObjectPrefixName = DRSTestData.DRSAddEntry_V2_Create_crossRef_newObjectPrefixName; //The prefix name of cn of the new object.

                    //Prepare the attribute of the new crossRef object.
                    string partitionsDN = "CN=Partitions," + DrsrHelper.GetNamingContextDN(dcServer.Domain, NamingContext.ConfigNC);
                    string domainDn = LdapUtility.GetDnFromNcType(dcServer, NamingContext.DomainNC);
                    string cn = newObjectPrefixName;
                    string dn = "CN=" + cn + "," + partitionsDN;

                    bool fObjectExisted = ldapAdapter.IsObjectExist(dcServer, dn);
                    int prefix = 0;
                    while (fObjectExisted)
                    {
                        cn = newObjectPrefixName + prefix;
                        dn = "CN=" + cn + "," + partitionsDN;
                        fObjectExisted = ldapAdapter.IsObjectExist(dcServer, dn);
                        prefix++;
                    }

                    string cnName = "DC=" + cn + "," + domainDn;
                    string dnsRoot = cn + "." + dcServer.Domain.DNSName;

                    BaseTestSite.Log.Add(LogEntryKind.Debug, "IDL_DRSAddEntry: Creating crossRef object {0}", dn);
                    ENTINF entInfObj = ldapAdapter.ConstructNewCrossRefObject(dcServer, dn, cn, cnName, dnsRoot);
                    ENTINF[] entInfArray = new ENTINF[1];
                    entInfArray[0] = entInfObj;
                    ENTINFLIST entInfObjList = DrsrHelper.CreateENTINFLIST(entInfArray);

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSAddEntry: Binding to DC server: {0}", dcServerType);
                    uint ret = drsTestClient.DrsBind(dcServerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: {1}, should return 0 on success.", ret, 0);

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSAddEntry: Creating crossRef object {0} on {1}", dn, dcServerType);
                    ret = drsTestClient.DrsAddEntry(dcServerType, reqVer, entInfObjList, pClientCreds, out outVersion, out outMessage);
                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSAddEntry: Checking return value - got: {0}, expect: {1}, should return 0 on success", ret, 0);

                    BaseTestSite.Assert.AreEqual<uint>((uint)reqVer, (uint)outVersion, "IDL_DRSAddEntry: Checking outVersion - got: {0}, expect: {1}", (uint)outVersion, (uint)reqVer);
                    BaseTestSite.Assert.IsNull(outMessage.Value.V2.pErrorObject, "IDL_DRSAddEntry: Checking pErrorObject in reply - got: {0}, expect: null", outMessage.Value.V2.pErrorObject);
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.errCode, 0, "IDL_DRSAddEntry: Checking errCode in reply - got: {0}, expect: 0", outMessage.Value.V2.errCode);
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.extendedErr, 0, "IDL_DRSAddEntry: Checking extendedErr in reply - got: {0}, expect: 0", outMessage.Value.V2.extendedErr);
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.problem, 0, "IDL_DRSAddEntry: Checking problem in reply - got: {0}, expect: 0", outMessage.Value.V2.problem);
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.cObjectsAdded, 1, "IDL_DRSAddEntry: Checking cObjectsAdded in reply - got: {0}, expect: 1", outMessage.Value.V2.cObjectsAdded);
                    ADDENTRY_REPLY_INFO infolist = outMessage.Value.V2.infoList[0];
                    BaseTestSite.Assert.IsTrue(DrsrHelper.IsNullSid(infolist.objSid), "IDL_DRSAddEntry: Checking objSid in infoList in reply - should be null");

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSAddEntry: Checking the existence of object {0} on DC {1}", dn, dcServerType);
                    DSNAME newCRObjectName = ldapAdapter.GetDsName(dcServer, dn).Value;
                    BaseTestSite.Assert.AreNotEqual(Guid.Empty, newCRObjectName.Guid, "IDL_DRSAddEntry: Checking GUID of object {0} - got: {1}", dn, newCRObjectName.Guid.ToString());

                    BaseTestSite.Assert.AreEqual<Guid>(
                        newCRObjectName.Guid,
                        infolist.objGuid,
                        "IDL_DRSAddEntry: Checking GUID of the infoList in reply - got: {0}, expect: {1}",
                        infolist.objGuid.ToString(),
                        newCRObjectName.Guid.ToString());

                    AddObjectUpdate addUpdate = new AddObjectUpdate(dcServerType, dn);
                    this.updateStorage.PushUpdate(addUpdate);
                    return;
                }
                catch (Exception e)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Test case failed: " + (e.Message == null ? "" : e.Message));
                    if (retry == 2)
                        throw;
                    else
                        System.Threading.Thread.Sleep(5 * 60000);
                    TestCleanup();
                    TestInitialize();
                }
            }
        }

        [Priority(1)]
        [TestCategory("Win2003")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling IDL_DRSAddEntry to create an nTDSDSA object.")]
        public void DRSR_DRSAddEntry_V2_Success_Create_nTDSDSA()
        {
            DrsrTestChecker.Check();
            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.DC_With_DomainNamingMasterRole;
                    DsServer dcServer = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
                    DRS_AddEntry_Versions reqVer = DRS_AddEntry_Versions.V2;
                    DRS_SecBufferDesc[] pClientCreds = null;
                    uint? outVersion;
                    DRS_MSG_ADDENTRYREPLY? outMessage;
                    const string newObjectPrefixName = DRSTestData.DRSAddEntry_V2_Create_crossRef_newObjectPrefixName; //The prefix name of cn of the new object.

                    #region Add a server object as parent of the nTDSDSA object to be added
                    string cn = "test";
                    string newServerDn = "CN=" + cn + ",CN=Servers," + dcServer.Site.DN.Replace(" ", "");

                    bool fObjectExisted = ldapAdapter.IsObjectExist(dcServer, newServerDn);
                    int prefix = 0;
                    while (fObjectExisted)
                    {
                        cn = newObjectPrefixName + prefix;
                        newServerDn = "CN=" + cn + ",CN=Servers," + dcServer.Site.DN.Replace(" ", "");
                        fObjectExisted = ldapAdapter.IsObjectExist(dcServer, newServerDn);
                        prefix++;
                    }

                    DirectoryAttribute newServerClsAtt = new DirectoryAttribute("objectClass", "server");
                    DirectoryAttribute newServerCnAtt = new DirectoryAttribute("cn", cn);
                    DirectoryAttributeCollection attCollecetion = new DirectoryAttributeCollection();
                    attCollecetion.Add(newServerClsAtt);
                    attCollecetion.Add(newServerCnAtt);

                    //Add server object
                    ResultCode rc = this.ldapAdapter.AddObjectWithAttributes(dcServer, newServerDn, attCollecetion);
                    this.Site.Assert.AreEqual<ResultCode>(ResultCode.Success, rc, "LDAP: server object should be added successfully: {0}", newServerDn);
                    //Add for cleanup
                    AddObjectUpdate addServerUpdate = new AddObjectUpdate(dcServerType, newServerDn);
                    this.updateStorage.PushUpdate(addServerUpdate);
                    #endregion

                    #region Add an nTDSDSA object under the new server object (new nTDSDSA)
                    string newDcDn = "CN=NTDS Settings," + newServerDn;
                    string serverDsaName = dcServer.NtdsDsaObjectName;
                    //hasMasterNCs
                    ExtendedDNControl exControl = new ExtendedDNControl();
                    DirectoryControlCollection ctlCollection = new DirectoryControlCollection();
                    ctlCollection.Add(exControl);
                    SearchResultEntryCollection resultEntrys;
                    rc = this.ldapAdapter.ControlledSearch(
                        dcServer,
                        serverDsaName,
                        "(objectClass=*)",
                        SearchScope.Base,
                        new string[] { "hasMasterNCs", "msDS-Behavior-Version", "dMDLocation", "options", "systemFlags" },
                        ctlCollection,
                        out resultEntrys);
                    this.Site.Assert.AreEqual<ResultCode>(ResultCode.Success, rc, "LDAP: query the hasMasterNCs attribute of server DSA should be successful.");
                    DirectoryAttributeCollection dsaAttCollection = new DirectoryAttributeCollection();
                    DirectoryAttribute[] resultArray = new DirectoryAttribute[resultEntrys[0].Attributes.Count];
                    resultEntrys[0].Attributes.CopyTo(resultArray, 0);
                    dsaAttCollection.AddRange(resultArray);
                    DirectoryAttribute clsAtt = new DirectoryAttribute("objectClass", "nTDSDSA");
                    dsaAttCollection.Add(clsAtt);

                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Going to create an nTDSDSA object {0}", newDcDn);
                    ENTINF entInfObj = ldapAdapter.ConstructENTINF(dcServer, newDcDn, dsaAttCollection);
                    ENTINF[] entInfArray = new ENTINF[1];
                    entInfArray[0] = entInfObj;
                    ENTINFLIST entInfObjList = DrsrHelper.CreateENTINFLIST(entInfArray);

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServerType);
                    uint ret = drsTestClient.DrsBind(dcServerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "Calling IDL_DRSAddEntry to create an nTDSDSA object on dc:{0}", dcServerType);
                    ret = drsTestClient.DrsAddEntry(dcServerType, reqVer, entInfObjList, pClientCreds, out outVersion, out outMessage);
                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSAddEntry should return 0 if successful adding an nTDSDSA object.");

                    BaseTestSite.Assert.AreEqual<uint>((uint)reqVer, (uint)outVersion, "reply message version should be equal to the request message version");
                    BaseTestSite.Assert.IsNull(outMessage.Value.V2.pErrorObject, "pErrorObject in reply message should be null");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.errCode, 0, "errCode in reply message should be 0");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.extendedErr, 0, "extendedErr in reply message should be 0");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.problem, 0, "problem in reply message should be 0");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.cObjectsAdded, 1, "cObjectsAdded in reply message should be 1");
                    ADDENTRY_REPLY_INFO infolist = outMessage.Value.V2.infoList[0];
                    BaseTestSite.Assert.IsTrue(DrsrHelper.IsNullSid(infolist.objSid), "objSid in infoList of reply message should be null");

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "verify entif object with DN as {0} is created on DC {1} successfully", newDcDn, dcServerType);
                    DSNAME newDsaObjectName = ldapAdapter.GetDsName(dcServer, newDcDn).Value;
                    BaseTestSite.Assert.AreNotEqual(Guid.Empty, newDsaObjectName.Guid, "The nTDSDSA Ref Object should exist on DC {0}", dcServerType);
                    BaseTestSite.Assert.AreEqual<Guid>(newDsaObjectName.Guid, infolist.objGuid, "The nTDSDSA Ref Object should exist on DC {0}", dcServerType);

                    #endregion
                    return;
                }
                catch (Exception e)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Test case failed: " + (e.Message == null ? "" : e.Message));
                    if (retry == 2)
                        throw;
                    else
                        System.Threading.Thread.Sleep(5 * 60000);
                    TestCleanup();
                    TestInitialize();
                }
            }
        }

        [BVT]
        [Ignore]
        [Priority(0)]
        [TestCategory("Win2003")]
        [ServerFSMORole(FSMORoles.DomainNaming)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling IDL_DRSAddEntry to create a crossRef object.")]
        public void DRSR_DRSAddEntry_V2_Success_Enable_crossRef()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.DC_With_DomainNamingMasterRole;
            DsServer dcServer = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
            DRS_AddEntry_Versions reqVer = DRS_AddEntry_Versions.V2;
            DRS_SecBufferDesc[] pClientCreds = null;
            uint? outVersion;
            DRS_MSG_ADDENTRYREPLY? outMessage;
            const string newObjectPrefixName = DRSTestData.DRSAddEntry_V2_Create_crossRef_newObjectPrefixName; //The prefix name of cn of the new object.

            //Prepare the attribute of the new crossRef object.
            string partitionsDN = "CN=Partitions," + DrsrHelper.GetNamingContextDN(dcServer.Domain, NamingContext.ConfigNC);
            string domainDn = LdapUtility.GetDnFromNcType(dcServer, NamingContext.DomainNC);
            string cn = newObjectPrefixName;
            string dn = "CN=" + cn + "," + partitionsDN;

            bool fObjectExisted = ldapAdapter.IsObjectExist(dcServer, dn);
            int prefix = 0;
            while (fObjectExisted)
            {
                cn = newObjectPrefixName + prefix;
                dn = "CN=" + cn + "," + partitionsDN;
                fObjectExisted = ldapAdapter.IsObjectExist(dcServer, dn);
                prefix++;
            }

            string cnName = "DC=" + cn + "," + domainDn;

            string dnsRoot = EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.Endpoint].DnsHostName;
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set DnsRoot attribute to client machine {0} to allow Enable operation", dnsRoot);

            ENTINF entInfObj = ldapAdapter.ConstructNewCrossRefObject(dcServer, dn, cn, cnName, dnsRoot);
            ENTINF[] entInfArray = new ENTINF[1];
            entInfArray[0] = entInfObj;
            ENTINFLIST entInfObjList = DrsrHelper.CreateENTINFLIST(entInfArray);



            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSAddEntry: Binding to DC server: {0}", dcServerType);
            uint ret = drsTestClient.DrsBind(dcServerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: {1}, should return 0 on success.", ret, 0);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSAddEntry: Start to create crossRef object {0} on {1}", dn, dcServerType);
            ret = drsTestClient.DrsAddEntry(dcServerType, reqVer, entInfObjList, pClientCreds, out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSAddEntry: Checking return value - got: {0}, expect: {1}, should return 0 on success", ret, 0);

            ResultCode code = ldapAdapter.ModifyAttribute(dcServer, dn, new DirectoryAttribute("enabled", System.Text.Encoding.ASCII.GetBytes("FALSE")));
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, code, "Should succeeded to disable crossRef");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSAddEntry: Start to enable crossRef object {0} on {1}", dn, dcServerType);
            ret = drsTestClient.DrsAddEntry(dcServerType, reqVer, entInfObjList, pClientCreds, out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSAddEntry: Checking return value - got: {0}, expect: {1}, should return 0 on success", ret, 0);

            string val = ldapAdapter.GetAttributeValueInString(dcServer, dn, "enabled");
            BaseTestSite.Assert.IsTrue(val == null ? true : bool.Parse(val), "Enabled attribute of crossRef should be true or not set now");
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcServerType, dn);
            this.updateStorage.PushUpdate(addUpdate);
        }


        [Priority(1)]
        [TestCategory("Win2003")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling IDL_DRSAddEntry to create an nTDSDSA object.")]
        public void DRSR_DRSAddEntry_V3_Success_Create_nTDSDSA()
        {
            BaseTestSite.Log.Add(LogEntryKind.Comment, "This test case is to test V3 reply. But from protocol behavior, it does not enforces server to use V3 reply. For Windows Client, it does not use the extension field of V3 reply. So for 3rd party implementation, this test case can fail.");

            DrsrTestChecker.Check();
            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.DC_With_DomainNamingMasterRole;

                    DsServer dcServer = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
                    DRS_AddEntry_Versions reqVer = DRS_AddEntry_Versions.V3;
                    string spn = null;
                    string[] spns = null;
                    if (EnvironmentConfig.TestDS)
                        spns = ldapAdapter.GetServicePrincipalName(dcServer);
                    else
                        spns = ldapAdapter.GetServicePrincipalName((DsServer)EnvironmentConfig.MainDC);

                    foreach (string sname in spns)
                    {
                        if (sname.StartsWith("GC", StringComparison.CurrentCultureIgnoreCase))
                        {
                            spn = sname;
                            break;
                        }
                    }

                    DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

                    DRS_SecBufferDesc[] pClientCreds = new DRS_SecBufferDesc[] { DrsrHelper.GetAuthenticationToken(user.Username, user.Password, EnvironmentConfig.TestDS == false ? EnvironmentConfig.MainDC.Domain.DNSName : user.Domain.DNSName, spn) };
                    uint? outVersion;
                    DRS_MSG_ADDENTRYREPLY? outMessage;
                    const string newObjectPrefixName = DRSTestData.DRSAddEntry_V2_Create_crossRef_newObjectPrefixName; //The prefix name of cn of the new object.

                    #region Add a server object as parent of the nTDSDSA object to be added
                    string cn = "test";
                    string newServerDn = "CN=" + cn + ",CN=Servers," + dcServer.Site.DN.Replace(" ", "");

                    bool fObjectExisted = ldapAdapter.IsObjectExist(dcServer, newServerDn);
                    int prefix = 0;
                    while (fObjectExisted)
                    {
                        cn = newObjectPrefixName + prefix;
                        newServerDn = "CN=" + cn + ",CN=Servers," + dcServer.Site.DN.Replace(" ", "");
                        fObjectExisted = ldapAdapter.IsObjectExist(dcServer, newServerDn);
                        prefix++;
                    }

                    DirectoryAttribute newServerClsAtt = new DirectoryAttribute("objectClass", "server");
                    DirectoryAttribute newServerCnAtt = new DirectoryAttribute("cn", cn);
                    DirectoryAttributeCollection attCollecetion = new DirectoryAttributeCollection();
                    attCollecetion.Add(newServerClsAtt);
                    attCollecetion.Add(newServerCnAtt);

                    //Add server object
                    ResultCode rc = this.ldapAdapter.AddObjectWithAttributes(dcServer, newServerDn, attCollecetion);
                    this.Site.Assert.AreEqual<ResultCode>(ResultCode.Success, rc, "LDAP: server object should be added successfully: {0}", newServerDn);
                    //Add for cleanup
                    AddObjectUpdate addServerUpdate = new AddObjectUpdate(dcServerType, newServerDn);
                    this.updateStorage.PushUpdate(addServerUpdate);
                    #endregion

                    #region Add an nTDSDSA object under the new server object (new nTDSDSA)
                    string newDcDn = "CN=NTDS Settings," + newServerDn;
                    string serverDsaName = dcServer.NtdsDsaObjectName;
                    //hasMasterNCs
                    ExtendedDNControl exControl = new ExtendedDNControl();
                    DirectoryControlCollection ctlCollection = new DirectoryControlCollection();
                    ctlCollection.Add(exControl);
                    SearchResultEntryCollection resultEntrys;
                    rc = this.ldapAdapter.ControlledSearch(
                        dcServer,
                        serverDsaName,
                        "(objectClass=*)",
                        SearchScope.Base,
                        new string[] { "hasMasterNCs", "msDS-Behavior-Version", "dMDLocation", "options", "systemFlags" },
                        ctlCollection,
                        out resultEntrys);
                    this.Site.Assert.AreEqual<ResultCode>(ResultCode.Success, rc, "LDAP: query the hasMasterNCs attribute of server DSA should be successful.");
                    DirectoryAttributeCollection dsaAttCollection = new DirectoryAttributeCollection();
                    DirectoryAttribute[] resultArray = new DirectoryAttribute[resultEntrys[0].Attributes.Count];
                    resultEntrys[0].Attributes.CopyTo(resultArray, 0);
                    dsaAttCollection.AddRange(resultArray);
                    DirectoryAttribute clsAtt = new DirectoryAttribute("objectClass", "nTDSDSA");
                    dsaAttCollection.Add(clsAtt);

                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Going to create an nTDSDSA object {0}", newDcDn);
                    ENTINF entInfObj = ldapAdapter.ConstructENTINF(dcServer, newDcDn, dsaAttCollection);
                    ENTINF[] entInfArray = new ENTINF[1];
                    entInfArray[0] = entInfObj;
                    ENTINFLIST entInfObjList = DrsrHelper.CreateENTINFLIST(entInfArray);

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServerType);
                    uint ret = drsTestClient.DrsBind(dcServerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_WHISTLER_BETA3);
                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "Calling IDL_DRSAddEntry to create an nTDSDSA object on dc:{0}", dcServerType);
                    ret = drsTestClient.DrsAddEntry(dcServerType, reqVer, entInfObjList, pClientCreds, out outVersion, out outMessage);
                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSAddEntry should return 0 if successful adding an nTDSDSA object.");

                    BaseTestSite.Assert.AreEqual<uint>((uint)reqVer, (uint)outVersion, "reply message version should be equal to the request message version");
                    BaseTestSite.Assert.IsNull(outMessage.Value.V3.pdsErrObject, "pErrorObject in reply message should be null");
                    BaseTestSite.Assert.IsNotNull(outMessage.Value.V3.pErrData, "pErrData in reply message should not be null");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V3.pErrData[0].V1.errCode, 0, "errCode in reply message should be 0");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V3.pErrData[0].V1.dwRepError, 0, "dwRepError in reply message should be 0");
                    BaseTestSite.Assert.IsNull(outMessage.Value.V3.pErrData[0].V1.pErrInfo, "pErrInfo in reply message should be null");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V3.cObjectsAdded, 1, "cObjectsAdded in reply message should be 1");
                    ADDENTRY_REPLY_INFO infolist = outMessage.Value.V3.infoList[0];
                    BaseTestSite.Assert.IsTrue(DrsrHelper.IsNullSid(infolist.objSid), "objSid in infoList of reply message should be null");

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "verify entif object with DN as {0} is created on DC {1} successfully", newDcDn, dcServerType);
                    DSNAME newDsaObjectName = ldapAdapter.GetDsName(dcServer, newDcDn).Value;
                    BaseTestSite.Assert.AreNotEqual(Guid.Empty, newDsaObjectName.Guid, "The nTDSDSA Ref Object should exist on DC {0}", dcServerType);
                    BaseTestSite.Assert.AreEqual<Guid>(newDsaObjectName.Guid, infolist.objGuid, "The nTDSDSA Ref Object should exist on DC {0}", dcServerType);

                    #endregion
                    return;
                }
                catch (Exception e)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Test case failed: " + (e.Message == null ? "" : e.Message));
                    if (retry == 2)
                        throw;
                    else
                        System.Threading.Thread.Sleep(5 * 60000);
                    TestCleanup();
                    TestInitialize();
                }
            }
        }

        [Priority(1)]
        [TestCategory("Win2008")]
        [SupportedADType(ADInstanceType.DS)] //User object used.
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling IDL_DRSAddEntry to modify multiple objects in a single transaction.")]
        public void DRSR_DRSAddEntry_V2_Success_Modify_Objects()
        {
            DrsrTestChecker.Check();
            for (int retry = 0; retry < 3; retry++)
            {
                try
                {
                    EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.WritableDC1;
                    DsServer dcServer = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
                    DRS_AddEntry_Versions reqVer = DRS_AddEntry_Versions.V2;
                    DRS_SecBufferDesc[] pClientCreds = null;
                    uint? outVersion;
                    DRS_MSG_ADDENTRYREPLY? outMessage;

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServerType);
                    uint ret = drsTestClient.DrsBind(dcServerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

                    #region Construct ENTINFLIST for modifying mutiple objects
                    //Object 1
                    DirectoryAttributeCollection atts = new DirectoryAttributeCollection();
                    string modObj1Dn = ldapAdapter.TestAddUserObj(dcServer);
                    DSNAME? obj1Name = ldapAdapter.GetDsName(dcServer, modObj1Dn);
                    string orgAtt1 = ldapAdapter.GetAttributeValueInString(dcServer, modObj1Dn, "displayName");
                    string modifiedAtt1 = orgAtt1 + "Modified";
                    DirectoryAttribute att1 = new DirectoryAttribute("displayName", modifiedAtt1);
                    atts.Add(att1);
                    ENTINF modInf1 = ldapAdapter.ConstructENTINF(dcServer, modObj1Dn, atts);
                    modInf1.ulFlags = 0x00010000; //ENTINF_REMOTE_MODIFY
                    modInf1.pName = obj1Name;
                    //Add for cleanup
                    AddObjectUpdate update1 = new AddObjectUpdate(dcServerType, modObj1Dn);
                    updateStorage.PushUpdate(update1);

                    //Object 2
                    atts = new DirectoryAttributeCollection();
                    string modObj2Dn = ldapAdapter.TestAddUserObj(dcServer);
                    DSNAME? obj2Name = ldapAdapter.GetDsName(dcServer, modObj2Dn);
                    string orgAtt2 = ldapAdapter.GetAttributeValueInString(dcServer, modObj2Dn, "displayName");
                    string modifiedAtt2 = orgAtt2 + "Modified";
                    DirectoryAttribute att2 = new DirectoryAttribute("displayName", modifiedAtt2);
                    atts.Add(att2);
                    ENTINF modInf2 = ldapAdapter.ConstructENTINF(dcServer, modObj2Dn, atts);
                    modInf2.ulFlags = 0x00010000; //ENTINF_REMOTE_MODIFY
                    modInf2.pName = obj2Name;
                    //add for cleanup
                    AddObjectUpdate update2 = new AddObjectUpdate(dcServerType, modObj2Dn);
                    updateStorage.PushUpdate(update2);

                    ENTINF[] entInfArray = new ENTINF[2];
                    entInfArray[0] = modInf1;
                    entInfArray[1] = modInf2;
                    ENTINFLIST entInfObjList = DrsrHelper.CreateENTINFLIST(entInfArray);
                    #endregion

                    #region Modify mutiple objects by calling IDL_DRSAddEntry

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "Calling IDL_DRSAddEntry to modify objects: {0}, {1}", modObj1Dn, modObj2Dn);
                    ret = drsTestClient.DrsAddEntry(dcServerType, reqVer, entInfObjList, pClientCreds, out outVersion, out outMessage);
                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSAddEntry should return 0 if successful adding an nTDSDSA object.");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.errCode, 0, "IDL_DRSAddEntry: errCode in reply message should be 0");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.extendedErr, 0, "IDL_DRSAddEntry: extendedErr in reply message should be 0");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.problem, 0, "IDL_DRSAddEntry: problem in reply message should be 0");
                    BaseTestSite.Assert.AreEqual<uint>(outMessage.Value.V2.cObjectsAdded, (uint)entInfArray.Length, "IDL_DRSAddEntry: cObjectsAdded in reply message should be the count of modified objects.");

                    //Verified the returned object info
                    for (int i = 0; i < outMessage.Value.V2.infoList.Length; i++)
                    {
                        BaseTestSite.Assert.AreEqual<Guid>(entInfArray[i].pName.Value.Guid, outMessage.Value.V2.infoList[i].objGuid, "IDL_DRSAddEntry: infoList: object Guid must be verified.");
                        BaseTestSite.Assert.IsTrue(DrsrHelper.NT4SID_Equals(entInfArray[i].pName.Value.Sid, outMessage.Value.V2.infoList[i].objSid), "IDL_DRSAddEntry: infoList: object sid must be verified.");
                    }
                    BaseTestSite.Assert.IsTrue(true, "IDL_DRSAddEntry: infoList: the item order matches the item order of values in the EntInfList field in the request structure.");

                    #endregion

                    #region Verify the modified attributes with LDAP

                    string afterModifiedAtt1 = ldapAdapter.GetAttributeValueInString(dcServer, modObj1Dn, "displayName");
                    BaseTestSite.Assert.AreEqual<string>(modifiedAtt1, afterModifiedAtt1.ToString(), "LDAP: the modified attribute should be verified.");

                    string afterModifiedAtt2 = ldapAdapter.GetAttributeValueInString(dcServer, modObj2Dn, "displayName");
                    BaseTestSite.Assert.AreEqual<string>(modifiedAtt2, afterModifiedAtt2.ToString(), "LDAP: the modified attribute should be verified.");

                    #endregion
                    return;
                }
                catch (Exception e)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Test case failed: " + (e.Message == null ? "" : e.Message));
                    if (retry == 2)
                        throw;
                    else
                        System.Threading.Thread.Sleep(5 * 60000);
                    TestCleanup();
                    TestInitialize();
                }
            }
        }

        #endregion

        #region DRSADDSidHistory
        [BVT]
        [Ignore]
        [Priority(0)]
        [TestCategory("Win2003")]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"This test case calles IDL_DRSAddSidHistory to add sid history from one test user object to another test user object without delete source")]
        public void DRSR_DRSAddSidHistory_V1_Success_FromSameDomainWithDeleteSource()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsUser User = dcServerMachine.Domain.Admin;

            IDL_DRSAddSidHistory_pdwOutVersion_Values? outVersion;
            DRS_MSG_ADDSIDREPLY? outMessage;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSAddSidHistory: Enabling Auditing on DC server: {0}", dcServer);
            bool enableAuditSucc = sutControlAdapter.EnableAuditingOnServer(dcServerMachine.DnsHostName, User.Domain.NetbiosName + "\\" + User.Username, User.Password);
            BaseTestSite.Assume.IsTrue(
                enableAuditSucc,
                "IDL_DRSAddSidHistory: Enabling Auditing on DC server {0} returned {1}",
                dcServer,
                enableAuditSucc);

            string dn1 = ldapAdapter.TestAddUserObj(dcServerMachine);
            string dn2 = ldapAdapter.TestAddUserObj(dcServerMachine);
            AddObjectUpdate updateObject1 = new AddObjectUpdate(dcServer, dn1);
            AddObjectUpdate updateObject2 = new AddObjectUpdate(dcServer, dn2);
            updateStorage.PushUpdate(updateObject1);
            updateStorage.PushUpdate(updateObject2);


            string sid1 = LdapUtility.GetObjectStringSid(dcServerMachine, dn1);
            string sid2 = LdapUtility.GetObjectStringSid(dcServerMachine, dn2);

            BaseTestSite.Assert.IsNotNull(dn1, "IDL_DRSAddSidHistory: Checking if {0} created on server {1} - {2}", dn1, dcServerMachine, dn1 != null);
            BaseTestSite.Assert.IsNotNull(dn2, "IDL_DRSAddSidHistory: Checking if {0} created on server {1} - {2}", dn2, dcServerMachine, dn2 != null);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSBind: Binding to DC server {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: 0, should return 0 on success", ret);

            ret = drsTestClient.DrsAddSidHistory(
                dcServer,
                EnvironmentConfig.User.None,
                DomainEnum.None,
                DomainEnum.None,
                IDL_DRSAddSidHistory_dwInVersion_Values.V1,
                DRS_ADDSID_FLAGS.DS_ADDSID_FLAG_PRIVATE_DEL_SRC_OBJ,
                dn2,
                ObjectNameType.DN,
                dn1,
                ObjectNameType.DN,
                EnvironmentConfig.Machine.WritableDC1,
                out outVersion,
                out outMessage);


            //verification
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSAddSidHistory: Checking return value - got: {0}, expect: 0, should return 0 on success", ret);
            BaseTestSite.Assert.AreEqual<uint>(1, (uint)outVersion.Value, "IDL_DRSAddSidHistory: Checking outVersion - got: {0}, expect: 1", (uint)outVersion.Value);
            BaseTestSite.Assert.AreEqual<uint>(0, outMessage.Value.V1.dwWin32Error, "IDL_DRSAddSidHistory: Checking dwWin32Error in reply - got: {0}, expect: 0", outMessage.Value.V1.dwWin32Error);

            string sid1AfterCall = LdapUtility.GetObjectStringSid(dcServerMachine, dn1);
            string sid1HistoryAfterCall = LdapUtility.GetObjectStringSidHistory(dcServerMachine, dn1);

            BaseTestSite.Assert.IsTrue(sid1AfterCall == sid1, "IDL_DRSAddSidHistory: Checking if SID of object {0} has changed - before: {1}, after: {2}", dn1, sid1, sid1AfterCall);
            BaseTestSite.Assert.IsTrue(
                sid1HistoryAfterCall.Contains(sid2),
                "IDL_DRSAddSidHistory: Checking if sidHistory of dn1 contains dn2's sid - dn1's sidHistory is {0}, dn2's SID is {1}",
                sid1HistoryAfterCall,
                sid2);

            bool isDn2Removed = LdapUtility.IsObjectExist(dcServerMachine, dn2);
            BaseTestSite.Assert.IsFalse(isDn2Removed, "IDL_DRSAddSidHistory: Checking if {0} is removed - {1}", dn2, !isDn2Removed);

            //clean up            
            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSAddSidHistory: Disable Auditing on DC server {0}", dcServer);
            bool isDisableAuditingSucc = sutControlAdapter.DisableAuditingOnServer(dcServerMachine.DnsHostName, User.Username, User.Password);
            BaseTestSite.Assume.IsTrue(
                isDisableAuditingSucc,
                "IDL_DRSAddSidHistory: Disable Auditing on DC server: {0} - got: {1}, expect: true",
                dcServer,
                isDisableAuditingSucc
                );

        }

        void disableAuditing(string svr, string user, string pwd)
        {
            BaseTestSite.Assume.IsTrue(sutControlAdapter.DisableAuditingOnServer(svr, user, pwd), "Disable Auditing on DC server: {0} failed", svr);
        }

        void enableAuditing(string svr, string user, string pwd)
        {
            BaseTestSite.Assume.IsTrue(sutControlAdapter.EnableAuditingOnServer(svr, user, pwd), "Enable Auditing on DC server: {0} failed", svr);
        }

        [Ignore]
        [Priority(0)]
        [TestCategory("Win2003")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [RequireDcPartner]
        [SupportedADType(ADInstanceType.DS)]
        [Description(@"This test case calles IDL_DRSAddSidHistory to add sid history from one test user object to another test user object without delete source")]
        public void DRSR_DRSAddSidHistory_V1_Success_FromExternalForestWithoutDeleteSource()
        {
            DrsrTestChecker.Check();
            DsServer destSvr = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];
            DsServer srcSvr = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.ExternalDC];
            DsUser User = srcSvr.Domain.Admin;
            DsUser destUser = destSvr.Domain.Admin;
            IDL_DRSAddSidHistory_pdwOutVersion_Values? outVersion;
            DRS_MSG_ADDSIDREPLY? outMessage;

            enableAuditing(srcSvr.DnsHostName, User.Domain.NetbiosName + "\\" + User.Username, User.Password);
            enableAuditing(destSvr.DnsHostName, destUser.Domain.NetbiosName + "\\" + destUser.Username, destUser.Password);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Create test objects");
            string srcObj = ldapAdapter.TestAddUserObj(srcSvr);
            string destObj = ldapAdapter.TestAddUserObj(destSvr);
            /* have unstable access right exceptions, commented for further fix
            AddObjectUpdate updateObject1 = new AddObjectUpdate(EnvironmentConfig.Machine.WritableDC1, destObj);
            AddObjectUpdate updateObject2 = new AddObjectUpdate(EnvironmentConfig.Machine.ExternalDC, srcObj);
            updateStorage.PushUpdate(updateObject1);
            updateStorage.PushUpdate(updateObject2);*/

            //get source object sid
            string srcSid = LdapUtility.GetObjectStringSid(srcSvr, srcObj);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsAddSidHistory(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ExternalDomainAdmin, DomainEnum.ExternalDomain, DomainEnum.PrimaryDomain, IDL_DRSAddSidHistory_dwInVersion_Values.V1, DRS_ADDSID_FLAGS.NONE,
                                                srcObj, ObjectNameType.SAM, destObj, ObjectNameType.SAM, EnvironmentConfig.Machine.ExternalDC, out outVersion, out outMessage);

            disableAuditing(srcSvr.DnsHostName, User.Domain.NetbiosName + "\\" + User.Username, User.Password);
            disableAuditing(destSvr.DnsHostName, destUser.Domain.NetbiosName + "\\" + destUser.Username, destUser.Password);
        }

        [Priority(0)]
        [Ignore]
        [TestCategory("Win2003")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"This test case calles IDL_DRSAddSidHistory to add sid history from one test user object to another test user object without delete source")]
        public void DRSR_DRSAddSidHistory_V1_Success_WithCheckFlag()
        {
            DrsrTestChecker.Check();
            IDL_DRSAddSidHistory_pdwOutVersion_Values? outVersion;
            DRS_MSG_ADDSIDREPLY? outMessage;

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsAddSidHistory(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.None, DomainEnum.None, DomainEnum.None,
                 IDL_DRSAddSidHistory_dwInVersion_Values.V1, DRS_ADDSID_FLAGS.DS_ADDSID_FLAG_PRIVATE_CHK_SECURE, null, ObjectNameType.NONE, null, ObjectNameType.NONE, EnvironmentConfig.Machine.None, out outVersion, out outMessage);

        }

        #endregion

        #region DrsWriteSPN
        [BVT]
        [Priority(0)]
        [TestCategory("Win2003")]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"This test case call  IDL_DRSWriteSPN to add SPNs for test user object")]
        public void DRSR_DRSWriteSPN_V1_Success_AddSPNs()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            pdwOutVersion_Values? outVersion;
            DRS_MSG_SPNREPLY? outMessage;

            //to do: Change to do the operation on machine
            string dn = ldapAdapter.TestAddUserObj(dcServerMachine);


            string[] sPNList = new string[2];
            sPNList[0] = DRSTestData.DRSWriteSPN_V1_Success_AddSPNs_sPN1;
            sPNList[1] = DRSTestData.DRSWriteSPN_V1_Success_AddSPNs_sPN2;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSBind: Binding to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: 0, should return 0 with a success bind to DC", ret);

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                @"call  IDL_DRSWriteSPN to add SPNs for test user object.");
            ret = drsTestClient.DrsWriteSPN(dcServer, dwInVersion_Values.V1, DS_SPN_OPREATION.DS_SPN_ADD_SPN_OP, dn,
                                            sPNList, out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSWriteSPN: Checking return value - got: {0}, expect: 0, should return 0 if successful.", ret);
            BaseTestSite.Assert.AreEqual<uint>(1, (uint)outVersion, "IDL_DRSWriteSPN: Checking outVersion in reply - got: {0}, expect: 1", (uint)outVersion);
            BaseTestSite.Assert.AreEqual<uint>(0, outMessage.Value.V1.retVal, "IDL_DRSWriteSPN: Checking retVal in reply - got: {0}, expect: 0", outMessage.Value.V1.retVal);

            string[] dnSpnList = ldapAdapter.GetServicePrincipalName(dcServerMachine, dn);

            int match = 0;
            foreach (string spn in sPNList)
            {
                foreach (string dspn in dnSpnList)
                {
                    if (dspn == spn)
                        match++;
                }
            }

            BaseTestSite.Assert.IsTrue(match == 2, "IDL_DRSWriteSPN: the SPN should contain {0} and {1}", sPNList[0], sPNList[1]);

            //clean up
            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSWriteSPN: Deleting dn objects: {0}", dn);

            ResultCode r = ldapAdapter.DeleteObject(dcServerMachine, dn);
            BaseTestSite.Assume.IsTrue(r == 0, "IDL_DRSWriteSPN: Checking if {0} is deleted - {1}.", dn, r == 0);

        }



        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("This test case call  IDL_DRSWriteSPN to replace SPNs for test  user object")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSWriteSPN_V1_Success_ReplaceSPNS()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            string dn = ldapAdapter.TestAddUserObj((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1]);

            string[] sPNList = new string[1];
            sPNList[0] = DRSTestData.DRSWriteSPN_V1_Success_AddSPNs_sPN1;
            pdwOutVersion_Values? outVer = null;
            DRS_MSG_SPNREPLY? reply = null;
            drsTestClient.DrsWriteSPN(EnvironmentConfig.Machine.WritableDC1, dwInVersion_Values.V1, DS_SPN_OPREATION.DS_SPN_ADD_SPN_OP, dn, sPNList,
                out outVer,
                out reply);

            sPNList[0] = DRSTestData.DRSWriteSPN_V1_Success_AddSPNs_sPN2;
            drsTestClient.DrsWriteSPN(EnvironmentConfig.Machine.WritableDC1, dwInVersion_Values.V1, DS_SPN_OPREATION.DS_SPN_REPLACE_SPN_OP, dn, sPNList,
                out outVer,
                out reply);
        }


        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with DRS_WRIT_REP flag")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSWriteSPN_V1_Success_RemoveSPNS()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            string dn = ldapAdapter.TestAddUserObj((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1]);

            string[] sPNList = new string[1];
            sPNList[0] = DRSTestData.DRSWriteSPN_V1_Success_AddSPNs_sPN1;
            pdwOutVersion_Values? outVer = null;
            DRS_MSG_SPNREPLY? reply = null;
            drsTestClient.DrsWriteSPN(EnvironmentConfig.Machine.WritableDC1, dwInVersion_Values.V1, DS_SPN_OPREATION.DS_SPN_ADD_SPN_OP, dn, sPNList,
                out outVer,
                out reply);

            sPNList = null;
            drsTestClient.DrsWriteSPN(EnvironmentConfig.Machine.WritableDC1, dwInVersion_Values.V1, DS_SPN_OPREATION.DS_SPN_REPLACE_SPN_OP, dn, sPNList,
                out outVer,
                out reply);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with DRS_WRIT_REP flag")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSWriteSPN_V1_Success_DeleteSpecifiedSPN()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            string dn = ldapAdapter.TestAddUserObj((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1]);

            string[] sPNList = new string[1];
            sPNList[0] = DRSTestData.DRSWriteSPN_V1_Success_AddSPNs_sPN1;
            pdwOutVersion_Values? outVer = null;
            DRS_MSG_SPNREPLY? reply = null;
            drsTestClient.DrsWriteSPN(EnvironmentConfig.Machine.WritableDC1, dwInVersion_Values.V1, DS_SPN_OPREATION.DS_SPN_ADD_SPN_OP, dn, sPNList,
                out outVer,
                out reply);

            drsTestClient.DrsWriteSPN(EnvironmentConfig.Machine.WritableDC1, dwInVersion_Values.V1, DS_SPN_OPREATION.DS_SPN_DELETE_SPN_OP, dn, sPNList,
                out outVer,
                out reply);
        }
        #endregion

    }
}
