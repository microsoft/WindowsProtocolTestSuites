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
    public partial class TestScenarioS6 : PtfTestClassBase {
        
        public TestScenarioS6() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void RemovePrivilegesFromAccountDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        
        public delegate void DeleteObjectDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleOutput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        
        public delegate void CloseDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase CloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "Close", typeof(int), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle).MakeByRefType());
        
        static System.Reflection.MethodBase RemovePrivilegesFromAccountInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "RemovePrivilegesFromAccount", typeof(int), typeof(bool), typeof(Microsoft.Modeling.Set<string>));
        
        static System.Reflection.MethodBase DeleteObjectInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "DeleteObject", typeof(int), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle).MakeByRefType());
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
        public void LSAD_TestScenarioS6S0() {
            this.Manager.BeginTest("TestScenarioS6S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp0);
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S60");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S60");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call CreateAccount(11,4061069327,Valid,\"SID\",out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.CreateAccount(11, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp2);
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp2, "accountHandle of CreateAccount, state S100");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp3, "return of CreateAccount, state S100");
            TestScenarioS6S120();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS6S120() {
            this.Manager.Comment("reaching state \'S120\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,21}\"})\'");
            temp4 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp4, "return of AddPrivilegesToAccount, state S130");
            this.Manager.Comment("reaching state \'S140\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp5;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp6 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp5);
            this.Manager.Comment("reaching state \'S150\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Invalid}]:InvalidHandle\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp5, "privileges of EnumeratePrivilegesAccount, state S150");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Invalid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp5.Rep, "privileges of EnumeratePrivilegesAccount, state S150, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp6, "return of EnumeratePrivilegesAccount, state S150");
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,21}\"})\'");
            temp7 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("checking step \'return RemovePrivilegesFromAccount/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp7, "return of RemovePrivilegesFromAccount, state S170");
            TestScenarioS6S179();
        }
        
        private void TestScenarioS6S179() {
            this.Manager.Comment("reaching state \'S179\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp9 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp8);
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp8, "handleOutput of DeleteObject, state S184");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp9, "return of DeleteObject, state S184");
            this.Manager.Comment("reaching state \'S188\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.Close(1, out temp10);
            this.Manager.AddReturn(CloseInfo, null, temp10, temp11);
            TestScenarioS6S191();
        }
        
        private void TestScenarioS6S191() {
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS6.CloseInfo, null, new CloseDelegate1(this.TestScenarioS6S0CloseChecker)));
            this.Manager.Comment("reaching state \'S193\'");
        }
        
        private void TestScenarioS6S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S191");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S191");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S10() {
            this.Manager.BeginTest("TestScenarioS6S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S45\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp12);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp12, "policyHandle of OpenPolicy, state S65");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp13, "return of OpenPolicy, state S65");
            this.Manager.Comment("reaching state \'S85\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp14);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp14, "accountHandle of CreateAccount, state S105");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp15, "return of CreateAccount, state S105");
            TestScenarioS6S125();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS6S125() {
            this.Manager.Comment("reaching state \'S125\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp16;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,21}\"})\'");
            temp16 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp16, "return of AddPrivilegesToAccount, state S135");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp17;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp18;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp18 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp17);
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Invalid}]:InvalidHandle\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp17, "privileges of EnumeratePrivilegesAccount, state S155");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Invalid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp17.Rep, "privileges of EnumeratePrivilegesAccount, state S155, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp18, "return of EnumeratePrivilegesAccount, state S155");
            this.Manager.Comment("reaching state \'S165\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,21}\"})\'");
            temp19 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return RemovePrivilegesFromAccount/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp19, "return of RemovePrivilegesFromAccount, state S175");
            TestScenarioS6S183();
        }
        
        private void TestScenarioS6S183() {
            this.Manager.Comment("reaching state \'S183\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp21 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp20);
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp20, "handleOutput of DeleteObject, state S187");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp21, "return of DeleteObject, state S187");
            TestScenarioS6S189();
        }
        
        private void TestScenarioS6S189() {
            this.Manager.Comment("reaching state \'S189\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.Close(1, out temp22);
            this.Manager.AddReturn(CloseInfo, null, temp22, temp23);
            TestScenarioS6S191();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S12() {
            this.Manager.BeginTest("TestScenarioS6S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp24);
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp24, "policyHandle of OpenPolicy, state S66");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp25, "return of OpenPolicy, state S66");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Invalid,\"SID\",out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp26);
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp26, "accountHandle of CreateAccount, state S106");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp27, "return of CreateAccount, state S106");
            this.Manager.Comment("reaching state \'S126\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp28;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,77}\"})\'");
            temp28 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp28, "return of AddPrivilegesToAccount, state S136");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp29;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp30;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp30 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp29);
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Invalid}]:InvalidHandle\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp29, "privileges of EnumeratePrivilegesAccount, state S156");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Invalid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp29.Rep, "privileges of EnumeratePrivilegesAccount, state S156, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp30, "return of EnumeratePrivilegesAccount, state S156");
            this.Manager.Comment("reaching state \'S166\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,77}\"})\'");
            temp31 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("checking step \'return RemovePrivilegesFromAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp31, "return of RemovePrivilegesFromAccount, state S176");
            TestScenarioS6S183();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S14() {
            this.Manager.BeginTest("TestScenarioS6S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S47\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp33 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp32);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp32, "policyHandle of OpenPolicy, state S67");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp33, "return of OpenPolicy, state S67");
            this.Manager.Comment("reaching state \'S87\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call CreateAccount(11,4061069327,Invalid,\"SID\",out _)\'");
            temp35 = this.ILsadManagedAdapterInstance.CreateAccount(11, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp34);
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp34, "accountHandle of CreateAccount, state S107");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp35, "return of CreateAccount, state S107");
            TestScenarioS6S125();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S16() {
            this.Manager.BeginTest("TestScenarioS6S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp37 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp36);
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp36, "policyHandle of OpenPolicy, state S68");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp37, "return of OpenPolicy, state S68");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call CreateAccount(11,0,Invalid,\"SID\",out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.CreateAccount(11, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp38);
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp38, "accountHandle of CreateAccount, state S108");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp39, "return of CreateAccount, state S108");
            TestScenarioS6S125();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S18() {
            this.Manager.BeginTest("TestScenarioS6S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S49\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp40);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp40, "policyHandle of OpenPolicy, state S69");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of OpenPolicy, state S69");
            this.Manager.Comment("reaching state \'S89\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp43 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp42);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp42, "accountHandle of CreateAccount, state S109");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp43, "return of CreateAccount, state S109");
            this.Manager.Comment("reaching state \'S127\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp44;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,77}\"})\'");
            temp44 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp44, "return of AddPrivilegesToAccount, state S137");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp45;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp46;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp46 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp45);
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Invalid}]:AccessDenied\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp45, "privileges of EnumeratePrivilegesAccount, state S157");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Invalid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp45.Rep, "privileges of EnumeratePrivilegesAccount, state S157, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp46, "return of EnumeratePrivilegesAccount, state S157");
            this.Manager.Comment("reaching state \'S167\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,77}\"})\'");
            temp47 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return RemovePrivilegesFromAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp47, "return of RemovePrivilegesFromAccount, state S177");
            TestScenarioS6S181();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS6S181() {
            this.Manager.Comment("reaching state \'S181\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp48);
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp48, "handleOutput of DeleteObject, state S186");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp49, "return of DeleteObject, state S186");
            this.Manager.Comment("reaching state \'S190\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp50;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp51 = this.ILsadManagedAdapterInstance.Close(1, out temp50);
            this.Manager.Comment("reaching state \'S192\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp50, "handleAfterClose of Close, state S192");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp51, "return of Close, state S192");
            this.Manager.Comment("reaching state \'S194\'");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S2() {
            this.Manager.BeginTest("TestScenarioS6S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S41\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp53 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp52);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp52, "policyHandle of OpenPolicy, state S61");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp53, "return of OpenPolicy, state S61");
            this.Manager.Comment("reaching state \'S81\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp55 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp54);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp54, "accountHandle of CreateAccount, state S101");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp55, "return of CreateAccount, state S101");
            this.Manager.Comment("reaching state \'S121\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp56;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,21}\"})\'");
            temp56 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp56, "return of AddPrivilegesToAccount, state S131");
            this.Manager.Comment("reaching state \'S141\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp57;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp58;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp58 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp57);
            this.Manager.Comment("reaching state \'S151\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Valid}]:Success\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp57, "privileges of EnumeratePrivilegesAccount, state S151");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Valid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp57.Rep, "privileges of EnumeratePrivilegesAccount, state S151, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp58, "return of EnumeratePrivilegesAccount, state S151");
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,21}\"})\'");
            temp59 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.AddReturn(RemovePrivilegesFromAccountInfo, null, temp59);
            TestScenarioS6S171();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS6S171() {
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS6.RemovePrivilegesFromAccountInfo, null, new RemovePrivilegesFromAccountDelegate1(this.TestScenarioS6S2RemovePrivilegesFromAccountChecker)));
            TestScenarioS6S180();
        }
        
        private void TestScenarioS6S2RemovePrivilegesFromAccountChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return RemovePrivilegesFromAccount/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of RemovePrivilegesFromAccount, state S171");
        }
        
        private void TestScenarioS6S180() {
            this.Manager.Comment("reaching state \'S180\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp60;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp61 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp60);
            this.Manager.AddReturn(DeleteObjectInfo, null, temp60, temp61);
            TestScenarioS6S185();
        }
        
        private void TestScenarioS6S185() {
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS6.DeleteObjectInfo, null, new DeleteObjectDelegate1(this.TestScenarioS6S2DeleteObjectChecker)));
            TestScenarioS6S189();
        }
        
        private void TestScenarioS6S2DeleteObjectChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleOutput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleOutput, "handleOutput of DeleteObject, state S185");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of DeleteObject, state S185");
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S20() {
            this.Manager.BeginTest("TestScenarioS6S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp63 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp62);
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp62, "policyHandle of OpenPolicy, state S70");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp63, "return of OpenPolicy, state S70");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp64;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp65;
            this.Manager.Comment("executing step \'call CreateAccount(11,0,Valid,\"SID\",out _)\'");
            temp65 = this.ILsadManagedAdapterInstance.CreateAccount(11, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp64);
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp64, "accountHandle of CreateAccount, state S110");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp65, "return of CreateAccount, state S110");
            TestScenarioS6S125();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S22() {
            this.Manager.BeginTest("TestScenarioS6S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S51\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp66;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp67 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp66);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp66, "policyHandle of OpenPolicy, state S71");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp67, "return of OpenPolicy, state S71");
            this.Manager.Comment("reaching state \'S91\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call CreateAccount(11,4061069327,Valid,\"SID\",out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.CreateAccount(11, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp68);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp68, "accountHandle of CreateAccount, state S111");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp69, "return of CreateAccount, state S111");
            TestScenarioS6S125();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S24() {
            this.Manager.BeginTest("TestScenarioS6S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp71 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp70);
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp70, "policyHandle of OpenPolicy, state S72");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp71, "return of OpenPolicy, state S72");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp72;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp73 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp72);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp72, "accountHandle of CreateAccount, state S112");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp73, "return of CreateAccount, state S112");
            TestScenarioS6S120();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S26() {
            this.Manager.BeginTest("TestScenarioS6S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S53\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp74;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp75;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp75 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp74);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp74, "policyHandle of OpenPolicy, state S73");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp75, "return of OpenPolicy, state S73");
            this.Manager.Comment("reaching state \'S93\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp76;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp77 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp76);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp76, "accountHandle of CreateAccount, state S113");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp77, "return of CreateAccount, state S113");
            this.Manager.Comment("reaching state \'S128\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp78;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,77}\"})\'");
            temp78 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp78, "return of AddPrivilegesToAccount, state S138");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp79;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp80;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp80 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp79);
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Invalid}]:InvalidHandle\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp79, "privileges of EnumeratePrivilegesAccount, state S158");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Invalid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp79.Rep, "privileges of EnumeratePrivilegesAccount, state S158, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp80, "return of EnumeratePrivilegesAccount, state S158");
            this.Manager.Comment("reaching state \'S168\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,77}\"})\'");
            temp81 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("checking step \'return RemovePrivilegesFromAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp81, "return of RemovePrivilegesFromAccount, state S178");
            TestScenarioS6S179();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S28() {
            this.Manager.BeginTest("TestScenarioS6S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp82;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp83;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp83 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp82);
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp82, "policyHandle of OpenPolicy, state S74");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp83, "return of OpenPolicy, state S74");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp84;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp85 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp84);
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp84, "accountHandle of CreateAccount, state S114");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp85, "return of CreateAccount, state S114");
            TestScenarioS6S120();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S30() {
            this.Manager.BeginTest("TestScenarioS6S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S55\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp86;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp87 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp86);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp86, "policyHandle of OpenPolicy, state S75");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp87, "return of OpenPolicy, state S75");
            this.Manager.Comment("reaching state \'S95\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp88;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Invalid,\"SID\",out _)\'");
            temp89 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp88);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp88, "accountHandle of CreateAccount, state S115");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp89, "return of CreateAccount, state S115");
            TestScenarioS6S120();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S32() {
            this.Manager.BeginTest("TestScenarioS6S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp90;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp91;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp91 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp90);
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp90, "policyHandle of OpenPolicy, state S76");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp91, "return of OpenPolicy, state S76");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp92;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp93;
            this.Manager.Comment("executing step \'call CreateAccount(11,4061069327,Invalid,\"SID\",out _)\'");
            temp93 = this.ILsadManagedAdapterInstance.CreateAccount(11, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp92);
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp92, "accountHandle of CreateAccount, state S116");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp93, "return of CreateAccount, state S116");
            TestScenarioS6S120();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S34() {
            this.Manager.BeginTest("TestScenarioS6S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S57\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp94;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp95;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp95 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp94);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp94, "policyHandle of OpenPolicy, state S77");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp95, "return of OpenPolicy, state S77");
            this.Manager.Comment("reaching state \'S97\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp96;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment("executing step \'call CreateAccount(11,0,Invalid,\"SID\",out _)\'");
            temp97 = this.ILsadManagedAdapterInstance.CreateAccount(11, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp96);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp96, "accountHandle of CreateAccount, state S117");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp97, "return of CreateAccount, state S117");
            TestScenarioS6S120();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S36() {
            this.Manager.BeginTest("TestScenarioS6S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp98;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp99;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp99 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp98);
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp98, "policyHandle of OpenPolicy, state S78");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp99, "return of OpenPolicy, state S78");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp100;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment("executing step \'call CreateAccount(11,0,Valid,\"SID\",out _)\'");
            temp101 = this.ILsadManagedAdapterInstance.CreateAccount(11, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp100);
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp100, "accountHandle of CreateAccount, state S118");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp101, "return of CreateAccount, state S118");
            TestScenarioS6S120();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S38() {
            this.Manager.BeginTest("TestScenarioS6S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S59\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp102;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp103 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp102);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp102, "policyHandle of OpenPolicy, state S79");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp103, "return of OpenPolicy, state S79");
            this.Manager.Comment("reaching state \'S99\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp104;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp105 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp104);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp104, "accountHandle of CreateAccount, state S119");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp105, "return of CreateAccount, state S119");
            this.Manager.Comment("reaching state \'S129\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp106;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,77}\"})\'");
            temp106 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp106, "return of AddPrivilegesToAccount, state S139");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp107;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp108;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp108 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp107);
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Valid}]:Success\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp107, "privileges of EnumeratePrivilegesAccount, state S159");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Valid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp107.Rep, "privileges of EnumeratePrivilegesAccount, state S159, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp108, "return of EnumeratePrivilegesAccount, state S159");
            this.Manager.Comment("reaching state \'S169\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp109;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,21}\"})\'");
            temp109 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.AddReturn(RemovePrivilegesFromAccountInfo, null, temp109);
            TestScenarioS6S171();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S4() {
            this.Manager.BeginTest("TestScenarioS6S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp110;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp111;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp111 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp110);
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp110, "policyHandle of OpenPolicy, state S62");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp111, "return of OpenPolicy, state S62");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp112;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp113;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp113 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp112);
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp112, "accountHandle of CreateAccount, state S102");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp113, "return of CreateAccount, state S102");
            this.Manager.Comment("reaching state \'S122\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp114;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,21}\"})\'");
            temp114 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp114, "return of AddPrivilegesToAccount, state S132");
            this.Manager.Comment("reaching state \'S142\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp115;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp116;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp116 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp115);
            this.Manager.Comment("reaching state \'S152\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Invalid}]:AccessDenied\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp115, "privileges of EnumeratePrivilegesAccount, state S152");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Invalid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp115.Rep, "privileges of EnumeratePrivilegesAccount, state S152, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp116, "return of EnumeratePrivilegesAccount, state S152");
            this.Manager.Comment("reaching state \'S162\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,21}\"})\'");
            temp117 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("checking step \'return RemovePrivilegesFromAccount/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp117, "return of RemovePrivilegesFromAccount, state S172");
            TestScenarioS6S181();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S6() {
            this.Manager.BeginTest("TestScenarioS6S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S43\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp118;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp119;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp119 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp118);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp118, "policyHandle of OpenPolicy, state S63");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp119, "return of OpenPolicy, state S63");
            this.Manager.Comment("reaching state \'S83\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp120;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp121;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp121 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp120);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp120, "accountHandle of CreateAccount, state S103");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp121, "return of CreateAccount, state S103");
            this.Manager.Comment("reaching state \'S123\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp122;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,77}\"})\'");
            temp122 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp122, "return of AddPrivilegesToAccount, state S133");
            this.Manager.Comment("reaching state \'S143\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp123;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp124;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp124 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp123);
            this.Manager.Comment("reaching state \'S153\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Valid}]:Success\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp123, "privileges of EnumeratePrivilegesAccount, state S153");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Valid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp123.Rep, "privileges of EnumeratePrivilegesAccount, state S153, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp124, "return of EnumeratePrivilegesAccount, state S153");
            this.Manager.Comment("reaching state \'S163\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp125;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,77}\"})\'");
            temp125 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return RemovePrivilegesFromAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp125, "return of RemovePrivilegesFromAccount, state S173");
            TestScenarioS6S180();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS6S8() {
            this.Manager.BeginTest("TestScenarioS6S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp126;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp127;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp127 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp126);
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp126, "policyHandle of OpenPolicy, state S64");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp127, "return of OpenPolicy, state S64");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp128;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp129;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp129 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp128);
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp128, "accountHandle of CreateAccount, state S104");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp129, "return of CreateAccount, state S104");
            this.Manager.Comment("reaching state \'S124\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp130;
            this.Manager.Comment("executing step \'call AddPrivilegesToAccount(2,{\"{0,21}\"})\'");
            temp130 = this.ILsadManagedAdapterInstance.AddPrivilegesToAccount(2, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,21}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("checking step \'return AddPrivilegesToAccount/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp130, "return of AddPrivilegesToAccount, state S134");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege> temp131;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp132;
            this.Manager.Comment("executing step \'call EnumeratePrivilegesAccount(2,out _)\'");
            temp132 = this.ILsadManagedAdapterInstance.EnumeratePrivilegesAccount(2, out temp131);
            this.Manager.Comment("reaching state \'S154\'");
            this.Manager.Comment("checking step \'return EnumeratePrivilegesAccount/[out {Valid}]:Success\'");
            TestManagerHelpers.AssertNotNull(this.Manager, temp131, "privileges of EnumeratePrivilegesAccount, state S154");
            TestManagerHelpers.AssertAreEqual<Microsoft.Xrt.Runtime.RuntimeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>>(this.Manager, Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountPrivilege.Valid, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                "Element"}, new object[] {
                                Microsoft.Xrt.Runtime.Singleton.Single})), temp131.Rep, "privileges of EnumeratePrivilegesAccount, state S154, field selection Rep");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp132, "return of EnumeratePrivilegesAccount, state S154");
            this.Manager.Comment("reaching state \'S164\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp133;
            this.Manager.Comment("executing step \'call RemovePrivilegesFromAccount(2,False,{\"{0,77}\"})\'");
            temp133 = this.ILsadManagedAdapterInstance.RemovePrivilegesFromAccount(2, false, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "{0,77}", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("checking step \'return RemovePrivilegesFromAccount/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp133, "return of RemovePrivilegesFromAccount, state S174");
            this.Manager.Comment("reaching state \'S182\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp134;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp135;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp135 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp134);
            this.Manager.AddReturn(DeleteObjectInfo, null, temp134, temp135);
            TestScenarioS6S185();
            this.Manager.EndTest();
        }
        #endregion
    }
}
