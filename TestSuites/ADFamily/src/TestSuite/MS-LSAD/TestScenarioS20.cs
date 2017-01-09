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
    public partial class TestScenarioS20 : PtfTestClassBase {
        
        public TestScenarioS20() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void DeleteTrustedDomainDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase DeleteTrustedDomainInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "DeleteTrustedDomain", typeof(int), typeof(string), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid));
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
        public void LSAD_TestScenarioS20S0() {
            this.Manager.BeginTest("TestScenarioS20S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S72");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S72");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp3 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp2);
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp2, "trustHandle of CreateTrustedDomainEx, state S120");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp3, "return of CreateTrustedDomainEx, state S120");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(2,\"Domain10\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp4);
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp4, "collisionInfo of SetForestTrustInformation, state S168");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp5, "return of SetForestTrustInformation, state S168");
            this.Manager.Comment("reaching state \'S192\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp6);
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp6, "trustInfo of QueryForestTrustInformation, state S216");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp7, "return of QueryForestTrustInformation, state S216");
            TestScenarioS20S240();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS20S240() {
            this.Manager.Comment("reaching state \'S240\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp8 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.AddReturn(DeleteTrustedDomainInfo, null, temp8);
            TestScenarioS20S243();
        }
        
        private void TestScenarioS20S243() {
            this.Manager.Comment("reaching state \'S243\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS20.DeleteTrustedDomainInfo, null, new DeleteTrustedDomainDelegate1(this.TestScenarioS20S0DeleteTrustedDomainChecker)));
            this.Manager.Comment("reaching state \'S245\'");
        }
        
        private void TestScenarioS20S0DeleteTrustedDomainChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of DeleteTrustedDomain, state S243");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S10() {
            this.Manager.BeginTest("TestScenarioS20S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S53\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp9;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp10 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp9);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp9, "policyHandle of OpenPolicy2, state S77");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp10, "return of OpenPolicy2, state S77");
            this.Manager.Comment("reaching state \'S101\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp11;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp12;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=1),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp12 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            1u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp11);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp11, "trustHandle of CreateTrustedDomainEx, state S125");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp12, "return of CreateTrustedDomainEx, state S125");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp13;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp14;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(2,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp14 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp13);
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp13, "collisionInfo of SetForestTrustInformation, state S173");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp14, "return of SetForestTrustInformation, state S173");
            this.Manager.Comment("reaching state \'S197\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp15;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp16;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp16 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp15);
            this.Manager.Comment("reaching state \'S221\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp15, "trustInfo of QueryForestTrustInformation, state S221");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp16, "return of QueryForestTrustInformation, state S221");
            TestScenarioS20S241();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS20S241() {
            this.Manager.Comment("reaching state \'S241\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp17 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.AddReturn(DeleteTrustedDomainInfo, null, temp17);
            TestScenarioS20S243();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S12() {
            this.Manager.BeginTest("TestScenarioS20S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp19 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp18);
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp18, "policyHandle of OpenPolicy2, state S78");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp19, "return of OpenPolicy2, state S78");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=1),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp21 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            1u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp20);
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp20, "trustHandle of CreateTrustedDomainEx, state S126");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp21, "return of CreateTrustedDomainEx, state S126");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(2,\"Domain\",Invalid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp22);
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp22, "collisionInfo of SetForestTrustInformation, state S174");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp23, "return of SetForestTrustInformation, state S174");
            this.Manager.Comment("reaching state \'S198\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp24);
            this.Manager.Comment("reaching state \'S222\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp24, "trustInfo of QueryForestTrustInformation, state S222");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp25, "return of QueryForestTrustInformation, state S222");
            TestScenarioS20S241();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S14() {
            this.Manager.BeginTest("TestScenarioS20S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S55\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp26);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy2, state S79");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy2, state S79");
            this.Manager.Comment("reaching state \'S103\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=1),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp29 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            1u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp28);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp28, "trustHandle of CreateTrustedDomainEx, state S127");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of CreateTrustedDomainEx, state S127");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(2,\"Domain10\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp31 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp30);
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp30, "collisionInfo of SetForestTrustInformation, state S175");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp31, "return of SetForestTrustInformation, state S175");
            this.Manager.Comment("reaching state \'S199\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain10\",Invalid,2,1,DS_BEHA" +
                    "VIOR_WIN2003,True,out _)\'");
            temp33 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp32);
            this.Manager.Comment("reaching state \'S223\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp32, "trustInfo of QueryForestTrustInformation, state S223");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp33, "return of QueryForestTrustInformation, state S223");
            TestScenarioS20S241();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S16() {
            this.Manager.BeginTest("TestScenarioS20S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp35 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp34);
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp34, "policyHandle of OpenPolicy2, state S80");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp35, "return of OpenPolicy2, state S80");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp37 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp36);
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp36, "trustHandle of CreateTrustedDomainEx, state S128");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp37, "return of CreateTrustedDomainEx, state S128");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp38);
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(0)), temp38, "collisionInfo of SetForestTrustInformation, state S176");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp39, "return of SetForestTrustInformation, state S176");
            this.Manager.Comment("reaching state \'S200\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp40);
            this.Manager.Comment("reaching state \'S224\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(0)), temp40, "trustInfo of QueryForestTrustInformation, state S224");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of QueryForestTrustInformation, state S224");
            TestScenarioS20S242();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS20S242() {
            this.Manager.Comment("reaching state \'S242\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp42;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp42 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.Comment("reaching state \'S244\'");
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp42, "return of DeleteTrustedDomain, state S244");
            this.Manager.Comment("reaching state \'S246\'");
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S18() {
            this.Manager.BeginTest("TestScenarioS20S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S57\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp43;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp44;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp44 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp43);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp43, "policyHandle of OpenPolicy2, state S81");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp44, "return of OpenPolicy2, state S81");
            this.Manager.Comment("reaching state \'S105\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp45;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp46;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp46 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp45);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp45, "trustHandle of CreateTrustedDomainEx, state S129");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp46, "return of CreateTrustedDomainEx, state S129");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp47;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp48;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp48 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp47);
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(0)), temp47, "collisionInfo of SetForestTrustInformation, state S177");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp48, "return of SetForestTrustInformation, state S177");
            this.Manager.Comment("reaching state \'S201\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp49;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp50;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp50 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp49);
            this.Manager.Comment("reaching state \'S225\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:InvalidParameter\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp49, "trustInfo of QueryForestTrustInformation, state S225");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp50, "return of QueryForestTrustInformation, state S225");
            TestScenarioS20S242();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S2() {
            this.Manager.BeginTest("TestScenarioS20S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S49\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp51;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp52;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp52 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp51);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp51, "policyHandle of OpenPolicy2, state S73");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp52, "return of OpenPolicy2, state S73");
            this.Manager.Comment("reaching state \'S97\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp53;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp54;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=1),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp54 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            1u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp53);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp53, "trustHandle of CreateTrustedDomainEx, state S121");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp54, "return of CreateTrustedDomainEx, state S121");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp55;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp56;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp56 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp55);
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp55, "collisionInfo of SetForestTrustInformation, state S169");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp56, "return of SetForestTrustInformation, state S169");
            this.Manager.Comment("reaching state \'S193\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp57;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp58;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp58 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp57);
            this.Manager.Comment("reaching state \'S217\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:InvalidParameter\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp57, "trustInfo of QueryForestTrustInformation, state S217");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp58, "return of QueryForestTrustInformation, state S217");
            TestScenarioS20S241();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S20() {
            this.Manager.BeginTest("TestScenarioS20S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp59;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp60;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp60 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp59);
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp59, "policyHandle of OpenPolicy2, state S82");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp60, "return of OpenPolicy2, state S82");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp61;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp62;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp62 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp61);
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp61, "trustHandle of CreateTrustedDomainEx, state S130");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp62, "return of CreateTrustedDomainEx, state S130");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp63;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp64;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp64 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp63);
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(0)), temp63, "collisionInfo of SetForestTrustInformation, state S178");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp64, "return of SetForestTrustInformation, state S178");
            this.Manager.Comment("reaching state \'S202\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp65;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp66;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain10\",Valid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp66 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp65);
            this.Manager.Comment("reaching state \'S226\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:NoSuchDomain\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp65, "trustInfo of QueryForestTrustInformation, state S226");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchDomain, temp66, "return of QueryForestTrustInformation, state S226");
            TestScenarioS20S242();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S22() {
            this.Manager.BeginTest("TestScenarioS20S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S59\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp67;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp68;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp68 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp67);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp67, "policyHandle of OpenPolicy2, state S83");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp68, "return of OpenPolicy2, state S83");
            this.Manager.Comment("reaching state \'S107\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp69;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp70;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp70 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp69);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp69, "trustHandle of CreateTrustedDomainEx, state S131");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp70, "return of CreateTrustedDomainEx, state S131");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp71;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp72;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp72 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp71);
            this.Manager.Comment("reaching state \'S179\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(0)), temp71, "collisionInfo of SetForestTrustInformation, state S179");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp72, "return of SetForestTrustInformation, state S179");
            this.Manager.Comment("reaching state \'S203\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp73;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp74;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp74 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp73);
            this.Manager.Comment("reaching state \'S227\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp73, "trustInfo of QueryForestTrustInformation, state S227");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp74, "return of QueryForestTrustInformation, state S227");
            TestScenarioS20S242();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S24() {
            this.Manager.BeginTest("TestScenarioS20S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp75;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp76;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp76 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp75);
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp75, "policyHandle of OpenPolicy2, state S84");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp76, "return of OpenPolicy2, state S84");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp77;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp78;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp78 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp77);
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp77, "trustHandle of CreateTrustedDomainEx, state S132");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp78, "return of CreateTrustedDomainEx, state S132");
            this.Manager.Comment("reaching state \'S156\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp79;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp80;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp80 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp79);
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(0)), temp79, "collisionInfo of SetForestTrustInformation, state S180");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp80, "return of SetForestTrustInformation, state S180");
            this.Manager.Comment("reaching state \'S204\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp81;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp82;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain10\",Invalid,2,1,DS_BEHA" +
                    "VIOR_WIN2003,True,out _)\'");
            temp82 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp81);
            this.Manager.Comment("reaching state \'S228\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp81, "trustInfo of QueryForestTrustInformation, state S228");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp82, "return of QueryForestTrustInformation, state S228");
            TestScenarioS20S242();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S26() {
            this.Manager.BeginTest("TestScenarioS20S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S61\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp83;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp84;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp84 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp83);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp83, "policyHandle of OpenPolicy2, state S85");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp84, "return of OpenPolicy2, state S85");
            this.Manager.Comment("reaching state \'S109\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp85;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp86;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp86 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp85);
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp85, "trustHandle of CreateTrustedDomainEx, state S133");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp86, "return of CreateTrustedDomainEx, state S133");
            this.Manager.Comment("reaching state \'S157\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp87;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp88;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp88 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp87);
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(0)), temp87, "collisionInfo of SetForestTrustInformation, state S181");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp88, "return of SetForestTrustInformation, state S181");
            this.Manager.Comment("reaching state \'S205\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp89;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp90;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain10\",Valid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp90 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp89);
            this.Manager.Comment("reaching state \'S229\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp89, "trustInfo of QueryForestTrustInformation, state S229");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp90, "return of QueryForestTrustInformation, state S229");
            TestScenarioS20S242();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S28() {
            this.Manager.BeginTest("TestScenarioS20S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp91;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp92;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp92 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp91);
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp91, "policyHandle of OpenPolicy2, state S86");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp92, "return of OpenPolicy2, state S86");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp93;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp94;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp94 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp93);
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp93, "trustHandle of CreateTrustedDomainEx, state S134");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp94, "return of CreateTrustedDomainEx, state S134");
            this.Manager.Comment("reaching state \'S158\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp95;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp96;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp96 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp95);
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(0)), temp95, "collisionInfo of SetForestTrustInformation, state S182");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp96, "return of SetForestTrustInformation, state S182");
            this.Manager.Comment("reaching state \'S206\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp97;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp98;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp98 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp97);
            this.Manager.Comment("reaching state \'S230\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp97, "trustInfo of QueryForestTrustInformation, state S230");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp98, "return of QueryForestTrustInformation, state S230");
            TestScenarioS20S242();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S30() {
            this.Manager.BeginTest("TestScenarioS20S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S63\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp99;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp100;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp100 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp99);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp99, "policyHandle of OpenPolicy2, state S87");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp100, "return of OpenPolicy2, state S87");
            this.Manager.Comment("reaching state \'S111\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp101;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp102;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp102 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp101);
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp101, "trustHandle of CreateTrustedDomainEx, state S135");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp102, "return of CreateTrustedDomainEx, state S135");
            this.Manager.Comment("reaching state \'S159\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp103;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp104;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Invalid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp104 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp103);
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp103, "collisionInfo of SetForestTrustInformation, state S183");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp104, "return of SetForestTrustInformation, state S183");
            this.Manager.Comment("reaching state \'S207\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp105;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp106;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain10\",Valid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp106 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp105);
            this.Manager.Comment("reaching state \'S231\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:NoSuchDomain\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp105, "trustInfo of QueryForestTrustInformation, state S231");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchDomain, temp106, "return of QueryForestTrustInformation, state S231");
            TestScenarioS20S240();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S32() {
            this.Manager.BeginTest("TestScenarioS20S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp107;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp108;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp108 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp107);
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp107, "policyHandle of OpenPolicy2, state S88");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp108, "return of OpenPolicy2, state S88");
            this.Manager.Comment("reaching state \'S112\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp109;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp110;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp110 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp109);
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp109, "trustHandle of CreateTrustedDomainEx, state S136");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp110, "return of CreateTrustedDomainEx, state S136");
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp111;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp112;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Invalid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp112 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp111);
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp111, "collisionInfo of SetForestTrustInformation, state S184");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp112, "return of SetForestTrustInformation, state S184");
            this.Manager.Comment("reaching state \'S208\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp113;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp114;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp114 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp113);
            this.Manager.Comment("reaching state \'S232\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:NotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp113, "trustInfo of QueryForestTrustInformation, state S232");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotFound, temp114, "return of QueryForestTrustInformation, state S232");
            TestScenarioS20S240();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S34() {
            this.Manager.BeginTest("TestScenarioS20S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S65\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp115;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp116;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp116 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp115);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp115, "policyHandle of OpenPolicy2, state S89");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp116, "return of OpenPolicy2, state S89");
            this.Manager.Comment("reaching state \'S113\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp117;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp118;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp118 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp117);
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp117, "trustHandle of CreateTrustedDomainEx, state S137");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp118, "return of CreateTrustedDomainEx, state S137");
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp119;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp120;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain10\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp120 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp119);
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:NoSuchDomain\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp119, "collisionInfo of SetForestTrustInformation, state S185");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchDomain, temp120, "return of SetForestTrustInformation, state S185");
            this.Manager.Comment("reaching state \'S209\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp121;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp122;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain10\",Invalid,2,1,DS_BEHA" +
                    "VIOR_WIN2003,True,out _)\'");
            temp122 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp121);
            this.Manager.Comment("reaching state \'S233\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp121, "trustInfo of QueryForestTrustInformation, state S233");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp122, "return of QueryForestTrustInformation, state S233");
            TestScenarioS20S240();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S36() {
            this.Manager.BeginTest("TestScenarioS20S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp123;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp124;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp124 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp123);
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp123, "policyHandle of OpenPolicy2, state S90");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp124, "return of OpenPolicy2, state S90");
            this.Manager.Comment("reaching state \'S114\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp125;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp126;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp126 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp125);
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp125, "trustHandle of CreateTrustedDomainEx, state S138");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp126, "return of CreateTrustedDomainEx, state S138");
            this.Manager.Comment("reaching state \'S162\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp127;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp128;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(2,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp128 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp127);
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp127, "collisionInfo of SetForestTrustInformation, state S186");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp128, "return of SetForestTrustInformation, state S186");
            this.Manager.Comment("reaching state \'S210\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp129;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp130;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp130 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp129);
            this.Manager.Comment("reaching state \'S234\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp129, "trustInfo of QueryForestTrustInformation, state S234");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp130, "return of QueryForestTrustInformation, state S234");
            TestScenarioS20S240();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S38() {
            this.Manager.BeginTest("TestScenarioS20S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S67\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp131;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp132;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp132 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp131);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp131, "policyHandle of OpenPolicy2, state S91");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp132, "return of OpenPolicy2, state S91");
            this.Manager.Comment("reaching state \'S115\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp133;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp134;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp134 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp133);
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp133, "trustHandle of CreateTrustedDomainEx, state S139");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp134, "return of CreateTrustedDomainEx, state S139");
            this.Manager.Comment("reaching state \'S163\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp135;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp136;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain10\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp136 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp135);
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp135, "collisionInfo of SetForestTrustInformation, state S187");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp136, "return of SetForestTrustInformation, state S187");
            this.Manager.Comment("reaching state \'S211\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp137;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp138;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain10\",Valid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp138 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp137);
            this.Manager.Comment("reaching state \'S235\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp137, "trustInfo of QueryForestTrustInformation, state S235");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp138, "return of QueryForestTrustInformation, state S235");
            TestScenarioS20S240();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S4() {
            this.Manager.BeginTest("TestScenarioS20S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp139;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp140;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp140 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp139);
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp139, "policyHandle of OpenPolicy2, state S74");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp140, "return of OpenPolicy2, state S74");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp141;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp142;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=1),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp142 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            1u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp141);
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp141, "trustHandle of CreateTrustedDomainEx, state S122");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp142, "return of CreateTrustedDomainEx, state S122");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp143;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp144;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Invalid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp144 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp143);
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp143, "collisionInfo of SetForestTrustInformation, state S170");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp144, "return of SetForestTrustInformation, state S170");
            this.Manager.Comment("reaching state \'S194\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp145;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp146;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp146 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp145);
            this.Manager.Comment("reaching state \'S218\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:InvalidParameter\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp145, "trustInfo of QueryForestTrustInformation, state S218");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp146, "return of QueryForestTrustInformation, state S218");
            TestScenarioS20S241();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S40() {
            this.Manager.BeginTest("TestScenarioS20S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp147;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp148;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp148 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp147);
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp147, "policyHandle of OpenPolicy2, state S92");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp148, "return of OpenPolicy2, state S92");
            this.Manager.Comment("reaching state \'S116\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp149;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp150;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp150 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp149);
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp149, "trustHandle of CreateTrustedDomainEx, state S140");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp150, "return of CreateTrustedDomainEx, state S140");
            this.Manager.Comment("reaching state \'S164\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp151;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp152;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(2,\"Domain10\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp152 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp151);
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp151, "collisionInfo of SetForestTrustInformation, state S188");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp152, "return of SetForestTrustInformation, state S188");
            this.Manager.Comment("reaching state \'S212\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp153;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp154;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp154 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp153);
            this.Manager.Comment("reaching state \'S236\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp153, "trustInfo of QueryForestTrustInformation, state S236");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp154, "return of QueryForestTrustInformation, state S236");
            TestScenarioS20S240();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S42() {
            this.Manager.BeginTest("TestScenarioS20S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S69\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp155;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp156;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp156 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp155);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp155, "policyHandle of OpenPolicy2, state S93");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp156, "return of OpenPolicy2, state S93");
            this.Manager.Comment("reaching state \'S117\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp157;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp158;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp158 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp157);
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp157, "trustHandle of CreateTrustedDomainEx, state S141");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp158, "return of CreateTrustedDomainEx, state S141");
            this.Manager.Comment("reaching state \'S165\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp159;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp160;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(2,\"Domain\",Invalid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp160 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(2, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp159);
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp159, "collisionInfo of SetForestTrustInformation, state S189");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp160, "return of SetForestTrustInformation, state S189");
            this.Manager.Comment("reaching state \'S213\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp161;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp162;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain10\",Invalid,2,1,DS_BEHA" +
                    "VIOR_WIN2003,True,out _)\'");
            temp162 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp161);
            this.Manager.Comment("reaching state \'S237\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp161, "trustInfo of QueryForestTrustInformation, state S237");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp162, "return of QueryForestTrustInformation, state S237");
            TestScenarioS20S240();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S44() {
            this.Manager.BeginTest("TestScenarioS20S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp163;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp164;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp164 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp163);
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp163, "policyHandle of OpenPolicy2, state S94");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp164, "return of OpenPolicy2, state S94");
            this.Manager.Comment("reaching state \'S118\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp165;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp166;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp166 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp165);
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp165, "trustHandle of CreateTrustedDomainEx, state S142");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp166, "return of CreateTrustedDomainEx, state S142");
            this.Manager.Comment("reaching state \'S166\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp167;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp168;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain\",Valid,2,1,DS_BEHAVIOR_W" +
                    "IN2003,True,out _)\'");
            temp168 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp167);
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(0)), temp167, "collisionInfo of SetForestTrustInformation, state S190");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp168, "return of SetForestTrustInformation, state S190");
            this.Manager.Comment("reaching state \'S214\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp169;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp170;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain10\",Invalid,2,1,DS_BEHA" +
                    "VIOR_WIN2003,True,out _)\'");
            temp170 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp169);
            this.Manager.Comment("reaching state \'S238\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp169, "trustInfo of QueryForestTrustInformation, state S238");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp170, "return of QueryForestTrustInformation, state S238");
            TestScenarioS20S242();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S46() {
            this.Manager.BeginTest("TestScenarioS20S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S71\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp171;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp172;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp172 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp171);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp171, "policyHandle of OpenPolicy2, state S95");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp172, "return of OpenPolicy2, state S95");
            this.Manager.Comment("reaching state \'S119\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp173;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp174;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=1),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp174 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            1u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp173);
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp173, "trustHandle of CreateTrustedDomainEx, state S143");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp174, "return of CreateTrustedDomainEx, state S143");
            this.Manager.Comment("reaching state \'S167\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp175;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp176;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(2,\"Domain10\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp176 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp175);
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp175, "collisionInfo of SetForestTrustInformation, state S191");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp176, "return of SetForestTrustInformation, state S191");
            this.Manager.Comment("reaching state \'S215\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp177;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp178;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(2,\"Domain10\",Valid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp178 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(2, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp177);
            this.Manager.Comment("reaching state \'S239\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp177, "trustInfo of QueryForestTrustInformation, state S239");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp178, "return of QueryForestTrustInformation, state S239");
            TestScenarioS20S241();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S6() {
            this.Manager.BeginTest("TestScenarioS20S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S51\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp179;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp180;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp180 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp179);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp179, "policyHandle of OpenPolicy2, state S75");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp180, "return of OpenPolicy2, state S75");
            this.Manager.Comment("reaching state \'S99\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp181;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp182;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=1),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp182 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            1u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp181);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp181, "trustHandle of CreateTrustedDomainEx, state S123");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp182, "return of CreateTrustedDomainEx, state S123");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp183;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp184;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain10\",Valid,2,1,DS_BEHAVIOR" +
                    "_WIN2003,True,out _)\'");
            temp184 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp183);
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:NoSuchDomain\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp183, "collisionInfo of SetForestTrustInformation, state S171");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchDomain, temp184, "return of SetForestTrustInformation, state S171");
            this.Manager.Comment("reaching state \'S195\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp185;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp186;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain10\",Valid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp186 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp185);
            this.Manager.Comment("reaching state \'S219\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:NoSuchDomain\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp185, "trustInfo of QueryForestTrustInformation, state S219");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchDomain, temp186, "return of QueryForestTrustInformation, state S219");
            TestScenarioS20S241();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS20S8() {
            this.Manager.BeginTest("TestScenarioS20S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp187;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp188;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp188 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp187);
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp187, "policyHandle of OpenPolicy2, state S76");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp188, "return of OpenPolicy2, state S76");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp189;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp190;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=1),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp190 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            1u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp189);
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp189, "trustHandle of CreateTrustedDomainEx, state S124");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp190, "return of CreateTrustedDomainEx, state S124");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo temp191;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp192;
            this.Manager.Comment("executing step \'call SetForestTrustInformation(1,\"Domain10\",Invalid,2,1,DS_BEHAVI" +
                    "OR_WIN2003,True,out _)\'");
            temp192 = this.ILsadManagedAdapterInstance.SetForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp191);
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("checking step \'return SetForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.CollisionInfo)(1)), temp191, "collisionInfo of SetForestTrustInformation, state S172");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp192, "return of SetForestTrustInformation, state S172");
            this.Manager.Comment("reaching state \'S196\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo temp193;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp194;
            this.Manager.Comment("executing step \'call QueryForestTrustInformation(1,\"Domain10\",Invalid,2,1,DS_BEHA" +
                    "VIOR_WIN2003,True,out _)\'");
            temp194 = this.ILsadManagedAdapterInstance.QueryForestTrustInformation(1, "Domain10", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), 2, 1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, out temp193);
            this.Manager.Comment("reaching state \'S220\'");
            this.Manager.Comment("checking step \'return QueryForestTrustInformation/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestTrustInfo)(1)), temp193, "trustInfo of QueryForestTrustInformation, state S220");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp194, "return of QueryForestTrustInformation, state S220");
            TestScenarioS20S241();
            this.Manager.EndTest();
        }
        #endregion
    }
}
