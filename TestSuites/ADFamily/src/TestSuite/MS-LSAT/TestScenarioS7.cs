// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.4.2965.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TestScenarioS7 : PtfTestClassBase {
        
        public TestScenarioS7() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000000");
        }
        
        #region Expect Delegates
        public delegate void InitializeDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase InitializeInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter), "Initialize", typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess), typeof(uint));
        #endregion
        
        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter ILsatAdapterInstance;
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
            this.ILsatAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter))));
        }
        
        protected override void TestCleanup() {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion
        
        #region Test Starting in S0
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS7S0() {
            this.Manager.BeginTest("TestScenarioS7S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp0;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp0 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp9 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS7.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS7S0InitializeChecker)), new ExpectedReturn(TestScenarioS7.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS7S0InitializeChecker1)));
            if ((temp9 == 0)) {
                this.Manager.Comment("reaching state \'S4\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp1;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp2;
                this.Manager.Comment("executing step \'call OpenPolicy(NonNull,8192,out _)\'");
                temp2 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(1)), 8192u, out temp1);
                this.Manager.Checkpoint("MS-LSAT_R175");
                this.Manager.Comment("reaching state \'S8\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Null]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, temp1, "openPolicyHandle of OpenPolicy, state S8");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp2, "return of OpenPolicy, state S8");
                this.Manager.Comment("reaching state \'S12\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp3;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp4;
                this.Manager.Comment("executing step \'call OpenPolicy2(NonNull,8192,out _)\'");
                temp4 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(1)), 8192u, out temp3);
                this.Manager.Checkpoint("MS-LSAT_R163");
                this.Manager.Comment("reaching state \'S16\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Null]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, temp3, "openPolicyHandle2 of OpenPolicy2, state S16");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp4, "return of OpenPolicy2, state S16");
                TestScenarioS7S20();
                goto label0;
            }
            if ((temp9 == 1)) {
                this.Manager.Comment("reaching state \'S5\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp5;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp6;
                this.Manager.Comment("executing step \'call OpenPolicy(NonNull,8192,out _)\'");
                temp6 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(1)), 8192u, out temp5);
                this.Manager.Checkpoint("MS-LSAT_R175");
                this.Manager.Comment("reaching state \'S9\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Null]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, temp5, "openPolicyHandle of OpenPolicy, state S9");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp6, "return of OpenPolicy, state S9");
                this.Manager.Comment("reaching state \'S13\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp7;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp8;
                this.Manager.Comment("executing step \'call OpenPolicy2(NonNull,8192,out _)\'");
                temp8 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(1)), 8192u, out temp7);
                this.Manager.Checkpoint("MS-LSAT_R163");
                this.Manager.Comment("reaching state \'S17\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Null]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, temp7, "openPolicyHandle2 of OpenPolicy2, state S17");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp8, "return of OpenPolicy2, state S17");
                TestScenarioS7S21();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS7S0InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S1");
        }
        
        private void TestScenarioS7S20() {
            this.Manager.Comment("reaching state \'S20\'");
        }
        
        private void TestScenarioS7S0InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S1");
        }
        
        private void TestScenarioS7S21() {
            this.Manager.Comment("reaching state \'S21\'");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS7S2() {
            this.Manager.BeginTest("TestScenarioS7S2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp10;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp10 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp10);
            this.Manager.Comment("reaching state \'S3\'");
            int temp19 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS7.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS7S2InitializeChecker)), new ExpectedReturn(TestScenarioS7.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS7S2InitializeChecker1)));
            if ((temp19 == 0)) {
                this.Manager.Comment("reaching state \'S6\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp11;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp12;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,8192,out _)\'");
                temp12 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 8192u, out temp11);
                this.Manager.Checkpoint("MS-LSAT_R177");
                this.Manager.Comment("reaching state \'S10\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Null]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, temp11, "openPolicyHandle of OpenPolicy, state S10");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp12, "return of OpenPolicy, state S10");
                this.Manager.Comment("reaching state \'S14\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp13;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp14;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,8192,out _)\'");
                temp14 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 8192u, out temp13);
                this.Manager.Checkpoint("MS-LSAT_R165");
                this.Manager.Comment("reaching state \'S18\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Null]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, temp13, "openPolicyHandle2 of OpenPolicy2, state S18");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp14, "return of OpenPolicy2, state S18");
                TestScenarioS7S21();
                goto label1;
            }
            if ((temp19 == 1)) {
                this.Manager.Comment("reaching state \'S7\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp15;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp16;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,8192,out _)\'");
                temp16 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 8192u, out temp15);
                this.Manager.Checkpoint("MS-LSAT_R177");
                this.Manager.Comment("reaching state \'S11\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Null]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, temp15, "openPolicyHandle of OpenPolicy, state S11");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp16, "return of OpenPolicy, state S11");
                this.Manager.Comment("reaching state \'S15\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp17;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp18;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,8192,out _)\'");
                temp18 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 8192u, out temp17);
                this.Manager.Checkpoint("MS-LSAT_R165");
                this.Manager.Comment("reaching state \'S19\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Null]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, temp17, "openPolicyHandle2 of OpenPolicy2, state S19");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp18, "return of OpenPolicy2, state S19");
                TestScenarioS7S20();
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS7S2InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S3");
        }
        
        private void TestScenarioS7S2InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S3");
        }
        #endregion
    }
}
