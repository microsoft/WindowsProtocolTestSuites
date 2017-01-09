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
    public partial class TestScenarioS10OfNotSupported : PtfTestClassBase {
        
        public TestScenarioS10OfNotSupported() {
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
        public void LSAD_TestScenarioS10OfNotSupportedS0() {
            this.Manager.BeginTest("TestScenarioS10OfNotSupportedS0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S9");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S9");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp2);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp2, "secretHandle of CreateSecret, state S15");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp3, "return of CreateSecret, state S15");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,3)\'");
            temp4 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 3);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return SetSecret/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp4, "return of SetSecret, state S21");
            TestScenarioS10OfNotSupportedS24();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS10OfNotSupportedS24() {
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp5;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call QuerySecret(2,out _,out _,out _,out _)\'");
            temp9 = this.ILsadManagedAdapterInstance.QuerySecret(2, out temp5, out temp6, out temp7, out temp8);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return QuerySecret/[out Valid,out Valid,out Valid,out Valid]:Succe" +
                    "ss\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)), temp5, "encryptedCurrentValue of QuerySecret, state S25");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(0)), temp6, "currentValueSetTime of QuerySecret, state S25");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)), temp7, "encryptedOldValue of QuerySecret, state S25");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValueSetTime)(0)), temp8, "oldValueSetTime of QuerySecret, state S25");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp9, "return of QuerySecret, state S25");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp10);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp10, "handleOutput of DeleteObject, state S27");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp11, "return of DeleteObject, state S27");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call Close(2,out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.Close(2, out temp12);
            this.Manager.Checkpoint("MS-LSAD_R848");
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Close/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp12, "handleAfterClose of Close, state S29");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp13, "return of Close, state S29");
            this.Manager.Comment("reaching state \'S30\'");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10OfNotSupportedS2() {
            this.Manager.BeginTest("TestScenarioS10OfNotSupportedS2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S7\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp14);
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp14, "policyHandle of OpenPolicy, state S10");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp15, "return of OpenPolicy, state S10");
            this.Manager.Comment("reaching state \'S13\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp16);
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp16, "secretHandle of CreateSecret, state S16");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of CreateSecret, state S16");
            this.Manager.Comment("reaching state \'S19\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp18;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,1)\'");
            temp18 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 1);
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return SetSecret/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp18, "return of SetSecret, state S22");
            TestScenarioS10OfNotSupportedS24();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS10OfNotSupportedS4() {
            this.Manager.BeginTest("TestScenarioS10OfNotSupportedS4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp19;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp20;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp20 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp19);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp19, "policyHandle of OpenPolicy, state S11");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp20, "return of OpenPolicy, state S11");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp21;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp22;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp22 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp21);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp21, "secretHandle of CreateSecret, state S17");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp22, "return of CreateSecret, state S17");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call SetSecret(2,Valid,Valid,2)\'");
            temp23 = this.ILsadManagedAdapterInstance.SetSecret(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CipherValue)(0)), 2);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return SetSecret/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp23, "return of SetSecret, state S23");
            TestScenarioS10OfNotSupportedS24();
            this.Manager.EndTest();
        }
        #endregion
    }
}
