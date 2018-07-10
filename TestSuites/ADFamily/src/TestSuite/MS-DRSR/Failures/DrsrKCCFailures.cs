// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsrKCCFailures : DrsrFailureTestClassBase
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrFailureTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            DrsrFailureTestClassBase.BaseCleanup();
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


        DRS_MSG_REPADD GenerateReplicaAddReq(EnvironmentConfig.Machine sut, EnvironmentConfig.Machine partner, DRS_MSG_REPADD_Versions version)
        {
            return drsTestClient.CreateReplicaAddReq(sut, version, (DsServer)EnvironmentConfig.MachineStore[partner], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
        }

        DRS_MSG_REPDEL GenerateReplicaDelReq(EnvironmentConfig.Machine machine, EnvironmentConfig.Machine src)
        {
            return drsTestClient.CreateReplicaDelReq(machine, (DsServer)EnvironmentConfig.MachineStore[src], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
        }

        /// <summary>
        /// test DRS ExecuteKCC access denied
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRSExecuteKCC access denied with a normal user account")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSExecuteKCC_Failed_WithNormalUser()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainUser, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            uint ret = drsTestClient.DrsExecuteKCC(EnvironmentConfig.Machine.WritableDC1, false);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED, ret, "server should return ERROR_DS_DRA_ACCESS_DENIED when caller is using a non-domain-admin account");
        }

        /// <summary>
        /// test DrsExecuteKCC when dwTaskID != 0
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRSExecuteKCC when dwTaskID != 0")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSExecuteKCC_Failed_WithBadTaskID()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            DRS_MSG_KCC_EXECUTE req = drsTestClient.CreateExecuteKCCReq();
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Change dwTaskID to 10");
            req.V1.dwTaskID = 0x10;
            uint ret = drsTestClient.DRSClient.DrsExecuteKcc(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_INVALID_PARAMETER, ret, "server should return ERROR_INVALID_PARAMETER when dwTaskID != 0");
        }


        /// <summary>
        /// test DRS replica add with invalid pNC
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS replica add with invalid pNC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V1_InvalidNC()
        {
            DrsrTestChecker.Check();

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_REPADD req = GenerateReplicaAddReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_MSG_REPADD_Versions.V1);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Make the pNC a default empty DSNAME");
            req.V1.pNC = DrsuapiClient.CreateDsName(null, Guid.Empty, null);
            uint ret = drsTestClient.DRSClient.DrsReplicaAdd(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_BAD_NC, ret, "server should return ERROR_DS_DRA_BAD_NC to V1 request if pNC is invalid but not null");
        }

        /// <summary>
        /// test DRS replica add with invalid pNC
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS replica add with invalid pNC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V2_InvalidNC()
        {
            DrsrTestChecker.Check();

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            DRS_MSG_REPADD req = GenerateReplicaAddReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_MSG_REPADD_Versions.V2);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Make the pNC a default empty DSNAME");
            req.V2.pNC = DrsuapiClient.CreateDsName(null, Guid.Empty, null);
            uint ret = drsTestClient.DRSClient.DrsReplicaAdd(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 2, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_BAD_NC, ret, "server should return ERROR_DS_DRA_BAD_NC to V2 request if pNC is invalid but not null");
        }

        /// <summary>
        /// test DRS replica add with invalid pNC
        /// </summary>
        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS replica add with invalid pNC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V3_InvalidNC()
        {
            DrsrTestChecker.Check();

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            DRS_MSG_REPADD req = GenerateReplicaAddReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_MSG_REPADD_Versions.V3);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Make the pNC a default empty DSNAME");
            req.V3.pNC = DrsuapiClient.CreateDsName(null, Guid.Empty, null);
            uint ret = drsTestClient.DRSClient.DrsReplicaAdd(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 3, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_BAD_NC, ret, "server should return ERROR_DS_DRA_BAD_NC to V3 request if pNC is invalid but not null");
        }


        /// <summary>
        /// test DRS replica add with empty psaDsaSrc
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS replica add with empty psaDsaSrc")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V1_EmptyDsaSrc()
        {
            DrsrTestChecker.Check();

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_REPADD req = GenerateReplicaAddReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_MSG_REPADD_Versions.V1);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Make the pszDsaSrc an empty string");
            req.V1.pszDsaSrc = string.Empty;
            uint ret = drsTestClient.DRSClient.DrsReplicaAdd(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "server should return ERROR_DS_DRA_INVALID_PARAMETER to V1 request if pszDsaSrc is empty");
        }

        /// <summary>
        /// test DRS replica add with empty pszSourceDsaAddress
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS replica add with empty pszSourceDsaAddress")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V2_EmptyDsaSrc()
        {
            DrsrTestChecker.Check();

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_REPADD req = GenerateReplicaAddReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_MSG_REPADD_Versions.V2);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Make the pszDsaSrc an empty string");
            req.V2.pszSourceDsaAddress = string.Empty;
            uint ret = drsTestClient.DRSClient.DrsReplicaAdd(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 2, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "server should return ERROR_DS_DRA_INVALID_PARAMETER to V2 request if pszSourceDsaAddress is empty");
        }

        /// <summary>
        /// test DRS replica add with empty pszSourceDsaAddress
        /// </summary>
        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS replica add with empty pszSourceDsaAddress")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V3_EmptyDsaSrc()
        {
            DrsrTestChecker.Check();

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_REPADD req = GenerateReplicaAddReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_MSG_REPADD_Versions.V3);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Make the pszDsaSrc an empty string");
            req.V3.pszSourceDsaAddress = string.Empty;
            uint ret = drsTestClient.DRSClient.DrsReplicaAdd(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 3, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "server should return ERROR_DS_DRA_INVALID_PARAMETER to V3 request if pszSourceDsaAddress is empty");
        }

        /// <summary>
        /// test DRS replica add with duplicated source
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("test DRS replica add with duplicated source")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V1_DuplicatedSource()
        {
            DrsrTestChecker.Check();

            drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            uint ret = drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
            if (ret == 0)
                //possible that dc didn't add the source back
                ret = drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);

            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_DN_EXISTS, ret, "server should return ERROR_DS_DRA_DN_EXISTS to V1 request if source is already in repsFrom");
        }

        /// <summary>
        /// test DRS replica add with duplicated source
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("test DRS replica add with duplicated source")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V2_DuplicatedSource()
        {
            DrsrTestChecker.Check();
            drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            uint ret = drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V2, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
            if (ret == 0)
                //possible that dc didn't add the source back
                ret = drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);

            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_DN_EXISTS, ret, "server should return ERROR_DS_DRA_DN_EXISTS to V1 request if source is already in repsFrom");
        }

        /// <summary>
        /// test DRS replica add with duplicated source
        /// </summary>
        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("test DRS replica add with duplicated source")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V3_DuplicatedSource()
        {
            DrsrTestChecker.Check();
            drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            uint ret = drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V3, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
            if (ret == 0)
                //possible that dc didn't add the source back
                ret = drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);

            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_DN_EXISTS, ret, "server should return ERROR_DS_DRA_DN_EXISTS to V1 request if source is already in repsFrom");
        }

        /// <summary>
        /// test DRS Replica Add denied due to normal user account
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS Replica Add denied due to normal user account")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V1_WithNormalUser()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainUser, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            uint ret = drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED, ret, "server should return ERROR_DS_DRA_ACCESS_DENIED to V1 request if credential in DRSBind is not admin user");
        }

        /// <summary>
        /// test DRS Replica Add denied due to normal user account
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS Replica Add denied due to normal user account")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V2_WithNormalUser()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainUser, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            uint ret = drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V2, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED, ret, "server should return ERROR_DS_DRA_ACCESS_DENIED to V2 request if credential in DRSBind is not admin user");
        }

        /// <summary>
        /// test DRS Replica Add denied due to normal user account
        /// </summary>
        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS Replica Add denied due to normal user account")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_Failed_V3_WithNormalUser()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainUser, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            uint ret = drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V3, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED, ret, "server should return ERROR_DS_DRA_ACCESS_DENIED to V3 request if credential in DRSBind is not admin user");
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS Replica Del with invalid pNC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDel_Failed_InvalidNC()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_REPDEL req = GenerateReplicaDelReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Set pNC to default empty DSNAME");
            req.V1.pNC = DrsuapiClient.CreateDsName(null, Guid.Empty, null);
            uint ret = drsTestClient.DRSClient.DrsReplicaDel(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_BAD_NC, ret, "server should return ERROR_DS_DRA_BAD_NC if pNC is invalid but not null");
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS Replica Del with invalid pszDsaSrc")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDel_Failed_InvalidDsaSrc()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_REPDEL req = GenerateReplicaDelReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Set pszDsaSrc to \"x\"");
            req.V1.pszDsaSrc = "x";
            uint ret = drsTestClient.DRSClient.DrsReplicaDel(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);
            if (EnvironmentConfig.TestDS)
                BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_NO_REPLICA, ret, "server should return ERROR_DS_DRA_NO_REPLICA if pszDsaSrc is invalid");
            else
                BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "server should return ERROR_DS_DRA_INVALID_PARAMETER if pszDsaSrc is invalid");

        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS Replica Del with empty pszDsaSrc")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDel_Failed_EmptyDsaSrc()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_REPDEL req = GenerateReplicaDelReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Set pszDsaSrc to empty");
            req.V1.pszDsaSrc = string.Empty;
            uint ret = drsTestClient.DRSClient.DrsReplicaDel(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "server should return ERROR_DS_DRA_INVALID_PARAMETER if pszDsaSrc is empty");
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("test DRS Replica Del with invalid pszDsaSrc")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDel_Failed_NullDsaSrc()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_REPDEL req = GenerateReplicaDelReq(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Set pszDsaSrc to NULL");
            req.V1.pszDsaSrc = null;
            uint ret = drsTestClient.DRSClient.DrsReplicaDel(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "server should return ERROR_DS_DRA_INVALID_PARAMETER if pszDsaSrc is null");
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with DRS_WRIT_REP flag")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDel_Failed_WithNormalUser()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainUser, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            uint ret = drsTestClient.DrsReplicaDel(EnvironmentConfig.Machine.WritableDC1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_ASYNC_OP, NamingContext.ConfigNC);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED, ret, "");
        }
        #region DRSUpdateRefs Negative Test
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with pszDsaDest = null and server will response ERROR_DS_DRA_INVALID_PARAMETER")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V1_pszDsaDest_NULL()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_pszDsaDest_NULL(DrsUpdateRefs_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with pszDsaDest = null and server will response ERROR_DS_DRA_INVALID_PARAMETER")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V2_pszDsaDest_NULL()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_pszDsaDest_NULL(DrsUpdateRefs_Versions.V2);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with uuidDsaObjDest = null and server will response ERROR_DS_DRA_INVALID_PARAMETER")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V1_uuidDsaObjDest_NULL()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_uuidDsaObjDest_NULL(DrsUpdateRefs_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with uuidDsaObjDest = null and server will response ERROR_DS_DRA_INVALID_PARAMETER")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V2_uuidDsaObjDest_NULL()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_uuidDsaObjDest_NULL(DrsUpdateRefs_Versions.V2);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with options does not contain DRS_ADD_REF and DRS_DEL_REF and server will response ERROR_DS_DRA_INVALID_PARAMETER")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V1_WithoutOptions()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_WithoutOptions(DrsUpdateRefs_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with options does not contain DRS_ADD_REF and DRS_DEL_REF and server will response ERROR_DS_DRA_INVALID_PARAMETER")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V2_WithoutOptions()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_WithoutOptions(DrsUpdateRefs_Versions.V2);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with nc!instanceType(pNC) is IT_WRITE, options has DRS_WRIT_REP, the server returns ERROR_DS_DRA_BAD_NC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V1_NONExist_NC()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_NONExist_NC(DrsUpdateRefs_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with nc!instanceType(pNC) is IT_WRITE, options has DRS_WRIT_REP, the server returns ERROR_DS_DRA_BAD_NC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V2_NONExist_NC()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_NONExist_NC(DrsUpdateRefs_Versions.V2);
        }
        #endregion

        #region Private Method
        private void DRSUpdateRefs_pszDsaDest_NULL(DrsUpdateRefs_Versions ver)
        {
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_UPDREFS req;

            EnvironmentConfig.Machine machine = EnvironmentConfig.Machine.WritableDC1;
            DsServer dest = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2];
            DRS_OPTIONS options;
            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Try to delete replication destination from repsTo for later testing. It MAY fails if there is no such record to delete");

                options = DRS_OPTIONS.DRS_DEL_REF;

                req = drsTestClient.CreateRequestForDrsUpdateRef(
                                        machine,
                                        ver,
                                        dest,
                                        options
                                        );
                ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
            }
            catch
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "No similar record to delete in repsTo. It's OK to continue");
            }

            options = DRS_OPTIONS.DRS_ADD_REF;
            req = drsTestClient.CreateRequestForDrsUpdateRef(
                        machine,
                        ver,
                        dest,
                        options
                        );
            req.V1.pszDsaDest = "";// Invalid value
            ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "the server should return ERROR_DS_DRA_INVALID_PARAMETER");
            options = DRS_OPTIONS.DRS_DEL_REF;
            req = drsTestClient.CreateRequestForDrsUpdateRef(
                        machine,
                        ver,
                        dest,
                        options
                        );
            ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
        }

        private void DRSUpdateRefs_uuidDsaObjDest_NULL(DrsUpdateRefs_Versions ver)
        {
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_UPDREFS req;

            EnvironmentConfig.Machine machine = EnvironmentConfig.Machine.WritableDC1;
            DsServer dest = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2];
            DRS_OPTIONS options;
            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Try to delete replication destination from repsTo for later testing. It MAY fails if there is no such record to delete");

                options = DRS_OPTIONS.DRS_DEL_REF;

                req = drsTestClient.CreateRequestForDrsUpdateRef(
                                        machine,
                                        ver,
                                        dest,
                                        options
                                        );
                ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
            }
            catch
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "No similar record to delete in repsTo. It's OK to continue");
            }

            options = DRS_OPTIONS.DRS_ADD_REF;
            req = drsTestClient.CreateRequestForDrsUpdateRef(
                        machine,
                        ver,
                        dest,
                        options
                        );
            req.V1.uuidDsaObjDest = Guid.Empty; // Invalid value
            ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "the server should return ERROR_DS_DRA_INVALID_PARAMETER");
            options = DRS_OPTIONS.DRS_DEL_REF;
            req = drsTestClient.CreateRequestForDrsUpdateRef(
                        machine,
                        ver,
                        dest,
                        options
                        );
            ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
        }

        private void DRSUpdateRefs_WithoutOptions(DrsUpdateRefs_Versions ver)
        {
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_UPDREFS req;

            EnvironmentConfig.Machine machine = EnvironmentConfig.Machine.WritableDC1;
            DsServer dest = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2];
            DRS_OPTIONS options;
            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Try to delete replication destination from repsTo for later testing. It MAY fails if there is no such record to delete");

                options = DRS_OPTIONS.DRS_DEL_REF;

                req = drsTestClient.CreateRequestForDrsUpdateRef(
                                        machine,
                                        ver,
                                        dest,
                                        options
                                        );
                ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
            }
            catch
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "No similar record to delete in repsTo. It's OK to continue");
            }

            options = DRS_OPTIONS.NONE; //invalid value
            req = drsTestClient.CreateRequestForDrsUpdateRef(
                        machine,
                        ver,
                        dest,
                        options
                        );
            ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "the server should return ERROR_DS_DRA_INVALID_PARAMETER");
            options = DRS_OPTIONS.DRS_DEL_REF;
            req = drsTestClient.CreateRequestForDrsUpdateRef(
                        machine,
                        ver,
                        dest,
                        options
                        );
            ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
        }

        private void DRSUpdateRefs_NONExist_NC(DrsUpdateRefs_Versions ver)
        {
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            DRS_MSG_UPDREFS req;

            EnvironmentConfig.Machine machine = EnvironmentConfig.Machine.WritableDC1;
            DsServer dest = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2];
            DRS_OPTIONS options;
            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Try to delete replication destination from repsTo for later testing. It MAY fails if there is no such record to delete");

                options = DRS_OPTIONS.DRS_DEL_REF;

                req = drsTestClient.CreateRequestForDrsUpdateRef(
                                        machine,
                                        ver,
                                        dest,
                                        options
                                        );
                ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
            }
            catch
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "No similar record to delete in repsTo. It's OK to continue");
            }


            options = DRS_OPTIONS.DRS_ADD_REF;
            req = drsTestClient.CreateRequestForDrsUpdateRef(
                        machine,
                        ver,
                        dest,
                        options,
                        NamingContext.None // invalid value
                        );
            ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_BAD_NC, ret, "the server should return ERROR_DS_DRA_BAD_NC");

            options = DRS_OPTIONS.DRS_DEL_REF;
            req = drsTestClient.CreateRequestForDrsUpdateRef(
                        machine,
                        ver,
                        dest,
                        options
                        );
            ret = drsTestClient.DRSClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)ver, req);
        }
        #endregion
    }


}
