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
    public partial class Test_LocateDc_DsrGetSiteName : PtfTestClassBase {
        
        public Test_LocateDc_DsrGetSiteName() {
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
        public void Test_LocateDc_DsrGetSiteNameS0() {
            this.Manager.BeginTest("Test_LocateDc_DsrGetSiteNameS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp4 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS0GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS0GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS0GetPlatformChecker2)));
            if ((temp4 == 0)) {
                this.Manager.Comment("reaching state \'S14\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call DsrGetDcSiteCoverageW(PrimaryDc)\'");
                temp1 = this.INrpcServerAdapterInstance.DsrGetDcSiteCoverageW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103282");
                this.Manager.Comment("reaching state \'S35\'");
                this.Manager.Comment("checking step \'return DsrGetDcSiteCoverageW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of DsrGetDcSiteCoverageW, state S35");
                Test_LocateDc_DsrGetSiteNameS56();
                goto label0;
            }
            if ((temp4 == 1)) {
                this.Manager.Comment("reaching state \'S15\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call DsrGetDcSiteCoverageW(PrimaryDc)\'");
                temp2 = this.INrpcServerAdapterInstance.DsrGetDcSiteCoverageW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103282");
                this.Manager.Comment("reaching state \'S36\'");
                this.Manager.Comment("checking step \'return DsrGetDcSiteCoverageW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp2, "return of DsrGetDcSiteCoverageW, state S36");
                Test_LocateDc_DsrGetSiteNameS57();
                goto label0;
            }
            if ((temp4 == 2)) {
                this.Manager.Comment("reaching state \'S16\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call DsrGetDcSiteCoverageW(PrimaryDc)\'");
                temp3 = this.INrpcServerAdapterInstance.DsrGetDcSiteCoverageW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103282");
                this.Manager.Comment("reaching state \'S37\'");
                this.Manager.Comment("checking step \'return DsrGetDcSiteCoverageW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of DsrGetDcSiteCoverageW, state S37");
                Test_LocateDc_DsrGetSiteNameS58();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrGetSiteNameS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS56() {
            this.Manager.Comment("reaching state \'S56\'");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS57() {
            this.Manager.Comment("reaching state \'S57\'");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS58() {
            this.Manager.Comment("reaching state \'S58\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrGetSiteNameS10() {
            this.Manager.BeginTest("Test_LocateDc_DsrGetSiteNameS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp5;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp5);
            this.Manager.AddReturn(GetPlatformInfo, null, temp5);
            this.Manager.Comment("reaching state \'S11\'");
            int temp9 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS10GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS10GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS10GetPlatformChecker2)));
            if ((temp9 == 0)) {
                this.Manager.Comment("reaching state \'S29\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call DsrGetSiteName(PrimaryDc)\'");
                temp6 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103446");
                this.Manager.Comment("reaching state \'S50\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp6, "return of DsrGetSiteName, state S50");
                Test_LocateDc_DsrGetSiteNameS56();
                goto label1;
            }
            if ((temp9 == 1)) {
                this.Manager.Comment("reaching state \'S30\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp7 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S51\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp7, "return of DsrGetSiteName, state S51");
                this.Manager.Comment("reaching state \'S64\'");
                goto label1;
            }
            if ((temp9 == 2)) {
                this.Manager.Comment("reaching state \'S31\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp8 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S52\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp8, "return of DsrGetSiteName, state S52");
                this.Manager.Comment("reaching state \'S65\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrGetSiteNameS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrGetSiteNameS12() {
            this.Manager.BeginTest("Test_LocateDc_DsrGetSiteNameS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp10;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp10);
            this.Manager.AddReturn(GetPlatformInfo, null, temp10);
            this.Manager.Comment("reaching state \'S13\'");
            int temp14 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS12GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS12GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS12GetPlatformChecker2)));
            if ((temp14 == 0)) {
                this.Manager.Comment("reaching state \'S32\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call DsrGetDcSiteCoverageW(NonDcServer)\'");
                temp11 = this.INrpcServerAdapterInstance.DsrGetDcSiteCoverageW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103501");
                this.Manager.Checkpoint("MS-NRPC_R10250");
                this.Manager.Comment("reaching state \'S53\'");
                this.Manager.Comment("checking step \'return DsrGetDcSiteCoverageW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp11, "return of DsrGetDcSiteCoverageW, state S53");
                Test_LocateDc_DsrGetSiteNameS56();
                goto label2;
            }
            if ((temp14 == 1)) {
                this.Manager.Comment("reaching state \'S33\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp12 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S54\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp12, "return of DsrGetSiteName, state S54");
                this.Manager.Comment("reaching state \'S66\'");
                goto label2;
            }
            if ((temp14 == 2)) {
                this.Manager.Comment("reaching state \'S34\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp13 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S55\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp13, "return of DsrGetSiteName, state S55");
                this.Manager.Comment("reaching state \'S67\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrGetSiteNameS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrGetSiteNameS2() {
            this.Manager.BeginTest("Test_LocateDc_DsrGetSiteNameS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.AddReturn(GetPlatformInfo, null, temp15);
            this.Manager.Comment("reaching state \'S3\'");
            int temp19 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS2GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS2GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS2GetPlatformChecker2)));
            if ((temp19 == 0)) {
                this.Manager.Comment("reaching state \'S17\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp16 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S38\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp16, "return of DsrGetSiteName, state S38");
                Test_LocateDc_DsrGetSiteNameS58();
                goto label3;
            }
            if ((temp19 == 1)) {
                this.Manager.Comment("reaching state \'S18\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp17 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S39\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp17, "return of DsrGetSiteName, state S39");
                Test_LocateDc_DsrGetSiteNameS57();
                goto label3;
            }
            if ((temp19 == 2)) {
                this.Manager.Comment("reaching state \'S19\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp18 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S40\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp18, "return of DsrGetSiteName, state S40");
                this.Manager.Comment("reaching state \'S59\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrGetSiteNameS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrGetSiteNameS4() {
            this.Manager.BeginTest("Test_LocateDc_DsrGetSiteNameS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp20;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp20);
            this.Manager.AddReturn(GetPlatformInfo, null, temp20);
            this.Manager.Comment("reaching state \'S5\'");
            int temp24 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS4GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS4GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS4GetPlatformChecker2)));
            if ((temp24 == 0)) {
                this.Manager.Comment("reaching state \'S20\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                this.Manager.Comment("executing step \'call DsrGetSiteName(PrimaryDc)\'");
                temp21 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103446");
                this.Manager.Comment("reaching state \'S41\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp21, "return of DsrGetSiteName, state S41");
                Test_LocateDc_DsrGetSiteNameS58();
                goto label4;
            }
            if ((temp24 == 1)) {
                this.Manager.Comment("reaching state \'S21\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp22;
                this.Manager.Comment("executing step \'call DsrGetSiteName(PrimaryDc)\'");
                temp22 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103446");
                this.Manager.Comment("reaching state \'S42\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp22, "return of DsrGetSiteName, state S42");
                Test_LocateDc_DsrGetSiteNameS57();
                goto label4;
            }
            if ((temp24 == 2)) {
                this.Manager.Comment("reaching state \'S22\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp23 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S43\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp23, "return of DsrGetSiteName, state S43");
                this.Manager.Comment("reaching state \'S60\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrGetSiteNameS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrGetSiteNameS6() {
            this.Manager.BeginTest("Test_LocateDc_DsrGetSiteNameS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp25;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp25);
            this.Manager.AddReturn(GetPlatformInfo, null, temp25);
            this.Manager.Comment("reaching state \'S7\'");
            int temp29 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS6GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS6GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS6GetPlatformChecker2)));
            if ((temp29 == 0)) {
                this.Manager.Comment("reaching state \'S23\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp26;
                this.Manager.Comment("executing step \'call DsrGetDcSiteCoverageW(NonDcServer)\'");
                temp26 = this.INrpcServerAdapterInstance.DsrGetDcSiteCoverageW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103501");
                this.Manager.Checkpoint("MS-NRPC_R10250");
                this.Manager.Comment("reaching state \'S44\'");
                this.Manager.Comment("checking step \'return DsrGetDcSiteCoverageW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp26, "return of DsrGetDcSiteCoverageW, state S44");
                Test_LocateDc_DsrGetSiteNameS58();
                goto label5;
            }
            if ((temp29 == 1)) {
                this.Manager.Comment("reaching state \'S24\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call DsrGetDcSiteCoverageW(NonDcServer)\'");
                temp27 = this.INrpcServerAdapterInstance.DsrGetDcSiteCoverageW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103501");
                this.Manager.Checkpoint("MS-NRPC_R10250");
                this.Manager.Comment("reaching state \'S45\'");
                this.Manager.Comment("checking step \'return DsrGetDcSiteCoverageW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp27, "return of DsrGetDcSiteCoverageW, state S45");
                Test_LocateDc_DsrGetSiteNameS57();
                goto label5;
            }
            if ((temp29 == 2)) {
                this.Manager.Comment("reaching state \'S25\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp28 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S46\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp28, "return of DsrGetSiteName, state S46");
                this.Manager.Comment("reaching state \'S61\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrGetSiteNameS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrGetSiteNameS8() {
            this.Manager.BeginTest("Test_LocateDc_DsrGetSiteNameS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp30;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp30);
            this.Manager.AddReturn(GetPlatformInfo, null, temp30);
            this.Manager.Comment("reaching state \'S9\'");
            int temp34 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS8GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS8GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrGetSiteName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrGetSiteNameS8GetPlatformChecker2)));
            if ((temp34 == 0)) {
                this.Manager.Comment("reaching state \'S26\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp31 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S47\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp31, "return of DsrGetSiteName, state S47");
                Test_LocateDc_DsrGetSiteNameS56();
                goto label6;
            }
            if ((temp34 == 1)) {
                this.Manager.Comment("reaching state \'S27\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp32;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp32 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S48\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp32, "return of DsrGetSiteName, state S48");
                this.Manager.Comment("reaching state \'S62\'");
                goto label6;
            }
            if ((temp34 == 2)) {
                this.Manager.Comment("reaching state \'S28\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp33;
                this.Manager.Comment("executing step \'call DsrGetSiteName(NonDcServer)\'");
                temp33 = this.INrpcServerAdapterInstance.DsrGetSiteName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103275");
                this.Manager.Checkpoint("MS-NRPC_R103447");
                this.Manager.Comment("reaching state \'S49\'");
                this.Manager.Comment("checking step \'return DsrGetSiteName/ERROR_NO_SITENAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SITENAME, temp33, "return of DsrGetSiteName, state S49");
                this.Manager.Comment("reaching state \'S63\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrGetSiteNameS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_LocateDc_DsrGetSiteNameS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
    }
}
