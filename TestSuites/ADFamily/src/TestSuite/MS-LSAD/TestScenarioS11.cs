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
    public partial class TestScenarioS11 : PtfTestClassBase {
        
        public TestScenarioS11() {
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
        public void LSAD_TestScenarioS11S0() {
            this.Manager.BeginTest("TestScenarioS11S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S108");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S108");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp2);
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp2, "secretHandle of CreateSecret, state S180");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp3, "return of CreateSecret, state S180");
            this.Manager.Comment("reaching state \'S216\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call OpenSecret(53,True,\"SecretName\",out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.OpenSecret(53, true, "SecretName", out temp4);
            this.Manager.Comment("reaching state \'S252\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp4, "secretHandle of OpenSecret, state S252");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp5, "return of OpenSecret, state S252");
            TestScenarioS11S288();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS11S288() {
            this.Manager.Comment("reaching state \'S288\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Valid)\'");
            temp6 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S312\'");
            this.Manager.Comment("checking step \'return StorePrivateData/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp6, "return of StorePrivateData, state S312");
            this.Manager.Comment("reaching state \'S336\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Valid,\"junkvalue\",out _)\'");
            temp8 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "junkvalue", out temp7);
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp7, "encryptedData of RetrievePrivateData, state S360");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp8, "return of RetrievePrivateData, state S360");
            TestScenarioS11S384();
        }
        
        private void TestScenarioS11S384() {
            this.Manager.Comment("reaching state \'S384\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp9;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp10 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp9);
            this.Manager.Comment("reaching state \'S387\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp9, "handleOutput of DeleteObject, state S387");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp10, "return of DeleteObject, state S387");
            this.Manager.Comment("reaching state \'S390\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp11;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp12;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp12 = this.ILsadManagedAdapterInstance.Close(1, out temp11);
            this.Manager.Comment("reaching state \'S393\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp11, "handleAfterClose of Close, state S393");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp12, "return of Close, state S393");
            this.Manager.Comment("reaching state \'S396\'");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S10() {
            this.Manager.BeginTest("TestScenarioS11S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S77\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp13;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp14;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp14 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp13);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp13, "policyHandle of OpenPolicy, state S113");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp14, "return of OpenPolicy, state S113");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp15;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp16;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp16 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp15);
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp15, "secretHandle of CreateSecret, state S185");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp16, "return of CreateSecret, state S185");
            this.Manager.Comment("reaching state \'S221\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp17;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp18;
            this.Manager.Comment("executing step \'call OpenSecret(53,True,\"G$$\",out _)\'");
            temp18 = this.ILsadManagedAdapterInstance.OpenSecret(53, true, "G$$", out temp17);
            this.Manager.Comment("reaching state \'S257\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp17, "secretHandle of OpenSecret, state S257");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp18, "return of OpenSecret, state S257");
            this.Manager.Comment("reaching state \'S293\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call StorePrivateData(53,\"SecretName\",Invalid)\'");
            temp19 = this.ILsadManagedAdapterInstance.StorePrivateData(53, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)));
            this.Manager.Comment("reaching state \'S317\'");
            this.Manager.Comment("checking step \'return StorePrivateData/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp19, "return of StorePrivateData, state S317");
            this.Manager.Comment("reaching state \'S341\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Invalid,\"junkvalue\",out _)\'");
            temp21 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "junkvalue", out temp20);
            this.Manager.Comment("reaching state \'S365\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp20, "encryptedData of RetrievePrivateData, state S365");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp21, "return of RetrievePrivateData, state S365");
            TestScenarioS11S385();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS11S385() {
            this.Manager.Comment("reaching state \'S385\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp22);
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp22, "handleOutput of DeleteObject, state S388");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp23, "return of DeleteObject, state S388");
            this.Manager.Comment("reaching state \'S391\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.Close(1, out temp24);
            this.Manager.Comment("reaching state \'S394\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp24, "handleAfterClose of Close, state S394");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp25, "return of Close, state S394");
            this.Manager.Comment("reaching state \'S397\'");
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S12() {
            this.Manager.BeginTest("TestScenarioS11S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp26);
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy, state S114");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy, state S114");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp28);
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp28, "secretHandle of CreateSecret, state S186");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of CreateSecret, state S186");
            this.Manager.Comment("reaching state \'S222\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call OpenSecret(1,False,\"G$$\",out _)\'");
            temp31 = this.ILsadManagedAdapterInstance.OpenSecret(1, false, "G$$", out temp30);
            this.Manager.Comment("reaching state \'S258\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp30, "secretHandle of OpenSecret, state S258");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp31, "return of OpenSecret, state S258");
            this.Manager.Comment("reaching state \'S294\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp32;
            this.Manager.Comment("executing step \'call StorePrivateData(53,\"SecretName\",Valid)\'");
            temp32 = this.ILsadManagedAdapterInstance.StorePrivateData(53, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S318\'");
            this.Manager.Comment("checking step \'return StorePrivateData/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp32, "return of StorePrivateData, state S318");
            this.Manager.Comment("reaching state \'S342\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp33;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp34;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Invalid,\"junkvalue\",out _)\'");
            temp34 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "junkvalue", out temp33);
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp33, "encryptedData of RetrievePrivateData, state S366");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp34, "return of RetrievePrivateData, state S366");
            TestScenarioS11S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S14() {
            this.Manager.BeginTest("TestScenarioS11S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S79\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp35;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp36;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp36 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp35);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp35, "policyHandle of OpenPolicy, state S115");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp36, "return of OpenPolicy, state S115");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp37;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp38;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp38 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp37);
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp37, "secretHandle of CreateSecret, state S187");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp38, "return of CreateSecret, state S187");
            this.Manager.Comment("reaching state \'S223\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp39;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp40;
            this.Manager.Comment("executing step \'call OpenSecret(1,True,\"G$$\",out _)\'");
            temp40 = this.ILsadManagedAdapterInstance.OpenSecret(1, true, "G$$", out temp39);
            this.Manager.Comment("reaching state \'S259\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp39, "secretHandle of OpenSecret, state S259");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp40, "return of OpenSecret, state S259");
            this.Manager.Comment("reaching state \'S295\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Null)\'");
            temp41 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue.Null);
            this.Manager.Comment("reaching state \'S319\'");
            this.Manager.Comment("checking step \'return StorePrivateData/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp41, "return of StorePrivateData, state S319");
            this.Manager.Comment("reaching state \'S343\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Valid,\"junkvalue\",out _)\'");
            temp43 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "junkvalue", out temp42);
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp42, "encryptedData of RetrievePrivateData, state S367");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp43, "return of RetrievePrivateData, state S367");
            TestScenarioS11S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S16() {
            this.Manager.BeginTest("TestScenarioS11S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp44;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp45 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp44);
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp44, "policyHandle of OpenPolicy, state S116");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp45, "return of OpenPolicy, state S116");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp46;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp47 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp46);
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp46, "secretHandle of CreateSecret, state S188");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp47, "return of CreateSecret, state S188");
            this.Manager.Comment("reaching state \'S224\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call OpenSecret(1,True,\"junkvalue\",out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.OpenSecret(1, true, "junkvalue", out temp48);
            this.Manager.Comment("reaching state \'S260\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp48, "secretHandle of OpenSecret, state S260");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp49, "return of OpenSecret, state S260");
            this.Manager.Comment("reaching state \'S296\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp50;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Null)\'");
            temp50 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue.Null);
            this.Manager.Comment("reaching state \'S320\'");
            this.Manager.Comment("checking step \'return StorePrivateData/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp50, "return of StorePrivateData, state S320");
            this.Manager.Comment("reaching state \'S344\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp51;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp52;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Valid,\"SecretName\",out _)\'");
            temp52 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SecretName", out temp51);
            this.Manager.Comment("reaching state \'S368\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp51, "encryptedData of RetrievePrivateData, state S368");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp52, "return of RetrievePrivateData, state S368");
            TestScenarioS11S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S18() {
            this.Manager.BeginTest("TestScenarioS11S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S81\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp53;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp54;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp54 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp53);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp53, "policyHandle of OpenPolicy, state S117");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp54, "return of OpenPolicy, state S117");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp55;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp56;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp56 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp55);
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp55, "secretHandle of CreateSecret, state S189");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp56, "return of CreateSecret, state S189");
            this.Manager.Comment("reaching state \'S225\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp57;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp58;
            this.Manager.Comment("executing step \'call OpenSecret(53,False,\"SecretName\",out _)\'");
            temp58 = this.ILsadManagedAdapterInstance.OpenSecret(53, false, "SecretName", out temp57);
            this.Manager.Comment("reaching state \'S261\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp57, "secretHandle of OpenSecret, state S261");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp58, "return of OpenSecret, state S261");
            TestScenarioS11S289();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS11S289() {
            this.Manager.Comment("reaching state \'S289\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Null)\'");
            temp59 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue.Null);
            this.Manager.Comment("reaching state \'S313\'");
            this.Manager.Comment("checking step \'return StorePrivateData/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp59, "return of StorePrivateData, state S313");
            this.Manager.Comment("reaching state \'S337\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp60;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Valid,\"junkvalue\",out _)\'");
            temp61 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "junkvalue", out temp60);
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp60, "encryptedData of RetrievePrivateData, state S361");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp61, "return of RetrievePrivateData, state S361");
            TestScenarioS11S385();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S2() {
            this.Manager.BeginTest("TestScenarioS11S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S73\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp63 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp62);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp62, "policyHandle of OpenPolicy, state S109");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp63, "return of OpenPolicy, state S109");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp64;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp65;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp65 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp64);
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp64, "secretHandle of CreateSecret, state S181");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp65, "return of CreateSecret, state S181");
            this.Manager.Comment("reaching state \'S217\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp66;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call OpenSecret(1,True,\"SecretName\",out _)\'");
            temp67 = this.ILsadManagedAdapterInstance.OpenSecret(1, true, "SecretName", out temp66);
            this.Manager.Comment("reaching state \'S253\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp66, "secretHandle of OpenSecret, state S253");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp67, "return of OpenSecret, state S253");
            TestScenarioS11S289();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S20() {
            this.Manager.BeginTest("TestScenarioS11S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp68);
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp68, "policyHandle of OpenPolicy, state S118");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp69, "return of OpenPolicy, state S118");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp71 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp70);
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp70, "secretHandle of CreateSecret, state S190");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp71, "return of CreateSecret, state S190");
            this.Manager.Comment("reaching state \'S226\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp72;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call OpenSecret(53,False,\"junkvalue\",out _)\'");
            temp73 = this.ILsadManagedAdapterInstance.OpenSecret(53, false, "junkvalue", out temp72);
            this.Manager.Comment("reaching state \'S262\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp72, "secretHandle of OpenSecret, state S262");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp73, "return of OpenSecret, state S262");
            TestScenarioS11S289();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S22() {
            this.Manager.BeginTest("TestScenarioS11S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S83\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp74;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp75;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp75 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp74);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp74, "policyHandle of OpenPolicy, state S119");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp75, "return of OpenPolicy, state S119");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp76;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp77 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp76);
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp76, "secretHandle of CreateSecret, state S191");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp77, "return of CreateSecret, state S191");
            this.Manager.Comment("reaching state \'S227\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp78;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
            this.Manager.Comment("executing step \'call OpenSecret(53,True,\"junkvalue\",out _)\'");
            temp79 = this.ILsadManagedAdapterInstance.OpenSecret(53, true, "junkvalue", out temp78);
            this.Manager.Comment("reaching state \'S263\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp78, "secretHandle of OpenSecret, state S263");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp79, "return of OpenSecret, state S263");
            TestScenarioS11S289();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S24() {
            this.Manager.BeginTest("TestScenarioS11S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp80;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp81 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp80);
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp80, "policyHandle of OpenPolicy, state S120");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp81, "return of OpenPolicy, state S120");
            this.Manager.Comment("reaching state \'S156\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp82;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp83;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp83 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp82);
            this.Manager.Comment("reaching state \'S192\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp82, "secretHandle of CreateSecret, state S192");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp83, "return of CreateSecret, state S192");
            this.Manager.Comment("reaching state \'S228\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp84;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call OpenSecret(1,True,\"SecretName\",out _)\'");
            temp85 = this.ILsadManagedAdapterInstance.OpenSecret(1, true, "SecretName", out temp84);
            this.Manager.Comment("reaching state \'S264\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp84, "secretHandle of OpenSecret, state S264");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp85, "return of OpenSecret, state S264");
            this.Manager.Comment("reaching state \'S297\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp86;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Valid)\'");
            temp86 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S321\'");
            this.Manager.Comment("checking step \'return StorePrivateData/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp86, "return of StorePrivateData, state S321");
            this.Manager.Comment("reaching state \'S345\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp87;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp88;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Valid,\"SecretName\",out _)\'");
            temp88 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SecretName", out temp87);
            this.Manager.Comment("reaching state \'S369\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp87, "encryptedData of RetrievePrivateData, state S369");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp88, "return of RetrievePrivateData, state S369");
            TestScenarioS11S386();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS11S386() {
            this.Manager.Comment("reaching state \'S386\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp89;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp90;
            this.Manager.Comment("executing step \'call DeleteObject(2,SecretObject,out _)\'");
            temp90 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(1)), out temp89);
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp89, "handleOutput of DeleteObject, state S389");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp90, "return of DeleteObject, state S389");
            this.Manager.Comment("reaching state \'S392\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp91;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp92;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp92 = this.ILsadManagedAdapterInstance.Close(1, out temp91);
            this.Manager.Comment("reaching state \'S395\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp91, "handleAfterClose of Close, state S395");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp92, "return of Close, state S395");
            this.Manager.Comment("reaching state \'S398\'");
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S26() {
            this.Manager.BeginTest("TestScenarioS11S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S85\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp93;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp94;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp94 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp93);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp93, "policyHandle of OpenPolicy, state S121");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp94, "return of OpenPolicy, state S121");
            this.Manager.Comment("reaching state \'S157\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp95;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp96;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp96 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp95);
            this.Manager.Comment("reaching state \'S193\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp95, "secretHandle of CreateSecret, state S193");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp96, "return of CreateSecret, state S193");
            this.Manager.Comment("reaching state \'S229\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp97;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp98;
            this.Manager.Comment("executing step \'call OpenSecret(1,False,\"SecretName\",out _)\'");
            temp98 = this.ILsadManagedAdapterInstance.OpenSecret(1, false, "SecretName", out temp97);
            this.Manager.Comment("reaching state \'S265\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp97, "secretHandle of OpenSecret, state S265");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp98, "return of OpenSecret, state S265");
            this.Manager.Comment("reaching state \'S298\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp99;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Null)\'");
            temp99 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue.Null);
            this.Manager.Comment("reaching state \'S322\'");
            this.Manager.Comment("checking step \'return StorePrivateData/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp99, "return of StorePrivateData, state S322");
            this.Manager.Comment("reaching state \'S346\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp100;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Invalid,\"SecretName\",out _)\'");
            temp101 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SecretName", out temp100);
            this.Manager.Comment("reaching state \'S370\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp100, "encryptedData of RetrievePrivateData, state S370");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp101, "return of RetrievePrivateData, state S370");
            TestScenarioS11S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S28() {
            this.Manager.BeginTest("TestScenarioS11S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp102;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp103 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp102);
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp102, "policyHandle of OpenPolicy, state S122");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp103, "return of OpenPolicy, state S122");
            this.Manager.Comment("reaching state \'S158\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp104;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp105 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp104);
            this.Manager.Comment("reaching state \'S194\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp104, "secretHandle of CreateSecret, state S194");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp105, "return of CreateSecret, state S194");
            this.Manager.Comment("reaching state \'S230\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp106;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp107;
            this.Manager.Comment("executing step \'call OpenSecret(1,False,\"junkvalue\",out _)\'");
            temp107 = this.ILsadManagedAdapterInstance.OpenSecret(1, false, "junkvalue", out temp106);
            this.Manager.Comment("reaching state \'S266\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp106, "secretHandle of OpenSecret, state S266");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp107, "return of OpenSecret, state S266");
            this.Manager.Comment("reaching state \'S299\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp108;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Invalid)\'");
            temp108 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)));
            this.Manager.Comment("reaching state \'S323\'");
            this.Manager.Comment("checking step \'return StorePrivateData/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp108, "return of StorePrivateData, state S323");
            this.Manager.Comment("reaching state \'S347\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp109;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp110;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Invalid,\"SecretName\",out _)\'");
            temp110 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SecretName", out temp109);
            this.Manager.Comment("reaching state \'S371\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp109, "encryptedData of RetrievePrivateData, state S371");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp110, "return of RetrievePrivateData, state S371");
            TestScenarioS11S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S30() {
            this.Manager.BeginTest("TestScenarioS11S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S87\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp111;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp112;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp112 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp111);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp111, "policyHandle of OpenPolicy, state S123");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp112, "return of OpenPolicy, state S123");
            this.Manager.Comment("reaching state \'S159\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp113;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp114;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp114 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp113);
            this.Manager.Comment("reaching state \'S195\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp113, "secretHandle of CreateSecret, state S195");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp114, "return of CreateSecret, state S195");
            this.Manager.Comment("reaching state \'S231\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp115;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp116;
            this.Manager.Comment("executing step \'call OpenSecret(53,False,\"G$$\",out _)\'");
            temp116 = this.ILsadManagedAdapterInstance.OpenSecret(53, false, "G$$", out temp115);
            this.Manager.Comment("reaching state \'S267\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp115, "secretHandle of OpenSecret, state S267");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp116, "return of OpenSecret, state S267");
            this.Manager.Comment("reaching state \'S300\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment("executing step \'call StorePrivateData(53,\"SecretName\",Invalid)\'");
            temp117 = this.ILsadManagedAdapterInstance.StorePrivateData(53, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)));
            this.Manager.Comment("reaching state \'S324\'");
            this.Manager.Comment("checking step \'return StorePrivateData/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp117, "return of StorePrivateData, state S324");
            this.Manager.Comment("reaching state \'S348\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp118;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp119;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Invalid,\"junkvalue\",out _)\'");
            temp119 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "junkvalue", out temp118);
            this.Manager.Comment("reaching state \'S372\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp118, "encryptedData of RetrievePrivateData, state S372");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp119, "return of RetrievePrivateData, state S372");
            TestScenarioS11S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S32() {
            this.Manager.BeginTest("TestScenarioS11S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp120;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp121;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp121 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp120);
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp120, "policyHandle of OpenPolicy, state S124");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp121, "return of OpenPolicy, state S124");
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp122;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp123;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp123 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp122);
            this.Manager.Comment("reaching state \'S196\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp122, "secretHandle of CreateSecret, state S196");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp123, "return of CreateSecret, state S196");
            this.Manager.Comment("reaching state \'S232\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp124;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp125;
            this.Manager.Comment("executing step \'call OpenSecret(53,True,\"G$$\",out _)\'");
            temp125 = this.ILsadManagedAdapterInstance.OpenSecret(53, true, "G$$", out temp124);
            this.Manager.Comment("reaching state \'S268\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp124, "secretHandle of OpenSecret, state S268");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp125, "return of OpenSecret, state S268");
            this.Manager.Comment("reaching state \'S301\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp126;
            this.Manager.Comment("executing step \'call StorePrivateData(53,\"SecretName\",Valid)\'");
            temp126 = this.ILsadManagedAdapterInstance.StorePrivateData(53, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S325\'");
            this.Manager.Comment("checking step \'return StorePrivateData/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp126, "return of StorePrivateData, state S325");
            this.Manager.Comment("reaching state \'S349\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp127;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp128;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Invalid,\"junkvalue\",out _)\'");
            temp128 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "junkvalue", out temp127);
            this.Manager.Comment("reaching state \'S373\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp127, "encryptedData of RetrievePrivateData, state S373");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp128, "return of RetrievePrivateData, state S373");
            TestScenarioS11S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S34() {
            this.Manager.BeginTest("TestScenarioS11S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S89\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp129;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp130;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp130 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp129);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp129, "policyHandle of OpenPolicy, state S125");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp130, "return of OpenPolicy, state S125");
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp131;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp132;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp132 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp131);
            this.Manager.Comment("reaching state \'S197\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp131, "secretHandle of CreateSecret, state S197");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp132, "return of CreateSecret, state S197");
            this.Manager.Comment("reaching state \'S233\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp133;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp134;
            this.Manager.Comment("executing step \'call OpenSecret(1,False,\"G$$\",out _)\'");
            temp134 = this.ILsadManagedAdapterInstance.OpenSecret(1, false, "G$$", out temp133);
            this.Manager.Comment("reaching state \'S269\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp133, "secretHandle of OpenSecret, state S269");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp134, "return of OpenSecret, state S269");
            this.Manager.Comment("reaching state \'S302\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp135;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Valid)\'");
            temp135 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S326\'");
            this.Manager.Comment("checking step \'return StorePrivateData/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp135, "return of StorePrivateData, state S326");
            this.Manager.Comment("reaching state \'S350\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp136;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp137;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Valid,\"junkvalue\",out _)\'");
            temp137 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "junkvalue", out temp136);
            this.Manager.Comment("reaching state \'S374\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp136, "encryptedData of RetrievePrivateData, state S374");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp137, "return of RetrievePrivateData, state S374");
            TestScenarioS11S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S36() {
            this.Manager.BeginTest("TestScenarioS11S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp138;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp139;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp139 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp138);
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp138, "policyHandle of OpenPolicy, state S126");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp139, "return of OpenPolicy, state S126");
            this.Manager.Comment("reaching state \'S162\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp140;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp141;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp141 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp140);
            this.Manager.Comment("reaching state \'S198\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp140, "secretHandle of CreateSecret, state S198");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp141, "return of CreateSecret, state S198");
            this.Manager.Comment("reaching state \'S234\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp142;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp143;
            this.Manager.Comment("executing step \'call OpenSecret(1,True,\"G$$\",out _)\'");
            temp143 = this.ILsadManagedAdapterInstance.OpenSecret(1, true, "G$$", out temp142);
            this.Manager.Comment("reaching state \'S270\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp142, "secretHandle of OpenSecret, state S270");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp143, "return of OpenSecret, state S270");
            this.Manager.Comment("reaching state \'S303\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp144;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Valid)\'");
            temp144 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S327\'");
            this.Manager.Comment("checking step \'return StorePrivateData/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp144, "return of StorePrivateData, state S327");
            this.Manager.Comment("reaching state \'S351\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp145;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp146;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Valid,\"SecretName\",out _)\'");
            temp146 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SecretName", out temp145);
            this.Manager.Comment("reaching state \'S375\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp145, "encryptedData of RetrievePrivateData, state S375");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp146, "return of RetrievePrivateData, state S375");
            TestScenarioS11S386();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S38() {
            this.Manager.BeginTest("TestScenarioS11S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S91\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp147;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp148;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp148 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp147);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp147, "policyHandle of OpenPolicy, state S127");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp148, "return of OpenPolicy, state S127");
            this.Manager.Comment("reaching state \'S163\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp149;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp150;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp150 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp149);
            this.Manager.Comment("reaching state \'S199\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp149, "secretHandle of CreateSecret, state S199");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp150, "return of CreateSecret, state S199");
            this.Manager.Comment("reaching state \'S235\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp151;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp152;
            this.Manager.Comment("executing step \'call OpenSecret(1,True,\"junkvalue\",out _)\'");
            temp152 = this.ILsadManagedAdapterInstance.OpenSecret(1, true, "junkvalue", out temp151);
            this.Manager.Comment("reaching state \'S271\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp151, "secretHandle of OpenSecret, state S271");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp152, "return of OpenSecret, state S271");
            TestScenarioS11S304();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS11S304() {
            this.Manager.Comment("reaching state \'S304\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp153;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Valid)\'");
            temp153 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S328\'");
            this.Manager.Comment("checking step \'return StorePrivateData/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp153, "return of StorePrivateData, state S328");
            this.Manager.Comment("reaching state \'S352\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp154;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp155;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Valid,\"junkvalue\",out _)\'");
            temp155 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "junkvalue", out temp154);
            this.Manager.Comment("reaching state \'S376\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp154, "encryptedData of RetrievePrivateData, state S376");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp155, "return of RetrievePrivateData, state S376");
            TestScenarioS11S386();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S4() {
            this.Manager.BeginTest("TestScenarioS11S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp156;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp157;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp157 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp156);
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp156, "policyHandle of OpenPolicy, state S110");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp157, "return of OpenPolicy, state S110");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp158;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp159;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp159 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp158);
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp158, "secretHandle of CreateSecret, state S182");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp159, "return of CreateSecret, state S182");
            this.Manager.Comment("reaching state \'S218\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp160;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp161;
            this.Manager.Comment("executing step \'call OpenSecret(1,False,\"SecretName\",out _)\'");
            temp161 = this.ILsadManagedAdapterInstance.OpenSecret(1, false, "SecretName", out temp160);
            this.Manager.Comment("reaching state \'S254\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp160, "secretHandle of OpenSecret, state S254");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp161, "return of OpenSecret, state S254");
            this.Manager.Comment("reaching state \'S290\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp162;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Null)\'");
            temp162 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue.Null);
            this.Manager.Comment("reaching state \'S314\'");
            this.Manager.Comment("checking step \'return StorePrivateData/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp162, "return of StorePrivateData, state S314");
            this.Manager.Comment("reaching state \'S338\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp163;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp164;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Valid,\"SecretName\",out _)\'");
            temp164 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SecretName", out temp163);
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)), temp163, "encryptedData of RetrievePrivateData, state S362");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp164, "return of RetrievePrivateData, state S362");
            TestScenarioS11S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S40() {
            this.Manager.BeginTest("TestScenarioS11S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp165;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp166;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp166 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp165);
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp165, "policyHandle of OpenPolicy, state S128");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp166, "return of OpenPolicy, state S128");
            this.Manager.Comment("reaching state \'S164\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp167;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp168;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp168 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp167);
            this.Manager.Comment("reaching state \'S200\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp167, "secretHandle of CreateSecret, state S200");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp168, "return of CreateSecret, state S200");
            this.Manager.Comment("reaching state \'S236\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp169;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp170;
            this.Manager.Comment("executing step \'call OpenSecret(53,False,\"SecretName\",out _)\'");
            temp170 = this.ILsadManagedAdapterInstance.OpenSecret(53, false, "SecretName", out temp169);
            this.Manager.Comment("reaching state \'S272\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp169, "secretHandle of OpenSecret, state S272");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp170, "return of OpenSecret, state S272");
            TestScenarioS11S304();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S42() {
            this.Manager.BeginTest("TestScenarioS11S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S93\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp171;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp172;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp172 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp171);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp171, "policyHandle of OpenPolicy, state S129");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp172, "return of OpenPolicy, state S129");
            this.Manager.Comment("reaching state \'S165\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp173;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp174;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp174 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp173);
            this.Manager.Comment("reaching state \'S201\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp173, "secretHandle of CreateSecret, state S201");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp174, "return of CreateSecret, state S201");
            this.Manager.Comment("reaching state \'S237\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp175;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp176;
            this.Manager.Comment("executing step \'call OpenSecret(53,False,\"junkvalue\",out _)\'");
            temp176 = this.ILsadManagedAdapterInstance.OpenSecret(53, false, "junkvalue", out temp175);
            this.Manager.Comment("reaching state \'S273\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp175, "secretHandle of OpenSecret, state S273");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp176, "return of OpenSecret, state S273");
            TestScenarioS11S304();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S44() {
            this.Manager.BeginTest("TestScenarioS11S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp177;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp178;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp178 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp177);
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp177, "policyHandle of OpenPolicy, state S130");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp178, "return of OpenPolicy, state S130");
            this.Manager.Comment("reaching state \'S166\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp179;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp180;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp180 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp179);
            this.Manager.Comment("reaching state \'S202\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp179, "secretHandle of CreateSecret, state S202");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp180, "return of CreateSecret, state S202");
            this.Manager.Comment("reaching state \'S238\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp181;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp182;
            this.Manager.Comment("executing step \'call OpenSecret(53,True,\"junkvalue\",out _)\'");
            temp182 = this.ILsadManagedAdapterInstance.OpenSecret(53, true, "junkvalue", out temp181);
            this.Manager.Comment("reaching state \'S274\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp181, "secretHandle of OpenSecret, state S274");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp182, "return of OpenSecret, state S274");
            TestScenarioS11S304();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S46() {
            this.Manager.BeginTest("TestScenarioS11S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S95\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp183;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp184;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp184 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp183);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp183, "policyHandle of OpenPolicy, state S131");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp184, "return of OpenPolicy, state S131");
            this.Manager.Comment("reaching state \'S167\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp185;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp186;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp186 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp185);
            this.Manager.Comment("reaching state \'S203\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp185, "secretHandle of CreateSecret, state S203");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp186, "return of CreateSecret, state S203");
            this.Manager.Comment("reaching state \'S239\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp187;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp188;
            this.Manager.Comment("executing step \'call OpenSecret(1,True,\"SecretName\",out _)\'");
            temp188 = this.ILsadManagedAdapterInstance.OpenSecret(1, true, "SecretName", out temp187);
            this.Manager.Comment("reaching state \'S275\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp187, "secretHandle of OpenSecret, state S275");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp188, "return of OpenSecret, state S275");
            TestScenarioS11S288();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S48() {
            this.Manager.BeginTest("TestScenarioS11S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp189;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp190;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp190 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp189);
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp189, "policyHandle of OpenPolicy, state S132");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp190, "return of OpenPolicy, state S132");
            this.Manager.Comment("reaching state \'S168\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp191;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp192;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp192 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp191);
            this.Manager.Comment("reaching state \'S204\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp191, "secretHandle of CreateSecret, state S204");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp192, "return of CreateSecret, state S204");
            this.Manager.Comment("reaching state \'S240\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp193;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp194;
            this.Manager.Comment("executing step \'call OpenSecret(1,False,\"SecretName\",out _)\'");
            temp194 = this.ILsadManagedAdapterInstance.OpenSecret(1, false, "SecretName", out temp193);
            this.Manager.Comment("reaching state \'S276\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp193, "secretHandle of OpenSecret, state S276");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp194, "return of OpenSecret, state S276");
            this.Manager.Comment("reaching state \'S305\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp195;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Valid)\'");
            temp195 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S329\'");
            this.Manager.Comment("checking step \'return StorePrivateData/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp195, "return of StorePrivateData, state S329");
            this.Manager.Comment("reaching state \'S353\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp196;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp197;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Valid,\"SecretName\",out _)\'");
            temp197 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SecretName", out temp196);
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)), temp196, "encryptedData of RetrievePrivateData, state S377");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp197, "return of RetrievePrivateData, state S377");
            TestScenarioS11S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S50() {
            this.Manager.BeginTest("TestScenarioS11S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S97\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp198;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp199;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp199 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp198);
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp198, "policyHandle of OpenPolicy, state S133");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp199, "return of OpenPolicy, state S133");
            this.Manager.Comment("reaching state \'S169\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp200;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp201;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp201 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp200);
            this.Manager.Comment("reaching state \'S205\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp200, "secretHandle of CreateSecret, state S205");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp201, "return of CreateSecret, state S205");
            this.Manager.Comment("reaching state \'S241\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp202;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp203;
            this.Manager.Comment("executing step \'call OpenSecret(1,False,\"junkvalue\",out _)\'");
            temp203 = this.ILsadManagedAdapterInstance.OpenSecret(1, false, "junkvalue", out temp202);
            this.Manager.Comment("reaching state \'S277\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp202, "secretHandle of OpenSecret, state S277");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp203, "return of OpenSecret, state S277");
            this.Manager.Comment("reaching state \'S306\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp204;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Null)\'");
            temp204 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue.Null);
            this.Manager.Comment("reaching state \'S330\'");
            this.Manager.Comment("checking step \'return StorePrivateData/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp204, "return of StorePrivateData, state S330");
            this.Manager.Comment("reaching state \'S354\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp205;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp206;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Invalid,\"SecretName\",out _)\'");
            temp206 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SecretName", out temp205);
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp205, "encryptedData of RetrievePrivateData, state S378");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp206, "return of RetrievePrivateData, state S378");
            TestScenarioS11S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S52() {
            this.Manager.BeginTest("TestScenarioS11S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp207;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp208;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp208 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp207);
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp207, "policyHandle of OpenPolicy, state S134");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp208, "return of OpenPolicy, state S134");
            this.Manager.Comment("reaching state \'S170\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp209;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp210;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp210 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp209);
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp209, "secretHandle of CreateSecret, state S206");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp210, "return of CreateSecret, state S206");
            this.Manager.Comment("reaching state \'S242\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp211;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp212;
            this.Manager.Comment("executing step \'call OpenSecret(53,False,\"G$$\",out _)\'");
            temp212 = this.ILsadManagedAdapterInstance.OpenSecret(53, false, "G$$", out temp211);
            this.Manager.Comment("reaching state \'S278\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp211, "secretHandle of OpenSecret, state S278");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp212, "return of OpenSecret, state S278");
            this.Manager.Comment("reaching state \'S307\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp213;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Invalid)\'");
            temp213 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)));
            this.Manager.Comment("reaching state \'S331\'");
            this.Manager.Comment("checking step \'return StorePrivateData/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp213, "return of StorePrivateData, state S331");
            this.Manager.Comment("reaching state \'S355\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp214;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp215;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Invalid,\"SecretName\",out _)\'");
            temp215 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SecretName", out temp214);
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp214, "encryptedData of RetrievePrivateData, state S379");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp215, "return of RetrievePrivateData, state S379");
            TestScenarioS11S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S54() {
            this.Manager.BeginTest("TestScenarioS11S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S99\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp216;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp217;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp217 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp216);
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp216, "policyHandle of OpenPolicy, state S135");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp217, "return of OpenPolicy, state S135");
            this.Manager.Comment("reaching state \'S171\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp218;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp219;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp219 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp218);
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp218, "secretHandle of CreateSecret, state S207");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp219, "return of CreateSecret, state S207");
            this.Manager.Comment("reaching state \'S243\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp220;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp221;
            this.Manager.Comment("executing step \'call OpenSecret(53,True,\"G$$\",out _)\'");
            temp221 = this.ILsadManagedAdapterInstance.OpenSecret(53, true, "G$$", out temp220);
            this.Manager.Comment("reaching state \'S279\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp220, "secretHandle of OpenSecret, state S279");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp221, "return of OpenSecret, state S279");
            this.Manager.Comment("reaching state \'S308\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp222;
            this.Manager.Comment("executing step \'call StorePrivateData(53,\"SecretName\",Invalid)\'");
            temp222 = this.ILsadManagedAdapterInstance.StorePrivateData(53, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)));
            this.Manager.Comment("reaching state \'S332\'");
            this.Manager.Comment("checking step \'return StorePrivateData/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp222, "return of StorePrivateData, state S332");
            this.Manager.Comment("reaching state \'S356\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp223;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp224;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Invalid,\"junkvalue\",out _)\'");
            temp224 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "junkvalue", out temp223);
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp223, "encryptedData of RetrievePrivateData, state S380");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp224, "return of RetrievePrivateData, state S380");
            TestScenarioS11S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S56() {
            this.Manager.BeginTest("TestScenarioS11S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp225;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp226;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp226 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp225);
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp225, "policyHandle of OpenPolicy, state S136");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp226, "return of OpenPolicy, state S136");
            this.Manager.Comment("reaching state \'S172\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp227;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp228;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp228 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp227);
            this.Manager.Comment("reaching state \'S208\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp227, "secretHandle of CreateSecret, state S208");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp228, "return of CreateSecret, state S208");
            this.Manager.Comment("reaching state \'S244\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp229;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp230;
            this.Manager.Comment("executing step \'call OpenSecret(1,False,\"G$$\",out _)\'");
            temp230 = this.ILsadManagedAdapterInstance.OpenSecret(1, false, "G$$", out temp229);
            this.Manager.Comment("reaching state \'S280\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp229, "secretHandle of OpenSecret, state S280");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp230, "return of OpenSecret, state S280");
            this.Manager.Comment("reaching state \'S309\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp231;
            this.Manager.Comment("executing step \'call StorePrivateData(53,\"SecretName\",Valid)\'");
            temp231 = this.ILsadManagedAdapterInstance.StorePrivateData(53, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S333\'");
            this.Manager.Comment("checking step \'return StorePrivateData/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp231, "return of StorePrivateData, state S333");
            this.Manager.Comment("reaching state \'S357\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp232;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp233;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Invalid,\"junkvalue\",out _)\'");
            temp233 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "junkvalue", out temp232);
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp232, "encryptedData of RetrievePrivateData, state S381");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp233, "return of RetrievePrivateData, state S381");
            TestScenarioS11S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S58() {
            this.Manager.BeginTest("TestScenarioS11S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S101\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp234;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp235;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp235 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp234);
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp234, "policyHandle of OpenPolicy, state S137");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp235, "return of OpenPolicy, state S137");
            this.Manager.Comment("reaching state \'S173\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp236;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp237;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp237 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp236);
            this.Manager.Comment("reaching state \'S209\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp236, "secretHandle of CreateSecret, state S209");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp237, "return of CreateSecret, state S209");
            this.Manager.Comment("reaching state \'S245\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp238;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp239;
            this.Manager.Comment("executing step \'call OpenSecret(1,True,\"G$$\",out _)\'");
            temp239 = this.ILsadManagedAdapterInstance.OpenSecret(1, true, "G$$", out temp238);
            this.Manager.Comment("reaching state \'S281\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp238, "secretHandle of OpenSecret, state S281");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp239, "return of OpenSecret, state S281");
            this.Manager.Comment("reaching state \'S310\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp240;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Valid)\'");
            temp240 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S334\'");
            this.Manager.Comment("checking step \'return StorePrivateData/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp240, "return of StorePrivateData, state S334");
            this.Manager.Comment("reaching state \'S358\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp241;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp242;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Valid,\"junkvalue\",out _)\'");
            temp242 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "junkvalue", out temp241);
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp241, "encryptedData of RetrievePrivateData, state S382");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp242, "return of RetrievePrivateData, state S382");
            TestScenarioS11S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S6() {
            this.Manager.BeginTest("TestScenarioS11S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S75\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp243;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp244;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp244 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp243);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp243, "policyHandle of OpenPolicy, state S111");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp244, "return of OpenPolicy, state S111");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp245;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp246;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp246 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp245);
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp245, "secretHandle of CreateSecret, state S183");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp246, "return of CreateSecret, state S183");
            this.Manager.Comment("reaching state \'S219\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp247;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp248;
            this.Manager.Comment("executing step \'call OpenSecret(1,False,\"junkvalue\",out _)\'");
            temp248 = this.ILsadManagedAdapterInstance.OpenSecret(1, false, "junkvalue", out temp247);
            this.Manager.Comment("reaching state \'S255\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp247, "secretHandle of OpenSecret, state S255");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp248, "return of OpenSecret, state S255");
            this.Manager.Comment("reaching state \'S291\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp249;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Valid)\'");
            temp249 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S315\'");
            this.Manager.Comment("checking step \'return StorePrivateData/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp249, "return of StorePrivateData, state S315");
            this.Manager.Comment("reaching state \'S339\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp250;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp251;
            this.Manager.Comment("executing step \'call RetrievePrivateData(1,Invalid,\"SecretName\",out _)\'");
            temp251 = this.ILsadManagedAdapterInstance.RetrievePrivateData(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SecretName", out temp250);
            this.Manager.Comment("reaching state \'S363\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp250, "encryptedData of RetrievePrivateData, state S363");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp251, "return of RetrievePrivateData, state S363");
            TestScenarioS11S385();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S60() {
            this.Manager.BeginTest("TestScenarioS11S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp252;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp253;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp253 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp252);
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp252, "policyHandle of OpenPolicy, state S138");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp253, "return of OpenPolicy, state S138");
            this.Manager.Comment("reaching state \'S174\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp254;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp255;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp255 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp254);
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp254, "secretHandle of CreateSecret, state S210");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp255, "return of CreateSecret, state S210");
            this.Manager.Comment("reaching state \'S246\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp256;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp257;
            this.Manager.Comment("executing step \'call OpenSecret(1,True,\"junkvalue\",out _)\'");
            temp257 = this.ILsadManagedAdapterInstance.OpenSecret(1, true, "junkvalue", out temp256);
            this.Manager.Comment("reaching state \'S282\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp256, "secretHandle of OpenSecret, state S282");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp257, "return of OpenSecret, state S282");
            this.Manager.Comment("reaching state \'S311\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp258;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Valid)\'");
            temp258 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(0)));
            this.Manager.Comment("reaching state \'S335\'");
            this.Manager.Comment("checking step \'return StorePrivateData/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp258, "return of StorePrivateData, state S335");
            this.Manager.Comment("reaching state \'S359\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp259;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp260;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Valid,\"SecretName\",out _)\'");
            temp260 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), "SecretName", out temp259);
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp259, "encryptedData of RetrievePrivateData, state S383");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp260, "return of RetrievePrivateData, state S383");
            TestScenarioS11S384();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S62() {
            this.Manager.BeginTest("TestScenarioS11S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S103\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp261;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp262;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp262 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp261);
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp261, "policyHandle of OpenPolicy, state S139");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp262, "return of OpenPolicy, state S139");
            this.Manager.Comment("reaching state \'S175\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp263;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp264;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp264 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp263);
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp263, "secretHandle of CreateSecret, state S211");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp264, "return of CreateSecret, state S211");
            this.Manager.Comment("reaching state \'S247\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp265;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp266;
            this.Manager.Comment("executing step \'call OpenSecret(53,False,\"SecretName\",out _)\'");
            temp266 = this.ILsadManagedAdapterInstance.OpenSecret(53, false, "SecretName", out temp265);
            this.Manager.Comment("reaching state \'S283\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp265, "secretHandle of OpenSecret, state S283");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp266, "return of OpenSecret, state S283");
            TestScenarioS11S288();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S64() {
            this.Manager.BeginTest("TestScenarioS11S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp267;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp268;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp268 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp267);
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp267, "policyHandle of OpenPolicy, state S140");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp268, "return of OpenPolicy, state S140");
            this.Manager.Comment("reaching state \'S176\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp269;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp270;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp270 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp269);
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp269, "secretHandle of CreateSecret, state S212");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp270, "return of CreateSecret, state S212");
            this.Manager.Comment("reaching state \'S248\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp271;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp272;
            this.Manager.Comment("executing step \'call OpenSecret(53,False,\"junkvalue\",out _)\'");
            temp272 = this.ILsadManagedAdapterInstance.OpenSecret(53, false, "junkvalue", out temp271);
            this.Manager.Comment("reaching state \'S284\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp271, "secretHandle of OpenSecret, state S284");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp272, "return of OpenSecret, state S284");
            TestScenarioS11S288();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S66() {
            this.Manager.BeginTest("TestScenarioS11S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S105\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp273;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp274;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp274 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp273);
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp273, "policyHandle of OpenPolicy, state S141");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp274, "return of OpenPolicy, state S141");
            this.Manager.Comment("reaching state \'S177\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp275;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp276;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",4061069327,out _)\'");
            temp276 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 4061069327u, out temp275);
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp275, "secretHandle of CreateSecret, state S213");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp276, "return of CreateSecret, state S213");
            this.Manager.Comment("reaching state \'S249\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp277;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp278;
            this.Manager.Comment("executing step \'call OpenSecret(53,True,\"junkvalue\",out _)\'");
            temp278 = this.ILsadManagedAdapterInstance.OpenSecret(53, true, "junkvalue", out temp277);
            this.Manager.Comment("reaching state \'S285\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp277, "secretHandle of OpenSecret, state S285");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp278, "return of OpenSecret, state S285");
            TestScenarioS11S288();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S68() {
            this.Manager.BeginTest("TestScenarioS11S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp279;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp280;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp280 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp279);
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp279, "policyHandle of OpenPolicy, state S142");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp280, "return of OpenPolicy, state S142");
            this.Manager.Comment("reaching state \'S178\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp281;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp282;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp282 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp281);
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp281, "secretHandle of CreateSecret, state S214");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp282, "return of CreateSecret, state S214");
            this.Manager.Comment("reaching state \'S250\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp283;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp284;
            this.Manager.Comment("executing step \'call OpenSecret(53,True,\"SecretName\",out _)\'");
            temp284 = this.ILsadManagedAdapterInstance.OpenSecret(53, true, "SecretName", out temp283);
            this.Manager.Comment("reaching state \'S286\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp283, "secretHandle of OpenSecret, state S286");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp284, "return of OpenSecret, state S286");
            TestScenarioS11S289();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S70() {
            this.Manager.BeginTest("TestScenarioS11S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S107\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp285;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp286;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp286 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp285);
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp285, "policyHandle of OpenPolicy, state S143");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp286, "return of OpenPolicy, state S143");
            this.Manager.Comment("reaching state \'S179\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp287;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp288;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",1,out _)\'");
            temp288 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 1u, out temp287);
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp287, "secretHandle of CreateSecret, state S215");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp288, "return of CreateSecret, state S215");
            this.Manager.Comment("reaching state \'S251\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp289;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp290;
            this.Manager.Comment("executing step \'call OpenSecret(53,True,\"SecretName\",out _)\'");
            temp290 = this.ILsadManagedAdapterInstance.OpenSecret(53, true, "SecretName", out temp289);
            this.Manager.Comment("reaching state \'S287\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp289, "secretHandle of OpenSecret, state S287");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp290, "return of OpenSecret, state S287");
            TestScenarioS11S304();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS11S8() {
            this.Manager.BeginTest("TestScenarioS11S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp291;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp292;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp292 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp291);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp291, "policyHandle of OpenPolicy, state S112");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp292, "return of OpenPolicy, state S112");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp293;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp294;
            this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
            temp294 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp293);
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("checking step \'return CreateSecret/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(0)), temp293, "secretHandle of CreateSecret, state S184");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp294, "return of CreateSecret, state S184");
            this.Manager.Comment("reaching state \'S220\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp295;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp296;
            this.Manager.Comment("executing step \'call OpenSecret(53,False,\"G$$\",out _)\'");
            temp296 = this.ILsadManagedAdapterInstance.OpenSecret(53, false, "G$$", out temp295);
            this.Manager.Comment("reaching state \'S256\'");
            this.Manager.Comment("checking step \'return OpenSecret/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp295, "secretHandle of OpenSecret, state S256");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp296, "return of OpenSecret, state S256");
            this.Manager.Comment("reaching state \'S292\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp297;
            this.Manager.Comment("executing step \'call StorePrivateData(1,\"SecretName\",Invalid)\'");
            temp297 = this.ILsadManagedAdapterInstance.StorePrivateData(1, "SecretName", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)));
            this.Manager.Comment("reaching state \'S316\'");
            this.Manager.Comment("checking step \'return StorePrivateData/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp297, "return of StorePrivateData, state S316");
            this.Manager.Comment("reaching state \'S340\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue temp298;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp299;
            this.Manager.Comment("executing step \'call RetrievePrivateData(53,Invalid,\"SecretName\",out _)\'");
            temp299 = this.ILsadManagedAdapterInstance.RetrievePrivateData(53, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), "SecretName", out temp298);
            this.Manager.Comment("reaching state \'S364\'");
            this.Manager.Comment("checking step \'return RetrievePrivateData/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.EncryptedValue)(1)), temp298, "encryptedData of RetrievePrivateData, state S364");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp299, "return of RetrievePrivateData, state S364");
            TestScenarioS11S385();
            this.Manager.EndTest();
        }
        #endregion
    }
}
