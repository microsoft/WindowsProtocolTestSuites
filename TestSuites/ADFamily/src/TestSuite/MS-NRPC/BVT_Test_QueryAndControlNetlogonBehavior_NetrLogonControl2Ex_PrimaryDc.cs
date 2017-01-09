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
    public partial class BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDc : PtfTestClassBase
    {

        public BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDc()
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
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS0()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S18\'");
            bool temp1;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp1);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp1, "isAdministrator of GetClientAccountType, state S27");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
            this.Manager.Comment("executing step \'call NetrLogonControl2Ex(PrimaryDc,1,1,Valid)\'");
            temp2 = this.INrpcServerAdapterInstance.NetrLogonControl2Ex(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 1u, 1u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.NetlogonControlDataInformationType.Valid);
            this.Manager.Checkpoint("MS-NRPC_R103974");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return NetrLogonControl2Ex/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp2, "return of NetrLogonControl2Ex, state S45");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54();
            this.Manager.EndTest();
        }

        private void BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54()
        {
            this.Manager.Comment("reaching state \'S54\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS10()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp3;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp3);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp3, "sutPlatform of GetPlatform, state S11");
            this.Manager.Comment("reaching state \'S23\'");
            bool temp4;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp4);
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp4, "isAdministrator of GetClientAccountType, state S32");
            this.Manager.Comment("reaching state \'S41\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call NetrLogonControl2Ex(PrimaryDc,6,1,Valid)\'");
            temp5 = this.INrpcServerAdapterInstance.NetrLogonControl2Ex(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 6u, 1u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.NetlogonControlDataInformationType.Valid);
            this.Manager.Checkpoint("MS-NRPC_R103974");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("checking step \'return NetrLogonControl2Ex/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of NetrLogonControl2Ex, state S50");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS12()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp6;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp6);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp6, "sutPlatform of GetPlatform, state S13");
            this.Manager.Comment("reaching state \'S24\'");
            bool temp7;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp7);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp7, "isAdministrator of GetClientAccountType, state S33");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp8;
            this.Manager.Comment("executing step \'call NetrLogonControl2Ex(PrimaryDc,7,1,Valid)\'");
            temp8 = this.INrpcServerAdapterInstance.NetrLogonControl2Ex(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 7u, 1u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.NetlogonControlDataInformationType.Valid);
            this.Manager.Checkpoint("MS-NRPC_R103974");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return NetrLogonControl2Ex/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp8, "return of NetrLogonControl2Ex, state S51");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS14()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp9;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp9);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp9, "sutPlatform of GetPlatform, state S15");
            this.Manager.Comment("reaching state \'S25\'");
            bool temp10;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp10);
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp10, "isAdministrator of GetClientAccountType, state S34");
            this.Manager.Comment("reaching state \'S43\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp11;
            this.Manager.Comment("executing step \'call NetrLogonControl2Ex(PrimaryDc,11,1,Valid)\'");
            temp11 = this.INrpcServerAdapterInstance.NetrLogonControl2Ex(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 11u, 1u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.NetlogonControlDataInformationType.Valid);
            this.Manager.Checkpoint("MS-NRPC_R103974");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return NetrLogonControl2Ex/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp11, "return of NetrLogonControl2Ex, state S52");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54();
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
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS16()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp12;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp12);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp12, "sutPlatform of GetPlatform, state S17");
            this.Manager.Comment("reaching state \'S26\'");
            bool temp13;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp13);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp13, "isAdministrator of GetClientAccountType, state S35");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp14;
            this.Manager.Comment("executing step \'call NetrLogonControl2Ex(PrimaryDc,9,1,Valid)\'");
            temp14 = this.INrpcServerAdapterInstance.NetrLogonControl2Ex(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 9u, 1u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.NetlogonControlDataInformationType.Valid);
            this.Manager.Checkpoint("MS-NRPC_R103974");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return NetrLogonControl2Ex/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp14, "return of NetrLogonControl2Ex, state S53");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS2()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp15, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S19\'");
            bool temp16;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp16);
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp16, "isAdministrator of GetClientAccountType, state S28");
            this.Manager.Comment("reaching state \'S37\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp17;
            this.Manager.Comment("executing step \'call NetrLogonControl2Ex(PrimaryDc,65534,1,Valid)\'");
            temp17 = this.INrpcServerAdapterInstance.NetrLogonControl2Ex(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 65534u, 1u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.NetlogonControlDataInformationType.Valid);
            this.Manager.Checkpoint("MS-NRPC_R103974");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return NetrLogonControl2Ex/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp17, "return of NetrLogonControl2Ex, state S46");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS4()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp18;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp18);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp18, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S20\'");
            bool temp19;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp19);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp19, "isAdministrator of GetClientAccountType, state S29");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp20;
            this.Manager.Comment("executing step \'call NetrLogonControl2Ex(PrimaryDc,12,1,Valid)\'");
            temp20 = this.INrpcServerAdapterInstance.NetrLogonControl2Ex(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 12u, 1u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.NetlogonControlDataInformationType.Valid);
            this.Manager.Checkpoint("MS-NRPC_R103974");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return NetrLogonControl2Ex/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp20, "return of NetrLogonControl2Ex, state S47");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54();
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
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS6()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp21;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp21);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp21, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S21\'");
            bool temp22;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp22);
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp22, "isAdministrator of GetClientAccountType, state S30");
            this.Manager.Comment("reaching state \'S39\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp23;
            this.Manager.Comment("executing step \'call NetrLogonControl2Ex(PrimaryDc,5,1,Valid)\'");
            temp23 = this.INrpcServerAdapterInstance.NetrLogonControl2Ex(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 5u, 1u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.NetlogonControlDataInformationType.Valid);
            this.Manager.Checkpoint("MS-NRPC_R103974");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("checking step \'return NetrLogonControl2Ex/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp23, "return of NetrLogonControl2Ex, state S48");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS8()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp24;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp24);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp24, "sutPlatform of GetPlatform, state S9");
            this.Manager.Comment("reaching state \'S22\'");
            bool temp25;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp25);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp25, "isAdministrator of GetClientAccountType, state S31");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp26;
            this.Manager.Comment("executing step \'call NetrLogonControl2Ex(PrimaryDc,65533,1,Valid)\'");
            temp26 = this.INrpcServerAdapterInstance.NetrLogonControl2Ex(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 65533u, 1u, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.NetlogonControlDataInformationType.Valid);
            this.Manager.Checkpoint("MS-NRPC_R103974");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return NetrLogonControl2Ex/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp26, "return of NetrLogonControl2Ex, state S49");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl2Ex_PrimaryDcS54();
            this.Manager.EndTest();
        }
        #endregion
    }
}
