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
    public partial class TestScenarioS23 : PtfTestClassBase {
        
        public TestScenarioS23() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void CloseDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        
        public delegate void EnumerateTrustedDomainsExDelegate1(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse);
        #endregion
        
        #region Event Metadata
        static System.Reflection.EventInfo EnumerateTrustedDomainsExInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "EnumerateTrustedDomainsEx");
        
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
            this.Manager.Subscribe(EnumerateTrustedDomainsExInfo, this.ILsadManagedAdapterInstance);
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
        public void LSAD_TestScenarioS23S0() {
            this.Manager.BeginTest("TestScenarioS23S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S66");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S66");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,5)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 5);
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS23S131() {
            this.Manager.Comment("reaching state \'S131\'");
            int temp4 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S0EnumerateTrustedDomainsExChecker)), new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S0EnumerateTrustedDomainsExChecker1)), new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S0EnumerateTrustedDomainsExChecker2)), new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S0EnumerateTrustedDomainsExChecker3)));
            if ((temp4 == 0)) {
                TestScenarioS23S134();
                goto label0;
            }
            if ((temp4 == 1)) {
                TestScenarioS23S134();
                goto label0;
            }
            if ((temp4 == 2)) {
                TestScenarioS23S134();
                goto label0;
            }
            if ((temp4 == 3)) {
                TestScenarioS23S134();
                goto label0;
            }
            this.Manager.CheckObservationTimeout(false, new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S0EnumerateTrustedDomainsExChecker)), new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S0EnumerateTrustedDomainsExChecker1)), new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S0EnumerateTrustedDomainsExChecker2)), new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S0EnumerateTrustedDomainsExChecker3)));
        label0:
;
        }
        
        private void TestScenarioS23S0EnumerateTrustedDomainsExChecker(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumerateTrustedDomainsEx(11,EnumerateNone)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 11, handleInput, "handleInput of EnumerateTrustedDomainsEx, state S131");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse.EnumerateNone, actionResponse, "actionResponse of EnumerateTrustedDomainsEx, state S131");
        }
        
        private void TestScenarioS23S134() {
            this.Manager.Comment("reaching state \'S134\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.Close(1, out temp2);
            this.Manager.AddReturn(CloseInfo, null, temp2, temp3);
            TestScenarioS23S136();
        }
        
        private void TestScenarioS23S136() {
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS23.CloseInfo, null, new CloseDelegate1(this.TestScenarioS23S0CloseChecker)));
            this.Manager.Comment("reaching state \'S137\'");
        }
        
        private void TestScenarioS23S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S136");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S136");
        }
        
        private void TestScenarioS23S0EnumerateTrustedDomainsExChecker1(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumerateTrustedDomainsEx(1,EnumerateSome)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 1, handleInput, "handleInput of EnumerateTrustedDomainsEx, state S131");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse)(1)), actionResponse, "actionResponse of EnumerateTrustedDomainsEx, state S131");
        }
        
        private void TestScenarioS23S0EnumerateTrustedDomainsExChecker2(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumerateTrustedDomainsEx(1,EnumerateNone)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 1, handleInput, "handleInput of EnumerateTrustedDomainsEx, state S131");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse.EnumerateNone, actionResponse, "actionResponse of EnumerateTrustedDomainsEx, state S131");
        }
        
        private void TestScenarioS23S0EnumerateTrustedDomainsExChecker3(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumerateTrustedDomainsEx(1,EnumerateAll)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 1, handleInput, "handleInput of EnumerateTrustedDomainsEx, state S131");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse)(0)), actionResponse, "actionResponse of EnumerateTrustedDomainsEx, state S131");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S10() {
            this.Manager.BeginTest("TestScenarioS23S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S49\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp5;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp6 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp5);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp5, "policyHandle of OpenPolicy, state S71");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp6, "return of OpenPolicy, state S71");
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,2)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 2);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS23S133() {
            this.Manager.Comment("reaching state \'S133\'");
            int temp9 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S10EnumerateTrustedDomainsExChecker)), new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S10EnumerateTrustedDomainsExChecker1)));
            if ((temp9 == 0)) {
                TestScenarioS23S135();
                goto label1;
            }
            if ((temp9 == 1)) {
                TestScenarioS23S135();
                goto label1;
            }
            this.Manager.CheckObservationTimeout(false, new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S10EnumerateTrustedDomainsExChecker)), new ExpectedEvent(TestScenarioS23.EnumerateTrustedDomainsExInfo, null, new EnumerateTrustedDomainsExDelegate1(this.TestScenarioS23S10EnumerateTrustedDomainsExChecker1)));
        label1:
