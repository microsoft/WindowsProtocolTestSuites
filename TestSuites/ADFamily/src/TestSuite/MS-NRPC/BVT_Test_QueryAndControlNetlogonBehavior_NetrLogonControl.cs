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
    public partial class BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl : PtfTestClassBase
    {

        public BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControl()
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
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S8\'");
            bool temp1;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp1);
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp1, "isAdministrator of GetClientAccountType, state S12");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
            this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,1)\'");
            temp2 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 1u, 1u);
            this.Manager.Checkpoint("MS-NRPC_R104215");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp2, "return of NetrLogonControl, state S20");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24();
            this.Manager.EndTest();
        }

        private void BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24()
        {
            this.Manager.Comment("reaching state \'S24\'");
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp3;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp3);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp3, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S9\'");
            bool temp4;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp4);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp4, "isAdministrator of GetClientAccountType, state S13");
            this.Manager.Comment("reaching state \'S17\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
            temp5 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
            this.Manager.Checkpoint("MS-NRPC_R104215");
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of NetrLogonControl, state S21");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp6;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp6);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp6, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S10\'");
            bool temp7;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp7);
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp7, "isAdministrator of GetClientAccountType, state S14");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp8;
            this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
            temp8 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.NonDcServer, 1u, 1u);
            this.Manager.Checkpoint("MS-NRPC_R104215");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp8, "return of NetrLogonControl, state S22");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6()
        {
            this.Manager.BeginTest("BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp9;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp9);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp9, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S11\'");
            bool temp10;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp10);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp10, "isAdministrator of GetClientAccountType, state S15");
            this.Manager.Comment("reaching state \'S19\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp11;
            this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65533,1)\'");
            temp11 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, 65533u, 1u);
            this.Manager.Checkpoint("MS-NRPC_R104215");
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp11, "return of NetrLogonControl, state S23");
            BVT_Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24();
            this.Manager.EndTest();
        }
        #endregion
    }
}
