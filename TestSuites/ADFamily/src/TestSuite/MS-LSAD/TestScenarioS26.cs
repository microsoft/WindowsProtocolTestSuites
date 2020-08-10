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
    public partial class TestScenarioS26 : PtfTestClassBase {
        
        public TestScenarioS26() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
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
        public void LSAD_TestScenarioS26S0() {
            this.Manager.BeginTest("TestScenarioS26S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(NonNull,8192,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(1)), 8192u, out temp0);
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp0, "policyHandle of OpenPolicy, state S6");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp1, "return of OpenPolicy, state S6");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call OpenPolicy2(NonNull,8192,out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(1)), 8192u, out temp2);
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp2, "policyHandle of OpenPolicy2, state S10");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp3, "return of OpenPolicy2, state S10");
            TestScenarioS26S12();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS26S12() {
            this.Manager.Comment("reaching state \'S12\'");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS26S2() {
            this.Manager.BeginTest("TestScenarioS26S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S5\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,8192,out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 8192u, out temp4);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp4, "policyHandle of OpenPolicy, state S7");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp5, "return of OpenPolicy, state S7");
            this.Manager.Comment("reaching state \'S9\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,8192,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 8192u, out temp6);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp6, "policyHandle of OpenPolicy2, state S11");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp7, "return of OpenPolicy2, state S11");
            TestScenarioS26S12();
            this.Manager.EndTest();
        }
        #endregion
    }
}
