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
    public partial class BVT_Test_LocateDc_NetrGetDCName : PtfTestClassBase
    {

        public BVT_Test_LocateDc_NetrGetDCName()
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
        public void NRPC_BVT_Test_LocateDc_NetrGetDCNameS0()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_NetrGetDCNameS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
            this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,TrustedDomainName)\'");
            temp1 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.TrustedDomainName);
            this.Manager.Checkpoint("MS-NRPC_R104832");
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of NetrGetAnyDCName, state S15");
            BVT_Test_LocateDc_NetrGetDCNameS20();
            this.Manager.EndTest();
        }

        private void BVT_Test_LocateDc_NetrGetDCNameS20()
        {
            this.Manager.Comment("reaching state \'S20\'");
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_NetrGetDCNameS2()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_NetrGetDCNameS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp2;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp2);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp2, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S11\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp3;
            this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,NetBiosFormatDomainName)\'");
            temp3 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NetBiosFormatDomainName);
            this.Manager.Checkpoint("MS-NRPC_R103436");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("checking step \'return NetrGetDCName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp3, "return of NetrGetDCName, state S16");
            BVT_Test_LocateDc_NetrGetDCNameS20();
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
        public void NRPC_BVT_Test_LocateDc_NetrGetDCNameS4()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_NetrGetDCNameS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp4;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp4);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp4, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call NetrGetAnyDCName(NonDcServer,NetBiosFormatDomainName)\'");
            temp5 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NetBiosFormatDomainName);
            this.Manager.Checkpoint("MS-NRPC_R104832");
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of NetrGetAnyDCName, state S17");
            BVT_Test_LocateDc_NetrGetDCNameS20();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_NetrGetDCNameS6()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_NetrGetDCNameS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp6;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp6);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp6, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S13\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp7;
            this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,NetBiosFormatDomainName)\'");
            temp7 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.NetBiosFormatDomainName);
            this.Manager.Checkpoint("MS-NRPC_R103260");
            this.Manager.Checkpoint("MS-NRPC_R614");
            this.Manager.Checkpoint("MS-NRPC_R104833");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp7, "return of NetrGetAnyDCName, state S18");
            BVT_Test_LocateDc_NetrGetDCNameS20();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_NetrGetDCNameS8()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_NetrGetDCNameS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp8;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp8);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp8, "sutPlatform of GetPlatform, state S9");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp9;
            this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,FqdnFormatDomainName)\'");
            temp9 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName);
            this.Manager.Checkpoint("MS-NRPC_R103260");
            this.Manager.Checkpoint("MS-NRPC_R614");
            this.Manager.Checkpoint("MS-NRPC_R104833");
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp9, "return of NetrGetAnyDCName, state S19");
            BVT_Test_LocateDc_NetrGetDCNameS20();
            this.Manager.EndTest();
        }
        #endregion
    }
}
