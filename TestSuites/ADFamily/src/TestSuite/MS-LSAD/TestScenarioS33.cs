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
    public partial class TestScenarioS33 : PtfTestClassBase {
        
        public TestScenarioS33() {
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
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS33S0() {
            this.Manager.BeginTest("TestScenarioS33S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,33554432,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 33554432u, out temp0);
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S6");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S6");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp2;
            this.Manager.Comment("executing step \'call SetInformationPolicy(53,PolicyAccountDomainInformation)\'");
            temp2 = this.ILsadManagedAdapterInstance.SetInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp2, "return of SetInformationPolicy, state S10");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAccountDomainInformation)\'");
            temp3 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp3, "return of SetInformationPolicy2, state S14");
            TestScenarioS33S16();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS33S16() {
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.Close(1, out temp4);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp4, "handleAfterClose of Close, state S17");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp5, "return of Close, state S17");
            this.Manager.Comment("reaching state \'S18\'");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS33S2() {
            this.Manager.BeginTest("TestScenarioS33S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S5\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,33554432,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 33554432u, out temp6);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp6, "policyHandle of OpenPolicy2, state S7");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp7, "return of OpenPolicy2, state S7");
            this.Manager.Comment("reaching state \'S9\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call SetInformationPolicy(1,PolicyAccountDomainInformation)\'");
            temp8 = this.ILsadManagedAdapterInstance.SetInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp8, "return of SetInformationPolicy, state S11");
            this.Manager.Comment("reaching state \'S13\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAccountDomainInformation)\'");
            temp9 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp9, "return of SetInformationPolicy2, state S15");
            TestScenarioS33S16();
            this.Manager.EndTest();
        }
        #endregion
    }
}
