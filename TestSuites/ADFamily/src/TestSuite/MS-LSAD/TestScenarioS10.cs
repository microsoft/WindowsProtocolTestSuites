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
    public partial class TestScenarioS10 : PtfTestClassBase {
        
        public TestScenarioS10() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void DeleteObjectDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleOutput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        
        public delegate void CloseDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase CloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "Close", typeof(int), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle).MakeByRefType());
        
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
        public void LSAD_TestScenarioS10S0() {
            this.Manager.BeginTest("TestScenarioS10S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S192\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S288\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S288");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S288");
            this.Manager.Comment("reaching state \'S384\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"L$G\",4061069327,out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.CreateSecret(53, "L$G", 4061069327u, out temp2);
            this.Manager.Comment("reaching state \'S480\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp2, "secretHandle of CreateSecret, state S480");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp3, "return of CreateSecret, state S480");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S576() {
            this.Manager.Comment("reaching state \'S576\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp4 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S625\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp4, "return of SetSecret, state S625");
            this.Manager.Comment("reaching state \'S674\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp5;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp9 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp5, out temp6, out temp7, out temp8);
            this.Manager.Comment("reaching state \'S696\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp5, "encryptedCurrentValue of QuerySecret, state S696");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp6, "currentValueSetTime of QuerySecret, state S696");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp7, "encryptedOldValue of QuerySecret, state S696");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp8, "oldValueSetTime of QuerySecret, state S696");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp9, "return of QuerySecret, state S696");
            TestScenarioS10S718();
        }
        
        private void TestScenarioS10S718() {
            this.Manager.Comment("reaching state \'S718\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp10);
            this.Manager.Comment("reaching state \'S729\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp10, "handleOutput of DeleteObject, state S729");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp11, "return of DeleteObject, state S729");
            TestScenarioS10S738();
        }
        
        private void TestScenarioS10S738() {
            this.Manager.Comment("reaching state \'S738\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.Close(1, out temp12);
            this.Manager.AddReturn(CloseInfo, null, temp12, temp13);
            TestScenarioS10S746();
        }
        
        private void TestScenarioS10S746() {
            this.Manager.Comment("reaching state \'S746\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS10.CloseInfo, null, new CloseDelegate1(this.TestScenarioS10S0CloseChecker)));
            this.Manager.Comment("reaching state \'S753\'");
        }
        
        private void TestScenarioS10S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S746");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S746");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S10() {
            this.Manager.BeginTest("TestScenarioS10S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S197\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp14);
            this.Manager.Comment("reaching state \'S293\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp14, "policyHandle of OpenPolicy, state S293");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp15, "return of OpenPolicy, state S293");
            this.Manager.Comment("reaching state \'S389\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",4061069327,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 4061069327u, out temp16);
            this.Manager.Comment("reaching state \'S485\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp16, "secretHandle of CreateSecret, state S485");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of CreateSecret, state S485");
            this.Manager.Comment("reaching state \'S581\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp18;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp18 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S630\'");
            this.Manager.Comment("checking step \'return SetSecret/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp18, "return of SetSecret, state S630");
            TestScenarioS10S677();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S677() {
            this.Manager.Comment("reaching state \'S677\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp19;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp21;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp19, out temp20, out temp21, out temp22);
            this.Manager.Comment("reaching state \'S699\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp19, "encryptedCurrentValue of QuerySecret, state S699");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp20, "currentValueSetTime of QuerySecret, state S699");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp21, "encryptedOldValue of QuerySecret, state S699");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp22, "oldValueSetTime of QuerySecret, state S699");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp23, "return of QuerySecret, state S699");
            TestScenarioS10S720();
        }
        
        private void TestScenarioS10S720() {
            this.Manager.Comment("reaching state \'S720\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp24);
            this.Manager.AddReturn(DeleteObjectInfo, null, temp24, temp25);
            TestScenarioS10S730();
        }
        
        private void TestScenarioS10S730() {
            this.Manager.Comment("reaching state \'S730\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS10.DeleteObjectInfo, null, new DeleteObjectDelegate1(this.TestScenarioS10S10DeleteObjectChecker)));
            TestScenarioS10S738();
        }
        
        private void TestScenarioS10S10DeleteObjectChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleOutput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleOutput, "handleOutput of DeleteObject, state S730");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of DeleteObject, state S730");
        }
        #endregion
        
        #region Test Starting in S100
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S100() {
            this.Manager.BeginTest("TestScenarioS10S100");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S242\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp26);
            this.Manager.Comment("reaching state \'S338\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy, state S338");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy, state S338");
            this.Manager.Comment("reaching state \'S434\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"G$$\",4061069327,out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.CreateSecret(53, "G$$", 4061069327u, out temp28);
            this.Manager.Comment("reaching state \'S530\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp28, "secretHandle of CreateSecret, state S530");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp29, "return of CreateSecret, state S530");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S613() {
            this.Manager.Comment("reaching state \'S613\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp30;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp30 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S662\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp30, "return of SetSecret, state S662");
            this.Manager.Comment("reaching state \'S693\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp31;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp33;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp35 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp31, out temp32, out temp33, out temp34);
            this.Manager.Comment("reaching state \'S715\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp31, "encryptedCurrentValue of QuerySecret, state S715");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp32, "currentValueSetTime of QuerySecret, state S715");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp33, "encryptedOldValue of QuerySecret, state S715");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp34, "oldValueSetTime of QuerySecret, state S715");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp35, "return of QuerySecret, state S715");
            TestScenarioS10S728();
        }
        
        private void TestScenarioS10S728() {
            this.Manager.Comment("reaching state \'S728\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp37 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp36);
            this.Manager.Comment("reaching state \'S737\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp36, "handleOutput of DeleteObject, state S737");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp37, "return of DeleteObject, state S737");
            this.Manager.Comment("reaching state \'S745\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.Close(1, out temp38);
            this.Manager.AddReturn(CloseInfo, null, temp38, temp39);
            TestScenarioS10S746();
        }
        #endregion
        
        #region Test Starting in S102
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S102() {
            this.Manager.BeginTest("TestScenarioS10S102");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S243\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp40);
            this.Manager.Comment("reaching state \'S339\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp40, "policyHandle of OpenPolicy, state S339");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of OpenPolicy, state S339");
            this.Manager.Comment("reaching state \'S435\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"G$$\",1,out _)\'");
            temp43 = this.ILsadManagedAdapterInstance.CreateSecret(1, "G$$", 1u, out temp42);
            this.Manager.Comment("reaching state \'S531\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp42, "secretHandle of CreateSecret, state S531");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp43, "return of CreateSecret, state S531");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S104
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S104() {
            this.Manager.BeginTest("TestScenarioS10S104");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S244\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp44;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp45 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp44);
            this.Manager.Comment("reaching state \'S340\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp44, "policyHandle of OpenPolicy, state S340");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp45, "return of OpenPolicy, state S340");
            this.Manager.Comment("reaching state \'S436\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp46;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"G$$\",4061069327,out _)\'");
            temp47 = this.ILsadManagedAdapterInstance.CreateSecret(1, "G$$", 4061069327u, out temp46);
            this.Manager.Comment("reaching state \'S532\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp46, "secretHandle of CreateSecret, state S532");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp47, "return of CreateSecret, state S532");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S106
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S106() {
            this.Manager.BeginTest("TestScenarioS10S106");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S245\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp48);
            this.Manager.Comment("reaching state \'S341\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp48, "policyHandle of OpenPolicy, state S341");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp49, "return of OpenPolicy, state S341");
            this.Manager.Comment("reaching state \'S437\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp50;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"G$$\",46,out _)\'");
            temp51 = this.ILsadManagedAdapterInstance.CreateSecret(1, "G$$", 46u, out temp50);
            this.Manager.Comment("reaching state \'S533\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp50, "secretHandle of CreateSecret, state S533");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp51, "return of CreateSecret, state S533");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S108
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S108() {
            this.Manager.BeginTest("TestScenarioS10S108");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S246\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp53 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp52);
            this.Manager.Comment("reaching state \'S342\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp52, "policyHandle of OpenPolicy, state S342");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp53, "return of OpenPolicy, state S342");
            this.Manager.Comment("reaching state \'S438\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"G$$\",46,out _)\'");
            temp55 = this.ILsadManagedAdapterInstance.CreateSecret(53, "G$$", 46u, out temp54);
            this.Manager.Comment("reaching state \'S534\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp54, "secretHandle of CreateSecret, state S534");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp55, "return of CreateSecret, state S534");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S110
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S110() {
            this.Manager.BeginTest("TestScenarioS10S110");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S247\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp56;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp57;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp57 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp56);
            this.Manager.Comment("reaching state \'S343\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp56, "policyHandle of OpenPolicy, state S343");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp57, "return of OpenPolicy, state S343");
            this.Manager.Comment("reaching state \'S439\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp58;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsj" +
                    "fgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgd" +
                    "fjgkldsjgdsfgdsfg\",1,out _)\'");
            temp59 = this.ILsadManagedAdapterInstance.CreateSecret(53, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 1u, out temp58);
            this.Manager.Comment("reaching state \'S535\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp58, "secretHandle of CreateSecret, state S535");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp59, "return of CreateSecret, state S535");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S112
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S112() {
            this.Manager.BeginTest("TestScenarioS10S112");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S248\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp60;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp61 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp60);
            this.Manager.Comment("reaching state \'S344\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp60, "policyHandle of OpenPolicy, state S344");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp61, "return of OpenPolicy, state S344");
            this.Manager.Comment("reaching state \'S440\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsj" +
                    "fgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgd" +
                    "fjgkldsjgdsfgdsfg\",46,out _)\'");
            temp63 = this.ILsadManagedAdapterInstance.CreateSecret(53, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 46u, out temp62);
            this.Manager.Comment("reaching state \'S536\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp62, "secretHandle of CreateSecret, state S536");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp63, "return of CreateSecret, state S536");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S114
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S114() {
            this.Manager.BeginTest("TestScenarioS10S114");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S249\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp64;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp65;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp65 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp64);
            this.Manager.Comment("reaching state \'S345\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp64, "policyHandle of OpenPolicy, state S345");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp65, "return of OpenPolicy, state S345");
            this.Manager.Comment("reaching state \'S441\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp66;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"$MACHINE.ACC\",1,out _)\'");
            temp67 = this.ILsadManagedAdapterInstance.CreateSecret(53, "$MACHINE.ACC", 1u, out temp66);
            this.Manager.Comment("reaching state \'S537\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp66, "secretHandle of CreateSecret, state S537");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp67, "return of CreateSecret, state S537");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S116
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S116() {
            this.Manager.BeginTest("TestScenarioS10S116");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S250\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp68);
            this.Manager.Comment("reaching state \'S346\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp68, "policyHandle of OpenPolicy, state S346");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp69, "return of OpenPolicy, state S346");
            this.Manager.Comment("reaching state \'S442\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"$MACHINE.ACC\",46,out _)\'");
            temp71 = this.ILsadManagedAdapterInstance.CreateSecret(53, "$MACHINE.ACC", 46u, out temp70);
            this.Manager.Comment("reaching state \'S538\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp70, "secretHandle of CreateSecret, state S538");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp71, "return of CreateSecret, state S538");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S118
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S118() {
            this.Manager.BeginTest("TestScenarioS10S118");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S251\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp72;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp73 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp72);
            this.Manager.Comment("reaching state \'S347\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp72, "policyHandle of OpenPolicy, state S347");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp73, "return of OpenPolicy, state S347");
            this.Manager.Comment("reaching state \'S443\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp74;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp75;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"$MACHINE.ACC\",4061069327,out _)\'");
            temp75 = this.ILsadManagedAdapterInstance.CreateSecret(53, "$MACHINE.ACC", 4061069327u, out temp74);
            this.Manager.Comment("reaching state \'S539\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp74, "secretHandle of CreateSecret, state S539");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp75, "return of CreateSecret, state S539");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S12() {
            this.Manager.BeginTest("TestScenarioS10S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S198\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp76;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp77 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp76);
            this.Manager.Comment("reaching state \'S294\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp76, "policyHandle of OpenPolicy, state S294");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp77, "return of OpenPolicy, state S294");
            this.Manager.Comment("reaching state \'S390\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp78;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",4061069327,out _)\'");
            temp79 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 4061069327u, out temp78);
            this.Manager.Comment("reaching state \'S486\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp78, "secretHandle of CreateSecret, state S486");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp79, "return of CreateSecret, state S486");
            this.Manager.Comment("reaching state \'S582\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp80;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Valid,1)\'");
            temp80 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S631\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp80, "return of SetSecret, state S631");
            this.Manager.Comment("reaching state \'S678\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp81;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp82;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp83;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp84;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp85 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp81, out temp82, out temp83, out temp84);
            this.Manager.Comment("reaching state \'S700\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp81, "encryptedCurrentValue of QuerySecret, state S700");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp82, "currentValueSetTime of QuerySecret, state S700");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp83, "encryptedOldValue of QuerySecret, state S700");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp84, "oldValueSetTime of QuerySecret, state S700");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp85, "return of QuerySecret, state S700");
            TestScenarioS10S720();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S120
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S120() {
            this.Manager.BeginTest("TestScenarioS10S120");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S252\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp86;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp87 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp86);
            this.Manager.Comment("reaching state \'S348\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp86, "policyHandle of OpenPolicy, state S348");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp87, "return of OpenPolicy, state S348");
            this.Manager.Comment("reaching state \'S444\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp88;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"SecretName\",1,out _)\'");
            temp89 = this.ILsadManagedAdapterInstance.CreateSecret(53, "SecretName", 1u, out temp88);
            this.Manager.Comment("reaching state \'S540\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp88, "secretHandle of CreateSecret, state S540");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp89, "return of CreateSecret, state S540");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S122
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S122() {
            this.Manager.BeginTest("TestScenarioS10S122");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S253\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp90;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp91;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp91 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp90);
            this.Manager.Comment("reaching state \'S349\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp90, "policyHandle of OpenPolicy, state S349");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp91, "return of OpenPolicy, state S349");
            this.Manager.Comment("reaching state \'S445\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp92;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp93;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"SecretName\",46,out _)\'");
            temp93 = this.ILsadManagedAdapterInstance.CreateSecret(53, "SecretName", 46u, out temp92);
            this.Manager.Comment("reaching state \'S541\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp92, "secretHandle of CreateSecret, state S541");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp93, "return of CreateSecret, state S541");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S124
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S124() {
            this.Manager.BeginTest("TestScenarioS10S124");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S254\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp94;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp95;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp95 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp94);
            this.Manager.Comment("reaching state \'S350\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp94, "policyHandle of OpenPolicy, state S350");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp95, "return of OpenPolicy, state S350");
            this.Manager.Comment("reaching state \'S446\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp96;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"SecretName\",4061069327,out _)\'");
            temp97 = this.ILsadManagedAdapterInstance.CreateSecret(53, "SecretName", 4061069327u, out temp96);
            this.Manager.Comment("reaching state \'S542\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp96, "secretHandle of CreateSecret, state S542");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp97, "return of CreateSecret, state S542");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S126
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S126() {
            this.Manager.BeginTest("TestScenarioS10S126");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S255\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp98;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp99;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp99 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp98);
            this.Manager.Comment("reaching state \'S351\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp98, "policyHandle of OpenPolicy, state S351");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp99, "return of OpenPolicy, state S351");
            this.Manager.Comment("reaching state \'S447\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp100;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsj" +
                    "fgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgd" +
                    "fjgkldsjgdsfgdsfg\",4061069327,out _)\'");
            temp101 = this.ILsadManagedAdapterInstance.CreateSecret(53, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 4061069327u, out temp100);
            this.Manager.Comment("reaching state \'S543\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp100, "secretHandle of CreateSecret, state S543");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp101, "return of CreateSecret, state S543");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S128
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S128() {
            this.Manager.BeginTest("TestScenarioS10S128");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S256\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp102;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp103 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp102);
            this.Manager.Comment("reaching state \'S352\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp102, "policyHandle of OpenPolicy, state S352");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp103, "return of OpenPolicy, state S352");
            this.Manager.Comment("reaching state \'S448\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp104;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"L$G\",1,out _)\'");
            temp105 = this.ILsadManagedAdapterInstance.CreateSecret(53, "L$G", 1u, out temp104);
            this.Manager.Comment("reaching state \'S544\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp104, "secretHandle of CreateSecret, state S544");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp105, "return of CreateSecret, state S544");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S130
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S130() {
            this.Manager.BeginTest("TestScenarioS10S130");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S257\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp106;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp107;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp107 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp106);
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp106, "policyHandle of OpenPolicy, state S353");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp107, "return of OpenPolicy, state S353");
            this.Manager.Comment("reaching state \'S449\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp108;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp109;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"L$G\",46,out _)\'");
            temp109 = this.ILsadManagedAdapterInstance.CreateSecret(53, "L$G", 46u, out temp108);
            this.Manager.Comment("reaching state \'S545\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp108, "secretHandle of CreateSecret, state S545");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp109, "return of CreateSecret, state S545");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S132
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S132() {
            this.Manager.BeginTest("TestScenarioS10S132");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S258\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp110;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp111;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp111 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp110);
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp110, "policyHandle of OpenPolicy, state S354");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp111, "return of OpenPolicy, state S354");
            this.Manager.Comment("reaching state \'S450\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp112;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp113;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjf" +
                    "gklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdf" +
                    "jgkldsjgdsfgdsfg\",46,out _)\'");
            temp113 = this.ILsadManagedAdapterInstance.CreateSecret(1, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 46u, out temp112);
            this.Manager.Comment("reaching state \'S546\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp112, "secretHandle of CreateSecret, state S546");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp113, "return of CreateSecret, state S546");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S134
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S134() {
            this.Manager.BeginTest("TestScenarioS10S134");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S259\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp114;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp115 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp114);
            this.Manager.Comment("reaching state \'S355\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp114, "policyHandle of OpenPolicy, state S355");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp115, "return of OpenPolicy, state S355");
            this.Manager.Comment("reaching state \'S451\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp116;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp117 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp116);
            this.Manager.Comment("reaching state \'S547\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp116, "secretHandle of CreateSecret, state S547");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp117, "return of CreateSecret, state S547");
            this.Manager.Comment("reaching state \'S615\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp118;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp118 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S664\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp118, "return of SetSecret, state S664");
            TestScenarioS10S675();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S675() {
            this.Manager.Comment("reaching state \'S675\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp119;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp120;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp121;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp122;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp123;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp123 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp119, out temp120, out temp121, out temp122);
            this.Manager.Comment("reaching state \'S697\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp119, "encryptedCurrentValue of QuerySecret, state S697");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp120, "currentValueSetTime of QuerySecret, state S697");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp121, "encryptedOldValue of QuerySecret, state S697");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp122, "oldValueSetTime of QuerySecret, state S697");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp123, "return of QuerySecret, state S697");
            TestScenarioS10S719();
        }
        
        private void TestScenarioS10S719() {
            this.Manager.Comment("reaching state \'S719\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp124;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp125;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp125 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp124);
            this.Manager.AddReturn(DeleteObjectInfo, null, temp124, temp125);
            TestScenarioS10S730();
        }
        #endregion
        
        #region Test Starting in S136
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S136() {
            this.Manager.BeginTest("TestScenarioS10S136");
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S260\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp126;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp127;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp127 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp126);
            this.Manager.Comment("reaching state \'S356\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp126, "policyHandle of OpenPolicy, state S356");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp127, "return of OpenPolicy, state S356");
            this.Manager.Comment("reaching state \'S452\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp128;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp129;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",4061069327,out _)\'");
            temp129 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 4061069327u, out temp128);
            this.Manager.Comment("reaching state \'S548\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp128, "secretHandle of CreateSecret, state S548");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp129, "return of CreateSecret, state S548");
            this.Manager.Comment("reaching state \'S616\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp130;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp130 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S665\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp130, "return of SetSecret, state S665");
            TestScenarioS10S677();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S138
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S138() {
            this.Manager.BeginTest("TestScenarioS10S138");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S261\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp131;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp132;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp132 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp131);
            this.Manager.Comment("reaching state \'S357\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp131, "policyHandle of OpenPolicy, state S357");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp132, "return of OpenPolicy, state S357");
            this.Manager.Comment("reaching state \'S453\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp133;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp134;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp134 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp133);
            this.Manager.Comment("reaching state \'S549\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp133, "secretHandle of CreateSecret, state S549");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp134, "return of CreateSecret, state S549");
            this.Manager.Comment("reaching state \'S617\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp135;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp135 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S666\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp135, "return of SetSecret, state S666");
            TestScenarioS10S680();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S680() {
            this.Manager.Comment("reaching state \'S680\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp136;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp137;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp138;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp139;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp140;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp140 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp136, out temp137, out temp138, out temp139);
            this.Manager.Comment("reaching state \'S702\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp136, "encryptedCurrentValue of QuerySecret, state S702");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp137, "currentValueSetTime of QuerySecret, state S702");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp138, "encryptedOldValue of QuerySecret, state S702");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp139, "oldValueSetTime of QuerySecret, state S702");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp140, "return of QuerySecret, state S702");
            TestScenarioS10S721();
        }
        
        private void TestScenarioS10S721() {
            this.Manager.Comment("reaching state \'S721\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp141;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp142;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp142 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp141);
            this.Manager.Comment("reaching state \'S731\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp141, "handleOutput of DeleteObject, state S731");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp142, "return of DeleteObject, state S731");
            this.Manager.Comment("reaching state \'S739\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp143;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp144;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp144 = this.ILsadManagedAdapterInstance.Close(1, out temp143);
            this.Manager.Comment("reaching state \'S747\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp143, "handleAfterClose of Close, state S747");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp144, "return of Close, state S747");
            this.Manager.Comment("reaching state \'S754\'");
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S14() {
            this.Manager.BeginTest("TestScenarioS10S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S199\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp145;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp146;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp146 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp145);
            this.Manager.Comment("reaching state \'S295\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp145, "policyHandle of OpenPolicy, state S295");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp146, "return of OpenPolicy, state S295");
            this.Manager.Comment("reaching state \'S391\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp147;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp148;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",4061069327,out _)\'");
            temp148 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 4061069327u, out temp147);
            this.Manager.Comment("reaching state \'S487\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp147, "secretHandle of CreateSecret, state S487");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp148, "return of CreateSecret, state S487");
            this.Manager.Comment("reaching state \'S583\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp149;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Invalid,1)\'");
            temp149 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S632\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp149, "return of SetSecret, state S632");
            TestScenarioS10S677();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S140
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S140() {
            this.Manager.BeginTest("TestScenarioS10S140");
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S262\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp150;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp151;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp151 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp150);
            this.Manager.Comment("reaching state \'S358\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp150, "policyHandle of OpenPolicy, state S358");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp151, "return of OpenPolicy, state S358");
            this.Manager.Comment("reaching state \'S454\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp152;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp153;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",1,out _)\'");
            temp153 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 1u, out temp152);
            this.Manager.Comment("reaching state \'S550\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp152, "secretHandle of CreateSecret, state S550");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp153, "return of CreateSecret, state S550");
            this.Manager.Comment("reaching state \'S618\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp154;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp154 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S667\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp154, "return of SetSecret, state S667");
            TestScenarioS10S681();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S681() {
            this.Manager.Comment("reaching state \'S681\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp155;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp156;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp157;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp158;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp159;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp159 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp155, out temp156, out temp157, out temp158);
            this.Manager.Comment("reaching state \'S703\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp155, "encryptedCurrentValue of QuerySecret, state S703");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp156, "currentValueSetTime of QuerySecret, state S703");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp157, "encryptedOldValue of QuerySecret, state S703");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp158, "oldValueSetTime of QuerySecret, state S703");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp159, "return of QuerySecret, state S703");
            TestScenarioS10S722();
        }
        
        private void TestScenarioS10S722() {
            this.Manager.Comment("reaching state \'S722\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp160;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp161;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp161 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp160);
            this.Manager.Comment("reaching state \'S732\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp160, "handleOutput of DeleteObject, state S732");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp161, "return of DeleteObject, state S732");
            this.Manager.Comment("reaching state \'S740\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp162;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp163;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp163 = this.ILsadManagedAdapterInstance.Close(1, out temp162);
            this.Manager.Comment("reaching state \'S748\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp162, "handleAfterClose of Close, state S748");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp163, "return of Close, state S748");
            this.Manager.Comment("reaching state \'S755\'");
        }
        #endregion
        
        #region Test Starting in S142
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S142() {
            this.Manager.BeginTest("TestScenarioS10S142");
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S263\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp164;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp165;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp165 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp164);
            this.Manager.Comment("reaching state \'S359\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp164, "policyHandle of OpenPolicy, state S359");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp165, "return of OpenPolicy, state S359");
            this.Manager.Comment("reaching state \'S455\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp166;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp167;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",1,out _)\'");
            temp167 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 1u, out temp166);
            this.Manager.Comment("reaching state \'S551\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp166, "secretHandle of CreateSecret, state S551");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp167, "return of CreateSecret, state S551");
            this.Manager.Comment("reaching state \'S619\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp168;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp168 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S668\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp168, "return of SetSecret, state S668");
            TestScenarioS10S684();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S684() {
            this.Manager.Comment("reaching state \'S684\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp169;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp170;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp171;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp172;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp173;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp173 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp169, out temp170, out temp171, out temp172);
            this.Manager.Comment("reaching state \'S706\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp169, "encryptedCurrentValue of QuerySecret, state S706");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp170, "currentValueSetTime of QuerySecret, state S706");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp171, "encryptedOldValue of QuerySecret, state S706");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp172, "oldValueSetTime of QuerySecret, state S706");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp173, "return of QuerySecret, state S706");
            TestScenarioS10S723();
        }
        
        private void TestScenarioS10S723() {
            this.Manager.Comment("reaching state \'S723\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp174;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp175;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp175 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp174);
            this.Manager.Comment("reaching state \'S733\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp174, "handleOutput of DeleteObject, state S733");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp175, "return of DeleteObject, state S733");
            this.Manager.Comment("reaching state \'S741\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp176;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp177;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp177 = this.ILsadManagedAdapterInstance.Close(1, out temp176);
            this.Manager.Comment("reaching state \'S749\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp176, "handleAfterClose of Close, state S749");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp177, "return of Close, state S749");
            this.Manager.Comment("reaching state \'S756\'");
        }
        #endregion
        
        #region Test Starting in S144
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S144() {
            this.Manager.BeginTest("TestScenarioS10S144");
            this.Manager.Comment("reaching state \'S144\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S145\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S264\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp178;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp179;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp179 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp178);
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp178, "policyHandle of OpenPolicy, state S360");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp179, "return of OpenPolicy, state S360");
            this.Manager.Comment("reaching state \'S456\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp180;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp181;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"G$$\",46,out _)\'");
            temp181 = this.ILsadManagedAdapterInstance.CreateSecret(53, "G$$", 46u, out temp180);
            this.Manager.Comment("reaching state \'S552\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp180, "secretHandle of CreateSecret, state S552");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp181, "return of CreateSecret, state S552");
            this.Manager.Comment("reaching state \'S620\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp182;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp182 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S669\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp182, "return of SetSecret, state S669");
            this.Manager.Comment("reaching state \'S695\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp183;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp184;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp185;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp186;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp187;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp187 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp183, out temp184, out temp185, out temp186);
            this.Manager.Comment("reaching state \'S717\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp183, "encryptedCurrentValue of QuerySecret, state S717");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp184, "currentValueSetTime of QuerySecret, state S717");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp185, "encryptedOldValue of QuerySecret, state S717");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp186, "oldValueSetTime of QuerySecret, state S717");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp187, "return of QuerySecret, state S717");
            TestScenarioS10S718();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S146
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S146() {
            this.Manager.BeginTest("TestScenarioS10S146");
            this.Manager.Comment("reaching state \'S146\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S147\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S265\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp188;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp189;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp189 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp188);
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp188, "policyHandle of OpenPolicy, state S361");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp189, "return of OpenPolicy, state S361");
            this.Manager.Comment("reaching state \'S457\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp190;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp191;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"G$$\",46,out _)\'");
            temp191 = this.ILsadManagedAdapterInstance.CreateSecret(1, "G$$", 46u, out temp190);
            this.Manager.Comment("reaching state \'S553\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp190, "secretHandle of CreateSecret, state S553");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp191, "return of CreateSecret, state S553");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S148
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S148() {
            this.Manager.BeginTest("TestScenarioS10S148");
            this.Manager.Comment("reaching state \'S148\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S149\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S266\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp192;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp193;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp193 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp192);
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp192, "policyHandle of OpenPolicy, state S362");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp193, "return of OpenPolicy, state S362");
            this.Manager.Comment("reaching state \'S458\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp194;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp195;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",46,out _)\'");
            temp195 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 46u, out temp194);
            this.Manager.Comment("reaching state \'S554\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp194, "secretHandle of CreateSecret, state S554");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp195, "return of CreateSecret, state S554");
            this.Manager.Comment("reaching state \'S621\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp196;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp196 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S670\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp196, "return of SetSecret, state S670");
            TestScenarioS10S686();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S686() {
            this.Manager.Comment("reaching state \'S686\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp197;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp198;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp199;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp200;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp201;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp201 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp197, out temp198, out temp199, out temp200);
            this.Manager.Comment("reaching state \'S708\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp197, "encryptedCurrentValue of QuerySecret, state S708");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp198, "currentValueSetTime of QuerySecret, state S708");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp199, "encryptedOldValue of QuerySecret, state S708");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp200, "oldValueSetTime of QuerySecret, state S708");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp201, "return of QuerySecret, state S708");
            TestScenarioS10S724();
        }
        
        private void TestScenarioS10S724() {
            this.Manager.Comment("reaching state \'S724\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp202;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp203;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp203 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp202);
            this.Manager.Comment("reaching state \'S734\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp202, "handleOutput of DeleteObject, state S734");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp203, "return of DeleteObject, state S734");
            this.Manager.Comment("reaching state \'S742\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp204;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp205;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp205 = this.ILsadManagedAdapterInstance.Close(1, out temp204);
            this.Manager.Comment("reaching state \'S750\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp204, "handleAfterClose of Close, state S750");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp205, "return of Close, state S750");
            this.Manager.Comment("reaching state \'S757\'");
        }
        #endregion
        
        #region Test Starting in S150
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S150() {
            this.Manager.BeginTest("TestScenarioS10S150");
            this.Manager.Comment("reaching state \'S150\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S151\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S267\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp206;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp207;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp207 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp206);
            this.Manager.Comment("reaching state \'S363\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp206, "policyHandle of OpenPolicy, state S363");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp207, "return of OpenPolicy, state S363");
            this.Manager.Comment("reaching state \'S459\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp208;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp209;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",46,out _)\'");
            temp209 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 46u, out temp208);
            this.Manager.Comment("reaching state \'S555\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp208, "secretHandle of CreateSecret, state S555");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp209, "return of CreateSecret, state S555");
            this.Manager.Comment("reaching state \'S622\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp210;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp210 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S671\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp210, "return of SetSecret, state S671");
            TestScenarioS10S687();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S687() {
            this.Manager.Comment("reaching state \'S687\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp211;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp212;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp213;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp214;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp215;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp215 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp211, out temp212, out temp213, out temp214);
            this.Manager.Comment("reaching state \'S709\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp211, "encryptedCurrentValue of QuerySecret, state S709");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp212, "currentValueSetTime of QuerySecret, state S709");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp213, "encryptedOldValue of QuerySecret, state S709");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp214, "oldValueSetTime of QuerySecret, state S709");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp215, "return of QuerySecret, state S709");
            TestScenarioS10S725();
        }
        
        private void TestScenarioS10S725() {
            this.Manager.Comment("reaching state \'S725\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp216;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp217;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp217 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp216);
            this.Manager.Comment("reaching state \'S735\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp216, "handleOutput of DeleteObject, state S735");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp217, "return of DeleteObject, state S735");
            this.Manager.Comment("reaching state \'S743\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp218;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp219;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp219 = this.ILsadManagedAdapterInstance.Close(1, out temp218);
            this.Manager.Comment("reaching state \'S751\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp218, "handleAfterClose of Close, state S751");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp219, "return of Close, state S751");
            this.Manager.Comment("reaching state \'S758\'");
        }
        #endregion
        
        #region Test Starting in S152
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S152() {
            this.Manager.BeginTest("TestScenarioS10S152");
            this.Manager.Comment("reaching state \'S152\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S153\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S268\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp220;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp221;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp221 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp220);
            this.Manager.Comment("reaching state \'S364\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp220, "policyHandle of OpenPolicy, state S364");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp221, "return of OpenPolicy, state S364");
            this.Manager.Comment("reaching state \'S460\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp222;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp223;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"G$$\",1,out _)\'");
            temp223 = this.ILsadManagedAdapterInstance.CreateSecret(53, "G$$", 1u, out temp222);
            this.Manager.Comment("reaching state \'S556\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp222, "secretHandle of CreateSecret, state S556");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp223, "return of CreateSecret, state S556");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S154
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S154() {
            this.Manager.BeginTest("TestScenarioS10S154");
            this.Manager.Comment("reaching state \'S154\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S269\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp224;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp225;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp225 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp224);
            this.Manager.Comment("reaching state \'S365\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp224, "policyHandle of OpenPolicy, state S365");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp225, "return of OpenPolicy, state S365");
            this.Manager.Comment("reaching state \'S461\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp226;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp227;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjf" +
                    "gklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdf" +
                    "jgkldsjgdsfgdsfg\",1,out _)\'");
            temp227 = this.ILsadManagedAdapterInstance.CreateSecret(1, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 1u, out temp226);
            this.Manager.Comment("reaching state \'S557\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp226, "secretHandle of CreateSecret, state S557");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp227, "return of CreateSecret, state S557");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S156
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S156() {
            this.Manager.BeginTest("TestScenarioS10S156");
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S270\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp228;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp229;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp229 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp228);
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp228, "policyHandle of OpenPolicy, state S366");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp229, "return of OpenPolicy, state S366");
            this.Manager.Comment("reaching state \'S462\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp230;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp231;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp231 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp230);
            this.Manager.Comment("reaching state \'S558\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp230, "secretHandle of CreateSecret, state S558");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp231, "return of CreateSecret, state S558");
            this.Manager.Comment("reaching state \'S623\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp232;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp232 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S672\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp232, "return of SetSecret, state S672");
            TestScenarioS10S690();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S690() {
            this.Manager.Comment("reaching state \'S690\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp233;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp234;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp235;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp236;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp237;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp237 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp233, out temp234, out temp235, out temp236);
            this.Manager.Comment("reaching state \'S712\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp233, "encryptedCurrentValue of QuerySecret, state S712");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp234, "currentValueSetTime of QuerySecret, state S712");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp235, "encryptedOldValue of QuerySecret, state S712");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp236, "oldValueSetTime of QuerySecret, state S712");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp237, "return of QuerySecret, state S712");
            TestScenarioS10S726();
        }
        
        private void TestScenarioS10S726() {
            this.Manager.Comment("reaching state \'S726\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp238;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp239;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp239 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp238);
            this.Manager.Comment("reaching state \'S736\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp238, "handleOutput of DeleteObject, state S736");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp239, "return of DeleteObject, state S736");
            this.Manager.Comment("reaching state \'S744\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp240;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp241;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp241 = this.ILsadManagedAdapterInstance.Close(1, out temp240);
            this.Manager.Comment("reaching state \'S752\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp240, "handleAfterClose of Close, state S752");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp241, "return of Close, state S752");
            this.Manager.Comment("reaching state \'S759\'");
        }
        #endregion
        
        #region Test Starting in S158
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S158() {
            this.Manager.BeginTest("TestScenarioS10S158");
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S271\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp242;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp243;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp243 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp242);
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp242, "policyHandle of OpenPolicy, state S367");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp243, "return of OpenPolicy, state S367");
            this.Manager.Comment("reaching state \'S463\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp244;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp245;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",4061069327,out _)\'");
            temp245 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 4061069327u, out temp244);
            this.Manager.Comment("reaching state \'S559\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp244, "secretHandle of CreateSecret, state S559");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp245, "return of CreateSecret, state S559");
            this.Manager.Comment("reaching state \'S624\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp246;
            this.Manager.Comment("executing step \'call SetSecret(53,Valid,Valid,1)\'");
            temp246 = this.ILsadManagedAdapterInstance.SetSecret(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S673\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp246, "return of SetSecret, state S673");
            TestScenarioS10S692();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10S692() {
            this.Manager.Comment("reaching state \'S692\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp247;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp248;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp249;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp250;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp251;
            this.Manager.Comment("executing step \'call QuerySecret(53,out _,out _,out _,out _)\'");
            temp251 = this.ILsadManagedAdapterInstance.QuerySecret(53, out temp247, out temp248, out temp249, out temp250);
            this.Manager.Comment("reaching state \'S714\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp247, "encryptedCurrentValue of QuerySecret, state S714");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp248, "currentValueSetTime of QuerySecret, state S714");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp249, "encryptedOldValue of QuerySecret, state S714");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp250, "oldValueSetTime of QuerySecret, state S714");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp251, "return of QuerySecret, state S714");
            TestScenarioS10S727();
        }
        
        private void TestScenarioS10S727() {
            this.Manager.Comment("reaching state \'S727\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp252;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp253;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp253 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp252);
            this.Manager.AddReturn(DeleteObjectInfo, null, temp252, temp253);
            TestScenarioS10S730();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S16() {
            this.Manager.BeginTest("TestScenarioS10S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S200\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp254;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp255;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp255 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp254);
            this.Manager.Comment("reaching state \'S296\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp254, "policyHandle of OpenPolicy, state S296");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp255, "return of OpenPolicy, state S296");
            this.Manager.Comment("reaching state \'S392\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp256;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp257;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",4061069327,out _)\'");
            temp257 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 4061069327u, out temp256);
            this.Manager.Comment("reaching state \'S488\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp256, "secretHandle of CreateSecret, state S488");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp257, "return of CreateSecret, state S488");
            this.Manager.Comment("reaching state \'S584\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp258;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Invalid,1)\'");
            temp258 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S633\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp258, "return of SetSecret, state S633");
            TestScenarioS10S677();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S160
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S160() {
            this.Manager.BeginTest("TestScenarioS10S160");
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S161\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S272\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp259;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp260;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp260 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp259);
            this.Manager.Comment("reaching state \'S368\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp259, "policyHandle of OpenPolicy, state S368");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp260, "return of OpenPolicy, state S368");
            this.Manager.Comment("reaching state \'S464\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp261;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp262;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjf" +
                    "gklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdf" +
                    "jgkldsjgdsfgdsfg\",4061069327,out _)\'");
            temp262 = this.ILsadManagedAdapterInstance.CreateSecret(1, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 4061069327u, out temp261);
            this.Manager.Comment("reaching state \'S560\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp261, "secretHandle of CreateSecret, state S560");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp262, "return of CreateSecret, state S560");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S162
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S162() {
            this.Manager.BeginTest("TestScenarioS10S162");
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S273\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp263;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp264;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp264 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp263);
            this.Manager.Comment("reaching state \'S369\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp263, "policyHandle of OpenPolicy, state S369");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp264, "return of OpenPolicy, state S369");
            this.Manager.Comment("reaching state \'S465\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp265;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp266;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"G$$\",4061069327,out _)\'");
            temp266 = this.ILsadManagedAdapterInstance.CreateSecret(1, "G$$", 4061069327u, out temp265);
            this.Manager.Comment("reaching state \'S561\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp265, "secretHandle of CreateSecret, state S561");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp266, "return of CreateSecret, state S561");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S164
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S164() {
            this.Manager.BeginTest("TestScenarioS10S164");
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S274\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp267;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp268;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp268 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp267);
            this.Manager.Comment("reaching state \'S370\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp267, "policyHandle of OpenPolicy, state S370");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp268, "return of OpenPolicy, state S370");
            this.Manager.Comment("reaching state \'S466\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp269;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp270;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"G$$\",1,out _)\'");
            temp270 = this.ILsadManagedAdapterInstance.CreateSecret(1, "G$$", 1u, out temp269);
            this.Manager.Comment("reaching state \'S562\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp269, "secretHandle of CreateSecret, state S562");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp270, "return of CreateSecret, state S562");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S166
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S166() {
            this.Manager.BeginTest("TestScenarioS10S166");
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S275\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp271;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp272;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp272 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp271);
            this.Manager.Comment("reaching state \'S371\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp271, "policyHandle of OpenPolicy, state S371");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp272, "return of OpenPolicy, state S371");
            this.Manager.Comment("reaching state \'S467\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp273;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp274;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"G$$\",4061069327,out _)\'");
            temp274 = this.ILsadManagedAdapterInstance.CreateSecret(53, "G$$", 4061069327u, out temp273);
            this.Manager.Comment("reaching state \'S563\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp273, "secretHandle of CreateSecret, state S563");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp274, "return of CreateSecret, state S563");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S168
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S168() {
            this.Manager.BeginTest("TestScenarioS10S168");
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S276\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp275;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp276;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp276 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp275);
            this.Manager.Comment("reaching state \'S372\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp275, "policyHandle of OpenPolicy, state S372");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp276, "return of OpenPolicy, state S372");
            this.Manager.Comment("reaching state \'S468\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp277;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp278;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsj" +
                    "fgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgd" +
                    "fjgkldsjgdsfgdsfg\",1,out _)\'");
            temp278 = this.ILsadManagedAdapterInstance.CreateSecret(53, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 1u, out temp277);
            this.Manager.Comment("reaching state \'S564\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp277, "secretHandle of CreateSecret, state S564");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp278, "return of CreateSecret, state S564");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S170
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S170() {
            this.Manager.BeginTest("TestScenarioS10S170");
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S277\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp279;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp280;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp280 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp279);
            this.Manager.Comment("reaching state \'S373\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp279, "policyHandle of OpenPolicy, state S373");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp280, "return of OpenPolicy, state S373");
            this.Manager.Comment("reaching state \'S469\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp281;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp282;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsj" +
                    "fgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgd" +
                    "fjgkldsjgdsfgdsfg\",46,out _)\'");
            temp282 = this.ILsadManagedAdapterInstance.CreateSecret(53, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 46u, out temp281);
            this.Manager.Comment("reaching state \'S565\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp281, "secretHandle of CreateSecret, state S565");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp282, "return of CreateSecret, state S565");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S172
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S172() {
            this.Manager.BeginTest("TestScenarioS10S172");
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S278\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp283;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp284;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp284 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp283);
            this.Manager.Comment("reaching state \'S374\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp283, "policyHandle of OpenPolicy, state S374");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp284, "return of OpenPolicy, state S374");
            this.Manager.Comment("reaching state \'S470\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp285;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp286;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"$MACHINE.ACC\",1,out _)\'");
            temp286 = this.ILsadManagedAdapterInstance.CreateSecret(53, "$MACHINE.ACC", 1u, out temp285);
            this.Manager.Comment("reaching state \'S566\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp285, "secretHandle of CreateSecret, state S566");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp286, "return of CreateSecret, state S566");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S174
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S174() {
            this.Manager.BeginTest("TestScenarioS10S174");
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S279\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp287;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp288;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp288 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp287);
            this.Manager.Comment("reaching state \'S375\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp287, "policyHandle of OpenPolicy, state S375");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp288, "return of OpenPolicy, state S375");
            this.Manager.Comment("reaching state \'S471\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp289;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp290;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"$MACHINE.ACC\",46,out _)\'");
            temp290 = this.ILsadManagedAdapterInstance.CreateSecret(53, "$MACHINE.ACC", 46u, out temp289);
            this.Manager.Comment("reaching state \'S567\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp289, "secretHandle of CreateSecret, state S567");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp290, "return of CreateSecret, state S567");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S176
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S176() {
            this.Manager.BeginTest("TestScenarioS10S176");
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S280\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp291;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp292;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp292 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp291);
            this.Manager.Comment("reaching state \'S376\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp291, "policyHandle of OpenPolicy, state S376");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp292, "return of OpenPolicy, state S376");
            this.Manager.Comment("reaching state \'S472\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp293;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp294;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"$MACHINE.ACC\",4061069327,out _)\'");
            temp294 = this.ILsadManagedAdapterInstance.CreateSecret(53, "$MACHINE.ACC", 4061069327u, out temp293);
            this.Manager.Comment("reaching state \'S568\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp293, "secretHandle of CreateSecret, state S568");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp294, "return of CreateSecret, state S568");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S178
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S178() {
            this.Manager.BeginTest("TestScenarioS10S178");
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S179\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S281\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp295;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp296;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp296 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp295);
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp295, "policyHandle of OpenPolicy, state S377");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp296, "return of OpenPolicy, state S377");
            this.Manager.Comment("reaching state \'S473\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp297;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp298;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"SecretName\",1,out _)\'");
            temp298 = this.ILsadManagedAdapterInstance.CreateSecret(53, "SecretName", 1u, out temp297);
            this.Manager.Comment("reaching state \'S569\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp297, "secretHandle of CreateSecret, state S569");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp298, "return of CreateSecret, state S569");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S18() {
            this.Manager.BeginTest("TestScenarioS10S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S201\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp299;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp300;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp300 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp299);
            this.Manager.Comment("reaching state \'S297\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp299, "policyHandle of OpenPolicy, state S297");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp300, "return of OpenPolicy, state S297");
            this.Manager.Comment("reaching state \'S393\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp301;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp302;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp302 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp301);
            this.Manager.Comment("reaching state \'S489\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp301, "secretHandle of CreateSecret, state S489");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp302, "return of CreateSecret, state S489");
            this.Manager.Comment("reaching state \'S585\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp303;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp303 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S634\'");
            this.Manager.Comment("checking step \'return SetSecret/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp303, "return of SetSecret, state S634");
            this.Manager.Comment("reaching state \'S679\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp304;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp305;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp306;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp307;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp308;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp308 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp304, out temp305, out temp306, out temp307);
            this.Manager.Comment("reaching state \'S701\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp304, "encryptedCurrentValue of QuerySecret, state S701");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp305, "currentValueSetTime of QuerySecret, state S701");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp306, "encryptedOldValue of QuerySecret, state S701");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp307, "oldValueSetTime of QuerySecret, state S701");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp308, "return of QuerySecret, state S701");
            TestScenarioS10S721();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S180
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S180() {
            this.Manager.BeginTest("TestScenarioS10S180");
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S282\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp309;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp310;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp310 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp309);
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp309, "policyHandle of OpenPolicy, state S378");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp310, "return of OpenPolicy, state S378");
            this.Manager.Comment("reaching state \'S474\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp311;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp312;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"SecretName\",46,out _)\'");
            temp312 = this.ILsadManagedAdapterInstance.CreateSecret(53, "SecretName", 46u, out temp311);
            this.Manager.Comment("reaching state \'S570\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp311, "secretHandle of CreateSecret, state S570");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp312, "return of CreateSecret, state S570");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S182
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S182() {
            this.Manager.BeginTest("TestScenarioS10S182");
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S283\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp313;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp314;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp314 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp313);
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp313, "policyHandle of OpenPolicy, state S379");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp314, "return of OpenPolicy, state S379");
            this.Manager.Comment("reaching state \'S475\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp315;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp316;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"SecretName\",4061069327,out _)\'");
            temp316 = this.ILsadManagedAdapterInstance.CreateSecret(53, "SecretName", 4061069327u, out temp315);
            this.Manager.Comment("reaching state \'S571\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp315, "secretHandle of CreateSecret, state S571");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp316, "return of CreateSecret, state S571");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S184
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S184() {
            this.Manager.BeginTest("TestScenarioS10S184");
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S284\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp317;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp318;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp318 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp317);
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp317, "policyHandle of OpenPolicy, state S380");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp318, "return of OpenPolicy, state S380");
            this.Manager.Comment("reaching state \'S476\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp319;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp320;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsj" +
                    "fgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgd" +
                    "fjgkldsjgdsfgdsfg\",4061069327,out _)\'");
            temp320 = this.ILsadManagedAdapterInstance.CreateSecret(53, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 4061069327u, out temp319);
            this.Manager.Comment("reaching state \'S572\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp319, "secretHandle of CreateSecret, state S572");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp320, "return of CreateSecret, state S572");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S186
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S186() {
            this.Manager.BeginTest("TestScenarioS10S186");
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S285\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp321;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp322;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp322 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp321);
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp321, "policyHandle of OpenPolicy, state S381");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp322, "return of OpenPolicy, state S381");
            this.Manager.Comment("reaching state \'S477\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp323;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp324;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"L$G\",1,out _)\'");
            temp324 = this.ILsadManagedAdapterInstance.CreateSecret(53, "L$G", 1u, out temp323);
            this.Manager.Comment("reaching state \'S573\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp323, "secretHandle of CreateSecret, state S573");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp324, "return of CreateSecret, state S573");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S188
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S188() {
            this.Manager.BeginTest("TestScenarioS10S188");
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S286\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp325;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp326;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp326 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp325);
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp325, "policyHandle of OpenPolicy, state S382");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp326, "return of OpenPolicy, state S382");
            this.Manager.Comment("reaching state \'S478\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp327;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp328;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"L$G\",46,out _)\'");
            temp328 = this.ILsadManagedAdapterInstance.CreateSecret(53, "L$G", 46u, out temp327);
            this.Manager.Comment("reaching state \'S574\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp327, "secretHandle of CreateSecret, state S574");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp328, "return of CreateSecret, state S574");
            TestScenarioS10S576();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S190
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S190() {
            this.Manager.BeginTest("TestScenarioS10S190");
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S287\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp329;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp330;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp330 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp329);
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp329, "policyHandle of OpenPolicy, state S383");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp330, "return of OpenPolicy, state S383");
            this.Manager.Comment("reaching state \'S479\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp331;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp332;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"L$G\",4061069327,out _)\'");
            temp332 = this.ILsadManagedAdapterInstance.CreateSecret(53, "L$G", 4061069327u, out temp331);
            this.Manager.Comment("reaching state \'S575\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp331, "secretHandle of CreateSecret, state S575");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp332, "return of CreateSecret, state S575");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S2() {
            this.Manager.BeginTest("TestScenarioS10S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S193\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp333;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp334;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp334 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp333);
            this.Manager.Comment("reaching state \'S289\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp333, "policyHandle of OpenPolicy, state S289");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp334, "return of OpenPolicy, state S289");
            this.Manager.Comment("reaching state \'S385\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp335;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp336;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp336 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp335);
            this.Manager.Comment("reaching state \'S481\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp335, "secretHandle of CreateSecret, state S481");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp336, "return of CreateSecret, state S481");
            this.Manager.Comment("reaching state \'S577\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp337;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp337 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S626\'");
            this.Manager.Comment("checking step \'return SetSecret/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp337, "return of SetSecret, state S626");
            TestScenarioS10S675();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S20() {
            this.Manager.BeginTest("TestScenarioS10S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S202\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp338;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp339;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp339 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp338);
            this.Manager.Comment("reaching state \'S298\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp338, "policyHandle of OpenPolicy, state S298");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp339, "return of OpenPolicy, state S298");
            this.Manager.Comment("reaching state \'S394\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp340;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp341;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp341 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp340);
            this.Manager.Comment("reaching state \'S490\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp340, "secretHandle of CreateSecret, state S490");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp341, "return of CreateSecret, state S490");
            this.Manager.Comment("reaching state \'S586\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp342;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Valid,1)\'");
            temp342 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S635\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp342, "return of SetSecret, state S635");
            TestScenarioS10S680();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S22() {
            this.Manager.BeginTest("TestScenarioS10S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S203\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp343;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp344;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp344 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp343);
            this.Manager.Comment("reaching state \'S299\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp343, "policyHandle of OpenPolicy, state S299");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp344, "return of OpenPolicy, state S299");
            this.Manager.Comment("reaching state \'S395\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp345;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp346;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp346 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp345);
            this.Manager.Comment("reaching state \'S491\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp345, "secretHandle of CreateSecret, state S491");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp346, "return of CreateSecret, state S491");
            this.Manager.Comment("reaching state \'S587\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp347;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Invalid,1)\'");
            temp347 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S636\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp347, "return of SetSecret, state S636");
            TestScenarioS10S680();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S24() {
            this.Manager.BeginTest("TestScenarioS10S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S204\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp348;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp349;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp349 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp348);
            this.Manager.Comment("reaching state \'S300\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp348, "policyHandle of OpenPolicy, state S300");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp349, "return of OpenPolicy, state S300");
            this.Manager.Comment("reaching state \'S396\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp350;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp351;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp351 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp350);
            this.Manager.Comment("reaching state \'S492\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp350, "secretHandle of CreateSecret, state S492");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp351, "return of CreateSecret, state S492");
            this.Manager.Comment("reaching state \'S588\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp352;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Invalid,1)\'");
            temp352 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S637\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp352, "return of SetSecret, state S637");
            TestScenarioS10S680();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S26() {
            this.Manager.BeginTest("TestScenarioS10S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S205\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp353;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp354;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp354 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp353);
            this.Manager.Comment("reaching state \'S301\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp353, "policyHandle of OpenPolicy, state S301");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp354, "return of OpenPolicy, state S301");
            this.Manager.Comment("reaching state \'S397\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp355;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp356;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",1,out _)\'");
            temp356 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 1u, out temp355);
            this.Manager.Comment("reaching state \'S493\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp355, "secretHandle of CreateSecret, state S493");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp356, "return of CreateSecret, state S493");
            this.Manager.Comment("reaching state \'S589\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp357;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp357 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S638\'");
            this.Manager.Comment("checking step \'return SetSecret/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp357, "return of SetSecret, state S638");
            TestScenarioS10S681();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S28() {
            this.Manager.BeginTest("TestScenarioS10S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S206\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp358;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp359;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp359 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp358);
            this.Manager.Comment("reaching state \'S302\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp358, "policyHandle of OpenPolicy, state S302");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp359, "return of OpenPolicy, state S302");
            this.Manager.Comment("reaching state \'S398\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp360;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp361;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",1,out _)\'");
            temp361 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 1u, out temp360);
            this.Manager.Comment("reaching state \'S494\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp360, "secretHandle of CreateSecret, state S494");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp361, "return of CreateSecret, state S494");
            this.Manager.Comment("reaching state \'S590\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp362;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Valid,1)\'");
            temp362 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S639\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp362, "return of SetSecret, state S639");
            this.Manager.Comment("reaching state \'S682\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp363;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp364;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp365;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp366;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp367;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp367 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp363, out temp364, out temp365, out temp366);
            this.Manager.Comment("reaching state \'S704\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp363, "encryptedCurrentValue of QuerySecret, state S704");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp364, "currentValueSetTime of QuerySecret, state S704");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp365, "encryptedOldValue of QuerySecret, state S704");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp366, "oldValueSetTime of QuerySecret, state S704");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp367, "return of QuerySecret, state S704");
            TestScenarioS10S722();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S30() {
            this.Manager.BeginTest("TestScenarioS10S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S207\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp368;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp369;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp369 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp368);
            this.Manager.Comment("reaching state \'S303\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp368, "policyHandle of OpenPolicy, state S303");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp369, "return of OpenPolicy, state S303");
            this.Manager.Comment("reaching state \'S399\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp370;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp371;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",1,out _)\'");
            temp371 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 1u, out temp370);
            this.Manager.Comment("reaching state \'S495\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp370, "secretHandle of CreateSecret, state S495");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp371, "return of CreateSecret, state S495");
            this.Manager.Comment("reaching state \'S591\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp372;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Invalid,1)\'");
            temp372 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S640\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp372, "return of SetSecret, state S640");
            TestScenarioS10S681();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S32() {
            this.Manager.BeginTest("TestScenarioS10S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S208\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp373;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp374;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp374 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp373);
            this.Manager.Comment("reaching state \'S304\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp373, "policyHandle of OpenPolicy, state S304");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp374, "return of OpenPolicy, state S304");
            this.Manager.Comment("reaching state \'S400\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp375;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp376;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",1,out _)\'");
            temp376 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 1u, out temp375);
            this.Manager.Comment("reaching state \'S496\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp375, "secretHandle of CreateSecret, state S496");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp376, "return of CreateSecret, state S496");
            this.Manager.Comment("reaching state \'S592\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp377;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Invalid,1)\'");
            temp377 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S641\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp377, "return of SetSecret, state S641");
            TestScenarioS10S681();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S34() {
            this.Manager.BeginTest("TestScenarioS10S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S209\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp378;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp379;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp379 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp378);
            this.Manager.Comment("reaching state \'S305\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp378, "policyHandle of OpenPolicy, state S305");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp379, "return of OpenPolicy, state S305");
            this.Manager.Comment("reaching state \'S401\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp380;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp381;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",1,out _)\'");
            temp381 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 1u, out temp380);
            this.Manager.Comment("reaching state \'S497\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp380, "secretHandle of CreateSecret, state S497");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp381, "return of CreateSecret, state S497");
            this.Manager.Comment("reaching state \'S593\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp382;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp382 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S642\'");
            this.Manager.Comment("checking step \'return SetSecret/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp382, "return of SetSecret, state S642");
            this.Manager.Comment("reaching state \'S683\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp383;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp384;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp385;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp386;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp387;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp387 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp383, out temp384, out temp385, out temp386);
            this.Manager.Comment("reaching state \'S705\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp383, "encryptedCurrentValue of QuerySecret, state S705");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp384, "currentValueSetTime of QuerySecret, state S705");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp385, "encryptedOldValue of QuerySecret, state S705");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp386, "oldValueSetTime of QuerySecret, state S705");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp387, "return of QuerySecret, state S705");
            TestScenarioS10S723();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S36() {
            this.Manager.BeginTest("TestScenarioS10S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S210\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp388;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp389;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp389 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp388);
            this.Manager.Comment("reaching state \'S306\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp388, "policyHandle of OpenPolicy, state S306");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp389, "return of OpenPolicy, state S306");
            this.Manager.Comment("reaching state \'S402\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp390;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp391;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",1,out _)\'");
            temp391 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 1u, out temp390);
            this.Manager.Comment("reaching state \'S498\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp390, "secretHandle of CreateSecret, state S498");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp391, "return of CreateSecret, state S498");
            this.Manager.Comment("reaching state \'S594\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp392;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Valid,1)\'");
            temp392 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S643\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp392, "return of SetSecret, state S643");
            TestScenarioS10S684();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S38() {
            this.Manager.BeginTest("TestScenarioS10S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S211\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp393;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp394;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp394 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp393);
            this.Manager.Comment("reaching state \'S307\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp393, "policyHandle of OpenPolicy, state S307");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp394, "return of OpenPolicy, state S307");
            this.Manager.Comment("reaching state \'S403\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp395;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp396;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",1,out _)\'");
            temp396 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 1u, out temp395);
            this.Manager.Comment("reaching state \'S499\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp395, "secretHandle of CreateSecret, state S499");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp396, "return of CreateSecret, state S499");
            this.Manager.Comment("reaching state \'S595\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp397;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Invalid,1)\'");
            temp397 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S644\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp397, "return of SetSecret, state S644");
            TestScenarioS10S684();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S4() {
            this.Manager.BeginTest("TestScenarioS10S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S194\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp398;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp399;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp399 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp398);
            this.Manager.Comment("reaching state \'S290\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp398, "policyHandle of OpenPolicy, state S290");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp399, "return of OpenPolicy, state S290");
            this.Manager.Comment("reaching state \'S386\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp400;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp401;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp401 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp400);
            this.Manager.Comment("reaching state \'S482\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp400, "secretHandle of CreateSecret, state S482");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp401, "return of CreateSecret, state S482");
            this.Manager.Comment("reaching state \'S578\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp402;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Valid,1)\'");
            temp402 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S627\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp402, "return of SetSecret, state S627");
            this.Manager.Comment("reaching state \'S676\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp403;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp404;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp405;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp406;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp407;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp407 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp403, out temp404, out temp405, out temp406);
            this.Manager.Comment("reaching state \'S698\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Valid,out Valid,out Valid,out Valid]:Succe" +
                    "ss\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)), temp403, "encryptedCurrentValue of QuerySecret, state S698");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(0)), temp404, "currentValueSetTime of QuerySecret, state S698");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)), temp405, "encryptedOldValue of QuerySecret, state S698");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(0)), temp406, "oldValueSetTime of QuerySecret, state S698");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp407, "return of QuerySecret, state S698");
            TestScenarioS10S719();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S40() {
            this.Manager.BeginTest("TestScenarioS10S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S212\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp408;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp409;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp409 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp408);
            this.Manager.Comment("reaching state \'S308\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp408, "policyHandle of OpenPolicy, state S308");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp409, "return of OpenPolicy, state S308");
            this.Manager.Comment("reaching state \'S404\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp410;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp411;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",1,out _)\'");
            temp411 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 1u, out temp410);
            this.Manager.Comment("reaching state \'S500\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp410, "secretHandle of CreateSecret, state S500");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp411, "return of CreateSecret, state S500");
            this.Manager.Comment("reaching state \'S596\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp412;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Invalid,1)\'");
            temp412 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S645\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp412, "return of SetSecret, state S645");
            TestScenarioS10S684();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S42() {
            this.Manager.BeginTest("TestScenarioS10S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S213\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp413;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp414;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp414 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp413);
            this.Manager.Comment("reaching state \'S309\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp413, "policyHandle of OpenPolicy, state S309");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp414, "return of OpenPolicy, state S309");
            this.Manager.Comment("reaching state \'S405\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp415;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp416;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",46,out _)\'");
            temp416 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 46u, out temp415);
            this.Manager.Comment("reaching state \'S501\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp415, "secretHandle of CreateSecret, state S501");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp416, "return of CreateSecret, state S501");
            this.Manager.Comment("reaching state \'S597\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp417;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp417 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S646\'");
            this.Manager.Comment("checking step \'return SetSecret/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp417, "return of SetSecret, state S646");
            this.Manager.Comment("reaching state \'S685\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp418;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp419;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp420;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp421;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp422;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp422 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp418, out temp419, out temp420, out temp421);
            this.Manager.Comment("reaching state \'S707\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp418, "encryptedCurrentValue of QuerySecret, state S707");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp419, "currentValueSetTime of QuerySecret, state S707");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp420, "encryptedOldValue of QuerySecret, state S707");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp421, "oldValueSetTime of QuerySecret, state S707");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp422, "return of QuerySecret, state S707");
            TestScenarioS10S724();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S44() {
            this.Manager.BeginTest("TestScenarioS10S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S214\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp423;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp424;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp424 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp423);
            this.Manager.Comment("reaching state \'S310\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp423, "policyHandle of OpenPolicy, state S310");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp424, "return of OpenPolicy, state S310");
            this.Manager.Comment("reaching state \'S406\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp425;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp426;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",46,out _)\'");
            temp426 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 46u, out temp425);
            this.Manager.Comment("reaching state \'S502\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp425, "secretHandle of CreateSecret, state S502");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp426, "return of CreateSecret, state S502");
            this.Manager.Comment("reaching state \'S598\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp427;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Valid,1)\'");
            temp427 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S647\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp427, "return of SetSecret, state S647");
            TestScenarioS10S686();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S46() {
            this.Manager.BeginTest("TestScenarioS10S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S215\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp428;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp429;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp429 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp428);
            this.Manager.Comment("reaching state \'S311\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp428, "policyHandle of OpenPolicy, state S311");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp429, "return of OpenPolicy, state S311");
            this.Manager.Comment("reaching state \'S407\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp430;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp431;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",46,out _)\'");
            temp431 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 46u, out temp430);
            this.Manager.Comment("reaching state \'S503\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp430, "secretHandle of CreateSecret, state S503");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp431, "return of CreateSecret, state S503");
            this.Manager.Comment("reaching state \'S599\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp432;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Invalid,1)\'");
            temp432 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S648\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp432, "return of SetSecret, state S648");
            TestScenarioS10S686();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S48() {
            this.Manager.BeginTest("TestScenarioS10S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S216\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp433;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp434;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp434 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp433);
            this.Manager.Comment("reaching state \'S312\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp433, "policyHandle of OpenPolicy, state S312");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp434, "return of OpenPolicy, state S312");
            this.Manager.Comment("reaching state \'S408\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp435;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp436;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",46,out _)\'");
            temp436 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 46u, out temp435);
            this.Manager.Comment("reaching state \'S504\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp435, "secretHandle of CreateSecret, state S504");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp436, "return of CreateSecret, state S504");
            this.Manager.Comment("reaching state \'S600\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp437;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Invalid,1)\'");
            temp437 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S649\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp437, "return of SetSecret, state S649");
            TestScenarioS10S686();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S50() {
            this.Manager.BeginTest("TestScenarioS10S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S217\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp438;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp439;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp439 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp438);
            this.Manager.Comment("reaching state \'S313\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp438, "policyHandle of OpenPolicy, state S313");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp439, "return of OpenPolicy, state S313");
            this.Manager.Comment("reaching state \'S409\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp440;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp441;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",46,out _)\'");
            temp441 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 46u, out temp440);
            this.Manager.Comment("reaching state \'S505\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp440, "secretHandle of CreateSecret, state S505");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp441, "return of CreateSecret, state S505");
            this.Manager.Comment("reaching state \'S601\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp442;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp442 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S650\'");
            this.Manager.Comment("checking step \'return SetSecret/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp442, "return of SetSecret, state S650");
            TestScenarioS10S687();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S52() {
            this.Manager.BeginTest("TestScenarioS10S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S218\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp443;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp444;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp444 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp443);
            this.Manager.Comment("reaching state \'S314\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp443, "policyHandle of OpenPolicy, state S314");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp444, "return of OpenPolicy, state S314");
            this.Manager.Comment("reaching state \'S410\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp445;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp446;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",46,out _)\'");
            temp446 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 46u, out temp445);
            this.Manager.Comment("reaching state \'S506\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp445, "secretHandle of CreateSecret, state S506");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp446, "return of CreateSecret, state S506");
            this.Manager.Comment("reaching state \'S602\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp447;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Valid,1)\'");
            temp447 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S651\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp447, "return of SetSecret, state S651");
            this.Manager.Comment("reaching state \'S688\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp448;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp449;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp450;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp451;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp452;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp452 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp448, out temp449, out temp450, out temp451);
            this.Manager.Comment("reaching state \'S710\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp448, "encryptedCurrentValue of QuerySecret, state S710");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp449, "currentValueSetTime of QuerySecret, state S710");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp450, "encryptedOldValue of QuerySecret, state S710");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp451, "oldValueSetTime of QuerySecret, state S710");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp452, "return of QuerySecret, state S710");
            TestScenarioS10S725();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S54() {
            this.Manager.BeginTest("TestScenarioS10S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S219\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp453;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp454;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp454 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp453);
            this.Manager.Comment("reaching state \'S315\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp453, "policyHandle of OpenPolicy, state S315");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp454, "return of OpenPolicy, state S315");
            this.Manager.Comment("reaching state \'S411\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp455;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp456;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",46,out _)\'");
            temp456 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 46u, out temp455);
            this.Manager.Comment("reaching state \'S507\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp455, "secretHandle of CreateSecret, state S507");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp456, "return of CreateSecret, state S507");
            this.Manager.Comment("reaching state \'S603\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp457;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Invalid,1)\'");
            temp457 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S652\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp457, "return of SetSecret, state S652");
            TestScenarioS10S687();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S56() {
            this.Manager.BeginTest("TestScenarioS10S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S220\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp458;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp459;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp459 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp458);
            this.Manager.Comment("reaching state \'S316\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp458, "policyHandle of OpenPolicy, state S316");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp459, "return of OpenPolicy, state S316");
            this.Manager.Comment("reaching state \'S412\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp460;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp461;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",46,out _)\'");
            temp461 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 46u, out temp460);
            this.Manager.Comment("reaching state \'S508\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp460, "secretHandle of CreateSecret, state S508");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp461, "return of CreateSecret, state S508");
            this.Manager.Comment("reaching state \'S604\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp462;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Invalid,1)\'");
            temp462 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S653\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp462, "return of SetSecret, state S653");
            TestScenarioS10S687();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S58() {
            this.Manager.BeginTest("TestScenarioS10S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S221\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp463;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp464;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp464 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp463);
            this.Manager.Comment("reaching state \'S317\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp463, "policyHandle of OpenPolicy, state S317");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp464, "return of OpenPolicy, state S317");
            this.Manager.Comment("reaching state \'S413\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp465;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp466;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp466 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp465);
            this.Manager.Comment("reaching state \'S509\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp465, "secretHandle of CreateSecret, state S509");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp466, "return of CreateSecret, state S509");
            this.Manager.Comment("reaching state \'S605\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp467;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp467 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S654\'");
            this.Manager.Comment("checking step \'return SetSecret/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp467, "return of SetSecret, state S654");
            this.Manager.Comment("reaching state \'S689\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp468;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp469;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp470;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp471;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp472;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp472 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp468, out temp469, out temp470, out temp471);
            this.Manager.Comment("reaching state \'S711\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Valid,out Valid,out Valid,out Valid]:Succe" +
                    "ss\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)), temp468, "encryptedCurrentValue of QuerySecret, state S711");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(0)), temp469, "currentValueSetTime of QuerySecret, state S711");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)), temp470, "encryptedOldValue of QuerySecret, state S711");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(0)), temp471, "oldValueSetTime of QuerySecret, state S711");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp472, "return of QuerySecret, state S711");
            TestScenarioS10S726();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S6() {
            this.Manager.BeginTest("TestScenarioS10S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S195\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp473;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp474;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp474 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp473);
            this.Manager.Comment("reaching state \'S291\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp473, "policyHandle of OpenPolicy, state S291");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp474, "return of OpenPolicy, state S291");
            this.Manager.Comment("reaching state \'S387\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp475;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp476;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp476 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp475);
            this.Manager.Comment("reaching state \'S483\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp475, "secretHandle of CreateSecret, state S483");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp476, "return of CreateSecret, state S483");
            this.Manager.Comment("reaching state \'S579\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp477;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Invalid,1)\'");
            temp477 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S628\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp477, "return of SetSecret, state S628");
            TestScenarioS10S675();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S60() {
            this.Manager.BeginTest("TestScenarioS10S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S222\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp478;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp479;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp479 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp478);
            this.Manager.Comment("reaching state \'S318\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp478, "policyHandle of OpenPolicy, state S318");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp479, "return of OpenPolicy, state S318");
            this.Manager.Comment("reaching state \'S414\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp480;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp481;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp481 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp480);
            this.Manager.Comment("reaching state \'S510\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp480, "secretHandle of CreateSecret, state S510");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp481, "return of CreateSecret, state S510");
            this.Manager.Comment("reaching state \'S606\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp482;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Valid,1)\'");
            temp482 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S655\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp482, "return of SetSecret, state S655");
            TestScenarioS10S690();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S62() {
            this.Manager.BeginTest("TestScenarioS10S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S223\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp483;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp484;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp484 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp483);
            this.Manager.Comment("reaching state \'S319\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp483, "policyHandle of OpenPolicy, state S319");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp484, "return of OpenPolicy, state S319");
            this.Manager.Comment("reaching state \'S415\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp485;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp486;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp486 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp485);
            this.Manager.Comment("reaching state \'S511\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp485, "secretHandle of CreateSecret, state S511");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp486, "return of CreateSecret, state S511");
            this.Manager.Comment("reaching state \'S607\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp487;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Invalid,1)\'");
            temp487 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S656\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp487, "return of SetSecret, state S656");
            TestScenarioS10S690();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S64() {
            this.Manager.BeginTest("TestScenarioS10S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S224\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp488;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp489;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp489 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp488);
            this.Manager.Comment("reaching state \'S320\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp488, "policyHandle of OpenPolicy, state S320");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp489, "return of OpenPolicy, state S320");
            this.Manager.Comment("reaching state \'S416\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp490;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp491;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp491 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp490);
            this.Manager.Comment("reaching state \'S512\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp490, "secretHandle of CreateSecret, state S512");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp491, "return of CreateSecret, state S512");
            this.Manager.Comment("reaching state \'S608\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp492;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Invalid,1)\'");
            temp492 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S657\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp492, "return of SetSecret, state S657");
            TestScenarioS10S690();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S66() {
            this.Manager.BeginTest("TestScenarioS10S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S225\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp493;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp494;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp494 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp493);
            this.Manager.Comment("reaching state \'S321\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp493, "policyHandle of OpenPolicy, state S321");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp494, "return of OpenPolicy, state S321");
            this.Manager.Comment("reaching state \'S417\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp495;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp496;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",4061069327,out _)\'");
            temp496 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 4061069327u, out temp495);
            this.Manager.Comment("reaching state \'S513\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp495, "secretHandle of CreateSecret, state S513");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp496, "return of CreateSecret, state S513");
            this.Manager.Comment("reaching state \'S609\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp497;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp497 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S658\'");
            this.Manager.Comment("checking step \'return SetSecret/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp497, "return of SetSecret, state S658");
            this.Manager.Comment("reaching state \'S691\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp498;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp499;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp500;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp501;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp502;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp502 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp498, out temp499, out temp500, out temp501);
            this.Manager.Comment("reaching state \'S713\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp498, "encryptedCurrentValue of QuerySecret, state S713");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp499, "currentValueSetTime of QuerySecret, state S713");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp500, "encryptedOldValue of QuerySecret, state S713");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp501, "oldValueSetTime of QuerySecret, state S713");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp502, "return of QuerySecret, state S713");
            TestScenarioS10S727();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S68() {
            this.Manager.BeginTest("TestScenarioS10S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S226\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp503;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp504;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp504 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp503);
            this.Manager.Comment("reaching state \'S322\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp503, "policyHandle of OpenPolicy, state S322");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp504, "return of OpenPolicy, state S322");
            this.Manager.Comment("reaching state \'S418\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp505;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp506;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",4061069327,out _)\'");
            temp506 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 4061069327u, out temp505);
            this.Manager.Comment("reaching state \'S514\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp505, "secretHandle of CreateSecret, state S514");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp506, "return of CreateSecret, state S514");
            this.Manager.Comment("reaching state \'S610\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp507;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Valid,1)\'");
            temp507 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S659\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp507, "return of SetSecret, state S659");
            TestScenarioS10S692();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S70() {
            this.Manager.BeginTest("TestScenarioS10S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S227\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp508;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp509;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp509 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp508);
            this.Manager.Comment("reaching state \'S323\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp508, "policyHandle of OpenPolicy, state S323");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp509, "return of OpenPolicy, state S323");
            this.Manager.Comment("reaching state \'S419\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp510;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp511;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",4061069327,out _)\'");
            temp511 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 4061069327u, out temp510);
            this.Manager.Comment("reaching state \'S515\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp510, "secretHandle of CreateSecret, state S515");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp511, "return of CreateSecret, state S515");
            this.Manager.Comment("reaching state \'S611\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp512;
            this.Manager.Comment("executing step \'call SetSecret(2,Invalid,Invalid,1)\'");
            temp512 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S660\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp512, "return of SetSecret, state S660");
            TestScenarioS10S692();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S72
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S72() {
            this.Manager.BeginTest("TestScenarioS10S72");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S228\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp513;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp514;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp514 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp513);
            this.Manager.Comment("reaching state \'S324\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp513, "policyHandle of OpenPolicy, state S324");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp514, "return of OpenPolicy, state S324");
            this.Manager.Comment("reaching state \'S420\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp515;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp516;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",4061069327,out _)\'");
            temp516 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 4061069327u, out temp515);
            this.Manager.Comment("reaching state \'S516\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp515, "secretHandle of CreateSecret, state S516");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp516, "return of CreateSecret, state S516");
            this.Manager.Comment("reaching state \'S612\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp517;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Invalid,1)\'");
            temp517 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S661\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp517, "return of SetSecret, state S661");
            TestScenarioS10S692();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S74
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S74() {
            this.Manager.BeginTest("TestScenarioS10S74");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S229\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp518;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp519;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp519 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp518);
            this.Manager.Comment("reaching state \'S325\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp518, "policyHandle of OpenPolicy, state S325");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp519, "return of OpenPolicy, state S325");
            this.Manager.Comment("reaching state \'S421\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp520;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp521;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjf" +
                    "gklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdf" +
                    "jgkldsjgdsfgdsfg\",1,out _)\'");
            temp521 = this.ILsadManagedAdapterInstance.CreateSecret(1, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 1u, out temp520);
            this.Manager.Comment("reaching state \'S517\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp520, "secretHandle of CreateSecret, state S517");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp521, "return of CreateSecret, state S517");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S76
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S76() {
            this.Manager.BeginTest("TestScenarioS10S76");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S230\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp522;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp523;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp523 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp522);
            this.Manager.Comment("reaching state \'S326\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp522, "policyHandle of OpenPolicy, state S326");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp523, "return of OpenPolicy, state S326");
            this.Manager.Comment("reaching state \'S422\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp524;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp525;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjf" +
                    "gklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdf" +
                    "jgkldsjgdsfgdsfg\",46,out _)\'");
            temp525 = this.ILsadManagedAdapterInstance.CreateSecret(1, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 46u, out temp524);
            this.Manager.Comment("reaching state \'S518\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp524, "secretHandle of CreateSecret, state S518");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp525, "return of CreateSecret, state S518");
            this.Manager.Comment("reaching state \'S614\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp526;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp526 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S663\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp526, "return of SetSecret, state S663");
            this.Manager.Comment("reaching state \'S694\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp527;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp528;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp529;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp530;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp531;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp531 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp527, out temp528, out temp529, out temp530);
            this.Manager.Comment("reaching state \'S716\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Invalid,out Invalid,out Invalid,out Invali" +
                    "d]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp527, "encryptedCurrentValue of QuerySecret, state S716");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp528, "currentValueSetTime of QuerySecret, state S716");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp529, "encryptedOldValue of QuerySecret, state S716");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(1)), temp530, "oldValueSetTime of QuerySecret, state S716");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp531, "return of QuerySecret, state S716");
            TestScenarioS10S728();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S78
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S78() {
            this.Manager.BeginTest("TestScenarioS10S78");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S231\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp532;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp533;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp533 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp532);
            this.Manager.Comment("reaching state \'S327\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp532, "policyHandle of OpenPolicy, state S327");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp533, "return of OpenPolicy, state S327");
            this.Manager.Comment("reaching state \'S423\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp534;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp535;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",1,out _)\'");
            temp535 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 1u, out temp534);
            this.Manager.Comment("reaching state \'S519\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp534, "secretHandle of CreateSecret, state S519");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp535, "return of CreateSecret, state S519");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S8() {
            this.Manager.BeginTest("TestScenarioS10S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S196\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp536;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp537;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp537 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp536);
            this.Manager.Comment("reaching state \'S292\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp536, "policyHandle of OpenPolicy, state S292");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp537, "return of OpenPolicy, state S292");
            this.Manager.Comment("reaching state \'S388\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp538;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp539;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp539 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp538);
            this.Manager.Comment("reaching state \'S484\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp538, "secretHandle of CreateSecret, state S484");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp539, "return of CreateSecret, state S484");
            this.Manager.Comment("reaching state \'S580\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp540;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Invalid,1)\'");
            temp540 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(1)), 1);
            this.Manager.Comment("reaching state \'S629\'");
            this.Manager.Comment("checking step \'return SetSecret/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp540, "return of SetSecret, state S629");
            TestScenarioS10S675();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S80
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S80() {
            this.Manager.BeginTest("TestScenarioS10S80");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S232\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp541;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp542;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp542 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp541);
            this.Manager.Comment("reaching state \'S328\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp541, "policyHandle of OpenPolicy, state S328");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp542, "return of OpenPolicy, state S328");
            this.Manager.Comment("reaching state \'S424\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp543;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp544;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",46,out _)\'");
            temp544 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 46u, out temp543);
            this.Manager.Comment("reaching state \'S520\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp543, "secretHandle of CreateSecret, state S520");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp544, "return of CreateSecret, state S520");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S82
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S82() {
            this.Manager.BeginTest("TestScenarioS10S82");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S233\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp545;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp546;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp546 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp545);
            this.Manager.Comment("reaching state \'S329\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp545, "policyHandle of OpenPolicy, state S329");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp546, "return of OpenPolicy, state S329");
            this.Manager.Comment("reaching state \'S425\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp547;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp548;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"$MACHINE.ACC\",4061069327,out _)\'");
            temp548 = this.ILsadManagedAdapterInstance.CreateSecret(1, "$MACHINE.ACC", 4061069327u, out temp547);
            this.Manager.Comment("reaching state \'S521\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp547, "secretHandle of CreateSecret, state S521");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp548, "return of CreateSecret, state S521");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S84
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S84() {
            this.Manager.BeginTest("TestScenarioS10S84");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S234\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp549;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp550;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp550 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp549);
            this.Manager.Comment("reaching state \'S330\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp549, "policyHandle of OpenPolicy, state S330");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp550, "return of OpenPolicy, state S330");
            this.Manager.Comment("reaching state \'S426\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp551;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp552;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp552 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp551);
            this.Manager.Comment("reaching state \'S522\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp551, "secretHandle of CreateSecret, state S522");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp552, "return of CreateSecret, state S522");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S86
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S86() {
            this.Manager.BeginTest("TestScenarioS10S86");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S235\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp553;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp554;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp554 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp553);
            this.Manager.Comment("reaching state \'S331\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp553, "policyHandle of OpenPolicy, state S331");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp554, "return of OpenPolicy, state S331");
            this.Manager.Comment("reaching state \'S427\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp555;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp556;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp556 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp555);
            this.Manager.Comment("reaching state \'S523\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp555, "secretHandle of CreateSecret, state S523");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp556, "return of CreateSecret, state S523");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S88
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S88() {
            this.Manager.BeginTest("TestScenarioS10S88");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S236\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp557;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp558;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp558 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp557);
            this.Manager.Comment("reaching state \'S332\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp557, "policyHandle of OpenPolicy, state S332");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp558, "return of OpenPolicy, state S332");
            this.Manager.Comment("reaching state \'S428\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp559;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp560;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp560 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp559);
            this.Manager.Comment("reaching state \'S524\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp559, "secretHandle of CreateSecret, state S524");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp560, "return of CreateSecret, state S524");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S90
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S90() {
            this.Manager.BeginTest("TestScenarioS10S90");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S237\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp561;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp562;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp562 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp561);
            this.Manager.Comment("reaching state \'S333\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp561, "policyHandle of OpenPolicy, state S333");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp562, "return of OpenPolicy, state S333");
            this.Manager.Comment("reaching state \'S429\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp563;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp564;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjf" +
                    "gklsdgklsdjfgklsdjfmsdnviodsnfvkdshvsdnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdf" +
                    "jgkldsjgdsfgdsfg\",4061069327,out _)\'");
            temp564 = this.ILsadManagedAdapterInstance.CreateSecret(1, "fkgsjdfkgsdfgkljdsfkgjdsfklgjklds;jgf;kldsjfgklsdgklsdjfgklsdjfmsdnviodsnfvkdshvs" +
                    "dnvdsvhjdsfvjskldfgjsdfklgjdsfklgjskldfjgdfjgkldsjgdsfgdsfg", 4061069327u, out temp563);
            this.Manager.Comment("reaching state \'S525\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp563, "secretHandle of CreateSecret, state S525");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp564, "return of CreateSecret, state S525");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S92
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S92() {
            this.Manager.BeginTest("TestScenarioS10S92");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S238\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp565;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp566;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp566 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp565);
            this.Manager.Comment("reaching state \'S334\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp565, "policyHandle of OpenPolicy, state S334");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp566, "return of OpenPolicy, state S334");
            this.Manager.Comment("reaching state \'S430\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp567;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp568;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",1,out _)\'");
            temp568 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 1u, out temp567);
            this.Manager.Comment("reaching state \'S526\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp567, "secretHandle of CreateSecret, state S526");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp568, "return of CreateSecret, state S526");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S94
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S94() {
            this.Manager.BeginTest("TestScenarioS10S94");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S239\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp569;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp570;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp570 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp569);
            this.Manager.Comment("reaching state \'S335\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp569, "policyHandle of OpenPolicy, state S335");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp570, "return of OpenPolicy, state S335");
            this.Manager.Comment("reaching state \'S431\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp571;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp572;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",46,out _)\'");
            temp572 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 46u, out temp571);
            this.Manager.Comment("reaching state \'S527\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp571, "secretHandle of CreateSecret, state S527");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp572, "return of CreateSecret, state S527");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S96
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S96() {
            this.Manager.BeginTest("TestScenarioS10S96");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S240\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp573;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp574;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp574 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp573);
            this.Manager.Comment("reaching state \'S336\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp573, "policyHandle of OpenPolicy, state S336");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp574, "return of OpenPolicy, state S336");
            this.Manager.Comment("reaching state \'S432\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp575;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp576;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"L$G\",4061069327,out _)\'");
            temp576 = this.ILsadManagedAdapterInstance.CreateSecret(1, "L$G", 4061069327u, out temp575);
            this.Manager.Comment("reaching state \'S528\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp575, "secretHandle of CreateSecret, state S528");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp576, "return of CreateSecret, state S528");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S98
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10S98() {
            this.Manager.BeginTest("TestScenarioS10S98");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S241\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp577;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp578;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
            temp578 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp577);
            this.Manager.Comment("reaching state \'S337\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp577, "policyHandle of OpenPolicy, state S337");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp578, "return of OpenPolicy, state S337");
            this.Manager.Comment("reaching state \'S433\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp579;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp580;
            this.Manager.Comment("executing step \'call CreateSecret(53,\"G$$\",1,out _)\'");
            temp580 = this.ILsadManagedAdapterInstance.CreateSecret(53, "G$$", 1u, out temp579);
            this.Manager.Comment("reaching state \'S529\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp579, "secretHandle of CreateSecret, state S529");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp580, "return of CreateSecret, state S529");
            TestScenarioS10S613();
            this.Manager.EndTest();
        }
        #endregion
    }
}
