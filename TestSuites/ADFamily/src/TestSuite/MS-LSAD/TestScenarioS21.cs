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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3130.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TestScenarioS21 : PtfTestClassBase {
        
        public TestScenarioS21() {
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
        public void LSAD_TestScenarioS21S0() {
            this.Manager.BeginTest("TestScenarioS21S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S48");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S48");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(53,Valid,\"invalidvalue\",out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "invalidvalue", out temp2);
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp2, "luid of LookupPrivilegeValue, state S80");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp3, "return of LookupPrivilegeValue, state S80");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp4);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), temp4, "privilegeName of LookupPrivilegeName, state S112");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp5, "return of LookupPrivilegeName, state S112");
            this.Manager.Comment("reaching state \'S128\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(53,Valid,\"invalidvalue\",out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "invalidvalue", out temp6);
            this.Manager.Comment("reaching state \'S144\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp6, "displayName of LookupPrivilegeDisplayName, state S144");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp7, "return of LookupPrivilegeDisplayName, state S144");
            TestScenarioS21S160();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS21S160() {
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp9 = this.ILsadManagedAdapterInstance.Close(1, out temp8);
            this.Manager.AddReturn(CloseInfo, null, temp8, temp9);
            TestScenarioS21S162();
        }
        
        private void TestScenarioS21S162() {
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS21.CloseInfo, null, new CloseDelegate1(this.TestScenarioS21S0CloseChecker)));
            this.Manager.Comment("reaching state \'S163\'");
        }
        
        private void TestScenarioS21S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S162");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S162");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S10() {
            this.Manager.BeginTest("TestScenarioS21S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S37\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp10);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp10, "policyHandle of OpenPolicy, state S53");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp11, "return of OpenPolicy, state S53");
            this.Manager.Comment("reaching state \'S69\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(1,Invalid,\"invalidvalue\",out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "invalidvalue", out temp12);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp12, "luid of LookupPrivilegeValue, state S85");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp13, "return of LookupPrivilegeValue, state S85");
            this.Manager.Comment("reaching state \'S101\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp14);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp14, "privilegeName of LookupPrivilegeName, state S117");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp15, "return of LookupPrivilegeName, state S117");
            this.Manager.Comment("reaching state \'S133\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(1,Invalid,\"invalidvalue\",out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "invalidvalue", out temp16);
            this.Manager.Comment("reaching state \'S149\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp16, "displayName of LookupPrivilegeDisplayName, state S149");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp17, "return of LookupPrivilegeDisplayName, state S149");
            TestScenarioS21S161();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS21S161() {
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp19 = this.ILsadManagedAdapterInstance.Close(1, out temp18);
            this.Manager.AddReturn(CloseInfo, null, temp18, temp19);
            TestScenarioS21S162();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S12() {
            this.Manager.BeginTest("TestScenarioS21S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp21 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp20);
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp20, "policyHandle of OpenPolicy, state S54");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp21, "return of OpenPolicy, state S54");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(53,Invalid,\"invalidvalue\",out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "invalidvalue", out temp22);
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp22, "luid of LookupPrivilegeValue, state S86");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp23, "return of LookupPrivilegeValue, state S86");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp24);
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp24, "privilegeName of LookupPrivilegeName, state S118");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp25, "return of LookupPrivilegeName, state S118");
            this.Manager.Comment("reaching state \'S134\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(53,Invalid,\"invalidvalue\",out _)\'" +
                    "");
            temp27 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "invalidvalue", out temp26);
            this.Manager.Comment("reaching state \'S150\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp26, "displayName of LookupPrivilegeDisplayName, state S150");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp27, "return of LookupPrivilegeDisplayName, state S150");
            TestScenarioS21S161();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S14() {
            this.Manager.BeginTest("TestScenarioS21S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S39\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp28);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp28, "policyHandle of OpenPolicy, state S55");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of OpenPolicy, state S55");
            this.Manager.Comment("reaching state \'S71\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(53,Valid,\"SeDebugPrivilege\",out _)\'");
            temp31 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SeDebugPrivilege", out temp30);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp30, "luid of LookupPrivilegeValue, state S87");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp31, "return of LookupPrivilegeValue, state S87");
            this.Manager.Comment("reaching state \'S103\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp33 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp32);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp32, "privilegeName of LookupPrivilegeName, state S119");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp33, "return of LookupPrivilegeName, state S119");
            this.Manager.Comment("reaching state \'S135\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(53,Valid,\"SeAssignPrimaryTokenPri" +
                    "vilege\",out _)\'");
            temp35 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SeAssignPrimaryTokenPrivilege", out temp34);
            this.Manager.Comment("reaching state \'S151\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp34, "displayName of LookupPrivilegeDisplayName, state S151");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp35, "return of LookupPrivilegeDisplayName, state S151");
            TestScenarioS21S161();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S16() {
            this.Manager.BeginTest("TestScenarioS21S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp37 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp36);
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp36, "policyHandle of OpenPolicy, state S56");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp37, "return of OpenPolicy, state S56");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(1,Valid,\"SeDebugPrivilege\",out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SeDebugPrivilege", out temp38);
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), temp38, "luid of LookupPrivilegeValue, state S88");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp39, "return of LookupPrivilegeValue, state S88");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp40);
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), temp40, "privilegeName of LookupPrivilegeName, state S120");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of LookupPrivilegeName, state S120");
            this.Manager.Comment("reaching state \'S136\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(1,Valid,\"SeAssignPrimaryTokenPriv" +
                    "ilege\",out _)\'");
            temp43 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SeAssignPrimaryTokenPrivilege", out temp42);
            this.Manager.Comment("reaching state \'S152\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), temp42, "displayName of LookupPrivilegeDisplayName, state S152");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp43, "return of LookupPrivilegeDisplayName, state S152");
            TestScenarioS21S160();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S18() {
            this.Manager.BeginTest("TestScenarioS21S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S41\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp44;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp45 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp44);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp44, "policyHandle of OpenPolicy, state S57");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp45, "return of OpenPolicy, state S57");
            this.Manager.Comment("reaching state \'S73\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp46;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(1,Invalid,\"SeDebugPrivilege\",out _)\'");
            temp47 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SeDebugPrivilege", out temp46);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp46, "luid of LookupPrivilegeValue, state S89");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp47, "return of LookupPrivilegeValue, state S89");
            this.Manager.Comment("reaching state \'S105\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,77}\",out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,77}", out temp48);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:NoSuchPrivilege\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp48, "privilegeName of LookupPrivilegeName, state S121");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchPrivilege, temp49, "return of LookupPrivilegeName, state S121");
            this.Manager.Comment("reaching state \'S137\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp50;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(1,Invalid,\"SeAssignPrimaryTokenPr" +
                    "ivilege\",out _)\'");
            temp51 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SeAssignPrimaryTokenPrivilege", out temp50);
            this.Manager.Comment("reaching state \'S153\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp50, "displayName of LookupPrivilegeDisplayName, state S153");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp51, "return of LookupPrivilegeDisplayName, state S153");
            TestScenarioS21S160();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S2() {
            this.Manager.BeginTest("TestScenarioS21S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S33\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp53 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp52);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp52, "policyHandle of OpenPolicy, state S49");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp53, "return of OpenPolicy, state S49");
            this.Manager.Comment("reaching state \'S65\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(1,Valid,\"SeDebugPrivilege\",out _)\'");
            temp55 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SeDebugPrivilege", out temp54);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp54, "luid of LookupPrivilegeValue, state S81");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp55, "return of LookupPrivilegeValue, state S81");
            this.Manager.Comment("reaching state \'S97\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp56;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp57;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp57 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp56);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp56, "privilegeName of LookupPrivilegeName, state S113");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp57, "return of LookupPrivilegeName, state S113");
            this.Manager.Comment("reaching state \'S129\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp58;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(1,Valid,\"SeAssignPrimaryTokenPriv" +
                    "ilege\",out _)\'");
            temp59 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SeAssignPrimaryTokenPrivilege", out temp58);
            this.Manager.Comment("reaching state \'S145\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp58, "displayName of LookupPrivilegeDisplayName, state S145");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp59, "return of LookupPrivilegeDisplayName, state S145");
            TestScenarioS21S161();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S20() {
            this.Manager.BeginTest("TestScenarioS21S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp60;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp61 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp60);
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp60, "policyHandle of OpenPolicy, state S58");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp61, "return of OpenPolicy, state S58");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(53,Invalid,\"SeDebugPrivilege\",out _)\'");
            temp63 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SeDebugPrivilege", out temp62);
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp62, "luid of LookupPrivilegeValue, state S90");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp63, "return of LookupPrivilegeValue, state S90");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp64;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp65;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(53,Valid,\"{0,20}\",out _)\'");
            temp65 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp64);
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp64, "privilegeName of LookupPrivilegeName, state S122");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp65, "return of LookupPrivilegeName, state S122");
            this.Manager.Comment("reaching state \'S138\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp66;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(53,Invalid,\"SeAssignPrimaryTokenP" +
                    "rivilege\",out _)\'");
            temp67 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SeAssignPrimaryTokenPrivilege", out temp66);
            this.Manager.Comment("reaching state \'S154\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp66, "displayName of LookupPrivilegeDisplayName, state S154");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp67, "return of LookupPrivilegeDisplayName, state S154");
            TestScenarioS21S160();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S22() {
            this.Manager.BeginTest("TestScenarioS21S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S43\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp68);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp68, "policyHandle of OpenPolicy, state S59");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp69, "return of OpenPolicy, state S59");
            this.Manager.Comment("reaching state \'S75\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(1,Invalid,\"invalidvalue\",out _)\'");
            temp71 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "invalidvalue", out temp70);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp70, "luid of LookupPrivilegeValue, state S91");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp71, "return of LookupPrivilegeValue, state S91");
            this.Manager.Comment("reaching state \'S107\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp72;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(53,Valid,\"{0,77}\",out _)\'");
            temp73 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,77}", out temp72);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp72, "privilegeName of LookupPrivilegeName, state S123");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp73, "return of LookupPrivilegeName, state S123");
            this.Manager.Comment("reaching state \'S139\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp74;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp75;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(1,Invalid,\"invalidvalue\",out _)\'");
            temp75 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "invalidvalue", out temp74);
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp74, "displayName of LookupPrivilegeDisplayName, state S155");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp75, "return of LookupPrivilegeDisplayName, state S155");
            TestScenarioS21S160();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S24() {
            this.Manager.BeginTest("TestScenarioS21S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp76;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp77 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp76);
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp76, "policyHandle of OpenPolicy, state S60");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp77, "return of OpenPolicy, state S60");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp78;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(53,Invalid,\"invalidvalue\",out _)\'");
            temp79 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "invalidvalue", out temp78);
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp78, "luid of LookupPrivilegeValue, state S92");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp79, "return of LookupPrivilegeValue, state S92");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp80;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp81 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp80);
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), temp80, "privilegeName of LookupPrivilegeName, state S124");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp81, "return of LookupPrivilegeName, state S124");
            this.Manager.Comment("reaching state \'S140\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp82;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp83;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(53,Invalid,\"invalidvalue\",out _)\'" +
                    "");
            temp83 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "invalidvalue", out temp82);
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp82, "displayName of LookupPrivilegeDisplayName, state S156");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp83, "return of LookupPrivilegeDisplayName, state S156");
            TestScenarioS21S160();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S26() {
            this.Manager.BeginTest("TestScenarioS21S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S45\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp84;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp85 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp84);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp84, "policyHandle of OpenPolicy, state S61");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp85, "return of OpenPolicy, state S61");
            this.Manager.Comment("reaching state \'S77\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp86;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(1,Valid,\"invalidvalue\",out _)\'");
            temp87 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "invalidvalue", out temp86);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:NoSuchPrivilege\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp86, "luid of LookupPrivilegeValue, state S93");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchPrivilege, temp87, "return of LookupPrivilegeValue, state S93");
            this.Manager.Comment("reaching state \'S109\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp88;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp89 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp88);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), temp88, "privilegeName of LookupPrivilegeName, state S125");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp89, "return of LookupPrivilegeName, state S125");
            this.Manager.Comment("reaching state \'S141\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp90;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp91;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(1,Valid,\"invalidvalue\",out _)\'");
            temp91 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "invalidvalue", out temp90);
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:NoSuchPrivilege\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp90, "displayName of LookupPrivilegeDisplayName, state S157");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchPrivilege, temp91, "return of LookupPrivilegeDisplayName, state S157");
            TestScenarioS21S160();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S28() {
            this.Manager.BeginTest("TestScenarioS21S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp92;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp93;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp93 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp92);
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp92, "policyHandle of OpenPolicy, state S62");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp93, "return of OpenPolicy, state S62");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp94;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp95;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(53,Valid,\"SeDebugPrivilege\",out _)\'");
            temp95 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SeDebugPrivilege", out temp94);
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp94, "luid of LookupPrivilegeValue, state S94");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp95, "return of LookupPrivilegeValue, state S94");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp96;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp97 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp96);
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), temp96, "privilegeName of LookupPrivilegeName, state S126");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp97, "return of LookupPrivilegeName, state S126");
            this.Manager.Comment("reaching state \'S142\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp98;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp99;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(53,Valid,\"SeAssignPrimaryTokenPri" +
                    "vilege\",out _)\'");
            temp99 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SeAssignPrimaryTokenPrivilege", out temp98);
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp98, "displayName of LookupPrivilegeDisplayName, state S158");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp99, "return of LookupPrivilegeDisplayName, state S158");
            TestScenarioS21S160();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S30() {
            this.Manager.BeginTest("TestScenarioS21S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S47\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp100;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp101 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp100);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp100, "policyHandle of OpenPolicy, state S63");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp101, "return of OpenPolicy, state S63");
            this.Manager.Comment("reaching state \'S79\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp102;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(53,Valid,\"invalidvalue\",out _)\'");
            temp103 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "invalidvalue", out temp102);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp102, "luid of LookupPrivilegeValue, state S95");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp103, "return of LookupPrivilegeValue, state S95");
            this.Manager.Comment("reaching state \'S111\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp104;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,20}\",out _)\'");
            temp105 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp104);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp104, "privilegeName of LookupPrivilegeName, state S127");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp105, "return of LookupPrivilegeName, state S127");
            this.Manager.Comment("reaching state \'S143\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp106;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp107;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(53,Valid,\"invalidvalue\",out _)\'");
            temp107 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "invalidvalue", out temp106);
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp106, "displayName of LookupPrivilegeDisplayName, state S159");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp107, "return of LookupPrivilegeDisplayName, state S159");
            TestScenarioS21S161();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S4() {
            this.Manager.BeginTest("TestScenarioS21S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp108;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp109;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp109 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp108);
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp108, "policyHandle of OpenPolicy, state S50");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp109, "return of OpenPolicy, state S50");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp110;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp111;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(1,Valid,\"invalidvalue\",out _)\'");
            temp111 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "invalidvalue", out temp110);
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp110, "luid of LookupPrivilegeValue, state S82");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp111, "return of LookupPrivilegeValue, state S82");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp112;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp113;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(1,Valid,\"{0,77}\",out _)\'");
            temp113 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,77}", out temp112);
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp112, "privilegeName of LookupPrivilegeName, state S114");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp113, "return of LookupPrivilegeName, state S114");
            this.Manager.Comment("reaching state \'S130\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp114;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(1,Valid,\"invalidvalue\",out _)\'");
            temp115 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "invalidvalue", out temp114);
            this.Manager.Comment("reaching state \'S146\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp114, "displayName of LookupPrivilegeDisplayName, state S146");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp115, "return of LookupPrivilegeDisplayName, state S146");
            TestScenarioS21S161();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S6() {
            this.Manager.BeginTest("TestScenarioS21S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S35\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp116;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp117 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp116);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp116, "policyHandle of OpenPolicy, state S51");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp117, "return of OpenPolicy, state S51");
            this.Manager.Comment("reaching state \'S67\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp118;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp119;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(1,Invalid,\"SeDebugPrivilege\",out _)\'");
            temp119 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SeDebugPrivilege", out temp118);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp118, "luid of LookupPrivilegeValue, state S83");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp119, "return of LookupPrivilegeValue, state S83");
            this.Manager.Comment("reaching state \'S99\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp120;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp121;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(53,Valid,\"{0,20}\",out _)\'");
            temp121 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,20}", out temp120);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp120, "privilegeName of LookupPrivilegeName, state S115");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp121, "return of LookupPrivilegeName, state S115");
            this.Manager.Comment("reaching state \'S131\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp122;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp123;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(1,Invalid,\"SeAssignPrimaryTokenPr" +
                    "ivilege\",out _)\'");
            temp123 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SeAssignPrimaryTokenPrivilege", out temp122);
            this.Manager.Comment("reaching state \'S147\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp122, "displayName of LookupPrivilegeDisplayName, state S147");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp123, "return of LookupPrivilegeDisplayName, state S147");
            TestScenarioS21S161();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS21S8() {
            this.Manager.BeginTest("TestScenarioS21S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp124;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp125;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp125 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp124);
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp124, "policyHandle of OpenPolicy, state S52");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp125, "return of OpenPolicy, state S52");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID temp126;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp127;
            this.Manager.Comment("executing step \'call LookupPrivilegeValue(53,Invalid,\"SeDebugPrivilege\",out _)\'");
            temp127 = this.ILsadManagedAdapterInstance.LookupPrivilegeValue(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SeDebugPrivilege", out temp126);
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeValue/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(1)), temp126, "luid of LookupPrivilegeValue, state S84");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp127, "return of LookupPrivilegeValue, state S84");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp128;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp129;
            this.Manager.Comment("executing step \'call LookupPrivilegeName(53,Valid,\"{0,77}\",out _)\'");
            temp129 = this.ILsadManagedAdapterInstance.LookupPrivilegeName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.PrivilegeLUID)(0)), "{0,77}", out temp128);
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp128, "privilegeName of LookupPrivilegeName, state S116");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp129, "return of LookupPrivilegeName, state S116");
            this.Manager.Comment("reaching state \'S132\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString temp130;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp131;
            this.Manager.Comment("executing step \'call LookupPrivilegeDisplayName(53,Invalid,\"SeAssignPrimaryTokenP" +
                    "rivilege\",out _)\'");
            temp131 = this.ILsadManagedAdapterInstance.LookupPrivilegeDisplayName(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SeAssignPrimaryTokenPrivilege", out temp130);
            this.Manager.Comment("reaching state \'S148\'");
            this.Manager.Comment("checking step \'return LookupPrivilegeDisplayName/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), temp130, "displayName of LookupPrivilegeDisplayName, state S148");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp131, "return of LookupPrivilegeDisplayName, state S148");
            TestScenarioS21S161();
            this.Manager.EndTest();
        }
        #endregion
    }
}
