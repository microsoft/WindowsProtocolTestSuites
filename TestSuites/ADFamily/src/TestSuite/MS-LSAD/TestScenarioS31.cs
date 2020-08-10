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
    public partial class TestScenarioS31 : PtfTestClassBase {
        
        public TestScenarioS31() {
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
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS31S0() {
            this.Manager.BeginTest("TestScenarioS31S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S6");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S6");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp2);
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:DirectoryServiceRequired\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp2, "trustHandle of CreateTrustedDomain, state S10");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.DirectoryServiceRequired, temp3, "return of CreateTrustedDomain, state S10");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=2),Valid,Valid,DS_BEHAVIOR_WIN2003,False,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp5 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "NetBiosName",
                            2u,
                            2u,
                            2u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), false, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp4);
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:DirectoryServiceRequire" +
                    "d\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp4, "trustHandle of CreateTrustedDomainEx, state S14");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.DirectoryServiceRequired, temp5, "return of CreateTrustedDomainEx, state S14");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx2(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=2),Valid,Valid,DS_BEHAVIOR_WIN2003,True,4061069327,out _)'");
            temp7 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx2(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "NetBiosName",
                            2u,
                            2u,
                            2u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 4061069327u, out temp6);
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx2/[out Invalid]:DirectoryServiceRequir" +
                    "ed\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp6, "trustHandle of CreateTrustedDomainEx2, state S18");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.DirectoryServiceRequired, temp7, "return of CreateTrustedDomainEx2, state S18");
            TestScenarioS31S20();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS31S20() {
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call OpenTrustedDomain(1,\"DomainSid\",Valid,True,out _)\'");
            temp9 = this.ILsadManagedAdapterInstance.OpenTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, out temp8);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return OpenTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp8, "trustHandle of OpenTrustedDomain, state S21");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp9, "return of OpenTrustedDomain, state S21");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp10);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp10, "collisionInfo of SetForestTrustInformation, state S23");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp11, "return of SetForestTrustInformation, state S23");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp12);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp12, "trustInfo of QueryForestTrustInformation, state S25");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp13, "return of QueryForestTrustInformation, state S25");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp14;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp14 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp14, "return of DeleteTrustedDomain, state S27");
            this.Manager.Comment("reaching state \'S28\'");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("DM")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS31S2() {
            this.Manager.BeginTest("TestScenarioS31S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(NonDomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S5\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp15;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp16;
            this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
            temp16 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp15);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp15, "policyHandle of OpenPolicy, state S7");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp16, "return of OpenPolicy, state S7");
            this.Manager.Comment("reaching state \'S9\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp17;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp18;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp18 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:DirectoryServiceRequired\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp17, "trustHandle of CreateTrustedDomain, state S11");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.DirectoryServiceRequired, temp18, "return of CreateTrustedDomain, state S11");
            this.Manager.Comment("reaching state \'S13\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp19;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp20;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,False,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp20 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "NetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), false, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp19);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:DirectoryServiceRequire" +
                    "d\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp19, "trustHandle of CreateTrustedDomainEx, state S15");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.DirectoryServiceRequired, temp20, "return of CreateTrustedDomainEx, state S15");
            this.Manager.Comment("reaching state \'S17\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp21;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp22;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx2(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,4061069327,out _)'");
            temp22 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx2(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "NetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 4061069327u, out temp21);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx2/[out Invalid]:DirectoryServiceRequir" +
                    "ed\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp21, "trustHandle of CreateTrustedDomainEx2, state S19");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.DirectoryServiceRequired, temp22, "return of CreateTrustedDomainEx2, state S19");
            TestScenarioS31S20();
            this.Manager.EndTest();
        }
        #endregion
    }
}
