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
    public partial class TestScenarioS27 : PtfTestClassBase {
        
        public TestScenarioS27() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void EnumeratePrivilegesDelegate1(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse);
        
        public delegate void CloseDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.EventInfo EnumeratePrivilegesInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "EnumeratePrivileges");
        
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
            this.Manager.Subscribe(EnumeratePrivilegesInfo, this.ILsadManagedAdapterInstance);
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
        public void LSAD_TestScenarioS27S0() {
            this.Manager.BeginTest("TestScenarioS27S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S36");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S36");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(11,35)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(11, 35);
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S72();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS27S72() {
            this.Manager.Comment("reaching state \'S72\'");
            int temp4 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S0EnumeratePrivilegesChecker)), new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S0EnumeratePrivilegesChecker1)), new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S0EnumeratePrivilegesChecker2)), new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S0EnumeratePrivilegesChecker3)));
            if ((temp4 == 0)) {
                TestScenarioS27S74();
                goto label0;
            }
            if ((temp4 == 1)) {
                TestScenarioS27S74();
                goto label0;
            }
            if ((temp4 == 2)) {
                TestScenarioS27S74();
                goto label0;
            }
            if ((temp4 == 3)) {
                TestScenarioS27S74();
                goto label0;
            }
            this.Manager.CheckObservationTimeout(false, new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S0EnumeratePrivilegesChecker)), new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S0EnumeratePrivilegesChecker1)), new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S0EnumeratePrivilegesChecker2)), new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S0EnumeratePrivilegesChecker3)));
        label0:
;
        }
        
        private void TestScenarioS27S0EnumeratePrivilegesChecker(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumeratePrivileges(11,EnumerateNone)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 11, handleInput, "handleInput of EnumeratePrivileges, state S72");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse.EnumerateNone, actionResponse, "actionResponse of EnumeratePrivileges, state S72");
        }
        
        private void TestScenarioS27S74() {
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.Close(1, out temp2);
            this.Manager.AddReturn(CloseInfo, null, temp2, temp3);
            TestScenarioS27S76();
        }
        
        private void TestScenarioS27S76() {
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS27.CloseInfo, null, new CloseDelegate1(this.TestScenarioS27S0CloseChecker)));
            this.Manager.Comment("reaching state \'S77\'");
        }
        
        private void TestScenarioS27S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S76");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S76");
        }
        
        private void TestScenarioS27S0EnumeratePrivilegesChecker1(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumeratePrivileges(1,EnumerateSome)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 1, handleInput, "handleInput of EnumeratePrivileges, state S72");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse)(1)), actionResponse, "actionResponse of EnumeratePrivileges, state S72");
        }
        
        private void TestScenarioS27S0EnumeratePrivilegesChecker2(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumeratePrivileges(1,EnumerateNone)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 1, handleInput, "handleInput of EnumeratePrivileges, state S72");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse.EnumerateNone, actionResponse, "actionResponse of EnumeratePrivileges, state S72");
        }
        
        private void TestScenarioS27S0EnumeratePrivilegesChecker3(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumeratePrivileges(1,EnumerateAll)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 1, handleInput, "handleInput of EnumeratePrivileges, state S72");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse)(0)), actionResponse, "actionResponse of EnumeratePrivileges, state S72");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S10() {
            this.Manager.BeginTest("TestScenarioS27S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S29\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp5;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp6 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp5);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp5, "policyHandle of OpenPolicy, state S41");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp6, "return of OpenPolicy, state S41");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(1,35)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(1, 35);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S73();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS27S73() {
            this.Manager.Comment("reaching state \'S73\'");
            int temp9 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S10EnumeratePrivilegesChecker)), new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S10EnumeratePrivilegesChecker1)));
            if ((temp9 == 0)) {
                TestScenarioS27S75();
                goto label1;
            }
            if ((temp9 == 1)) {
                TestScenarioS27S75();
                goto label1;
            }
            this.Manager.CheckObservationTimeout(false, new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S10EnumeratePrivilegesChecker)), new ExpectedEvent(TestScenarioS27.EnumeratePrivilegesInfo, null, new EnumeratePrivilegesDelegate1(this.TestScenarioS27S10EnumeratePrivilegesChecker1)));
        label1:
