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
    public partial class TestScenarioS22 : PtfTestClassBase {
        
        public TestScenarioS22() {
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
        public void LSAD_TestScenarioS22S0() {
            this.Manager.BeginTest("TestScenarioS22S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp0);
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S108");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S108");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Invalid,\"SID\",out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp2);
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp2, "accountHandle of CreateAccount, state S180");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp3, "return of CreateAccount, state S180");
            this.Manager.Comment("reaching state \'S216\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp4);
            this.Manager.Comment("reaching state \'S252\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp4, "trustHandle of CreateTrustedDomain, state S252");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp5, "return of CreateTrustedDomain, state S252");
            this.Manager.Comment("reaching state \'S288\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,DACLSecurityInformation,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, out temp6);
            this.Manager.Comment("reaching state \'S324\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp6, "securityDescriptor of QuerySecurityObject, state S324");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp7, "return of QuerySecurityObject, state S324");
            this.Manager.Comment("reaching state \'S360\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Valid)\'");
            temp8 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp8, "return of SetSecurityObject, state S396");
            TestScenarioS22S432();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS22S432() {
            this.Manager.Comment("reaching state \'S432\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp9;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
            this.Manager.Comment("executing step \'call DeleteObject(1,AccountObject,out _)\'");
            temp10 = this.ILsadManagedAdapterInstance.DeleteObject(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp9);
            this.Manager.Comment("reaching state \'S444\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp9, "handleOutput of DeleteObject, state S444");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp10, "return of DeleteObject, state S444");
            this.Manager.Comment("reaching state \'S456\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp11;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp12;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp12 = this.ILsadManagedAdapterInstance.Close(1, out temp11);
            this.Manager.Comment("reaching state \'S464\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp11, "handleAfterClose of Close, state S464");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp12, "return of Close, state S464");
            this.Manager.Comment("reaching state \'S472\'");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S10() {
            this.Manager.BeginTest("TestScenarioS22S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S77\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp13;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp14;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp14 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp13);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp13, "policyHandle of OpenPolicy, state S113");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp14, "return of OpenPolicy, state S113");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp15;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp16;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp16 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp15);
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp15, "accountHandle of CreateAccount, state S185");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp16, "return of CreateAccount, state S185");
            this.Manager.Comment("reaching state \'S221\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp17;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp18;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp18 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp17);
            this.Manager.Comment("reaching state \'S257\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp17, "trustHandle of CreateTrustedDomain, state S257");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp18, "return of CreateTrustedDomain, state S257");
            this.Manager.Comment("reaching state \'S293\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp19;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp20;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp20 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp19);
            this.Manager.Comment("reaching state \'S329\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp19, "securityDescriptor of QuerySecurityObject, state S329");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp20, "return of QuerySecurityObject, state S329");
            this.Manager.Comment("reaching state \'S365\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Null)\'");
            temp21 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S401\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp21, "return of SetSecurityObject, state S401");
            TestScenarioS22S433();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS22S433() {
            this.Manager.Comment("reaching state \'S433\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call DeleteObject(1,AccountObject,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.DeleteObject(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp22);
            this.Manager.Comment("reaching state \'S445\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp22, "handleOutput of DeleteObject, state S445");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp23, "return of DeleteObject, state S445");
            this.Manager.Comment("reaching state \'S457\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.Close(1, out temp24);
            this.Manager.Comment("reaching state \'S465\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp24, "handleAfterClose of Close, state S465");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp25, "return of Close, state S465");
            this.Manager.Comment("reaching state \'S473\'");
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S12() {
            this.Manager.BeginTest("TestScenarioS22S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp26);
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy, state S114");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy, state S114");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp28);
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp28, "accountHandle of CreateAccount, state S186");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of CreateAccount, state S186");
            this.Manager.Comment("reaching state \'S222\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp31 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp30);
            this.Manager.Comment("reaching state \'S258\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp30, "trustHandle of CreateTrustedDomain, state S258");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp31, "return of CreateTrustedDomain, state S258");
            this.Manager.Comment("reaching state \'S294\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp33 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp32);
            this.Manager.Comment("reaching state \'S330\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp32, "securityDescriptor of QuerySecurityObject, state S330");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp33, "return of QuerySecurityObject, state S330");
            this.Manager.Comment("reaching state \'S366\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp34;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Valid)\'");
            temp34 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S402\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp34, "return of SetSecurityObject, state S402");
            TestScenarioS22S435();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS22S435() {
            this.Manager.Comment("reaching state \'S435\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp35;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp36;
            this.Manager.Comment("executing step \'call DeleteObject(1,AccountObject,out _)\'");
            temp36 = this.ILsadManagedAdapterInstance.DeleteObject(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp35);
            this.Manager.Comment("reaching state \'S447\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp35, "handleOutput of DeleteObject, state S447");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp36, "return of DeleteObject, state S447");
            this.Manager.Comment("reaching state \'S459\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp37;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp38;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp38 = this.ILsadManagedAdapterInstance.Close(1, out temp37);
            this.Manager.Comment("reaching state \'S467\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp37, "handleAfterClose of Close, state S467");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp38, "return of Close, state S467");
            this.Manager.Comment("reaching state \'S475\'");
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S14() {
            this.Manager.BeginTest("TestScenarioS22S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S79\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp39;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp40;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp40 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp39);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp39, "policyHandle of OpenPolicy, state S115");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp40, "return of OpenPolicy, state S115");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp41;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp42;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp42 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp41);
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp41, "accountHandle of CreateAccount, state S187");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp42, "return of CreateAccount, state S187");
            this.Manager.Comment("reaching state \'S223\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp43;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp44;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp44 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp43);
            this.Manager.Comment("reaching state \'S259\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp43, "trustHandle of CreateTrustedDomain, state S259");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp44, "return of CreateTrustedDomain, state S259");
            this.Manager.Comment("reaching state \'S295\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp45;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp46;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp46 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp45);
            this.Manager.Comment("reaching state \'S331\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp45, "securityDescriptor of QuerySecurityObject, state S331");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp46, "return of QuerySecurityObject, state S331");
            this.Manager.Comment("reaching state \'S367\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Valid)\'");
            temp47 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S403\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp47, "return of SetSecurityObject, state S403");
            this.Manager.Comment("reaching state \'S436\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp48);
            this.Manager.Comment("reaching state \'S448\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp48, "handleOutput of DeleteObject, state S448");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp49, "return of DeleteObject, state S448");
            this.Manager.Comment("reaching state \'S460\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp50;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp51 = this.ILsadManagedAdapterInstance.Close(1, out temp50);
            this.Manager.Comment("reaching state \'S468\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp50, "handleAfterClose of Close, state S468");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp51, "return of Close, state S468");
            this.Manager.Comment("reaching state \'S476\'");
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S16() {
            this.Manager.BeginTest("TestScenarioS22S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp53 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp52);
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp52, "policyHandle of OpenPolicy, state S116");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp53, "return of OpenPolicy, state S116");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp55 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp54);
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp54, "accountHandle of CreateAccount, state S188");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp55, "return of CreateAccount, state S188");
            this.Manager.Comment("reaching state \'S224\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp56;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp57;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp57 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp56);
            this.Manager.Comment("reaching state \'S260\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp56, "trustHandle of CreateTrustedDomain, state S260");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp57, "return of CreateTrustedDomain, state S260");
            this.Manager.Comment("reaching state \'S296\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp58;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp59 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp58);
            this.Manager.Comment("reaching state \'S332\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp58, "securityDescriptor of QuerySecurityObject, state S332");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp59, "return of QuerySecurityObject, state S332");
            this.Manager.Comment("reaching state \'S368\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp60;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Invalid)\'");
            temp60 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S404\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp60, "return of SetSecurityObject, state S404");
            TestScenarioS22S435();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S18() {
            this.Manager.BeginTest("TestScenarioS22S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S81\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp61;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp62;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp62 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp61);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp61, "policyHandle of OpenPolicy, state S117");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp62, "return of OpenPolicy, state S117");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp63;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp64;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp64 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp63);
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp63, "accountHandle of CreateAccount, state S189");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp64, "return of CreateAccount, state S189");
            this.Manager.Comment("reaching state \'S225\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp65;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp66;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp66 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp65);
            this.Manager.Comment("reaching state \'S261\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp65, "trustHandle of CreateTrustedDomain, state S261");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp66, "return of CreateTrustedDomain, state S261");
            this.Manager.Comment("reaching state \'S297\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp67;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp68;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp68 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp67);
            this.Manager.Comment("reaching state \'S333\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp67, "securityDescriptor of QuerySecurityObject, state S333");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp68, "return of QuerySecurityObject, state S333");
            this.Manager.Comment("reaching state \'S369\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Null)\'");
            temp69 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S405\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp69, "return of SetSecurityObject, state S405");
            TestScenarioS22S435();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S2() {
            this.Manager.BeginTest("TestScenarioS22S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S73\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp71 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp70);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp70, "policyHandle of OpenPolicy, state S109");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp71, "return of OpenPolicy, state S109");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp72;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp73 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp72);
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp72, "accountHandle of CreateAccount, state S181");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp73, "return of CreateAccount, state S181");
            this.Manager.Comment("reaching state \'S217\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp74;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp75;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp75 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp74);
            this.Manager.Comment("reaching state \'S253\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp74, "trustHandle of CreateTrustedDomain, state S253");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp75, "return of CreateTrustedDomain, state S253");
            this.Manager.Comment("reaching state \'S289\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp76;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp77 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp76);
            this.Manager.Comment("reaching state \'S325\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp76, "securityDescriptor of QuerySecurityObject, state S325");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp77, "return of QuerySecurityObject, state S325");
            this.Manager.Comment("reaching state \'S361\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp78;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Valid)\'");
            temp78 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S397\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp78, "return of SetSecurityObject, state S397");
            TestScenarioS22S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S20() {
            this.Manager.BeginTest("TestScenarioS22S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp79;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp80;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp80 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp79);
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp79, "policyHandle of OpenPolicy, state S118");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp80, "return of OpenPolicy, state S118");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp81;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp82;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp82 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp81);
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp81, "accountHandle of CreateAccount, state S190");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp82, "return of CreateAccount, state S190");
            this.Manager.Comment("reaching state \'S226\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp83;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp84;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp84 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp83);
            this.Manager.Comment("reaching state \'S262\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp83, "trustHandle of CreateTrustedDomain, state S262");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp84, "return of CreateTrustedDomain, state S262");
            this.Manager.Comment("reaching state \'S298\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp85;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp86;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,DACLSecurityInformation,out _)\'");
            temp86 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, out temp85);
            this.Manager.Comment("reaching state \'S334\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp85, "securityDescriptor of QuerySecurityObject, state S334");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp86, "return of QuerySecurityObject, state S334");
            this.Manager.Comment("reaching state \'S370\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Invalid)\'");
            temp87 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S406\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp87, "return of SetSecurityObject, state S406");
            TestScenarioS22S435();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S22() {
            this.Manager.BeginTest("TestScenarioS22S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S83\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp88;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp89 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp88);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp88, "policyHandle of OpenPolicy, state S119");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp89, "return of OpenPolicy, state S119");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp90;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp91;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp91 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp90);
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp90, "accountHandle of CreateAccount, state S191");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp91, "return of CreateAccount, state S191");
            this.Manager.Comment("reaching state \'S227\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp92;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp93;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp93 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp92);
            this.Manager.Comment("reaching state \'S263\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp92, "trustHandle of CreateTrustedDomain, state S263");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp93, "return of CreateTrustedDomain, state S263");
            this.Manager.Comment("reaching state \'S299\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp94;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp95;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,DACLSecurityInformation,out _)\'");
            temp95 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, out temp94);
            this.Manager.Comment("reaching state \'S335\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp94, "securityDescriptor of QuerySecurityObject, state S335");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp95, "return of QuerySecurityObject, state S335");
            this.Manager.Comment("reaching state \'S371\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp96;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Null)\'");
            temp96 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S407\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp96, "return of SetSecurityObject, state S407");
            TestScenarioS22S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S24() {
            this.Manager.BeginTest("TestScenarioS22S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp97;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp98;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp98 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp97);
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp97, "policyHandle of OpenPolicy, state S120");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp98, "return of OpenPolicy, state S120");
            this.Manager.Comment("reaching state \'S156\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp99;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp100;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp100 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp99);
            this.Manager.Comment("reaching state \'S192\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp99, "accountHandle of CreateAccount, state S192");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp100, "return of CreateAccount, state S192");
            this.Manager.Comment("reaching state \'S228\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp101;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp102;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp102 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp101);
            this.Manager.Comment("reaching state \'S264\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp101, "trustHandle of CreateTrustedDomain, state S264");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp102, "return of CreateTrustedDomain, state S264");
            this.Manager.Comment("reaching state \'S300\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp103;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp104;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp104 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp103);
            this.Manager.Comment("reaching state \'S336\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp103, "securityDescriptor of QuerySecurityObject, state S336");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp104, "return of QuerySecurityObject, state S336");
            this.Manager.Comment("reaching state \'S372\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Invalid)\'");
            temp105 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S408\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp105, "return of SetSecurityObject, state S408");
            TestScenarioS22S437();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS22S437() {
            this.Manager.Comment("reaching state \'S437\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp106;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp107;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp107 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp106);
            this.Manager.Comment("reaching state \'S449\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp106, "handleOutput of DeleteObject, state S449");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp107, "return of DeleteObject, state S449");
            TestScenarioS22S458();
        }
        
        private void TestScenarioS22S458() {
            this.Manager.Comment("reaching state \'S458\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp108;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp109;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp109 = this.ILsadManagedAdapterInstance.Close(1, out temp108);
            this.Manager.Comment("reaching state \'S466\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp108, "handleAfterClose of Close, state S466");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp109, "return of Close, state S466");
            this.Manager.Comment("reaching state \'S474\'");
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S26() {
            this.Manager.BeginTest("TestScenarioS22S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S85\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp110;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp111;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp111 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp110);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp110, "policyHandle of OpenPolicy, state S121");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp111, "return of OpenPolicy, state S121");
            this.Manager.Comment("reaching state \'S157\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp112;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp113;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp113 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp112);
            this.Manager.Comment("reaching state \'S193\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp112, "accountHandle of CreateAccount, state S193");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp113, "return of CreateAccount, state S193");
            this.Manager.Comment("reaching state \'S229\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp114;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp115 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp114);
            this.Manager.Comment("reaching state \'S265\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp114, "trustHandle of CreateTrustedDomain, state S265");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp115, "return of CreateTrustedDomain, state S265");
            this.Manager.Comment("reaching state \'S301\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp116;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp117 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp116);
            this.Manager.Comment("reaching state \'S337\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp116, "securityDescriptor of QuerySecurityObject, state S337");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp117, "return of QuerySecurityObject, state S337");
            this.Manager.Comment("reaching state \'S373\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp118;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Invalid)\'");
            temp118 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S409\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp118, "return of SetSecurityObject, state S409");
            this.Manager.Comment("reaching state \'S438\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp119;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp120;
            this.Manager.Comment("executing step \'call DeleteObject(1,AccountObject,out _)\'");
            temp120 = this.ILsadManagedAdapterInstance.DeleteObject(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp119);
            this.Manager.Comment("reaching state \'S450\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp119, "handleOutput of DeleteObject, state S450");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp120, "return of DeleteObject, state S450");
            TestScenarioS22S458();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S28() {
            this.Manager.BeginTest("TestScenarioS22S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp121;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp122;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp122 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp121);
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp121, "policyHandle of OpenPolicy, state S122");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp122, "return of OpenPolicy, state S122");
            this.Manager.Comment("reaching state \'S158\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp123;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp124;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp124 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp123);
            this.Manager.Comment("reaching state \'S194\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp123, "accountHandle of CreateAccount, state S194");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp124, "return of CreateAccount, state S194");
            this.Manager.Comment("reaching state \'S230\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp125;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp126;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp126 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp125);
            this.Manager.Comment("reaching state \'S266\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp125, "trustHandle of CreateTrustedDomain, state S266");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp126, "return of CreateTrustedDomain, state S266");
            this.Manager.Comment("reaching state \'S302\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp127;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp128;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp128 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp127);
            this.Manager.Comment("reaching state \'S338\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp127, "securityDescriptor of QuerySecurityObject, state S338");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp128, "return of QuerySecurityObject, state S338");
            this.Manager.Comment("reaching state \'S374\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp129;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Null)\'");
            temp129 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S410\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp129, "return of SetSecurityObject, state S410");
            TestScenarioS22S437();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S30() {
            this.Manager.BeginTest("TestScenarioS22S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S87\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp130;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp131;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp131 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp130);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp130, "policyHandle of OpenPolicy, state S123");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp131, "return of OpenPolicy, state S123");
            this.Manager.Comment("reaching state \'S159\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp132;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp133;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp133 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp132);
            this.Manager.Comment("reaching state \'S195\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp132, "accountHandle of CreateAccount, state S195");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp133, "return of CreateAccount, state S195");
            this.Manager.Comment("reaching state \'S231\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp134;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp135;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp135 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp134);
            this.Manager.Comment("reaching state \'S267\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp134, "trustHandle of CreateTrustedDomain, state S267");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp135, "return of CreateTrustedDomain, state S267");
            this.Manager.Comment("reaching state \'S303\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp136;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp137;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp137 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp136);
            this.Manager.Comment("reaching state \'S339\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp136, "securityDescriptor of QuerySecurityObject, state S339");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp137, "return of QuerySecurityObject, state S339");
            this.Manager.Comment("reaching state \'S375\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp138;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Null)\'");
            temp138 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp138, "return of SetSecurityObject, state S411");
            TestScenarioS22S437();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S32() {
            this.Manager.BeginTest("TestScenarioS22S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp139;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp140;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp140 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp139);
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp139, "policyHandle of OpenPolicy, state S124");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp140, "return of OpenPolicy, state S124");
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp141;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp142;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp142 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp141);
            this.Manager.Comment("reaching state \'S196\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp141, "accountHandle of CreateAccount, state S196");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp142, "return of CreateAccount, state S196");
            this.Manager.Comment("reaching state \'S232\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp143;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp144;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp144 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp143);
            this.Manager.Comment("reaching state \'S268\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp143, "trustHandle of CreateTrustedDomain, state S268");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp144, "return of CreateTrustedDomain, state S268");
            this.Manager.Comment("reaching state \'S304\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp145;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp146;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,DACLSecurityInformation,out _)\'");
            temp146 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, out temp145);
            this.Manager.Comment("reaching state \'S340\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp145, "securityDescriptor of QuerySecurityObject, state S340");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp146, "return of QuerySecurityObject, state S340");
            this.Manager.Comment("reaching state \'S376\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp147;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Valid)\'");
            temp147 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp147, "return of SetSecurityObject, state S412");
            TestScenarioS22S437();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S34() {
            this.Manager.BeginTest("TestScenarioS22S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S89\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp148;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp149;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp149 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp148);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp148, "policyHandle of OpenPolicy, state S125");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp149, "return of OpenPolicy, state S125");
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp150;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp151;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp151 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp150);
            this.Manager.Comment("reaching state \'S197\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp150, "accountHandle of CreateAccount, state S197");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp151, "return of CreateAccount, state S197");
            this.Manager.Comment("reaching state \'S233\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp152;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp153;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp153 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp152);
            this.Manager.Comment("reaching state \'S269\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp152, "trustHandle of CreateTrustedDomain, state S269");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp153, "return of CreateTrustedDomain, state S269");
            this.Manager.Comment("reaching state \'S305\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp154;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp155;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp155 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp154);
            this.Manager.Comment("reaching state \'S341\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp154, "securityDescriptor of QuerySecurityObject, state S341");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp155, "return of QuerySecurityObject, state S341");
            this.Manager.Comment("reaching state \'S377\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp156;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Valid)\'");
            temp156 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S413\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp156, "return of SetSecurityObject, state S413");
            TestScenarioS22S439();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS22S439() {
            this.Manager.Comment("reaching state \'S439\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp157;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp158;
            this.Manager.Comment("executing step \'call DeleteObject(1,AccountObject,out _)\'");
            temp158 = this.ILsadManagedAdapterInstance.DeleteObject(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp157);
            this.Manager.Comment("reaching state \'S451\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp157, "handleOutput of DeleteObject, state S451");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp158, "return of DeleteObject, state S451");
            TestScenarioS22S461();
        }
        
        private void TestScenarioS22S461() {
            this.Manager.Comment("reaching state \'S461\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp159;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp160;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp160 = this.ILsadManagedAdapterInstance.Close(1, out temp159);
            this.Manager.Comment("reaching state \'S469\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp159, "handleAfterClose of Close, state S469");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp160, "return of Close, state S469");
            this.Manager.Comment("reaching state \'S477\'");
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S36() {
            this.Manager.BeginTest("TestScenarioS22S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp161;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp162;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp162 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp161);
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp161, "policyHandle of OpenPolicy, state S126");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp162, "return of OpenPolicy, state S126");
            this.Manager.Comment("reaching state \'S162\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp163;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp164;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp164 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp163);
            this.Manager.Comment("reaching state \'S198\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp163, "accountHandle of CreateAccount, state S198");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp164, "return of CreateAccount, state S198");
            this.Manager.Comment("reaching state \'S234\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp165;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp166;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp166 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp165);
            this.Manager.Comment("reaching state \'S270\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp165, "trustHandle of CreateTrustedDomain, state S270");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp166, "return of CreateTrustedDomain, state S270");
            this.Manager.Comment("reaching state \'S306\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp167;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp168;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp168 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp167);
            this.Manager.Comment("reaching state \'S342\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp167, "securityDescriptor of QuerySecurityObject, state S342");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp168, "return of QuerySecurityObject, state S342");
            this.Manager.Comment("reaching state \'S378\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp169;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Valid)\'");
            temp169 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S414\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp169, "return of SetSecurityObject, state S414");
            this.Manager.Comment("reaching state \'S440\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp170;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp171;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp171 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp170);
            this.Manager.Comment("reaching state \'S452\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp170, "handleOutput of DeleteObject, state S452");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp171, "return of DeleteObject, state S452");
            TestScenarioS22S461();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S38() {
            this.Manager.BeginTest("TestScenarioS22S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S91\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp172;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp173;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp173 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp172);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp172, "policyHandle of OpenPolicy, state S127");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp173, "return of OpenPolicy, state S127");
            this.Manager.Comment("reaching state \'S163\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp174;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp175;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp175 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp174);
            this.Manager.Comment("reaching state \'S199\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp174, "accountHandle of CreateAccount, state S199");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp175, "return of CreateAccount, state S199");
            this.Manager.Comment("reaching state \'S235\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp176;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp177;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp177 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp176);
            this.Manager.Comment("reaching state \'S271\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp176, "trustHandle of CreateTrustedDomain, state S271");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp177, "return of CreateTrustedDomain, state S271");
            this.Manager.Comment("reaching state \'S307\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp178;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp179;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp179 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp178);
            this.Manager.Comment("reaching state \'S343\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp178, "securityDescriptor of QuerySecurityObject, state S343");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp179, "return of QuerySecurityObject, state S343");
            this.Manager.Comment("reaching state \'S379\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp180;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Invalid)\'");
            temp180 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S415\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp180, "return of SetSecurityObject, state S415");
            TestScenarioS22S439();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S4() {
            this.Manager.BeginTest("TestScenarioS22S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp181;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp182;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp182 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp181);
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp181, "policyHandle of OpenPolicy, state S110");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp182, "return of OpenPolicy, state S110");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp183;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp184;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp184 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp183);
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp183, "accountHandle of CreateAccount, state S182");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp184, "return of CreateAccount, state S182");
            this.Manager.Comment("reaching state \'S218\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp185;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp186;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp186 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp185);
            this.Manager.Comment("reaching state \'S254\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp185, "trustHandle of CreateTrustedDomain, state S254");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp186, "return of CreateTrustedDomain, state S254");
            this.Manager.Comment("reaching state \'S290\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp187;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp188;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp188 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp187);
            this.Manager.Comment("reaching state \'S326\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp187, "securityDescriptor of QuerySecurityObject, state S326");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp188, "return of QuerySecurityObject, state S326");
            this.Manager.Comment("reaching state \'S362\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp189;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Valid)\'");
            temp189 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S398\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp189, "return of SetSecurityObject, state S398");
            this.Manager.Comment("reaching state \'S434\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp190;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp191;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp191 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp190);
            this.Manager.Comment("reaching state \'S446\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp190, "handleOutput of DeleteObject, state S446");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp191, "return of DeleteObject, state S446");
            TestScenarioS22S458();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S40() {
            this.Manager.BeginTest("TestScenarioS22S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp192;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp193;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp193 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp192);
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp192, "policyHandle of OpenPolicy, state S128");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp193, "return of OpenPolicy, state S128");
            this.Manager.Comment("reaching state \'S164\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp194;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp195;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp195 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp194);
            this.Manager.Comment("reaching state \'S200\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp194, "accountHandle of CreateAccount, state S200");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp195, "return of CreateAccount, state S200");
            this.Manager.Comment("reaching state \'S236\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp196;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp197;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp197 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp196);
            this.Manager.Comment("reaching state \'S272\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp196, "trustHandle of CreateTrustedDomain, state S272");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp197, "return of CreateTrustedDomain, state S272");
            this.Manager.Comment("reaching state \'S308\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp198;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp199;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp199 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp198);
            this.Manager.Comment("reaching state \'S344\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp198, "securityDescriptor of QuerySecurityObject, state S344");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp199, "return of QuerySecurityObject, state S344");
            this.Manager.Comment("reaching state \'S380\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp200;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Invalid)\'");
            temp200 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S416\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp200, "return of SetSecurityObject, state S416");
            TestScenarioS22S439();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S42() {
            this.Manager.BeginTest("TestScenarioS22S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S93\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp201;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp202;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp202 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp201);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp201, "policyHandle of OpenPolicy, state S129");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp202, "return of OpenPolicy, state S129");
            this.Manager.Comment("reaching state \'S165\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp203;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp204;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp204 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp203);
            this.Manager.Comment("reaching state \'S201\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp203, "accountHandle of CreateAccount, state S201");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp204, "return of CreateAccount, state S201");
            this.Manager.Comment("reaching state \'S237\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp205;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp206;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp206 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp205);
            this.Manager.Comment("reaching state \'S273\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp205, "trustHandle of CreateTrustedDomain, state S273");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp206, "return of CreateTrustedDomain, state S273");
            this.Manager.Comment("reaching state \'S309\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp207;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp208;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp208 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp207);
            this.Manager.Comment("reaching state \'S345\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp207, "securityDescriptor of QuerySecurityObject, state S345");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp208, "return of QuerySecurityObject, state S345");
            this.Manager.Comment("reaching state \'S381\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp209;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Null)\'");
            temp209 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S417\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp209, "return of SetSecurityObject, state S417");
            TestScenarioS22S439();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S44() {
            this.Manager.BeginTest("TestScenarioS22S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp210;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp211;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp211 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp210);
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp210, "policyHandle of OpenPolicy, state S130");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp211, "return of OpenPolicy, state S130");
            this.Manager.Comment("reaching state \'S166\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp212;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp213;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp213 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp212);
            this.Manager.Comment("reaching state \'S202\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp212, "accountHandle of CreateAccount, state S202");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp213, "return of CreateAccount, state S202");
            this.Manager.Comment("reaching state \'S238\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp214;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp215;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp215 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp214);
            this.Manager.Comment("reaching state \'S274\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp214, "trustHandle of CreateTrustedDomain, state S274");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp215, "return of CreateTrustedDomain, state S274");
            this.Manager.Comment("reaching state \'S310\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp216;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp217;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp217 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp216);
            this.Manager.Comment("reaching state \'S346\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp216, "securityDescriptor of QuerySecurityObject, state S346");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp217, "return of QuerySecurityObject, state S346");
            this.Manager.Comment("reaching state \'S382\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp218;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Valid)\'");
            temp218 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S418\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp218, "return of SetSecurityObject, state S418");
            TestScenarioS22S441();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS22S441() {
            this.Manager.Comment("reaching state \'S441\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp219;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp220;
            this.Manager.Comment("executing step \'call DeleteObject(1,AccountObject,out _)\'");
            temp220 = this.ILsadManagedAdapterInstance.DeleteObject(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp219);
            this.Manager.Comment("reaching state \'S453\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp219, "handleOutput of DeleteObject, state S453");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp220, "return of DeleteObject, state S453");
            TestScenarioS22S462();
        }
        
        private void TestScenarioS22S462() {
            this.Manager.Comment("reaching state \'S462\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp221;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp222;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp222 = this.ILsadManagedAdapterInstance.Close(1, out temp221);
            this.Manager.Comment("reaching state \'S470\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp221, "handleAfterClose of Close, state S470");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp222, "return of Close, state S470");
            this.Manager.Comment("reaching state \'S478\'");
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S46() {
            this.Manager.BeginTest("TestScenarioS22S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S95\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp223;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp224;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp224 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp223);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp223, "policyHandle of OpenPolicy, state S131");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp224, "return of OpenPolicy, state S131");
            this.Manager.Comment("reaching state \'S167\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp225;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp226;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp226 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp225);
            this.Manager.Comment("reaching state \'S203\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp225, "accountHandle of CreateAccount, state S203");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp226, "return of CreateAccount, state S203");
            this.Manager.Comment("reaching state \'S239\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp227;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp228;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp228 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp227);
            this.Manager.Comment("reaching state \'S275\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp227, "trustHandle of CreateTrustedDomain, state S275");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp228, "return of CreateTrustedDomain, state S275");
            this.Manager.Comment("reaching state \'S311\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp229;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp230;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp230 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp229);
            this.Manager.Comment("reaching state \'S347\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp229, "securityDescriptor of QuerySecurityObject, state S347");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp230, "return of QuerySecurityObject, state S347");
            this.Manager.Comment("reaching state \'S383\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp231;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Valid)\'");
            temp231 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S419\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp231, "return of SetSecurityObject, state S419");
            this.Manager.Comment("reaching state \'S442\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp232;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp233;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp233 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp232);
            this.Manager.Comment("reaching state \'S454\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp232, "handleOutput of DeleteObject, state S454");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp233, "return of DeleteObject, state S454");
            TestScenarioS22S462();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S48() {
            this.Manager.BeginTest("TestScenarioS22S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp234;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp235;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp235 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp234);
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp234, "policyHandle of OpenPolicy, state S132");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp235, "return of OpenPolicy, state S132");
            this.Manager.Comment("reaching state \'S168\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp236;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp237;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp237 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp236);
            this.Manager.Comment("reaching state \'S204\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp236, "accountHandle of CreateAccount, state S204");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp237, "return of CreateAccount, state S204");
            this.Manager.Comment("reaching state \'S240\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp238;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp239;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp239 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp238);
            this.Manager.Comment("reaching state \'S276\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp238, "trustHandle of CreateTrustedDomain, state S276");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp239, "return of CreateTrustedDomain, state S276");
            this.Manager.Comment("reaching state \'S312\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp240;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp241;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp241 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp240);
            this.Manager.Comment("reaching state \'S348\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp240, "securityDescriptor of QuerySecurityObject, state S348");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp241, "return of QuerySecurityObject, state S348");
            this.Manager.Comment("reaching state \'S384\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp242;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Invalid)\'");
            temp242 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S420\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp242, "return of SetSecurityObject, state S420");
            TestScenarioS22S441();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S50() {
            this.Manager.BeginTest("TestScenarioS22S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S97\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp243;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp244;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp244 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp243);
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp243, "policyHandle of OpenPolicy, state S133");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp244, "return of OpenPolicy, state S133");
            this.Manager.Comment("reaching state \'S169\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp245;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp246;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp246 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp245);
            this.Manager.Comment("reaching state \'S205\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp245, "accountHandle of CreateAccount, state S205");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp246, "return of CreateAccount, state S205");
            this.Manager.Comment("reaching state \'S241\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp247;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp248;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp248 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp247);
            this.Manager.Comment("reaching state \'S277\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp247, "trustHandle of CreateTrustedDomain, state S277");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp248, "return of CreateTrustedDomain, state S277");
            this.Manager.Comment("reaching state \'S313\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp249;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp250;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp250 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp249);
            this.Manager.Comment("reaching state \'S349\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp249, "securityDescriptor of QuerySecurityObject, state S349");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp250, "return of QuerySecurityObject, state S349");
            this.Manager.Comment("reaching state \'S385\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp251;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Null)\'");
            temp251 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S421\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp251, "return of SetSecurityObject, state S421");
            TestScenarioS22S441();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S52() {
            this.Manager.BeginTest("TestScenarioS22S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp252;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp253;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp253 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp252);
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp252, "policyHandle of OpenPolicy, state S134");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp253, "return of OpenPolicy, state S134");
            this.Manager.Comment("reaching state \'S170\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp254;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp255;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp255 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp254);
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp254, "accountHandle of CreateAccount, state S206");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp255, "return of CreateAccount, state S206");
            this.Manager.Comment("reaching state \'S242\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp256;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp257;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp257 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp256);
            this.Manager.Comment("reaching state \'S278\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp256, "trustHandle of CreateTrustedDomain, state S278");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp257, "return of CreateTrustedDomain, state S278");
            this.Manager.Comment("reaching state \'S314\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp258;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp259;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,DACLSecurityInformation,out _)\'");
            temp259 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, out temp258);
            this.Manager.Comment("reaching state \'S350\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp258, "securityDescriptor of QuerySecurityObject, state S350");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp259, "return of QuerySecurityObject, state S350");
            this.Manager.Comment("reaching state \'S386\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp260;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Invalid)\'");
            temp260 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S422\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp260, "return of SetSecurityObject, state S422");
            TestScenarioS22S441();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S54() {
            this.Manager.BeginTest("TestScenarioS22S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S99\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp261;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp262;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp262 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp261);
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp261, "policyHandle of OpenPolicy, state S135");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp262, "return of OpenPolicy, state S135");
            this.Manager.Comment("reaching state \'S171\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp263;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp264;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp264 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp263);
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp263, "accountHandle of CreateAccount, state S207");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp264, "return of CreateAccount, state S207");
            this.Manager.Comment("reaching state \'S243\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp265;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp266;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp266 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp265);
            this.Manager.Comment("reaching state \'S279\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp265, "trustHandle of CreateTrustedDomain, state S279");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp266, "return of CreateTrustedDomain, state S279");
            this.Manager.Comment("reaching state \'S315\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp267;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp268;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,DACLSecurityInformation,out _)\'");
            temp268 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, out temp267);
            this.Manager.Comment("reaching state \'S351\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp267, "securityDescriptor of QuerySecurityObject, state S351");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp268, "return of QuerySecurityObject, state S351");
            this.Manager.Comment("reaching state \'S387\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp269;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Null)\'");
            temp269 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S423\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp269, "return of SetSecurityObject, state S423");
            TestScenarioS22S439();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S56() {
            this.Manager.BeginTest("TestScenarioS22S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp270;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp271;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp271 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp270);
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp270, "policyHandle of OpenPolicy, state S136");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp271, "return of OpenPolicy, state S136");
            this.Manager.Comment("reaching state \'S172\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp272;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp273;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp273 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp272);
            this.Manager.Comment("reaching state \'S208\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp272, "accountHandle of CreateAccount, state S208");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp273, "return of CreateAccount, state S208");
            this.Manager.Comment("reaching state \'S244\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp274;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp275;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp275 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp274);
            this.Manager.Comment("reaching state \'S280\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp274, "trustHandle of CreateTrustedDomain, state S280");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp275, "return of CreateTrustedDomain, state S280");
            this.Manager.Comment("reaching state \'S316\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp276;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp277;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp277 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp276);
            this.Manager.Comment("reaching state \'S352\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp276, "securityDescriptor of QuerySecurityObject, state S352");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp277, "return of QuerySecurityObject, state S352");
            this.Manager.Comment("reaching state \'S388\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp278;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Valid)\'");
            temp278 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S424\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp278, "return of SetSecurityObject, state S424");
            this.Manager.Comment("reaching state \'S443\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp279;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp280;
            this.Manager.Comment("executing step \'call DeleteObject(2,AccountObject,out _)\'");
            temp280 = this.ILsadManagedAdapterInstance.DeleteObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum)(0)), out temp279);
            this.Manager.Comment("reaching state \'S455\'");
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp279, "handleOutput of DeleteObject, state S455");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp280, "return of DeleteObject, state S455");
            this.Manager.Comment("reaching state \'S463\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp281;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp282;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp282 = this.ILsadManagedAdapterInstance.Close(1, out temp281);
            this.Manager.Comment("reaching state \'S471\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp281, "handleAfterClose of Close, state S471");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp282, "return of Close, state S471");
            this.Manager.Comment("reaching state \'S479\'");
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S58() {
            this.Manager.BeginTest("TestScenarioS22S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S101\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp283;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp284;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp284 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp283);
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp283, "policyHandle of OpenPolicy, state S137");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp284, "return of OpenPolicy, state S137");
            this.Manager.Comment("reaching state \'S173\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp285;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp286;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp286 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp285);
            this.Manager.Comment("reaching state \'S209\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp285, "accountHandle of CreateAccount, state S209");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp286, "return of CreateAccount, state S209");
            this.Manager.Comment("reaching state \'S245\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp287;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp288;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp288 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp287);
            this.Manager.Comment("reaching state \'S281\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp287, "trustHandle of CreateTrustedDomain, state S281");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp288, "return of CreateTrustedDomain, state S281");
            this.Manager.Comment("reaching state \'S317\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp289;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp290;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp290 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp289);
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp289, "securityDescriptor of QuerySecurityObject, state S353");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp290, "return of QuerySecurityObject, state S353");
            this.Manager.Comment("reaching state \'S389\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp291;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Null)\'");
            temp291 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S425\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp291, "return of SetSecurityObject, state S425");
            TestScenarioS22S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S6() {
            this.Manager.BeginTest("TestScenarioS22S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S75\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp292;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp293;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp293 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp292);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp292, "policyHandle of OpenPolicy, state S111");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp293, "return of OpenPolicy, state S111");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp294;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp295;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp295 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp294);
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp294, "accountHandle of CreateAccount, state S183");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp295, "return of CreateAccount, state S183");
            this.Manager.Comment("reaching state \'S219\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp296;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp297;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp297 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp296);
            this.Manager.Comment("reaching state \'S255\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp296, "trustHandle of CreateTrustedDomain, state S255");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp297, "return of CreateTrustedDomain, state S255");
            this.Manager.Comment("reaching state \'S291\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp298;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp299;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp299 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp298);
            this.Manager.Comment("reaching state \'S327\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp298, "securityDescriptor of QuerySecurityObject, state S327");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp299, "return of QuerySecurityObject, state S327");
            this.Manager.Comment("reaching state \'S363\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp300;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Invalid)\'");
            temp300 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S399\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp300, "return of SetSecurityObject, state S399");
            TestScenarioS22S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S60() {
            this.Manager.BeginTest("TestScenarioS22S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp301;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp302;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp302 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp301);
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp301, "policyHandle of OpenPolicy, state S138");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp302, "return of OpenPolicy, state S138");
            this.Manager.Comment("reaching state \'S174\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp303;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp304;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp304 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp303);
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp303, "accountHandle of CreateAccount, state S210");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp304, "return of CreateAccount, state S210");
            this.Manager.Comment("reaching state \'S246\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp305;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp306;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp306 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp305);
            this.Manager.Comment("reaching state \'S282\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp305, "trustHandle of CreateTrustedDomain, state S282");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp306, "return of CreateTrustedDomain, state S282");
            this.Manager.Comment("reaching state \'S318\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp307;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp308;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp308 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp307);
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp307, "securityDescriptor of QuerySecurityObject, state S354");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp308, "return of QuerySecurityObject, state S354");
            this.Manager.Comment("reaching state \'S390\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp309;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,OwnerSecurityInformation,Invalid)\'");
            temp309 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S426\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp309, "return of SetSecurityObject, state S426");
            TestScenarioS22S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S62() {
            this.Manager.BeginTest("TestScenarioS22S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S103\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp310;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp311;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp311 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp310);
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp310, "policyHandle of OpenPolicy, state S139");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp311, "return of OpenPolicy, state S139");
            this.Manager.Comment("reaching state \'S175\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp312;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp313;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp313 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp312);
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp312, "accountHandle of CreateAccount, state S211");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp313, "return of CreateAccount, state S211");
            this.Manager.Comment("reaching state \'S247\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp314;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp315;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp315 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp314);
            this.Manager.Comment("reaching state \'S283\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp314, "trustHandle of CreateTrustedDomain, state S283");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp315, "return of CreateTrustedDomain, state S283");
            this.Manager.Comment("reaching state \'S319\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp316;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp317;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp317 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp316);
            this.Manager.Comment("reaching state \'S355\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp316, "securityDescriptor of QuerySecurityObject, state S355");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp317, "return of QuerySecurityObject, state S355");
            this.Manager.Comment("reaching state \'S391\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp318;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Invalid)\'");
            temp318 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S427\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp318, "return of SetSecurityObject, state S427");
            TestScenarioS22S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S64() {
            this.Manager.BeginTest("TestScenarioS22S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp319;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp320;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp320 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp319);
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp319, "policyHandle of OpenPolicy, state S140");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp320, "return of OpenPolicy, state S140");
            this.Manager.Comment("reaching state \'S176\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp321;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp322;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp322 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp321);
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp321, "accountHandle of CreateAccount, state S212");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp322, "return of CreateAccount, state S212");
            this.Manager.Comment("reaching state \'S248\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp323;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp324;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp324 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp323);
            this.Manager.Comment("reaching state \'S284\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp323, "trustHandle of CreateTrustedDomain, state S284");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp324, "return of CreateTrustedDomain, state S284");
            this.Manager.Comment("reaching state \'S320\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp325;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp326;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp326 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp325);
            this.Manager.Comment("reaching state \'S356\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp325, "securityDescriptor of QuerySecurityObject, state S356");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp326, "return of QuerySecurityObject, state S356");
            this.Manager.Comment("reaching state \'S392\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp327;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Null)\'");
            temp327 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S428\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp327, "return of SetSecurityObject, state S428");
            TestScenarioS22S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S66() {
            this.Manager.BeginTest("TestScenarioS22S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S105\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp328;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp329;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp329 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp328);
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp328, "policyHandle of OpenPolicy, state S141");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp329, "return of OpenPolicy, state S141");
            this.Manager.Comment("reaching state \'S177\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp330;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp331;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp331 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp330);
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp330, "accountHandle of CreateAccount, state S213");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp331, "return of CreateAccount, state S213");
            this.Manager.Comment("reaching state \'S249\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp332;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp333;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp333 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp332);
            this.Manager.Comment("reaching state \'S285\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp332, "trustHandle of CreateTrustedDomain, state S285");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp333, "return of CreateTrustedDomain, state S285");
            this.Manager.Comment("reaching state \'S321\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp334;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp335;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp335 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp334);
            this.Manager.Comment("reaching state \'S357\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp334, "securityDescriptor of QuerySecurityObject, state S357");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp335, "return of QuerySecurityObject, state S357");
            this.Manager.Comment("reaching state \'S393\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp336;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Null)\'");
            temp336 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S429\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp336, "return of SetSecurityObject, state S429");
            TestScenarioS22S435();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S68() {
            this.Manager.BeginTest("TestScenarioS22S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp337;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp338;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp338 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp337);
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp337, "policyHandle of OpenPolicy, state S142");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp338, "return of OpenPolicy, state S142");
            this.Manager.Comment("reaching state \'S178\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp339;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp340;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Invalid,\"SID\",out _)\'");
            temp340 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(1)), "SID", out temp339);
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp339, "accountHandle of CreateAccount, state S214");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp340, "return of CreateAccount, state S214");
            this.Manager.Comment("reaching state \'S250\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp341;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp342;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp342 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp341);
            this.Manager.Comment("reaching state \'S286\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp341, "trustHandle of CreateTrustedDomain, state S286");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp342, "return of CreateTrustedDomain, state S286");
            this.Manager.Comment("reaching state \'S322\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp343;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp344;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp344 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp343);
            this.Manager.Comment("reaching state \'S358\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp343, "securityDescriptor of QuerySecurityObject, state S358");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp344, "return of QuerySecurityObject, state S358");
            this.Manager.Comment("reaching state \'S394\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp345;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Valid)\'");
            temp345 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)));
            this.Manager.Comment("reaching state \'S430\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp345, "return of SetSecurityObject, state S430");
            TestScenarioS22S437();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S70() {
            this.Manager.BeginTest("TestScenarioS22S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S107\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp346;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp347;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp347 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp346);
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp346, "policyHandle of OpenPolicy, state S143");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp347, "return of OpenPolicy, state S143");
            this.Manager.Comment("reaching state \'S179\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp348;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp349;
            this.Manager.Comment("executing step \'call CreateAccount(1,0,Valid,\"SID\",out _)\'");
            temp349 = this.ILsadManagedAdapterInstance.CreateAccount(1, 0u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp348);
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp348, "accountHandle of CreateAccount, state S215");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp349, "return of CreateAccount, state S215");
            this.Manager.Comment("reaching state \'S251\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp350;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp351;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp351 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp350);
            this.Manager.Comment("reaching state \'S287\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp350, "trustHandle of CreateTrustedDomain, state S287");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp351, "return of CreateTrustedDomain, state S287");
            this.Manager.Comment("reaching state \'S323\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp352;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp353;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp353 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp352);
            this.Manager.Comment("reaching state \'S359\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)), temp352, "securityDescriptor of QuerySecurityObject, state S359");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp353, "return of QuerySecurityObject, state S359");
            this.Manager.Comment("reaching state \'S395\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp354;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Null)\'");
            temp354 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor.Null);
            this.Manager.Comment("reaching state \'S431\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp354, "return of SetSecurityObject, state S431");
            TestScenarioS22S441();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS22S8() {
            this.Manager.BeginTest("TestScenarioS22S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp355;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp356;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,16,out _)\'");
            temp356 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 16u, out temp355);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp355, "policyHandle of OpenPolicy, state S112");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp356, "return of OpenPolicy, state S112");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp357;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp358;
            this.Manager.Comment("executing step \'call CreateAccount(1,4061069327,Valid,\"SID\",out _)\'");
            temp358 = this.ILsadManagedAdapterInstance.CreateAccount(1, 4061069327u, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AccountSid)(0)), "SID", out temp357);
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("checking step \'return CreateAccount/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp357, "accountHandle of CreateAccount, state S184");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp358, "return of CreateAccount, state S184");
            this.Manager.Comment("reaching state \'S220\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp359;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp360;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp360 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp359);
            this.Manager.Comment("reaching state \'S256\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp359, "trustHandle of CreateTrustedDomain, state S256");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp360, "return of CreateTrustedDomain, state S256");
            this.Manager.Comment("reaching state \'S292\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor temp361;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp362;
            this.Manager.Comment("executing step \'call QuerySecurityObject(2,OwnerSecurityInformation,out _)\'");
            temp362 = this.ILsadManagedAdapterInstance.QuerySecurityObject(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo)(1)), out temp361);
            this.Manager.Comment("reaching state \'S328\'");
            this.Manager.Comment("checking step \'return QuerySecurityObject/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(0)), temp361, "securityDescriptor of QuerySecurityObject, state S328");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp362, "return of QuerySecurityObject, state S328");
            this.Manager.Comment("reaching state \'S364\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp363;
            this.Manager.Comment("executing step \'call SetSecurityObject(2,DACLSecurityInformation,Invalid)\'");
            temp363 = this.ILsadManagedAdapterInstance.SetSecurityObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityInfo.DACLSecurityInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecurityDescriptor)(1)));
            this.Manager.Comment("reaching state \'S400\'");
            this.Manager.Comment("checking step \'return SetSecurityObject/InvalidSecurityDescr\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSecurityDescr, temp363, "return of SetSecurityObject, state S400");
            TestScenarioS22S433();
            this.Manager.EndTest();
        }
        #endregion
    }
}
