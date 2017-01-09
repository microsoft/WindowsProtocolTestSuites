// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Nrpc {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class Test_ObsoleteMethods : PtfTestClassBase {
        
        public Test_ObsoleteMethods() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }
        
        #region Expect Delegates
        public delegate void GetPlatformDelegate1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase GetPlatformInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter), "GetPlatform", typeof(Microsoft.Protocols.TestSuites.Nrpc.PlatformType).MakeByRefType());
        #endregion
        
        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter INrpcServerAdapterInstance;
        #endregion
        
        #region Class Initialization and Cleanup
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void ClassInitialize(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context) {
            PtfTestClassBase.Initialize(context);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void ClassCleanup() {
            PtfTestClassBase.Cleanup();
        }
        #endregion
        
        #region Test Initialization and Cleanup
        protected override void TestInitialize() {
            this.InitializeTestManager();
            this.INrpcServerAdapterInstance = ((Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter))));
        }
        
        protected override void TestCleanup() {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion
        
        #region Test Starting in S0
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_ObsoleteMethodsS0() {
            this.Manager.BeginTest("Test_ObsoleteMethodsS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp4 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_ObsoleteMethods.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_ObsoleteMethodsS0GetPlatformChecker)), new ExpectedReturn(Test_ObsoleteMethods.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_ObsoleteMethodsS0GetPlatformChecker1)), new ExpectedReturn(Test_ObsoleteMethods.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_ObsoleteMethodsS0GetPlatformChecker2)));
            if ((temp4 == 0)) {
                this.Manager.Comment("reaching state \'S6\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrLogonUasLogoff()\'");
                temp1 = this.INrpcServerAdapterInstance.NetrLogonUasLogoff();
                this.Manager.Comment("reaching state \'S15\'");
                this.Manager.Comment("checking step \'return NetrLogonUasLogoff/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of NetrLogonUasLogoff, state S15");
                Test_ObsoleteMethodsS24();
                goto label0;
            }
            if ((temp4 == 1)) {
                this.Manager.Comment("reaching state \'S7\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrLogonUasLogoff()\'");
                temp2 = this.INrpcServerAdapterInstance.NetrLogonUasLogoff();
                this.Manager.Comment("reaching state \'S16\'");
                this.Manager.Comment("checking step \'return NetrLogonUasLogoff/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp2, "return of NetrLogonUasLogoff, state S16");
                Test_ObsoleteMethodsS25();
                goto label0;
            }
            if ((temp4 == 2)) {
                this.Manager.Comment("reaching state \'S8\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrLogonUasLogoff()\'");
                temp3 = this.INrpcServerAdapterInstance.NetrLogonUasLogoff();
                this.Manager.Comment("reaching state \'S17\'");
                this.Manager.Comment("checking step \'return NetrLogonUasLogoff/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of NetrLogonUasLogoff, state S17");
                Test_ObsoleteMethodsS26();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_ObsoleteMethodsS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_ObsoleteMethodsS24() {
            this.Manager.Comment("reaching state \'S24\'");
        }
        
        private void Test_ObsoleteMethodsS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_ObsoleteMethodsS25() {
            this.Manager.Comment("reaching state \'S25\'");
        }
        
        private void Test_ObsoleteMethodsS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_ObsoleteMethodsS26() {
            this.Manager.Comment("reaching state \'S26\'");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_ObsoleteMethodsS2() {
            this.Manager.BeginTest("Test_ObsoleteMethodsS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp5;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp5);
            this.Manager.AddReturn(GetPlatformInfo, null, temp5);
            this.Manager.Comment("reaching state \'S3\'");
            int temp9 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_ObsoleteMethods.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_ObsoleteMethodsS2GetPlatformChecker)), new ExpectedReturn(Test_ObsoleteMethods.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_ObsoleteMethodsS2GetPlatformChecker1)), new ExpectedReturn(Test_ObsoleteMethods.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_ObsoleteMethodsS2GetPlatformChecker2)));
            if ((temp9 == 0)) {
                this.Manager.Comment("reaching state \'S10\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrLogonUasLogon()\'");
                temp6 = this.INrpcServerAdapterInstance.NetrLogonUasLogon();
                this.Manager.Comment("reaching state \'S19\'");
                this.Manager.Comment("checking step \'return NetrLogonUasLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp6, "return of NetrLogonUasLogon, state S19");
                Test_ObsoleteMethodsS25();
                goto label1;
            }
            if ((temp9 == 1)) {
                this.Manager.Comment("reaching state \'S11\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call NetrLogonUasLogon()\'");
                temp7 = this.INrpcServerAdapterInstance.NetrLogonUasLogon();
                this.Manager.Comment("reaching state \'S20\'");
                this.Manager.Comment("checking step \'return NetrLogonUasLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp7, "return of NetrLogonUasLogon, state S20");
                this.Manager.Comment("reaching state \'S27\'");
                goto label1;
            }
            if ((temp9 == 2)) {
                this.Manager.Comment("reaching state \'S9\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call NetrLogonUasLogon()\'");
                temp8 = this.INrpcServerAdapterInstance.NetrLogonUasLogon();
                this.Manager.Comment("reaching state \'S18\'");
                this.Manager.Comment("checking step \'return NetrLogonUasLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp8, "return of NetrLogonUasLogon, state S18");
                Test_ObsoleteMethodsS26();
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_ObsoleteMethodsS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_ObsoleteMethodsS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_ObsoleteMethodsS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_ObsoleteMethodsS4() {
            this.Manager.BeginTest("Test_ObsoleteMethodsS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp10;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp10);
            this.Manager.AddReturn(GetPlatformInfo, null, temp10);
            this.Manager.Comment("reaching state \'S5\'");
            int temp14 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_ObsoleteMethods.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_ObsoleteMethodsS4GetPlatformChecker)), new ExpectedReturn(Test_ObsoleteMethods.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_ObsoleteMethodsS4GetPlatformChecker1)), new ExpectedReturn(Test_ObsoleteMethods.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_ObsoleteMethodsS4GetPlatformChecker2)));
            if ((temp14 == 0)) {
                this.Manager.Comment("reaching state \'S12\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call NetrLogonUasLogon()\'");
                temp11 = this.INrpcServerAdapterInstance.NetrLogonUasLogon();
                this.Manager.Comment("reaching state \'S21\'");
                this.Manager.Comment("checking step \'return NetrLogonUasLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp11, "return of NetrLogonUasLogon, state S21");
                Test_ObsoleteMethodsS24();
                goto label2;
            }
            if ((temp14 == 1)) {
                this.Manager.Comment("reaching state \'S13\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call NetrLogonUasLogon()\'");
                temp12 = this.INrpcServerAdapterInstance.NetrLogonUasLogon();
                this.Manager.Comment("reaching state \'S22\'");
                this.Manager.Comment("checking step \'return NetrLogonUasLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp12, "return of NetrLogonUasLogon, state S22");
                this.Manager.Comment("reaching state \'S28\'");
                goto label2;
            }
            if ((temp14 == 2)) {
                this.Manager.Comment("reaching state \'S14\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call NetrLogonUasLogon()\'");
                temp13 = this.INrpcServerAdapterInstance.NetrLogonUasLogon();
                this.Manager.Comment("reaching state \'S23\'");
                this.Manager.Comment("checking step \'return NetrLogonUasLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp13, "return of NetrLogonUasLogon, state S23");
                this.Manager.Comment("reaching state \'S29\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_ObsoleteMethodsS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_ObsoleteMethodsS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_ObsoleteMethodsS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
    }
}
