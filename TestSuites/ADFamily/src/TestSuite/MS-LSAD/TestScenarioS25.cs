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
    public partial class TestScenarioS25 : PtfTestClassBase {
        
        public TestScenarioS25() {
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
        public void LSAD_TestScenarioS25S0() {
            this.Manager.BeginTest("TestScenarioS25S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S96");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S96");
            this.Manager.Comment("reaching state \'S128\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp2);
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp2, "accountHandle of CreateAccount, state S160");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp3, "return of CreateAccount, state S160");
            this.Manager.Comment("reaching state \'S192\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp4 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S224\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp4, "return of AddAccountRights, state S224");
            this.Manager.Comment("reaching state \'S256\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp5;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(11,Valid,\"SID\",out _)\'");
            temp6 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(11, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp5);
            this.Manager.Comment("reaching state \'S288\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp5, "userRights of EnumerateAccountRights, state S288");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp6, "return of EnumerateAccountRights, state S288");
            this.Manager.Comment("reaching state \'S320\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"invalidvalue\",Valid,out _" +
                    ")\'");
            temp8 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp7);
            this.Manager.Comment("reaching state \'S352\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidHandle\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp7, "enumerationBuffer of EnumerateAccountsWithUserRight, state S352");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp8, "return of EnumerateAccountsWithUserRight, state S352");
            TestScenarioS25S384();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS25S384() {
            this.Manager.Comment("reaching state \'S384\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp9;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp10 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp9);
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp9, "handleOutput of DeleteObject, state S388");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp10, "return of DeleteObject, state S388");
            this.Manager.Comment("reaching state \'S392\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp11;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp12;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp12 = this.ILsadManagedAdapterInstance.Close(1, out temp11);
            this.Manager.AddReturn(CloseInfo, null, temp11, temp12);
            TestScenarioS25S396();
        }
        
        private void TestScenarioS25S396() {
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS25.CloseInfo, null, new CloseDelegate1(this.TestScenarioS25S0CloseChecker)));
            this.Manager.Comment("reaching state \'S397\'");
        }
        
        private void TestScenarioS25S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S396");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S396");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S10() {
            this.Manager.BeginTest("TestScenarioS25S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S69\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp13;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp14;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,15,out _)\'");
            temp14 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 15u, out temp13);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp13, "policyHandle of OpenPolicy, state S101");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp14, "return of OpenPolicy, state S101");
            this.Manager.Comment("reaching state \'S133\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp15;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp16;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp16 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp15);
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp15, "accountHandle of CreateAccount, state S165");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp16, "return of CreateAccount, state S165");
            this.Manager.Comment("reaching state \'S197\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp17 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S229\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp17, "return of AddAccountRights, state S229");
            this.Manager.Comment("reaching state \'S261\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(11,Invalid,\"SID\",out _)\'");
            temp19 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(11, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp18);
            this.Manager.Comment("reaching state \'S293\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp18, "userRights of EnumerateAccountRights, state S293");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp19, "return of EnumerateAccountRights, state S293");
            this.Manager.Comment("reaching state \'S325\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"invalidvalue\",Invalid,out " +
                    "_)\'");
            temp21 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp20);
            this.Manager.Comment("reaching state \'S357\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp20, "enumerationBuffer of EnumerateAccountsWithUserRight, state S357");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp21, "return of EnumerateAccountsWithUserRight, state S357");
            TestScenarioS25S385();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS25S385() {
            this.Manager.Comment("reaching state \'S385\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp22);
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp22, "handleOutput of DeleteObject, state S389");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp23, "return of DeleteObject, state S389");
            this.Manager.Comment("reaching state \'S393\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.Close(1, out temp24);
            this.Manager.AddReturn(CloseInfo, null, temp24, temp25);
            TestScenarioS25S396();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S12() {
            this.Manager.BeginTest("TestScenarioS25S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,15,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 15u, out temp26);
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy, state S102");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy, state S102");
            this.Manager.Comment("reaching state \'S134\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp28);
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp28, "accountHandle of CreateAccount, state S166");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp29, "return of CreateAccount, state S166");
            this.Manager.Comment("reaching state \'S198\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp30;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp30 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S230\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp30, "return of AddAccountRights, state S230");
            this.Manager.Comment("reaching state \'S262\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp31;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp32;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp32 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp31);
            this.Manager.Comment("reaching state \'S294\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp31, "userRights of EnumerateAccountRights, state S294");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp32, "return of EnumerateAccountRights, state S294");
            this.Manager.Comment("reaching state \'S326\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp33;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp34;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"invalidvalue\",Invalid,out" +
                    " _)\'");
            temp34 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp33);
            this.Manager.Comment("reaching state \'S358\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp33, "enumerationBuffer of EnumerateAccountsWithUserRight, state S358");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp34, "return of EnumerateAccountsWithUserRight, state S358");
            TestScenarioS25S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S14() {
            this.Manager.BeginTest("TestScenarioS25S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S71\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp35;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp36;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,15,out _)\'");
            temp36 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 15u, out temp35);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp35, "policyHandle of OpenPolicy, state S103");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp36, "return of OpenPolicy, state S103");
            this.Manager.Comment("reaching state \'S135\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp37;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp38;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp38 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp37);
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp37, "accountHandle of CreateAccount, state S167");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp38, "return of CreateAccount, state S167");
            this.Manager.Comment("reaching state \'S199\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp39 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S231\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp39, "return of AddAccountRights, state S231");
            this.Manager.Comment("reaching state \'S263\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp40);
            this.Manager.Comment("reaching state \'S295\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp40, "userRights of EnumerateAccountRights, state S295");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp41, "return of EnumerateAccountRights, state S295");
            this.Manager.Comment("reaching state \'S327\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"SeAssignPrimaryTokenPrivi" +
                    "lege\",Valid,out _)\'");
            temp43 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp42);
            this.Manager.Comment("reaching state \'S359\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidHandle\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp42, "enumerationBuffer of EnumerateAccountsWithUserRight, state S359");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp43, "return of EnumerateAccountsWithUserRight, state S359");
            TestScenarioS25S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S16() {
            this.Manager.BeginTest("TestScenarioS25S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp44;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,0,out _)\'");
            temp45 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 0u, out temp44);
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp44, "policyHandle of OpenPolicy, state S104");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp45, "return of OpenPolicy, state S104");
            this.Manager.Comment("reaching state \'S136\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp46;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp47 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp46);
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp46, "accountHandle of CreateAccount, state S168");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp47, "return of CreateAccount, state S168");
            this.Manager.Comment("reaching state \'S200\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp48;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp48 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S232\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp48, "return of AddAccountRights, state S232");
            this.Manager.Comment("reaching state \'S264\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp49;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp50;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp50 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp49);
            this.Manager.Comment("reaching state \'S296\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp49, "userRights of EnumerateAccountRights, state S296");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp50, "return of EnumerateAccountRights, state S296");
            this.Manager.Comment("reaching state \'S328\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp51;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp52;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"SeAssignPrimaryTokenPrivil" +
                    "ege\",Invalid,out _)\'");
            temp52 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp51);
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp51, "enumerationBuffer of EnumerateAccountsWithUserRight, state S360");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp52, "return of EnumerateAccountsWithUserRight, state S360");
            TestScenarioS25S386();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS25S386() {
            this.Manager.Comment("reaching state \'S386\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp53;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp54;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp54 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp53);
            this.Manager.Comment("reaching state \'S390\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp53, "handleOutput of DeleteObject, state S390");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp54, "return of DeleteObject, state S390");
            this.Manager.Comment("reaching state \'S394\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp55;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp56;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp56 = this.ILsadManagedAdapterInstance.Close(1, out temp55);
            this.Manager.AddReturn(CloseInfo, null, temp55, temp56);
            TestScenarioS25S396();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S18() {
            this.Manager.BeginTest("TestScenarioS25S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S73\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp57;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp58;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,0,out _)\'");
            temp58 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 0u, out temp57);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp57, "policyHandle of OpenPolicy, state S105");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp58, "return of OpenPolicy, state S105");
            this.Manager.Comment("reaching state \'S137\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp59;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp60;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp60 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp59);
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp59, "accountHandle of CreateAccount, state S169");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp60, "return of CreateAccount, state S169");
            this.Manager.Comment("reaching state \'S201\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp61 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S233\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp61, "return of AddAccountRights, state S233");
            this.Manager.Comment("reaching state \'S265\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp63 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp62);
            this.Manager.Comment("reaching state \'S297\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp62, "userRights of EnumerateAccountRights, state S297");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp63, "return of EnumerateAccountRights, state S297");
            this.Manager.Comment("reaching state \'S329\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp64;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp65;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"invalidvalue\",Invalid,out " +
                    "_)\'");
            temp65 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp64);
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp64, "enumerationBuffer of EnumerateAccountsWithUserRight, state S361");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp65, "return of EnumerateAccountsWithUserRight, state S361");
            TestScenarioS25S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S2() {
            this.Manager.BeginTest("TestScenarioS25S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S65\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp66;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,15,out _)\'");
            temp67 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 15u, out temp66);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp66, "policyHandle of OpenPolicy, state S97");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp67, "return of OpenPolicy, state S97");
            this.Manager.Comment("reaching state \'S129\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp68);
            this.Manager.Comment("reaching state \'S161\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp68, "accountHandle of CreateAccount, state S161");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp69, "return of CreateAccount, state S161");
            this.Manager.Comment("reaching state \'S193\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp70;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp70 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S225\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp70, "return of AddAccountRights, state S225");
            this.Manager.Comment("reaching state \'S257\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp71;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp72;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp72 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp71);
            this.Manager.Comment("reaching state \'S289\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp71, "userRights of EnumerateAccountRights, state S289");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp72, "return of EnumerateAccountRights, state S289");
            this.Manager.Comment("reaching state \'S321\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp73;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp74;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"SeAssignPrimaryTokenPrivil" +
                    "ege\",Invalid,out _)\'");
            temp74 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp73);
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp73, "enumerationBuffer of EnumerateAccountsWithUserRight, state S353");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp74, "return of EnumerateAccountsWithUserRight, state S353");
            TestScenarioS25S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S20() {
            this.Manager.BeginTest("TestScenarioS25S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp75;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp76;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,0,out _)\'");
            temp76 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 0u, out temp75);
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp75, "policyHandle of OpenPolicy, state S106");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp76, "return of OpenPolicy, state S106");
            this.Manager.Comment("reaching state \'S138\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp77;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp78;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp78 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp77);
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp77, "accountHandle of CreateAccount, state S170");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp78, "return of CreateAccount, state S170");
            this.Manager.Comment("reaching state \'S202\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp79 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S234\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp79, "return of AddAccountRights, state S234");
            this.Manager.Comment("reaching state \'S266\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp80;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp81 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp80);
            this.Manager.Comment("reaching state \'S298\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp80, "userRights of EnumerateAccountRights, state S298");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp81, "return of EnumerateAccountRights, state S298");
            this.Manager.Comment("reaching state \'S330\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp82;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp83;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"invalidvalue\",Invalid,out" +
                    " _)\'");
            temp83 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp82);
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp82, "enumerationBuffer of EnumerateAccountsWithUserRight, state S362");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp83, "return of EnumerateAccountsWithUserRight, state S362");
            TestScenarioS25S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S22() {
            this.Manager.BeginTest("TestScenarioS25S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S75\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp84;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,0,out _)\'");
            temp85 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 0u, out temp84);
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp84, "policyHandle of OpenPolicy, state S107");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp85, "return of OpenPolicy, state S107");
            this.Manager.Comment("reaching state \'S139\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp86;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp87 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp86);
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp86, "accountHandle of CreateAccount, state S171");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp87, "return of CreateAccount, state S171");
            this.Manager.Comment("reaching state \'S203\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp88;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp88 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S235\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp88, "return of AddAccountRights, state S235");
            this.Manager.Comment("reaching state \'S267\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp89;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp90;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp90 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp89);
            this.Manager.Comment("reaching state \'S299\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp89, "userRights of EnumerateAccountRights, state S299");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp90, "return of EnumerateAccountRights, state S299");
            this.Manager.Comment("reaching state \'S331\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp91;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp92;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"SeAssignPrimaryTokenPrivi" +
                    "lege\",Valid,out _)\'");
            temp92 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp91);
            this.Manager.Comment("reaching state \'S363\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidHandle\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp91, "enumerationBuffer of EnumerateAccountsWithUserRight, state S363");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp92, "return of EnumerateAccountsWithUserRight, state S363");
            TestScenarioS25S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S24() {
            this.Manager.BeginTest("TestScenarioS25S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp93;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp94;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,0,out _)\'");
            temp94 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 0u, out temp93);
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp93, "policyHandle of OpenPolicy, state S108");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp94, "return of OpenPolicy, state S108");
            this.Manager.Comment("reaching state \'S140\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp95;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp96;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp96 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp95);
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp95, "accountHandle of CreateAccount, state S172");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp96, "return of CreateAccount, state S172");
            this.Manager.Comment("reaching state \'S204\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp97 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S236\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp97, "return of AddAccountRights, state S236");
            this.Manager.Comment("reaching state \'S268\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp98;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp99;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(11,Invalid,\"SID\",out _)\'");
            temp99 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(11, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp98);
            this.Manager.Comment("reaching state \'S300\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp98, "userRights of EnumerateAccountRights, state S300");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp99, "return of EnumerateAccountRights, state S300");
            this.Manager.Comment("reaching state \'S332\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp100;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"SeAssignPrimaryTokenPrivi" +
                    "lege\",Invalid,out _)\'");
            temp101 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp100);
            this.Manager.Comment("reaching state \'S364\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp100, "enumerationBuffer of EnumerateAccountsWithUserRight, state S364");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp101, "return of EnumerateAccountsWithUserRight, state S364");
            TestScenarioS25S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S26() {
            this.Manager.BeginTest("TestScenarioS25S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S77\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp102;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,0,out _)\'");
            temp103 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 0u, out temp102);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp102, "policyHandle of OpenPolicy, state S109");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp103, "return of OpenPolicy, state S109");
            this.Manager.Comment("reaching state \'S141\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp104;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp105 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp104);
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp104, "accountHandle of CreateAccount, state S173");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp105, "return of CreateAccount, state S173");
            this.Manager.Comment("reaching state \'S205\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp106;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp106 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S237\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp106, "return of AddAccountRights, state S237");
            this.Manager.Comment("reaching state \'S269\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp107;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp108;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp108 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp107);
            this.Manager.Comment("reaching state \'S301\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp107, "userRights of EnumerateAccountRights, state S301");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp108, "return of EnumerateAccountRights, state S301");
            this.Manager.Comment("reaching state \'S333\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp109;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp110;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"invalidvalue\",Valid,out _)" +
                    "\'");
            temp110 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp109);
            this.Manager.Comment("reaching state \'S365\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:NoSuchPrivileg" +
                    "e\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp109, "enumerationBuffer of EnumerateAccountsWithUserRight, state S365");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchPrivilege, temp110, "return of EnumerateAccountsWithUserRight, state S365");
            TestScenarioS25S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S28() {
            this.Manager.BeginTest("TestScenarioS25S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp111;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp112;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,0,out _)\'");
            temp112 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 0u, out temp111);
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp111, "policyHandle of OpenPolicy, state S110");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp112, "return of OpenPolicy, state S110");
            this.Manager.Comment("reaching state \'S142\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp113;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp114;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp114 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp113);
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp113, "accountHandle of CreateAccount, state S174");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp114, "return of CreateAccount, state S174");
            this.Manager.Comment("reaching state \'S206\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp115 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S238\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp115, "return of AddAccountRights, state S238");
            this.Manager.Comment("reaching state \'S270\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp116;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(11,Valid,\"SID\",out _)\'");
            temp117 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(11, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp116);
            this.Manager.Comment("reaching state \'S302\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp116, "userRights of EnumerateAccountRights, state S302");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp117, "return of EnumerateAccountRights, state S302");
            this.Manager.Comment("reaching state \'S334\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp118;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp119;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"SeAssignPrimaryTokenPrivil" +
                    "ege\",Valid,out _)\'");
            temp119 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp118);
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:NoMoreEntries\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp118, "enumerationBuffer of EnumerateAccountsWithUserRight, state S366");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoMoreEntries, temp119, "return of EnumerateAccountsWithUserRight, state S366");
            TestScenarioS25S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S30() {
            this.Manager.BeginTest("TestScenarioS25S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S79\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp120;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp121;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp121 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp120);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp120, "policyHandle of OpenPolicy, state S111");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp121, "return of OpenPolicy, state S111");
            this.Manager.Comment("reaching state \'S143\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp122;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp123;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp123 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp122);
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp122, "accountHandle of CreateAccount, state S175");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp123, "return of CreateAccount, state S175");
            this.Manager.Comment("reaching state \'S207\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp124;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp124 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S239\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp124, "return of AddAccountRights, state S239");
            this.Manager.Comment("reaching state \'S271\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp125;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp126;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp126 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp125);
            this.Manager.Comment("reaching state \'S303\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp125, "userRights of EnumerateAccountRights, state S303");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp126, "return of EnumerateAccountRights, state S303");
            this.Manager.Comment("reaching state \'S335\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp127;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp128;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"SeAssignPrimaryTokenPrivil" +
                    "ege\",Valid,out _)\'");
            temp128 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp127);
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp127, "enumerationBuffer of EnumerateAccountsWithUserRight, state S367");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp128, "return of EnumerateAccountsWithUserRight, state S367");
            TestScenarioS25S387();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS25S387() {
            this.Manager.Comment("reaching state \'S387\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp129;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp130;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp130 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp129);
            this.Manager.Comment("reaching state \'S391\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp129, "handleOutput of DeleteObject, state S391");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp130, "return of DeleteObject, state S391");
            this.Manager.Comment("reaching state \'S395\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp131;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp132;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp132 = this.ILsadManagedAdapterInstance.Close(1, out temp131);
            this.Manager.AddReturn(CloseInfo, null, temp131, temp132);
            TestScenarioS25S396();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S32() {
            this.Manager.BeginTest("TestScenarioS25S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp133;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp134;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp134 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp133);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp133, "policyHandle of OpenPolicy, state S112");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp134, "return of OpenPolicy, state S112");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp135;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp136;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp136 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp135);
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp135, "accountHandle of CreateAccount, state S176");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp136, "return of CreateAccount, state S176");
            this.Manager.Comment("reaching state \'S208\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp137;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp137 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S240\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp137, "return of AddAccountRights, state S240");
            this.Manager.Comment("reaching state \'S272\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp138;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp139;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp139 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp138);
            this.Manager.Comment("reaching state \'S304\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp138, "userRights of EnumerateAccountRights, state S304");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp139, "return of EnumerateAccountRights, state S304");
            this.Manager.Comment("reaching state \'S336\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp140;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp141;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"SeAssignPrimaryTokenPrivil" +
                    "ege\",Invalid,out _)\'");
            temp141 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp140);
            this.Manager.Comment("reaching state \'S368\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp140, "enumerationBuffer of EnumerateAccountsWithUserRight, state S368");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp141, "return of EnumerateAccountsWithUserRight, state S368");
            TestScenarioS25S387();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S34() {
            this.Manager.BeginTest("TestScenarioS25S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S81\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp142;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp143;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp143 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp142);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp142, "policyHandle of OpenPolicy, state S113");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp143, "return of OpenPolicy, state S113");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp144;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp145;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp145 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp144);
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp144, "accountHandle of CreateAccount, state S177");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp145, "return of CreateAccount, state S177");
            this.Manager.Comment("reaching state \'S209\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp146;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp146 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S241\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp146, "return of AddAccountRights, state S241");
            this.Manager.Comment("reaching state \'S273\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp147;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp148;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp148 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp147);
            this.Manager.Comment("reaching state \'S305\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp147, "userRights of EnumerateAccountRights, state S305");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp148, "return of EnumerateAccountRights, state S305");
            this.Manager.Comment("reaching state \'S337\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp149;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp150;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"invalidvalue\",Invalid,out " +
                    "_)\'");
            temp150 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp149);
            this.Manager.Comment("reaching state \'S369\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp149, "enumerationBuffer of EnumerateAccountsWithUserRight, state S369");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp150, "return of EnumerateAccountsWithUserRight, state S369");
            TestScenarioS25S387();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S36() {
            this.Manager.BeginTest("TestScenarioS25S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp151;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp152;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp152 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp151);
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp151, "policyHandle of OpenPolicy, state S114");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp152, "return of OpenPolicy, state S114");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp153;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp154;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp154 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp153);
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp153, "accountHandle of CreateAccount, state S178");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp154, "return of CreateAccount, state S178");
            this.Manager.Comment("reaching state \'S210\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp155;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp155 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S242\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp155, "return of AddAccountRights, state S242");
            this.Manager.Comment("reaching state \'S274\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp156;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp157;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp157 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp156);
            this.Manager.Comment("reaching state \'S306\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp156, "userRights of EnumerateAccountRights, state S306");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp157, "return of EnumerateAccountRights, state S306");
            this.Manager.Comment("reaching state \'S338\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp158;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp159;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"SeAssignPrimaryTokenPrivi" +
                    "lege\",Valid,out _)\'");
            temp159 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp158);
            this.Manager.Comment("reaching state \'S370\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidHandle\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp158, "enumerationBuffer of EnumerateAccountsWithUserRight, state S370");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp159, "return of EnumerateAccountsWithUserRight, state S370");
            TestScenarioS25S387();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S38() {
            this.Manager.BeginTest("TestScenarioS25S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S83\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp160;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp161;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp161 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp160);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp160, "policyHandle of OpenPolicy, state S115");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp161, "return of OpenPolicy, state S115");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp162;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp163;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp163 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp162);
            this.Manager.Comment("reaching state \'S179\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp162, "accountHandle of CreateAccount, state S179");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp163, "return of CreateAccount, state S179");
            this.Manager.Comment("reaching state \'S211\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp164;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp164 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S243\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp164, "return of AddAccountRights, state S243");
            this.Manager.Comment("reaching state \'S275\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp165;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp166;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp166 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp165);
            this.Manager.Comment("reaching state \'S307\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp165, "userRights of EnumerateAccountRights, state S307");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp166, "return of EnumerateAccountRights, state S307");
            this.Manager.Comment("reaching state \'S339\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp167;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp168;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"SeAssignPrimaryTokenPrivi" +
                    "lege\",Invalid,out _)\'");
            temp168 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp167);
            this.Manager.Comment("reaching state \'S371\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp167, "enumerationBuffer of EnumerateAccountsWithUserRight, state S371");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp168, "return of EnumerateAccountsWithUserRight, state S371");
            TestScenarioS25S387();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S4() {
            this.Manager.BeginTest("TestScenarioS25S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp169;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp170;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,15,out _)\'");
            temp170 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 15u, out temp169);
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp169, "policyHandle of OpenPolicy, state S98");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp170, "return of OpenPolicy, state S98");
            this.Manager.Comment("reaching state \'S130\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp171;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp172;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp172 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp171);
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp171, "accountHandle of CreateAccount, state S162");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp172, "return of CreateAccount, state S162");
            this.Manager.Comment("reaching state \'S194\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp173;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp173 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S226\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp173, "return of AddAccountRights, state S226");
            this.Manager.Comment("reaching state \'S258\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp174;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp175;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp175 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp174);
            this.Manager.Comment("reaching state \'S290\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp174, "userRights of EnumerateAccountRights, state S290");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp175, "return of EnumerateAccountRights, state S290");
            this.Manager.Comment("reaching state \'S322\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp176;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp177;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"SeAssignPrimaryTokenPrivi" +
                    "lege\",Invalid,out _)\'");
            temp177 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp176);
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp176, "enumerationBuffer of EnumerateAccountsWithUserRight, state S354");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp177, "return of EnumerateAccountsWithUserRight, state S354");
            TestScenarioS25S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S40() {
            this.Manager.BeginTest("TestScenarioS25S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp178;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp179;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp179 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp178);
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp178, "policyHandle of OpenPolicy, state S116");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp179, "return of OpenPolicy, state S116");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp180;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp181;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp181 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp180);
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp180, "accountHandle of CreateAccount, state S180");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp181, "return of CreateAccount, state S180");
            this.Manager.Comment("reaching state \'S212\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp182;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp182 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S244\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp182, "return of AddAccountRights, state S244");
            this.Manager.Comment("reaching state \'S276\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp183;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp184;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(11,Invalid,\"SID\",out _)\'");
            temp184 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(11, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp183);
            this.Manager.Comment("reaching state \'S308\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp183, "userRights of EnumerateAccountRights, state S308");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp184, "return of EnumerateAccountRights, state S308");
            this.Manager.Comment("reaching state \'S340\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp185;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp186;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"invalidvalue\",Invalid,out" +
                    " _)\'");
            temp186 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp185);
            this.Manager.Comment("reaching state \'S372\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp185, "enumerationBuffer of EnumerateAccountsWithUserRight, state S372");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp186, "return of EnumerateAccountsWithUserRight, state S372");
            TestScenarioS25S387();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S42() {
            this.Manager.BeginTest("TestScenarioS25S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S85\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp187;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp188;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp188 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp187);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp187, "policyHandle of OpenPolicy, state S117");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp188, "return of OpenPolicy, state S117");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp189;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp190;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp190 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp189);
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp189, "accountHandle of CreateAccount, state S181");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp190, "return of CreateAccount, state S181");
            this.Manager.Comment("reaching state \'S213\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp191;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp191 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S245\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp191, "return of AddAccountRights, state S245");
            this.Manager.Comment("reaching state \'S277\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp192;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp193;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(11,Valid,\"SID\",out _)\'");
            temp193 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(11, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp192);
            this.Manager.Comment("reaching state \'S309\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp192, "userRights of EnumerateAccountRights, state S309");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp193, "return of EnumerateAccountRights, state S309");
            this.Manager.Comment("reaching state \'S341\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp194;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp195;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"invalidvalue\",Valid,out _)" +
                    "\'");
            temp195 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp194);
            this.Manager.Comment("reaching state \'S373\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:NoSuchPrivileg" +
                    "e\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp194, "enumerationBuffer of EnumerateAccountsWithUserRight, state S373");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchPrivilege, temp195, "return of EnumerateAccountsWithUserRight, state S373");
            TestScenarioS25S387();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S44() {
            this.Manager.BeginTest("TestScenarioS25S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp196;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp197;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp197 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp196);
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp196, "policyHandle of OpenPolicy, state S118");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp197, "return of OpenPolicy, state S118");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp198;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp199;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp199 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp198);
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp198, "accountHandle of CreateAccount, state S182");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp199, "return of CreateAccount, state S182");
            this.Manager.Comment("reaching state \'S214\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp200;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp200 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S246\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp200, "return of AddAccountRights, state S246");
            this.Manager.Comment("reaching state \'S278\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp201;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp202;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp202 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp201);
            this.Manager.Comment("reaching state \'S310\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp201, "userRights of EnumerateAccountRights, state S310");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp202, "return of EnumerateAccountRights, state S310");
            this.Manager.Comment("reaching state \'S342\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp203;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp204;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"SeAssignPrimaryTokenPrivil" +
                    "ege\",Valid,out _)\'");
            temp204 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp203);
            this.Manager.Comment("reaching state \'S374\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), temp203, "enumerationBuffer of EnumerateAccountsWithUserRight, state S374");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp204, "return of EnumerateAccountsWithUserRight, state S374");
            TestScenarioS25S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S46() {
            this.Manager.BeginTest("TestScenarioS25S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S87\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp205;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp206;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp206 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp205);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp205, "policyHandle of OpenPolicy, state S119");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp206, "return of OpenPolicy, state S119");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp207;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp208;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp208 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp207);
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp207, "accountHandle of CreateAccount, state S183");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp208, "return of CreateAccount, state S183");
            this.Manager.Comment("reaching state \'S215\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp209;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp209 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S247\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp209, "return of AddAccountRights, state S247");
            this.Manager.Comment("reaching state \'S279\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp210;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp211;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp211 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp210);
            this.Manager.Comment("reaching state \'S311\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp210, "userRights of EnumerateAccountRights, state S311");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp211, "return of EnumerateAccountRights, state S311");
            this.Manager.Comment("reaching state \'S343\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp212;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp213;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"SeAssignPrimaryTokenPrivil" +
                    "ege\",Invalid,out _)\'");
            temp213 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp212);
            this.Manager.Comment("reaching state \'S375\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp212, "enumerationBuffer of EnumerateAccountsWithUserRight, state S375");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp213, "return of EnumerateAccountsWithUserRight, state S375");
            TestScenarioS25S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S48() {
            this.Manager.BeginTest("TestScenarioS25S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp214;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp215;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp215 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp214);
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp214, "policyHandle of OpenPolicy, state S120");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp215, "return of OpenPolicy, state S120");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp216;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp217;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp217 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp216);
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp216, "accountHandle of CreateAccount, state S184");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp217, "return of CreateAccount, state S184");
            this.Manager.Comment("reaching state \'S216\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp218;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp218 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S248\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp218, "return of AddAccountRights, state S248");
            this.Manager.Comment("reaching state \'S280\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp219;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp220;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp220 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp219);
            this.Manager.Comment("reaching state \'S312\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp219, "userRights of EnumerateAccountRights, state S312");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp220, "return of EnumerateAccountRights, state S312");
            this.Manager.Comment("reaching state \'S344\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp221;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp222;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"SeAssignPrimaryTokenPrivi" +
                    "lege\",Invalid,out _)\'");
            temp222 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp221);
            this.Manager.Comment("reaching state \'S376\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp221, "enumerationBuffer of EnumerateAccountsWithUserRight, state S376");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp222, "return of EnumerateAccountsWithUserRight, state S376");
            TestScenarioS25S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S50() {
            this.Manager.BeginTest("TestScenarioS25S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S89\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp223;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp224;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp224 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp223);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp223, "policyHandle of OpenPolicy, state S121");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp224, "return of OpenPolicy, state S121");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp225;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp226;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp226 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp225);
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp225, "accountHandle of CreateAccount, state S185");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp226, "return of CreateAccount, state S185");
            this.Manager.Comment("reaching state \'S217\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp227;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp227 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S249\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp227, "return of AddAccountRights, state S249");
            this.Manager.Comment("reaching state \'S281\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp228;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp229;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp229 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp228);
            this.Manager.Comment("reaching state \'S313\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp228, "userRights of EnumerateAccountRights, state S313");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp229, "return of EnumerateAccountRights, state S313");
            this.Manager.Comment("reaching state \'S345\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp230;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp231;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"invalidvalue\",Valid,out _)" +
                    "\'");
            temp231 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp230);
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:NoSuchPrivileg" +
                    "e\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp230, "enumerationBuffer of EnumerateAccountsWithUserRight, state S377");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchPrivilege, temp231, "return of EnumerateAccountsWithUserRight, state S377");
            TestScenarioS25S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S52() {
            this.Manager.BeginTest("TestScenarioS25S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp232;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp233;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp233 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp232);
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp232, "policyHandle of OpenPolicy, state S122");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp233, "return of OpenPolicy, state S122");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp234;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp235;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp235 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp234);
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp234, "accountHandle of CreateAccount, state S186");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp235, "return of CreateAccount, state S186");
            this.Manager.Comment("reaching state \'S218\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp236;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp236 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S250\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp236, "return of AddAccountRights, state S250");
            this.Manager.Comment("reaching state \'S282\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp237;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp238;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp238 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp237);
            this.Manager.Comment("reaching state \'S314\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp237, "userRights of EnumerateAccountRights, state S314");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp238, "return of EnumerateAccountRights, state S314");
            this.Manager.Comment("reaching state \'S346\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp239;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp240;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"invalidvalue\",Invalid,out " +
                    "_)\'");
            temp240 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp239);
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp239, "enumerationBuffer of EnumerateAccountsWithUserRight, state S378");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp240, "return of EnumerateAccountsWithUserRight, state S378");
            TestScenarioS25S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S54() {
            this.Manager.BeginTest("TestScenarioS25S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S91\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp241;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp242;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp242 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp241);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp241, "policyHandle of OpenPolicy, state S123");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp242, "return of OpenPolicy, state S123");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp243;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp244;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp244 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp243);
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp243, "accountHandle of CreateAccount, state S187");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp244, "return of CreateAccount, state S187");
            this.Manager.Comment("reaching state \'S219\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp245;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp245 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S251\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp245, "return of AddAccountRights, state S251");
            this.Manager.Comment("reaching state \'S283\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp246;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp247;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(11,Invalid,\"SID\",out _)\'");
            temp247 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(11, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp246);
            this.Manager.Comment("reaching state \'S315\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp246, "userRights of EnumerateAccountRights, state S315");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp247, "return of EnumerateAccountRights, state S315");
            this.Manager.Comment("reaching state \'S347\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp248;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp249;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"invalidvalue\",Invalid,out" +
                    " _)\'");
            temp249 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), out temp248);
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidParamet" +
                    "er\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp248, "enumerationBuffer of EnumerateAccountsWithUserRight, state S379");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp249, "return of EnumerateAccountsWithUserRight, state S379");
            TestScenarioS25S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S56() {
            this.Manager.BeginTest("TestScenarioS25S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp250;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp251;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp251 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp250);
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp250, "policyHandle of OpenPolicy, state S124");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp251, "return of OpenPolicy, state S124");
            this.Manager.Comment("reaching state \'S156\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp252;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp253;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp253 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp252);
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp252, "accountHandle of CreateAccount, state S188");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp253, "return of CreateAccount, state S188");
            this.Manager.Comment("reaching state \'S220\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp254;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp254 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S252\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp254, "return of AddAccountRights, state S252");
            this.Manager.Comment("reaching state \'S284\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp255;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp256;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp256 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp255);
            this.Manager.Comment("reaching state \'S316\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp255, "userRights of EnumerateAccountRights, state S316");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp256, "return of EnumerateAccountRights, state S316");
            this.Manager.Comment("reaching state \'S348\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp257;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp258;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"SeAssignPrimaryTokenPrivi" +
                    "lege\",Valid,out _)\'");
            temp258 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp257);
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidHandle\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp257, "enumerationBuffer of EnumerateAccountsWithUserRight, state S380");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp258, "return of EnumerateAccountsWithUserRight, state S380");
            TestScenarioS25S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S58() {
            this.Manager.BeginTest("TestScenarioS25S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S93\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp259;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp260;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,15,out _)\'");
            temp260 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 15u, out temp259);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp259, "policyHandle of OpenPolicy, state S125");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp260, "return of OpenPolicy, state S125");
            this.Manager.Comment("reaching state \'S157\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp261;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp262;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp262 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp261);
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp261, "accountHandle of CreateAccount, state S189");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp262, "return of CreateAccount, state S189");
            this.Manager.Comment("reaching state \'S221\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp263;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp263 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S253\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp263, "return of AddAccountRights, state S253");
            this.Manager.Comment("reaching state \'S285\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp264;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp265;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(11,Valid,\"SID\",out _)\'");
            temp265 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(11, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp264);
            this.Manager.Comment("reaching state \'S317\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp264, "userRights of EnumerateAccountRights, state S317");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp265, "return of EnumerateAccountRights, state S317");
            this.Manager.Comment("reaching state \'S349\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp266;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp267;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"invalidvalue\",Valid,out _" +
                    ")\'");
            temp267 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp266);
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidHandle\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp266, "enumerationBuffer of EnumerateAccountsWithUserRight, state S381");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp267, "return of EnumerateAccountsWithUserRight, state S381");
            TestScenarioS25S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S6() {
            this.Manager.BeginTest("TestScenarioS25S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S67\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp268;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp269;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,15,out _)\'");
            temp269 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 15u, out temp268);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp268, "policyHandle of OpenPolicy, state S99");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp269, "return of OpenPolicy, state S99");
            this.Manager.Comment("reaching state \'S131\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp270;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp271;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp271 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp270);
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp270, "accountHandle of CreateAccount, state S163");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp271, "return of CreateAccount, state S163");
            this.Manager.Comment("reaching state \'S195\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp272;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp272 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S227\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp272, "return of AddAccountRights, state S227");
            this.Manager.Comment("reaching state \'S259\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp273;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp274;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp274 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp273);
            this.Manager.Comment("reaching state \'S291\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp273, "userRights of EnumerateAccountRights, state S291");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp274, "return of EnumerateAccountRights, state S291");
            this.Manager.Comment("reaching state \'S323\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp275;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp276;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"invalidvalue\",Valid,out _)" +
                    "\'");
            temp276 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp275);
            this.Manager.Comment("reaching state \'S355\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:NoSuchPrivileg" +
                    "e\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp275, "enumerationBuffer of EnumerateAccountsWithUserRight, state S355");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchPrivilege, temp276, "return of EnumerateAccountsWithUserRight, state S355");
            TestScenarioS25S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S60() {
            this.Manager.BeginTest("TestScenarioS25S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp277;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp278;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,0,out _)\'");
            temp278 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 0u, out temp277);
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp277, "policyHandle of OpenPolicy, state S126");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp278, "return of OpenPolicy, state S126");
            this.Manager.Comment("reaching state \'S158\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp279;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp280;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp280 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp279);
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp279, "accountHandle of CreateAccount, state S190");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp280, "return of CreateAccount, state S190");
            this.Manager.Comment("reaching state \'S222\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp281;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp281 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S254\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp281, "return of AddAccountRights, state S254");
            this.Manager.Comment("reaching state \'S286\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp282;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp283;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp283 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp282);
            this.Manager.Comment("reaching state \'S318\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp282, "userRights of EnumerateAccountRights, state S318");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp283, "return of EnumerateAccountRights, state S318");
            this.Manager.Comment("reaching state \'S350\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp284;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp285;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"invalidvalue\",Valid,out _" +
                    ")\'");
            temp285 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp284);
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidHandle\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp284, "enumerationBuffer of EnumerateAccountsWithUserRight, state S382");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp285, "return of EnumerateAccountsWithUserRight, state S382");
            TestScenarioS25S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S62() {
            this.Manager.BeginTest("TestScenarioS25S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S95\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp286;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp287;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp287 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp286);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp286, "policyHandle of OpenPolicy, state S127");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp287, "return of OpenPolicy, state S127");
            this.Manager.Comment("reaching state \'S159\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp288;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp289;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp289 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp288);
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp288, "accountHandle of CreateAccount, state S191");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp289, "return of CreateAccount, state S191");
            this.Manager.Comment("reaching state \'S223\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp290;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp290 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S255\'");
            this.Manager.Comment("checking step \'return AddAccountRights/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp290, "return of AddAccountRights, state S255");
            this.Manager.Comment("reaching state \'S287\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp291;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp292;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Valid,\"SID\",out _)\'");
            temp292 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp291);
            this.Manager.Comment("reaching state \'S319\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(0)), temp291, "userRights of EnumerateAccountRights, state S319");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp292, "return of EnumerateAccountRights, state S319");
            this.Manager.Comment("reaching state \'S351\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp293;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp294;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(11,\"invalidvalue\",Valid,out _" +
                    ")\'");
            temp294 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(11, "invalidvalue", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp293);
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:InvalidHandle\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp293, "enumerationBuffer of EnumerateAccountsWithUserRight, state S383");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp294, "return of EnumerateAccountsWithUserRight, state S383");
            TestScenarioS25S387();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS25S8() {
            this.Manager.BeginTest("TestScenarioS25S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp295;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp296;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,15,out _)\'");
            temp296 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 15u, out temp295);
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp295, "policyHandle of OpenPolicy, state S100");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp296, "return of OpenPolicy, state S100");
            this.Manager.Comment("reaching state \'S132\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp297;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp298;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp298 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp297);
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp297, "accountHandle of CreateAccount, state S164");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp298, "return of CreateAccount, state S164");
            this.Manager.Comment("reaching state \'S196\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp299;
            this.Manager.Comment("executing step \'call AddAccountRights(1,\"SID\",Valid,{\"SeAssignPrimaryTokenPrivile" +
                    "ge\"})\'");
            temp299 = this.ILsadManagedAdapterInstance.AddAccountRights(1, "SID", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SeAssignPrimaryTokenPrivilege", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S228\'");
            this.Manager.Comment("checking step \'return AddAccountRights/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp299, "return of AddAccountRights, state S228");
            this.Manager.Comment("reaching state \'S260\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight temp300;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp301;
            this.Manager.Comment("executing step \'call EnumerateAccountRights(1,Invalid,\"SID\",out _)\'");
            temp301 = this.ILsadManagedAdapterInstance.EnumerateAccountRights(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp300);
            this.Manager.Comment("reaching state \'S292\'");
            this.Manager.Comment("checking step \'return EnumerateAccountRights/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.userRight)(1)), temp300, "userRights of EnumerateAccountRights, state S292");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp301, "return of EnumerateAccountRights, state S292");
            this.Manager.Comment("reaching state \'S324\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid temp302;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp303;
            this.Manager.Comment("executing step \'call EnumerateAccountsWithUserRight(1,\"SeAssignPrimaryTokenPrivil" +
                    "ege\",Valid,out _)\'");
            temp303 = this.ILsadManagedAdapterInstance.EnumerateAccountsWithUserRight(1, "SeAssignPrimaryTokenPrivilege", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), out temp302);
            this.Manager.Comment("reaching state \'S356\'");
            this.Manager.Comment("checking step \'return EnumerateAccountsWithUserRight/[out Invalid]:NoMoreEntries\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), temp302, "enumerationBuffer of EnumerateAccountsWithUserRight, state S356");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoMoreEntries, temp303, "return of EnumerateAccountsWithUserRight, state S356");
            TestScenarioS25S385();
            this.Manager.EndTest();
        }
        #endregion
    }
}
