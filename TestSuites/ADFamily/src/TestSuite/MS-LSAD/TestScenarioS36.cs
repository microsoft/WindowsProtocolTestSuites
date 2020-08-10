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
    public partial class TestScenarioS36 : PtfTestClassBase {
        
        public TestScenarioS36() {
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
        [TestCategory("RODC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS36S0()
        {
            this.Manager.BeginTest("TestScenarioS36S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(ReadOnlyDomainController,Disable,Windows2k8,2,Tru" +
                    "e)\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.ReadOnlyDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S3");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S3");
            this.Manager.Comment("reaching state \'S4\'");
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
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp2, "trustHandle of CreateTrustedDomainEx, state S5");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp3, "return of CreateTrustedDomainEx, state S5");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx2(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,4061069327,out _)'");
            temp5 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx2(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 4061069327u, out temp4);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx2/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp4, "trustHandle of CreateTrustedDomainEx2, state S7");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp5, "return of CreateTrustedDomainEx2, state S7");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call CreateTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDom" +
                    "ainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"DomainNetB" +
                    "ios\",TrustType=0,TrustDir=0,TrustAttr=0),Valid,True,Valid,4061069439,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.CreateTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), true, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), 4061069439u, out temp6);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomain/[out Invalid]:ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp6, "trustHandle of CreateTrustedDomain, state S9");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp7, "return of CreateTrustedDomain, state S9");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfoByName(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx,True)'");
            temp8 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfoByName(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
                            "TrustDomainName",
                            "TrustDomain_Sid",
                            "TrustDomain_NetBiosName",
                            "TrustType",
                            "TrustDir",
                            "TrustAttr"}, new object[] {
                            "Domain",
                            "DomainSid",
                            "DomainNetBios",
                            2u,
                            1u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfoByName/ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp8, "return of SetTrustedDomainInfoByName, state S11");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainNameInformation,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp9 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), true, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), 4061069443u, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/ObjectNameNotFound\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ObjectNameNotFound, temp9, "return of SetTrustedDomainInfo, state S13");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp10 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.Checkpoint("MS-LSAD_R633");
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/NotSupported\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotSupported, temp10, "return of DeleteTrustedDomain, state S15");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
