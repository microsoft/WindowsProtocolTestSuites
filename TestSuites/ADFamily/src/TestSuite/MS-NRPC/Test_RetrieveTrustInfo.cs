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
    public partial class Test_RetrieveTrustInfo : PtfTestClassBase {
        
        public Test_RetrieveTrustInfo() {
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
        public void Test_RetrieveTrustInfoS0() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp4 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS0GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS0GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS0GetPlatformChecker2)));
            if ((temp4 == 0)) {
                this.Manager.Comment("reaching state \'S116\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,1)\'" +
                        "");
                temp1 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103645");
                this.Manager.Comment("reaching state \'S290\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of DsrGetForestTrustInformation, state S290");
                Test_RetrieveTrustInfoS464();
                goto label0;
            }
            if ((temp4 == 1)) {
                this.Manager.Comment("reaching state \'S117\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,1)\'" +
                        "");
                temp2 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103645");
                this.Manager.Comment("reaching state \'S291\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp2, "return of DsrGetForestTrustInformation, state S291");
                Test_RetrieveTrustInfoS465();
                goto label0;
            }
            if ((temp4 == 2)) {
                this.Manager.Comment("reaching state \'S118\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,1)\'" +
                        "");
                temp3 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103645");
                this.Manager.Comment("reaching state \'S292\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of DsrGetForestTrustInformation, state S292");
                Test_RetrieveTrustInfoS466();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_RetrieveTrustInfoS464() {
            this.Manager.Comment("reaching state \'S464\'");
        }
        
        private void Test_RetrieveTrustInfoS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_RetrieveTrustInfoS465() {
            this.Manager.Comment("reaching state \'S465\'");
        }
        
        private void Test_RetrieveTrustInfoS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_RetrieveTrustInfoS466() {
            this.Manager.Comment("reaching state \'S466\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS10() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp5;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp5);
            this.Manager.AddReturn(GetPlatformInfo, null, temp5);
            this.Manager.Comment("reaching state \'S11\'");
            int temp9 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS10GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS10GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS10GetPlatformChecker2)));
            if ((temp9 == 0)) {
                this.Manager.Comment("reaching state \'S131\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S305\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S479\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,FqdnFormatDomainName," +
                        "0)\'");
                temp6 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S583\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp6, "return of DsrGetForestTrustInformation, state S583");
                Test_RetrieveTrustInfoS608();
                goto label1;
            }
            if ((temp9 == 1)) {
                this.Manager.Comment("reaching state \'S132\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S306\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S480\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,FqdnFormatDomainName," +
                        "0)\'");
                temp7 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S584\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp7, "return of DsrGetForestTrustInformation, state S584");
                Test_RetrieveTrustInfoS609();
                goto label1;
            }
            if ((temp9 == 2)) {
                this.Manager.Comment("reaching state \'S133\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp8 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S307\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp8, "return of DsrEnumerateDomainTrusts, state S307");
                this.Manager.Comment("reaching state \'S481\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_RetrieveTrustInfoS608() {
            this.Manager.Comment("reaching state \'S608\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S620\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_RetrieveTrustInfoS466();
        }
        
        private void Test_RetrieveTrustInfoS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_RetrieveTrustInfoS609() {
            this.Manager.Comment("reaching state \'S609\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S621\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_RetrieveTrustInfoS465();
        }
        
        private void Test_RetrieveTrustInfoS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S100
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS100() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS100");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp10;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp10);
            this.Manager.AddReturn(GetPlatformInfo, null, temp10);
            this.Manager.Comment("reaching state \'S101\'");
            int temp14 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS100GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS100GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS100GetPlatformChecker2)));
            if ((temp14 == 0)) {
                this.Manager.Comment("reaching state \'S266\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(NonDcServer,TrustedDomainName,2" +
                        "147483648)\'");
                temp11 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104255");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S440\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp11, "return of DsrGetForestTrustInformation, state S440");
                Test_RetrieveTrustInfoS464();
                goto label2;
            }
            if ((temp14 == 1)) {
                this.Manager.Comment("reaching state \'S267\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp12 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S441\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp12, "return of DsrEnumerateDomainTrusts, state S441");
                this.Manager.Comment("reaching state \'S559\'");
                goto label2;
            }
            if ((temp14 == 2)) {
                this.Manager.Comment("reaching state \'S268\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp13 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S442\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp13, "return of DsrEnumerateDomainTrusts, state S442");
                this.Manager.Comment("reaching state \'S560\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS100GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_RetrieveTrustInfoS100GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_RetrieveTrustInfoS100GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        #endregion
        
        #region Test Starting in S102
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS102() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS102");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.AddReturn(GetPlatformInfo, null, temp15);
            this.Manager.Comment("reaching state \'S103\'");
            int temp19 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS102GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS102GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS102GetPlatformChecker2)));
            if ((temp19 == 0)) {
                this.Manager.Comment("reaching state \'S269\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidDomainName,0)\'" +
                        "");
                temp16 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S443\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp16, "return of DsrGetForestTrustInformation, state S443");
                Test_RetrieveTrustInfoS464();
                goto label3;
            }
            if ((temp19 == 1)) {
                this.Manager.Comment("reaching state \'S270\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp17 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S444\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp17, "return of DsrEnumerateDomainTrusts, state S444");
                this.Manager.Comment("reaching state \'S561\'");
                goto label3;
            }
            if ((temp19 == 2)) {
                this.Manager.Comment("reaching state \'S271\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp18 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S445\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp18, "return of DsrEnumerateDomainTrusts, state S445");
                this.Manager.Comment("reaching state \'S562\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS102GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_RetrieveTrustInfoS102GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_RetrieveTrustInfoS102GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        #endregion
        
        #region Test Starting in S104
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS104() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS104");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp20;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp20);
            this.Manager.AddReturn(GetPlatformInfo, null, temp20);
            this.Manager.Comment("reaching state \'S105\'");
            int temp24 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS104GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS104GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS104GetPlatformChecker2)));
            if ((temp24 == 0)) {
                this.Manager.Comment("reaching state \'S272\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,Null,0)\'");
                temp21 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), 0u);
                this.Manager.Checkpoint("MS-NRPC_R103645");
                this.Manager.Comment("reaching state \'S446\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp21, "return of DsrGetForestTrustInformation, state S446");
                Test_RetrieveTrustInfoS464();
                goto label4;
            }
            if ((temp24 == 1)) {
                this.Manager.Comment("reaching state \'S273\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp22;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp22 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S447\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp22, "return of DsrEnumerateDomainTrusts, state S447");
                this.Manager.Comment("reaching state \'S563\'");
                goto label4;
            }
            if ((temp24 == 2)) {
                this.Manager.Comment("reaching state \'S274\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp23 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S448\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp23, "return of DsrEnumerateDomainTrusts, state S448");
                this.Manager.Comment("reaching state \'S564\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS104GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_RetrieveTrustInfoS104GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_RetrieveTrustInfoS104GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        #endregion
        
        #region Test Starting in S106
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS106() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS106");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp25;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp25);
            this.Manager.AddReturn(GetPlatformInfo, null, temp25);
            this.Manager.Comment("reaching state \'S107\'");
            int temp29 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS106GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS106GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS106GetPlatformChecker2)));
            if ((temp29 == 0)) {
                this.Manager.Comment("reaching state \'S275\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp26;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,NetBiosFormatDomainNa" +
                        "me,0)\'");
                temp26 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S449\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp26, "return of DsrGetForestTrustInformation, state S449");
                Test_RetrieveTrustInfoS464();
                goto label5;
            }
            if ((temp29 == 1)) {
                this.Manager.Comment("reaching state \'S276\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp27 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S450\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp27, "return of DsrEnumerateDomainTrusts, state S450");
                this.Manager.Comment("reaching state \'S565\'");
                goto label5;
            }
            if ((temp29 == 2)) {
                this.Manager.Comment("reaching state \'S277\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp28 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S451\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp28, "return of DsrEnumerateDomainTrusts, state S451");
                this.Manager.Comment("reaching state \'S566\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS106GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_RetrieveTrustInfoS106GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_RetrieveTrustInfoS106GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        #endregion
        
        #region Test Starting in S108
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS108() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS108");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp30;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp30);
            this.Manager.AddReturn(GetPlatformInfo, null, temp30);
            this.Manager.Comment("reaching state \'S109\'");
            int temp34 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS108GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS108GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS108GetPlatformChecker2)));
            if ((temp34 == 0)) {
                this.Manager.Comment("reaching state \'S278\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,FqdnFormatDomainName," +
                        "0)\'");
                temp31 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S452\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp31, "return of DsrGetForestTrustInformation, state S452");
                Test_RetrieveTrustInfoS464();
                goto label6;
            }
            if ((temp34 == 1)) {
                this.Manager.Comment("reaching state \'S279\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp32;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp32 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S453\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp32, "return of DsrEnumerateDomainTrusts, state S453");
                this.Manager.Comment("reaching state \'S567\'");
                goto label6;
            }
            if ((temp34 == 2)) {
                this.Manager.Comment("reaching state \'S280\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp33;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp33 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S454\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp33, "return of DsrEnumerateDomainTrusts, state S454");
                this.Manager.Comment("reaching state \'S568\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS108GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_RetrieveTrustInfoS108GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_RetrieveTrustInfoS108GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        #endregion
        
        #region Test Starting in S110
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS110() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS110");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp35;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp35);
            this.Manager.AddReturn(GetPlatformInfo, null, temp35);
            this.Manager.Comment("reaching state \'S111\'");
            int temp39 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS110GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS110GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS110GetPlatformChecker2)));
            if ((temp39 == 0)) {
                this.Manager.Comment("reaching state \'S281\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp36;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidFormatDomainNa" +
                        "me,0)\'");
                temp36 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S455\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp36, "return of DsrGetForestTrustInformation, state S455");
                Test_RetrieveTrustInfoS464();
                goto label7;
            }
            if ((temp39 == 1)) {
                this.Manager.Comment("reaching state \'S282\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp37 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S456\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp37, "return of DsrEnumerateDomainTrusts, state S456");
                this.Manager.Comment("reaching state \'S569\'");
                goto label7;
            }
            if ((temp39 == 2)) {
                this.Manager.Comment("reaching state \'S283\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp38 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S457\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp38, "return of DsrEnumerateDomainTrusts, state S457");
                this.Manager.Comment("reaching state \'S570\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS110GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_RetrieveTrustInfoS110GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_RetrieveTrustInfoS110GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        #endregion
        
        #region Test Starting in S112
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS112() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS112");
            this.Manager.Comment("reaching state \'S112\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp40;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp40);
            this.Manager.AddReturn(GetPlatformInfo, null, temp40);
            this.Manager.Comment("reaching state \'S113\'");
            int temp44 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS112GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS112GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS112GetPlatformChecker2)));
            if ((temp44 == 0)) {
                this.Manager.Comment("reaching state \'S284\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,0)\'" +
                        "");
                temp41 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103645");
                this.Manager.Comment("reaching state \'S458\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp41, "return of DsrGetForestTrustInformation, state S458");
                Test_RetrieveTrustInfoS464();
                goto label8;
            }
            if ((temp44 == 1)) {
                this.Manager.Comment("reaching state \'S285\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp42 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S459\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp42, "return of DsrEnumerateDomainTrusts, state S459");
                this.Manager.Comment("reaching state \'S571\'");
                goto label8;
            }
            if ((temp44 == 2)) {
                this.Manager.Comment("reaching state \'S286\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp43;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp43 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S460\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp43, "return of DsrEnumerateDomainTrusts, state S460");
                this.Manager.Comment("reaching state \'S572\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS112GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_RetrieveTrustInfoS112GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_RetrieveTrustInfoS112GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        #endregion
        
        #region Test Starting in S114
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS114() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS114");
            this.Manager.Comment("reaching state \'S114\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp45;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp45);
            this.Manager.AddReturn(GetPlatformInfo, null, temp45);
            this.Manager.Comment("reaching state \'S115\'");
            int temp49 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS114GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS114GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS114GetPlatformChecker2)));
            if ((temp49 == 0)) {
                this.Manager.Comment("reaching state \'S287\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp46;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,214" +
                        "7483648)\'");
                temp46 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Checkpoint("MS-NRPC_R103653");
                this.Manager.Comment("reaching state \'S461\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_INVALID_FLAGS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_FLAGS, temp46, "return of DsrGetForestTrustInformation, state S461");
                Test_RetrieveTrustInfoS464();
                goto label9;
            }
            if ((temp49 == 1)) {
                this.Manager.Comment("reaching state \'S288\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp47;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp47 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S462\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp47, "return of DsrEnumerateDomainTrusts, state S462");
                this.Manager.Comment("reaching state \'S573\'");
                goto label9;
            }
            if ((temp49 == 2)) {
                this.Manager.Comment("reaching state \'S289\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp48 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S463\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp48, "return of DsrEnumerateDomainTrusts, state S463");
                this.Manager.Comment("reaching state \'S574\'");
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS114GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_RetrieveTrustInfoS114GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_RetrieveTrustInfoS114GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS12() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp50;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp50);
            this.Manager.AddReturn(GetPlatformInfo, null, temp50);
            this.Manager.Comment("reaching state \'S13\'");
            int temp54 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS12GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS12GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS12GetPlatformChecker2)));
            if ((temp54 == 0)) {
                this.Manager.Comment("reaching state \'S134\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S308\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S482\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp51;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,0)\'" +
                        "");
                temp51 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S585\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp51, "return of DsrGetForestTrustInformation, state S585");
                Test_RetrieveTrustInfoS608();
                goto label10;
            }
            if ((temp54 == 1)) {
                this.Manager.Comment("reaching state \'S135\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S309\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S483\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,0)\'" +
                        "");
                temp52 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S586\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp52, "return of DsrGetForestTrustInformation, state S586");
                Test_RetrieveTrustInfoS609();
                goto label10;
            }
            if ((temp54 == 2)) {
                this.Manager.Comment("reaching state \'S136\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp53 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S310\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp53, "return of DsrEnumerateDomainTrusts, state S310");
                this.Manager.Comment("reaching state \'S484\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_RetrieveTrustInfoS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_RetrieveTrustInfoS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS14() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp55;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp55);
            this.Manager.AddReturn(GetPlatformInfo, null, temp55);
            this.Manager.Comment("reaching state \'S15\'");
            int temp59 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS14GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS14GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS14GetPlatformChecker2)));
            if ((temp59 == 0)) {
                this.Manager.Comment("reaching state \'S137\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S311\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S485\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,1)\'" +
                        "");
                temp56 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S587\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp56, "return of DsrGetForestTrustInformation, state S587");
                Test_RetrieveTrustInfoS608();
                goto label11;
            }
            if ((temp59 == 1)) {
                this.Manager.Comment("reaching state \'S138\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S312\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S486\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp57;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,1)\'" +
                        "");
                temp57 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S588\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp57, "return of DsrGetForestTrustInformation, state S588");
                Test_RetrieveTrustInfoS609();
                goto label11;
            }
            if ((temp59 == 2)) {
                this.Manager.Comment("reaching state \'S139\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp58;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp58 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S313\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp58, "return of DsrEnumerateDomainTrusts, state S313");
                this.Manager.Comment("reaching state \'S487\'");
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_RetrieveTrustInfoS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_RetrieveTrustInfoS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS16() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp60;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp60);
            this.Manager.AddReturn(GetPlatformInfo, null, temp60);
            this.Manager.Comment("reaching state \'S17\'");
            int temp66 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS16GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS16GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS16GetPlatformChecker2)));
            if ((temp66 == 0)) {
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S314\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S488\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,Null,0)\'");
                temp61 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S589\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp61, "return of DsrGetForestTrustInformation, state S589");
                Test_RetrieveTrustInfoS610();
                goto label12;
            }
            if ((temp66 == 1)) {
                this.Manager.Comment("reaching state \'S141\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp62 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S315\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp62, "return of NetrServerReqChallenge, state S315");
                this.Manager.Comment("reaching state \'S489\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp63 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S590\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp63, "return of NetrServerAuthenticate3, state S590");
                this.Manager.Comment("reaching state \'S611\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp64;
                this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(NonDcServer,DomainMemberComputerAccou" +
                        "nt,WorkstationSecureChannel,Client,True)\'");
                temp64 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104257");
                this.Manager.Checkpoint("MS-NRPC_R103677");
                this.Manager.Comment("reaching state \'S623\'");
                this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp64, "return of NetrServerGetTrustInfo, state S623");
                Test_RetrieveTrustInfoS632();
                goto label12;
            }
            if ((temp66 == 2)) {
                this.Manager.Comment("reaching state \'S142\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp65;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp65 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S316\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp65, "return of DsrEnumerateDomainTrusts, state S316");
                this.Manager.Comment("reaching state \'S490\'");
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_RetrieveTrustInfoS610() {
            this.Manager.Comment("reaching state \'S610\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S622\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_RetrieveTrustInfoS464();
        }
        
        private void Test_RetrieveTrustInfoS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_RetrieveTrustInfoS632() {
            this.Manager.Comment("reaching state \'S632\'");
        }
        
        private void Test_RetrieveTrustInfoS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS18() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp67;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp67);
            this.Manager.AddReturn(GetPlatformInfo, null, temp67);
            this.Manager.Comment("reaching state \'S19\'");
            int temp73 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS18GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS18GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS18GetPlatformChecker2)));
            if ((temp73 == 0)) {
                this.Manager.Comment("reaching state \'S143\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S317\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S491\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp68;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidDomainName,0)\'" +
                        "");
                temp68 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S591\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp68, "return of DsrGetForestTrustInformation, state S591");
                Test_RetrieveTrustInfoS610();
                goto label13;
            }
            if ((temp73 == 1)) {
                this.Manager.Comment("reaching state \'S144\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp69;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp69 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S318\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp69, "return of NetrServerReqChallenge, state S318");
                this.Manager.Comment("reaching state \'S492\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp70;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp70 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S592\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp70, "return of NetrServerAuthenticate3, state S592");
                this.Manager.Comment("reaching state \'S612\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp71;
                this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,False)\'");
                temp71 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false);
                this.Manager.Checkpoint("MS-NRPC_R103682");
                this.Manager.Checkpoint("MS-NRPC_R103677");
                this.Manager.Comment("reaching state \'S624\'");
                this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp71, "return of NetrServerGetTrustInfo, state S624");
                Test_RetrieveTrustInfoS632();
                goto label13;
            }
            if ((temp73 == 2)) {
                this.Manager.Comment("reaching state \'S145\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp72 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S319\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp72, "return of DsrEnumerateDomainTrusts, state S319");
                this.Manager.Comment("reaching state \'S493\'");
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_RetrieveTrustInfoS18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_RetrieveTrustInfoS18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS2() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp74;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp74);
            this.Manager.AddReturn(GetPlatformInfo, null, temp74);
            this.Manager.Comment("reaching state \'S3\'");
            int temp78 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS2GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS2GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS2GetPlatformChecker2)));
            if ((temp78 == 0)) {
                this.Manager.Comment("reaching state \'S119\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S293\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S467\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp75;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,Null,0)\'");
                temp75 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S575\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp75, "return of DsrGetForestTrustInformation, state S575");
                Test_RetrieveTrustInfoS608();
                goto label14;
            }
            if ((temp78 == 1)) {
                this.Manager.Comment("reaching state \'S120\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S294\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S468\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp76;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,Null,0)\'");
                temp76 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S576\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp76, "return of DsrGetForestTrustInformation, state S576");
                Test_RetrieveTrustInfoS609();
                goto label14;
            }
            if ((temp78 == 2)) {
                this.Manager.Comment("reaching state \'S121\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp77;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp77 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S295\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp77, "return of DsrEnumerateDomainTrusts, state S295");
                this.Manager.Comment("reaching state \'S469\'");
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_RetrieveTrustInfoS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_RetrieveTrustInfoS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS20() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp79;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp79);
            this.Manager.AddReturn(GetPlatformInfo, null, temp79);
            this.Manager.Comment("reaching state \'S21\'");
            int temp85 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS20GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS20GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS20GetPlatformChecker2)));
            if ((temp85 == 0)) {
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S320\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S494\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp80;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidFormatDomainNa" +
                        "me,0)\'");
                temp80 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S593\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp80, "return of DsrGetForestTrustInformation, state S593");
                Test_RetrieveTrustInfoS610();
                goto label15;
            }
            if ((temp85 == 1)) {
                this.Manager.Comment("reaching state \'S147\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp81 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S321\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp81, "return of NetrServerReqChallenge, state S321");
                this.Manager.Comment("reaching state \'S495\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp82;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp82 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S594\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp82, "return of NetrServerAuthenticate3, state S594");
                this.Manager.Comment("reaching state \'S613\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp83;
                this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(NonDcServer,DomainMemberComputerAccou" +
                        "nt,WorkstationSecureChannel,Client,True)\'");
                temp83 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104257");
                this.Manager.Checkpoint("MS-NRPC_R103677");
                this.Manager.Comment("reaching state \'S625\'");
                this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp83, "return of NetrServerGetTrustInfo, state S625");
                Test_RetrieveTrustInfoS633();
                goto label15;
            }
            if ((temp85 == 2)) {
                this.Manager.Comment("reaching state \'S148\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp84;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp84 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S322\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp84, "return of DsrEnumerateDomainTrusts, state S322");
                this.Manager.Comment("reaching state \'S496\'");
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_RetrieveTrustInfoS20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_RetrieveTrustInfoS633() {
            this.Manager.Comment("reaching state \'S633\'");
        }
        
        private void Test_RetrieveTrustInfoS20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS22() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp86;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp86);
            this.Manager.AddReturn(GetPlatformInfo, null, temp86);
            this.Manager.Comment("reaching state \'S23\'");
            int temp92 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS22GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS22GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS22GetPlatformChecker2)));
            if ((temp92 == 0)) {
                this.Manager.Comment("reaching state \'S149\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S323\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S497\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp87;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,NetBiosFormatDomainNa" +
                        "me,0)\'");
                temp87 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S595\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp87, "return of DsrGetForestTrustInformation, state S595");
                Test_RetrieveTrustInfoS610();
                goto label16;
            }
            if ((temp92 == 1)) {
                this.Manager.Comment("reaching state \'S150\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp88;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp88 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S324\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp88, "return of NetrServerReqChallenge, state S324");
                this.Manager.Comment("reaching state \'S498\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp89;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp89 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S596\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp89, "return of NetrServerAuthenticate3, state S596");
                this.Manager.Comment("reaching state \'S614\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp90;
                this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,False)\'");
                temp90 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false);
                this.Manager.Checkpoint("MS-NRPC_R103682");
                this.Manager.Checkpoint("MS-NRPC_R103677");
                this.Manager.Comment("reaching state \'S626\'");
                this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp90, "return of NetrServerGetTrustInfo, state S626");
                Test_RetrieveTrustInfoS633();
                goto label16;
            }
            if ((temp92 == 2)) {
                this.Manager.Comment("reaching state \'S151\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp91;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp91 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S325\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp91, "return of DsrEnumerateDomainTrusts, state S325");
                this.Manager.Comment("reaching state \'S499\'");
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_RetrieveTrustInfoS22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_RetrieveTrustInfoS22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS24() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp93;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp93);
            this.Manager.AddReturn(GetPlatformInfo, null, temp93);
            this.Manager.Comment("reaching state \'S25\'");
            int temp97 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS24GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS24GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS24GetPlatformChecker2)));
            if ((temp97 == 0)) {
                this.Manager.Comment("reaching state \'S152\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S326\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S500\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp94;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,FqdnFormatDomainName," +
                        "0)\'");
                temp94 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S597\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp94, "return of DsrGetForestTrustInformation, state S597");
                Test_RetrieveTrustInfoS610();
                goto label17;
            }
            if ((temp97 == 1)) {
                this.Manager.Comment("reaching state \'S153\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp95;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp95 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S327\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp95, "return of DsrEnumerateDomainTrusts, state S327");
                Test_RetrieveTrustInfoS466();
                goto label17;
            }
            if ((temp97 == 2)) {
                this.Manager.Comment("reaching state \'S154\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp96;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp96 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S328\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp96, "return of DsrEnumerateDomainTrusts, state S328");
                this.Manager.Comment("reaching state \'S501\'");
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_RetrieveTrustInfoS24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_RetrieveTrustInfoS24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS26() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp98;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp98);
            this.Manager.AddReturn(GetPlatformInfo, null, temp98);
            this.Manager.Comment("reaching state \'S27\'");
            int temp102 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS26GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS26GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS26GetPlatformChecker2)));
            if ((temp102 == 0)) {
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S329\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S502\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp99;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,0)\'" +
                        "");
                temp99 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S598\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp99, "return of DsrGetForestTrustInformation, state S598");
                Test_RetrieveTrustInfoS610();
                goto label18;
            }
            if ((temp102 == 1)) {
                this.Manager.Comment("reaching state \'S156\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp100;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,2)\'");
                temp100 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S330\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp100, "return of DsrEnumerateDomainTrusts, state S330");
                Test_RetrieveTrustInfoS466();
                goto label18;
            }
            if ((temp102 == 2)) {
                this.Manager.Comment("reaching state \'S157\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp101;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp101 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S331\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp101, "return of DsrEnumerateDomainTrusts, state S331");
                this.Manager.Comment("reaching state \'S503\'");
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_RetrieveTrustInfoS26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_RetrieveTrustInfoS26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS28() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp103;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp103);
            this.Manager.AddReturn(GetPlatformInfo, null, temp103);
            this.Manager.Comment("reaching state \'S29\'");
            int temp107 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS28GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS28GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS28GetPlatformChecker2)));
            if ((temp107 == 0)) {
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S332\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S504\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp104;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,1)\'" +
                        "");
                temp104 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S599\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp104, "return of DsrGetForestTrustInformation, state S599");
                Test_RetrieveTrustInfoS610();
                goto label19;
            }
            if ((temp107 == 1)) {
                this.Manager.Comment("reaching state \'S159\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp105;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,4)\'");
                temp105 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S333\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp105, "return of DsrEnumerateDomainTrusts, state S333");
                Test_RetrieveTrustInfoS466();
                goto label19;
            }
            if ((temp107 == 2)) {
                this.Manager.Comment("reaching state \'S160\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp106;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp106 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S334\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp106, "return of DsrEnumerateDomainTrusts, state S334");
                this.Manager.Comment("reaching state \'S505\'");
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_RetrieveTrustInfoS28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_RetrieveTrustInfoS28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS30() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp108;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp108);
            this.Manager.AddReturn(GetPlatformInfo, null, temp108);
            this.Manager.Comment("reaching state \'S31\'");
            int temp114 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS30GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS30GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS30GetPlatformChecker2)));
            if ((temp114 == 0)) {
                this.Manager.Comment("reaching state \'S161\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp109;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp109 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S335\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp109, "return of NetrServerReqChallenge, state S335");
                this.Manager.Comment("reaching state \'S506\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp110;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp110 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S600\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp110, "return of NetrServerAuthenticate3, state S600");
                this.Manager.Comment("reaching state \'S615\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp111;
                this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(NonDcServer,DomainMemberComputerAccou" +
                        "nt,WorkstationSecureChannel,Client,True)\'");
                temp111 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104257");
                this.Manager.Checkpoint("MS-NRPC_R103677");
                this.Manager.Comment("reaching state \'S627\'");
                this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp111, "return of NetrServerGetTrustInfo, state S627");
                Test_RetrieveTrustInfoS634();
                goto label20;
            }
            if ((temp114 == 1)) {
                this.Manager.Comment("reaching state \'S162\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp112;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,8)\'");
                temp112 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S336\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp112, "return of DsrEnumerateDomainTrusts, state S336");
                Test_RetrieveTrustInfoS466();
                goto label20;
            }
            if ((temp114 == 2)) {
                this.Manager.Comment("reaching state \'S163\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp113;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp113 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S337\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp113, "return of DsrEnumerateDomainTrusts, state S337");
                this.Manager.Comment("reaching state \'S507\'");
                goto label20;
            }
            throw new InvalidOperationException("never reached");
        label20:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_RetrieveTrustInfoS634() {
            this.Manager.Comment("reaching state \'S634\'");
        }
        
        private void Test_RetrieveTrustInfoS30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_RetrieveTrustInfoS30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS32() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp115;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp115);
            this.Manager.AddReturn(GetPlatformInfo, null, temp115);
            this.Manager.Comment("reaching state \'S33\'");
            int temp121 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS32GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS32GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS32GetPlatformChecker2)));
            if ((temp121 == 0)) {
                this.Manager.Comment("reaching state \'S164\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp116 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S338\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp116, "return of NetrServerReqChallenge, state S338");
                this.Manager.Comment("reaching state \'S508\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp117 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S601\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp117, "return of NetrServerAuthenticate3, state S601");
                this.Manager.Comment("reaching state \'S616\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,False)\'");
                temp118 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false);
                this.Manager.Checkpoint("MS-NRPC_R103682");
                this.Manager.Checkpoint("MS-NRPC_R103677");
                this.Manager.Comment("reaching state \'S628\'");
                this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp118, "return of NetrServerGetTrustInfo, state S628");
                Test_RetrieveTrustInfoS634();
                goto label21;
            }
            if ((temp121 == 1)) {
                this.Manager.Comment("reaching state \'S165\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp119;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,16)\'");
                temp119 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 16u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S339\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp119, "return of DsrEnumerateDomainTrusts, state S339");
                Test_RetrieveTrustInfoS466();
                goto label21;
            }
            if ((temp121 == 2)) {
                this.Manager.Comment("reaching state \'S166\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp120;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp120 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S340\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp120, "return of DsrEnumerateDomainTrusts, state S340");
                this.Manager.Comment("reaching state \'S509\'");
                goto label21;
            }
            throw new InvalidOperationException("never reached");
        label21:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_RetrieveTrustInfoS32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_RetrieveTrustInfoS32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS34() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp122;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp122);
            this.Manager.AddReturn(GetPlatformInfo, null, temp122);
            this.Manager.Comment("reaching state \'S35\'");
            int temp126 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS34GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS34GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS34GetPlatformChecker2)));
            if ((temp126 == 0)) {
                this.Manager.Comment("reaching state \'S167\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp123;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,32)\'");
                temp123 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 32u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S341\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp123, "return of DsrEnumerateDomainTrusts, state S341");
                Test_RetrieveTrustInfoS466();
                goto label22;
            }
            if ((temp126 == 1)) {
                this.Manager.Comment("reaching state \'S168\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp124;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp124 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S342\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp124, "return of DsrEnumerateDomainTrusts, state S342");
                Test_RetrieveTrustInfoS465();
                goto label22;
            }
            if ((temp126 == 2)) {
                this.Manager.Comment("reaching state \'S169\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp125;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp125 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S343\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp125, "return of DsrEnumerateDomainTrusts, state S343");
                this.Manager.Comment("reaching state \'S510\'");
                goto label22;
            }
            throw new InvalidOperationException("never reached");
        label22:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_RetrieveTrustInfoS34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_RetrieveTrustInfoS34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS36() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp127;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp127);
            this.Manager.AddReturn(GetPlatformInfo, null, temp127);
            this.Manager.Comment("reaching state \'S37\'");
            int temp131 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS36GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS36GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS36GetPlatformChecker2)));
            if ((temp131 == 0)) {
                this.Manager.Comment("reaching state \'S170\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp128;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,2147483648)\'");
                temp128 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R103579");
                this.Manager.Checkpoint("MS-NRPC_R103575");
                this.Manager.Comment("reaching state \'S344\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_INVALID_FLAGS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_FLAGS, temp128, "return of DsrEnumerateDomainTrusts, state S344");
                Test_RetrieveTrustInfoS466();
                goto label23;
            }
            if ((temp131 == 1)) {
                this.Manager.Comment("reaching state \'S171\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp129;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,2)\'");
                temp129 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S345\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp129, "return of DsrEnumerateDomainTrusts, state S345");
                Test_RetrieveTrustInfoS465();
                goto label23;
            }
            if ((temp131 == 2)) {
                this.Manager.Comment("reaching state \'S172\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp130;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp130 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S346\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp130, "return of DsrEnumerateDomainTrusts, state S346");
                this.Manager.Comment("reaching state \'S511\'");
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_RetrieveTrustInfoS36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_RetrieveTrustInfoS36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS38() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp132;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp132);
            this.Manager.AddReturn(GetPlatformInfo, null, temp132);
            this.Manager.Comment("reaching state \'S39\'");
            int temp136 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS38GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS38GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS38GetPlatformChecker2)));
            if ((temp136 == 0)) {
                this.Manager.Comment("reaching state \'S173\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomainsEx(NonDcServer)\'");
                temp133 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomainsEx(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103592");
                this.Manager.Comment("reaching state \'S347\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomainsEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp133, "return of NetrEnumerateTrustedDomainsEx, state S347");
                Test_RetrieveTrustInfoS466();
                goto label24;
            }
            if ((temp136 == 1)) {
                this.Manager.Comment("reaching state \'S174\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S348\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S512\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp134;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,214" +
                        "7483648)\'");
                temp134 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S602\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp134, "return of DsrGetForestTrustInformation, state S602");
                Test_RetrieveTrustInfoS609();
                goto label24;
            }
            if ((temp136 == 2)) {
                this.Manager.Comment("reaching state \'S175\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp135;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp135 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S349\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp135, "return of DsrEnumerateDomainTrusts, state S349");
                this.Manager.Comment("reaching state \'S513\'");
                goto label24;
            }
            throw new InvalidOperationException("never reached");
        label24:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_RetrieveTrustInfoS38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_RetrieveTrustInfoS38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS4() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp137;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp137);
            this.Manager.AddReturn(GetPlatformInfo, null, temp137);
            this.Manager.Comment("reaching state \'S5\'");
            int temp141 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS4GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS4GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS4GetPlatformChecker2)));
            if ((temp141 == 0)) {
                this.Manager.Comment("reaching state \'S122\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S296\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S470\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp138;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidDomainName,0)\'" +
                        "");
                temp138 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S577\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp138, "return of DsrGetForestTrustInformation, state S577");
                Test_RetrieveTrustInfoS608();
                goto label25;
            }
            if ((temp141 == 1)) {
                this.Manager.Comment("reaching state \'S123\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S297\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S471\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp139;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidDomainName,0)\'" +
                        "");
                temp139 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S578\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp139, "return of DsrGetForestTrustInformation, state S578");
                Test_RetrieveTrustInfoS609();
                goto label25;
            }
            if ((temp141 == 2)) {
                this.Manager.Comment("reaching state \'S124\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp140;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp140 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S298\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp140, "return of DsrEnumerateDomainTrusts, state S298");
                this.Manager.Comment("reaching state \'S472\'");
                goto label25;
            }
            throw new InvalidOperationException("never reached");
        label25:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_RetrieveTrustInfoS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_RetrieveTrustInfoS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS40() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp142;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp142);
            this.Manager.AddReturn(GetPlatformInfo, null, temp142);
            this.Manager.Comment("reaching state \'S41\'");
            int temp148 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS40GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS40GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS40GetPlatformChecker2)));
            if ((temp148 == 0)) {
                this.Manager.Comment("reaching state \'S176\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp143;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomainsEx(PrimaryDc)\'");
                temp143 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomainsEx(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103592");
                this.Manager.Comment("reaching state \'S350\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomainsEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp143, "return of NetrEnumerateTrustedDomainsEx, state S350");
                Test_RetrieveTrustInfoS466();
                goto label26;
            }
            if ((temp148 == 1)) {
                this.Manager.Comment("reaching state \'S177\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp144;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp144 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S351\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp144, "return of NetrServerReqChallenge, state S351");
                this.Manager.Comment("reaching state \'S514\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp145;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp145 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S603\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp145, "return of NetrServerAuthenticate3, state S603");
                this.Manager.Comment("reaching state \'S617\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp146;
                this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,True)\'");
                temp146 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R103676");
                this.Manager.Comment("reaching state \'S629\'");
                this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp146, "return of NetrServerGetTrustInfo, state S629");
                Test_RetrieveTrustInfoS633();
                goto label26;
            }
            if ((temp148 == 2)) {
                this.Manager.Comment("reaching state \'S178\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp147;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp147 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S352\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp147, "return of DsrEnumerateDomainTrusts, state S352");
                this.Manager.Comment("reaching state \'S515\'");
                goto label26;
            }
            throw new InvalidOperationException("never reached");
        label26:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_RetrieveTrustInfoS40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_RetrieveTrustInfoS40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS42() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp149;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp149);
            this.Manager.AddReturn(GetPlatformInfo, null, temp149);
            this.Manager.Comment("reaching state \'S43\'");
            int temp153 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS42GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS42GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS42GetPlatformChecker2)));
            if ((temp153 == 0)) {
                this.Manager.Comment("reaching state \'S179\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp150;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomains(NonDcServer)\'");
                temp150 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomains(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103599");
                this.Manager.Comment("reaching state \'S353\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomains/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp150, "return of NetrEnumerateTrustedDomains, state S353");
                Test_RetrieveTrustInfoS466();
                goto label27;
            }
            if ((temp153 == 1)) {
                this.Manager.Comment("reaching state \'S180\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,4)\'");
                temp151 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S354\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp151, "return of DsrEnumerateDomainTrusts, state S354");
                Test_RetrieveTrustInfoS465();
                goto label27;
            }
            if ((temp153 == 2)) {
                this.Manager.Comment("reaching state \'S181\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp152;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp152 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S355\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp152, "return of DsrEnumerateDomainTrusts, state S355");
                this.Manager.Comment("reaching state \'S516\'");
                goto label27;
            }
            throw new InvalidOperationException("never reached");
        label27:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_RetrieveTrustInfoS42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_RetrieveTrustInfoS42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS44() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp154;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp154);
            this.Manager.AddReturn(GetPlatformInfo, null, temp154);
            this.Manager.Comment("reaching state \'S45\'");
            int temp158 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS44GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS44GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS44GetPlatformChecker2)));
            if ((temp158 == 0)) {
                this.Manager.Comment("reaching state \'S182\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp155;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomains(PrimaryDc)\'");
                temp155 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomains(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103599");
                this.Manager.Comment("reaching state \'S356\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomains/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp155, "return of NetrEnumerateTrustedDomains, state S356");
                Test_RetrieveTrustInfoS466();
                goto label28;
            }
            if ((temp158 == 1)) {
                this.Manager.Comment("reaching state \'S183\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp156;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,8)\'");
                temp156 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S357\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp156, "return of DsrEnumerateDomainTrusts, state S357");
                Test_RetrieveTrustInfoS465();
                goto label28;
            }
            if ((temp158 == 2)) {
                this.Manager.Comment("reaching state \'S184\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp157;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp157 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S358\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp157, "return of DsrEnumerateDomainTrusts, state S358");
                this.Manager.Comment("reaching state \'S517\'");
                goto label28;
            }
            throw new InvalidOperationException("never reached");
        label28:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_RetrieveTrustInfoS44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_RetrieveTrustInfoS44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS46() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp159;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp159);
            this.Manager.AddReturn(GetPlatformInfo, null, temp159);
            this.Manager.Comment("reaching state \'S47\'");
            int temp163 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS46GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS46GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS46GetPlatformChecker2)));
            if ((temp163 == 0)) {
                this.Manager.Comment("reaching state \'S185\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp160;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(NonDcServer,TrustedDomainName,0" +
                        ")\'");
                temp160 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104255");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S359\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp160, "return of DsrGetForestTrustInformation, state S359");
                Test_RetrieveTrustInfoS466();
                goto label29;
            }
            if ((temp163 == 1)) {
                this.Manager.Comment("reaching state \'S186\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp161;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,16)\'");
                temp161 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 16u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S360\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp161, "return of DsrEnumerateDomainTrusts, state S360");
                Test_RetrieveTrustInfoS465();
                goto label29;
            }
            if ((temp163 == 2)) {
                this.Manager.Comment("reaching state \'S187\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp162;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp162 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S361\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp162, "return of DsrEnumerateDomainTrusts, state S361");
                this.Manager.Comment("reaching state \'S518\'");
                goto label29;
            }
            throw new InvalidOperationException("never reached");
        label29:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_RetrieveTrustInfoS46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_RetrieveTrustInfoS46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS48() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp164;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp164);
            this.Manager.AddReturn(GetPlatformInfo, null, temp164);
            this.Manager.Comment("reaching state \'S49\'");
            int temp168 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS48GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS48GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS48GetPlatformChecker2)));
            if ((temp168 == 0)) {
                this.Manager.Comment("reaching state \'S188\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp165;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(NonDcServer,TrustedDomainName,1" +
                        ")\'");
                temp165 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 1u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104255");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S362\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp165, "return of DsrGetForestTrustInformation, state S362");
                Test_RetrieveTrustInfoS466();
                goto label30;
            }
            if ((temp168 == 1)) {
                this.Manager.Comment("reaching state \'S189\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp166;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,32)\'");
                temp166 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 32u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S363\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp166, "return of DsrEnumerateDomainTrusts, state S363");
                Test_RetrieveTrustInfoS465();
                goto label30;
            }
            if ((temp168 == 2)) {
                this.Manager.Comment("reaching state \'S190\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp167;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp167 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S364\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp167, "return of DsrEnumerateDomainTrusts, state S364");
                this.Manager.Comment("reaching state \'S519\'");
                goto label30;
            }
            throw new InvalidOperationException("never reached");
        label30:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_RetrieveTrustInfoS48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_RetrieveTrustInfoS48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS50() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp169;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp169);
            this.Manager.AddReturn(GetPlatformInfo, null, temp169);
            this.Manager.Comment("reaching state \'S51\'");
            int temp173 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS50GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS50GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS50GetPlatformChecker2)));
            if ((temp173 == 0)) {
                this.Manager.Comment("reaching state \'S191\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp170;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(NonDcServer,TrustedDomainName,2" +
                        "147483648)\'");
                temp170 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104255");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S365\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp170, "return of DsrGetForestTrustInformation, state S365");
                Test_RetrieveTrustInfoS466();
                goto label31;
            }
            if ((temp173 == 1)) {
                this.Manager.Comment("reaching state \'S192\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp171;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,2147483648)\'");
                temp171 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R103579");
                this.Manager.Checkpoint("MS-NRPC_R103575");
                this.Manager.Comment("reaching state \'S366\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_INVALID_FLAGS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_FLAGS, temp171, "return of DsrEnumerateDomainTrusts, state S366");
                Test_RetrieveTrustInfoS465();
                goto label31;
            }
            if ((temp173 == 2)) {
                this.Manager.Comment("reaching state \'S193\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp172;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp172 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S367\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp172, "return of DsrEnumerateDomainTrusts, state S367");
                this.Manager.Comment("reaching state \'S520\'");
                goto label31;
            }
            throw new InvalidOperationException("never reached");
        label31:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_RetrieveTrustInfoS50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_RetrieveTrustInfoS50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS52() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp174;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp174);
            this.Manager.AddReturn(GetPlatformInfo, null, temp174);
            this.Manager.Comment("reaching state \'S53\'");
            int temp178 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS52GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS52GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS52GetPlatformChecker2)));
            if ((temp178 == 0)) {
                this.Manager.Comment("reaching state \'S194\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp175;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidDomainName,0)\'" +
                        "");
                temp175 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S368\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp175, "return of DsrGetForestTrustInformation, state S368");
                Test_RetrieveTrustInfoS466();
                goto label32;
            }
            if ((temp178 == 1)) {
                this.Manager.Comment("reaching state \'S195\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp176;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomainsEx(NonDcServer)\'");
                temp176 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomainsEx(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103592");
                this.Manager.Comment("reaching state \'S369\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomainsEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp176, "return of NetrEnumerateTrustedDomainsEx, state S369");
                Test_RetrieveTrustInfoS465();
                goto label32;
            }
            if ((temp178 == 2)) {
                this.Manager.Comment("reaching state \'S196\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp177;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp177 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S370\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp177, "return of DsrEnumerateDomainTrusts, state S370");
                this.Manager.Comment("reaching state \'S521\'");
                goto label32;
            }
            throw new InvalidOperationException("never reached");
        label32:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_RetrieveTrustInfoS52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_RetrieveTrustInfoS52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS54() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp179;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp179);
            this.Manager.AddReturn(GetPlatformInfo, null, temp179);
            this.Manager.Comment("reaching state \'S55\'");
            int temp183 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS54GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS54GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS54GetPlatformChecker2)));
            if ((temp183 == 0)) {
                this.Manager.Comment("reaching state \'S197\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp180;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,Null,0)\'");
                temp180 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), 0u);
                this.Manager.Checkpoint("MS-NRPC_R103645");
                this.Manager.Comment("reaching state \'S371\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp180, "return of DsrGetForestTrustInformation, state S371");
                Test_RetrieveTrustInfoS466();
                goto label33;
            }
            if ((temp183 == 1)) {
                this.Manager.Comment("reaching state \'S198\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp181;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomainsEx(PrimaryDc)\'");
                temp181 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomainsEx(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103592");
                this.Manager.Comment("reaching state \'S372\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomainsEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp181, "return of NetrEnumerateTrustedDomainsEx, state S372");
                Test_RetrieveTrustInfoS465();
                goto label33;
            }
            if ((temp183 == 2)) {
                this.Manager.Comment("reaching state \'S199\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp182;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp182 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S373\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp182, "return of DsrEnumerateDomainTrusts, state S373");
                this.Manager.Comment("reaching state \'S522\'");
                goto label33;
            }
            throw new InvalidOperationException("never reached");
        label33:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS54GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_RetrieveTrustInfoS54GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_RetrieveTrustInfoS54GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        #endregion
        
        #region Test Starting in S56
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS56() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp184;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp184);
            this.Manager.AddReturn(GetPlatformInfo, null, temp184);
            this.Manager.Comment("reaching state \'S57\'");
            int temp188 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS56GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS56GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS56GetPlatformChecker2)));
            if ((temp188 == 0)) {
                this.Manager.Comment("reaching state \'S200\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp185;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,NetBiosFormatDomainNa" +
                        "me,0)\'");
                temp185 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S374\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp185, "return of DsrGetForestTrustInformation, state S374");
                Test_RetrieveTrustInfoS466();
                goto label34;
            }
            if ((temp188 == 1)) {
                this.Manager.Comment("reaching state \'S201\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp186;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomains(NonDcServer)\'");
                temp186 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomains(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103599");
                this.Manager.Comment("reaching state \'S375\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomains/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp186, "return of NetrEnumerateTrustedDomains, state S375");
                Test_RetrieveTrustInfoS465();
                goto label34;
            }
            if ((temp188 == 2)) {
                this.Manager.Comment("reaching state \'S202\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp187;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp187 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S376\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp187, "return of DsrEnumerateDomainTrusts, state S376");
                this.Manager.Comment("reaching state \'S523\'");
                goto label34;
            }
            throw new InvalidOperationException("never reached");
        label34:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS56GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_RetrieveTrustInfoS56GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_RetrieveTrustInfoS56GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        #endregion
        
        #region Test Starting in S58
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS58() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp189;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp189);
            this.Manager.AddReturn(GetPlatformInfo, null, temp189);
            this.Manager.Comment("reaching state \'S59\'");
            int temp193 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS58GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS58GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS58GetPlatformChecker2)));
            if ((temp193 == 0)) {
                this.Manager.Comment("reaching state \'S203\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp190;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,FqdnFormatDomainName," +
                        "0)\'");
                temp190 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S377\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp190, "return of DsrGetForestTrustInformation, state S377");
                Test_RetrieveTrustInfoS466();
                goto label35;
            }
            if ((temp193 == 1)) {
                this.Manager.Comment("reaching state \'S204\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp191;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomains(PrimaryDc)\'");
                temp191 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomains(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103599");
                this.Manager.Comment("reaching state \'S378\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomains/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp191, "return of NetrEnumerateTrustedDomains, state S378");
                Test_RetrieveTrustInfoS465();
                goto label35;
            }
            if ((temp193 == 2)) {
                this.Manager.Comment("reaching state \'S205\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp192;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp192 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S379\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp192, "return of DsrEnumerateDomainTrusts, state S379");
                this.Manager.Comment("reaching state \'S524\'");
                goto label35;
            }
            throw new InvalidOperationException("never reached");
        label35:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS58GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_RetrieveTrustInfoS58GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_RetrieveTrustInfoS58GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS6() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp194;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp194);
            this.Manager.AddReturn(GetPlatformInfo, null, temp194);
            this.Manager.Comment("reaching state \'S7\'");
            int temp198 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS6GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS6GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS6GetPlatformChecker2)));
            if ((temp198 == 0)) {
                this.Manager.Comment("reaching state \'S125\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S299\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S473\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp195;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidFormatDomainNa" +
                        "me,0)\'");
                temp195 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S579\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp195, "return of DsrGetForestTrustInformation, state S579");
                Test_RetrieveTrustInfoS608();
                goto label36;
            }
            if ((temp198 == 1)) {
                this.Manager.Comment("reaching state \'S126\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S300\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S474\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp196;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidFormatDomainNa" +
                        "me,0)\'");
                temp196 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S580\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp196, "return of DsrGetForestTrustInformation, state S580");
                Test_RetrieveTrustInfoS609();
                goto label36;
            }
            if ((temp198 == 2)) {
                this.Manager.Comment("reaching state \'S127\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp197;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp197 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S301\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp197, "return of DsrEnumerateDomainTrusts, state S301");
                this.Manager.Comment("reaching state \'S475\'");
                goto label36;
            }
            throw new InvalidOperationException("never reached");
        label36:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_RetrieveTrustInfoS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_RetrieveTrustInfoS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S60
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS60() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp199;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp199);
            this.Manager.AddReturn(GetPlatformInfo, null, temp199);
            this.Manager.Comment("reaching state \'S61\'");
            int temp203 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS60GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS60GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS60GetPlatformChecker2)));
            if ((temp203 == 0)) {
                this.Manager.Comment("reaching state \'S206\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp200;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidFormatDomainNa" +
                        "me,0)\'");
                temp200 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S380\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp200, "return of DsrGetForestTrustInformation, state S380");
                Test_RetrieveTrustInfoS466();
                goto label37;
            }
            if ((temp203 == 1)) {
                this.Manager.Comment("reaching state \'S207\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp201;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(NonDcServer,TrustedDomainName,0" +
                        ")\'");
                temp201 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104255");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S381\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp201, "return of DsrGetForestTrustInformation, state S381");
                Test_RetrieveTrustInfoS465();
                goto label37;
            }
            if ((temp203 == 2)) {
                this.Manager.Comment("reaching state \'S208\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp202;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp202 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S382\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp202, "return of DsrEnumerateDomainTrusts, state S382");
                this.Manager.Comment("reaching state \'S525\'");
                goto label37;
            }
            throw new InvalidOperationException("never reached");
        label37:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS60GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_RetrieveTrustInfoS60GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_RetrieveTrustInfoS60GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        #endregion
        
        #region Test Starting in S62
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS62() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp204;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp204);
            this.Manager.AddReturn(GetPlatformInfo, null, temp204);
            this.Manager.Comment("reaching state \'S63\'");
            int temp208 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS62GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS62GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS62GetPlatformChecker2)));
            if ((temp208 == 0)) {
                this.Manager.Comment("reaching state \'S209\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp205;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,0)\'" +
                        "");
                temp205 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103645");
                this.Manager.Comment("reaching state \'S383\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp205, "return of DsrGetForestTrustInformation, state S383");
                Test_RetrieveTrustInfoS466();
                goto label38;
            }
            if ((temp208 == 1)) {
                this.Manager.Comment("reaching state \'S210\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp206;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(NonDcServer,TrustedDomainName,1" +
                        ")\'");
                temp206 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 1u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104255");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S384\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp206, "return of DsrGetForestTrustInformation, state S384");
                Test_RetrieveTrustInfoS465();
                goto label38;
            }
            if ((temp208 == 2)) {
                this.Manager.Comment("reaching state \'S211\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp207;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp207 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S385\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp207, "return of DsrEnumerateDomainTrusts, state S385");
                this.Manager.Comment("reaching state \'S526\'");
                goto label38;
            }
            throw new InvalidOperationException("never reached");
        label38:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS62GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_RetrieveTrustInfoS62GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_RetrieveTrustInfoS62GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        #endregion
        
        #region Test Starting in S64
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS64() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS64");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp209;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp209);
            this.Manager.AddReturn(GetPlatformInfo, null, temp209);
            this.Manager.Comment("reaching state \'S65\'");
            int temp213 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS64GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS64GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS64GetPlatformChecker2)));
            if ((temp213 == 0)) {
                this.Manager.Comment("reaching state \'S212\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp210;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,214" +
                        "7483648)\'");
                temp210 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Checkpoint("MS-NRPC_R103653");
                this.Manager.Comment("reaching state \'S386\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_INVALID_FLAGS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_FLAGS, temp210, "return of DsrGetForestTrustInformation, state S386");
                Test_RetrieveTrustInfoS466();
                goto label39;
            }
            if ((temp213 == 1)) {
                this.Manager.Comment("reaching state \'S213\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp211;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(NonDcServer,TrustedDomainName,2" +
                        "147483648)\'");
                temp211 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104255");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S387\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp211, "return of DsrGetForestTrustInformation, state S387");
                Test_RetrieveTrustInfoS465();
                goto label39;
            }
            if ((temp213 == 2)) {
                this.Manager.Comment("reaching state \'S214\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp212;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp212 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S388\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp212, "return of DsrEnumerateDomainTrusts, state S388");
                this.Manager.Comment("reaching state \'S527\'");
                goto label39;
            }
            throw new InvalidOperationException("never reached");
        label39:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS64GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_RetrieveTrustInfoS64GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_RetrieveTrustInfoS64GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        #endregion
        
        #region Test Starting in S66
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS66() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS66");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp214;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp214);
            this.Manager.AddReturn(GetPlatformInfo, null, temp214);
            this.Manager.Comment("reaching state \'S67\'");
            int temp218 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS66GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS66GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS66GetPlatformChecker2)));
            if ((temp218 == 0)) {
                this.Manager.Comment("reaching state \'S215\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S389\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S528\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp215;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,214" +
                        "7483648)\'");
                temp215 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S604\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp215, "return of DsrGetForestTrustInformation, state S604");
                Test_RetrieveTrustInfoS608();
                goto label40;
            }
            if ((temp218 == 1)) {
                this.Manager.Comment("reaching state \'S216\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp216;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidDomainName,0)\'" +
                        "");
                temp216 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S390\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp216, "return of DsrGetForestTrustInformation, state S390");
                Test_RetrieveTrustInfoS465();
                goto label40;
            }
            if ((temp218 == 2)) {
                this.Manager.Comment("reaching state \'S217\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp217;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp217 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S391\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp217, "return of DsrEnumerateDomainTrusts, state S391");
                this.Manager.Comment("reaching state \'S529\'");
                goto label40;
            }
            throw new InvalidOperationException("never reached");
        label40:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS66GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_RetrieveTrustInfoS66GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_RetrieveTrustInfoS66GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        #endregion
        
        #region Test Starting in S68
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS68() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS68");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp219;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp219);
            this.Manager.AddReturn(GetPlatformInfo, null, temp219);
            this.Manager.Comment("reaching state \'S69\'");
            int temp225 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS68GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS68GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS68GetPlatformChecker2)));
            if ((temp225 == 0)) {
                this.Manager.Comment("reaching state \'S218\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp220;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp220 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S392\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp220, "return of NetrServerReqChallenge, state S392");
                this.Manager.Comment("reaching state \'S530\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp221;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp221 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S605\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp221, "return of NetrServerAuthenticate3, state S605");
                this.Manager.Comment("reaching state \'S618\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp222;
                this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,True)\'");
                temp222 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R103676");
                this.Manager.Comment("reaching state \'S630\'");
                this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp222, "return of NetrServerGetTrustInfo, state S630");
                Test_RetrieveTrustInfoS632();
                goto label41;
            }
            if ((temp225 == 1)) {
                this.Manager.Comment("reaching state \'S219\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp223;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,Null,0)\'");
                temp223 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), 0u);
                this.Manager.Checkpoint("MS-NRPC_R103645");
                this.Manager.Comment("reaching state \'S393\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp223, "return of DsrGetForestTrustInformation, state S393");
                Test_RetrieveTrustInfoS465();
                goto label41;
            }
            if ((temp225 == 2)) {
                this.Manager.Comment("reaching state \'S220\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp224;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp224 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S394\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp224, "return of DsrEnumerateDomainTrusts, state S394");
                this.Manager.Comment("reaching state \'S531\'");
                goto label41;
            }
            throw new InvalidOperationException("never reached");
        label41:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS68GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_RetrieveTrustInfoS68GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_RetrieveTrustInfoS68GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        #endregion
        
        #region Test Starting in S70
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS70() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS70");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp226;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp226);
            this.Manager.AddReturn(GetPlatformInfo, null, temp226);
            this.Manager.Comment("reaching state \'S71\'");
            int temp230 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS70GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS70GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS70GetPlatformChecker2)));
            if ((temp230 == 0)) {
                this.Manager.Comment("reaching state \'S221\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp227;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,NetBiosFormatDomainNa" +
                        "me,0)\'");
                temp227 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S395\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp227, "return of DsrGetForestTrustInformation, state S395");
                Test_RetrieveTrustInfoS465();
                goto label42;
            }
            if ((temp230 == 1)) {
                this.Manager.Comment("reaching state \'S222\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp228;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp228 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S396\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp228, "return of DsrEnumerateDomainTrusts, state S396");
                Test_RetrieveTrustInfoS464();
                goto label42;
            }
            if ((temp230 == 2)) {
                this.Manager.Comment("reaching state \'S223\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp229;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp229 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S397\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp229, "return of DsrEnumerateDomainTrusts, state S397");
                this.Manager.Comment("reaching state \'S532\'");
                goto label42;
            }
            throw new InvalidOperationException("never reached");
        label42:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS70GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_RetrieveTrustInfoS70GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_RetrieveTrustInfoS70GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        #endregion
        
        #region Test Starting in S72
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS72() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS72");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp231;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp231);
            this.Manager.AddReturn(GetPlatformInfo, null, temp231);
            this.Manager.Comment("reaching state \'S73\'");
            int temp235 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS72GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS72GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS72GetPlatformChecker2)));
            if ((temp235 == 0)) {
                this.Manager.Comment("reaching state \'S224\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp232;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,FqdnFormatDomainName," +
                        "0)\'");
                temp232 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S398\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp232, "return of DsrGetForestTrustInformation, state S398");
                Test_RetrieveTrustInfoS465();
                goto label43;
            }
            if ((temp235 == 1)) {
                this.Manager.Comment("reaching state \'S225\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp233;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,2)\'");
                temp233 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S399\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp233, "return of DsrEnumerateDomainTrusts, state S399");
                Test_RetrieveTrustInfoS464();
                goto label43;
            }
            if ((temp235 == 2)) {
                this.Manager.Comment("reaching state \'S226\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp234;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp234 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S400\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp234, "return of DsrEnumerateDomainTrusts, state S400");
                this.Manager.Comment("reaching state \'S533\'");
                goto label43;
            }
            throw new InvalidOperationException("never reached");
        label43:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS72GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_RetrieveTrustInfoS72GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_RetrieveTrustInfoS72GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        #endregion
        
        #region Test Starting in S74
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS74() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS74");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp236;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp236);
            this.Manager.AddReturn(GetPlatformInfo, null, temp236);
            this.Manager.Comment("reaching state \'S75\'");
            int temp240 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS74GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS74GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS74GetPlatformChecker2)));
            if ((temp240 == 0)) {
                this.Manager.Comment("reaching state \'S227\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp237;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,InvalidFormatDomainNa" +
                        "me,0)\'");
                temp237 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103655");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S401\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NO_SUCH_DOMAIN\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp237, "return of DsrGetForestTrustInformation, state S401");
                Test_RetrieveTrustInfoS465();
                goto label44;
            }
            if ((temp240 == 1)) {
                this.Manager.Comment("reaching state \'S228\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S402\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S534\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp238;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,214" +
                        "7483648)\'");
                temp238 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S606\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp238, "return of DsrGetForestTrustInformation, state S606");
                Test_RetrieveTrustInfoS610();
                goto label44;
            }
            if ((temp240 == 2)) {
                this.Manager.Comment("reaching state \'S229\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp239;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp239 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S403\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp239, "return of DsrEnumerateDomainTrusts, state S403");
                this.Manager.Comment("reaching state \'S535\'");
                goto label44;
            }
            throw new InvalidOperationException("never reached");
        label44:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS74GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_RetrieveTrustInfoS74GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_RetrieveTrustInfoS74GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        #endregion
        
        #region Test Starting in S76
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS76() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS76");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp241;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp241);
            this.Manager.AddReturn(GetPlatformInfo, null, temp241);
            this.Manager.Comment("reaching state \'S77\'");
            int temp247 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS76GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS76GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS76GetPlatformChecker2)));
            if ((temp247 == 0)) {
                this.Manager.Comment("reaching state \'S230\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp242;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,0)\'" +
                        "");
                temp242 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103645");
                this.Manager.Comment("reaching state \'S404\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp242, "return of DsrGetForestTrustInformation, state S404");
                Test_RetrieveTrustInfoS465();
                goto label45;
            }
            if ((temp247 == 1)) {
                this.Manager.Comment("reaching state \'S231\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp243;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp243 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S405\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp243, "return of NetrServerReqChallenge, state S405");
                this.Manager.Comment("reaching state \'S536\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp244;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp244 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S607\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp244, "return of NetrServerAuthenticate3, state S607");
                this.Manager.Comment("reaching state \'S619\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp245;
                this.Manager.Comment("executing step \'call NetrServerGetTrustInfo(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,True)\'");
                temp245 = this.INrpcServerAdapterInstance.NetrServerGetTrustInfo(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R103676");
                this.Manager.Comment("reaching state \'S631\'");
                this.Manager.Comment("checking step \'return NetrServerGetTrustInfo/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp245, "return of NetrServerGetTrustInfo, state S631");
                Test_RetrieveTrustInfoS634();
                goto label45;
            }
            if ((temp247 == 2)) {
                this.Manager.Comment("reaching state \'S232\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp246;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp246 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S406\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp246, "return of DsrEnumerateDomainTrusts, state S406");
                this.Manager.Comment("reaching state \'S537\'");
                goto label45;
            }
            throw new InvalidOperationException("never reached");
        label45:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS76GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_RetrieveTrustInfoS76GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_RetrieveTrustInfoS76GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        #endregion
        
        #region Test Starting in S78
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS78() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS78");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp248;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp248);
            this.Manager.AddReturn(GetPlatformInfo, null, temp248);
            this.Manager.Comment("reaching state \'S79\'");
            int temp252 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS78GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS78GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS78GetPlatformChecker2)));
            if ((temp252 == 0)) {
                this.Manager.Comment("reaching state \'S233\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp249;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,TrustedDomainName,214" +
                        "7483648)\'");
                temp249 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Checkpoint("MS-NRPC_R103653");
                this.Manager.Comment("reaching state \'S407\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_INVALID_FLAGS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_FLAGS, temp249, "return of DsrGetForestTrustInformation, state S407");
                Test_RetrieveTrustInfoS465();
                goto label46;
            }
            if ((temp252 == 1)) {
                this.Manager.Comment("reaching state \'S234\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp250;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,4)\'");
                temp250 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S408\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp250, "return of DsrEnumerateDomainTrusts, state S408");
                Test_RetrieveTrustInfoS464();
                goto label46;
            }
            if ((temp252 == 2)) {
                this.Manager.Comment("reaching state \'S235\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp251;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp251 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S409\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp251, "return of DsrEnumerateDomainTrusts, state S409");
                this.Manager.Comment("reaching state \'S538\'");
                goto label46;
            }
            throw new InvalidOperationException("never reached");
        label46:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS78GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_RetrieveTrustInfoS78GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_RetrieveTrustInfoS78GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS8() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp253;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp253);
            this.Manager.AddReturn(GetPlatformInfo, null, temp253);
            this.Manager.Comment("reaching state \'S9\'");
            int temp257 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS8GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS8GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS8GetPlatformChecker2)));
            if ((temp257 == 0)) {
                this.Manager.Comment("reaching state \'S128\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S302\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S476\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp254;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,NetBiosFormatDomainNa" +
                        "me,0)\'");
                temp254 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S581\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp254, "return of DsrGetForestTrustInformation, state S581");
                Test_RetrieveTrustInfoS608();
                goto label47;
            }
            if ((temp257 == 1)) {
                this.Manager.Comment("reaching state \'S129\'");
                this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                this.Manager.Comment("reaching state \'S303\'");
                this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                this.Manager.Comment("reaching state \'S477\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp255;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(PrimaryDc,NetBiosFormatDomainNa" +
                        "me,0)\'");
                temp255 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R103649");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S582\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp255, "return of DsrGetForestTrustInformation, state S582");
                Test_RetrieveTrustInfoS609();
                goto label47;
            }
            if ((temp257 == 2)) {
                this.Manager.Comment("reaching state \'S130\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp256;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp256 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S304\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp256, "return of DsrEnumerateDomainTrusts, state S304");
                this.Manager.Comment("reaching state \'S478\'");
                goto label47;
            }
            throw new InvalidOperationException("never reached");
        label47:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_RetrieveTrustInfoS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_RetrieveTrustInfoS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
        
        #region Test Starting in S80
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS80() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS80");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp258;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp258);
            this.Manager.AddReturn(GetPlatformInfo, null, temp258);
            this.Manager.Comment("reaching state \'S81\'");
            int temp262 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS80GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS80GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS80GetPlatformChecker2)));
            if ((temp262 == 0)) {
                this.Manager.Comment("reaching state \'S236\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp259;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,8)\'");
                temp259 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S410\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp259, "return of DsrEnumerateDomainTrusts, state S410");
                Test_RetrieveTrustInfoS464();
                goto label48;
            }
            if ((temp262 == 1)) {
                this.Manager.Comment("reaching state \'S237\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp260;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp260 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S411\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp260, "return of DsrEnumerateDomainTrusts, state S411");
                this.Manager.Comment("reaching state \'S539\'");
                goto label48;
            }
            if ((temp262 == 2)) {
                this.Manager.Comment("reaching state \'S238\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp261;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp261 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S412\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp261, "return of DsrEnumerateDomainTrusts, state S412");
                this.Manager.Comment("reaching state \'S540\'");
                goto label48;
            }
            throw new InvalidOperationException("never reached");
        label48:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS80GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_RetrieveTrustInfoS80GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_RetrieveTrustInfoS80GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        #endregion
        
        #region Test Starting in S82
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS82() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS82");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp263;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp263);
            this.Manager.AddReturn(GetPlatformInfo, null, temp263);
            this.Manager.Comment("reaching state \'S83\'");
            int temp267 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS82GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS82GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS82GetPlatformChecker2)));
            if ((temp267 == 0)) {
                this.Manager.Comment("reaching state \'S239\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp264;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,16)\'");
                temp264 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 16u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S413\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp264, "return of DsrEnumerateDomainTrusts, state S413");
                Test_RetrieveTrustInfoS464();
                goto label49;
            }
            if ((temp267 == 1)) {
                this.Manager.Comment("reaching state \'S240\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp265;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp265 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S414\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp265, "return of DsrEnumerateDomainTrusts, state S414");
                this.Manager.Comment("reaching state \'S541\'");
                goto label49;
            }
            if ((temp267 == 2)) {
                this.Manager.Comment("reaching state \'S241\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp266;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp266 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S415\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp266, "return of DsrEnumerateDomainTrusts, state S415");
                this.Manager.Comment("reaching state \'S542\'");
                goto label49;
            }
            throw new InvalidOperationException("never reached");
        label49:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS82GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_RetrieveTrustInfoS82GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_RetrieveTrustInfoS82GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        #endregion
        
        #region Test Starting in S84
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS84() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS84");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp268;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp268);
            this.Manager.AddReturn(GetPlatformInfo, null, temp268);
            this.Manager.Comment("reaching state \'S85\'");
            int temp272 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS84GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS84GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS84GetPlatformChecker2)));
            if ((temp272 == 0)) {
                this.Manager.Comment("reaching state \'S242\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp269;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,32)\'");
                temp269 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 32u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S416\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp269, "return of DsrEnumerateDomainTrusts, state S416");
                Test_RetrieveTrustInfoS464();
                goto label50;
            }
            if ((temp272 == 1)) {
                this.Manager.Comment("reaching state \'S243\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp270;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp270 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S417\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp270, "return of DsrEnumerateDomainTrusts, state S417");
                this.Manager.Comment("reaching state \'S543\'");
                goto label50;
            }
            if ((temp272 == 2)) {
                this.Manager.Comment("reaching state \'S244\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp271;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp271 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S418\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp271, "return of DsrEnumerateDomainTrusts, state S418");
                this.Manager.Comment("reaching state \'S544\'");
                goto label50;
            }
            throw new InvalidOperationException("never reached");
        label50:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS84GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_RetrieveTrustInfoS84GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_RetrieveTrustInfoS84GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        #endregion
        
        #region Test Starting in S86
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS86() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS86");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp273;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp273);
            this.Manager.AddReturn(GetPlatformInfo, null, temp273);
            this.Manager.Comment("reaching state \'S87\'");
            int temp277 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS86GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS86GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS86GetPlatformChecker2)));
            if ((temp277 == 0)) {
                this.Manager.Comment("reaching state \'S245\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp274;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,2147483648)\'");
                temp274 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R103579");
                this.Manager.Checkpoint("MS-NRPC_R103575");
                this.Manager.Comment("reaching state \'S419\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_INVALID_FLAGS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_FLAGS, temp274, "return of DsrEnumerateDomainTrusts, state S419");
                Test_RetrieveTrustInfoS464();
                goto label51;
            }
            if ((temp277 == 1)) {
                this.Manager.Comment("reaching state \'S246\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp275;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp275 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S420\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp275, "return of DsrEnumerateDomainTrusts, state S420");
                this.Manager.Comment("reaching state \'S545\'");
                goto label51;
            }
            if ((temp277 == 2)) {
                this.Manager.Comment("reaching state \'S247\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp276;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp276 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S421\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp276, "return of DsrEnumerateDomainTrusts, state S421");
                this.Manager.Comment("reaching state \'S546\'");
                goto label51;
            }
            throw new InvalidOperationException("never reached");
        label51:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS86GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_RetrieveTrustInfoS86GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_RetrieveTrustInfoS86GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        #endregion
        
        #region Test Starting in S88
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS88() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS88");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp278;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp278);
            this.Manager.AddReturn(GetPlatformInfo, null, temp278);
            this.Manager.Comment("reaching state \'S89\'");
            int temp282 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS88GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS88GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS88GetPlatformChecker2)));
            if ((temp282 == 0)) {
                this.Manager.Comment("reaching state \'S248\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp279;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomainsEx(NonDcServer)\'");
                temp279 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomainsEx(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103592");
                this.Manager.Comment("reaching state \'S422\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomainsEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp279, "return of NetrEnumerateTrustedDomainsEx, state S422");
                Test_RetrieveTrustInfoS464();
                goto label52;
            }
            if ((temp282 == 1)) {
                this.Manager.Comment("reaching state \'S249\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp280;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp280 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S423\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp280, "return of DsrEnumerateDomainTrusts, state S423");
                this.Manager.Comment("reaching state \'S547\'");
                goto label52;
            }
            if ((temp282 == 2)) {
                this.Manager.Comment("reaching state \'S250\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp281;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp281 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S424\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp281, "return of DsrEnumerateDomainTrusts, state S424");
                this.Manager.Comment("reaching state \'S548\'");
                goto label52;
            }
            throw new InvalidOperationException("never reached");
        label52:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS88GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_RetrieveTrustInfoS88GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_RetrieveTrustInfoS88GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        #endregion
        
        #region Test Starting in S90
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS90() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS90");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp283;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp283);
            this.Manager.AddReturn(GetPlatformInfo, null, temp283);
            this.Manager.Comment("reaching state \'S91\'");
            int temp287 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS90GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS90GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS90GetPlatformChecker2)));
            if ((temp287 == 0)) {
                this.Manager.Comment("reaching state \'S251\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp284;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomainsEx(PrimaryDc)\'");
                temp284 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomainsEx(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103592");
                this.Manager.Comment("reaching state \'S425\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomainsEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp284, "return of NetrEnumerateTrustedDomainsEx, state S425");
                Test_RetrieveTrustInfoS464();
                goto label53;
            }
            if ((temp287 == 1)) {
                this.Manager.Comment("reaching state \'S252\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp285;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp285 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S426\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp285, "return of DsrEnumerateDomainTrusts, state S426");
                this.Manager.Comment("reaching state \'S549\'");
                goto label53;
            }
            if ((temp287 == 2)) {
                this.Manager.Comment("reaching state \'S253\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp286;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp286 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S427\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp286, "return of DsrEnumerateDomainTrusts, state S427");
                this.Manager.Comment("reaching state \'S550\'");
                goto label53;
            }
            throw new InvalidOperationException("never reached");
        label53:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS90GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_RetrieveTrustInfoS90GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_RetrieveTrustInfoS90GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        #endregion
        
        #region Test Starting in S92
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS92() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS92");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp288;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp288);
            this.Manager.AddReturn(GetPlatformInfo, null, temp288);
            this.Manager.Comment("reaching state \'S93\'");
            int temp292 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS92GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS92GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS92GetPlatformChecker2)));
            if ((temp292 == 0)) {
                this.Manager.Comment("reaching state \'S254\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp289;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomains(NonDcServer)\'");
                temp289 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomains(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                this.Manager.Checkpoint("MS-NRPC_R103599");
                this.Manager.Comment("reaching state \'S428\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomains/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp289, "return of NetrEnumerateTrustedDomains, state S428");
                Test_RetrieveTrustInfoS464();
                goto label54;
            }
            if ((temp292 == 1)) {
                this.Manager.Comment("reaching state \'S255\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp290;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp290 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S429\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp290, "return of DsrEnumerateDomainTrusts, state S429");
                this.Manager.Comment("reaching state \'S551\'");
                goto label54;
            }
            if ((temp292 == 2)) {
                this.Manager.Comment("reaching state \'S256\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp291;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp291 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S430\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp291, "return of DsrEnumerateDomainTrusts, state S430");
                this.Manager.Comment("reaching state \'S552\'");
                goto label54;
            }
            throw new InvalidOperationException("never reached");
        label54:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS92GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_RetrieveTrustInfoS92GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_RetrieveTrustInfoS92GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        #endregion
        
        #region Test Starting in S94
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS94() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS94");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp293;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp293);
            this.Manager.AddReturn(GetPlatformInfo, null, temp293);
            this.Manager.Comment("reaching state \'S95\'");
            int temp297 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS94GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS94GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS94GetPlatformChecker2)));
            if ((temp297 == 0)) {
                this.Manager.Comment("reaching state \'S257\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp294;
                this.Manager.Comment("executing step \'call NetrEnumerateTrustedDomains(PrimaryDc)\'");
                temp294 = this.INrpcServerAdapterInstance.NetrEnumerateTrustedDomains(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                this.Manager.Checkpoint("MS-NRPC_R103599");
                this.Manager.Comment("reaching state \'S431\'");
                this.Manager.Comment("checking step \'return NetrEnumerateTrustedDomains/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp294, "return of NetrEnumerateTrustedDomains, state S431");
                Test_RetrieveTrustInfoS464();
                goto label55;
            }
            if ((temp297 == 1)) {
                this.Manager.Comment("reaching state \'S258\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp295;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp295 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S432\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp295, "return of DsrEnumerateDomainTrusts, state S432");
                this.Manager.Comment("reaching state \'S553\'");
                goto label55;
            }
            if ((temp297 == 2)) {
                this.Manager.Comment("reaching state \'S259\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp296;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp296 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S433\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp296, "return of DsrEnumerateDomainTrusts, state S433");
                this.Manager.Comment("reaching state \'S554\'");
                goto label55;
            }
            throw new InvalidOperationException("never reached");
        label55:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS94GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_RetrieveTrustInfoS94GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_RetrieveTrustInfoS94GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        #endregion
        
        #region Test Starting in S96
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS96() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS96");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp298;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp298);
            this.Manager.AddReturn(GetPlatformInfo, null, temp298);
            this.Manager.Comment("reaching state \'S97\'");
            int temp302 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS96GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS96GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS96GetPlatformChecker2)));
            if ((temp302 == 0)) {
                this.Manager.Comment("reaching state \'S260\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp299;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(NonDcServer,TrustedDomainName,0" +
                        ")\'");
                temp299 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 0u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104255");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S434\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp299, "return of DsrGetForestTrustInformation, state S434");
                Test_RetrieveTrustInfoS464();
                goto label56;
            }
            if ((temp302 == 1)) {
                this.Manager.Comment("reaching state \'S261\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp300;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp300 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S435\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp300, "return of DsrEnumerateDomainTrusts, state S435");
                this.Manager.Comment("reaching state \'S555\'");
                goto label56;
            }
            if ((temp302 == 2)) {
                this.Manager.Comment("reaching state \'S262\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp301;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp301 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S436\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp301, "return of DsrEnumerateDomainTrusts, state S436");
                this.Manager.Comment("reaching state \'S556\'");
                goto label56;
            }
            throw new InvalidOperationException("never reached");
        label56:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS96GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_RetrieveTrustInfoS96GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_RetrieveTrustInfoS96GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        #endregion
        
        #region Test Starting in S98
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_RetrieveTrustInfoS98() {
            this.Manager.BeginTest("Test_RetrieveTrustInfoS98");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp303;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp303);
            this.Manager.AddReturn(GetPlatformInfo, null, temp303);
            this.Manager.Comment("reaching state \'S99\'");
            int temp307 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS98GetPlatformChecker)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS98GetPlatformChecker1)), new ExpectedReturn(Test_RetrieveTrustInfo.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_RetrieveTrustInfoS98GetPlatformChecker2)));
            if ((temp307 == 0)) {
                this.Manager.Comment("reaching state \'S263\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp304;
                this.Manager.Comment("executing step \'call DsrGetForestTrustInformation(NonDcServer,TrustedDomainName,1" +
                        ")\'");
                temp304 = this.INrpcServerAdapterInstance.DsrGetForestTrustInformation(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, 1u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104255");
                this.Manager.Checkpoint("MS-NRPC_R103646");
                this.Manager.Comment("reaching state \'S437\'");
                this.Manager.Comment("checking step \'return DsrGetForestTrustInformation/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp304, "return of DsrGetForestTrustInformation, state S437");
                Test_RetrieveTrustInfoS464();
                goto label57;
            }
            if ((temp307 == 1)) {
                this.Manager.Comment("reaching state \'S264\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp305;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp305 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S438\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp305, "return of DsrEnumerateDomainTrusts, state S438");
                this.Manager.Comment("reaching state \'S557\'");
                goto label57;
            }
            if ((temp307 == 2)) {
                this.Manager.Comment("reaching state \'S265\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp306;
                this.Manager.Comment("executing step \'call DsrEnumerateDomainTrusts(PrimaryDc,1)\'");
                temp306 = this.INrpcServerAdapterInstance.DsrEnumerateDomainTrusts(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103574");
                this.Manager.Comment("reaching state \'S439\'");
                this.Manager.Comment("checking step \'return DsrEnumerateDomainTrusts/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp306, "return of DsrEnumerateDomainTrusts, state S439");
                this.Manager.Comment("reaching state \'S558\'");
                goto label57;
            }
            throw new InvalidOperationException("never reached");
        label57:
;
            this.Manager.EndTest();
        }
        
        private void Test_RetrieveTrustInfoS98GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_RetrieveTrustInfoS98GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_RetrieveTrustInfoS98GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        #endregion
    }
}
