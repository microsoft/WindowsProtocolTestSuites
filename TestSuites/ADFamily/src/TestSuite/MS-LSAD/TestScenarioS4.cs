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
    public partial class TestScenarioS4 : PtfTestClassBase {
        
        public TestScenarioS4() {
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
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S0() {
            this.Manager.BeginTest("TestScenarioS4S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp0);
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S216");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S216");
            this.Manager.Comment("reaching state \'S288\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditEventsInformation,out " +
                    "_)\'");
            temp3 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation, out temp2);
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp2, "policyInformation of QueryInformationPolicy2, state S360");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp3, "return of QueryInformationPolicy2, state S360");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S432() {
            this.Manager.Comment("reaching state \'S432\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.Close(1, out temp4);
            this.Manager.AddReturn(CloseInfo, null, temp4, temp5);
            TestScenarioS4S435();
        }
        
        private void TestScenarioS4S435() {
            this.Manager.Comment("reaching state \'S435\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.CloseInfo, null, new CloseDelegate1(this.TestScenarioS4S0CloseChecker)));
            this.Manager.Comment("reaching state \'S436\'");
        }
        
        private void TestScenarioS4S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S435");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S435");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S10() {
            this.Manager.BeginTest("TestScenarioS4S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp6);
            this.Manager.Comment("reaching state \'S221\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp6, "policyHandle of OpenPolicy2, state S221");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp7, "return of OpenPolicy2, state S221");
            this.Manager.Comment("reaching state \'S293\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditEventsInformation,out _" +
                    ")\'");
            temp9 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation, out temp8);
            this.Manager.Comment("reaching state \'S365\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp8, "policyInformation of QueryInformationPolicy2, state S365");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp9, "return of QueryInformationPolicy2, state S365");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S433() {
            this.Manager.Comment("reaching state \'S433\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.Close(1, out temp10);
            this.Manager.AddReturn(CloseInfo, null, temp10, temp11);
            TestScenarioS4S435();
        }
        #endregion
        
        #region Test Starting in S100
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S100() {
            this.Manager.BeginTest("TestScenarioS4S100");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S194\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp12);
            this.Manager.Comment("reaching state \'S266\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp12, "policyHandle of OpenPolicy2, state S266");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp13, "return of OpenPolicy2, state S266");
            this.Manager.Comment("reaching state \'S338\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyPrimaryDomainInformation,out" +
                    " _)\'");
            temp15 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation, out temp14);
            this.Manager.Comment("reaching state \'S410\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp14, "policyInformation of QueryInformationPolicy2, state S410");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp15, "return of QueryInformationPolicy2, state S410");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S102
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S102() {
            this.Manager.BeginTest("TestScenarioS4S102");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S195\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp16);
            this.Manager.Comment("reaching state \'S267\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp16, "policyHandle of OpenPolicy2, state S267");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of OpenPolicy2, state S267");
            this.Manager.Comment("reaching state \'S339\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditEventsInformation,out _" +
                    ")\'");
            temp19 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation, out temp18);
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp18, "policyInformation of QueryInformationPolicy2, state S411");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp19, "return of QueryInformationPolicy2, state S411");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S104
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S104() {
            this.Manager.BeginTest("TestScenarioS4S104");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S196\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp21 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp20);
            this.Manager.Comment("reaching state \'S268\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp20, "policyHandle of OpenPolicy2, state S268");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp21, "return of OpenPolicy2, state S268");
            this.Manager.Comment("reaching state \'S340\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAccountDomainInformation,out" +
                    " _)\'");
            temp23 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp22);
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp22, "policyInformation of QueryInformationPolicy2, state S412");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp23, "return of QueryInformationPolicy2, state S412");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S106
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S106() {
            this.Manager.BeginTest("TestScenarioS4S106");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S197\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp24);
            this.Manager.Comment("reaching state \'S269\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp24, "policyHandle of OpenPolicy2, state S269");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp25, "return of OpenPolicy2, state S269");
            this.Manager.Comment("reaching state \'S341\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditLogInformation,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass)(1)), out temp26);
            this.Manager.Comment("reaching state \'S413\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp26, "policyInformation of QueryInformationPolicy2, state S413");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of QueryInformationPolicy2, state S413");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S108
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S108() {
            this.Manager.BeginTest("TestScenarioS4S108");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S198\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp28);
            this.Manager.Comment("reaching state \'S270\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp28, "policyHandle of OpenPolicy2, state S270");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of OpenPolicy2, state S270");
            this.Manager.Comment("reaching state \'S342\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyDnsDomainInformation,out _)\'" +
                    "");
            temp31 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation, out temp30);
            this.Manager.Comment("reaching state \'S414\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp30, "policyInformation of QueryInformationPolicy2, state S414");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp31, "return of QueryInformationPolicy2, state S414");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S110
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S110() {
            this.Manager.BeginTest("TestScenarioS4S110");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S199\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp33 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp32);
            this.Manager.Comment("reaching state \'S271\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp32, "policyHandle of OpenPolicy2, state S271");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp33, "return of OpenPolicy2, state S271");
            this.Manager.Comment("reaching state \'S343\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyLsaServerRoleInformation,out" +
                    " _)\'");
            temp35 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation, out temp34);
            this.Manager.Comment("reaching state \'S415\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp34, "policyInformation of QueryInformationPolicy2, state S415");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp35, "return of QueryInformationPolicy2, state S415");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S112
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S112() {
            this.Manager.BeginTest("TestScenarioS4S112");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S200\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp37 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp36);
            this.Manager.Comment("reaching state \'S272\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp36, "policyHandle of OpenPolicy2, state S272");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp37, "return of OpenPolicy2, state S272");
            this.Manager.Comment("reaching state \'S344\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditFullQueryInformation,o" +
                    "ut _)\'");
            temp39 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation, out temp38);
            this.Manager.Comment("reaching state \'S416\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp38, "policyInformation of QueryInformationPolicy2, state S416");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp39, "return of QueryInformationPolicy2, state S416");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S114
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S114() {
            this.Manager.BeginTest("TestScenarioS4S114");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S201\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp40);
            this.Manager.Comment("reaching state \'S273\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp40, "policyHandle of OpenPolicy2, state S273");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of OpenPolicy2, state S273");
            this.Manager.Comment("reaching state \'S345\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyModificationInformation,out" +
                    " _)\'");
            temp43 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation, out temp42);
            this.Manager.Comment("reaching state \'S417\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp42, "policyInformation of QueryInformationPolicy2, state S417");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp43, "return of QueryInformationPolicy2, state S417");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S116
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S116() {
            this.Manager.BeginTest("TestScenarioS4S116");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S202\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp44;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp45 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp44);
            this.Manager.Comment("reaching state \'S274\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp44, "policyHandle of OpenPolicy2, state S274");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp45, "return of OpenPolicy2, state S274");
            this.Manager.Comment("reaching state \'S346\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp46;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyModificationInformation,out " +
                    "_)\'");
            temp47 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation, out temp46);
            this.Manager.Comment("reaching state \'S418\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp46, "policyInformation of QueryInformationPolicy2, state S418");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp47, "return of QueryInformationPolicy2, state S418");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S118
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S118() {
            this.Manager.BeginTest("TestScenarioS4S118");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S203\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp48);
            this.Manager.Comment("reaching state \'S275\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp48, "policyHandle of OpenPolicy2, state S275");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp49, "return of OpenPolicy2, state S275");
            this.Manager.Comment("reaching state \'S347\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp50;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditFullSetInformation,out" +
                    " _)\'");
            temp51 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation, out temp50);
            this.Manager.Comment("reaching state \'S419\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp50, "policyInformation of QueryInformationPolicy2, state S419");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp51, "return of QueryInformationPolicy2, state S419");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S12() {
            this.Manager.BeginTest("TestScenarioS4S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp53 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp52);
            this.Manager.Comment("reaching state \'S222\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp52, "policyHandle of OpenPolicy2, state S222");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp53, "return of OpenPolicy2, state S222");
            this.Manager.Comment("reaching state \'S294\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAccountDomainInformation,out" +
                    " _)\'");
            temp55 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp54);
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp54, "policyInformation of QueryInformationPolicy2, state S366");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp55, "return of QueryInformationPolicy2, state S366");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S120
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S120() {
            this.Manager.BeginTest("TestScenarioS4S120");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S204\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp56;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp57;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp57 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp56);
            this.Manager.Comment("reaching state \'S276\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp56, "policyHandle of OpenPolicy2, state S276");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp57, "return of OpenPolicy2, state S276");
            this.Manager.Comment("reaching state \'S348\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp58;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditFullSetInformation,out " +
                    "_)\'");
            temp59 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation, out temp58);
            this.Manager.Comment("reaching state \'S420\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp58, "policyInformation of QueryInformationPolicy2, state S420");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp59, "return of QueryInformationPolicy2, state S420");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S122
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S122() {
            this.Manager.BeginTest("TestScenarioS4S122");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S205\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp60;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp61 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp60);
            this.Manager.Comment("reaching state \'S277\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp60, "policyHandle of OpenPolicy2, state S277");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp61, "return of OpenPolicy2, state S277");
            this.Manager.Comment("reaching state \'S349\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditFullQueryInformation,ou" +
                    "t _)\'");
            temp63 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation, out temp62);
            this.Manager.Comment("reaching state \'S421\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp62, "policyInformation of QueryInformationPolicy2, state S421");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp63, "return of QueryInformationPolicy2, state S421");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S124
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S124() {
            this.Manager.BeginTest("TestScenarioS4S124");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S206\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp64;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp65;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp65 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp64);
            this.Manager.Comment("reaching state \'S278\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp64, "policyHandle of OpenPolicy2, state S278");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp65, "return of OpenPolicy2, state S278");
            this.Manager.Comment("reaching state \'S350\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp66;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditLogInformation,out _)\'" +
                    "");
            temp67 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass)(1)), out temp66);
            this.Manager.Comment("reaching state \'S422\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp66, "policyInformation of QueryInformationPolicy2, state S422");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp67, "return of QueryInformationPolicy2, state S422");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S126
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S126() {
            this.Manager.BeginTest("TestScenarioS4S126");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S207\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp68);
            this.Manager.Comment("reaching state \'S279\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp68, "policyHandle of OpenPolicy2, state S279");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp69, "return of OpenPolicy2, state S279");
            this.Manager.Comment("reaching state \'S351\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyPrimaryDomainInformation,ou" +
                    "t _)\'");
            temp71 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation, out temp70);
            this.Manager.Comment("reaching state \'S423\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp70, "policyInformation of QueryInformationPolicy2, state S423");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp71, "return of QueryInformationPolicy2, state S423");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S128
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S128() {
            this.Manager.BeginTest("TestScenarioS4S128");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S208\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp72;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp73 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp72);
            this.Manager.Comment("reaching state \'S280\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp72, "policyHandle of OpenPolicy2, state S280");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp73, "return of OpenPolicy2, state S280");
            this.Manager.Comment("reaching state \'S352\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp74;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp75;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyPdAccountInformation,out _)" +
                    "\'");
            temp75 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation, out temp74);
            this.Manager.Comment("reaching state \'S424\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp74, "policyInformation of QueryInformationPolicy2, state S424");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp75, "return of QueryInformationPolicy2, state S424");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S130
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S130() {
            this.Manager.BeginTest("TestScenarioS4S130");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S209\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp76;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp77 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp76);
            this.Manager.Comment("reaching state \'S281\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp76, "policyHandle of OpenPolicy2, state S281");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp77, "return of OpenPolicy2, state S281");
            this.Manager.Comment("reaching state \'S353\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp78;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAccountDomainInformation,ou" +
                    "t _)\'");
            temp79 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp78);
            this.Manager.Comment("reaching state \'S425\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp78, "policyInformation of QueryInformationPolicy2, state S425");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp79, "return of QueryInformationPolicy2, state S425");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S132
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S132() {
            this.Manager.BeginTest("TestScenarioS4S132");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S210\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp80;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp81 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp80);
            this.Manager.Comment("reaching state \'S282\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp80, "policyHandle of OpenPolicy2, state S282");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp81, "return of OpenPolicy2, state S282");
            this.Manager.Comment("reaching state \'S354\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp82;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp83;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyLsaServerRoleInformation,ou" +
                    "t _)\'");
            temp83 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation, out temp82);
            this.Manager.Comment("reaching state \'S426\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp82, "policyInformation of QueryInformationPolicy2, state S426");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp83, "return of QueryInformationPolicy2, state S426");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S134
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S134() {
            this.Manager.BeginTest("TestScenarioS4S134");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S211\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp84;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp85 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp84);
            this.Manager.Comment("reaching state \'S283\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp84, "policyHandle of OpenPolicy2, state S283");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp85, "return of OpenPolicy2, state S283");
            this.Manager.Comment("reaching state \'S355\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp86;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyReplicaSourceInformation,ou" +
                    "t _)\'");
            temp87 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation, out temp86);
            this.Manager.Comment("reaching state \'S427\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp86, "policyInformation of QueryInformationPolicy2, state S427");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp87, "return of QueryInformationPolicy2, state S427");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S136
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S136() {
            this.Manager.BeginTest("TestScenarioS4S136");
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S212\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp88;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp89 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp88);
            this.Manager.Comment("reaching state \'S284\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp88, "policyHandle of OpenPolicy2, state S284");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp89, "return of OpenPolicy2, state S284");
            this.Manager.Comment("reaching state \'S356\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp90;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp91;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyDnsDomainInformationInt,out" +
                    " _)\'");
            temp91 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt, out temp90);
            this.Manager.Comment("reaching state \'S428\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp90, "policyInformation of QueryInformationPolicy2, state S428");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp91, "return of QueryInformationPolicy2, state S428");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S138
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S138() {
            this.Manager.BeginTest("TestScenarioS4S138");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S213\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp92;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp93;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp93 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp92);
            this.Manager.Comment("reaching state \'S285\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp92, "policyHandle of OpenPolicy2, state S285");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp93, "return of OpenPolicy2, state S285");
            this.Manager.Comment("reaching state \'S357\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp94;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp95;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyDnsDomainInformation,out _)" +
                    "\'");
            temp95 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation, out temp94);
            this.Manager.Comment("reaching state \'S429\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp94, "policyInformation of QueryInformationPolicy2, state S429");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp95, "return of QueryInformationPolicy2, state S429");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S14() {
            this.Manager.BeginTest("TestScenarioS4S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp96;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp97 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp96);
            this.Manager.Comment("reaching state \'S223\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp96, "policyHandle of OpenPolicy2, state S223");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp97, "return of OpenPolicy2, state S223");
            this.Manager.Comment("reaching state \'S295\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp98;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp99;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditLogInformation,out _)\'");
            temp99 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass)(1)), out temp98);
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp98, "policyInformation of QueryInformationPolicy2, state S367");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp99, "return of QueryInformationPolicy2, state S367");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S140
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S140() {
            this.Manager.BeginTest("TestScenarioS4S140");
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S214\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp100;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp101 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp100);
            this.Manager.Comment("reaching state \'S286\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp100, "policyHandle of OpenPolicy2, state S286");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp101, "return of OpenPolicy2, state S286");
            this.Manager.Comment("reaching state \'S358\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp102;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditEventsInformation,out " +
                    "_)\'");
            temp103 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation, out temp102);
            this.Manager.Comment("reaching state \'S430\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp102, "policyInformation of QueryInformationPolicy2, state S430");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp103, "return of QueryInformationPolicy2, state S430");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S142
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S142() {
            this.Manager.BeginTest("TestScenarioS4S142");
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S215\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp104;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp105 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp104);
            this.Manager.Comment("reaching state \'S287\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp104, "policyHandle of OpenPolicy2, state S287");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp105, "return of OpenPolicy2, state S287");
            this.Manager.Comment("reaching state \'S359\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp106;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp107;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditEventsInformation,out " +
                    "_)\'");
            temp107 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation, out temp106);
            this.Manager.Comment("reaching state \'S431\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp106, "policyInformation of QueryInformationPolicy2, state S431");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp107, "return of QueryInformationPolicy2, state S431");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S434() {
            this.Manager.Comment("reaching state \'S434\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp108;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp109;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp109 = this.ILsadManagedAdapterInstance.Close(1, out temp108);
            this.Manager.AddReturn(CloseInfo, null, temp108, temp109);
            TestScenarioS4S435();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S16() {
            this.Manager.BeginTest("TestScenarioS4S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp110;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp111;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp111 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp110);
            this.Manager.Comment("reaching state \'S224\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp110, "policyHandle of OpenPolicy2, state S224");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp111, "return of OpenPolicy2, state S224");
            this.Manager.Comment("reaching state \'S296\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp112;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp113;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyDnsDomainInformation,out _)\'" +
                    "");
            temp113 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation, out temp112);
            this.Manager.Comment("reaching state \'S368\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp112, "policyInformation of QueryInformationPolicy2, state S368");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp113, "return of QueryInformationPolicy2, state S368");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S18() {
            this.Manager.BeginTest("TestScenarioS4S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp114;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp115 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp114);
            this.Manager.Comment("reaching state \'S225\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp114, "policyHandle of OpenPolicy2, state S225");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp115, "return of OpenPolicy2, state S225");
            this.Manager.Comment("reaching state \'S297\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp116;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyLsaServerRoleInformation,out" +
                    " _)\'");
            temp117 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation, out temp116);
            this.Manager.Comment("reaching state \'S369\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp116, "policyInformation of QueryInformationPolicy2, state S369");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp117, "return of QueryInformationPolicy2, state S369");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S2() {
            this.Manager.BeginTest("TestScenarioS4S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp118;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp119;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp119 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp118);
            this.Manager.Comment("reaching state \'S217\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp118, "policyHandle of OpenPolicy2, state S217");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp119, "return of OpenPolicy2, state S217");
            this.Manager.Comment("reaching state \'S289\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp120;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp121;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyReplicaSourceInformation,out" +
                    " _)\'");
            temp121 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation, out temp120);
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp120, "policyInformation of QueryInformationPolicy2, state S361");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp121, "return of QueryInformationPolicy2, state S361");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S20() {
            this.Manager.BeginTest("TestScenarioS4S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp122;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp123;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp123 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp122);
            this.Manager.Comment("reaching state \'S226\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp122, "policyHandle of OpenPolicy2, state S226");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp123, "return of OpenPolicy2, state S226");
            this.Manager.Comment("reaching state \'S298\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp124;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp125;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditFullQueryInformation,o" +
                    "ut _)\'");
            temp125 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation, out temp124);
            this.Manager.Comment("reaching state \'S370\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp124, "policyInformation of QueryInformationPolicy2, state S370");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp125, "return of QueryInformationPolicy2, state S370");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S22() {
            this.Manager.BeginTest("TestScenarioS4S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp126;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp127;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp127 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp126);
            this.Manager.Comment("reaching state \'S227\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp126, "policyHandle of OpenPolicy2, state S227");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp127, "return of OpenPolicy2, state S227");
            this.Manager.Comment("reaching state \'S299\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp128;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp129;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyModificationInformation,out" +
                    " _)\'");
            temp129 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation, out temp128);
            this.Manager.Comment("reaching state \'S371\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp128, "policyInformation of QueryInformationPolicy2, state S371");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp129, "return of QueryInformationPolicy2, state S371");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S24() {
            this.Manager.BeginTest("TestScenarioS4S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S156\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp130;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp131;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp131 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp130);
            this.Manager.Comment("reaching state \'S228\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp130, "policyHandle of OpenPolicy2, state S228");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp131, "return of OpenPolicy2, state S228");
            this.Manager.Comment("reaching state \'S300\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp132;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp133;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyModificationInformation,out " +
                    "_)\'");
            temp133 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation, out temp132);
            this.Manager.Comment("reaching state \'S372\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp132, "policyInformation of QueryInformationPolicy2, state S372");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp133, "return of QueryInformationPolicy2, state S372");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S26() {
            this.Manager.BeginTest("TestScenarioS4S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S157\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp134;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp135;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp135 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp134);
            this.Manager.Comment("reaching state \'S229\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp134, "policyHandle of OpenPolicy2, state S229");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp135, "return of OpenPolicy2, state S229");
            this.Manager.Comment("reaching state \'S301\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp136;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp137;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditFullSetInformation,out" +
                    " _)\'");
            temp137 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation, out temp136);
            this.Manager.Comment("reaching state \'S373\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp136, "policyInformation of QueryInformationPolicy2, state S373");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp137, "return of QueryInformationPolicy2, state S373");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S28() {
            this.Manager.BeginTest("TestScenarioS4S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S158\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp138;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp139;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp139 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp138);
            this.Manager.Comment("reaching state \'S230\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp138, "policyHandle of OpenPolicy2, state S230");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp139, "return of OpenPolicy2, state S230");
            this.Manager.Comment("reaching state \'S302\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp140;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp141;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditFullSetInformation,out " +
                    "_)\'");
            temp141 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation, out temp140);
            this.Manager.Comment("reaching state \'S374\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp140, "policyInformation of QueryInformationPolicy2, state S374");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp141, "return of QueryInformationPolicy2, state S374");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S30() {
            this.Manager.BeginTest("TestScenarioS4S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S159\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp142;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp143;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp143 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp142);
            this.Manager.Comment("reaching state \'S231\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp142, "policyHandle of OpenPolicy2, state S231");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp143, "return of OpenPolicy2, state S231");
            this.Manager.Comment("reaching state \'S303\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp144;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp145;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditFullQueryInformation,ou" +
                    "t _)\'");
            temp145 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation, out temp144);
            this.Manager.Comment("reaching state \'S375\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp144, "policyInformation of QueryInformationPolicy2, state S375");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp145, "return of QueryInformationPolicy2, state S375");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S32() {
            this.Manager.BeginTest("TestScenarioS4S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp146;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp147;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp147 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp146);
            this.Manager.Comment("reaching state \'S232\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp146, "policyHandle of OpenPolicy2, state S232");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp147, "return of OpenPolicy2, state S232");
            this.Manager.Comment("reaching state \'S304\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp148;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp149;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditLogInformation,out _)\'" +
                    "");
            temp149 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass)(1)), out temp148);
            this.Manager.Comment("reaching state \'S376\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp148, "policyInformation of QueryInformationPolicy2, state S376");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp149, "return of QueryInformationPolicy2, state S376");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S34() {
            this.Manager.BeginTest("TestScenarioS4S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp150;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp151;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp151 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp150);
            this.Manager.Comment("reaching state \'S233\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp150, "policyHandle of OpenPolicy2, state S233");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp151, "return of OpenPolicy2, state S233");
            this.Manager.Comment("reaching state \'S305\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp152;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp153;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyPrimaryDomainInformation,ou" +
                    "t _)\'");
            temp153 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation, out temp152);
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp152, "policyInformation of QueryInformationPolicy2, state S377");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp153, "return of QueryInformationPolicy2, state S377");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S36() {
            this.Manager.BeginTest("TestScenarioS4S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S162\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp154;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp155;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp155 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp154);
            this.Manager.Comment("reaching state \'S234\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp154, "policyHandle of OpenPolicy2, state S234");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp155, "return of OpenPolicy2, state S234");
            this.Manager.Comment("reaching state \'S306\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp156;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp157;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyPdAccountInformation,out _)" +
                    "\'");
            temp157 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation, out temp156);
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp156, "policyInformation of QueryInformationPolicy2, state S378");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp157, "return of QueryInformationPolicy2, state S378");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S38() {
            this.Manager.BeginTest("TestScenarioS4S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S163\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp158;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp159;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp159 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp158);
            this.Manager.Comment("reaching state \'S235\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp158, "policyHandle of OpenPolicy2, state S235");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp159, "return of OpenPolicy2, state S235");
            this.Manager.Comment("reaching state \'S307\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp160;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp161;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAccountDomainInformation,ou" +
                    "t _)\'");
            temp161 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp160);
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp160, "policyInformation of QueryInformationPolicy2, state S379");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp161, "return of QueryInformationPolicy2, state S379");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S4() {
            this.Manager.BeginTest("TestScenarioS4S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp162;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp163;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp163 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp162);
            this.Manager.Comment("reaching state \'S218\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp162, "policyHandle of OpenPolicy2, state S218");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp163, "return of OpenPolicy2, state S218");
            this.Manager.Comment("reaching state \'S290\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp164;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp165;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyDnsDomainInformationInt,out " +
                    "_)\'");
            temp165 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt, out temp164);
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp164, "policyInformation of QueryInformationPolicy2, state S362");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp165, "return of QueryInformationPolicy2, state S362");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S40() {
            this.Manager.BeginTest("TestScenarioS4S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S164\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp166;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp167;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp167 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp166);
            this.Manager.Comment("reaching state \'S236\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp166, "policyHandle of OpenPolicy2, state S236");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp167, "return of OpenPolicy2, state S236");
            this.Manager.Comment("reaching state \'S308\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp168;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp169;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyLsaServerRoleInformation,ou" +
                    "t _)\'");
            temp169 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation, out temp168);
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp168, "policyInformation of QueryInformationPolicy2, state S380");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp169, "return of QueryInformationPolicy2, state S380");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S42() {
            this.Manager.BeginTest("TestScenarioS4S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S165\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp170;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp171;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp171 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp170);
            this.Manager.Comment("reaching state \'S237\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp170, "policyHandle of OpenPolicy2, state S237");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp171, "return of OpenPolicy2, state S237");
            this.Manager.Comment("reaching state \'S309\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp172;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp173;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyReplicaSourceInformation,ou" +
                    "t _)\'");
            temp173 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation, out temp172);
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp172, "policyInformation of QueryInformationPolicy2, state S381");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp173, "return of QueryInformationPolicy2, state S381");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S44() {
            this.Manager.BeginTest("TestScenarioS4S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S166\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp174;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp175;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp175 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp174);
            this.Manager.Comment("reaching state \'S238\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp174, "policyHandle of OpenPolicy2, state S238");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp175, "return of OpenPolicy2, state S238");
            this.Manager.Comment("reaching state \'S310\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp176;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp177;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyDnsDomainInformationInt,out" +
                    " _)\'");
            temp177 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt, out temp176);
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp176, "policyInformation of QueryInformationPolicy2, state S382");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp177, "return of QueryInformationPolicy2, state S382");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S46() {
            this.Manager.BeginTest("TestScenarioS4S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S167\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp178;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp179;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp179 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp178);
            this.Manager.Comment("reaching state \'S239\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp178, "policyHandle of OpenPolicy2, state S239");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp179, "return of OpenPolicy2, state S239");
            this.Manager.Comment("reaching state \'S311\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp180;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp181;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyDnsDomainInformation,out _)" +
                    "\'");
            temp181 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation, out temp180);
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp180, "policyInformation of QueryInformationPolicy2, state S383");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp181, "return of QueryInformationPolicy2, state S383");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S48() {
            this.Manager.BeginTest("TestScenarioS4S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S168\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp182;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp183;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp183 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp182);
            this.Manager.Comment("reaching state \'S240\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp182, "policyHandle of OpenPolicy2, state S240");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp183, "return of OpenPolicy2, state S240");
            this.Manager.Comment("reaching state \'S312\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp184;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp185;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyReplicaSourceInformation,out" +
                    " _)\'");
            temp185 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation, out temp184);
            this.Manager.Comment("reaching state \'S384\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp184, "policyInformation of QueryInformationPolicy2, state S384");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp185, "return of QueryInformationPolicy2, state S384");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S50() {
            this.Manager.BeginTest("TestScenarioS4S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S169\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp186;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp187;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp187 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp186);
            this.Manager.Comment("reaching state \'S241\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp186, "policyHandle of OpenPolicy2, state S241");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp187, "return of OpenPolicy2, state S241");
            this.Manager.Comment("reaching state \'S313\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp188;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp189;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyDnsDomainInformationInt,out " +
                    "_)\'");
            temp189 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt, out temp188);
            this.Manager.Comment("reaching state \'S385\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp188, "policyInformation of QueryInformationPolicy2, state S385");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp189, "return of QueryInformationPolicy2, state S385");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S52() {
            this.Manager.BeginTest("TestScenarioS4S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S170\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp190;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp191;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp191 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp190);
            this.Manager.Comment("reaching state \'S242\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp190, "policyHandle of OpenPolicy2, state S242");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp191, "return of OpenPolicy2, state S242");
            this.Manager.Comment("reaching state \'S314\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp192;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp193;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyPdAccountInformation,out _)\'" +
                    "");
            temp193 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation, out temp192);
            this.Manager.Comment("reaching state \'S386\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp192, "policyInformation of QueryInformationPolicy2, state S386");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp193, "return of QueryInformationPolicy2, state S386");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S54() {
            this.Manager.BeginTest("TestScenarioS4S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S171\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp194;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp195;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp195 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp194);
            this.Manager.Comment("reaching state \'S243\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp194, "policyHandle of OpenPolicy2, state S243");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp195, "return of OpenPolicy2, state S243");
            this.Manager.Comment("reaching state \'S315\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp196;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp197;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyPrimaryDomainInformation,out" +
                    " _)\'");
            temp197 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation, out temp196);
            this.Manager.Comment("reaching state \'S387\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp196, "policyInformation of QueryInformationPolicy2, state S387");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp197, "return of QueryInformationPolicy2, state S387");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S56() {
            this.Manager.BeginTest("TestScenarioS4S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S172\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp198;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp199;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp199 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp198);
            this.Manager.Comment("reaching state \'S244\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp198, "policyHandle of OpenPolicy2, state S244");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp199, "return of OpenPolicy2, state S244");
            this.Manager.Comment("reaching state \'S316\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp200;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp201;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditEventsInformation,out _" +
                    ")\'");
            temp201 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation, out temp200);
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp200, "policyInformation of QueryInformationPolicy2, state S388");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp201, "return of QueryInformationPolicy2, state S388");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S58() {
            this.Manager.BeginTest("TestScenarioS4S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S173\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp202;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp203;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp203 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp202);
            this.Manager.Comment("reaching state \'S245\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp202, "policyHandle of OpenPolicy2, state S245");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp203, "return of OpenPolicy2, state S245");
            this.Manager.Comment("reaching state \'S317\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp204;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp205;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAccountDomainInformation,out" +
                    " _)\'");
            temp205 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp204);
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp204, "policyInformation of QueryInformationPolicy2, state S389");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp205, "return of QueryInformationPolicy2, state S389");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S6() {
            this.Manager.BeginTest("TestScenarioS4S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp206;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp207;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp207 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp206);
            this.Manager.Comment("reaching state \'S219\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp206, "policyHandle of OpenPolicy2, state S219");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp207, "return of OpenPolicy2, state S219");
            this.Manager.Comment("reaching state \'S291\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp208;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp209;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyPdAccountInformation,out _)\'" +
                    "");
            temp209 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation, out temp208);
            this.Manager.Comment("reaching state \'S363\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp208, "policyInformation of QueryInformationPolicy2, state S363");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp209, "return of QueryInformationPolicy2, state S363");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S60() {
            this.Manager.BeginTest("TestScenarioS4S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S174\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp210;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp211;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp211 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp210);
            this.Manager.Comment("reaching state \'S246\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp210, "policyHandle of OpenPolicy2, state S246");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp211, "return of OpenPolicy2, state S246");
            this.Manager.Comment("reaching state \'S318\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp212;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp213;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditLogInformation,out _)\'");
            temp213 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass)(1)), out temp212);
            this.Manager.Comment("reaching state \'S390\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp212, "policyInformation of QueryInformationPolicy2, state S390");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp213, "return of QueryInformationPolicy2, state S390");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S62() {
            this.Manager.BeginTest("TestScenarioS4S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S175\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp214;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp215;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp215 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp214);
            this.Manager.Comment("reaching state \'S247\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp214, "policyHandle of OpenPolicy2, state S247");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp215, "return of OpenPolicy2, state S247");
            this.Manager.Comment("reaching state \'S319\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp216;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp217;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyDnsDomainInformation,out _)\'" +
                    "");
            temp217 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation, out temp216);
            this.Manager.Comment("reaching state \'S391\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp216, "policyInformation of QueryInformationPolicy2, state S391");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp217, "return of QueryInformationPolicy2, state S391");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S64() {
            this.Manager.BeginTest("TestScenarioS4S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S176\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp218;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp219;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp219 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp218);
            this.Manager.Comment("reaching state \'S248\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp218, "policyHandle of OpenPolicy2, state S248");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp219, "return of OpenPolicy2, state S248");
            this.Manager.Comment("reaching state \'S320\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp220;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp221;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyLsaServerRoleInformation,out" +
                    " _)\'");
            temp221 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation, out temp220);
            this.Manager.Comment("reaching state \'S392\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp220, "policyInformation of QueryInformationPolicy2, state S392");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp221, "return of QueryInformationPolicy2, state S392");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S66() {
            this.Manager.BeginTest("TestScenarioS4S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S177\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp222;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp223;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp223 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp222);
            this.Manager.Comment("reaching state \'S249\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp222, "policyHandle of OpenPolicy2, state S249");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp223, "return of OpenPolicy2, state S249");
            this.Manager.Comment("reaching state \'S321\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp224;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp225;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditFullQueryInformation,o" +
                    "ut _)\'");
            temp225 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation, out temp224);
            this.Manager.Comment("reaching state \'S393\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp224, "policyInformation of QueryInformationPolicy2, state S393");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp225, "return of QueryInformationPolicy2, state S393");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S68() {
            this.Manager.BeginTest("TestScenarioS4S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S178\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp226;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp227;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp227 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp226);
            this.Manager.Comment("reaching state \'S250\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp226, "policyHandle of OpenPolicy2, state S250");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp227, "return of OpenPolicy2, state S250");
            this.Manager.Comment("reaching state \'S322\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp228;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp229;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyModificationInformation,out" +
                    " _)\'");
            temp229 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation, out temp228);
            this.Manager.Comment("reaching state \'S394\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp228, "policyInformation of QueryInformationPolicy2, state S394");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp229, "return of QueryInformationPolicy2, state S394");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S70() {
            this.Manager.BeginTest("TestScenarioS4S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S179\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp230;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp231;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp231 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp230);
            this.Manager.Comment("reaching state \'S251\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp230, "policyHandle of OpenPolicy2, state S251");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp231, "return of OpenPolicy2, state S251");
            this.Manager.Comment("reaching state \'S323\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp232;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp233;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyModificationInformation,out " +
                    "_)\'");
            temp233 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation, out temp232);
            this.Manager.Comment("reaching state \'S395\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp232, "policyInformation of QueryInformationPolicy2, state S395");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp233, "return of QueryInformationPolicy2, state S395");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S72
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S72() {
            this.Manager.BeginTest("TestScenarioS4S72");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S180\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp234;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp235;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp235 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp234);
            this.Manager.Comment("reaching state \'S252\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp234, "policyHandle of OpenPolicy2, state S252");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp235, "return of OpenPolicy2, state S252");
            this.Manager.Comment("reaching state \'S324\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp236;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp237;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditFullSetInformation,out" +
                    " _)\'");
            temp237 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation, out temp236);
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp236, "policyInformation of QueryInformationPolicy2, state S396");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp237, "return of QueryInformationPolicy2, state S396");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S74
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S74() {
            this.Manager.BeginTest("TestScenarioS4S74");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S181\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp238;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp239;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp239 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp238);
            this.Manager.Comment("reaching state \'S253\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp238, "policyHandle of OpenPolicy2, state S253");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp239, "return of OpenPolicy2, state S253");
            this.Manager.Comment("reaching state \'S325\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp240;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp241;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditFullSetInformation,out " +
                    "_)\'");
            temp241 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation, out temp240);
            this.Manager.Comment("reaching state \'S397\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp240, "policyInformation of QueryInformationPolicy2, state S397");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp241, "return of QueryInformationPolicy2, state S397");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S76
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S76() {
            this.Manager.BeginTest("TestScenarioS4S76");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S182\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp242;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp243;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp243 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp242);
            this.Manager.Comment("reaching state \'S254\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp242, "policyHandle of OpenPolicy2, state S254");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp243, "return of OpenPolicy2, state S254");
            this.Manager.Comment("reaching state \'S326\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp244;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp245;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyAuditFullQueryInformation,ou" +
                    "t _)\'");
            temp245 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation, out temp244);
            this.Manager.Comment("reaching state \'S398\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp244, "policyInformation of QueryInformationPolicy2, state S398");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp245, "return of QueryInformationPolicy2, state S398");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S78
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S78() {
            this.Manager.BeginTest("TestScenarioS4S78");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S183\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp246;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp247;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp247 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp246);
            this.Manager.Comment("reaching state \'S255\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp246, "policyHandle of OpenPolicy2, state S255");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp247, "return of OpenPolicy2, state S255");
            this.Manager.Comment("reaching state \'S327\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp248;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp249;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAuditLogInformation,out _)\'" +
                    "");
            temp249 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass)(1)), out temp248);
            this.Manager.Comment("reaching state \'S399\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp248, "policyInformation of QueryInformationPolicy2, state S399");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp249, "return of QueryInformationPolicy2, state S399");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S8() {
            this.Manager.BeginTest("TestScenarioS4S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp250;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp251;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp251 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp250);
            this.Manager.Comment("reaching state \'S220\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp250, "policyHandle of OpenPolicy2, state S220");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp251, "return of OpenPolicy2, state S220");
            this.Manager.Comment("reaching state \'S292\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp252;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp253;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyPrimaryDomainInformation,out" +
                    " _)\'");
            temp253 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation, out temp252);
            this.Manager.Comment("reaching state \'S364\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp252, "policyInformation of QueryInformationPolicy2, state S364");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp253, "return of QueryInformationPolicy2, state S364");
            TestScenarioS4S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S80
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S80() {
            this.Manager.BeginTest("TestScenarioS4S80");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S184\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp254;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp255;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp255 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp254);
            this.Manager.Comment("reaching state \'S256\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp254, "policyHandle of OpenPolicy2, state S256");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp255, "return of OpenPolicy2, state S256");
            this.Manager.Comment("reaching state \'S328\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp256;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp257;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyPrimaryDomainInformation,ou" +
                    "t _)\'");
            temp257 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation, out temp256);
            this.Manager.Comment("reaching state \'S400\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp256, "policyInformation of QueryInformationPolicy2, state S400");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp257, "return of QueryInformationPolicy2, state S400");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S82
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S82() {
            this.Manager.BeginTest("TestScenarioS4S82");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S185\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp258;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp259;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp259 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp258);
            this.Manager.Comment("reaching state \'S257\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp258, "policyHandle of OpenPolicy2, state S257");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp259, "return of OpenPolicy2, state S257");
            this.Manager.Comment("reaching state \'S329\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp260;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp261;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyPdAccountInformation,out _)" +
                    "\'");
            temp261 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation, out temp260);
            this.Manager.Comment("reaching state \'S401\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp260, "policyInformation of QueryInformationPolicy2, state S401");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp261, "return of QueryInformationPolicy2, state S401");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S84
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S84() {
            this.Manager.BeginTest("TestScenarioS4S84");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S186\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp262;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp263;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp263 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp262);
            this.Manager.Comment("reaching state \'S258\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp262, "policyHandle of OpenPolicy2, state S258");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp263, "return of OpenPolicy2, state S258");
            this.Manager.Comment("reaching state \'S330\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp264;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp265;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyAccountDomainInformation,ou" +
                    "t _)\'");
            temp265 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation, out temp264);
            this.Manager.Comment("reaching state \'S402\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp264, "policyInformation of QueryInformationPolicy2, state S402");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp265, "return of QueryInformationPolicy2, state S402");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S86
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S86() {
            this.Manager.BeginTest("TestScenarioS4S86");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S187\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp266;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp267;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp267 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp266);
            this.Manager.Comment("reaching state \'S259\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp266, "policyHandle of OpenPolicy2, state S259");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp267, "return of OpenPolicy2, state S259");
            this.Manager.Comment("reaching state \'S331\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp268;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp269;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyLsaServerRoleInformation,ou" +
                    "t _)\'");
            temp269 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation, out temp268);
            this.Manager.Comment("reaching state \'S403\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp268, "policyInformation of QueryInformationPolicy2, state S403");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp269, "return of QueryInformationPolicy2, state S403");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S88
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S88() {
            this.Manager.BeginTest("TestScenarioS4S88");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S188\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp270;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp271;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp271 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp270);
            this.Manager.Comment("reaching state \'S260\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp270, "policyHandle of OpenPolicy2, state S260");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp271, "return of OpenPolicy2, state S260");
            this.Manager.Comment("reaching state \'S332\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp272;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp273;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyReplicaSourceInformation,ou" +
                    "t _)\'");
            temp273 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation, out temp272);
            this.Manager.Comment("reaching state \'S404\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp272, "policyInformation of QueryInformationPolicy2, state S404");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp273, "return of QueryInformationPolicy2, state S404");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S90
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S90() {
            this.Manager.BeginTest("TestScenarioS4S90");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S189\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp274;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp275;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp275 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp274);
            this.Manager.Comment("reaching state \'S261\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp274, "policyHandle of OpenPolicy2, state S261");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp275, "return of OpenPolicy2, state S261");
            this.Manager.Comment("reaching state \'S333\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp276;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp277;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyDnsDomainInformationInt,out" +
                    " _)\'");
            temp277 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt, out temp276);
            this.Manager.Comment("reaching state \'S405\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp276, "policyInformation of QueryInformationPolicy2, state S405");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp277, "return of QueryInformationPolicy2, state S405");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S92
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S92() {
            this.Manager.BeginTest("TestScenarioS4S92");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S190\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp278;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp279;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp279 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp278);
            this.Manager.Comment("reaching state \'S262\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp278, "policyHandle of OpenPolicy2, state S262");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp279, "return of OpenPolicy2, state S262");
            this.Manager.Comment("reaching state \'S334\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp280;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp281;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(53,PolicyDnsDomainInformation,out _)" +
                    "\'");
            temp281 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation, out temp280);
            this.Manager.Comment("reaching state \'S406\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp280, "policyInformation of QueryInformationPolicy2, state S406");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp281, "return of QueryInformationPolicy2, state S406");
            TestScenarioS4S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S94
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S94() {
            this.Manager.BeginTest("TestScenarioS4S94");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S191\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp282;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp283;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp283 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp282);
            this.Manager.Comment("reaching state \'S263\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp282, "policyHandle of OpenPolicy2, state S263");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp283, "return of OpenPolicy2, state S263");
            this.Manager.Comment("reaching state \'S335\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp284;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp285;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyReplicaSourceInformation,out" +
                    " _)\'");
            temp285 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation, out temp284);
            this.Manager.Comment("reaching state \'S407\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp284, "policyInformation of QueryInformationPolicy2, state S407");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp285, "return of QueryInformationPolicy2, state S407");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S96
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S96() {
            this.Manager.BeginTest("TestScenarioS4S96");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S192\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp286;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp287;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp287 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp286);
            this.Manager.Comment("reaching state \'S264\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp286, "policyHandle of OpenPolicy2, state S264");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp287, "return of OpenPolicy2, state S264");
            this.Manager.Comment("reaching state \'S336\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp288;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp289;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyDnsDomainInformationInt,out " +
                    "_)\'");
            temp289 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt, out temp288);
            this.Manager.Comment("reaching state \'S408\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp288, "policyInformation of QueryInformationPolicy2, state S408");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp289, "return of QueryInformationPolicy2, state S408");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S98
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS4S98() {
            this.Manager.BeginTest("TestScenarioS4S98");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S193\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp290;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp291;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp291 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp290);
            this.Manager.Comment("reaching state \'S265\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp290, "policyHandle of OpenPolicy2, state S265");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp291, "return of OpenPolicy2, state S265");
            this.Manager.Comment("reaching state \'S337\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp292;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp293;
            this.Manager.Comment("executing step \'call QueryInformationPolicy2(1,PolicyPdAccountInformation,out _)\'" +
                    "");
            temp293 = this.ILsadManagedAdapterInstance.QueryInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation, out temp292);
            this.Manager.Comment("reaching state \'S409\'");
            this.Manager.Comment("checking step \'return QueryInformationPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp292, "policyInformation of QueryInformationPolicy2, state S409");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp293, "return of QueryInformationPolicy2, state S409");
            TestScenarioS4S432();
            this.Manager.EndTest();
        }
        #endregion
    }
}
