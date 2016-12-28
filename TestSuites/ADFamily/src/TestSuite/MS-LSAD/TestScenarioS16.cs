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
    public partial class TestScenarioS16 : PtfTestClassBase {
        
        public TestScenarioS16() {
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
        public void LSAD_TestScenarioS16S0() {
            this.Manager.BeginTest("TestScenarioS16S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S216\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S324\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S324");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S324");
            this.Manager.Comment("reaching state \'S432\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Invalid,True,Invalid,65663,out _)\'");
            temp3 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(1)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp2);
            this.Manager.Comment("reaching state \'S540\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp2, "trustHandle of CreateTrustedDomain, state S540");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp3, "return of CreateTrustedDomain, state S540");
            this.Manager.Comment("reaching state \'S648\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformationInternal,out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, out temp4);
            this.Manager.Comment("reaching state \'S756\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp4, "trustDomainInfo of QueryTrustedDomainInfoByName, state S756");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp5, "return of QueryTrustedDomainInfoByName, state S756");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS16S864() {
            this.Manager.Comment("reaching state \'S864\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp6 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.Comment("reaching state \'S866\'");
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/NoSuchDomain\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchDomain, temp6, "return of DeleteTrustedDomain, state S866");
            this.Manager.Comment("reaching state \'S868\'");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S10() {
            this.Manager.BeginTest("TestScenarioS16S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S221\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp8 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp7);
            this.Manager.Comment("reaching state \'S329\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp7, "policyHandle of OpenPolicy2, state S329");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp8, "return of OpenPolicy2, state S329");
            this.Manager.Comment("reaching state \'S437\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp9;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp10 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp9);
            this.Manager.Comment("reaching state \'S545\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp9, "trustHandle of CreateTrustedDomain, state S545");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp10, "return of CreateTrustedDomain, state S545");
            this.Manager.Comment("reaching state \'S653\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp11;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp12;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainN" +
                    "ameInformation,out _)\'");
            temp12 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), out temp11);
            this.Manager.Comment("reaching state \'S761\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp11, "trustDomainInfo of QueryTrustedDomainInfoByName, state S761");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp12, "return of QueryTrustedDomainInfoByName, state S761");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS16S865() {
            this.Manager.Comment("reaching state \'S865\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp13 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.Comment("reaching state \'S867\'");
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp13, "return of DeleteTrustedDomain, state S867");
            this.Manager.Comment("reaching state \'S869\'");
        }
        #endregion
        
        #region Test Starting in S100
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S100() {
            this.Manager.BeginTest("TestScenarioS16S100");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S266\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp14);
            this.Manager.Comment("reaching state \'S374\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp14, "policyHandle of OpenPolicy2, state S374");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp15, "return of OpenPolicy2, state S374");
            this.Manager.Comment("reaching state \'S482\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp16);
            this.Manager.Comment("reaching state \'S590\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp16, "trustHandle of CreateTrustedDomain, state S590");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of CreateTrustedDomain, state S590");
            this.Manager.Comment("reaching state \'S698\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nAuthInformation,out _)\'");
            temp19 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, out temp18);
            this.Manager.Comment("reaching state \'S806\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp18, "trustDomainInfo of QueryTrustedDomainInfoByName, state S806");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp19, "return of QueryTrustedDomainInfoByName, state S806");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S102
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S102() {
            this.Manager.BeginTest("TestScenarioS16S102");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S267\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp21 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp20);
            this.Manager.Comment("reaching state \'S375\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp20, "policyHandle of OpenPolicy2, state S375");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp21, "return of OpenPolicy2, state S375");
            this.Manager.Comment("reaching state \'S483\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp22);
            this.Manager.Comment("reaching state \'S591\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp22, "trustHandle of CreateTrustedDomain, state S591");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp23, "return of CreateTrustedDomain, state S591");
            this.Manager.Comment("reaching state \'S699\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,Invalid,out _)" +
                    "\'");
            temp25 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, out temp24);
            this.Manager.Comment("reaching state \'S807\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp24, "trustDomainInfo of QueryTrustedDomainInfoByName, state S807");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp25, "return of QueryTrustedDomainInfoByName, state S807");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S104
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S104() {
            this.Manager.BeginTest("TestScenarioS16S104");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S268\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp26);
            this.Manager.Comment("reaching state \'S376\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy2, state S376");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy2, state S376");
            this.Manager.Comment("reaching state \'S484\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp28);
            this.Manager.Comment("reaching state \'S592\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp28, "trustHandle of CreateTrustedDomain, state S592");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp29, "return of CreateTrustedDomain, state S592");
            this.Manager.Comment("reaching state \'S700\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainN" +
                    "ameInformation,out _)\'");
            temp31 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), out temp30);
            this.Manager.Comment("reaching state \'S808\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ObjectNameNotFou" +
                    "nd\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp30, "trustDomainInfo of QueryTrustedDomainInfoByName, state S808");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp31, "return of QueryTrustedDomainInfoByName, state S808");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S106
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S106() {
            this.Manager.BeginTest("TestScenarioS16S106");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S269\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp33 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp32);
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp32, "policyHandle of OpenPolicy2, state S377");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp33, "return of OpenPolicy2, state S377");
            this.Manager.Comment("reaching state \'S485\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp35 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp34);
            this.Manager.Comment("reaching state \'S593\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp34, "trustHandle of CreateTrustedDomain, state S593");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp35, "return of CreateTrustedDomain, state S593");
            this.Manager.Comment("reaching state \'S701\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedPosixOf" +
                    "fsetInformation,out _)\'");
            temp37 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, out temp36);
            this.Manager.Comment("reaching state \'S809\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ObjectNameNotFou" +
                    "nd\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp36, "trustDomainInfo of QueryTrustedDomainInfoByName, state S809");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp37, "return of QueryTrustedDomainInfoByName, state S809");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S108
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S108() {
            this.Manager.BeginTest("TestScenarioS16S108");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S270\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp38);
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp38, "policyHandle of OpenPolicy2, state S378");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp39, "return of OpenPolicy2, state S378");
            this.Manager.Comment("reaching state \'S486\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp40);
            this.Manager.Comment("reaching state \'S594\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp40, "trustHandle of CreateTrustedDomain, state S594");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp41, "return of CreateTrustedDomain, state S594");
            this.Manager.Comment("reaching state \'S702\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainI" +
                    "nformationEx,out _)\'");
            temp43 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, out temp42);
            this.Manager.Comment("reaching state \'S810\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ObjectNameNotFou" +
                    "nd\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp42, "trustDomainInfo of QueryTrustedDomainInfoByName, state S810");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp43, "return of QueryTrustedDomainInfoByName, state S810");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S110
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S110() {
            this.Manager.BeginTest("TestScenarioS16S110");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S271\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp44;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp45 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp44);
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp44, "policyHandle of OpenPolicy2, state S379");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp45, "return of OpenPolicy2, state S379");
            this.Manager.Comment("reaching state \'S487\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp46;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp47 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp46);
            this.Manager.Comment("reaching state \'S595\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp46, "trustHandle of CreateTrustedDomain, state S595");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp47, "return of CreateTrustedDomain, state S595");
            this.Manager.Comment("reaching state \'S703\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nAuthInformation,out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, out temp48);
            this.Manager.Comment("reaching state \'S811\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp48, "trustDomainInfo of QueryTrustedDomainInfoByName, state S811");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp49, "return of QueryTrustedDomainInfoByName, state S811");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S112
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S112() {
            this.Manager.BeginTest("TestScenarioS16S112");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S272\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp50;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp51 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp50);
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp50, "policyHandle of OpenPolicy2, state S380");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp51, "return of OpenPolicy2, state S380");
            this.Manager.Comment("reaching state \'S488\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp53 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp52);
            this.Manager.Comment("reaching state \'S596\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp52, "trustHandle of CreateTrustedDomain, state S596");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp53, "return of CreateTrustedDomain, state S596");
            this.Manager.Comment("reaching state \'S704\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nSupportedEncryptionTypes,out _)\'");
            temp55 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, out temp54);
            this.Manager.Comment("reaching state \'S812\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp54, "trustDomainInfo of QueryTrustedDomainInfoByName, state S812");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp55, "return of QueryTrustedDomainInfoByName, state S812");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S114
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S114() {
            this.Manager.BeginTest("TestScenarioS16S114");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S273\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp56;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp57;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp57 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp56);
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp56, "policyHandle of OpenPolicy2, state S381");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp57, "return of OpenPolicy2, state S381");
            this.Manager.Comment("reaching state \'S489\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp58;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp59 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp58);
            this.Manager.Comment("reaching state \'S597\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp58, "trustHandle of CreateTrustedDomain, state S597");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp59, "return of CreateTrustedDomain, state S597");
            this.Manager.Comment("reaching state \'S705\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp60;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nAuthInformationInternal,out _)\'");
            temp61 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, out temp60);
            this.Manager.Comment("reaching state \'S813\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp60, "trustDomainInfo of QueryTrustedDomainInfoByName, state S813");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp61, "return of QueryTrustedDomainInfoByName, state S813");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S116
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S116() {
            this.Manager.BeginTest("TestScenarioS16S116");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S274\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp63 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp62);
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp62, "policyHandle of OpenPolicy2, state S382");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp63, "return of OpenPolicy2, state S382");
            this.Manager.Comment("reaching state \'S490\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp64;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp65;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp65 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp64);
            this.Manager.Comment("reaching state \'S598\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp64, "trustHandle of CreateTrustedDomain, state S598");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp65, "return of CreateTrustedDomain, state S598");
            this.Manager.Comment("reaching state \'S706\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp66;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainI" +
                    "nformationEx2Internal,out _)\'");
            temp67 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, out temp66);
            this.Manager.Comment("reaching state \'S814\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp66, "trustDomainInfo of QueryTrustedDomainInfoByName, state S814");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp67, "return of QueryTrustedDomainInfoByName, state S814");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S118
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S118() {
            this.Manager.BeginTest("TestScenarioS16S118");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S275\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp68);
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp68, "policyHandle of OpenPolicy2, state S383");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp69, "return of OpenPolicy2, state S383");
            this.Manager.Comment("reaching state \'S491\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp71 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp70);
            this.Manager.Comment("reaching state \'S599\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp70, "trustHandle of CreateTrustedDomain, state S599");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp71, "return of CreateTrustedDomain, state S599");
            this.Manager.Comment("reaching state \'S707\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp72;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedPosixOf" +
                    "fsetInformation,out _)\'");
            temp73 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, out temp72);
            this.Manager.Comment("reaching state \'S815\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp72, "trustDomainInfo of QueryTrustedDomainInfoByName, state S815");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp73, "return of QueryTrustedDomainInfoByName, state S815");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S12() {
            this.Manager.BeginTest("TestScenarioS16S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S222\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp74;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp75;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp75 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp74);
            this.Manager.Comment("reaching state \'S330\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp74, "policyHandle of OpenPolicy2, state S330");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp75, "return of OpenPolicy2, state S330");
            this.Manager.Comment("reaching state \'S438\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp76;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp77 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp76);
            this.Manager.Comment("reaching state \'S546\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp76, "trustHandle of CreateTrustedDomain, state S546");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp77, "return of CreateTrustedDomain, state S546");
            this.Manager.Comment("reaching state \'S654\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp78;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainI" +
                    "nformationEx,out _)\'");
            temp79 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, out temp78);
            this.Manager.Comment("reaching state \'S762\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp78, "trustDomainInfo of QueryTrustedDomainInfoByName, state S762");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp79, "return of QueryTrustedDomainInfoByName, state S762");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S120
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S120() {
            this.Manager.BeginTest("TestScenarioS16S120");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S276\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp80;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp81 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp80);
            this.Manager.Comment("reaching state \'S384\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp80, "policyHandle of OpenPolicy2, state S384");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp81, "return of OpenPolicy2, state S384");
            this.Manager.Comment("reaching state \'S492\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp82;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp83;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp83 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp82);
            this.Manager.Comment("reaching state \'S600\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp82, "trustHandle of CreateTrustedDomain, state S600");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp83, "return of CreateTrustedDomain, state S600");
            this.Manager.Comment("reaching state \'S708\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp84;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainS" +
                    "upportedEncryptionTypes,out _)\'");
            temp85 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, out temp84);
            this.Manager.Comment("reaching state \'S816\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp84, "trustDomainInfo of QueryTrustedDomainInfoByName, state S816");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp85, "return of QueryTrustedDomainInfoByName, state S816");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S122
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S122() {
            this.Manager.BeginTest("TestScenarioS16S122");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S277\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp86;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp87 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp86);
            this.Manager.Comment("reaching state \'S385\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp86, "policyHandle of OpenPolicy2, state S385");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp87, "return of OpenPolicy2, state S385");
            this.Manager.Comment("reaching state \'S493\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp88;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp89 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp88);
            this.Manager.Comment("reaching state \'S601\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp88, "trustHandle of CreateTrustedDomain, state S601");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp89, "return of CreateTrustedDomain, state S601");
            this.Manager.Comment("reaching state \'S709\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp90;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp91;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainA" +
                    "uthInformationInternal,out _)\'");
            temp91 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, out temp90);
            this.Manager.Comment("reaching state \'S817\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp90, "trustDomainInfo of QueryTrustedDomainInfoByName, state S817");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp91, "return of QueryTrustedDomainInfoByName, state S817");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S124
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S124() {
            this.Manager.BeginTest("TestScenarioS16S124");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S278\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp92;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp93;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp93 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp92);
            this.Manager.Comment("reaching state \'S386\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp92, "policyHandle of OpenPolicy2, state S386");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp93, "return of OpenPolicy2, state S386");
            this.Manager.Comment("reaching state \'S494\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp94;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp95;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp95 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp94);
            this.Manager.Comment("reaching state \'S602\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp94, "trustHandle of CreateTrustedDomain, state S602");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp95, "return of CreateTrustedDomain, state S602");
            this.Manager.Comment("reaching state \'S710\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp96;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainI" +
                    "nformationBasic,out _)\'");
            temp97 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, out temp96);
            this.Manager.Comment("reaching state \'S818\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp96, "trustDomainInfo of QueryTrustedDomainInfoByName, state S818");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp97, "return of QueryTrustedDomainInfoByName, state S818");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S126
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S126() {
            this.Manager.BeginTest("TestScenarioS16S126");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S279\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp98;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp99;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp99 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp98);
            this.Manager.Comment("reaching state \'S387\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp98, "policyHandle of OpenPolicy2, state S387");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp99, "return of OpenPolicy2, state S387");
            this.Manager.Comment("reaching state \'S495\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp100;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp101 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp100);
            this.Manager.Comment("reaching state \'S603\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp100, "trustHandle of CreateTrustedDomain, state S603");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp101, "return of CreateTrustedDomain, state S603");
            this.Manager.Comment("reaching state \'S711\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp102;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationBasic,out _)\'");
            temp103 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, out temp102);
            this.Manager.Comment("reaching state \'S819\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp102, "trustDomainInfo of QueryTrustedDomainInfoByName, state S819");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp103, "return of QueryTrustedDomainInfoByName, state S819");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S128
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S128() {
            this.Manager.BeginTest("TestScenarioS16S128");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S280\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp104;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp105 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp104);
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp104, "policyHandle of OpenPolicy2, state S388");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp105, "return of OpenPolicy2, state S388");
            this.Manager.Comment("reaching state \'S496\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp106;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp107;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp107 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp106);
            this.Manager.Comment("reaching state \'S604\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp106, "trustHandle of CreateTrustedDomain, state S604");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp107, "return of CreateTrustedDomain, state S604");
            this.Manager.Comment("reaching state \'S712\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp108;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp109;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationBasic,out _)\'");
            temp109 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, out temp108);
            this.Manager.Comment("reaching state \'S820\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp108, "trustDomainInfo of QueryTrustedDomainInfoByName, state S820");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp109, "return of QueryTrustedDomainInfoByName, state S820");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S130
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S130() {
            this.Manager.BeginTest("TestScenarioS16S130");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S281\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp110;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp111;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp111 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp110);
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp110, "policyHandle of OpenPolicy2, state S389");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp111, "return of OpenPolicy2, state S389");
            this.Manager.Comment("reaching state \'S497\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp112;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp113;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp113 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp112);
            this.Manager.Comment("reaching state \'S605\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp112, "trustHandle of CreateTrustedDomain, state S605");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp113, "return of CreateTrustedDomain, state S605");
            this.Manager.Comment("reaching state \'S713\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp114;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedPosix" +
                    "OffsetInformation,out _)\'");
            temp115 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, out temp114);
            this.Manager.Comment("reaching state \'S821\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp114, "trustDomainInfo of QueryTrustedDomainInfoByName, state S821");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp115, "return of QueryTrustedDomainInfoByName, state S821");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S132
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S132() {
            this.Manager.BeginTest("TestScenarioS16S132");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S282\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp116;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp117 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp116);
            this.Manager.Comment("reaching state \'S390\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp116, "policyHandle of OpenPolicy2, state S390");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp117, "return of OpenPolicy2, state S390");
            this.Manager.Comment("reaching state \'S498\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp118;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp119;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp119 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp118);
            this.Manager.Comment("reaching state \'S606\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp118, "trustHandle of CreateTrustedDomain, state S606");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp119, "return of CreateTrustedDomain, state S606");
            this.Manager.Comment("reaching state \'S714\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp120;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp121;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nSupportedEncryptionTypes,out _)\'");
            temp121 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, out temp120);
            this.Manager.Comment("reaching state \'S822\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp120, "trustDomainInfo of QueryTrustedDomainInfoByName, state S822");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp121, "return of QueryTrustedDomainInfoByName, state S822");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S134
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S134() {
            this.Manager.BeginTest("TestScenarioS16S134");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S283\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp122;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp123;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp123 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp122);
            this.Manager.Comment("reaching state \'S391\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp122, "policyHandle of OpenPolicy2, state S391");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp123, "return of OpenPolicy2, state S391");
            this.Manager.Comment("reaching state \'S499\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp124;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp125;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp125 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp124);
            this.Manager.Comment("reaching state \'S607\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp124, "trustHandle of CreateTrustedDomain, state S607");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp125, "return of CreateTrustedDomain, state S607");
            this.Manager.Comment("reaching state \'S715\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp126;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp127;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nNameInformation,out _)\'");
            temp127 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), out temp126);
            this.Manager.Comment("reaching state \'S823\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp126, "trustDomainInfo of QueryTrustedDomainInfoByName, state S823");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp127, "return of QueryTrustedDomainInfoByName, state S823");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S136
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S136() {
            this.Manager.BeginTest("TestScenarioS16S136");
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S284\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp128;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp129;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp129 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp128);
            this.Manager.Comment("reaching state \'S392\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp128, "policyHandle of OpenPolicy2, state S392");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp129, "return of OpenPolicy2, state S392");
            this.Manager.Comment("reaching state \'S500\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp130;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp131;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp131 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp130);
            this.Manager.Comment("reaching state \'S608\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp130, "trustHandle of CreateTrustedDomain, state S608");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp131, "return of CreateTrustedDomain, state S608");
            this.Manager.Comment("reaching state \'S716\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp132;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp133;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedPassw" +
                    "ordInformation,out _)\'");
            temp133 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, out temp132);
            this.Manager.Comment("reaching state \'S824\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp132, "trustDomainInfo of QueryTrustedDomainInfoByName, state S824");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp133, "return of QueryTrustedDomainInfoByName, state S824");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S138
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S138() {
            this.Manager.BeginTest("TestScenarioS16S138");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S285\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp134;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp135;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp135 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp134);
            this.Manager.Comment("reaching state \'S393\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp134, "policyHandle of OpenPolicy2, state S393");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp135, "return of OpenPolicy2, state S393");
            this.Manager.Comment("reaching state \'S501\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp136;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp137;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp137 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp136);
            this.Manager.Comment("reaching state \'S609\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp136, "trustHandle of CreateTrustedDomain, state S609");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp137, "return of CreateTrustedDomain, state S609");
            this.Manager.Comment("reaching state \'S717\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp138;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp139;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformationInternal,out _)\'");
            temp139 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, out temp138);
            this.Manager.Comment("reaching state \'S825\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp138, "trustDomainInfo of QueryTrustedDomainInfoByName, state S825");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp139, "return of QueryTrustedDomainInfoByName, state S825");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S14() {
            this.Manager.BeginTest("TestScenarioS16S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S223\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp140;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp141;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp141 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp140);
            this.Manager.Comment("reaching state \'S331\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp140, "policyHandle of OpenPolicy2, state S331");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp141, "return of OpenPolicy2, state S331");
            this.Manager.Comment("reaching state \'S439\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp142;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp143;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp143 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp142);
            this.Manager.Comment("reaching state \'S547\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp142, "trustHandle of CreateTrustedDomain, state S547");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp143, "return of CreateTrustedDomain, state S547");
            this.Manager.Comment("reaching state \'S655\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp144;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp145;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nSupportedEncryptionTypes,out _)\'");
            temp145 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, out temp144);
            this.Manager.Comment("reaching state \'S763\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp144, "trustDomainInfo of QueryTrustedDomainInfoByName, state S763");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp145, "return of QueryTrustedDomainInfoByName, state S763");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S140
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S140() {
            this.Manager.BeginTest("TestScenarioS16S140");
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S286\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp146;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp147;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp147 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp146);
            this.Manager.Comment("reaching state \'S394\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp146, "policyHandle of OpenPolicy2, state S394");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp147, "return of OpenPolicy2, state S394");
            this.Manager.Comment("reaching state \'S502\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp148;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp149;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp149 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp148);
            this.Manager.Comment("reaching state \'S610\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp148, "trustHandle of CreateTrustedDomain, state S610");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp149, "return of CreateTrustedDomain, state S610");
            this.Manager.Comment("reaching state \'S718\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp150;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp151;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,Invalid,out " +
                    "_)\'");
            temp151 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, out temp150);
            this.Manager.Comment("reaching state \'S826\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp150, "trustDomainInfo of QueryTrustedDomainInfoByName, state S826");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp151, "return of QueryTrustedDomainInfoByName, state S826");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S142
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S142() {
            this.Manager.BeginTest("TestScenarioS16S142");
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S287\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp152;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp153;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp153 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp152);
            this.Manager.Comment("reaching state \'S395\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp152, "policyHandle of OpenPolicy2, state S395");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp153, "return of OpenPolicy2, state S395");
            this.Manager.Comment("reaching state \'S503\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp154;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp155;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp155 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp154);
            this.Manager.Comment("reaching state \'S611\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp154, "trustHandle of CreateTrustedDomain, state S611");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp155, "return of CreateTrustedDomain, state S611");
            this.Manager.Comment("reaching state \'S719\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp156;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp157;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedContr" +
                    "ollersInformation,out _)\'");
            temp157 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, out temp156);
            this.Manager.Comment("reaching state \'S827\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp156, "trustDomainInfo of QueryTrustedDomainInfoByName, state S827");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp157, "return of QueryTrustedDomainInfoByName, state S827");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S144
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S144() {
            this.Manager.BeginTest("TestScenarioS16S144");
            this.Manager.Comment("reaching state \'S144\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S145\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S288\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp158;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp159;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp159 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp158);
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp158, "policyHandle of OpenPolicy2, state S396");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp159, "return of OpenPolicy2, state S396");
            this.Manager.Comment("reaching state \'S504\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp160;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp161;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp161 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp160);
            this.Manager.Comment("reaching state \'S612\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp160, "trustHandle of CreateTrustedDomain, state S612");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp161, "return of CreateTrustedDomain, state S612");
            this.Manager.Comment("reaching state \'S720\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp162;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp163;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformation2Internal,out _)\'");
            temp163 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, out temp162);
            this.Manager.Comment("reaching state \'S828\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp162, "trustDomainInfo of QueryTrustedDomainInfoByName, state S828");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp163, "return of QueryTrustedDomainInfoByName, state S828");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S146
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S146() {
            this.Manager.BeginTest("TestScenarioS16S146");
            this.Manager.Comment("reaching state \'S146\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S147\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S289\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp164;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp165;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp165 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp164);
            this.Manager.Comment("reaching state \'S397\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp164, "policyHandle of OpenPolicy2, state S397");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp165, "return of OpenPolicy2, state S397");
            this.Manager.Comment("reaching state \'S505\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp166;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp167;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp167 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp166);
            this.Manager.Comment("reaching state \'S613\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp166, "trustHandle of CreateTrustedDomain, state S613");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp167, "return of CreateTrustedDomain, state S613");
            this.Manager.Comment("reaching state \'S721\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp168;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp169;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedContr" +
                    "ollersInformation,out _)\'");
            temp169 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, out temp168);
            this.Manager.Comment("reaching state \'S829\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp168, "trustDomainInfo of QueryTrustedDomainInfoByName, state S829");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp169, "return of QueryTrustedDomainInfoByName, state S829");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S148
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S148() {
            this.Manager.BeginTest("TestScenarioS16S148");
            this.Manager.Comment("reaching state \'S148\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S149\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S290\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp170;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp171;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp171 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp170);
            this.Manager.Comment("reaching state \'S398\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp170, "policyHandle of OpenPolicy2, state S398");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp171, "return of OpenPolicy2, state S398");
            this.Manager.Comment("reaching state \'S506\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp172;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp173;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp173 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp172);
            this.Manager.Comment("reaching state \'S614\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp172, "trustHandle of CreateTrustedDomain, state S614");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp173, "return of CreateTrustedDomain, state S614");
            this.Manager.Comment("reaching state \'S722\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp174;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp175;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationEx2Internal,out _)\'");
            temp175 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, out temp174);
            this.Manager.Comment("reaching state \'S830\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp174, "trustDomainInfo of QueryTrustedDomainInfoByName, state S830");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp175, "return of QueryTrustedDomainInfoByName, state S830");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S150
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S150() {
            this.Manager.BeginTest("TestScenarioS16S150");
            this.Manager.Comment("reaching state \'S150\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S151\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S291\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp176;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp177;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp177 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp176);
            this.Manager.Comment("reaching state \'S399\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp176, "policyHandle of OpenPolicy2, state S399");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp177, "return of OpenPolicy2, state S399");
            this.Manager.Comment("reaching state \'S507\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp178;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp179;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp179 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp178);
            this.Manager.Comment("reaching state \'S615\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp178, "trustHandle of CreateTrustedDomain, state S615");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp179, "return of CreateTrustedDomain, state S615");
            this.Manager.Comment("reaching state \'S723\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp180;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp181;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationEx,out _)\'");
            temp181 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, out temp180);
            this.Manager.Comment("reaching state \'S831\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp180, "trustDomainInfo of QueryTrustedDomainInfoByName, state S831");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp181, "return of QueryTrustedDomainInfoByName, state S831");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S152
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S152() {
            this.Manager.BeginTest("TestScenarioS16S152");
            this.Manager.Comment("reaching state \'S152\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S153\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S292\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp182;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp183;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp183 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp182);
            this.Manager.Comment("reaching state \'S400\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp182, "policyHandle of OpenPolicy2, state S400");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp183, "return of OpenPolicy2, state S400");
            this.Manager.Comment("reaching state \'S508\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp184;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp185;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp185 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp184);
            this.Manager.Comment("reaching state \'S616\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp184, "trustHandle of CreateTrustedDomain, state S616");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp185, "return of CreateTrustedDomain, state S616");
            this.Manager.Comment("reaching state \'S724\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp186;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp187;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformation2Internal,out _)\'");
            temp187 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, out temp186);
            this.Manager.Comment("reaching state \'S832\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp186, "trustDomainInfo of QueryTrustedDomainInfoByName, state S832");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp187, "return of QueryTrustedDomainInfoByName, state S832");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S154
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S154() {
            this.Manager.BeginTest("TestScenarioS16S154");
            this.Manager.Comment("reaching state \'S154\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S293\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp188;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp189;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp189 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp188);
            this.Manager.Comment("reaching state \'S401\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp188, "policyHandle of OpenPolicy2, state S401");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp189, "return of OpenPolicy2, state S401");
            this.Manager.Comment("reaching state \'S509\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp190;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp191;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp191 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp190);
            this.Manager.Comment("reaching state \'S617\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp190, "trustHandle of CreateTrustedDomain, state S617");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp191, "return of CreateTrustedDomain, state S617");
            this.Manager.Comment("reaching state \'S725\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp192;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp193;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationEx,out _)\'");
            temp193 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, out temp192);
            this.Manager.Comment("reaching state \'S833\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp192, "trustDomainInfo of QueryTrustedDomainInfoByName, state S833");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp193, "return of QueryTrustedDomainInfoByName, state S833");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S156
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S156() {
            this.Manager.BeginTest("TestScenarioS16S156");
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S294\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp194;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp195;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp195 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp194);
            this.Manager.Comment("reaching state \'S402\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp194, "policyHandle of OpenPolicy2, state S402");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp195, "return of OpenPolicy2, state S402");
            this.Manager.Comment("reaching state \'S510\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp196;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp197;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp196);
            this.Manager.Comment("reaching state \'S618\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp196, "trustHandle of CreateTrustedDomain, state S618");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp197, "return of CreateTrustedDomain, state S618");
            this.Manager.Comment("reaching state \'S726\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp198;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp199;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformationInternal,out _)\'");
            temp199 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, out temp198);
            this.Manager.Comment("reaching state \'S834\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp198, "trustDomainInfo of QueryTrustedDomainInfoByName, state S834");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp199, "return of QueryTrustedDomainInfoByName, state S834");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S158
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S158() {
            this.Manager.BeginTest("TestScenarioS16S158");
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S295\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp200;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp201;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp201 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp200);
            this.Manager.Comment("reaching state \'S403\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp200, "policyHandle of OpenPolicy2, state S403");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp201, "return of OpenPolicy2, state S403");
            this.Manager.Comment("reaching state \'S511\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp202;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp203;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp203 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp202);
            this.Manager.Comment("reaching state \'S619\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp202, "trustHandle of CreateTrustedDomain, state S619");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp203, "return of CreateTrustedDomain, state S619");
            this.Manager.Comment("reaching state \'S727\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp204;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp205;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,Invalid,out " +
                    "_)\'");
            temp205 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, out temp204);
            this.Manager.Comment("reaching state \'S835\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp204, "trustDomainInfo of QueryTrustedDomainInfoByName, state S835");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp205, "return of QueryTrustedDomainInfoByName, state S835");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S16() {
            this.Manager.BeginTest("TestScenarioS16S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S224\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp206;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp207;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp207 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp206);
            this.Manager.Comment("reaching state \'S332\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp206, "policyHandle of OpenPolicy2, state S332");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp207, "return of OpenPolicy2, state S332");
            this.Manager.Comment("reaching state \'S440\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp208;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp209;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp209 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp208);
            this.Manager.Comment("reaching state \'S548\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp208, "trustHandle of CreateTrustedDomain, state S548");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp209, "return of CreateTrustedDomain, state S548");
            this.Manager.Comment("reaching state \'S656\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp210;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp211;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationBasic,out _)\'");
            temp211 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, out temp210);
            this.Manager.Comment("reaching state \'S764\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp210, "trustDomainInfo of QueryTrustedDomainInfoByName, state S764");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp211, "return of QueryTrustedDomainInfoByName, state S764");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S160
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S160() {
            this.Manager.BeginTest("TestScenarioS16S160");
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S161\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S296\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp212;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp213;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp213 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp212);
            this.Manager.Comment("reaching state \'S404\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp212, "policyHandle of OpenPolicy2, state S404");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp213, "return of OpenPolicy2, state S404");
            this.Manager.Comment("reaching state \'S512\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp214;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp215;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp215 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp214);
            this.Manager.Comment("reaching state \'S620\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp214, "trustHandle of CreateTrustedDomain, state S620");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp215, "return of CreateTrustedDomain, state S620");
            this.Manager.Comment("reaching state \'S728\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp216;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp217;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedPassw" +
                    "ordInformation,out _)\'");
            temp217 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, out temp216);
            this.Manager.Comment("reaching state \'S836\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp216, "trustDomainInfo of QueryTrustedDomainInfoByName, state S836");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp217, "return of QueryTrustedDomainInfoByName, state S836");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S162
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S162() {
            this.Manager.BeginTest("TestScenarioS16S162");
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S297\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp218;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp219;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp219 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp218);
            this.Manager.Comment("reaching state \'S405\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp218, "policyHandle of OpenPolicy2, state S405");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp219, "return of OpenPolicy2, state S405");
            this.Manager.Comment("reaching state \'S513\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp220;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp221;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp221 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp220);
            this.Manager.Comment("reaching state \'S621\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp220, "trustHandle of CreateTrustedDomain, state S621");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp221, "return of CreateTrustedDomain, state S621");
            this.Manager.Comment("reaching state \'S729\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp222;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp223;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformation,out _)\'");
            temp223 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, out temp222);
            this.Manager.Comment("reaching state \'S837\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp222, "trustDomainInfo of QueryTrustedDomainInfoByName, state S837");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp223, "return of QueryTrustedDomainInfoByName, state S837");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S164
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S164() {
            this.Manager.BeginTest("TestScenarioS16S164");
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S298\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp224;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp225;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp225 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp224);
            this.Manager.Comment("reaching state \'S406\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp224, "policyHandle of OpenPolicy2, state S406");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp225, "return of OpenPolicy2, state S406");
            this.Manager.Comment("reaching state \'S514\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp226;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp227;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp227 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp226);
            this.Manager.Comment("reaching state \'S622\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp226, "trustHandle of CreateTrustedDomain, state S622");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp227, "return of CreateTrustedDomain, state S622");
            this.Manager.Comment("reaching state \'S730\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp228;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp229;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformation,out _)\'");
            temp229 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, out temp228);
            this.Manager.Comment("reaching state \'S838\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp228, "trustDomainInfo of QueryTrustedDomainInfoByName, state S838");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp229, "return of QueryTrustedDomainInfoByName, state S838");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S166
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S166() {
            this.Manager.BeginTest("TestScenarioS16S166");
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S299\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp230;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp231;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp231 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp230);
            this.Manager.Comment("reaching state \'S407\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp230, "policyHandle of OpenPolicy2, state S407");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp231, "return of OpenPolicy2, state S407");
            this.Manager.Comment("reaching state \'S515\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp232;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp233;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp233 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp232);
            this.Manager.Comment("reaching state \'S623\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp232, "trustHandle of CreateTrustedDomain, state S623");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp233, "return of CreateTrustedDomain, state S623");
            this.Manager.Comment("reaching state \'S731\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp234;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp235;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nNameInformation,out _)\'");
            temp235 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), out temp234);
            this.Manager.Comment("reaching state \'S839\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp234, "trustDomainInfo of QueryTrustedDomainInfoByName, state S839");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp235, "return of QueryTrustedDomainInfoByName, state S839");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S168
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S168() {
            this.Manager.BeginTest("TestScenarioS16S168");
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S300\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp236;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp237;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp237 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp236);
            this.Manager.Comment("reaching state \'S408\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp236, "policyHandle of OpenPolicy2, state S408");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp237, "return of OpenPolicy2, state S408");
            this.Manager.Comment("reaching state \'S516\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp238;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp239;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp239 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp238);
            this.Manager.Comment("reaching state \'S624\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp238, "trustHandle of CreateTrustedDomain, state S624");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp239, "return of CreateTrustedDomain, state S624");
            this.Manager.Comment("reaching state \'S732\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp240;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp241;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedPosix" +
                    "OffsetInformation,out _)\'");
            temp241 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, out temp240);
            this.Manager.Comment("reaching state \'S840\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp240, "trustDomainInfo of QueryTrustedDomainInfoByName, state S840");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp241, "return of QueryTrustedDomainInfoByName, state S840");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S170
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S170() {
            this.Manager.BeginTest("TestScenarioS16S170");
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S301\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp242;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp243;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp243 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp242);
            this.Manager.Comment("reaching state \'S409\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp242, "policyHandle of OpenPolicy2, state S409");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp243, "return of OpenPolicy2, state S409");
            this.Manager.Comment("reaching state \'S517\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp244;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp245;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp245 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp244);
            this.Manager.Comment("reaching state \'S625\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp244, "trustHandle of CreateTrustedDomain, state S625");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp245, "return of CreateTrustedDomain, state S625");
            this.Manager.Comment("reaching state \'S733\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp246;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp247;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationEx2Internal,out _)\'");
            temp247 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, out temp246);
            this.Manager.Comment("reaching state \'S841\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp246, "trustDomainInfo of QueryTrustedDomainInfoByName, state S841");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp247, "return of QueryTrustedDomainInfoByName, state S841");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S172
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S172() {
            this.Manager.BeginTest("TestScenarioS16S172");
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S302\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp248;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp249;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp249 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp248);
            this.Manager.Comment("reaching state \'S410\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp248, "policyHandle of OpenPolicy2, state S410");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp249, "return of OpenPolicy2, state S410");
            this.Manager.Comment("reaching state \'S518\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp250;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp251;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp251 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp250);
            this.Manager.Comment("reaching state \'S626\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp250, "trustHandle of CreateTrustedDomain, state S626");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp251, "return of CreateTrustedDomain, state S626");
            this.Manager.Comment("reaching state \'S734\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp252;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp253;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nAuthInformationInternal,out _)\'");
            temp253 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, out temp252);
            this.Manager.Comment("reaching state \'S842\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp252, "trustDomainInfo of QueryTrustedDomainInfoByName, state S842");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp253, "return of QueryTrustedDomainInfoByName, state S842");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S174
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S174() {
            this.Manager.BeginTest("TestScenarioS16S174");
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S303\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp254;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp255;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp255 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp254);
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp254, "policyHandle of OpenPolicy2, state S411");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp255, "return of OpenPolicy2, state S411");
            this.Manager.Comment("reaching state \'S519\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp256;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp257;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp257 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp256);
            this.Manager.Comment("reaching state \'S627\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp256, "trustHandle of CreateTrustedDomain, state S627");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp257, "return of CreateTrustedDomain, state S627");
            this.Manager.Comment("reaching state \'S735\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp258;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp259;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,Invalid,out _)" +
                    "\'");
            temp259 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, out temp258);
            this.Manager.Comment("reaching state \'S843\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp258, "trustDomainInfo of QueryTrustedDomainInfoByName, state S843");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp259, "return of QueryTrustedDomainInfoByName, state S843");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S176
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S176() {
            this.Manager.BeginTest("TestScenarioS16S176");
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S304\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp260;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp261;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp261 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp260);
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp260, "policyHandle of OpenPolicy2, state S412");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp261, "return of OpenPolicy2, state S412");
            this.Manager.Comment("reaching state \'S520\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp262;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp263;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp263 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp262);
            this.Manager.Comment("reaching state \'S628\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp262, "trustHandle of CreateTrustedDomain, state S628");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp263, "return of CreateTrustedDomain, state S628");
            this.Manager.Comment("reaching state \'S736\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp264;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp265;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformationInternal,out _)\'");
            temp265 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, out temp264);
            this.Manager.Comment("reaching state \'S844\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp264, "trustDomainInfo of QueryTrustedDomainInfoByName, state S844");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp265, "return of QueryTrustedDomainInfoByName, state S844");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S178
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S178() {
            this.Manager.BeginTest("TestScenarioS16S178");
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S179\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S305\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp266;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp267;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp267 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp266);
            this.Manager.Comment("reaching state \'S413\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp266, "policyHandle of OpenPolicy2, state S413");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp267, "return of OpenPolicy2, state S413");
            this.Manager.Comment("reaching state \'S521\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp268;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp269;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp269 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp268);
            this.Manager.Comment("reaching state \'S629\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp268, "trustHandle of CreateTrustedDomain, state S629");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp269, "return of CreateTrustedDomain, state S629");
            this.Manager.Comment("reaching state \'S737\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp270;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp271;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedPasswor" +
                    "dInformation,out _)\'");
            temp271 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, out temp270);
            this.Manager.Comment("reaching state \'S845\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp270, "trustDomainInfo of QueryTrustedDomainInfoByName, state S845");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp271, "return of QueryTrustedDomainInfoByName, state S845");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S18() {
            this.Manager.BeginTest("TestScenarioS16S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S225\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp272;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp273;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp273 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp272);
            this.Manager.Comment("reaching state \'S333\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp272, "policyHandle of OpenPolicy2, state S333");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp273, "return of OpenPolicy2, state S333");
            this.Manager.Comment("reaching state \'S441\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp274;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp275;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp274);
            this.Manager.Comment("reaching state \'S549\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp274, "trustHandle of CreateTrustedDomain, state S549");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp275, "return of CreateTrustedDomain, state S549");
            this.Manager.Comment("reaching state \'S657\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp276;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp277;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainI" +
                    "nformationBasic,out _)\'");
            temp277 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, out temp276);
            this.Manager.Comment("reaching state \'S765\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp276, "trustDomainInfo of QueryTrustedDomainInfoByName, state S765");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp277, "return of QueryTrustedDomainInfoByName, state S765");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S180
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S180() {
            this.Manager.BeginTest("TestScenarioS16S180");
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S306\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp278;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp279;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp279 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp278);
            this.Manager.Comment("reaching state \'S414\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp278, "policyHandle of OpenPolicy2, state S414");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp279, "return of OpenPolicy2, state S414");
            this.Manager.Comment("reaching state \'S522\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp280;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp281;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp281 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp280);
            this.Manager.Comment("reaching state \'S630\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp280, "trustHandle of CreateTrustedDomain, state S630");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp281, "return of CreateTrustedDomain, state S630");
            this.Manager.Comment("reaching state \'S738\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp282;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp283;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedControl" +
                    "lersInformation,out _)\'");
            temp283 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, out temp282);
            this.Manager.Comment("reaching state \'S846\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp282, "trustDomainInfo of QueryTrustedDomainInfoByName, state S846");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp283, "return of QueryTrustedDomainInfoByName, state S846");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S182
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S182() {
            this.Manager.BeginTest("TestScenarioS16S182");
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S307\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp284;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp285;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp285 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp284);
            this.Manager.Comment("reaching state \'S415\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp284, "policyHandle of OpenPolicy2, state S415");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp285, "return of OpenPolicy2, state S415");
            this.Manager.Comment("reaching state \'S523\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp286;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp287;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp287 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp286);
            this.Manager.Comment("reaching state \'S631\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp286, "trustHandle of CreateTrustedDomain, state S631");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp287, "return of CreateTrustedDomain, state S631");
            this.Manager.Comment("reaching state \'S739\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp288;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp289;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainI" +
                    "nformationEx,out _)\'");
            temp289 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, out temp288);
            this.Manager.Comment("reaching state \'S847\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp288, "trustDomainInfo of QueryTrustedDomainInfoByName, state S847");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp289, "return of QueryTrustedDomainInfoByName, state S847");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S184
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S184() {
            this.Manager.BeginTest("TestScenarioS16S184");
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S308\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp290;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp291;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp291 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp290);
            this.Manager.Comment("reaching state \'S416\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp290, "policyHandle of OpenPolicy2, state S416");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp291, "return of OpenPolicy2, state S416");
            this.Manager.Comment("reaching state \'S524\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp292;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp293;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp293 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp292);
            this.Manager.Comment("reaching state \'S632\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp292, "trustHandle of CreateTrustedDomain, state S632");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp293, "return of CreateTrustedDomain, state S632");
            this.Manager.Comment("reaching state \'S740\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp294;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp295;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformation2Internal,out _)\'");
            temp295 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, out temp294);
            this.Manager.Comment("reaching state \'S848\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp294, "trustDomainInfo of QueryTrustedDomainInfoByName, state S848");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp295, "return of QueryTrustedDomainInfoByName, state S848");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S186
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S186() {
            this.Manager.BeginTest("TestScenarioS16S186");
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S309\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp296;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp297;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp297 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp296);
            this.Manager.Comment("reaching state \'S417\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp296, "policyHandle of OpenPolicy2, state S417");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp297, "return of OpenPolicy2, state S417");
            this.Manager.Comment("reaching state \'S525\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp298;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp299;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp299 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp298);
            this.Manager.Comment("reaching state \'S633\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp298, "trustHandle of CreateTrustedDomain, state S633");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp299, "return of CreateTrustedDomain, state S633");
            this.Manager.Comment("reaching state \'S741\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp300;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp301;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedControl" +
                    "lersInformation,out _)\'");
            temp301 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, out temp300);
            this.Manager.Comment("reaching state \'S849\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp300, "trustDomainInfo of QueryTrustedDomainInfoByName, state S849");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp301, "return of QueryTrustedDomainInfoByName, state S849");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S188
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S188() {
            this.Manager.BeginTest("TestScenarioS16S188");
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S310\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp302;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp303;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp303 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp302);
            this.Manager.Comment("reaching state \'S418\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp302, "policyHandle of OpenPolicy2, state S418");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp303, "return of OpenPolicy2, state S418");
            this.Manager.Comment("reaching state \'S526\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp304;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp305;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp305 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp304);
            this.Manager.Comment("reaching state \'S634\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp304, "trustHandle of CreateTrustedDomain, state S634");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp305, "return of CreateTrustedDomain, state S634");
            this.Manager.Comment("reaching state \'S742\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp306;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp307;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedPasswor" +
                    "dInformation,out _)\'");
            temp307 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, out temp306);
            this.Manager.Comment("reaching state \'S850\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp306, "trustDomainInfo of QueryTrustedDomainInfoByName, state S850");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp307, "return of QueryTrustedDomainInfoByName, state S850");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S190
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S190() {
            this.Manager.BeginTest("TestScenarioS16S190");
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S311\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp308;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp309;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp309 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp308);
            this.Manager.Comment("reaching state \'S419\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp308, "policyHandle of OpenPolicy2, state S419");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp309, "return of OpenPolicy2, state S419");
            this.Manager.Comment("reaching state \'S527\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp310;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp311;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp311 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp310);
            this.Manager.Comment("reaching state \'S635\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp310, "trustHandle of CreateTrustedDomain, state S635");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp311, "return of CreateTrustedDomain, state S635");
            this.Manager.Comment("reaching state \'S743\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp312;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp313;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainI" +
                    "nformationBasic,out _)\'");
            temp313 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, out temp312);
            this.Manager.Comment("reaching state \'S851\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp312, "trustDomainInfo of QueryTrustedDomainInfoByName, state S851");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp313, "return of QueryTrustedDomainInfoByName, state S851");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S192
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S192() {
            this.Manager.BeginTest("TestScenarioS16S192");
            this.Manager.Comment("reaching state \'S192\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S193\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S312\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp314;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp315;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp315 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp314);
            this.Manager.Comment("reaching state \'S420\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp314, "policyHandle of OpenPolicy2, state S420");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp315, "return of OpenPolicy2, state S420");
            this.Manager.Comment("reaching state \'S528\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp316;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp317;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp317 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp316);
            this.Manager.Comment("reaching state \'S636\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp316, "trustHandle of CreateTrustedDomain, state S636");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp317, "return of CreateTrustedDomain, state S636");
            this.Manager.Comment("reaching state \'S744\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp318;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp319;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainI" +
                    "nformationEx2Internal,out _)\'");
            temp319 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, out temp318);
            this.Manager.Comment("reaching state \'S852\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp318, "trustDomainInfo of QueryTrustedDomainInfoByName, state S852");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp319, "return of QueryTrustedDomainInfoByName, state S852");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S194
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S194() {
            this.Manager.BeginTest("TestScenarioS16S194");
            this.Manager.Comment("reaching state \'S194\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S195\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S313\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp320;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp321;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp321 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp320);
            this.Manager.Comment("reaching state \'S421\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp320, "policyHandle of OpenPolicy2, state S421");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp321, "return of OpenPolicy2, state S421");
            this.Manager.Comment("reaching state \'S529\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp322;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp323;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp323 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp322);
            this.Manager.Comment("reaching state \'S637\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp322, "trustHandle of CreateTrustedDomain, state S637");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp323, "return of CreateTrustedDomain, state S637");
            this.Manager.Comment("reaching state \'S745\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp324;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp325;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,Invalid,out _)" +
                    "\'");
            temp325 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, out temp324);
            this.Manager.Comment("reaching state \'S853\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp324, "trustDomainInfo of QueryTrustedDomainInfoByName, state S853");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp325, "return of QueryTrustedDomainInfoByName, state S853");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S196
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S196() {
            this.Manager.BeginTest("TestScenarioS16S196");
            this.Manager.Comment("reaching state \'S196\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S197\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S314\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp326;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp327;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp327 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp326);
            this.Manager.Comment("reaching state \'S422\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp326, "policyHandle of OpenPolicy2, state S422");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp327, "return of OpenPolicy2, state S422");
            this.Manager.Comment("reaching state \'S530\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp328;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp329;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp329 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp328);
            this.Manager.Comment("reaching state \'S638\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp328, "trustHandle of CreateTrustedDomain, state S638");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp329, "return of CreateTrustedDomain, state S638");
            this.Manager.Comment("reaching state \'S746\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp330;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp331;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainA" +
                    "uthInformationInternal,out _)\'");
            temp331 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, out temp330);
            this.Manager.Comment("reaching state \'S854\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp330, "trustDomainInfo of QueryTrustedDomainInfoByName, state S854");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp331, "return of QueryTrustedDomainInfoByName, state S854");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S198
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S198() {
            this.Manager.BeginTest("TestScenarioS16S198");
            this.Manager.Comment("reaching state \'S198\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S199\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S315\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp332;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp333;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp333 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp332);
            this.Manager.Comment("reaching state \'S423\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp332, "policyHandle of OpenPolicy2, state S423");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp333, "return of OpenPolicy2, state S423");
            this.Manager.Comment("reaching state \'S531\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp334;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp335;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp335 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp334);
            this.Manager.Comment("reaching state \'S639\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp334, "trustHandle of CreateTrustedDomain, state S639");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp335, "return of CreateTrustedDomain, state S639");
            this.Manager.Comment("reaching state \'S747\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp336;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp337;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainN" +
                    "ameInformation,out _)\'");
            temp337 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), out temp336);
            this.Manager.Comment("reaching state \'S855\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp336, "trustDomainInfo of QueryTrustedDomainInfoByName, state S855");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp337, "return of QueryTrustedDomainInfoByName, state S855");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S2() {
            this.Manager.BeginTest("TestScenarioS16S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S217\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp338;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp339;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp339 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp338);
            this.Manager.Comment("reaching state \'S325\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp338, "policyHandle of OpenPolicy2, state S325");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp339, "return of OpenPolicy2, state S325");
            this.Manager.Comment("reaching state \'S433\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp340;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp341;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp341 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp340);
            this.Manager.Comment("reaching state \'S541\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp340, "trustHandle of CreateTrustedDomain, state S541");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp341, "return of CreateTrustedDomain, state S541");
            this.Manager.Comment("reaching state \'S649\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp342;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp343;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainS" +
                    "upportedEncryptionTypes,out _)\'");
            temp343 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, out temp342);
            this.Manager.Comment("reaching state \'S757\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp342, "trustDomainInfo of QueryTrustedDomainInfoByName, state S757");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp343, "return of QueryTrustedDomainInfoByName, state S757");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S20() {
            this.Manager.BeginTest("TestScenarioS16S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S226\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp344;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp345;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp345 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp344);
            this.Manager.Comment("reaching state \'S334\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp344, "policyHandle of OpenPolicy2, state S334");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp345, "return of OpenPolicy2, state S334");
            this.Manager.Comment("reaching state \'S442\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp346;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp347;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp347 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp346);
            this.Manager.Comment("reaching state \'S550\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp346, "trustHandle of CreateTrustedDomain, state S550");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp347, "return of CreateTrustedDomain, state S550");
            this.Manager.Comment("reaching state \'S658\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp348;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp349;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainI" +
                    "nformationEx2Internal,out _)\'");
            temp349 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, out temp348);
            this.Manager.Comment("reaching state \'S766\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp348, "trustDomainInfo of QueryTrustedDomainInfoByName, state S766");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp349, "return of QueryTrustedDomainInfoByName, state S766");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S200
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S200() {
            this.Manager.BeginTest("TestScenarioS16S200");
            this.Manager.Comment("reaching state \'S200\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S201\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S316\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp350;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp351;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp351 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp350);
            this.Manager.Comment("reaching state \'S424\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp350, "policyHandle of OpenPolicy2, state S424");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp351, "return of OpenPolicy2, state S424");
            this.Manager.Comment("reaching state \'S532\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp352;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp353;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp353 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp352);
            this.Manager.Comment("reaching state \'S640\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp352, "trustHandle of CreateTrustedDomain, state S640");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp353, "return of CreateTrustedDomain, state S640");
            this.Manager.Comment("reaching state \'S748\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp354;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp355;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformation,out _)\'");
            temp355 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, out temp354);
            this.Manager.Comment("reaching state \'S856\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp354, "trustDomainInfo of QueryTrustedDomainInfoByName, state S856");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp355, "return of QueryTrustedDomainInfoByName, state S856");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S202
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S202() {
            this.Manager.BeginTest("TestScenarioS16S202");
            this.Manager.Comment("reaching state \'S202\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S203\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S317\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp356;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp357;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp357 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp356);
            this.Manager.Comment("reaching state \'S425\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp356, "policyHandle of OpenPolicy2, state S425");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp357, "return of OpenPolicy2, state S425");
            this.Manager.Comment("reaching state \'S533\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp358;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp359;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Invalid,True,Valid,65663,out _)\'");
            temp359 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(1)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp358);
            this.Manager.Comment("reaching state \'S641\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidSid\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp358, "trustHandle of CreateTrustedDomain, state S641");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSid, temp359, "return of CreateTrustedDomain, state S641");
            this.Manager.Comment("reaching state \'S749\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp360;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp361;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformation,out _)\'");
            temp361 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, out temp360);
            this.Manager.Comment("reaching state \'S857\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ObjectNameNotFou" +
                    "nd\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp360, "trustDomainInfo of QueryTrustedDomainInfoByName, state S857");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp361, "return of QueryTrustedDomainInfoByName, state S857");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S204
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S204() {
            this.Manager.BeginTest("TestScenarioS16S204");
            this.Manager.Comment("reaching state \'S204\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S205\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S318\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp362;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp363;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp363 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp362);
            this.Manager.Comment("reaching state \'S426\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp362, "policyHandle of OpenPolicy2, state S426");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp363, "return of OpenPolicy2, state S426");
            this.Manager.Comment("reaching state \'S534\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp364;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp365;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"CurrentDomain\",TrustDomain_Sid=\"CurrentDomainSid\",TrustDomain_NetBiosNa" +
                    "me=\"CurrentDomainNetBios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,6" +
                    "5663,out _)\'");
            temp365 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentDomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp364);
            this.Manager.Comment("reaching state \'S642\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:CurrentDomainNotAllowed\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp364, "trustHandle of CreateTrustedDomain, state S642");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp365, "return of CreateTrustedDomain, state S642");
            this.Manager.Comment("reaching state \'S750\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp366;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp367;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformation2Internal,out _)\'");
            temp367 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, out temp366);
            this.Manager.Comment("reaching state \'S858\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ObjectNameNotFou" +
                    "nd\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp366, "trustDomainInfo of QueryTrustedDomainInfoByName, state S858");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp367, "return of QueryTrustedDomainInfoByName, state S858");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S206
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S206() {
            this.Manager.BeginTest("TestScenarioS16S206");
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S319\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp368;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp369;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp369 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp368);
            this.Manager.Comment("reaching state \'S427\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp368, "policyHandle of OpenPolicy2, state S427");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp369, "return of OpenPolicy2, state S427");
            this.Manager.Comment("reaching state \'S535\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp370;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp371;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"CurrentDomain\",TrustDomain_Sid=\"CurrentDomainSid\",TrustDomain_NetBiosNa" +
                    "me=\"CurrentDomainNetBios\",TrustType=0,TrustDir=0,TrustAttr=0),Invalid,True,Valid" +
                    ",65663,out _)\'");
            temp371 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentDomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(1)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp370);
            this.Manager.Comment("reaching state \'S643\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp370, "trustHandle of CreateTrustedDomain, state S643");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp371, "return of CreateTrustedDomain, state S643");
            this.Manager.Comment("reaching state \'S751\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp372;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp373;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainS" +
                    "upportedEncryptionTypes,out _)\'");
            temp373 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, out temp372);
            this.Manager.Comment("reaching state \'S859\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ObjectNameNotFou" +
                    "nd\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp372, "trustDomainInfo of QueryTrustedDomainInfoByName, state S859");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp373, "return of QueryTrustedDomainInfoByName, state S859");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S208
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S208() {
            this.Manager.BeginTest("TestScenarioS16S208");
            this.Manager.Comment("reaching state \'S208\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S209\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S320\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp374;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp375;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp375 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp374);
            this.Manager.Comment("reaching state \'S428\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp374, "policyHandle of OpenPolicy2, state S428");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp375, "return of OpenPolicy2, state S428");
            this.Manager.Comment("reaching state \'S536\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp376;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp377;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentDomainNetBios"",TrustType=0,TrustDir=0,TrustAttr=0),Invalid,True,Invalid,65663,out _)'");
            temp377 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentDomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(1)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp376);
            this.Manager.Comment("reaching state \'S644\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp376, "trustHandle of CreateTrustedDomain, state S644");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp377, "return of CreateTrustedDomain, state S644");
            this.Manager.Comment("reaching state \'S752\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp378;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp379;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainA" +
                    "uthInformation,out _)\'");
            temp379 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, out temp378);
            this.Manager.Comment("reaching state \'S860\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp378, "trustDomainInfo of QueryTrustedDomainInfoByName, state S860");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp379, "return of QueryTrustedDomainInfoByName, state S860");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S210
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S210() {
            this.Manager.BeginTest("TestScenarioS16S210");
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S321\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp380;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp381;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp381 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp380);
            this.Manager.Comment("reaching state \'S429\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp380, "policyHandle of OpenPolicy2, state S429");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp381, "return of OpenPolicy2, state S429");
            this.Manager.Comment("reaching state \'S537\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp382;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp383;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"CurrentDomain\",TrustDomain_Sid=\"CurrentDomainSid\",TrustDomain_NetBiosNa" +
                    "me=\"CurrentDomainNetBios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid" +
                    ",65663,out _)\'");
            temp383 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentDomainNetBios",
                            0u,
                            0u,
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp382);
            this.Manager.Comment("reaching state \'S645\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp382, "trustHandle of CreateTrustedDomain, state S645");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp383, "return of CreateTrustedDomain, state S645");
            this.Manager.Comment("reaching state \'S753\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp384;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp385;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nAuthInformation,out _)\'");
            temp385 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, out temp384);
            this.Manager.Comment("reaching state \'S861\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp384, "trustDomainInfo of QueryTrustedDomainInfoByName, state S861");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp385, "return of QueryTrustedDomainInfoByName, state S861");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S212
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S212() {
            this.Manager.BeginTest("TestScenarioS16S212");
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S322\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp386;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp387;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp387 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp386);
            this.Manager.Comment("reaching state \'S430\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp386, "policyHandle of OpenPolicy2, state S430");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp387, "return of OpenPolicy2, state S430");
            this.Manager.Comment("reaching state \'S538\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp388;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp389;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp389 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp388);
            this.Manager.Comment("reaching state \'S646\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp388, "trustHandle of CreateTrustedDomain, state S646");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp389, "return of CreateTrustedDomain, state S646");
            this.Manager.Comment("reaching state \'S754\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp390;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp391;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,Invalid,out " +
                    "_)\'");
            temp391 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, out temp390);
            this.Manager.Comment("reaching state \'S862\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp390, "trustDomainInfo of QueryTrustedDomainInfoByName, state S862");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp391, "return of QueryTrustedDomainInfoByName, state S862");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S214
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S214() {
            this.Manager.BeginTest("TestScenarioS16S214");
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S323\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp392;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp393;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp393 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp392);
            this.Manager.Comment("reaching state \'S431\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp392, "policyHandle of OpenPolicy2, state S431");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp393, "return of OpenPolicy2, state S431");
            this.Manager.Comment("reaching state \'S539\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp394;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp395;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Invalid,65663,out _)\'");
            temp395 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 65663u, out temp394);
            this.Manager.Comment("reaching state \'S647\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp394, "trustHandle of CreateTrustedDomain, state S647");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp395, "return of CreateTrustedDomain, state S647");
            this.Manager.Comment("reaching state \'S755\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp396;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp397;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainA" +
                    "uthInformation,out _)\'");
            temp397 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, out temp396);
            this.Manager.Comment("reaching state \'S863\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp396, "trustDomainInfo of QueryTrustedDomainInfoByName, state S863");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp397, "return of QueryTrustedDomainInfoByName, state S863");
            TestScenarioS16S864();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S22() {
            this.Manager.BeginTest("TestScenarioS16S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S227\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp398;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp399;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp399 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp398);
            this.Manager.Comment("reaching state \'S335\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp398, "policyHandle of OpenPolicy2, state S335");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp399, "return of OpenPolicy2, state S335");
            this.Manager.Comment("reaching state \'S443\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp400;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp401;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp401 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp400);
            this.Manager.Comment("reaching state \'S551\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp400, "trustHandle of CreateTrustedDomain, state S551");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp401, "return of CreateTrustedDomain, state S551");
            this.Manager.Comment("reaching state \'S659\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp402;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp403;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationEx2Internal,out _)\'");
            temp403 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, out temp402);
            this.Manager.Comment("reaching state \'S767\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp402, "trustDomainInfo of QueryTrustedDomainInfoByName, state S767");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp403, "return of QueryTrustedDomainInfoByName, state S767");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S24() {
            this.Manager.BeginTest("TestScenarioS16S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S228\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp404;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp405;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp405 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp404);
            this.Manager.Comment("reaching state \'S336\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp404, "policyHandle of OpenPolicy2, state S336");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp405, "return of OpenPolicy2, state S336");
            this.Manager.Comment("reaching state \'S444\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp406;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp407;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp407 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp406);
            this.Manager.Comment("reaching state \'S552\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp406, "trustHandle of CreateTrustedDomain, state S552");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp407, "return of CreateTrustedDomain, state S552");
            this.Manager.Comment("reaching state \'S660\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp408;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp409;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformation,out _)\'");
            temp409 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, out temp408);
            this.Manager.Comment("reaching state \'S768\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp408, "trustDomainInfo of QueryTrustedDomainInfoByName, state S768");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp409, "return of QueryTrustedDomainInfoByName, state S768");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S26() {
            this.Manager.BeginTest("TestScenarioS16S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S229\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp410;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp411;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp411 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp410);
            this.Manager.Comment("reaching state \'S337\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp410, "policyHandle of OpenPolicy2, state S337");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp411, "return of OpenPolicy2, state S337");
            this.Manager.Comment("reaching state \'S445\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp412;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp413;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp413 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp412);
            this.Manager.Comment("reaching state \'S553\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp412, "trustHandle of CreateTrustedDomain, state S553");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp413, "return of CreateTrustedDomain, state S553");
            this.Manager.Comment("reaching state \'S661\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp414;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp415;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedPosix" +
                    "OffsetInformation,out _)\'");
            temp415 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, out temp414);
            this.Manager.Comment("reaching state \'S769\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp414, "trustDomainInfo of QueryTrustedDomainInfoByName, state S769");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp415, "return of QueryTrustedDomainInfoByName, state S769");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S28() {
            this.Manager.BeginTest("TestScenarioS16S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S230\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp416;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp417;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp417 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp416);
            this.Manager.Comment("reaching state \'S338\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp416, "policyHandle of OpenPolicy2, state S338");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp417, "return of OpenPolicy2, state S338");
            this.Manager.Comment("reaching state \'S446\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp418;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp419;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp419 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp418);
            this.Manager.Comment("reaching state \'S554\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp418, "trustHandle of CreateTrustedDomain, state S554");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp419, "return of CreateTrustedDomain, state S554");
            this.Manager.Comment("reaching state \'S662\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp420;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp421;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformation2Internal,out _)\'");
            temp421 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, out temp420);
            this.Manager.Comment("reaching state \'S770\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp420, "trustDomainInfo of QueryTrustedDomainInfoByName, state S770");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp421, "return of QueryTrustedDomainInfoByName, state S770");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S30() {
            this.Manager.BeginTest("TestScenarioS16S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S231\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp422;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp423;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp423 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp422);
            this.Manager.Comment("reaching state \'S339\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp422, "policyHandle of OpenPolicy2, state S339");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp423, "return of OpenPolicy2, state S339");
            this.Manager.Comment("reaching state \'S447\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp424;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp425;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp425 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp424);
            this.Manager.Comment("reaching state \'S555\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp424, "trustHandle of CreateTrustedDomain, state S555");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp425, "return of CreateTrustedDomain, state S555");
            this.Manager.Comment("reaching state \'S663\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp426;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp427;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nNameInformation,out _)\'");
            temp427 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), out temp426);
            this.Manager.Comment("reaching state \'S771\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp426, "trustDomainInfo of QueryTrustedDomainInfoByName, state S771");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp427, "return of QueryTrustedDomainInfoByName, state S771");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S32() {
            this.Manager.BeginTest("TestScenarioS16S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S232\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp428;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp429;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp429 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp428);
            this.Manager.Comment("reaching state \'S340\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp428, "policyHandle of OpenPolicy2, state S340");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp429, "return of OpenPolicy2, state S340");
            this.Manager.Comment("reaching state \'S448\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp430;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp431;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp431 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp430);
            this.Manager.Comment("reaching state \'S556\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp430, "trustHandle of CreateTrustedDomain, state S556");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp431, "return of CreateTrustedDomain, state S556");
            this.Manager.Comment("reaching state \'S664\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp432;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp433;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationEx,out _)\'");
            temp433 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, out temp432);
            this.Manager.Comment("reaching state \'S772\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp432, "trustDomainInfo of QueryTrustedDomainInfoByName, state S772");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp433, "return of QueryTrustedDomainInfoByName, state S772");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S34() {
            this.Manager.BeginTest("TestScenarioS16S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S233\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp434;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp435;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp435 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp434);
            this.Manager.Comment("reaching state \'S341\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp434, "policyHandle of OpenPolicy2, state S341");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp435, "return of OpenPolicy2, state S341");
            this.Manager.Comment("reaching state \'S449\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp436;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp437;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp437 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp436);
            this.Manager.Comment("reaching state \'S557\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp436, "trustHandle of CreateTrustedDomain, state S557");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp437, "return of CreateTrustedDomain, state S557");
            this.Manager.Comment("reaching state \'S665\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp438;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp439;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedPassw" +
                    "ordInformation,out _)\'");
            temp439 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, out temp438);
            this.Manager.Comment("reaching state \'S773\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp438, "trustDomainInfo of QueryTrustedDomainInfoByName, state S773");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp439, "return of QueryTrustedDomainInfoByName, state S773");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S36() {
            this.Manager.BeginTest("TestScenarioS16S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S234\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp440;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp441;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp441 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp440);
            this.Manager.Comment("reaching state \'S342\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp440, "policyHandle of OpenPolicy2, state S342");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp441, "return of OpenPolicy2, state S342");
            this.Manager.Comment("reaching state \'S450\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp442;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp443;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp443 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp442);
            this.Manager.Comment("reaching state \'S558\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp442, "trustHandle of CreateTrustedDomain, state S558");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp443, "return of CreateTrustedDomain, state S558");
            this.Manager.Comment("reaching state \'S666\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp444;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp445;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedPasswor" +
                    "dInformation,out _)\'");
            temp445 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, out temp444);
            this.Manager.Comment("reaching state \'S774\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidParameter" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp444, "trustDomainInfo of QueryTrustedDomainInfoByName, state S774");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp445, "return of QueryTrustedDomainInfoByName, state S774");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S38() {
            this.Manager.BeginTest("TestScenarioS16S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S235\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp446;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp447;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp447 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp446);
            this.Manager.Comment("reaching state \'S343\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp446, "policyHandle of OpenPolicy2, state S343");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp447, "return of OpenPolicy2, state S343");
            this.Manager.Comment("reaching state \'S451\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp448;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp449;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp449 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp448);
            this.Manager.Comment("reaching state \'S559\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp448, "trustHandle of CreateTrustedDomain, state S559");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp449, "return of CreateTrustedDomain, state S559");
            this.Manager.Comment("reaching state \'S667\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp450;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp451;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformationInternal,out _)\'");
            temp451 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, out temp450);
            this.Manager.Comment("reaching state \'S775\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidInfoClass" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp450, "trustDomainInfo of QueryTrustedDomainInfoByName, state S775");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidInfoClass, temp451, "return of QueryTrustedDomainInfoByName, state S775");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S4() {
            this.Manager.BeginTest("TestScenarioS16S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S218\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp452;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp453;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp453 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp452);
            this.Manager.Comment("reaching state \'S326\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp452, "policyHandle of OpenPolicy2, state S326");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp453, "return of OpenPolicy2, state S326");
            this.Manager.Comment("reaching state \'S434\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp454;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp455;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp455 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp454);
            this.Manager.Comment("reaching state \'S542\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp454, "trustHandle of CreateTrustedDomain, state S542");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp455, "return of CreateTrustedDomain, state S542");
            this.Manager.Comment("reaching state \'S650\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp456;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp457;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformation2Internal,out _)\'");
            temp457 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, out temp456);
            this.Manager.Comment("reaching state \'S758\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp456, "trustDomainInfo of QueryTrustedDomainInfoByName, state S758");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp457, "return of QueryTrustedDomainInfoByName, state S758");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S40() {
            this.Manager.BeginTest("TestScenarioS16S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S236\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp458;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp459;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp459 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp458);
            this.Manager.Comment("reaching state \'S344\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp458, "policyHandle of OpenPolicy2, state S344");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp459, "return of OpenPolicy2, state S344");
            this.Manager.Comment("reaching state \'S452\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp460;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp461;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp461 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp460);
            this.Manager.Comment("reaching state \'S560\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp460, "trustHandle of CreateTrustedDomain, state S560");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp461, "return of CreateTrustedDomain, state S560");
            this.Manager.Comment("reaching state \'S668\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp462;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp463;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainA" +
                    "uthInformationInternal,out _)\'");
            temp463 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, out temp462);
            this.Manager.Comment("reaching state \'S776\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidInfoClass" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp462, "trustDomainInfo of QueryTrustedDomainInfoByName, state S776");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidInfoClass, temp463, "return of QueryTrustedDomainInfoByName, state S776");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S42() {
            this.Manager.BeginTest("TestScenarioS16S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S237\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp464;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp465;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp465 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp464);
            this.Manager.Comment("reaching state \'S345\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp464, "policyHandle of OpenPolicy2, state S345");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp465, "return of OpenPolicy2, state S345");
            this.Manager.Comment("reaching state \'S453\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp466;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp467;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp467 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp466);
            this.Manager.Comment("reaching state \'S561\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp466, "trustHandle of CreateTrustedDomain, state S561");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp467, "return of CreateTrustedDomain, state S561");
            this.Manager.Comment("reaching state \'S669\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp468;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp469;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainA" +
                    "uthInformation,out _)\'");
            temp469 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, out temp468);
            this.Manager.Comment("reaching state \'S777\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidInfoClass" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp468, "trustDomainInfo of QueryTrustedDomainInfoByName, state S777");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidInfoClass, temp469, "return of QueryTrustedDomainInfoByName, state S777");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S44() {
            this.Manager.BeginTest("TestScenarioS16S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S238\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp470;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp471;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp471 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp470);
            this.Manager.Comment("reaching state \'S346\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp470, "policyHandle of OpenPolicy2, state S346");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp471, "return of OpenPolicy2, state S346");
            this.Manager.Comment("reaching state \'S454\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp472;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp473;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp473 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp472);
            this.Manager.Comment("reaching state \'S562\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp472, "trustHandle of CreateTrustedDomain, state S562");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp473, "return of CreateTrustedDomain, state S562");
            this.Manager.Comment("reaching state \'S670\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp474;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp475;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainS" +
                    "upportedEncryptionTypes,out _)\'");
            temp475 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, out temp474);
            this.Manager.Comment("reaching state \'S778\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp474, "trustDomainInfo of QueryTrustedDomainInfoByName, state S778");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp475, "return of QueryTrustedDomainInfoByName, state S778");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S46() {
            this.Manager.BeginTest("TestScenarioS16S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S239\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp476;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp477;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp477 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp476);
            this.Manager.Comment("reaching state \'S347\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp476, "policyHandle of OpenPolicy2, state S347");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp477, "return of OpenPolicy2, state S347");
            this.Manager.Comment("reaching state \'S455\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp478;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp479;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp479 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp478);
            this.Manager.Comment("reaching state \'S563\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp478, "trustHandle of CreateTrustedDomain, state S563");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp479, "return of CreateTrustedDomain, state S563");
            this.Manager.Comment("reaching state \'S671\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp480;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp481;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedPosixOf" +
                    "fsetInformation,out _)\'");
            temp481 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, out temp480);
            this.Manager.Comment("reaching state \'S779\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp480, "trustDomainInfo of QueryTrustedDomainInfoByName, state S779");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp481, "return of QueryTrustedDomainInfoByName, state S779");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S48() {
            this.Manager.BeginTest("TestScenarioS16S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S240\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp482;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp483;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp483 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp482);
            this.Manager.Comment("reaching state \'S348\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp482, "policyHandle of OpenPolicy2, state S348");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp483, "return of OpenPolicy2, state S348");
            this.Manager.Comment("reaching state \'S456\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp484;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp485;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp485 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp484);
            this.Manager.Comment("reaching state \'S564\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp484, "trustHandle of CreateTrustedDomain, state S564");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp485, "return of CreateTrustedDomain, state S564");
            this.Manager.Comment("reaching state \'S672\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp486;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp487;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformation2Internal,out _)\'");
            temp487 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, out temp486);
            this.Manager.Comment("reaching state \'S780\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp486, "trustDomainInfo of QueryTrustedDomainInfoByName, state S780");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp487, "return of QueryTrustedDomainInfoByName, state S780");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S50() {
            this.Manager.BeginTest("TestScenarioS16S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S241\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp488;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp489;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp489 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp488);
            this.Manager.Comment("reaching state \'S349\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp488, "policyHandle of OpenPolicy2, state S349");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp489, "return of OpenPolicy2, state S349");
            this.Manager.Comment("reaching state \'S457\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp490;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp491;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp491 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp490);
            this.Manager.Comment("reaching state \'S565\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp490, "trustHandle of CreateTrustedDomain, state S565");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp491, "return of CreateTrustedDomain, state S565");
            this.Manager.Comment("reaching state \'S673\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp492;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp493;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformation,out _)\'");
            temp493 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, out temp492);
            this.Manager.Comment("reaching state \'S781\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp492, "trustDomainInfo of QueryTrustedDomainInfoByName, state S781");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp493, "return of QueryTrustedDomainInfoByName, state S781");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S52() {
            this.Manager.BeginTest("TestScenarioS16S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S242\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp494;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp495;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp495 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp494);
            this.Manager.Comment("reaching state \'S350\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp494, "policyHandle of OpenPolicy2, state S350");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp495, "return of OpenPolicy2, state S350");
            this.Manager.Comment("reaching state \'S458\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp496;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp497;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp497 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp496);
            this.Manager.Comment("reaching state \'S566\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp496, "trustHandle of CreateTrustedDomain, state S566");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp497, "return of CreateTrustedDomain, state S566");
            this.Manager.Comment("reaching state \'S674\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp498;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp499;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainI" +
                    "nformationEx,out _)\'");
            temp499 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, out temp498);
            this.Manager.Comment("reaching state \'S782\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp498, "trustDomainInfo of QueryTrustedDomainInfoByName, state S782");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp499, "return of QueryTrustedDomainInfoByName, state S782");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S54() {
            this.Manager.BeginTest("TestScenarioS16S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S243\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp500;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp501;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp501 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp500);
            this.Manager.Comment("reaching state \'S351\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp500, "policyHandle of OpenPolicy2, state S351");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp501, "return of OpenPolicy2, state S351");
            this.Manager.Comment("reaching state \'S459\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp502;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp503;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp503 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp502);
            this.Manager.Comment("reaching state \'S567\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp502, "trustHandle of CreateTrustedDomain, state S567");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp503, "return of CreateTrustedDomain, state S567");
            this.Manager.Comment("reaching state \'S675\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp504;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp505;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainN" +
                    "ameInformation,out _)\'");
            temp505 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), out temp504);
            this.Manager.Comment("reaching state \'S783\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp504, "trustDomainInfo of QueryTrustedDomainInfoByName, state S783");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp505, "return of QueryTrustedDomainInfoByName, state S783");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S56() {
            this.Manager.BeginTest("TestScenarioS16S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S244\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp506;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp507;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp507 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp506);
            this.Manager.Comment("reaching state \'S352\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp506, "policyHandle of OpenPolicy2, state S352");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp507, "return of OpenPolicy2, state S352");
            this.Manager.Comment("reaching state \'S460\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp508;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp509;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp509 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp508);
            this.Manager.Comment("reaching state \'S568\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp508, "trustHandle of CreateTrustedDomain, state S568");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp509, "return of CreateTrustedDomain, state S568");
            this.Manager.Comment("reaching state \'S676\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp510;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp511;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedControl" +
                    "lersInformation,out _)\'");
            temp511 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, out temp510);
            this.Manager.Comment("reaching state \'S784\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp510, "trustDomainInfo of QueryTrustedDomainInfoByName, state S784");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp511, "return of QueryTrustedDomainInfoByName, state S784");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S58() {
            this.Manager.BeginTest("TestScenarioS16S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S245\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp512;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp513;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp513 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp512);
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp512, "policyHandle of OpenPolicy2, state S353");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp513, "return of OpenPolicy2, state S353");
            this.Manager.Comment("reaching state \'S461\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp514;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp515;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp515 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp514);
            this.Manager.Comment("reaching state \'S569\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp514, "trustHandle of CreateTrustedDomain, state S569");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp515, "return of CreateTrustedDomain, state S569");
            this.Manager.Comment("reaching state \'S677\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp516;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp517;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformationInternal,out _)\'");
            temp517 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, out temp516);
            this.Manager.Comment("reaching state \'S785\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp516, "trustDomainInfo of QueryTrustedDomainInfoByName, state S785");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp517, "return of QueryTrustedDomainInfoByName, state S785");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S6() {
            this.Manager.BeginTest("TestScenarioS16S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S219\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp518;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp519;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp519 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp518);
            this.Manager.Comment("reaching state \'S327\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp518, "policyHandle of OpenPolicy2, state S327");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp519, "return of OpenPolicy2, state S327");
            this.Manager.Comment("reaching state \'S435\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp520;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp521;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp521 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp520);
            this.Manager.Comment("reaching state \'S543\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp520, "trustHandle of CreateTrustedDomain, state S543");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp521, "return of CreateTrustedDomain, state S543");
            this.Manager.Comment("reaching state \'S651\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp522;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp523;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformation,out _)\'");
            temp523 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, out temp522);
            this.Manager.Comment("reaching state \'S759\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp522, "trustDomainInfo of QueryTrustedDomainInfoByName, state S759");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp523, "return of QueryTrustedDomainInfoByName, state S759");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S60() {
            this.Manager.BeginTest("TestScenarioS16S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S246\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp524;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp525;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp525 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp524);
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp524, "policyHandle of OpenPolicy2, state S354");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp525, "return of OpenPolicy2, state S354");
            this.Manager.Comment("reaching state \'S462\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp526;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp527;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp527 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp526);
            this.Manager.Comment("reaching state \'S570\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp526, "trustHandle of CreateTrustedDomain, state S570");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp527, "return of CreateTrustedDomain, state S570");
            this.Manager.Comment("reaching state \'S678\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp528;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp529;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedPassw" +
                    "ordInformation,out _)\'");
            temp529 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, out temp528);
            this.Manager.Comment("reaching state \'S786\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp528, "trustDomainInfo of QueryTrustedDomainInfoByName, state S786");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp529, "return of QueryTrustedDomainInfoByName, state S786");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S62() {
            this.Manager.BeginTest("TestScenarioS16S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S247\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp530;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp531;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp531 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp530);
            this.Manager.Comment("reaching state \'S355\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp530, "policyHandle of OpenPolicy2, state S355");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp531, "return of OpenPolicy2, state S355");
            this.Manager.Comment("reaching state \'S463\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp532;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp533;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp533 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp532);
            this.Manager.Comment("reaching state \'S571\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp532, "trustHandle of CreateTrustedDomain, state S571");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp533, "return of CreateTrustedDomain, state S571");
            this.Manager.Comment("reaching state \'S679\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp534;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp535;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformation,out _)\'");
            temp535 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, out temp534);
            this.Manager.Comment("reaching state \'S787\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp534, "trustDomainInfo of QueryTrustedDomainInfoByName, state S787");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp535, "return of QueryTrustedDomainInfoByName, state S787");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S64() {
            this.Manager.BeginTest("TestScenarioS16S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S248\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp536;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp537;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp537 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp536);
            this.Manager.Comment("reaching state \'S356\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp536, "policyHandle of OpenPolicy2, state S356");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp537, "return of OpenPolicy2, state S356");
            this.Manager.Comment("reaching state \'S464\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp538;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp539;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp539 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp538);
            this.Manager.Comment("reaching state \'S572\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp538, "trustHandle of CreateTrustedDomain, state S572");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp539, "return of CreateTrustedDomain, state S572");
            this.Manager.Comment("reaching state \'S680\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp540;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp541;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationEx,out _)\'");
            temp541 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, out temp540);
            this.Manager.Comment("reaching state \'S788\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp540, "trustDomainInfo of QueryTrustedDomainInfoByName, state S788");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp541, "return of QueryTrustedDomainInfoByName, state S788");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S66() {
            this.Manager.BeginTest("TestScenarioS16S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S249\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp542;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp543;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp543 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp542);
            this.Manager.Comment("reaching state \'S357\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp542, "policyHandle of OpenPolicy2, state S357");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp543, "return of OpenPolicy2, state S357");
            this.Manager.Comment("reaching state \'S465\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp544;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp545;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp545 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp544);
            this.Manager.Comment("reaching state \'S573\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp544, "trustHandle of CreateTrustedDomain, state S573");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp545, "return of CreateTrustedDomain, state S573");
            this.Manager.Comment("reaching state \'S681\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp546;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp547;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nAuthInformationInternal,out _)\'");
            temp547 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, out temp546);
            this.Manager.Comment("reaching state \'S789\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp546, "trustDomainInfo of QueryTrustedDomainInfoByName, state S789");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp547, "return of QueryTrustedDomainInfoByName, state S789");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S68() {
            this.Manager.BeginTest("TestScenarioS16S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S250\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp548;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp549;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp549 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp548);
            this.Manager.Comment("reaching state \'S358\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp548, "policyHandle of OpenPolicy2, state S358");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp549, "return of OpenPolicy2, state S358");
            this.Manager.Comment("reaching state \'S466\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp550;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp551;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp551 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp550);
            this.Manager.Comment("reaching state \'S574\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp550, "trustHandle of CreateTrustedDomain, state S574");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp551, "return of CreateTrustedDomain, state S574");
            this.Manager.Comment("reaching state \'S682\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp552;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp553;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nSupportedEncryptionTypes,out _)\'");
            temp553 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, out temp552);
            this.Manager.Comment("reaching state \'S790\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp552, "trustDomainInfo of QueryTrustedDomainInfoByName, state S790");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp553, "return of QueryTrustedDomainInfoByName, state S790");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S70() {
            this.Manager.BeginTest("TestScenarioS16S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S251\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp554;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp555;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp555 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp554);
            this.Manager.Comment("reaching state \'S359\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp554, "policyHandle of OpenPolicy2, state S359");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp555, "return of OpenPolicy2, state S359");
            this.Manager.Comment("reaching state \'S467\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp556;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp557;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp557 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp556);
            this.Manager.Comment("reaching state \'S575\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp556, "trustHandle of CreateTrustedDomain, state S575");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp557, "return of CreateTrustedDomain, state S575");
            this.Manager.Comment("reaching state \'S683\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp558;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp559;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationBasic,out _)\'");
            temp559 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, out temp558);
            this.Manager.Comment("reaching state \'S791\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp558, "trustDomainInfo of QueryTrustedDomainInfoByName, state S791");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp559, "return of QueryTrustedDomainInfoByName, state S791");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S72
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S72() {
            this.Manager.BeginTest("TestScenarioS16S72");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S252\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp560;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp561;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp561 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp560);
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp560, "policyHandle of OpenPolicy2, state S360");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp561, "return of OpenPolicy2, state S360");
            this.Manager.Comment("reaching state \'S468\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp562;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp563;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp563 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp562);
            this.Manager.Comment("reaching state \'S576\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp562, "trustHandle of CreateTrustedDomain, state S576");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp563, "return of CreateTrustedDomain, state S576");
            this.Manager.Comment("reaching state \'S684\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp564;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp565;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nAuthInformation,out _)\'");
            temp565 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, out temp564);
            this.Manager.Comment("reaching state \'S792\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp564, "trustDomainInfo of QueryTrustedDomainInfoByName, state S792");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp565, "return of QueryTrustedDomainInfoByName, state S792");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S74
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S74() {
            this.Manager.BeginTest("TestScenarioS16S74");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S253\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp566;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp567;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp567 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp566);
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp566, "policyHandle of OpenPolicy2, state S361");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp567, "return of OpenPolicy2, state S361");
            this.Manager.Comment("reaching state \'S469\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp568;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp569;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp569 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp568);
            this.Manager.Comment("reaching state \'S577\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp568, "trustHandle of CreateTrustedDomain, state S577");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp569, "return of CreateTrustedDomain, state S577");
            this.Manager.Comment("reaching state \'S685\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp570;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp571;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nInformationEx2Internal,out _)\'");
            temp571 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, out temp570);
            this.Manager.Comment("reaching state \'S793\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp570, "trustDomainInfo of QueryTrustedDomainInfoByName, state S793");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp571, "return of QueryTrustedDomainInfoByName, state S793");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S76
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S76() {
            this.Manager.BeginTest("TestScenarioS16S76");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S254\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp572;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp573;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp573 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp572);
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp572, "policyHandle of OpenPolicy2, state S362");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp573, "return of OpenPolicy2, state S362");
            this.Manager.Comment("reaching state \'S470\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp574;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp575;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp575 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp574);
            this.Manager.Comment("reaching state \'S578\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp574, "trustHandle of CreateTrustedDomain, state S578");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp575, "return of CreateTrustedDomain, state S578");
            this.Manager.Comment("reaching state \'S686\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp576;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp577;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformationInternal,out _)\'");
            temp577 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, out temp576);
            this.Manager.Comment("reaching state \'S794\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp576, "trustDomainInfo of QueryTrustedDomainInfoByName, state S794");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp577, "return of QueryTrustedDomainInfoByName, state S794");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S78
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S78() {
            this.Manager.BeginTest("TestScenarioS16S78");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S255\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp578;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp579;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp579 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp578);
            this.Manager.Comment("reaching state \'S363\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp578, "policyHandle of OpenPolicy2, state S363");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp579, "return of OpenPolicy2, state S363");
            this.Manager.Comment("reaching state \'S471\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp580;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp581;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp581 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp580);
            this.Manager.Comment("reaching state \'S579\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp580, "trustHandle of CreateTrustedDomain, state S579");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp581, "return of CreateTrustedDomain, state S579");
            this.Manager.Comment("reaching state \'S687\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp582;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp583;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nNameInformation,out _)\'");
            temp583 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), out temp582);
            this.Manager.Comment("reaching state \'S795\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp582, "trustDomainInfo of QueryTrustedDomainInfoByName, state S795");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp583, "return of QueryTrustedDomainInfoByName, state S795");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S8() {
            this.Manager.BeginTest("TestScenarioS16S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S220\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp584;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp585;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp585 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp584);
            this.Manager.Comment("reaching state \'S328\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp584, "policyHandle of OpenPolicy2, state S328");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp585, "return of OpenPolicy2, state S328");
            this.Manager.Comment("reaching state \'S436\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp586;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp587;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp587 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp586);
            this.Manager.Comment("reaching state \'S544\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp586, "trustHandle of CreateTrustedDomain, state S544");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp587, "return of CreateTrustedDomain, state S544");
            this.Manager.Comment("reaching state \'S652\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp588;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp589;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Valid,TrustedPosixOf" +
                    "fsetInformation,out _)\'");
            temp589 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, out temp588);
            this.Manager.Comment("reaching state \'S760\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp588, "trustDomainInfo of QueryTrustedDomainInfoByName, state S760");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp589, "return of QueryTrustedDomainInfoByName, state S760");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S80
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S80() {
            this.Manager.BeginTest("TestScenarioS16S80");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S256\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp590;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp591;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp591 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp590);
            this.Manager.Comment("reaching state \'S364\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp590, "policyHandle of OpenPolicy2, state S364");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp591, "return of OpenPolicy2, state S364");
            this.Manager.Comment("reaching state \'S472\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp592;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp593;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp593 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp592);
            this.Manager.Comment("reaching state \'S580\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp592, "trustHandle of CreateTrustedDomain, state S580");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp593, "return of CreateTrustedDomain, state S580");
            this.Manager.Comment("reaching state \'S688\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp594;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp595;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedDomai" +
                    "nFullInformation2Internal,out _)\'");
            temp595 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, out temp594);
            this.Manager.Comment("reaching state \'S796\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp594, "trustDomainInfo of QueryTrustedDomainInfoByName, state S796");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp595, "return of QueryTrustedDomainInfoByName, state S796");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S82
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S82() {
            this.Manager.BeginTest("TestScenarioS16S82");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S257\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp596;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp597;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp597 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp596);
            this.Manager.Comment("reaching state \'S365\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp596, "policyHandle of OpenPolicy2, state S365");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp597, "return of OpenPolicy2, state S365");
            this.Manager.Comment("reaching state \'S473\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp598;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp599;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp599 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp598);
            this.Manager.Comment("reaching state \'S581\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp598, "trustHandle of CreateTrustedDomain, state S581");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp599, "return of CreateTrustedDomain, state S581");
            this.Manager.Comment("reaching state \'S689\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp600;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp601;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedPosix" +
                    "OffsetInformation,out _)\'");
            temp601 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, out temp600);
            this.Manager.Comment("reaching state \'S797\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp600, "trustDomainInfo of QueryTrustedDomainInfoByName, state S797");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp601, "return of QueryTrustedDomainInfoByName, state S797");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S84
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S84() {
            this.Manager.BeginTest("TestScenarioS16S84");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S258\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp602;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp603;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp603 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp602);
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp602, "policyHandle of OpenPolicy2, state S366");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp603, "return of OpenPolicy2, state S366");
            this.Manager.Comment("reaching state \'S474\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp604;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp605;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp605 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp604);
            this.Manager.Comment("reaching state \'S582\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp604, "trustHandle of CreateTrustedDomain, state S582");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp605, "return of CreateTrustedDomain, state S582");
            this.Manager.Comment("reaching state \'S690\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp606;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp607;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(1,\"Domain\",Invalid,TrustedDomai" +
                    "nAuthInformationInternal,out _)\'");
            temp607 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, out temp606);
            this.Manager.Comment("reaching state \'S798\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp606, "trustDomainInfo of QueryTrustedDomainInfoByName, state S798");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp607, "return of QueryTrustedDomainInfoByName, state S798");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S86
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S86() {
            this.Manager.BeginTest("TestScenarioS16S86");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S259\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp608;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp609;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp609 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp608);
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp608, "policyHandle of OpenPolicy2, state S367");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp609, "return of OpenPolicy2, state S367");
            this.Manager.Comment("reaching state \'S475\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp610;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp611;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp611 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp610);
            this.Manager.Comment("reaching state \'S583\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp610, "trustHandle of CreateTrustedDomain, state S583");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp611, "return of CreateTrustedDomain, state S583");
            this.Manager.Comment("reaching state \'S691\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp612;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp613;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainI" +
                    "nformationBasic,out _)\'");
            temp613 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, out temp612);
            this.Manager.Comment("reaching state \'S799\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp612, "trustDomainInfo of QueryTrustedDomainInfoByName, state S799");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp613, "return of QueryTrustedDomainInfoByName, state S799");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S88
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S88() {
            this.Manager.BeginTest("TestScenarioS16S88");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S260\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp614;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp615;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp615 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp614);
            this.Manager.Comment("reaching state \'S368\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp614, "policyHandle of OpenPolicy2, state S368");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp615, "return of OpenPolicy2, state S368");
            this.Manager.Comment("reaching state \'S476\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp616;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp617;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp617 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp616);
            this.Manager.Comment("reaching state \'S584\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp616, "trustHandle of CreateTrustedDomain, state S584");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp617, "return of CreateTrustedDomain, state S584");
            this.Manager.Comment("reaching state \'S692\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp618;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp619;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainA" +
                    "uthInformation,out _)\'");
            temp619 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, out temp618);
            this.Manager.Comment("reaching state \'S800\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp618, "trustDomainInfo of QueryTrustedDomainInfoByName, state S800");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp619, "return of QueryTrustedDomainInfoByName, state S800");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S90
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S90() {
            this.Manager.BeginTest("TestScenarioS16S90");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S261\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp620;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp621;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp621 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp620);
            this.Manager.Comment("reaching state \'S369\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp620, "policyHandle of OpenPolicy2, state S369");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp621, "return of OpenPolicy2, state S369");
            this.Manager.Comment("reaching state \'S477\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp622;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp623;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp623 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp622);
            this.Manager.Comment("reaching state \'S585\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp622, "trustHandle of CreateTrustedDomain, state S585");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp623, "return of CreateTrustedDomain, state S585");
            this.Manager.Comment("reaching state \'S693\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp624;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp625;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainI" +
                    "nformationEx2Internal,out _)\'");
            temp625 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, out temp624);
            this.Manager.Comment("reaching state \'S801\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp624, "trustDomainInfo of QueryTrustedDomainInfoByName, state S801");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp625, "return of QueryTrustedDomainInfoByName, state S801");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S92
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S92() {
            this.Manager.BeginTest("TestScenarioS16S92");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S262\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp626;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp627;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp627 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp626);
            this.Manager.Comment("reaching state \'S370\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp626, "policyHandle of OpenPolicy2, state S370");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp627, "return of OpenPolicy2, state S370");
            this.Manager.Comment("reaching state \'S478\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp628;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp629;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp629 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp628);
            this.Manager.Comment("reaching state \'S586\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp628, "trustHandle of CreateTrustedDomain, state S586");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp629, "return of CreateTrustedDomain, state S586");
            this.Manager.Comment("reaching state \'S694\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp630;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp631;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainF" +
                    "ullInformationInternal,out _)\'");
            temp631 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, out temp630);
            this.Manager.Comment("reaching state \'S802\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp630, "trustDomainInfo of QueryTrustedDomainInfoByName, state S802");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp631, "return of QueryTrustedDomainInfoByName, state S802");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S94
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S94() {
            this.Manager.BeginTest("TestScenarioS16S94");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S263\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp632;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp633;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp633 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp632);
            this.Manager.Comment("reaching state \'S371\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp632, "policyHandle of OpenPolicy2, state S371");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp633, "return of OpenPolicy2, state S371");
            this.Manager.Comment("reaching state \'S479\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp634;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp635;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp635 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp634);
            this.Manager.Comment("reaching state \'S587\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp634, "trustHandle of CreateTrustedDomain, state S587");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp635, "return of CreateTrustedDomain, state S587");
            this.Manager.Comment("reaching state \'S695\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp636;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp637;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedDomainA" +
                    "uthInformationInternal,out _)\'");
            temp637 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, out temp636);
            this.Manager.Comment("reaching state \'S803\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp636, "trustDomainInfo of QueryTrustedDomainInfoByName, state S803");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp637, "return of QueryTrustedDomainInfoByName, state S803");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S96
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S96() {
            this.Manager.BeginTest("TestScenarioS16S96");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S264\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp638;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp639;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp639 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp638);
            this.Manager.Comment("reaching state \'S372\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp638, "policyHandle of OpenPolicy2, state S372");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp639, "return of OpenPolicy2, state S372");
            this.Manager.Comment("reaching state \'S480\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp640;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp641;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp641 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp640);
            this.Manager.Comment("reaching state \'S588\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp640, "trustHandle of CreateTrustedDomain, state S588");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp641, "return of CreateTrustedDomain, state S588");
            this.Manager.Comment("reaching state \'S696\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp642;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp643;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Invalid,TrustedContr" +
                    "ollersInformation,out _)\'");
            temp643 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, out temp642);
            this.Manager.Comment("reaching state \'S804\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp642, "trustDomainInfo of QueryTrustedDomainInfoByName, state S804");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp643, "return of QueryTrustedDomainInfoByName, state S804");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S98
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS16S98() {
            this.Manager.BeginTest("TestScenarioS16S98");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S265\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp644;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp645;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp645 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp644);
            this.Manager.Comment("reaching state \'S373\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp644, "policyHandle of OpenPolicy2, state S373");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp645, "return of OpenPolicy2, state S373");
            this.Manager.Comment("reaching state \'S481\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp646;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp647;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,65663,out _)\'");
            temp647 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 65663u, out temp646);
            this.Manager.Comment("reaching state \'S589\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp646, "trustHandle of CreateTrustedDomain, state S589");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp647, "return of CreateTrustedDomain, state S589");
            this.Manager.Comment("reaching state \'S697\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp648;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp649;
            this.Manager.Comment("executing step \'call QueryTrustedDomainInfoByName(2,\"Domain\",Valid,TrustedPasswor" +
                    "dInformation,out _)\'");
            temp649 = this.ILsadManagedAdapterInstance.QueryTrustedDomainInfoByName(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, out temp648);
            this.Manager.Comment("reaching state \'S805\'");
            this.Manager.Comment("checking step \'return QueryTrustedDomainInfoByName/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp648, "trustDomainInfo of QueryTrustedDomainInfoByName, state S805");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp649, "return of QueryTrustedDomainInfoByName, state S805");
            TestScenarioS16S865();
            this.Manager.EndTest();
        }
        #endregion
    }
}
