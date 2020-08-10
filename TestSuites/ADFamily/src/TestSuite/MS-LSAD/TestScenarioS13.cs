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
    public partial class TestScenarioS13 : PtfTestClassBase {
        
        public TestScenarioS13() {
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
        public void LSAD_TestScenarioS13S0() {
            this.Manager.BeginTest("TestScenarioS13S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S27");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S27");
            this.Manager.Comment("reaching state \'S36\'");
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
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp2, "trustHandle of CreateTrustedDomain, state S45");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp3, "return of CreateTrustedDomain, state S45");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainFullInformationInternal,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp4 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp4, "return of SetTrustedDomainInfo, state S63");
            TestScenarioS13S72();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS13S72() {
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp5 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp5, "return of DeleteTrustedDomain, state S73");
            this.Manager.Comment("reaching state \'S74\'");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS13S10() {
            this.Manager.BeginTest("TestScenarioS13S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S23\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp6);
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp6, "policyHandle of OpenPolicy2, state S32");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp7, "return of OpenPolicy2, state S32");
            this.Manager.Comment("reaching state \'S41\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp9 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp8);
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp8, "trustHandle of CreateTrustedDomain, state S50");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp9, "return of CreateTrustedDomain, state S50");
            this.Manager.Comment("reaching state \'S59\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainSupportedEncryptionTypes,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp10 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp10, "return of SetTrustedDomainInfo, state S68");
            TestScenarioS13S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS13S12() {
            this.Manager.BeginTest("TestScenarioS13S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp11;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp12;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp12 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp11);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp11, "policyHandle of OpenPolicy2, state S33");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp12, "return of OpenPolicy2, state S33");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp13;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp14;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp14 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp13);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp13, "trustHandle of CreateTrustedDomain, state S51");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp14, "return of CreateTrustedDomain, state S51");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformation,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp15 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp15, "return of SetTrustedDomainInfo, state S69");
            TestScenarioS13S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS13S14() {
            this.Manager.BeginTest("TestScenarioS13S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S25\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp16);
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp16, "policyHandle of OpenPolicy2, state S34");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of OpenPolicy2, state S34");
            this.Manager.Comment("reaching state \'S43\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp18;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp19;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp19 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp18);
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp18, "trustHandle of CreateTrustedDomain, state S52");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp19, "return of CreateTrustedDomain, state S52");
            this.Manager.Comment("reaching state \'S61\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp20;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformationInternal,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp20 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp20, "return of SetTrustedDomainInfo, state S70");
            TestScenarioS13S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS13S16() {
            this.Manager.BeginTest("TestScenarioS13S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp21;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp22;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp22 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp21);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp21, "policyHandle of OpenPolicy2, state S35");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp22, "return of OpenPolicy2, state S35");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp23;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp24;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp24 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp23);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp23, "trustHandle of CreateTrustedDomain, state S53");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp24, "return of CreateTrustedDomain, state S53");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp25 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp25, "return of SetTrustedDomainInfo, state S71");
            TestScenarioS13S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS13S2() {
            this.Manager.BeginTest("TestScenarioS13S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S19\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp26;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp27 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp26);
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp26, "policyHandle of OpenPolicy2, state S28");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp27, "return of OpenPolicy2, state S28");
            this.Manager.Comment("reaching state \'S37\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp28);
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp28, "trustHandle of CreateTrustedDomain, state S46");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of CreateTrustedDomain, state S46");
            this.Manager.Comment("reaching state \'S55\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp30;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation2Internal,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp30 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp30, "return of SetTrustedDomainInfo, state S64");
            TestScenarioS13S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS13S4() {
            this.Manager.BeginTest("TestScenarioS13S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp31;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp32;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp32 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp31);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp31, "policyHandle of OpenPolicy2, state S29");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp32, "return of OpenPolicy2, state S29");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp33;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp34;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp34 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp33);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp33, "trustHandle of CreateTrustedDomain, state S47");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp34, "return of CreateTrustedDomain, state S47");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainInformationBasic,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp35 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp35, "return of SetTrustedDomainInfo, state S65");
            TestScenarioS13S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS13S6() {
            this.Manager.BeginTest("TestScenarioS13S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S21\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp36;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp37;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp37 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp36);
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp36, "policyHandle of OpenPolicy2, state S30");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp37, "return of OpenPolicy2, state S30");
            this.Manager.Comment("reaching state \'S39\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp38;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp39 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp38);
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp38, "trustHandle of CreateTrustedDomain, state S48");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp39, "return of CreateTrustedDomain, state S48");
            this.Manager.Comment("reaching state \'S57\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp40;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx2Internal,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp40 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp40, "return of SetTrustedDomainInfo, state S66");
            TestScenarioS13S72();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS13S8() {
            this.Manager.BeginTest("TestScenarioS13S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp41;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp42;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp42 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp41);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp41, "policyHandle of OpenPolicy2, state S31");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp42, "return of OpenPolicy2, state S31");
            this.Manager.Comment("reaching state \'S40\'");
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
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp43, "trustHandle of CreateTrustedDomain, state S49");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp44, "return of CreateTrustedDomain, state S49");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedControllersInformation,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp45 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp45, "return of SetTrustedDomainInfo, state S67");
            TestScenarioS13S72();
            this.Manager.EndTest();
        }
        #endregion
    }
}
