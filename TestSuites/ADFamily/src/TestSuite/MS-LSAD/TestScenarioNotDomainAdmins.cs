// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.4.2965.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TestScenarioNotDomainAdmins : PtfTestClassBase {
        
        public TestScenarioNotDomainAdmins() {
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
            string userPassword = LsadManagedAdapter.Instance(BaseTestSite).DomainUserPassword;
            string newUserName = LsadManagedAdapter.DomainUserName;
            string domainNC = "DC=" + LsadManagedAdapter.Instance(BaseTestSite).PrimaryDomainDnsName.Replace(".", ",DC=");
            string parentDN = string.Format("CN=Users,{0}", domainNC);
            string userDN = string.Format("CN={0},CN=Users,{1}", newUserName, domainNC);
            if (Utilities.IsObjectExist(userDN, LsadManagedAdapter.Instance(BaseTestSite).PDCNetbiosName, LsadManagedAdapter.Instance(BaseTestSite).ADDSPortNum))
            {
                Utilities.RemoveUser(LsadManagedAdapter.Instance(BaseTestSite).PDCNetbiosName, LsadManagedAdapter.Instance(BaseTestSite).ADDSPortNum, parentDN, newUserName);                
            }
            Utilities.NewUser(LsadManagedAdapter.Instance(BaseTestSite).PDCNetbiosName, LsadManagedAdapter.Instance(BaseTestSite).ADDSPortNum, parentDN, newUserName, userPassword); 
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void ClassCleanup() {
            PtfTestClassBase.Cleanup();
            string domainNC = "DC=" + LsadManagedAdapter.Instance(BaseTestSite).PrimaryDomainDnsName.Replace(".", ",DC=");
            string parentDN = string.Format("CN=Users,{0}", domainNC);
            string userDN = string.Format("CN={0},CN=Users,{1}", LsadManagedAdapter.DomainUserName, domainNC);
            if (Utilities.IsObjectExist(userDN, LsadManagedAdapter.Instance(BaseTestSite).PDCNetbiosName, LsadManagedAdapter.Instance(BaseTestSite).ADDSPortNum))
            {
                Utilities.RemoveUser(LsadManagedAdapter.Instance(BaseTestSite).PDCNetbiosName, LsadManagedAdapter.Instance(BaseTestSite).ADDSPortNum, parentDN, LsadManagedAdapter.DomainUserName);
            }
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
        public void LSAD_TestScenarioNotDomainAdminsS0() {
            this.Manager.BeginTest("TestScenarioNotDomainAdminsS0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,Fals" +
                    "e)\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, false);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp0);
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S6");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S6");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp2;
            this.Manager.Comment(@"executing step 'call SetTrustedDomainInfo(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,DS_BEHAVIOR_WIN2003,TrustedDomainNameInformation,True,TRUSTED_DOMAIN_AUTH_INFORMATION(IncomingAuthInfos=0,OutgoingAuthInfos=0),4061069443,True)'");
            temp2 = this.ILsadManagedAdapterInstance.SetTrustedDomainInfo(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("checking step \'return SetTrustedDomainInfo/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp2, "return of SetTrustedDomainInfo, state S10");
            TestScenarioNotDomainAdminsS12();
            this.Manager.EndTest();
        }
        
        private void TestScenarioNotDomainAdminsS12() {
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp3;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp4 = this.ILsadManagedAdapterInstance.Close(1, out temp3);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp3, "handleAfterClose of Close, state S13");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp4, "return of Close, state S13");
            this.Manager.Comment("reaching state \'S14\'");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioNotDomainAdminsS2() {
            this.Manager.BeginTest("TestScenarioNotDomainAdminsS2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,Fals" +
                    "e)\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, false);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S5\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp5;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
            temp6 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1u, out temp5);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp5, "policyHandle of OpenPolicy2, state S7");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp6, "return of OpenPolicy2, state S7");
            this.Manager.Comment("reaching state \'S9\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp8;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx2(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=2,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,4061069327,out _)'");
            temp8 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx2(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 4061069327u, out temp7);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx2/[out Invalid]:AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(1)), temp7, "trustHandle of CreateTrustedDomainEx2, state S11");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp8, "return of CreateTrustedDomainEx2, state S11");
            TestScenarioNotDomainAdminsS12();
            this.Manager.EndTest();
        }
        #endregion
    }
}