;
        }
        
        private void TestScenarioS27S10EnumeratePrivilegesChecker(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumeratePrivileges(11,EnumerateNone)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 11, handleInput, "handleInput of EnumeratePrivileges, state S73");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse.EnumerateNone, actionResponse, "actionResponse of EnumeratePrivileges, state S73");
        }
        
        private void TestScenarioS27S75() {
            this.Manager.Comment("reaching state \'S75\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp8 = this.ILsadManagedAdapterInstance.Close(1, out temp7);
            this.Manager.AddReturn(CloseInfo, null, temp7, temp8);
            TestScenarioS27S76();
        }
        
        private void TestScenarioS27S10EnumeratePrivilegesChecker1(int handleInput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse actionResponse) {
            this.Manager.Comment("checking step \'event EnumeratePrivileges(1,EnumerateNone)\'");
            TestManagerHelpers.AssertAreEqual<int>(this.Manager, 1, handleInput, "handleInput of EnumeratePrivileges, state S73");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.enumerateResponse.EnumerateNone, actionResponse, "actionResponse of EnumeratePrivileges, state S73");
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S12() {
            this.Manager.BeginTest("TestScenarioS27S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp10);
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp10, "policyHandle of OpenPolicy, state S42");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp11, "return of OpenPolicy, state S42");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(11,0)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(11, 0);
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S14() {
            this.Manager.BeginTest("TestScenarioS27S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S31\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp12);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp12, "policyHandle of OpenPolicy, state S43");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp13, "return of OpenPolicy, state S43");
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(11,5)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(11, 5);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S16() {
            this.Manager.BeginTest("TestScenarioS27S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp14);
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp14, "policyHandle of OpenPolicy, state S44");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp15, "return of OpenPolicy, state S44");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(1,0)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(1, 0);
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S18() {
            this.Manager.BeginTest("TestScenarioS27S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S33\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp16);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp16, "policyHandle of OpenPolicy, state S45");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of OpenPolicy, state S45");
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(1,5)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(1, 5);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S2() {
            this.Manager.BeginTest("TestScenarioS27S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S25\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp19 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp18);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp18, "policyHandle of OpenPolicy, state S37");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp19, "return of OpenPolicy, state S37");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(11,0)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(11, 0);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S73();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S20() {
            this.Manager.BeginTest("TestScenarioS27S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp21 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp20);
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp20, "policyHandle of OpenPolicy, state S46");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp21, "return of OpenPolicy, state S46");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(1,34)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(1, 34);
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S22() {
            this.Manager.BeginTest("TestScenarioS27S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S35\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp22);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp22, "policyHandle of OpenPolicy, state S47");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp23, "return of OpenPolicy, state S47");
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(11,35)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(11, 35);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S73();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S4() {
            this.Manager.BeginTest("TestScenarioS27S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp24);
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp24, "policyHandle of OpenPolicy, state S38");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp25, "return of OpenPolicy, state S38");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(11,5)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(11, 5);
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S73();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S6() {
            this.Manager.BeginTest("TestScenarioS27S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S27\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp26);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy, state S39");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy, state S39");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(1,0)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(1, 0);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S73();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS27S8() {
            this.Manager.BeginTest("TestScenarioS27S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3506,out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3506u, out temp28);
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp28, "policyHandle of OpenPolicy, state S40");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of OpenPolicy, state S40");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call EnumeratePrivilegesRequest(1,5)\'");
            this.ILsadManagedAdapterInstance.EnumeratePrivilegesRequest(1, 5);
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesRequest\'");
            TestScenarioS27S73();
            this.Manager.EndTest();
        }
        #endregion
    }
}
