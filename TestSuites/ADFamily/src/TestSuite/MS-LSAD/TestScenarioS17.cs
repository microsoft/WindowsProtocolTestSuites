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
    public partial class TestScenarioS17 : PtfTestClassBase {
        
        public TestScenarioS17() {
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
        public void LSAD_TestScenarioS17S0() {
            this.Manager.BeginTest("TestScenarioS17S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S140\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S210");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S210");
            this.Manager.Comment("reaching state \'S280\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Invalid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp2);
            this.Manager.Comment("reaching state \'S350\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:ErrorUnKnown\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp2, "trustHandle of CreateTrustedDomainEx, state S350");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.ErrorUnKnown, temp3, "return of CreateTrustedDomainEx, state S350");
            this.Manager.Comment("reaching state \'S420\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation,True)'");
            temp4 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, true);
            this.Manager.Comment("reaching state \'S490\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp4, "return of SetInformationTrustedDomain, state S490");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS17S560() {
            this.Manager.Comment("reaching state \'S560\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp5 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.Comment("reaching state \'S565\'");
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/NoSuchDomain\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NoSuchDomain, temp5, "return of DeleteTrustedDomain, state S565");
            this.Manager.Comment("reaching state \'S567\'");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S10() {
            this.Manager.BeginTest("TestScenarioS17S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp6);
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp6, "policyHandle of OpenPolicy2, state S215");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp7, "return of OpenPolicy2, state S215");
            this.Manager.Comment("reaching state \'S285\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp9 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp8);
            this.Manager.Comment("reaching state \'S355\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp8, "trustHandle of CreateTrustedDomainEx, state S355");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp9, "return of CreateTrustedDomainEx, state S355");
            this.Manager.Comment("reaching state \'S425\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp10;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation,True)'");
            temp10 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, true);
            this.Manager.Comment("reaching state \'S495\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp10, "return of SetInformationTrustedDomain, state S495");
            TestScenarioS17S563();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS17S563() {
            this.Manager.Comment("reaching state \'S563\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp11 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.AddReturn(DeleteTrustedDomainInfo, null, temp11);
            TestScenarioS17S566();
        }
        
        private void TestScenarioS17S566() {
            this.Manager.Comment("reaching state \'S566\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS17.DeleteTrustedDomainInfo, null, new DeleteTrustedDomainDelegate1(this.TestScenarioS17S10DeleteTrustedDomainChecker)));
            this.Manager.Comment("reaching state \'S568\'");
        }
        
        private void TestScenarioS17S10DeleteTrustedDomainChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return DeleteTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of DeleteTrustedDomain, state S566");
        }
        #endregion
        
        #region Test Starting in S100
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S100() {
            this.Manager.BeginTest("TestScenarioS17S100");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S190\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp12;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp13;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp13 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp12);
            this.Manager.Comment("reaching state \'S260\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp12, "policyHandle of OpenPolicy2, state S260");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp13, "return of OpenPolicy2, state S260");
            this.Manager.Comment("reaching state \'S330\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp14;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp15 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp14);
            this.Manager.Comment("reaching state \'S400\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp14, "trustHandle of CreateTrustedDomainEx, state S400");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp15, "return of CreateTrustedDomainEx, state S400");
            this.Manager.Comment("reaching state \'S470\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp16;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx,True)'");
            temp16 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, true);
            this.Manager.Comment("reaching state \'S540\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp16, "return of SetInformationTrustedDomain, state S540");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S102
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S102() {
            this.Manager.BeginTest("TestScenarioS17S102");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S191\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp17;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp18;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp18 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp17);
            this.Manager.Comment("reaching state \'S261\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp17, "policyHandle of OpenPolicy2, state S261");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp18, "return of OpenPolicy2, state S261");
            this.Manager.Comment("reaching state \'S331\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp19;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp20;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp19);
            this.Manager.Comment("reaching state \'S401\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp19, "trustHandle of CreateTrustedDomainEx, state S401");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp20, "return of CreateTrustedDomainEx, state S401");
            this.Manager.Comment("reaching state \'S471\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformationInternal,True)'");
            temp21 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, true);
            this.Manager.Comment("reaching state \'S541\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp21, "return of SetInformationTrustedDomain, state S541");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S104
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S104() {
            this.Manager.BeginTest("TestScenarioS17S104");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S192\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp22);
            this.Manager.Comment("reaching state \'S262\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp22, "policyHandle of OpenPolicy2, state S262");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp23, "return of OpenPolicy2, state S262");
            this.Manager.Comment("reaching state \'S332\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp24;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp25;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp25 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp24);
            this.Manager.Comment("reaching state \'S402\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp24, "trustHandle of CreateTrustedDomainEx, state S402");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp25, "return of CreateTrustedDomainEx, state S402");
            this.Manager.Comment("reaching state \'S472\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp26;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformationInternal,True)'");
            temp26 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, true);
            this.Manager.Comment("reaching state \'S542\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp26, "return of SetInformationTrustedDomain, state S542");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S106
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S106() {
            this.Manager.BeginTest("TestScenarioS17S106");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S193\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp27;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp28;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp28 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp27);
            this.Manager.Comment("reaching state \'S263\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp27, "policyHandle of OpenPolicy2, state S263");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp28, "return of OpenPolicy2, state S263");
            this.Manager.Comment("reaching state \'S333\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp29;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp30;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp30 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp29);
            this.Manager.Comment("reaching state \'S403\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp29, "trustHandle of CreateTrustedDomainEx, state S403");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp30, "return of CreateTrustedDomainEx, state S403");
            this.Manager.Comment("reaching state \'S473\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp31;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation,True)'");
            temp31 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, true);
            this.Manager.Comment("reaching state \'S543\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp31, "return of SetInformationTrustedDomain, state S543");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S108
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S108() {
            this.Manager.BeginTest("TestScenarioS17S108");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S194\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp32;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp33 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp32);
            this.Manager.Comment("reaching state \'S264\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp32, "policyHandle of OpenPolicy2, state S264");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp33, "return of OpenPolicy2, state S264");
            this.Manager.Comment("reaching state \'S334\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp35 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp34);
            this.Manager.Comment("reaching state \'S404\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp34, "trustHandle of CreateTrustedDomainEx, state S404");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp35, "return of CreateTrustedDomainEx, state S404");
            this.Manager.Comment("reaching state \'S474\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp36;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation,True)'");
            temp36 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, true);
            this.Manager.Comment("reaching state \'S544\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp36, "return of SetInformationTrustedDomain, state S544");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S110
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S110() {
            this.Manager.BeginTest("TestScenarioS17S110");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S195\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp37;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp38;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp38 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp37);
            this.Manager.Comment("reaching state \'S265\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp37, "policyHandle of OpenPolicy2, state S265");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp38, "return of OpenPolicy2, state S265");
            this.Manager.Comment("reaching state \'S335\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp39;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp40;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp40 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp39);
            this.Manager.Comment("reaching state \'S405\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp39, "trustHandle of CreateTrustedDomainEx, state S405");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp40, "return of CreateTrustedDomainEx, state S405");
            this.Manager.Comment("reaching state \'S475\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx,True)'");
            temp41 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, true);
            this.Manager.Comment("reaching state \'S545\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp41, "return of SetInformationTrustedDomain, state S545");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S112
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S112() {
            this.Manager.BeginTest("TestScenarioS17S112");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S196\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp42;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp43;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp43 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp42);
            this.Manager.Comment("reaching state \'S266\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp42, "policyHandle of OpenPolicy2, state S266");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp43, "return of OpenPolicy2, state S266");
            this.Manager.Comment("reaching state \'S336\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp44;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp45;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp45 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp44);
            this.Manager.Comment("reaching state \'S406\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp44, "trustHandle of CreateTrustedDomainEx, state S406");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp45, "return of CreateTrustedDomainEx, state S406");
            this.Manager.Comment("reaching state \'S476\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp46;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainSupportedEncryptionTypes,True)'");
            temp46 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, true);
            this.Manager.Comment("reaching state \'S546\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp46, "return of SetInformationTrustedDomain, state S546");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S114
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S114() {
            this.Manager.BeginTest("TestScenarioS17S114");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S197\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp47;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp48;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp48 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp47);
            this.Manager.Comment("reaching state \'S267\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp47, "policyHandle of OpenPolicy2, state S267");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp48, "return of OpenPolicy2, state S267");
            this.Manager.Comment("reaching state \'S337\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp49;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp50;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp50 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp49);
            this.Manager.Comment("reaching state \'S407\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp49, "trustHandle of CreateTrustedDomainEx, state S407");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp50, "return of CreateTrustedDomainEx, state S407");
            this.Manager.Comment("reaching state \'S477\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp51;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainSupportedEncryptionTypes,True)'");
            temp51 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, true);
            this.Manager.Comment("reaching state \'S547\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp51, "return of SetInformationTrustedDomain, state S547");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S116
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S116() {
            this.Manager.BeginTest("TestScenarioS17S116");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S198\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp52;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp53 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp52);
            this.Manager.Comment("reaching state \'S268\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp52, "policyHandle of OpenPolicy2, state S268");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp53, "return of OpenPolicy2, state S268");
            this.Manager.Comment("reaching state \'S338\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp55 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp54);
            this.Manager.Comment("reaching state \'S408\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp54, "trustHandle of CreateTrustedDomainEx, state S408");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp55, "return of CreateTrustedDomainEx, state S408");
            this.Manager.Comment("reaching state \'S478\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp56;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation,True)'");
            temp56 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, true);
            this.Manager.Comment("reaching state \'S548\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp56, "return of SetInformationTrustedDomain, state S548");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S118
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S118() {
            this.Manager.BeginTest("TestScenarioS17S118");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S199\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp57;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp58;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp58 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp57);
            this.Manager.Comment("reaching state \'S269\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp57, "policyHandle of OpenPolicy2, state S269");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp58, "return of OpenPolicy2, state S269");
            this.Manager.Comment("reaching state \'S339\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp59;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp60;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp60 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp59);
            this.Manager.Comment("reaching state \'S409\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp59, "trustHandle of CreateTrustedDomainEx, state S409");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp60, "return of CreateTrustedDomainEx, state S409");
            this.Manager.Comment("reaching state \'S479\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformationInternal,True)'");
            temp61 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, true);
            this.Manager.Comment("reaching state \'S549\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp61, "return of SetInformationTrustedDomain, state S549");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S12() {
            this.Manager.BeginTest("TestScenarioS17S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp62;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp63;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp63 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp62);
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp62, "policyHandle of OpenPolicy2, state S216");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp63, "return of OpenPolicy2, state S216");
            this.Manager.Comment("reaching state \'S286\'");
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
            this.Manager.Comment("reaching state \'S356\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp64, "trustHandle of CreateTrustedDomainEx, state S356");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp65, "return of CreateTrustedDomainEx, state S356");
            this.Manager.Comment("reaching state \'S426\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp66;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainSupportedEncryptionTypes,True)'");
            temp66 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, true);
            this.Manager.Comment("reaching state \'S496\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp66, "return of SetInformationTrustedDomain, state S496");
            TestScenarioS17S562();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS17S562() {
            this.Manager.Comment("reaching state \'S562\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp67 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.AddReturn(DeleteTrustedDomainInfo, null, temp67);
            TestScenarioS17S566();
        }
        #endregion
        
        #region Test Starting in S120
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S120() {
            this.Manager.BeginTest("TestScenarioS17S120");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S200\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp68;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp69;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp69 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp68);
            this.Manager.Comment("reaching state \'S270\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp68, "policyHandle of OpenPolicy2, state S270");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp69, "return of OpenPolicy2, state S270");
            this.Manager.Comment("reaching state \'S340\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp70;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp71;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp70);
            this.Manager.Comment("reaching state \'S410\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp70, "trustHandle of CreateTrustedDomainEx, state S410");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp71, "return of CreateTrustedDomainEx, state S410");
            this.Manager.Comment("reaching state \'S480\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp72;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformation,True)'");
            temp72 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, true);
            this.Manager.Comment("reaching state \'S550\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp72, "return of SetInformationTrustedDomain, state S550");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S122
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S122() {
            this.Manager.BeginTest("TestScenarioS17S122");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S201\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp73;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp74;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp74 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp73);
            this.Manager.Comment("reaching state \'S271\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp73, "policyHandle of OpenPolicy2, state S271");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp74, "return of OpenPolicy2, state S271");
            this.Manager.Comment("reaching state \'S341\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp75;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp76;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp76 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp75);
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp75, "trustHandle of CreateTrustedDomainEx, state S411");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp76, "return of CreateTrustedDomainEx, state S411");
            this.Manager.Comment("reaching state \'S481\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPosixOffsetInformation,True)'");
            temp77 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, true);
            this.Manager.Comment("reaching state \'S551\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp77, "return of SetInformationTrustedDomain, state S551");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S124
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S124() {
            this.Manager.BeginTest("TestScenarioS17S124");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S202\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp78;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp79;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp79 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp78);
            this.Manager.Comment("reaching state \'S272\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp78, "policyHandle of OpenPolicy2, state S272");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp79, "return of OpenPolicy2, state S272");
            this.Manager.Comment("reaching state \'S342\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp80;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp81;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp81 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp80);
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp80, "trustHandle of CreateTrustedDomainEx, state S412");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp81, "return of CreateTrustedDomainEx, state S412");
            this.Manager.Comment("reaching state \'S482\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp82;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformationInternal,True)'");
            temp82 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, true);
            this.Manager.Comment("reaching state \'S552\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp82, "return of SetInformationTrustedDomain, state S552");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S126
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S126() {
            this.Manager.BeginTest("TestScenarioS17S126");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S203\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp83;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp84;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp84 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp83);
            this.Manager.Comment("reaching state \'S273\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp83, "policyHandle of OpenPolicy2, state S273");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp84, "return of OpenPolicy2, state S273");
            this.Manager.Comment("reaching state \'S343\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp85;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp86;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp85);
            this.Manager.Comment("reaching state \'S413\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp85, "trustHandle of CreateTrustedDomainEx, state S413");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp86, "return of CreateTrustedDomainEx, state S413");
            this.Manager.Comment("reaching state \'S483\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp87;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainSupportedEncryptionTypes,True)'");
            temp87 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, true);
            this.Manager.Comment("reaching state \'S553\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp87, "return of SetInformationTrustedDomain, state S553");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S128
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S128() {
            this.Manager.BeginTest("TestScenarioS17S128");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S204\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp88;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp89 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp88);
            this.Manager.Comment("reaching state \'S274\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp88, "policyHandle of OpenPolicy2, state S274");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp89, "return of OpenPolicy2, state S274");
            this.Manager.Comment("reaching state \'S344\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp90;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp91;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp91 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp90);
            this.Manager.Comment("reaching state \'S414\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp90, "trustHandle of CreateTrustedDomainEx, state S414");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp91, "return of CreateTrustedDomainEx, state S414");
            this.Manager.Comment("reaching state \'S484\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp92;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainSupportedEncryptionTypes,True)'");
            temp92 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, true);
            this.Manager.Comment("reaching state \'S554\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp92, "return of SetInformationTrustedDomain, state S554");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S130
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S130() {
            this.Manager.BeginTest("TestScenarioS17S130");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S205\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp93;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp94;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp94 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp93);
            this.Manager.Comment("reaching state \'S275\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp93, "policyHandle of OpenPolicy2, state S275");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp94, "return of OpenPolicy2, state S275");
            this.Manager.Comment("reaching state \'S345\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp95;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp96;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp96 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp95);
            this.Manager.Comment("reaching state \'S415\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp95, "trustHandle of CreateTrustedDomainEx, state S415");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp96, "return of CreateTrustedDomainEx, state S415");
            this.Manager.Comment("reaching state \'S485\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx,True)'");
            temp97 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, true);
            this.Manager.Comment("reaching state \'S555\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp97, "return of SetInformationTrustedDomain, state S555");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S132
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S132() {
            this.Manager.BeginTest("TestScenarioS17S132");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S206\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp98;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp99;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp99 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp98);
            this.Manager.Comment("reaching state \'S276\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp98, "policyHandle of OpenPolicy2, state S276");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp99, "return of OpenPolicy2, state S276");
            this.Manager.Comment("reaching state \'S346\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp100;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp100);
            this.Manager.Comment("reaching state \'S416\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp100, "trustHandle of CreateTrustedDomainEx, state S416");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp101, "return of CreateTrustedDomainEx, state S416");
            this.Manager.Comment("reaching state \'S486\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp102;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformation,True)'");
            temp102 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, true);
            this.Manager.Comment("reaching state \'S556\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp102, "return of SetInformationTrustedDomain, state S556");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S134
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S134() {
            this.Manager.BeginTest("TestScenarioS17S134");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S207\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp103;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp104;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp104 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp103);
            this.Manager.Comment("reaching state \'S277\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp103, "policyHandle of OpenPolicy2, state S277");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp104, "return of OpenPolicy2, state S277");
            this.Manager.Comment("reaching state \'S347\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp105;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp106;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Invalid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp106 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp105);
            this.Manager.Comment("reaching state \'S417\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidSid\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp105, "trustHandle of CreateTrustedDomainEx, state S417");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidSid, temp106, "return of CreateTrustedDomainEx, state S417");
            this.Manager.Comment("reaching state \'S487\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp107;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformationInternal,True)'");
            temp107 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, true);
            this.Manager.Comment("reaching state \'S557\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp107, "return of SetInformationTrustedDomain, state S557");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S136
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S136() {
            this.Manager.BeginTest("TestScenarioS17S136");
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S208\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp108;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp109;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp109 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp108);
            this.Manager.Comment("reaching state \'S278\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp108, "policyHandle of OpenPolicy2, state S278");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp109, "return of OpenPolicy2, state S278");
            this.Manager.Comment("reaching state \'S348\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp110;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp111;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp111 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp110);
            this.Manager.Comment("reaching state \'S418\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp110, "trustHandle of CreateTrustedDomainEx, state S418");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp111, "return of CreateTrustedDomainEx, state S418");
            this.Manager.Comment("reaching state \'S488\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp112;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformationInternal,True)'");
            temp112 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, true);
            this.Manager.Comment("reaching state \'S558\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp112, "return of SetInformationTrustedDomain, state S558");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS17S564() {
            this.Manager.Comment("reaching state \'S564\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp113;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp113 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.AddReturn(DeleteTrustedDomainInfo, null, temp113);
            TestScenarioS17S566();
        }
        #endregion
        
        #region Test Starting in S138
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S138() {
            this.Manager.BeginTest("TestScenarioS17S138");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S209\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp114;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp115 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp114);
            this.Manager.Comment("reaching state \'S279\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp114, "policyHandle of OpenPolicy2, state S279");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp115, "return of OpenPolicy2, state S279");
            this.Manager.Comment("reaching state \'S349\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp116;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp117;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp117 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp116);
            this.Manager.Comment("reaching state \'S419\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp116, "trustHandle of CreateTrustedDomainEx, state S419");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp117, "return of CreateTrustedDomainEx, state S419");
            this.Manager.Comment("reaching state \'S489\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp118;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx,True)'");
            temp118 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, true);
            this.Manager.Comment("reaching state \'S559\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp118, "return of SetInformationTrustedDomain, state S559");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S14() {
            this.Manager.BeginTest("TestScenarioS17S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp119;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp120;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp120 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp119);
            this.Manager.Comment("reaching state \'S217\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp119, "policyHandle of OpenPolicy2, state S217");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp120, "return of OpenPolicy2, state S217");
            this.Manager.Comment("reaching state \'S287\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp121;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp122;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp122 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp121);
            this.Manager.Comment("reaching state \'S357\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp121, "trustHandle of CreateTrustedDomainEx, state S357");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp122, "return of CreateTrustedDomainEx, state S357");
            this.Manager.Comment("reaching state \'S427\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp123;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformationInternal,True)'");
            temp123 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, true);
            this.Manager.Comment("reaching state \'S497\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp123, "return of SetInformationTrustedDomain, state S497");
            TestScenarioS17S561();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS17S561() {
            this.Manager.Comment("reaching state \'S561\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp124;
            this.Manager.Comment("executing step \'call DeleteTrustedDomain(1,\"DomainSid\",Valid)\'");
            temp124 = this.ILsadManagedAdapterInstance.DeleteTrustedDomain(1, "DomainSid", ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)));
            this.Manager.AddReturn(DeleteTrustedDomainInfo, null, temp124);
            TestScenarioS17S566();
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S16() {
            this.Manager.BeginTest("TestScenarioS17S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp125;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp126;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp126 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp125);
            this.Manager.Comment("reaching state \'S218\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp125, "policyHandle of OpenPolicy2, state S218");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp126, "return of OpenPolicy2, state S218");
            this.Manager.Comment("reaching state \'S288\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp127;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp128;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp128 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp127);
            this.Manager.Comment("reaching state \'S358\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp127, "trustHandle of CreateTrustedDomainEx, state S358");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp128, "return of CreateTrustedDomainEx, state S358");
            this.Manager.Comment("reaching state \'S428\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp129;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformation,True)'");
            temp129 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, true);
            this.Manager.Comment("reaching state \'S498\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp129, "return of SetInformationTrustedDomain, state S498");
            TestScenarioS17S562();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S18() {
            this.Manager.BeginTest("TestScenarioS17S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp130;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp131;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp131 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp130);
            this.Manager.Comment("reaching state \'S219\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp130, "policyHandle of OpenPolicy2, state S219");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp131, "return of OpenPolicy2, state S219");
            this.Manager.Comment("reaching state \'S289\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp132;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp133;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp133 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp132);
            this.Manager.Comment("reaching state \'S359\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp132, "trustHandle of CreateTrustedDomainEx, state S359");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp133, "return of CreateTrustedDomainEx, state S359");
            this.Manager.Comment("reaching state \'S429\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp134;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx,True)'");
            temp134 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, true);
            this.Manager.Comment("reaching state \'S499\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp134, "return of SetInformationTrustedDomain, state S499");
            TestScenarioS17S561();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S2() {
            this.Manager.BeginTest("TestScenarioS17S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S141\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp135;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp136;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp136 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp135);
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp135, "policyHandle of OpenPolicy2, state S211");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp136, "return of OpenPolicy2, state S211");
            this.Manager.Comment("reaching state \'S281\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp137;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp138;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp138 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp137);
            this.Manager.Comment("reaching state \'S351\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp137, "trustHandle of CreateTrustedDomainEx, state S351");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp138, "return of CreateTrustedDomainEx, state S351");
            this.Manager.Comment("reaching state \'S421\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp139;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation,True)'");
            temp139 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, true);
            this.Manager.Comment("reaching state \'S491\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp139, "return of SetInformationTrustedDomain, state S491");
            TestScenarioS17S561();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S20() {
            this.Manager.BeginTest("TestScenarioS17S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp140;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp141;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp141 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp140);
            this.Manager.Comment("reaching state \'S220\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp140, "policyHandle of OpenPolicy2, state S220");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp141, "return of OpenPolicy2, state S220");
            this.Manager.Comment("reaching state \'S290\'");
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
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp142, "trustHandle of CreateTrustedDomainEx, state S360");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp143, "return of CreateTrustedDomainEx, state S360");
            this.Manager.Comment("reaching state \'S430\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp144;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPosixOffsetInformation,True)'");
            temp144 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, true);
            this.Manager.Comment("reaching state \'S500\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp144, "return of SetInformationTrustedDomain, state S500");
            TestScenarioS17S562();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S22() {
            this.Manager.BeginTest("TestScenarioS17S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp145;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp146;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp146 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp145);
            this.Manager.Comment("reaching state \'S221\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp145, "policyHandle of OpenPolicy2, state S221");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp146, "return of OpenPolicy2, state S221");
            this.Manager.Comment("reaching state \'S291\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp147;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp148;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp148 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp147);
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp147, "trustHandle of CreateTrustedDomainEx, state S361");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp148, "return of CreateTrustedDomainEx, state S361");
            this.Manager.Comment("reaching state \'S431\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp149;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformation,True)'");
            temp149 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, true);
            this.Manager.Comment("reaching state \'S501\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp149, "return of SetInformationTrustedDomain, state S501");
            TestScenarioS17S562();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S24() {
            this.Manager.BeginTest("TestScenarioS17S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp150;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp151;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp151 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp150);
            this.Manager.Comment("reaching state \'S222\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp150, "policyHandle of OpenPolicy2, state S222");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp151, "return of OpenPolicy2, state S222");
            this.Manager.Comment("reaching state \'S292\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp152;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp153;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp153 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp152);
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp152, "trustHandle of CreateTrustedDomainEx, state S362");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp153, "return of CreateTrustedDomainEx, state S362");
            this.Manager.Comment("reaching state \'S432\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp154;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx,True)'");
            temp154 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, true);
            this.Manager.Comment("reaching state \'S502\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp154, "return of SetInformationTrustedDomain, state S502");
            TestScenarioS17S563();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S26() {
            this.Manager.BeginTest("TestScenarioS17S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp155;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp156;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp156 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp155);
            this.Manager.Comment("reaching state \'S223\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp155, "policyHandle of OpenPolicy2, state S223");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp156, "return of OpenPolicy2, state S223");
            this.Manager.Comment("reaching state \'S293\'");
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
            this.Manager.Comment("reaching state \'S363\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp157, "trustHandle of CreateTrustedDomainEx, state S363");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp158, "return of CreateTrustedDomainEx, state S363");
            this.Manager.Comment("reaching state \'S433\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp159;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPosixOffsetInformation,True)'");
            temp159 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, true);
            this.Manager.Comment("reaching state \'S503\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp159, "return of SetInformationTrustedDomain, state S503");
            TestScenarioS17S562();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S28() {
            this.Manager.BeginTest("TestScenarioS17S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp160;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp161;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp161 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp160);
            this.Manager.Comment("reaching state \'S224\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp160, "policyHandle of OpenPolicy2, state S224");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp161, "return of OpenPolicy2, state S224");
            this.Manager.Comment("reaching state \'S294\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp162;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp163;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp163 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp162);
            this.Manager.Comment("reaching state \'S364\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp162, "trustHandle of CreateTrustedDomainEx, state S364");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp163, "return of CreateTrustedDomainEx, state S364");
            this.Manager.Comment("reaching state \'S434\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp164;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformationInternal,True)'");
            temp164 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, true);
            this.Manager.Comment("reaching state \'S504\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp164, "return of SetInformationTrustedDomain, state S504");
            TestScenarioS17S563();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S30() {
            this.Manager.BeginTest("TestScenarioS17S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp165;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp166;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp166 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp165);
            this.Manager.Comment("reaching state \'S225\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp165, "policyHandle of OpenPolicy2, state S225");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp166, "return of OpenPolicy2, state S225");
            this.Manager.Comment("reaching state \'S295\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp167;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp168;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp168 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp167);
            this.Manager.Comment("reaching state \'S365\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp167, "trustHandle of CreateTrustedDomainEx, state S365");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp168, "return of CreateTrustedDomainEx, state S365");
            this.Manager.Comment("reaching state \'S435\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp169;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPasswordInformation,True)'");
            temp169 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, true);
            this.Manager.Comment("reaching state \'S505\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp169, "return of SetInformationTrustedDomain, state S505");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S32() {
            this.Manager.BeginTest("TestScenarioS17S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S156\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp170;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp171;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp171 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp170);
            this.Manager.Comment("reaching state \'S226\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp170, "policyHandle of OpenPolicy2, state S226");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp171, "return of OpenPolicy2, state S226");
            this.Manager.Comment("reaching state \'S296\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp172;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp173;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp173 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp172);
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp172, "trustHandle of CreateTrustedDomainEx, state S366");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp173, "return of CreateTrustedDomainEx, state S366");
            this.Manager.Comment("reaching state \'S436\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp174;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationBasic,True)'");
            temp174 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, true);
            this.Manager.Comment("reaching state \'S506\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp174, "return of SetInformationTrustedDomain, state S506");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S34() {
            this.Manager.BeginTest("TestScenarioS17S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S157\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp175;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp176;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp176 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp175);
            this.Manager.Comment("reaching state \'S227\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp175, "policyHandle of OpenPolicy2, state S227");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp176, "return of OpenPolicy2, state S227");
            this.Manager.Comment("reaching state \'S297\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp177;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp178;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp178 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp177);
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp177, "trustHandle of CreateTrustedDomainEx, state S367");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp178, "return of CreateTrustedDomainEx, state S367");
            this.Manager.Comment("reaching state \'S437\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp179;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationBasic,True)'");
            temp179 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationBasic, true);
            this.Manager.Comment("reaching state \'S507\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp179, "return of SetInformationTrustedDomain, state S507");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S36() {
            this.Manager.BeginTest("TestScenarioS17S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S158\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp180;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp181;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp181 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp180);
            this.Manager.Comment("reaching state \'S228\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp180, "policyHandle of OpenPolicy2, state S228");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp181, "return of OpenPolicy2, state S228");
            this.Manager.Comment("reaching state \'S298\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp182;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp183;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp183 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp182);
            this.Manager.Comment("reaching state \'S368\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp182, "trustHandle of CreateTrustedDomainEx, state S368");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp183, "return of CreateTrustedDomainEx, state S368");
            this.Manager.Comment("reaching state \'S438\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp184;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx2Internal,True)'");
            temp184 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, true);
            this.Manager.Comment("reaching state \'S508\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp184, "return of SetInformationTrustedDomain, state S508");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S38() {
            this.Manager.BeginTest("TestScenarioS17S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S159\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp185;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp186;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp186 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp185);
            this.Manager.Comment("reaching state \'S229\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp185, "policyHandle of OpenPolicy2, state S229");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp186, "return of OpenPolicy2, state S229");
            this.Manager.Comment("reaching state \'S299\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp187;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp188;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp188 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp187);
            this.Manager.Comment("reaching state \'S369\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp187, "trustHandle of CreateTrustedDomainEx, state S369");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp188, "return of CreateTrustedDomainEx, state S369");
            this.Manager.Comment("reaching state \'S439\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp189;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx2Internal,True)'");
            temp189 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx2Internal, true);
            this.Manager.Comment("reaching state \'S509\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp189, "return of SetInformationTrustedDomain, state S509");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S4() {
            this.Manager.BeginTest("TestScenarioS17S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S142\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp190;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp191;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp191 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp190);
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp190, "policyHandle of OpenPolicy2, state S212");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp191, "return of OpenPolicy2, state S212");
            this.Manager.Comment("reaching state \'S282\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp192;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp193;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp193 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp192);
            this.Manager.Comment("reaching state \'S352\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp192, "trustHandle of CreateTrustedDomainEx, state S352");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp193, "return of CreateTrustedDomainEx, state S352");
            this.Manager.Comment("reaching state \'S422\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp194;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainSupportedEncryptionTypes,True)'");
            temp194 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, true);
            this.Manager.Comment("reaching state \'S492\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp194, "return of SetInformationTrustedDomain, state S492");
            TestScenarioS17S562();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S40() {
            this.Manager.BeginTest("TestScenarioS17S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp195;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp196;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp196 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp195);
            this.Manager.Comment("reaching state \'S230\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp195, "policyHandle of OpenPolicy2, state S230");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp196, "return of OpenPolicy2, state S230");
            this.Manager.Comment("reaching state \'S300\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp197;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp198;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp198 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp197);
            this.Manager.Comment("reaching state \'S370\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp197, "trustHandle of CreateTrustedDomainEx, state S370");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp198, "return of CreateTrustedDomainEx, state S370");
            this.Manager.Comment("reaching state \'S440\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp199;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedControllersInformation,True)'");
            temp199 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, true);
            this.Manager.Comment("reaching state \'S510\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp199, "return of SetInformationTrustedDomain, state S510");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S42() {
            this.Manager.BeginTest("TestScenarioS17S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp200;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp201;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp201 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp200);
            this.Manager.Comment("reaching state \'S231\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp200, "policyHandle of OpenPolicy2, state S231");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp201, "return of OpenPolicy2, state S231");
            this.Manager.Comment("reaching state \'S301\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp202;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp203;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp203 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp202);
            this.Manager.Comment("reaching state \'S371\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp202, "trustHandle of CreateTrustedDomainEx, state S371");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp203, "return of CreateTrustedDomainEx, state S371");
            this.Manager.Comment("reaching state \'S441\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp204;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainNameInformation,True)'");
            temp204 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), true);
            this.Manager.Comment("reaching state \'S511\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp204, "return of SetInformationTrustedDomain, state S511");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S44() {
            this.Manager.BeginTest("TestScenarioS17S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S162\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp205;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp206;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp206 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp205);
            this.Manager.Comment("reaching state \'S232\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp205, "policyHandle of OpenPolicy2, state S232");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp206, "return of OpenPolicy2, state S232");
            this.Manager.Comment("reaching state \'S302\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp207;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp208;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp208 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp207);
            this.Manager.Comment("reaching state \'S372\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp207, "trustHandle of CreateTrustedDomainEx, state S372");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp208, "return of CreateTrustedDomainEx, state S372");
            this.Manager.Comment("reaching state \'S442\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp209;
            this.Manager.Comment("executing step \'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(" +
                    "TrustDomainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"Do" +
                    "mainNetBios\",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,Invalid,Tru" +
                    "e)\'");
            temp209 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, true);
            this.Manager.Comment("reaching state \'S512\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp209, "return of SetInformationTrustedDomain, state S512");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S46() {
            this.Manager.BeginTest("TestScenarioS17S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S163\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp210;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp211;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp211 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp210);
            this.Manager.Comment("reaching state \'S233\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp210, "policyHandle of OpenPolicy2, state S233");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp211, "return of OpenPolicy2, state S233");
            this.Manager.Comment("reaching state \'S303\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp212;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp213;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp213 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp212);
            this.Manager.Comment("reaching state \'S373\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp212, "trustHandle of CreateTrustedDomainEx, state S373");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp213, "return of CreateTrustedDomainEx, state S373");
            this.Manager.Comment("reaching state \'S443\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp214;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation2Internal,True)'");
            temp214 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, true);
            this.Manager.Comment("reaching state \'S513\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp214, "return of SetInformationTrustedDomain, state S513");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S48() {
            this.Manager.BeginTest("TestScenarioS17S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S164\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp215;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp216;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp216 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp215);
            this.Manager.Comment("reaching state \'S234\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp215, "policyHandle of OpenPolicy2, state S234");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp216, "return of OpenPolicy2, state S234");
            this.Manager.Comment("reaching state \'S304\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp217;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp218;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp218 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp217);
            this.Manager.Comment("reaching state \'S374\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp217, "trustHandle of CreateTrustedDomainEx, state S374");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp218, "return of CreateTrustedDomainEx, state S374");
            this.Manager.Comment("reaching state \'S444\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp219;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation2Internal,True)'");
            temp219 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation2Internal, true);
            this.Manager.Comment("reaching state \'S514\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp219, "return of SetInformationTrustedDomain, state S514");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S50() {
            this.Manager.BeginTest("TestScenarioS17S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S165\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp220;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp221;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp221 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp220);
            this.Manager.Comment("reaching state \'S235\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp220, "policyHandle of OpenPolicy2, state S235");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp221, "return of OpenPolicy2, state S235");
            this.Manager.Comment("reaching state \'S305\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp222;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp223;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp223 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp222);
            this.Manager.Comment("reaching state \'S375\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp222, "trustHandle of CreateTrustedDomainEx, state S375");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp223, "return of CreateTrustedDomainEx, state S375");
            this.Manager.Comment("reaching state \'S445\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp224;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainNameInformation,True)'");
            temp224 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass)(1)), true);
            this.Manager.Comment("reaching state \'S515\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp224, "return of SetInformationTrustedDomain, state S515");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S52() {
            this.Manager.BeginTest("TestScenarioS17S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S166\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp225;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp226;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp226 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp225);
            this.Manager.Comment("reaching state \'S236\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp225, "policyHandle of OpenPolicy2, state S236");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp226, "return of OpenPolicy2, state S236");
            this.Manager.Comment("reaching state \'S306\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp227;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp228;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp228 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp227);
            this.Manager.Comment("reaching state \'S376\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp227, "trustHandle of CreateTrustedDomainEx, state S376");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp228, "return of CreateTrustedDomainEx, state S376");
            this.Manager.Comment("reaching state \'S446\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp229;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedControllersInformation,True)'");
            temp229 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedControllersInformation, true);
            this.Manager.Comment("reaching state \'S516\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp229, "return of SetInformationTrustedDomain, state S516");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S54() {
            this.Manager.BeginTest("TestScenarioS17S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S167\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp230;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp231;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp231 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp230);
            this.Manager.Comment("reaching state \'S237\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp230, "policyHandle of OpenPolicy2, state S237");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp231, "return of OpenPolicy2, state S237");
            this.Manager.Comment("reaching state \'S307\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp232;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp233;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp233 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp232);
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp232, "trustHandle of CreateTrustedDomainEx, state S377");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp233, "return of CreateTrustedDomainEx, state S377");
            this.Manager.Comment("reaching state \'S447\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp234;
            this.Manager.Comment("executing step \'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(" +
                    "TrustDomainName=\"Domain\",TrustDomain_Sid=\"DomainSid\",TrustDomain_NetBiosName=\"Do" +
                    "mainNetBios\",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,Invalid,Tru" +
                    "e)\'");
            temp234 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.Invalid, true);
            this.Manager.Comment("reaching state \'S517\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp234, "return of SetInformationTrustedDomain, state S517");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S56() {
            this.Manager.BeginTest("TestScenarioS17S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S168\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp235;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp236;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp236 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp235);
            this.Manager.Comment("reaching state \'S238\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp235, "policyHandle of OpenPolicy2, state S238");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp236, "return of OpenPolicy2, state S238");
            this.Manager.Comment("reaching state \'S308\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp237;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp238;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp238 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp237);
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp237, "trustHandle of CreateTrustedDomainEx, state S378");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp238, "return of CreateTrustedDomainEx, state S378");
            this.Manager.Comment("reaching state \'S448\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp239;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPasswordInformation,True)'");
            temp239 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPasswordInformation, true);
            this.Manager.Comment("reaching state \'S518\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp239, "return of SetInformationTrustedDomain, state S518");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S58() {
            this.Manager.BeginTest("TestScenarioS17S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S169\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp240;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp241;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp241 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp240);
            this.Manager.Comment("reaching state \'S239\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp240, "policyHandle of OpenPolicy2, state S239");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp241, "return of OpenPolicy2, state S239");
            this.Manager.Comment("reaching state \'S309\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp242;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp243;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp243 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp242);
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp242, "trustHandle of CreateTrustedDomainEx, state S379");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp243, "return of CreateTrustedDomainEx, state S379");
            this.Manager.Comment("reaching state \'S449\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp244;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPosixOffsetInformation,True)'");
            temp244 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, true);
            this.Manager.Comment("reaching state \'S519\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp244, "return of SetInformationTrustedDomain, state S519");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S6() {
            this.Manager.BeginTest("TestScenarioS17S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S143\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp245;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp246;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp246 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp245);
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp245, "policyHandle of OpenPolicy2, state S213");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp246, "return of OpenPolicy2, state S213");
            this.Manager.Comment("reaching state \'S283\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp247;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp248;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp248 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp247);
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp247, "trustHandle of CreateTrustedDomainEx, state S353");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp248, "return of CreateTrustedDomainEx, state S353");
            this.Manager.Comment("reaching state \'S423\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp249;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformationInternal,True)'");
            temp249 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, true);
            this.Manager.Comment("reaching state \'S493\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp249, "return of SetInformationTrustedDomain, state S493");
            TestScenarioS17S562();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S60() {
            this.Manager.BeginTest("TestScenarioS17S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S170\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp250;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp251;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp251 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp250);
            this.Manager.Comment("reaching state \'S240\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp250, "policyHandle of OpenPolicy2, state S240");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp251, "return of OpenPolicy2, state S240");
            this.Manager.Comment("reaching state \'S310\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp252;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp253;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp253 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp252);
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp252, "trustHandle of CreateTrustedDomainEx, state S380");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp253, "return of CreateTrustedDomainEx, state S380");
            this.Manager.Comment("reaching state \'S450\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp254;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformation,True)'");
            temp254 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, true);
            this.Manager.Comment("reaching state \'S520\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp254, "return of SetInformationTrustedDomain, state S520");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S62() {
            this.Manager.BeginTest("TestScenarioS17S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S171\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp255;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp256;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp256 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp255);
            this.Manager.Comment("reaching state \'S241\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp255, "policyHandle of OpenPolicy2, state S241");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp256, "return of OpenPolicy2, state S241");
            this.Manager.Comment("reaching state \'S311\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp257;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp258;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp258 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp257);
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp257, "trustHandle of CreateTrustedDomainEx, state S381");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp258, "return of CreateTrustedDomainEx, state S381");
            this.Manager.Comment("reaching state \'S451\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp259;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformationInternal,True)'");
            temp259 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, true);
            this.Manager.Comment("reaching state \'S521\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp259, "return of SetInformationTrustedDomain, state S521");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S64() {
            this.Manager.BeginTest("TestScenarioS17S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S172\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp260;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp261;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp261 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp260);
            this.Manager.Comment("reaching state \'S242\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp260, "policyHandle of OpenPolicy2, state S242");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp261, "return of OpenPolicy2, state S242");
            this.Manager.Comment("reaching state \'S312\'");
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
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp262, "trustHandle of CreateTrustedDomainEx, state S382");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp263, "return of CreateTrustedDomainEx, state S382");
            this.Manager.Comment("reaching state \'S452\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp264;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainSupportedEncryptionTypes,True)'");
            temp264 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, true);
            this.Manager.Comment("reaching state \'S522\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp264, "return of SetInformationTrustedDomain, state S522");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S66() {
            this.Manager.BeginTest("TestScenarioS17S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S173\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp265;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp266;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp266 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp265);
            this.Manager.Comment("reaching state \'S243\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp265, "policyHandle of OpenPolicy2, state S243");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp266, "return of OpenPolicy2, state S243");
            this.Manager.Comment("reaching state \'S313\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp267;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp268;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp268 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp267);
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp267, "trustHandle of CreateTrustedDomainEx, state S383");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp268, "return of CreateTrustedDomainEx, state S383");
            this.Manager.Comment("reaching state \'S453\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp269;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformationInternal,True)'");
            temp269 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, true);
            this.Manager.Comment("reaching state \'S523\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp269, "return of SetInformationTrustedDomain, state S523");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S68() {
            this.Manager.BeginTest("TestScenarioS17S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S174\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp270;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp271;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp271 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp270);
            this.Manager.Comment("reaching state \'S244\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp270, "policyHandle of OpenPolicy2, state S244");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp271, "return of OpenPolicy2, state S244");
            this.Manager.Comment("reaching state \'S314\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp272;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp273;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp273 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp272);
            this.Manager.Comment("reaching state \'S384\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp272, "trustHandle of CreateTrustedDomainEx, state S384");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp273, "return of CreateTrustedDomainEx, state S384");
            this.Manager.Comment("reaching state \'S454\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp274;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation,True)'");
            temp274 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, true);
            this.Manager.Comment("reaching state \'S524\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp274, "return of SetInformationTrustedDomain, state S524");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S70() {
            this.Manager.BeginTest("TestScenarioS17S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S175\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp275;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp276;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp276 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp275);
            this.Manager.Comment("reaching state \'S245\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp275, "policyHandle of OpenPolicy2, state S245");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp276, "return of OpenPolicy2, state S245");
            this.Manager.Comment("reaching state \'S315\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp277;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp278;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp278 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp277);
            this.Manager.Comment("reaching state \'S385\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp277, "trustHandle of CreateTrustedDomainEx, state S385");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp278, "return of CreateTrustedDomainEx, state S385");
            this.Manager.Comment("reaching state \'S455\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp279;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx,True)'");
            temp279 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, true);
            this.Manager.Comment("reaching state \'S525\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp279, "return of SetInformationTrustedDomain, state S525");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S72
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S72() {
            this.Manager.BeginTest("TestScenarioS17S72");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S176\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp280;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp281;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp281 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp280);
            this.Manager.Comment("reaching state \'S246\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp280, "policyHandle of OpenPolicy2, state S246");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp281, "return of OpenPolicy2, state S246");
            this.Manager.Comment("reaching state \'S316\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp282;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp283;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp283 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp282);
            this.Manager.Comment("reaching state \'S386\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp282, "trustHandle of CreateTrustedDomainEx, state S386");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp283, "return of CreateTrustedDomainEx, state S386");
            this.Manager.Comment("reaching state \'S456\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp284;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainInformationEx,True)'");
            temp284 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainInformationEx, true);
            this.Manager.Comment("reaching state \'S526\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp284, "return of SetInformationTrustedDomain, state S526");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S74
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S74() {
            this.Manager.BeginTest("TestScenarioS17S74");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S177\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp285;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp286;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp286 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp285);
            this.Manager.Comment("reaching state \'S247\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp285, "policyHandle of OpenPolicy2, state S247");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp286, "return of OpenPolicy2, state S247");
            this.Manager.Comment("reaching state \'S317\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp287;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp288;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp288 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp287);
            this.Manager.Comment("reaching state \'S387\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp287, "trustHandle of CreateTrustedDomainEx, state S387");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp288, "return of CreateTrustedDomainEx, state S387");
            this.Manager.Comment("reaching state \'S457\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp289;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformation,True)'");
            temp289 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformation, true);
            this.Manager.Comment("reaching state \'S527\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp289, "return of SetInformationTrustedDomain, state S527");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S76
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S76() {
            this.Manager.BeginTest("TestScenarioS17S76");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S178\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp290;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp291;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp291 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp290);
            this.Manager.Comment("reaching state \'S248\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp290, "policyHandle of OpenPolicy2, state S248");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp291, "return of OpenPolicy2, state S248");
            this.Manager.Comment("reaching state \'S318\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp292;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp293;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp293 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp292);
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp292, "trustHandle of CreateTrustedDomainEx, state S388");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp293, "return of CreateTrustedDomainEx, state S388");
            this.Manager.Comment("reaching state \'S458\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp294;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformationInternal,True)'");
            temp294 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, true);
            this.Manager.Comment("reaching state \'S528\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp294, "return of SetInformationTrustedDomain, state S528");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S78
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S78() {
            this.Manager.BeginTest("TestScenarioS17S78");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S179\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp295;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp296;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp296 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp295);
            this.Manager.Comment("reaching state \'S249\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp295, "policyHandle of OpenPolicy2, state S249");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp296, "return of OpenPolicy2, state S249");
            this.Manager.Comment("reaching state \'S319\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp297;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp298;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp298 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp297);
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp297, "trustHandle of CreateTrustedDomainEx, state S389");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp298, "return of CreateTrustedDomainEx, state S389");
            this.Manager.Comment("reaching state \'S459\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp299;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPosixOffsetInformation,True)'");
            temp299 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, true);
            this.Manager.Comment("reaching state \'S529\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp299, "return of SetInformationTrustedDomain, state S529");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S8() {
            this.Manager.BeginTest("TestScenarioS17S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp300;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp301;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp301 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp300);
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp300, "policyHandle of OpenPolicy2, state S214");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp301, "return of OpenPolicy2, state S214");
            this.Manager.Comment("reaching state \'S284\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp302;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp303;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp303 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp302);
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp302, "trustHandle of CreateTrustedDomainEx, state S354");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp303, "return of CreateTrustedDomainEx, state S354");
            this.Manager.Comment("reaching state \'S424\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp304;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformationInternal,True)'");
            temp304 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformationInternal, true);
            this.Manager.Comment("reaching state \'S494\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp304, "return of SetInformationTrustedDomain, state S494");
            TestScenarioS17S562();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S80
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S80() {
            this.Manager.BeginTest("TestScenarioS17S80");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S180\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp305;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp306;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp306 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp305);
            this.Manager.Comment("reaching state \'S250\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp305, "policyHandle of OpenPolicy2, state S250");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp306, "return of OpenPolicy2, state S250");
            this.Manager.Comment("reaching state \'S320\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp307;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp308;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp308 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp307);
            this.Manager.Comment("reaching state \'S390\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp307, "trustHandle of CreateTrustedDomainEx, state S390");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp308, "return of CreateTrustedDomainEx, state S390");
            this.Manager.Comment("reaching state \'S460\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp309;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainSupportedEncryptionTypes,True)'");
            temp309 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainSupportedEncryptionTypes, true);
            this.Manager.Comment("reaching state \'S530\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp309, "return of SetInformationTrustedDomain, state S530");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S82
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S82() {
            this.Manager.BeginTest("TestScenarioS17S82");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S181\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp310;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp311;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp311 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp310);
            this.Manager.Comment("reaching state \'S251\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp310, "policyHandle of OpenPolicy2, state S251");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp311, "return of OpenPolicy2, state S251");
            this.Manager.Comment("reaching state \'S321\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp312;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp313;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp313 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            0u}), out temp312);
            this.Manager.Comment("reaching state \'S391\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp312, "trustHandle of CreateTrustedDomainEx, state S391");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp313, "return of CreateTrustedDomainEx, state S391");
            this.Manager.Comment("reaching state \'S461\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp314;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformation,True)'");
            temp314 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, true);
            this.Manager.Comment("reaching state \'S531\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp314, "return of SetInformationTrustedDomain, state S531");
            TestScenarioS17S564();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S84
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S84() {
            this.Manager.BeginTest("TestScenarioS17S84");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S182\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp315;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp316;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp316 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp315);
            this.Manager.Comment("reaching state \'S252\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp315, "policyHandle of OpenPolicy2, state S252");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp316, "return of OpenPolicy2, state S252");
            this.Manager.Comment("reaching state \'S322\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp317;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp318;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp318 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp317);
            this.Manager.Comment("reaching state \'S392\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp317, "trustHandle of CreateTrustedDomainEx, state S392");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp318, "return of CreateTrustedDomainEx, state S392");
            this.Manager.Comment("reaching state \'S462\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp319;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPosixOffsetInformation,True)'");
            temp319 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, true);
            this.Manager.Comment("reaching state \'S532\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp319, "return of SetInformationTrustedDomain, state S532");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S86
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S86() {
            this.Manager.BeginTest("TestScenarioS17S86");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S183\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp320;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp321;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp321 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp320);
            this.Manager.Comment("reaching state \'S253\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp320, "policyHandle of OpenPolicy2, state S253");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp321, "return of OpenPolicy2, state S253");
            this.Manager.Comment("reaching state \'S323\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp322;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp323;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp323 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp322);
            this.Manager.Comment("reaching state \'S393\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp322, "trustHandle of CreateTrustedDomainEx, state S393");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp323, "return of CreateTrustedDomainEx, state S393");
            this.Manager.Comment("reaching state \'S463\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp324;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPosixOffsetInformation,True)'");
            temp324 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, true);
            this.Manager.Comment("reaching state \'S533\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp324, "return of SetInformationTrustedDomain, state S533");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S88
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S88() {
            this.Manager.BeginTest("TestScenarioS17S88");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S184\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp325;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp326;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp326 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp325);
            this.Manager.Comment("reaching state \'S254\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp325, "policyHandle of OpenPolicy2, state S254");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp326, "return of OpenPolicy2, state S254");
            this.Manager.Comment("reaching state \'S324\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp327;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp328;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp328 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp327);
            this.Manager.Comment("reaching state \'S394\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp327, "trustHandle of CreateTrustedDomainEx, state S394");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp328, "return of CreateTrustedDomainEx, state S394");
            this.Manager.Comment("reaching state \'S464\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp329;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformation,True)'");
            temp329 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, true);
            this.Manager.Comment("reaching state \'S534\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp329, "return of SetInformationTrustedDomain, state S534");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S90
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S90() {
            this.Manager.BeginTest("TestScenarioS17S90");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S185\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp330;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp331;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp331 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp330);
            this.Manager.Comment("reaching state \'S255\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp330, "policyHandle of OpenPolicy2, state S255");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp331, "return of OpenPolicy2, state S255");
            this.Manager.Comment("reaching state \'S325\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp332;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp333;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp333 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp332);
            this.Manager.Comment("reaching state \'S395\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp332, "trustHandle of CreateTrustedDomainEx, state S395");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp333, "return of CreateTrustedDomainEx, state S395");
            this.Manager.Comment("reaching state \'S465\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp334;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=1,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformationInternal,True)'");
            temp334 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, true);
            this.Manager.Comment("reaching state \'S535\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp334, "return of SetInformationTrustedDomain, state S535");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S92
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S92() {
            this.Manager.BeginTest("TestScenarioS17S92");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S186\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp335;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp336;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp336 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp335);
            this.Manager.Comment("reaching state \'S256\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp335, "policyHandle of OpenPolicy2, state S256");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp336, "return of OpenPolicy2, state S256");
            this.Manager.Comment("reaching state \'S326\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp337;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp338;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp338 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp337);
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp337, "trustHandle of CreateTrustedDomainEx, state S396");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp338, "return of CreateTrustedDomainEx, state S396");
            this.Manager.Comment("reaching state \'S466\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp339;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformationInternal,True)'");
            temp339 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, true);
            this.Manager.Comment("reaching state \'S536\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp339, "return of SetInformationTrustedDomain, state S536");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S94
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S94() {
            this.Manager.BeginTest("TestScenarioS17S94");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S187\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp340;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp341;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp341 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp340);
            this.Manager.Comment("reaching state \'S257\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp340, "policyHandle of OpenPolicy2, state S257");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp341, "return of OpenPolicy2, state S257");
            this.Manager.Comment("reaching state \'S327\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp342;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp343;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp343 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp342);
            this.Manager.Comment("reaching state \'S397\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp342, "trustHandle of CreateTrustedDomainEx, state S397");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp343, "return of CreateTrustedDomainEx, state S397");
            this.Manager.Comment("reaching state \'S467\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp344;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(2,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainFullInformationInternal,True)'");
            temp344 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(2, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainFullInformationInternal, true);
            this.Manager.Comment("reaching state \'S537\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp344, "return of SetInformationTrustedDomain, state S537");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S96
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S96() {
            this.Manager.BeginTest("TestScenarioS17S96");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S188\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp345;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp346;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp346 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp345);
            this.Manager.Comment("reaching state \'S258\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp345, "policyHandle of OpenPolicy2, state S258");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp346, "return of OpenPolicy2, state S258");
            this.Manager.Comment("reaching state \'S328\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp347;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp348;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp348 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp347);
            this.Manager.Comment("reaching state \'S398\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp347, "trustHandle of CreateTrustedDomainEx, state S398");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp348, "return of CreateTrustedDomainEx, state S398");
            this.Manager.Comment("reaching state \'S468\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp349;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedDomainAuthInformation,True)'");
            temp349 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedDomainAuthInformation, true);
            this.Manager.Comment("reaching state \'S538\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp349, "return of SetInformationTrustedDomain, state S538");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S98
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS17S98() {
            this.Manager.BeginTest("TestScenarioS17S98");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S189\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp350;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp351;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp351 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp350);
            this.Manager.Comment("reaching state \'S259\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp350, "policyHandle of OpenPolicy2, state S259");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp351, "return of OpenPolicy2, state S259");
            this.Manager.Comment("reaching state \'S329\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp352;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp353;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Invalid,Valid,DS_BEHAVIOR_WIN2003,True,65663,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),out _)'");
            temp353 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 65663u, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_AUTH_INFORMATION>(new string[] {
                            "IncomingAuthInfos",
                            "OutgoingAuthInfos"}, new object[] {
                            0u,
                            0u}), out temp352);
            this.Manager.Comment("reaching state \'S399\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx/[out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp352, "trustHandle of CreateTrustedDomainEx, state S399");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp353, "return of CreateTrustedDomainEx, state S399");
            this.Manager.Comment("reaching state \'S469\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp354;
            this.Manager.Comment(@"executing step 'call SetInformationTrustedDomain(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""DomainNetBios"",TrustType=2,TrustDir=3,TrustAttr=8),DS_BEHAVIOR_WIN2003,TrustedPosixOffsetInformation,True)'");
            temp354 = this.ILsadManagedAdapterInstance.SetInformationTrustedDomain(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TrustedInformationClass.TrustedPosixOffsetInformation, true);
            this.Manager.Comment("reaching state \'S539\'");
            this.Manager.Comment("checking step \'return SetInformationTrustedDomain/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp354, "return of SetInformationTrustedDomain, state S539");
            TestScenarioS17S560();
            this.Manager.EndTest();
        }
        #endregion
    }
}
