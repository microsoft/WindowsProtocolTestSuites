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
    public partial class TestScenarioS19TrustedDomainNetBIOSName : PtfTestClassBase {
        
        public TestScenarioS19TrustedDomainNetBIOSName() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void DeleteObjectDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleOutput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase DeleteObjectInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "DeleteObject", typeof(int), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle).MakeByRefType());
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
        public void LSAD_TestScenarioS19TrustedDomainNetBIOSNameS0() {
            this.Manager.BeginTest("TestScenarioS19TrustedDomainNetBIOSNameS0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S6");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S6");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp2;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp3;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx2(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=1,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,4061069327,out _)'");
            temp3 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx2(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            1u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 4061069327u, out temp2);
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp2, "trustHandle of CreateTrustedDomainEx2, state S10");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp3, "return of CreateTrustedDomainEx2, state S10");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp4;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp5;
            this.Manager.Comment("executing step \'call DeleteObject(2,TrustDomainObject,out _)\'");
            temp5 = this.ILsadManagedAdapterInstance.DeleteObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum.TrustDomainObject, out temp4);
            this.Manager.AddReturn(DeleteObjectInfo, null, temp4, temp5);
            TestScenarioS19TrustedDomainNetBIOSNameS14();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS19TrustedDomainNetBIOSNameS14() {
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS19TrustedDomainNetBIOSName.DeleteObjectInfo, null, new DeleteObjectDelegate1(this.TestScenarioS19TrustedDomainNetBIOSNameS0DeleteObjectChecker)));
            this.Manager.Comment("reaching state \'S15\'");
        }
        
        private void TestScenarioS19TrustedDomainNetBIOSNameS0DeleteObjectChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleOutput, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return DeleteObject/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleOutput, "handleOutput of DeleteObject, state S14");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of DeleteObject, state S14");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS19TrustedDomainNetBIOSNameS2() {
            this.Manager.BeginTest("TestScenarioS19TrustedDomainNetBIOSNameS2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(PrimaryDomainController,Disable,Windows2k8,2,True" +
                    ")\'");
            this.ILsadManagedAdapterInstance.Initialize(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig.PrimaryDomainController, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S5\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,3507,out _)\'");
            temp7 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp6);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp6, "policyHandle of OpenPolicy2, state S7");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp7, "return of OpenPolicy2, state S7");
            this.Manager.Comment("reaching state \'S9\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment(@"executing step 'call CreateTrustedDomainEx2(1,TRUSTED_DOMAIN_INFORMATION_EX(TrustDomainName=""Domain"",TrustDomain_Sid=""DomainSid"",TrustDomain_NetBiosName=""NetBiosName"",TrustType=2,TrustDir=3,TrustAttr=8),Valid,Valid,DS_BEHAVIOR_WIN2003,True,4061069327,out _)'");
            temp9 = this.ILsadManagedAdapterInstance.CreateTrustedDomainEx2(1, this.Make<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.TRUSTED_DOMAIN_INFORMATION_EX>(new string[] {
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
                            3u,
                            8u}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ValidString)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.DomainSid)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ForestFunctionalLevel)(1)), true, 4061069327u, out temp8);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return CreateTrustedDomainEx2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp8, "trustHandle of CreateTrustedDomainEx2, state S11");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp9, "return of CreateTrustedDomainEx2, state S11");
            this.Manager.Comment("reaching state \'S13\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call DeleteObject(2,TrustDomainObject,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.DeleteObject(2, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ObjectEnum.TrustDomainObject, out temp10);
            this.Manager.AddReturn(DeleteObjectInfo, null, temp10, temp11);
            TestScenarioS19TrustedDomainNetBIOSNameS14();
            this.Manager.EndTest();
        }
        #endregion
    }
}
