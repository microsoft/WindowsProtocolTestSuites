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
    public partial class TestScenarioS34 : PtfTestClassBase {
        
        public TestScenarioS34() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void CloseDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase CloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "Close", typeof(int), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle).MakeByRefType());
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
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS34S0() {
            this.Manager.BeginTest("TestScenarioS34S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp0);
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S18");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S18");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(53,PolicyAccountDomainInformation,out" +
                    " _)\'");
            temp3 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp2);
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp2, "policyInformation of QueryInformationPolicy, state S30");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp3, "return of QueryInformationPolicy, state S30");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAccountDomainInformation,ou" +
                    "t _)\'");
            temp5 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp4);
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp4, "policyInformation of QueryInformationPolicy2, state S42");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp5, "return of QueryInformationPolicy2, state S42");
            TestScenarioS34S48();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS34S48() {
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.Close(1, out temp6);
            this.Manager.AddReturn(CloseInfo, null, temp6, temp7);
            TestScenarioS34S51();
        }
        
        private void TestScenarioS34S51() {
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS34.CloseInfo, null, new CloseDelegate1(this.TestScenarioS34S0CloseChecker)));
            this.Manager.Comment("reaching state \'S52\'");
        }
        
        private void TestScenarioS34S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S51");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S51");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS34S10() {
            this.Manager.BeginTest("TestScenarioS34S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S17\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp9 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp8);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp8, "policyHandle of OpenPolicy2, state S23");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp9, "return of OpenPolicy2, state S23");
            this.Manager.Comment("reaching state \'S29\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(53,PolicyAccountDomainInformation,out" +
                    " _)\'");
            temp11 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp10);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp10, "policyInformation of QueryInformationPolicy, state S35");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp11, "return of QueryInformationPolicy, state S35");
            this.Manager.Comment("reaching state \'S41\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAccountDomainInformation,ou" +
                    "t _)\'");
            temp13 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp12);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp12, "policyInformation of QueryInformationPolicy2, state S47");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp13, "return of QueryInformationPolicy2, state S47");
            TestScenarioS34S50();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS34S50() {
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.Close(1, out temp14);
            this.Manager.AddReturn(CloseInfo, null, temp14, temp15);
            TestScenarioS34S51();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS34S2() {
            this.Manager.BeginTest("TestScenarioS34S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S13\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp16);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp16, "policyHandle of OpenPolicy2, state S19");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of OpenPolicy2, state S19");
            this.Manager.Comment("reaching state \'S25\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyAccountDomainInformation,out " +
                    "_)\'");
            temp19 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp18);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp18, "policyInformation of QueryInformationPolicy, state S31");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp19, "return of QueryInformationPolicy, state S31");
            this.Manager.Comment("reaching state \'S37\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAccountDomainInformation,out" +
                    " _)\'");
            temp21 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp20);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp20, "policyInformation of QueryInformationPolicy2, state S43");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp21, "return of QueryInformationPolicy2, state S43");
            TestScenarioS34S49();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS34S49() {
            this.Manager.Comment("reaching state \'S49\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.Close(1, out temp22);
            this.Manager.AddReturn(CloseInfo, null, temp22, temp23);
            TestScenarioS34S51();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS34S4() {
            this.Manager.BeginTest("TestScenarioS34S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp24);
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp24, "policyHandle of OpenPolicy2, state S20");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp25, "return of OpenPolicy2, state S20");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyAccountDomainInformation,out " +
                    "_)\'");
            temp27 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp26);
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp26, "policyInformation of QueryInformationPolicy, state S32");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of QueryInformationPolicy, state S32");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAccountDomainInformation,out" +
                    " _)\'");
            temp29 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp28);
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp28, "policyInformation of QueryInformationPolicy2, state S44");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of QueryInformationPolicy2, state S44");
            TestScenarioS34S50();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS34S6() {
            this.Manager.BeginTest("TestScenarioS34S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S15\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp31 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp30);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp30, "policyHandle of OpenPolicy2, state S21");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp31, "return of OpenPolicy2, state S21");
            this.Manager.Comment("reaching state \'S27\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(1,PolicyAccountDomainInformation,out " +
                    "_)\'");
            temp33 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp32);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp32, "policyInformation of QueryInformationPolicy, state S33");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp33, "return of QueryInformationPolicy, state S33");
            this.Manager.Comment("reaching state \'S39\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAccountDomainInformation,out" +
                    " _)\'");
            temp35 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp34);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp34, "policyInformation of QueryInformationPolicy2, state S45");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp35, "return of QueryInformationPolicy2, state S45");
            TestScenarioS34S48();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS34S8() {
            this.Manager.BeginTest("TestScenarioS34S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp37 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp36);
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp36, "policyHandle of OpenPolicy2, state S22");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp37, "return of OpenPolicy2, state S22");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call QueryInformationPolicy(53,PolicyAccountDomainInformation,out" +
                    " _)\'");
            temp39 = this.ILsadManagedAdapterInstance.QueryInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp38);
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp38, "policyInformation of QueryInformationPolicy, state S34");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp39, "return of QueryInformationPolicy, state S34");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAccountDomainInformation,ou" +
                    "t _)\'");
            temp41 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp40);
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp40, "policyInformation of QueryInformationPolicy2, state S46");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp41, "return of QueryInformationPolicy2, state S46");
            TestScenarioS34S49();
            this.Manager.EndTest();
        }
        #endregion
    }
}
