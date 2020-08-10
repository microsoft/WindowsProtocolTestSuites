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
    public partial class TestScenarioS5For2K8R2 : PtfTestClassBase {
        
        public TestScenarioS5For2K8R2() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void GetSUTOSVersionDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return);
        
        public delegate void CloseDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase GetSUTOSVersionInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "GetSUTOSVersion", typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server).MakeByRefType());
        
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
        public void LSAD_TestScenarioS5For2K8R2S0() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp0;
            bool temp1;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp0);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp0, temp1);
            this.Manager.Comment("reaching state \'S1\'");
            int temp23 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S0GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S0GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S0GetSUTOSVersionChecker2)));
            if ((temp23 == 0)) {
                this.Manager.Comment("reaching state \'S40\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S100\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S160\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp3 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp2);
                this.Manager.Comment("reaching state \'S220\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp2, "policyHandle of OpenPolicy, state S220");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp3, "return of OpenPolicy, state S220");
                this.Manager.Comment("reaching state \'S280\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainKerberosTicketInfo" +
                        "rmation)\'");
                temp4 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S340\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp4, "return of SetDomainInformationPolicy, state S340");
                this.Manager.Comment("reaching state \'S400\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp5;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                        "Information,out _)\'");
                temp6 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp5);
                this.Manager.Comment("reaching state \'S458\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp5, "policyInformation of QueryDomainInformationPolicy, state S458");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp6, "return of QueryDomainInformationPolicy, state S458");
                TestScenarioS5For2K8R2S516();
                goto label0;
            }
            if ((temp23 == 1)) {
                this.Manager.Comment("reaching state \'S41\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S101\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S161\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp9;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp10 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp9);
                this.Manager.Comment("reaching state \'S221\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp9, "policyHandle of OpenPolicy, state S221");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp10, "return of OpenPolicy, state S221");
                this.Manager.Comment("reaching state \'S281\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainKerberosTicketInfo" +
                        "rmation)\'");
                temp11 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S341\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp11, "return of SetDomainInformationPolicy, state S341");
                TestScenarioS5For2K8R2S401();
                goto label0;
            }
            if ((temp23 == 2)) {
                this.Manager.Comment("reaching state \'S42\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S102\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S162\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp16;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp17 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp16);
                this.Manager.Comment("reaching state \'S222\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp16, "policyHandle of OpenPolicy, state S222");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of OpenPolicy, state S222");
                this.Manager.Comment("reaching state \'S282\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp18;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp18 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S342\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp18, "return of SetDomainInformationPolicy, state S342");
                this.Manager.Comment("reaching state \'S402\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp19;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp20;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainEfsInformation,o" +
                        "ut _)\'");
                temp20 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp19);
                this.Manager.Comment("reaching state \'S460\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp19, "policyInformation of QueryDomainInformationPolicy, state S460");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp20, "return of QueryDomainInformationPolicy, state S460");
                TestScenarioS5For2K8R2S518();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S0GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S1");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S1");
        }
        
        private void TestScenarioS5For2K8R2S516() {
            this.Manager.Comment("reaching state \'S516\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp8 = this.ILsadManagedAdapterInstance.Close(1, out temp7);
            this.Manager.AddReturn(CloseInfo, null, temp7, temp8);
            TestScenarioS5For2K8R2S548();
        }
        
        private void TestScenarioS5For2K8R2S548() {
            this.Manager.Comment("reaching state \'S548\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.CloseInfo, null, new CloseDelegate1(this.TestScenarioS5For2K8R2S0CloseChecker)));
            this.Manager.Comment("reaching state \'S577\'");
        }
        
        private void TestScenarioS5For2K8R2S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S548");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S548");
        }
        
        private void TestScenarioS5For2K8R2S0GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S1");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S1");
        }
        
        private void TestScenarioS5For2K8R2S401() {
            this.Manager.Comment("reaching state \'S401\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                    "Information,out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp12);
            this.Manager.Comment("reaching state \'S459\'");
            this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp12, "policyInformation of QueryDomainInformationPolicy, state S459");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp13, "return of QueryDomainInformationPolicy, state S459");
            TestScenarioS5For2K8R2S517();
        }
        
        private void TestScenarioS5For2K8R2S517() {
            this.Manager.Comment("reaching state \'S517\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.Close(1, out temp14);
            this.Manager.AddReturn(CloseInfo, null, temp14, temp15);
            TestScenarioS5For2K8R2S549();
        }
        
        private void TestScenarioS5For2K8R2S549() {
            this.Manager.Comment("reaching state \'S549\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.CloseInfo, null, new CloseDelegate1(this.TestScenarioS5For2K8R2S0CloseChecker1)));
            this.Manager.Comment("reaching state \'S578\'");
        }
        
        private void TestScenarioS5For2K8R2S0CloseChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S549");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S549");
        }
        
        private void TestScenarioS5For2K8R2S0GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S1");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S1");
        }
        
        private void TestScenarioS5For2K8R2S518() {
            this.Manager.Comment("reaching state \'S518\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp21;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp22;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp22 = this.ILsadManagedAdapterInstance.Close(1, out temp21);
            this.Manager.Comment("reaching state \'S550\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp21, "handleAfterClose of Close, state S550");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp22, "return of Close, state S550");
            this.Manager.Comment("reaching state \'S579\'");
        }
        #endregion
        
        #region Test Starting in S10
        //[TestCategory("PDC")]
        //[TestCategory("DomainWin2008R2")]
        //[TestCategory("ForestWin2008R2")]
        //[TestCategory("MS-LSAD")]
        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S10() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp24;
            bool temp25;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp24);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp24, temp25);
            this.Manager.Comment("reaching state \'S11\'");
            int temp47 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S10GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S10GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S10GetSUTOSVersionChecker2)));
            if ((temp47 == 0)) {
                this.Manager.Comment("reaching state \'S55\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S115\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S175\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp27 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp26);
                this.Manager.Comment("reaching state \'S235\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy, state S235");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy, state S235");
                this.Manager.Comment("reaching state \'S295\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp28;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp28 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S355\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp28, "return of SetDomainInformationPolicy, state S355");
                this.Manager.Comment("reaching state \'S415\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp29;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp30;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp30 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp29);
                this.Manager.Comment("reaching state \'S473\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp29, "policyInformation of QueryDomainInformationPolicy, state S473");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp30, "return of QueryDomainInformationPolicy, state S473");
                TestScenarioS5For2K8R2S524();
                goto label1;
            }
            if ((temp47 == 1)) {
                this.Manager.Comment("reaching state \'S56\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S116\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S176\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp33;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp34;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp34 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp33);
                this.Manager.Comment("reaching state \'S236\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp33, "policyHandle of OpenPolicy, state S236");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp34, "return of OpenPolicy, state S236");
                this.Manager.Comment("reaching state \'S296\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp35 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S356\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp35, "return of SetDomainInformationPolicy, state S356");
                this.Manager.Comment("reaching state \'S416\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp36;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp37 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp36);
                this.Manager.Comment("reaching state \'S474\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp36, "policyInformation of QueryDomainInformationPolicy, state S474");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp37, "return of QueryDomainInformationPolicy, state S474");
                TestScenarioS5For2K8R2S525();
                goto label1;
            }
            if ((temp47 == 2)) {
                this.Manager.Comment("reaching state \'S57\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S117\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S177\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp40;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp41 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp40);
                this.Manager.Comment("reaching state \'S237\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp40, "policyHandle of OpenPolicy, state S237");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of OpenPolicy, state S237");
                this.Manager.Comment("reaching state \'S297\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp42;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp42 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S357\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp42, "return of SetDomainInformationPolicy, state S357");
                this.Manager.Comment("reaching state \'S417\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp43;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp44;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp44 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp43);
                this.Manager.Comment("reaching state \'S475\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp43, "policyInformation of QueryDomainInformationPolicy, state S475");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp44, "return of QueryDomainInformationPolicy, state S475");
                this.Manager.Comment("reaching state \'S527\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp45;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp46;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp46 = this.ILsadManagedAdapterInstance.Close(1, out temp45);
                this.Manager.Comment("reaching state \'S559\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp45, "handleAfterClose of Close, state S559");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp46, "return of Close, state S559");
                this.Manager.Comment("reaching state \'S588\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S10GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S11");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S11");
        }
        
        private void TestScenarioS5For2K8R2S524() {
            this.Manager.Comment("reaching state \'S524\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp31;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp32;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp32 = this.ILsadManagedAdapterInstance.Close(1, out temp31);
            this.Manager.Comment("reaching state \'S556\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp31, "handleAfterClose of Close, state S556");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp32, "return of Close, state S556");
            this.Manager.Comment("reaching state \'S585\'");
        }
        
        private void TestScenarioS5For2K8R2S10GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S11");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S11");
        }
        
        private void TestScenarioS5For2K8R2S525() {
            this.Manager.Comment("reaching state \'S525\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.Close(1, out temp38);
            this.Manager.AddReturn(CloseInfo, null, temp38, temp39);
            TestScenarioS5For2K8R2S557();
        }
        
        private void TestScenarioS5For2K8R2S557() {
            this.Manager.Comment("reaching state \'S557\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.CloseInfo, null, new CloseDelegate1(this.TestScenarioS5For2K8R2S10CloseChecker)));
            this.Manager.Comment("reaching state \'S586\'");
        }
        
        private void TestScenarioS5For2K8R2S10CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S557");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S557");
        }
        
        private void TestScenarioS5For2K8R2S10GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S11");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S11");
        }
        #endregion
        
        #region Test Starting in S12
        //[TestCategory("PDC")]
        //[TestCategory("DomainWin2008R2")]
        //[TestCategory("ForestWin2008R2")]
        //[TestCategory("MS-LSAD")]
        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S12() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp48;
            bool temp49;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp48);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp48, temp49);
            this.Manager.Comment("reaching state \'S13\'");
            int temp67 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S12GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S12GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S12GetSUTOSVersionChecker2)));
            if ((temp67 == 0)) {
                this.Manager.Comment("reaching state \'S58\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S118\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S178\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp50;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp51 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp50);
                this.Manager.Comment("reaching state \'S238\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp50, "policyHandle of OpenPolicy, state S238");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp51, "return of OpenPolicy, state S238");
                this.Manager.Comment("reaching state \'S298\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp52;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp52 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S358\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp52, "return of SetDomainInformationPolicy, state S358");
                this.Manager.Comment("reaching state \'S418\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp53;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp54;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                        "Information,out _)\'");
                temp54 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp53);
                this.Manager.Comment("reaching state \'S476\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp53, "policyInformation of QueryDomainInformationPolicy, state S476");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp54, "return of QueryDomainInformationPolicy, state S476");
                TestScenarioS5For2K8R2S524();
                goto label2;
            }
            if ((temp67 == 1)) {
                this.Manager.Comment("reaching state \'S59\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S119\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S179\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp55;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp56;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp56 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp55);
                this.Manager.Comment("reaching state \'S239\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp55, "policyHandle of OpenPolicy, state S239");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp56, "return of OpenPolicy, state S239");
                this.Manager.Comment("reaching state \'S299\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp57;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainEfsInformation)\'");
                temp57 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S359\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp57, "return of SetDomainInformationPolicy, state S359");
                this.Manager.Comment("reaching state \'S419\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp58;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp59 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp58);
                this.Manager.Comment("reaching state \'S477\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp58, "policyInformation of QueryDomainInformationPolicy, state S477");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp59, "return of QueryDomainInformationPolicy, state S477");
                TestScenarioS5For2K8R2S525();
                goto label2;
            }
            if ((temp67 == 2)) {
                this.Manager.Comment("reaching state \'S60\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S120\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S180\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp60;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp61 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp60);
                this.Manager.Comment("reaching state \'S240\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp60, "policyHandle of OpenPolicy, state S240");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp61, "return of OpenPolicy, state S240");
                this.Manager.Comment("reaching state \'S300\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp62;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp62 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S360\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp62, "return of SetDomainInformationPolicy, state S360");
                this.Manager.Comment("reaching state \'S420\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp63;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp64;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp64 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp63);
                this.Manager.Comment("reaching state \'S478\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp63, "policyInformation of QueryDomainInformationPolicy, state S478");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp64, "return of QueryDomainInformationPolicy, state S478");
                this.Manager.Comment("reaching state \'S528\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp65;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp66;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp66 = this.ILsadManagedAdapterInstance.Close(1, out temp65);
                this.Manager.Comment("reaching state \'S560\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp65, "handleAfterClose of Close, state S560");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp66, "return of Close, state S560");
                this.Manager.Comment("reaching state \'S589\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S12GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S13");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S13");
        }
        
        private void TestScenarioS5For2K8R2S12GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S13");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S13");
        }
        
        private void TestScenarioS5For2K8R2S12GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S13");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S14() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp68;
            bool temp69;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp68);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp68, temp69);
            this.Manager.Comment("reaching state \'S15\'");
            int temp89 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S14GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S14GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S14GetSUTOSVersionChecker2)));
            if ((temp89 == 0)) {
                this.Manager.Comment("reaching state \'S61\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S121\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S181\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp70;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp71 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp70);
                this.Manager.Comment("reaching state \'S241\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp70, "policyHandle of OpenPolicy, state S241");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp71, "return of OpenPolicy, state S241");
                this.Manager.Comment("reaching state \'S301\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp72;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp72 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S361\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp72, "return of SetDomainInformationPolicy, state S361");
                this.Manager.Comment("reaching state \'S421\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp73;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp74;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainKerberosTicketIn" +
                        "formation,out _)\'");
                temp74 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp73);
                this.Manager.Comment("reaching state \'S479\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp73, "policyInformation of QueryDomainInformationPolicy, state S479");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp74, "return of QueryDomainInformationPolicy, state S479");
                TestScenarioS5For2K8R2S519();
                goto label3;
            }
            if ((temp89 == 1)) {
                this.Manager.Comment("reaching state \'S62\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S122\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S182\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp77;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp78;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp78 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp77);
                this.Manager.Comment("reaching state \'S242\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp77, "policyHandle of OpenPolicy, state S242");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp78, "return of OpenPolicy, state S242");
                this.Manager.Comment("reaching state \'S302\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp79 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S362\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp79, "return of SetDomainInformationPolicy, state S362");
                this.Manager.Comment("reaching state \'S422\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp80;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp81 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp80);
                this.Manager.Comment("reaching state \'S480\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp80, "policyInformation of QueryDomainInformationPolicy, state S480");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp81, "return of QueryDomainInformationPolicy, state S480");
                TestScenarioS5For2K8R2S518();
                goto label3;
            }
            if ((temp89 == 2)) {
                this.Manager.Comment("reaching state \'S63\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S123\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S183\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp82;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp83;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp83 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp82);
                this.Manager.Comment("reaching state \'S243\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp82, "policyHandle of OpenPolicy, state S243");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp83, "return of OpenPolicy, state S243");
                this.Manager.Comment("reaching state \'S303\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp84;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp84 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S363\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp84, "return of SetDomainInformationPolicy, state S363");
                this.Manager.Comment("reaching state \'S423\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp85;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp86;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp86 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp85);
                this.Manager.Comment("reaching state \'S481\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp85, "policyInformation of QueryDomainInformationPolicy, state S481");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp86, "return of QueryDomainInformationPolicy, state S481");
                this.Manager.Comment("reaching state \'S529\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp87;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp88;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp88 = this.ILsadManagedAdapterInstance.Close(1, out temp87);
                this.Manager.Comment("reaching state \'S561\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp87, "handleAfterClose of Close, state S561");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp88, "return of Close, state S561");
                this.Manager.Comment("reaching state \'S590\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S14GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S15");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S15");
        }
        
        private void TestScenarioS5For2K8R2S519() {
            this.Manager.Comment("reaching state \'S519\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp75;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp76;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp76 = this.ILsadManagedAdapterInstance.Close(1, out temp75);
            this.Manager.Comment("reaching state \'S551\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp75, "handleAfterClose of Close, state S551");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp76, "return of Close, state S551");
            this.Manager.Comment("reaching state \'S580\'");
        }
        
        private void TestScenarioS5For2K8R2S14GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S15");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S15");
        }
        
        private void TestScenarioS5For2K8R2S14GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S15");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        //[TestCategory("PDC")]
        //[TestCategory("DomainWin2008R2")]
        //[TestCategory("ForestWin2008R2")]
        //[TestCategory("MS-LSAD")]
        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S16() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp90;
            bool temp91;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp91 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp90);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp90, temp91);
            this.Manager.Comment("reaching state \'S17\'");
            int temp111 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S16GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S16GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S16GetSUTOSVersionChecker2)));
            if ((temp111 == 0)) {
                this.Manager.Comment("reaching state \'S64\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S124\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S184\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp92;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp93;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp93 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp92);
                this.Manager.Comment("reaching state \'S244\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp92, "policyHandle of OpenPolicy, state S244");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp93, "return of OpenPolicy, state S244");
                this.Manager.Comment("reaching state \'S304\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp94;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainEfsInformation)\'");
                temp94 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S364\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp94, "return of SetDomainInformationPolicy, state S364");
                this.Manager.Comment("reaching state \'S424\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp95;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp96;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp96 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp95);
                this.Manager.Comment("reaching state \'S482\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp95, "policyInformation of QueryDomainInformationPolicy, state S482");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp96, "return of QueryDomainInformationPolicy, state S482");
                TestScenarioS5For2K8R2S530();
                goto label4;
            }
            if ((temp111 == 1)) {
                this.Manager.Comment("reaching state \'S65\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S125\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S185\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp99;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp100;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp100 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp99);
                this.Manager.Comment("reaching state \'S245\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp99, "policyHandle of OpenPolicy, state S245");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp100, "return of OpenPolicy, state S245");
                this.Manager.Comment("reaching state \'S305\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp101 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S365\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp101, "return of SetDomainInformationPolicy, state S365");
                this.Manager.Comment("reaching state \'S425\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp102;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp103 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp102);
                this.Manager.Comment("reaching state \'S483\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp102, "policyInformation of QueryDomainInformationPolicy, state S483");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp103, "return of QueryDomainInformationPolicy, state S483");
                TestScenarioS5For2K8R2S518();
                goto label4;
            }
            if ((temp111 == 2)) {
                this.Manager.Comment("reaching state \'S66\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S126\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S186\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp104;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp105 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp104);
                this.Manager.Comment("reaching state \'S246\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp104, "policyHandle of OpenPolicy, state S246");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp105, "return of OpenPolicy, state S246");
                this.Manager.Comment("reaching state \'S306\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp106;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp106 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S366\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp106, "return of SetDomainInformationPolicy, state S366");
                this.Manager.Comment("reaching state \'S426\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp107;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp108;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp108 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp107);
                this.Manager.Comment("reaching state \'S484\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp107, "policyInformation of QueryDomainInformationPolicy, state S484");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp108, "return of QueryDomainInformationPolicy, state S484");
                this.Manager.Comment("reaching state \'S531\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp109;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp110;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp110 = this.ILsadManagedAdapterInstance.Close(1, out temp109);
                this.Manager.Comment("reaching state \'S562\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp109, "handleAfterClose of Close, state S562");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp110, "return of Close, state S562");
                this.Manager.Comment("reaching state \'S591\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S16GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S17");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S17");
        }
        
        private void TestScenarioS5For2K8R2S530() {
            this.Manager.Comment("reaching state \'S530\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp97;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp98;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp98 = this.ILsadManagedAdapterInstance.Close(1, out temp97);
            this.Manager.AddReturn(CloseInfo, null, temp97, temp98);
            TestScenarioS5For2K8R2S548();
        }
        
        private void TestScenarioS5For2K8R2S16GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S17");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S17");
        }
        
        private void TestScenarioS5For2K8R2S16GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S17");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S18() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp112;
            bool temp113;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp113 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp112);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp112, temp113);
            this.Manager.Comment("reaching state \'S19\'");
            int temp131 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S18GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S18GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S18GetSUTOSVersionChecker2)));
            if ((temp131 == 0)) {
                this.Manager.Comment("reaching state \'S67\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S127\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S187\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp114;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp115 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp114);
                this.Manager.Comment("reaching state \'S247\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp114, "policyHandle of OpenPolicy, state S247");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp115, "return of OpenPolicy, state S247");
                this.Manager.Comment("reaching state \'S307\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp116;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainKerberosTicketInfo" +
                        "rmation)\'");
                temp116 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S367\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp116, "return of SetDomainInformationPolicy, state S367");
                this.Manager.Comment("reaching state \'S427\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp117;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp118;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                        "Information,out _)\'");
                temp118 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp117);
                this.Manager.Comment("reaching state \'S485\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp117, "policyInformation of QueryDomainInformationPolicy, state S485");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp118, "return of QueryDomainInformationPolicy, state S485");
                TestScenarioS5For2K8R2S530();
                goto label5;
            }
            if ((temp131 == 1)) {
                this.Manager.Comment("reaching state \'S68\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S128\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S188\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp119;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp120;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp120 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp119);
                this.Manager.Comment("reaching state \'S248\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp119, "policyHandle of OpenPolicy, state S248");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp120, "return of OpenPolicy, state S248");
                this.Manager.Comment("reaching state \'S308\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp121;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp121 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S368\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp121, "return of SetDomainInformationPolicy, state S368");
                this.Manager.Comment("reaching state \'S428\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp122;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp123;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                        "Information,out _)\'");
                temp123 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp122);
                this.Manager.Comment("reaching state \'S486\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp122, "policyInformation of QueryDomainInformationPolicy, state S486");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp123, "return of QueryDomainInformationPolicy, state S486");
                TestScenarioS5For2K8R2S518();
                goto label5;
            }
            if ((temp131 == 2)) {
                this.Manager.Comment("reaching state \'S69\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S129\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S189\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp124;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp125;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp125 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp124);
                this.Manager.Comment("reaching state \'S249\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp124, "policyHandle of OpenPolicy, state S249");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp125, "return of OpenPolicy, state S249");
                this.Manager.Comment("reaching state \'S309\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp126;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp126 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S369\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp126, "return of SetDomainInformationPolicy, state S369");
                this.Manager.Comment("reaching state \'S429\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp127;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp128;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp128 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp127);
                this.Manager.Comment("reaching state \'S487\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp127, "policyInformation of QueryDomainInformationPolicy, state S487");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp128, "return of QueryDomainInformationPolicy, state S487");
                this.Manager.Comment("reaching state \'S532\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp129;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp130;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp130 = this.ILsadManagedAdapterInstance.Close(1, out temp129);
                this.Manager.Comment("reaching state \'S563\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp129, "handleAfterClose of Close, state S563");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp130, "return of Close, state S563");
                this.Manager.Comment("reaching state \'S592\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S18GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S19");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S19");
        }
        
        private void TestScenarioS5For2K8R2S18GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S19");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S19");
        }
        
        private void TestScenarioS5For2K8R2S18GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S19");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S2() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp132;
            bool temp133;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp133 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp132);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp132, temp133);
            this.Manager.Comment("reaching state \'S3\'");
            int temp153 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S2GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S2GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S2GetSUTOSVersionChecker2)));
            if ((temp153 == 0)) {
                this.Manager.Comment("reaching state \'S43\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S103\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S163\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp134;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp135;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp135 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp134);
                this.Manager.Comment("reaching state \'S223\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp134, "policyHandle of OpenPolicy, state S223");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp135, "return of OpenPolicy, state S223");
                this.Manager.Comment("reaching state \'S283\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp136;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp136 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S343\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp136, "return of SetDomainInformationPolicy, state S343");
                this.Manager.Comment("reaching state \'S403\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp137;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp138;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp138 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp137);
                this.Manager.Comment("reaching state \'S461\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp137, "policyInformation of QueryDomainInformationPolicy, state S461");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp138, "return of QueryDomainInformationPolicy, state S461");
                TestScenarioS5For2K8R2S519();
                goto label6;
            }
            if ((temp153 == 1)) {
                this.Manager.Comment("reaching state \'S44\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S104\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S164\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp139;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp140;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp140 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp139);
                this.Manager.Comment("reaching state \'S224\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp139, "policyHandle of OpenPolicy, state S224");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp140, "return of OpenPolicy, state S224");
                this.Manager.Comment("reaching state \'S284\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp141;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp141 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S344\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp141, "return of SetDomainInformationPolicy, state S344");
                this.Manager.Comment("reaching state \'S404\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp142;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp143;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp143 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp142);
                this.Manager.Comment("reaching state \'S462\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp142, "policyInformation of QueryDomainInformationPolicy, state S462");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp143, "return of QueryDomainInformationPolicy, state S462");
                TestScenarioS5For2K8R2S520();
                goto label6;
            }
            if ((temp153 == 2)) {
                this.Manager.Comment("reaching state \'S45\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S105\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S165\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp146;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp147;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp147 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp146);
                this.Manager.Comment("reaching state \'S225\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp146, "policyHandle of OpenPolicy, state S225");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp147, "return of OpenPolicy, state S225");
                this.Manager.Comment("reaching state \'S285\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp148;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp148 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S345\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp148, "return of SetDomainInformationPolicy, state S345");
                this.Manager.Comment("reaching state \'S405\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp149;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp150;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp150 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp149);
                this.Manager.Comment("reaching state \'S463\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp149, "policyInformation of QueryDomainInformationPolicy, state S463");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp150, "return of QueryDomainInformationPolicy, state S463");
                this.Manager.Comment("reaching state \'S521\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp151;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp152;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp152 = this.ILsadManagedAdapterInstance.Close(1, out temp151);
                this.Manager.Comment("reaching state \'S553\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp151, "handleAfterClose of Close, state S553");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp152, "return of Close, state S553");
                this.Manager.Comment("reaching state \'S582\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S2GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S3");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S3");
        }
        
        private void TestScenarioS5For2K8R2S2GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S3");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S3");
        }
        
        private void TestScenarioS5For2K8R2S520() {
            this.Manager.Comment("reaching state \'S520\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp144;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp145;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp145 = this.ILsadManagedAdapterInstance.Close(1, out temp144);
            this.Manager.Comment("reaching state \'S552\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp144, "handleAfterClose of Close, state S552");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp145, "return of Close, state S552");
            this.Manager.Comment("reaching state \'S581\'");
        }
        
        private void TestScenarioS5For2K8R2S2GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S3");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S20() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp154;
            bool temp155;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp155 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp154);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp154, temp155);
            this.Manager.Comment("reaching state \'S21\'");
            int temp175 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S20GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S20GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S20GetSUTOSVersionChecker2)));
            if ((temp175 == 0)) {
                this.Manager.Comment("reaching state \'S70\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S130\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S190\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp156;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp157;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp157 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp156);
                this.Manager.Comment("reaching state \'S250\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp156, "policyHandle of OpenPolicy, state S250");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp157, "return of OpenPolicy, state S250");
                this.Manager.Comment("reaching state \'S310\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp158;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp158 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S370\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp158, "return of SetDomainInformationPolicy, state S370");
                this.Manager.Comment("reaching state \'S430\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp159;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp160;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainKerberosTicketIn" +
                        "formation,out _)\'");
                temp160 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp159);
                this.Manager.Comment("reaching state \'S488\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp159, "policyInformation of QueryDomainInformationPolicy, state S488");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp160, "return of QueryDomainInformationPolicy, state S488");
                TestScenarioS5For2K8R2S520();
                goto label7;
            }
            if ((temp175 == 1)) {
                this.Manager.Comment("reaching state \'S71\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S131\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S191\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp161;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp162;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp162 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp161);
                this.Manager.Comment("reaching state \'S251\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp161, "policyHandle of OpenPolicy, state S251");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp162, "return of OpenPolicy, state S251");
                this.Manager.Comment("reaching state \'S311\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp163;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp163 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S371\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp163, "return of SetDomainInformationPolicy, state S371");
                TestScenarioS5For2K8R2S431();
                goto label7;
            }
            if ((temp175 == 2)) {
                this.Manager.Comment("reaching state \'S72\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S132\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S192\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp168;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp169;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp169 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp168);
                this.Manager.Comment("reaching state \'S252\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp168, "policyHandle of OpenPolicy, state S252");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp169, "return of OpenPolicy, state S252");
                this.Manager.Comment("reaching state \'S312\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp170;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp170 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S372\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp170, "return of SetDomainInformationPolicy, state S372");
                this.Manager.Comment("reaching state \'S432\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp171;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp172;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp172 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp171);
                this.Manager.Comment("reaching state \'S490\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp171, "policyInformation of QueryDomainInformationPolicy, state S490");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp172, "return of QueryDomainInformationPolicy, state S490");
                this.Manager.Comment("reaching state \'S534\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp173;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp174;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp174 = this.ILsadManagedAdapterInstance.Close(1, out temp173);
                this.Manager.Comment("reaching state \'S564\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp173, "handleAfterClose of Close, state S564");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp174, "return of Close, state S564");
                this.Manager.Comment("reaching state \'S593\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S20GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S21");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S21");
        }
        
        private void TestScenarioS5For2K8R2S20GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S21");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S21");
        }
        
        private void TestScenarioS5For2K8R2S431() {
            this.Manager.Comment("reaching state \'S431\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp164;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp165;
            this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                    "Information,out _)\'");
            temp165 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp164);
            this.Manager.Comment("reaching state \'S489\'");
            this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp164, "policyInformation of QueryDomainInformationPolicy, state S489");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp165, "return of QueryDomainInformationPolicy, state S489");
            TestScenarioS5For2K8R2S533();
        }
        
        private void TestScenarioS5For2K8R2S533() {
            this.Manager.Comment("reaching state \'S533\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp166;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp167;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp167 = this.ILsadManagedAdapterInstance.Close(1, out temp166);
            this.Manager.AddReturn(CloseInfo, null, temp166, temp167);
            TestScenarioS5For2K8R2S549();
        }
        
        private void TestScenarioS5For2K8R2S20GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S21");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        //[TestCategory("PDC")]
        //[TestCategory("DomainWin2008R2")]
        //[TestCategory("ForestWin2008R2")]
        //[TestCategory("MS-LSAD")]
        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S22() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp176;
            bool temp177;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp177 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp176);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp176, temp177);
            this.Manager.Comment("reaching state \'S23\'");
            int temp197 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S22GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S22GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S22GetSUTOSVersionChecker2)));
            if ((temp197 == 0)) {
                this.Manager.Comment("reaching state \'S73\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S133\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S193\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp178;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp179;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp179 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp178);
                this.Manager.Comment("reaching state \'S253\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp178, "policyHandle of OpenPolicy, state S253");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp179, "return of OpenPolicy, state S253");
                this.Manager.Comment("reaching state \'S313\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp180;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainEfsInformation)\'");
                temp180 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S373\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp180, "return of SetDomainInformationPolicy, state S373");
                this.Manager.Comment("reaching state \'S433\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp181;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp182;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp182 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp181);
                this.Manager.Comment("reaching state \'S491\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp181, "policyInformation of QueryDomainInformationPolicy, state S491");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp182, "return of QueryDomainInformationPolicy, state S491");
                TestScenarioS5For2K8R2S535();
                goto label8;
            }
            if ((temp197 == 1)) {
                this.Manager.Comment("reaching state \'S74\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S134\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S194\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp185;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp186;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp186 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp185);
                this.Manager.Comment("reaching state \'S254\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp185, "policyHandle of OpenPolicy, state S254");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp186, "return of OpenPolicy, state S254");
                this.Manager.Comment("reaching state \'S314\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp187;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp187 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S374\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp187, "return of SetDomainInformationPolicy, state S374");
                this.Manager.Comment("reaching state \'S434\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp188;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp189;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp189 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp188);
                this.Manager.Comment("reaching state \'S492\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp188, "policyInformation of QueryDomainInformationPolicy, state S492");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp189, "return of QueryDomainInformationPolicy, state S492");
                TestScenarioS5For2K8R2S533();
                goto label8;
            }
            if ((temp197 == 2)) {
                this.Manager.Comment("reaching state \'S75\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S135\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S195\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp190;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp191;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp191 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp190);
                this.Manager.Comment("reaching state \'S255\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp190, "policyHandle of OpenPolicy, state S255");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp191, "return of OpenPolicy, state S255");
                this.Manager.Comment("reaching state \'S315\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp192;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp192 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S375\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp192, "return of SetDomainInformationPolicy, state S375");
                this.Manager.Comment("reaching state \'S435\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp193;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp194;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp194 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp193);
                this.Manager.Comment("reaching state \'S493\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp193, "policyInformation of QueryDomainInformationPolicy, state S493");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp194, "return of QueryDomainInformationPolicy, state S493");
                this.Manager.Comment("reaching state \'S536\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp195;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp196;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp196 = this.ILsadManagedAdapterInstance.Close(1, out temp195);
                this.Manager.Comment("reaching state \'S565\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp195, "handleAfterClose of Close, state S565");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp196, "return of Close, state S565");
                this.Manager.Comment("reaching state \'S594\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S22GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S23");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S23");
        }
        
        private void TestScenarioS5For2K8R2S535() {
            this.Manager.Comment("reaching state \'S535\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp183;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp184;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp184 = this.ILsadManagedAdapterInstance.Close(1, out temp183);
            this.Manager.AddReturn(CloseInfo, null, temp183, temp184);
            TestScenarioS5For2K8R2S557();
        }
        
        private void TestScenarioS5For2K8R2S22GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S23");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S23");
        }
        
        private void TestScenarioS5For2K8R2S22GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S23");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S24() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp198;
            bool temp199;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp199 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp198);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp198, temp199);
            this.Manager.Comment("reaching state \'S25\'");
            int temp217 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S24GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S24GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S24GetSUTOSVersionChecker2)));
            if ((temp217 == 0)) {
                this.Manager.Comment("reaching state \'S76\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S136\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S196\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp200;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp201;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp201 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp200);
                this.Manager.Comment("reaching state \'S256\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp200, "policyHandle of OpenPolicy, state S256");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp201, "return of OpenPolicy, state S256");
                this.Manager.Comment("reaching state \'S316\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp202;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainKerberosTicketInfo" +
                        "rmation)\'");
                temp202 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S376\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp202, "return of SetDomainInformationPolicy, state S376");
                this.Manager.Comment("reaching state \'S436\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp203;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp204;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                        "Information,out _)\'");
                temp204 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp203);
                this.Manager.Comment("reaching state \'S494\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp203, "policyInformation of QueryDomainInformationPolicy, state S494");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp204, "return of QueryDomainInformationPolicy, state S494");
                TestScenarioS5For2K8R2S535();
                goto label9;
            }
            if ((temp217 == 1)) {
                this.Manager.Comment("reaching state \'S77\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S137\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S197\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp205;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp206;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp206 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp205);
                this.Manager.Comment("reaching state \'S257\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp205, "policyHandle of OpenPolicy, state S257");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp206, "return of OpenPolicy, state S257");
                this.Manager.Comment("reaching state \'S317\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp207;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainEfsInformation)\'");
                temp207 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S377\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp207, "return of SetDomainInformationPolicy, state S377");
                this.Manager.Comment("reaching state \'S437\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp208;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp209;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp209 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp208);
                this.Manager.Comment("reaching state \'S495\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp208, "policyInformation of QueryDomainInformationPolicy, state S495");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp209, "return of QueryDomainInformationPolicy, state S495");
                TestScenarioS5For2K8R2S533();
                goto label9;
            }
            if ((temp217 == 2)) {
                this.Manager.Comment("reaching state \'S78\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S138\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S198\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp210;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp211;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp211 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp210);
                this.Manager.Comment("reaching state \'S258\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp210, "policyHandle of OpenPolicy, state S258");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp211, "return of OpenPolicy, state S258");
                this.Manager.Comment("reaching state \'S318\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp212;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp212 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S378\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp212, "return of SetDomainInformationPolicy, state S378");
                this.Manager.Comment("reaching state \'S438\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp213;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp214;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp214 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp213);
                this.Manager.Comment("reaching state \'S496\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp213, "policyInformation of QueryDomainInformationPolicy, state S496");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp214, "return of QueryDomainInformationPolicy, state S496");
                this.Manager.Comment("reaching state \'S537\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp215;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp216;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp216 = this.ILsadManagedAdapterInstance.Close(1, out temp215);
                this.Manager.Comment("reaching state \'S566\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp215, "handleAfterClose of Close, state S566");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp216, "return of Close, state S566");
                this.Manager.Comment("reaching state \'S595\'");
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S24GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S25");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S25");
        }
        
        private void TestScenarioS5For2K8R2S24GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S25");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S25");
        }
        
        private void TestScenarioS5For2K8R2S24GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S25");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S26() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp218;
            bool temp219;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp219 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp218);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp218, temp219);
            this.Manager.Comment("reaching state \'S27\'");
            int temp239 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S26GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S26GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S26GetSUTOSVersionChecker2)));
            if ((temp239 == 0)) {
                this.Manager.Comment("reaching state \'S79\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S199\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp220;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp221;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp221 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp220);
                this.Manager.Comment("reaching state \'S259\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp220, "policyHandle of OpenPolicy, state S259");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp221, "return of OpenPolicy, state S259");
                this.Manager.Comment("reaching state \'S319\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp222;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainKerberosTicketInfo" +
                        "rmation)\'");
                temp222 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S379\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp222, "return of SetDomainInformationPolicy, state S379");
                this.Manager.Comment("reaching state \'S439\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp223;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp224;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                        "Information,out _)\'");
                temp224 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp223);
                this.Manager.Comment("reaching state \'S497\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp223, "policyInformation of QueryDomainInformationPolicy, state S497");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp224, "return of QueryDomainInformationPolicy, state S497");
                TestScenarioS5For2K8R2S525();
                goto label10;
            }
            if ((temp239 == 1)) {
                this.Manager.Comment("reaching state \'S80\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S200\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp225;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp226;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp226 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp225);
                this.Manager.Comment("reaching state \'S260\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp225, "policyHandle of OpenPolicy, state S260");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp226, "return of OpenPolicy, state S260");
                this.Manager.Comment("reaching state \'S320\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp227;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp227 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S380\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp227, "return of SetDomainInformationPolicy, state S380");
                this.Manager.Comment("reaching state \'S440\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp228;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp229;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp229 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp228);
                this.Manager.Comment("reaching state \'S498\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp228, "policyInformation of QueryDomainInformationPolicy, state S498");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp229, "return of QueryDomainInformationPolicy, state S498");
                TestScenarioS5For2K8R2S538();
                goto label10;
            }
            if ((temp239 == 2)) {
                this.Manager.Comment("reaching state \'S81\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S201\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp232;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp233;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp233 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp232);
                this.Manager.Comment("reaching state \'S261\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp232, "policyHandle of OpenPolicy, state S261");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp233, "return of OpenPolicy, state S261");
                this.Manager.Comment("reaching state \'S321\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp234;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp234 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S381\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp234, "return of SetDomainInformationPolicy, state S381");
                this.Manager.Comment("reaching state \'S441\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp235;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp236;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp236 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp235);
                this.Manager.Comment("reaching state \'S499\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp235, "policyInformation of QueryDomainInformationPolicy, state S499");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp236, "return of QueryDomainInformationPolicy, state S499");
                this.Manager.Comment("reaching state \'S539\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp237;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp238;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp238 = this.ILsadManagedAdapterInstance.Close(1, out temp237);
                this.Manager.Comment("reaching state \'S568\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp237, "handleAfterClose of Close, state S568");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp238, "return of Close, state S568");
                this.Manager.Comment("reaching state \'S597\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S26GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S27");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S27");
        }
        
        private void TestScenarioS5For2K8R2S26GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S27");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S27");
        }
        
        private void TestScenarioS5For2K8R2S538() {
            this.Manager.Comment("reaching state \'S538\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp230;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp231;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp231 = this.ILsadManagedAdapterInstance.Close(1, out temp230);
            this.Manager.Comment("reaching state \'S567\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp230, "handleAfterClose of Close, state S567");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp231, "return of Close, state S567");
            this.Manager.Comment("reaching state \'S596\'");
        }
        
        private void TestScenarioS5For2K8R2S26GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S27");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S28() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp240;
            bool temp241;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp241 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp240);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp240, temp241);
            this.Manager.Comment("reaching state \'S29\'");
            int temp259 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S28GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S28GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S28GetSUTOSVersionChecker2)));
            if ((temp259 == 0)) {
                this.Manager.Comment("reaching state \'S82\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S202\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp242;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp243;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp243 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp242);
                this.Manager.Comment("reaching state \'S262\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp242, "policyHandle of OpenPolicy, state S262");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp243, "return of OpenPolicy, state S262");
                this.Manager.Comment("reaching state \'S322\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp244;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp244 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S382\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp244, "return of SetDomainInformationPolicy, state S382");
                this.Manager.Comment("reaching state \'S442\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp245;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp246;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp246 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp245);
                this.Manager.Comment("reaching state \'S500\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp245, "policyInformation of QueryDomainInformationPolicy, state S500");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp246, "return of QueryDomainInformationPolicy, state S500");
                TestScenarioS5For2K8R2S538();
                goto label11;
            }
            if ((temp259 == 1)) {
                this.Manager.Comment("reaching state \'S83\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S143\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S203\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp247;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp248;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp248 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp247);
                this.Manager.Comment("reaching state \'S263\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp247, "policyHandle of OpenPolicy, state S263");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp248, "return of OpenPolicy, state S263");
                this.Manager.Comment("reaching state \'S323\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp249;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp249 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S383\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp249, "return of SetDomainInformationPolicy, state S383");
                this.Manager.Comment("reaching state \'S443\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp250;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp251;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp251 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp250);
                this.Manager.Comment("reaching state \'S501\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp250, "policyInformation of QueryDomainInformationPolicy, state S501");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp251, "return of QueryDomainInformationPolicy, state S501");
                TestScenarioS5For2K8R2S516();
                goto label11;
            }
            if ((temp259 == 2)) {
                this.Manager.Comment("reaching state \'S84\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S144\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S204\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp252;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp253;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp253 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp252);
                this.Manager.Comment("reaching state \'S264\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp252, "policyHandle of OpenPolicy, state S264");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp253, "return of OpenPolicy, state S264");
                this.Manager.Comment("reaching state \'S324\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp254;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp254 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S384\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp254, "return of SetDomainInformationPolicy, state S384");
                this.Manager.Comment("reaching state \'S444\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp255;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp256;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp256 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp255);
                this.Manager.Comment("reaching state \'S502\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp255, "policyInformation of QueryDomainInformationPolicy, state S502");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp256, "return of QueryDomainInformationPolicy, state S502");
                this.Manager.Comment("reaching state \'S540\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp257;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp258;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp258 = this.ILsadManagedAdapterInstance.Close(1, out temp257);
                this.Manager.Comment("reaching state \'S569\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp257, "handleAfterClose of Close, state S569");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp258, "return of Close, state S569");
                this.Manager.Comment("reaching state \'S598\'");
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S28GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S29");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S29");
        }
        
        private void TestScenarioS5For2K8R2S28GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S29");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S29");
        }
        
        private void TestScenarioS5For2K8R2S28GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S29");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S30() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp260;
            bool temp261;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp261 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp260);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp260, temp261);
            this.Manager.Comment("reaching state \'S31\'");
            int temp279 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S30GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S30GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S30GetSUTOSVersionChecker2)));
            if ((temp279 == 0)) {
                this.Manager.Comment("reaching state \'S85\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S145\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S205\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp262;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp263;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp263 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp262);
                this.Manager.Comment("reaching state \'S265\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp262, "policyHandle of OpenPolicy, state S265");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp263, "return of OpenPolicy, state S265");
                this.Manager.Comment("reaching state \'S325\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp264;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp264 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S385\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp264, "return of SetDomainInformationPolicy, state S385");
                this.Manager.Comment("reaching state \'S445\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp265;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp266;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                        "Information,out _)\'");
                temp266 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp265);
                this.Manager.Comment("reaching state \'S503\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp265, "policyInformation of QueryDomainInformationPolicy, state S503");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp266, "return of QueryDomainInformationPolicy, state S503");
                TestScenarioS5For2K8R2S538();
                goto label12;
            }
            if ((temp279 == 1)) {
                this.Manager.Comment("reaching state \'S86\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S206\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp267;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp268;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp268 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp267);
                this.Manager.Comment("reaching state \'S266\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp267, "policyHandle of OpenPolicy, state S266");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp268, "return of OpenPolicy, state S266");
                this.Manager.Comment("reaching state \'S326\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp269;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp269 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S386\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp269, "return of SetDomainInformationPolicy, state S386");
                this.Manager.Comment("reaching state \'S446\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp270;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp271;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp271 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp270);
                this.Manager.Comment("reaching state \'S504\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp270, "policyInformation of QueryDomainInformationPolicy, state S504");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp271, "return of QueryDomainInformationPolicy, state S504");
                TestScenarioS5For2K8R2S516();
                goto label12;
            }
            if ((temp279 == 2)) {
                this.Manager.Comment("reaching state \'S87\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S147\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S207\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp272;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp273;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp273 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp272);
                this.Manager.Comment("reaching state \'S267\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp272, "policyHandle of OpenPolicy, state S267");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp273, "return of OpenPolicy, state S267");
                this.Manager.Comment("reaching state \'S327\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp274;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp274 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S387\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp274, "return of SetDomainInformationPolicy, state S387");
                this.Manager.Comment("reaching state \'S447\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp275;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp276;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp276 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp275);
                this.Manager.Comment("reaching state \'S505\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp275, "policyInformation of QueryDomainInformationPolicy, state S505");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp276, "return of QueryDomainInformationPolicy, state S505");
                this.Manager.Comment("reaching state \'S541\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp277;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp278;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp278 = this.ILsadManagedAdapterInstance.Close(1, out temp277);
                this.Manager.Comment("reaching state \'S570\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp277, "handleAfterClose of Close, state S570");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp278, "return of Close, state S570");
                this.Manager.Comment("reaching state \'S599\'");
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S30GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S31");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S31");
        }
        
        private void TestScenarioS5For2K8R2S30GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S31");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S31");
        }
        
        private void TestScenarioS5For2K8R2S30GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S31");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S32() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp280;
            bool temp281;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp281 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp280);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp280, temp281);
            this.Manager.Comment("reaching state \'S33\'");
            int temp299 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S32GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S32GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S32GetSUTOSVersionChecker2)));
            if ((temp299 == 0)) {
                this.Manager.Comment("reaching state \'S88\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S148\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S208\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp282;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp283;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp283 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp282);
                this.Manager.Comment("reaching state \'S268\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp282, "policyHandle of OpenPolicy, state S268");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp283, "return of OpenPolicy, state S268");
                this.Manager.Comment("reaching state \'S328\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp284;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp284 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S388\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp284, "return of SetDomainInformationPolicy, state S388");
                this.Manager.Comment("reaching state \'S448\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp285;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp286;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp286 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp285);
                this.Manager.Comment("reaching state \'S506\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp285, "policyInformation of QueryDomainInformationPolicy, state S506");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp286, "return of QueryDomainInformationPolicy, state S506");
                TestScenarioS5For2K8R2S517();
                goto label13;
            }
            if ((temp299 == 1)) {
                this.Manager.Comment("reaching state \'S89\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S149\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S209\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp287;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp288;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp288 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp287);
                this.Manager.Comment("reaching state \'S269\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp287, "policyHandle of OpenPolicy, state S269");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp288, "return of OpenPolicy, state S269");
                this.Manager.Comment("reaching state \'S329\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp289;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainEfsInformation)\'");
                temp289 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S389\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp289, "return of SetDomainInformationPolicy, state S389");
                this.Manager.Comment("reaching state \'S449\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp290;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp291;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp291 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp290);
                this.Manager.Comment("reaching state \'S507\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp290, "policyInformation of QueryDomainInformationPolicy, state S507");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp291, "return of QueryDomainInformationPolicy, state S507");
                TestScenarioS5For2K8R2S516();
                goto label13;
            }
            if ((temp299 == 2)) {
                this.Manager.Comment("reaching state \'S90\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S150\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S210\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp292;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp293;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp293 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp292);
                this.Manager.Comment("reaching state \'S270\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp292, "policyHandle of OpenPolicy, state S270");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp293, "return of OpenPolicy, state S270");
                this.Manager.Comment("reaching state \'S330\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp294;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp294 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S390\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp294, "return of SetDomainInformationPolicy, state S390");
                this.Manager.Comment("reaching state \'S450\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp295;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp296;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp296 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp295);
                this.Manager.Comment("reaching state \'S508\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp295, "policyInformation of QueryDomainInformationPolicy, state S508");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp296, "return of QueryDomainInformationPolicy, state S508");
                this.Manager.Comment("reaching state \'S542\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp297;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp298;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp298 = this.ILsadManagedAdapterInstance.Close(1, out temp297);
                this.Manager.Comment("reaching state \'S571\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp297, "handleAfterClose of Close, state S571");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp298, "return of Close, state S571");
                this.Manager.Comment("reaching state \'S600\'");
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S32GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S33");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S33");
        }
        
        private void TestScenarioS5For2K8R2S32GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S33");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S33");
        }
        
        private void TestScenarioS5For2K8R2S32GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S33");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S34() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp300;
            bool temp301;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp301 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp300);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp300, temp301);
            this.Manager.Comment("reaching state \'S35\'");
            int temp317 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S34GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S34GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S34GetSUTOSVersionChecker2)));
            if ((temp317 == 0)) {
                this.Manager.Comment("reaching state \'S91\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S151\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S211\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp302;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp303;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp303 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp302);
                this.Manager.Comment("reaching state \'S271\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp302, "policyHandle of OpenPolicy, state S271");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp303, "return of OpenPolicy, state S271");
                this.Manager.Comment("reaching state \'S331\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp304;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainEfsInformation)\'");
                temp304 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S391\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp304, "return of SetDomainInformationPolicy, state S391");
                TestScenarioS5For2K8R2S401();
                goto label14;
            }
            if ((temp317 == 1)) {
                this.Manager.Comment("reaching state \'S92\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S152\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S212\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp305;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp306;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp306 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp305);
                this.Manager.Comment("reaching state \'S272\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp305, "policyHandle of OpenPolicy, state S272");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp306, "return of OpenPolicy, state S272");
                this.Manager.Comment("reaching state \'S332\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp307;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp307 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S392\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp307, "return of SetDomainInformationPolicy, state S392");
                this.Manager.Comment("reaching state \'S451\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp308;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp309;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainEfsInformation,o" +
                        "ut _)\'");
                temp309 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp308);
                this.Manager.Comment("reaching state \'S509\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp308, "policyInformation of QueryDomainInformationPolicy, state S509");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp309, "return of QueryDomainInformationPolicy, state S509");
                TestScenarioS5For2K8R2S524();
                goto label14;
            }
            if ((temp317 == 2)) {
                this.Manager.Comment("reaching state \'S93\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S153\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S213\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp310;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp311;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp311 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp310);
                this.Manager.Comment("reaching state \'S273\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp310, "policyHandle of OpenPolicy, state S273");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp311, "return of OpenPolicy, state S273");
                this.Manager.Comment("reaching state \'S333\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp312;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp312 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S393\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp312, "return of SetDomainInformationPolicy, state S393");
                this.Manager.Comment("reaching state \'S452\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp313;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp314;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp314 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp313);
                this.Manager.Comment("reaching state \'S510\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp313, "policyInformation of QueryDomainInformationPolicy, state S510");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp314, "return of QueryDomainInformationPolicy, state S510");
                this.Manager.Comment("reaching state \'S543\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp315;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp316;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp316 = this.ILsadManagedAdapterInstance.Close(1, out temp315);
                this.Manager.Comment("reaching state \'S572\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp315, "handleAfterClose of Close, state S572");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp316, "return of Close, state S572");
                this.Manager.Comment("reaching state \'S601\'");
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S34GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S35");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S35");
        }
        
        private void TestScenarioS5For2K8R2S34GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S35");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S35");
        }
        
        private void TestScenarioS5For2K8R2S34GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S35");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S36() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp318;
            bool temp319;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp319 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp318);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp318, temp319);
            this.Manager.Comment("reaching state \'S37\'");
            int temp339 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S36GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S36GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S36GetSUTOSVersionChecker2)));
            if ((temp339 == 0)) {
                this.Manager.Comment("reaching state \'S94\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S154\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S214\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp320;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp321;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp321 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp320);
                this.Manager.Comment("reaching state \'S274\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp320, "policyHandle of OpenPolicy, state S274");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp321, "return of OpenPolicy, state S274");
                this.Manager.Comment("reaching state \'S334\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp322;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp322 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S394\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp322, "return of SetDomainInformationPolicy, state S394");
                this.Manager.Comment("reaching state \'S453\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp323;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp324;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainKerberosTicketIn" +
                        "formation,out _)\'");
                temp324 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp323);
                this.Manager.Comment("reaching state \'S511\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp323, "policyInformation of QueryDomainInformationPolicy, state S511");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp324, "return of QueryDomainInformationPolicy, state S511");
                TestScenarioS5For2K8R2S538();
                goto label15;
            }
            if ((temp339 == 1)) {
                this.Manager.Comment("reaching state \'S95\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S215\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp325;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp326;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp326 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp325);
                this.Manager.Comment("reaching state \'S275\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp325, "policyHandle of OpenPolicy, state S275");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp326, "return of OpenPolicy, state S275");
                this.Manager.Comment("reaching state \'S335\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp327;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp327 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S395\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp327, "return of SetDomainInformationPolicy, state S395");
                this.Manager.Comment("reaching state \'S454\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp328;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp329;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp329 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp328);
                this.Manager.Comment("reaching state \'S512\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp328, "policyInformation of QueryDomainInformationPolicy, state S512");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp329, "return of QueryDomainInformationPolicy, state S512");
                this.Manager.Comment("reaching state \'S544\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp330;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp331;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp331 = this.ILsadManagedAdapterInstance.Close(1, out temp330);
                this.Manager.Comment("reaching state \'S573\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp330, "handleAfterClose of Close, state S573");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp331, "return of Close, state S573");
                this.Manager.Comment("reaching state \'S602\'");
                goto label15;
            }
            if ((temp339 == 2)) {
                this.Manager.Comment("reaching state \'S96\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S156\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S216\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp332;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp333;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp333 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp332);
                this.Manager.Comment("reaching state \'S276\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp332, "policyHandle of OpenPolicy, state S276");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp333, "return of OpenPolicy, state S276");
                this.Manager.Comment("reaching state \'S336\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp334;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp334 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S396\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp334, "return of SetDomainInformationPolicy, state S396");
                this.Manager.Comment("reaching state \'S455\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp335;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp336;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp336 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp335);
                this.Manager.Comment("reaching state \'S513\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp335, "policyInformation of QueryDomainInformationPolicy, state S513");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp336, "return of QueryDomainInformationPolicy, state S513");
                this.Manager.Comment("reaching state \'S545\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp337;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp338;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp338 = this.ILsadManagedAdapterInstance.Close(1, out temp337);
                this.Manager.Comment("reaching state \'S574\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp337, "handleAfterClose of Close, state S574");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp338, "return of Close, state S574");
                this.Manager.Comment("reaching state \'S603\'");
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S36GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S37");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S37");
        }
        
        private void TestScenarioS5For2K8R2S36GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S37");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S37");
        }
        
        private void TestScenarioS5For2K8R2S36GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S37");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S38() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp340;
            bool temp341;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp341 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp340);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp340, temp341);
            this.Manager.Comment("reaching state \'S39\'");
            int temp359 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S38GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S38GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S38GetSUTOSVersionChecker2)));
            if ((temp359 == 0)) {
                this.Manager.Comment("reaching state \'S97\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S157\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S217\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp342;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp343;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp343 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp342);
                this.Manager.Comment("reaching state \'S277\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp342, "policyHandle of OpenPolicy, state S277");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp343, "return of OpenPolicy, state S277");
                this.Manager.Comment("reaching state \'S337\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp344;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(53,PolicyDomainKerberosTicketInfo" +
                        "rmation)\'");
                temp344 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S397\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/InvalidHandle\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp344, "return of SetDomainInformationPolicy, state S397");
                TestScenarioS5For2K8R2S431();
                goto label16;
            }
            if ((temp359 == 1)) {
                this.Manager.Comment("reaching state \'S98\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S218\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp345;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp346;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp346 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp345);
                this.Manager.Comment("reaching state \'S278\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp345, "policyHandle of OpenPolicy, state S278");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp346, "return of OpenPolicy, state S278");
                this.Manager.Comment("reaching state \'S338\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp347;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp347 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S398\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp347, "return of SetDomainInformationPolicy, state S398");
                this.Manager.Comment("reaching state \'S456\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp348;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp349;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp349 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp348);
                this.Manager.Comment("reaching state \'S514\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp348, "policyInformation of QueryDomainInformationPolicy, state S514");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp349, "return of QueryDomainInformationPolicy, state S514");
                this.Manager.Comment("reaching state \'S546\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp350;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp351;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp351 = this.ILsadManagedAdapterInstance.Close(1, out temp350);
                this.Manager.Comment("reaching state \'S575\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp350, "handleAfterClose of Close, state S575");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp351, "return of Close, state S575");
                this.Manager.Comment("reaching state \'S604\'");
                goto label16;
            }
            if ((temp359 == 2)) {
                this.Manager.Comment("reaching state \'S99\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S159\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S219\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp352;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp353;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp353 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp352);
                this.Manager.Comment("reaching state \'S279\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp352, "policyHandle of OpenPolicy, state S279");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp353, "return of OpenPolicy, state S279");
                this.Manager.Comment("reaching state \'S339\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp354;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp354 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S399\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp354, "return of SetDomainInformationPolicy, state S399");
                this.Manager.Comment("reaching state \'S457\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp355;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp356;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp356 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp355);
                this.Manager.Comment("reaching state \'S515\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp355, "policyInformation of QueryDomainInformationPolicy, state S515");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp356, "return of QueryDomainInformationPolicy, state S515");
                this.Manager.Comment("reaching state \'S547\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp357;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp358;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp358 = this.ILsadManagedAdapterInstance.Close(1, out temp357);
                this.Manager.Comment("reaching state \'S576\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp357, "handleAfterClose of Close, state S576");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp358, "return of Close, state S576");
                this.Manager.Comment("reaching state \'S605\'");
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S38GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S39");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S39");
        }
        
        private void TestScenarioS5For2K8R2S38GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S39");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S39");
        }
        
        private void TestScenarioS5For2K8R2S38GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S39");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S4() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp360;
            bool temp361;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp361 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp360);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp360, temp361);
            this.Manager.Comment("reaching state \'S5\'");
            int temp379 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S4GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S4GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S4GetSUTOSVersionChecker2)));
            if ((temp379 == 0)) {
                this.Manager.Comment("reaching state \'S46\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S106\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S166\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp362;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp363;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp363 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp362);
                this.Manager.Comment("reaching state \'S226\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp362, "policyHandle of OpenPolicy, state S226");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp363, "return of OpenPolicy, state S226");
                this.Manager.Comment("reaching state \'S286\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp364;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp364 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S346\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp364, "return of SetDomainInformationPolicy, state S346");
                this.Manager.Comment("reaching state \'S406\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp365;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp366;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp366 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp365);
                this.Manager.Comment("reaching state \'S464\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp365, "policyInformation of QueryDomainInformationPolicy, state S464");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp366, "return of QueryDomainInformationPolicy, state S464");
                TestScenarioS5For2K8R2S519();
                goto label17;
            }
            if ((temp379 == 1)) {
                this.Manager.Comment("reaching state \'S47\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S107\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S167\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp367;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp368;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp368 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp367);
                this.Manager.Comment("reaching state \'S227\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp367, "policyHandle of OpenPolicy, state S227");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp368, "return of OpenPolicy, state S227");
                this.Manager.Comment("reaching state \'S287\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp369;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp369 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S347\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp369, "return of SetDomainInformationPolicy, state S347");
                this.Manager.Comment("reaching state \'S407\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp370;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp371;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainQualityOfServiceI" +
                        "nformation,out _)\'");
                temp371 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp370);
                this.Manager.Comment("reaching state \'S465\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp370, "policyInformation of QueryDomainInformationPolicy, state S465");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp371, "return of QueryDomainInformationPolicy, state S465");
                TestScenarioS5For2K8R2S520();
                goto label17;
            }
            if ((temp379 == 2)) {
                this.Manager.Comment("reaching state \'S48\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S108\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S168\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp372;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp373;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp373 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp372);
                this.Manager.Comment("reaching state \'S228\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp372, "policyHandle of OpenPolicy, state S228");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp373, "return of OpenPolicy, state S228");
                this.Manager.Comment("reaching state \'S288\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp374;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp374 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S348\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp374, "return of SetDomainInformationPolicy, state S348");
                this.Manager.Comment("reaching state \'S408\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp375;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp376;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp376 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp375);
                this.Manager.Comment("reaching state \'S466\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp375, "policyInformation of QueryDomainInformationPolicy, state S466");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp376, "return of QueryDomainInformationPolicy, state S466");
                this.Manager.Comment("reaching state \'S522\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp377;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp378;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp378 = this.ILsadManagedAdapterInstance.Close(1, out temp377);
                this.Manager.Comment("reaching state \'S554\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp377, "handleAfterClose of Close, state S554");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp378, "return of Close, state S554");
                this.Manager.Comment("reaching state \'S583\'");
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S4GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S5");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S5");
        }
        
        private void TestScenarioS5For2K8R2S4GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S5");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S5");
        }
        
        private void TestScenarioS5For2K8R2S4GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S5");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S5");
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S6() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp380;
            bool temp381;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp381 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp380);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp380, temp381);
            this.Manager.Comment("reaching state \'S7\'");
            int temp399 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S6GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S6GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S6GetSUTOSVersionChecker2)));
            if ((temp399 == 0)) {
                this.Manager.Comment("reaching state \'S49\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S109\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S169\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp382;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp383;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp383 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp382);
                this.Manager.Comment("reaching state \'S229\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp382, "policyHandle of OpenPolicy, state S229");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp383, "return of OpenPolicy, state S229");
                this.Manager.Comment("reaching state \'S289\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp384;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp384 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S349\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp384, "return of SetDomainInformationPolicy, state S349");
                this.Manager.Comment("reaching state \'S409\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp385;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp386;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                        "Information,out _)\'");
                temp386 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp385);
                this.Manager.Comment("reaching state \'S467\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp385, "policyInformation of QueryDomainInformationPolicy, state S467");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp386, "return of QueryDomainInformationPolicy, state S467");
                TestScenarioS5For2K8R2S519();
                goto label18;
            }
            if ((temp399 == 1)) {
                this.Manager.Comment("reaching state \'S50\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S110\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S170\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp387;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp388;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp388 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp387);
                this.Manager.Comment("reaching state \'S230\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp387, "policyHandle of OpenPolicy, state S230");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp388, "return of OpenPolicy, state S230");
                this.Manager.Comment("reaching state \'S290\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp389;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp389 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S350\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp389, "return of SetDomainInformationPolicy, state S350");
                this.Manager.Comment("reaching state \'S410\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp390;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp391;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(53,PolicyDomainQualityOfService" +
                        "Information,out _)\'");
                temp391 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass)(1)), out temp390);
                this.Manager.Comment("reaching state \'S468\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:InvalidParameter" +
                        "\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp390, "policyInformation of QueryDomainInformationPolicy, state S468");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp391, "return of QueryDomainInformationPolicy, state S468");
                TestScenarioS5For2K8R2S520();
                goto label18;
            }
            if ((temp399 == 2)) {
                this.Manager.Comment("reaching state \'S51\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S111\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S171\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp392;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp393;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp393 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp392);
                this.Manager.Comment("reaching state \'S231\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp392, "policyHandle of OpenPolicy, state S231");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp393, "return of OpenPolicy, state S231");
                this.Manager.Comment("reaching state \'S291\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp394;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp394 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S351\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp394, "return of SetDomainInformationPolicy, state S351");
                this.Manager.Comment("reaching state \'S411\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp395;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp396;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp396 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp395);
                this.Manager.Comment("reaching state \'S469\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp395, "policyInformation of QueryDomainInformationPolicy, state S469");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp396, "return of QueryDomainInformationPolicy, state S469");
                this.Manager.Comment("reaching state \'S523\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp397;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp398;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp398 = this.ILsadManagedAdapterInstance.Close(1, out temp397);
                this.Manager.Comment("reaching state \'S555\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp397, "handleAfterClose of Close, state S555");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp398, "return of Close, state S555");
                this.Manager.Comment("reaching state \'S584\'");
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S6GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S7");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S7");
        }
        
        private void TestScenarioS5For2K8R2S6GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S7");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S7");
        }
        
        private void TestScenarioS5For2K8R2S6GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S7");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S7");
        }
        #endregion
        
        #region Test Starting in S8
        //[TestCategory("PDC")]
        //[TestCategory("DomainWin2008R2")]
        //[TestCategory("ForestWin2008R2")]
        //[TestCategory("MS-LSAD")]
        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS5For2K8R2S8() {
            this.Manager.BeginTest("TestScenarioS5For2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server temp400;
            bool temp401;
            this.Manager.Comment("executing step \'call GetSUTOSVersion(out _)\'");
            temp401 = this.ILsadManagedAdapterInstance.GetSUTOSVersion(out temp400);
            this.Manager.AddReturn(GetSUTOSVersionInfo, null, temp400, temp401);
            this.Manager.Comment("reaching state \'S9\'");
            int temp419 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S8GetSUTOSVersionChecker)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S8GetSUTOSVersionChecker1)), new ExpectedReturn(TestScenarioS5For2K8R2.GetSUTOSVersionInfo, null, new GetSUTOSVersionDelegate1(this.TestScenarioS5For2K8R2S8GetSUTOSVersionChecker2)));
            if ((temp419 == 0)) {
                this.Manager.Comment("reaching state \'S52\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S112\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S172\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp402;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp403;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp403 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp402);
                this.Manager.Comment("reaching state \'S232\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp402, "policyHandle of OpenPolicy, state S232");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp403, "return of OpenPolicy, state S232");
                this.Manager.Comment("reaching state \'S292\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp404;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp404 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S352\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp404, "return of SetDomainInformationPolicy, state S352");
                this.Manager.Comment("reaching state \'S412\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp405;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp406;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainEfsInformation,ou" +
                        "t _)\'");
                temp406 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation, out temp405);
                this.Manager.Comment("reaching state \'S470\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp405, "policyInformation of QueryDomainInformationPolicy, state S470");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp406, "return of QueryDomainInformationPolicy, state S470");
                TestScenarioS5For2K8R2S524();
                goto label19;
            }
            if ((temp419 == 1)) {
                this.Manager.Comment("reaching state \'S53\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S113\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S173\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp407;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp408;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
                temp408 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp407);
                this.Manager.Comment("reaching state \'S233\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp407, "policyHandle of OpenPolicy, state S233");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp408, "return of OpenPolicy, state S233");
                this.Manager.Comment("reaching state \'S293\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp409;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainEfsInformation)\'");
                temp409 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainEfsInformation);
                this.Manager.Comment("reaching state \'S353\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp409, "return of SetDomainInformationPolicy, state S353");
                this.Manager.Comment("reaching state \'S413\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp410;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp411;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp411 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp410);
                this.Manager.Comment("reaching state \'S471\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(1)), temp410, "policyInformation of QueryDomainInformationPolicy, state S471");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp411, "return of QueryDomainInformationPolicy, state S471");
                TestScenarioS5For2K8R2S525();
                goto label19;
            }
            if ((temp419 == 2)) {
                this.Manager.Comment("reaching state \'S54\'");
                this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8r2,2,True)\'");
                this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2, 2, true);
                this.Manager.Comment("reaching state \'S114\'");
                this.Manager.Comment("checking step \'return Initialize\'");
                this.Manager.Comment("reaching state \'S174\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp412;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp413;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1027,out _)\'");
                temp413 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1027u, out temp412);
                this.Manager.Comment("reaching state \'S234\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp412, "policyHandle of OpenPolicy, state S234");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp413, "return of OpenPolicy, state S234");
                this.Manager.Comment("reaching state \'S294\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp414;
                this.Manager.Comment("executing step \'call SetDomainInformationPolicy(1,PolicyDomainKerberosTicketInfor" +
                        "mation)\'");
                temp414 = this.ILsadManagedAdapterInstance.SetDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation);
                this.Manager.Comment("reaching state \'S354\'");
                this.Manager.Comment("checking step \'return SetDomainInformationPolicy/Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp414, "return of SetDomainInformationPolicy, state S354");
                this.Manager.Comment("reaching state \'S414\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation temp415;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp416;
                this.Manager.Comment("executing step \'call QueryDomainInformationPolicy(1,PolicyDomainKerberosTicketInf" +
                        "ormation,out _)\'");
                temp416 = this.ILsadManagedAdapterInstance.QueryDomainInformationPolicy(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainInformationClass.PolicyDomainKerberosTicketInformation, out temp415);
                this.Manager.Comment("reaching state \'S472\'");
                this.Manager.Comment("checking step \'return QueryDomainInformationPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PolicyInformation)(0)), temp415, "policyInformation of QueryDomainInformationPolicy, state S472");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp416, "return of QueryDomainInformationPolicy, state S472");
                this.Manager.Comment("reaching state \'S526\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp417;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp418;
                this.Manager.Comment("executing step \'call Close(1,out _)\'");
                temp418 = this.ILsadManagedAdapterInstance.Close(1, out temp417);
                this.Manager.Comment("reaching state \'S558\'");
                this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp417, "handleAfterClose of Close, state S558");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp418, "return of Close, state S558");
                this.Manager.Comment("reaching state \'S587\'");
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS5For2K8R2S8GetSUTOSVersionChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k3]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k3 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S9");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S9");
        }
        
        private void TestScenarioS5For2K8R2S8GetSUTOSVersionChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S9");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S9");
        }
        
        private void TestScenarioS5For2K8R2S8GetSUTOSVersionChecker2(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server sutOSVersion, bool @return) {
            this.Manager.Comment("checking step \'return GetSUTOSVersion/[out Windows2k8r2]:True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8r2 <= sutOSVersion, "sutOSVersion of GetSUTOSVersion, state S9");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, @return, "return of GetSUTOSVersion, state S9");
        }
        #endregion
    }
}
