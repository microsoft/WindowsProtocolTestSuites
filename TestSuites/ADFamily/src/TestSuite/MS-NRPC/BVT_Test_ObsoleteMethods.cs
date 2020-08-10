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
    public partial class BVT_Test_ObsoleteMethods : PtfTestClassBase
    {

        public BVT_Test_ObsoleteMethods()
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
        public void NRPC_BVT_Test_ObsoleteMethodsS0()
        {
            this.Manager.BeginTest("BVT_Test_ObsoleteMethodsS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
            this.Manager.Comment("executing step \'call NetrLogonUasLogoff()\'");
            temp1 = this.INrpcServerAdapterInstance.NetrLogonUasLogoff();
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("checking step \'return NetrLogonUasLogoff/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of NetrLogonUasLogoff, state S6");
            BVT_Test_ObsoleteMethodsS8();
            this.Manager.EndTest();
        }

        private void BVT_Test_ObsoleteMethodsS8()
        {
            this.Manager.Comment("reaching state \'S8\'");
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_ObsoleteMethodsS2()
        {
            this.Manager.BeginTest("BVT_Test_ObsoleteMethodsS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp2;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp2);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp2, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S5\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp3;
            this.Manager.Comment("executing step \'call NetrLogonUasLogon()\'");
            temp3 = this.INrpcServerAdapterInstance.NetrLogonUasLogon();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return NetrLogonUasLogon/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp3, "return of NetrLogonUasLogon, state S7");
            BVT_Test_ObsoleteMethodsS8();
            this.Manager.EndTest();
        }
        #endregion
    }
}
