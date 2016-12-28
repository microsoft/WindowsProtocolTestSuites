// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.4.2965.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TestScenarioS35 : PtfTestClassBase {
        
        public TestScenarioS35() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void SetInformationPolicy2Delegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        
        public delegate void CloseDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase CloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "Close", typeof(int), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle).MakeByRefType());
        
        static System.Reflection.MethodBase SetInformationPolicy2Info = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "SetInformationPolicy2", typeof(int), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass));
        #endregion
        
        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter ILsadManagedAdapterInstance;
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
            this.ILsadManagedAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter))));
        }
        
        protected override void TestCleanup() {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion
        
        #region Test Starting in S0
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S0() {
            this.Manager.BeginTest("TestScenarioS35S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp0);
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S36");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S36");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp2;
            this.Manager.Comment("executing step \'call SetInformationPolicy(53,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp2 = this.ILsadManagedAdapterInstance.SetInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp2, "return of SetInformationPolicy, state S60");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp3;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(53,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp4 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp3);
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp3, "policyInformation of QueryInformationPolicy, state S84");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp4, "return of QueryInformationPolicy, state S84");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLocalAccountDomainInformation" +
                    ")\'");
            temp5 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp5, "return of SetInformationPolicy2, state S108");
            this.Manager.Comment("reaching state \'S118\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyLocalAccountDomainInformati" +
                    "on,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp6);
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp6, "policyInformation of QueryInformationPolicy2, state S128");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp7, "return of QueryInformationPolicy2, state S128");
            TestScenarioS35S138();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS35S138() {
            this.Manager.Comment("reaching state \'S138\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp9 = this.ILsadManagedAdapterInstance.Close(1, out temp8);
            this.Manager.AddReturn(CloseInfo, null, temp8, temp9);
            TestScenarioS35S143();
        }
        
        private void TestScenarioS35S143() {
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS35.CloseInfo, null, new CloseDelegate1(this.TestScenarioS35S0CloseChecker)));
            this.Manager.Comment("reaching state \'S145\'");
        }
        
        private void TestScenarioS35S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S143");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S143");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S10() {
            this.Manager.BeginTest("TestScenarioS35S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S29\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp10);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp10, "policyHandle of OpenPolicy2, state S41");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp11, "return of OpenPolicy2, state S41");
            this.Manager.Comment("reaching state \'S53\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp12;
            this.Manager.Comment("executing step \'call SetInformationPolicy(1,PolicyLocalAccountDomainInformation)\'" +
                    "");
            temp12 = this.ILsadManagedAdapterInstance.SetInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp12, "return of SetInformationPolicy, state S65");
            this.Manager.Comment("reaching state \'S77\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp13;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp14;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(53,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp14 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp13);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp13, "policyInformation of QueryInformationPolicy, state S89");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp14, "return of QueryInformationPolicy, state S89");
            this.Manager.Comment("reaching state \'S101\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLocalAccountDomainInformation" +
                    ")\'");
            temp15 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp15, "return of SetInformationPolicy2, state S112");
            this.Manager.Comment("reaching state \'S122\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyLocalAccountDomainInformati" +
                    "on,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp16);
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp16, "policyInformation of QueryInformationPolicy2, state S132");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp17, "return of QueryInformationPolicy2, state S132");
            TestScenarioS35S140();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS35S140() {
            this.Manager.Comment("reaching state \'S140\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp19 = this.ILsadManagedAdapterInstance.Close(1, out temp18);
            this.Manager.AddReturn(CloseInfo, null, temp18, temp19);
            TestScenarioS35S144();
        }
        
        private void TestScenarioS35S144() {
            this.Manager.Comment("reaching state \'S144\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS35.CloseInfo, null, new CloseDelegate1(this.TestScenarioS35S10CloseChecker)));
            this.Manager.Comment("reaching state \'S146\'");
        }
        
        private void TestScenarioS35S10CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S144");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S144");
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S12() {
            this.Manager.BeginTest("TestScenarioS35S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp21 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp20);
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp20, "policyHandle of OpenPolicy2, state S42");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp21, "return of OpenPolicy2, state S42");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp22;
            this.Manager.Comment("executing step \'call SetInformationPolicy(53,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp22 = this.ILsadManagedAdapterInstance.SetInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp22, "return of SetInformationPolicy, state S66");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp23;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp24;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyLocalAccountDomainInformation" +
                    ",out _)\'");
            temp24 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp23);
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp23, "policyInformation of QueryInformationPolicy, state S90");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp24, "return of QueryInformationPolicy, state S90");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLocalAccountDomainInformation" +
                    ")\'");
            temp25 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp25, "return of SetInformationPolicy2, state S113");
            this.Manager.Comment("reaching state \'S123\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyLocalAccountDomainInformati" +
                    "on,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp26);
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp26, "policyInformation of QueryInformationPolicy2, state S133");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp27, "return of QueryInformationPolicy2, state S133");
            TestScenarioS35S139();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS35S139() {
            this.Manager.Comment("reaching state \'S139\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.Close(1, out temp28);
            this.Manager.AddReturn(CloseInfo, null, temp28, temp29);
            TestScenarioS35S143();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S14() {
            this.Manager.BeginTest("TestScenarioS35S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S31\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp31 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp30);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp30, "policyHandle of OpenPolicy2, state S43");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp31, "return of OpenPolicy2, state S43");
            this.Manager.Comment("reaching state \'S55\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp32;
            this.Manager.Comment("executing step \'call SetInformationPolicy(1,PolicyLocalAccountDomainInformation)\'" +
                    "");
            temp32 = this.ILsadManagedAdapterInstance.SetInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp32, "return of SetInformationPolicy, state S67");
            this.Manager.Comment("reaching state \'S79\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp33;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp34;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyLocalAccountDomainInformation" +
                    ",out _)\'");
            temp34 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp33);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp33, "policyInformation of QueryInformationPolicy, state S91");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp34, "return of QueryInformationPolicy, state S91");
            this.Manager.Comment("reaching state \'S103\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp35 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.AddReturn(SetInformationPolicy2Info, null, temp35);
            TestScenarioS35S114();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS35S114() {
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS35.SetInformationPolicy2Info, null, new SetInformationPolicy2Delegate1(this.TestScenarioS35S14SetInformationPolicy2Checker)));
            this.Manager.Comment("reaching state \'S124\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp37 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp36);
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp36, "policyInformation of QueryInformationPolicy2, state S134");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp37, "return of QueryInformationPolicy2, state S134");
            TestScenarioS35S142();
        }
        
        private void TestScenarioS35S14SetInformationPolicy2Checker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of SetInformationPolicy2, state S114");
        }
        
        private void TestScenarioS35S142() {
            this.Manager.Comment("reaching state \'S142\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.Close(1, out temp38);
            this.Manager.AddReturn(CloseInfo, null, temp38, temp39);
            TestScenarioS35S144();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S16() {
            this.Manager.BeginTest("TestScenarioS35S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp40);
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp40, "policyHandle of OpenPolicy2, state S44");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of OpenPolicy2, state S44");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp42;
            this.Manager.Comment("executing step \'call SetInformationPolicy(53,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp42 = this.ILsadManagedAdapterInstance.SetInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp42, "return of SetInformationPolicy, state S68");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp43;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp44;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(53,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp44 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp43);
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp43, "policyInformation of QueryInformationPolicy, state S92");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp44, "return of QueryInformationPolicy, state S92");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp45 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.AddReturn(SetInformationPolicy2Info, null, temp45);
            TestScenarioS35S114();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S18() {
            this.Manager.BeginTest("TestScenarioS35S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S33\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp46;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp47 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp46);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp46, "policyHandle of OpenPolicy2, state S45");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp47, "return of OpenPolicy2, state S45");
            this.Manager.Comment("reaching state \'S57\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp48;
            this.Manager.Comment("executing step \'call SetInformationPolicy(1,PolicyLocalAccountDomainInformation)\'" +
                    "");
            temp48 = this.ILsadManagedAdapterInstance.SetInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp48, "return of SetInformationPolicy, state S69");
            this.Manager.Comment("reaching state \'S81\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp49;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp50;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyLocalAccountDomainInformation" +
                    ",out _)\'");
            temp50 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp49);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp49, "policyInformation of QueryInformationPolicy, state S93");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp50, "return of QueryInformationPolicy, state S93");
            this.Manager.Comment("reaching state \'S105\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp51 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp51, "return of SetInformationPolicy2, state S115");
            this.Manager.Comment("reaching state \'S125\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp53 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp52);
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp52, "policyInformation of QueryInformationPolicy2, state S135");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp53, "return of QueryInformationPolicy2, state S135");
            TestScenarioS35S138();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S2() {
            this.Manager.BeginTest("TestScenarioS35S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S25\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp55 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp54);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp54, "policyHandle of OpenPolicy2, state S37");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp55, "return of OpenPolicy2, state S37");
            this.Manager.Comment("reaching state \'S49\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp56;
            this.Manager.Comment("executing step \'call SetInformationPolicy(53,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp56 = this.ILsadManagedAdapterInstance.SetInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp56, "return of SetInformationPolicy, state S61");
            this.Manager.Comment("reaching state \'S73\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp57;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp58;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyLocalAccountDomainInformation" +
                    ",out _)\'");
            temp58 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp57);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp57, "policyInformation of QueryInformationPolicy, state S85");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp58, "return of QueryInformationPolicy, state S85");
            this.Manager.Comment("reaching state \'S97\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLocalAccountDomainInformation" +
                    ")\'");
            temp59 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp59, "return of SetInformationPolicy2, state S109");
            this.Manager.Comment("reaching state \'S119\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp60;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp61 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp60);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp60, "policyInformation of QueryInformationPolicy2, state S129");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp61, "return of QueryInformationPolicy2, state S129");
            TestScenarioS35S139();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S20() {
            this.Manager.BeginTest("TestScenarioS35S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp63 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp62);
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp62, "policyHandle of OpenPolicy2, state S46");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp63, "return of OpenPolicy2, state S46");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp64;
            this.Manager.Comment("executing step \'call SetInformationPolicy(53,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp64 = this.ILsadManagedAdapterInstance.SetInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp64, "return of SetInformationPolicy, state S70");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp65;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp66;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(53,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp66 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp65);
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp65, "policyInformation of QueryInformationPolicy, state S94");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp66, "return of QueryInformationPolicy, state S94");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLocalAccountDomainInformation" +
                    ")\'");
            temp67 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp67, "return of SetInformationPolicy2, state S116");
            this.Manager.Comment("reaching state \'S126\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyLocalAccountDomainInformati" +
                    "on,out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp68);
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp68, "policyInformation of QueryInformationPolicy2, state S136");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp69, "return of QueryInformationPolicy2, state S136");
            TestScenarioS35S141();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS35S141() {
            this.Manager.Comment("reaching state \'S141\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp71 = this.ILsadManagedAdapterInstance.Close(1, out temp70);
            this.Manager.AddReturn(CloseInfo, null, temp70, temp71);
            TestScenarioS35S143();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S22() {
            this.Manager.BeginTest("TestScenarioS35S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S35\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp72;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp73 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp72);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp72, "policyHandle of OpenPolicy2, state S47");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp73, "return of OpenPolicy2, state S47");
            this.Manager.Comment("reaching state \'S59\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp74;
            this.Manager.Comment("executing step \'call SetInformationPolicy(1,PolicyLocalAccountDomainInformation)\'" +
                    "");
            temp74 = this.ILsadManagedAdapterInstance.SetInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp74, "return of SetInformationPolicy, state S71");
            this.Manager.Comment("reaching state \'S83\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp75;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp76;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(53,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp76 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp75);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp75, "policyInformation of QueryInformationPolicy, state S95");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp76, "return of QueryInformationPolicy, state S95");
            this.Manager.Comment("reaching state \'S107\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLocalAccountDomainInformation" +
                    ")\'");
            temp77 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp77, "return of SetInformationPolicy2, state S117");
            this.Manager.Comment("reaching state \'S127\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp78;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyLocalAccountDomainInformati" +
                    "on,out _)\'");
            temp79 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp78);
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp78, "policyInformation of QueryInformationPolicy2, state S137");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp79, "return of QueryInformationPolicy2, state S137");
            TestScenarioS35S142();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S4() {
            this.Manager.BeginTest("TestScenarioS35S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp80;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp81 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp80);
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp80, "policyHandle of OpenPolicy2, state S38");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp81, "return of OpenPolicy2, state S38");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp82;
            this.Manager.Comment("executing step \'call SetInformationPolicy(1,PolicyLocalAccountDomainInformation)\'" +
                    "");
            temp82 = this.ILsadManagedAdapterInstance.SetInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp82, "return of SetInformationPolicy, state S62");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp83;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp84;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyLocalAccountDomainInformation" +
                    ",out _)\'");
            temp84 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp83);
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp83, "policyInformation of QueryInformationPolicy, state S86");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp84, "return of QueryInformationPolicy, state S86");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp85 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.AddReturn(SetInformationPolicy2Info, null, temp85);
            TestScenarioS35S110();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS35S110() {
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS35.SetInformationPolicy2Info, null, new SetInformationPolicy2Delegate1(this.TestScenarioS35S4SetInformationPolicy2Checker)));
            this.Manager.Comment("reaching state \'S120\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp86;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp87 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp86);
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp86, "policyInformation of QueryInformationPolicy2, state S130");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp87, "return of QueryInformationPolicy2, state S130");
            TestScenarioS35S140();
        }
        
        private void TestScenarioS35S4SetInformationPolicy2Checker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of SetInformationPolicy2, state S110");
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S6() {
            this.Manager.BeginTest("TestScenarioS35S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S27\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp88;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp89 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp88);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp88, "policyHandle of OpenPolicy2, state S39");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp89, "return of OpenPolicy2, state S39");
            this.Manager.Comment("reaching state \'S51\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp90;
            this.Manager.Comment("executing step \'call SetInformationPolicy(53,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp90 = this.ILsadManagedAdapterInstance.SetInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp90, "return of SetInformationPolicy, state S63");
            this.Manager.Comment("reaching state \'S75\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp91;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp92;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyLocalAccountDomainInformation" +
                    ",out _)\'");
            temp92 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp91);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp91, "policyInformation of QueryInformationPolicy, state S87");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp92, "return of QueryInformationPolicy, state S87");
            this.Manager.Comment("reaching state \'S99\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp93;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLocalAccountDomainInformation" +
                    ")\'");
            temp93 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp93, "return of SetInformationPolicy2, state S111");
            this.Manager.Comment("reaching state \'S121\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp94;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp95;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyLocalAccountDomainInformatio" +
                    "n,out _)\'");
            temp95 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp94);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp94, "policyInformation of QueryInformationPolicy2, state S131");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp95, "return of QueryInformationPolicy2, state S131");
            TestScenarioS35S141();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS35S8() {
            this.Manager.BeginTest("TestScenarioS35S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp96;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp97 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp96);
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp96, "policyHandle of OpenPolicy2, state S40");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp97, "return of OpenPolicy2, state S40");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp98;
            this.Manager.Comment("executing step \'call SetInformationPolicy(53,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp98 = this.ILsadManagedAdapterInstance.SetInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp98, "return of SetInformationPolicy, state S64");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp99;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp100;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyLocalAccountDomainInformation" +
                    ",out _)\'");
            temp100 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation, out temp99);
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp99, "policyInformation of QueryInformationPolicy, state S88");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp100, "return of QueryInformationPolicy, state S88");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyLocalAccountDomainInformation)" +
                    "\'");
            temp101 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLocalAccountDomainInformation);
            this.Manager.AddReturn(SetInformationPolicy2Info, null, temp101);
            TestScenarioS35S110();
            this.Manager.EndTest();
        }
        #endregion
    }
}
