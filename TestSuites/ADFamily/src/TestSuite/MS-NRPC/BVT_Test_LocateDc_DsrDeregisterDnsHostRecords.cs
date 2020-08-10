// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [TestClassAttribute()]
    public partial class BVT_Test_LocateDc_DsrDeregisterDnsHostRecords : PtfTestClassBase
    {

        public BVT_Test_LocateDc_DsrDeregisterDnsHostRecords()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter INrpcServerAdapterInstance;

        private Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerSutControlAdapter INrpcServerSutControlAdapterInstance;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.INrpcServerAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter))));
            this.INrpcServerSutControlAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerSutControlAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerSutControlAdapter))));
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Test Starting in S0
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS0()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S24\'");
            bool temp1;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp1);
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp1, "isAdministrator of GetClientAccountType, state S36");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,T" +
                    "rustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp2 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp2, "return of DsrDeregisterDnsHostRecords, state S60");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }

        private void BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72()
        {
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call RestartNetlogonService()\'");
            Common.ADCommonServerAdapter adapter = Common.ADCommonServerAdapter.Instance(BaseTestSite);
            this.INrpcServerSutControlAdapterInstance.RestartNetlogonService(
                adapter.ENDPOINTNetbiosName + "." + adapter.PrimaryDomainDnsName,
                NrpcServerAdapter.tdcFQDN);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return RestartNetlogonService\'");
            this.Manager.Comment("reaching state \'S74\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS10()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp3;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp3);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp3, "sutPlatform of GetPlatform, state S11");
            this.Manager.Comment("reaching state \'S29\'");
            bool temp4;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp4);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp4, "isAdministrator of GetClientAccountType, state S41");
            this.Manager.Comment("reaching state \'S53\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                    "e,Null,DsaGuidOne,PrimaryDc)\'");
            temp5 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of DsrDeregisterDnsHostRecords, state S65");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS12()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp6;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp6);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp6, "sutPlatform of GetPlatform, state S13");
            this.Manager.Comment("reaching state \'S30\'");
            bool temp7;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp7);
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp7, "isAdministrator of GetClientAccountType, state S42");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp8;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                    "e,InvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp8 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp8, "return of DsrDeregisterDnsHostRecords, state S66");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS14()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp9;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp9);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp9, "sutPlatform of GetPlatform, state S15");
            this.Manager.Comment("reaching state \'S31\'");
            bool temp10;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp10);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp10, "isAdministrator of GetClientAccountType, state S43");
            this.Manager.Comment("reaching state \'S55\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp11;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                    "e,PrimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp11 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp11, "return of DsrDeregisterDnsHostRecords, state S67");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS16()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp12;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp12);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp12, "sutPlatform of GetPlatform, state S17");
            this.Manager.Comment("reaching state \'S32\'");
            bool temp13;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp13);
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp13, "isAdministrator of GetClientAccountType, state S44");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp14;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                    "e,TrustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp14 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp14, "return of DsrDeregisterDnsHostRecords, state S68");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS18()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp15, "sutPlatform of GetPlatform, state S19");
            this.Manager.Comment("reaching state \'S33\'");
            bool temp16;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp16);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp16, "isAdministrator of GetClientAccountType, state S45");
            this.Manager.Comment("reaching state \'S57\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp17;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,N" +
                    "ull,DsaGuidOne,PrimaryDc)\'");
            temp17 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp17, "return of DsrDeregisterDnsHostRecords, state S69");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS2()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp18;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp18);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp18, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S25\'");
            bool temp19;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp19);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp19, "isAdministrator of GetClientAccountType, state S37");
            this.Manager.Comment("reaching state \'S49\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp20;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                    ",DsaGuidOne,PrimaryDc)\'");
            temp20 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NonExistDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp20, "return of DsrDeregisterDnsHostRecords, state S61");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS20()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp21;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp21);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp21, "sutPlatform of GetPlatform, state S21");
            this.Manager.Comment("reaching state \'S34\'");
            bool temp22;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp22);
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp22, "isAdministrator of GetClientAccountType, state S46");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp23;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,I" +
                    "nvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp23 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp23, "return of DsrDeregisterDnsHostRecords, state S70");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS22()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp24;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp24);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp24, "sutPlatform of GetPlatform, state S23");
            this.Manager.Comment("reaching state \'S35\'");
            bool temp25;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp25);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp25, "isAdministrator of GetClientAccountType, state S47");
            this.Manager.Comment("reaching state \'S59\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp26;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,P" +
                    "rimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp26 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp26, "return of DsrDeregisterDnsHostRecords, state S71");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS4()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp27;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp27);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp27, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S26\'");
            bool temp28;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp28);
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp28, "isAdministrator of GetClientAccountType, state S38");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp29;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Inva" +
                    "lidDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp29 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NonExistDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp29, "return of DsrDeregisterDnsHostRecords, state S62");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS6()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp30;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp30);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp30, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S27\'");
            bool temp31;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp31);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp31, "isAdministrator of GetClientAccountType, state S39");
            this.Manager.Comment("reaching state \'S51\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp32;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Prim" +
                    "aryDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp32 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NonExistDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp32, "return of DsrDeregisterDnsHostRecords, state S63");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS8()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp33;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp33);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp33, "sutPlatform of GetPlatform, state S9");
            this.Manager.Comment("reaching state \'S28\'");
            bool temp34;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp34);
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp34, "isAdministrator of GetClientAccountType, state S40");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp35;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Trus" +
                    "tedDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp35 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NonExistDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103453");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp35, "return of DsrDeregisterDnsHostRecords, state S64");
            BVT_Test_LocateDc_DsrDeregisterDnsHostRecordsS72();
            this.Manager.EndTest();
        }
        #endregion
    }
}
