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
    public partial class BVT_Test_LocateDc_DsrGetDcName : PtfTestClassBase
    {

        public BVT_Test_LocateDc_DsrGetDcName()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter INrpcServerAdapterInstance;
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
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS0()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,NonNull,0)\'" +
                    "");
            temp1 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(1)), 0u);
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of DsrGetDcName, state S156");
            this.Manager.Comment("reaching state \'S208\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,0)\'");
            temp2 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 0u);
            this.Manager.Checkpoint("MS-NRPC_R104876");
            this.Manager.Comment("reaching state \'S237\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp2, "return of DsrGetDcName, state S237");
            this.Manager.Comment("reaching state \'S239\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S10
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS10()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp3;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp3);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp3, "sutPlatform of GetPlatform, state S11");
            this.Manager.Comment("reaching state \'S109\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp4;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,TrustedDomainName,Null,Null,0)\'");
            temp4 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Comment("reaching state \'S161\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp4, "return of DsrGetDcNameEx, state S161");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }

        private void BVT_Test_LocateDc_DsrGetDcNameS212()
        {
            this.Manager.Comment("reaching state \'S212\'");
        }
        #endregion

        #region Test Starting in S100
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS100()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS100");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp5;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp5);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp5, "sutPlatform of GetPlatform, state S101");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp6;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,257)\'");
            temp6 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 257u);
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp6, "return of DsrGetDcName, state S206");
            this.Manager.Comment("reaching state \'S235\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S102
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS102()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS102");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp7;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp7);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp7, "sutPlatform of GetPlatform, state S103");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp8;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,131072" +
                    ")\'");
            temp8 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 131072u);
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp8, "return of DsrGetDcName, state S207");
            this.Manager.Comment("reaching state \'S236\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS12()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp9;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp9);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp9, "sutPlatform of GetPlatform, state S13");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp10;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,PrimaryDomainG" +
                    "uid,Null,0)\'");
            temp10 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp10, "return of DsrGetDcNameEx, state S162");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS14()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp11;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp11);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp11, "sutPlatform of GetPlatform, state S15");
            this.Manager.Comment("reaching state \'S111\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp12;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,SiteNameO" +
                    "ne,0)\'");
            temp12 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType.SiteNameOne, 0u);
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp12, "return of DsrGetDcNameEx, state S163");
            BVT_Test_LocateDc_DsrGetDcNameS212();
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
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS16()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp13;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp13);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp13, "sutPlatform of GetPlatform, state S17");
            this.Manager.Comment("reaching state \'S112\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp14;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,TrustedDomainName,Null,Null,64)\'");
            temp14 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 64u);
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp14, "return of DsrGetDcNameEx, state S164");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS18()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp15, "sutPlatform of GetPlatform, state S19");
            this.Manager.Comment("reaching state \'S113\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp16;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,InvalidDomainG" +
                    "uid,Null,0)\'");
            temp16 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp16, "return of DsrGetDcNameEx, state S165");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS2()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp17;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp17);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp17, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S105\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp18;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx2(PrimaryDc,DomainMemberComputerAccount,4096,F" +
                    "qdnFormatDomainName,Null,Null,0)\'");
            temp18 = this.INrpcServerAdapterInstance.DsrGetDcNameEx2(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, 4096u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Checkpoint("MS-NRPC_R103119");
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx2/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp18, "return of DsrGetDcNameEx2, state S157");
            this.Manager.Comment("reaching state \'S209\'");
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
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS20()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp19;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp19);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp19, "sutPlatform of GetPlatform, state S21");
            this.Manager.Comment("reaching state \'S114\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp20;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,TrustedDomainG" +
                    "uid,Null,0)\'");
            temp20 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp20, "return of DsrGetDcNameEx, state S166");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS22()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp21;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp21);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp21, "sutPlatform of GetPlatform, state S23");
            this.Manager.Comment("reaching state \'S115\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp22;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,0)\'");
            temp22 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp22, "return of DsrGetDcNameEx, state S167");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS24()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp23;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp23);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp23, "sutPlatform of GetPlatform, state S25");
            this.Manager.Comment("reaching state \'S116\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp24;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,64)\'" +
                    "");
            temp24 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 64u);
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp24, "return of DsrGetDcNameEx, state S168");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS26()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp25;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp25);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp25, "sutPlatform of GetPlatform, state S27");
            this.Manager.Comment("reaching state \'S117\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp26;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,256)" +
                    "\'");
            temp26 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 256u);
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp26, "return of DsrGetDcNameEx, state S169");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS28()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp27;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp27);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp27, "sutPlatform of GetPlatform, state S29");
            this.Manager.Comment("reaching state \'S118\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp28;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,512)" +
                    "\'");
            temp28 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 512u);
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp28, "return of DsrGetDcNameEx, state S170");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS30()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp29;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp29);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp29, "sutPlatform of GetPlatform, state S31");
            this.Manager.Comment("reaching state \'S119\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp30;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,2621" +
                    "44)\'");
            temp30 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 262144u);
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp30, "return of DsrGetDcNameEx, state S171");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS32()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp31;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp31);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp31, "sutPlatform of GetPlatform, state S33");
            this.Manager.Comment("reaching state \'S120\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp32;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,8192" +
                    ")\'");
            temp32 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 8192u);
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp32, "return of DsrGetDcNameEx, state S172");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS34()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp33;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp33);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp33, "sutPlatform of GetPlatform, state S35");
            this.Manager.Comment("reaching state \'S121\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp34;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,32)\'" +
                    "");
            temp34 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 32u);
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp34, "return of DsrGetDcNameEx, state S173");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS36()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp35;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp35);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp35, "sutPlatform of GetPlatform, state S37");
            this.Manager.Comment("reaching state \'S122\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp36;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,128)" +
                    "\'");
            temp36 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 128u);
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp36, "return of DsrGetDcNameEx, state S174");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS38()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp37;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp37);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp37, "sutPlatform of GetPlatform, state S39");
            this.Manager.Comment("reaching state \'S123\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp38;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,2048" +
                    ")\'");
            temp38 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 2048u);
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp38, "return of DsrGetDcNameEx, state S175");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS4()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp39;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp39);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp39, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp40;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx2(PrimaryDc,NormalDomainUserAccount,512,FqdnFo" +
                    "rmatDomainName,Null,Null,0)\'");
            temp40 = this.INrpcServerAdapterInstance.DsrGetDcNameEx2(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.NormalDomainUserAccount, 512u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Checkpoint("MS-NRPC_R103119");
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx2/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp40, "return of DsrGetDcNameEx2, state S158");
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S40
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS40()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp41;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp41);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp41, "sutPlatform of GetPlatform, state S41");
            this.Manager.Comment("reaching state \'S124\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp42;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,5242" +
                    "88)\'");
            temp42 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 524288u);
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp42, "return of DsrGetDcNameEx, state S176");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S42
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS42()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp43;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp43);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp43, "sutPlatform of GetPlatform, state S43");
            this.Manager.Comment("reaching state \'S125\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp44;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,16)\'" +
                    "");
            temp44 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 16u);
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp44, "return of DsrGetDcNameEx, state S177");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S44
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS44()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp45;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp45);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp45, "sutPlatform of GetPlatform, state S45");
            this.Manager.Comment("reaching state \'S126\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp46;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,1)\'");
            temp46 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 1u);
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp46, "return of DsrGetDcNameEx, state S178");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S46
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS46()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp47;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp47);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp47, "sutPlatform of GetPlatform, state S47");
            this.Manager.Comment("reaching state \'S127\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp48;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,1024" +
                    ")\'");
            temp48 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 1024u);
            this.Manager.Comment("reaching state \'S179\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp48, "return of DsrGetDcNameEx, state S179");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S48
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS48()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp49;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp49);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp49, "sutPlatform of GetPlatform, state S49");
            this.Manager.Comment("reaching state \'S128\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp50;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,4096" +
                    ")\'");
            temp50 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 4096u);
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp50, "return of DsrGetDcNameEx, state S180");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S50
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS50()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp51;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp51);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp51, "sutPlatform of GetPlatform, state S51");
            this.Manager.Comment("reaching state \'S129\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp52;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,2147" +
                    "483648)\'");
            temp52 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 2147483648u);
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp52, "return of DsrGetDcNameEx, state S181");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S52
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS52()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp53;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp53);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp53, "sutPlatform of GetPlatform, state S53");
            this.Manager.Comment("reaching state \'S130\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp54;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,257)" +
                    "\'");
            temp54 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 257u);
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp54, "return of DsrGetDcNameEx, state S182");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S54
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS54()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp55;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp55);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp55, "sutPlatform of GetPlatform, state S55");
            this.Manager.Comment("reaching state \'S131\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp56;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(PrimaryDc,FqdnFormatDomainName,Null,Null,1310" +
                    "72)\'");
            temp56 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 131072u);
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp56, "return of DsrGetDcNameEx, state S183");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S56
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS56()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp57;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp57);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp57, "sutPlatform of GetPlatform, state S57");
            this.Manager.Comment("reaching state \'S132\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp58;
            this.Manager.Comment("executing step \'call DsrGetDcName(NonDcServer,FqdnFormatDomainName,PrimaryDomainG" +
                    "uid,Null,0)\'");
            temp58 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 0x80u);
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp58, "return of DsrGetDcName, state S184");
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S58
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS58()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp59;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp59);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp59, "sutPlatform of GetPlatform, state S59");
            this.Manager.Comment("reaching state \'S133\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp60;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,TrustedDomainName,Null,Null,0)\'");
            temp60 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 0u);
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp60, "return of DsrGetDcName, state S185");
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS6()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp61;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp61);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp61, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S107\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp62;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx2(PrimaryDc,Null,2147483648,FqdnFormatDomainNa" +
                    "me,Null,Null,0)\'");
            temp62 = this.INrpcServerAdapterInstance.DsrGetDcNameEx2(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType)(0)), 2147483648u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Checkpoint("MS-NRPC_R103119");
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx2/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp62, "return of DsrGetDcNameEx2, state S159");
            this.Manager.Comment("reaching state \'S211\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp63;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx2(PrimaryDc,Null,0,FqdnFormatDomainName,Null,N" +
                    "ull,0)\'");
            temp63 = this.INrpcServerAdapterInstance.DsrGetDcNameEx2(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType)(0)), 0u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Checkpoint("MS-NRPC_R103119");
            this.Manager.Checkpoint("MS-NRPC_R103114");
            this.Manager.Comment("reaching state \'S238\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx2/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp63, "return of DsrGetDcNameEx2, state S238");
            this.Manager.Comment("reaching state \'S240\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S60
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS60()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp64;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp64);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp64, "sutPlatform of GetPlatform, state S61");
            this.Manager.Comment("reaching state \'S134\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp65;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,PrimaryDomainGui" +
                    "d,Null,0)\'");
            temp65 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 0u);
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp65, "return of DsrGetDcName, state S186");
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S62
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS62()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp66;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp66);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp66, "sutPlatform of GetPlatform, state S63");
            this.Manager.Comment("reaching state \'S135\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp67;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,NonNull,0)\'" +
                    "");
            temp67 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(1)), 0u);
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp67, "return of DsrGetDcName, state S187");
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S64
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS64()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS64");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp68;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp68);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp68, "sutPlatform of GetPlatform, state S65");
            this.Manager.Comment("reaching state \'S136\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp69;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,TrustedDomainName,Null,Null,64)\'");
            temp69 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 64u);
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp69, "return of DsrGetDcName, state S188");
            this.Manager.Comment("reaching state \'S217\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S66
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS66()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS66");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp70;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp70);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp70, "sutPlatform of GetPlatform, state S67");
            this.Manager.Comment("reaching state \'S137\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp71;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,InvalidDomainGui" +
                    "d,Null,0)\'");
            temp71 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 0u);
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp71, "return of DsrGetDcName, state S189");
            this.Manager.Comment("reaching state \'S218\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S68
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS68()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS68");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp72;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp72);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp72, "sutPlatform of GetPlatform, state S69");
            this.Manager.Comment("reaching state \'S138\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp73;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,TrustedDomainGui" +
                    "d,Null,0)\'");
            temp73 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 0u);
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp73, "return of DsrGetDcName, state S190");
            this.Manager.Comment("reaching state \'S219\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S70
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS70()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS70");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp74;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp74);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp74, "sutPlatform of GetPlatform, state S71");
            this.Manager.Comment("reaching state \'S139\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp75;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,0)\'");
            temp75 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 0u);
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp75, "return of DsrGetDcName, state S191");
            this.Manager.Comment("reaching state \'S220\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S72
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS72()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS72");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp76;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp76);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp76, "sutPlatform of GetPlatform, state S73");
            this.Manager.Comment("reaching state \'S140\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp77;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,64)\'");
            temp77 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 64u);
            this.Manager.Comment("reaching state \'S192\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp77, "return of DsrGetDcName, state S192");
            this.Manager.Comment("reaching state \'S221\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S74
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS74()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS74");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp78;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp78);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp78, "sutPlatform of GetPlatform, state S75");
            this.Manager.Comment("reaching state \'S141\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp79;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,256)\'");
            temp79 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 256u);
            this.Manager.Comment("reaching state \'S193\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp79, "return of DsrGetDcName, state S193");
            this.Manager.Comment("reaching state \'S222\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S76
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS76()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS76");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp80;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp80);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp80, "sutPlatform of GetPlatform, state S77");
            this.Manager.Comment("reaching state \'S142\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp81;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,512)\'");
            temp81 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 512u);
            this.Manager.Comment("reaching state \'S194\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp81, "return of DsrGetDcName, state S194");
            this.Manager.Comment("reaching state \'S223\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S78
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS78()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS78");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp82;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp82);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp82, "sutPlatform of GetPlatform, state S79");
            this.Manager.Comment("reaching state \'S143\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp83;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,262144" +
                    ")\'");
            temp83 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 262144u);
            this.Manager.Comment("reaching state \'S195\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp83, "return of DsrGetDcName, state S195");
            this.Manager.Comment("reaching state \'S224\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("SDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS8()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp84;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp84);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp84, "sutPlatform of GetPlatform, state S9");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp85;
            this.Manager.Comment("executing step \'call DsrGetDcNameEx(NonDcServer,FqdnFormatDomainName,PrimaryDomai" +
                    "nGuid,Null,0)\'");
            temp85 = this.INrpcServerAdapterInstance.DsrGetDcNameEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteNameType)(0)), 0u);
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("checking step \'return DsrGetDcNameEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp85, "return of DsrGetDcNameEx, state S160");
            BVT_Test_LocateDc_DsrGetDcNameS212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S80
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS80()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS80");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp86;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp86);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp86, "sutPlatform of GetPlatform, state S81");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp87;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,8192)\'" +
                    "");
            temp87 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 8192u);
            this.Manager.Comment("reaching state \'S196\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp87, "return of DsrGetDcName, state S196");
            this.Manager.Comment("reaching state \'S225\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S82
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS82()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS82");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp88;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp88);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp88, "sutPlatform of GetPlatform, state S83");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp89;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,32)\'");
            temp89 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 32u);
            this.Manager.Comment("reaching state \'S197\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp89, "return of DsrGetDcName, state S197");
            this.Manager.Comment("reaching state \'S226\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S84
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS84()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS84");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp90;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp90);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp90, "sutPlatform of GetPlatform, state S85");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp91;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,128)\'");
            temp91 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 128u);
            this.Manager.Comment("reaching state \'S198\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp91, "return of DsrGetDcName, state S198");
            this.Manager.Comment("reaching state \'S227\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S86
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS86()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS86");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp92;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp92);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp92, "sutPlatform of GetPlatform, state S87");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp93;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,2048)\'" +
                    "");
            temp93 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 2048u);
            this.Manager.Comment("reaching state \'S199\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp93, "return of DsrGetDcName, state S199");
            this.Manager.Comment("reaching state \'S228\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S88
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS88()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS88");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp94;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp94);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp94, "sutPlatform of GetPlatform, state S89");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp95;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,524288" +
                    ")\'");
            temp95 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 524288u);
            this.Manager.Comment("reaching state \'S200\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp95, "return of DsrGetDcName, state S200");
            this.Manager.Comment("reaching state \'S229\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S90
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS90()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS90");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp96;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp96);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp96, "sutPlatform of GetPlatform, state S91");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp97;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,16)\'");
            temp97 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 16u);
            this.Manager.Comment("reaching state \'S201\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp97, "return of DsrGetDcName, state S201");
            this.Manager.Comment("reaching state \'S230\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S92
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS92()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS92");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp98;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp98);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp98, "sutPlatform of GetPlatform, state S93");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp99;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,1)\'");
            temp99 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 1u);
            this.Manager.Comment("reaching state \'S202\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp99, "return of DsrGetDcName, state S202");
            this.Manager.Comment("reaching state \'S231\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S94
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS94()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS94");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp100;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp100);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp100, "sutPlatform of GetPlatform, state S95");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp101;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,1024)\'" +
                    "");
            temp101 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 1024u);
            this.Manager.Comment("reaching state \'S203\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp101, "return of DsrGetDcName, state S203");
            this.Manager.Comment("reaching state \'S232\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S96
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS96()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS96");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp102;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp102);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp102, "sutPlatform of GetPlatform, state S97");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp103;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,4096)\'" +
                    "");
            temp103 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 4096u);
            this.Manager.Comment("reaching state \'S204\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp103, "return of DsrGetDcName, state S204");
            this.Manager.Comment("reaching state \'S233\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S98
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrGetDcNameS98()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrGetDcNameS98");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp104;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp104);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp104, "sutPlatform of GetPlatform, state S99");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp105;
            this.Manager.Comment("executing step \'call DsrGetDcName(PrimaryDc,FqdnFormatDomainName,Null,Null,214748" +
                    "3648)\'");
            temp105 = this.INrpcServerAdapterInstance.DsrGetDcName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SiteGuidType)(0)), 2147483648u);
            this.Manager.Comment("reaching state \'S205\'");
            this.Manager.Comment("checking step \'return DsrGetDcName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp105, "return of DsrGetDcName, state S205");
            this.Manager.Comment("reaching state \'S234\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
