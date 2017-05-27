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
    public partial class BVT_Test_RetrieveTrustInfo : PtfTestClassBase
    {

        public BVT_Test_RetrieveTrustInfo()
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
        public void NRPC_BVT_Test_RetrieveTrustInfoS0()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
            this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,1)\'" +
                    "");
            temp1 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.TrustedDomainName, 1u);
            this.Manager.Checkpoint("MS-NRPC_R103645");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of DsrGetForestTrustInformation, state S42");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }

        private void BVT_Test_RetrieveTrustInfoS56()
        {
            this.Manager.Comment("reaching state \'S56\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS10()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp2;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp2);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp2, "sutPlatform of GetPlatform, state S11");
            this.Manager.Comment("reaching state \'S33\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp3;
            this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,32)\'");
            temp3 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 32u);
            this.Manager.Checkpoint("MS-NRPC_R103574");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp3, "return of DsrEnumerateDomainTrusts, state S47");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS12()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp4;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp4);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp4, "sutPlatform of GetPlatform, state S13");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,8)\'");
            temp5 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 8u);
            this.Manager.Checkpoint("MS-NRPC_R103574");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of DsrEnumerateDomainTrusts, state S48");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS14()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp6;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp6);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp6, "sutPlatform of GetPlatform, state S15");
            this.Manager.Comment("reaching state \'S35\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp7;
            this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,16)\'");
            temp7 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 16u);
            this.Manager.Checkpoint("MS-NRPC_R103574");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp7, "return of DsrEnumerateDomainTrusts, state S49");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("SDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS16()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp8;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp8);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp8, "sutPlatform of GetPlatform, state S17");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp9;
            this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomainsEx(NonDcServer)\'");
            temp9 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomainsEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.NonDcServer);
            this.Manager.Checkpoint("MS-NRPC_R103592");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomainsEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp9, "return of NetrEnumerateTrustedDomainsEx, state S50");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS18()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp10;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp10);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp10, "sutPlatform of GetPlatform, state S19");
            this.Manager.Comment("reaching state \'S37\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp11;
            this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomainsEx(PrimaryDc)\'");
            temp11 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomainsEx(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103592");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomainsEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp11, "return of NetrEnumerateTrustedDomainsEx, state S51");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS2()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp12;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp12);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp12, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S29\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp13;
            this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
            temp13 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 1u);
            this.Manager.Checkpoint("MS-NRPC_R103574");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp13, "return of DsrEnumerateDomainTrusts, state S43");
            BVT_Test_RetrieveTrustInfoS56();
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
        public void NRPC_BVT_Test_RetrieveTrustInfoS20()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp14;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp14);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp14, "sutPlatform of GetPlatform, state S21");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp15;
            this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomains(NonDcServer)\'");
            temp15 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomains(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.NonDcServer);
            this.Manager.Checkpoint("MS-NRPC_R103599");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomains/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp15, "return of NetrEnumerateTrustedDomains, state S52");
            BVT_Test_RetrieveTrustInfoS56();
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
        public void NRPC_BVT_Test_RetrieveTrustInfoS22()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp16;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp16);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp16, "sutPlatform of GetPlatform, state S23");
            this.Manager.Comment("reaching state \'S39\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp17;
            this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomains(PrimaryDc)\'");
            temp17 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomains(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103599");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomains/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp17, "return of NetrEnumerateTrustedDomains, state S53");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS24()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp18;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp18);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp18, "sutPlatform of GetPlatform, state S25");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp19;
            this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,Null,0)\'");
            temp19 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType)(0)), 0u);
            this.Manager.Checkpoint("MS-NRPC_R103645");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp19, "return of DsrGetForestTrustInformation, state S54");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS26()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp20;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp20);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp20, "sutPlatform of GetPlatform, state S27");
            this.Manager.Comment("reaching state \'S41\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp21;
            this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,0)\'" +
                    "");
            temp21 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.TrustedDomainName, 0u);
            this.Manager.Checkpoint("MS-NRPC_R103645");
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp21, "return of DsrGetForestTrustInformation, state S55");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS4()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp22;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp22);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp22, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp23;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp23 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp23, "return of NetrServerReqChallenge, state S44");
            this.Manager.Comment("reaching state \'S57\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp24;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp24 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp24, "return of NetrServerAuthenticate3, state S58");
            this.Manager.Comment("reaching state \'S59\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp25;
            this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(PrimaryDc,DomainMemberComputerAccount" +
                    ",WorkstationSecureChannel,Client,True)\'");
            temp25 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true);
            this.Manager.Checkpoint("MS-NRPC_R103676");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp25, "return of NetrServerGetTrustInfo, state S60");
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS6()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp26;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp26);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp26, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S31\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp27;
            this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,2)\'");
            temp27 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 2u);
            this.Manager.Checkpoint("MS-NRPC_R103574");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp27, "return of DsrEnumerateDomainTrusts, state S45");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_RetrieveTrustInfoS8()
        {
            this.Manager.BeginTest("BVT_Test_RetrieveTrustInfoS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp28;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp28);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp28, "sutPlatform of GetPlatform, state S9");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp29;
            this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,4)\'");
            temp29 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 4u);
            this.Manager.Checkpoint("MS-NRPC_R103574");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp29, "return of DsrEnumerateDomainTrusts, state S46");
            BVT_Test_RetrieveTrustInfoS56();
            this.Manager.EndTest();
        }
        #endregion
    }
}
