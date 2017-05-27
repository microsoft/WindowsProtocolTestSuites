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
    public partial class TestScenarioS6 : PtfTestClassBase {
        
        public TestScenarioS6() {
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
        public void LSAT_TestScenarioS6S0() {
            this.Manager.BeginTest("TestScenarioS6S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp0;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp0 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp7 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS6.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS6S0InitializeChecker)), new ExpectedReturn(TestScenarioS6.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS6S0InitializeChecker1)));
            if ((temp7 == 0)) {
                this.Manager.Comment("reaching state \'S2\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name temp1;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name temp2;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp3;
                this.Manager.Comment("executing step \'call GetUserName(Authenticated,out _,out _)\'");
                temp3 = this.ILsatAdapterInstance.GetUserName(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.User)(0)), out temp1, out temp2);
                this.Manager.Checkpoint("MS-LSAT_R192");
                this.Manager.Comment("reaching state \'S4\'");
                this.Manager.Comment("checking step \'return GetUserName/[out valid,out valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name)(0)), temp1, "nameOfTheUser of GetUserName, state S4");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name)(0)), temp2, "nameOfTheDomain of GetUserName, state S4");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp3, "return of GetUserName, state S4");
                this.Manager.Comment("reaching state \'S6\'");
                goto label0;
            }
            if ((temp7 == 1)) {
                this.Manager.Comment("reaching state \'S3\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name temp4;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name temp5;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp6;
                this.Manager.Comment("executing step \'call GetUserName(Authenticated,out _,out _)\'");
                temp6 = this.ILsatAdapterInstance.GetUserName(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.User)(0)), out temp4, out temp5);
                this.Manager.Checkpoint("MS-LSAT_R192");
                this.Manager.Comment("reaching state \'S5\'");
                this.Manager.Comment("checking step \'return GetUserName/[out valid,out valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name)(0)), temp4, "nameOfTheUser of GetUserName, state S5");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Name)(0)), temp5, "nameOfTheDomain of GetUserName, state S5");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp6, "return of GetUserName, state S5");
                this.Manager.Comment("reaching state \'S7\'");
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS6S0InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S1");
        }
        
        private void TestScenarioS6S0InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S1");
        }
        #endregion
    }
}
