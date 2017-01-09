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
    public partial class Test_LocateDc_NetrGetDCName : PtfTestClassBase {
        
        public Test_LocateDc_NetrGetDCName() {
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
        public void Test_LocateDc_NetrGetDCNameS0() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp4 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS0GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS0GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS0GetPlatformChecker2)));
            if ((temp4 == 0)) {
                this.Manager.Comment("reaching state \'S54\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,InvalidDomainName)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S135\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp1, "return of NetrGetAnyDCName, state S135");
                Test_LocateDc_NetrGetDCNameS216();
                goto label0;
            }
            if ((temp4 == 1)) {
                this.Manager.Comment("reaching state \'S55\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,InvalidDomainName)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S136\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp2, "return of NetrGetAnyDCName, state S136");
                Test_LocateDc_NetrGetDCNameS217();
                goto label0;
            }
            if ((temp4 == 2)) {
                this.Manager.Comment("reaching state \'S56\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,InvalidDomainName)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S137\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp3, "return of NetrGetAnyDCName, state S137");
                Test_LocateDc_NetrGetDCNameS218();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_NetrGetDCNameS216() {
            this.Manager.Comment("reaching state \'S216\'");
        }
        
        private void Test_LocateDc_NetrGetDCNameS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_NetrGetDCNameS217() {
            this.Manager.Comment("reaching state \'S217\'");
        }
        
        private void Test_LocateDc_NetrGetDCNameS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_NetrGetDCNameS218() {
            this.Manager.Comment("reaching state \'S218\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS10() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp5;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp5);
            this.Manager.AddReturn(GetPlatformInfo, null, temp5);
            this.Manager.Comment("reaching state \'S11\'");
            int temp9 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS10GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS10GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS10GetPlatformChecker2)));
            if ((temp9 == 0)) {
                this.Manager.Comment("reaching state \'S69\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,Null)\'");
                temp6 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)));
                this.Manager.Checkpoint("MS-NRPC_R103436");
                this.Manager.Comment("reaching state \'S150\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp6, "return of NetrGetDCName, state S150");
                Test_LocateDc_NetrGetDCNameS218();
                goto label1;
            }
            if ((temp9 == 1)) {
                this.Manager.Comment("reaching state \'S70\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,Null)\'");
                temp7 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)));
                this.Manager.Checkpoint("MS-NRPC_R103436");
                this.Manager.Comment("reaching state \'S151\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp7, "return of NetrGetDCName, state S151");
                Test_LocateDc_NetrGetDCNameS217();
                goto label1;
            }
            if ((temp9 == 2)) {
                this.Manager.Comment("reaching state \'S71\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp8 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S152\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp8, "return of NetrGetDCName, state S152");
                this.Manager.Comment("reaching state \'S223\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_LocateDc_NetrGetDCNameS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_LocateDc_NetrGetDCNameS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS12() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp10;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp10);
            this.Manager.AddReturn(GetPlatformInfo, null, temp10);
            this.Manager.Comment("reaching state \'S13\'");
            int temp14 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS12GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS12GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS12GetPlatformChecker2)));
            if ((temp14 == 0)) {
                this.Manager.Comment("reaching state \'S72\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,TrustedDomainName)\'");
                temp11 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S153\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp11, "return of NetrGetDCName, state S153");
                Test_LocateDc_NetrGetDCNameS218();
                goto label2;
            }
            if ((temp14 == 1)) {
                this.Manager.Comment("reaching state \'S73\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,TrustedDomainName)\'");
                temp12 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S154\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp12, "return of NetrGetDCName, state S154");
                Test_LocateDc_NetrGetDCNameS217();
                goto label2;
            }
            if ((temp14 == 2)) {
                this.Manager.Comment("reaching state \'S74\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp13 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp13, "return of NetrGetDCName, state S155");
                this.Manager.Comment("reaching state \'S224\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_LocateDc_NetrGetDCNameS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_LocateDc_NetrGetDCNameS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS14() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.AddReturn(GetPlatformInfo, null, temp15);
            this.Manager.Comment("reaching state \'S15\'");
            int temp19 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS14GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS14GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS14GetPlatformChecker2)));
            if ((temp19 == 0)) {
                this.Manager.Comment("reaching state \'S75\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(NonDcServer,NetBiosFormatDomainName)\'");
                temp16 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R104832");
                this.Manager.Comment("reaching state \'S156\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp16, "return of NetrGetAnyDCName, state S156");
                Test_LocateDc_NetrGetDCNameS218();
                goto label3;
            }
            if ((temp19 == 1)) {
                this.Manager.Comment("reaching state \'S76\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(NonDcServer,NetBiosFormatDomainName)\'");
                temp17 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R104832");
                this.Manager.Comment("reaching state \'S157\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp17, "return of NetrGetAnyDCName, state S157");
                Test_LocateDc_NetrGetDCNameS217();
                goto label3;
            }
            if ((temp19 == 2)) {
                this.Manager.Comment("reaching state \'S77\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp18 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp18, "return of NetrGetDCName, state S158");
                this.Manager.Comment("reaching state \'S225\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_LocateDc_NetrGetDCNameS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_LocateDc_NetrGetDCNameS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS16() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp20;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp20);
            this.Manager.AddReturn(GetPlatformInfo, null, temp20);
            this.Manager.Comment("reaching state \'S17\'");
            int temp24 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS16GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS16GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS16GetPlatformChecker2)));
            if ((temp24 == 0)) {
                this.Manager.Comment("reaching state \'S78\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,TrustedDomainName)\'");
                temp21 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName);
                this.Manager.Checkpoint("MS-NRPC_R104832");
                this.Manager.Comment("reaching state \'S159\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp21, "return of NetrGetAnyDCName, state S159");
                Test_LocateDc_NetrGetDCNameS218();
                goto label4;
            }
            if ((temp24 == 1)) {
                this.Manager.Comment("reaching state \'S79\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp22;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,TrustedDomainName)\'");
                temp22 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName);
                this.Manager.Checkpoint("MS-NRPC_R104832");
                this.Manager.Comment("reaching state \'S160\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp22, "return of NetrGetAnyDCName, state S160");
                Test_LocateDc_NetrGetDCNameS217();
                goto label4;
            }
            if ((temp24 == 2)) {
                this.Manager.Comment("reaching state \'S80\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp23 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S161\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp23, "return of NetrGetDCName, state S161");
                this.Manager.Comment("reaching state \'S226\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_LocateDc_NetrGetDCNameS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_LocateDc_NetrGetDCNameS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS18() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp25;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp25);
            this.Manager.AddReturn(GetPlatformInfo, null, temp25);
            this.Manager.Comment("reaching state \'S19\'");
            int temp29 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS18GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS18GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS18GetPlatformChecker2)));
            if ((temp29 == 0)) {
                this.Manager.Comment("reaching state \'S81\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp26;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,Null)\'");
                temp26 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)));
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S162\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp26, "return of NetrGetAnyDCName, state S162");
                Test_LocateDc_NetrGetDCNameS218();
                goto label5;
            }
            if ((temp29 == 1)) {
                this.Manager.Comment("reaching state \'S82\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,Null)\'");
                temp27 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)));
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S163\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp27, "return of NetrGetAnyDCName, state S163");
                Test_LocateDc_NetrGetDCNameS217();
                goto label5;
            }
            if ((temp29 == 2)) {
                this.Manager.Comment("reaching state \'S83\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp28 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S164\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp28, "return of NetrGetDCName, state S164");
                this.Manager.Comment("reaching state \'S227\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_LocateDc_NetrGetDCNameS18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_LocateDc_NetrGetDCNameS18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS2() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp30;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp30);
            this.Manager.AddReturn(GetPlatformInfo, null, temp30);
            this.Manager.Comment("reaching state \'S3\'");
            int temp34 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS2GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS2GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS2GetPlatformChecker2)));
            if ((temp34 == 0)) {
                this.Manager.Comment("reaching state \'S57\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp31 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S138\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp31, "return of NetrGetDCName, state S138");
                Test_LocateDc_NetrGetDCNameS218();
                goto label6;
            }
            if ((temp34 == 1)) {
                this.Manager.Comment("reaching state \'S58\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp32;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp32 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp32, "return of NetrGetDCName, state S139");
                Test_LocateDc_NetrGetDCNameS217();
                goto label6;
            }
            if ((temp34 == 2)) {
                this.Manager.Comment("reaching state \'S59\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp33;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp33 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp33, "return of NetrGetDCName, state S140");
                this.Manager.Comment("reaching state \'S219\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_LocateDc_NetrGetDCNameS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_LocateDc_NetrGetDCNameS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS20() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp35;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp35);
            this.Manager.AddReturn(GetPlatformInfo, null, temp35);
            this.Manager.Comment("reaching state \'S21\'");
            int temp39 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS20GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS20GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS20GetPlatformChecker2)));
            if ((temp39 == 0)) {
                this.Manager.Comment("reaching state \'S84\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp36;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,EmptyDomainName)\'");
                temp36 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S165\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp36, "return of NetrGetAnyDCName, state S165");
                Test_LocateDc_NetrGetDCNameS218();
                goto label7;
            }
            if ((temp39 == 1)) {
                this.Manager.Comment("reaching state \'S85\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,EmptyDomainName)\'");
                temp37 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S166\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp37, "return of NetrGetAnyDCName, state S166");
                Test_LocateDc_NetrGetDCNameS217();
                goto label7;
            }
            if ((temp39 == 2)) {
                this.Manager.Comment("reaching state \'S86\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp38 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S167\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp38, "return of NetrGetDCName, state S167");
                this.Manager.Comment("reaching state \'S228\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_LocateDc_NetrGetDCNameS20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_LocateDc_NetrGetDCNameS20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS22() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp40;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp40);
            this.Manager.AddReturn(GetPlatformInfo, null, temp40);
            this.Manager.Comment("reaching state \'S23\'");
            int temp44 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS22GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS22GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS22GetPlatformChecker2)));
            if ((temp44 == 0)) {
                this.Manager.Comment("reaching state \'S87\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,NetBiosFormatDomainName)\'");
                temp41 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S168\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp41, "return of NetrGetAnyDCName, state S168");
                Test_LocateDc_NetrGetDCNameS218();
                goto label8;
            }
            if ((temp44 == 1)) {
                this.Manager.Comment("reaching state \'S88\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,NetBiosFormatDomainName)\'");
                temp42 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S169\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp42, "return of NetrGetAnyDCName, state S169");
                Test_LocateDc_NetrGetDCNameS217();
                goto label8;
            }
            if ((temp44 == 2)) {
                this.Manager.Comment("reaching state \'S89\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp43;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp43 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S170\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp43, "return of NetrGetDCName, state S170");
                this.Manager.Comment("reaching state \'S229\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_LocateDc_NetrGetDCNameS22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_LocateDc_NetrGetDCNameS22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS24() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp45;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp45);
            this.Manager.AddReturn(GetPlatformInfo, null, temp45);
            this.Manager.Comment("reaching state \'S25\'");
            int temp49 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS24GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS24GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS24GetPlatformChecker2)));
            if ((temp49 == 0)) {
                this.Manager.Comment("reaching state \'S90\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp46;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp46 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S171\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp46, "return of NetrGetAnyDCName, state S171");
                Test_LocateDc_NetrGetDCNameS218();
                goto label9;
            }
            if ((temp49 == 1)) {
                this.Manager.Comment("reaching state \'S91\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp47;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp47 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S172\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp47, "return of NetrGetAnyDCName, state S172");
                Test_LocateDc_NetrGetDCNameS217();
                goto label9;
            }
            if ((temp49 == 2)) {
                this.Manager.Comment("reaching state \'S92\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp48 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S173\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp48, "return of NetrGetDCName, state S173");
                this.Manager.Comment("reaching state \'S230\'");
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_LocateDc_NetrGetDCNameS24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_LocateDc_NetrGetDCNameS24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS26() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp50;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp50);
            this.Manager.AddReturn(GetPlatformInfo, null, temp50);
            this.Manager.Comment("reaching state \'S27\'");
            int temp54 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS26GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS26GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS26GetPlatformChecker2)));
            if ((temp54 == 0)) {
                this.Manager.Comment("reaching state \'S93\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp51;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,InvalidFormatDomainName)\'");
                temp51 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S174\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp51, "return of NetrGetAnyDCName, state S174");
                Test_LocateDc_NetrGetDCNameS218();
                goto label10;
            }
            if ((temp54 == 1)) {
                this.Manager.Comment("reaching state \'S94\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,InvalidFormatDomainName)\'");
                temp52 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S175\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp52, "return of NetrGetAnyDCName, state S175");
                Test_LocateDc_NetrGetDCNameS217();
                goto label10;
            }
            if ((temp54 == 2)) {
                this.Manager.Comment("reaching state \'S95\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp53 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S176\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp53, "return of NetrGetDCName, state S176");
                this.Manager.Comment("reaching state \'S231\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_LocateDc_NetrGetDCNameS26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_LocateDc_NetrGetDCNameS26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS28() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp55;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp55);
            this.Manager.AddReturn(GetPlatformInfo, null, temp55);
            this.Manager.Comment("reaching state \'S29\'");
            int temp59 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS28GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS28GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS28GetPlatformChecker2)));
            if ((temp59 == 0)) {
                this.Manager.Comment("reaching state \'S96\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp56 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S177\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp56, "return of NetrGetDCName, state S177");
                Test_LocateDc_NetrGetDCNameS216();
                goto label11;
            }
            if ((temp59 == 1)) {
                this.Manager.Comment("reaching state \'S97\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp57;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp57 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S178\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp57, "return of NetrGetDCName, state S178");
                this.Manager.Comment("reaching state \'S232\'");
                goto label11;
            }
            if ((temp59 == 2)) {
                this.Manager.Comment("reaching state \'S98\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp58;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp58 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S179\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp58, "return of NetrGetDCName, state S179");
                this.Manager.Comment("reaching state \'S233\'");
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_LocateDc_NetrGetDCNameS28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_LocateDc_NetrGetDCNameS28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS30() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp60;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp60);
            this.Manager.AddReturn(GetPlatformInfo, null, temp60);
            this.Manager.Comment("reaching state \'S31\'");
            int temp64 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS30GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS30GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS30GetPlatformChecker2)));
            if ((temp64 == 0)) {
                this.Manager.Comment("reaching state \'S100\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp61 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S181\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp61, "return of NetrGetDCName, state S181");
                this.Manager.Comment("reaching state \'S234\'");
                goto label12;
            }
            if ((temp64 == 1)) {
                this.Manager.Comment("reaching state \'S101\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp62 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S182\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp62, "return of NetrGetDCName, state S182");
                this.Manager.Comment("reaching state \'S235\'");
                goto label12;
            }
            if ((temp64 == 2)) {
                this.Manager.Comment("reaching state \'S99\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,InvalidFormatDomainName)\'");
                temp63 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S180\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp63, "return of NetrGetDCName, state S180");
                Test_LocateDc_NetrGetDCNameS216();
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_LocateDc_NetrGetDCNameS30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_LocateDc_NetrGetDCNameS30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS32() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp65;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp65);
            this.Manager.AddReturn(GetPlatformInfo, null, temp65);
            this.Manager.Comment("reaching state \'S33\'");
            int temp69 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS32GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS32GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS32GetPlatformChecker2)));
            if ((temp69 == 0)) {
                this.Manager.Comment("reaching state \'S102\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp66;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,NetBiosFormatDomainName)\'");
                temp66 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103436");
                this.Manager.Comment("reaching state \'S183\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp66, "return of NetrGetDCName, state S183");
                Test_LocateDc_NetrGetDCNameS216();
                goto label13;
            }
            if ((temp69 == 1)) {
                this.Manager.Comment("reaching state \'S103\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp67 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S184\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp67, "return of NetrGetDCName, state S184");
                this.Manager.Comment("reaching state \'S236\'");
                goto label13;
            }
            if ((temp69 == 2)) {
                this.Manager.Comment("reaching state \'S104\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp68;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp68 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S185\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp68, "return of NetrGetDCName, state S185");
                this.Manager.Comment("reaching state \'S237\'");
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_LocateDc_NetrGetDCNameS32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_LocateDc_NetrGetDCNameS32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS34() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp70;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp70);
            this.Manager.AddReturn(GetPlatformInfo, null, temp70);
            this.Manager.Comment("reaching state \'S35\'");
            int temp74 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS34GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS34GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS34GetPlatformChecker2)));
            if ((temp74 == 0)) {
                this.Manager.Comment("reaching state \'S105\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp71;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,TrustedDomainName)\'");
                temp71 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S186\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp71, "return of NetrGetDCName, state S186");
                Test_LocateDc_NetrGetDCNameS216();
                goto label14;
            }
            if ((temp74 == 1)) {
                this.Manager.Comment("reaching state \'S106\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp72 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S187\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp72, "return of NetrGetDCName, state S187");
                this.Manager.Comment("reaching state \'S238\'");
                goto label14;
            }
            if ((temp74 == 2)) {
                this.Manager.Comment("reaching state \'S107\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp73 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S188\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp73, "return of NetrGetDCName, state S188");
                this.Manager.Comment("reaching state \'S239\'");
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_LocateDc_NetrGetDCNameS34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_LocateDc_NetrGetDCNameS34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS36() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp75;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp75);
            this.Manager.AddReturn(GetPlatformInfo, null, temp75);
            this.Manager.Comment("reaching state \'S37\'");
            int temp79 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS36GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS36GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS36GetPlatformChecker2)));
            if ((temp79 == 0)) {
                this.Manager.Comment("reaching state \'S108\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp76;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,InvalidDomainName)\'");
                temp76 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103244");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S189\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp76, "return of NetrGetDCName, state S189");
                Test_LocateDc_NetrGetDCNameS216();
                goto label15;
            }
            if ((temp79 == 1)) {
                this.Manager.Comment("reaching state \'S109\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp77;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp77 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S190\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp77, "return of NetrGetDCName, state S190");
                this.Manager.Comment("reaching state \'S240\'");
                goto label15;
            }
            if ((temp79 == 2)) {
                this.Manager.Comment("reaching state \'S110\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp78;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp78 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S191\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp78, "return of NetrGetDCName, state S191");
                this.Manager.Comment("reaching state \'S241\'");
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_LocateDc_NetrGetDCNameS36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_LocateDc_NetrGetDCNameS36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS38() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp80;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp80);
            this.Manager.AddReturn(GetPlatformInfo, null, temp80);
            this.Manager.Comment("reaching state \'S39\'");
            int temp84 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS38GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS38GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS38GetPlatformChecker2)));
            if ((temp84 == 0)) {
                this.Manager.Comment("reaching state \'S111\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,Null)\'");
                temp81 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)));
                this.Manager.Checkpoint("MS-NRPC_R103436");
                this.Manager.Comment("reaching state \'S192\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp81, "return of NetrGetDCName, state S192");
                Test_LocateDc_NetrGetDCNameS216();
                goto label16;
            }
            if ((temp84 == 1)) {
                this.Manager.Comment("reaching state \'S112\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp82;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp82 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S193\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp82, "return of NetrGetDCName, state S193");
                this.Manager.Comment("reaching state \'S242\'");
                goto label16;
            }
            if ((temp84 == 2)) {
                this.Manager.Comment("reaching state \'S113\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp83;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp83 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S194\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp83, "return of NetrGetDCName, state S194");
                this.Manager.Comment("reaching state \'S243\'");
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_LocateDc_NetrGetDCNameS38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_LocateDc_NetrGetDCNameS38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS4() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp85;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp85);
            this.Manager.AddReturn(GetPlatformInfo, null, temp85);
            this.Manager.Comment("reaching state \'S5\'");
            int temp89 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS4GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS4GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS4GetPlatformChecker2)));
            if ((temp89 == 0)) {
                this.Manager.Comment("reaching state \'S60\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp86;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,InvalidFormatDomainName)\'");
                temp86 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp86, "return of NetrGetDCName, state S141");
                Test_LocateDc_NetrGetDCNameS218();
                goto label17;
            }
            if ((temp89 == 1)) {
                this.Manager.Comment("reaching state \'S61\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp87;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,InvalidFormatDomainName)\'");
                temp87 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp87, "return of NetrGetDCName, state S142");
                Test_LocateDc_NetrGetDCNameS217();
                goto label17;
            }
            if ((temp89 == 2)) {
                this.Manager.Comment("reaching state \'S62\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp88;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp88 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S143\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp88, "return of NetrGetDCName, state S143");
                this.Manager.Comment("reaching state \'S220\'");
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_LocateDc_NetrGetDCNameS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_LocateDc_NetrGetDCNameS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS40() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp90;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp90);
            this.Manager.AddReturn(GetPlatformInfo, null, temp90);
            this.Manager.Comment("reaching state \'S41\'");
            int temp94 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS40GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS40GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS40GetPlatformChecker2)));
            if ((temp94 == 0)) {
                this.Manager.Comment("reaching state \'S114\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp91;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(NonDcServer,NetBiosFormatDomainName)\'");
                temp91 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R104832");
                this.Manager.Comment("reaching state \'S195\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp91, "return of NetrGetAnyDCName, state S195");
                Test_LocateDc_NetrGetDCNameS216();
                goto label18;
            }
            if ((temp94 == 1)) {
                this.Manager.Comment("reaching state \'S115\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp92;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp92 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S196\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp92, "return of NetrGetDCName, state S196");
                this.Manager.Comment("reaching state \'S244\'");
                goto label18;
            }
            if ((temp94 == 2)) {
                this.Manager.Comment("reaching state \'S116\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp93;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp93 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S197\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp93, "return of NetrGetDCName, state S197");
                this.Manager.Comment("reaching state \'S245\'");
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_LocateDc_NetrGetDCNameS40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_LocateDc_NetrGetDCNameS40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS42() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp95;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp95);
            this.Manager.AddReturn(GetPlatformInfo, null, temp95);
            this.Manager.Comment("reaching state \'S43\'");
            int temp99 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS42GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS42GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS42GetPlatformChecker2)));
            if ((temp99 == 0)) {
                this.Manager.Comment("reaching state \'S117\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp96;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,TrustedDomainName)\'");
                temp96 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName);
                this.Manager.Checkpoint("MS-NRPC_R104832");
                this.Manager.Comment("reaching state \'S198\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp96, "return of NetrGetAnyDCName, state S198");
                Test_LocateDc_NetrGetDCNameS216();
                goto label19;
            }
            if ((temp99 == 1)) {
                this.Manager.Comment("reaching state \'S118\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp97;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp97 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S199\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp97, "return of NetrGetDCName, state S199");
                this.Manager.Comment("reaching state \'S246\'");
                goto label19;
            }
            if ((temp99 == 2)) {
                this.Manager.Comment("reaching state \'S119\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp98;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp98 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S200\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp98, "return of NetrGetDCName, state S200");
                this.Manager.Comment("reaching state \'S247\'");
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_LocateDc_NetrGetDCNameS42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_LocateDc_NetrGetDCNameS42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS44() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp100;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp100);
            this.Manager.AddReturn(GetPlatformInfo, null, temp100);
            this.Manager.Comment("reaching state \'S45\'");
            int temp104 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS44GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS44GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS44GetPlatformChecker2)));
            if ((temp104 == 0)) {
                this.Manager.Comment("reaching state \'S120\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp101;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,Null)\'");
                temp101 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)));
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S201\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp101, "return of NetrGetAnyDCName, state S201");
                Test_LocateDc_NetrGetDCNameS216();
                goto label20;
            }
            if ((temp104 == 1)) {
                this.Manager.Comment("reaching state \'S121\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp102;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp102 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S202\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp102, "return of NetrGetDCName, state S202");
                this.Manager.Comment("reaching state \'S248\'");
                goto label20;
            }
            if ((temp104 == 2)) {
                this.Manager.Comment("reaching state \'S122\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp103;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp103 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S203\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp103, "return of NetrGetDCName, state S203");
                this.Manager.Comment("reaching state \'S249\'");
                goto label20;
            }
            throw new InvalidOperationException("never reached");
        label20:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_LocateDc_NetrGetDCNameS44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_LocateDc_NetrGetDCNameS44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS46() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp105;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp105);
            this.Manager.AddReturn(GetPlatformInfo, null, temp105);
            this.Manager.Comment("reaching state \'S47\'");
            int temp109 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS46GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS46GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS46GetPlatformChecker2)));
            if ((temp109 == 0)) {
                this.Manager.Comment("reaching state \'S123\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp106;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,EmptyDomainName)\'");
                temp106 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S204\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp106, "return of NetrGetAnyDCName, state S204");
                Test_LocateDc_NetrGetDCNameS216();
                goto label21;
            }
            if ((temp109 == 1)) {
                this.Manager.Comment("reaching state \'S124\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp107;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp107 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S205\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp107, "return of NetrGetDCName, state S205");
                this.Manager.Comment("reaching state \'S250\'");
                goto label21;
            }
            if ((temp109 == 2)) {
                this.Manager.Comment("reaching state \'S125\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp108 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S206\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp108, "return of NetrGetDCName, state S206");
                this.Manager.Comment("reaching state \'S251\'");
                goto label21;
            }
            throw new InvalidOperationException("never reached");
        label21:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_LocateDc_NetrGetDCNameS46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_LocateDc_NetrGetDCNameS46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS48() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp110;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp110);
            this.Manager.AddReturn(GetPlatformInfo, null, temp110);
            this.Manager.Comment("reaching state \'S49\'");
            int temp114 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS48GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS48GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS48GetPlatformChecker2)));
            if ((temp114 == 0)) {
                this.Manager.Comment("reaching state \'S126\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp111;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,NetBiosFormatDomainName)\'");
                temp111 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S207\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp111, "return of NetrGetAnyDCName, state S207");
                Test_LocateDc_NetrGetDCNameS216();
                goto label22;
            }
            if ((temp114 == 1)) {
                this.Manager.Comment("reaching state \'S127\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp112;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp112 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S208\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp112, "return of NetrGetDCName, state S208");
                this.Manager.Comment("reaching state \'S252\'");
                goto label22;
            }
            if ((temp114 == 2)) {
                this.Manager.Comment("reaching state \'S128\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp113;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp113 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S209\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp113, "return of NetrGetDCName, state S209");
                this.Manager.Comment("reaching state \'S253\'");
                goto label22;
            }
            throw new InvalidOperationException("never reached");
        label22:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_LocateDc_NetrGetDCNameS48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_LocateDc_NetrGetDCNameS48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS50() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp115;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp115);
            this.Manager.AddReturn(GetPlatformInfo, null, temp115);
            this.Manager.Comment("reaching state \'S51\'");
            int temp119 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS50GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS50GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS50GetPlatformChecker2)));
            if ((temp119 == 0)) {
                this.Manager.Comment("reaching state \'S129\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp116 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S210\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp116, "return of NetrGetAnyDCName, state S210");
                Test_LocateDc_NetrGetDCNameS216();
                goto label23;
            }
            if ((temp119 == 1)) {
                this.Manager.Comment("reaching state \'S130\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp117 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S211\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp117, "return of NetrGetDCName, state S211");
                this.Manager.Comment("reaching state \'S254\'");
                goto label23;
            }
            if ((temp119 == 2)) {
                this.Manager.Comment("reaching state \'S131\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp118 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S212\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp118, "return of NetrGetDCName, state S212");
                this.Manager.Comment("reaching state \'S255\'");
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_LocateDc_NetrGetDCNameS50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_LocateDc_NetrGetDCNameS50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS52() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp120;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp120);
            this.Manager.AddReturn(GetPlatformInfo, null, temp120);
            this.Manager.Comment("reaching state \'S53\'");
            int temp124 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS52GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS52GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS52GetPlatformChecker2)));
            if ((temp124 == 0)) {
                this.Manager.Comment("reaching state \'S132\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp121;
                this.Manager.Comment("executing step \'call NetrGetAnyDCName(PrimaryDc,InvalidFormatDomainName)\'");
                temp121 = this.INrpcServerAdapterInstance.NetrGetAnyDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103260");
                this.Manager.Checkpoint("MS-NRPC_R614");
                this.Manager.Checkpoint("MS-NRPC_R104833");
                this.Manager.Comment("reaching state \'S213\'");
                this.Manager.Comment("checking step \'return NetrGetAnyDCName/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp121, "return of NetrGetAnyDCName, state S213");
                Test_LocateDc_NetrGetDCNameS216();
                goto label24;
            }
            if ((temp124 == 1)) {
                this.Manager.Comment("reaching state \'S133\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp122 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S214\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp122, "return of NetrGetDCName, state S214");
                this.Manager.Comment("reaching state \'S256\'");
                goto label24;
            }
            if ((temp124 == 2)) {
                this.Manager.Comment("reaching state \'S134\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp123;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp123 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S215\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp123, "return of NetrGetDCName, state S215");
                this.Manager.Comment("reaching state \'S257\'");
                goto label24;
            }
            throw new InvalidOperationException("never reached");
        label24:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_LocateDc_NetrGetDCNameS52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_LocateDc_NetrGetDCNameS52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS6() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp125;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp125);
            this.Manager.AddReturn(GetPlatformInfo, null, temp125);
            this.Manager.Comment("reaching state \'S7\'");
            int temp129 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS6GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS6GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS6GetPlatformChecker2)));
            if ((temp129 == 0)) {
                this.Manager.Comment("reaching state \'S63\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp126;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,NetBiosFormatDomainName)\'");
                temp126 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103436");
                this.Manager.Comment("reaching state \'S144\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp126, "return of NetrGetDCName, state S144");
                Test_LocateDc_NetrGetDCNameS218();
                goto label25;
            }
            if ((temp129 == 1)) {
                this.Manager.Comment("reaching state \'S64\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp127;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,NetBiosFormatDomainName)\'");
                temp127 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103436");
                this.Manager.Comment("reaching state \'S145\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp127, "return of NetrGetDCName, state S145");
                Test_LocateDc_NetrGetDCNameS217();
                goto label25;
            }
            if ((temp129 == 2)) {
                this.Manager.Comment("reaching state \'S65\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp128;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp128 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp128, "return of NetrGetDCName, state S146");
                this.Manager.Comment("reaching state \'S221\'");
                goto label25;
            }
            throw new InvalidOperationException("never reached");
        label25:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_LocateDc_NetrGetDCNameS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_LocateDc_NetrGetDCNameS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_NetrGetDCNameS8() {
            this.Manager.BeginTest("Test_LocateDc_NetrGetDCNameS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp130;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp130);
            this.Manager.AddReturn(GetPlatformInfo, null, temp130);
            this.Manager.Comment("reaching state \'S9\'");
            int temp134 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS8GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS8GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_NetrGetDCName.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_NetrGetDCNameS8GetPlatformChecker2)));
            if ((temp134 == 0)) {
                this.Manager.Comment("reaching state \'S66\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp131;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,InvalidDomainName)\'");
                temp131 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103244");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S147\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp131, "return of NetrGetDCName, state S147");
                Test_LocateDc_NetrGetDCNameS218();
                goto label26;
            }
            if ((temp134 == 1)) {
                this.Manager.Comment("reaching state \'S67\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp132;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,InvalidDomainName)\'");
                temp132 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103244");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S148\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp132, "return of NetrGetDCName, state S148");
                Test_LocateDc_NetrGetDCNameS217();
                goto label26;
            }
            if ((temp134 == 2)) {
                this.Manager.Comment("reaching state \'S68\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                this.Manager.Comment("executing step \'call NetrGetDCName(PrimaryDc,FqdnFormatDomainName)\'");
                temp133 = this.INrpcServerAdapterInstance.NetrGetDCName(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                this.Manager.Checkpoint("MS-NRPC_R103242");
                this.Manager.Checkpoint("MS-NRPC_R103437");
                this.Manager.Comment("reaching state \'S149\'");
                this.Manager.Comment("checking step \'return NetrGetDCName/NERR_DCNotFound\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.NERR_DCNotFound, temp133, "return of NetrGetDCName, state S149");
                this.Manager.Comment("reaching state \'S222\'");
                goto label26;
            }
            throw new InvalidOperationException("never reached");
        label26:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_NetrGetDCNameS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_LocateDc_NetrGetDCNameS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_LocateDc_NetrGetDCNameS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
    }
}