;
        }
        
        private void TestScenarioS23S10EnumerateTrustedDomainsExChecker(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumerateTrustedDomainsEx(11,EnumerateNone)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 11, handleInput, "handleInput of EnumerateTrustedDomainsEx, state S133");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse.EnumerateNone, actionResponse, "actionResponse of EnumerateTrustedDomainsEx, state S133");
        }
        
        private void TestScenarioS23S135() {
            this.Manager.Comment("reaching state \'S135\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp8 = this.ILsadManagedAdapterInstance.Close(1, out temp7);
            this.Manager.AddReturn(CloseInfo, null, temp7, temp8);
            TestScenarioS23S136();
        }
        
        private void TestScenarioS23S10EnumerateTrustedDomainsExChecker1(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumerateTrustedDomainsEx(1,EnumerateNone)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 1, handleInput, "handleInput of EnumerateTrustedDomainsEx, state S133");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse.EnumerateNone, actionResponse, "actionResponse of EnumerateTrustedDomainsEx, state S133");
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S12() {
            this.Manager.BeginTest("TestScenarioS23S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp10);
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp10, "policyHandle of OpenPolicy, state S72");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp11, "return of OpenPolicy, state S72");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,4)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 4);
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S14() {
            this.Manager.BeginTest("TestScenarioS23S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S51\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp12);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp12, "policyHandle of OpenPolicy, state S73");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp13, "return of OpenPolicy, state S73");
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,4)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 4);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S16() {
            this.Manager.BeginTest("TestScenarioS23S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp14);
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp14, "policyHandle of OpenPolicy, state S74");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp15, "return of OpenPolicy, state S74");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,2)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 2);
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S18() {
            this.Manager.BeginTest("TestScenarioS23S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S53\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp16);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp16, "policyHandle of OpenPolicy, state S75");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of OpenPolicy, state S75");
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,3)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 3);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S2() {
            this.Manager.BeginTest("TestScenarioS23S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S45\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp19 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp18);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp18, "policyHandle of OpenPolicy, state S67");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp19, "return of OpenPolicy, state S67");
            this.Manager.Comment("reaching state \'S89\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp21 = this.ILsadManagedAdapterInstance.Close(1, out temp20);
            this.Manager.AddReturn(CloseInfo, null, temp20, temp21);
            TestScenarioS23S111();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS23S111() {
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS23.CloseInfo, null, new CloseDelegate1(this.TestScenarioS23S2CloseChecker)));
            this.Manager.Comment("reaching state \'S132\'");
        }
        
        private void TestScenarioS23S2CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S111");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S111");
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S20() {
            this.Manager.BeginTest("TestScenarioS23S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp22);
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp22, "policyHandle of OpenPolicy, state S76");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp23, "return of OpenPolicy, state S76");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,0)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 0);
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S22() {
            this.Manager.BeginTest("TestScenarioS23S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S55\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp24);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp24, "policyHandle of OpenPolicy, state S77");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp25, "return of OpenPolicy, state S77");
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,0)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 0);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S24() {
            this.Manager.BeginTest("TestScenarioS23S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp26);
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy, state S78");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy, state S78");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.Close(1, out temp28);
            this.Manager.AddReturn(CloseInfo, null, temp28, temp29);
            TestScenarioS23S111();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S26() {
            this.Manager.BeginTest("TestScenarioS23S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S57\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp31 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp30);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp30, "policyHandle of OpenPolicy, state S79");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp31, "return of OpenPolicy, state S79");
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,5)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 5);
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S28() {
            this.Manager.BeginTest("TestScenarioS23S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp33 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp32);
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp32, "policyHandle of OpenPolicy, state S80");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp33, "return of OpenPolicy, state S80");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,3)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 3);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S30() {
            this.Manager.BeginTest("TestScenarioS23S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S59\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp35 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp34);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp34, "policyHandle of OpenPolicy, state S81");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp35, "return of OpenPolicy, state S81");
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,2)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 2);
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S32() {
            this.Manager.BeginTest("TestScenarioS23S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp37 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp36);
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp36, "policyHandle of OpenPolicy, state S82");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp37, "return of OpenPolicy, state S82");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,4)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 4);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S34() {
            this.Manager.BeginTest("TestScenarioS23S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S61\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp38);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp38, "policyHandle of OpenPolicy, state S83");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp39, "return of OpenPolicy, state S83");
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,4)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 4);
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S36() {
            this.Manager.BeginTest("TestScenarioS23S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp40);
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp40, "policyHandle of OpenPolicy, state S84");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of OpenPolicy, state S84");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,2)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 2);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S38() {
            this.Manager.BeginTest("TestScenarioS23S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S63\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp43 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp42);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp42, "policyHandle of OpenPolicy, state S85");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp43, "return of OpenPolicy, state S85");
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,3)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 3);
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S4() {
            this.Manager.BeginTest("TestScenarioS23S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp44;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp45 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp44);
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp44, "policyHandle of OpenPolicy, state S68");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp45, "return of OpenPolicy, state S68");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,0)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 0);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S40() {
            this.Manager.BeginTest("TestScenarioS23S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp46;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp47 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp46);
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp46, "policyHandle of OpenPolicy, state S86");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp47, "return of OpenPolicy, state S86");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,0)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 0);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S131();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S42() {
            this.Manager.BeginTest("TestScenarioS23S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S65\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp48);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp48, "policyHandle of OpenPolicy, state S87");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp49, "return of OpenPolicy, state S87");
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(1,5)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(1, 5);
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S6() {
            this.Manager.BeginTest("TestScenarioS23S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S47\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp50;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp51 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp50);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp50, "policyHandle of OpenPolicy, state S69");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp51, "return of OpenPolicy, state S69");
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,5)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 5);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS23S8() {
            this.Manager.BeginTest("TestScenarioS23S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp53 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp52);
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp52, "policyHandle of OpenPolicy, state S70");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp53, "return of OpenPolicy, state S70");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call EnumerateTrustedDomainsExRequest(11,3)\'");
            this.ILsadManagedAdapterInstance.EnumerateTrustedDomainsExRequest(11, 3);
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return EnumerateTrustedDomainsExRequest\'");
            TestScenarioS23S133();
            this.Manager.EndTest();
        }
        #endregion
    }
}
