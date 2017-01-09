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
    public partial class TestScenarioS18 : PtfTestClassBase {
        
        public TestScenarioS18() {
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
        public void LSAD_TestScenarioS18S0() {
            this.Manager.BeginTest("TestScenarioS18S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S162");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S162");
            this.Manager.Comment("reaching state \'S216\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp3 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp2);
            this.Manager.Comment("reaching state \'S270\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp2, "trustHandle of CreateTrustedDomainEx, state S270");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp3, "return of CreateTrustedDomainEx, state S270");
            this.Manager.Comment("reaching state \'S324\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainAuthInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp4);
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp4, "trustDomainInfo of QueryInfoTrustedDomain, state S378");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp5, "return of QueryInfoTrustedDomain, state S378");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS18S432() {
            this.Manager.Comment("reaching state \'S432\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp6 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.Comment("reaching state \'S434\'");
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/NoSuchDomain\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchDomain, temp6, "return of DeleteTrustedDomain, state S434");
            this.Manager.Comment("reaching state \'S436\'");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S10() {
            this.Manager.BeginTest("TestScenarioS18S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S113\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp8 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp7);
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp7, "policyHandle of OpenPolicy2, state S167");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp8, "return of OpenPolicy2, state S167");
            this.Manager.Comment("reaching state \'S221\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp9;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp10 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp9);
            this.Manager.Comment("reaching state \'S275\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp9, "trustHandle of CreateTrustedDomainEx, state S275");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp10, "return of CreateTrustedDomainEx, state S275");
            this.Manager.Comment("reaching state \'S329\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp11;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp12;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainNameInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp12 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp11);
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp11, "trustDomainInfo of QueryInfoTrustedDomain, state S383");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp12, "return of QueryInfoTrustedDomain, state S383");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS18S433() {
            this.Manager.Comment("reaching state \'S433\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp13 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.Comment("reaching state \'S435\'");
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp13, "return of DeleteTrustedDomain, state S435");
            this.Manager.Comment("reaching state \'S437\'");
        }
        #endregion
        
        #region Test Starting in S100
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S100() {
            this.Manager.BeginTest("TestScenarioS18S100");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S158\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp15 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp14);
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp14, "policyHandle of OpenPolicy2, state S212");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp15, "return of OpenPolicy2, state S212");
            this.Manager.Comment("reaching state \'S266\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp17 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp16);
            this.Manager.Comment("reaching state \'S320\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp16, "trustHandle of CreateTrustedDomainEx, state S320");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp17, "return of CreateTrustedDomainEx, state S320");
            this.Manager.Comment("reaching state \'S374\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedControllersInformation,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp19 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp18);
            this.Manager.Comment("reaching state \'S428\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp18, "trustDomainInfo of QueryInfoTrustedDomain, state S428");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp19, "return of QueryInfoTrustedDomain, state S428");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S102
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S102() {
            this.Manager.BeginTest("TestScenarioS18S102");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S159\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp20;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp21 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp20);
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp20, "policyHandle of OpenPolicy2, state S213");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp21, "return of OpenPolicy2, state S213");
            this.Manager.Comment("reaching state \'S267\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp23 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp22);
            this.Manager.Comment("reaching state \'S321\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp22, "trustHandle of CreateTrustedDomainEx, state S321");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp23, "return of CreateTrustedDomainEx, state S321");
            this.Manager.Comment("reaching state \'S375\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainInformationEx,DS_BEHAV" +
                    "IOR_WIN2003,out _)\'");
            temp25 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp24);
            this.Manager.Comment("reaching state \'S429\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp24, "trustDomainInfo of QueryInfoTrustedDomain, state S429");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp25, "return of QueryInfoTrustedDomain, state S429");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S104
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S104() {
            this.Manager.BeginTest("TestScenarioS18S104");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp26);
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy2, state S214");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy2, state S214");
            this.Manager.Comment("reaching state \'S268\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp28);
            this.Manager.Comment("reaching state \'S322\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp28, "trustHandle of CreateTrustedDomainEx, state S322");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of CreateTrustedDomainEx, state S322");
            this.Manager.Comment("reaching state \'S376\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp30;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainInformationBasic,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp31 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp30);
            this.Manager.Comment("reaching state \'S430\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp30, "trustDomainInfo of QueryInfoTrustedDomain, state S430");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp31, "return of QueryInfoTrustedDomain, state S430");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S106
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S106() {
            this.Manager.BeginTest("TestScenarioS18S106");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp33 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp32);
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp32, "policyHandle of OpenPolicy2, state S215");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp33, "return of OpenPolicy2, state S215");
            this.Manager.Comment("reaching state \'S269\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp35 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp34);
            this.Manager.Comment("reaching state \'S323\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp34, "trustHandle of CreateTrustedDomainEx, state S323");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp35, "return of CreateTrustedDomainEx, state S323");
            this.Manager.Comment("reaching state \'S377\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,Invalid,DS_BEHAVIOR_WIN2003,out _)\'" +
                    "");
            temp37 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp36);
            this.Manager.Comment("reaching state \'S431\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp36, "trustDomainInfo of QueryInfoTrustedDomain, state S431");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp37, "return of QueryInfoTrustedDomain, state S431");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S12() {
            this.Manager.BeginTest("TestScenarioS18S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S114\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp38);
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp38, "policyHandle of OpenPolicy2, state S168");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp39, "return of OpenPolicy2, state S168");
            this.Manager.Comment("reaching state \'S222\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp41 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp40);
            this.Manager.Comment("reaching state \'S276\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp40, "trustHandle of CreateTrustedDomainEx, state S276");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of CreateTrustedDomainEx, state S276");
            this.Manager.Comment("reaching state \'S330\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainInformationEx,DS_BEHAV" +
                    "IOR_WIN2003,out _)\'");
            temp43 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp42);
            this.Manager.Comment("reaching state \'S384\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp42, "trustDomainInfo of QueryInfoTrustedDomain, state S384");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp43, "return of QueryInfoTrustedDomain, state S384");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S14() {
            this.Manager.BeginTest("TestScenarioS18S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S115\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp44;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp45 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp44);
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp44, "policyHandle of OpenPolicy2, state S169");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp45, "return of OpenPolicy2, state S169");
            this.Manager.Comment("reaching state \'S223\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp46;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp47 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp46);
            this.Manager.Comment("reaching state \'S277\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp46, "trustHandle of CreateTrustedDomainEx, state S277");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp47, "return of CreateTrustedDomainEx, state S277");
            this.Manager.Comment("reaching state \'S331\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedPasswordInformation,DS_BEHAV" +
                    "IOR_WIN2003,out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp48);
            this.Manager.Comment("reaching state \'S385\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp48, "trustDomainInfo of QueryInfoTrustedDomain, state S385");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp49, "return of QueryInfoTrustedDomain, state S385");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S16() {
            this.Manager.BeginTest("TestScenarioS18S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S116\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp50;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp51 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp50);
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp50, "policyHandle of OpenPolicy2, state S170");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp51, "return of OpenPolicy2, state S170");
            this.Manager.Comment("reaching state \'S224\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp53 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp52);
            this.Manager.Comment("reaching state \'S278\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp52, "trustHandle of CreateTrustedDomainEx, state S278");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp53, "return of CreateTrustedDomainEx, state S278");
            this.Manager.Comment("reaching state \'S332\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainInformationEx2Internal" +
                    ",DS_BEHAVIOR_WIN2003,out _)\'");
            temp55 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp54);
            this.Manager.Comment("reaching state \'S386\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp54, "trustDomainInfo of QueryInfoTrustedDomain, state S386");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp55, "return of QueryInfoTrustedDomain, state S386");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S18() {
            this.Manager.BeginTest("TestScenarioS18S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S117\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp56;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp57;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp57 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp56);
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp56, "policyHandle of OpenPolicy2, state S171");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp57, "return of OpenPolicy2, state S171");
            this.Manager.Comment("reaching state \'S225\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp58;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp59 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp58);
            this.Manager.Comment("reaching state \'S279\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp58, "trustHandle of CreateTrustedDomainEx, state S279");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp59, "return of CreateTrustedDomainEx, state S279");
            this.Manager.Comment("reaching state \'S333\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp60;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainInformationBasic,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp61 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp60);
            this.Manager.Comment("reaching state \'S387\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp60, "trustDomainInfo of QueryInfoTrustedDomain, state S387");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp61, "return of QueryInfoTrustedDomain, state S387");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S2() {
            this.Manager.BeginTest("TestScenarioS18S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S109\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp63 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp62);
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp62, "policyHandle of OpenPolicy2, state S163");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp63, "return of OpenPolicy2, state S163");
            this.Manager.Comment("reaching state \'S217\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp64;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp65;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp65 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp64);
            this.Manager.Comment("reaching state \'S271\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp64, "trustHandle of CreateTrustedDomainEx, state S271");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp65, "return of CreateTrustedDomainEx, state S271");
            this.Manager.Comment("reaching state \'S325\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp66;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainSupportedEncryptionTyp" +
                    "es,DS_BEHAVIOR_WIN2003,out _)\'");
            temp67 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp66);
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp66, "trustDomainInfo of QueryInfoTrustedDomain, state S379");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp67, "return of QueryInfoTrustedDomain, state S379");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S20() {
            this.Manager.BeginTest("TestScenarioS18S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S118\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp68);
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp68, "policyHandle of OpenPolicy2, state S172");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp69, "return of OpenPolicy2, state S172");
            this.Manager.Comment("reaching state \'S226\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp71 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp70);
            this.Manager.Comment("reaching state \'S280\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp70, "trustHandle of CreateTrustedDomainEx, state S280");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp71, "return of CreateTrustedDomainEx, state S280");
            this.Manager.Comment("reaching state \'S334\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp72;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainFullInformationInterna" +
                    "l,DS_BEHAVIOR_WIN2003,out _)\'");
            temp73 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp72);
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidInfoClass\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp72, "trustDomainInfo of QueryInfoTrustedDomain, state S388");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidInfoClass, temp73, "return of QueryInfoTrustedDomain, state S388");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S22() {
            this.Manager.BeginTest("TestScenarioS18S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S119\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp74;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp75;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp75 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp74);
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp74, "policyHandle of OpenPolicy2, state S173");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp75, "return of OpenPolicy2, state S173");
            this.Manager.Comment("reaching state \'S227\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp76;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp77 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp76);
            this.Manager.Comment("reaching state \'S281\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp76, "trustHandle of CreateTrustedDomainEx, state S281");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp77, "return of CreateTrustedDomainEx, state S281");
            this.Manager.Comment("reaching state \'S335\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp78;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainAuthInformationInterna" +
                    "l,DS_BEHAVIOR_WIN2003,out _)\'");
            temp79 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp78);
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidInfoClass\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp78, "trustDomainInfo of QueryInfoTrustedDomain, state S389");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidInfoClass, temp79, "return of QueryInfoTrustedDomain, state S389");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S24() {
            this.Manager.BeginTest("TestScenarioS18S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S120\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp80;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp81 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp80);
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp80, "policyHandle of OpenPolicy2, state S174");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp81, "return of OpenPolicy2, state S174");
            this.Manager.Comment("reaching state \'S228\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp82;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp83;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp83 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp82);
            this.Manager.Comment("reaching state \'S282\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp82, "trustHandle of CreateTrustedDomainEx, state S282");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp83, "return of CreateTrustedDomainEx, state S282");
            this.Manager.Comment("reaching state \'S336\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp84;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainAuthInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp85 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp84);
            this.Manager.Comment("reaching state \'S390\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidInfoClass\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp84, "trustDomainInfo of QueryInfoTrustedDomain, state S390");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidInfoClass, temp85, "return of QueryInfoTrustedDomain, state S390");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S26() {
            this.Manager.BeginTest("TestScenarioS18S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S121\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp86;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp87 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp86);
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp86, "policyHandle of OpenPolicy2, state S175");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp87, "return of OpenPolicy2, state S175");
            this.Manager.Comment("reaching state \'S229\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp88;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp89 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp88);
            this.Manager.Comment("reaching state \'S283\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp88, "trustHandle of CreateTrustedDomainEx, state S283");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp89, "return of CreateTrustedDomainEx, state S283");
            this.Manager.Comment("reaching state \'S337\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp90;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp91;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainNameInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp91 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp90);
            this.Manager.Comment("reaching state \'S391\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp90, "trustDomainInfo of QueryInfoTrustedDomain, state S391");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp91, "return of QueryInfoTrustedDomain, state S391");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S28() {
            this.Manager.BeginTest("TestScenarioS18S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S122\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp92;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp93;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp93 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp92);
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp92, "policyHandle of OpenPolicy2, state S176");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp93, "return of OpenPolicy2, state S176");
            this.Manager.Comment("reaching state \'S230\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp94;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp95;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp95 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp94);
            this.Manager.Comment("reaching state \'S284\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp94, "trustHandle of CreateTrustedDomainEx, state S284");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp95, "return of CreateTrustedDomainEx, state S284");
            this.Manager.Comment("reaching state \'S338\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp96;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedPosixOffsetInformation,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp97 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp96);
            this.Manager.Comment("reaching state \'S392\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp96, "trustDomainInfo of QueryInfoTrustedDomain, state S392");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp97, "return of QueryInfoTrustedDomain, state S392");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S30() {
            this.Manager.BeginTest("TestScenarioS18S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S123\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp98;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp99;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp99 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp98);
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp98, "policyHandle of OpenPolicy2, state S177");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp99, "return of OpenPolicy2, state S177");
            this.Manager.Comment("reaching state \'S231\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp100;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp101 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp100);
            this.Manager.Comment("reaching state \'S285\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp100, "trustHandle of CreateTrustedDomainEx, state S285");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp101, "return of CreateTrustedDomainEx, state S285");
            this.Manager.Comment("reaching state \'S339\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp102;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainInformationEx,DS_BEHAV" +
                    "IOR_WIN2003,out _)\'");
            temp103 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp102);
            this.Manager.Comment("reaching state \'S393\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp102, "trustDomainInfo of QueryInfoTrustedDomain, state S393");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp103, "return of QueryInfoTrustedDomain, state S393");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S32() {
            this.Manager.BeginTest("TestScenarioS18S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S124\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp104;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp105;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp105 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp104);
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp104, "policyHandle of OpenPolicy2, state S178");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp105, "return of OpenPolicy2, state S178");
            this.Manager.Comment("reaching state \'S232\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp106;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp107;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp107 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp106);
            this.Manager.Comment("reaching state \'S286\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp106, "trustHandle of CreateTrustedDomainEx, state S286");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp107, "return of CreateTrustedDomainEx, state S286");
            this.Manager.Comment("reaching state \'S340\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp108;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp109;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainFullInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp109 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp108);
            this.Manager.Comment("reaching state \'S394\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp108, "trustDomainInfo of QueryInfoTrustedDomain, state S394");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp109, "return of QueryInfoTrustedDomain, state S394");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S34() {
            this.Manager.BeginTest("TestScenarioS18S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S125\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp110;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp111;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp111 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp110);
            this.Manager.Comment("reaching state \'S179\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp110, "policyHandle of OpenPolicy2, state S179");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp111, "return of OpenPolicy2, state S179");
            this.Manager.Comment("reaching state \'S233\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp112;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp113;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp113 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp112);
            this.Manager.Comment("reaching state \'S287\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp112, "trustHandle of CreateTrustedDomainEx, state S287");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp113, "return of CreateTrustedDomainEx, state S287");
            this.Manager.Comment("reaching state \'S341\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp114;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainFullInformation2Intern" +
                    "al,DS_BEHAVIOR_WIN2003,out _)\'");
            temp115 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp114);
            this.Manager.Comment("reaching state \'S395\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp114, "trustDomainInfo of QueryInfoTrustedDomain, state S395");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp115, "return of QueryInfoTrustedDomain, state S395");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S36() {
            this.Manager.BeginTest("TestScenarioS18S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S126\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp116;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp117 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp116);
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp116, "policyHandle of OpenPolicy2, state S180");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp117, "return of OpenPolicy2, state S180");
            this.Manager.Comment("reaching state \'S234\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp118;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp119;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp119 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp118);
            this.Manager.Comment("reaching state \'S288\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp118, "trustHandle of CreateTrustedDomainEx, state S288");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp119, "return of CreateTrustedDomainEx, state S288");
            this.Manager.Comment("reaching state \'S342\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp120;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp121;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainSupportedEncryptionTyp" +
                    "es,DS_BEHAVIOR_WIN2003,out _)\'");
            temp121 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp120);
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp120, "trustDomainInfo of QueryInfoTrustedDomain, state S396");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp121, "return of QueryInfoTrustedDomain, state S396");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S38() {
            this.Manager.BeginTest("TestScenarioS18S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S127\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp122;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp123;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp123 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp122);
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp122, "policyHandle of OpenPolicy2, state S181");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp123, "return of OpenPolicy2, state S181");
            this.Manager.Comment("reaching state \'S235\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp124;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp125;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp125 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp124);
            this.Manager.Comment("reaching state \'S289\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp124, "trustHandle of CreateTrustedDomainEx, state S289");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp125, "return of CreateTrustedDomainEx, state S289");
            this.Manager.Comment("reaching state \'S343\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp126;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp127;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedPasswordInformation,DS_BEHAV" +
                    "IOR_WIN2003,out _)\'");
            temp127 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp126);
            this.Manager.Comment("reaching state \'S397\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp126, "trustDomainInfo of QueryInfoTrustedDomain, state S397");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp127, "return of QueryInfoTrustedDomain, state S397");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S4() {
            this.Manager.BeginTest("TestScenarioS18S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp128;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp129;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp129 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp128);
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp128, "policyHandle of OpenPolicy2, state S164");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp129, "return of OpenPolicy2, state S164");
            this.Manager.Comment("reaching state \'S218\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp130;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp131;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp131 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp130);
            this.Manager.Comment("reaching state \'S272\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp130, "trustHandle of CreateTrustedDomainEx, state S272");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp131, "return of CreateTrustedDomainEx, state S272");
            this.Manager.Comment("reaching state \'S326\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp132;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp133;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainFullInformation2Intern" +
                    "al,DS_BEHAVIOR_WIN2003,out _)\'");
            temp133 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp132);
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp132, "trustDomainInfo of QueryInfoTrustedDomain, state S380");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp133, "return of QueryInfoTrustedDomain, state S380");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S40() {
            this.Manager.BeginTest("TestScenarioS18S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S128\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp134;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp135;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp135 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp134);
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp134, "policyHandle of OpenPolicy2, state S182");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp135, "return of OpenPolicy2, state S182");
            this.Manager.Comment("reaching state \'S236\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp136;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp137;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp137 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp136);
            this.Manager.Comment("reaching state \'S290\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp136, "trustHandle of CreateTrustedDomainEx, state S290");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp137, "return of CreateTrustedDomainEx, state S290");
            this.Manager.Comment("reaching state \'S344\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp138;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp139;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,Invalid,DS_BEHAVIOR_WIN2003,out _)\'" +
                    "");
            temp139 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp138);
            this.Manager.Comment("reaching state \'S398\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp138, "trustDomainInfo of QueryInfoTrustedDomain, state S398");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp139, "return of QueryInfoTrustedDomain, state S398");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S42() {
            this.Manager.BeginTest("TestScenarioS18S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S129\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp140;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp141;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp141 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp140);
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp140, "policyHandle of OpenPolicy2, state S183");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp141, "return of OpenPolicy2, state S183");
            this.Manager.Comment("reaching state \'S237\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp142;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp143;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp143 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp142);
            this.Manager.Comment("reaching state \'S291\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp142, "trustHandle of CreateTrustedDomainEx, state S291");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp143, "return of CreateTrustedDomainEx, state S291");
            this.Manager.Comment("reaching state \'S345\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp144;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp145;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainAuthInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp145 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp144);
            this.Manager.Comment("reaching state \'S399\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp144, "trustDomainInfo of QueryInfoTrustedDomain, state S399");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp145, "return of QueryInfoTrustedDomain, state S399");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S44() {
            this.Manager.BeginTest("TestScenarioS18S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S130\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp146;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp147;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp147 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp146);
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp146, "policyHandle of OpenPolicy2, state S184");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp147, "return of OpenPolicy2, state S184");
            this.Manager.Comment("reaching state \'S238\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp148;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp149;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp149 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp148);
            this.Manager.Comment("reaching state \'S292\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp148, "trustHandle of CreateTrustedDomainEx, state S292");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp149, "return of CreateTrustedDomainEx, state S292");
            this.Manager.Comment("reaching state \'S346\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp150;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp151;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainInformationEx2Internal" +
                    ",DS_BEHAVIOR_WIN2003,out _)\'");
            temp151 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp150);
            this.Manager.Comment("reaching state \'S400\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp150, "trustDomainInfo of QueryInfoTrustedDomain, state S400");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp151, "return of QueryInfoTrustedDomain, state S400");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S46() {
            this.Manager.BeginTest("TestScenarioS18S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S131\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp152;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp153;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp153 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp152);
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp152, "policyHandle of OpenPolicy2, state S185");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp153, "return of OpenPolicy2, state S185");
            this.Manager.Comment("reaching state \'S239\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp154;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp155;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp155 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp154);
            this.Manager.Comment("reaching state \'S293\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp154, "trustHandle of CreateTrustedDomainEx, state S293");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp155, "return of CreateTrustedDomainEx, state S293");
            this.Manager.Comment("reaching state \'S347\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp156;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp157;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainFullInformationInterna" +
                    "l,DS_BEHAVIOR_WIN2003,out _)\'");
            temp157 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp156);
            this.Manager.Comment("reaching state \'S401\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp156, "trustDomainInfo of QueryInfoTrustedDomain, state S401");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp157, "return of QueryInfoTrustedDomain, state S401");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S48() {
            this.Manager.BeginTest("TestScenarioS18S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S132\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp158;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp159;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp159 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp158);
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp158, "policyHandle of OpenPolicy2, state S186");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp159, "return of OpenPolicy2, state S186");
            this.Manager.Comment("reaching state \'S240\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp160;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp161;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp161 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp160);
            this.Manager.Comment("reaching state \'S294\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp160, "trustHandle of CreateTrustedDomainEx, state S294");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp161, "return of CreateTrustedDomainEx, state S294");
            this.Manager.Comment("reaching state \'S348\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp162;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp163;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainAuthInformationInterna" +
                    "l,DS_BEHAVIOR_WIN2003,out _)\'");
            temp163 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp162);
            this.Manager.Comment("reaching state \'S402\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp162, "trustDomainInfo of QueryInfoTrustedDomain, state S402");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp163, "return of QueryInfoTrustedDomain, state S402");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S50() {
            this.Manager.BeginTest("TestScenarioS18S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S133\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp164;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp165;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp165 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp164);
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp164, "policyHandle of OpenPolicy2, state S187");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp165, "return of OpenPolicy2, state S187");
            this.Manager.Comment("reaching state \'S241\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp166;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp167;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp167 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp166);
            this.Manager.Comment("reaching state \'S295\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp166, "trustHandle of CreateTrustedDomainEx, state S295");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp167, "return of CreateTrustedDomainEx, state S295");
            this.Manager.Comment("reaching state \'S349\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp168;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp169;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedControllersInformation,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp169 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp168);
            this.Manager.Comment("reaching state \'S403\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp168, "trustDomainInfo of QueryInfoTrustedDomain, state S403");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp169, "return of QueryInfoTrustedDomain, state S403");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S52() {
            this.Manager.BeginTest("TestScenarioS18S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S134\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp170;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp171;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp171 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp170);
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp170, "policyHandle of OpenPolicy2, state S188");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp171, "return of OpenPolicy2, state S188");
            this.Manager.Comment("reaching state \'S242\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp172;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp173;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp173 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp172);
            this.Manager.Comment("reaching state \'S296\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp172, "trustHandle of CreateTrustedDomainEx, state S296");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp173, "return of CreateTrustedDomainEx, state S296");
            this.Manager.Comment("reaching state \'S350\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp174;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp175;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainNameInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp175 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp174);
            this.Manager.Comment("reaching state \'S404\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp174, "trustDomainInfo of QueryInfoTrustedDomain, state S404");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp175, "return of QueryInfoTrustedDomain, state S404");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S54() {
            this.Manager.BeginTest("TestScenarioS18S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S135\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp176;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp177;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp177 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp176);
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp176, "policyHandle of OpenPolicy2, state S189");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp177, "return of OpenPolicy2, state S189");
            this.Manager.Comment("reaching state \'S243\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp178;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp179;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp179 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp178);
            this.Manager.Comment("reaching state \'S297\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp178, "trustHandle of CreateTrustedDomainEx, state S297");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp179, "return of CreateTrustedDomainEx, state S297");
            this.Manager.Comment("reaching state \'S351\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp180;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp181;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainNameInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp181 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp180);
            this.Manager.Comment("reaching state \'S405\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp180, "trustDomainInfo of QueryInfoTrustedDomain, state S405");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp181, "return of QueryInfoTrustedDomain, state S405");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S56() {
            this.Manager.BeginTest("TestScenarioS18S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S136\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp182;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp183;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp183 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp182);
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp182, "policyHandle of OpenPolicy2, state S190");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp183, "return of OpenPolicy2, state S190");
            this.Manager.Comment("reaching state \'S244\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp184;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp185;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp185 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp184);
            this.Manager.Comment("reaching state \'S298\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp184, "trustHandle of CreateTrustedDomainEx, state S298");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp185, "return of CreateTrustedDomainEx, state S298");
            this.Manager.Comment("reaching state \'S352\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp186;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp187;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedPosixOffsetInformation,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp187 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp186);
            this.Manager.Comment("reaching state \'S406\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp186, "trustDomainInfo of QueryInfoTrustedDomain, state S406");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp187, "return of QueryInfoTrustedDomain, state S406");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S58() {
            this.Manager.BeginTest("TestScenarioS18S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S137\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp188;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp189;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp189 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp188);
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp188, "policyHandle of OpenPolicy2, state S191");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp189, "return of OpenPolicy2, state S191");
            this.Manager.Comment("reaching state \'S245\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp190;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp191;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp191 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp190);
            this.Manager.Comment("reaching state \'S299\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp190, "trustHandle of CreateTrustedDomainEx, state S299");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp191, "return of CreateTrustedDomainEx, state S299");
            this.Manager.Comment("reaching state \'S353\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp192;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp193;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainFullInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp193 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp192);
            this.Manager.Comment("reaching state \'S407\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp192, "trustDomainInfo of QueryInfoTrustedDomain, state S407");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp193, "return of QueryInfoTrustedDomain, state S407");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S6() {
            this.Manager.BeginTest("TestScenarioS18S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S111\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp194;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp195;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp195 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp194);
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp194, "policyHandle of OpenPolicy2, state S165");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp195, "return of OpenPolicy2, state S165");
            this.Manager.Comment("reaching state \'S219\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp196;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp197;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp197 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp196);
            this.Manager.Comment("reaching state \'S273\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp196, "trustHandle of CreateTrustedDomainEx, state S273");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp197, "return of CreateTrustedDomainEx, state S273");
            this.Manager.Comment("reaching state \'S327\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp198;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp199;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainFullInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp199 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp198);
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp198, "trustDomainInfo of QueryInfoTrustedDomain, state S381");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp199, "return of QueryInfoTrustedDomain, state S381");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S60() {
            this.Manager.BeginTest("TestScenarioS18S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S138\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp200;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp201;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp201 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp200);
            this.Manager.Comment("reaching state \'S192\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp200, "policyHandle of OpenPolicy2, state S192");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp201, "return of OpenPolicy2, state S192");
            this.Manager.Comment("reaching state \'S246\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp202;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp203;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp203 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp202);
            this.Manager.Comment("reaching state \'S300\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp202, "trustHandle of CreateTrustedDomainEx, state S300");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp203, "return of CreateTrustedDomainEx, state S300");
            this.Manager.Comment("reaching state \'S354\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp204;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp205;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainFullInformation2Intern" +
                    "al,DS_BEHAVIOR_WIN2003,out _)\'");
            temp205 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp204);
            this.Manager.Comment("reaching state \'S408\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp204, "trustDomainInfo of QueryInfoTrustedDomain, state S408");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp205, "return of QueryInfoTrustedDomain, state S408");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S62() {
            this.Manager.BeginTest("TestScenarioS18S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S139\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp206;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp207;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp207 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp206);
            this.Manager.Comment("reaching state \'S193\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp206, "policyHandle of OpenPolicy2, state S193");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp207, "return of OpenPolicy2, state S193");
            this.Manager.Comment("reaching state \'S247\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp208;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp209;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp209 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp208);
            this.Manager.Comment("reaching state \'S301\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp208, "trustHandle of CreateTrustedDomainEx, state S301");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp209, "return of CreateTrustedDomainEx, state S301");
            this.Manager.Comment("reaching state \'S355\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp210;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp211;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainSupportedEncryptionTyp" +
                    "es,DS_BEHAVIOR_WIN2003,out _)\'");
            temp211 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp210);
            this.Manager.Comment("reaching state \'S409\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp210, "trustDomainInfo of QueryInfoTrustedDomain, state S409");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp211, "return of QueryInfoTrustedDomain, state S409");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S64() {
            this.Manager.BeginTest("TestScenarioS18S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S140\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp212;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp213;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp213 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp212);
            this.Manager.Comment("reaching state \'S194\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp212, "policyHandle of OpenPolicy2, state S194");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp213, "return of OpenPolicy2, state S194");
            this.Manager.Comment("reaching state \'S248\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp214;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp215;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp215 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp214);
            this.Manager.Comment("reaching state \'S302\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp214, "trustHandle of CreateTrustedDomainEx, state S302");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp215, "return of CreateTrustedDomainEx, state S302");
            this.Manager.Comment("reaching state \'S356\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp216;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp217;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedPosixOffsetInformation,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp217 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp216);
            this.Manager.Comment("reaching state \'S410\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp216, "trustDomainInfo of QueryInfoTrustedDomain, state S410");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp217, "return of QueryInfoTrustedDomain, state S410");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S66() {
            this.Manager.BeginTest("TestScenarioS18S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S141\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp218;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp219;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp219 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp218);
            this.Manager.Comment("reaching state \'S195\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp218, "policyHandle of OpenPolicy2, state S195");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp219, "return of OpenPolicy2, state S195");
            this.Manager.Comment("reaching state \'S249\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp220;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp221;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp221 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp220);
            this.Manager.Comment("reaching state \'S303\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp220, "trustHandle of CreateTrustedDomainEx, state S303");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp221, "return of CreateTrustedDomainEx, state S303");
            this.Manager.Comment("reaching state \'S357\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp222;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp223;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainInformationEx,DS_BEHAV" +
                    "IOR_WIN2003,out _)\'");
            temp223 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp222);
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp222, "trustDomainInfo of QueryInfoTrustedDomain, state S411");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp223, "return of QueryInfoTrustedDomain, state S411");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S68() {
            this.Manager.BeginTest("TestScenarioS18S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S142\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp224;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp225;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp225 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp224);
            this.Manager.Comment("reaching state \'S196\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp224, "policyHandle of OpenPolicy2, state S196");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp225, "return of OpenPolicy2, state S196");
            this.Manager.Comment("reaching state \'S250\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp226;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp227;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp227 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp226);
            this.Manager.Comment("reaching state \'S304\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp226, "trustHandle of CreateTrustedDomainEx, state S304");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp227, "return of CreateTrustedDomainEx, state S304");
            this.Manager.Comment("reaching state \'S358\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp228;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp229;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainFullInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp229 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp228);
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp228, "trustDomainInfo of QueryInfoTrustedDomain, state S412");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp229, "return of QueryInfoTrustedDomain, state S412");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S70() {
            this.Manager.BeginTest("TestScenarioS18S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S143\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp230;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp231;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp231 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp230);
            this.Manager.Comment("reaching state \'S197\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp230, "policyHandle of OpenPolicy2, state S197");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp231, "return of OpenPolicy2, state S197");
            this.Manager.Comment("reaching state \'S251\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp232;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp233;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp233 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp232);
            this.Manager.Comment("reaching state \'S305\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp232, "trustHandle of CreateTrustedDomainEx, state S305");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp233, "return of CreateTrustedDomainEx, state S305");
            this.Manager.Comment("reaching state \'S359\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp234;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp235;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainFullInformation2Intern" +
                    "al,DS_BEHAVIOR_WIN2003,out _)\'");
            temp235 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp234);
            this.Manager.Comment("reaching state \'S413\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp234, "trustDomainInfo of QueryInfoTrustedDomain, state S413");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp235, "return of QueryInfoTrustedDomain, state S413");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S72
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S72() {
            this.Manager.BeginTest("TestScenarioS18S72");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp236;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp237;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp237 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp236);
            this.Manager.Comment("reaching state \'S198\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp236, "policyHandle of OpenPolicy2, state S198");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp237, "return of OpenPolicy2, state S198");
            this.Manager.Comment("reaching state \'S252\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp238;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp239;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp239 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp238);
            this.Manager.Comment("reaching state \'S306\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp238, "trustHandle of CreateTrustedDomainEx, state S306");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp239, "return of CreateTrustedDomainEx, state S306");
            this.Manager.Comment("reaching state \'S360\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp240;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp241;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainSupportedEncryptionTyp" +
                    "es,DS_BEHAVIOR_WIN2003,out _)\'");
            temp241 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp240);
            this.Manager.Comment("reaching state \'S414\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp240, "trustDomainInfo of QueryInfoTrustedDomain, state S414");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp241, "return of QueryInfoTrustedDomain, state S414");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S74
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S74() {
            this.Manager.BeginTest("TestScenarioS18S74");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp242;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp243;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp243 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp242);
            this.Manager.Comment("reaching state \'S199\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp242, "policyHandle of OpenPolicy2, state S199");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp243, "return of OpenPolicy2, state S199");
            this.Manager.Comment("reaching state \'S253\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp244;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp245;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp245 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp244);
            this.Manager.Comment("reaching state \'S307\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp244, "trustHandle of CreateTrustedDomainEx, state S307");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp245, "return of CreateTrustedDomainEx, state S307");
            this.Manager.Comment("reaching state \'S361\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp246;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp247;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedPasswordInformation,DS_BEHAV" +
                    "IOR_WIN2003,out _)\'");
            temp247 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp246);
            this.Manager.Comment("reaching state \'S415\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp246, "trustDomainInfo of QueryInfoTrustedDomain, state S415");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp247, "return of QueryInfoTrustedDomain, state S415");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S76
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S76() {
            this.Manager.BeginTest("TestScenarioS18S76");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp248;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp249;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp249 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp248);
            this.Manager.Comment("reaching state \'S200\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp248, "policyHandle of OpenPolicy2, state S200");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp249, "return of OpenPolicy2, state S200");
            this.Manager.Comment("reaching state \'S254\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp250;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp251;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp251 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp250);
            this.Manager.Comment("reaching state \'S308\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp250, "trustHandle of CreateTrustedDomainEx, state S308");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp251, "return of CreateTrustedDomainEx, state S308");
            this.Manager.Comment("reaching state \'S362\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp252;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp253;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedPasswordInformation,DS_BEHAV" +
                    "IOR_WIN2003,out _)\'");
            temp253 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp252);
            this.Manager.Comment("reaching state \'S416\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp252, "trustDomainInfo of QueryInfoTrustedDomain, state S416");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp253, "return of QueryInfoTrustedDomain, state S416");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S78
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S78() {
            this.Manager.BeginTest("TestScenarioS18S78");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp254;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp255;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp255 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp254);
            this.Manager.Comment("reaching state \'S201\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp254, "policyHandle of OpenPolicy2, state S201");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp255, "return of OpenPolicy2, state S201");
            this.Manager.Comment("reaching state \'S255\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp256;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp257;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp257 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp256);
            this.Manager.Comment("reaching state \'S309\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp256, "trustHandle of CreateTrustedDomainEx, state S309");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp257, "return of CreateTrustedDomainEx, state S309");
            this.Manager.Comment("reaching state \'S363\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp258;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp259;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainInformationEx2Internal" +
                    ",DS_BEHAVIOR_WIN2003,out _)\'");
            temp259 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp258);
            this.Manager.Comment("reaching state \'S417\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp258, "trustDomainInfo of QueryInfoTrustedDomain, state S417");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp259, "return of QueryInfoTrustedDomain, state S417");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S8() {
            this.Manager.BeginTest("TestScenarioS18S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S112\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp260;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp261;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp261 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp260);
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp260, "policyHandle of OpenPolicy2, state S166");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp261, "return of OpenPolicy2, state S166");
            this.Manager.Comment("reaching state \'S220\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp262;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp263;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp263 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp262);
            this.Manager.Comment("reaching state \'S274\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp262, "trustHandle of CreateTrustedDomainEx, state S274");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp263, "return of CreateTrustedDomainEx, state S274");
            this.Manager.Comment("reaching state \'S328\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp264;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp265;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedPosixOffsetInformation,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp265 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp264);
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(0)), temp264, "trustDomainInfo of QueryInfoTrustedDomain, state S382");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp265, "return of QueryInfoTrustedDomain, state S382");
            TestScenarioS18S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S80
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S80() {
            this.Manager.BeginTest("TestScenarioS18S80");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp266;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp267;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp267 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp266);
            this.Manager.Comment("reaching state \'S202\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp266, "policyHandle of OpenPolicy2, state S202");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp267, "return of OpenPolicy2, state S202");
            this.Manager.Comment("reaching state \'S256\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp268;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp269;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp269 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp268);
            this.Manager.Comment("reaching state \'S310\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp268, "trustHandle of CreateTrustedDomainEx, state S310");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp269, "return of CreateTrustedDomainEx, state S310");
            this.Manager.Comment("reaching state \'S364\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp270;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp271;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,Invalid,DS_BEHAVIOR_WIN2003,out _)\'" +
                    "");
            temp271 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp270);
            this.Manager.Comment("reaching state \'S418\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp270, "trustDomainInfo of QueryInfoTrustedDomain, state S418");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp271, "return of QueryInfoTrustedDomain, state S418");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S82
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S82() {
            this.Manager.BeginTest("TestScenarioS18S82");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp272;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp273;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp273 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp272);
            this.Manager.Comment("reaching state \'S203\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp272, "policyHandle of OpenPolicy2, state S203");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp273, "return of OpenPolicy2, state S203");
            this.Manager.Comment("reaching state \'S257\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp274;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp275;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp275 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp274);
            this.Manager.Comment("reaching state \'S311\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp274, "trustHandle of CreateTrustedDomainEx, state S311");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp275, "return of CreateTrustedDomainEx, state S311");
            this.Manager.Comment("reaching state \'S365\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp276;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp277;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedControllersInformation,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp277 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp276);
            this.Manager.Comment("reaching state \'S419\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp276, "trustDomainInfo of QueryInfoTrustedDomain, state S419");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp277, "return of QueryInfoTrustedDomain, state S419");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S84
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S84() {
            this.Manager.BeginTest("TestScenarioS18S84");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp278;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp279;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp279 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp278);
            this.Manager.Comment("reaching state \'S204\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp278, "policyHandle of OpenPolicy2, state S204");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp279, "return of OpenPolicy2, state S204");
            this.Manager.Comment("reaching state \'S258\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp280;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp281;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp281 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp280);
            this.Manager.Comment("reaching state \'S312\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp280, "trustHandle of CreateTrustedDomainEx, state S312");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp281, "return of CreateTrustedDomainEx, state S312");
            this.Manager.Comment("reaching state \'S366\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp282;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp283;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainFullInformationInterna" +
                    "l,DS_BEHAVIOR_WIN2003,out _)\'");
            temp283 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp282);
            this.Manager.Comment("reaching state \'S420\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp282, "trustDomainInfo of QueryInfoTrustedDomain, state S420");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp283, "return of QueryInfoTrustedDomain, state S420");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S86
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S86() {
            this.Manager.BeginTest("TestScenarioS18S86");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp284;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp285;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp285 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp284);
            this.Manager.Comment("reaching state \'S205\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp284, "policyHandle of OpenPolicy2, state S205");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp285, "return of OpenPolicy2, state S205");
            this.Manager.Comment("reaching state \'S259\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp286;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp287;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp287 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp286);
            this.Manager.Comment("reaching state \'S313\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp286, "trustHandle of CreateTrustedDomainEx, state S313");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp287, "return of CreateTrustedDomainEx, state S313");
            this.Manager.Comment("reaching state \'S367\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp288;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp289;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainAuthInformationInterna" +
                    "l,DS_BEHAVIOR_WIN2003,out _)\'");
            temp289 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp288);
            this.Manager.Comment("reaching state \'S421\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp288, "trustDomainInfo of QueryInfoTrustedDomain, state S421");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp289, "return of QueryInfoTrustedDomain, state S421");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S88
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S88() {
            this.Manager.BeginTest("TestScenarioS18S88");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp290;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp291;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp291 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp290);
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp290, "policyHandle of OpenPolicy2, state S206");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp291, "return of OpenPolicy2, state S206");
            this.Manager.Comment("reaching state \'S260\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp292;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp293;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp293 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp292);
            this.Manager.Comment("reaching state \'S314\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp292, "trustHandle of CreateTrustedDomainEx, state S314");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp293, "return of CreateTrustedDomainEx, state S314");
            this.Manager.Comment("reaching state \'S368\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp294;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp295;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainAuthInformation,DS_BEH" +
                    "AVIOR_WIN2003,out _)\'");
            temp295 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp294);
            this.Manager.Comment("reaching state \'S422\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp294, "trustDomainInfo of QueryInfoTrustedDomain, state S422");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp295, "return of QueryInfoTrustedDomain, state S422");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S90
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S90() {
            this.Manager.BeginTest("TestScenarioS18S90");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp296;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp297;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp297 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp296);
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp296, "policyHandle of OpenPolicy2, state S207");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp297, "return of OpenPolicy2, state S207");
            this.Manager.Comment("reaching state \'S261\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp298;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp299;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp299 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp298);
            this.Manager.Comment("reaching state \'S315\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp298, "trustHandle of CreateTrustedDomainEx, state S315");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp299, "return of CreateTrustedDomainEx, state S315");
            this.Manager.Comment("reaching state \'S369\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp300;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp301;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(1,TrustedDomainInformationBasic,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp301 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp300);
            this.Manager.Comment("reaching state \'S423\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp300, "trustDomainInfo of QueryInfoTrustedDomain, state S423");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp301, "return of QueryInfoTrustedDomain, state S423");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S92
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S92() {
            this.Manager.BeginTest("TestScenarioS18S92");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp302;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp303;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp303 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp302);
            this.Manager.Comment("reaching state \'S208\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp302, "policyHandle of OpenPolicy2, state S208");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp303, "return of OpenPolicy2, state S208");
            this.Manager.Comment("reaching state \'S262\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp304;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp305;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp305 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp304);
            this.Manager.Comment("reaching state \'S316\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp304, "trustHandle of CreateTrustedDomainEx, state S316");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp305, "return of CreateTrustedDomainEx, state S316");
            this.Manager.Comment("reaching state \'S370\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp306;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp307;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainInformationBasic,DS_BE" +
                    "HAVIOR_WIN2003,out _)\'");
            temp307 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp306);
            this.Manager.Comment("reaching state \'S424\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp306, "trustDomainInfo of QueryInfoTrustedDomain, state S424");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp307, "return of QueryInfoTrustedDomain, state S424");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S94
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S94() {
            this.Manager.BeginTest("TestScenarioS18S94");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp308;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp309;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp309 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp308);
            this.Manager.Comment("reaching state \'S209\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp308, "policyHandle of OpenPolicy2, state S209");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp309, "return of OpenPolicy2, state S209");
            this.Manager.Comment("reaching state \'S263\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp310;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp311;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp311 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp310);
            this.Manager.Comment("reaching state \'S317\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp310, "trustHandle of CreateTrustedDomainEx, state S317");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp311, "return of CreateTrustedDomainEx, state S317");
            this.Manager.Comment("reaching state \'S371\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp312;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp313;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainAuthInformationInterna" +
                    "l,DS_BEHAVIOR_WIN2003,out _)\'");
            temp313 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp312);
            this.Manager.Comment("reaching state \'S425\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp312, "trustDomainInfo of QueryInfoTrustedDomain, state S425");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp313, "return of QueryInfoTrustedDomain, state S425");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S96
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S96() {
            this.Manager.BeginTest("TestScenarioS18S96");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S156\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp314;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp315;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp315 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp314);
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp314, "policyHandle of OpenPolicy2, state S210");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp315, "return of OpenPolicy2, state S210");
            this.Manager.Comment("reaching state \'S264\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp316;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp317;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp317 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp316);
            this.Manager.Comment("reaching state \'S318\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp316, "trustHandle of CreateTrustedDomainEx, state S318");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp317, "return of CreateTrustedDomainEx, state S318");
            this.Manager.Comment("reaching state \'S372\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp318;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp319;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainInformationEx2Internal" +
                    ",DS_BEHAVIOR_WIN2003,out _)\'");
            temp319 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp318);
            this.Manager.Comment("reaching state \'S426\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp318, "trustDomainInfo of QueryInfoTrustedDomain, state S426");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp319, "return of QueryInfoTrustedDomain, state S426");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S98
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS18S98() {
            this.Manager.BeginTest("TestScenarioS18S98");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S157\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp320;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp321;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp321 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp320);
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp320, "policyHandle of OpenPolicy2, state S211");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp321, "return of OpenPolicy2, state S211");
            this.Manager.Comment("reaching state \'S265\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp322;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp323;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""CurrentDomain"",TrustDomain_Sid=""CurrentDomainSid"",TrustDomain_NetBiosName=""CurrentNetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp323 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "CurrentDomain",
                            "CurrentDomainSid",
                            "CurrentNetBiosName",
                            2u,
                            2u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp322);
            this.Manager.Comment("reaching state \'S319\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:CurrentDomainNotAllowed" +
                    "\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp322, "trustHandle of CreateTrustedDomainEx, state S319");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.CurrentDomainNotAllowed, temp323, "return of CreateTrustedDomainEx, state S319");
            this.Manager.Comment("reaching state \'S373\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation temp324;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp325;
            this.Manager.Comment("executing step \'call QueryInfoTrustedDomain(2,TrustedDomainFullInformationInterna" +
                    "l,DS_BEHAVIOR_WIN2003,out _)\'");
            temp325 = this.ILsadManagedAdapterInstance.QueryInfoTrustedDomain(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), out temp324);
            this.Manager.Comment("reaching state \'S427\'");
            this.Manager.Comment("checking step \'return QueryInfoTrustedDomain/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedDomainInformation)(1)), temp324, "trustDomainInfo of QueryInfoTrustedDomain, state S427");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp325, "return of QueryInfoTrustedDomain, state S427");
            TestScenarioS18S432();
            this.Manager.EndTest();
        }
        #endregion
    }
}
